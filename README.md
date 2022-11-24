## Boids written in C#/WPF and C/Raylib

<!-- ## TODO. Create easy installation -->

### HOW TO RUN C#/WPF
1. install .NET 7 (winget install Microsoft.DotNet.SDK.7)
2. run RunBoids_C#.cmd

### HOW TO RUN FOR C/Raylib
1. Go here https://github.com/niXman/mingw-builds-binaries/releases
2. Download x86_64-12.2.0-release-win32-seh-rt_v10-rev0.7z
3. Extract to C:\mingw64
3. Extract set env path varibale to C:\mingw64\bin
3. Go here https://github.com/raysan5/raylib/releases/tag/4.2.0
4. Download "Source code (zip)"
5. Exctract
6. Run command. C:\raylib\src\nmingw32-make PLATFORM=PLATFORM_DESKTOP
7. Move libraylib.a to C:\mingw64\x86_64-w64-mingw32\lib
8. Move raylib.h    to C:\mingw64\x86_64-w64-mingw32\include
9. run RunBoids_C.cmd
