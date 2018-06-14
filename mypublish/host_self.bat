cd /d %~dp0
cd..
cd %pathServer%
dotnet run -c Release --launch-profile %lanuchProfile%