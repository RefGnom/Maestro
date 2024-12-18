namespace Maestro.Operational.ProcessesCore;

public class ProcessRunner(IProcessProvider processProvider) : IProcessRunner
{
    private readonly IRegularProcess[] _regularProcesses = processProvider.SelectAll();

    public async Task RunAsync()
    {
        var activeByDefaultRegularProcess = _regularProcesses.Where(regularProcess => regularProcess.IsActiveByDefault);
        foreach (var regularProcess in activeByDefaultRegularProcess)
        {
            await regularProcess.StartAsync();
        }
    }

    public async Task StartProcessAsync(string processName, bool isRepeat = false)
    {
        var regularProcess = _regularProcesses.Single(x => x.ProcessName == processName);
        await regularProcess.StartAsync(isRepeat);
    }

    public async Task StopProcessAsync(string processName)
    {
        var regularProcess = _regularProcesses.Single(x => x.ProcessName == processName);
        await regularProcess.StopAsync();
    }
}