using System.Text.Json.Serialization;

namespace ModulbankInternship.Accounts.Transactions.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ETransactionType
{
    Credit,
    Debit
}