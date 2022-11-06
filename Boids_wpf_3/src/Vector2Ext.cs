using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public static class Vector2Ext
{
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
    public static Vector2 ClampLength(this Vector2 v, float min, float max)
    {
        var length = Helper.Length(v);
        if (length > max)
            return v*(max/length);
        if (length < min)
            return v*(min/length);
        // if (length > max)
        //     return v*max/length;
        // if (length < min)
        //     return v*min/length;
        return v;
    }
    public static Vector2 Abs(this Vector2 v) => new Vector2(MathF.Abs(v.X),MathF.Abs(v.Y));
    public static Vector2 RandomVector(Random rand, float length) => RandomVectorNorm(rand) * Rand(rand,0,1) * length;
    public static Vector2 RandomVectorNorm(Random rand) => Vector2.Normalize( new Vector2(Rand(rand,-1f,1f),Rand(rand,-1f,1f)) );
    private static float Rand(Random rand, float min, float max)
    {
        System.Diagnostics.Debug.Assert(min < max);
        var length = max - min;
        var t = (rand.NextSingle()-.5f)*2f;
        return min + length*t;
    }
}