@echo off
setlocal enabledelayedexpansion

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
set DEST_DIR="."

echo ========= BASE DIRECTORY =========
echo %CD%

@REM rmdir /s /q "%DEST_DIR%"
@REM mkdir "%DEST_DIR%"

:: Check if the ZIP file exists
if not exist "%ZIP_FILE%" (
    echo Error: ZIP file %ZIP_FILE% does not exist.
    exit /b 1
)

:: Check if the destination directory exists, create if not
if not exist "%DEST_DIR%" (
    echo Destination directory does not exist, creating it...
    mkdir "%DEST_DIR%"
)

:: Use PowerShell to unzip the file
echo ========= Unzipping %ZIP_FILE% to %DEST_DIR%...  =========
powershell.exe -Command "Expand-Archive -Path '%ZIP_FILE%' -DestinationPath '%DEST_DIR%'"

:: Check if unzip was successful
if %ERRORLEVEL% equ 0 (
    echo Unzip completed successfully.
) else (
    echo Error: Unzipping failed.
    exit /b 1
)

::   -------------------------
echo ========= create venv folder and activate it =========
call %python_path% -m venv venv
call ".\venv\Scripts\activate"
call pip install -r requirements.txt

echo  ======= Remove misc file  =======
rmdir /s /q "%ZIP_FILE%"

echo ========= set environemt variable =========
setx MARKOFIDLE "%CD%" /M

echo ========= create shortcut =========
cscript //nologo ".\shortcut.vbs"

echo ======= Starting the application ======
start /B mark_of_idle.exe

