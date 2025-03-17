@echo off

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
echo ======= Copying contents of mark_of_idle\bin\Debug\net8.0-windows to output folder...  =======
xcopy /E /H /Y ".\mark_of_idle\bin\Debug\net8.0-windows\*" ".\output\"


echo ======= Activating venv inside of script and updating requirements.txt =======
call ".\scripts\venv\Scripts\activate.bat" 
call pip freeze > ".\scripts\requirements.txt"
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


echo  ======= Building the setup project...  =======
msbuild "%SETUP_PROJECT%" /p:Configuration=Debug /p:Platform="x86" /t:Build

echo  ======= Copying contents of setup\bin\Debug to output folder...  =======
xcopy /E /H /Y ".\setup\bin\Debug" ".\output\"


echo  ======= Remove temp_scripts folder  =======
rmdir /s /q ".\output\temp_scripts"


echo  ======= Build successfully  =======
pause
