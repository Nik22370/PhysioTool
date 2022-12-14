@echo off
FOR %%d IN (Physiotool Physiotool/Physiotool.Application Physiotool/Physiotool.Webapi) DO (
    rd /S /Q "%%d/bin" 2> nul 
    rd /S /Q "%%d/obj" 2> nul
    rd /S /Q "%%d/.vs" 2> nul
    rd /S /Q "%%d/.vscode" 2> nul
)

cd Physiotool
cd Physiotool.Webapi
dotnet watch run -c Debug
