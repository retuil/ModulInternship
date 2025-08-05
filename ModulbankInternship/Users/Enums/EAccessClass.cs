using System.Text.Json.Serialization;

namespace ModulbankInternship.Auth.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EAccessClass
{
    Manager,
    Owner,
    Cashier,
    Anyone
}