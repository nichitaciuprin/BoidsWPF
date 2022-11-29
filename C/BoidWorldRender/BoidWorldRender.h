#ifndef BOIDWORLDRENDER_H
#define BOIDWORLDRENDER_H

#include "..\BoidWorld\BoidWorld.h"
#include "MyWindow.h"

MyVector2 BoidWorldRender_boidPoint_1 = (MyVector2) { 0.00f, 0.00f};
MyVector2 BoidWorldRender_boidPoint_2 = (MyVector2) {-0.25f,-0.25f};
MyVector2 BoidWorldRender_boidPoint_3 = (MyVector2) { 0.00f, 0.50f};
MyVector2 BoidWorldRender_boidPoint_4 = (MyVector2) { 0.25f,-0.25f};

void BoidWorldRender_DrawAABB(AABB* aabb)
{
    MyVector2 pos = (MyVector2) { AABB_MinX(aabb), AABB_MinY(aabb) };
    MyVector2 size = AABB_Size(aabb);
    MyWindow_DrawRectangleLines(pos.x,pos.y,size.x,size.y,DARKGREEN);
}
void BoidWorldRender_DrawBoid(Boid* boid, Color color)
{
    MyVector2 v1 = BoidWorldRender_boidPoint_1;
    MyVector2 v2 = BoidWorldRender_boidPoint_2;
    MyVector2 v3 = BoidWorldRender_boidPoint_3;
    MyVector2 v4 = BoidWorldRender_boidPoint_4;

    float angle = MyVector2_Angle(boid->vel);

    v1 = MyVector2_Rotate(v1,angle);
    v2 = MyVector2_Rotate(v2,angle);
    v3 = MyVector2_Rotate(v3,angle);
    v4 = MyVector2_Rotate(v4,angle);

    v1 = MyVector2_Add(boid->pos,v1);
    v2 = MyVector2_Add(boid->pos,v2);
    v3 = MyVector2_Add(boid->pos,v3);
    v4 = MyVector2_Add(boid->pos,v4);

    MyWindow_DrawTriangle(v1, v2, v3, color);
    MyWindow_DrawTriangle(v1, v4, v3, color);
}
void BoidWorldRender_Update(BoidWorld* game)
{
    MyWindow_BeginDrawing();

    if (BOIDWORLD_BOIDSCOUNT > 0)
    {
        BoidWorldRender_DrawBoid(&game->boids[0], RED);
        for (int i = 1; i < game->boidsLength; i++)
            BoidWorldRender_DrawBoid(&game->boids[i], WHITE);
    }
    BoidWorldRender_DrawAABB(&game->aabb);

    MyWindow_EndDrawing();
}
void BoidWorldRender_Init(BoidWorld* game)
{
    MyWindow_Init();
    BoidWorldRender_Update(game);
}
void BoidWorldRender_End()
{
    CloseWindow();
}
bool BoidWorldRender_ShouldExit()
{
    return MyWindow_ShouldClose();
}

#endif