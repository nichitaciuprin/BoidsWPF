#include <stdlib.h>
#include <math.h>
#include <raylib.h>
#include <raymath.h>
#include "aabb.h"
#include "boid.h"
#include "game.h"

GameModel model;

void Game_Init()
{
    SetRandomSeed(0);
    model.aabb.p0 = (Vector2){ 0, 0 };
    model.aabb.p1 = (Vector2){ 50, 50 };
	for (int i = 0; i < GAME_BOIDSCOUNT; i++)
		model.boids[i] = CreateBoidRand(model.aabb);
}
void Game_Update(double deltaTime)
{
    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
        UpdateBoid(model.boids[i], model.boids, GAME_BOIDSCOUNT, model.aabb, deltaTime);
}
void Game_End()
{
    for (int i = 0; i < GAME_BOIDSCOUNT; i++)
		FreeBoid(model.boids[i]);
}
GameModel* Game_GetModel() { return &model; }