using Microsoft.Extensions.Logging;

namespace DatabaseProject.Interfaces
{
    public interface ILoggable
    {
        protected ILogger Logger { get; }
    }
}
