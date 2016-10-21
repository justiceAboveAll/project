using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DALLib.Models;
using SQLInfoCollectionService.Configuration;
using SQLInfoCollectionService.Contracts;
using Microsoft.Practices.ServiceLocation;

using System.Threading.Tasks.Schedulers;
using System.Threading.Tasks.Dataflow;
using SQLInfoCollectionService.Scheduler;
using DALLib.Repos;
using SQLInfoCollectionService.Collectors;
using System.Data.SqlClient;
using SQLInfoCollectorService.Scheduler;

namespace SQLInfoCollectorService.Scheduler
{
    public class Scheduler
    {
        const int BUFFER_CAPACITY = 10000;


        IInstanceDataCollector collector;
        DALLib.EF.MsSqlMonitorEntities dbContext;
        ILogger logger;

        SynchronizedCollection<Instance> nowCollecting = new SynchronizedCollection<Instance>();
        List<Instance> allInstances = new List<Instance>();

        int repeatTime = 1; //repeat time in seconds
        int mutualWrites = 1;  //haw many information portions write in one transaction
        int maxParallelReads = 1; // How many create  parallel threads for reading data 

        System.Timers.Timer timer;



        public JobType.UpdateInfoType UpdateInfoType { get; }

        //TPL DATAFLOW variables
        QueuedTaskScheduler scheduler;


        //buffer for urgent  update tasks
        BufferBlock<SchedulerJob> bufferBlockHighP;

        //buffer for common update tasks
        BufferBlock<SchedulerJob> bufferBlockLowP;


        TaskScheduler highPriorityScheduler;  //scheduler for common updates
        TaskScheduler lowPriorityScheduler; //scheduler for refresh update

        //Dataflow options
        ExecutionDataflowBlockOptions optionsReadHighP;
        ExecutionDataflowBlockOptions optionsReadLowP;
        ExecutionDataflowBlockOptions optionsWriteBlock;
        GroupingDataflowBlockOptions optionsBatchBlock;
        DataflowLinkOptions optionsLink;

        TransformBlock<SchedulerJob, CollectionResult> highPriorityReadInfoBlock; //action block for update instance information
        TransformBlock<SchedulerJob, CollectionResult> lowPriorityReadInfoBlock; //action block for urgent information update



        BatchBlock<CollectionResult> batchBlock; //batch block to combine results into array from <MUTUAL_WRITES> elements
        ActionBlock<CollectionResult[]> writeInfoBlock; //action block to save collected info

        CancellationTokenSource cancelTokenSrc = new CancellationTokenSource();

        System.Threading.Timer triggerBatchTimer; //uses to triger batch block, if posted jobs count less than <mutualWrites> 
        public static readonly int TRIGGER_AFTER_MS = 10000; //triger batch block after 10 seconds

        void TriggerBatch(object state)
        {
           if (batchBlock!=null)  batchBlock.TriggerBatch();
        }



        public void InstanceUpdateFinished(Instance instance)
        {
            if (nowCollecting.Contains(instance)) nowCollecting.Remove(instance);
        }


        public Scheduler(JobType.UpdateInfoType type,ILogger logger)
        {
            this.logger = logger;

            triggerBatchTimer = new Timer(TriggerBatch);

            //create timer
            timer = new System.Timers.Timer();
            timer.Elapsed += OnTimedEvent;


            dbContext = new DALLib.EF.MsSqlMonitorEntities();

            UpdateInfoType = type;

            JobType jobType = null;

            try
            {
                jobType = dbContext.JobTypes.Where(i => i.Type == type).First();
            }
            catch (Exception e)
            {
                logger.Error("Job with type " + type + " not found in database!");
                jobType = null;
              
            }

            if (jobType == null)
            {
                logger.Debug("Creating job type " + type + "");

                CreateJobTypeInDataBase(dbContext);

                return;
            }

           



            repeatTime = jobType.RepeatTimeSec*1000;
            mutualWrites = jobType.MaxMutualWrites;
            maxParallelReads = jobType.MaxParallelReads;

            if (repeatTime<1)
            {
                logger.Error("Job " + type + "  repeat time must be more than 1 second");
                return;
            }

            if (mutualWrites < 1)
            {
                logger.Error("Job " + type + "  mutual writes must be more than 1 ");
                return;
            }

            if (maxParallelReads < 1)
            {
                logger.Error("Job " + type + "  max parallel reads must be more than 1 ");
                return;
            }

            GetAllInstances();


            if (allInstances.Count <1)
            {
                logger.Debug("Job " + type + "  not found instances for that job");
                return;
            }






        }

        private void CreateJobTypeInDataBase(DALLib.EF.MsSqlMonitorEntities dbContext)
        {
            JobType jobType = new JobType();
            jobType.Type = UpdateInfoType;
            jobType.MaxMutualWrites = Environment.ProcessorCount;
            jobType.MaxParallelReads = Environment.ProcessorCount;
            jobType.RepeatTimeSec = 60 * 5;//5 Minutes

           // DALLib.EF.MsSqlMonitorEntities dbContext = new DALLib.EF.MsSqlMonitorEntities();
            dbContext.JobTypes.Add(jobType);
            dbContext.SaveChanges();
        }

