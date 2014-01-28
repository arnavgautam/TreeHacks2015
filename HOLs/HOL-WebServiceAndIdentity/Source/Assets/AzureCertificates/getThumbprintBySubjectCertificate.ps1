$storeName = $args[0];
$storeLocation = $args[1];
$certSubject = $args[2];

$store = new-Object System.Security.Cryptography.X509Certificates.X509Store $storeName, $storeLocation;
$store.Open("ReadOnly");

$cert = $store.Certificates | where { $_.Subject -eq $certSubject }
$store.Close();

if ($cert -ne $null) {
	$cert.Thumbprint;
}