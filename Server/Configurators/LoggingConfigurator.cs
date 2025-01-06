namespace Maestro.Server.Configurators;

public static class LoggingConfigurator
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.AddSimpleConsole(options =>
        {
            options.IncludeScopes = true;
            options.UseUtcTimestamp = false;
            options.TimestampFormat = "[HH:mm:ss.fff] ";
        });
        builder.Logging.Configure(options => options.ActivityTrackingOptions = ActivityTrackingOptions.TraceId);
    }
}