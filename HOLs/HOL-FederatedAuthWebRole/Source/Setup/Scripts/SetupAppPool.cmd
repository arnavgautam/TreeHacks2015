@echo off

set IsW7=
(ver | findstr /C:"6.1") && set IsW7=true

@if "%IsW7%" == "true" (
	ECHO "Enabling LoadUserProfile for DefaultAppPool..."
	%windir%\system32\inetsrv\AppCmd set config -section:applicationPools /[name='DefaultAppPool'].processModel.loadUserProfile:true

	IF NOT %ERRORLEVEL%==0 GOTO ERROR
	
	IISReset
)

echo.
echo ======================================
echo Application Pool configured correctly.
echo ======================================
echo.
GOTO FINISH

:ERROR
echo.
echo ============================================================
echo An error has occured while configuring the Application Pool.
echo ============================================================
echo.

:FINISH

REM @pause