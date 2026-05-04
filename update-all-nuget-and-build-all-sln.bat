@echo off
setlocal

set "SCRIPT_DIR=%~dp0"
set "PS_SCRIPT=%SCRIPT_DIR%scripts\Update-AllSolutions-NuGetAndBuild.ps1"

if not exist "%PS_SCRIPT%" (
  echo ERROR: Script not found: "%PS_SCRIPT%"
  exit /b 1
)

powershell -NoProfile -ExecutionPolicy Bypass -File "%PS_SCRIPT%" %*
set "EXIT_CODE=%ERRORLEVEL%"

if not "%EXIT_CODE%"=="0" (
  echo.
  echo FAILED with exit code %EXIT_CODE%
  exit /b %EXIT_CODE%
)

echo.
echo SUCCESS: NuGet update and solution builds completed.
exit /b 0
