using Maestro.Server.Authorization;

namespace Maestro.Server.Services;

public class PoliciesValidator : IPoliciesValidator
{
    public bool Validate(string policy)
    {
        return policy switch
        {
            AuthorizationPolicies.Daemon or AuthorizationPolicies.Integrator => true,
            _ => false
        };
    }
}