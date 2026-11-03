@echo off
setlocal

pushd "%~dp0"

where docfx >nul 2>&1
if errorlevel 1 (
	echo DocFX not found. Installing...
	dotnet tool install -g docfx
	if errorlevel 1 (
		echo Failed to install DocFX.
		popd
		exit /b 1
	)
)

if /i "%~1"=="serve" (
	docfx docfx.json --serve
) else (
	docfx docfx.json
)
set "RESULT=%ERRORLEVEL%"

popd
exit /b %RESULT%
