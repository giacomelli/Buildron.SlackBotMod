@echo off

if "%1" == "mac" (
  SET MODS_FOLDER=Buildron.app\Mods\
  SET ZIP_FOLDER=..\..\..\
  GOTO :PACK
)

SET MODS_FOLDER=Mods\
SET ZIP_FOLDER=..\..\

:PACK
echo ================[ Packing mod for %1
call build.cmd %1

echo ================[ Compressing mod
pushd "%cd%\..\build\%1\%MODS_FOLDER%"
7z a -tzip %ZIP_FOLDER%\Giacomelli.Buildron.SlackbotMod.%1.zip Giacomelli.Buildron.SlackbotMod -x!*.DS_Store -xr!*__instances__* -x!*.pdb -x!*.mdb
popd

echo ================[ Mod package .zip available at build folder:
pushd ..\build
dir *%1*.zip
popd

echo ================[ Mod pack done.
