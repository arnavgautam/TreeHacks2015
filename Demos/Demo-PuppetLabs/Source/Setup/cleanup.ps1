Param([string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
pushd ".."

# Import required settings from config.local.xml
[xml] $xmlAzureSettings = Get-Content $azureSettingsFile

# General Settings 
[string] $azureSubscription = $xmlAzureSettings.configuration.generalSettings.azureSubscriptionName
[string] $dclocation = $xmlAzureSettings.configuration.generalSettings.location
[string] $adminUserName = $xmlAzureSettings.configuration.generalSettings.adminUserName
[string] $adminPassword = $xmlAzureSettings.configuration.generalSettings.adminPassword
[string] $storageAccount = $xmlAzureSettings.configuration.generalSettings.storageAccount

# Puppet Settings
$cloudServiceName = $xmlAzureSettings.configuration.Puppet.CloudServiceName
$adminUsername = $xmlAzureSettings.configuration.Puppet.AdminUserName
$consoleUsername = $xmlAzureSettings.configuration.Puppet.ConsoleUsername
$consolePassword = $xmlAzureSettings.configuration.Puppet.ConsolePassword

# Puppet Agent Settings
[string] $agentCloudServiceName = $xmlAzureSettings.configuration.puppetAgentSettings.cloudServiceName
[string] $agentVMName = $xmlAzureSettings.configuration.puppetAgentSettings.vmName

$hostVM = "$adminUsername@$cloudServiceName.cloudapp.net"
popd

#Invoke Puppet VM Creation if they dont exists
Invoke-Expression ".\tasks\puppetVMsCreation.ps1 -azureSubscription `"$azureSubscription`" -dclocation `"$dclocation`" -storageAccountName `"$storageAccount`" -adminUserName `"$adminUserName`" -adminPassword `"$adminPassword`" -masterCloudServiceName `"$masterCloudServiceName`" -masterVMName `"$masterVMName`" -agentCloudServiceName `"$agentCloudServiceName`" -agentVMName `"$agentVMName`""

#VM Reset
Invoke-Expression -Command ".\reset.sh '$hostVM' '$consoleUsername' '$consolePassword'"