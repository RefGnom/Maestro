namespace Maestro.Operational.TimestampProvider;

public class TimestampProviderFactory : ITimestampProviderFactory
{
    public ITimestampProvider Create(TimestampProviderOptions? options = null)
    {
        return new TimestampProvider(options ?? new TimestampProviderOptions());
    }
}