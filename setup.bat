@echo off
setlocal enabledelayedexpansion



:: Display current date and time
echo =========================== Current date and time: %date% %time% =================================



:: Initialize the variables
set "python_path="
set "pip_path="

:: Query user-specific Path (HKCU)
for /f "tokens=2* delims= " %%a in ('reg query "HKCU\Environment" /v Path 2^>nul') do (
    set USER_PATH=%%b
)

:: Query system-wide Path (HKLM)
for /f "tokens=2* delims= " %%a in ('reg query "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Environment" /v Path 2^>nul') do (
    set SYSTEM_PATH=%%b
)

:: Combine the paths into one variable
set "COMBINED_PATH=!USER_PATH!;!SYSTEM_PATH!"

:: Loop through combined paths and search for python.exe and pip.exe
for %%P in (!COMBINED_PATH!) do (
    echo %%P
    if exist "%%P\python.exe" (
        set "python_path=%%P\python.exe"
    )
)

:: Display the found paths
if defined python_path (
    echo Python found at: !python_path!
) else (
    echo Python not found.
    exit \b 1
)

::-----------------------------------------------------------------------------
:: Path to the ZIP file

set ZIP_FILE=.\scripts.zip

:: Destination folder where the files will be extracted
set BASE_DIRECTORY=%CD%
set SCRIPTS_FOLDER=%CD%\scripts

echo ========= BASE DIRECTORY =========
echo %CD%
echo ========= SCRIPTS DIRECTORY =========
echo %SCRIPTS_FOLDER%

@REM rmdir /s /q "%DEST_DIR%"
@REM mkdir "%DEST_DIR%"

:: Check if the ZIP file exists
if not exist "%ZIP_FILE%" (
    echo Error: ZIP file %ZIP_FILE% does not exist.
    exit /b 1
)

mkdir "%SCRIPTS_FOLDER%"

:: Use PowerShell to unzip the file
echo ========= Unzipping %ZIP_FILE% to %SCRIPTS_FOLDER%...  =========
powershell.exe -Command "Expand-Archive -Path '%ZIP_FILE%' -DestinationPath '%SCRIPTS_FOLDER%'"

:: Check if unzip was successful
if %ERRORLEVEL% equ 0 (
    echo Unzip completed successfully.
) else (
    echo Error: Unzipping failed.
    exit /b 1
)

::   -------------------------
echo ========= create venv folder and activate it =========
cd %SCRIPTS_FOLDER%
echo %CD%
call %python_path% -m venv venv
call "./venv/Scripts/activate"
call pip install -r requirements.txt
cd %BASE_DIRECTORY%

echo "go back to main directory %BASE_DIRECTORY%"


echo ========= set environemt variable =========
setx MARKOFIDLE "%BASE_DIRECTORY%" /M

echo ========= create shortcut =========
cscript //nologo "%BASE_DIRECTORY%\shortcut.vbs"

echo  ======= Remove misc file  =======
del /s /q %ZIP_FILE%
del /s /q "%BASE_DIRECTORY%\shortcut.vbs"

echo ======= Starting the application ======
start /B mark_of_idle.exe 

