public static class Helper
{
    public static string ToHex(float value) => System.BitConverter.SingleToUInt32Bits(value).ToString("X");
}