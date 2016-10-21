using CommonLib;
using DALLib.EF;
using DALLib.Models;
using Microsoft.Practices.ServiceLocation;
using SQLInfoCollectionService.Configuration;
using SQLInfoCollectionService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleForTesting

{


    class testService
    {
        private ILogger logger;

        SQLInfoCollectorService.Scheduler.Scheduler schedularStatusUpdate;
        SQLInfoCollectorService.Scheduler.Scheduler schedularFullUpdate;


        public testService()
        {
            DependencyConfig.Initialize();
            ILogger logger = ServiceLocator.Current.GetInstance<ILogger>();

            this.logger = logger;

            schedularStatusUpdate = new SQLInfoCollectorService.Scheduler.Scheduler(JobType.UpdateInfoType.CheckStatus, logger);
            schedularFullUpdate = new SQLInfoCollectorService.Scheduler.Scheduler(JobType.UpdateInfoType.Full, logger);
        }



        public void start()
        {
            schedularStatusUpdate.Start();
            schedularFullUpdate.Start();
        }

        public void stop()
        {
            schedularStatusUpdate.Stop();
            schedularFullUpdate.Stop();
        }



    }
    class Program
    {





        static void Main(string[] args)
        {

            addDataForTesting();



            testService t = new testService();
            t.start();



            Console.WriteLine("Press any key to stop....");
            Console.ReadKey();


            t.stop();

        }


        static void  addDataForTesting()
        {
            List<BrowsableInstance> list = CommonLib.BrowsableInstance.GetInstances();
            MsSqlMonitorEntities db = new MsSqlMonitorEntities();

            User user;
            if (db.Users.LongCount<User>() == 0)
            {
                user = new User();
                user.Login = "admin";
                user.Password = "admin";
                user.Role = UserRole.Admin;
                db.Users.Add(user);
                db.SaveChanges();
            } else
            {
                user = db.Users.First<User>();
            }

            foreach (var browsableInstance in list)
            {
                if (db.Instances.Where<Instance>(x => x.InstanceName.Equals(browsableInstance.InstanceName)).LongCount() != 0) continue;

                Instance newInst = new Instance();
                newInst.Authentication = AuthenticationType.Windows;
                newInst.InstanceName = browsableInstance.InstanceName;
                newInst.ServerName = browsableInstance.ServerName;
                newInst.Login = "x";
                newInst.Password = "x";
                newInst.IsDeleted = false;

                db.Instances.Add(newInst);
                db.SaveChanges();
                ///////////////////////////////////////////////////

                Assign assign = new Assign();
                assign.User = user;
                assign.UserId = user.Id;
                assign.Instance = newInst;
                assign.InstanceId = newInst.Id;

                db.Assigns.Add(assign);

                db.SaveChanges();
                //////////////////////////////////////////////

                InstanceUpdateJob updateJob = new InstanceUpdateJob();
                updateJob.Instance = newInst;
                updateJob.InstanceId = newInst.Id;

                JobType  jobType = db.JobTypes.Where<JobType>(i => i.Type == JobType.UpdateInfoType.Full).First<JobType>();

                updateJob.JobType = jobType;
                updateJob.JobTypeId = jobType.Type;


                db.InstanceUpdateJobs.Add(updateJob);


                db.SaveChanges();
                //////////////////////////////////////////////

                InstanceUpdateJob updateJob2 = new InstanceUpdateJob();
                updateJob2.Instance = newInst;
                updateJob2.InstanceId = newInst.Id;

                JobType jobType2 = db.JobTypes.Where<JobType>(i => i.Type == JobType.UpdateInfoType.CheckStatus).First<JobType>();

                updateJob2.JobType = jobType2;
                updateJob2.JobTypeId = jobType.Type;


                db.InstanceUpdateJobs.Add(updateJob2);



                db.SaveChanges();


                //////////////////////////////////////////////

            }
            db.SaveChanges();



 



            db.SaveChanges();
        }




    }
}
