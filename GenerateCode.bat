@echo off
dotnet run --project ./src/CodeGenerator/CodeGenerator.csproj -c Release
echo Generated code. Moving generated files to NativeFileDialogNET project
for %%f in (*.cs) do move /Y "%%f" "%~dp0src\NativeFileDialogNET\GeneratedCode\%%~nxf"
for %%f in (*.cpp) do del "%%f"
echo Everything ready. Press any key to close..
pause >nul