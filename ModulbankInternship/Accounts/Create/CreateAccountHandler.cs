using MediatR;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Create;

public class CreateAccountHandler(IMediator mediator, ICreateAccountRepository rep): IRequestHandler<CreateAccountCommand, Guid>
{
    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var newAccountRequest = request.NewAccountRequest;
        await mediator.Send(new CheckExecutorAccessCommand(newAccountRequest.OwnerId, request.Executor,
            new[] { EAccessClass.Owner, EAccessClass.Manager }), cancellationToken);

        var newAccountId = await rep.AddAsync(new AccountModel()
        {
            Currency = newAccountRequest.Currency,
            InterestRate = newAccountRequest.InterestRate,
            OwnerId = newAccountRequest.OwnerId,
            AccountType = newAccountRequest.AccountType,
            OpeningDate = DateTime.UtcNow
        });
        return newAccountId!.Value;
    }
}