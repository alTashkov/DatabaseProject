using Autofac;
using DatabaseProject.Data;
using DatabaseProject.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace DatabaseProject.Helpers
{
    public static class OperationManager
    {
        /// <summary>
        /// Reads data from the database based on an input query and saves it in a file.
        /// </summary>
        /// <param name="outputFilePath">The path to the file used for outputting data to.</param>
        /// <param name="scope">The lifetime of the container.</param>
        /// <param name="context">The context for the database.</param>
        public static void ReadWithFilter(string outputFilePath, ILifetimeScope scope, SocialMediaContext context)
        {
            if (outputFilePath != null)
            {
                Console.WriteLine("Enter entity type: ");
                string? readEntityType = Console.ReadLine();

                if (!(string.IsNullOrEmpty(readEntityType)))
                {
                    var entityTypeMetadata = GetEntityTypeMetadata(readEntityType, context);
                    if (entityTypeMetadata != null)
                    {
                        Console.WriteLine($"Processing entity type {entityTypeMetadata.Name}");

                        var clrType = entityTypeMetadata.ClrType;

                        // Build the generic types and resolve.
                        var processorType = typeof(IJsonProcessor<>).MakeGenericType(clrType);
                        var jsonFileService = scope.Resolve(processorType);

                        Console.WriteLine("Enter a query to filter data by: ");
                        var inputFilter = Console.ReadLine();

                        var buildFilterMethod = typeof(AdvancedFilterBuilder).GetMethod("BuildFilter")!.MakeGenericMethod(clrType);
                        if (!(string.IsNullOrEmpty(inputFilter)))
                        {
                            var builtFilter = buildFilterMethod.Invoke(null, new object[] { inputFilter });

                            var outputterType = typeof(IBulkExporter<>).MakeGenericType(clrType);
                            var bulkOutputService = scope.Resolve(outputterType);

                            var method = typeof(DataProcessor).GetMethod("Read")!.MakeGenericMethod(clrType);
                            try
                            {
                                method.Invoke(null, new object[] { jsonFileService, bulkOutputService, scope, outputFilePath, builtFilter! });
                            }
                            catch (TargetInvocationException ex)
                            {
                                Console.WriteLine($"Operation failed: {ex.InnerException?.Message ?? ex.Message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid filter!");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("File path must be entered to continue operation!");
            }
        }

        /// <summary>
        /// Inserts data into the database from a file.
        /// </summary>
        /// <param name="inputFilePath">The path to the file used for input.</param>
        /// <param name="context">The database context.</param>
        /// <param name="scope">The lifetime of the container.</param>
        public static void Insert(string inputFilePath, SocialMediaContext context, ILifetimeScope scope)
        {
            if (inputFilePath != null)
            {
                Console.WriteLine("Enter entity type: ");
                string? insertEntityType = Console.ReadLine();

                if (!(string.IsNullOrEmpty(insertEntityType)))
                {
                    var entityTypeMetadata = GetEntityTypeMetadata(insertEntityType!, context);
                    if (entityTypeMetadata != null)
                    {
                        Console.WriteLine($"Processing entity type {entityTypeMetadata.Name}");

                        var clrType = entityTypeMetadata.ClrType;

                        // Build the generic types and resolve.
                        var processorType = typeof(IJsonProcessor<>).MakeGenericType(clrType);
                        var jsonFileService = scope.Resolve(processorType);

                        var inserterType = typeof(IBulkInserter<>).MakeGenericType(clrType);
                        var bulkInsertService = scope.Resolve(inserterType);

                        var method = typeof(DataProcessor).GetMethod("Insert")!
                            .MakeGenericMethod(clrType);
                        try
                        {
                            method?.Invoke(null, new object[] { jsonFileService, bulkInsertService, scope, inputFilePath });
                        }
                        catch (TargetInvocationException ex)
                        {
                            Console.WriteLine($"Operation failed: {ex.InnerException?.Message ?? ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("File path must be entered to continue operation!");
                }
            }
        }

        /// <summary>
        /// Updates entity data in the database based on some property value.
        /// </summary>
        /// <param name="entityType">The type of entity being processed.</param>
        /// <param name="context">The database context.</param>
        /// <param name="scope">The lifetime of the container.</param>
        public static void Update(string entityType, SocialMediaContext context, ILifetimeScope scope)
        {
            var entityTypeMetadata = GetEntityTypeMetadata(entityType, context);
            if (entityTypeMetadata != null)
            {
                // Get primary key type
                var keyProperty = entityTypeMetadata.FindPrimaryKey()?.Properties.FirstOrDefault();
                if (keyProperty == null)
                {
                    Console.WriteLine($"Entity {entityTypeMetadata.Name} has no primary key defined.");
                }

                Type keyType = keyProperty!.ClrType;

                Console.WriteLine("Enter primary key value: ");
                string? pkInput = Console.ReadLine();
                object? parsedKey = null;

                if (keyType == typeof(int) && int.TryParse(pkInput, out int intKey))
                {
                    parsedKey = intKey;
                }
                else if (keyType == typeof(Guid) && Guid.TryParse(pkInput, out Guid guidKey))
                {
                    parsedKey = guidKey;
                }
                else if (keyType == typeof(string))
                {
                    parsedKey = pkInput;
                }
                else
                {
                    Console.WriteLine("Unsupported primary key type or invalid input.");
                }

                Console.WriteLine("Enter property to update: ");
                string? property = Console.ReadLine();

                if (!string.IsNullOrEmpty(property))
                {
                    bool propertyExists = entityTypeMetadata.GetProperties()
                        .Any(p => p.Name.Equals(property, StringComparison.OrdinalIgnoreCase));

                    if (!propertyExists)
                    {
                        Console.WriteLine($"Property {property} doesn't exist on {entityTypeMetadata.Name}.");
                    }

                    Console.WriteLine("Enter property value you want to set: ");
                    string? propertyValue = Console.ReadLine();

                    if (propertyValue != null)
                    {
                        var method = typeof(DataProcessor).GetMethod("Update");

                        var updaterType = typeof(IDataUpdater<>).MakeGenericType(entityTypeMetadata.ClrType);
                        var updateDataService = scope.Resolve(updaterType);

                        var genericMethod = method!.MakeGenericMethod(entityTypeMetadata.ClrType, keyType);
                        try
                        {
                            genericMethod?.Invoke(null, new object[] { updateDataService, scope, parsedKey!, propertyValue, property });
                        }
                        catch (TargetInvocationException ex)
                        {
                            Console.WriteLine($"Operation failed: {ex.InnerException?.Message ?? ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                        }

                        Console.WriteLine($"{entityTypeMetadata.Name} with key {parsedKey} updated successfully.");
                    }
                }
            }
        }

        /// <summary>
        /// Deletes an entry from the database based on its primary key.
        /// </summary>
        /// <param name="deleteEntityType">The type of entity being processed.</param>
        /// <param name="context">The database context.</param>
        /// <param name="scope">The lifetime of the container.</param>
        public static void Delete(string deleteEntityType, SocialMediaContext context, ILifetimeScope scope)
        {
            if (!string.IsNullOrEmpty(deleteEntityType))
            {
                var entityTypeMetadata = GetEntityTypeMetadata(deleteEntityType, context);
                if (entityTypeMetadata != null)
                {
                    // Get primary key type
                    var keyProperty = entityTypeMetadata.FindPrimaryKey()?.Properties.FirstOrDefault();
                    if (keyProperty == null)
                    {
                        Console.WriteLine($"Entity {entityTypeMetadata.Name} has no primary key defined.");
                    }

                    Type keyType = keyProperty!.ClrType;

                    Console.WriteLine("Enter primary key value: ");
                    string? pkInput = Console.ReadLine();
                    object? parsedKey = null;

                    if (keyType == typeof(int) && int.TryParse(pkInput, out int intKey))
                    {
                        parsedKey = intKey;
                    }
                    else if (keyType == typeof(Guid) && Guid.TryParse(pkInput, out Guid guidKey))
                    {
                        parsedKey = guidKey;
                    }
                    else if (keyType == typeof(string))
                    {
                        parsedKey = pkInput;
                    }
                    else
                    {
                        Console.WriteLine("Unsupported primary key type or invalid input.");
                    }

                    var deleterType = typeof(IDataDeleter<>).MakeGenericType(entityTypeMetadata.ClrType);
                    var deleteDataService = scope.Resolve(deleterType);

                    var method = typeof(DataProcessor).GetMethod("Delete");
                    var genericMethod = method!.MakeGenericMethod(entityTypeMetadata.ClrType, keyType);
                    try
                    {
                        genericMethod.Invoke(null, new object[] { deleteDataService, scope, parsedKey! });
                    }
                    catch (TargetInvocationException ex)
                    {
                        Console.WriteLine($"Operation failed: {ex.InnerException?.Message ?? ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                    }

                    Console.WriteLine($"{entityTypeMetadata.Name} with key {parsedKey} deleted successfully.");
                }
            }
        }

        /// <summary>
        /// Gets the metadata for an entity type.
        /// </summary>
        /// <param name="entityTypeName">The type of entity being processed.</param>
        /// <param name="context">The database context.</param>
        /// <returns>The metadata of the entity type, if found in the model.</returns>
        private static IEntityType? GetEntityTypeMetadata(string entityTypeName, SocialMediaContext context)
        {
            if (string.IsNullOrEmpty(entityTypeName))
            {
                Console.WriteLine("Entity type name cannot be empty.");
                return null;
            }

            var normalizedName = entityTypeName.Trim();
            var entityTypes = context.Model.GetEntityTypes();

            var metadata = entityTypes
                .FirstOrDefault(e => e.ClrType.Name.Equals(normalizedName, StringComparison.OrdinalIgnoreCase));

            if (metadata == null)
            {
                Console.WriteLine($"Entity type '{entityTypeName}' not found in the model.");
            }

            return metadata;
        }
    }
}
