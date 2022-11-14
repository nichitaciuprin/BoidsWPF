#include "boid.h"

#define GAME_BOIDSCOUNT 300

AABB aabb;
Boid boids[GAME_BOIDSCOUNT];

void Game_Init()
{
    Subgen_Init(0);

    aabb.p0 = (Vector2){ 0, 0 };
    aabb.p1 = (Vector2){ 50, 50 };
	for (int i = 0; i < GAME_BOIDSCOUNT; i++)
		boids[i] = CreateBoidRand(&aabb);
}
void Game_Update()
{
    Boid_Update(boids,GAME_BOIDSCOUNT,&aabb,0.02f);
}
void Game_End()
{
    printf("First boid: ");
    PrintBoid(&boids[0]);
}