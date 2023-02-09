@echo off
set /p MigrationName=Name: 

dotnet ef migrations add %MigrationName%

pause