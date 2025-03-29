
set SETUP_PROJECT=.\uninstall\uninstall.csproj  

@REM echo  ======= Building the setup project...  =======
@REM msbuild "%SETUP_PROJECT%" /p:Configuration=Debug /p:Platform="x86" /t:Build

@REM echo  ======= uninstalling the app  =======
@REM start "" ".\uninstall\bin\Debug\net8.0\uninstall.exe"

set BASE_DIRECTORY=%CD%
setx MARKOFIDLE "%BASE_DIRECTORY%" /M