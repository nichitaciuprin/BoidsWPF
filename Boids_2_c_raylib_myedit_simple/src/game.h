#include "boid.h"

#define GAME_BOIDSCOUNT 100

AABB aabb;
Boid boids[GAME_BOIDSCOUNT];

void Game_Init()
{
    Subgen_Init(0);

    aabb.p0 = (Vector2){ 0, 0 };
    aabb.p1 = (Vector2){ 50, 50 };
	for (int i = 0; i < GAME_BOIDSCOUNT; i++)
		boids[i] = CreateBoidRand(aabb);
}
void Game_Update(bool debug)
{
    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
        UpdateVelocity(&boids[i],boids,GAME_BOIDSCOUNT,debug);

    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
        UpdatePosition(&boids[i],aabb,0.02);
}
void Game_End()
{
    Print(&boids[0]);
}