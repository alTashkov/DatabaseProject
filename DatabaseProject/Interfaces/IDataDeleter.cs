namespace DatabaseProject.Interfaces
{
    /// <summary>
    /// Provides methods for deleting data from the database.
    /// </summary>
    public interface IDataDeleter<T> where T : class
    {
        /// <summary>
        /// Deletes the entity from the database which has a matching primary key.
        /// </summary>
        public bool DeleteEntityData<TKey>(TKey primaryKey);
    }
}
