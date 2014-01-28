	
Set-AzureSubscription '[SUBSCRIPTION-NAME]' -CurrentStorageAccount '[STORAGE-ACCOUNT-NAME]'
Select-AzureSubscription '[SUBSCRIPTION-NAME]'

$svcName = '[SERVICE-NAME]'

# Restore the VM 
$iisvm1 = Import-AzureVM -Path 'C:\Temp\iisvm1.xml'
$iisvm2 = Import-AzureVM -Path 'C:\Temp\iisvm2.xml' 

New-AzureVM -ServiceName $svcName -VMs $iisvm1,$iisvm2
