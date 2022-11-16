#include "Game.h"

long realTime = 0;
long gameTime = 0;
long frameCount = 0;
const long timeStep_update = 15;
const long timeStep_fixedUpdate = 20;
long timer_update = timeStep_update;
long timer_fixedUpdate = timeStep_fixedUpdate;
bool countFrame = false;

void GameEngine_Init()
{
    Game_Init();
}
void GameEngine_End()
{
    Game_End();
}
void GameEngine_Loop(long deltaTime)
{
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
        Game_FixedUpdate(timeStep_fixedUpdate);
        long time2 = clock();
        long calcTime = time2 - time1;
        Helper_MaybeWarn("Game_FixedUpdate",calcTime,timeStep_fixedUpdate);
        countFrame = true;
    }
    if (timer_update <= 0)
    {
        timer_update = timeStep_update;
        long time1 = clock();
        Game_Update(timeStep_update - timer_update);
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