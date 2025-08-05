using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts;

public class CreateAccountHandler(IMediator _mediator, IAccountsRepository _AccountsRepository)
    : IRequestHandler<CreateAccountCommand, Guid>
{
    public Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var newAccountRequest = request.NewAccountRequest;
        _mediator.Send(new CheckExecutorAccessCommand(newAccountRequest.OwnerId, request.Executor,
            new[] { EAccessClass.Owner, EAccessClass.Manager }), cancellationToken);

        var newAccountId = _AccountsRepository.Add(new AccountModel()
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