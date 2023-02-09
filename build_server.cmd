@echo off

pushd Server
dotnet publish -c Release
popd

rmdir /s /q dist
mkdir ../cfx-server-data-master/server-data/resources/project

xcopy /y /e Server\bin\Release\netstandard2.0\publish ..\cfx-server-data-master\server-data\resources\[system]\project\Server\
@pause