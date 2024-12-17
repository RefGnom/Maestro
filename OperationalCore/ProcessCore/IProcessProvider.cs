namespace Maestro.OperationalCore.ProcessCore;

public interface IProcessProvider
{
    IRegularProcess[] SelectAll();
    IRegularProcess[] SelectByMode(bool modeIsRan);
}