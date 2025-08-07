using MediatR;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Models;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Handlers;

public class CreateAccountHandler(IMediator mediator, IAccountsRepository accountsRepository)
    : IRequestHandler<CreateAccountCommand, Guid>
{
    public Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var newAccountRequest = request.NewAccountRequest;
        mediator.Send(new CheckExecutorAccessCommand(newAccountRequest.OwnerId, request.Executor,
            new[] { EAccessClass.Owner, EAccessClass.Manager }), cancellationToken);

        var newAccountId = accountsRepository.Add(new AccountModel()
        {
            Currency = newAccountRequest.Currency,
            InterestRate = newAccountRequest.InterestRate,
            OwnerId = newAccountRequest.OwnerId,
            AccountType = newAccountRequest.AccountType,
            OpeningDate = DateTime.Now
        });
        return Task.FromResult(newAccountId);
    }
}