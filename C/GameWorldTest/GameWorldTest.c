#include <time.h>
#include "..\GameWorld\GameWorld.h"

// #define KB32 32768
// #define L1SIZE KB32

int main(void)
{
    GameWorld gameWorld = GameWorld_Init();

    // printf("%lli\n",sizeof(GameWorld));

    long time1 = clock();
    for (int i = 0; i < 3000; i++) GameWorld_Update(&gameWorld,20);
    long time2 = clock();

    printf("Boids_C");
    printf("  ");
    Boid_PrintBoid(&gameWorld.boids[0]);
    printf(" ");
    printf("%li",time2-time1);
    printf(" \n");

    GameWorld_End(&gameWorld);

	return 0;
}