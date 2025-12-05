using Autofac;
using DatabaseProject.Data;
using DatabaseProject.Helpers;
using DatabaseProject.Models;
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

                Console.WriteLine("Social media DBMS");
                Console.WriteLine("Please choose an operation (\n1. Insert\n2. Read\n3. Update\n4. Delete: ");
                if (int.TryParse(Console.ReadLine(), out int operartionType))
                {
                    switch (operartionType)
                    {
                        case 1:
                            Console.WriteLine("\nOPERATION: Insert");
                            Console.WriteLine("Please enter path to file to read from: ");

                            string? inputFilePath = Console.ReadLine();
                            if (inputFilePath != null)
                            {
                                Console.WriteLine("Enter entity type: ");
                                string? insertEntityType = Console.ReadLine();

                                if (!(string.IsNullOrEmpty(insertEntityType)))
                                {
                                    var entityTypes = context.Model.GetEntityTypes();

                                    string normalizedEntityType = insertEntityType?.Trim() ?? string.Empty;
                                    
                                    bool exists = entityTypes.Any(e => e.ClrType.Name.Equals(normalizedEntityType, 
                                        StringComparison.OrdinalIgnoreCase));
                                    if (exists)
                                    {
                                        Console.WriteLine($"Processing entity type {char.ToUpper(normalizedEntityType[0]) + 
                                            normalizedEntityType.Substring(1)}");
                                        
                                        switch (insertEntityType?.Trim().ToLower())
                                        {
                                            case "user":
                                                DataProcessor.Insert<User>(scope, inputFilePath);
                                                break;
                                            case "post":
                                                DataProcessor.Insert<Post>(scope, inputFilePath);
                                                break;
                                            case "comment":
                                                DataProcessor.Insert<Comment>(scope, inputFilePath);
                                                break;
                                            case "friendship":
                                                DataProcessor.Insert<Friendship>(scope, inputFilePath);
                                                break;
                                            case "group":
                                                DataProcessor.Insert<Group>(scope, inputFilePath);
                                                break;
                                            case "profile":
                                                DataProcessor.Insert<Profile>(scope, inputFilePath);
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("File path must be entered to continue operation!");
                            }
                            break;

                        case 2:
                            Console.WriteLine("\nOPERATION: Read");
                            Console.WriteLine("Please enter path to file to read to: ");

                            string? outputFilePath = Console.ReadLine();
                            if (outputFilePath != null)
                            {
                                Console.WriteLine("Enter entity type: ");
                                string? readEntityType = Console.ReadLine();

                                if (!(string.IsNullOrEmpty(readEntityType)))
                                {
                                    var entityTypes = context.Model.GetEntityTypes();

                                    string normalizedEntityType = readEntityType?.Trim() ?? string.Empty;

                                    bool exists = entityTypes.Any(e => e.ClrType.Name.Equals(normalizedEntityType,
                                        StringComparison.OrdinalIgnoreCase));
                                    if (exists)
                                    {
                                        Console.WriteLine($"Processing entity type {char.ToUpper(normalizedEntityType[0]) +
                                            normalizedEntityType.Substring(1)}");

                                        switch (readEntityType?.Trim().ToLower())
                                        {
                                            case "user":
                                                DataProcessor.Read<User>(scope, outputFilePath);
                                                break;
                                            case "post":
                                                DataProcessor.Read<Post>(scope, outputFilePath);
                                                break;
                                            case "comment":
                                                DataProcessor.Read<Comment>(scope, outputFilePath);
                                                break;
                                            case "friendship":
                                                DataProcessor.Read<Friendship>(scope, outputFilePath);
                                                break;
                                            case "group":
                                                DataProcessor.Read<Group>(scope, outputFilePath);
                                                break;
                                            case "profile":
                                                DataProcessor.Read<Profile>(scope, outputFilePath);
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("File path must be entered to continue operation!");
                            }
                            break;

                        case 3:
                            Console.WriteLine("\nOPERATION: Update");

                            Console.WriteLine("Enter entity type: ");
                            string? entityType = Console.ReadLine();

                            if (!string.IsNullOrEmpty(entityType))
                            {
                                var entityTypes = context.Model.GetEntityTypes();
                                string normalizedEntityType = entityType.Trim();

                                var entityTypeMetadata = entityTypes
                                    .FirstOrDefault(e => e.ClrType.Name.Equals(normalizedEntityType, StringComparison.OrdinalIgnoreCase));

                                if (entityTypeMetadata != null)
                                {
                                    // Get primary key type
                                    var keyProperty = entityTypeMetadata.FindPrimaryKey()?.Properties.FirstOrDefault();
                                    if (keyProperty == null)
                                    {
                                        Console.WriteLine($"Entity {normalizedEntityType} has no primary key defined.");
                                        break;
                                    }

                                    Type keyType = keyProperty.ClrType;

                                    Console.WriteLine("Enter primary key value: ");
                                    string? pkInput = Console.ReadLine();
                                    object? parsedKey = null;

                                    if (keyType == typeof(int) && int.TryParse(pkInput, out int intKey))
                                        parsedKey = intKey;
                                    else if (keyType == typeof(Guid) && Guid.TryParse(pkInput, out Guid guidKey))
                                        parsedKey = guidKey;
                                    else if (keyType == typeof(string))
                                        parsedKey = pkInput;
                                    else
                                    {
                                        Console.WriteLine("Unsupported primary key type or invalid input.");
                                        break;
                                    }

                                    Console.WriteLine("Enter property to update: ");
                                    string? property = Console.ReadLine();

                                    if (!string.IsNullOrEmpty(property))
                                    {
                                        bool propertyExists = entityTypeMetadata.GetProperties()
                                            .Any(p => p.Name.Equals(property, StringComparison.OrdinalIgnoreCase));

                                        if (!propertyExists)
                                        {
                                            Console.WriteLine($"Property {property} doesn't exist on {normalizedEntityType}.");
                                            break;
                                        }

                                        Console.WriteLine("Enter property value you want to set: ");
                                        string? propertyValue = Console.ReadLine();

                                        if (propertyValue != null)
                                        {
                                            // Dynamically call DataProcessor.Update<T, TKey>
                                            var method = typeof(DataProcessor).GetMethod("Update");
                                            var genericMethod = method.MakeGenericMethod(entityTypeMetadata.ClrType, keyType);

                                            genericMethod.Invoke(null, new object[] { scope, parsedKey!, propertyValue, property });

                                            Console.WriteLine($"{char.ToUpper(normalizedEntityType[0]) +
                                            normalizedEntityType.Substring(1)} with key {parsedKey} updated successfully.");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Entity type '{entityType}' not found in the model.");
                                }
                            }
                            break;

                        case 4:
                            Console.WriteLine("\nOPERATION: Delete");

                            Console.WriteLine("Enter entity type: ");
                            string? deleteEntityType = Console.ReadLine();

                            if (!string.IsNullOrEmpty(deleteEntityType))
                            {
                                var entityTypes = context.Model.GetEntityTypes();
                                string normalizedEntityType = deleteEntityType.Trim();

                                var entityTypeMetadata = entityTypes
                                    .FirstOrDefault(e => e.ClrType.Name.Equals(normalizedEntityType, StringComparison.OrdinalIgnoreCase));

                                if (entityTypeMetadata != null)
                                {
                                    // Get primary key type
                                    var keyProperty = entityTypeMetadata.FindPrimaryKey()?.Properties.FirstOrDefault();
                                    if (keyProperty == null)
                                    {
                                        Console.WriteLine($"Entity {normalizedEntityType} has no primary key defined.");
                                        break;
                                    }

                                    Type keyType = keyProperty.ClrType;

                                    Console.WriteLine("Enter primary key value: ");
                                    string? pkInput = Console.ReadLine();
                                    object? parsedKey = null;

                                    if (keyType == typeof(int) && int.TryParse(pkInput, out int intKey))
                                        parsedKey = intKey;
                                    else if (keyType == typeof(Guid) && Guid.TryParse(pkInput, out Guid guidKey))
                                        parsedKey = guidKey;
                                    else if (keyType == typeof(string))
                                        parsedKey = pkInput;
                                    else
                                    {
                                        Console.WriteLine("Unsupported primary key type or invalid input.");
                                        break;
                                    }

                                    // Dynamically call DataProcessor.Delete<T, TKey>
                                    var method = typeof(DataProcessor).GetMethod("Delete");
                                    var genericMethod = method.MakeGenericMethod(entityTypeMetadata.ClrType, keyType);

                                    genericMethod.Invoke(null, new object[] { scope, parsedKey! });

                                    Console.WriteLine($"{char.ToUpper(normalizedEntityType[0]) +
                                        normalizedEntityType.Substring(1)} with key {parsedKey} deleted successfully.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Entity type '{deleteEntityType}' not found in the model.");
                                }
                                break;
                    }
                }
            }
        }
    }
}