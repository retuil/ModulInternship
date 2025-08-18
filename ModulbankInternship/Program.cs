using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ModulbankInternship;
using ModulbankInternship.Accounts.Services;
using ModulbankInternship.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

new Registration(builder)
    .RegisterDB()
    .RegisterHangfire()
    .RegisterMediatR()
    .RegistryInjections()
    .RegisterValidators()
    .RegisterJWT()
    .RegisterSwagger();




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
