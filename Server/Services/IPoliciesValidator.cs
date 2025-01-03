namespace Maestro.Server.Services;

public interface IPoliciesValidator
{
    bool Validate(string policy);
}