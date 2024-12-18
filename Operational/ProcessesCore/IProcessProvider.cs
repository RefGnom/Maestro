namespace Maestro.Operational.ProcessesCore;

public interface IProcessProvider
{
    IRegularProcess[] SelectAll();
    IRegularProcess[] SelectByMode(bool modeIsRunning);
}