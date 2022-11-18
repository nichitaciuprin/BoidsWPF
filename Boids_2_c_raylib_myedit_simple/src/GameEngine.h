#include "Game.h"
#include "Draw.h"

long realTime = 0;
long gameTime = 0;
long frameCount = 0;
const long timeStep_update = 15;
const long timeStep_fixedUpdate = 20;
long timer_update = timeStep_update;
long timer_fixedUpdate = timeStep_fixedUpdate;
bool countFrame = false;
bool shutdown = false;

GameState gameState;

bool GameEngine_ShutdownCalled() { return shutdown; }
void GameEngine_Init()
{
    gameState = Game_Init();
    Draw_Init(&gameState);
}
void GameEngine_End()
{
    Draw_End();
	Game_End(&gameState);
}
void GameEngine_Loop(long deltaTime) // realTime in milliseconds
{
    shutdown = WindowShouldClose();
    if (shutdown) return;
    if (deltaTime == 0) return;
    realTime += deltaTime;
    timer_fixedUpdate -= deltaTime;
    if (timer_fixedUpdate < 0)
    {
        deltaTime += timer_fixedUpdate;
        timer_fixedUpdate = 0;
    }
    timer_update -= deltaTime;
    gameTime += deltaTime;
#ifdef DEBUG
    if (deltaTime < 0) printf("!!!!!!!!!!!!  deltaTime < 0  \n");
#endif

    if (timer_fixedUpdate == 0)
    {
        timer_fixedUpdate = timeStep_fixedUpdate;
        long time1 = clock();
        Game_Update(&gameState, timeStep_fixedUpdate);
        long time2 = clock();
        long calcTime = time2 - time1;
        Helper_MaybeWarn("Game_FixedUpdate",calcTime,timeStep_fixedUpdate);
        countFrame = true;
    }
    if (timer_update <= 0)
    {
        timer_update = timeStep_update;
        long time1 = clock();
        Draw_Update(&gameState);
        // Game_Update(timeStep_update - timer_update);
        long time2 = clock();
        long calcTime = time2 - time1;
        Helper_MaybeWarn("Game_Update",calcTime,timeStep_update);
        countFrame = true;
    }

    if (countFrame)
    {
        frameCount++;
        countFrame = false;
    }
}