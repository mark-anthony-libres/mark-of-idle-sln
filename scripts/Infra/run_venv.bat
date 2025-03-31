@echo off
setlocal enabledelayedexpansion

for /f "tokens=3" %%a in ('reg query "HKLM\System\CurrentControlSet\Control\Session Manager\Environment" /v MARKOFIDLE 2^>nul') do set (MARKOFIDLE=%%a)

REM Echo the value of MARKOFIDLE environment variable to check if it's set correctly
echo hello =>> !MARKOFIDLE!

REM Check if the variable is set
if defined MARKOFIDLE (
    echo Value of MARKOFIDLE: !MARKOFIDLE!
) else (
    echo MARKOFIDLE is not set or registry value not found.
)

cd /d "!MARKOFIDLE!\scripts"

REM Activate the virtual environment
call venv\Scripts\activate.bat

REM Run the Python script
python main.py

REM Optionally deactivate the environment after execution
deactivate


endlocal