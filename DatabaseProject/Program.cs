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
            using (ILifetimeScope scope = container.BeginLifetimeScope()) 
            {
                SocialMediaContext context = scope.Resolve<SocialMediaContext>();
                context.Database.Migrate();

                Console.WriteLine("Social media DBMS: ");

                bool isEndOperation = false;
                var opeartionManager = scope.Resolve<OperationManager>();
                while (!isEndOperation)
                {
                    Console.WriteLine("\nPlease choose an operation \n1. Insert\n2. Read\n3. Update\n4. Delete\n5. Exit");
                    Console.WriteLine("\nOperation:");
                    if (int.TryParse(Console.ReadLine(), out int operationType))
                    {
                        if (operationType != 5)
                        {
                            switch (operationType)
                            {
                                case 1:
                                    Console.WriteLine("\nOPERATION - Insert");
                                    Console.WriteLine("Please enter path to file to read from: ");
                                    opeartionManager.Insert(Console.ReadLine() ?? "");
                                    break;
                                case 2:
                                    Console.WriteLine("\nOPERATION - Read");
                                    Console.WriteLine("Please enter path to file to read to: ");
                                    opeartionManager.ReadWithFilter(Console.ReadLine() ?? "");
                                    break;
                                case 3:
                                    Console.WriteLine("\nOPERATION - Update");
                                    Console.WriteLine("Enter entity type: ");
                                    opeartionManager.Update(Console.ReadLine() ?? "");
                                    break;

                                case 4:
                                    Console.WriteLine("\nOPERATION - Delete");
                                    Console.WriteLine("Enter entity type: ");
                                    opeartionManager.Delete(Console.ReadLine() ?? "");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Closing social media CLI...");
                            isEndOperation = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}