namespace Maestro.Operational.TimestampProvider;

public interface ITimestampProvider
{
    DateTime Get(string key);
    void Set(string key, DateTime timestamp);
}