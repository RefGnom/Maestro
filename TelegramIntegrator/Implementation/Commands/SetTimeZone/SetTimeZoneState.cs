using Maestro.Core.Logging;
using Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;
using Maestro.TelegramIntegrator.Implementation.ParsHelpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Maestro.TelegramIntegrator.Implementation.Commands.SetTimeZone;

public class SetTimeZoneState(
    ILog<SetTimeZoneState> log,
    IStateSwitcher stateSwitcher,
    IReplyMarkupFactory replyMarkupFactory,
    ITelegramBotClient telegramBotClient,
    IUserDateTimeService userDateTimeService
) : BaseState<SetTimeZoneState>(log, stateSwitcher, replyMarkupFactory)
{
    private readonly ITelegramBotClient _telegramBotClient = telegramBotClient;
    private readonly IUserDateTimeService _userDateTimeService = userDateTimeService;

    public override Task InitializeAsync(long userId)
    {
        return _telegramBotClient.SendMessage(
            userId,
            "Введите дельту от международного UTC времени. Москва: +3",
            replyMarkup: new InlineKeyboardMarkup()
                .AddButton("+3 Мск", "+3")
                .AddButton("+5 Екб", "+5")
        );
    }

    protected override Task ReceiveEditedMessageAsync(Message message) => ReceiveMessageAsync(message);

    protected override Task ReceiveMessageAsync(Message message)
    {
        var userId = message.From!.Id;
        var text = message.Text!;

        return SetTimeZoneAsync(userId, text);
    }

    protected override Task ReceiveCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        var userId = callbackQuery.From!.Id;
        var text = callbackQuery.Data!;

        return SetTimeZoneAsync(userId, text);
    }

    private async Task SetTimeZoneAsync(long userId, string deltaText)
    {
        var deltaParseResult = ParserHelper.ParseInt(deltaText);
        if (!deltaParseResult.IsSuccessful)
        {
            await _telegramBotClient.SendMessage(userId, deltaParseResult.ParseFailureMessage);
            return;
        }

        var utcOffset = TimeSpan.FromHours(deltaParseResult.Value);
        var timeZoneInfo = TimeZoneInfo.GetSystemTimeZones()
            .FirstOrDefault(x => x.BaseUtcOffset == utcOffset);

        if (timeZoneInfo is null)
        {
            await _telegramBotClient.SendMessage(userId, $"Не нашли временную зону для дельты {deltaParseResult.Value}");
        }
        else
        {
            _userDateTimeService.SetUserTimeZone(userId, timeZoneInfo);
            await _telegramBotClient.SendMessage(userId, $"Установлена временная зона {timeZoneInfo}");
        }

        await StateSwitcher.SetStateAsync<MainState>(userId);
    }
}