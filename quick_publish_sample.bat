@echo off
cls

appcmd stop sites "Startup.Core"
iisreset /stop

call E:\Projects\git\StartupNetCore20\mypublish\setup.bat

iisreset /start
appcmd start sites "Startup.Core"

@pause