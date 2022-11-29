#define DEBUG
#include <time.h>
#include "..\BoidWorld\BoidWorld.h"

int main(void)
{
    BoidWorld game = BoidWorld_Init();

    long time1 = clock();
    for (int i = 0; i < 3000; i++) BoidWorld_Update(&game,20);
    long time2 = clock();

    printf("Boids_C");
    printf("  ");
    PrintBoid(&game.boids[0]);
    printf(" ");
    printf("%li",time2-time1);
    printf(" \n");

    BoidWorld_End(&game);

	return 0;
}