namespace Maestro.Server.Extensions;

public static class HttpContextExtensions
{
    private const string IntegratorIdKey = "IntegratorId";

    public static long GetIntegratorId(this HttpContext httpContext)
    {
        return (long)httpContext.Items[IntegratorIdKey]!;
    }

    public static void SetIntegratorId(this HttpContext httpContext, long integratorId)
    {
        httpContext.Items.Add(IntegratorIdKey, integratorId);
    }
}