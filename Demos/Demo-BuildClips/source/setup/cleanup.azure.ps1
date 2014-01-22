Param([string] $demoSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# Import Windows Azure PowerShell modules
Get-ChildItem "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows Azure\PowerShell\Azure\*.psd1" | ForEach-Object {Import-Module $_ | Out-Null}

# "========= Initialization =========" #
pushd ".."
# Get settings from user configuration file
if($demoSettingsFile -eq $null -or $demoSettingsFile -eq "")
{
    $demoSettingsFile = "Config.Azure.xml"
}

[xml] $xmlDemoSettings = Get-Content $demoSettingsFile

[string] $publishSettingsFilePath = $xmlDemoSettings.configuration.publishSettingsFilePath
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
[string] $publishSettingsFilePath = Resolve-Path $xmlDemoSettings.configuration.publishSettingsFilePath
[string] $subscriptionName = $xmlDemoSettings.configuration.subscriptionName

[string] $websiteName = $xmlDemoSettings.configuration.websiteName


popd
# "========= Main Script =========" #

write-host
write-host
write-host "========= Importing Azure publish settings file...  ========="
Import-AzurePublishSettingsFile $publishSettingsFilePath
Select-AzureSubscription $subscriptionName
write-host "Importing Azure publish settings file Done!"


write-host
write-host
write-host "========= Removing Azure SQL Database Server associated to the Web site...  =========" -ForegroundColor Yellow
$website = Get-AzureWebsite "$websiteName"
if (!($website -eq $null))
{
    [string] $connectionString = $website.ConnectionStrings.ConnectionString

    if (!($connectionString -eq $null -or $connectionString -eq ""))
    {
        $start = $connectionString.IndexOf(':') + 1
        $databaseServerName = $connectionString.Substring( $start, $connectionString.IndexOf('.') - $start)

        Remove-AzureSqlDatabaseServer $databaseServerName -Force
    }
    else
    {
        write-host "There is no SQL server associated to the Web site."
    }
}
write-host "Removing Azure SQL Database Server associated to the Web site Done!"
