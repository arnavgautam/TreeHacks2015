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

[string] $plistpath = Join-Path -path $scriptDir -childpath "..\code\endv2\FacilityApp\FacilityApp.UI.IOS\Settings.plist"  -resolve
[string] $facilityAppClient = Join-Path -path $scriptDir -childpath "..\code\endv2\FacilityApp\Client\Assets\Configuration\Settings.xml"  -resolve
[string] $msSettings = Join-Path -path $scriptDir -childpath "..\code\endv2\FacilityApp\Service\Web.config"  -resolve
[string] $src_dir_location = Join-Path -path $scriptDir -childpath ".\assets\image\Map_Large.png" -resolve
[string] $dst_dir_location = Join-Path -path $scriptDir -childpath "..\code\endv2\FacilityApp\Client\Assets\Map_Large.png" -resolve

popd

# "========= Main Script =========" #

Write-Action "Truncating Azure Facilities Table..."
& "SqlCmd" @("-S", "$SQLAzureServerName", "-U", "$SQLAzureUsername", "-P", "$SQLAzurePassword",  "-d", "$SQLAzureDbName", "-Q", "TRUNCATE Table $SQLAzureTableName");
Write-Done

Write-Action "Cleaning SharePoint Library..."
.\tasks\clean-sharepoint-library\CleanSharePointLibrary.exe -SharePointSiteUrl "$sharepointBaseUrl" -SharePointUserName "$sharepointUsername" -SharePointPassword "$sharepointPassword" -SharePointFolder "$sharepointFolderName"
Write-Done

Write-Action "Updating images in Windows Store application"
Copy-Item $src_dir_location $dst_dir_location -force
Write-Done

Write-Action "Setting Windows Store client settings..."
Invoke-Expression  ".\tasks\updateConfig.ps1 -settingsConfig `"$facilityAppClient`" -azureSettingsFile `"..\config.xml`"  -node `"appSettings`""
Write-Done

Write-Action "Setting iOS client settings..."
Invoke-Expression ".\tasks\updatePlist.ps1 -plistPath `"$plistpath`" -azureSettingsFile `"..\config.xml`" "
Write-Done

Write-Action "Setting Mobile Services settings..."
Invoke-Expression ".\tasks\updateConfig.ps1 -settingsConfig `"$msSettings`" -azureSettingsFile `"..\config.xml`" -node `"configuration/appSettings`""
Write-Done