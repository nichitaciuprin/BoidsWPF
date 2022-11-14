#include <stdlib.h>
#include <stdbool.h>
// #include <raylib.h>
// #include <raymath.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include "Subgen.h"

typedef struct Vector2
{
    float x;
    float y;
} Vector2;

bool    Vector2_IsZero(Vector2 v1)           { return v1.x == 0 && v1.y == 0; }
Vector2 Vector2_Add(Vector2 v1, Vector2 v2)  { return (Vector2) { v1.x + v2.x , v1.y + v2.y }; }
Vector2 Vector2_Sub(Vector2 v1, Vector2 v2)  { return (Vector2) { v1.x - v2.x , v1.y - v2.y }; }
Vector2 Vector2_Mul(Vector2 v1, float value) { return (Vector2) { v1.x * value, v1.y * value }; }
Vector2 Vector2_Div(Vector2 v1, float value) { return (Vector2) { v1.x / value, v1.y / value }; }
Vector2 Vector2_Abs(Vector2 v1)              { return (Vector2) { fabs(v1.x), fabs(v1.y) }; }
Vector2 Vector2_Negate(Vector2 v1)           { return (Vector2) { -v1.x,-v1.y }; }
Vector2 Vector2_Zero()                       { return (Vector2) { 0.0f, 0.0f }; }
float   Vector2_Length(Vector2 v)
{
    float distSquared = v.x*v.x + v.y*v.y;
    float dist = sqrtf(distSquared);
    return dist;
}
Vector2 Vector2_Normalized(Vector2 v)
{
    float dist = Vector2_Length(v);
    float t = 1.0f / dist;
    return (Vector2) { v.x*t , v.y*t };
}
Vector2 Vector2_RandNormDir()
{
    float x = Subgen_FractionSigned();
    float y = Subgen_FractionSigned();
    return Vector2_Normalized( (Vector2) { x,y } );
}
Vector2 Vector2_ClampLength(Vector2 v1, float min, float max)
{
    float length = Vector2_Length(v1);
    if (length > max)
        return Vector2_Mul(v1,(max/length));
    if (length < min)
        return Vector2_Mul(v1,(min/length));
    return v1;
}
void    Vector2_PrintVector2(Vector2 v1)
{
    printf("<%f,%f>\n",v1.x,v1.y);
}
void    Vector2_PrintVector2Hex(Vector2 v1)
{
    Helper_PrintFloatHex(v1.x);
    Helper_PrintFloatHex(v1.y);
}
