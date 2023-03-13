@echo off

set thisFileName=%~n0
set root=%~f0\..\..
set gcc=%root%\deps\mingw64\bin\gcc.exe
set srcFile=%root%\src\%thisFileName%.c
set exeFile=%root%\build\%thisFileName%.exe
set include=%root%\include
set libs=-lraylib -ldloadhelper -lglmf32 -lm -lopengl32 -lgdi32 -lwinmm
set cfiles=%root%\src\MyVector.c %root%\src\AABB.c
set flags=-g3 -O3 -Wall -std=c99 -Wno-missing-braces -pthread -m64

mkdir %root%\build

%gcc% %srcFile% %cfiles% -I%include%\ %libs% -o %exeFile% %flags%

@REM Stop cmd if build failed
if %errorlevel% neq 0 exit /b %errorlevel%

%exeFile%

if %errorlevel% neq 0 echo ERROR!!!