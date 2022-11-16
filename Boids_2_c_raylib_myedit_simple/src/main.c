#define DEBUG
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include "GameEngine.h"
#include "Draw.h"

long oldTime = 0;
long newTime = 0;
long deltaTime = 0;

void CalcDeltaTime()
{
    newTime = clock();
    deltaTime = newTime - oldTime;
    oldTime = newTime;
}
int main(void)
{
    GameEngine_Init();
	Draw_Init();
	while (true)
    {
        if (Draw_ShouldExit()) break;
        CalcDeltaTime();
		GameEngine_Loop(deltaTime);
		Draw_Update(); // !!! MUST BE CALLED TO AVOID WINDOW FREEZE !!!
	}
	Draw_End();
	GameEngine_End();
    return 0;

    // Game_Init();
    // long time1 = clock();
    // for (int i = 0; i < 3000; i++) Game_FixedUpdate(20);
    // long time2 = clock();
    // Game_End();
    // printf("ms: %li\n",time2-time1);
	// return 0;
}