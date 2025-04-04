@echo off

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

:: Now zip the scripts folder
echo  ======= Zipping scripts folder  =======
cd  "output\temp_scripts" || exit /b 1
echo %CD%
tar -a -c -f "..\scripts.zip" "*" || exit /b 1
cd "../../"

