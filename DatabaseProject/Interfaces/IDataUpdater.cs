namespace DatabaseProject.Interfaces
{
    public interface IDataUpdater<T> where T : class
    {
        /// <summary>
        /// Finds entity by primary key and changes its parameters based on given value.
        /// </summary>
        public bool UpdateEntityData<TKey>(TKey primaryKey, string propertyValue, string property);
    }
}
