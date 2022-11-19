#include "Boid.h"

#define GAME_BOIDSCOUNT 300

typedef struct Game
{
    AABB aabb;
    Boid boids[GAME_BOIDSCOUNT];
    int boidsLength;
} Game;

Game Game_Init()
{
    Subgen_Init(0);

    Game gameState;
    gameState.boidsLength = GAME_BOIDSCOUNT;
    gameState.aabb.p0 = (MyVector2){ 0, 0 };
    gameState.aabb.p1 = (MyVector2){ 50, 50 };
	for (int i = 0; i < GAME_BOIDSCOUNT; i++)
		gameState.boids[i] = CreateBoidRand(&gameState.aabb);
    return gameState;
}
void Game_End(Game* gameState)
{
}
void Game_Update(Game* gameState, long realDeltaTimeInMilliseconds)
{
    float deltaTimeInSeconds = ((float)realDeltaTimeInMilliseconds)/1000;
    Boid_Update(gameState->boids,gameState->boidsLength,&gameState->aabb,deltaTimeInSeconds);
}
