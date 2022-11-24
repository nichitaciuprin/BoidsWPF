@echo off
set name=Test
set filePath=%name%\%name%
del %filePath%.exe
gcc %filePath%.c ^
-lraylib ^
-ldloadhelper ^
-lglmf32 ^
-lm ^
-lopengl32 ^
-lgdi32 ^
-lwinmm ^
-o %filePath%.exe -g3 -O3 -Wall -std=c99 -Wno-missing-braces
%filePath%.exe