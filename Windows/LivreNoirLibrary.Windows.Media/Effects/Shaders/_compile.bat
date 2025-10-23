@echo off
cd /d "%~dp0"

for %%F in (*.hlsl) do (
  "C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\fxc.exe" "%%F" /T ps_3_0 /Fo "%%~nF.ps"
)

pause