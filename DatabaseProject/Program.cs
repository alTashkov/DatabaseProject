using Autofac;
using DatabaseProject.Data;
using DatabaseProject.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DatabaseProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            ContainerConfig.Configure(builder);

            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope()) 
            {
                SocialMediaContext context = scope.Resolve<SocialMediaContext>();
                context.Database.Migrate();

                Console.WriteLine("Social media DBMS: ");

                bool shouldEnd = false;
                while (!shouldEnd)
                {
                    Console.WriteLine("\nPlease choose an operation \n1. Insert\n2. Read\n3. Update\n4. Delete\n5. Exit");
                    Console.WriteLine("\nOperation:");
                    if (int.TryParse(Console.ReadLine(), out int operationType))
                    {
                        var opeartionManager = scope.Resolve<OperationManager>();
                        if (operationType != 5)
                        {
                            switch (operationType)
                            {
                                case 1:
                                    Console.WriteLine("\nOPERATION - Insert");
                                    Console.WriteLine("Please enter path to file to read from: ");

                                    string? inputFilePath = Console.ReadLine();
                                    if (!(string.IsNullOrEmpty(inputFilePath)))
                                    {
                                        opeartionManager.Insert(inputFilePath);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input file path! Please try again.");
                                        break;
                                    }
                                break;

                                case 2:
                                    Console.WriteLine("\nOPERATION - Read");
                                    Console.WriteLine("Please enter path to file to read to: ");

                                    string? outputFilePath = Console.ReadLine();
                                    if (!(string.IsNullOrEmpty(outputFilePath)))
                                    {
                                        opeartionManager.ReadWithFilter(outputFilePath);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid output file path! Please try again.");
                                        break;
                                    }
                                break;

                                case 3:
                                    Console.WriteLine("\nOPERATION - Update");
                                    Console.WriteLine("Enter entity type: ");

                                    string? entityType = Console.ReadLine();
                                    if (!(string.IsNullOrEmpty(entityType)))
                                    {
                                        opeartionManager.Update(entityType);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid entity type! Please try again.");
                                        break;
                                    }
                                break;

                                case 4:
                                    Console.WriteLine("\nOPERATION - Delete");
                                    Console.WriteLine("Enter entity type: ");

                                    string? deleteEntityType = Console.ReadLine();
                                    if (!(string.IsNullOrEmpty(deleteEntityType)))
                                    {
                                        opeartionManager.Delete(deleteEntityType);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid entity type! Please try again.");
                                        break;
                                    }
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Closing social media CLI...");
                            shouldEnd = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}