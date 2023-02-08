@echo off
pushd Client
dotnet publish -c Release
popd

pushd Server
dotnet publish -c Release
popd

rmdir /s /q dist
mkdir ../cfx-server-data-master/server-data/resources/project

copy /y fxmanifest.lua ..\cfx-server-data-master\server-data\resources\[system]\project
xcopy /y /e Client\bin\Release\net452\publish ..\cfx-server-data-master\server-data\resources\[system]\project\Client\bin\Release\net452\publish\
xcopy /y /e Server\bin\Release\netstandard2.0\publish ..\cfx-server-data-master\server-data\resources\[system]\project\Server\bin\Release\netstandard2.0\publish\

@pause