Param([string] $configFile)

function CheckLease([string] $blobpath)
{
	$acctName = $xml.configuration.storageAccount.name
	$acctKey = $xml.configuration.storageAccount.key
	
    $creds = "DefaultEndpointsProtocol=http;AccountName=$($acctName);AccountKey=$($acctKey)"
	$acctobj = [Microsoft.WindowsAzure.Storage.CloudStorageAccount]::Parse($creds)
	$uri = $acctobj.Credentials.TransformUri($uri)
	$blobclient = New-Object Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient($acctobj.BlobEndpoint, $acctobj.Credentials)
	$blobclient.ServerTimeout = (New-TimeSpan -Minutes 1)
	$blob = New-Object Microsoft.WindowsAzure.Storage.Blob.CloudPageBlob($blobpath, $acctobj.Credentials)
	
    try
	{
		$blob.FetchAttributes()
		Write-Host $blobpath " Lease Status: " $blob.Properties.LeaseStatus
		
		if($blob.Properties.LeaseStatus -eq [Microsoft.WindowsAzure.Storage.Blob.LeaseStatus]::Locked) 
		{
			write-host "Blob is Locked - Still Copying." -foregroundcolor "red"
			return $false
		}
		else
		{
			Write-Host "Blob has completed copying." -ForegroundColor "green"
		}
	}
	catch
	{
		Write-Host $blobpath " does not exist." -ForegroundColor "red"
		
		return $false
	}
	return $true
}

Import-Module 'C:\Program Files (x86)\Microsoft SDKs\Windows Azure\PowerShell\Azure\Azure.psd1' 
 
[xml]$xml = Get-Content $configFile

Set-AzureSubscription -SubscriptionName $xml.configuration.subscriptionName -CurrentStorageAccount $xml.configuration.storageAccount.name
Select-AzureSubscription -SubscriptionName $xml.configuration.subscriptionName

#Remove-AzureService -ServiceName $xml.configuration.cloudServiceName1 -Force -ErrorAction SilentlyContinue

[string] $storageAccount = $xml.configuration.storageAccount.name
[string] $storageKey = $xml.configuration.storageAccount.key
[string] $storageContainer = $xml.configuration.storageAccount.container

$iisvm1osdisk = "http://" + $xml.configuration.storageAccount.name + ".blob.core.windows.net" + "/" + $storageContainer + "/iisvm1.vhd"
$iisvm2osdisk = "http://" + $xml.configuration.storageAccount.name + ".blob.core.windows.net" + "/" + $storageContainer + "/iisvm2.vhd"
$sqlvm1osdisk = "http://" + $xml.configuration.storageAccount.name + ".blob.core.windows.net" + "/" + $storageContainer + "/sqlvm1.vhd"


if((CheckLease $iisvm1osdisk) -eq $false)
{
	return 
}

if((CheckLease $iisvm2osdisk) -eq $false)
{
	return
}
if((CheckLease $sqlvm1osdisk) -eq $false)
{
	return
}


# Create disk references
Add-AzureDisk -DiskName 'iisvm1osdisk' -MediaLocation $iisvm1osdisk -OS Windows 
Add-AzureDisk -DiskName 'iisvm2osdisk' -MediaLocation $iisvm2osdisk -OS Windows
Add-AzureDisk -DiskName 'sqlvm1osdisk' -MediaLocation $sqlvm1osdisk -OS Windows


# Create VMs	
$iisvm1 = New-AzureVMConfig -Name 'iisvm1' -InstanceSize Small -DiskName 'iisvm1osdisk' |
	Add-AzureEndpoint -Name RDP -LocalPort 3389 -Protocol tcp |
	Add-AzureEndpoint -Name web -LocalPort 80 -PublicPort 80 -Protocol tcp -LBSetName web -ProbePath '/' -ProbeProtocol http -ProbePort 80

$iisvm2 = New-AzureVMConfig -Name 'iisvm2' -InstanceSize Small -DiskName 'iisvm2osdisk'  |
	Add-AzureEndpoint -Name RDP -LocalPort 3389 -Protocol tcp |
	Add-AzureEndpoint -Name web -LocalPort 80 -PublicPort 80 -Protocol tcp -LBSetName web -ProbePath '/' -ProbeProtocol http -ProbePort 80 

$sqlvm1 = New-AzureVMConfig -Name 'sqlvm1' -InstanceSize Medium -DiskName 'sqlvm1osdisk' |
	Add-AzureEndpoint -Name RDP -LocalPort 3389 -Protocol tcp |
	Add-AzureDataDisk -CreateNew -DiskSizeInGB 100 -DiskLabel 'data' -LUN 0 |
	Add-AzureDataDisk -CreateNew -DiskSizeInGB 100 -DiskLabel 'logs' -LUN 1 
	
$location = Get-AzureStorageAccount -StorageAccountName $xml.configuration.storageAccount.name | select -ExpandProperty Location

New-AzureVM -ServiceName $xml.configuration.cloudServiceName1 -VMs $iisvm1, $iisvm2, $sqlvm1 -Location $location

Write-Host "If you received internal server errors the blobs may not have finished copying yet."


		