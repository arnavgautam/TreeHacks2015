@echo off

:: Check for 64-bit Framework
IF EXIST %WINDIR%\Microsoft.NET\Framework64 (
	set msbuild=%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe

) ELSE (
	set msbuild=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe


)

%msbuild% "%~dp0..\..\Assets\LegacyCOM\LegacyCOM.sln" /p:configuration=release /p:platform=x64

%msbuild% "%~dp0..\..\Assets\LegacyCOM\LegacyCOM.sln" /p:configuration=release /p:platform=win32


:: Copy the files for 64-bit and 32bits

Copy "%~dp0..\..\Assets\LegacyCOM\amd64\LegacyCOM.dll" "%~dp0..\..\Ex2-StartupTasks\CS\Begin\SampleWebApp\amd64\"
Copy "%~dp0..\..\Assets\LegacyCOM\x86\LegacyCOM.dll" "%~dp0..\..\Ex2-StartupTasks\CS\Begin\SampleWebApp\x86\"

Copy "%~dp0..\..\Assets\LegacyCOM\amd64\LegacyCOM.dll" "%~dp0..\..\Ex2-StartupTasks\CS\End\SampleWebApp\amd64\"
Copy "%~dp0..\..\Assets\LegacyCOM\x86\LegacyCOM.dll" "%~dp0..\..\Ex2-StartupTasks\CS\End\SampleWebApp\x86\"


REM pause
:end