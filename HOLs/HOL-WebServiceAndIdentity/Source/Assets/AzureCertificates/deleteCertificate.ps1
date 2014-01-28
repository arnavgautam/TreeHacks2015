$storeName = $args[0];
$storeLocation = $args[1];
$certThumbprint = $args[2];

$store = new-Object System.Security.Cryptography.X509Certificates.X509Store $storeName, $storeLocation;
$store.Open("ReadWrite");

$cert = $store.Certificates | where { $_.Thumbprint -eq $certThumbprint }

$store.Remove($cert);
$store.Close();