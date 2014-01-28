@ECHO OFF

%windir%\system32\inetsrv\AppCmd.exe ADD app /site.name:"Default Web Site" /app.name:%1 /path:%1 /physicalPath:%2 > nul
IF %errorlevel%==183 GOTO EXISTS

GOTO END

:EXISTS

ECHO vdir %1 alread exists

:END