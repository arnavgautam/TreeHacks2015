Param([string] $demoSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
pushd ".."

if($demoSettingsFile -eq $nul -or $demoSettingsFile -eq "")
{
	$demoSettingsFile = "Config.Local.xml"
}

[xml] $xmlDemoSettings = Get-Content $demoSettingsFile

# Import required settings from config.local.xml if neccessary #
[string] $workingDir = $xmlDemoSettings.configuration.localPaths.workingDir

popd
# "========= Main Script =========" #

write-host
write-host
write-host "========= Closing Visual Studio... ========="
Close-VS -Force
Start-Sleep -s 2
write-host "Closing Visual Studio Done!"

write-host
write-host
write-host "========= Closing IE... ========="
Close-IE -Force
Start-Sleep -s 2
write-host "Closing IE Done!"

# Step commented out to avoid making intrusive changes to the environment
# write-host "========= Clearing IE History ========="
# # Clear-IEFormData -ClearStoredPasswords
# Clear-IEFormData
# Clear-IEHistory
# # Clear-IECookies
# write-host "Clearing IE History Done!"

# Step commented out to avoid making intrusive changes to the environment
# write-host "========= Removing VS most recently used projects... ========="
# Clear-VSProjectMRUList
# Clear-VSFileMRUList
# write-host "Removing VS most recently used projects done!"

write-host
write-host
write-host "========= Removing current working directory... ========="
if (Test-Path "$workingDir")
{
	Remove-Item "$workingDir" -recurse -force
}
write-host "Removing current working directory done!"

# Step commented out to avoid making intrusive changes to the environment
# write-host "========= Removing Code Snippets ... ========="
# [string] $documentsFolder = [Environment]::GetFolderPath("MyDocuments")
# if (-NOT (test-path "$documentsFolder"))
# {
    # $documentsFolder = "$env:UserProfile\Documents";
# }
# [string] $myCSharpSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\Visual C#\My Code Snippets"
# [string] $myHTMLSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\Visual Web Developer\My HTML Snippets"
# [string] $myJavaScriptSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\JavaScript\My Code Snippets"
# [string] $myXMLScriptSnippetsLocation = "$documentsFolder\Visual Studio 2012\Code Snippets\XML\My Xml Snippets"

# Remove-Item "$myCSharpSnippetsLocation\*.snippet" -Force -ErrorAction SilentlyContinue
# Remove-Item "$myHTMLSnippetsLocation\*.snippet" -Force -ErrorAction SilentlyContinue
# Remove-Item "$myJavaScriptSnippetsLocation\*.snippet" -Force -ErrorAction SilentlyContinue
# Remove-Item "$myXMLScriptSnippetsLocation\*.snippet" -Force -ErrorAction SilentlyContinue

# write-host "Removing Code Snippets done!"