Param([string] $demoSettingsFile, [string] $userSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
if($userSettingsFile -eq $nul -or $userSettingsFile -eq "")
{
	$userSettingsFile = "..\config.local.xml"
}

# Get settings from demo configuration file
if($demoSettingsFile -eq $nul -or $demoSettingsFile -eq "")
{
	$demoSettingsFile = "setup.xml"
}

# Get the key and account setting from configuration using namespace
[xml]$xmlUserSettings = Get-Content $userSettingsFile
[xml]$xmlDemoSettings = Get-Content $demoSettingsFile

[string] $workingDir = $xmlUserSettings.configuration.localPaths.workingDir
[string] $sourceCodeDir = Resolve-Path "..\Code\Begin"
[string] $assetsDir = Resolve-Path "..\Assets"
[string] $sqlServerName = $xmlUserSettings.configuration.localSqlserver.sqlServerName

[string] $CSharpSnippets = $xmlDemoSettings.configuration.codeSnippets.cSharp
[string] $htmlSnippets = $xmlDemoSettings.configuration.codeSnippets.html
[string] $xmlSnippets = $xmlDemoSettings.configuration.codeSnippets.xml

[string] $receiptsAssetsDir = $xmlDemoSettings.configuration.copyAssets.receiptsDir
[string] $federationsAssetsDir = $xmlDemoSettings.configuration.copyAssets.federationsDir

$receiptsAssetsDir = Resolve-Path $receiptsAssetsDir
$federationsAssetsDir = Resolve-Path $federationsAssetsDir

# "========= Main Script =========" #
write-host "========= Create working directory... ========="
if (!(Test-Path "$workingDir"))
{
	New-Item "$workingDir" -type directory | Out-Null
}
write-host "Creating working directory done!"

write-host "========= Copying Begin solution to working directory...  ========="
Copy-Item "$sourceCodeDir\*" "$workingDir" -recurse -Force
write-host "Copying Begin solution to working directory done!"

write-host "========= Copying assets code to working directory... ========="
if (!(Test-Path "$workingDir\Assets"))
{
	New-Item "$workingDir\Assets" -type directory | Out-Null
}
Copy-Item "$assetsDir\*" "$workingDir\Assets" -recurse -Force
write-host "Copying Assets code to working directory done!"


#========= Install Code Snippets... =========
& ".\tasks\install-code-snippets.ps1" -CSharpSnippets $CSharpSnippets -htmlSnippets $htmlSnippets -xmlSnippets $xmlSnippets

write-host "========= Updating web.config file... ========="
[string] $fileName = Resolve-Path(Join-Path $workingDir "\Expenses.Web\web.config")
$fileContent = Get-Content $fileName
$fileContent = $fileContent.Replace("Server=(localdb)\v11.0", "Server=" + $sqlServerName)
Set-Content $fileName $fileContent
write-host "Updating web.config file done!"

write-host "========= Install Node Package ... ========="
& npm install azure -g
write-host "========= Installing Node Package done! ... ========="
