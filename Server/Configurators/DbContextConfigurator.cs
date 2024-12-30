using Maestro.Data;
using Microsoft.EntityFrameworkCore;

namespace Maestro.Server.Configurators;

public static class DbContextConfigurator
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSection = configuration.GetSection("Database");

        var port = dbSection.GetValue<string>("Port");
        var dataBase = dbSection.GetValue<string>("Database");
        var username = dbSection.GetValue<string>("Username");
        var host = dbSection.GetValue<string>("Host");
        var password = dbSection.GetValue<string>("Password");

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", isEnabled: true);

        services.AddDbContext<DataContext>(
            options => { options.UseNpgsql($"Port={port}; Database={dataBase}; Username={username}; Host={host}; Password={password};"); },
            ServiceLifetime.Transient);
    }

    public static void EnsureDbCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<DataContext>().Database.EnsureCreated();
    }
}