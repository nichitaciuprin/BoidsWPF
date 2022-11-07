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
    for (int i = 0; i < 2; i++) Game_Update(false);
    long time2 = clock();

    printf("%li\n",time2-time1);

	Game_End();

	return 0;
}
/*
3FF3C65B 41964E32 410F4FF3 41258155
-------
0
3FF3C65A 41964E33 410F4FEA 41258157
*/