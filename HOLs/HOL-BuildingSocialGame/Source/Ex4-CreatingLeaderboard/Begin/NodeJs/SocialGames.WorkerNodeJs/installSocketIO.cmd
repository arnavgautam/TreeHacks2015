@ECHO off
%~d0
CD "%~dp0"

IF EXIST %WINDIR%\SysWow64 (
set powerShellDir=%WINDIR%\SysWow64\windowspowershell\v1.0
) ELSE (
set powerShellDir=%WINDIR%\system32\windowspowershell\v1.0
)

ECHO Downloading npm..

CALL %powerShellDir%\powershell.exe -Command Set-ExecutionPolicy unrestricted
CALL %powerShellDir%\powershell.exe -Command "& .\DownloadAndExtractNPM.ps1 '%CD%'"

ECHO Done!

ECHO Installing socket.io..

CALL npm install socket.io

ECHO Done!