using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using SQLInfoCollectionService.Collectors;
using SQLInfoCollectionService.Contracts;
using System.Data.SqlClient;
using System.Data;
using SQLInfoCollectionService.Entities;
using SQLInfoCollectionService.InstanceInfoUpdating;

namespace SQLInfoCollectionService.Configuration
{
    public class DependencyConfig
    {
        public static IUnityContainer Initialize()
        {

            IUnityContainer container = new UnityContainer();

            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IDbConnection, SqlConnection>();
            container.RegisterType<IResourceManager, ResourceManager>();
            //  container.RegisterType<IInstanceDataCollector, InstanceDataCollector>();

            container.RegisterType<ILocalStorage, LocalDb>();

            container.RegisterType<IInstanceDataCollector, SQLInfoCollectionService.Collectors.InstanceDataCollector>(
                                                                        new InjectionConstructor(typeof(SqlConnection), typeof(IResourceManager), typeof(ILogger)));

            UnityServiceLocator serviceProvider = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceProvider);

            return container;
        }
    }
}
