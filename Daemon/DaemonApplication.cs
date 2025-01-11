using Maestro.Core.Configuration;
using Maestro.Operational.ProcessesCore;

namespace Maestro.Daemon;

public class DaemonApplication(IProcessRunner processRunner) : IApplication
{
    private readonly IProcessRunner _processRunner = processRunner;

    public void SetUp()
    {
    }

    public Task RunAsync() => _processRunner.RunAsync();
}