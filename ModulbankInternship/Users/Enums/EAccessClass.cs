using System.Text.Json.Serialization;

namespace ModulbankInternship.Users.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EAccessClass
{
    Manager,
    Owner,
    Cashier,
    Anyone
}