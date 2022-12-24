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
const float acc = 10;

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
MyVector2 Boid_TargetVelocity(Boid* boid)
{
    MyVector2 result = boid->vel;

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

    result = MyVector2_Add(result,boid->vec_1);
    result = MyVector2_Add(result,boid->vec_2);
    result = MyVector2_Add(result,boid->vec_3);

    result = MyVector2_ClampLength(result,minSpeed,maxSpeed);

    Boid_InitCatche(boid);

    return result;
}
void Boid_Update(Boid* boids, int boidsLength, AABB* aabb, float deltaTime)
{
    int length = boidsLength;

    // ALL UNIQUE PAIRS
    for (int i = 0;   i < length; i++)
    for (int j = i+1; j < length; j++)
        Boid_UpdateVelocity_1(&boids[i], &boids[j]);

    for (int i = 0; i < length; i++)
    {
        Boid* boid = &boids[i];
        MyVector2 vel1 = boid->vel;
        MyVector2 targetVelocity = Boid_TargetVelocity(boid);
        MyVector2 vel2 = MyVector2_MoveTowards(vel1,targetVelocity,acc*deltaTime);
        boid->pos = MyVector2_Add(boid->pos,MyVector2_Mul(MyVector2_Div(MyVector2_Sub(vel2,vel1),2),deltaTime));
        boid->vel = vel2;
        // A.pos = Snap(A.pos,B.pos);
        boid->pos = AABB_WrapAround(aabb,boid->pos);

        // Boid* boid = &boids[i];
        // boid->vel = Boid_TargetVelocity(boid);
        // MyVector2 velocityDelta = MyVector2_Mul(boid->vel,deltaTime);
        // boid->pos = MyVector2_Add(boid->pos,velocityDelta);
        // boid->pos = AABB_WrapAround(aabb,boid->pos);
    }
}

#endif