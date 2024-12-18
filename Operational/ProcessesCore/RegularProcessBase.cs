using Maestro.Core.Logging;

namespace Maestro.Operational.ProcessesCore;

public abstract class RegularProcessBase<TProcess>(ILog<TProcess> log) : IRegularProcess
{
    private bool _isRunning;
    private readonly ILog<TProcess> _log = log;
    private readonly object _modeLockObject = new();

    public abstract string ProcessName { get; }
    public abstract bool IsActiveByDefault { get; }
    public bool IsRunning
    {
        get
        {
            lock (_modeLockObject)
            {
                return _isRunning;
            }
        }
        private set
        {
            lock (_modeLockObject)
            {
                _isRunning = value;
            }
        }
    }
    protected abstract TimeSpan Timeout { get; }

    protected abstract Task TryRunAsync();

    protected virtual Task HandleErrorAsync(Exception exception)
    {
        _log.Error($"Regular process {ProcessName} finished with error: {exception}");
        return Task.CompletedTask;
    }

    public async Task StartAsync(bool isRepeatable = true)
    {
        IsRunning = true;
        _log.Info($"Starting regular process {ProcessName}, IsRepeat: {isRepeatable}");

        do
        {
            await SafeRunAsync();
            await Task.Delay(Timeout);
        } while (isRepeatable && IsRunning);
    }

    private async Task SafeRunAsync()
    {
        _log.Info($"Attempt to run regular process {ProcessName}");

        try
        {
            await TryRunAsync();
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(exception);
        }
    }

    public Task StopAsync()
    {
        IsRunning = false;
        _log.Info($"Stopping regular process {ProcessName}");

        return Task.CompletedTask;
    }
}