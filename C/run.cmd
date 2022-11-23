set name=run
del %name%.exe
gcc %name%.c ^
-lraylib ^
-ldloadhelper ^
-lglmf32 ^
-lm ^
-lopengl32 ^
-lgdi32 ^
-lwinmm ^
-o %name%.exe -g3 -O3 -Wall -std=c99 -Wno-missing-braces
%name%.exe