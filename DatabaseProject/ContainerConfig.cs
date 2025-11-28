using Autofac;
using DatabaseProject.Data;
using DatabaseProject.Models;
using DatabaseProject.Services;

namespace DatabaseProject
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            // Register DbContext and generic helper service
            builder.RegisterType<SocialMediaContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(JsonFileService<>)).AsSelf().InstancePerLifetimeScope();

            // Create service
            builder.RegisterGeneric(typeof(BulkInsertService<>)).AsSelf().InstancePerLifetimeScope();

            // Read service
            builder.RegisterGeneric(typeof(BulkOutputService<>)).AsSelf().InstancePerLifetimeScope();
            
            // Update service
            builder.RegisterGeneric(typeof(UpdateDataService<>)).AsSelf().InstancePerLifetimeScope();

            // Delete service
            builder.RegisterGeneric(typeof(DeleteDataService<>)).AsSelf().InstancePerLifetimeScope();

            return builder.Build(); 
        }
    }
}
