using DatabaseProject.Data;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DatabaseProject.Services
{
    public class BulkOutputService<T> where T : class
    {
        private readonly SocialMediaContext _context;
        private readonly ILogger<BulkOutputService<T>> _logger;

        public BulkOutputService(SocialMediaContext context, ILogger<BulkOutputService<T>> logger) 
        {
            _context = context;
            _logger = logger;
        }

        public void OutputFilteredDataToFile(JsonFileService<T> jsonFileService, string outputJsonPath, Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
                var filteredEntities = query.ToList();

                if (jsonFileService.WriteDataToFile(outputJsonPath, filteredEntities))
                {
                    _logger.LogInformation("Entity of type {EntityType} was successfully written to {FilePath}.",
                    typeof(T).Name, outputJsonPath);
                }
            }
            else
            {
                _logger.LogError("Filter must be chosen.");
            }
        }
    }
}
