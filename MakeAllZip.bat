@echo off

call .\MakeZip.bat x86 release vroonsh
call .\MakeZip.bat x64 release vroonsh
call .\MakeZip.bat x86 debug vroonsh
call .\MakeZip.bat x64 debug vroonsh
