Param([string] $azureNodeSDKDir,
	[string] $wazPublishSettings,
	[string] $azureServiceName)
	
$ensureDeployments = @(
	@{
		"Slot" = 'production';
	},
    @{
		"Slot" = 'staging';
	}
)

# "========= Main Script =========" #
write-host "========= Delete Cloud Service...  ========= "	

write-host "========= Importing the Windows Azure Management Module... ========="
# Ensure that we are loading the Azure module from the correct folder
Import-Module "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows Azure\PowerShell\Azure\Azure.psd1"

write-host "========= Importing the Windows Azure Subscription Settings File... ========="
$wazPublishSettings = Resolve-Path $wazPublishSettings
Import-AzurePublishSettingsFile $wazPublishSettings
write-host "Importing the Windows Azure Subscription Settings File done!"

write-host "========= Removing all cloud service deployments ... ========="
foreach ($deploy in $ensureDeployments){
	write-host "========= Remove Cloud Service $deploy["Slot"] deployment... ========="
	if (Get-AzureDeployment -ServiceName $azureServiceName -Slot $deploy["Slot"])
	{
		# Create Storage Service
		Remove-AzureDeployment -ServiceName $azureServiceName -Slot $deploy["Slot"] -Force | out-null
		write-host "Cloud Service removed!"
	}
}
write-host "========= Removed all cloud service deployments ... ========="
if($azureNodeSDKDir) {
	$azureCmd = (Join-Path $azureNodeSDKDir "node.exe") + " " + (Join-Path $azureNodeSDKDir "bin\azure")
	}
else {
	$azureCmd = "azure"
	}

Invoke-Expression ("$azureCmd service delete " + $azureServiceName)

write-host "========= Deleting Cloud Service done! ========= "
