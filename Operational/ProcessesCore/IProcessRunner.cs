namespace Maestro.Operational.ProcessesCore;

public interface IProcessRunner
{
    Task RunAsync();
    Task StartProcessAsync(string processName, bool isRepeatable = false);
    Task StopProcessAsync(string processName);
}