using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration.Install;

using Microsoft.Practices.ServiceLocation;
using SQLInfoCollectionService.Contracts;
using SQLInfoCollectionService.Configuration;

namespace SQLInfoCollectionService
{
    static class Program
    {

        static string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        private static bool IsInstalled()
        {
            using (ServiceController controller =
                new ServiceController(name))
            {
                try
                {
                    ServiceControllerStatus status = controller.Status;
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        private static bool IsRunning()
        {
            using (ServiceController controller =
                new ServiceController(name))
            {
                if (!IsInstalled()) return false;
                return (controller.Status == ServiceControllerStatus.Running);
            }
        }

        private static AssemblyInstaller GetInstaller(ILogger logger)
        {
            AssemblyInstaller installer = new AssemblyInstaller( typeof(CollectionService).Assembly, null);
            installer.UseNewContext = true;
            return installer;
        }

        private static void InstallService(ILogger logger)
        {
            if (IsInstalled())
            {
                logger.Debug("Try to install collection service but, service already installed! ");
                return;
            }

            logger.Debug("Install collection service... ");

            AssemblyInstaller installer = GetInstaller(logger);
            IDictionary state = new Hashtable();

            try
            {

                installer.Install(state);
                installer.Commit(state);

            }
            catch (Exception e)
            {

                
                logger.Error("Install CollectionService error " + e.Message);
                logger.Error(e.ToString());
            }
            finally
            {
                try { installer.Rollback(state); } catch { };
            }
        }

        private static void UninstallService(ILogger logger)
        {
            if (!IsInstalled())
            {
                logger.Debug("CollectionService already uninstalled! ");
                return;
            }


            logger.Debug("Start collection service uninstall... ");


            try
            {
                using (AssemblyInstaller installer = GetInstaller(logger))
                {
                    IDictionary state = new Hashtable();
                    try
                    {
                        installer.Uninstall(state);
                    }
                    catch (Exception e)
                    {
                        logger.Error("Uninstall CollecionService error " + e.Message);
                        logger.Error(e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("Uninstall CollecionService error " + e.Message);
                logger.Error(e.ToString());
            }
        }

        private static void StartService(ILogger logger)
        {
            if (!IsInstalled())
            {
                logger.Debug("CollectionService already started! ");
                return;
            }

            logger.Debug("Starting  collection service.... ");

            using (ServiceController controller =
                new ServiceController(name))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Running)
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                    }
                }
                catch (Exception e)
                {
                    logger.Error("Start CollectionService() error " + e.Message);
                    logger.Error(e.ToString());
                }
            }
        }

        private static void StopService(ILogger logger)
        {
            if (!IsInstalled())
            {
                logger.Debug("try stop  collection service, but it is not installed! ");
                return;
            }

            logger.Debug("Stopping  collection service... ");


            using (ServiceController controller =
                new ServiceController(name))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Stopped)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped,
                             TimeSpan.FromSeconds(10));
                    }
                }
                catch (Exception e)
                {
                    logger.Error("Stop CollectionService error " + e.Message);
                    logger.Error(e.ToString());
                }
            }
        }




        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            DependencyConfig.Initialize();
            ILogger logger = ServiceLocator.Current.GetInstance<ILogger>();

            //StopService(logger);
           // UninstallService(logger);
           // return;


            if (args.Length == 0) //no arguments
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new CollectionService(logger)
                };
                ServiceBase.Run(ServicesToRun);
            }
            else if(args.Length == 1)
            {
                switch (args[0])
                {
                    case "--install":
                        InstallService(logger);
                        StartService(logger);
                        break;
                    case "--uninstall":
                        StopService(logger);
                        UninstallService(logger);
                        break;
                    default:
                        logger.Debug("collection service wrong  run argument ");
                        break;
                }
            }


        }///Main



    }//Program


}
