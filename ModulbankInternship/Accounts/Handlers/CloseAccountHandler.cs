using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ModulbankInternship.Account;
using ModulbankInternship.Account.Exceptions;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;

namespace ModulbankInternship.Accounts;

public class CloseAccountHandler(IMediator _mediator, IAccountsRepository _AccountsRepository)
    : ICommandHandler<CloseAccountCommand, bool>
{
    public Task<bool> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        var Account = _AccountsRepository.Get(request.AccountId);
        _mediator.Send(new CheckExecutorAccessCommand(Account.OwnerId, request.ExecutorId,
                    new[] { EAccessClass.Owner, EAccessClass.Manager }));
        if (!Account.IsExist)
        {
            throw new ResourceNotFoundException($"No open Account with id: {request.AccountId}");
        }

        _AccountsRepository.Delete(request.AccountId);
        return Task.FromResult(true);
    }
}