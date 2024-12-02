using Maestro.Core.Configuration;
using Operational.RegularProcesses.ProcessCore;

namespace Operational;

public class OperationalApplication(IProcessRunner processRunner, IRegularProcess regularProcess) : IApplication
{
    private readonly IProcessRunner _processRunner = processRunner;
    private readonly IRegularProcess _regularProcess = regularProcess;

    public void SetUp()
    {
    }

    public async Task RunAsync()
    {
        _processRunner.RegisterProcess(_regularProcess);
        await _processRunner.RunAsync();
    }
}