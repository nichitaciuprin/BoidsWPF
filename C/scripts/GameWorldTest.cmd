@echo off

set thisFileName=%~n0
set root=%~f0\..\..
set gcc=%root%\deps\mingw64\bin\gcc.exe
set srcFile=%root%\src\%thisFileName%.c
set exeFile=%root%\build\%thisFileName%.exe
set include=%root%\include

%gcc% %srcFile% -I%include%\ ^
-lraylib      ^
-ldloadhelper ^
-lglmf32      ^
-lm           ^
-lopengl32    ^
-lgdi32       ^
-lwinmm       ^
-o %exeFile% -g3 -O3 -Wall -std=c99 -Wno-missing-braces -pthread -m64

@REM Stop cmd if build failed
if %errorlevel% neq 0 exit /b %errorlevel%

%exeFile%

if %errorlevel% neq 0 echo ERROR!!!