#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include "game.h"

int main(void)
{
    Game_Init();

    long time1 = clock();
    for (int i = 0; i < 2000; i++) Game_Update();
    long time2 = clock();

    printf("%li\n",time2-time1);

	Game_End();

	return 0;
}