using AutoMapper;
using Maestro.Core.Models;
using Maestro.Data.Models;

namespace Maestro.Server.Startup;

public static class MapperConfigurator
{
    public static void AddMapper(this IServiceCollection services)
    {
        var configuration = new MapperConfiguration(configure => { configure.CreateMap<ReminderDbo, ReminderDto>(); });
        var mapper = configuration.CreateMapper();

        services.AddSingleton(mapper);
    }
}