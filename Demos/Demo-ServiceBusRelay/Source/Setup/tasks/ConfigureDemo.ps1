param([string]$demoPath, [string]$namespace, [string]$issuer, [string]$secretKey)

function UpdateAppSetting($configurationFile, $keyName, $value)
{
	[xml]$xml = get-content $configurationFile;

	$keySetting = $xml.configuration.appSettings.Add | where { $_.key -eq $keyName }
    
    if ($keySetting -eq $null)
    {
        $keySetting = $xml.CreateElement("Add");
        $keySetting.SetAttribute("value", $value);
        $xml.configuration.appSettings.AppendChild($keySetting);
    } else {
		$keySetting.SetAttribute("value", $value);
	}	
	
	$xml.Save($configurationFile);
}

function UpdateConfigurationSetting($configurationFile, $settingKey, $value)
{
    [xml]$xml = get-content $configurationFile;

	$entry = $xml.ServiceConfiguration.Role.ConfigurationSettings.Setting | Where-Object { $_.name -match $settingKey }
	$entry.value = $value 

    $xml.Save($configurationFile);
}

$appConfigPath = "$demoPath\ServiceBusRelay-Console\ServiceBusRelay.Console\App.Config";
$serviceConfigurationLocalPath = "$demoPath\ServiceBusRelay-Web\ServiceBusRelay.Web.Azure\ServiceConfiguration.Local.cscfg";
$serviceConfigurationCloudPath = "$demoPath\ServiceBusRelay-Web\ServiceBusRelay.Web.Azure\ServiceConfiguration.Cloud.cscfg";
$webConfigPath = "$demoPath\ServiceBusRelay-Web\ServiceBusRelay.Web\Web.config";
$serviceBusConnectionString = "Endpoint=sb://$namespace.servicebus.windows.net;SharedSecretIssuer=$issuer;SharedSecretValue=$secretKey"

# -----------------------------------
# Updating App.Config file
# -----------------------------------
Write-Output ""
Write-Output "Updating Console Application Configuration..."

UpdateAppSetting $appConfigPath "Microsoft.ServiceBus.ConnectionString" $serviceBusConnectionString

# ------------------------------------------ 
# Updating Service Configuration local file
# ------------------------------------------
Write-Output ""
Write-Output "Updating Local Service Configuration..."

UpdateConfigurationSetting $serviceConfigurationLocalPath "Microsoft.ServiceBus.ConnectionString" $serviceBusConnectionString

# ------------------------------------------ 
# Updating Service Configuration cloud file
# ------------------------------------------
Write-Output ""
Write-Output "Updating Cloud Service Configuration..."

UpdateConfigurationSetting $serviceConfigurationCloudPath "Microsoft.ServiceBus.ConnectionString" $serviceBusConnectionString

# -----------------------------
# Updating web.config
# -----------------------------
Write-Output ""
Write-Output "Updating Web.config..."

UpdateAppSetting $webConfigPath "Microsoft.ServiceBus.ConnectionString" $serviceBusConnectionString