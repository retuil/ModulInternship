using System.Text.Json.Serialization;

namespace ModulbankInternship.Account;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EAccountType
{
    Checking,
    Deposit,
    Credit
}