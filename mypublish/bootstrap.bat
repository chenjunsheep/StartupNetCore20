@echo off
echo =============^> initialize environment...
cd /d %~dp0
call declare_variables.bat

cd /d %~dp0
cd..
if exist %pathScedule%\ rd /s /q %pathScedule%\
xcopy %pathBat%\declare_variables.bat %pathScedule%\ /s /y /i
xcopy %pathBat%\process.bat %pathScedule%\ /s /y /i

echo =============^> raising temporary server...
cd /d %~dp0
call host_self.bat