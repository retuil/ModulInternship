using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Validators;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Repositories;
using ModulbankInternship.Accounts.Services;
using ModulbankInternship.Accounts.Validators;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Interfaces;

namespace ModulbankInternship;

public class Registration(WebApplicationBuilder builder)
{
    public Registration RegisterMediatR()
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
        
        return this;
    }
    public Registration RegisterValidators()
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

        return this;
    }

    public Registration RegistryInjections()
    {
        builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
        builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
        builder.Services.AddScoped<InterestService>();
        return this;
    }

    public Registration RegisterSwagger()
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
                    new[] { "openid", "profile", "roles" }
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

        return this;
    }

    public Registration RegisterJWT()
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
                    NameClaimType = "preferred_username",
                    RoleClaimType = "roles"
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();

        return this;
    }

    public Registration RegisterDB()
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        return this;
    }

    public Registration RegisterHangfire()
    {
        builder.Services.AddHangfire(cfg =>
            cfg.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        builder.Services.AddHangfireServer();

        builder.Services.AddHostedService<RecurringJobsHostedService>();

        return this;
    }
}