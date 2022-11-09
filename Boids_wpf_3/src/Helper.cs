using System.Numerics;

public static class Helper
{
    public static Vector2 RandNormDir()
    {
        var x = Subgen.FractionSigned();
        var y = Subgen.FractionSigned();
        var result = Normalize( new Vector2(x, y) );
        // Helper.PrintVector2Hex(result);
        // System.Console.Write('=');
        // Helper.PrintFloatHex(x);
        // Helper.PrintFloatHex(y);
        // System.Console.WriteLine();
        return result;
    }
    public static Vector2 Add(Vector2 v1, Vector2 v2)  { return v1+v2; }
    public static Vector2 Sub(Vector2 v1, Vector2 v2)  { return v1-v2; }
    public static Vector2 Mul(Vector2 v1, float value) { return v1*value; }
    public static Vector2 Div(Vector2 v1, float value) { return v1/value; }
    public static Vector2 Abs(Vector2 v1)              { return Vector2.Abs(v1); }
    public static bool    IsZero(Vector2 v1)           { return v1.X == 0 && v1.Y == 0; }
    public static void PrintVector2(Vector2 v1) { System.Console.WriteLine(v1); }
    public static void PrintFloatHex(float value)
    {
        var result = System.BitConverter.SingleToUInt32Bits(value);
        var hex = result.ToString("X");
        System.Console.Write(hex);
    }
    public static string ToHex(float value) => System.BitConverter.SingleToUInt32Bits(value).ToString("X");
    public static string ToHex(Vector2 value) => ToHex(value.X) + ToHex(value.Y);
    public static void PrintVector2Hex(Vector2 value)
    {
        PrintFloatHex(value.X);
        PrintFloatHex(value.Y);
    }
    public static Vector2 Normalize(Vector2 value)
    {
        var l = Length(value);
        var factor = 1.0f / l;
        return new Vector2(value.X * factor, value.Y * factor);
    }
    public static float Length(Vector2 value)
    {
        var ls = value.X * value.X + value.Y * value.Y;
        var l = System.MathF.Sqrt(ls);
        return l;
    }
}