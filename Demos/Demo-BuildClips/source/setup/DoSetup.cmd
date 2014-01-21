@ECHO OFF
SETLOCAL EnableDelayedExpansion
%~d0

CD "%~dp0"

SET powerShellDir=%WINDIR%\system32\windowspowershell\v1.0
echo.
echo ========= Setting PowerShell Execution Policy =========
%powerShellDir%\powershell.exe -NonInteractive -Command "Set-ExecutionPolicy unrestricted"
echo Setting Execution Policy Done!


REM Begin setup Local
call %powerShellDir%\powershell.exe -Command "& '%~dp0\tasks\show-consent-message.ps1' -SetupLocal "; exit $LASTEXITCODE
IF %ERRORLEVEL% == 1 GOTO exit
cls

call %powerShellDir%\powershell.exe -Command "& '%~dp0\tasks\show-config-xml-message.ps1' Config.Local.xml"; exit $LASTEXITCODE
IF %ERRORLEVEL% == 1 GOTO exit
cls

%powerShellDir%\powershell.exe -NonInteractive -command "%~dp0\setup.local.ps1" ".\Config.Local.xml"
REM End setup Local

:exit

echo.

@pause
