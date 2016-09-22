@echo off

if "%1" == "linux" (
  SET BUILD_TARGET=StandaloneLinux
  GOTO :BUILD
)

if "%1" == "mac" (
  SET BUILD_TARGET=StandaloneOSXIntel
  GOTO :BUILD
)

if "%1" == "win" (
  SET BUILD_TARGET=StandaloneWindows
  GOTO :BUILD
)

echo ERROR: invalid platform. Valid are: linux, mac or win.
GOTO :EXIT

:BUILD
echo ================[ Building mod for %1
echo ================[ Compiling mod C# class library...
C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild ../src/Code/Giacomelli.Buildron.SlackbotMod.sln /verbosity:quiet /t:rebuild /p:Configuration=%1

echo ================[ Starting mod asset building...
@"C:\Program Files\Unity\Editor\Unity.exe" -projectPath %cd%\..\src\Unity\Giacomelli.Buildron.SlackbotMod -quit -batchmode -executeMethod ModBuilder.BuildFromCommandLine %cd%\..\build %BUILD_TARGET%

echo ================[  Build mod done.

:EXIT
