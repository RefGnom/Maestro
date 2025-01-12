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
    protected abstract TimeSpan Interval { get; }
    protected virtual TimeSpan CancellationTimeout => TimeSpan.FromMinutes(5);
    protected abstract Task UnsafeRunAsync();

    protected virtual Task HandleErrorAsync(Exception exception)
    {
        _log.Error($"Regular process {ProcessName} finished with error: {exception}");
        return Task.CompletedTask;
    }

    public async Task StartAsync(bool isRepeatable = true)
    {
        if (IsRunning)
        {
            throw new InvalidOperationException($"Нельзя запустить уже запущенный процесс {ProcessName}");
        }

        IsRunning = true;
        _log.Info($"Starting regular process {ProcessName}, IsRepeat: {isRepeatable}");

        do
        {
            await SafeRunAsync().ConfigureAwait(false);
            await Task.Delay(Interval).ConfigureAwait(false);
        } while (isRepeatable && IsRunning);
    }

    private async Task SafeRunAsync()
    {
        _log.Info($"Attempt to run regular process {ProcessName}");
        var cancellationTokenSource = new CancellationTokenSource(CancellationTimeout);

        try
        {
            await UnsafeRunAsync().WaitAsync(cancellationTokenSource.Token).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(exception).ConfigureAwait(false);
        }
        finally
        {
            cancellationTokenSource.Dispose();
        }
    }

    public Task StopAsync()
    {
        if (!IsRunning)
        {
            throw new InvalidOperationException($"Нельзя остановить не запущеннный процесс {ProcessName}");
        }

        IsRunning = false;
        _log.Info($"Stopping regular process {ProcessName}");

        return Task.CompletedTask;
    }
}