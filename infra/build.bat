@echo off

echo  ======= Publish the app project...  =======
dotnet publish "%APP_PROJECT%" -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:ReadyToRun=true --output ".\output"


echo ======= Activating venv inside of script and updating requirements.txt =======
:: Check if the venv directory exists
IF NOT EXIST ".\scripts\venv\Scripts\activate.bat" (
    echo No virtual environment found. Creating venv...
    python -m venv .\scripts\venv
    call ".\scripts\venv\Scripts\activate.bat" 
    call pip install -r ".\scripts\requirements.txt"
) ELSE (
    echo Virtual environment already exists.
    call ".\scripts\venv\Scripts\activate.bat" 
    call pip freeze > ".\scripts\requirements.txt"
)
call ".\scripts\venv\Scripts\deactivate.bat"

:: Now zip the scripts folder
echo  ======= Zipping scripts folder  =======
cd  "output\temp_scripts"
tar -a -c -f "..\scripts.zip" "*"
cd "../../"
