#define DEBUG
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include "GameEngine.h"

int main(void)
{
    long oldTime = 0;
    long newTime = 0;
    long deltaTime = 0;
    GameEngine_Init();
	while (true)
    {
        if (GameEngine_ShutdownCalled()) break;
        newTime = clock();
        deltaTime = newTime - oldTime;
        oldTime = newTime;
		GameEngine_Loop(deltaTime);
	}
	GameEngine_End();
    return 0;

    // gameState = Game_Init();
    // long time1 = clock();
    // for (int i = 0; i < 3000; i++) Game_Update(&gameState,20);
    // long time2 = clock();
    // Game_End(&gameState);
    // printf("ms: %li\n",time2-time1);
	// return 0;
}