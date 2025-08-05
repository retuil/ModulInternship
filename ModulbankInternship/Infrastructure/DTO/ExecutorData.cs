using ModulbankInternship.Users.Enums;

namespace ModulbankInternship.Users.DTO;

public class ExecutorData
{
    public Guid UserId { get; set; }
    public string? Role { get; set; }
}