using Maestro.Core.Configuration;
using Maestro.Core.Linq;
using Maestro.Operational.RegularProcesses.ProcessCore;

namespace Maestro.Operational;

public class OperationalApplication(IProcessRunner processRunner, IEnumerable<IRegularProcess> regularProcesses) : IApplication
{
    private readonly IProcessRunner _processRunner = processRunner;
    private readonly IRegularProcess[] _regularProcesses = regularProcesses.ToArray();

    public void SetUp()
    {
    }

    public async Task RunAsync()
    {
        _regularProcesses.ForEach(_processRunner.RegisterProcess);
        await _processRunner.RunAsync();
    }
}