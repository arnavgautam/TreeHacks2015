Param([string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
pushd ".."

# Import required settings from config.local.xml
[xml] $xmlAzureSettings = Get-Content $azureSettingsFile

# General Settings 

# Puppet Settings
$cloudServiceName = $xmlAzureSettings.configuration.Puppet.CloudServiceName
$adminUsername = $xmlAzureSettings.configuration.Puppet.AdminUserName
$consoleUsername = $xmlAzureSettings.configuration.Puppet.ConsoleUsername
$consolePassword = $xmlAzureSettings.configuration.Puppet.ConsolePassword

Write-Host $adminUsername

$hostVM = "$adminUsername@$cloudServiceName.cloudapp.net"
Write-Host $hostVM
popd

#Puppet VM Creation if they don't exists

#VM Reset
Invoke-Expression -Command ".\reset.sh '$hostVM' '$consoleUsername' '$consolePassword'"