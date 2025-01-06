using Maestro.Server.Authentication;

namespace Maestro.Server.Services;

public class RolesValidator : IRolesValidator
{
    public bool Validate(string role)
    {
        return role switch
        {
            Roles.Daemon or Roles.Integrator => true,
            _ => false
        };
    }
}