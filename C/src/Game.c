#include <time.h>
#include "GameWorld.h"
#include "GameWorldWindow.h"
// #include "MyVector2.h"
#include "Raylib.h"

void Wait(long milliseconds)
{
    if (milliseconds <= 0) return;
    float inFloat = ((float)milliseconds) / 1000;
    WaitTime(inFloat);
}
int main(void)
{
    GameWorld gameWorld = GameWorld_Create();
    GameWorldWindow_Init(&gameWorld);
    long timeStep = 20;
    while (true)
    {
        if (GameWorldWindow_ShouldExit()) break;
        long time1 = clock();
        GameWorld_Update(&gameWorld,timeStep);
        GameWorldWindow_Update(&gameWorld);
        long time2 = clock();
        long diff = time2 - time1;
        long waitTime = timeStep - diff;
        Wait(waitTime);
    }
	return 0;
}