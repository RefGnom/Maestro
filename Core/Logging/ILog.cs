namespace Maestro.Core.Logging;

public interface ILog<TContext>
{
    void Info(string message);
    void Warn(string message);
    void Error(string message);
}