#ifndef HELPER_H
#define HELPER_H

#define _USE_MATH_DEFINES
#include <raylib.h>
#include <raymath.h>

float   HelperRandFactorUnsigned();
float   HelperRandFactorSigned();
float   HelperRange(float min, float max);
Vector2 HelperRandNormDir();
Vector2 HelperAdd(Vector2 v1, Vector2 v2);
Vector2 HelperSub(Vector2 v1, Vector2 v2);
Vector2 HelperMul(Vector2 v1, float value);
Vector2 HelperDiv(Vector2 v1, float value);
Vector2 HelperAbs(Vector2 v);
bool    HelperIsZero(Vector2 v1);

#endif
