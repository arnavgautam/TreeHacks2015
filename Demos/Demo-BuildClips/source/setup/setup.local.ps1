Param([string] $demoSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
pushd ".."
# Get settings from user configuration file
if($demoSettingsFile -eq $nul -or $demoSettingsFile -eq "")
{
	$demoSettingsFile = "Config.Local.xml"
}

[xml] $xmlDemoSettings = Get-Content $demoSettingsFile

# Import required settings from config.local.xml if neccessary #
[string] $workingDir = $xmlDemoSettings.configuration.localPaths.workingDir
[string] $solutionWorkingDir = $xmlDemoSettings.configuration.localPaths.solutionWorkingDir
[string] $segment2SolutionWorkingDir = $xmlDemoSettings.configuration.localPaths.segment2SolutionWorkingDir
[string] $endSolutionWorkingDir = $xmlDemoSettings.configuration.localPaths.endSolutionWorkingDir

[string] $beginSolutionDir = Resolve-Path $xmlDemoSettings.configuration.localPaths.beginSolutionDir
[string] $beginSolutionSegment2Dir = Resolve-Path $xmlDemoSettings.configuration.localPaths.beginSolutionSegment2Dir
[string] $endSolutionDir = Resolve-Path $xmlDemoSettings.configuration.localPaths.endSolutionDir

[string] $assetsDir = Resolve-Path $xmlDemoSettings.configuration.localPaths.assetsDir

[string] $CSharpSnippets = Resolve-Path $xmlDemoSettings.configuration.codeSnippets.cSharp
[string] $htmlSnippets = Resolve-Path $xmlDemoSettings.configuration.codeSnippets.html
[string] $JavaScriptSnippets = Resolve-Path $xmlDemoSettings.configuration.codeSnippets.JavaScript
#[string] $xmlSnippets = Resolve-Path $xmlDemoSettings.configuration.codeSnippets.xml

[string] $webConfig = $xmlDemoSettings.configuration.localPaths.webConfig
[string] $webReleaseConfig = $xmlDemoSettings.configuration.localPaths.webReleaseConfig
[string] $win8ConfigJs = $xmlDemoSettings.configuration.localPaths.win8ConfigJs
[string] $appConfigAsset = $xmlDemoSettings.configuration.localPaths.appConfigAsset

$configNode = $xmlDemoSettings.configuration.appSettings
[string] $manualResetFile = Resolve-Path $xmlDemoSettings.configuration.manualResetFile

[string] $videosDir = Resolve-Path $xmlDemoSettings.configuration.localPaths.videosDir

popd
# "========= Main Script =========" #

if (!(Test-Path "$workingDir"))
{
	New-Item "$workingDir" -type directory | Out-Null
}


write-host
write-host
write-host "========= Copying Begin solution to working directory... ========="
if (!(Test-Path "$solutionWorkingDir"))
{
	New-Item "$solutionWorkingDir" -type directory | Out-Null
}
Copy-Item "$beginSolutionDir\*" "$solutionWorkingDir" -Recurse -Force
write-host "Copying Begin solution to working directory done!"


write-host
write-host
write-host "========= Copying Begin solution for Segment 2 to working directory... ========="
if (!(Test-Path "$segment2SolutionWorkingDir"))
{
	New-Item "$segment2SolutionWorkingDir" -type directory | Out-Null
}
Copy-Item "$beginSolutionSegment2Dir\*" "$segment2SolutionWorkingDir" -Recurse -Force
write-host "Copying Begin solution for Segment 2 to working done!"


write-host
write-host
write-host "========= Copying Begin solution for Segment 5 to working directory... ========="
if (!(Test-Path "$endSolutionWorkingDir"))
{
	New-Item "$endSolutionWorkingDir" -type directory | Out-Null
}
Copy-Item "$endSolutionDir\*" "$endSolutionWorkingDir" -Recurse -Force
write-host "Copying Begin solution for Segment 5 to working done!"


write-host
write-host
write-host "========= Copying assets code to working directory... ========="
if (!(Test-Path "$workingDir\Assets"))
{
	New-Item "$workingDir\Assets" -type directory | Out-Null
}
Copy-Item "$assetsDir\*" "$workingDir\Assets\" -Recurse -force
write-host "Copying Assets code to working directory done!"


write-host
write-host
write-host "========= Update web config settings in Begin solution... ========="
$webConfigFile = Join-Path $solutionWorkingDir $webConfig
$webConfigReleaseFile = Join-Path $solutionWorkingDir $webReleaseConfig
$win8ConfigJsFile = Join-Path $solutionWorkingDir $win8ConfigJs

.\tasks\config-solution.ps1 $webConfigFile $webConfigReleaseFile $win8ConfigJsFile $configNode
write-host "Update web config settings in Begin solution done!"


write-host
write-host
write-host "========= Update web config settings in Begin solution for Segment 2... ========="
$webConfigFile = Join-Path $segment2SolutionWorkingDir $webConfig
$webConfigReleaseFile = Join-Path $segment2SolutionWorkingDir $webReleaseConfig
$win8ConfigJsFile = Join-Path $segment2SolutionWorkingDir $win8ConfigJs

.\tasks\config-solution.ps1 $webConfigFile $webConfigReleaseFile $win8ConfigJsFile $configNode
write-host "Update web config settings in Begin solution for Segment 2 done!"


write-host
write-host
write-host "========= Update app config settings in Begin solution... ========="
$appConfigAssetFile = Join-Path $workingDir $appConfigAsset
[string] $mediaServicesAccountName = $configNode.mediaServicesAccountName
[string] $mediaServicesAccountKey = $configNode.mediaServicesAccountKey
# Begin updating web.config file
[string] $file = Resolve-Path $appConfigAssetFile 
$xml = New-Object xml
$xml.psbase.PreserveWhitespace = $true
$xml.Load($file)

$xml.SelectNodes("//appSettings/add[@key = 'MediaServicesAccountName']").setAttribute("value", $mediaServicesAccountName)
$xml.SelectNodes("//appSettings/add[@key = 'MediaServicesAccountKey']").setAttribute("value", $mediaServicesAccountKey)

$xml.Save($file)
# End updating web.config file
write-host "Update app config settings in Begin solution done!"


write-host
write-host
write-host "========= Resetting Azure Compute Emulator & Dev Storage...  =========" -ForegroundColor Yellow
$CSRunFile = Join-Path $env:ProgramFiles "\Microsoft SDKs\Windows Azure\Emulator\csrun.exe"
$DSInitFile = Join-Path $env:ProgramFiles "\Microsoft SDKs\Windows Azure\Emulator\devstore\DSInit.exe"
& $CSRunFile @("/devfabric:start")
& $CSRunFile @("/devstore:start")
& $CSRunFile @("/devstore:shutdown")
& $CSRunFile @("/devfabric:shutdown")
Start-Process $DSInitFile @("/ForceCreate", "/silent") -Wait
& $CSRunFile @("/devfabric:shutdown")
& $CSRunFile @("/devfabric:clean")
& $CSRunFile @("/devfabric:start")
& $CSRunFile @("/devstore:start")
& $CSRunFile @("/removeAll")
write-host "Resetting Azure Comoute Emulator & Dev Storage Done!"

write-host
write-host
write-host "========= Compiling Web Site Solution... ========="
$MsBuildFile = Join-Path $env:windir "\Microsoft.NET\Framework\v4.0.30319\MsBuild.exe"
[string] $beginSolutionFile = Join-Path $solutionWorkingDir "BuildClips.Web\BuildClips.sln"
& $MsBuildFile @($beginSolutionFile, "/p:Configuration=Debug")
write-host "Compiling Begin Web Site Solution Done!"


write-host
write-host
write-host "========= Configuring IIS Express Web Site... ========="
[string] $appCmdFile = Join-Path ${env:ProgramFiles(x86)} "\IIS Express\appcmd.exe"
# Delete All sites
& $appCmdFile @("list", "site", "/xml") | & $appCmdFile @("delete", "site", "/in")

[string] $webSolutionfolder = Join-Path $solutionWorkingDir "BuildClips.Web\BuildClips"
# Add BuildClips site
& $appCmdFile @("add", "site", "/name:BuildClips", "/bindings:http://127.0.0.1:81", "/physicalpath:""$webSolutionfolder""")
# Set App Pool
& $appCmdFile @("set", "app", "BuildClips/", "/applicationpool:Clr4IntegratedAppPool")
# Stop IIS Express
Get-Process | Where-Object {$_.ProcessName -eq "iisexpress"} | Stop-Process
write-host "Configuring IIS Express Web Site Done!"

write-host
write-host
write-host "========= Running Local Web Site... ========="
[string] $IISExpressFile = Join-Path ${env:ProgramFiles(x86)} "\IIS Express\iisexpress.exe"
Start-Process $IISExpressFile @("/site:BuildClips") -WindowStyle Hidden
write-host "Running Local Web Site Done!"

write-host
write-host
write-host "========= Testing Local Web Site... ========="
$isWebSiteRunning = .\tasks\testWebSite.ps1 "http://127.0.0.1:81"
write-host "========= Testing Local Web Site Done! ========="

if ($isWebSiteRunning)
{
    write-host
    write-host
    write-host "========= Uploading Sample Videos to Local Web Site... ========="
    .\tasks\uploadVideos $videosDir "http://127.0.0.1:81/api/videos"
    write-host "Uploading Sample Videos to Local Web Site Done!"
}

write-host
write-host
write-host "========= Stopping Local Web Site... ========="
Get-Process | Where-Object {$_.ProcessName -eq "iisexpress"} | Stop-Process
write-host "Stopping Local Web Site Done!"

write-host
write-host
write-host "========= Installing Code Snippets... ========="
[string] $documentsFolder = [Environment]::GetFolderPath("MyDocuments")
if (-NOT (test-path "$documentsFolder"))
{
    $documentsFolder = "$env:UserProfile\Documents";
}

[string] $myCSharpSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\Visual C#\My Code Snippets"
Copy-Item "$CSharpSnippets\*.snippet" "$myCSharpSnippetsLocation" -force

[string] $myHTMLSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\Visual Web Developer\My HTML Snippets"
Copy-Item "$htmlSnippets\*.snippet" "$myHTMLSnippetsLocation" -force

[string] $myJavaScriptSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\JavaScript\My Code Snippets"
Copy-Item "$JavaScriptSnippets\*.snippet" "$myJavaScriptSnippetsLocation" -force

#[string] $myXMLScriptSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\XML\My Xml Snippets"
#Copy-Item "$xmlSnippets\*.snippet" "$myXMLScriptSnippetsLocation" -force

write-host "Installing Code Snippets done!"


write-host
write-host
write-host "========= Opening Manual Reset steps file... ========="
& "notepad" @("$manualResetFile")
write-host "Opening Manual Reset steps file Done!"
