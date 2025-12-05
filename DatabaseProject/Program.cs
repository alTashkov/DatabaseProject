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
                SocialMediaContext context = scope.Resolve<SocialMediaContext>();
                context.Database.Migrate();

                string jsonPath = "D:\\tempExercises\\DatabaseProject\\DatabaseProject\\users.json";
                JsonFileService<User> jsonService = scope.Resolve<JsonFileService<User>>();
                List<User> users = jsonService.ReadDataFromFile(jsonPath);
                if (users == null || users.Count == 0)
                {
                    return;
                }

                List<User> filteredUsers = context.Users
                    .Where(u => u.Username!.ToLower().StartsWith("a"))
                    .ToList();
                string outputPath = "D:\\tempExercises\\DatabaseProject\\DatabaseProject\\usersOutputAgain.json";
                jsonService.WriteDataToFile(outputPath, filteredUsers);

                // Bulk insert data to database.
                BulkInsertService<User> bulkInsertService = scope.Resolve<BulkInsertService<User>>();
                bulkInsertService.InsertInBatches(users, 10);

                // Read and output to file.
                BulkOutputService<User> bulkOutputService = scope.Resolve<BulkOutputService<User>>();
                string jsonPathReadOutput = "D:\\tempExercises\\DatabaseProject\\DatabaseProject\\dbOutputFromReading.json";
                bulkOutputService.OutputFilteredDataToFile(jsonService, jsonPathReadOutput, u => u.UserId > 8);

                // Update data of entity.
                UpdateDataService<User> updateDataService = scope.Resolve<UpdateDataService<User>>();
                int entityId = 9;
                updateDataService.UpdateEntityData(entityId, "tashkovID9@gmail.com", "Email");
                updateDataService.UpdateEntityData(entityId, "UPDATED_tashkovID9", "Username");

                // Delete entity.
                DeleteDataService<User> deleteDataService = scope.Resolve<DeleteDataService<User>>();
                entityId = 15;
                deleteDataService.DeleteEntityData(entityId);
            }
        }
    }
}