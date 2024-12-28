namespace Maestro.Operational.ProcessesCore;

public interface IRegularProcess
{
    string ProcessName { get; }
    bool IsActiveByDefault { get; }
    bool IsRunning { get; }
    Task StartAsync(bool isRepeatable = true);
    Task StopAsync();
}