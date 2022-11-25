#include "..\Game\Game.h"
#include <raylib.h>
#include <raymath.h>
#include <rlgl.h>
#include <math.h>

int width = 500;
int height = 500;
float scale = 10;
MyVector2 position_1 = (MyVector2){ 0.00, 0.25};
MyVector2 position_2 = (MyVector2){-0.15,-0.25};
MyVector2 position_3 = (MyVector2){ 0.15,-0.25};

Vector2 rv(MyVector2 v) { return (Vector2) { v.x, v.y }; }
MyVector2 ToScreenSpace(MyVector2 v)
{
    return (MyVector2) { v.x, height-v.y };
}
void DrawTriangle2(MyVector2 v1, MyVector2 v2, MyVector2 v3, Color color)
{
    v1 = Vector2_Mul(v1,scale);
    v2 = Vector2_Mul(v2,scale);
    v3 = Vector2_Mul(v3,scale);
    v1 = ToScreenSpace(v1);
    v2 = ToScreenSpace(v2);
    v3 = ToScreenSpace(v3);
    DrawTriangle(rv(v1), rv(v2), rv(v3), color);
}
float GetAngle(MyVector2 v)
{
    return Vector2_IsZero(v) ? 0 : atan2f(v.y,v.x) - M_PI_2;
}
void DrawAABB(AABB* aabb)
{
    MyVector2 pos = (MyVector2) { AABB_MinX(aabb), AABB_MinY(aabb) };
    MyVector2 size = AABB_Size(aabb);
    pos = Vector2_Mul(pos,scale);
    size = Vector2_Mul(size,scale);
    pos = ToScreenSpace(pos);
    pos.y -= size.y;
    DrawRectangleLines(pos.x,pos.y,size.x,size.y,DARKGREEN);
}
void DrawBoid(Boid* boid, Color color)
{
    MyVector2 v1 = position_1;
    MyVector2 v2 = position_2;
    MyVector2 v3 = position_3;

    float angle = GetAngle(boid->vel);

    v1 = Vector2_Rotate(v1,angle);
    v2 = Vector2_Rotate(v2,angle);
    v3 = Vector2_Rotate(v3,angle);

    v1 = Vector2_Add(boid->pos,v1);
    v2 = Vector2_Add(boid->pos,v2);
    v3 = Vector2_Add(boid->pos,v3);

    DrawTriangle2(v1, v2, v3, color);
}
void Draw_Update(Game* game)
{
    BeginDrawing();
    ClearBackground(BLACK);
    DrawFPS(0,0);
    if (GAME_BOIDSCOUNT > 0)
    {
        DrawBoid(&game->boids[0], RED);
        for (int i = 1; i < game->boidsLength; i++)
            DrawBoid(&game->boids[i], WHITE);
    }
    DrawAABB(&game->aabb);
    EndDrawing();
}
void Draw_Init(Game* game)
{
    InitWindow(width, height, "BOIDS_C");
    ClearBackground(BLACK);
    SetWindowPosition(0,0);
	rlDisableBackfaceCulling();
    printf("\n"); // padding from Raylib print
    Draw_Update(game);
}
void Draw_End()
{
    CloseWindow();
}
bool Draw_ShouldExit()
{
    return WindowShouldClose();
}