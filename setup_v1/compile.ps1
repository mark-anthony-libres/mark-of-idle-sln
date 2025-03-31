

# Define the path for the file EmbedResources.targets in the current directory (relative to the script's directory)
$scriptDirectory = Get-Location  # Get the current directory of the script
$xmlFilePath = Join-Path -Path $scriptDirectory -ChildPath "EmbedResources.targets"  # Make sure it's within 'setup_v1'


# Check if the file exists
if (Test-Path -Path $xmlFilePath) {
    Write-Host "File EmbedResources.targets already exists. Removing it..."
    Remove-Item -Path $xmlFilePath -Force
}

Write-Host "Creating EmbedResources.targets file..."


# Create a new XML document
$xmlContent = New-Object -TypeName System.Xml.XmlDocument

# Create the root element <Project> and set the XML namespace
$rootElement = $xmlContent.CreateElement("Project")
$rootElement.SetAttribute("xmlns", "http://schemas.microsoft.com/developer/msbuild/2003")


$itemGroupRemove = $xmlContent.CreateElement("ItemGroup")
$itemGroupAdd = $xmlContent.CreateElement("ItemGroup")


# Set the paths
$CONFIG_FILE = ".\config.json"
$RESOURCES_FOLDER = ".\resources"
$PROJECT_FILE = ".\setup_v1.csproj"

# Clear the resources folder (remove existing files)
Write-Host "Deleting all files in the resources folder... $RESOURCES_FOLDER"
Remove-Item -Path "$RESOURCES_FOLDER\*" -Force -Recurse

# Read the JSON file and loop through the file paths
$files = Get-Content -Path $CONFIG_FILE | ConvertFrom-Json

foreach ($line in $files) {
    # Check if the file exists
    if (-not (Test-Path -Path $line)) {
        Write-Host "ERROR: File $line not found."
        exit 1
    }

    # Copy the file to the resources folder
    Write-Host "Copying $line to resources folder..."
    Copy-Item -Path $line -Destination $RESOURCES_FOLDER -Force

    # Extract the file name (name + extension) from the full path
    $fileName = [System.IO.Path]::GetFileName($line)
    $filename_resource = Join-Path -Path $RESOURCES_FOLDER -ChildPath $fileName

    # Check if the file is copied successfully
    if (-not (Test-Path -Path $filename_resource)) {
        Write-Host "ERROR: File $filename_resource not found after copying."
        exit 1
    }

    $noneRemove = $xmlContent.CreateElement("None")
    $noneRemove.SetAttribute("Remove", "resources\" + $fileName)
    $itemGroupRemove.AppendChild($noneRemove)


    $embeddedResource = $xmlContent.CreateElement("EmbeddedResource")
    $embeddedResource.SetAttribute("Include", "resources\" + $fileName)
    $itemGroupAdd.AppendChild($embeddedResource)
    # Update the project file to mark the file as EmbeddedResource

    

}


$rootElement.AppendChild($itemGroupRemove)
$rootElement.AppendChild($itemGroupAdd)

$xmlContent.AppendChild($rootElement)
$xmlContent.Save($xmlFilePath)



Write-Host "All files processed successfully."





# # Define the XML file path
# $xmlFilePath = "sample.xml"

# # Load the XML file
# [xml]$xmlContent = Get-Content -Path $xmlFilePath

# # Create the first ItemGroup to remove 'first.bat'
# $itemGroupRemove = $xmlContent.CreateElement("ItemGroup")
# $noneRemove = $xmlContent.CreateElement("None")
# $noneRemove.SetAttribute("Remove", "resources\first.bat")
# $itemGroupRemove.AppendChild($noneRemove)

# # Create the second ItemGroup to add 'file2.bat' as EmbeddedResource
# $itemGroupAdd = $xmlContent.CreateElement("ItemGroup")
# $embeddedResource = $xmlContent.CreateElement("EmbeddedResource")
# $embeddedResource.SetAttribute("Include", "resources\file2.bat")
# $itemGroupAdd.AppendChild($embeddedResource)

# # Append both ItemGroup elements to the XML
# $xmlContent.Project.AppendChild($itemGroupRemove)
# $xmlContent.Project.AppendChild($itemGroupAdd)

# # Save the modified XML back to the file
# $xmlContent.Save($xmlFilePath)

# Write-Host "XML file updated successfully."
