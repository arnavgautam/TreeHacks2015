$relyingPartyUrl = $args[0];

$certPassword = ""
if ($args.Length -gt 1)
{
	$certPassword = $args[1];
}

$relyingPartyCertSubject = "CN=" + $relyingPartyUrl;

$relyingPartyCertThumbprint = .\getThumbprintBySubjectCertificate.ps1 "My" "LocalMachine" $relyingPartyCertSubject;
if ($relyingPartyCertThumbprint -eq $null) {
	Write-Host
	Write-Host Generating certificate with subject $relyingPartyCertSubject;

	cmd /c createCertificate.cmd $relyingPartyUrl My LocalMachine
}

Write-Host
Write-Host "Copying certificate from 'LocalMachine\Personal' to 'CurrentUser\Personal'..."

$currentLocation = get-Location;
$tempCertPath = $currentLocation.ToString() + "\" + $relyingPartyUrl + ".pfx";
$relyingPartyCertThumbprint = .\getThumbprintBySubjectCertificate.ps1 My LocalMachine $relyingPartyCertSubject;
.\exportCertificate.ps1 "My" "LocalMachine" $relyingPartyCertThumbprint $tempCertPath $certPassword
.\importCertificate.ps1 "My" "CurrentUser" $tempCertPath $certPassword

Write-Host
Write-Host "Copying certificate from 'CurrentUser\Personal' to 'CurrentUser\TrustedPeople'..."

$tempCertPath = $Env::TMP + "\" + $relyingPartyUrl + ".cer";
.\exportCertificate.ps1 "My" "LocalMachine" $relyingPartyCertThumbprint $tempCertPath
.\importCertificate.ps1 "TrustedPeople" "CurrentUser" $tempCertPath ""