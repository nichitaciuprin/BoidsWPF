#include "helper.h"




#define PRECISION1 1000
#define PRECISION2 PRECISION1*2
float   HelperRandFactorUnsigned()         { return (float) (GetRandomValue(0, PRECISION1))              / PRECISION1; }
float   HelperRandFactorSigned()           { return (float) (GetRandomValue(0, PRECISION2) - PRECISION1) / PRECISION1; }
float   HelperRange(float min, float max)  { return min + ((max - min) * HelperRandFactorUnsigned()); }
Vector2 HelperRandNormDir()                { return Vector2Normalize( (Vector2){ HelperRandFactorSigned(), HelperRandFactorSigned() } ); }
Vector2 HelperAdd(Vector2 v1, Vector2 v2)  { return Vector2Add(v1,v2); }
Vector2 HelperSub(Vector2 v1, Vector2 v2)  { return Vector2Subtract(v1,v2); }
Vector2 HelperMul(Vector2 v1, float value) { return (Vector2) { v1.x * value, v1.y * value }; }
Vector2 HelperDiv(Vector2 v1, float value) { return (Vector2) { v1.x / value, v1.y / value }; }
Vector2 HelperAbs(Vector2 v1)              { return (Vector2) { fabs(v1.x), fabs(v1.y) }; }
bool    HelperIsZero(Vector2 v1)           { return v1.x == 0 && v1.y == 0; }