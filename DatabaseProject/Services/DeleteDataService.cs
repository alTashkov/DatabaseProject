using DatabaseProject.Data;
using System.Linq;
using System.Linq.Expressions;

namespace DatabaseProject.Services
{
    public class DeleteDataService<T> where T : class
    {
        private readonly SocialMediaContext _context;

        public DeleteDataService(SocialMediaContext context)
        {
            _context = context;
        }

        public void DeleteEntityData<TKey>(TKey entityId)
        {
            var entity = _context.Set<T>().Find(entityId);

            if (entity == null)
            {
                throw new ArgumentException($"No entity was found with id = {entityId}.");
            }
            else
            {
                _context.Remove(entity);
                _context.SaveChanges();
            } 
        }
    }
}
