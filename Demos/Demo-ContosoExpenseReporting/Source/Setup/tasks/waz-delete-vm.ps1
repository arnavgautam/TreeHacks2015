Param([string] $wazVMHostName,
      [string] $wazPublishSettings)


write-host "========= Deleting Windows Azure VM ... ========="

# Ensure that we are loading the Azure module from the correct folder
Import-Module "${env:ProgramFiles(x86)}\Microsoft SDKs\Windows Azure\PowerShell\Azure\Azure.psd1"

# Importing the Windows Azure Subscription Settings File
$wazPublishSettings = Resolve-Path $wazPublishSettings
Import-AzurePublishSettingsFile $wazPublishSettings

$vmToRemove = Get-AzureVM | Where { $_.ServiceName -eq $wazVMHostName }

if ($vmToRemove -eq $null) {
    write-host "The specified VM to remove ($wasVMHostName) could not be found. Please verify the configured value in the settings file for this script."
}
else {
    $vmToRemove | Remove-AzureVM
    write-host "Deleting Windows Azure VMs done!"
}



