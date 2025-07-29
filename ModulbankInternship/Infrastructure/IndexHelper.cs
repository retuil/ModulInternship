namespace ModulbankInternship.Infrastructure;

public static class IndexHelper
{
    private static readonly int[] ByteOrder = [15, 14, 13, 12, 11, 10, 9, 8, 6, 7, 4, 5, 0, 1, 2, 3];

    public static Guid NextGuid(Guid guid)
    {
        var bytes = guid.ToByteArray();
        var canIncrement = ByteOrder.Any(i => ++bytes[i] != 0);
        return new Guid(canIncrement ? bytes : new byte[16]);
    }
}