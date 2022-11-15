#define DEBUG
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include "GameEngine.h"
#include "Draw.h"

// int main(void)
// {
//     // Vector2_PrintVector2(result);
//     // printf("%f\n",length1);
//     // printf("%f\n",length2);

//     MainInit();

//     long time1 = clock();
//     for (int i = 0; i < 3000; i++) MainUpdate(20);
//     long time2 = clock();

// 	MainEnd();

//     printf("ms: %li\n",time2-time1);

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
    printf("\n"); // padding from Raylib print

    time2 = clock();
    deltaTime = time2 - time1;
    Warn(deltaTime);

	while (true)
    {
        if (Draw_ShouldExit()) break;

        time1 = clock();

		MainUpdate(deltaTime);
		Draw_Update(); // !!! MUST BE CALLED TO AVOID WINDOW FREEZE !!!

        time2 = clock();
        deltaTime = time2 - time1;
        Warn(deltaTime);
	}

	Game_End();
	Draw_End();

	return 0;
}
void Warn(long deltaTime) { printf("MainLoop is SLOW. %ld ms\n", deltaTime); }