        private void GetAllInstances()
        {
            allInstances.Clear();

            //find all jobs with specified  type
            var list = dbContext.InstanceUpdateJobs.Where(i => i.JobType.Type == UpdateInfoType);

            foreach (var elem in list)
               if (!elem.Instance.IsDeleted) allInstances.Add(elem.Instance);
        }

        void InitDataFlow()
        {

            //Create schedulers
            scheduler = new QueuedTaskScheduler(TaskScheduler.Default, maxParallelReads);
            highPriorityScheduler = scheduler.ActivateNewQueue(0);
            lowPriorityScheduler = scheduler.ActivateNewQueue(1);

            //create options
            optionsReadHighP = new ExecutionDataflowBlockOptions
            {
                TaskScheduler = highPriorityScheduler,
                MaxDegreeOfParallelism = maxParallelReads,
                CancellationToken = cancelTokenSrc.Token
            };

            //create options
            optionsReadHighP = new ExecutionDataflowBlockOptions
            {
                TaskScheduler = highPriorityScheduler,
                MaxDegreeOfParallelism = maxParallelReads,
                CancellationToken = cancelTokenSrc.Token
            };

            optionsReadLowP = new ExecutionDataflowBlockOptions
            {
                TaskScheduler = lowPriorityScheduler,
                MaxDegreeOfParallelism = maxParallelReads,
                CancellationToken = cancelTokenSrc.Token
            };


            optionsWriteBlock = new ExecutionDataflowBlockOptions
            {

                CancellationToken = cancelTokenSrc.Token
            };

            optionsBatchBlock = new GroupingDataflowBlockOptions
            {

                BoundedCapacity = BUFFER_CAPACITY,
                Greedy = true,
                CancellationToken = cancelTokenSrc.Token,

            };

            optionsLink = new DataflowLinkOptions { PropagateCompletion = true, };

            CollectionInfoSaver collectionInfoSaver = new CollectionInfoSaver(logger);

            //create blocks
            bufferBlockHighP = new BufferBlock<SchedulerJob>(new DataflowBlockOptions { BoundedCapacity = 1 });
            bufferBlockLowP = new BufferBlock<SchedulerJob>(new DataflowBlockOptions { BoundedCapacity = BUFFER_CAPACITY });
            highPriorityReadInfoBlock = new TransformBlock<SchedulerJob, CollectionResult>(sqlJob => sqlJob.CollectInfo(triggerBatchTimer), optionsReadHighP);
            lowPriorityReadInfoBlock = new TransformBlock<SchedulerJob, CollectionResult>(sqlJob => sqlJob.CollectInfo(triggerBatchTimer), optionsReadLowP);
            batchBlock = new BatchBlock<CollectionResult>(mutualWrites, optionsBatchBlock);
            writeInfoBlock = new ActionBlock<CollectionResult[]>(sqlInfoArray => collectionInfoSaver.SaveArray(batchBlock, sqlInfoArray), optionsWriteBlock);


            //link blocks
            bufferBlockHighP.LinkTo(highPriorityReadInfoBlock, optionsLink);
            bufferBlockLowP.LinkTo(lowPriorityReadInfoBlock, optionsLink);

            highPriorityReadInfoBlock.LinkTo(batchBlock, optionsLink);
            lowPriorityReadInfoBlock.LinkTo(batchBlock, optionsLink);

            batchBlock.LinkTo(writeInfoBlock, optionsLink);
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            TryStartDataFlow();
        }

        void TryStartDataFlow()
        {
            if (bufferBlockLowP == null || lowPriorityReadInfoBlock == null || writeInfoBlock == null) return;

            //check  dataflow block are busy now?
            //  if ((bufferBlockLowP.Count + lowPriorityReadInfoBlock.InputCount +
            //      lowPriorityReadInfoBlock.OutputCount + writeInfoBlock.InputCount) > 0) return;
            GetAllInstances();

            var needToUpdateList = allInstances.Except(nowCollecting);



            //get connection string to local base
            String connectionString = DALLib.EF.MsSqlMonitorEntities.GetConnectionString();
            if (connectionString == null)
            {
                logger.Error("Collection service don't have connection string to own database");
                return;
            }




            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            foreach (Instance instance in needToUpdateList)
            {
                SchedulerJob schedulerJob = new SchedulerJob(instance, logger, UpdateInfoType,this);
                while (!bufferBlockLowP.Post(schedulerJob)) { }

                logger.Debug("post to BufferBlock new  job type="+ UpdateInfoType+" instanceName="+instance.InstanceName);
            }


        }

        private void InitConnectionStringBuilder(SqlConnectionStringBuilder builder, Instance instance)
        {

            builder.DataSource = instance.DataSource;
            builder.IntegratedSecurity = instance.Authentication == AuthenticationType.Windows ? true : false;
            builder.UserID = instance.Login;
            builder.Password = instance.Password;
        }

        public void Start()
        {
            logger.Debug("Start scheduler for jobtype = "+UpdateInfoType+" interval = "+ repeatTime+" seconds");

            InitDataFlow();

            //initialize timer
            timer.Interval = repeatTime*1000;
            timer.AutoReset = true;
            timer.Enabled = true;


            //run colleting before onTImmedEvent will call
            TryStartDataFlow();
        }

        public void Stop()
        {
            logger.Debug("Stop scheduler for jobtype = " + UpdateInfoType);

            //stop timer
            timer.Enabled = false;

            cancelTokenSrc.Cancel();
        }
    }
}
