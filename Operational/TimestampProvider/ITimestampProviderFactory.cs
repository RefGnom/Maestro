namespace Maestro.Operational.TimestampProvider;

public interface ITimestampProviderFactory
{
    ITimestampProvider Create(TimestampProviderOptions? options = null);
}