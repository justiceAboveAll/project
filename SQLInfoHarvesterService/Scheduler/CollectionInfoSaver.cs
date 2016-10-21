using DALLib.EF;
using DALLib.Models;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Entities;
using SQLInfoCollectionService.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Data.Entity;
using SQLInfoCollectionService.InstanceInfoUpdating;
using SQLInfoCollectionService.InstanceInfoUpdating.Entities;
using Microsoft.Practices.ServiceLocation;

namespace SQLInfoCollectorService.Scheduler
{
    class CollectionInfoSaver
    {
        ILogger Logger;
        ILocalStorage localDb;

        public CollectionInfoSaver(ILogger logger)
        {
            Logger = logger;
            //localDb = new LocalDb();
            localDb = ServiceLocator.Current.GetInstance<ILocalStorage>();
        }



        //Save array of collections
        public  void SaveArray(BatchBlock<CollectionResult> batchBlock, CollectionResult[] jobArray)
        {
            Logger.Debug("CollectionInfoSaver received array for saving length=" + jobArray.Length);

            List<InstanceInfo> listStatus = new List<InstanceInfo>();
            List<InstanceInfo> listFull = new List<InstanceInfo>();

            foreach (CollectionResult res in jobArray)            
                switch (res.UpdateInfoType)
                {
                    case JobType.UpdateInfoType.Full: listFull.Add(res.InstanceInfo);  break;

                    case JobType.UpdateInfoType.CheckStatus: listStatus.Add(res.InstanceInfo); break;
        

                    default:
                        Logger.Error("Job type "+ res.UpdateInfoType+" do not supported!");
                        break;
                }


            if ( listFull.Count>0 ) localDb.SaveInstances( listFull.ToArray(), Logger );
            if ( listStatus.Count > 0 ) localDb.SaveStatusOnly(listStatus.ToArray() , Logger);

            foreach (CollectionResult result in jobArray)
            {
                result.Scheduler.InstanceUpdateFinished(result.Instance);
                Logger.Debug("InstanceUpdateFinished  job type=" + result.UpdateInfoType + " instanceName=" + result.Instance.InstanceName);
            }

 

              //need to call to switch to next block
              batchBlock.TriggerBatch();
        }

      



    }//collection info saver
}//namespace
