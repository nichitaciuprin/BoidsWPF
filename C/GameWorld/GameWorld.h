#ifndef GAMEWORLD_H
#define GAMEWORLD_H

#include "Boid.h"

#define GAMEWORLD_BOIDSCOUNT 300

typedef struct GameWorld
{
    AABB aabb;
    Boid boids[GAMEWORLD_BOIDSCOUNT];
    int boidsLength;
} GameWorld;

GameWorld GameWorld_Init()
{
    Subgen subgen = Subgen_Init(0);

    GameWorld gameWorld;
    gameWorld.boidsLength = GAMEWORLD_BOIDSCOUNT;
    gameWorld.aabb.p0 = (MyVector2){ 0, 0 };
    gameWorld.aabb.p1 = (MyVector2){ 50, 50 };
	for (int i = 0; i < GAMEWORLD_BOIDSCOUNT; i++)
		gameWorld.boids[i] = Boid_Create(&gameWorld.aabb,&subgen);
    return gameWorld;
}
void GameWorld_End(GameWorld* gameWorld)
{
}
void GameWorld_Update(GameWorld* gameWorld, long realDeltaTimeInMilliseconds)
{
    float deltaTimeInSeconds = ((float)realDeltaTimeInMilliseconds)/1000;
    Boid_Update(gameWorld->boids,gameWorld->boidsLength,&gameWorld->aabb,deltaTimeInSeconds);
}

#endif