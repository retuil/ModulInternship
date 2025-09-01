using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Infrastructure;

namespace ModulbankInternship.HealthChecks.Live;

[ApiController]
[Route("health")]
public class HealthCheckLiveEndpoint: ControllerBase
{
    /// <summary>
    /// Проверка подключения к сервису
    /// </summary>
    /// <returns>Return 200. Подключение к сервису имеется</returns>
    /// <response code="200">Подключение к сервису имеется</response>

    [HttpGet]
    [Route("live")]
    public MbResult<string> IsConnectionLive()
    {
        return MbResult.Success("Alive");
    }
}