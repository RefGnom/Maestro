﻿using Telegram.Bot;
using Telegram.Bot.Types;

namespace Maestro.TelegramIntegrator.Implementation;

public interface IMaestroCommandHandler
{
    public Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken);
    public Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToke);
}