using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.HealthChecks.Ready.Outbox;

public class OutboxHealthCheck(ApplicationDbContext db, ILogger<OutboxHealthCheck> logger) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var pending = await db.OutboxMessages
            .CountAsync(m => !m.IsDispatched, cancellationToken);

        if (pending > 100)
        {
            logger.LogWarning("");
            return HealthCheckResult.Degraded($"В очереди {pending} непубликованных сообщений");
        }

        return HealthCheckResult.Healthy($"В очереди {pending} сообщений");
    }
}
