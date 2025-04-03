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

echo %APP_PROJECT%