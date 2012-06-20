@echo off
if not exist ..\Binaries mkdir ..\Binaries
devenv ArraySegments.sln /rebuild Release
.nuget\nuget.exe pack -sym ArraySegments.nuspec -o ..\Binaries
