using Maestro.Server.Private.Authentication;
using Maestro.Server.Private.Services;

namespace Maestro.Server.Services;

public class RolesValidator : IRolesValidator
{
    public bool Validate(string role)
    {
        return role switch
        {
            ServiceRoles.Daemon or IntegratorsRoles.Integrator => true,
            _ => false
        };
    }
}