using FluentValidation;
using ModulbankInternship.Accounts.Transfer.MakeTransfer;

namespace ModulbankInternship.Accounts.Transfer;

public static class TransferRegistration
{
    public static void RegisterInjections(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMakeTransferRepository, MakeTransferRepository>();
    }

    public static void RegisterValidators(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<TransferValidator>();
    }

    public static void RegisterMediator(WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<TransferValidator>();
        });
    }
}