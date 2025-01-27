﻿namespace Maestro.Operational.ProcessesCore;

public class ProcessRunner(IProcessProvider processProvider) : IProcessRunner
{
    private readonly IRegularProcess[] _regularProcesses = processProvider.SelectAll();

    public async Task RunAsync()
    {
        var activeByDefaultRegularProcess = _regularProcesses.Where(regularProcess => regularProcess.IsActiveByDefault);
        foreach (var regularProcess in activeByDefaultRegularProcess)
        {
            await Task.Factory.StartNew(() => regularProcess.StartAsync());
        }
    }

    public async Task StartProcessAsync(string processName, bool isRepeatable = false)
    {
        var regularProcess = _regularProcesses.Single(x => x.ProcessName == processName);
        await regularProcess.StartAsync(isRepeatable);
    }

    public async Task StopProcessAsync(string processName)
    {
        var regularProcess = _regularProcesses.Single(x => x.ProcessName == processName);
        await regularProcess.StopAsync();
    }
}