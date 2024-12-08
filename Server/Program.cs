using Maestro.Server.Startup;

namespace Maestro.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext(builder.Configuration);
        builder.Services.AddRepositories();

        var app = builder.Build();

        app.EnsureDbCreated();

        app.MapControllers();

        app.Run();
    }
}