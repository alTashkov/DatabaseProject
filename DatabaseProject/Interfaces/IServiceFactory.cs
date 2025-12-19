namespace DatabaseProject.Interfaces
{
    public interface IServiceFactory
    {
        object ResolveProcessor(Type entityType, Type serviceType);
    }
}
