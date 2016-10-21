using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Collectors;
using DALLib.Models;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SQLInfoCollectionService.Entities;
using System;
using System.Collections.Generic;
using SQLInfoCollectionService.InstanceInfoUpdating.Entities;
using Microsoft.Practices.Unity;
using SQLInfoCollectionService.Configuration;

namespace SQLInfoCollectionService.Scheduler
{
    class SchedulerJob
    {
        Instance Instance { get; set; }

        JobType.UpdateInfoType CurJobType { get; set; }
        SQLInfoCollectorService.Scheduler.Scheduler Scheduler { get; set; }

        InstanceInfoUpdating.InstanceInfoUpdater InstanceInfoUpdater;


        private ILogger Logger;


        public SchedulerJob(Instance instance ,ILogger logger, JobType.UpdateInfoType jobType, SQLInfoCollectorService.Scheduler.Scheduler scheduler)
        {
            this.Logger = logger;
            this.Instance = instance;

            this.Scheduler = scheduler;
            this.CurJobType = jobType;

            Configuration.DependencyConfig.Initialize();

            Microsoft.Practices.Unity.UnityContainer unitycontainer= new Microsoft.Practices.Unity.UnityContainer();

           // IResourceManager resourceManager = ResourceManager();
            //unitycontainer.RegisterType(typeof(ILogger), typeof(Logger), new InjectionConstructor());
          //  unitycontainer.RegisterType(typeof(IResourceManager), typeof(ResourceManager), new InjectionConstructor());

            InstanceInfoUpdater = new InstanceInfoUpdating.InstanceInfoUpdater(DependencyConfig.Initialize(), Logger);

           // InctanceID = inctanceID;
           //  InstanceName = instanceName;
           // ConnectionString = ConnectionString;

        }


       // Collect all SQL info for current instanceName
        public CollectionResult CollectInfo(System.Threading.Timer triggerBatchTimer) 
        {

            CollectionResult result = null;

            switch (CurJobType)
            {
                case JobType.UpdateInfoType.Full:
                    result = CollectFullInfo();
                    break;

                case JobType.UpdateInfoType.CheckStatus:
                    result = CollectStatusInfo();
                    break;

                default:
                    Logger.Error("Job type " + result.UpdateInfoType + " do not supported!");
                    break;
            }



            if (result == null) Logger.Error("Job type " + result.UpdateInfoType + " collect error for instance "+Instance.InstanceName);

            triggerBatchTimer.Change(SQLInfoCollectorService.Scheduler.Scheduler.TRIGGER_AFTER_MS, System.Threading.Timeout.Infinite);

            return result;
        } //collectInfo

        CollectionResult CollectStatusInfo()
        {
            Logger.Debug("Start collecting status info, instance name = " + Instance.InstanceName);

            CollectionResult result = new CollectionResult();
            result.Instance = Instance;
            result.Scheduler = Scheduler;
            result.InstanceInfo = InstanceInfoUpdater.UpdateStatusOnly(Instance);
            result.UpdateInfoType = JobType.UpdateInfoType.CheckStatus;

            if (result.InstanceInfo == null) Logger.Error("Error while collecting status for instance "+Instance.InstanceName);

            Logger.Debug("End collecting status info, instance name = " + Instance.InstanceName);

            return result;
        }


         CollectionResult CollectFullInfo()
        {

            Logger.Debug("Start collecting full info instance name = " + Instance.InstanceName);

            CollectionResult result = new CollectionResult();
            result.Instance = Instance;
            result.Scheduler = Scheduler;
            result.InstanceInfo = InstanceInfoUpdater.Update(Instance);
            result.UpdateInfoType = JobType.UpdateInfoType.Full;

            if (result.InstanceInfo == null) Logger.Error("Error while collecting info for instance " + Instance.InstanceName);


            Logger.Debug("End collecting full info instance name = " + Instance.InstanceName);

            return result;
        }


    }
}
