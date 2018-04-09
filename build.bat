"%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" /m:8 /p:Configuration=Release "OpenRastaSwagger.sln"
if %errorlevel% neq 0 exit /b %errorlevel%