namespace Maestro.Server.Repositories.Results.ApiKeys;

public class AddApiKeyRepositoryResult(bool isSuccessful, long apiKeyId) : BaseRepositoryResultWithData<long>(isSuccessful, apiKeyId)
{
}