using Maestro.Server.Configurators;
using Maestro.Server.Middlewares.Extensions;

namespace Maestro.Server;

// ReSharper disable once ClassNeverInstantiated.Global
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext(builder.Configuration);
        builder.Services.AddRepositories();
        builder.Services.AddMapper();
        builder.Services.AddServices();

        var app = builder.Build();

        app.EnsureDbCreated();

        app.MapControllers();

        app.UseRequestLogging();
        app.UseApiKeysAuthorization();

        app.Run();
    }
}