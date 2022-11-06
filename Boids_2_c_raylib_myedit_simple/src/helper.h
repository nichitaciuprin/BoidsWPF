#include <stdlib.h>
#include <raylib.h>
#include <raymath.h>
#define _USE_MATH_DEFINES
#include <math.h>
#include "subgen.h"


Vector2 HelperAdd(Vector2 v1, Vector2 v2)  { return Vector2Add(v1,v2); }
Vector2 HelperSub(Vector2 v1, Vector2 v2)  { return Vector2Subtract(v1,v2); }
Vector2 HelperMul(Vector2 v1, float value) { return (Vector2) { v1.x * value, v1.y * value }; }
Vector2 HelperDiv(Vector2 v1, float value) { return (Vector2) { v1.x / value, v1.y / value }; }
Vector2 HelperAbs(Vector2 v1)              { return (Vector2) { fabs(v1.x), fabs(v1.y) }; }
bool    HelperIsZero(Vector2 v1)           { return v1.x == 0 && v1.y == 0; }
void PrintEmptyLine() { printf("\n"); }
void PrintFloatHex(float f) { printf("%X", *(unsigned int *)&f); }
void PrintVector2(Vector2 v1) { printf("<%f,%f>\n",v1.x,v1.y); }
void PrintVector2Hex(Vector2 v1) { PrintFloatHex(v1.x), PrintFloatHex(v1.y); }
Vector2 HelperRandNormDir()
{
    float x = Subgen_FractionSigned();
    float y = Subgen_FractionSigned();
    Vector2 result = Vector2Normalize( (Vector2) { x,y } );
    return result;
}