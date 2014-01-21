Param([string] $demoSettingsFile, [string] $userSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #

# Get settings from demo configuration file
if($demoSettingsFile -eq $nul -or $demoSettingsFile -eq "")
{
	$demoSettingsFile = "setup.xml"
}

[xml] $xmlDemoSettings = Get-Content $demoSettingsFile
[string] $CSharpSnippets = $xmlDemoSettings.configuration.codeSnippets.cSharp
[string] $htmlSnippets = $xmlDemoSettings.configuration.codeSnippets.html


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

write-host "========= Copying assets code to working directory... ========="
if (!(Test-Path "$workingDir"))
{
	New-Item "$workingDir" -type directory | Out-Null
}
Copy-Item "$sourceCodeDir\Assets\*" "$workingDir\Assets" -Recurse -Force
write-host "Copying Assets code to working directory done!"

write-host "========= Configuring the demo... ========="
& ".\tasks\ConfigureDemo.ps1" -demoPath $workingDir -namespace $namespace -issuer $issuer -secretKey $secretKey
write-host "Configuring the demo done!"

#========= Install Code Snippets... =========
& ".\tasks\install-code-snippets.ps1" -CSharpSnippets $CSharpSnippets -htmlSnippets $htmlSnippets
