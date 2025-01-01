using System.Timers;
using Maestro.Core.Logging;
using Timer = System.Timers.Timer;

namespace Maestro.Operational.ProcessesCore;

public abstract class RegularProcessBase<TProcess> : IRegularProcess
{
    private readonly ILog<TProcess> _log;
    private readonly Timer _timer;
    private readonly object _timerLockObject = new object();

    protected RegularProcessBase(ILog<TProcess> log)
    {
        _log = log;
        _timer = new Timer();
        _timer.Elapsed += TimerOnElapsed;
    }
    protected abstract TimeSpan Timeout { get; }

    public abstract string ProcessName { get; }
    public abstract bool IsActiveByDefault { get; }
    public bool IsRunning
    {
        get
        {
            lock (_timerLockObject)
            {
                return _timer.Enabled;
            }
        }
    }

    public Task StartAsync(bool isRepeatable = true)
    {
        lock (_timerLockObject)
        {
            _log.Info($"Starting regular process {ProcessName}, IsRepeat: {isRepeatable}");
            _timer.Interval = Timeout.TotalMilliseconds;
            _timer.AutoReset = isRepeatable;
            _timer.Start();
        }

        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        lock (_timerLockObject)
        {
            _log.Info($"Stopping regular process {ProcessName}");
            _timer.Stop();
        }

        return Task.CompletedTask;
    }

    protected abstract Task TryRunAsync();

    protected virtual Task HandleErrorAsync(Exception exception)
    {
        _log.Error($"Regular process {ProcessName} finished with error: {exception}");
        return Task.CompletedTask;
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        _log.Info($"Attempt to run regular process {ProcessName}");

        try
        {
            TryRunAsync().GetAwaiter().GetResult();
        }
        catch (Exception exception)
        {
            HandleErrorAsync(exception).GetAwaiter().GetResult();
        }
    }
}