@echo off


:: cmd.exe /c "cd ..\ && build.bat $(ProjectDir)$(OutDir)"
set OUTPUT_DIR=%1

exit \b 1

:: Delete all files and subfolders in the output folder
echo  ======= Deleting all items in the output folder...  =======
del /f /s /q ".\output\*" 
rmdir /s /q ".\output\"

:: Recreate the output folder
mkdir ".\output"

:: ======= Ensure the temp_scripts folder exists =======
echo creating temp_scripts folder
mkdir ".\output\temp_scripts" 

copy ".\debug.bat" ".\output"

:: Copy contents of the mark_of_idle\bin\Debug\net8.0-windows folder to the output folder
echo ======= %OUTPUT_DIR% to output folder...  =======
mkdir ".\output\app" 
xcopy /E /H /Y %OUTPUT_DIR% ".\output\app"


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

echo ======= Copying contents of scripts to output folder...  =======
xcopy /H /Y "scripts\*.*" ".\output\temp_scripts"
mkdir ".\output\temp_scripts\Infra"
xcopy /E /H /Y "scripts\Infra\*" ".\output\temp_scripts\Infra"
mkdir ".\output\temp_scripts\logs"


:: Now zip the scripts folder
echo  ======= Zipping scripts folder  =======
cd  "output\temp_scripts"
tar -a -c -f "..\scripts.zip" "*"
cd "../../"

:: Adjust the path to your WiX project
set SETUP_PROJECT=.\setup\mark_of_idle_setup.wixproj  
set UNINSTALL_PROJECT=.\uninstall\uninstall.csproj  

echo  ======= Building the uninstall project...  =======
dotnet publish "%UNINSTALL_PROJECT%" -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:ReadyToRun=true -p:PublishTrimmed=true --output ".\output"

echo  ======= Building the setup project...  =======
msbuild "%SETUP_PROJECT%" /p:Configuration=Debug /p:Platform="x86" /t:Build


echo  ======= Remove temp_scripts folder  =======
rmdir /s /q ".\output\temp_scripts"


echo  ======= Build successfully  =======
pause
