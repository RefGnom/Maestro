﻿using Telegram.Bot;
using Telegram.Bot.Types;

namespace Client;

public interface IMaestroService
{
    public Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken);
    public Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToke);
}