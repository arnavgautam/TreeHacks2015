msiexec /qn /i "%~dp0capicom_dc_sdk.msi"


IF EXIST "%PROGRAMFILES%\Microsoft CAPICOM 2.1.0.2 SDK" (
    SET capicompath="%PROGRAMFILES%\Microsoft CAPICOM 2.1.0.2 SDK\Samples\vbs\cstore.vbs"
    SET cscript=%windir%\system32\cscript.exe
)

IF EXIST "%PROGRAMFILES(x86)%\Microsoft CAPICOM 2.1.0.2 SDK" (
    SET capicompath="%PROGRAMFILES(x86)%\Microsoft CAPICOM 2.1.0.2 SDK\Samples\vbs\cstore.vbs"
    SET cscript=%windir%\syswow64\cscript.exe

    ECHO Setting up CAPICOM for 64 bits environment...
    copy /y "%PROGRAMFILES(x86)%\Microsoft CAPICOM 2.1.0.2 SDK\Lib\X86\capicom.dll" %windir%\syswow64
    %windir%\syswow64\regsvr32.exe /s %windir%\syswow64\capicom.dll
)


%cscript% /nologo %capicompath% import -l LM "%~dp0%1" "%2"