@echo off

:: Check for 64-bit Framework
IF EXIST %WINDIR%\Microsoft.NET\Framework64 (
	set msbuild=%WINDIR%\Microsoft.NET\Framework64\v3.5\msbuild.exe
) ELSE (
	set msbuild=%WINDIR%\Microsoft.NET\Framework\v3.5\msbuild.exe
)


%msbuild% "%~dp0..\..\Assets\BlobsExplorer\BlobsExplorer.sln"

REM pause
:end