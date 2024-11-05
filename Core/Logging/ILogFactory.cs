namespace Core.Logging;

public interface ILogFactory
{
    ILog ForContext<T>();
}