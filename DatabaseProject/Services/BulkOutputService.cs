using DatabaseProject.Data;
using System.Linq.Expressions;

namespace DatabaseProject.Services
{
    public class BulkOutputService<T> where T : class
    {
        private readonly SocialMediaContext _context;

        public BulkOutputService(SocialMediaContext context) 
        {
            _context = context;
        }

        public void OutputFilteredDataToFile(JsonFileService<T> jsonFileService,string outputJsonPath, Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var filteredEntities = query.ToList();

            jsonFileService.WriteDataToFile(outputJsonPath, filteredEntities);
        }
    }
}
