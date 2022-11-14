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

    // Debug.Assert(deltaTime >= 0);
    // Debug.Assert(fixedUpdateTimer >= 0);

    if (updateTimer <= 0)
    {
        // updateWatch.Restart();
        Update(updateTimeStep - updateTimer);
        // updateWatch.Stop();

        // var calcTime = updateWatch.ElapsedTicks;
        // if (calcTime > updateTimeStep)
        //     System.Console.WriteLine($"Update IS SLOW {Helper.ToMilliseconds(calcTime)}ms");

        frameCount++;
        updateTimer = updateTimeStep;
    }
    if (fixedUpdateTimer == 0)
    {
        // fixedUpdateWatch.Restart();
        FixedUpdate(fixedUpdateTimeStep);
        // fixedUpdateWatch.Stop();

        fixedUpdateTimer = fixedUpdateTimeStep;

        // var calcTime = fixedUpdateWatch.ElapsedTicks;
        // if (calcTime > fixedUpdateTimeStep)
        //     System.Console.WriteLine($"FixedUpdate IS SLOW {Helper.ToMilliseconds(calcTime)}ms");
    }
}