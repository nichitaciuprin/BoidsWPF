#ifndef BOID_H
#define BOID_H

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

    // ALIGHMENT
    if (distSquared >= rangeSquared_2) return;

    boid1->vec_2 = MyVector2_Add(boid1->vec_2,boid2->vel); boid1->count_2++;

    // SEPARATION
    if (distSquared >= rangeSquared_3) return;

    float normFraction = 1.0f/dist;
    MyVector2 normDiff = (MyVector2) { diff.x*normFraction, diff.y*normFraction };
    float dist2 = range_3 - dist;
    normDiff = MyVector2_Mul(normDiff,dist2);

    boid1->vec_3 = MyVector2_Add(boid1->vec_3,normDiff);
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

    for (int i = 0; i < length; i++)
    for (int j = 0; j < length; j++)
    {
        if (i == j) continue;
        Boid_UpdateVelocity_1(&boids[i], &boids[j]);
    }

    for (int i = 0;   i < length; i++)
        Boid_UpdateVelocity_2(&boids[i]);

    for (int i = 0; i < length; i++)
        Boid_UpdatePosition(&boids[i],aabb,deltaTime);
}

#endif