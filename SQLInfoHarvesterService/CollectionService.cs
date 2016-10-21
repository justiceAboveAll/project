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

namespace SQLInfoCollectionService
{
    public partial class CollectionService : ServiceBase, IWCFInterface
    { 


        private ILogger logger;

        SQLInfoCollectorService.Scheduler.Scheduler schedularStatusUpdate;
        SQLInfoCollectorService.Scheduler.Scheduler schedularFullUpdate;



        public CollectionService(ILogger logger)
        {
            InitializeComponent();

           
            this.logger = logger;

            schedularStatusUpdate = new SQLInfoCollectorService.Scheduler.Scheduler(JobType.UpdateInfoType.CheckStatus, logger);
            schedularFullUpdate = new SQLInfoCollectorService.Scheduler.Scheduler(JobType.UpdateInfoType.Full, logger);



        }




        protected override void OnStart(string[] args)
        {
            logger.Debug("collection service started");

            schedularStatusUpdate.Start();
            schedularFullUpdate.Start();
        }



        protected override void OnStop()
        {

            logger.Debug("collection service stopped");

            schedularStatusUpdate.Stop();
            schedularFullUpdate.Stop();

        }

        public void RefereshInstanceInfo(int instanceId)
        {
            logger.Debug("refresh was called by WCF");

        

            //Post job to dataflow
           // SchedulerJob job = CreateJobFromInstanceID(instanceId);
           // while (!bufferBlockHighP.Post(job)) { }

        }
    }




}
