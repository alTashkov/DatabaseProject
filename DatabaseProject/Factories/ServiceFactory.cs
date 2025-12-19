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

        /// <summary>
        /// Resolves a service and makes it generic, if it's not one already.
        /// </summary>
        /// <param name="entityType">The type of entity being processed.</param>
        /// <param name="serviceType">The service type being processed.</param>
        /// <returns>The resolved generic service object.</returns>
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
