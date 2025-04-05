# Fetch the latest tags
git fetch --tags

# Get the latest tag (version)
$LastTag = "v1.9.9"


# Regular expression pattern to check version format (vX.Y.Z)
$versionPattern = '^v(\d+)\.(\d+)\.(\d+)$'

# Check if the LastTag matches the version format
if ($LastTag -match $versionPattern) {
    # Extract version parts (major, minor, patch)
    $Major = [int]$matches[1]
    $Minor = [int]$matches[2]
    $Patch = [int]$matches[3]

     # Increment the patch version first
     $Patch += 1

     # If patch version is greater than 9, increment the minor version and reset patch to 0
     if ($Patch -gt 9) {
         $Minor += 1
         $Patch = 0
     }
 
     # If minor version is greater than 9, increment the major version and reset minor to 0
     if ($Minor -gt 9) {
         $Major += 1
         $Minor = 0
     }
 
     # Construct the new version
     $NewVersion = "v$Major.$Minor.$Patch"


} else {
    Write-Error "Invalid tag format detected: $LastTag. Expected format: vX.Y.Z"
    exit 1
}

Write-Host "New version: $NewVersion"

git tag $NewVersion
# git push origin $NewVersion

# GitLab API URL
$GitLabApiUrl = "https://gitlab.com/api/v4/projects/$ProjectId/uploads"

# GitLab private token for authentication (replace with your token)
$PrivateToken = "your_gitlab_private_token"

# Define the file to upload
$FilePath = "./output/setup_v1.exe"

# Upload the file
$UploadResponse = Invoke-RestMethod -Uri $GitLabApiUrl -Method Post -Headers @{ "PRIVATE-TOKEN" = $PrivateToken } -Form @{ file = Get-Item $FilePath }

# Get the URL of the uploaded file from the response
$FileUrl = $UploadResponse.url

Write-Host "File uploaded successfully: $FileUrl"