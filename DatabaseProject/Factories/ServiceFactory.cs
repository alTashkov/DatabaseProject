using Autofac;
using DatabaseProject.Interfaces;

namespace DatabaseProject.Factories
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly ILifetimeScope _scope;

        public ServiceFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public object ResolveProcessor(Type entityType, Type serviceType)
        {
            if (!serviceType.IsGenericTypeDefinition)
            {
                return _scope.Resolve(serviceType);
            }

            var closedType = serviceType.MakeGenericType(entityType);
            return _scope.Resolve(closedType);
        }
    }
}
