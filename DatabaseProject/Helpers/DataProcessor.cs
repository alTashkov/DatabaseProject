using Autofac;
using DatabaseProject.Services;
using System.Linq.Expressions;

namespace DatabaseProject.Helpers
{
    public static class DataProcessor
    {
        public static void Insert<T>(ILifetimeScope scope, string inputFilePath) where T : class
        {
            var jsonService = scope.Resolve<JsonFileService<T>>();
            List<T> entities = jsonService.ReadDataFromFile(inputFilePath);

            if (entities == null || entities.Count == 0)
            {
                Console.WriteLine($"No {typeof(T).Name.ToLower()} data found in given file.");
                return;
            }

            var bulkInsertService = scope.Resolve<BulkInsertService<T>>();
            bulkInsertService.InsertInBatches(entities, 10);

            Console.WriteLine($"{entities.Count} {typeof(T).Name} records inserted successfully.");
        }

        public static void Read<T>(ILifetimeScope scope, string outputFilePath, Expression<Func<T, bool>> filter = null) where T : class
        {
            var jsonService = scope.Resolve<JsonFileService<T>>();
            BulkOutputService<T> bulkOutputService = scope.Resolve<BulkOutputService<T>>();

            // Example filter:  u => u.UserId > 8)
            bulkOutputService.OutputFilteredDataToFile(jsonService, outputFilePath, filter);
        }

        public static void Update<T, TKey>(ILifetimeScope scope, TKey primaryKey, string propertyValue, string property) where T : class
        {
            UpdateDataService<T> updateDataService = scope.Resolve<UpdateDataService<T>>();
            updateDataService.UpdateEntityData(primaryKey, propertyValue, property);
        }

        public static void Delete<T, TKey>(ILifetimeScope scope, TKey primaryKey) where T : class
        {
            DeleteDataService<T> deleteDataService = scope.Resolve<DeleteDataService<T>>();
            deleteDataService.DeleteEntityData(primaryKey);
        }
    }
}
