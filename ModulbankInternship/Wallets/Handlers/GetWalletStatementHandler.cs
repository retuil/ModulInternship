using MediatR;
using ModulbankInternship.Account;
using ModulbankInternship.Auth.Enums;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Users.Requests;
using ModulbankInternship.Wallets.Interfaces;
using ModulbankInternship.Wallets.Requests;

namespace ModulbankInternship.Wallets;

public class GetWalletStatementHandler(IMediator _mediator, IWalletsRepository _walletsRepository)
    : IQueryHandler<GetWalletStatementQuery, WalletStatementResponse>
{
    public Task<WalletStatementResponse> Handle(GetWalletStatementQuery request, CancellationToken cancellationToken)
    {
        var startDate = request.StartDate;
        var finishDate = request.FinishDate;
        var wallet = _walletsRepository.Get(request.WalletId);
        _mediator.Send(new CheckExecutorAccessCommand(wallet.OwnerId, request.ExecutorId,
            new[] { EAccessClass.Owner, EAccessClass.Manager }));
        
        var statementTransactions = wallet.Transactions
            .Where(t => t.DateTime >= startDate && t.DateTime <= finishDate);
        var response = new WalletStatementResponse()
        {
            CreationDate = DateTime.Now,
            StartDate = startDate,
            FinishDate = finishDate,
            Transactions = statementTransactions
        };

        return Task.FromResult(response);
    }
}