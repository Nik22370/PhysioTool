@echo off
REM L�scht alle tempor�ren Visual Studio Dateien

FOR %%d IN (Physiotool Physiotool/Physiotool.Application Physiotool/Physiotool.Webapi) DO (
    rd /S /Q "%%d/bin" 2> nul 
    rd /S /Q "%%d/obj" 2> nul
    rd /S /Q "%%d/.vs" 2> nul
    rd /S /Q "%%d/.vscode" 2> nul
)

FOR %%d IN (Physiotool/Physiotool.Client) DO (
  rd /S /Q "%%d/node_modules" 2> nul
)

