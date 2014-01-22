Param([string] $wazStorageAccountName,
      [string] $wazPublishSettings)

write-host "========= Removing Windows Azure Storage Account ... ========="

# Ensure that we are loading the Azure module from the correct folder
Import-Module "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows Azure\PowerShell\Azure\Azure.psd1"

# Importing the Windows Azure Subscription Settings File
$wazPublishSettings = Resolve-Path $wazPublishSettings
Import-AzurePublishSettingsFile $wazPublishSettings

if (-Not ((Get-AzureStorageAccount | Where { $_.StorageAccountName -eq $wazStorageAccountName }) -eq $nul))
{
	Remove-AzureStorageAccount $wazStorageAccountName
}

write-host "Removing Windows Azure Storage Account done!"


