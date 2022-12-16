#include <time.h>
#include "..\GameWorld\GameWorld.h"

#define GB (long unsigned int)(1073741824)
#define MB (long unsigned int)(1048576)
#define KB (long unsigned int)(1024)
#define stacksize GB
// #define stacksize 65536
char stack [stacksize];

int main(void)
{
    for (size_t i = 0; i < stacksize; i++)
        stack[i] = 1;

    printf("%lu",stacksize);

    return 0;

    GameWorld* gameWorld = GameWorld_Init();

    long time1 = clock();
    for (int i = 0; i < 3000; i++) GameWorld_Update(gameWorld,20);
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