using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace ModulbankInternship.HealthChecks.Ready.RabbitMQ;

public class RabbitMqHealthCheck(IConnectionFactory connectionFactory) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        { 
            using var connection = connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();
            return Task.FromResult(HealthCheckResult.Healthy("RabbitMQ доступен"));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("RabbitMQ недоступен", ex));
        }
    }
}