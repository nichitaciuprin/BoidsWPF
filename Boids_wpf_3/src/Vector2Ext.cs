using System.Numerics;

public static class Vector2Ext
{
    public static Vector2 Add   (Vector2 v1, Vector2 v2)  { return v1+v2; }
    public static Vector2 Sub   (Vector2 v1, Vector2 v2)  { return v1-v2; }
    public static Vector2 Mul   (Vector2 v1, float value) { return v1*value; }
    public static Vector2 Div   (Vector2 v1, float value) { return v1/value; }
    public static Vector2 Abs   (Vector2 v)               { return new Vector2( MathF.Abs(v.X), MathF.Abs(v.Y) ); }
    public static bool    IsZero(Vector2 v)               { return v.X == 0 && v.Y == 0; }
    public static Vector2 Sum(this IEnumerable<Vector2> source)
    {
        var result = Vector2.Zero;
        foreach (var item in source)
            result += item;
        return result;
    }
    public static Vector2 Average(this IEnumerable<Vector2> source)
    {
        var result = Vector2.Zero;
        foreach (var item in source)
            result += item;
        return result/source.Count();
    }
    public static Vector2 RandNormDir()
    {
        var x = Subgen.FractionSigned();
        var y = Subgen.FractionSigned();
        var result = new Vector2(x, y);
        Normalize(ref result);
        return result;
    }
    public static Vector2 ClampLength(this Vector2 v, float min, float max)
    {
        var length = Length(v);
        if (length > max)
            return v*(max/length);
        if (length < min)
            return v*(min/length);
        return v;
    }
    public static void PrintVector2Hex(ref Vector2 value)
    {
        System.Console.WriteLine(Helper.ToHex(value.X));
    }
    public static void Normalize(ref Vector2 value)
    {
        var l = Length(value);
        var factor = 1.0f / l;
        value.X *= factor;
        value.Y *= factor;
    }
    public static Vector2 Normalize(Vector2 value)
    {
        var l = Length(value);
        var factor = 1.0f / l;
        return new Vector2(
            value.X * factor,
            value.Y * factor);
    }
    public static float Length(Vector2 value)
    {
        var ls = value.X * value.X + value.Y * value.Y;
        var l = System.MathF.Sqrt(ls);
        return l;
    }
    public static string ToHex(Vector2 value) => Helper.ToHex(value.X) + Helper.ToHex(value.Y);
}