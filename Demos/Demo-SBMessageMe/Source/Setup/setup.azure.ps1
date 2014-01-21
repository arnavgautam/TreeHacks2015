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
[string] $queuename = $xmlAzureSettings.configuration.queuename

  
#load the storage client in order to do our operations
$sblibs = Get-ChildItem "$env:ProgramFiles\Microsoft SDKs\Windows Azure\.NET SDK\*\ref\Microsoft.ServiceBus.dll"
$sblib = $sblibs[0].FullName
& ".\tasks\reset-azure-queue.ps1" -sblib $sblib -connectionString $serviceBusConnectionString -queue $queuename
