using ModulbankInternship;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

new Registration(builder)
    .RegisterDB()
    .RegisterHangfire()
    .RegisterMediatR()
    .RegistryInjections()
    .RegisterValidators()
    .RegisterJWT()
    .RegisterSwagger()
    .RegisterLogger()
    .RegisterHealthCheck()
    .RegisterRabbit();

builder.WebHost.UseUrls("http://0.0.0.0:80");




var app = builder.Build();
app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();


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

app.UseDeveloperExceptionPage();

// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     db.Database.Migrate();
// }

app.MapControllers();

app.Run();
