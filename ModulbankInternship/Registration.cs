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
using ModulbankInternship.Wallets;
using ModulbankInternship.Wallets.Interfaces;
using ModulbankInternship.Wallets.Validators;

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
            cfg.RegisterServicesFromAssemblyContaining<AddTransactionToWalletValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CloseWalletValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CreateWalletValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<MakeTransferValidator>();
        });
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<ModifyWalletParametersValidator>();
        });
    }
    public static void RegisterValidators(WebApplicationBuilder? builder)
    {
        
        builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<RegistrationValidator>();

        
        builder.Services.AddValidatorsFromAssemblyContaining<AddTransactionToRepositoryValidator>();
        
       
        builder.Services.AddValidatorsFromAssemblyContaining<MakeTransactionValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<CheckExecutorAccessValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<AddTransactionToWalletValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<CloseWalletValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<CreateWalletValidator>();
        
        
        builder.Services.AddValidatorsFromAssemblyContaining<MakeTransferValidator>();
        
       
        builder.Services.AddValidatorsFromAssemblyContaining<ModifyWalletParametersValidator>();
        
        
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    public static void RegistryInjections(WebApplicationBuilder? builder)
    {
        builder.Services.AddSingleton<ITransactionsRepository, TransactionsRepository>();
        builder.Services.AddSingleton<IUserRepository, UserRepository>();
        builder.Services.AddSingleton<IWalletsRepository, WalletsRepository>();
    }

    public static void RegisterSwagger(WebApplicationBuilder? builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }
}