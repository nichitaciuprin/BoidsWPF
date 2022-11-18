#include "Game.h"
#include "Draw.h"

long realTime = 0;
long gameTime = 0;
long frameCount = 0;
const long timeStep_update = 15;
const long timeStep_fixedUpdate = 20;
long timer_fixedUpdate = timeStep_fixedUpdate;
bool shutdown = false;

GameState gameState;
GameState gameState_forRender;

bool GameEngine_ShutdownCalled() { return shutdown; }
void GameEngine_Init()
{
    gameState = Game_Init();
    gameState_forRender = gameState;
    Draw_Init(&gameState_forRender);
}
void GameEngine_End()
{
    Draw_End();
	Game_End(&gameState);
}
void GameEngine_Loop(long deltaTime) // realTime in milliseconds
{
    shutdown = Draw_ShouldExit();
    if (shutdown) return;
    if (deltaTime == 0) return;
    realTime += deltaTime;
    timer_fixedUpdate -= deltaTime;
    if (timer_fixedUpdate < 0)
    {
        deltaTime += timer_fixedUpdate;
        timer_fixedUpdate = 0;
    }
    gameTime += deltaTime;

#ifdef DEBUG
    if (deltaTime < 0) printf("!!!!!!!!!!!!  deltaTime < 0  \n");
#endif

    if (timer_fixedUpdate == 0)
    {
        timer_fixedUpdate = timeStep_fixedUpdate;
        long time1 = clock();
        Game_Update(&gameState, timeStep_fixedUpdate);
        gameState_forRender = gameState;
        long time2 = clock();
        long calcTime = time2 - time1;
        Helper_MaybeWarn("GameLogic",calcTime,timeStep_fixedUpdate);
    }
    else
    {
        Game_Update(&gameState_forRender, deltaTime);
        Draw_Update(&gameState_forRender);
    }

    frameCount++;
}