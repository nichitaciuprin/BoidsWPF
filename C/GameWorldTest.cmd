@echo off
set cmdFileName=%~n0
set filePath=%cmdFileName%\%cmdFileName%
@REM del %filePath%.exe
gcc %filePath%.c ^
-lraylib ^
-ldloadhelper ^
-lglmf32 ^
-lm ^
-lopengl32 ^
-lgdi32 ^
-lwinmm ^
-o %filePath%.exe -g3 -O3 -Wall -std=c99 -Wno-missing-braces -pthread

@REM Stop cmd if build failed
if %errorlevel% neq 0 exit /b %errorlevel%

%filePath%.exe

if %errorlevel% neq 0 echo ERROR!!!