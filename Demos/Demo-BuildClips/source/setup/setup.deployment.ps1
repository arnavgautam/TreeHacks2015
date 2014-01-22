Param([string] $demoSettingsAzureFile, [string] $demoSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# Import Windows Azure PowerShell modules
Get-ChildItem "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows Azure\PowerShell\Azure\*.psd1" | ForEach-Object {Import-Module $_ | Out-Null}

# "========= Initialization =========" #
pushd ".."
# Get settings from user configuration file for azure
if($demoSettingsAzureFile -eq $null -or $demoSettingsAzureFile -eq "")
{
    $demoSettingsAzureFile = Resolve-Path "Config.Azure.xml"
}

$xmlDemoSettingsAzure = New-Object xml
$xmlDemoSettingsAzure.psbase.PreserveWhitespace = $true
$xmlDemoSettingsAzure.Load($demoSettingsAzureFile)

[string] $publishSettingsFilePath = $xmlDemoSettingsAzure.configuration.publishSettingsFilePath
if ($publishSettingsFilePath -eq "" -or $publishSettingsFilePath -eq $null -or !(Test-Path $publishSettingsFilePath))
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
[string] $diagnosticsStorageAccountLocation = $xmlDemoSettingsAzure.configuration.storageAccounts.diagnosticsStorageAccount.location


[string] $cloudServiceName = $xmlDemoSettingsAzure.configuration.cloudService.name
[string] $cloudServiceLocation = $xmlDemoSettingsAzure.configuration.cloudService.location

[string] $sqlUsername = $xmlDemoSettingsAzure.configuration.sqlDatabase.username
[string] $sqlPassword = $xmlDemoSettingsAzure.configuration.sqlDatabase.password
[string] $sqlDBname = $xmlDemoSettingsAzure.configuration.sqlDatabase.name
[string] $sqlLocation = $xmlDemoSettingsAzure.configuration.sqlDatabase.location
[string] $sqlServerName = $xmlDemoSettingsAzure.configuration.sqlDatabase.serverName


# Get settings from user configuration file
if($demoSettingsFile -eq $null -or $demoSettingsFile -eq "")
{
    $demoSettingsFile = Resolve-Path "Config.Local.xml"
}

$xmlDemoSettings = New-Object xml
$xmlDemoSettings.psbase.PreserveWhitespace = $true
$xmlDemoSettings.Load($demoSettingsFile)

# Import required settings from config.local.xml if neccessary #
[string] $workingDir = $xmlDemoSettings.configuration.localPaths.workingDir
[string] $solutionWorkingDir = $xmlDemoSettings.configuration.localPaths.endSolutionWorkingDir
[string] $solutionDir = Resolve-Path $xmlDemoSettings.configuration.localPaths.endSolutionDir

[string] $webConfig = $xmlDemoSettings.configuration.localPaths.webConfig
[string] $webReleaseConfig = $xmlDemoSettings.configuration.localPaths.webReleaseConfig
[string] $appConfig = $xmlDemoSettings.configuration.localPaths.appConfig
[string] $serviceConfiguration = $xmlDemoSettings.configuration.localPaths.serviceConfiguration
[string] $serviceDefinition = $xmlDemoSettings.configuration.localPaths.serviceDefinition

[string] $mediaServicesAccountName = $xmlDemoSettings.configuration.appSettings.mediaServicesAccountName
[string] $mediaServicesAccountKey = $xmlDemoSettings.configuration.appSettings.mediaServicesAccountKey
[string] $storageAccountConnectionString = $xmlDemoSettings.configuration.appSettings.storageAccountConnectionString
[string] $serviceBusConnectionString = $xmlDemoSettings.configuration.appSettings.serviceBusConnectionString

[string] $facebookApplicationId = $xmlDemoSettings.configuration.appSettings.cloudService.facebookApplicationId
[string] $facebookApplicationSecret = $xmlDemoSettings.configuration.appSettings.cloudService.facebookApplicationSecret
[string] $twitterConsumerKey = $xmlDemoSettings.configuration.appSettings.cloudService.twitterConsumerKey
[string] $twitterConsumerSecret = $xmlDemoSettings.configuration.appSettings.cloudService.twitterConsumerSecret

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
write-host "========= Creating Windows Azure storage account for diagnostics...  =========" -ForegroundColor Yellow
$storageAccountExists = $false
Get-AzureStorageAccount | ForEach-Object { if ($_.StorageAccountName -eq "$diagnosticsStorageAccountName") { $storageAccountExists = $true }}
if (!$storageAccountExists)
{
    $result = New-AzureStorageAccount "$diagnosticsStorageAccountName" -Location "$diagnosticsStorageAccountLocation"

    if($result -eq $null -or $result -eq "")
    {
        exit 2
    }
}

$diagnosticsStorageAccountKeys = Get-AzureStorageKey "$diagnosticsStorageAccountName"
[string] $diagnosticsPrimaryKey = $diagnosticsStorageAccountKeys.Primary

$diagnosticsStorageAccountConnectionString = "DefaultEndpointsProtocol=https;AccountName=$diagnosticsStorageAccountName;AccountKey=$diagnosticsPrimaryKey"
write-host "Creating Windows Azure storage account for diagnostics Done!"


write-host
write-host
write-host "========= Creating cloud service for deployment...  =========" -ForegroundColor Yellow
$cloudServiceExists = $false
Get-AzureService | ForEach-Object { if ($_.ServiceName -eq "$cloudServiceName") { $cloudServiceExists = $true }}
if (!$cloudServiceExists)
{
    $result = New-AzureService "$cloudServiceName" -Location "$cloudServiceLocation"

    if($result -eq $null -or $result -eq "")
    {
        exit 3
    }
} else {
    $deployment = Get-AzureDeployment "$cloudServiceName" -Slot Production -ErrorAction SilentlyContinue
    if($deployment)
    {
        Write-Host "You already have a deployment in Production for the specified cloud service. Please remove the deployment and run this script again." -ForegroundColor Red
        exit 4
    }
}
[string] $apiBaseUrl = "http://$cloudServiceName.cloudapp.net/"
write-host "Creating cloud service for deployment Done!"


write-host
write-host
write-host "========= Creating SQL server for deployment...  =========" -ForegroundColor Yellow
if($sqlServerName -eq $null -or $sqlServerName -eq ""){
    $sqlServer = New-AzureSqlDatabaseServer -AdministratorLogin "$sqlUsername" -AdministratorLoginPassword "$sqlPassword" -Location "$sqlLocation" -Force -ErrorAction Stop
    [string] $sqlServerName = $sqlServer.ServerName
    New-AzureSqlDatabaseServerFirewallRule -ServerName $sqlServerName -RuleName "All" -StartIpAddress "0.0.0.0" -EndIpAddress "255.255.255.255" -Force

    $secpasswd = ConvertTo-SecureString "$sqlPassword" -AsPlainText -Force
    $creds = New-Object System.Management.Automation.PSCredential ("$sqlUsername", $secpasswd)
    $ctx = New-AzureSqlDatabaseServerContext -ServerName $sqlServerName -Credential $creds
    $database = New-AzureSqlDatabase $ctx $sqlDBname
}

$dbConnectionString = "Server=tcp:$sqlServerName.database.windows.net,1433;Database=$sqlDBname;User ID=$sqlUsername@$sqlServerName;Password=$sqlPassword;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"
write-host "Creating SQL server for deployment Done!"


write-host
write-host
write-host "========= Updating the Config.Azure.xml file...  =========" -ForegroundColor Yellow
$xmlDemoSettingsAzure.configuration.sqlDatabase.serverName = $sqlServerName

$xmlDemoSettingsAzure.Save($demoSettingsAzureFile)
write-host "Updating the Config.Azure.xml file Done!"


################# Begin deployment solution configuration ################# 

if (!(Test-Path "$workingDir"))
{
    New-Item "$workingDir" -type directory | Out-Null
}

if (!(Test-Path "$solutionWorkingDir"))
{
    write-host
    write-host
    write-host "========= Copying Begin solution for Segment 5 to working directory... ========="
    New-Item "$solutionWorkingDir" -type directory | Out-Null
    Copy-Item "$solutionDir\*" "$solutionWorkingDir" -recurse -Force
    write-host "Copying Begin solution to working directory done!"
}


write-host
write-host
write-host "========= Update Web.config... ========="
$webConfigFilePath = Join-Path $solutionWorkingDir $webConfig
# Begin updating Web.config file
[string] $webConfigFile = Resolve-Path $webConfigFilePath 
$xml = New-Object xml
$xml.psbase.PreserveWhitespace = $true
$xml.Load($webConfigFile)
$xml.SelectNodes("//connectionStrings/add[@name = 'DefaultConnection']").setAttribute("connectionString", $dbConnectionString)

$xml.SelectNodes("//appSettings/add[@key = 'FacebookApplicationId']").setAttribute("value", $facebookApplicationId)
$xml.SelectNodes("//appSettings/add[@key = 'FacebookApplicationSecret']").setAttribute("value", $facebookApplicationSecret)
$xml.SelectNodes("//appSettings/add[@key = 'TwitterConsumerKey']").setAttribute("value", $twitterConsumerKey)
$xml.SelectNodes("//appSettings/add[@key = 'TwitterConsumerSecret']").setAttribute("value", $twitterConsumerSecret)

$xml.SelectNodes("//appSettings/add[@key = 'MediaServicesAccountName']").setAttribute("value", $mediaServicesAccountName)
$xml.SelectNodes("//appSettings/add[@key = 'MediaServicesAccountKey']").setAttribute("value", $mediaServicesAccountKey)
$xml.SelectNodes("//appSettings/add[@key = 'ServiceBusConnectionString']").setAttribute("value", $serviceBusConnectionString)

$xml.Save($webConfigFile)
# End updating Web.config file
write-host "Update Web.config done!"


write-host
write-host
write-host "========= Update app.config... ========="
$appConfigFilePath = Join-Path $solutionWorkingDir $appConfig
# Begin updating app.config file
[string] $appConfigFile = Resolve-Path $appConfigFilePath 
$xml = New-Object xml
$xml.psbase.PreserveWhitespace = $true
$xml.Load($appConfigFile)
$xml.SelectNodes("//connectionStrings/add[@name = 'DefaultConnection']").setAttribute("connectionString", $dbConnectionString)

$xml.SelectNodes("//appSettings/add[@key = 'MediaServicesAccountName']").setAttribute("value", $mediaServicesAccountName)
$xml.SelectNodes("//appSettings/add[@key = 'MediaServicesAccountKey']").setAttribute("value", $mediaServicesAccountKey)
$xml.SelectNodes("//appSettings/add[@key = 'ApiBaseUrl']").setAttribute("value", $apiBaseUrl)

$xml.Save($appConfigFile)
# End updating app.config file
write-host "Update app.config done!"


write-host
write-host
write-host "========= Update Web.Release.config... ========="
$webConfigReleaseFilePath = Join-Path $solutionWorkingDir $webReleaseConfig
# Begin updating Web.Release.config file
[string] $webConfigReleaseFile = Resolve-Path $webConfigReleaseFilePath 
$xml = New-Object xml
$xml.psbase.PreserveWhitespace = $true
$xml.Load($webConfigReleaseFile)
$xml.SelectNodes("//appSettings/add[@key = 'FacebookApplicationId']").setAttribute("value", $facebookApplicationId)
$xml.SelectNodes("//appSettings/add[@key = 'FacebookApplicationSecret']").setAttribute("value", $facebookApplicationSecret)
$xml.SelectNodes("//appSettings/add[@key = 'TwitterConsumerKey']").setAttribute("value", $twitterConsumerKey)
$xml.SelectNodes("//appSettings/add[@key = 'TwitterConsumerSecret']").setAttribute("value", $twitterConsumerSecret)

$xml.SelectNodes("//appSettings/add[@key = 'StorageConnectionString']").setAttribute("value", $storageAccountConnectionString)

$xml.Save($webConfigReleaseFile)
# End updating Web.Release.config file
write-host "Update Web.Release.config done!"


write-host
write-host
write-host "========= Update ServiceConfiguration.Cloud.cscfg... ========="
$serviceConfigurationFilePath = Join-Path $solutionWorkingDir $serviceConfiguration

# Begin updating ServiceConfiguration.Cloud.cscfg file
[string] $serviceConfigurationFile = Resolve-Path $serviceConfigurationFilePath 
$xml = New-Object xml
$xml.psbase.PreserveWhitespace = $true
$xml.Load($serviceConfigurationFile)

$xml.ServiceConfiguration.SelectNodes("*[@name = 'BuildClips']").ConfigurationSettings.SelectNodes("*[@name = 'Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString']").setAttribute("value", $diagnosticsStorageAccountConnectionString)
$xml.ServiceConfiguration.SelectNodes("*[@name = 'BuildClips']").ConfigurationSettings.SelectNodes("*[@name = 'Microsoft.WindowsAzure.Plugins.Caching.ConfigStoreConnectionString']").setAttribute("value", $storageAccountConnectionString)
$xml.ServiceConfiguration.SelectNodes("*[@name = 'BackgroundService']").ConfigurationSettings.SelectNodes("*[@name = 'Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString']").setAttribute("value", $diagnosticsStorageAccountConnectionString)

$xml.Save($serviceConfigurationFile)
# End updating ServiceConfiguration.Cloud.cscfg file
write-host "Update ServiceConfiguration.Cloud.cscfg done!"


write-host
write-host
write-host "========= Update ServiceDefinition.csdef... ========="
$serviceDefinitionFilePath = Join-Path $solutionWorkingDir $serviceDefinition

# Begin updating ServiceDefinition.csdef file
[string] $serviceDefinitionFile = Resolve-Path $serviceDefinitionFilePath 
$xml = New-Object xml
$xml.psbase.PreserveWhitespace = $true
$xml.Load($serviceDefinitionFile)

$xml.ServiceDefinition.WebRole.Endpoints.InputEndpoint.setAttribute("port", "80")

$xml.Save($serviceDefinitionFile)
# End updating ServiceDefinition.csdef file
write-host "Update ServiceDefinition.csdef done!"

################# End deployment solution configuration ################# 


write-host
write-host
write-host "========= Creating package to deploy... ========="
$MsBuildFile = Join-Path $env:windir "\Microsoft.NET\Framework\v4.0.30319\MsBuild.exe"
[string] $beginSolutionFile = Join-Path $solutionWorkingDir "BuildClips.Web\BuildClips.sln"
& $MsBuildFile @($beginSolutionFile, "/p:Configuration=Release", "/t:publish")
write-host "Creating package to deploy Done!"

write-host
write-host
write-host "========= Deploying the package... ========="
[string] $packagePath = Join-Path $solutionWorkingDir "BuildClips.Web\BuildClips.Azure\bin\Release\app.publish"
[string] $packageFile = Join-Path $packagePath "BuildClips.Azure.cspkg"
[string] $serviceConfigurationFile = Join-Path $packagePath "ServiceConfiguration.Cloud.cscfg"


Set-AzureSubscription "$subscriptionName" -CurrentStorageAccount $diagnosticsStorageAccountName

New-AzureDeployment -ServiceName "$cloudServiceName" -Package $packageFile -Configuration $serviceConfigurationFile -Slot "Production" -ErrorAction Stop
write-host "Deploying the package Done!"