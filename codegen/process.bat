@echo off
echo =============^> initializing codegen environment...

cd /d %~dp0
if exist output\ rd /s /q output

echo =============^> generating codes for models and services...
java -jar ../../Shared.Source/swagger-codegen-cli.jar generate -i %host%/swagger/%versionTarget%/swagger.json -l typescript-angular -o output -c config.json
cd..
if exist %pathWeb%\src\app\%pathCodegen%\ rd /s /q %pathWeb%\src\app\%pathCodegen%
xcopy %pathCodegen%\output\api\*.* %pathWeb%\src\app\%pathCodegen%\api\ /s /y /i
xcopy %pathCodegen%\output\model\*.* %pathWeb%\src\app\%pathCodegen%\model\ /s /y /i
xcopy %pathCodegen%\output\*.ts %pathWeb%\src\app\%pathCodegen%\ /s /y /i

echo =============^> deleting temporary codegen files...
if exist %pathCodegen%\output\ rd /s /q %pathCodegen%\output