#pragma once

#include "AABB.h"
#include "Boid.h"

#define GAMEWORLD_BOIDSCOUNT 300

typedef struct GameWorld
{
    AABB aabb;
    Boid boids[GAMEWORLD_BOIDSCOUNT];
} GameWorld;

GameWorld GameWorld_Init();
void GameWorld_Update(GameWorld* gameWorld, long realDeltaTimeInMilliseconds);
void GameWorld_End(GameWorld* gameWorld);