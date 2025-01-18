using Maestro.TelegramIntegrator.Implementation.Commands.Handlers;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.SetTimeZone;

public class SetTimeZoneCommandHandler(IStateSwitcher stateSwitcher)
    : CommandHandlerBase<SetTimeZoneCommandModel>
{
    private readonly IStateSwitcher _stateSwitcher = stateSwitcher;
    public override string CommandName => TelegramCommandNames.SetTimeZone;

    protected override Task ExecuteAsync(ChatContext context, SetTimeZoneCommandModel command)
    {
        return _stateSwitcher.SetStateAsync<SetTimeZoneState>(context.UserId);
    }
}