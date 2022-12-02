#ifndef GAMEWORLD_H
#define GAMEWORLD_H

#include "Boid2.h"

typedef struct GameWorld
{
    AABB aabb;
    // BoidsState* boidsState;
} GameWorld;

GameWorld* GameWorld_Init()
{
    GameWorld* gameWorld = (GameWorld*)malloc(sizeof(GameWorld));

    // gameWorld->aabb.p0 = (MyVector2){ 0, 0 };
    // gameWorld->aabb.p1 = (MyVector2){ 50, 50 };
    // gameWorld->boidsState = Boid_Create(&gameWorld->aabb);

    return gameWorld;
}
void GameWorld_End(GameWorld* gameWorld)
{
    free(gameWorld);
}
void GameWorld_Update(GameWorld* gameWorld, long realDeltaTimeInMilliseconds)
{
    // float deltaTimeInSeconds = ((float)realDeltaTimeInMilliseconds)/1000;
    // Boid_Update(gameWorld->boids,GAMEWORLD_BOIDSCOUNT,&gameWorld->aabb,deltaTimeInSeconds);
}

#endif