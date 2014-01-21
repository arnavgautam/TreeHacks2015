Param([string] $SQLAzureServerName,
	[string] $SQLAzureUsername,
	[string] $SQLAzurePassword,
	[string] $azureDbName)

write-host "========= Dropping Configured SQL Azure Database... ========="
& "SqlCmd" @("-S", "$SQLAzureServerName", "-U", "$SQLAzureUsername", "-P", "$SQLAzurePassword", "-Q", "DROP DATABASE $azureDbName;");
write-host "========= Dropping Configured SQL Azure Database Done! ========= "