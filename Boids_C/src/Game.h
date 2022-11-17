#include <stdio.h>
#include "Boid.h"

#define GAME_BOIDSCOUNT 300

AABB aabb;
Boid boids[GAME_BOIDSCOUNT];

__declspec( dllexport ) void Game_Init()
{
    Subgen_Init(0);

    aabb.p0 = (MyVector2){ 0, 0 };
    aabb.p1 = (MyVector2){ 50, 50 };
	for (int i = 0; i < GAME_BOIDSCOUNT; i++)
		boids[i] = CreateBoidRand(&aabb);
}
__declspec( dllexport ) void Game_End()
{
    PrintBoid(&boids[0]);
    printf(" \n");
}
__declspec( dllexport ) void Game_Update(long deltaTime)
{

}
__declspec( dllexport ) void Game_FixedUpdate(long deltaTime)
{
    float deltaTimeInSeconds = ((float)deltaTime)/1000;
    Boid_Update(boids,GAME_BOIDSCOUNT,&aabb,deltaTimeInSeconds);
}
