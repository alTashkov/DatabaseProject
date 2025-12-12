using Autofac;
using DatabaseProject.Services;
using System.Linq.Expressions;

namespace DatabaseProject.Helpers
{
    public static class DataProcessor
    {
        /// <summary>
        /// Inserts bulk data from a file into a table in the database.
        /// </summary>
        /// <typeparam name="T">The type of entity being processed.</typeparam>
        /// <param name="scope">The lifetime of the container.</param>
        /// <param name="inputFilePath">The path to the file containing the data.</param>
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

        /// <summary>
        /// Reads data from the database and outputs it into a file.
        /// </summary>
        /// <typeparam name="T">The type of entity being processed.</typeparam>
        /// <param name="scope">The lifetime of the container</param>
        /// <param name="outputFilePath">The path to the file used for output.</param>
        /// <param name="filter">The filter that will be applied to the data before output.</param>
        public static void Read<T>(ILifetimeScope scope, string outputFilePath, Expression<Func<T, bool>> filter = null) where T : class
        {
            var jsonService = scope.Resolve<JsonFileService<T>>();
            BulkOutputService<T> bulkOutputService = scope.Resolve<BulkOutputService<T>>();

            // Example filter:  u => u.UserId > 8)
            bulkOutputService.OutputFilteredDataToFile(jsonService, outputFilePath, filter);
        }

        /// <summary>
        /// Updates entity parameters in the database.
        /// </summary>
        /// <typeparam name="T">The type of entity being processed.</typeparam>
        /// <typeparam name="TKey">The type of primary key that has been applied to the entity.</typeparam>
        /// <param name="scope">The lifetime of the container.</param>
        /// <param name="primaryKey">The type of primary key.</param>
        /// <param name="propertyValue">The value of the property that will be configured.</param>
        /// <param name="property">The property that will be configured.</param>
        public static void Update<T, TKey>(ILifetimeScope scope, TKey primaryKey, string propertyValue, string property) where T : class
        {
            UpdateDataService<T> updateDataService = scope.Resolve<UpdateDataService<T>>();
            updateDataService.UpdateEntityData(primaryKey, propertyValue, property);
        }

        /// <summary>
        /// Deletes an entity entry from the database.
        /// </summary>
        /// <typeparam name="T">The type of entity being processed.</typeparam>
        /// <typeparam name="TKey">The type of primary key that has been applied to the entity.</typeparam>
        /// <param name="scope">The lifetime of the container.</param>
        /// <param name="primaryKey">The type of primary key.</param>
        public static void Delete<T, TKey>(ILifetimeScope scope, TKey primaryKey) where T : class
        {
            DeleteDataService<T> deleteDataService = scope.Resolve<DeleteDataService<T>>();
            deleteDataService.DeleteEntityData(primaryKey);
        }
    }
}
