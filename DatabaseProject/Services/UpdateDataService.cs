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

        public bool UpdateEntityData<TKey>(TKey entityId, string propertyValue, string property)
        {
            var entity = _context.Set<T>().Find(entityId);
            if (entity == null)
            {
                _logger.LogError("Entity of type {EntityType} with id {EntityId} was not found.",
                typeof(T).Name, entityId);

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
                    typeof(T).Name, entityId);

                    return true;
                }
                else
                {
                    _logger.LogError("Property of type {Property} with id {EntityId} was not found.",
                     property, entityId);

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
