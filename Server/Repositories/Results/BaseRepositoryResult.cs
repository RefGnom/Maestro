namespace Maestro.Server.Repositories.Results;

public abstract class BaseRepositoryResult(bool isSuccessful)
{
    public bool IsSuccessful { get; } = isSuccessful;

    public virtual void Throw()
    {
        throw new InvalidOperationException();
    }
}