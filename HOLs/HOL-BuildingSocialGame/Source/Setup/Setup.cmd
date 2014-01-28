@ECHO off
%~d0
CD "%~dp0"

ECHO Install Visual Studio 2010 Code Snippets for the lab:
ECHO -------------------------------------------------------------------------------
CALL .\scripts\InstallCodeSnippets.cmd
ECHO Done!
ECHO.
ECHO *******************************************************************************
ECHO.
@PAUSE
