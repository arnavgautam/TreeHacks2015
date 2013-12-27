Param(
    [Parameter(Mandatory = $true)]
    [String]$Name,
    
    [String]$Location = "West US"
)

# Create a new storage account
Write-Verbose ("[Start] creating storage account {0} in location {1}" -f $Name, $Location)
New-AzureStorageAccount -StorageAccountName $Name -Location $Location -Verbose
Write-Verbose ("[Finish] creating storage account {0} in location {1}" -f $Name, $Location)

# Get the access key of the storage account
$key = Get-AzureStorageKey -StorageAccountName $Name

# Generate the connection string of the storage account
$connectionString = "BlobEndpoint=http://{0}.blob.core.windows.net/;QueueEndpoint=http://{0}.queue.core.windows.net/;TableEndpoint=http://{0}.table.core.windows.net/;AccountName={0};AccountKey={1}" -f $Name, $key.Primary

Return @{AccountName = $Name; AccessKey = $key.Primary; ConnectionString = $connectionString}