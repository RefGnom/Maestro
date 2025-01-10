using Maestro.TelegramIntegrator.Implementation.Commands;

namespace Maestro.TelegramIntegrator.Implementation.View;

public static class TelegramMessageBuilder
{
    public static string BuildByCommandPattern(string telegramCommandPattern) => $"Пожалуйста, введите сообщение по шаблону:\n\n\"{telegramCommandPattern}\"";

    public static string BuildUnknownCommand()
    {
        return $"Не смогли распознать введёную вами команду. Список возможных команд:" +
               $"\n{string.Join('\n', TelegramCommandNames.GetCommandNames())}";
    }
}