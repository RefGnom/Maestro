using AutoMapper;
using Maestro.Data.Models;
using Maestro.Server.Public.Models;

namespace Maestro.Server.Configurators;

public static class MapperConfigurator
{
    public static void AddMapper(this IServiceCollection services)
    {
        var configuration = new MapperConfiguration(configure =>
        {
            configure.CreateMap<ReminderDto, ReminderDbo>().ReverseMap();
            configure.CreateMap<ReminderDbo, ReminderWithIdDto>();
        });
        var mapper = configuration.CreateMapper();

        services.AddSingleton(mapper);
    }
}