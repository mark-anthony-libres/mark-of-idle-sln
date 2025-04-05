@echo off

@REM set OUTPUT_DIR=%1

exit /b 1

set ORIGINAL_DIR=%CD%
set OUTPUT_DIR=./output
set APP_PROJECT=./mark_of_idle/mark_of_idle.csproj
set SETUP_V1_PROJECT=./setup_v1/setup_v1.csproj
set UNINSTALL_PROJECT=./uninstall/uninstall.csproj

:: Delete all files and subfolders in the output folder
echo  ======= Deleting all items in the output folder...  =======
del /f /s /q ".\output\*" || exit /b 1
rmdir /s /q ".\output\" || exit /b 1

:: Recreate the output folder
mkdir ".\output" || exit /b 1

:: ======= Ensure the temp_scripts folder exists =======
echo creating temp_scripts folder
mkdir ".\output\temp_scripts" || exit /b 1 

copy ".\debug.bat" ".\output" || exit /b 1

echo  ======= Publish the app project...  =======
dotnet publish "%APP_PROJECT%" -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:ReadyToRun=true --output ".\output" || exit /b 1


echo ======= Activating venv inside of script and updating requirements.txt =======
:: Check if the venv directory exists
IF NOT EXIST ".\scripts\venv\Scripts\activate.bat" (
    echo No virtual environment found. Creating venv...
    python -m venv .\scripts\venv || exit /b 1
    call ".\scripts\venv\Scripts\activate.bat" || exit /b 1
    call pip install -r ".\scripts\requirements.txt" || exit /b 1
) ELSE (
    echo Virtual environment already exists.
    call ".\scripts\venv\Scripts\activate.bat" || exit /b 1
    call pip freeze > ".\scripts\requirements.txt" || exit /b 1
)
call ".\scripts\venv\Scripts\deactivate.bat" || exit /b 1

echo ======= Copying contents of scripts to output folder...  =======
xcopy /H /Y "scripts\*.*" ".\output\temp_scripts" || exit /b 1
mkdir ".\output\temp_scripts\Infra" || exit /b 1
xcopy /E /H /Y "scripts\Infra\*" ".\output\temp_scripts\Infra" || exit /b 1
mkdir ".\output\temp_scripts\logs" || exit /b 1


:: Now zip the scripts folder
echo  ======= Zipping scripts folder  =======
cd  "output\temp_scripts" || exit /b 1
tar -a -c -f "..\scripts.zip" "*" || exit /b 1
cd "../../" || exit /b 1

:: Adjust the path to your WiX project

echo  ======= Publish the uninstall project...  =======
dotnet publish "%UNINSTALL_PROJECT%" -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:ReadyToRun=true --output ".\output" || exit /b 1

@REM echo  ======= Building the setup project...  =======
@REM msbuild "%SETUP_PROJECT%" /p:Configuration=Debug /p:Platform="x86" /t:Build

echo  ======= Publish the setup project...  =======
cd .\setup_v1 || exit /b 1
powershell -ExecutionPolicy Bypass -File ".\compile.ps1" || exit /b 1
cd %ORIGINAL_DIR% || exit /b 1

dotnet publish "%SETUP_V1_PROJECT%" -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:ReadyToRun=true --output ".\output" || exit /b 1


echo  ======= Remove temp_scripts folder  =======
rmdir /s /q ".\output\temp_scripts" || exit /b 1


echo  ======= Build successfully  =======
pause
