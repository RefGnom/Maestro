namespace Maestro.Server.Repositories.Results.IntegratorsRoles;

public class GetIntegratorRolesRepositoryResult(bool isSuccessful, string? integratorRole)
    : BaseRepositoryResultWithData<string?>(isSuccessful, integratorRole)
{
    public bool? IsIntegratorRoleFound { get; init; }
}