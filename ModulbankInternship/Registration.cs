using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using ModulbankInternship.Auth.Validators;
using ModulbankInternship.Transactions;
using ModulbankInternship.Transactions.Validators;
using ModulbankInternship.Users;
using ModulbankInternship.Users.Interfaces;
using ModulbankInternship.Users.Validators;
using ModulbankInternship.Accounts;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Validators;

namespace ModulbankInternship;

public static class Registration
{
    public static void RegisterMediatR(WebApplicationBuilder? builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<LoginValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<RegistrationValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AddTransactionToRepositoryValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<MakeTransactionValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CheckExecutorAccessValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AddTransactionToAccountValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CloseAccountValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CreateAccountValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<MakeTransferValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<ModifyAccountParametersValidator>();
        });
    }
    public static void RegisterValidators(WebApplicationBuilder? builder)
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
     
        
        builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<RegistrationValidator>();

        
        builder.Services.AddValidatorsFromAssemblyContaining<AddTransactionToRepositoryValidator>();
        
       
        builder.Services.AddValidatorsFromAssemblyContaining<MakeTransactionValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<CheckExecutorAccessValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<AddTransactionToAccountValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<CloseAccountValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<CreateAccountValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<MakeTransferValidator>();
        
       
        builder.Services.AddValidatorsFromAssemblyContaining<ModifyAccountParametersValidator>();
        
        
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    public static void RegistryInjections(WebApplicationBuilder? builder)
    {
        builder.Services.AddSingleton<ITransactionsRepository, TransactionsRepository>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IAccountsRepository, AccountsRepository>();
    }

    public static void RegisterSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            c.IncludeXmlComments(xmlPath);
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


}