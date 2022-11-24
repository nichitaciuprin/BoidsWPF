#define DEBUG
#include <time.h>
// #include "..\src\Game.h"
#include "Draw.h"

void Wait(long milliseconds)
{
    if (milliseconds <= 0) return;
    float inSeconds = ((float)milliseconds) / 1000;
    WaitTime(inSeconds);
}
int main(void)
{
    Game game = Game_Init();
    Draw_Init(&game);
    while (true)
    {
        if (Draw_ShouldExit()) break;

        long time1 = clock();
        Game_Update(&game,20);
        Draw_Update(&game);
        long time2 = clock();
        long diff = time2 - time1;
        Wait(20-diff);
    }
    Game_End(&game);
	return 0;
}