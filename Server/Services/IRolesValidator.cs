namespace Maestro.Server.Services;

public interface IRolesValidator
{
    bool Validate(string role);
}