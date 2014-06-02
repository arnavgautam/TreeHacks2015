Param([string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

$FunctionToRegister = Join-Path $scriptDir ".\Tasks\Invoke-AzureEnvironmentSetup.ps1"
. "$FunctionToRegister"

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
[string] $DisplayName = $xmlAzureSettings.configuration.clientSettings.DisplayName


# Windows Azure
[string] $EnvironmentSubscriptionName = $xmlAzureSettings.configuration.windowsAzureSubscription.EnvironmentSubscriptionName
[string] $EnvironmentPrimaryLocation = $xmlAzureSettings.configuration.windowsAzureSubscription.EnvironmentPrimaryLocation
[string] $StorageAccountName = $xmlAzureSettings.configuration.windowsAzureSubscription.StorageAccountName
[string] $WebsiteName = $xmlAzureSettings.configuration.windowsAzureSubscription.WebsiteName
$PublishSettingsFile = Join-Path $scriptDir ".\assets\publishSettings\azure.publishsettings"
$StorageContainers = @('uploads','memes')
$EnvironmentWebSites = @{$WebsiteName=$EnvironmentPrimaryLocation}
$EnvironmentStagingSites = @{$WebsiteName=$EnvironmentPrimaryLocation}

$AppSettings = @{'DisplayName'=$DisplayName; "TrafficManagerRegion"=$EnvironmentPrimaryLocation}

popd

Invoke-AzureEnvironmentSetup -EnvironmentSubscriptionName $EnvironmentSubscriptionName `
                             -EnvironmentPrimaryLocation $EnvironmentPrimaryLocation `
                             -StorageEnvironmentLocation $EnvironmentPrimaryLocation `
                             -EnvironmentWebSites $EnvironmentWebSites `
                             -EnvironmentStagingSites $EnvironmentStagingSites `
                             -EnvironmentStorageAccount $StorageAccountName `
                             -StorageContainers $StorageContainers `
							 -AppSettings $AppSettings `
							 -PublishSettingsFile $PublishSettingsFile