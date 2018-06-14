@echo off
cls
cd /d %~dp0

set npmproxy=cnpm

call bootstrap.bat

@pause