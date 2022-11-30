#include "Boid.h"

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

typedef struct BoidCache
{
    MyVector2 vec_1;
    MyVector2 vec_2;
    MyVector2 vec_3;
    int count_1;
    int count_2;
} BoidCache;

typedef struct BoidCalc
{
    int arrayIndex;
    Boid boid;
    BoidCache cache;
} BoidCalc;

typedef struct BoidCalcUnit
{
    BoidCalc boidCalc1;
    BoidCalc boidCalc2;
} BoidCalcUnit;

BoidCache BoidCache_Create()
{
    BoidCache result;
    result.vec_1 = MyVector2_Zero();
    result.vec_2 = MyVector2_Zero();
    result.vec_3 = MyVector2_Zero();
    result.count_1 = 0;
    result.count_2 = 0;
    return result;
}
BoidCalc BoidCalc_Create(Boid boid, int arrayIndex)
{
    BoidCalc result;
    result.arrayIndex = arrayIndex;
    result.boid = boid;
    result.cache = BoidCache_Create();
    return result;
}
BoidCalcUnit BoidCalcUnit_Create(Boid* boids, int boid1Index, int boid2Index)
{
    BoidCalcUnit pair;
    pair.boidCalc1 = BoidCalc_Create(boids[boid1Index], boid1Index);
    pair.boidCalc2 = BoidCalc_Create(boids[boid2Index], boid2Index);
    return pair;
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
void Boid_UpdateBoidPair(BoidCalcUnit* boidPair)
{
    Boid* boid1 = boidPair->boid1;
    Boid* boid2 = boidPair->boid2;

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
void Boid_Update(Boid* boids, int boidsLength)
{
    int boidPairsIndex = 0;
    int pairsCount = (boidsLength * boidsLength - boidsLength) / 2;
    BoidCalcUnit units[pairsCount];
    for (int i = 0;   i < boidsLength; i++)
    for (int j = i+1; j < boidsLength; j++)
    {
        units[boidPairsIndex] = BoidCalcUnit_Create(boids,i,j);
        boidPairsIndex++;
    }

    for (int i = 0;   i < boidsLength; i++)
        Boid_UpdateVelocity_2(&boids[i]);
}