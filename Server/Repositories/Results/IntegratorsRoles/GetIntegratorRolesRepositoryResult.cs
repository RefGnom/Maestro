namespace Maestro.Server.Repositories.Results.IntegratorsRoles;

public class GetIntegratorRolesRepositoryResult(bool isSuccessful, List<string> integratorRoles)
    : BaseRepositoryResultWithData<List<string>>(isSuccessful, integratorRoles)
{
}