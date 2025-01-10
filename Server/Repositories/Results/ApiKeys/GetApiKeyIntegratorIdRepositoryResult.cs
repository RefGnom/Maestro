namespace Maestro.Server.Repositories.Results.ApiKeys;

public class GetApiKeyIntegratorIdRepositoryResult(bool isSuccessful, long? integratorId)
    : BaseRepositoryResultWithData<long?>(isSuccessful, integratorId)
{
    public bool? IsApiKeyFound { get; init; }
}