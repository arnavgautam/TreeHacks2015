Param([string] $demoSettingsAzureFile, [string] $demoSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# Import Windows Azure PowerShell modules
Get-ChildItem "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows Azure\PowerShell\Azure\*.psd1" | ForEach-Object {Import-Module $_ | Out-Null}

# "========= Initialization =========" #
pushd ".."
# Get settings from user configuration file for azure
if($demoSettingsAzureFile -eq $nul -or $demoSettingsAzureFile -eq "")
{
    $demoSettingsAzureFile = Resolve-Path "Config.Azure.xml"
}

$xmlDemoSettingsAzure = New-Object xml
$xmlDemoSettingsAzure.psbase.PreserveWhitespace = $true
$xmlDemoSettingsAzure.Load($demoSettingsAzureFile)

[string] $publishSettingsFilePath = $xmlDemoSettingsAzure.configuration.publishSettingsFilePath
if ($publishSettingsFilePath -eq "" -or $publishSettingsFilePath -eq $nul -or !(Test-Path $publishSettingsFilePath))
{
    write-host "You need to specify the publish setting profile."
    write-host "After downloading the publish settings profile from the management portal, specify the file location in the configuration file path under the publishSettingsFilePath element."
    write-host

    Write-Host "Press any key to continue ..."
    [void][System.Console]::ReadKey($true)

    Get-AzurePublishSettingsFile
    
    exit 1
}

# Import required settings from Config.Azure.xml if neccessary #
[string] $publishSettingsFilePath = Resolve-Path $xmlDemoSettingsAzure.configuration.publishSettingsFilePath
[string] $subscriptionName = $xmlDemoSettingsAzure.configuration.subscriptionName

[string] $diagnosticsStorageAccountName = $xmlDemoSettingsAzure.configuration.storageAccounts.diagnosticsStorageAccount.name
[string] $cloudServiceName = $xmlDemoSettingsAzure.configuration.cloudService.name
[string] $cloudServiceLocation = $xmlDemoSettingsAzure.configuration.cloudService.location
[string] $databaseServerName = $xmlDemoSettingsAzure.configuration.sqlDatabase.serverName

# Get settings from user configuration file
if($demoSettingsFile -eq $nul -or $demoSettingsFile -eq "")
{
    $demoSettingsFile = Resolve-Path "Config.Local.xml"
}

$xmlDemoSettings = New-Object xml
$xmlDemoSettings.psbase.PreserveWhitespace = $true
$xmlDemoSettings.Load($demoSettingsFile)

[string] $solutionWorkingDir = $xmlDemoSettings.configuration.localPaths.endSolutionWorkingDir

popd
# "========= Main Script =========" #

write-host
write-host
write-host "========= Importing Windows Azure publish settings file...  ========="
Import-AzurePublishSettingsFile $publishSettingsFilePath
Select-AzureSubscription $subscriptionName
write-host "Importing Windows Azure publish settings file Done!"


write-host
write-host
write-host "========= Removing cloud service...  =========" -ForegroundColor Yellow
Remove-AzureService "$cloudServiceName" -Force  -ErrorAction Stop
write-host "Removing cloud service Done!"


write-host
write-host
write-host "========= Removing Windows Azure storage account for diagnostics...  =========" -ForegroundColor Yellow
Remove-AzureStorageAccount $diagnosticsStorageAccountName
write-host "Removing Windows Azure storage account for diagnostics Done!"


write-host
write-host
write-host "========= Removing Windows Azure SQL Database Server...  =========" -ForegroundColor Yellow
Remove-AzureSqlDatabaseServer $databaseServerName -Force
write-host "Removing Windows Azure SQL Database Server Done!"


write-host
write-host
write-host "========= Updating the Config.Azure.xml file...  =========" -ForegroundColor Yellow
$xmlDemoSettingsAzure.configuration.sqlDatabase.serverName = ""

$xmlDemoSettingsAzure.Save($demoSettingsAzureFile)
write-host "Updating the Config.Azure.xml file Done!"


write-host
write-host
write-host "========= Removing begin solution for Segment 5 from working directory... ========="
if (Test-Path "$solutionWorkingDir")
{
    Remove-Item "$solutionWorkingDir" -recurse -force
}
write-host "Removing begin solution for Segment 5 from working directory done!"