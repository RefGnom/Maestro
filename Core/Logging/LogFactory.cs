using Core.IO;
using Core.Providers;

namespace Core.Logging;

public class LogFactory(IDateTimeProvider dateTimeProvider, IWriter writer) : ILogFactory
{
    public ILog<T> ForContext<T>()
    {
        return new Log<T>(dateTimeProvider, writer);
    }
}