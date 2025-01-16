using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Maestro.TelegramIntegrator.Implementation.Extensions;

public static class UpdateExtensions
{
    public static long GetUserId(this Update update)
    {
        return update.Type switch
        {
            UpdateType.Message => update.Message!.From!.Id,
            UpdateType.CallbackQuery => update.CallbackQuery!.From.Id,
            _ => throw new NotSupportedException($"Для обновления типа {update.Type} не реализовано получение userId")
        };
    }
}