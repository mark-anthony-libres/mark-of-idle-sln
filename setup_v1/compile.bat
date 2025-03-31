@echo off
setlocal enabledelayedexpansion

:: Set the paths
set CONFIG_FILE=.\config.json
set RESOURCES_FOLDER=.\resources
set PROJECT_FILE=.\setup_v1.csproj

:: Clear the resources folder (remove existing files)
echo Deleting all files in the resources folder... %RESOURCES_FOLDER%
del /q "%RESOURCES_FOLDER%\*"


:: Use PowerShell to read the JSON file and extract file paths
for /f "delims=" %%A in ('powershell -Command "Get-Content '%CONFIG_FILE%' | ConvertFrom-Json | ForEach-Object { $_ }"') do (
    set "line=%%A"
    
    :: Check if the file exists
    if not exist "!line!" (
        echo ERROR: File !line! not found.
        exit /b 1
    )

    :: Copy the file to the resources folder
    echo Copying !line! to resources folder...
    copy /y "!line!" "%RESOURCES_FOLDER%\"

    :: Extract the file name (name + extension) from the full path
    set "fileName=%%~nxA"
    set filename_resource=%RESOURCES_FOLDER%\!fileName!

    if not exist "!filename_resource!" (
        echo ERROR: File !filename_resource! not found.
        exit /b 1
    )

    :: Update project file using powershell with delayed expansion
    ::powershell -Command "(Get-Content '%PROJECT_FILE%') -replace '<None Include=\"%RESOURCES_FOLDER%\!fileName!\" />', '<EmbeddedResource Include=\"%RESOURCES_FOLDER%\!fileName!\" />' | Set-Content '%PROJECT_FILE%'"


)


endlocal