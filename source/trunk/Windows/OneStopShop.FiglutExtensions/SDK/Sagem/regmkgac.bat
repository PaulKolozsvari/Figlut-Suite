@echo off

cd /D "%MorphoKit%\bin"

REM Search for the .NET 2.0 SDK
set NetSDK2RegistryKey="HKLM\SOFTWARE\Microsoft\Microsoft SDKs\.NETFramework\v2.0"
for /F "usebackq tokens=2*" %%A in (
	`REG.EXE QUERY %NetSDK2RegistryKey% /V "InstallationFolder" ^2^>NUL ^| FIND "REG_SZ"`
) do (
	set NetSDK2Dir=%%B
)

if exist "%SDK_GAC_PATH%\gacutil.exe" (
	set "PATH=%Path%;%SDK_GAC_PATH%"
	goto :DoWork
)

REM Previous test should have worked in every case but we keep the default paths backup tests
REM ========================= DEFAULT PATH BACKUP TEST
REM Test if 64-bit OS version
set ProgRoot=%ProgramFiles%

if not "%ProgramFiles(x86)%" == "" (
	set "ProgRoot=%ProgramFiles(x86)%"
)

set "VS2008_GAC_PATH=%ProgRoot%\Microsoft SDKs\Windows\v6.0A\bin"
set "VS2005_GAC_PATH=%ProgRoot%\Microsoft Visual Studio 8\SDK\v2.0\Bin"
set "SDK_GAC_PATH=%ProgRoot%\Microsoft.NET\SDK\v2.0\Bin"
REM Visual Studio 2010 use same path as 2008

echo %SDK_GAC_PATH%

if exist "%SDK_GAC_PATH%\gacutil.exe" (
	set "PATH=%Path%;%SDK_GAC_PATH%"
	goto :DoWork
)
if exist "%VS2005_GAC_PATH%\gacutil.exe" (
	set "PATH=%Path%;%VS2005_GAC_PATH%"
	goto :DoWork
)
if exist "%VS2008_GAC_PATH%\gacutil.exe" (
	set "PATH=%Path%;%VS2008_GAC_PATH%"
	goto :DoWork
)
REM ========================= DEFAULT PATH BACKUP TEST END

goto :NotExist

:NotExist
echo gacutil is not available in your configuration.
echo.
echo gacutil is available in .NET Framework 2.0 SDK.
echo.
echo This SDK is provided as a standalone package by Microsoft
echo and is included in Visual Studio 2005 and 2008.
echo You must install either of them to be able to register MorphoKit in GAC.
goto :End

:DoWork
if not "%1"=="/u" (
	gacutil /i Sagem.MorphoKit.dll
	gacutil /i Sagem.ImageLibrary.dll
	gacutil /i Sagem.MorphoKit.AcquisitionComponent.dll
	gacutil /i ActiveMKit_Enroll.dll
	gacutil /i Morpho.MorphoKit_FVP.dll
	gacutil /i Morpho.MorphoAcquisition.dll
	echo.
	echo Installation in Global Assembly Cache successful
) else (
	gacutil /u Morpho.MorphoAcquisition.dll
	gacutil /u Morpho.MorphoKit_FVP.dll
	gacutil /u ActiveMKit_Enroll.dll
	gacutil /u Sagem.MorphoKit.AcquisitionComponent.dll
	gacutil /u Sagem.ImageLibrary.dll
	gacutil /u Sagem.MorphoKit.dll
	echo.
	echo Uninstallation from Global Assembly Cache successful
)

:End
pause
