namespace ModulbankInternship.Infrastructure;

public interface IModel
{
    public Guid Id { get; set; }
    public bool IsExist { get; set; }
}