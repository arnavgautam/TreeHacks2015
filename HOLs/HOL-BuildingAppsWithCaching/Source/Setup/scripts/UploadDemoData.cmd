@echo off

setlocal 
%~d0
cd "%~dp0"

echo.
echo =======================================
echo Windows Azure Storage credentials setup
echo =======================================
echo.

IF EXIST %WINDIR%\SysWow64 (
set powerShellDir=%WINDIR%\SysWow64\windowspowershell\v1.0
) ELSE (
set powerShellDir=%WINDIR%\system32\windowspowershell\v1.0
)

CHOICE /C YN /D Y /T 10 /M "Would you like to provide your Windows Azure Storage credentials? Otherwise Storage Emulator will be used (remember to start the emulator if you need to)"
IF ERRORLEVEL 2 GOTO devstorage


SET /p AccountName=Please enter your account name: 
SET /p AccountKey=Please enter your account key: 
call %powerShellDir%\powershell.exe -Command Set-ExecutionPolicy unrestricted
call %powerShellDir%\powershell.exe -Command "&'.\UploadDemoData.ps1' -AccountName %AccountName% -AccountKey %AccountKey%
GOTO exit


:devstorage
call %powerShellDir%\powershell.exe -Command Set-ExecutionPolicy unrestricted
call %powerShellDir%\powershell.exe -Command "&'.\UploadDemoData.ps1' -UseDevStorage
GOTO exit

:exit