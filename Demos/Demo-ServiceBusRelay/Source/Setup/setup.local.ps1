Param([string] $userSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #

# Get settings from user configuration file
if($userSettingsFile -eq $nul -or $userSettingsFile -eq "")
{
	$userSettingsFile = "..\Config.Local.xml"
}

[xml]$xmlUserSettings = Get-Content $userSettingsFile

[string] $workingDir = $xmlUserSettings.configuration.localPaths.workingDir
[string] $namespace = $xmlUserSettings.Configuration.ServiceBus.Namespace;
[string] $issuer = $xmlUserSettings.Configuration.ServiceBus.Issuer;
[string] $secretKey = $xmlUserSettings.Configuration.ServiceBus.SecretKey;

[string] $sourceCodeDir = Resolve-Path "..\Code"

write-host "========= Copying source code to working directory... ========="
if (!(Test-Path "$workingDir"))
{
	New-Item "$workingDir" -type directory | Out-Null
}
Copy-Item "$sourceCodeDir\*" "$workingDir" -Recurse -Force
write-host "Copying source code to working directory done!"

write-host "========= Creating LocalDB Northwind database ... ========="
& "SqlCmd" @("-S", "(localdb)\v11.0", "-E", "-i", "tasks\instnwnd.sql");
write-host "Creating LocalDB Northwind database done!"

write-host "========= Configuring the demo... ========="
& ".\tasks\ConfigureDemo.ps1" -demoPath $workingDir -namespace $namespace -issuer $issuer -secretKey $secretKey
write-host "Configuring the demo done!"