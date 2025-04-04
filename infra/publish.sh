#!/bin/bash

# Ensure script stops on error
set -e

echo "======= Publish the app project... ======="

# Publish the app project
dotnet publish "$APP_PROJECT" -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true -p:ReadyToRun=true --output "./output"

echo "======= Activating venv inside of script and updating requirements.txt ======="

# Check if the venv directory exists
if [ ! -d "./scripts/venv" ]; then
    echo "No virtual environment found. Creating venv..."
    python3 -m venv "./scripts/venv"
    # Activate the virtual environment
    source "./scripts/venv/bin/activate"
    pip install -r "./scripts/requirements.txt"
else
    echo "Virtual environment already exists."
    # Activate the virtual environment
    source "./scripts/venv/bin/activate"
    pip freeze > "./scripts/requirements.txt"
fi

# Deactivate the virtual environment
deactivate

# Now zip the scripts folder
echo "======= Zipping scripts folder ======="

# Change to the temp_scripts directory
cd "./output/temp_scripts"
# Create a tar.gz file with the scripts
tar -czf "../scripts.tar.gz" "*"

# Go back to the original directory
cd "../../"

echo "======= Process complete! ======="
