using System;
using System.Numerics;

public struct AABB
{
    public AABB(Vector2 p0, Vector2 p1)
    {
        this.p0 = p0;
        this.p1 = p1;
    }
    public Vector2 p0;
    public Vector2 p1;
    public Vector2 Size => (p0 - p1).Abs();
    public float MinX => MathF.Min(p0.X,p1.X);
    public float MinY => MathF.Min(p0.Y,p1.Y);
    public float MaxX => MathF.Max(p0.X,p1.X);
    public float MaxY => MathF.Max(p0.Y,p1.Y);
    public float RandPointInside_X => Subgen.Range( MinX, MaxX );
    public float RandPointInside_Y => Subgen.Range( MinY, MaxY );
    public Vector2 RandPointInside => new Vector2( RandPointInside_X, RandPointInside_Y );
    public Vector2 WrapAround(Vector2 point)
    {
        var size = Size;
        if      (point.X < MinX) point.X += size.X;
        else if (point.X > MaxX) point.X -= size.X;
        if      (point.Y < MinY) point.Y += size.Y;
        else if (point.Y > MaxY) point.Y -= size.Y;
        return point;
    }
}