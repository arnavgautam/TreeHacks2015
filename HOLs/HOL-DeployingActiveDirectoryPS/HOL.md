<a name="Deploy-AD-in-Windows-Azure" />
# Deploy Active Directory in Windows Azure using PowerShell#

---
<a name="Overview" /></a>
## Overview ##

When deploying Active Directory in Windows Azure, two aspects are important to point out.

The first one is the networking configuration. Domain members and domain controllers need to find the DNS server hosting the domain DNS information. You will use the Azure network configuration, to set up the DNS service.

Secondly, it is important to prevent Active Directory database corruption. Active Directory assumes that it can write its database updates directly to disk. That means that you should place the Active Directory database files on a data disk that does not have write caching enabled.

<a name="Objectives" /></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Provision a data disk to a Virtual Machine
- Deploy a Domain Controller in Windows Azure

<a name="Prerequisites" />
### Prerequisites ###
 
The following is required to complete this hands-on lab:
 
- [Windows PowerShell 3.0]( http://microsoft.com/powershell/) (or higher)
- Windows Azure PowerShell Cmdlets v0.7.1 (or higher)
	- Follow the [Install Windows Azure PowerShell](http://www.windowsazure.com/en-us/manage/install-and-configure-windows-powershell/#Install) how to guide to install the cmdlets
- A Windows Azure subscription
	- Sign up for a [Free Trial](http://aka.ms/watk-freetrial)
	- If you are a Visual Studio Professional, Test Professional, Premium or Ultimate with MSDN or MSDN Platforms subscriber, activate your [MSDN benefit](http://aka.ms/watk-msdn) now to start development and test on Windows Azure.
	- [BizSpark](http://aka.ms/watk-bizspark) members automatically receive the Windows Azure benefit through their Visual Studio Ultimate with MSDN subscriptions.
	- Members of the [Microsoft Partner Network](http://aka.ms/watk-mpn) Cloud Essentials program receive monthly credits of Windows Azure at no charge.
- A Windows Server 2012 virtual machine
	- Follow the [Quickly create a virtual machine](http://msdn.microsoft.com/en-us/library/windowsazure/jj835085.aspx#bk_Quick) section of the [Create or Delete Virtual Machines Using Windows Azure Cmdlets](http://msdn.microsoft.com/en-us/library/windowsazure/jj835085.aspx) how to guide to create a Windows Server virtual machine (make sure to pick a Windows Server 2012 image from the images list).

		> **Note:**  You can use the following command to retrieve the name of the latest Windows Server 2012 image available.

		> ```PowerShell
		$imageName = @($images | Where {$_.ImageName -match "106__Windows-Server-2012"})[-1].ImageName
		```


>**Note:** In order to run through the complete hands-on lab, you must have network connectivity. 

<a name="gettingstarted" /></a>
### Getting Started: Obtaining Subscription's Credentials ###

In order to complete this lab, you will need your subscription’s secure credentials. Windows Azure lets you download a Publish Settings file with all the information required to manage your account in your development environment.

<a name="GSTask1" /></a>
#### Task 1 - Downloading and Importing a Publish Settings file ####

> **Note:** If you have done these steps in a previous lab on the same computer you can move on to Exercise 1.

In this task, you will log on to the Windows Azure Portal and download the Publish Settings file. This file contains the secure credentials and additional information about your Windows Azure Subscription that you will use in your development environment. Therefore, you will import this file using the Windows Azure Cmdlets in order to install the certificate and obtain the account information.

1. Search for **Windows Azure PowerShell** in the Start screen and choose **Run as Administrator**.

1.	Change the PowerShell execution policy to **RemoteSigned**. When asked to confirm press **Y** and then **Enter**.
	
	````PowerShell
	Set-ExecutionPolicy RemoteSigned
	````

	> **Note:** The Set-ExecutionPolicy cmdlet enables you to determine which Windows PowerShell scripts (if any) will be allowed to run on your computer. Windows PowerShell has four different execution policies:
	>
	> - _Restricted_ - No scripts can be run. Windows PowerShell can be used only in interactive mode.
	> - _AllSigned_ - Only scripts signed by a trusted publisher can be run.
	> - _RemoteSigned_ - Downloaded scripts must be signed by a trusted publisher before they can be run.
	> - _Unrestricted_ - No restrictions; all Windows PowerShell scripts can be run.
	>
	> For more information about Execution Policies refer to this TechNet article: <http://technet.microsoft.com/en-us/library/ee176961.aspx>

1.	Execute the following command to download the subscription information. This command will open a web page on the Windows Azure Management Portal.

	````PowerShell
	Get-AzurePublishSettingsFile
	````

1.	Sign in using the **Microsoft Account** associated with your **Windows Azure** account.

1.	**Save** the Publish Settings file to your local file system.

	![Downloading publish-settings file](Images/downloading-publish-settings-file.png?raw=true 'Downloading publish-settings file')

	_Downloading Publish Settings file_

1.	The following script imports your Publish Settings file and generates an XML file with your account information. You will use these values during the lab to manage your Windows Azure Subscription. Replace the placeholder with the path to your Publish Setting file and execute the script.

	````PowerShell
	Import-AzurePublishSettingsFile '[YOUR-PUBLISH-SETTINGS-PATH]'   
	````

	> **Note:** It is recommend that you delete the publishing profile that you downloaded using _Get-AzurePublishSettingsFile_ after you import those settings. Because the management certificate includes security credentials, it should not be accessed by unauthorized users. If you need information about your subscriptions, you can get it from the Windows Azure Management Portal or the Microsoft Online Services Customer Portal.

1. Execute the following command and take note of the subscription name you will use for this exercise.
 
	````PowerShell
	Get-AzureSubscription | select SubscriptionName
	````
1. Execute the following command and take note of the storage account name you will use for the exercise.

	````PowerShell
	Get-AzureStorageAccount | Where { $_.Location -eq '[DC-LOCATION]' } | select StorageAccountName
	````
 
	> **Note:** For the _[DC-LOCATION]_ placeholder above, please replace it with the deployment location of your virtual machine.
 
1. If the preceding command does NOT return a storage account, you should create one first. To do this, execute the following command:
               
	````PowerShell
	New-AzureStorageAccount -StorageAccountName '[YOUR-STORAGE-ACCOUNT]' -Location '[DC-LOCATION]'
	````
 
	> **Note:** For the _[DC-LOCATION]_ placeholder above, please replace it with the deployment location of your virtual machine.
 
1. Execute the following command to set your current storage account for your subscription.

	````PowerShell
	Set-AzureSubscription -SubscriptionName '[YOUR-SUBSCRIPTION-NAME]' -CurrentStorageAccount '[YOUR-STORAGE-ACCOUNT]'
	````

<a name="Exercises" /></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Adding a new data disk to the virtual machine](#Exercise1)
1. [Deploying a new domain controller in Windows Server 2012](#Exercise2)

<a name="Exercise1" /></a>
### Exercise 1: Adding a new data disk to the virtual machine ###

You will now modify the virtual machine you already created. We will create and provision a data disk to this existing VM which will be used in exercise 2 to place the AD database files.

Exercise 1 contains 2 tasks:

1. Attaching a data disk to your VM
1. Configuring a new data disk on your VM

<a name="Ex1Task1" /></a>
#### Task 1 - Attaching a data disk to your VM####

1. Start **Windows Azure PowerShell**.

1. Run the following command to add a data disk to the existing virtual machine. Make sure you replace the placeholder accordingly, using the service name and the virtual machine name you provided when creating the virtual machine for this lab.

	````PowerShell
	$cloudSvcName = '[YOUR-SERVICE-NAME]'
	$vmname = '[YOUR-VM-NAME]'

	Get-AzureVM -Name $vmname -ServiceName $cloudSvcName |
		Add-AzureDataDisk -CreateNew -DiskSizeInGB 10 -DiskLabel 'AD-data' -HostCaching 'None' -LUN 0 |
		Update-AzureVM 
	````

	>**Note:** Notice the HostCaching option set to None. For use with the Active Directory database files, we need to use a data disk without caching. 

	![Adding data disk](./Images/add-data-disk.png?raw=true "Adding data disk")

	_Adding data disk_

<a name="Ex1Task2" /></a>
#### Task 2 - Configuring a new data disk on your VM####

1. In **Windows Azure PowerShell**, run the following command to save the DNS in a variable.

	````PowerShell
	$dnsName = (Get-AzureVM $cloudSvcName).DNSName.split('/')[2]
	````

1. Now execute the following command to save to a variable the remote PowerShell endpoint that was created when you provisioned the virtual machine.

	````PowerShell
	$winRmHTTpsEndpoint = Get-AzureVM $cloudSvcName | Get-AzureEndpoint -Name "WinRmHTTPs"
	````

1. In Windows Azure PowerShell, type the following command to access remotely to the virtual machine. Note that this command use the _$dnsName_ and _$winRmHTTpsEndpoint_ variables obtained in the previous steps. Replace [YOUR-VM-USERNAME] with the administrator username provided when you created the virtual machine.

	````PowerShell
	Enter-PSSession -ComputerName $dnsName -Port $winRmHTTpsEndpoint.Port -Authentication Negotiate -Credential '[YOUR-VM-USERNAME]' -UseSSL -SessionOption (New-PSSessionOption -SkipCACheck -SkipCNCheck)
	````

	>**Note:** When prompted, login with the administrator password.

1. You should now be at a prompt with the host name to the left.

1. Type the following command to list the available disks in the virtual machine and take note of the Number of the disk you added in the previous task.

	>**Note:** You can identify the disk you added through the size, 10 GB, and the partition style listed as RAW.

	````PowerShell
	Get-Disk
	````

	![Get-Disk Cmdlet Output](Images/get-disk-cmdlet-output.png?raw=true)

	_Get-Disk Cmdlet Output_

1. Type the following command to initialize the disk. This will allow the creation of a partition and volume. Make sure to replace [YOUR-DISK-NUMBER] with the disk number you got in the previous step.

	````PowerShell
	Initialize-Disk -Number [YOUR-DISK-NUMBER] -PartitionStyle MBR
	````

	>**Note:** After the command executes, type again _Get-Disk_. You will see the disk partition style now listed as MBR.

	![Initialize-Disk Cmdlet Output](Images/initialize-disk-cmdlet-output.png?raw=true)

	_Initialize-Disk Cmdlet Output_

1. Now you need to create a new partition on the initialized disk and then format the volume. To do this, type the following command. Make sure to replace [YOUR-DISK-NUMBER] with the disk number you got in step 6. When asked for confirmation, type Y to continue.

	````PowerShell
	New-Partition -DiskNumber [YOUR-DISK-NUMBER] -UseMaximumSize -DriveLetter 'F' | 
      Format-Volume -NewFileSystemLabel "AD DS Data" -FileSystem NTFS
	````

	>**Note:** This command will create a new partition on the disk, assigning the drive letter F and using the whole space available. Then, it will format a volume on the newly created partition using NTFS file system.

	![New-Partition Cmdlet Output](Images/new-partition-cmdlet-output.png?raw=true)

	_New-Partition Cmdlet Output_

	>**Note:** Do not close the remote session as you will need it in the next exercise.

<a name="Exercise2" /></a>
### Exercise 2: Deploying a new domain controller in Windows Server 2012 ###
You have just created a base virtual machine, attached the necessary data disk, and provisioned the disk. Now you are going to install and configure active directory and then verify the install was successful.

Exercise 2 contains 3 tasks:

1. Installing the Active Directory Domain Services Role 
1. Configuring the Active Directory Domain Services Role
1. Verifying the Domain Controller Installed Successfully

<a name="Ex2Task1" /></a>
#### Task 1 - Installing the Active Directory Domain Services Role ####

1. In the PowerShell remote session from the previous exercise, type the following command to install the Active Directory role and features:

	````PowerShell
	Add-WindowsFeature -Name AD-Domain-Services  -IncludeManagementTools
	````

	![Adding the AD feature](./Images/adding-the-ad-feature.png?raw=true "Adding the AD feature")

	_Windows is installing the Active Directory Domain Services role_

<a name="Ex2Task2" /></a>
#### Task 2 - Configuring the Active Directory Domain Services Role ####
1. When the feature installation is completed, type the following single command to promote the domain controller:

	````PowerShell
	Install-ADDSForest  -DomainName "contoso.com" -InstallDns:$true  -DatabasePath "F:\NTDS"  -LogPath "F:\NTDS"  -SysvolPath "F:\SYSVOL"  -NoRebootOnCompletion:$false  -Force:$true
	````

	>**Note:** The C: disk is the OS disk, and has caching enabled. The Active Directory database should not be stored on a disk that has write caching enabled. The F: disk is the data disk that you added earlier, and does not have this feature enabled.

1. At the **SafeModeAdministratorPassword** prompt and the **Confirm SafeModeAdministratorPassword** prompt, type the administrator password, and then press **Enter**. 

	![Configuring the administrator password](./Images/configuring-the-administrator-password.png?raw=true "Configuring the administrator password")

	_Configuring the administrator password_

1. Wait for the command to finish.

	![Promoting the domain controller with PowerShell ](./Images/promoting-the-domain-controller-with-powershell.png?raw=true "Promoting the domain controller with PowerShell")

	_Promoting the domain controller with PowerShell_

1. Once the command finishes and the computer is promoted to domain controller, the Virtual Machine will restart. You will lose connection to the remote session and it will attempt to reconnect.

	![Domain controller configured ](./Images/domain-controller-configured.png?raw=true "Domain controller configured")

	_Domain controller configured_

<a name="Ex2Task3" /></a>
#### Task 3 - Verifying the Domain Controller Installed Successfully ####

1. If you lose the remote connection, wait two to three minutes for the Virtual Machine to restart and type the following command in Windows Azure PowerShell in order to connect again. Note that this command use the _$dnsName_ and _$winRmHTTpsEndpoint_ variables obtained in Exercise 1. Replace [YOUR-VM-USERNAME] with the administrator username provided when you created the virtual machine.

	````PowerShell
	Enter-PSSession -ComputerName $dnsName -Port $winRmHTTpsEndpoint.Port -Authentication Negotiate -Credential '[YOUR-VM-USERNAME]' -UseSSL -SessionOption (New-PSSessionOption -SkipCACheck -SkipCNCheck)
	````

	>**Note:** When prompted, login with the administrator password.

1. To verify that the virtual machine is working properly, run the following command:

	````PowerShell
	dcdiag.exe
	````

	![Dcdiag command output](./Images/dcdiag-output.png?raw=true "Dcdiag command output")

	_Dcdiag command output_

	>**Note:** The output of the command confirms that the virtual machine was successfully promoted to domain controller

---

## Next Steps ##

To learn more about configuring Windows virtual machines on Windows Azure, please refer to the following articles:

**Technical Reference**

This is a list of articles that expands the information on the technologies explained on this lab:

- You can continue reading the Hands-on lab **Understanding Virtual Machine Imaging with Capture (PowerShell)**.

- [Windows Azure Management Cmdlets Reference](http://aka.ms/M6sfun): This topic provides reference information on the cmdlet sets that are included in the Windows Azure PowerShell module.

- [Windows Azure Virtual Networks](http://aka.ms/Tj1lj3): Windows Azure Virtual Network provides you with the capability to extend your network into Windows Azure and treat deployments in Windows as a natural extension of your on-premises network.

- [Add a Virtual Machine to a Virtual Network](http://aka.ms/pej5x8): This tutorial walks you through the steps to create a Windows Azure storage account and virtual machine (VM) to add to a virtual network.

- [Load Balancing Virtual Machines](http://aka.ms/Vf3h6k): Endpoints can be used for different purposes, such as to balance the load of network traffic among them, to maintain high availability, or for direct virtual machine connectivity through protocols such as RDP or SSH.

- [How to use PowerShell to set up a SQL Server virtual machine in Windows Azure](http://aka.ms/ehtolo): In this tutorial, you can learn how to create multiple SQL Server virtual machines in the same Cloud Service by using the PowerShell cmdlets.

**Development**

This is a list of useful sample scripts from [Script Center](http://aka.ms/bv06qh) to manage Windows Azure Virtual Machines:

- [Domain Joining Windows Azure Virtual Machines on Provision](http://aka.ms/M0v77a): This example shows how to configure domain join when provisioning virtual machines using the Windows Azure PowerShell cmdlets. It is a requirement to have Active Directory connectivity already in place for this sample to work.

- [Start Windows Azure Virtual Machines on a Schedule](http://aka.ms/dsgp6a): Demonstrates starting a single Virtual Machine or set of Virtual Machines (using a wildcard pattern) within a Cloud Service. This is done by creating scheduled tasks to start the Virtual Machine(s) on a schedule at a specified time.

- [Stop Windows Azure Virtual Machines on a Schedule](http://aka.ms/rx7dvy): Demonstrates stopping a single Virtual Machine or set of Virtual Machines (using a wildcard pattern) within a Cloud Service. This is done by creating scheduled tasks to stop the Virtual Machine(s) on a schedule at a specified time.

- [Deploy Windows Azure VMs to an Availability Set and Load Balanced on an Endpoint](http://aka.ms/htx61t): Deploy a specified number of Virtual Machines based on a given image name. The Virtual Machines are placed in the same availability set and load balanced on a given endpoint name.

- [Deploy Multiple Windows Azure VMs in the Same Windows Azure Virtual Network](http://aka.ms/yg0n7j): Creates four Windows Server 2012 Virtual Machines across two separate cloud services and adds them to the same virtual network. If the virtual network indicated does not exist, it is then created.


---

<a name="Summary"/>
## Summary ##

In this lab, you went through the steps of deploying a new Active Directory Domain controller in a new forest using Windows Azure virtual machines and remote PowerShell.
