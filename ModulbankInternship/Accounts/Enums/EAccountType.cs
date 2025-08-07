using System.Text.Json.Serialization;

namespace ModulbankInternship.Accounts.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EAccountType
{
    Checking,
    Deposit,
    Credit
}