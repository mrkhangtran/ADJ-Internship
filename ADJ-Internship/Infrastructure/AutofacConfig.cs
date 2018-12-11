using System;
using System.Linq;
using ADJ.BusinessService.Core;
using ADJ.BusinessService.Implementations;
using ADJ.BusinessService.Interfaces;
using ADJ.Common;
using ADJ.Repository.Core;
using ADJ.Repository.Implementations;
using ADJ.Repository.Interfaces;
using Autofac;

namespace ADJ.Infrastructure
{
    public class AutofacConfig
    {
        public static void Register(ContainerBuilder builder)
        {
            // Unit of Work
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // Service Context
            builder.RegisterType<ApplicationContext>().AsSelf().InstancePerLifetimeScope();

            // A trick to load dependency assembly prior to do the registration
            // Just need to register for 1 class to let the assembly load, once it is loaded, 
            // it can then be used to scan for other classes and registered dynamically

            // Repositories
            builder.RegisterType<PurchaseOrderRepository>().As<IPurchaseOrderRepository>();

            // Services
            builder.RegisterType<PurchaseOrderService>().As<IPurchaseOrderService>();
            builder.RegisterGeneric(typeof(DataProvider<>)).As(typeof(IDataProvider<>));

            RegisterDependenciesByConvention(builder);
        }

        /// <summary>
        /// Registers dynamically all implementations with their interface
        /// </summary>
        private static void RegisterDependenciesByConvention(ContainerBuilder builder)
        {
            var neededAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("ADJ")).ToArray();
            builder.RegisterAssemblyTypes(neededAssemblies)
                .AsImplementedInterfaces()
                .PreserveExistingDefaults();
        }
    }
}
