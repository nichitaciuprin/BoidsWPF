#include "..\Game\Game.h"
#include "MyWindow.h"

MyVector2 Draw_boidPoint_1 = (MyVector2) { 0.00f, 0.00f};
MyVector2 Draw_boidPoint_2 = (MyVector2) {-0.25f,-0.25f};
MyVector2 Draw_boidPoint_3 = (MyVector2) { 0.00f, 0.50f};
MyVector2 Draw_boidPoint_4 = (MyVector2) { 0.25f,-0.25f};

void Draw_DrawAABB(AABB* aabb)
{
    MyVector2 pos = (MyVector2) { AABB_MinX(aabb), AABB_MinY(aabb) };
    MyVector2 size = AABB_Size(aabb);
    MyWindow_DrawRectangleLines(pos.x,pos.y,size.x,size.y,DARKGREEN);
}
void Draw_DrawBoid(Boid* boid, Color color)
{
    MyVector2 v1 = Draw_boidPoint_1;
    MyVector2 v2 = Draw_boidPoint_2;
    MyVector2 v3 = Draw_boidPoint_3;
    MyVector2 v4 = Draw_boidPoint_4;

    float angle = Vector2_Angle(boid->vel);

    v1 = Vector2_Rotate(v1,angle);
    v2 = Vector2_Rotate(v2,angle);
    v3 = Vector2_Rotate(v3,angle);
    v4 = Vector2_Rotate(v4,angle);

    v1 = Vector2_Add(boid->pos,v1);
    v2 = Vector2_Add(boid->pos,v2);
    v3 = Vector2_Add(boid->pos,v3);
    v4 = Vector2_Add(boid->pos,v4);

    MyWindow_DrawTriangle(v1, v2, v3, color);
    MyWindow_DrawTriangle(v1, v4, v3, color);
}
void Draw_Update(Game* game)
{
    MyWindow_BeginDrawing();

    if (GAME_BOIDSCOUNT > 0)
    {
        Draw_DrawBoid(&game->boids[0], RED);
        for (int i = 1; i < game->boidsLength; i++)
            Draw_DrawBoid(&game->boids[i], WHITE);
    }
    Draw_DrawAABB(&game->aabb);

    MyWindow_EndDrawing();
}
void Draw_Init(Game* game)
{
    MyWindow_Init();
    Draw_Update(game);
}
void Draw_End()
{
    CloseWindow();
}
bool Draw_ShouldExit()
{
    return MyWindow_ShouldClose();
}