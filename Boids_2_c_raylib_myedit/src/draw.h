#include <stdio.h>
#include <raylib.h>
#include <rlgl.h>

int width = 500;
int height = 500;
float scale = 10;
Vector2 position_1 = (Vector2){ 0.00, 0.25};
Vector2 position_2 = (Vector2){-0.15,-0.25};
Vector2 position_3 = (Vector2){ 0.15,-0.25};

void Draw_Update();
void Draw_Init();
void Draw_End();
bool Draw_ShouldExit();

Vector2 ToScreenSpace(Vector2 v)
{
    return (Vector2) { v.x, height-v.y };
}
void DrawTriangle2(Vector2 v1, Vector2 v2, Vector2 v3, Color color)
{
    v1 = HelperMul(v1,scale);
    v2 = HelperMul(v2,scale);
    v3 = HelperMul(v3,scale);
    v1 = ToScreenSpace(v1);
    v2 = ToScreenSpace(v2);
    v3 = ToScreenSpace(v3);
    DrawTriangle(v1, v2, v3, color);
}
float GetAngle(Vector2 v)
{
    return HelperIsZero(v) ? 0 : atan2f(v.y,v.x) - M_PI_2;
}
void DrawAABB(AABB aabb)
{
    Vector2 pos = (Vector2) { AABB_MinX(aabb), AABB_MinY(aabb) };
    Vector2 size = AABB_Size(aabb);
    pos = HelperMul(pos,scale);
    size = HelperMul(size,scale);
    pos = ToScreenSpace(pos);
    pos.y -= size.y;
    DrawRectangleLines(pos.x,pos.y,size.x,size.y,DARKGREEN);
}
void DrawBoid(Boid* boid, Color color)
{
    Vector2 v1 = position_1;
    Vector2 v2 = position_2;
    Vector2 v3 = position_3;

    float angle = GetAngle(boid->velocity);

    v1 = Vector2Rotate(v1,angle);
    v2 = Vector2Rotate(v2,angle);
    v3 = Vector2Rotate(v3,angle);

    v1 = HelperAdd(boid->position,v1);
    v2 = HelperAdd(boid->position,v2);
    v3 = HelperAdd(boid->position,v3);

    DrawTriangle2(v1, v2, v3, color);
}

void Draw_Update()
{
    BeginDrawing();
    ClearBackground(BLACK);

    // Vector2 v1 = (Vector2) {  0,  0};
    // Vector2 v2 = (Vector2) { 10,  0};
    // Vector2 v3 = (Vector2) { 10, 10};
    // DrawTriangle2(v1, v2, v3, RED);

    GameModel* model = Game_GetModel();

    Boid** boids = model->boids;
    if (GAME_BOIDSCOUNT > 0)
    {
        DrawBoid(boids[0], RED);
        // printf("%f\n",boids[0]->velocity.y);
        for (int i = 1; i < GAME_BOIDSCOUNT; i++)
            DrawBoid(boids[i], WHITE);
    }

    DrawAABB(model->aabb);

    EndDrawing();
}
void Draw_Init()
{
    InitWindow(width, height, "WindowName");
    ClearBackground(BLACK);
    SetWindowPosition(0,0);
	rlDisableBackfaceCulling();
	SetTargetFPS(60);
    Draw_Update();
}
void Draw_End()
{
    CloseWindow();
}
bool Draw_ShouldExit()
{
    return WindowShouldClose();
}