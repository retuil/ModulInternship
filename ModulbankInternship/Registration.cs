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
using ModulbankInternship.Accounts;
using ModulbankInternship.Accounts.Transactions;
using ModulbankInternship.Accounts.Transfer;
using ModulbankInternship.HealthChecks.Ready;
using ModulbankInternship.HealthChecks.Ready.Outbox;
using ModulbankInternship.HealthChecks.Ready.RabbitMQ;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Rabbit;
using ModulbankInternship.Infrastructure.Rabbit.Inbox.Antifraud;
using ModulbankInternship.Infrastructure.Rabbit.Inbox.Audit;
using ModulbankInternship.Infrastructure.Rabbit.Outbox;
using ModulbankInternship.Infrastructure.Rabbit.Outbox.publisher;
using RabbitMQ.Client;
using Reo.Core.RabbitMQ.Fakes;
using Serilog;
using IModel = RabbitMQ.Client.IModel;


namespace ModulbankInternship;

public class Registration(WebApplicationBuilder builder)
{
    public Registration RegisterMediatR()
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        
        AccountRegistration.RegisterMediator(builder);
        TransactionRegistration.RegisterMediator(builder);
        TransferRegistration.RegisterMediator(builder);
        
        return this;
    }
    public Registration RegisterValidators()
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        
        AccountRegistration.RegisterValidators(builder);
        TransactionRegistration.RegisterValidators(builder);
        TransferRegistration.RegisterValidators(builder);
        
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return this;
    }

    public Registration RegistryInjections()
    {
        AccountRegistration.RegisterInjections(builder);
        TransactionRegistration.RegisterInjections(builder);
        TransferRegistration.RegisterInjections(builder);
        
        builder.Services.AddScoped<IMessagePublisher, RabbitMessagePublisher>();
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
                options.Authority = "http://keycloak:8080/realms/modulbank";
                options.Audience = "modulbank-api";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = "http://localhost:8080/realms/modulbank",
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

    public Registration RegisterLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.File("logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();


        builder.Host.UseSerilog();
        return this;
    }

    public Registration RegisterHealthCheck()
    {
        builder.Services.AddHealthChecks()
            .AddCheck<RabbitMqHealthCheck>("rabbitmq")
            .AddCheck<OutboxHealthCheck>("outbox");
        return this;
    }

    public Registration RegisterRabbit()
    {
        if (!builder.Configuration.GetValue<bool>("RabbitMQ:Enabled"))
        {
            return this;
        }
        
        builder.Services.AddSingleton<RabbitMQ.Client.IConnectionFactory>(_ =>
            new ConnectionFactory
            {
                Uri = new Uri(builder.Configuration["RabbitMq:ConnectionString"]!),
                DispatchConsumersAsync = true
            });


        builder.Services.AddSingleton<IConnection>(sp =>
        {
            var factory = sp.GetRequiredService<RabbitMQ.Client.IConnectionFactory>();
            return factory.CreateConnection();
        });

        builder.Services.AddSingleton<IModel>(sp =>
        {
            var conn = sp.GetRequiredService<IConnection>();
            var channel = conn.CreateModel();

            DeclareRabbitTopology(channel);

            return channel;
        });

        builder.Services.AddHostedService<OutboxDispatcher>();
        builder.Services.AddHostedService<AntifraudConsumer>();
        builder.Services.AddHostedService<AuditConsumer>();

        return this;
    }
    
    private static void DeclareRabbitTopology(IModel ch)
    {
        ch.ExchangeDeclare("account.events", ExchangeType.Topic, durable: true);

        ch.QueueDeclare("account.crm", durable: true, exclusive: false, autoDelete: false);
        ch.QueueDeclare("account.notifications", durable: true, exclusive: false, autoDelete: false);
        ch.QueueDeclare("account.antifraud", durable: true, exclusive: false, autoDelete: false);
        ch.QueueDeclare("account.audit", durable: true, exclusive: false, autoDelete: false);

        ch.QueueBind("account.crm", "account.events", "account.*");
        ch.QueueBind("account.notifications", "account.events", "money.*");
        ch.QueueBind("account.antifraud", "account.events", "antifraud.client.#");
        ch.QueueBind("account.audit", "account.events", "#");
    }
}