Param([string] $configFile)

Import-Module 'C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\Azure\Azure.psd1' 

[xml]$xml = Get-Content $configFile

Set-AzureSubscription -SubscriptionName $xml.configuration.subscriptionName -CurrentStorageAccount $xml.configuration.storageAccount.name
Select-AzureSubscription -SubscriptionName $xml.configuration.subscriptionName

# Remove Everything Previously Created

Remove-AzureService -ServiceName $xml.configuration.cloudServiceName1 -Force
Remove-AzureService -ServiceName $xml.configuration.cloudServiceName2 -Force

Get-AzureDisk | Where { $_.DiskName -like '*iisvm1*' -and $_.AttachedTo -eq $null } | Remove-AzureDisk -DeleteVHD
Get-AzureDisk | Where { $_.DiskName -like '*iisvm2*' -and $_.AttachedTo -eq $null } | Remove-AzureDisk -DeleteVHD
Get-AzureDisk | Where { $_.DiskName -like '*sqlvm1*' -and $_.AttachedTo -eq $null } | Remove-AzureDisk -DeleteVHD

