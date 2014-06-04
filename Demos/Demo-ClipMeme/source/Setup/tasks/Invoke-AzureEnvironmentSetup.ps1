function Invoke-AzureEnvironmentSetup {
    [CmdletBinding()]
    param([Parameter(Mandatory=$true)]$EnvironmentSubscriptionName,
          [Parameter(Mandatory=$true)]$EnvironmentPrimaryLocation, 
          [Parameter(Mandatory=$true)]$StorageEnvironmentLocation, 
          [Parameter(Mandatory=$true)]$EnvironmentWebSites,
          [Parameter(Mandatory=$true)]$EnvironmentStagingSites,
          [Parameter(Mandatory=$true)]$EnvironmentStorageAccount,
          [Parameter(Mandatory=$true)]$StorageContainers,
		  [Parameter(Mandatory=$true)]$AppSettings,
		  [Parameter(Mandatory=$true)]$TrafficManagerProfile,
		  [Parameter(Mandatory=$false)]$publishSettingsFile)

    begin {	
		# Import progress functions
		. ".\tasks\progress-functions.ps1"
		
		$TrafficManagerDomain =  "$TrafficManagerProfile.trafficmanager.net"
		
		Get-AzureAccount | Remove-AzureAccount -Force
		
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
			$DeleteStagingWebSitesJobs += Start-Job -ScriptBlock { if(Test-AzureName -WebSite ($args[0])) { Remove-AzureWebsite -Name $args[0] -Slot Staging -Force } } -ArgumentList $_.Name
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
			
			Write-Action "Deleting Storage Table"
			$StorageTableJob = Start-Job -ScriptBlock { Remove-AzureStorageTable -Name "MemeMetadata" -Force -ErrorAction ignore }			
			Wait-Job $StorageTableJob
			Write-Done
			
			Write-Action "Deleting Queue"			
			$StorageQueueJob = Start-Job -ScriptBlock { Remove-AzureStorageQueue -Name "uploads" -Force -ErrorAction ignore }
			Wait-Job $StorageQueueJob
			Write-Done
        }       

		if (Test-AzureTrafficManagerDomainName -DomainName $TrafficManagerProfile) 
		{
			Write-Action "Deleting Traffic Manager Profile"
			$TrafficManagerJob = Start-Job -ScriptBlock { Remove-AzureTrafficManagerProfile -Name  -Force  } -ArgumentList $TrafficManagerProfile
			
			Wait-Job $TrafficManagerJob
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
			Write-Host $args[0]			
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
		
		Write-Action "Creating Storage Table"
		$StorageTableJob = Start-Job -ScriptBlock { New-AzureStorageTable -Name "MemeMetadata" }			
		Wait-Job $StorageTableJob
		Write-Done
		
		Write-Action "Creating Queue"			
		$StorageQueueJob = Start-Job -ScriptBlock { New-AzureStorageQueue -Name "uploads" }
		Wait-Job $StorageQueueJob
		Write-Done
		
        #########################
        ### Shared Operations ###
        #########################

        ## azure site connectionstring add
		Write-Action "Injecting Settings into Web Sites"
        $StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=$EnvironmentStorageAccount;AccountKey=$StorageAccountKey"
        $WebSitesConnectionStrings = @(@{Name="AzureJobsRuntime";ConnectionString=$StorageConnectionString;Type="Custom"},@{Name="AzureJobsData";ConnectionString=$StorageConnectionString;Type="Custom"},@{Name="StorageConnectionString";ConnectionString=$StorageConnectionString;Type="Custom"})
        
        $EnvironmentWebSites.GetEnumerator() | % {
            $UpdateSettingsJob = Start-Job -ScriptBlock { Set-AzureWebsite -AppSettings $args[3] -WebSocketsEnabled $true -ConnectionStrings $args[2] -Name $args[0] } -ArgumentList $_.Name, $_.Value, $WebSitesConnectionStrings, $AppSettings
			Wait-Job -Job $UpdateSettingsJob;
        }
		
        $EnvironmentStagingSites.GetEnumerator() | % {
            $UpdateSettingsJob = Start-Job -ScriptBlock { Set-AzureWebsite -AppSettings $args[3] -WebSocketsEnabled $true -ConnectionStrings $args[1] -Name $args[0] -Slot 'Staging' } -ArgumentList $_.Name, $WebSitesConnectionStrings, $AppSettings
			Wait-Job -Job $UpdateSettingsJob;
        }

        Write-Done
		
		## Traffic Manager Profile ##
		Write-Action "Creating Traffic Manager Profile"
		$CreateTrafficManagerJob = Start-Job -ScriptBlock { New-AzureTrafficManagerProfile -Name $args[0] -DomainName $args[1] -LoadBalancingMethod "Performance" -MonitorProtocol "Http" -MonitorPort 80 -MonitorRelativePath "/" -Ttl 30 } -ArgumentList $TrafficManagerProfile, $TrafficManagerDomain
		Wait-Job $CreateTrafficManagerJob
		Write-Done
    }
}