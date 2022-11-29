#ifndef BOIDWORLD_H
#define BOIDWORLD_H

#include "Boid.h"

#define BOIDWORLD_BOIDSCOUNT 300

typedef struct BoidWorld
{
    AABB aabb;
    Boid boids[BOIDWORLD_BOIDSCOUNT];
    int boidsLength;
} BoidWorld;

BoidWorld Game_Init()
{
    Subgen subgen = Subgen_Init(0);

    BoidWorld boidWorld;
    boidWorld.boidsLength = BOIDWORLD_BOIDSCOUNT;
    boidWorld.aabb.p0 = (MyVector2){ 0, 0 };
    boidWorld.aabb.p1 = (MyVector2){ 50, 50 };
	for (int i = 0; i < BOIDWORLD_BOIDSCOUNT; i++)
		boidWorld.boids[i] = CreateBoidRand(&boidWorld.aabb,&subgen);
    return boidWorld;
}
void Game_End(BoidWorld* boidWorld)
{
}
void Game_Update(BoidWorld* boidWorld, long realDeltaTimeInMilliseconds)
{
    float deltaTimeInSeconds = ((float)realDeltaTimeInMilliseconds)/1000;
    Boid_Update(boidWorld->boids,boidWorld->boidsLength,&boidWorld->aabb,deltaTimeInSeconds);
}

#endif