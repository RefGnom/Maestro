using System.Timers;
using Core.Logging;
using Timer = System.Timers.Timer;

namespace Operational.RegularProcesses.ProcessCore;

public abstract class RegularProcessBase : IRegularProcess
{
    private readonly Timer _timer;
    private readonly ILog _log;

    protected RegularProcessBase(ILog log)
    {
        _log = log;
        _timer = new Timer();
        _timer.Elapsed += TimerOnElapsed;
    }


    public abstract string ProcessName { get; }
    public abstract bool IsActiveByDefault { get; }
    protected abstract TimeSpan Timeout { get; }

    public abstract Task RunAsync();

    public Task StartAsync(bool isRepeat = true)
    {
        _log.Info($"Starting regular process, IsRepeat: {isRepeat}");
        _timer.Interval = Timeout.TotalMilliseconds;
        _timer.AutoReset = isRepeat;
        _timer.Start();

        return Task.CompletedTask;
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        _log.Info("Running regular process");
        RunAsync().GetAwaiter().GetResult();
    }

    public Task StopAsync()
    {
        _log.Info("Stoping regular process");
        _timer.Stop();

        return Task.CompletedTask;
    }
}