using FluentValidation;
using ModulbankInternship.Accounts.Transactions.MakeTransaction;
using ModulbankInternship.Transactions.MakeTransaction;
using ModulbankInternship.Transactions.Validators;

namespace ModulbankInternship.Accounts.Transactions;

public static class TransactionRegistration
{
    public static void RegisterInjections(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMakeTransactionRepository, MakeTransactionRepository>();
    }

    public static void RegisterValidators(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<NewTransactionValidator>();
    }

    public static void RegisterMediator(WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<NewTransactionValidator>();
        });
    }
}