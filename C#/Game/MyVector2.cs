public struct MyVector2
{
    public float x;
    public float y;
    public MyVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static bool IsZero (MyVector2 v) => v.x == 0 && v.y == 0;
    public static void PrintVector2Hex(MyVector2 v)
    {
        Helper.PrintFloatHex(v.x);
        Helper.PrintFloatHex(v.y);
    }
    public static float Length(MyVector2 v)
    {
        float distSquared = v.x*v.x + v.y*v.y;
        float dist = MathF.Sqrt(distSquared);
        return dist;
    }
    public static float Vector2_LengthSquared(MyVector2 v)
    {
        float distSquared = v.x*v.x + v.y*v.y;
        return distSquared;
    }
    public static float Angle(MyVector2 v)
    {
        // v ( 0, 1) =  0.000000
        // v ( 1, 0) = -1.570796
        // v ( 0,-1) = -3.141593
        // v (-1, 0) =  1.570796
        return MyVector2.IsZero(v) ? 0 : MathF.Atan2(v.y,v.x) - MathF.PI/2;
    }
    public static MyVector2 Zero => new MyVector2(0,0);
    public static MyVector2 Add (MyVector2 v1, MyVector2 v2)  => new MyVector2( v1.x + v2.x  , v1.y + v2.y  );
    public static MyVector2 Sub (MyVector2 v1, MyVector2 v2)  => new MyVector2( v1.x - v2.x  , v1.y - v2.y  );
    public static MyVector2 Mul (MyVector2 v1, float value) => new MyVector2( v1.x * value , v1.y * value );
    public static MyVector2 Div (MyVector2 v1, float value) => new MyVector2( v1.x / value , v1.y / value );
    public static MyVector2 Abs (MyVector2 v)               => new MyVector2( MathF.Abs(v.x) , MathF.Abs(v.y) );
    public static MyVector2 Negate(MyVector2 v)             => new MyVector2(-v.x,-v.y);
    public static MyVector2 Normalized(MyVector2 v)
    {
        var factor = 1.0f / MyVector2.Length(v);
        return new MyVector2(
            v.x * factor,
            v.y * factor);
    }
    public static MyVector2 RandNormDir(Subgen subgen)
    {
        var x = subgen.FractionSigned();
        var y = subgen.FractionSigned();
        var result = new MyVector2(x, y);
        return Normalized(result);
    }
    public static MyVector2 ClampLength(MyVector2 v, float min, float max)
    {
        var length = Length(v);
        if (length > max)
            return Mul(v,(max/length));
        if (length < min)
            return Mul(v,(min/length));
        return v;
    }
    public static MyVector2 Rotate(MyVector2 v, float angle)
    {
        var result = Zero;
        float cosres = MathF.Cos(angle);
        float sinres = MathF.Sin(angle);
        result.x = v.x*cosres - v.y*sinres;
        result.y = v.x*sinres + v.y*cosres;
        return result;
    }
}