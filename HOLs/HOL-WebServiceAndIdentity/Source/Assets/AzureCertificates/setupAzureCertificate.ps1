$azureCertSubject = "CN=127.0.0.1, O=TESTING ONLY, OU=Windows Azure DevFabric";

$azureCertThumbprint = .\getThumbprintBySubjectCertificate.ps1 My LocalMachine $azureCertSubject;
if ($azureCertThumbprint -eq $null) {
	Write-Warning "Cannot find the Windows Azure DevFabric Test Certificate";
	return;
}

Write-Host
Write-Host "Copying certificate from 'LocalMachine\Personal' to 'LocalMachine\Root'..."
Write-Host

$tempCertPath = $Env:TMP + "\DevFabricSSLCert.cer";
.\exportCertificate.ps1 My LocalMachine $azureCertThumbprint $tempCertPath;
.\importCertificate.ps1 Root LocalMachine $tempCertPath;