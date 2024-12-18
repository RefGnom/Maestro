namespace Maestro.Operational.ProcessesCore;

public class ProcessProvider(IEnumerable<IRegularProcess> regularProcesses) : IProcessProvider
{
    private readonly IRegularProcess[] _regularProcesses = regularProcesses.ToArray();

    public IRegularProcess[] SelectAll() => _regularProcesses;

    public IRegularProcess[] SelectByMode(bool modeIsRunning) => _regularProcesses.Where(x => x.IsRunning && modeIsRunning).ToArray();
}