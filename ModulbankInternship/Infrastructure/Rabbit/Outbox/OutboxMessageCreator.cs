using System.Text.Json;
using ModulbankInternship.Accounts.Domain.Models;
using ModulbankInternship.Accounts.Transactions.Domain.Enums;
using ModulbankInternship.Infrastructure.Rabbit.Models;
using ModulbankInternship.Infrastructure.Rabbit.Outbox.Events;
using ModulbankInternship.Transactions.Models;

namespace ModulbankInternship.Infrastructure.Rabbit.Outbox;

public static class OutboxMessageCreator
{
    public static OutboxMessageModel AccountOpened(AccountModel account)
    {
        const string eventType = "AccountOpened";
        const string routingKey = "account.opened";
        var payload = new AccountOpenedEvent
        (
            Guid.NewGuid(),
            DateTime.UtcNow,
            account.Id,
            account.OwnerId,
            account.Currency,
            account.AccountType
        );

        return CreateMessage(eventType, routingKey, JsonSerializer.Serialize(payload));
    }

    public static OutboxMessageModel NewTransaction(TransactionModel transaction)
    {
        var accountId = transaction.AccountId;
        var amount = transaction.Amount;
        var currency = transaction.Currency;
        var operationId = transaction.Id;

        return transaction.Type switch
        {
            ETransactionType.Credit => MoneyCredited(accountId, amount, currency, operationId),
            ETransactionType.Debit => MoneyDebited(accountId, amount, currency, operationId, transaction.Description),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private static OutboxMessageModel MoneyCredited(Guid accountId, decimal amount, string currency, Guid operationId)
    {
        const string eventType = "MoneyCredited";
        const string routingKey = "money.credited";
        var payload = new MoneyCreditedEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            accountId,
            amount,
            currency,
            operationId
            );

        return CreateMessage(eventType, routingKey, JsonSerializer.Serialize(payload));
    }
    
    private static OutboxMessageModel MoneyDebited(Guid accountId, decimal amount, string currency, Guid operationId, string reason)
    {
        const string eventType = "MoneyDebited";
        const string routingKey = "money.debited";
        var payload = new MoneyDebitedEvent
        (
            Guid.NewGuid(),
            DateTime.UtcNow,
            accountId,
            amount,
            currency,
            operationId,
            reason
        );

        return CreateMessage(eventType, routingKey, JsonSerializer.Serialize(payload));
    }
    
    public static OutboxMessageModel TransferCompleted(TransferModel transfer)
    {
        const string eventType = "TransferCompleted";
        const string routingKey = "money.transfer.completed";
        var payload = new TransferCompletedEvent
        (
            Guid.NewGuid(),
            DateTime.UtcNow,
            transfer.SourceAccountId,
            transfer.DestinationAccountId,
            transfer.Amount,
            transfer.Currency,
            transfer.Id
        );

        return CreateMessage(eventType, routingKey, JsonSerializer.Serialize(payload));
    }
    
    public static OutboxMessageModel InterestAccrued(Guid accountId, DateTime periodFrom, DateTime periodTo, decimal amount)
    {
        const string eventType = "InterestAccrued";
        const string routingKey = "money.interest.accrued";
        var payload = new InterestOccuredEvent
        (
            Guid.NewGuid(),
            DateTime.UtcNow,
            accountId,
            periodFrom,
            periodTo,
            amount
        );

        return CreateMessage(eventType, routingKey, JsonSerializer.Serialize(payload));
    }
    
    private static OutboxMessageModel CreateMessage(string eventType, string routingKey, string payload)
    {
        return new OutboxMessageModel()
        {
            CreatedAt = DateTime.UtcNow,
            EventType = eventType,
            RoutingKey = routingKey,
            Payload = payload
        };
    }
}