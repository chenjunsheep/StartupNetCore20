cd /d %~dp0
cd..
cd %pathServer%
dotnet publish -c Release -o %pathBin%\%projectName%