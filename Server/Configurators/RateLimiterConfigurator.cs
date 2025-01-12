using System.Threading.RateLimiting;
using Maestro.Server.Extensions;
using Maestro.Server.Private.Authentication;

namespace Maestro.Server.Configurators;

public static class RateLimiterConfigurator
{
    public static void AddRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, long>(context =>
            {
                var role = context.GetUserRole();

                switch (role)
                {
                    case ServiceRoles.Admin:
                        return RateLimitPartition.GetNoLimiter(-1L);
                    case ServiceRoles.Daemon:
                        return RateLimitPartition.GetConcurrencyLimiter(-2L, _ => new ConcurrencyLimiterOptions
                        {
                            PermitLimit = 50,
                            QueueLimit = 100,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                    default:
                    {
                        var integratorId = context.GetIntegratorId();

                        return RateLimitPartition.GetFixedWindowLimiter(integratorId, _ =>
                        {
                            return role switch
                            {
                                IntegratorsRoles.Base => new FixedWindowRateLimiterOptions
                                {
                                    PermitLimit = 100,
                                    Window = TimeSpan.FromMinutes(1),
                                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                                    QueueLimit = 25
                                },
                                _ => throw new ArgumentOutOfRangeException($"{nameof(RateLimitPartition)} is not configured for Role \"{role}\"")
                            };
                        });
                    }
                }
            });

            options.OnRejected = (context, _) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return ValueTask.CompletedTask;
            };
        });
    }
}