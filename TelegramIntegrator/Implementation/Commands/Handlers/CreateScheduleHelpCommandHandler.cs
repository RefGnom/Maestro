using Maestro.TelegramIntegrator.Implementation.Commands.Models;
using Maestro.TelegramIntegrator.Models;
using Telegram.Bot;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Handlers;

public class CreateScheduleHelpCommandHandler(ITelegramBotClient telegramBotClient)
    : CommandHandlerBase<CreateScheduleHelpCommandModel>
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;

    public override string CommandName => TelegramCommandNames.CreateScheduleHelp;

    protected override Task ExecuteAsync(ChatContext context, CreateScheduleHelpCommandModel command)
    {
        return _telegramBotClient.SendMessage(context.ChatId, command.HelpDescription);
    }
}