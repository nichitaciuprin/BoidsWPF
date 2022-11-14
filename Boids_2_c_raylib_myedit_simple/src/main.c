#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include "GameEngine.h"
#include "Draw.h"

// int main(void)
// {
//     MainInit();

//     long time1 = clock();
//     for (int i = 0; i < 3000; i++) MainUpdate(20);
//     long time2 = clock();

//     printf("ms: %li\n",time2-time1);

// 	MainEnd();

// 	return 0;
// }

int main(void)
{
    long time1 = 0;
    long time2 = 0;
    long deltaTime = 0;

    time1 = clock();
    MainInit();
	Draw_Init();
    time2 = clock();
    deltaTime = time2 - time1;

	while (true)
    {
        if (Draw_ShouldExit()) break;

        time1 = clock();
		MainUpdate(deltaTime);
		Draw_Update(); // !!! MUST BE CALLED TO AVOID WINDOW FREEZE !!!
        time2 = clock();
        deltaTime = time2 - time1;
        if (deltaTime > 20)
            printf("MainLoop is SLOW. %ld ms\n", deltaTime);
	}

	Game_End();
	Draw_End();

	return 0;
}