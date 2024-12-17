using System.Timers;
using Maestro.Core.Logging;
using Timer = System.Timers.Timer;

namespace Maestro.OperationalCore.ProcessCore;

public abstract class RegularProcessBase : IRegularProcess
{
    private readonly Timer _timer;
    private readonly ILog _log;
    private readonly object _timerLockObject = new object();

    public abstract string ProcessName { get; }
    public abstract bool IsActiveByDefault { get; }
    public bool IsRan
    {
        get
        {
            lock (_timerLockObject)
            {
                return _timer.Enabled;
            }
        }
    }
    protected abstract TimeSpan Timeout { get; }

    protected RegularProcessBase(ILog log)
    {
        _log = log;
        _timer = new Timer();
        _timer.Elapsed += TimerOnElapsed;
    }

    protected abstract Task RunAsync();

    public Task StartAsync(bool isRepeat = true)
    {
        lock (_timerLockObject)
        {
            _log.Info($"Starting regular process, IsRepeat: {isRepeat}");
            _timer.Interval = Timeout.TotalMilliseconds;
            _timer.AutoReset = isRepeat;
            _timer.Start();
        }

        return Task.CompletedTask;
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        _log.Info("Running regular process");
        RunAsync().GetAwaiter().GetResult();
    }

    public Task StopAsync()
    {
        lock (_timerLockObject)
        {
            _log.Info("Stopping regular process");
            _timer.Stop();
        }

        return Task.CompletedTask;
    }
}