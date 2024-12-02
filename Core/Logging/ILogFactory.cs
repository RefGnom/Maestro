namespace Maestro.Core.Logging;

public interface ILogFactory
{
    ILog<T> CreateGenericLog<T>();
    ILog CreateLog<T>();
}