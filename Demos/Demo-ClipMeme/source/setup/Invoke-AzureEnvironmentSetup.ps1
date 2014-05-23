function Invoke-AzureEnvironmentSetup {
    [CmdletBinding()]
    param([Parameter(Mandatory=$true)]$EnvironmentSubscriptionName,
          [Parameter(Mandatory=$true)]$EnvironmentPrimaryLocation, 
          [Parameter(Mandatory=$true)]$StorageEnvironmentLocation, 
          [Parameter(Mandatory=$true)]$EnvironmentWebSites,
          [Parameter(Mandatory=$true)]$EnvironmentStagingSites,
          [Parameter(Mandatory=$true)]$EnvironmentStorageAccount,
          [Parameter(Mandatory=$true)]$StorageContainers,
		  [Parameter(Mandatory=$false)]$publishSettingsFile)

    begin {
	
		Write-Host "Importing Publish Settings file: " (Get-Date)
		if ($publishSettingsFile) { 
            Import-AzurePublishSettingsFile -PublishSettingsFile $publishSettingsFile 
            azure account import $publishSettingsFile
        }
		
        Select-AzureSubscription -Default $EnvironmentSubscriptionName
        azure account set $EnvironmentSubscriptionName

        Write-Host "Start Deleting Services: " (Get-Date)
        
        Write-Host "Deleting Staging WebSites: " (Get-Date)
        $DeleteStagingWebSitesJobs = @()
		$EnvironmentStagingSites.GetEnumerator() | % {
			$DeleteStagingWebSitesJobs += Start-Job -ScriptBlock { if(Test-AzureName -WebSite ($args[0] + "-staging")) { Remove-AzureWebsite -Name $args[0] -Slot Staging -Force } } -ArgumentList $_.Name
		}
		Wait-Job -Job $DeleteStagingWebSitesJobs

        Write-Host "Deleting WebSites: " (Get-Date)
        $DeleteWebSitesJobs = @()
		$EnvironmentWebSites.GetEnumerator() | % {
			$DeleteWebSitesJobs += Start-Job -ScriptBlock { if(Test-AzureName -WebSite $args[0]) { Remove-AzureWebsite -Name $args[0] -Force } } -ArgumentList $_.Name
		}
		Wait-Job -Job $DeleteWebSitesJobs
		
		
        if ((Test-AzureName -Storage $EnvironmentStorageAccount))	{
		    Write-Host "Deleting Storage Containers: " (Get-Date)
            $StorageAccountKey = (Get-AzureStorageKey -StorageAccountName $EnvironmentStorageAccount).Primary        
            $StorageContainerJobs = @()
            $StorageContainers | % {
                $StorageContainerJobs += Start-Job -ScriptBlock { param($container,$storageName,$storageKey) Invoke-Expression -Command "azure storage container delete --container $container --account-name $storageName --account-key $storageKey -q" } -ArgumentList $_, $EnvironmentStorageAccount, $StorageAccountKey
            }
            Wait-Job $StorageContainerJobs
        }

        Write-Host "Finished Deleting Services: " (Get-Date)
    }

    process {
        Write-Host "Starting to Provision Services: " (Get-Date)

        ############################
        ### Web Sites Operations ###
        ############################
		
        Write-Host "Creating Web Sites: " (Get-Date)
        $SiteCreateJobs = @()
        $EnvironmentWebSites.GetEnumerator() | % {    
            #PowerShell doesn't support Scale Mode... Xplat to the rescue!        
            $SiteCreateJobs += Start-Job -ScriptBlock { azure site create --location $args[1] $args[0]; azure site scale mode 'standard' $args[0] } -ArgumentList $_.Name, $_.Value
        }
        Wait-Job -Job $SiteCreateJobs

        Write-Host "Finished Changing Web Sites Mode"

        ## azure site create --slot
        Write-Host "Adding Site Slot to Web Sites"
        $StagingCreateJobs = @()
        $EnvironmentStagingSites.GetEnumerator() | % {
            $StagingCreateJobs += Start-Job -ScriptBlock { New-AzureWebsite -Name $args[0] -Location $args[1] -Slot "Staging" } -ArgumentList $_.Name, $_.Value
        }
        Wait-Job -Job $StagingCreateJobs
        Write-Host "Finished Web Sites Creation"
        
        ##########################
        ### Storage Operations ###
        ##########################
		
		## create storage account if it does not exist
		if (!(Test-AzureName -Storage $EnvironmentStorageAccount))	{
			Write-Host "Creating Storage Account: " (Get-Date)
			$StorageCreateJob =  Start-Job -ScriptBlock { New-AzureStorageAccount -StorageAccountName $args[0] -Label $args[0] -Location $args[1] } -ArgumentList $EnvironmentStorageAccount, $StorageEnvironmentLocation
			
			Wait-Job -Job $StorageCreateJob; # if ($StorageCreateJob.HasMoreData) { Receive-Job -Job $StorageCreateJob };
			Write-Host "Finished Storage Account Creation: " (Get-Date)
		 }

        $StorageContainerJobs = @()
        $StorageAccountKey = (Get-AzureStorageKey -StorageAccountName $EnvironmentStorageAccount).Primary 
        $StorageContainers | % {
            $StorageContainerJobs += Start-Job -ScriptBlock {param($container,$storageName,$storageKey) Invoke-Expression -Command "azure storage container create --permission 'Blob' --container $container --account-name $storageName --account-key $storageKey" } -ArgumentList $_, $EnvironmentStorageAccount, $StorageAccountKey
        }
        Wait-Job $StorageContainerJobs

        #########################
        ### Shared Operations ###
        #########################

        ## azure site connectionstring add
        $StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=$EnvironmentStorageAccount;AccountKey=$StorageAccountKey"
        $WebSitesConnectionStrings = @(@{Name="AzureJobsRuntime";ConnectionString=$StorageConnectionString;Type="Custom"},@{Name="AzureJobsData";ConnectionString=$StorageConnectionString;Type="Custom"},@{Name="StorageConnectionString";ConnectionString=$StorageConnectionString;Type="Custom"})

        Write-Host "Inject Connection Strings into Web Sites (Async)"
        $EnvironmentWebSites.GetEnumerator() | % {
            Start-Job -ScriptBlock { Set-AzureWebsite -ConnectionStrings $args[2] -AppSettings @{"TrafficManagerRegion"=$args[1]} -Name $args[0] } -ArgumentList $_.Name, $_.Value, $WebSitesConnectionStrings
        }
        $EnvironmentStagingSites.GetEnumerator() | % {
            Start-Job -ScriptBlock { Set-AzureWebsite -ConnectionStrings $args[1] -Name $args[0] -Slot 'Staging' } -ArgumentList $_.Name, $WebSitesConnectionStrings
        }

        Write-Host "Finished Provisioning Services: " (Get-Date)
    }
}