#include <stdlib.h>
#include <stdbool.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include "Subgen.h"
#include "Helper.h"

typedef struct MyVector2
{
    float x;
    float y;
} MyVector2;

bool      Vector2_IsZero(MyVector2 v1)             { return v1.x == 0 && v1.y == 0; }
MyVector2 Vector2_Add(MyVector2 v1, MyVector2 v2)  { return (MyVector2) { v1.x + v2.x , v1.y + v2.y }; }
MyVector2 Vector2_Sub(MyVector2 v1, MyVector2 v2)  { return (MyVector2) { v1.x - v2.x , v1.y - v2.y }; }
MyVector2 Vector2_Mul(MyVector2 v1, float value)   { return (MyVector2) { v1.x * value, v1.y * value }; }
MyVector2 Vector2_Div(MyVector2 v1, float value)   { return (MyVector2) { v1.x / value, v1.y / value }; }
MyVector2 Vector2_Abs(MyVector2 v1)                { return (MyVector2) { fabs(v1.x), fabs(v1.y) }; }
MyVector2 Vector2_Negate(MyVector2 v1)             { return (MyVector2) { -v1.x,-v1.y }; }
MyVector2 Vector2_Zero()                           { return (MyVector2) { 0.0f, 0.0f }; }
float     Vector2_Length(MyVector2 v)
{
    float distSquared = v.x*v.x + v.y*v.y;
    float dist = sqrtf(distSquared);
    return dist;
}
MyVector2 Vector2_Normalized(MyVector2 v)
{
    float dist = Vector2_Length(v);
    float t = 1.0f / dist;
    return (MyVector2) { v.x*t , v.y*t };
}
MyVector2 Vector2_RandNormDir()
{
    float x = Subgen_FractionSigned();
    float y = Subgen_FractionSigned();
    return Vector2_Normalized( (MyVector2) { x,y } );
}
MyVector2 Vector2_ClampLength(MyVector2 v1, float min, float max)
{
    float length = Vector2_Length(v1);
    if (length > max)
        return Vector2_Mul(v1,(max/length));
    if (length < min)
        return Vector2_Mul(v1,(min/length));
    return v1;
}
void      Vector2_PrintVector2(MyVector2 v1)
{
    printf("<%f,%f>\n",v1.x,v1.y);
}
void      Vector2_PrintVector2Hex(MyVector2 v1)
{
    Helper_PrintFloatHex(v1.x);
    Helper_PrintFloatHex(v1.y);
}
MyVector2 Vector2_Rotate(MyVector2 v, float angle)
{
    MyVector2 result = Vector2_Zero();
    float cosres = cosf(angle);
    float sinres = sinf(angle);
    result.x = v.x*cosres - v.y*sinres;
    result.y = v.x*sinres + v.y*cosres;
    return result;
}