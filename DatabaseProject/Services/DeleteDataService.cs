using DatabaseProject.Data;
using Microsoft.Extensions.Logging;

namespace DatabaseProject.Services
{
    public class DeleteDataService<T> where T : class
    {
        private readonly SocialMediaContext _context;
        private readonly ILogger<DeleteDataService<T>> _logger;

        public DeleteDataService(SocialMediaContext context, ILogger<DeleteDataService<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public bool DeleteEntityData<TKey>(TKey entityId)
        {
            var entity = _context.Set<T>().Find(entityId);

            if (entity == null)
            {
                _logger.LogError("Entity of type {EntityType} with id {EntityId} was not found.",
                typeof(T).Name, entityId);

                return false;
            }
            else
            {
                _context.Remove(entity);
                _context.SaveChanges();

                _logger.LogInformation("Entity of type {EntityType} with id {EntityId} was successfully deleted.",
                typeof(T).Name, entityId);

                return true;
            } 
        }
    }
}
