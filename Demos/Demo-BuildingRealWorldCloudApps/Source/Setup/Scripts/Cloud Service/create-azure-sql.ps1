Param(
    [String]$AppDatabaseName = "appdb",
    [String]$MemberDatabaseName = "memberdb",
    [String]$UserName = "dbuser",
    [String]$Password,
    [String]$FirewallRuleName,
    [String]$StartIPAddress,
    [String]$EndIPAddress,
    [String]$Location = "West US"
)

# Begin - Helper functions --------------------------------------------------------------------------------------------------------------------------

# Get the IP Range needed to be whitelisted for SQL Azure
Function Detect-IPAddress
{
    $ipregex = "(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)"
    $text = Invoke-RestMethod 'http://www.whatismyip.com/api/wimi.php'
    $result = $null

    If($text -match $ipregex)
    {
        $ipaddress = $matches[0]
        $ipparts = $ipaddress.Split('.')
        $ipparts[3] = 0
        $startip = [string]::Join('.',$ipparts)
        $ipparts[3] = 255
        $endip = [string]::Join('.',$ipparts)

        $result = @{StartIPAddress = $startip; EndIPAddress = $endip}
    }

    Return $result
}

# Create a PSCrendential object from plain text password.
# The PS Credential object will be used to create a database context, which will be used to create database.
Function New-PSCredentialFromPlainText
{
    Param(
        [String]$UserName,
        [String]$Password
    )

    $securePassword = ConvertTo-SecureString -String $Password -AsPlainText -Force

    Return New-Object System.Management.Automation.PSCredential($UserName, $securePassword)
}

# Generate connection string of a given SQL Azure database
Function Get-SQLAzureDatabaseConnectionString
{
    Param(
        [String]$DatabaseServerName,
        [String]$DatabaseName,
        [String]$UserName,
        [String]$Password
    )

    Return "Server=tcp:{0}.database.windows.net,1433;Database={1};User ID={2}@{0};Password={3};Trusted_Connection=False;Encrypt=True;Connection Timeout=30;" -f
        $DatabaseServerName, $DatabaseName, $UserName, $Password
}

# End - Helper functions --------------------------------------------------------------------------------------------------------------------------

# Begin - Actual script ---------------------------------------------------------------------------------------------------------------------------

$VerbosePreference = "Continue"
$ErrorActionPreference = "Stop"

# Detect IP range for SQL Azure whitelisting if the IP range is not specified
If (-not ($StartIPAddress -and $EndIPAddress))
{
    $ipRange = Detect-IPAddress
    $StartIPAddress = $ipRange.StartIPAddress
    $EndIPAddress = $ipRange.EndIPAddress
}

Write-Verbose ("[Start] creating SQL Azure database server in location {0} with username {1} and password {2}" -f $Location, $UserName, $Password)
$databaseServer = New-AzureSqlDatabaseServer -AdministratorLogin $UserName -AdministratorLoginPassword $Password -Location $Location
Write-Verbose ("[Finish] creating SQL Azure database server {3} in location {0} with username {1} and password {2}" -f $Location, $UserName, $Password, $databaseServer.ServerName)

# Create a SQL Azure database server firewall rule for the IP address of the machine in which this script will run
# This will also whitelist all the Azure IP so that the website can access the database server
Write-Verbose ("[Start] creating firewall rule {0} in database server {1} for IP addresses {2} - {3}" -f $RuleName, $databaseServer.ServerName, $StartIPAddress, $EndIPAddress)
New-AzureSqlDatabaseServerFirewallRule -ServerName $databaseServer.ServerName -RuleName $FirewallRuleName -StartIpAddress $StartIPAddress -EndIpAddress $EndIPAddress -Verbose
New-AzureSqlDatabaseServerFirewallRule -ServerName $databaseServer.ServerName -RuleName "AllowAllAzureIP" -StartIpAddress "0.0.0.0" -EndIpAddress "0.0.0.0" -Verbose
Write-Verbose ("[Finish] creating firewall rule {0} in database server {1} for IP addresses {2} - {3}" -f $FirewallRuleName, $databaseServer.ServerName, $StartIPAddress, $EndIPAddress)

# Create a database context which includes the server name and credential
# These are all local operations. No API call to Windows Azure
$credential = New-PSCredentialFromPlainText -UserName $UserName -Password $Password
$context = New-AzureSqlDatabaseServerContext -ServerName $databaseServer.ServerName -Credential $credential

# Use the database context to create app database
Write-Verbose ("[Start] creating database {0} in database server {1}" -f $AppDatabaseName, $databaseServer.ServerName)
New-AzureSqlDatabase -DatabaseName $AppDatabaseName -Context $context -Verbose
Write-Verbose ("[Finish] creating database {0} in database server {1}" -f $AppDatabaseName, $databaseServer.ServerName)

# Use the database context to create member database
Write-Verbose ("[Start] creating database {0} in database server {1}" -f $MemberDatabaseName, $databaseServer.ServerName)
New-AzureSqlDatabase -DatabaseName $MemberDatabaseName -Context $context -Verbose
Write-Verbose ("[Finish] creating database {0} in database server {1}" -f $MemberDatabaseName, $databaseServer.ServerName)

$appDatabaseConnectionString = Get-SQLAzureDatabaseConnectionString -DatabaseServerName $databaseServer.ServerName -DatabaseName $AppDatabaseName -UserName $UserName -Password $Password
$memberDatabaseConnectionString = Get-SQLAzureDatabaseConnectionString -DatabaseServerName $databaseServer.ServerName -DatabaseName $MemberDatabaseName -UserName $UserName -Password $Password

Return @{ `
    Server = $databaseServer.ServerName; UserName = $UserName; Password = $Password; `
    AppDatabase = @{Name = $AppDatabaseName; ConnectionString = $appDatabaseConnectionString}; `
    MemberDatabase = @{Name = $MemberDatabaseName; ConnectionString = $memberDatabaseConnectionString} `
}

# End - Actual script -----------------------------------------------------------------------------------------------------------------------------