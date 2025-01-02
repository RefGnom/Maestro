namespace Maestro.Server.Configurators;

public static class LoggingConfigurator
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.AddSimpleConsole();
    }
}