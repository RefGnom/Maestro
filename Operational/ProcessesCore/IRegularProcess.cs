namespace Maestro.Operational.ProcessesCore;

public interface IRegularProcess
{
    string ProcessName { get; }
    bool IsActiveByDefault { get; }
    bool IsRan { get; }
    Task StartAsync(bool isRepeat = true);
    Task StopAsync();
}