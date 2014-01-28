@echo off

SET subjectName=%1
SET storeName=%2
SET storeLocation=%3

IF EXIST %WINDIR%\SysWow64 (
	CALL "%PROGRAMFILES(x86)%\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" > nul
) ELSE (
	CALL "%PROGRAMFILES%\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" > nul
)

CALL makecert -a sha1 -n CN=%subjectName% -pe -r -sky exchange -ss %storeName% -sr %storeLocation%

REM Setup cetificates permissions to run locally in IIS (in a real Azure scenario this is not needed)

set IsWinClient=false
set IsW7=
(ver | findstr /C:"6.1") && set IsW7=true

@if "%IsW7%" == "true" (
	"%~dp0winhttpcertcfg.exe" -g -c LOCAL_MACHINE\My -s %subjectName% -a "IIS_IUSRS"
	set IsWinClient=true
) else (
	"%~dp0winhttpcertcfg.exe" -g -c LOCAL_MACHINE\My -s %subjectName% -a "NETWORK SERVICE"
)