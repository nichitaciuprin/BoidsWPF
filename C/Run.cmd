@echo off
:: mwindows hides console
mingw64\bin\gcc.exe game\Game\Game.c ^
-lraylib ^
-ldloadhelper ^
-lglmf32 ^
-lm ^
-lopengl32 ^
-lgdi32 ^
-lwinmm ^
-o Boids.exe -g3 -O3 -Wall -std=c99 -Wno-missing-braces -pthread -m64 ^
-mwindows
:: stop on build fail
if %errorlevel% neq 0 exit /b %errorlevel%
Boids.exe
if %errorlevel% neq 0 echo ERROR!!!