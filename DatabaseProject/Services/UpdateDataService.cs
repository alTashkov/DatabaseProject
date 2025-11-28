using DatabaseProject.Data;
using Microsoft.Extensions.Logging;

namespace DatabaseProject.Services
{
    public class UpdateDataService <T> where T : class
    {
        private readonly SocialMediaContext _context;
        private readonly ILogger<UpdateDataService<T>> _logger;

        public UpdateDataService(SocialMediaContext context, ILogger<UpdateDataService<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        ///     Finds entity by primary key and changes its parameters based on given value.
        /// </summary>
        /// <typeparam name="TKey">The type of primary key of the entity.s</typeparam>
        /// <param name="primaryKey">The value of the primary key.</param>
        /// <param name="propertyValue">The new value that will be assigned to the given property.</param>
        /// <param name="property">The chosen property to perform changes to.</param>
        /// <returns>
        ///     <b>True</b> if the property of the entity was successfully alitered;<br></br>
        ///     <b>False</b> if the entity with matching primary key was not found or no property was chosen.
        /// </returns>
        public bool UpdateEntityData<TKey>(TKey primaryKey, string propertyValue, string property)
        {
            var entity = _context.Set<T>().Find(primaryKey);
            if (entity == null)
            {
                _logger.LogError("Entity of type {EntityType} with primary key {PrimaryKey} was not found.",
                typeof(T).Name, primaryKey);

                return false;
            }

            if (!(string.IsNullOrEmpty(property)))
            {
                var propInfo = typeof(T).GetProperty(property);

                if (propInfo != null)
                {
                    var typedValue = Convert.ChangeType(propertyValue, propInfo.PropertyType);

                    propInfo.SetValue(entity, typedValue);
                    _context.SaveChanges();

                    _logger.LogInformation("Entity of type {EntityType} with id {EntityId} was successfully updated.",
                    typeof(T).Name, primaryKey);

                    return true;
                }
                else
                {
                    _logger.LogError("Property of type {Property} with id {EntityId} was not found.",
                     property, primaryKey);

                    return false;
                }
            }
            else
            {
                _logger.LogError("No property was chosen.");
                return false;
            }
        }
    }
}
