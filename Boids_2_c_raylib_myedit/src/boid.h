#ifndef BOID_H
#define BOID_H

#include <stdlib.h>
#include <raylib.h>
#include "aabb.h"

typedef struct Boid
{
    Vector2 position;
    Vector2 velocity;
} Boid;

void FreeBoid(Boid* boid);
Boid* CreateBoidRand(AABB aabb);
Boid* CreateBoid(Vector2 origin, Vector2 velocity);
void UpdateBoid(Boid* boid, Boid** flock, int boidsLength, AABB aabb, double deltaTime);

#endif