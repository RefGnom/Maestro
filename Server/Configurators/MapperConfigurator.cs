using AutoMapper;
using Maestro.Data.Models;
using Maestro.Server.Public.Models.Reminders;

namespace Maestro.Server.Configurators;

public static class MapperConfigurator
{
    public static void AddMapper(this IServiceCollection services)
    {
        var configuration = new MapperConfiguration(configure =>
        {
            configure.CreateMap<ReminderDto, ReminderDbo>().ReverseMap();
            configure.CreateMap<ReminderDbo, ReminderWithIdDto>()
                .ForMember(reminderWithIdDto => reminderWithIdDto.ReminderId,
                    options => options.MapFrom(reminderDbo => reminderDbo.Id));
            ;
        });
        var mapper = configuration.CreateMapper();

        services.AddSingleton(mapper);
    }
}