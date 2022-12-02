#ifndef BOID_H
#define BOID_H

#include "../Base/AABB.h"

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

typedef struct BoidPair
{
    Boid boid1;
    Boid boid2;
} BoidPair;

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

void Boid_Update_1(Boid* boid)
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
void Boid_Update_0(BoidPair* boidPair)
{
    Boid* boid1 = &boidPair->boid1;
    Boid* boid2 = &boidPair->boid2;

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
void Boid_Update2(BoidPair* boidPair, int length)
{
    for (int i = 0; i < length; i++)
        Boid_Update_0(boidPair);
}
void Boid_Add(Boid* left, Boid* right)
{
    left->count_1 += right->count_1;
    left->count_2 += right->count_2;
    left->vec_1 = MyVector2_Add(left->vec_1,right->vec_1);
    left->vec_2 = MyVector2_Add(left->vec_2,right->vec_2);
    left->vec_3 = MyVector2_Add(left->vec_3,right->vec_3);
}
void Boid_Update_2(Boid* boid, AABB* aabb, float deltaTime)
{
    MyVector2 velocityDelta = MyVector2_Mul(boid->vel,deltaTime);
    boid->pos = MyVector2_Add(boid->pos,velocityDelta);
    boid->pos = AABB_WrapAround(aabb,boid->pos);
}
void Boid_Update(Boid* boids, int boidsLength, AABB* aabb, float deltaTime)
{
    int boidPairsIndex;
    int pairsCount = (boidsLength * boidsLength - boidsLength) / 2;
    BoidPair boidPairs[pairsCount];

    boidPairsIndex = 0;
    for (int i = 0;   i < boidsLength; i++)
    for (int j = i+1; j < boidsLength; j++, boidPairsIndex++)
    {
        boidPairs[boidPairsIndex] = (BoidPair) { boids[i],boids[j] };
    }

    // NEED PARALLEL CALC
    for (int i = 0; i < pairsCount; i++)
        Boid_Update_0(&boidPairs[i]);

    boidPairsIndex = 0;
    for (int i = 0;   i < boidsLength; i++)
    for (int j = i+1; j < boidsLength; j++, boidPairsIndex++)
    {
        Boid_Add(&boids[i],&boidPairs[boidPairsIndex].boid1);
        Boid_Add(&boids[j],&boidPairs[boidPairsIndex].boid2);
    }

    // NEED PARALLEL CALC
    for (int i = 0; i < boidsLength; i++)
        Boid_Update_1(&boids[i]);

    // NEED PARALLEL CALC
    for (int i = 0; i < boidsLength; i++)
        Boid_Update_2(&boids[i], aabb, deltaTime);
}

#endif