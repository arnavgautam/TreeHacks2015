$path = "c:\sysinternals"
$file = "SysinternalsSuite.zip"
$check = Test-Path -PathType Container $path
if ($check -eq $true) {exit 0}
New-Item $path -Type Directory
$web = new-object net.webclient
$web.DownloadFile("http://download.sysinternals.com/files/SysinternalsSuite.zip", $path + "\" + $file)
$shell = new-object -com shell.application
$zip = $shell.Namespace($path + "\" + $file)
foreach ($item in $zip.items()) { $shell.Namespace($path).copyhere($item, 0x14) }
