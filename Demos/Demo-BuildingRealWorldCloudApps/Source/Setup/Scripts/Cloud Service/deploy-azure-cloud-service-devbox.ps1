Param(
    [Parameter(Mandatory = $true)]
    [String]$ProjectFile,          # Point to the .ccproj file of the project you want to deploy
    [Switch]$Launch                # Use this switch parameter if you want to launch a browser to show the website
)

Function Generate-Cscfg
{
    Param(
        [Xml]$EnvXml,
        [String]$SourceCscfgFile
    )

    # Get content of the project source cscfg file
    [Xml]$cscfgXml = Get-Content $SourceCscfgFile

    # Update the cscfg in memory
    Foreach ($role in $cscfgXml.ServiceConfiguration.Role)
    {

        # Change the
        # 1. diagnostics connection string to use the storage account
        # 2. appdb connection string to use the SQL Azure appdb
        # 3. DefaultConnection connection string to use the SQL Azure memberdb
        Foreach ($setting in $role.ConfigurationSettings.Setting)
        {
            Switch ($setting.name)
            {
                "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" {$setting.value = $EnvXml.environment.storage.connectionString}
                "appdb" {$setting.value = $EnvXml.environment.sqlAzure.appdb.connectionString}
                "DefaultConnection" {$setting.value = $EnvXml.environment.sqlAzure.memberdb.connectionString}
            }
        }
    }

    $file = "{0}\ServiceConfiguration.{1}.cscfg" -f $scriptPath, $EnvXml.environment.name
    $cscfgXml.InnerXml | Out-File -Encoding utf8 $file

    Return $file
}

$VerbosePreference = "Continue"
$ErrorActionPreference = "Stop"

# Mark the start time of the script execution
$startTime = Get-Date

# Get the directory of the current script
$scriptPath = Split-Path -parent $PSCommandPath

$publishDir = "{0}\" -f $scriptPath
$cspkg = "{0}\{1}.cspkg" -f $scriptPath, (Get-Item $ProjectFile).BaseName

# Read from cloud-service-environment.xml to get the environment name
[Xml]$envXml = Get-Content ("{0}\cloud-service-environment.xml" -f $scriptPath)
$cloudServiceName = $envXml.environment.name

# Generate the custom cscfg file
$cscfg = Generate-Cscfg -EnvXml $envXml -SourceCscfgFile ("{0}\ServiceConfiguration.Local.cscfg" -f (Get-Item $ProjectFile).DirectoryName)

# Generate the cspkg file
& "$env:windir\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" $ProjectFile /t:Publish /p:TargetProfile=Local /p:PublishDir=$publishDir

# If there is no existing deployment on the cloud service, create a new deployment
# Otherwise, upgrade the deployment using simultaneous mode
# Notice: first time deployment always uses simultaneous mode
$deployment = $null
Try
{
    $deployment = Get-AzureDeployment -ServiceName $cloudServiceName
}
Catch
{
    New-AzureDeployment -ServiceName $cloudServiceName -Slot Production -Configuration $cscfg -Package $cspkg
}
If ($deployment)
{
    Set-AzureDeployment -ServiceName $cloudServiceName -Slot Production -Configuration $cscfg -Package $cspkg -Mode Simultaneous -Upgrade
}

# Mark the finish time of the script execution
$finishTime = Get-Date
# Output the time consumed in seconds
Write-Output ("Total time used (seconds): {0}" -f ($finishTime - $startTime).TotalSeconds)

# Launch the browser to show the website
If ($Launch)
{
    Start-Process -FilePath ("http://{0}.cloudapp.net" -f $cloudServiceName)
}