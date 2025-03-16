@echo off

:: Delete all files and subfolders in the output folder
echo Deleting all items in the output folder...
del /f /s /q ".\output\*" 
rmdir /s /q ".\output\"

:: Recreate the output folder
mkdir ".\output"

:: Copy contents of the mark_of_idle\bin\Debug\net8.0-windows folder to the output folder
echo Copying contents of mark_of_idle\bin\Debug\net8.0-windows to output folder...
xcopy /E /H /Y ".\mark_of_idle\bin\Debug\net8.0-windows\*" ".\output\"

:: Now zip the scripts folder
echo Zipping scripts folder
tar -a -c -f ".\output\scripts.zip" "scripts\*"

:: Adjust the path to your WiX project
set SETUP_PROJECT=.\setup\mark_of_idle_setup.wixproj  


echo Building the setup project...
msbuild "%SETUP_PROJECT%" /p:Configuration=Debug /p:Platform="x86" /t:Build

echo Copying contents of setup\bin\Debug to output folder...
xcopy /E /H /Y ".\setup\bin\Debug" ".\output\"

echo Folder has been zipped successfully.
pause
