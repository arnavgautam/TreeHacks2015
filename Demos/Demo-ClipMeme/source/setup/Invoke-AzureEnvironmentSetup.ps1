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
		# Import progress functions
		. ".\tasks\progress-functions.ps1"
		
		Write-Action "Importing Publish Settings file"
		if ($publishSettingsFile) { 
            Import-AzurePublishSettingsFile -PublishSettingsFile $publishSettingsFile 
            azure account import $publishSettingsFile
        }
		
        Select-AzureSubscription -Default $EnvironmentSubscriptionName
        azure account set $EnvironmentSubscriptionName
		
		Write-Done
		      
        Write-Action "Deleting Staging WebSite"
        $DeleteStagingWebSitesJobs = @()
		$EnvironmentStagingSites.GetEnumerator() | % {
			$DeleteStagingWebSitesJobs += Start-Job -ScriptBlock { if(Test-AzureName -WebSite ($args[0] + "-staging")) { Remove-AzureWebsite -Name $args[0] -Slot Staging -Force } } -ArgumentList $_.Name
		}
		Wait-Job -Job $DeleteStagingWebSitesJobs
		
		Write-Done
		
        Write-Action "Deleting WebSite"
        $DeleteWebSitesJobs = @()
		$EnvironmentWebSites.GetEnumerator() | % {
			$DeleteWebSitesJobs += Start-Job -ScriptBlock { if(Test-AzureName -WebSite $args[0]) { Remove-AzureWebsite -Name $args[0] -Force } } -ArgumentList $_.Name
		}
		Wait-Job -Job $DeleteWebSitesJobs
		
		Write-Done
		
        if ((Test-AzureName -Storage $EnvironmentStorageAccount))	{
		    Write-Action "Deleting Storage Containers"
            $StorageAccountKey = (Get-AzureStorageKey -StorageAccountName $EnvironmentStorageAccount).Primary        
            $StorageContainerJobs = @()
            $StorageContainers | % {
                $StorageContainerJobs += Start-Job -ScriptBlock { param($container,$storageName,$storageKey) Invoke-Expression -Command "azure storage container delete --container $container --account-name $storageName --account-key $storageKey -q" } -ArgumentList $_, $EnvironmentStorageAccount, $StorageAccountKey
            }
            Wait-Job $StorageContainerJobs
			
			Write-Done
        }        
    }

    process {
        ############################
        ### Web Sites Operations ###
        ############################
		
        Write-Action "Creating Web Sites"
        $SiteCreateJobs = @()
        $EnvironmentWebSites.GetEnumerator() | % {    
            #PowerShell doesn't support Scale Mode... Xplat to the rescue!        
            $SiteCreateJobs += Start-Job -ScriptBlock { azure site create --location $args[1] $args[0]; azure site scale mode 'standard' $args[0] } -ArgumentList $_.Name, $_.Value
        }
        Wait-Job -Job $SiteCreateJobs

        Write-Done

        ## azure site create --slot
        Write-ACTION "Adding Site Slot to Web Sites"
        $StagingCreateJobs = @()
        $EnvironmentStagingSites.GetEnumerator() | % {
            $StagingCreateJobs += Start-Job -ScriptBlock { New-AzureWebsite -Name $args[0] -Location $args[1] -Slot "Staging" } -ArgumentList $_.Name, $_.Value
        }
        Wait-Job -Job $StagingCreateJobs
        
		Write-Done
        
        ##########################
        ### Storage Operations ###
        ##########################
		
		## create storage account if it does not exist
		if (!(Test-AzureName -Storage $EnvironmentStorageAccount))	{
			Write-Action "Creating Storage Account"
			$StorageCreateJob =  Start-Job -ScriptBlock { New-AzureStorageAccount -StorageAccountName $args[0] -Label $args[0] -Location $args[1] } -ArgumentList $EnvironmentStorageAccount, $StorageEnvironmentLocation
			
			Wait-Job -Job $StorageCreateJob; # if ($StorageCreateJob.HasMoreData) { Receive-Job -Job $StorageCreateJob };
			Write-Done
		 }

		Write-Action "Creating Storage Containers"
        $StorageContainerJobs = @()
        $StorageAccountKey = (Get-AzureStorageKey -StorageAccountName $EnvironmentStorageAccount).Primary 
        $StorageContainers | % {
            $StorageContainerJobs += Start-Job -ScriptBlock {param($container,$storageName,$storageKey) Invoke-Expression -Command "azure storage container create --permission 'Blob' --container $container --account-name $storageName --account-key $storageKey" } -ArgumentList $_, $EnvironmentStorageAccount, $StorageAccountKey
        }
        Wait-Job $StorageContainerJobs
		Write-Done
		
        #########################
        ### Shared Operations ###
        #########################

        ## azure site connectionstring add
		Write-Action "Injecting Connection String into Web Sites"
        $StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=$EnvironmentStorageAccount;AccountKey=$StorageAccountKey"
        $WebSitesConnectionStrings = @(@{Name="AzureJobsRuntime";ConnectionString=$StorageConnectionString;Type="Custom"},@{Name="AzureJobsData";ConnectionString=$StorageConnectionString;Type="Custom"},@{Name="StorageConnectionString";ConnectionString=$StorageConnectionString;Type="Custom"})
        
        $EnvironmentWebSites.GetEnumerator() | % {
            Start-Job -ScriptBlock { Set-AzureWebsite -ConnectionStrings $args[2] -AppSettings @{"TrafficManagerRegion"=$args[1]} -Name $args[0] } -ArgumentList $_.Name, $_.Value, $WebSitesConnectionStrings
        }
        $EnvironmentStagingSites.GetEnumerator() | % {
            Start-Job -ScriptBlock { Set-AzureWebsite -ConnectionStrings $args[1] -Name $args[0] -Slot 'Staging' } -ArgumentList $_.Name, $WebSitesConnectionStrings
        }

        Write-Done
    }
}