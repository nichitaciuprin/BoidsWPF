#include <time.h>
#include "..\GameWorld\GameWorld.h"


struct student
{
    int a;
    char b;
    char c;
};
int main(void)
{
    printf("%lli", 10*sizeof(double));
    return 0;
    //asm
    struct student stud1;
    // Displaying the size of the structure student.
    printf("%lli", sizeof(stud1));
    return 0;

    printf("%lli",sizeof(Boid) * 300);

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