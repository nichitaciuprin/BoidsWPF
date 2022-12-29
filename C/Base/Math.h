#ifndef MATH_H
#define MATH_H

// #define _USE_MATH_DEFINES
// #include <math.h>
// #include <stdlib.h>
// #include <stdbool.h>
#include <raymath.h>

inline float Math_Clamp(float value, float min, float max)
{
    return Clamp(value,min,max);
}
inline float Math_Sign(float value)
{
    if (value > 0) return  1;
    if (value < 0) return -1;
                   return  0;
}
inline float Math_Abs(float value)
{
    return abs(value);
}
inline float Math_Sqrt(float value)
{
    return sqrt(value);
}


#endif