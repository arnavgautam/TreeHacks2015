Param([string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# Import progress functions
. ".\tasks\progress-functions.ps1"

# "========= Initialization =========" #
pushd ".."

# Import required settings from config.local.xml
[xml] $xmlAzureSettings = Get-Content $azureSettingsFile

# Local Paths

[string] $solutionWorkingDir = $xmlAzureSettings.configuration.localPaths.solutionWorkingDir
[string] $solutionsDir = $xmlAzureSettings.configuration.localPaths.solutionsDir
[string] $beginSolutionDir = $xmlAzureSettings.configuration.localPaths.solutionsDir + "\Begin\ClipMeme\ClipMeme\web.config"
[string] $endSolutionDir = $xmlAzureSettings.configuration.localPaths.solutionsDir + "\End\ClipMeme\ClipMeme\web.config"

# Client Settings
[string] $username = $xmlAzureSettings.configuration.clientSettings.username


# Windows Azure
[string] $storageAccountName = $xmlAzureSettings.configuration.windowsAzureSubscription.storageAccountName
[string] $subscriptionName = $xmlAzureSettings.configuration.windowsAzureSubscription.subscriptionName

popd

# "========= Main Script =========" #

Write-Action "Connecting to Azure..."
Add-AzureAccount
Write-Done

Write-Action "Verifying Storage Account"
try
{
	$storageAccount = Get-AzureStorageAccount -StorageAccountName $storageAccountName -ErrorAction Stop
	Write-Host "Found..."
	$found = $TRUE
} catch 
{
	Write-Host "Not Found..."
	$found = $FALSE	
}

if (-not $found)
{
	Write-Action "Creating Storage Account"
	New-AzureStorageAccount -StorageAccountName $storageAccountName -Location "East US"	
	Set-AzureSubscription -SubscriptionName $subscriptionName -CurrentStorageAccountName $storageAccountName	
	Write-Done
	

}

# Containers

try{
	Get-AzureStorageContainer -Name "uploads" -ErrorAction Stop
	Write-Action "Removing blobs from container: uploads"
	Get-AzureStorageBlob -Container "uploads" | Remove-AzureStorageBlob
	Write-Done
}catch
{
	Write-Action "Creating Container Uploads"
	New-AzureStorageContainer -Name "uploads" -Permission Container
}

try{
	Get-AzureStorageContainer -Name "memes" -ErrorAction Stop
	Write-Action "Removing blobs from container: memes"
	Get-AzureStorageBlob -Container "memes" | Remove-AzureStorageBlob
	Write-Done
}catch
{
	Write-Action "Creating container: memes"
	New-AzureStorageContainer -Name "memes" -Permission Container
}



#
#Write-Action "Cleaning SharePoint Library..."
#.\tasks\clean-sharepoint-library\CleanSharePointLibrary.exe -SharePointSiteUrl "$sharepointBaseUrl" -SharePointUserName "$sharepointUsername" -SharePointPassword "$sharepointPassword" -SharePointFolder "$sharepointFolderName"
#Write-Done
#
#Write-Action "Updating images in the Windows Store application (Begin sln)"
#Copy-Item $src_dir_location $dst_dir_locationbegin -force
#Write-Done
#
#Write-Action "Updating images in the Windows Store application (End sln)"
#Copy-Item $src_dir_location $dst_dir_locationend -force
#Write-Done
#
#Write-Action "Updating Windows Store client settings (Begin sln)..."
#Invoke-Expression  ".\tasks\updateConfig.ps1 -settingsConfig `"$facilityAppClientbegin`" -azureSettingsFile `"..\config.xml`"  -node `"appSettings`""
#Write-Done
#
#
#
#Write-Action "Updating iOS client settings (Begin sln)..."
#Invoke-Expression ".\tasks\updatePlist.ps1 -plistPath `"$plistpathbegin`" -azureSettingsFile `"..\config.xml`" "
#Write-Done
#
#Write-Action "Updating iOS client settings (End sln)..."
#Invoke-Expression ".\tasks\updatePlist.ps1 -plistPath `"$plistpathend`" -azureSettingsFile `"..\config.xml`" "
#Write-Done
#
#Write-Action "Updating the Mobile Service settings (Begin sln)..."
#Invoke-Expression ".\tasks\updateConfig.ps1 -settingsConfig `"$msSettingsbegin`" -azureSettingsFile `"..\config.xml`" -node `"configuration/appSettings`""
#Write-Done
#
#Write-Action "Updating the Mobile Service settings (End sln)..."
#Invoke-Expression ".\tasks\updateConfig.ps1 -settingsConfig `"$msSettingsend`" -azureSettingsFile `"..\config.xml`" -node `"configuration/appSettings`""
#Write-Done
#
#Write-Action "Removing current working directory..."
#if (Test-Path "$solutionWorkingDir")
#{
#	Remove-Item "$solutionWorkingDir" -recurse -force
#}
#Write-Done
#
#Write-Action "Creating working directory..."
#New-Item "$solutionWorkingDir" -type directory | Out-Null
#if (!(Test-Path "$solutionWorkingDir"))
#{
#	New-Item "$solutionWorkingDir" -type directory | Out-Null
#}
#Copy-Item "$beginSolutionDir\*" "$solutionWorkingDir" -Recurse -Force
#Write-Done
