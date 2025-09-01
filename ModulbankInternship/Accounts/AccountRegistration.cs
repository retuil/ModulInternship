using FluentValidation;
using ModulbankInternship.Accounts.AntifraudBlocks;
using ModulbankInternship.Accounts.Close;
using ModulbankInternship.Accounts.Create;
using ModulbankInternship.Accounts.Create.ForAnyUser;
using ModulbankInternship.Accounts.Create.ForCurrentUser;
using ModulbankInternship.Accounts.Get;
using ModulbankInternship.Accounts.Get.ById;
using ModulbankInternship.Accounts.Get.ByOwnerId;
using ModulbankInternship.Accounts.Modify;
using ModulbankInternship.Accounts.Transfer;
using ModulbankInternship.Transactions.Validators;

namespace ModulbankInternship.Accounts;

public static class AccountRegistration
{
    public static void RegisterInjections(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICreateAccountRepository, CreateAccountRepository>();
        builder.Services.AddScoped<IGetAccountByIdRepository, GetAccountByIdRepository>();
        builder.Services.AddScoped<IGetAccountsByPersonIdRepository, GetAccountsByPersonIdRepository>();
        builder.Services.AddScoped<IModifyAccountRepository, ModifyAccountRepository>();
        builder.Services.AddScoped<ICloseAccountRepository, CloseAccountRepository>();
        builder.Services.AddScoped<IBlockAccountRepository, BlockAccountRepository>();
    }

    public static void RegisterValidators(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<ModifyAccountValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<NewAccountForCurrentUserValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<NewAccountForAnyUserValidator>();
    }

    public static void RegisterMediator(WebApplicationBuilder builder)
    {
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
    }
}