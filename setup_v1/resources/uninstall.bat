@echo off
setlocal

echo This script will now clean up any remaining files associated with the application.
echo It is triggered after you click "Uninstall" in the Apps & Features section.
echo The cleanup process will finalize any leftover files, ensuring the application is fully removed from your system.
echo If you want to manually uninstall the application, please find the file "uninstall.exe".

echo %MARKOFIDLE% | findstr /I "repos" >nul
if %errorlevel% equ 0 (
    exit \b 1
)

echo =========== STOP THE APP ===========

set processName=mark_of_idle.exe

:: Check if the process is running
tasklist /FI "IMAGENAME eq %processName%" 2>NUL | find /I "%processName%" > NUL

if errorlevel 1 (
    echo Application is not running.
) else (
    echo Application found. Stopping the application...
    taskkill /IM %processName% /F
    echo Application stopped successfully.
)

echo =========== STOP THE SCRIPT ===========

:: Define the path to the JSON file
set JSON_FILE=%MARKOFIDLE%\scripts\data.json

:: Use PowerShell to modify the JSON file
powershell -Command "$json = Get-Content -Path '%JSON_FILE%' | ConvertFrom-Json; $json.is_active = $false; $json | ConvertTo-Json -Depth 3 | Set-Content -Path '%JSON_FILE%'"

echo "The is_active value has been set to false."

echo =========== REMOVING FILES AND FOLDERS ===========

:: Delete all files in %MARKOFIDLE% directory
del /f /q "%MARKOFIDLE%\*.*"

:: Remove all subdirectories in %MARKOFIDLE%
rd /s /q "%MARKOFIDLE%"

echo =========== REMOVING SHORTCUT ===========

:: Set the name of the shortcut (replace with your shortcut name)
set shortcutName="Mark of Idle"

:: Define the path to the Start Menu shortcuts folder
set shortcutPath=%APPDATA%\Microsoft\Windows\Start Menu\Programs\%shortcutName%.lnk

:: Check if the shortcut exists
if exist "%shortcutPath%" (
    :: Try to delete the shortcut
    del /f /q "%shortcutPath%"
    echo Shortcut "%shortcutName%.lnk" deleted.
) else (
    echo Shortcut "%shortcutName%.lnk" does not exist.
)


endlocal