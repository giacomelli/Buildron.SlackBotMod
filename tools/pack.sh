if [ "$1" == "mac" ]
then
  MODS_FOLDER="Buildron.app/Mods/"
  ZIP_FOLDER="../../../"
else
  MODS_FOLDER="Mods/"
  ZIP_FOLDER="../../"
fi

echo ================[ Packing Giacomelli.Buildron.SlackBotMod for $1
./build.sh $1

echo ================[ Compressing Giacomelli.Buildron.SlackBotMod
pushd $PWD/../build/$1/$MODS_FOLDER >/dev/null
zip -vrq $ZIP_FOLDER/Giacomelli.Buildron.SlackBotMod.$1.zip Giacomelli.Buildron.SlackBotMod -x "*.DS_Store" -x "*__instances__*" -x "*.pdb" -x "*.mdb"
popd >/dev/null

echo ================[ Giacomelli.Buildron.SlackBotMod package .zip available at build folder:
pushd ../build >/dev/null
ls Giacomelli.Buildron.SlackBotMod.$1.zip
popd >/dev/null

echo ================[ Giacomelli.Buildron.SlackBotMod pack done.
