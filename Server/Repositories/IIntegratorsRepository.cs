using Maestro.Server.Repositories.Results.Integrators;

namespace Maestro.Server.Repositories;

public interface IIntegratorsRepository
{
    Task<AddIntegratorRepositoryResult> AddIntegratorAsync(CancellationToken cancellationToken);
}