using DatabaseProject.Data;
using System.Linq;
using System.Linq.Expressions;

namespace DatabaseProject.Services
{
    public class UpdateDataService <T> where T : class
    {
        private readonly SocialMediaContext _context;

        public UpdateDataService(SocialMediaContext context)
        {
            _context = context;
        }

        public void UpdateEntityData<TKey>(TKey entityId, string propertyValue, string property)
        {
            var entity = _context.Set<T>().Find(entityId);
            if (entity == null)
            {
                throw new ArgumentException($"No entity was found with id = {entityId}.");
            }

            if (!(string.IsNullOrEmpty(property)))
            {
                var propInfo = typeof(T).GetProperty(property);

                if (propInfo != null)
                {
                    var typedValue = Convert.ChangeType(propertyValue, propInfo.PropertyType);

                    propInfo.SetValue(entity, typedValue);
                    _context.SaveChanges();
                }
            }
            else
            {
                throw new ArgumentException($"Property `{property}` does not exist for type `{typeof(T).Name}");
            }
        }
    }
}
