#include "boid.h"

#define GAME_BOIDSCOUNT 2

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
    // printf("?\n");
    // Print(&boids[0]);
    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
        UpdateVelocity(&boids[i],boids,GAME_BOIDSCOUNT,debug);

    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
        UpdatePosition(&boids[i],aabb,0.02);
}
void Game_End()
{
    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
        Print(&boids[i]);
}
/*
417F1F53420751AD40CB613E40D08617
3FE5B43B4142EAEEC081A248C100966C
409F1515422979BD408A9CE04100F09A
41D277764226E840C1406CAFC0C0FACC
41B9DB5041068C5CBDB2DB29C12C4582
420CBB6441E59711C1532FB1C02EB636
-------
417F1F53420751AD40CB613E40D08617
3FE5B43B4142EAEEC081A248C100966C
409F1515422979BD408A9CE04100F09A
41D277764226E840C1406CAFC0C0FACC
41B9DB5041068C5CBDB2DB29C12C4582
420CBB6441E59711C1532FB1C02EB636
*/