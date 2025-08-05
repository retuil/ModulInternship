using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Validators;
using ModulbankInternship.Users;
using ModulbankInternship.Users.Interfaces;
using ModulbankInternship.Accounts;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Validators;
using ModulbankInternship.Transactions.Interfaces;

namespace ModulbankInternship;

public static class Registration
{
    public static void RegisterMediatR(WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<ModifyAccountValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<NewAccountForCurrentUserValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<NewAccountForAnyUserValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<TransferValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<NewTransactionValidator>();
        });
    }
    public static void RegisterValidators(WebApplicationBuilder builder)
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        
        builder.Services.AddValidatorsFromAssemblyContaining<ModifyAccountValidator>();
        
       
        builder.Services.AddValidatorsFromAssemblyContaining<NewAccountForCurrentUserValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<NewAccountForAnyUserValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<TransferValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<NewTransactionValidator>();
        
        
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    public static void RegistryInjections(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ITransactionsRepository, TransactionsRepository>();
        builder.Services.AddSingleton<IAccountsRepository, AccountsRepository>();
    }

    public static void RegisterSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Modulbank API", Version = "v1" });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            c.IncludeXmlComments(xmlPath);

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("http://localhost:8080/realms/modulbank/protocol/openid-connect/auth?prompt=login"),
                        TokenUrl = new Uri("http://localhost:8080/realms/modulbank/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID" },
                            { "profile", "User profile" },
                            { "roles", "Access roles" }
                        }
                    }
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new[] { "openid", "profile", "email" }
                }
            });
        });

        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });
    }

    public static void RegisterJWT(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "http://localhost:8080/realms/modulbank";
                options.Audience = "modulbank-api";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidAudience = "modulbank-api",
                    NameClaimType = "preferred_username",
                    RoleClaimType = "roles"
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
    }
}