#include "aabb.h"
#include <stdlib.h>

typedef struct Boid
{
    Vector2 pos;
    Vector2 vel;
} Boid;

float minSpeed = 9;
float maxSpeed = 15;
float range_1 = 5;
float range_2 = 5;
float range_3 = 2;
float power1 = 0.01;
float power2 = 0.01;
float power3 = 0.04;

void Print(Boid* boid)
{
    PrintVector2Hex(boid->pos);
    PrintVector2Hex(boid->vel);
    PrintEmptyLine();
}
Boid CreateBoidRand(AABB aabb)
{
    float randSpeed = Subgen_Range(minSpeed,maxSpeed);
    Vector2 randDirection = HelperRandNormDir();

    Vector2 pos = AABB_RandPointInside(aabb);
    Vector2 vel = HelperMul(randDirection,randSpeed);

    Boid boid;
    boid.pos = pos;
    boid.vel = vel;

    return boid;
}
void UpdateVelocity(Boid* boid, Boid* boids, int boidsLength, bool debug)
{
    Vector2 vec_1 = Vector2Zero();
    Vector2 vec_2 = Vector2Zero();
    Vector2 vec_3 = Vector2Zero();
    int count_1 = 0;
    int count_2 = 0;

    for (int i = 0; i < boidsLength; i++)
    {
        Boid* otherBoid = &boids[i];

        if (boid == otherBoid) continue;

        Vector2 diff = HelperSub(boid->pos,otherBoid->pos);
        float dist = Vector2Length(diff);

        // COHESION
        if (dist < range_1)
        {
            vec_1 = HelperAdd(vec_1,otherBoid->pos);
            count_1++;
        }

        // ALIGHMENT
        if (dist < range_2)
        {
            vec_2 = HelperAdd(vec_2,otherBoid->vel);
            count_2++;
        }

        // SEPARATION
        if (dist < range_3)
        {
            Vector2 normDiff = Vector2Normalize(diff);
            float d2 = range_3 - dist;
            normDiff = HelperMul(normDiff,d2);
            vec_3 = HelperAdd(vec_3,normDiff);
        }
    }

    if (count_1 != 0)
    {
        vec_1 = HelperDiv(vec_1,count_1);
        vec_1 = HelperSub(vec_1,boid->pos);
    }
    if (count_2 != 0)
    {
        vec_2 = HelperDiv(vec_2,count_2);
        vec_2 = HelperSub(vec_2,boid->vel);
    }

    vec_1 = HelperMul(vec_1,power1);
    vec_2 = HelperMul(vec_2,power2);
    vec_3 = HelperMul(vec_3,power3);

    Vector2 result = boid->vel;
    result = HelperAdd(HelperAdd(HelperAdd(result,vec_1),vec_2),vec_3);
    result = Vector2ClampValue(result,minSpeed,maxSpeed);
    boid->vel = result;
}
void UpdatePosition(Boid* boid, AABB aabb, float deltaTime)
{
    Vector2 velocityDelta = HelperMul(boid->vel,deltaTime);
    boid->pos = HelperAdd(boid->pos,velocityDelta);
    boid->pos = AABB_WrapAround(aabb,boid->pos);
}
void Update(Boid* boids, int boidsLength, AABB aabb, float deltaTime, bool debug)
{
    for (int i = 0; i < boidsLength; i++)
    {
        UpdateVelocity(&boids[i],boids,boidsLength,debug);
        UpdatePosition(&boids[i],aabb,deltaTime);
    }
}