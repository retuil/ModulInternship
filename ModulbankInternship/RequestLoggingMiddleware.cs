using System.Diagnostics;

namespace ModulbankInternship;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var correlationId = context.TraceIdentifier;

        try
        {
            logger.LogInformation(
                "HTTP {Method} {Path} started. CorrelationId={CorrelationId}, User={User}",
                context.Request.Method,
                context.Request.Path,
                correlationId,
                context.User?.Identity?.Name ?? "anonymous");

            await next(context);

            sw.Stop();
            logger.LogInformation(
                "HTTP {Method} {Path} finished {StatusCode} in {Elapsed}ms. CorrelationId={CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                correlationId);
        }
        catch (Exception ex)
        {
            sw.Stop();
            logger.LogError(
                ex,
                "HTTP {Method} {Path} failed after {Elapsed}ms. CorrelationId={CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                sw.ElapsedMilliseconds,
                correlationId);

            throw;
        }
    }
}
