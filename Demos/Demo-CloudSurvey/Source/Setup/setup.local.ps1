Param([string] $userSettingsFile)

$scriptDir = (split-path $myinvocation.mycommand.path -parent)
Set-Location $scriptDir

# "========= Initialization =========" #

# Get settings from user configuration file
if($userSettingsFile -eq $nul -or $userSettingsFile -eq "")
{
	$userSettingsFile = "..\config.local.xml"
}

[xml]$xmlUserSettings = Get-Content $userSettingsFile

[string] $workingDir = $xmlUserSettings.configuration.localPaths.workingDir
[string] $surveyConnectionString = $xmlUserSettings.configuration.surveyConnectionString
[string] $publishSettingsFile = $xmlUserSettings.configuration.publishSettingsFile
[string] $publishProfileDownloadUrl = $xmlUserSettings.configuration.urls.publishProfileDownloadUrl

[string] $sourceCodeDir = Resolve-Path "..\Code"

write-host "========= Copying source code to working directory... ========="
if (!(Test-Path "$workingDir"))
{
	New-Item "$workingDir" -type directory | Out-Null
}
Copy-Item "$sourceCodeDir\*" "$workingDir" -Recurse -Force
write-host "Copying source code to working directory done!"

write-host "========= Updating Surveys connection string... ========="
[string] $file = Join-Path $workingDir "CloudSurvey\CloudSurvey\web.release.config"
$x = [xml] (Get-Content $file)
$x.SelectNodes("//connectionStrings/add[@name = 'SurveyConnection']").setAttribute("connectionString", $surveyConnectionString)
$x.Save($file)
write-host "Updating Surveys connection string done!"

write-host "========= Install Node Package ... ========="
& npm install azure -g
write-host "========= Installing Node Package done! ... ========="

# "========= Main Script =========" #
if (-not ($publishSettingsFile) -or -not (test-path $publishSettingsFile)) {
    Write-Error "You must specify the publish setting profile. After downloading the publish settings profile from the management portal, specify the file location in the configuration file path under the publishSettingsFile element."
	Write-Host "You should save the publish setting profile into a known and safe location to avoid being removed. Then configure the publishSettingFile in the config.local.xml file."
	
    start $publishProfileDownloadUrl
    exit 1
}

#========= Importing the Windows Azure Subscription Settings File... =========
& ".\tasks\import-waz-publishsettings.ps1" -wazPublishSettings $publishSettingsFile
