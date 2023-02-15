@echo off

pushd Client
dotnet publish -c Release
popd

rmdir /s /q dist
mkdir ../cfx-server-data-master/server-data/resources/project

xcopy /y /e Client\bin\Release\net452\publish ..\cfx-server-data-master\server-data\resources\[system]\project\Client
xcopy /y /e ClientLib ..\cfx-server-data-master\server-data\resources\[system]\project\Client