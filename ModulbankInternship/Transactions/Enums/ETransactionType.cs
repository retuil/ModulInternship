using System.Text.Json.Serialization;

namespace ModulbankInternship.Transactions.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ETransactionType
{
    Credit,
    Debit
}