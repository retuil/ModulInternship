using Microsoft.EntityFrameworkCore;
using ModulbankInternship.Accounts.Domain.Exceptions;
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
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = MbResult.Failure<bool>("400", "Validation failed");

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ForbiddenException ex)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var response = MbResult.Failure<bool>("403", ex.Message);

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ResourceNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            var response = MbResult.Failure<bool>("404", ex.Message);

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            context.Response.ContentType = "application/json";

            var response = MbResult.Failure<bool>("409", ex.Message);

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (AccountBlockedException ex)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            context.Response.ContentType = "application/json";

            var response = MbResult.Failure<bool>("409", ex.Message);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
