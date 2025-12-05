using Autofac;
using DatabaseProject.Data;
using DatabaseProject.Models;
using DatabaseProject.Services;
using Microsoft.EntityFrameworkCore;

namespace DatabaseProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope()) 
            {
                var context = scope.Resolve<SocialMediaContext>();
                context.Database.Migrate();

                string jsonPath = "D:\\tempExercises\\DatabaseProject\\DatabaseProject\\users.json";
                var jsonService = scope.Resolve<JsonFileService<User>>();
                List<User> users = jsonService.ReadDataFromFile(jsonPath);
                if (users == null || users.Count == 0)
                {
                    return;
                }

                var filteredUsers = context.Users
                    .Where(u => u.Username!.ToLower().StartsWith("a"))
                    .ToList();
                var outputPath = "D:\\tempExercises\\DatabaseProject\\DatabaseProject\\usersOutputAgain.json";
                jsonService.WriteDataToFile(outputPath, filteredUsers);

                // Bulk insert data to database.
                var bulkInsertService = scope.Resolve<BulkInsertService<User>>();
                bulkInsertService.InsertInBatches(users, 10);

                // Read and output to file.
                var bulkOutputService = scope.Resolve<BulkOutputService<User>>();
                string jsonPathReadOutput = "D:\\tempExercises\\DatabaseProject\\DatabaseProject\\dbOutputFromReading.json";
                bulkOutputService.OutputFilteredDataToFile(jsonService, jsonPathReadOutput, u => u.UserId > 8);

                // Update data of entity.
                var updateDataService = scope.Resolve<UpdateDataService<User>>();
                int entityId = 9;
                updateDataService.UpdateEntityData(entityId, "tashkovID9@gmail.com", "Email");
                updateDataService.UpdateEntityData(entityId, "UPDATED_tashkovID9", "Username");

                // Delete entity.
                var deleteDataService = scope.Resolve<DeleteDataService<User>>();
                entityId = 15;
                deleteDataService.DeleteEntityData(entityId);
            }
        }
    }
}