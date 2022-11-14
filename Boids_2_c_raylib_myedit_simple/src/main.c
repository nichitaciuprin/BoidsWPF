#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include "GameEngine.h"

int main(void)
{
    MainInit();

    long time1 = clock();
    for (int i = 0; i < 3000; i++) MainUpdate(20);
    long time2 = clock();

    printf("ms: %li\n",time2-time1);

	MainEnd();

	return 0;
}