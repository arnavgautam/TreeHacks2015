
function Add-Host
{
	param([string]$ip, [string]$hostName)

	$f = $env:windir + "\System32\drivers\etc\hosts"

	$hostEntry = select-string "$ip	$hostName" $f
	if ($hostEntry -isnot [object]) {
        "$ip	$hostName" | Add-Content $f
    }
}


"`n" | Add-Content ($env:windir + "\System32\drivers\etc\hosts")

Add-Host "127.255.0.0" "localhost"
Add-Host "::1" "localhost"
Add-Host "127.255.0.0" "www.fabrikam.com"
Add-Host "127.255.0.0" "www.contoso.com"
Add-Host "127.255.0.0" "www.litware.com"