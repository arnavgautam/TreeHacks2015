$storeName = $args[0];
$storeLocation = $args[1];
$certFileName = $args[2];

$store = new-Object System.Security.Cryptography.X509Certificates.X509Store $storeName, $storeLocation;
$store.Open("MaxAllowed");

if ($args.Length -gt 3) {
	$certPassword = $args[3];
	$cert = new-Object System.Security.Cryptography.X509Certificates.X509Certificate2 $certFileName, $certPassword, "PersistKeySet";
} else {
	$cert = new-Object System.Security.Cryptography.X509Certificates.X509Certificate2 $certFileName;
}

$store.Add($cert);
$store.Close();