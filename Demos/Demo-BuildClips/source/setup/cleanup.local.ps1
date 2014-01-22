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
[string] $SQLServerName = $xmlDemoSettings.configuration.localSqlServer.servername
[string] $dbName = $xmlDemoSettings.configuration.localSqlServer.dbName

popd
# "========= Main Script =========" #

write-host
write-host
write-host "========= Removing current working directory... ========="
if (Test-Path "$workingDir")
{
	Remove-Item "$workingDir" -recurse -force
}
write-host "Removing current working directory done!"


write-host
write-host
write-host "========= Removing Code Snippets ... ========="
[string] $documentsFolder = [Environment]::GetFolderPath("MyDocuments")
if (-NOT (test-path "$documentsFolder"))
{
    $documentsFolder = "$env:UserProfile\Documents";
}
[string] $myCSharpSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\Visual C#\My Code Snippets"
[string] $myHTMLSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\Visual Web Developer\My HTML Snippets"
[string] $myJavaScriptSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\JavaScript\My Code Snippets"
#[string] $myXMLScriptSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\XML\My Xml Snippets"

Remove-Item "$myCSharpSnippetsLocation\*.snippet" -Force -ErrorAction SilentlyContinue
Remove-Item "$myHTMLSnippetsLocation\*.snippet" -Force -ErrorAction SilentlyContinue
Remove-Item "$myJavaScriptSnippetsLocation\*.snippet" -Force -ErrorAction SilentlyContinue
#Remove-Item "$myXMLScriptSnippetsLocation\*.snippet" -Force -ErrorAction SilentlyContinue

write-host "Removing Code Snippets done!"


write-host
write-host
write-host "========= Dropping Local Database... ========="
& "SqlCmd" @("-S", "$SQLServerName", "-E", "-Q", "ALTER DATABASE  $dbName SET SINGLE_USER WITH ROLLBACK IMMEDIATE;");
& "SqlCmd" @("-S", "$SQLServerName", "-E", "-Q", "DROP DATABASE  $dbName;");
write-host "Dropping local database Done!"


write-host
write-host
write-host "========= Removing IIS Express Web Site... ========="
[string] $appCmdFile = "C:\Program Files (x86)\IIS Express\appcmd.exe"
# Delete All sites
& $appCmdFile @("list", "site", "/xml") | & $appCmdFile @("delete", "site", "/in")
write-host "Removing IIS Express Web Site Done!"