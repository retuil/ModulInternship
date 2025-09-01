using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.HealthChecks.Ready;

[ApiController]
[Route("health")]
public class HealthCheckReadyEndpoint(HealthCheckService healthChecks): ControllerBase
{
    /// <summary>
    /// Проверка работы
    /// </summary>
    /// <response code="200">Подключение к сервису имеется</response>
    /// <response code="429">Много сообщений в очереди outbox</response>

    [HttpGet]
    [Route("ready")]
    public async Task<IActionResult> Ready()
    {
        var report = await healthChecks.CheckHealthAsync();

        var statusCode = report.Status switch
        {
            HealthStatus.Healthy => StatusCodes.Status200OK,
            HealthStatus.Degraded => StatusCodes.Status429TooManyRequests,
            _ => StatusCodes.Status503ServiceUnavailable
        };

        return StatusCode(statusCode, new
        {
            status = report.Status.ToString(),
            results = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
    }
}