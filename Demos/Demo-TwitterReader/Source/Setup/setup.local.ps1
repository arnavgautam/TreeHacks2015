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


# Get settings from user configuration file
if($userSettingsFile -eq $nul -or $userSettingsFile -eq "")
{
	$userSettingsFile = "..\Config.Local.xml"
}

[xml]$xmlUserSettings = Get-Content $userSettingsFile
[string] $workingDir = $xmlUserSettings.configuration.localPaths.workingDir

[string] $sourceCodeDir = Resolve-Path "..\Code"

write-host "========= Copying source code to working directory... ========="
if (!(Test-Path "$workingDir"))
{
	New-Item "$workingDir" -type directory | Out-Null
}
Copy-Item $sourceCodeDir\* $workingDir -Recurse -Force
write-host "Copying source code to working directory done!"

write-host "========= Configuring the demo... ========="
$CSharpSnippets = Resolve-Path $CSharpSnippets
popd
& ".\tasks\install-code-snippets.ps1" -CSharpSnippets $CSharpSnippets
