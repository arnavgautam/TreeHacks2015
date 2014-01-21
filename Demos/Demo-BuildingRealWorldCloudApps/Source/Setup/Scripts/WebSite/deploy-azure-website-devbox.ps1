# How to run the script
# deploy-azure-website-devbox-webdeploy.ps1 -ProjectFile
 
# Define input parameters
Param(
    [Parameter(Mandatory = $true)]
    [String]$ProjectFile,          # Point to the .csproj file of the project you want to deploy
    [Switch]$Launch                # Use this switch parameter if you want to launch a browser to show the website
)

# Begin - Actual script -----------------------------------------------------------------------------------------------------------------------------
 
# Set the output level to verbose and make the script stop on error
$VerbosePreference = "Continue"
$ErrorActionPreference = "Stop"
 
$scriptPath = Split-Path -parent $PSCommandPath
 
# Mark the start time of the script execution
$startTime = Get-Date

# Build and publish the project via web deploy package using msbuild.exe 

Write-Verbose ("[Start] deploying to Windows Azure website {0}" -f $websiteName)

# Read from website-environment.xml to get the environment name
[Xml]$envXml = Get-Content ("{0}\website-environment.xml" -f $scriptPath)
$websiteName = $envXml.environment.name

# Read from the publish settings file to get the deploy password
$publishXmlFile = "{0}\{1}.pubxml" -f $scriptPath, $websiteName
[Xml]$xml = Get-Content ("{0}\{1}.publishsettings" -f $scriptPath, $websiteName)
$password = $xml.publishData.publishProfile.userPWD.get(0)

# Run MSBuild to publish the project
& "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" $ProjectFile `
    /p:VisualStudioVersion=11.0 `
    /p:DeployOnBuild=true `
    /p:PublishProfile=$publishXmlFile `
    /p:Password=$password

Write-Verbose ("[Finish] deploying to Windows Azure website {0}" -f $websiteName)

# Mark the finish time of the script execution
$finishTime = Get-Date

# Output the time consumed in seconds
Write-Output ("Total time used (seconds): {0}" -f ($finishTime - $startTime).TotalSeconds)

# Launch the browser to show the website
If ($Launch)
{
    Show-AzureWebsite -Name $websiteName
}

# End - Actual script -------------------------------------------------------------------------------------------------------------------------------