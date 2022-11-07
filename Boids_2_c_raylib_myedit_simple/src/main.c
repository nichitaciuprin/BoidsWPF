#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include "game.h"

/*
C0F6CC3F *   41100000 / 410D9257   = C0FB07F7
C0F6CC3F * ( 41100000 / 410D9257 ) = C0FB07F9
*/

int main(void)
{
    Game_Init();

    long time1 = clock();
    // for (int i = 0; i < 18; i++) Game_Update(false);
    // for (int i = 0; i < 1; i++) Game_Update(true);
    for (int i = 0; i < 1000; i++) Game_Update(false);
    long time2 = clock();

    printf("%li\n",time2-time1);

	Game_End();

	return 0;
}