Param([string] $localSettingsFile, [string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
if(-not $localSettingsFile)
{
	$localSettingsFile = "config.local.xml"
}
[xml] $xmlLocalSettings = Get-Content $localSettingsFile
[string] $namespace = $xmlLocalSettings.Configuration.ServiceBus.Namespace;
[string] $issuer = $xmlLocalSettings.Configuration.ServiceBus.Issuer;
[string] $secretKey = $xmlLocalSettings.Configuration.ServiceBus.SecretKey;
$serviceBusConnectionString = "Endpoint=sb://$namespace.servicebus.windows.net;SharedSecretIssuer=$issuer;SharedSecretValue=$secretKey"

if(-not $azureSettingsFile)
{
	$azureSettingsFile = "config.azure.xml"
}
[xml] $xmlAzureSettings = Get-Content $azureSettingsFile
[string] $wazPublishSettings = $xmlAzureSettings.configuration.windowsAzureSubscription.publishSettingsFile
[string] $publishProfileDownloadUrl = $xmlAzureSettings.configuration.urls.publishProfileDownloadUrl
[string] $webSitesToDelete = $xmlAzureSettings.configuration.windowsAzureSubscription.webSitesToDelete
[string] $queuename = $xmlAzureSettings.configuration.queuename

# "========= Main Script =========" #

if (-not ($wazPublishSettings) -or -not (test-path $wazPublishSettings)) {
    Write-Error "You must specify the publish setting profile. After downloading the publish settings profile from the management portal, specify the file location in the configuration file path under the publishSettingsFile element."
	Write-Host "You should save the publish setting profile into a known and safe location to avoid being removed. Then configure the publishSettingFile in the Config.Azure.xml file."
	
    start $publishProfileDownloadUrl
    return
}

#========= Importing the Windows Azure Subscription Settings File... =========
& ".\tasks\import-waz-publishsettings.ps1" -wazPublishSettings $wazPublishSettings

#========= Deleting Configured Windows Azure Web Sites... =========
& ".\tasks\waz-delete-websites.ps1" -webSitesToDelete $webSitesToDelete

#========= Deleting Configured Windows Azure Service Bus Queues... =========
$sblibs = Get-ChildItem "$env:ProgramFiles\Microsoft SDKs\Windows Azure\.NET SDK\*\ref\Microsoft.ServiceBus.dll"
$sblib = $sblibs[0].FullName
& ".\tasks\remove-azure-queue.ps1" -sblib $sblib -connectionString $serviceBusConnectionString -queue $queuename
