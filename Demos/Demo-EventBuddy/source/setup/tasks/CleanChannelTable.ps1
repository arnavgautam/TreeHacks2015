param([string]$mobileServiceUrl, [string]$mobileServiceKey)

Add-Type -Assembly System.ServiceModel.Web,System.Runtime.Serialization
function Convert-JsonToXml([string]$json)
{
    $bytes = [byte[]][char[]]$json
    $quotas = [System.Xml.XmlDictionaryReaderQuotas]::Max
    $jsonReader = [System.Runtime.Serialization.Json.JsonReaderWriterFactory]::CreateJsonReader($bytes,$quotas)
    try
    {
        $xml = New-Object System.Xml.XmlDocument
 
        $xml.Load($jsonReader)
        $xml
    }
    finally
    {
        $jsonReader.Close()
    }
}

$client = New-Object System.Net.WebClient;
$client.Headers["X-ZUMO-APPLICATION"] = $mobileServiceKey;

if (!$mobileServiceUrl.EndsWith("/"))
{
	$mobileServiceUrl = $mobileServiceUrl + "/";
}

$endpoint = $mobileServiceUrl + "tables/Channel/";
$result = $client.DownloadString($endpoint);

$xml = Convert-JsonToXml($result)

if ($xml.root.haschildnodes)
{
	foreach ($channel in $xml.root.item)
	{
		write-host "Removing channel. ID: " $channel.id."#text"

		$request = [System.Net.WebRequest]::Create($endpoint + $channel.id."#text");
		$request.Headers.Add("X-ZUMO-APPLICATION", $mobileServiceKey);
		$request.Method = "DELETE";
		$response = $request.GetResponse();
	}
}
else
{
	write-host "Channel table is empty."
}
