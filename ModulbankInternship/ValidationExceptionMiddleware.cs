using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Exceptions;

namespace ModulbankInternship;

public class ValidationExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (FluentValidation.ValidationException ex)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";

            var response = MbResult.Failure<bool>("400", "Validation failed");

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ForbiddenException ex)
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";

            var response = MbResult.Failure<bool>("403", ex.Message);

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ResourceNotFoundException ex)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";

            var response = MbResult.Failure<bool>("404", ex.Message);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
