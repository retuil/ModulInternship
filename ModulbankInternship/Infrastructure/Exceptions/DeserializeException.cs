namespace ModulbankInternship.Infrastructure.Exceptions;

public class DeserializeException(string targetClassName, string message)
    : FormatException($"Failed to deserialize the message in {targetClassName}: {message}");