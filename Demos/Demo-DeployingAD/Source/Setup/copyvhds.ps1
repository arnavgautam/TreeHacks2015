Param([string] $configFile)


# Get the key and account setting from configuration using namespace
[xml]$xml = Get-Content $configFile
[string] $storageAccount = $xml.configuration.storageAccount.name
[string] $storageKey = $xml.configuration.storageAccount.key
[string] $storageContainer = $xml.configuration.storageAccount.container


$xml.configuration.blobsToCopy.url | foreach {
   $pathArr = $_.Split("/")
   $fileName = $pathArr[$pathArr.Length - 1]
   $exe = "azure" 
   [Array]$params = "vm", "disk", "upload", $_, ("http://" + $storageAccount + ".blob.core.windows.net" + "/" + $storageContainer + "/" + $fileName), $storageKey
   & $exe $params; 
}
