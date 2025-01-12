namespace Maestro.Server.Repositories.Results.Integrators;

public class AddIntegratorRepositoryResult(bool isSuccessful, long integratorId) : BaseRepositoryResultWithData<long>(isSuccessful, integratorId)
{
}