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

            var response = new
            {
                message = "Validation failed",
                errors = ex.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ForbiddenException ex)
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = ex.Message,
                error = "Forbidden Exception"
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ResourceNotFoundException ex)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";

            var response = new
            {
                message = ex.Message,
                error = "Not Found Error"
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
