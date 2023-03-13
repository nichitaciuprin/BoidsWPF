#pragma once

#include "AABB.h"
#include <pthread.h>
#include <stdio.h>

typedef struct Boid
{
    MyVector2 pos;
    MyVector2 vel;
    MyVector2 velNew;
} Boid;

typedef struct BoidTask
{
    pthread_t thread;
    Boid* boids;
    int boidsCount;
    int startIndex;
    int count;
    volatile bool enabled;
} BoidTask;

void* BoidTask_Thread(void* arg);
void Boid_Update_Velocity(int boidIndex, Boid* boids, int boidsCount);
void Boid_Update_Position(Boid* boid, AABB* aabb, float deltaTime);
void Boid_Update(Boid* boids, int boidsLength, AABB* aabb, float deltaTime);
Boid Boid_Create(AABB* aabb, Subgen* subgen);
void Boid_Print(Boid* boid);
void Boid_MaybeInit();
void BoidTask_InitThread(BoidTask* boidTask);
void BoidTask_Start(BoidTask* boidTask, Boid* boids, int boidsCount, int index, int count);
void BoidTask_Wait(BoidTask* boidTask);
void* BoidTask_Thread(void* arg);
void BoidTask_InitThread(BoidTask* boidTask);
void BoidTask_Start(BoidTask* boidTask, Boid* boids, int boidsCount, int index, int count);
void BoidTask_Wait(BoidTask* boidTask);
void* BoidTask_Thread(void* arg);
void Boid_MaybeInit();
void Boid_Print(Boid* boid);
Boid Boid_Create(AABB* aabb, Subgen* subgen);
void Boid_Update_Velocity(int boidIndex, Boid* boids, int boidsCount);
void Boid_Update_Position(Boid* boid, AABB* aabb, float deltaTime);
void Boid_Update(Boid* boids, int boidsLength, AABB* aabb, float deltaTime);