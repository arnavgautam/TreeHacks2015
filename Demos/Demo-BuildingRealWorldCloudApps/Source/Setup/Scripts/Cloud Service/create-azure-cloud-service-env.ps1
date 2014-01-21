# How to run the script
# create-azure-cloud-service-env.ps1 -Name yourcloudservicename -SqlDatabasePassw0rd P@ssw0rd

Param(
    [Parameter(Mandatory = $true)]
    [String]$Name,

    [String]$Location = "West US",

    [String]$SqlDatabaseUserName = "dbuser",   # optional    default to "dbuser"
    
    [Parameter(Mandatory = $true)]
    [String]$SqlDatabasePassword,              # required    you can set the value here and make the parameter optional
    
                                               # optional    start IP address of the range you want to whitelist in SQL Azure firewall
    [String]$StartIPAddress,                   #             will try to detect if not specified

                                               # optional    end IP address of the range you want to whitelist in SQL Azure firewall
    [String]$EndIPAddress                      #             will try to detect if not specified
)

# Begin - Helper functions --------------------------------------------------------------------------------------------------------------------------

# Generate environment xml file, which will be used by the deploy script later.
Function Generate-EnvironmentXml
{
    Param(
        [String]$EnvironmentName,
        [String]$CloudServiceName,
        [Object]$Storage,
        [Object]$Sql
    )

    [String]$template = Get-Content ("{0}\cloud-service-environment.template" -f $scriptPath)

    $xml = $template -f $EnvironmentName, $CloudServiceName, `
                        $Storage.AccountName, $Storage.AccessKey, $Storage.ConnectionString, `
                        ([String]$Sql.Server).Trim(), $Sql.UserName, $Sql.Password, `
                        $Sql.AppDatabase.Name, $Sql.AppDatabase.ConnectionString, `
                        $Sql.MemberDatabase.Name, $Sql.MemberDatabase.ConnectionString
    
    $xml | Out-File -Encoding utf8 ("{0}\cloud-service-environment.xml" -f $scriptPath)
}

# End - Helper functions --------------------------------------------------------------------------------------------------------------------------

# Begin - Actual script -----------------------------------------------------------------------------------------------------------------------------

$VerbosePreference = "Continue"
$ErrorActionPreference = "Stop"

# Mark the start time of the script execution
$startTime = Get-Date

Write-Verbose ("[Start] creating Windows Azure cloud service environment {0}" -f $Name)

# Define the names of storage account, SQL Azure database and SQL Azure database server firewall rule
$Name = $Name.ToLower()
$storageAccountName = "{0}storage" -f $Name
$sqlAppDatabaseName = "appdb"
$sqlMemberDatabaseName = "memberdb"
$sqlDatabaseServerFirewallRuleName = "{0}rule" -f $Name

# Get the directory of the current script
$scriptPath = Split-Path -parent $PSCommandPath

# Create a new cloud service
Write-Verbose ("[Start] creating cloud service {0} in location {1}" -f $Name, $Location)
New-AzureService -ServiceName $Name -Location $Location
Write-Verbose ("[Finish] creating cloud service {0} in location {1}" -f $Name, $Location)

# Create a new storage account
$storage = & "$scriptPath\create-azure-storage.ps1" `
    -Name $storageAccountName `
    -Location $Location

# Create a SQL Azure database server, app and member databases
$sql = & "$scriptPath\create-azure-sql.ps1" `
    -AppDatabaseName $sqlAppDatabaseName `
    -MemberDatabaseName $sqlMemberDatabaseName `
    -UserName $SqlDatabaseUserName `
    -Password $SqlDatabasePassword `
    -FirewallRuleName $sqlDatabaseServerFirewallRuleName `
    -StartIPAddress $StartIPAddress `
    -EndIPAddress $EndIPAddress `
    -Location $Location

# Set the default storage account of the subscription
# This storage account will be used when deploying the cloud service cspkg
$s = Get-AzureSubscription -Current
Set-AzureSubscription -SubscriptionName $s.SubscriptionName -CurrentStorageAccount $storage.AccountName

Write-Verbose ("[Finish] creating Windows Azure cloud service environment {0}" -f $Name)

# Write the environment info to an xml file so that the deploy script can consume
Write-Verbose "[Begin] writing environment info to cloud-service-environment.xml"
Generate-EnvironmentXml -EnvironmentName $Name -CloudServiceName $Name -Storage $storage -Sql $sql
Write-Verbose ("{0}\cloud-service-environment.xml" -f $scriptPath)
Write-Verbose "[Finish] writing environment info to cloud-service-environment.xml"

# Mark the finish time of the script execution
$finishTime = Get-Date
# Output the time consumed in seconds
Write-Output ("Total time used (seconds): {0}" -f ($finishTime - $startTime).TotalSeconds)

# End - Actual script -----------------------------------------------------------------------------------------------------------------------------