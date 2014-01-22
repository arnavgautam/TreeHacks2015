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

$appConfigPath = "$demoPath\Assets\MessageReceiver\App.Config";
$serviceBusConnectionString = "Endpoint=sb://$namespace.servicebus.windows.net;SharedSecretIssuer=$issuer;SharedSecretValue=$secretKey"

# -----------------------------------
# Updating App.Config file
# -----------------------------------
Write-Output ""
Write-Output "Updating Console Application Configuration..."

UpdateAppSetting $appConfigPath "Microsoft.ServiceBus.ConnectionString" $serviceBusConnectionString
