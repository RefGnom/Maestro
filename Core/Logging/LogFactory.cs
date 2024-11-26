using Core.IO;
using Core.Providers;

namespace Core.Logging;

public class LogFactory(IDateTimeProvider dateTimeProvider, IWriter writer) : ILogFactory
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IWriter _writer = writer;

    public ILog ForContext<T>()
    {
        return new Log(_dateTimeProvider, _writer, typeof(T).Name);
    }
}