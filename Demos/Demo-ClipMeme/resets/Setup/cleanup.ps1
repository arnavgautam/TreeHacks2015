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

if ($found)
{
	Write-Action "Cleaning Storage Account Blobs"
	Set-AzureStorageAccount -StorageAccountName $storageAccountName
	Remove-AzureStorageContainer -Name "uploads"
	Remove-AzureStorageContainer -Name "memes"
	Write-Done
}else{
	Write-Action "Creating Storage Account"
	New-AzureStorageAccount -StorageAccountName $storageAccountName -Location "East US"
	Set-AzureStorageAccount -StorageAccountName $storageAccountName
	Write-Done
}

Write-Action "Creating containers"
	$keys = Get-AzureStorageKey -StorageAccountName $storageAccountName
	$context = New-AzureStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $keys.Primary
	New-AzureStorageContainer -Name "uploads" -Permission Container -Context $context
	New-AzureStorageContainer -Name "memes" -Permission Container -Context $context
Write-Done


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
