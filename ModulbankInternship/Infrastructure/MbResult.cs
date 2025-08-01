namespace ModulbankInternship.Infrastructure;

public abstract class MbResult<T>(bool isSuccess, T? value, string? errorCode, string? errorMessage)
{
    public bool IsSuccess { get; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public T? Value { get; } = value;
    public string? ErrorCode { get; } = errorCode;
    public string? ErrorMessage { get; } = errorMessage;
}
