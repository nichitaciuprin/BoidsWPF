@echo off
set cmdFileName=%~n0
set filePath=%cmdFileName%\%cmdFileName%
dotnet run --project %filePath%.csproj --configuration Release