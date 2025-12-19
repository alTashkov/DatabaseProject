using DatabaseProject.Data;
using DatabaseProject.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DatabaseProject.Services
{
    public class BulkOutputService<T> : IBulkOutputter<T>, ILoggable where T : class
    {
        private readonly SocialMediaContext _context;
        public ILogger Logger { get; }

        public BulkOutputService(SocialMediaContext context, ILogger<IBulkExporter<T>> logger) 
        {
            _context = context;
            Logger = logger;
        }

        /// <param name="jsonFileService">The helper service from JSON operations.</param>
        /// <param name="outputJsonPath">The output path of the new file.</param>
        /// <param name="filter">The filter expression that will be applied to the data.</param>
        /// <returns>
        /// <b>True</b> if the filtered data was successfully written to file;<br></br>
        /// <b>False</b> if the filtered data was not written to a new file or no filter was chosen.
        /// </returns>
        public bool OutputFilteredDataToFile(IJsonProcessor<T> jsonFileService, string outputJsonPath, Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
                var filteredEntities = query.ToList();

                if (jsonFileService.WriteDataToFile(outputJsonPath, filteredEntities))
                {
                    Logger.LogInformation("Entity of type {EntityType} was successfully written to {FilePath}.",
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
                Logger.LogError("Filter must be chosen.");
                return false;
            }
        }
    }
}
