namespace Maestro.Operational.ProcessesCore;

public interface IProcessRunner
{
    Task RunAsync();
    Task StartProcessAsync(string processName, bool isRepeat = false);
    Task StopProcessAsync(string processName);
}