	
Set-AzureSubscription '[SUBSCRIPTION-NAME]' -CurrentStorageAccount '[STORAGE-ACCOUNT-NAME]'
Select-AzureSubscription '[SUBSCRIPTION-NAME]'

$svcName = '[SERVICE-NAME]'

# Export the settings 
Export-AzureVM -ServiceName $svcName -Name 'iisvm1' -Path 'C:\temp\iisvm1.xml'
Export-AzureVM -ServiceName $svcName -Name 'iisvm2' -Path 'C:\temp\iisvm2.xml'

# Remove the VM 
Remove-AzureVM -ServiceName $svcName -Name 'iisvm1' 
Remove-AzureVM -ServiceName $svcName -Name 'iisvm2' 