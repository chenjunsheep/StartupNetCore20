@echo off

cd /d %~dp0
call declare_variables.bat

cd..
if exist %pathScedule%\ rd /s /q %pathScedule%
if exist %pathWeb%\wwwroot\ rd /s /q %pathWeb%\wwwroot
if exist %pathServer%\wwwroot\ rd /s /q %pathServer%\wwwroot

echo =============^> cleaned up
@pause