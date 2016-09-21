if [ "$1" == "linux" ]
then
  BUILD_TARGET="StandaloneLinux"
  MODS_FOLDER="Mods/"
elif [ "$1" == "mac" ]
then
  BUILD_TARGET="StandaloneOSXIntel"
  MODS_FOLDER="Buildron.app/Mods/"
elif [ "$1" == "win" ]
then
  BUILD_TARGET="StandaloneWindows"
  MODS_FOLDER="Mods/"
else
  echo "ERROR: invalid platform. Valid are: linux, mac or win."
  exit
fi

echo ================[ Building Giacomelli.Buildron.SlackBotMod for $1
echo ================[ Compiling Giacomelli.Buildron.SlackBotMod C# class library...
xbuild ../src/Code/Giacomelli.Buildron.SlackBotMod.sln /verbosity:quiet /t:rebuild /p:Configuration=$1 >/dev/null

echo ================[ Starting Giacomelli.Buildron.SlackBotMod asset building...
/Applications/Unity/Unity.app/Contents/MacOS/Unity -projectPath $PWD/../src/Unity/Giacomelli.Buildron.SlackBotMod -quit -batchmode -executeMethod ModBuilder.BuildFromCommandLine $PWD/../build/$1/$MODS_FOLDER $BUILD_TARGET

#echo 'Logs from build'
#cat ~/Library/Logs/Unity/Editor.log

echo ================[  Build Giacomelli.Buildron.SlackBotMod done.
