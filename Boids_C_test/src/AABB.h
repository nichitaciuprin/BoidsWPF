#include "Vector2.h"

typedef struct AABB
{
    MyVector2 p0;
    MyVector2 p1;
} AABB;

float AABB_MinX(AABB* aabb) { return fminf(aabb->p0.x,aabb->p1.x); }
float AABB_MinY(AABB* aabb) { return fminf(aabb->p0.y,aabb->p1.y); }
float AABB_MaxX(AABB* aabb) { return fmaxf(aabb->p0.x,aabb->p1.x); }
float AABB_MaxY(AABB* aabb) { return fmaxf(aabb->p0.y,aabb->p1.y); }
MyVector2 AABB_Size(AABB* aabb) { MyVector2 duno = Vector2_Sub(aabb->p0,aabb->p1); return Vector2_Abs(duno); }
float AABB_RandPointInside_X(AABB* aabb) { return Subgen_Range( AABB_MinX(aabb), AABB_MaxX(aabb) ); }
float AABB_RandPointInside_Y(AABB* aabb) { return Subgen_Range( AABB_MinY(aabb), AABB_MaxY(aabb) ); }
MyVector2 AABB_RandPointInside(AABB* aabb) { return (MyVector2){ AABB_RandPointInside_X(aabb), AABB_RandPointInside_Y(aabb) }; }
MyVector2 AABB_WrapAround(AABB* aabb, MyVector2 point)
{
    MyVector2 size = AABB_Size(aabb);
    if      (point.x < AABB_MinX(aabb)) point.x += size.x;
    else if (point.x > AABB_MaxX(aabb)) point.x -= size.x;
    if      (point.y < AABB_MinY(aabb)) point.y += size.y;
    else if (point.y > AABB_MaxY(aabb)) point.y -= size.y;
    return point;
}