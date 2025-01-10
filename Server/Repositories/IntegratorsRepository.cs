using Maestro.Data;
using Maestro.Data.Models;
using Maestro.Server.Repositories.Results.Integrators;

namespace Maestro.Server.Repositories;

public class IntegratorsRepository(DataContext dataContext) : IIntegratorsRepository
{
    private readonly DataContext _dataContext = dataContext;

    public async Task<AddIntegratorRepositoryResult> AddIntegratorAsync(CancellationToken cancellationToken)
    {
        var integratorDbo = (await _dataContext.Integrators.AddAsync(new IntegratorDbo(), cancellationToken)).Entity;
        await _dataContext.SaveChangesAsync(cancellationToken);
        return new AddIntegratorRepositoryResult(true, integratorDbo.Id);
    }
}