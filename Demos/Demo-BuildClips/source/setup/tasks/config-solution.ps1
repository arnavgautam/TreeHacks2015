param($webConfigFile, $webReleaseConfigFile, $win8ConfigJsFile, $configNode)

[string] $mediaServicesAccountName = $configNode.mediaServicesAccountName
[string] $mediaServicesAccountKey = $configNode.mediaServicesAccountKey
[string] $storageAccountConnectionString = $configNode.storageAccountConnectionString
[string] $serviceBusConnectionString = $configNode.serviceBusConnectionString

[string] $localFacebookApplicationId = $configNode.local.facebookApplicationId
[string] $localFacebookApplicationSecret = $configNode.local.facebookApplicationSecret
[string] $localTwitterConsumerKey = $configNode.local.twitterConsumerKey
[string] $localTwitterConsumerSecret = $configNode.local.twitterConsumerSecret

[string] $websiteFacebookApplicationId = $configNode.website.facebookApplicationId
[string] $websiteFacebookApplicationSecret = $configNode.website.facebookApplicationSecret
[string] $websiteTwitterConsumerKey = $configNode.website.twitterConsumerKey
[string] $websiteTwitterConsumerSecret = $configNode.website.twitterConsumerSecret

[string] $apiBaseUrl = $configNode.website.apiBaseUrl


# Begin updating web.config file
[string] $file = Resolve-Path $webConfigFile 
$xml = New-Object xml
$xml.psbase.PreserveWhitespace = $true
$xml.Load($file)
$xml.SelectNodes("//appSettings/add[@key = 'FacebookApplicationId']").setAttribute("value", $localFacebookApplicationId)
$xml.SelectNodes("//appSettings/add[@key = 'FacebookApplicationSecret']").setAttribute("value", $localFacebookApplicationSecret)
$xml.SelectNodes("//appSettings/add[@key = 'TwitterConsumerKey']").setAttribute("value", $localTwitterConsumerKey)
$xml.SelectNodes("//appSettings/add[@key = 'TwitterConsumerSecret']").setAttribute("value", $localTwitterConsumerSecret)

$xml.SelectNodes("//appSettings/add[@key = 'MediaServicesAccountName']").setAttribute("value", $mediaServicesAccountName)
$xml.SelectNodes("//appSettings/add[@key = 'MediaServicesAccountKey']").setAttribute("value", $mediaServicesAccountKey)
$xml.SelectNodes("//appSettings/add[@key = 'ServiceBusConnectionString']").setAttribute("value", $serviceBusConnectionString)

$xml.Save($file)
# End updating web.config file

# Begin updating web.Release.config file
[string] $file = Resolve-Path $webReleaseConfigFile 
$xml = New-Object xml
$xml.psbase.PreserveWhitespace = $true
$xml.Load($file)
$xml.SelectNodes("//appSettings/add[@key = 'FacebookApplicationId']").setAttribute("value", $websiteFacebookApplicationId)
$xml.SelectNodes("//appSettings/add[@key = 'FacebookApplicationSecret']").setAttribute("value", $websiteFacebookApplicationSecret)
$xml.SelectNodes("//appSettings/add[@key = 'TwitterConsumerKey']").setAttribute("value", $websiteTwitterConsumerKey)
$xml.SelectNodes("//appSettings/add[@key = 'TwitterConsumerSecret']").setAttribute("value", $websiteTwitterConsumerSecret)

$xml.SelectNodes("//appSettings/add[@key = 'StorageConnectionString']").setAttribute("value", $storageAccountConnectionString)

$xml.Save($file)
# End updating web.Release.config file

# Begin updating config.js file
[string] $file = Resolve-Path $win8ConfigJsFile
.\tasks\updateConfigSettings.ps1 $file "ApiBaseUrl" $apiBaseUrl -JsFormat $true

.\tasks\updateConfigSettings.ps1 $file "FacebookClientId" $localFacebookApplicationId -JsFormat $true 
.\tasks\updateConfigSettings.ps1 $file "TwitterConsumerKey" $localTwitterConsumerKey -JsFormat $true 
.\tasks\updateConfigSettings.ps1 $file "TwitterConsumerSecret" $localTwitterConsumerSecret -JsFormat $true 
# End updating config.js file