@echo off

REM Enable the Windows Update service (required by Web PI)
sc config wuauserv start= demand

ECHO "Starting PHP Installation" >> log.txt
"%~dp0Assets\webpicmd\WebPICmdLine.exe" /Products: PHP53 /log:phplog.txt /AcceptEula
ECHO "Completed PHP Installation" >> log.txt

REM Restore the Windows Update service start up type 
REM Do NOT enable the Windows Update service in a Windows Azure role
net stop wuauserv
sc config wuauserv start= disabled
