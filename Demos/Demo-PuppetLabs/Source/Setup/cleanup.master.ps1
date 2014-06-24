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

$PublishSettingsFile = Join-Path $scriptDir ".\assets\publishSettings\azure.publishsettings"

popd

#Invoke Puppet VM Creation if they dont exists
# Invoke-Expression ".\tasks\puppetVMsCreation.ps1 -azureSubscription `"$azureSubscription`" -dclocation `"$dclocation`" -storageAccountName `"$storageAccount`" -adminUserName `"$adminUserName`" -adminPassword `"$adminPassword`" -masterCloudServiceName `"$masterCloudServiceName`" -masterVMName `"$masterVMName`" -agentCloudServiceName `"$agentCloudServiceName`" -agentVMName `"$agentVMName`""

Get-AzureAccount | Remove-AzureAccount -Force

write-host "Importing Publish Settings file"
if ($publishSettingsFile) { 
	Import-AzurePublishSettingsFile -PublishSettingsFile $publishSettingsFile 
}

Select-AzureSubscription -Default $azureSubscription

#Configuration Values Verification
write-host "Verifying Services and VM names..."
$service = Test-AzureName -Service $masterCloudServiceName
if ($service) 
{ 
	Write-Host "There's already a CloudService named $masterCloudServiceName"   
	Exit
}

write-host "Creating Master Puppet Storage Account..."
$storage = Get-AzureStorageAccount -StorageAccountName "$storageAccount" -ErrorAction SilentlyContinue 
if($storage)
{ 
	Write-Host "There's already a storage account named $storageAccount for this subscription. The script will continue using the existing storage account."  
}
else
{
	New-AzureStorageAccount -Location $dclocation -StorageAccountName $storageAccount 
}

#Master Puppet VM Creation#
$imageName = "de89c2ed05c748f5aded3ddc75fdcce4__PuppetEnterpriseMaster-3_2_3-amd64-server-20140514-en-us-30GB"

write-host "Binding Master Puppet Storage Account with Subscription..."
Set-AzureSubscription -SubscriptionName $azureSubscription -CurrentStorageAccount $storageAccount

write-host "Creating Puppet Master Configuration..."
$vmConfig = New-AzureVMConfig -Name $masterVMName -InstanceSize Medium -ImageName $imageName | Add-AzureProvisioningConfig -Linux -LinuxUser $adminUserName -Password $adminPassword | Add-AzureEndpoint -Protocol tcp -LocalPort 443 -PublicPort 443 -Name 'HTTPS' | Add-AzureEndpoint -Protocol tcp -LocalPort 8140 -PublicPort 8140 -Name 'Puppet' | Add-AzureEndpoint -Protocol tcp -LocalPort 61613 -PublicPort 61613 -Name 'MCollective'

write-host "Provisioning Master Puppet VM..."
New-AzureVM -ServiceName $masterCloudServiceName -VMs $vmConfig -Location $dclocation 
Get-AzureVM -ServiceName $masterCloudServiceName -Name $masterVMName | Set-AzureEndpoint -Name ssh -Protocol tcp -PublicPort 22 -LocalPort 22 | Update-AzureVM

