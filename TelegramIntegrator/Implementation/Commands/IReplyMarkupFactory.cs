using Telegram.Bot.Types.ReplyMarkups;

namespace Maestro.TelegramIntegrator.Implementation.Commands;

public interface IReplyMarkupFactory
{
    IReplyMarkup CreateExitToMainMenuReplyMarkup();
    IReplyMarkup CreateOptionsReplyMarkup();
}