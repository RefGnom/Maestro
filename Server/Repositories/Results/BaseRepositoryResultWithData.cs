namespace Maestro.Server.Repositories.Results;

public abstract class BaseRepositoryResultWithData<TValue>(bool isSuccessful, TValue data) : BaseRepositoryResult(isSuccessful)
{
    public TValue? Data { get; } = data;
}