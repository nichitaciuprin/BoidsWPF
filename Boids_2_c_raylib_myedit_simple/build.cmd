del main.exe
gcc src\*.c -lraylib -ldloadhelper -lglmf32 -lm -lopengl32 -lgdi32 -lwinmm -o main.exe -g3 -O3 -Wall -std=c99 -Wno-missing-braces