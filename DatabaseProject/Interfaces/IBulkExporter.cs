using System.Linq.Expressions;

namespace DatabaseProject.Interfaces
{
    /// <summary>
    /// Provides methods for outputting data from the database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBulkExporter<T> where T : class
    {
        /// <summary>
        /// Outputs filtered data from the database to a file.
        /// </summary>
        /// <param name="jsonFileService">The helper service from JSON operations.</param>
        /// <param name="outputJsonPath">The output path of the new file.</param>
        /// <param name="filter">The filter expression that will be applied to the data.</param>
        /// <returns>
        /// <b>True</b> if the filtered data was successfully written to file;<br></br>
        /// <b>False</b> if the filtered data was not written to a new file or no filter was chosen.
        /// </returns>
        public bool OutputFilteredDataToFile(IJsonProcessor<T> jsonFileService, string outputJsonPath, Expression<Func<T, bool>> filter);
    }
}
