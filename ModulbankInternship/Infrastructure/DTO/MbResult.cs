namespace ModulbankInternship.Infrastructure;

public class MbResult<T>(bool isSuccess, T? value, string? errorCode, string? errorMessage)
{
    public bool IsSuccess { get; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public T? Value { get; } = value;
    public string? ErrorCode { get; } = errorCode;
    public string? ErrorMessage { get; } = errorMessage;
}


public static class MbResult
{
    public static MbResult<T> Success<T>(T value)
    {
        return new MbResult<T>(true, value, null, null);
    }

    public static MbResult<T> Failure<T>(string? errorCode, string? errorMessage)
    {
        return new MbResult<T>(false, default, errorCode, errorMessage);
    }
}