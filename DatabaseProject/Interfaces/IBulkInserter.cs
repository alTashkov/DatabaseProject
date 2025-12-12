
namespace DatabaseProject.Interfaces
{
    /// <summary>
    /// Provides methods for inserting data in bulk to the database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBulkInserter<T> where T : class 
    {
        /// <summary>
        /// Inserts a fixed amount of entities into the database.
        /// </summary>
        public bool InsertInBatches(List<T> entities, int batchSize);
    }
}
