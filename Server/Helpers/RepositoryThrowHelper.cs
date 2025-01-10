using Maestro.Server.Repositories.Results;

namespace Maestro.Server.Helpers;

public static class RepositoryThrowHelper
{
    public static void ThrowUnexpectedRepositoryResult()
    {
        throw GetUnexpectedRepositoryResultException();
    }

    public static InvalidOperationException GetUnexpectedRepositoryResultException()
    {
        return new InvalidOperationException(RepositoryResultsErrorMessages.UnexpectedRepositoryResult);
    }
}