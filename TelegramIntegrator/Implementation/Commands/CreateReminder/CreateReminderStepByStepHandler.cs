using Maestro.TelegramIntegrator.Implementation.Commands.Handlers;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateReminder;

public class CreateReminderStepByStepHandler(IStateSwitcher stateSwitcher)
    : CommandHandlerBase<CreateReminderStepByStepCommandModel>
{
    private readonly IStateSwitcher _stateSwitcher = stateSwitcher;

    public override string CommandName => TelegramCommandNames.CreateReminderStepByStep;

    protected async override Task ExecuteAsync(ChatContext context, CreateReminderStepByStepCommandModel command)
    {
        await _stateSwitcher.SetStateAsync<EnterReminderDescriptionState>(context.UserId);
    }
}