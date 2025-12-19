using Autofac;
using DatabaseProject.Data;
using DatabaseProject.Interfaces;
using DatabaseProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Configuration;

namespace DatabaseProject
{
    public static class ContainerConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            // Register database context
            builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<SocialMediaContext>();
                optionsBuilder.UseSqlServer(
                    ConfigurationManager.ConnectionStrings["DatabaseProject"].ConnectionString);
                return new SocialMediaContext(optionsBuilder.Options);
            })
            .AsSelf()
            .InstancePerLifetimeScope();

            // Register services as interfaces
            builder.RegisterGeneric(typeof(JsonFileService<>))
               .As(typeof(IJsonProcessor<>))
               .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BulkInsertService<>))
                   .As(typeof(IBulkInserter<>))
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BulkOutputService<>))
                   .As(typeof(IBulkExporter<>))
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(UpdateDataService<>))
                   .As(typeof(IDataUpdater<>))
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(DeleteDataService<>))
                   .As(typeof(IDataDeleter<>))
                   .InstancePerLifetimeScope();

            // Logger factory
            var loggerFactory = LoggerFactory.Create(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });

            builder.RegisterInstance(loggerFactory).As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).SingleInstance();
        }
    }
}
