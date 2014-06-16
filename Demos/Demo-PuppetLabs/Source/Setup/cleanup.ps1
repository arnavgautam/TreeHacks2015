Param([string] $azureSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #
pushd ".."

# Import required settings from config.local.xml
[xml] $xmlAzureSettings = Get-Content $azureSettingsFile

# General Settings 

# Puppet Master Settings

# Puppet Agent Settings

popd

#Puppet VM Creation if they don't exists

#VM Reset