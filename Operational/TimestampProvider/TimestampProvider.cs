namespace Maestro.Operational.TimestampProvider;

public class TimestampProvider(TimestampProviderOptions timestampProviderOptions) : ITimestampProvider
{
    private readonly TimestampProviderOptions _timestampProviderOptions = timestampProviderOptions;

    public DateTime Read(string key) => Find(key) ?? throw new ArgumentException($"Не нашли timestamp по ключу {key}");

    public DateTime? Find(string key)
    {
        var timestamps = ReadTimestampDictionary();
        if (timestamps.TryGetValue(key, out var timestamp))
        {
            return timestamp;
        }

        return null;
    }

    public void Set(string key, DateTime timestamp)
    {
        var timestamps = ReadTimestampDictionary();
        timestamps[key] = timestamp;
        var lines = timestamps.Select(x => $"{x.Key}={x.Value}");
        File.WriteAllLines(_timestampProviderOptions.FullPath, lines, _timestampProviderOptions.Encoding);
    }

    private Dictionary<string, DateTime> ReadTimestampDictionary()
    {
        var fileExist = File.Exists(_timestampProviderOptions.FullPath);
        return fileExist
            ? File.ReadAllLines(_timestampProviderOptions.FullPath, _timestampProviderOptions.Encoding)
                .Select(x => x.Split('=').Select(y => y.Trim()).ToArray())
                .ToDictionary(x => x[0], x => DateTime.Parse(x[1]))
            : new Dictionary<string, DateTime>();
    }
}