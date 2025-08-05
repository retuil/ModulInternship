using MediatR;
using ModulbankInternship.Accounts;
using ModulbankInternship.Accounts.Requests;
using ModulbankInternship.Infrastructure;
using ModulbankInternship.Transactions.Requests;

namespace ModulbankInternship.Transactions.Handlers;

public class NewTransactionHandler(IMediator _mediator)
    : ICommandHandler<NewTransactionCommand, Guid>
{
    public async Task<Guid> Handle(NewTransactionCommand request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AddTransactionToAccountCommand(request.Model), cancellationToken);
        var id = await _mediator.Send(new AddTransactionToRepositoryCommand(request.Model), cancellationToken);
        return id;
    }
}