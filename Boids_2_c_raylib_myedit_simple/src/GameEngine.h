#include "Game.h"

long realTime = 0;
long gameTime = 0;
long frameCount = 0;
const long updateTimeStep = 15; // ~60FPS;
const long fixedUpdateTimeStep = 20;
long updateTimer = updateTimeStep;
long fixedUpdateTimer = fixedUpdateTimeStep;

void Init()
{
    Game_Init();
}
void End()
{
    Game_End();
}
void Update(long deltaTime)
{
}
void FixedUpdate(long deltaTime)
{
    Game_Update();
}

void MainInit()
{
    Init();
}
void MainEnd()
{
    End();
}
void MainUpdate(long deltaTime)
{
    realTime += deltaTime;
    fixedUpdateTimer -= deltaTime;
    if (fixedUpdateTimer < 0)
    {
        deltaTime += fixedUpdateTimer;
        fixedUpdateTimer = 0;
    }
    updateTimer -= deltaTime;
    gameTime += deltaTime;

    if (deltaTime < 0) printf("!!!!!!!!!!!!  deltaTime < 0  \n");
    if (fixedUpdateTimer < 0) printf("!!!!!!!!!!!!  fixedUpdateTimer < 0  \n");

    if (updateTimer <= 0)
    {
        long time1 = clock();
        Update(updateTimeStep - updateTimer);
        long time2 = clock();
        long calcTime = time2 - time1;
        if (calcTime > updateTimeStep)
            printf("Update IS SLOW. %ld ms\n", calcTime);

        frameCount++;
        updateTimer = updateTimeStep;
    }
    if (fixedUpdateTimer == 0)
    {
        long time1 = clock();
        FixedUpdate(fixedUpdateTimeStep);
        long time2 = clock();
        long calcTime = time2 - time1;
        if (calcTime > fixedUpdateTimeStep)
            printf("Update IS SLOW. %ld ms\n", calcTime);

        fixedUpdateTimer = fixedUpdateTimeStep;
    }
}