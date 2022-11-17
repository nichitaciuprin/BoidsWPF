del main.exe
gcc src\main.c ^
-lraylib ^
-ldloadhelper ^
-lglmf32 ^
-lm ^
-lopengl32 ^
-lgdi32 ^
-lwinmm ^
-shared -o mylib.dll -g3 -O3 -Wall -std=c99 -Wno-missing-braces