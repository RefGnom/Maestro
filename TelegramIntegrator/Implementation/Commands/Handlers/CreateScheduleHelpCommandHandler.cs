using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Handlers;

public class CreateScheduleHelpCommandHandler(ITelegramBotClient telegramBotClient) : CommandHandlerBase<CreateScheduleCommandModel>
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public override string Name => TelegramCommandNames.CreateScheduleHelp;

    protected override Task ExecuteAsync(ChatContext context, CreateScheduleCommandModel command)
    {
        return _telegramBotClient.SendMessage(context.ChatId, command.HelpDescription);
    }
}