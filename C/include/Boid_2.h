#pragma once

#include "MyVector2.h"
#include "AABB.h"
#include "Subgen.h"
#include "AABB.h"

typedef struct Boid
{
    MyVector2 pos;
    MyVector2 vel;
    MyVector2 velNew;
} Boid;

void Boid_Print(Boid* boid);
Boid Boid_Create(AABB* aabb, Subgen* subgen);
void Boid_Update_Position(Boid* boid, AABB* aabb, float deltaTime);
void Boid_Update_Velocity(int boidIndex, Boid* boids, int boidsCount);
void Boid_Update(Boid* boids, int boidsLength, AABB* aabb, float deltaTime);
