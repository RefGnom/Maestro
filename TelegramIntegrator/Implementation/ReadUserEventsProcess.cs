using Maestro.Client;
using Maestro.Core.Logging;
using Maestro.Operational.ProcessesCore;

namespace Maestro.TelegramIntegrator.Implementation;

public class ReadUserEventsProcess(ILog<ReadUserEventsProcess> log, IEventsApiClient eventsApiClient) : RegularProcessBase<ReadUserEventsProcess>(log)
{
    private readonly IEventsApiClient _eventsApiClient = eventsApiClient;
    public override string ProcessName => "Чтение пользовательских событий";
    public override bool IsActiveByDefault => true;
    protected override TimeSpan Timeout => TimeSpan.FromSeconds(5);

    protected override Task TryRunAsync()
    {
        // Тут читаем события по всем пользователям и кидаем их, если нужно
        return Task.CompletedTask;
    }
}