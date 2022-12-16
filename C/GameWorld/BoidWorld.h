#ifndef BOIDWORLD_H
#define BOIDWORLD_H

#include "../Base/AABB.h"

#define minSpeed 9
#define maxSpeed 15
#define range_1 5
#define range_2 5
#define range_3 2
#define rangeSquared_1 range_1*range_1
#define rangeSquared_2 range_2*range_2
#define rangeSquared_3 range_3*range_3
#define power1 0.01
#define power2 0.01
#define power3 0.04
#define boidCount 300
#define xMin 0
#define xMax 50
#define yMin 0
#define yMax 50

typedef struct Boid
{
    MyVector2 pos;
    MyVector2 vel;
    MyVector2 velNew;
} Boid;
typedef struct BoidWorld
{
    int boidIndexOther;
    Boid* boids[boidCount];
} BoidWorld;
BoidWorld* boidWorld = NULL;

void Boid_PrintBoid(Boid* boid)
{
    MyVector2_PrintVector2Hex(boid->pos);
    MyVector2_PrintVector2Hex(boid->vel);
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

    return boid;
}
void BoidWorld_Init()
{
    if (boidWorld == NULL) return;

    boidWorld = (BoidWorld*)malloc(sizeof(BoidWorld));

    Subgen subgen = Subgen_Init(0);
    boidWorld->aabb.p0 = (MyVector2){ 0, 0 };
    boidWorld->aabb.p1 = (MyVector2){ 50, 50 };
	for (int i = 0; i < boidsCount; i++)
		boidWorld->boids[i] = Boid_Create(&boidWorld->aabb,&subgen);
}
void BoidWorld_Update(float deltaTimeMilliseconds)
{
    if (boidWorld == NULL) return;

    for (boidIndex = 0; boidIndex < boidWorld->boidsCount; boidIndex++)
    {
        if (boidWorld == NULL) return;
        #define boid boidWorld->boids[boidIndex]

        for (int boidIndexOther = 0; boidIndexOther < boidWorld->boidsCount; boidIndexOther++)
        {
            if (boidIndex == boidIndexOther) continue;

            #define boidOther boidWorld->boids[boidIndexOther]

            boidWorld->diff = MyVector2_Sub(boid.pos,boidOther.pos);
            boidWorld->distSquared = boidWorld->diff.x*boidWorld->diff.x + boidWorld->diff.y*boidWorld->diff.y;
            boidWorld->dist = sqrtf(boidWorld->distSquared);

            // COHESION
            if (boidWorld->distSquared >= rangeSquared_1) continue;
            boidWorld->vec_1 = MyVector2_Add(boidWorld->vec_1,boidOther.pos); boidWorld->count_1++;

            // ALIGHMENT
            if (boidWorld->distSquared >= rangeSquared_2) continue;
            boidWorld->vec_2 = MyVector2_Add(boidWorld->vec_2,boidOther.vel); boidWorld->count_2++;

            // SEPARATION
            if (boidWorld->distSquared >= rangeSquared_3) continue;
            boidWorld->normFraction = 1.0f/boidWorld->dist;
            boidWorld->normDiff = (MyVector2) { boidWorld->diff.x*boidWorld->normFraction, boidWorld->diff.y*boidWorld->normFraction };
            boidWorld->dist2 = range_3 - boidWorld->dist;
            boidWorld->normDiff = MyVector2_Mul(boidWorld->normDiff,boidWorld->dist2);
            boidWorld->vec_3 = MyVector2_Add(boidWorld->vec_3,boidWorld->normDiff);
        }

        if (boidWorld->count_1 != 0)
        {
            boidWorld->vec_1 = MyVector2_Div(boidWorld->vec_1,boidWorld->count_1);
            boidWorld->vec_1 = MyVector2_Sub(boidWorld->vec_1,boid.pos);
        }
        if (boidWorld->count_2 != 0)
        {
            boidWorld->vec_2 = MyVector2_Div(boidWorld->vec_2,boidWorld->count_2);
            boidWorld->vec_2 = MyVector2_Sub(boidWorld->vec_2,boid.vel);
        }

        boidWorld->vec_1 = MyVector2_Mul(boidWorld->vec_1,power1);
        boidWorld->vec_2 = MyVector2_Mul(boidWorld->vec_2,power2);
        boidWorld->vec_3 = MyVector2_Mul(boidWorld->vec_3,power3);

        boid.velNew = boid.vel;
        boid.velNew = MyVector2_Add(boid.velNew,boidWorld->vec_1);
        boid.velNew = MyVector2_Add(boid.velNew,boidWorld->vec_2);
        boid.velNew = MyVector2_Add(boid.velNew,boidWorld->vec_3);
        boid.velNew = MyVector2_ClampLength(boid.velNew,minSpeed,maxSpeed);
    }

    for (int i = 0; i < boidWorld->boidsCount; i++)
        boidWorld->boids[i].vel = boidWorld->boids[i].velNew;

    float deltaTimeSeconds = ((float)deltaTimeMilliseconds)/1000;

    for (int i = 0; i < boidWorld->boidsCount; i++)
    {
        #define boid boidWorld->boids[boidIndex]
        MyVector2 velocityDelta = MyVector2_Mul(boid.vel,deltaTime);
        boid.pos = MyVector2_Add(boid.pos,velocityDelta);
        boid.pos = AABB_WrapAround(&boidWorld->aabb,boid.pos);
    }
}

#endif