$storeName = $args[0];
$storeLocation = $args[1];
$certThumbprint = $args[2];
$certFileName = $args[3];

$store = new-Object System.Security.Cryptography.X509Certificates.X509Store $storeName, $storeLocation;
$store.Open("ReadOnly");

$certToExport = $store.Certificates | where { $_.Thumbprint -eq $certThumbprint }

if (($certToExport.HasPrivateKey -eq $true) -and ($args.Length -gt 4)) {
	$certPassword = $args[4];
	$certData = $certToExport.Export("Pfx", $certPassword);
} else {
	$certData = $certToExport.Export("Cert");
}

$store.Close();

[System.IO.File]::WriteAllBytes($certFileName, $certData);