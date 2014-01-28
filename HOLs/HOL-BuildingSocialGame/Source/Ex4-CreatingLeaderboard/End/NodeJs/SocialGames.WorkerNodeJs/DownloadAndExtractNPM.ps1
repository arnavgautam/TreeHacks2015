param([string]$currentPath)


function ExtractZip([string]$filePath, [string]$destination)
{
	$shellApplication = new-object -com shell.application
	$zipPackage = $shellApplication.NameSpace($filePath)
	$destinationFolder = $shellApplication.NameSpace($destination)
	$destinationFolder.CopyHere($zipPackage.Items())
}

$zipPath = $currentPath + "\npm.zip"
$downloadURL = "http://npmjs.org/dist/npm-1.1.0-beta-7.zip";

$ws = New-Object System.Net.WebClient
$ws.DownloadFile($downloadURL,$zipPath)

ExtractZip $zipPath $currentPath