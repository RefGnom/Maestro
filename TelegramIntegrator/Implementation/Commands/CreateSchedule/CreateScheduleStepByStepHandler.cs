using Maestro.TelegramIntegrator.Implementation.Commands.Handlers;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Maestro.TelegramIntegrator.Models;

namespace Maestro.TelegramIntegrator.Implementation.Commands.CreateSchedule;

public class CreateScheduleStepByStepHandler(IStateSwitcher stateSwitcher)
    : CommandHandlerBase<CreateScheduleStepByStepCommandModel>
{
    private readonly IStateSwitcher _stateSwitcher = stateSwitcher;
    public override string CommandName => TelegramCommandNames.CreateScheduleStepByStep;

    protected override Task ExecuteAsync(ChatContext context, CreateScheduleStepByStepCommandModel command)
    {
        return _stateSwitcher.SetStateAsync<EnterScheduleDescriptionState>(context.UserId);
    }
}