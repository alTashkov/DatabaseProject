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

        /// <summary>
        /// Deletes the entity from the database which has a matching primary key.
        /// </summary>
        /// <typeparam name="TKey">The primary key of the given entity.</typeparam>
        /// <param name="primaryKey">The value of the primary key.</param>
        /// <returns>
        /// <b>True</b> if the entity was successfully removed from the database;<br></br>
        /// <b>False</b> if an entity with a matching primary key was not found.
        /// </returns>
        public bool DeleteEntityData<TKey>(TKey primaryKey)
        {
            var entity = _context.Set<T>().Find(primaryKey);

            if (entity == null)
            {
                _logger.LogError("Entity of type {EntityType} with id {EntityId} was not found.",
                typeof(T).Name, primaryKey);

                return false;
            }
            else
            {
                _context.Remove(entity);
                _context.SaveChanges();

                _logger.LogInformation("Entity of type {EntityType} with id {EntityId} was successfully deleted.",
                typeof(T).Name, primaryKey);

                return true;
            } 
        }
    }
}
