namespace Operational.RegularProcesses.ProcessCore;

public interface IRegularProcess
{
    string ProcessName { get; }
    bool IsActiveByDefault { get; }
    Task StartAsync(bool isRepeat = true);
    Task StopAsync();
}