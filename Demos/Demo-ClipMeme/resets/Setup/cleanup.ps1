Param([string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

$FunctionToRegister = Join-Path $scriptDir ".\Invoke-AzureEnvironmentSetup.ps1"

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
[string] $displayName = $xmlAzureSettings.configuration.clientSettings.displayName


# Windows Azure
[string] $EnvironmentSubscriptionName = 

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

# Azure Web Site
$appSettings = @{"displayName" = $displayName;}
New-AzureWebSite -Name 	$webSiteName
Set-AzureWebSite -Name $webSiteName -AppSettings $appSettings -WebSocketsEnabled $true

