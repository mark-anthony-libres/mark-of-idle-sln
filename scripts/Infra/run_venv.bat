@echo off

for /f "tokens=3" %%a in ('reg query "HKLM\System\CurrentControlSet\Control\Session Manager\Environment" /v MARKOFIDLE 2^>nul') do set MARKOFIDLE=%%a

REM Echo the value of MARKOFIDLE environment variable to check if it's set correctly
echo %MARKOFIDLE%

REM Ensure MARKOFIDLE is pointing to the correct project directory and that it ends with a backslash
if "%MARKOFIDLE%"=="" (
    echo ERROR: MARKOFIDLE environment variable is not set.
    exit /b 1
)

cd %MARKOFIDLE%\scripts

REM Activate the virtual environment
call venv\Scripts\activate.bat

REM Run the Python script
python main.py

REM Optionally deactivate the environment after execution
deactivate
