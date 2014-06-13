Param([string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# Import progress functions
. ".\tasks\progress-functions.ps1"

# "========= Initialization =========" #
pushd ".."

# Import required settings from config.local.xml
[xml] $xmlAzureSettings = Get-Content $azureSettingsFile

# Database
[string] $SQLAzureServerName = $xmlAzureSettings.configuration.windowsAzureSubscription.mobileService.sqlServer
[string] $SQLAzureUsername = $xmlAzureSettings.configuration.windowsAzureSubscription.mobileService.sqlUsername
[string] $SQLAzurePassword = $xmlAzureSettings.configuration.windowsAzureSubscription.mobileService.sqlPassword
[string] $SQLAzureDbName = $xmlAzureSettings.configuration.windowsAzureSubscription.mobileService.db
[string] $SQLAzureTableName = $xmlAzureSettings.configuration.windowsAzureSubscription.mobileService.sqlTable

[string] $sharepointBaseUrl = $xmlAzureSettings.configuration.sharepoint.baseUrl
[string] $sharepointUsername = $xmlAzureSettings.configuration.sharepoint.username
[string] $sharepointPassword = $xmlAzureSettings.configuration.sharepoint.password
[string] $sharepointFolderName = $xmlAzureSettings.configuration.sharepoint.folderName

[string] $src_dir_location = Join-Path -path $scriptDir -childpath ".\assets\image\Map_Large.png" -resolve

[string] $plistpathbegin = Join-Path -path $scriptDir -childpath "..\FacilityApp\Begin\FacilityApp.UI.IOS\Settings.plist"  -resolve
[string] $facilityAppClientbegin = Join-Path -path $scriptDir -childpath "..\FacilityApp\Begin\Client\Assets\Configuration\Settings.xml"  -resolve
[string] $msSettingsbegin = Join-Path -path $scriptDir -childpath "..\FacilityApp\Begin\Service\Web.config"  -resolve
[string] $dst_dir_locationbegin = Join-Path -path $scriptDir -childpath "..\FacilityApp\Begin\Client\Assets\Map_Large.png" -resolve

[string] $plistpathend = Join-Path -path $scriptDir -childpath "..\FacilityApp\End\FacilityApp.UI.IOS\Settings.plist"  -resolve
[string] $facilityAppClientend = Join-Path -path $scriptDir -childpath "..\FacilityApp\End\Client\Assets\Configuration\Settings.xml"  -resolve
[string] $msSettingsend = Join-Path -path $scriptDir -childpath "..\FacilityApp\End\Service\Web.config"  -resolve
[string] $dst_dir_locationend = Join-Path -path $scriptDir -childpath "..\FacilityApp\End\Client\Assets\Map_Large.png" -resolve

[string] $workingDir = $xmlAzureSettings.configuration.localPaths.workingDir
[string] $solutionWorkingDir = $xmlAzureSettings.configuration.localPaths.solutionWorkingDir
[string] $beginSolutionDir = Resolve-Path $xmlAzureSettings.configuration.localPaths.beginSolutionDir

popd

# "========= Main Script =========" #

Write-Action "Truncating Azure Facilities Table..."
& "SqlCmd" @("-S", "$SQLAzureServerName", "-U", "$SQLAzureUsername", "-P", "$SQLAzurePassword",  "-d", "$SQLAzureDbName", "-Q", "TRUNCATE Table $SQLAzureTableName");
Write-Done

Write-Action "Cleaning SharePoint Library..."
.\tasks\clean-sharepoint-library\CleanSharePointLibrary.exe -SharePointSiteUrl "$sharepointBaseUrl" -SharePointUserName "$sharepointUsername" -SharePointPassword "$sharepointPassword" -SharePointFolder "$sharepointFolderName"
Write-Done

Write-Action "Updating images in the Windows Store application (Begin sln)"
Copy-Item $src_dir_location $dst_dir_locationbegin -force
Write-Done

Write-Action "Updating images in the Windows Store application (End sln)"
Copy-Item $src_dir_location $dst_dir_locationend -force
Write-Done

Write-Action "Updating Windows Store client settings (Begin sln)..."
Invoke-Expression  ".\tasks\updateConfig.ps1 -settingsConfig `"$facilityAppClientbegin`" -azureSettingsFile `"..\config.xml`"  -node `"appSettings`""
Write-Done

Write-Action "Updating Windows Store client settings (End sln)..."
Invoke-Expression  ".\tasks\updateConfig.ps1 -settingsConfig `"$facilityAppClientend`" -azureSettingsFile `"..\config.xml`"  -node `"appSettings`""
Write-Done

Write-Action "Updating iOS client settings (Begin sln)..."
Invoke-Expression ".\tasks\updatePlist.ps1 -plistPath `"$plistpathbegin`" -azureSettingsFile `"..\config.xml`" "
Write-Done

Write-Action "Updating iOS client settings (End sln)..."
Invoke-Expression ".\tasks\updatePlist.ps1 -plistPath `"$plistpathend`" -azureSettingsFile `"..\config.xml`" "
Write-Done

Write-Action "Updating the Mobile Service settings (Begin sln)..."
Invoke-Expression ".\tasks\updateConfig.ps1 -settingsConfig `"$msSettingsbegin`" -azureSettingsFile `"..\config.xml`" -node `"configuration/appSettings`""
Write-Done

Write-Action "Updating the Mobile Service settings (End sln)..."
Invoke-Expression ".\tasks\updateConfig.ps1 -settingsConfig `"$msSettingsend`" -azureSettingsFile `"..\config.xml`" -node `"configuration/appSettings`""
Write-Done

Write-Action "Removing current working directory..."
if (Test-Path "$solutionWorkingDir")
{
	Remove-Item "$solutionWorkingDir" -recurse -force
}
Write-Done

Write-Action "Creating working directory..."
New-Item "$solutionWorkingDir" -type directory | Out-Null
if (!(Test-Path "$solutionWorkingDir"))
{
	New-Item "$solutionWorkingDir" -type directory | Out-Null
}
Copy-Item "$beginSolutionDir\*" "$solutionWorkingDir" -Recurse -Force
Write-Done
