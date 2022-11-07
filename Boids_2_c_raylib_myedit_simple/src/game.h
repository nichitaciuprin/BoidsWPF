#include "boid.h"

#define GAME_BOIDSCOUNT 5

AABB aabb;
Boid boids[GAME_BOIDSCOUNT];

void Game_Init()
{
    Subgen_Init(0);

    aabb.p0 = (Vector2){ 0, 0 };
    aabb.p1 = (Vector2){ 50, 50 };
	for (int i = 0; i < GAME_BOIDSCOUNT; i++)
		boids[i] = CreateBoidRand(aabb);
    // Print(&boids[0]);
}
void Game_Update(bool debug)
{
    Update(boids,GAME_BOIDSCOUNT,aabb,0.02,debug);
}
void Game_End()
{
    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
        Print(&boids[i]);
}
