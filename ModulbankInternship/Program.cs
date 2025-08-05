using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using ModulbankInternship;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

Registration.RegisterMediatR(builder);
Registration.RegistryInjections(builder);
Registration.RegisterValidators(builder);
Registration.RegisterJWT(builder);
Registration.RegisterSwagger(builder);


var app = builder.Build();
app.UseMiddleware<ValidationExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Modulbank API v1");

        c.OAuthClientId("modulbank-api");
        c.OAuthUsePkce();
        c.OAuthScopeSeparator(" ");
    });
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }

        await next();
    });
}

app.MapControllers();

app.Run();