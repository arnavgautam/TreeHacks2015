Param([string] $settingsFile)

$Error.Clear();
$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# import demo tookit
Import-Module ".\tasks\demo-toolkit\DemoToolkit.psd1" -DisableNameChecking

# import progress functions
. ".\tasks\progress-functions.ps1"

# "========= Initialization =========" #
pushd ".."

if($settingsFile -eq $nul -or $settingsFile -eq "")
{
	$settingsFile = "config.primary.xml"
}

[string] $environment = "Primary"

if($settingsFile.ToUpperInvariant().IndexOf("SHADOW") -gt -1) 
{
    $environment = "Shadow"
}

# Import required settings
[xml] $settings = Get-Content $settingsFile

# [string] $setting = $xmlDemoSettings.configuration.setting


popd
# "========= Main Script =========" #

Write-Host ""
Write-Host "Setup Azure $environment"
Write-Host "".PadRight(68, "=")

# --------------------------------- #