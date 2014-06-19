Param([string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
pushd ".."

# Import required settings from config.local.xml
[xml] $xmlAzureSettings = Get-Content $azureSettingsFile

# General Settings
[string] $azureSubscription = $xmlAzureSettings.configuration.generalSettings.azureSubscriptionName
[string] $adminUserName = $xmlAzureSettings.configuration.generalSettings.adminUserName
[string] $adminPassword = $xmlAzureSettings.configuration.generalSettings.adminPassword
[string] $masterCloudServiceName = $xmlAzureSettings.configuration.puppetMasterSettings.cloudServiceName
[string] $agentCloudServiceName = $xmlAzureSettings.configuration.puppetAgentSettings.cloudServiceName
[string] $consoleUsername = $xmlAzureSettings.configuration.puppetMasterSettings.consoleUsername
[string] $consolePassword = $xmlAzureSettings.configuration.puppetMasterSettings.consolePassword
$hostVM = "$adminUsername@$masterCloudServiceName.cloudapp.net"

popd

#Reset Task Manager in Puppet Agent VM
write-host "Reset Task Manager in Puppet Agent VM"
Select-AzureSubscription $azureSubscription
$endpoint = Get-AzureVM $agentCloudServiceName | Get-AzureEndpoint -Name "WinRmHTTPs"
$port = $endpoint.Port
$path = "HKLM:\Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Taskmgr.exe"
$session = New-PSSession -ComputerName "$agentCloudServiceName.cloudapp.net" -Port $port -Authentication Negotiate -Credential "$adminUserName" -UseSSL -SessionOption (New-PSSessionOption -SkipCACheck -SkipCNCheck)
Invoke-Command -Session $session -ScriptBlock {
	if (Test-Path -Path $args[0])
	{
		write-host $args[0]
		Remove-Item -Path $args[0] -Recurse
	}
	
	# puppet agent --onetime --verbose
} -ArgumentList $path

write-Host "Done"

#Invoke Puppet Bash Script
Invoke-Expression -Command ".\reset.sh '$hostVM' '$consoleUsername' '$consolePassword'"