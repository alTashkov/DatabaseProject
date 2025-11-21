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

            // Register DbContext
            builder.RegisterType<SocialMediaContext>().AsSelf().InstancePerLifetimeScope();

            // Register generic services
            builder.RegisterGeneric(typeof(JsonFileService<>)).AsSelf().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BulkInsertService<>)).AsSelf().InstancePerLifetimeScope();

            return builder.Build(); 
        }

    }
}
