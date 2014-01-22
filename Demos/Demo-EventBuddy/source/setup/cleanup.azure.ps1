Param([string] $demoSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
pushd ".."

if($demoSettingsFile -eq $nul -or $demoSettingsFile -eq "")
{
	$demoSettingsFile = "Config.Azure.xml"
}

[xml] $xmlDemoSettings = Get-Content $demoSettingsFile

# Import Mobile Service name #
[string] $mobileServiceUrl = $xmlDemoSettings.configuration.mobileServiceUrl
[string] $mobileServiceKey = $xmlDemoSettings.configuration.mobileServiceKey

popd
# "========= Main Script =========" #

write-host "========= Cleaning up Channel Table... ========="
.\tasks\CleanChannelTable.ps1 $mobileServiceUrl $mobileServiceKey
write-host "Cleaning up Channel Table Done!"
