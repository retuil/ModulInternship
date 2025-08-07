using MediatR;
using ModulbankInternship.Accounts.Interfaces;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Infrastructure.Exceptions;
using ModulbankInternship.Infrastructure.Interfaces;
using ModulbankInternship.Users.Enums;
using ModulbankInternship.Users.Requests;

namespace ModulbankInternship.Accounts.Handlers;

public class CloseAccountHandler(IMediator mediator, IAccountsRepository accountsRepository)
    : ICommandHandler<CloseAccountCommand, bool>
{
    public Task<bool> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        var account = accountsRepository.Get(request.AccountId);
        if (account is null || !account.IsExist)
        {
            throw new ResourceNotFoundException($"No open Account with id: {request.AccountId}");
        }
        
        mediator.Send(new CheckExecutorAccessCommand(account.OwnerId, request.Executor,
                    new[] { EAccessClass.Owner, EAccessClass.Manager }), cancellationToken);
        
        accountsRepository.Delete(request.AccountId);
        return Task.FromResult(true);
    }
}