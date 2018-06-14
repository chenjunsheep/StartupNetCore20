@echo off

echo =============^> clearing history files...
cd /d %~dp0
call declare_variables.bat

cd..
if exist %pathWeb%\wwwroot\ rd /s /q %pathWeb%\wwwroot
if exist %pathServer%\wwwroot\ rd /s /q %pathServer%\wwwroot
if exist %pathRelease%\ rd /s /q %pathRelease%

echo =============^> generating codes via swagger engine...
call %pathCodegen%\process.bat

echo =============^> restoring npm missing packages for %projectName% Web...
call %pathBat%\npm_update.bat
cd /d %~dp0
cd..

echo =============^> publishing %projectName% Web...
call %pathBat%\buildWeb.bat
cd /d %~dp0
cd..
xcopy %pathWeb%\wwwroot\*.* %pathServer%\wwwroot\ /s /y /i

echo =============^> %projectName% Web has been published

echo =============^> restoring nuget packages and publishing %projectName% Server...
call %pathBat%\buildServer.bat
cd /d %~dp0
cd..
xcopy %pathBat%\mysettings\*.* %pathRelease%\ /s /y /i

echo =============^> deleting temporary files...
if exist %pathWeb%\wwwroot\ rd /s /q %pathWeb%\wwwroot
if exist %pathServer%\wwwroot\ rd /s /q %pathServer%\wwwroot

echo =============^> %projectName% Server has been published
echo =============^>
echo =============^> Please find the published files at %cd%\%pathRelease%
echo =============^>