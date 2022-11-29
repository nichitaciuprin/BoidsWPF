#include <time.h>
#include "../BoidWorld/BoidWorld.h"
#include "../BoidWorldRender/BoidWorldRender.h"

void Wait(long milliseconds)
{
    if (milliseconds <= 0) return;
    float inFloat = ((float)milliseconds) / 1000;
    WaitTime(inFloat);
}
int main(void)
{
    BoidWorld game = BoidWorld_Init();
    BoidWorldRender_Init(&game);
    long timeStep = 20;
    while (!BoidWorldRender_ShouldExit())
    {
        long time1 = clock();
        BoidWorld_Update(&game,timeStep);
        BoidWorldRender_Update(&game);
        long time2 = clock();
        long diff = time2 - time1;
        long waitTime = timeStep - diff;
        Wait(waitTime);
    }
    BoidWorld_End(&game);
	return 0;
}