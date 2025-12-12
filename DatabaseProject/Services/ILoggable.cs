using Microsoft.Extensions.Logging;

namespace DatabaseProject.Services
{
    public interface ILoggable
    {
        protected ILogger Logger { get; }
    }
}
