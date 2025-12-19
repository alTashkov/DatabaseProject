using System.Linq.Expressions;

namespace DatabaseProject.Interfaces
{
    /// <summary>
    /// Provides methods for outputting data from the database.
    /// </summary>
    public interface IBulkExporter<T> where T : class
    {
        /// <summary>
        /// Outputs filtered data from the database to a file.
        /// </summary>
        public bool OutputFilteredDataToFile(IJsonProcessor<T> jsonFileService, string outputJsonPath, Expression<Func<T, bool>> filter);
    }
}
