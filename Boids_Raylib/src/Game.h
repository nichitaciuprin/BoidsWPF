#include "Boid.h"

#define GAME_BOIDSCOUNT 300

typedef struct GameState
{
    AABB aabb;
    Boid boids[GAME_BOIDSCOUNT];
    int boidsLength;
} GameState;

GameState Game_Init()
{
    Subgen_Init(0);

    GameState gameState;
    gameState.boidsLength = GAME_BOIDSCOUNT;
    gameState.aabb.p0 = (MyVector2){ 0, 0 };
    gameState.aabb.p1 = (MyVector2){ 50, 50 };
	for (int i = 0; i < GAME_BOIDSCOUNT; i++)
		gameState.boids[i] = CreateBoidRand(&gameState.aabb);
    return gameState;
}
void Game_End(GameState* gameState)
{
    PrintBoid(&gameState->boids[0]);
    printf(" \n");
}
void Game_Update(GameState* gameState, long realDeltaTimeInMilliseconds)
{
    float deltaTimeInSeconds = ((float)realDeltaTimeInMilliseconds)/1000;
    Boid_Update(gameState->boids,gameState->boidsLength,&gameState->aabb,deltaTimeInSeconds);
}
