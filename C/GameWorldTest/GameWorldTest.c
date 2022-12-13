#include <time.h>
#include "..\GameWorld\GameWorld.h"

bool verbose = false;

int main(void)
{
    GameWorld* gameWorld = GameWorld_Init();

    long time1 = clock();
    // for (int i = 0; i < 58; i++)
    for (int i = 0; i < 3000; i++)
        GameWorld_Update(gameWorld,20);
    // verbose = true;
    // GameWorld_Update(gameWorld,20);
    long time2 = clock();

    printf("Boids_C");
    printf("  ");
    Boid_PrintBoid(&gameWorld->boids[0]);
    printf(" ");
    printf("%li",time2-time1);
    printf(" \n");

    GameWorld_End(gameWorld);

	return 0;
}