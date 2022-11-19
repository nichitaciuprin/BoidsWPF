#define DEBUG
#include <time.h>
#include "Game.h"

int main(void)
{
    GameState gameState = Game_Init();

    long time1 = clock();
    for (int i = 0; i < 3000; i++) Game_Update(&gameState,20);
    long time2 = clock();

    printf("Boids_C");
    printf("  ");
    PrintBoid(&gameState.boids[0]);
    printf(" ");
    printf("%li",time2-time1);
    printf(" \n");

    Game_End(&gameState);

	return 0;
}