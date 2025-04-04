# Define the current working directory (base directory)
$currentWorkingDirectory = Get-Location

# Define the isolated environment directory
$ENV_DIR = "$env:TEMP\git-isolated-environment"

# Define multiple folders to exclude
$EXCLUDE_FOLDERS = @(
    ".git", 
    ".vs", 
    "mark_of_idle\bin", 
    "mark_of_idle\obj\Debug",
    "mark_of_idle\obj\Release",
    "output",
    "setup",
    "setup_v1\bin",
    "setup_v1\obj\Debug",
    "setup_v1\obj\Release",
    "uninstall\bin",
    "uninstall\obj\Debug",
    "uninstall\obj\Release",
    "scripts\logs",
    "scripts\venv",
    "scripts\__pycache__"
)

# Loop through each folder to exclude
for ($i = 0; $i -lt $EXCLUDE_FOLDERS.Length; $i++) {
    # Concatenate the current directory with each folder to form an absolute path
    $EXCLUDE_FOLDERS[$i] = Join-Path -Path $currentWorkingDirectory -ChildPath $EXCLUDE_FOLDERS[$i]
}

# Step 1: Clean up any existing environment (if it exists)
if (Test-Path -Path $ENV_DIR) {
    Write-Host "Cleaning up existing environment..."
    Remove-Item -Path $ENV_DIR -Recurse -Force
}


# Step 2: Create a new isolated environment
Write-Host "Creating isolated environment at $ENV_DIR..."
New-Item -ItemType Directory -Path $ENV_DIR

Read-Host "Press Enter to continue..."

# Step 3: Copy project files to the isolated environment, excluding specified folders
Write-Host "Copying the project files to the isolated environment..."

# Loop through each folder to exclude and use robocopy
$robocopyArgs = @()
# Add exclusion folders to robocopy arguments
foreach ($folder in $EXCLUDE_FOLDERS) {
    Write-Host "testing"
    $robocopyArgs += "/XD"  # Add the /XD flag to exclude a directory
    $robocopyArgs += "$folder"  # Add the folder path to exclude
}

# Run robocopy to copy everything while excluding the specified folders

# Write-Host $robocopyArgs
robocopy . $ENV_DIR /E $robocopyArgs

Write-Host "Files copied, excluding specified folders."

# End of script
