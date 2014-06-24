Param([string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
pushd ".."

# Import required settings from config.local.xml
[xml] $xmlAzureSettings = Get-Content $azureSettingsFile

# General Settings 
[string] $azureSubscription = $xmlAzureSettings.configuration.generalSettings.azureSubscriptionName
[string] $dclocation = $xmlAzureSettings.configuration.generalSettings.location
[string] $adminUserName = $xmlAzureSettings.configuration.generalSettings.adminUserName
[string] $adminPassword = $xmlAzureSettings.configuration.generalSettings.adminPassword
[string] $storageAccount = $xmlAzureSettings.configuration.generalSettings.storageAccount

# Puppet Settings
[string] $masterCloudServiceName = $xmlAzureSettings.configuration.puppetMasterSettings.cloudServiceName
[string] $masterVMName = $xmlAzureSettings.configuration.puppetMasterSettings.vmName
[string] $agentCloudServiceName = $xmlAzureSettings.configuration.puppetAgentSettings.cloudServiceName
[string] $agentVMName = $xmlAzureSettings.configuration.puppetAgentSettings.vmName

popd

$service = Test-AzureName -Service $agentCloudServiceName
if ($service) 
{ 	
	Write-Host "There's already a CloudService named $agentCloudServiceName"
}

write-host "Creating Agent Puppet Storage Account..."
$storage = Get-AzureStorageAccount -StorageAccountName "$storageAccount" -ErrorAction SilentlyContinue 
if($storage)
{ 
	Write-Host "There's already a storage account named $storageAccount for this subscription. The script will continue using the existing storage account."  
}
else
{
	New-AzureStorageAccount -Location $dclocation -StorageAccountName $storageAccount 
}

#Puppet Agent VM Creation#
$imageName = "a699494373c04fc0bc8f2bb1389d6106__Windows-Server-2012-R2-201405.01-en.us-127GB.vhd"

write-host "Creating Agent VM Configuration..."
$vmConfig = New-AzureVMConfig -Name $agentVMName -InstanceSize Medium -ImageName $imageName | Add-AzureProvisioningConfig -Windows -AdminUser $adminUserName -Password $adminPassword | Add-AzureEndpoint -Protocol tcp -LocalPort 8140 -PublicPort 8140 -Name 'Puppet' | Add-AzureEndpoint -Protocol tcp -LocalPort 61613 -PublicPort 61613 -Name 'Orchestration' 
$vmConfig = Set-AzureVMPuppetExtension -VM $vmConfig -PuppetMasterServer "$masterCloudServiceName.cloudapp.net"

write-host "Provisioning Agent VM..."
New-AzureVM -ServiceName $agentCloudServiceName -VMs $vmConfig -Location $dclocation

write-host "Creating Remote Desktop file..."
$desktopPath = [Environment]::GetFolderPath("Desktop")
Get-AzureRemoteDesktopFile -ServiceName $agentCloudServiceName -Name $agentVMName -LocalPath "$desktopPath\PuppetAgentVM.rdp"
