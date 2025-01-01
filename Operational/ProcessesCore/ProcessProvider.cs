namespace Maestro.Operational.ProcessesCore;

public class ProcessProvider(IEnumerable<IRegularProcess> regularProcesses) : IProcessProvider
{
    private readonly IRegularProcess[] _regularProcesses = regularProcesses.ToArray();

    public IRegularProcess[] SelectAll()
    {
        return _regularProcesses;
    }

    public IRegularProcess[] SelectByMode(bool modeIsRunning)
    {
        return _regularProcesses.Where(x => x.IsRunning && modeIsRunning).ToArray();
    }
}