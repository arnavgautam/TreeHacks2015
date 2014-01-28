@echo off
SET msbuild=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe

%msbuild% "%~dp0..\..\Assets\AcmBrowser\Source\AcmBrowser.sln"

REM pause
:end