@echo off
echo %~f0
set cmdFileName=%~n0
set workspace=%cd%
set filePath=%cmdFileName%\%cmdFileName%
deps\mingw64\bin\gcc.exe %filePath%.c ^
-lraylib ^
-ldloadhelper ^
-lglmf32 ^
-lm ^
-lopengl32 ^
-lgdi32 ^
-lwinmm ^
-o %filePath%.exe -g3 -O3 -Wall -std=c99 -Wno-missing-braces -pthread -m64

@REM Stop cmd if build failed
if %errorlevel% neq 0 exit /b %errorlevel%

%filePath%.exe

if %errorlevel% neq 0 echo ERROR!!!