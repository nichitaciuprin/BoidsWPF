#define DEBUG
#include <time.h>
#include "Game.h"
#include "Draw.h"

void Wait(long milliseconds)
{
    if (milliseconds <= 0) return;
    float inSeconds = ((float)milliseconds) / 1000;
    WaitTime(inSeconds);
}
int main(void)
{
    // gameState = Game_Init();
    // long time1 = clock();
    // for (int i = 0; i < 3000; i++) Game_Update(&gameState,20);
    // long time2 = clock();
    // Game_End(&gameState);
    // printf("ms: %li\n",time2-time1);
	// return 0;

    GameState gameState = Game_Init();
    Draw_Init(&gameState);
    while (true)
    {
        if (Draw_ShouldExit()) break;

        long time1 = clock();
        Game_Update(&gameState,20);
        Draw_Update(&gameState);
        long time2 = clock();
        long diff = time2 - time1;
        Wait(20-diff);
    }
    Game_End(&gameState);
	return 0;
}