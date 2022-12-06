#ifndef BOID2_H
#define BOID2_H

#include "../Base/AABB.h"

typedef struct Boid
{
    MyVector2 pos;
    MyVector2 vel;
    MyVector2 vec_1;
    MyVector2 vec_2;
    MyVector2 vec_3;
    int count_1;
    int count_2;
} Boid;

const float minSpeed = 9;
const float maxSpeed = 15;
const float range_1 = 5;
const float range_2 = 5;
const float range_3 = 2;
const float rangeSquared_1 = range_1*range_1;
const float rangeSquared_2 = range_2*range_2;
const float rangeSquared_3 = range_3*range_3;
const float power1 = 0.01;
const float power2 = 0.01;
const float power3 = 0.04;

void Boid_PrintBoid(Boid* boid)
{
    MyVector2_PrintVector2Hex(boid->pos);
    MyVector2_PrintVector2Hex(boid->vel);
}
void Boid_InitCatche(Boid* boid)
{
    boid->vec_1 = MyVector2_Zero();
    boid->vec_2 = MyVector2_Zero();
    boid->vec_3 = MyVector2_Zero();
    boid->count_1 = 0;
    boid->count_2 = 0;
}
Boid Boid_Create(AABB* aabb, Subgen* subgen)
{
    float randSpeed = Subgen_Range(subgen,minSpeed,maxSpeed);
    MyVector2 randDirection = MyVector2_RandNormDir(subgen);

    MyVector2 pos = AABB_RandPointInside(subgen,aabb);
    MyVector2 vel = MyVector2_Mul(randDirection,randSpeed);

    Boid boid;
    boid.pos = pos;
    boid.vel = vel;

    Boid_InitCatche(&boid);

    return boid;
}
void Boid_UpdateVelocity_1(Boid* boid1, Boid* boid2)
{
    MyVector2 diff = MyVector2_Sub(boid1->pos,boid2->pos);
    float distSquared = diff.x*diff.x + diff.y*diff.y;
    float dist = sqrtf(distSquared);

    // COHESION
    if (distSquared >= rangeSquared_1) return;

    boid1->vec_1 = MyVector2_Add(boid1->vec_1,boid2->pos); boid1->count_1++;
    boid2->vec_1 = MyVector2_Add(boid2->vec_1,boid1->pos); boid2->count_1++;

    // ALIGHMENT
    if (distSquared >= rangeSquared_2) return;

    boid1->vec_2 = MyVector2_Add(boid1->vec_2,boid2->vel); boid1->count_2++;
    boid2->vec_2 = MyVector2_Add(boid2->vec_2,boid1->vel); boid2->count_2++;

    // SEPARATION
    if (distSquared >= rangeSquared_3) return;

    float normFraction = 1.0f/dist;
    MyVector2 normDiff = (MyVector2) { diff.x*normFraction, diff.y*normFraction };
    float dist2 = range_3 - dist;
    normDiff = MyVector2_Mul(normDiff,dist2);

    boid1->vec_3 = MyVector2_Add(boid1->vec_3,normDiff);
    boid2->vec_3 = MyVector2_Add(boid2->vec_3, MyVector2_Negate(normDiff));
}
void Boid_UpdateVelocity_2(Boid* boid)
{
    if (boid->count_1 != 0)
    {
        boid->vec_1 = MyVector2_Div(boid->vec_1,boid->count_1);
        boid->vec_1 = MyVector2_Sub(boid->vec_1,boid->pos);
    }
    if (boid->count_2 != 0)
    {
        boid->vec_2 = MyVector2_Div(boid->vec_2,boid->count_2);
        boid->vec_2 = MyVector2_Sub(boid->vec_2,boid->vel);
    }

    boid->vec_1 = MyVector2_Mul(boid->vec_1,power1);
    boid->vec_2 = MyVector2_Mul(boid->vec_2,power2);
    boid->vec_3 = MyVector2_Mul(boid->vec_3,power3);

    boid->vel = MyVector2_Add(boid->vel,boid->vec_1);
    boid->vel = MyVector2_Add(boid->vel,boid->vec_2);
    boid->vel = MyVector2_Add(boid->vel,boid->vec_3);

    boid->vel = MyVector2_ClampLength(boid->vel,minSpeed,maxSpeed);

    Boid_InitCatche(boid);
}
void Boid_UpdatePosition(Boid* boid, AABB* aabb, float deltaTime)
{
    MyVector2 velocityDelta = MyVector2_Mul(boid->vel,deltaTime);
    boid->pos = MyVector2_Add(boid->pos,velocityDelta);
    boid->pos = AABB_WrapAround(aabb,boid->pos);
}
void Boid_Update(Boid* boids, int boidsLength, AABB* aabb, float deltaTime)
{
    int length = boidsLength;

    // ALL UNIQUE PAIRS
    for (int i = 0;   i < length; i++)
    for (int j = i+1; j < length; j++)
        Boid_UpdateVelocity_1(&boids[i], &boids[j]);

    // // WITH EXTRA PAIRS
    // for (int i = 0; i < length; i++)
    // for (int j = 0; j < length; j++)
    // {
    //     if (i == j) continue;
    //     Boid_UpdateVelocity_1(&boids[i], &boids[j]);
    // }

    for (int i = 0;   i < length; i++)
        Boid_UpdateVelocity_2(&boids[i]);

    for (int i = 0; i < length; i++)
        Boid_UpdatePosition(&boids[i],aabb,deltaTime);
}

#endif


// float deltaTimeInSeconds = ((float)realDeltaTimeInMilliseconds)/1000;
// Boid* boids = gameWorld->boids;
// AABB* aabb = &gameWorld->aabb;
// pthread_t thread_1;
// pthread_t thread_2;
// ParArg parArg1;
// ParArg parArg2;
// parArg1.boids = boids;
// parArg2.boids = boids;
// parArg1.startIndex = 0;
// parArg2.startIndex = 149;
// parArg1.count = 150;
// parArg2.count = 150;
// pthread_create(&thread_1, NULL, ParralelTask, &parArg1 );
// pthread_create(&thread_2, NULL, ParralelTask, &parArg2 );
// pthread_join(thread_1, NULL);
// pthread_join(thread_2, NULL);

// typedef struct ParArg
// {
//     Boid* boids;
//     int startIndex;
//     int count;
// } ParArg;


// void * ParralelTask(void * arg)
// {
//     ParArg* parArg = (ParArg*)arg;
//     Boid* boids = parArg->boids;
//     for (int i = parArg->startIndex; i < parArg->count; i++)
//     for (int j = 0; j < GAMEWORLD_BOIDSCOUNT; j++)
//     {
//         if (i == j) continue;
//         Boid_UpdateVelocity_1(&boids[i], &boids[j]);
//     }
//     return 0;
// }

// // WITH EXTRA PAIRS
// for (int i = 0; i < length; i++)
// for (int j = 0; j < length; j++)
// {
//     if (i == j) continue;
//     Boid_UpdateVelocity_1(&boids[i], &boids[j]);
// }