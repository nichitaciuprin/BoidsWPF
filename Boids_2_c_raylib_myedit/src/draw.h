#ifndef DRAW_H
#define DRAW_H





#include <stdio.h>
#include <raylib.h>
#include <rlgl.h>
#include "helper.h"
#include "aabb.h"
#include "boid.h"
#include "game.h"

void Draw_Update();
void Draw_Init();
void Draw_End();
bool Draw_ShouldExit();

#endif