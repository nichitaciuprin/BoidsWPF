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
    Boid boids2[GAME_BOIDSCOUNT];

    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
        boids2[i] = UpdateVelocity(i,boids,GAME_BOIDSCOUNT,debug);

    // if (debug)
    // {
    //     for (int i = 0; i < GAME_BOIDSCOUNT; i++)
    //         PrintBoid(&boids2[i]);
    // }

    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
        boids[i] = boids2[i];

    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
        UpdatePosition(&boids[i],aabb,0.02);
}
void Game_End()
{
    Print(&boids[0]);
    // for (int i = 0; i < GAME_BOIDSCOUNT; i++)
    //     Print(&boids[i]);
}
/*
40A6909E41B49C924110C3F741259639
4106D8B541C88A20C0A845F6C111035E
-------
40A6909E41B49C924110C3F741259639
4106D8B541C88A20C0A845F6C111035E
*/