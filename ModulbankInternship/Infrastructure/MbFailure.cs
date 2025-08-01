namespace ModulbankInternship.Infrastructure;

public class MbFailure<T>(string errorCode, string errorMessage): MbResult<T>(false, default, errorCode, errorMessage);