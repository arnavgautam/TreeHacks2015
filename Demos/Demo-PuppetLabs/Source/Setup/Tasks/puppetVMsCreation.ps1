$dclocation = "West US"
$cloudServiceName = "pspuppetservice"
$vmName = "falvpspuppet"
$adminUserName = "useradmin"
$adminPassword = "Passw0rd!"

$storageAccountName = "falvpsstorage"
$azureSubscription = "Azdem169F29744N"

$imageName = "de89c2ed05c748f5aded3ddc75fdcce4__PuppetEnterpriseMaster-3_2_3-amd64-server-20140514-en-us-30GB"

Select-AzureSubscription $azureSubscription

write-host "Creating Master Puppet Storage Account"
New-AzureStorageAccount -Location $dclocation -StorageAccountName $storageAccountName 

write-host "Binding Master Puppet Storage Account with Subscription"
Set-AzureSubscription -SubscriptionName $azureSubscription -CurrentStorageAccount $storageAccountName

write-host "Creating Puppet Master Configuration"
$vmConfig = New-AzureVMConfig -Name $vmName -InstanceSize Medium -ImageName $imageName | Add-AzureProvisioningConfig -Linux -LinuxUser $adminUserName -Password $adminPassword | Add-AzureEndpoint -Protocol tcp -LocalPort 443 -PublicPort 443 -Name 'HTTPS' | Add-AzureEndpoint -Protocol tcp -LocalPort 8140 -PublicPort 8140 -Name 'Puppet' | Add-AzureEndpoint -Protocol tcp -LocalPort 61613 -PublicPort 61613 -Name 'MCollective' -Verbose

write-host "Provisioning Master Puppet VM"
New-AzureVM -ServiceName $cloudServiceName -VMs $vmConfig -Location $dclocation

#####################

$storageAccountName = "falvpsagent2"
$azureSubscription = "Azdem169F29744N"
$vmName = "falvpsagent2"
$cloudAgentServiceName = "psagentservice2"

write-host "Creating Storage Account"
New-AzureStorageAccount -Location $dclocation -StorageAccountName $storageAccountName 

Set-AzureSubscription -SubscriptionName $azureSubscription -CurrentStorageAccount $storageAccountName

$imageName = "a699494373c04fc0bc8f2bb1389d6106__Windows-Server-2012-R2-201405.01-en.us-127GB.vhd"

write-host "Creating Agent VM Configuration"
$vmConfig = New-AzureVMConfig -Name $vmName -InstanceSize Medium -ImageName $imageName | Add-AzureProvisioningConfig -Windows -AdminUser $adminUserName -Password $adminPassword | Add-AzureEndpoint -Protocol tcp -LocalPort 8140 -PublicPort 8140 -Name 'Puppet' | Add-AzureEndpoint -Protocol tcp -LocalPort 61613 -PublicPort 61613 -Name 'Orchestration' -Verbose
$vmConfig = Set-AzureVMPuppetExtension -VM $vmConfig -PuppetMasterServer "$cloudServiceName.cloudapp.net"

write-host "Provisioning Agent VM"
New-AzureVM -ServiceName $cloudAgentServiceName -VMs $vmConfig -Location $dclocation

