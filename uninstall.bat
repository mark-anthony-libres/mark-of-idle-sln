@echo off
setlocal

echo Please don't close the window, this will clean up the remaining files to remove...

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

@REM echo =========== REMOVING FILES AND FOLDERS ===========

@REM :: Delete all files in %MARKOFIDLE% directory
@REM del /f /q "%MARKOFIDLE%\*.*"

@REM :: Remove all subdirectories in %MARKOFIDLE%
@REM rd /s /q "%MARKOFIDLE%"

@REM echo =========== REMOVING SHORTCUT ===========

@REM :: Set the name of the shortcut (replace with your shortcut name)
@REM set shortcutName="Mark of Idle"

@REM :: Define the path to the Start Menu shortcuts folder
@REM set shortcutPath=%APPDATA%\Microsoft\Windows\Start Menu\Programs\%shortcutName%.lnk

@REM :: Check if the shortcut exists
@REM if exist "%shortcutPath%" (
@REM     :: Try to delete the shortcut
@REM     del /f /q "%shortcutPath%"
@REM     echo Shortcut "%shortcutName%.lnk" deleted.
@REM ) else (
@REM     echo Shortcut "%shortcutName%.lnk" does not exist.
@REM )


@REM endlocal