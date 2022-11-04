#include "game.h"
#include "draw.h"
#include <stdio.h>
#include <stdlib.h>
// #include "subgen.h"

int main(void)
{
    unsigned int frame = 0;
    double deltaTime = 0;

    Game_Init();
	Draw_Init();

	while (true)
    {
        if (Draw_ShouldExit()) break;
        double time1 = GetTime();
		Game_Update(deltaTime);
		Draw_Update(); /* MUST BE CALLED TO AVOID WINDOW FREEZE */
        double time2 = GetTime();
        deltaTime = time2 - time1;
        frame++;
	}

	Game_End();
	Draw_End();

	return 0;
}