using Autofac;
using DatabaseProject.Data;
using DatabaseProject.Models;
using DatabaseProject.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
                    List<User> users = jsonService.ReadData(jsonPath);
                    if (users == null || users.Count == 0)
                    {
                        Console.WriteLine("No data found in JSON.");
                        return;
                    }

                    var filteredUsers = context.Users
                        .Where(u => u.Username!.ToLower().StartsWith("a"))
                        .ToList();
                    var outputPath = "D:\\tempExercises\\DatabaseProject\\DatabaseProject\\usersOutputAgain.json";
                    jsonService.WriteData(outputPath, filteredUsers);

                    var bulkInsertService = scope.Resolve<BulkInsertService<User>>();
                    bulkInsertService.InsertInBatches(users, 10);

                    foreach (var u in context.Users)
                    {
                        Console.WriteLine($"Username: {u.Username}, Email: {u.Email}");
                    }

                    foreach (var p in context.Posts)
                    {
                        Console.WriteLine($"Content: {p.Content}");
                    }

                    foreach (var p in context.Profiles)
                    {
                        Console.WriteLine($"Profile1: {p.Bio}");
                    }

                    Console.ReadLine();
            }
        }
    }
}