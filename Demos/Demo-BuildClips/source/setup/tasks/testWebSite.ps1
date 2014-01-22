Param([string] $webSiteUrl)

$request = [System.Net.HttpWebRequest]::Create($webSiteUrl)

$webSiteRunning = $false
$attemptToConnect = 0

While ((-not $apiRunning) -and $attemptToConnect -le 5)
{
    Start-Sleep -Seconds 5
    $attemptToConnect += 1
    Try
    {
        $response = $request.GetResponse()
        if ($response.StatusCode -eq [System.Net.HttpStatusCode]::OK)
        {
            $webSiteRunning = $true
        }
    }
    Catch
    {
    }
}

if ($webSiteRunning)
{
    Write-Host "The website is running OK."
}
else
{
    Write-Host "The website is not running." -ForegroundColor Red
}

return $webSiteRunning