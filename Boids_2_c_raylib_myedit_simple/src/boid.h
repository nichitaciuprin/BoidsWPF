#include "aabb.h"
#include <stdlib.h>

typedef struct Boid
{
    Vector2 pos;
    Vector2 vel;
    Vector2 vec_1;
    Vector2 vec_2;
    Vector2 vec_3;
    int count_1;
    int count_2;
} Boid;

float minSpeed = 9;
float maxSpeed = 15;
float range_1 = 5;
float range_2 = 5;
float range_3 = 2;
// float range_1 = 25;
// float range_2 = 25;
// float range_3 = 4;
float power1 = 0.01;
float power2 = 0.01;
float power3 = 0.04;

Boid CreateBoidRand(AABB* aabb)
{
    float randSpeed = Subgen_Range(minSpeed,maxSpeed);
    Vector2 randDirection = Helper_RandNormDir();

    Vector2 pos = AABB_RandPointInside(aabb);
    Vector2 vel = Helper_Mul(randDirection,randSpeed);

    Boid boid;
    boid.pos = pos;
    boid.vel = vel;

    return boid;
}
void UpdateVelocity_1(Boid* boid1, Boid* boid2)
{
    Vector2 diff = Helper_Sub(boid1->pos,boid2->pos);
    float dist1Squared = diff.x*diff.x + diff.y*diff.y;
    float dist1 = sqrtf(dist1Squared);

    // COHESION
    if (dist1 >= range_1) return;

    boid1->vec_1 = Helper_Add(boid1->vec_1,boid2->pos); boid1->count_1++;
    boid2->vec_1 = Helper_Add(boid2->vec_1,boid1->pos); boid2->count_1++;

    // ALIGHMENT
    if (dist1 >= range_2) return;

    boid1->vec_2 = Helper_Add(boid1->vec_2,boid2->vel); boid1->count_2++;
    boid2->vec_2 = Helper_Add(boid2->vec_2,boid1->vel); boid2->count_2++;

    // SEPARATION
    if (dist1 >= range_3) return;

    float ilength = 1.0f/dist1;
    Vector2 normDiff = (Vector2) { diff.x*ilength, diff.y*ilength };
    float dist2 = range_3 - dist1;
    normDiff = Helper_Mul(normDiff,dist2);
    boid1->vec_3 = Helper_Add(boid1->vec_3,normDiff);
    boid2->vec_3 = Helper_Add(boid2->vec_3,Vector2Negate(normDiff));
}
void UpdateVelocity_2(Boid* boid)
{
    if (boid->count_1 != 0)
    {
        boid->vec_1 = Helper_Div(boid->vec_1,boid->count_1);
        boid->vec_1 = Helper_Sub(boid->vec_1,boid->pos);
    }
    if (boid->count_2 != 0)
    {
        boid->vec_2 = Helper_Div(boid->vec_2,boid->count_2);
        boid->vec_2 = Helper_Sub(boid->vec_2,boid->vel);
    }

    boid->vec_1 = Helper_Mul(boid->vec_1,power1);
    boid->vec_2 = Helper_Mul(boid->vec_2,power2);
    boid->vec_3 = Helper_Mul(boid->vec_3,power3);

    boid->vel = Helper_Add(boid->vel,boid->vec_1);
    boid->vel = Helper_Add(boid->vel,boid->vec_2);
    boid->vel = Helper_Add(boid->vel,boid->vec_3);

    boid->vel = Vector2ClampValue(boid->vel,minSpeed,maxSpeed);

    boid->vec_1 = Vector2Zero();
    boid->vec_2 = Vector2Zero();
    boid->vec_3 = Vector2Zero();
    boid->count_1 = 0;
    boid->count_2 = 0;
}
void UpdatePosition(Boid* boid, AABB* aabb, float deltaTime)
{
    Vector2 velocityDelta = Helper_Mul(boid->vel,deltaTime);
    boid->pos = Helper_Add(boid->pos,velocityDelta);
    boid->pos = AABB_WrapAround(aabb,boid->pos);
}
void Boid_Update(Boid* boids, int boidsLength, AABB* aabb, float deltaTime)
{
    int length = boidsLength;

    // ALL UNIQUE PAIRS
    for (int i = 0;   i < length; i++)
    for (int j = i+1; j < length; j++)
        UpdateVelocity_1(&boids[i], &boids[j]);

    for (int i = 0;   i < length; i++)
        UpdateVelocity_2(&boids[i]);

    for (int i = 0; i < length; i++)
        UpdatePosition(&boids[i],aabb,deltaTime);
}
void PrintBoid(Boid* boid)
{
    Helper_PrintVector2Hex(boid->pos);
    Helper_PrintVector2Hex(boid->vel);
    Helper_PrintEmptyLine();
}