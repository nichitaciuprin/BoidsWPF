namespace MyVector2
{
    public struct Vector2
    {
        public float x;
        public float y;
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool IsZero (Vector2 v) => v.x == 0 && v.y == 0;
        public static void PrintVector2Hex(Vector2 v)
        {
            Helper.PrintFloatHex(v.x);
            Helper.PrintFloatHex(v.y);
        }
        public static float Length(Vector2 v)
        {
            float distSquared = v.x*v.x + v.y*v.y;
            float dist = MathF.Sqrt(distSquared);
            return dist;
        }
        public static float Vector2_LengthSquared(Vector2 v)
        {
            float distSquared = v.x*v.x + v.y*v.y;
            return distSquared;
        }
        public static float Angle(Vector2 v)
        {
            // v ( 0, 1) =  0.000000
            // v ( 1, 0) = -1.570796
            // v ( 0,-1) = -3.141593
            // v (-1, 0) =  1.570796
            return Vector2.IsZero(v) ? 0 : MathF.Atan2(v.y,v.x) - MathF.PI/2;
        }
        public static Vector2 Zero => new Vector2(0,0);
        public static Vector2 Add (Vector2 v1, Vector2 v2)  => new Vector2( v1.x + v2.x  , v1.y + v2.y  );
        public static Vector2 Sub (Vector2 v1, Vector2 v2)  => new Vector2( v1.x - v2.x  , v1.y - v2.y  );
        public static Vector2 Mul (Vector2 v1, float value) => new Vector2( v1.x * value , v1.y * value );
        public static Vector2 Div (Vector2 v1, float value) => new Vector2( v1.x / value , v1.y / value );
        public static Vector2 Abs (Vector2 v)               => new Vector2( MathF.Abs(v.x) , MathF.Abs(v.y) );
        public static Vector2 Negate(Vector2 v)             => new Vector2(-v.x,-v.y);
        public static Vector2 Normalized(Vector2 v)
        {
            var factor = 1.0f / Vector2.Length(v);
            return new Vector2(
                v.x * factor,
                v.y * factor);
        }
        public static Vector2 RandNormDir(Subgen subgen)
        {
            var x = subgen.FractionSigned();
            var y = subgen.FractionSigned();
            var result = new Vector2(x, y);
            return Normalized(result);
        }
        public static Vector2 ClampLength(Vector2 v, float min, float max)
        {
            var length = Length(v);
            if (length > max)
                return Mul(v,(max/length));
            if (length < min)
                return Mul(v,(min/length));
            return v;
        }
        public static Vector2 Rotate(Vector2 v, float angle)
        {
            var result = Zero;
            float cosres = MathF.Cos(angle);
            float sinres = MathF.Sin(angle);
            result.x = v.x*cosres - v.y*sinres;
            result.y = v.x*sinres + v.y*cosres;
            return result;
        }
    }
}
