using Microsoft.Extensions.Logging;

namespace DatabaseProject.Services
{
    public interface IServiceWithLogger
    {
        protected ILogger Logger { get; }
    }
}
