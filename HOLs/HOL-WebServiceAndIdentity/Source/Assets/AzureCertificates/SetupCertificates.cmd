@echo off
%~d0
cd "%~dp0"

echo.
echo =========================
echo Relying Party certificate
echo =========================
echo.

SET relyingPartyUrl=""
SET /P relyingPartyUrl=Insert your Windows Azure hosted service domain (i.e.: foo.cloudapp.net) or press ENTER to skip: 
IF %relyingPartyUrl%=="" GOTO DEVFABRICCERT

SET certPassword=""
SET /P certPassword=Type a Password for your Relying Party Certificate or press ENTER to use the default one (Password=123456): 
IF %certPassword%=="" SET certPassword=123456
powershell %~dp0setupRpCertificate.ps1 "%relyingPartyUrl%" "%certPassword%"

GOTO DEVFABRICCERT

:DEVFABRICCERT
echo.
echo ===================================
echo Windows Azure DevFabric certificate
echo ===================================

powershell %~dp0setupAzureCertificate.ps1

GOTO FINISH

:FINISH
pause