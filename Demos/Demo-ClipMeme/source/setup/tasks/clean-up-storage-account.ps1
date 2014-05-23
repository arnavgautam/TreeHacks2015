Param([string] $containerName, [string] $tableName, [string] $storageConnectionString, [string] $directory)

$scriptDir = (split-path $myinvocation.mycommand.path -parent) + "\storageClient"

pushd $scriptDir

[Reflection.Assembly]::LoadFile((Get-Item 'System.Spatial.dll').FullName)
[Reflection.Assembly]::LoadFile((Get-Item 'Microsoft.Data.Edm.dll').FullName)
[Reflection.Assembly]::LoadFile((Get-Item 'Microsoft.Data.Odata.dll').FullName)
[Reflection.Assembly]::LoadFile((Get-Item 'Microsoft.WindowsAzure.Storage.dll').FullName)
[Reflection.Assembly]::LoadFile((Get-Item 'Microsoft.Data.Services.Client.dll').FullName)

# $connectionStringMask = "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}"
# $connectionString = [string]::Format($connectionStringMask, $storageAccount, $storageAccountKey)

$connectionString = $storageConnectionString

$azureStorage = [Microsoft.WindowsAzure.Storage.CloudStorageAccount]::Parse($connectionString)

$blobClient = $azureStorage.CreateCloudBlobClient();
$tableClient = $azureStorage.CreateCloudTableClient()

$cloudTable = $tableClient.GetTableReference($tableName)
$container = $blobClient.GetContainerReference($containerName)

if(-not $container)
{
    Write-Output "Blob Container not found, exiting."
    return;
}

if(-not $cloudTable){
    Write-Output "Cloud Table not found, exiting."
    return;
}

# Get white-listed blobs' name
$whiteListedBlobs = @()
Foreach ($file in Get-ChildItem $directory){
    $whiteListedBlobs += $file.Name;
}

# get all names from the blobs in container
$blobs = $container.ListBlobs()
$blobsInStorage = @();

foreach ($blob in $blobs)
{
    $blobName = $blob.Name
    $blobExists = $whiteListedBlobs -contains $blobName

    if(!$blobExists)
    {
        Write-Output "Deleting $blobName"
        $blob.Delete()
    }
}

$entity = New-Object Microsoft.WindowsAzure.Storage.Table.TableEntity
$query = New-Object Microsoft.WindowsAzure.Storage.Table.TableQuery
$query.FilterString = ""

$tableRows = $cloudTable.ExecuteQuery($query);

foreach ($row in $tableRows)
{
    $blobName = $row.Item("BlobName").StringValue
    $rowExists = $whiteListedBlobs -contains $blobName

    if(!$rowExists)
    {
        Write-Output "Deleting $blobName"
        $deleteOperation = [Microsoft.WindowsAzure.Storage.Table.TableOperation]::Delete($row)
        $cloudTable.Execute($deleteOperation)
    }
}


popd