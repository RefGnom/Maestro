namespace Maestro.Core.Logging;

public interface ILogFactory
{
    ILog<T> ForContext<T>();
}