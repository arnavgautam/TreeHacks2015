Param([string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
pushd ".."

# Import required settings from config.local.xml
[xml] $xmlAzureSettings = Get-Content $azureSettingsFile

# General Settings 

[string] $adminUserName = $xmlAzureSettings.configuration.generalSettings.adminUserName
[string] $masterCloudServiceName = $xmlAzureSettings.configuration.puppetMasterSettings.cloudServiceName
[string] $consoleUsername = $xmlAzureSettings.configuration.puppetMasterSettings.consoleUsername
[string] $consolePassword = $xmlAzureSettings.configuration.puppetMasterSettings.consolePassword
$hostVM = "$adminUsername@$masterCloudServiceName.cloudapp.net"

popd

#Invoke Puppet VM Creation if they dont exists
Invoke-Expression -Command ".\reset.sh '$hostVM' '$consoleUsername' '$consolePassword'"