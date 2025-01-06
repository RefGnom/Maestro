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
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDbContext(builder.Configuration);
        builder.Services.AddRepositories();
        builder.Services.AddMapper();
        builder.Services.AddServices();
        builder.Services.AddAuthenticationWithSchemes();

        builder.ConfigureLogging();

        var app = builder.Build();

        app.EnsureDbCreated();

        app.MapControllers();

        app.UseRequestLogging();
        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();
    }
}