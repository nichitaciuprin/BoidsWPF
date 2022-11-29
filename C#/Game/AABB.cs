public struct AABB
{
    public AABB(MyVector2 p0, MyVector2 p1)
    {
        this.p0 = p0;
        this.p1 = p1;
    }
    public MyVector2 p0;
    public MyVector2 p1;
    public MyVector2 Size => MyVector2.Abs(MyVector2.Sub(p0,p1));
    public float MinX => MathF.Min(p0.x,p1.x);
    public float MinY => MathF.Min(p0.y,p1.y);
    public float MaxX => MathF.Max(p0.x,p1.x);
    public float MaxY => MathF.Max(p0.y,p1.y);
    public float RandPointInside_X(Subgen subgen) => subgen.Range( MinX, MaxX );
    public float RandPointInside_Y(Subgen subgen) => subgen.Range( MinY, MaxY );
    public MyVector2 RandPointInside(Subgen subgen) => new MyVector2( RandPointInside_X(subgen), RandPointInside_Y(subgen) );
    public MyVector2 WrapAround(MyVector2 point)
    {
        var size = Size;
        if      (point.x < MinX) point.x += size.x;
        else if (point.x > MaxX) point.x -= size.x;
        if      (point.y < MinY) point.y += size.y;
        else if (point.y > MaxY) point.y -= size.y;
        return point;
    }
}