call build.bat
if %errorlevel% neq 0 exit /b %errorlevel%

call create-package.bat
if %errorlevel% neq 0 exit /b %errorlevel%