using Maestro.Core.Configuration;
using Maestro.Operational.ProcessesCore;

namespace Daemon;

public class DaemonApplication(IProcessRunner processRunner) : IApplication
{
    private readonly IProcessRunner _processRunner = processRunner;

    public void SetUp()
    {
    }

    public async Task RunAsync()
    {
        await _processRunner.RunAsync();
    }
}