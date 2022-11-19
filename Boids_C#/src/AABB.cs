using MyVector2;

public struct AABB
{
    public AABB(Vector2 p0, Vector2 p1)
    {
        this.p0 = p0;
        this.p1 = p1;
    }
    public Vector2 p0;
    public Vector2 p1;
    public Vector2 Size => Vector2.Abs(Vector2.Sub(p0,p1));
    public float MinX => MathF.Min(p0.x,p1.x);
    public float MinY => MathF.Min(p0.y,p1.y);
    public float MaxX => MathF.Max(p0.x,p1.x);
    public float MaxY => MathF.Max(p0.y,p1.y);
    public float RandPointInside_X => Subgen.Range( MinX, MaxX );
    public float RandPointInside_Y => Subgen.Range( MinY, MaxY );
    public Vector2 RandPointInside => new Vector2( RandPointInside_X, RandPointInside_Y );
    public Vector2 WrapAround(Vector2 point)
    {
        var size = Size;
        if      (point.x < MinX) point.x += size.x;
        else if (point.x > MaxX) point.x -= size.x;
        if      (point.y < MinY) point.y += size.y;
        else if (point.y > MaxY) point.y -= size.y;
        return point;
    }
}