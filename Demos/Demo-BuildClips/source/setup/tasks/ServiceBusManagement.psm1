# -- Windows Azure Service Bus Management Module --

# -------------------
# - Exported Commands
# -------------------

function Import-AzureSubscription(  [string] $PublishSettingsFilePath,
                                    [string] $SubscriptionName)
{
    [xml] $xmlPublishSettings = Get-Content $PublishSettingsFilePath
    $rawCertificateData = [Convert]::FromBase64String($xmlPublishSettings.PublishData.PublishProfile.ManagementCertificate)

    $global:currentSubscription = @{
        Name = $subscriptionName
        SubscriptionId = (Select-Xml "/PublishData/PublishProfile/Subscription[@Name='$SubscriptionName']" $xmlPublishSettings).Node.Id
        Certificate = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2(,$rawCertificateData)
    }
    
    return $global:currentSubscription
}

function Get-ServiceBusNamespace([string] $NamespaceName)
{
    $requestUri = "https://management.core.windows.net/<subscription-id>/services/servicebus/namespaces"
    if ($NamespaceName -ne $null -and -not [String]::IsNullOrWhiteSpace($NamespaceName))
    {
        $requestUri = $requestUri + "/$NamespaceName"
    }

    $context = Invoke-ManagementApi -RequestUri $requestUri
    if ($context.StatusCode -eq "OK")
    {
        if ($NamespaceName -ne $null -and -not [String]::IsNullOrWhiteSpace($NamespaceName))
        {
            $serviceBusNamespace = Select-Xml -Content $context.ResponseText -XPath "/atom:entry[atom:title='$NamespaceName']/atom:content/sb:NamespaceDescription" -Namespace @{atom=$namespaceAtom; sb=$namespaceServiceBus}
        }
        else
        {
            $serviceBusNamespace = Select-Xml -Content $context.ResponseText -XPath "/atom:feed/atom:entry/atom:content/sb:NamespaceDescription" -Namespace @{atom=$namespaceAtom; sb=$namespaceServiceBus}
        }

        return $serviceBusNamespace.Node        
    }

    Write-Error $context.ErrorMessage
    return $null
}

function New-ServiceBusNamespace(   [string] $NamespaceName, 
                                    [string] $Location)
{
    $context = Invoke-ManagementApi -RequestUri "https://management.core.windows.net/<subscription-id>/services/servicebus/namespaces/$NamespaceName" "PUT"
    if ($context.StatusCode -eq "OK")
    {
        $operationStatus = Get-OperationStatus -OperationId $context.OperationId
        $serviceBusNamespace = Select-Xml -Content $context.ResponseText -XPath "/atom:entry[atom:title='$NamespaceName']/atom:content/sb:NamespaceDescription" -Namespace @{atom=$namespaceAtom; sb=$namespaceServiceBus}
        return $serviceBusNamespace.Node
    }

    Write-Error $context.ErrorMessage
    return $null
}

function Remove-ServiceBusNamespace([string] $NamespaceName)
{
    $context = Invoke-ManagementApi -RequestUri "https://management.core.windows.net/<subscription-id>/services/servicebus/namespaces/$NamespaceName" "DELETE"
    if ($context.StatusCode -eq "OK")
    {
        $operationStatus = Get-OperationStatus -SubscriptionId $SubscriptionId -Certificate $Certificate -OperationId $context.OperationId
        return $context.ResponseText
    }

    Write-Error $context.ErrorMessage
    return $null
}

# -------------------
# - Internal Commands
# -------------------

$defaultApiVersion = "2012-03-01"
$namespaceServiceBus = "http://schemas.microsoft.com/netservices/2010/10/servicebus/connect"
$namespaceAtom = "http://www.w3.org/2005/Atom"
$namespaceWindowsAzure = "http://schemas.microsoft.com/windowsazure"
[hashtable] $currentSubscription = $null

function Invoke-ManagementApi(  [string] $RequestUri,
                                [string] $HttpMethod = "GET",
                                [string] $ApiVersion = $defaultApiVersion,
                                [string] $Payload = $null)
{
    [hashtable] $context = @{}
    try
    {
        $subscriptionData = Get-CurrentSubscription
        if ($subscriptionData -eq $null)
        {
            $context.Success = false
            $context.ErrorMessage = "Missing subscription data"
        }

        $request = [System.Net.HttpWebRequest]::Create($RequestUri.Replace("<subscription-id>", $subscriptionData.SubscriptionId))
        $request.Headers.Add("x-ms-version", $ApiVersion)
        $request.Method = $HttpMethod
        if ($Payload -ne $null -and -not [String]::IsNullOrWhiteSpace($Payload))
        {
            $request.ContentType = "application/xml"
            $request.ContentLength = $Payload.Length
            $requestStream = $request.GetRequestStream()
            $payloadData = System.Text.Encoding.UTF8::GetBytes($Payload)
            $requestStream.Write($payloadData, 0, $payloadData.Length)
            $requestStream.Close()
        }
        else
        {
            $request.ContentLength = 0
        }

        [void] $request.ClientCertificates.Add($subscriptionData.Certificate)
        
        $response = $request.GetResponse()
        $reader = New-Object System.IO.StreamReader($response.GetResponseStream())
        
        $context.ResponseText = $reader.ReadToEnd()
        $context.OperationId = $response.Headers["x-ms-request-id"]
        $context.StatusCode = $response.StatusCode
        $context.ContentType = $response.ContentType
        $context.StatusDescription = $response.StatusDescription
        $context.Success = $true
    }
    catch [System.Net.WebException]
    {
        $response = $_.Exception.Response
        $context.StatusDescription = $response.StatusDescription
        $context.StatusCode = $response.StatusCode
        $context.Success = $false
        $context.ErrorMessage = $_.Exception.Message
    }
    finally
    {
        if ($reader -ne $null)
        {
            $reader.Dispose()
        }

        if ($response -ne $null)
        {
            $response.Dispose()
        }
    }   

    return $context
}

function Get-OperationStatus([string] $OperationId)
{
    [hashtable] $context = @{}
    $requestUri = "https://management.core.windows.net/<subscription-id>/operations/" + $OperationId
    $context = Invoke-ManagementApi -RequestUri $requestUri
    if ($context.StatusCode -eq "OK")
    {
        $operation = Select-Xml -Content $context.ResponseText -XPath "/ns:Operation" -Namespace @{ns=$namespaceWindowsAzure}
        $context.Status = $operation.Node.Status
        $context.HttpStatusCode = $operation.Node.HttpStatusCode
    }
    else
    {
        $context.Status = "Failure"
        $context.HttpStatusCode = $context.StatusCode
    }

    return $context
}

function Get-CurrentSubscription()
{
    if ($global:currentSubscription -ne $null)
    {
        return $global:currentSubscription
    }

    if ((Get-Module Azure) -ne $null)
    {
        return Get-AzureSubscription -Current
    }

    Write-Error "Missing subscription data. Use Import-AzureSubscription in this module or Import-AzurePublishSettingsFile / Select-AzureSubscription in the standard Windows Azure Powershell module to load the subscription data."
            
    return $null
}

Export-ModuleMember -function Import-AzureSubscription
Export-ModuleMember -function Get-ServiceBusNamespace
Export-ModuleMember -function New-ServiceBusNamespace
Export-ModuleMember -function Remove-ServiceBusNamespace
