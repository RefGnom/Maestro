namespace Maestro.Core.Factories;

public class GuidFactory : IGuidFactory
{
    public Guid Create()
    {
        return Guid.NewGuid();
    }
}