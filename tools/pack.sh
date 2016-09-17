if [ "$1" == "mac" ]
then
  MODS_FOLDER="Buildron.app/Mods/"
  ZIP_FOLDER="../../../"
else
  MODS_FOLDER="Mods/"
  ZIP_FOLDER="../../"
fi

echo ================[ Packing Giacomelli.Buildron.SlackBot for $1
./build.sh $1

echo ================[ Compressing Giacomelli.Buildron.SlackBot
pushd $PWD/../build/$1/$MODS_FOLDER >/dev/null
zip -vrq $ZIP_FOLDER/Giacomelli.Buildron.SlackBot.$1.zip Giacomelli.Buildron.SlackBot -x "*.DS_Store" -x "*__instances__*" -x "*.pdb" -x "*.mdb"
popd >/dev/null

echo ================[ Giacomelli.Buildron.SlackBot package .zip available at build folder:
pushd ../build >/dev/null
ls Giacomelli.Buildron.SlackBot.$1.zip
popd >/dev/null

echo ================[ Giacomelli.Buildron.SlackBot pack done.
