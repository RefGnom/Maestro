using Maestro.TelegramIntegrator.View;

namespace Maestro.TelegramIntegrator.Implementation.Commands.Models;

public class CreateReminderHelpCommandModel : ICommandModel
{
    public string TelegramCommand => TelegramCommandNames.CreateReminderHelp;
    public string HelpDescription => TelegramMessageBuilder.BuildByCommandPattern(TelegramCommandPatterns.CreateReminderCommandPattern);
}
