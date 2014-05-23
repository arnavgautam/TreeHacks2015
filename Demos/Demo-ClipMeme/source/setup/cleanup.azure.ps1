Param([string] $settingsFile)

$Error.Clear();
$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# import demo tookit
Import-Module ".\tasks\demo-toolkit\DemoToolkit.psd1" -DisableNameChecking

# import progress functions
. ".\tasks\progress-functions.ps1"

# "========= Initialization =========" #
pushd ".."

if($settingsFile -eq $nul -or $settingsFile -eq "")
{
	$settingsFile = "config.primary.xml"
}

[string] $environment = "Primary"

if($settingsFile.ToUpperInvariant().IndexOf("SHADOW") -gt -1) 
{
    $environment = "Shadow"
}

# Import required settings
[xml] $settings = Get-Content $settingsFile


[string] $wazPublishSettings = Resolve-Path $settings.configuration.azureSubscription.publishSettingsFile
[string] $accountName =  $settings.configuration.azureSubscription.accountName
[boolean] $enableWebSitesDelete = [boolean] $settings.configuration.azureSubscription.enableWebSitesDelete
[string] $webSitesToKeep = $settings.configuration.azureSubscription.webSitesToKeep

[string] $storageConnectionString = $settings.configuration.appSettings.storageConnectionString

[string] $storageMemesContainerName = $settings.configuration.azureSubscription.storageMemesContainerName
[string] $storageMemesTableName = $settings.configuration.azureSubscription.storageMemesTableName
[string] $storageDirectoryToScan = Resolve-Path $settings.configuration.azureSubscription.storageDirectoryToScan

[string] $webJobFunctionName = $settings.configuration.azureSubscription.webJobFunctionName

popd
# "========= Main Script =========" #

write-host ""
write-host "Cleanup Azure $environment"
write-host "".PadRight(68, "=")


Write-Action "Deleting all Azure WebSites not in the whitelist..."
if ($wazPublishSettings -ne "" -and $wazPublishSettings -ne $nul) {
$accountImport = 'azure account import "' + $wazPublishSettings + '"'
Invoke-Expression $accountImport
$accountImport = 'azure account set "' + $accountName + '"'
Invoke-Expression $accountImport

if ($enableWebSitesDelete) {
[array] $keepList = $webSitesToKeep.Split(",") | % { $_.Trim() }
[regex]::matches((Invoke-Expression "azure site list"), "data:\s+(\S+)\s+([^-\s]+)") | Where { $keepList -notcontains $_.Groups[1].value -and $_.Groups[2].value -ne "Slot" } | foreach-object { Invoke-Expression ("azure site delete -q " + $_.Groups[1].value) }
}
}

Write-Done

Write-Action "Cleaning up storage account images and metadata..."
.\tasks\clean-up-storage-account.ps1 "$storageMemesContainerName" "$storageMemesTableName" "$storageConnectionString" "$storageDirectoryToScan"
Write-Done

Write-Action "Cleaning up and Regenerating the WebJob Logs..."
.\tasks\GenerateWebJobLogs\GenerateWebJobLogs.exe -NonInteractive -TargetAccount "$storageConnectionString" -WebJobFunctionName $webJobFunctionName
Write-Done



# --------------------------------- #