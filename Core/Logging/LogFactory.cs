using Maestro.Core.IO;
using Maestro.Core.Providers;

namespace Maestro.Core.Logging;

public class LogFactory(IDateTimeProvider dateTimeProvider, IWriter writer) : ILogFactory
{
    [Obsolete("Не использовать. Нужен для логов на этапе конфигурации")]
    public static LogFactory ClosedFactory => new(new DateTimeProvider(), new Writer());

    public ILog<T> ForContext<T>()
    {
        return new Log<T>(dateTimeProvider, writer);
    }
}