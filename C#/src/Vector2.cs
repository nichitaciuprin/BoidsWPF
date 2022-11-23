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

        public static Vector2 Zero = new Vector2(0,0);
        public static Vector2 Add    (Vector2 v1, Vector2 v2)  => new Vector2( v1.x + v2.x  , v1.y + v2.y  );
        public static Vector2 Sub    (Vector2 v1, Vector2 v2)  => new Vector2( v1.x - v2.x  , v1.y - v2.y  );
        public static Vector2 Mul    (Vector2 v1, float value) => new Vector2( v1.x * value , v1.y * value );
        public static Vector2 Div    (Vector2 v1, float value) => new Vector2( v1.x / value , v1.y / value );
        public static Vector2 Abs    (Vector2 v)               => new Vector2( MathF.Abs(v.x) , MathF.Abs(v.y) );
        public static bool    IsZero (Vector2 v)               => v.x == 0 && v.y == 0;
        public Vector2 Negate => new Vector2(-x,-y);
        public static Vector2 Sum(Vector2[] vs)
        {
            var result = Vector2.Zero;
            foreach (var item in vs)
                result = Add(result,item);
            return result;
        }
        public static Vector2 Average(Vector2[] vs)
        {
            var result = Vector2.Zero;
            foreach (var item in vs)
                result = Add(result,item);
            return Div(result,vs.Length);
        }
        public static Vector2 RandNormDir(Subgen subgen)
        {
            var x = subgen.FractionSigned();
            var y = subgen.FractionSigned();
            var result = new Vector2(x, y);
            result.Normalize();
            return result;
        }
        public Vector2 ClampLength(float min, float max)
        {
            var length = Length();
            if (length > max)
                return Mul(this,(max/length));
            if (length < min)
                return Mul(this,(min/length));
            return this;
        }
        public void Normalize()
        {
            var factor = 1.0f / Length();
            x *= factor;
            y *= factor;
        }
        public Vector2 Normalized()
        {
            var factor = 1.0f / Length();
            return new Vector2(
                x * factor,
                y * factor);
        }
        public float Length() => System.MathF.Sqrt(x*x + y*y);
        public string ToHex() => Helper.ToHex(x) + Helper.ToHex(y);
    }
}
