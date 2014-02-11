.nuget\nuget pack "OpenRastaSwagger\OpenRastaSwagger.csproj" -Properties Configuration=Release
if %errorlevel% neq 0 exit /b %errorlevel%