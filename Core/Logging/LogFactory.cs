using Maestro.Core.IO;
using Maestro.Core.Providers;

namespace Maestro.Core.Logging;

public class LogFactory(IDateTimeProvider dateTimeProvider, IWriter writer) : ILogFactory
{
    public ILog<T> ForContext<T>()
    {
        return new Log<T>(dateTimeProvider, writer);
    }
}