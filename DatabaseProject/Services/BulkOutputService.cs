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

        /// <summary>
        /// Filters data based on a given expression and writes it to a new file.
        /// </summary>
        /// <param name="jsonFileService">The helper service from JSON operations.</param>
        /// <param name="outputJsonPath">The output path of the new file.</param>
        /// <param name="filter">The filter expression that will be applied to the data.</param>
        /// <returns>
        /// <b>True</b> if the filtered data was successfully written to file;<br></br>
        /// <b>False</b> if the filtered data was not written to a new file or no filter was chosen.
        /// </returns>
        public bool OutputFilteredDataToFile(JsonFileService<T> jsonFileService, string outputJsonPath, Expression<Func<T, bool>> filter = null)
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                _logger.LogError("Filter must be chosen.");
                return false;
            }
        }
    }
}
