using System.Text.Json.Serialization;

namespace ModulbankInternship.Users.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EUserRole
{
    Client,
    Cashier,
    Manager
}