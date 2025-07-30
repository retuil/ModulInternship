using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using ModulbankInternship;
using ModulbankInternship.Auth.Validators;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

Registration.RegisterMediatR(builder);
Registration.RegistryInjections(builder);
Registration.RegisterValidators(builder);
Registration.RegisterSwagger(builder);


var app = builder.Build();
app.UseMiddleware<ValidationExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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