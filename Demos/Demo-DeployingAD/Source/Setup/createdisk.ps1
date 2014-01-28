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


[xml]$xml = Get-Content $configFile

Set-AzureSubscription -SubscriptionName $xml.configuration.subscriptionName -CurrentStorageAccount $xml.configuration.storageAccount.name
Select-AzureSubscription -SubscriptionName $xml.configuration.subscriptionName

#Remove-AzureService -ServiceName $xml.configuration.cloudServiceName1 -Force -ErrorAction SilentlyContinue

[string] $storageAccount = $xml.configuration.storageAccount.name
[string] $storageKey = $xml.configuration.storageAccount.key
[string] $storageContainer = $xml.configuration.storageAccount.container

$addcosdisk = "http://" + $xml.configuration.storageAccount.name + ".blob.core.windows.net" + "/" + $storageContainer + "/adosdisk.vhd"


if((CheckLease $addcosdisk) -eq $false)
{
	return 
}



# Create disk references
Add-AzureDisk -DiskName 'ad-dcosdisk' -MediaLocation $addcosdisk -OS Windows 





		