#ifndef GAMEWORLD_H
#define GAMEWORLD_H

#include "Boid.h"
#include <pthread.h>

#define GAMEWORLD_BOIDSCOUNT 300

typedef struct GameWorld
{
    AABB aabb;
    Boid boids[GAMEWORLD_BOIDSCOUNT];
} GameWorld;

typedef struct ParArg
{
    Boid* boids;
    int boidsCount;
    AABB* aabb;
    float deltaTimeInSeconds;
} ParArg;

void * ParralelTask(void * arg)
{
    ParArg* parArg = (ParArg*)arg;
    Boid_Update(parArg->boids,parArg->boidsCount,parArg->aabb,parArg->deltaTimeInSeconds);
    return 0;
}
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

    // float deltaTimeInSeconds = ((float)realDeltaTimeInMilliseconds)/1000;
    // pthread_t thread_1;
    // pthread_t thread_2;
    // ParArg parArg1;
    // ParArg parArg2;
    // parArg1.boids = &gameWorld->boids[0];
    // parArg2.boids = &gameWorld->boids[149];
    // parArg1.aabb = &gameWorld->aabb;
    // parArg2.aabb = &gameWorld->aabb;
    // parArg1.boidsCount = 150;
    // parArg2.boidsCount = 150;
    // parArg1.deltaTimeInSeconds = deltaTimeInSeconds;
    // parArg2.deltaTimeInSeconds = deltaTimeInSeconds;
    // pthread_create(&thread_1, NULL, ParralelTask, &parArg1 );
    // pthread_create(&thread_2, NULL, ParralelTask, &parArg2 );
    // pthread_join(thread_1, NULL);
    // pthread_join(thread_2, NULL);
}

#endif