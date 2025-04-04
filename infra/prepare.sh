#!/bin/bash

# Delete all files and subfolders in the output folder
echo "======= Deleting all items in the output folder... ======="
rm -rf ./output/*  # Delete files recursively
rmdir ./output/    # Remove the output directory

# Recreate the output folder
mkdir ./output

# Ensure the temp_scripts folder exists
echo "creating temp_scripts folder"
mkdir ./output/temp_scripts

# Copy the debug.bat to the output folder
cp ./debug.bat ./output/
