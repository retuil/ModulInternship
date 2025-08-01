namespace ModulbankInternship.Infrastructure;

public class MbSuccess<T>(T value): MbResult<T>(true, value, null, null);