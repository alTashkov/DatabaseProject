namespace DatabaseProject.Interfaces
{
    public interface IDataUpdater<T> where T : class
    {
        public bool UpdateEntityData<TKey>(TKey primaryKey, string propertyValue, string property);
    }
}
