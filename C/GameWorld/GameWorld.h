#ifndef GAMEWORLD_H
#define GAMEWORLD_H

#include "Boid4.h"

#define GAMEWORLD_BOIDSCOUNT 300

typedef struct GameWorld
{
    AABB aabb;
    Boid boids[GAMEWORLD_BOIDSCOUNT];
} GameWorld;

GameWorld* GameWorld_Init()
{
    GameWorld* gameWorld = (GameWorld*)malloc(sizeof(GameWorld));

    Subgen subgen = Subgen_Init(0);
    gameWorld->aabb.p0 = (MyVector2){ 0, 0 };
    gameWorld->aabb.p1 = (MyVector2){ 50, 50 };
	for (int i = 0; i < GAMEWORLD_BOIDSCOUNT; i++)
		gameWorld->boids[i] = Boid_Create(&gameWorld->aabb,&subgen);

    return gameWorld;
}
void GameWorld_End(GameWorld* gameWorld)
{
}
void GameWorld_Update(GameWorld* gameWorld, long realDeltaTimeInMilliseconds)
{
    float deltaTimeInSeconds = ((float)realDeltaTimeInMilliseconds)/1000;
    Boid_Update(gameWorld->boids,GAMEWORLD_BOIDSCOUNT,&gameWorld->aabb,deltaTimeInSeconds);
}

#endif