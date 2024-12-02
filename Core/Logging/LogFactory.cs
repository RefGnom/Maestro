using Maestro.Core.IO;
using Maestro.Core.Providers;

namespace Maestro.Core.Logging;

public class LogFactory(IDateTimeProvider dateTimeProvider, IWriter writer) : ILogFactory
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IWriter _writer = writer;

    [Obsolete("Не использовать. Нужен для логов на этапе конфигурации")]
    public static LogFactory ClosedFactory => new(new DateTimeProvider(), new Writer());

    public ILog<T> ForContext<T>()
    {
        return new Log<T>(_dateTimeProvider, _writer);
    }
}