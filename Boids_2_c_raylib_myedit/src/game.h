#ifndef  GAME_H
#define  GAME_H

#include "boid.h"

#define GAME_BOIDSCOUNT 128
typedef struct GameModel
{
    AABB aabb;
    Boid* boids[GAME_BOIDSCOUNT];
} GameModel;

void Game_Init();
void Game_Update(double deltaTime);
void Game_End();
GameModel* Game_GetModel();

#endif