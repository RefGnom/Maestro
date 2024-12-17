namespace Maestro.OperationalCore.ProcessCore;

public class ProcessProvider(IEnumerable<IRegularProcess> regularProcesses) : IProcessProvider
{
    private readonly IRegularProcess[] _regularProcesses = regularProcesses.ToArray();

    public IRegularProcess[] SelectAll() => _regularProcesses;

    public IRegularProcess[] SelectByMode(bool modeIsRan) => _regularProcesses.Where(x => x.IsRan && modeIsRan).ToArray();
}