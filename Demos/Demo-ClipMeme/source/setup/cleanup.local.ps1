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
[string] $workingDir = $settings.configuration.localPaths.workingDir
[string] $beginSolutionDir = Resolve-Path $settings.configuration.localPaths.beginSolutionDir
[string] $solutionWorkingDir = $settings.configuration.localPaths.solutionWorkingDir

popd

# "========= Main Script =========" #
Write-Host ""
Write-Host "Cleanup Local $environment"
Write-Host "".PadRight(68, "=")

Write-Action "Closing Visual Studio..."
Close-VS -Force
Start-Sleep -s 5
Write-Done

Write-Action "Closing Notepad..."
Close-Process "Notepad"
Write-Done

Write-Action "Closing IE..."
Close-IE -Force
Start-Sleep -s 2
Write-Done

Write-Action "Closing Chrome..."
get-process | where {$_.name -eq "chrome"} | Stop-Process
Write-Done

Write-Action "Removing VS most recently used projects.."
Clear-VSProjectMRUList
Clear-VSFileMRUList
Write-Done 

Write-Action "Removing current working directory..."
if (Test-Path "$workingDir")
{
	Remove-Item "$workingDir" -recurse -force
}
Write-Done

# --------------------------------- #