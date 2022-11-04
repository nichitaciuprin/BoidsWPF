#ifndef AABB_H
#define AABB_H

#include "helper.h"

typedef struct AABB
{
    Vector2 p0;
    Vector2 p1;
} AABB;

float RandPointInside_X(AABB aabb);
float RandPointInside_Y(AABB aabb);

float AABB_MinX(AABB aabb);
float AABB_MinY(AABB aabb);
float AABB_MaxX(AABB aabb);
float AABB_MaxY(AABB aabb);
Vector2 AABB_Size(AABB aabb);
Vector2 AABB_RandPointInside(AABB aabb);
Vector2 AABB_WrapAround(AABB aabb, Vector2 point);

#endif