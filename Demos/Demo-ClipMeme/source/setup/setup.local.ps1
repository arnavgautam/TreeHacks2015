[CmdletBinding()]
param([Parameter(Mandatory=$false)] [string] $settingsFile,
		[Parameter(Mandatory=$false)] [boolean] $ignoreAzureSteps = $false)

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
[string] $vsSuoFile = Resolve-Path $settings.configuration.localPaths.vsSuoFile
[string] $webConfigFile = $settings.configuration.localPaths.webConfig
[string] $solutionFile = $settings.configuration.localPaths.solution
[string] $vsSettingsFile = Resolve-Path $settings.configuration.localPaths.vsSettingsFile
[string] $gifFile = Resolve-Path $settings.configuration.localPaths.gifFile


[string] $storageConnectionString = $settings.configuration.appSettings.storageConnectionString

[string] $webDeployServer = $settings.configuration.webDeploy.server
[string] $webDeploySiteName = $settings.configuration.webDeploy.siteName
[string] $webDeployUsername = $settings.configuration.webDeploy.username
[string] $webDeployPassword = $settings.configuration.webDeploy.password
[string] $webDeployStagingSiteName = $settings.configuration.webDeploy.stagingSiteName
[string] $webDeployStagingUsername = $settings.configuration.webDeploy.stagingUsername
[string] $webDeployStagingPassword = $settings.configuration.webDeploy.stagingPassword

[string] $iisExpressPort = $settings.configuration.iisExpressPort

[string] $browserTabsToOpen = $settings.configuration.browserTabsToOpen

popd


# "========= Main Script =========" #
Write-Host ""
Write-Host "Setup Local $environment"
Write-Host "".PadRight(68, "=")

Write-Action "Copying begin solution to working directory..."
if (!(Test-Path "$workingDir"))
{
	New-Item "$workingDir" -type directory | Out-Null
}

if (!(Test-Path "$solutionWorkingDir"))
{
	New-Item "$solutionWorkingDir" -type directory | Out-Null
}

Copy-Item "$beginSolutionDir\*" "$solutionWorkingDir" -Recurse -Force
Write-Done

Write-Action "Copying Visual Studio .suo to working directory solution..."
Copy-Item "$vsSuoFile" "$solutionWorkingDir" -Recurse -Force
Write-Done


Write-Action "Update config settings in Web begin solution (Web.config)..."
[string] $file = Resolve-Path $webConfigFile 
$xml = New-Object xml
$xml.psbase.PreserveWhitespace = $true
$xml.Load($file)
$xml.SelectNodes("//connectionStrings/add[@name = 'StorageConnectionString']").setAttribute("connectionString", $storageConnectionString)
$xml.Save($file)
# (Get-Content $file) | Out-File $file -Encoding ASCII
Write-Done

if (-Not $ignoreAzureSteps) {
Write-Action "Deploying the begin solution to Azure STAGING using WebDeploy..."
$msbuild = "$env:windir\Microsoft.NET\Framework\v4.0.30319\MsBuild.exe"
$env:VisualStudioVersion = "12.0"

& $msbuild "$solutionFile" `
          /P:DeployOnBuild=True /P:DeployTarget=MSDeployPublish `
          /P:MsDeployServiceUrl=https://$webDeployServer/msdeploy.axd?site=$webDeployStagingSiteName `
          /P:AllowUntrustedCertificate=True /P:MSDeployPublishMethod=WMSvc /P:CreatePackageOnPublish=True `
          /P:Configuration=Release `
          /P:UserName=$webDeployStagingUsername /P:Password=$webDeployStagingPassword  /p:DeployIisAppPath="$webDeployStagingSiteName"
Write-Done

Write-Action "Deploying the begin solution to Azure using WebDeploy..."
$msbuild = "$env:windir\Microsoft.NET\Framework\v4.0.30319\MsBuild.exe"
$env:VisualStudioVersion = "12.0"

& $msbuild "$solutionFile" `
          /P:DeployOnBuild=True /P:DeployTarget=MSDeployPublish `
          /P:MsDeployServiceUrl=https://$webDeployServer/msdeploy.axd?site=$webDeploySiteName `
          /P:AllowUntrustedCertificate=True /P:MSDeployPublishMethod=WMSvc /P:CreatePackageOnPublish=True `
          /P:Configuration=Release `
          /P:UserName=$webDeployUsername /P:Password=$webDeployPassword  /p:DeployIisAppPath="$webDeploySiteName"
Write-Done
}

Write-Action "Copying Gif to Desktop..."
[string] $desktopFolder = [Environment]::GetFolderPath("Desktop")
Copy-Item "$gifFile" "$desktopFolder" -Force
Write-Done

# write-host "Importing VS settings..."
# Import-VSSettings $vsSettingsFile
# Start-Sleep -s 10
# Close-VS -Force
# Start-Sleep -s 2
# Write-Done

Write-Action "Setting VS New Project Dialog Defaults..."
Set-VSNewProjectDialogDefaults -Path "$workingDir"
Write-Done

Write-Action "Opening begin solution in Visual Studio..."
[string] $devenvPath = "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\devenv.exe"
[string] $memeSolutionDir = Resolve-Path (Join-Path $solutionWorkingDir '.\ClipMeme.sln')
[string] $solutionShortcut = Join-Path $env:temp 'ClipMeme.lnk'
if (Test-Path "$solutionShortcut") { Remove-Item "$solutionShortcut" -Force }
.\tasks\set-shortcut.ps1 "$devenvPath" "$memeSolutionDir" "$solutionShortcut"

& explorer.exe "$solutionShortcut"

Start-Sleep -s 5
Write-Done


Write-Action "Opening empty Visual Studio..."
& explorer.exe "$devenvPath"
Write-Done


# Write-Action "Starting IIS Express and launching the app in IE..."
# .\tasks\start-iisexpress.ps1 "$solutionWorkingDir\ClipMeme" "$iisExpressPort"

# Open-IE "http://localhost:$iisExpressPort/"
# Write-Done

Write-Action "Setting IE Home Page and Secondary Tabs..."
$homePage = $browserTabsToOpen.Split(',')[0];
Set-ItemProperty -Path "HKCU:\Software\Microsoft\Internet Explorer\Main" -Name "Start Page" -Value "$homePage"
$secondaryTabs = $browserTabsToOpen.Split(",").Trim()[1 .. ($browserTabsToOpen.Split(",").length - 1)]
Set-ItemProperty -Path "HKCU:\Software\Microsoft\Internet Explorer\Main" -Name "Secondary Start Pages" -Value $secondaryTabs -Type Multistring
Write-Done

# Write-Action "Opening IE Tabs..."
# Open-IE -URLs "$browserTabsToOpen"
# Write-Done

Write-Action "Opening Manual Reset steps file..."
& "notepad" @("..\ManualReset.txt")
Write-Done 


# --------------------------------- #