namespace Maestro.Operational.TimestampProvider;

public interface ITimestampProvider
{
    DateTime Read(string key);
    DateTime? Find(string key);
    void Set(string key, DateTime timestamp);
}