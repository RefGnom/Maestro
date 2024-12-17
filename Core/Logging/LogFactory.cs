using Maestro.Core.IO;
using Maestro.Core.Providers;

namespace Maestro.Core.Logging;

public class LogFactory(IDateTimeProvider dateTimeProvider, IWriter writer) : ILogFactory
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IWriter _writer = writer;


    public ILog<T> CreateLog<T>()
    {
        return new Log<T>(_dateTimeProvider, _writer);
    }
}