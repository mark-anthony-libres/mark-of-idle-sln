@echo off


REM Echo the value of MARKOFIDLE environment variable to check if it's set correctly
echo %MARKOFIDLE%

REM Ensure MARKOFIDLE is pointing to the correct project directory and that it ends with a backslash
if "%MARKOFIDLE%"=="" (
    echo ERROR: MARKOFIDLE environment variable is not set.
    exit /b 1
)

cd %MARKOFIDLE%

REM Activate the virtual environment
call venv\Scripts\activate.bat

REM Run the Python script
python main.py

REM Optionally deactivate the environment after execution
deactivate
