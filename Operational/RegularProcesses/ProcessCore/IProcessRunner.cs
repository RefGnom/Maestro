namespace Maestro.Operational.RegularProcesses.ProcessCore;

public interface IProcessRunner
{
    void RegisterProcess(IRegularProcess regularProcess);
    Task RunAsync();
    Task StartProcessAsync(string processName, bool isRepeat = false);
    Task StopProcessAsync(string processName);
}