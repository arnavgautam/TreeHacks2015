<a name='HOLTop' />
# Managing Virtual Machines with the Windows Azure PowerShell Cmdlets #

<a name='Overview' />
## Overview ##
In this hands-on lab you will understand the capabilities of automating the deployment and management of virtual machines in Windows Azure.


<a name='Objectives' />
### Objectives ###
In this hands-on lab, you will learn how to:

- Provision virtual machines
- Perform post provisioning configuration 
- Reboot or start a virtual machine
- Manage disk and image libraries
- Export and import virtual machines
- Enable or disable virtual machine endpoints

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Windows PowerShell 3.0]( <http://microsoft.com/powershell/>) (or higher)
- Windows Azure PowerShell Cmdlets v0.7.1 (or higher)
	- Follow the [Install Windows Azure PowerShell](<http://www.windowsazure.com/en-us/manage/install-and-configure-windows-powershell/#Install>) how-to guide to install the Cmdlets 
- A Windows Azure subscription
	- Sign up for a [Free Trial](<http://aka.ms/watk-freetrial>).
	- If you are a Visual Studio Professional, Test Professional, Premium or Ultimate with MSDN or MSDN Platforms subscriber, activate your [MSDN benefit](<http://aka.ms/watk-msdn>) now to start development and test on Windows Azure.
	- [BizSpark](<http://aka.ms/watk-bizspark>) members automatically receive the Windows Azure benefit through their Visual Studio Ultimate with MSDN subscriptions.
	- Members of the [Microsoft Partner Network](<http://aka.ms/watk-mpn>) Cloud Essentials program receive monthly credits of Windows Azure at no charge.

---

<a name='Exercises' />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Provisioning a Virtual Machine using Windows Azure PowerShell Cmdlets](#Exercise1)

1. [Using PowerShell Cmdlets for Advanced Provisioning](#Exercise2)

<a name="gettingstarted" /></a>
### Getting Started: Obtaining Subscription Credentials ###

In order to complete this lab, you will need your subscription’s secure credentials. Windows Azure lets you download a Publish Settings file with all the information required to manage your account in your development environment.

<a name="GSTask1" /></a>
#### Task 1 - Downloading and Importing a Publish Settings file ####

> **Note:** If you have completed these steps in a previous lab on the same computer, you can move on to Exercise 1.

In this task, you will log on to the Windows Azure Management Portal and download the Publish Settings file. This file contains the secure credentials and additional information about your Windows Azure Subscription that you will use in your development environment. Therefore, you will import this file using the Windows Azure Cmdlets in order to install the certificate and obtain the account information.

1. Search for **Windows Azure PowerShell** in the Start screen and choose **Run as Administrator**.

1. Change the PowerShell execution policy to **RemoteSigned**. When asked to confirm, press **Y** and then **Enter**.
            
	````PowerShell
	Set-ExecutionPolicy RemoteSigned
	````

	> **Note:** The Set-ExecutionPolicy Cmdlet enables you to determine which Windows PowerShell scripts (if any) will be allowed to run on your computer. Windows PowerShell has four different execution policies:
	>
	> - _Restricted_ - No scripts can be run. Windows PowerShell can be used only in interactive mode.
	> - _AllSigned_ - Only scripts signed by a trusted publisher can be run.
	> - _RemoteSigned_ - Downloaded scripts must be signed by a trusted publisher before they can be run.
	> - _Unrestricted_ - No restrictions; all Windows PowerShell scripts can be run.
	>
	> For more information about Execution Policies, refer to this TechNet article: <http://technet.microsoft.com/en-us/library/ee176961.aspx>

1. Execute the following command to download the subscription information. This command will open a web page on the Windows Azure Management Portal.

	````PowerShell
	Get-AzurePublishSettingsFile
	````

1. Sign in using the **Microsoft Account** associated with your **Windows Azure** account.

1. **Save** the Publish Settings file to your local file system.

	![Downloading publish-settings file](Images/downloading-publish-settings-file.png?raw=true 'Downloading publish-settings file')

	_Downloading Publish Settings file_

1. The following script imports your Publish Settings file and generates an XML file with your account information. You will use these values during the lab to manage your Windows Azure Subscription. Replace the placeholder with the path to your Publish Settings file and execute the script.

	````PowerShell
	Import-AzurePublishSettingsFile '[YOUR-PUBLISH-SETTINGS-PATH]'   
	````

	> **Note:** It is recommended that you delete the publishing profile that you downloaded using _Get-AzurePublishSettingsFile_ after you import those settings. Because the management certificate includes security credentials, unauthorized users should not have access. If needed, you can access information about your subscriptions from the Windows Azure Management Portal or the Microsoft Online Services Customer Portal.

1. Execute the following commands and take note of the Subscription name and storage account name you will use for the exercise.

	````PowerShell
	Get-AzureSubscription | select SubscriptionName
	Get-AzureStorageAccount | select StorageAccountName 
	````

	> **Note:** If the command Get-AzureSubscription shows more than one subscription, you can select a subscription other than the default by executing the following:

	````PowerShell
	Select-AzureSubscription 'YOUR-SUBSCRIPTION-NAME'
	````

1. If the preceding command does NOT return a storage account, you should create one first.
  
	1. Run the following command to determine in which data center you will create your storage account. Ensure you pick a data center that shows support for PersistentVMRole. 

		````PowerShell
		Get-AzureLocation  
		````

	1. Create your storage account. 

		````PowerShell
		New-AzureStorageAccount -StorageAccountName '[YOUR-STORAGE-ACCOUNT]' -Location '[DC-LOCATION]'
		````

	1. Execute the following command to set your current storage account for your subscription.

		````PowerShell
		Set-AzureSubscription -SubscriptionName '[YOUR-SUBSCRIPTION-NAME]' -CurrentStorageAccount '[YOUR-STORAGE-ACCOUNT]'
		````

---

<a name='Exercise1' />
### Exercise 1: Provisioning a Virtual Machine using Windows Azure PowerShell Cmdlets###

In this exercise, you will learn how to provision a simple virtual machine in Windows Azure using PowerShell. 

<a name='Ex1Task1'></a>
#### Task 1 - Provisioning a Virtual Machine ####

The first step to create a virtual machine in Windows Azure is to define the virtual machine configuration for items such as endpoints or data disks, and then define which cloud service and data center the virtual machine will reside in. 

1. If not already opened, start **Windows Azure PowerShell** with administrator privileges.

1. Define the **$dclocation** variable with the location of the storage account you've configured in the getting started section (for example, *East US*).

	````PowerShell
	$dclocation = '[YOUR-LOCATION]'
	````
	Once the location is selected, you will need to create the virtual machine configuration. 

	To create virtual machines you will need a few pieces of information: The cloud service name in which the virtual machine will be contained and the virtual machine image name.

1. Select a unique name for your cloud service. To validate the name is not in use you can use **Test-AzureName** Cmdlet. It will return true if the service name already exists.

	````PowerShell
	Test-AzureName -Service '[YOUR-CLOUD-SERVICE-NAME]'

	$cloudSvcName = '[YOUR-CLOUD-SERVICE-NAME]'
	````

1. Select the virtual machine image you want to use as the basis of the virtual machine.

	````PowerShell
	# Retrieves all available virtual machine images 
	Get-AzureVMImage | select ImageName
	````

	````PowerShell
	$image = '[YOUR-SELECTED-IMAGE-NAME]'
	````

	> **Note:** You can choose either Windows or Linux as there are detailed steps for each OS.

1. Next, choose the virtual machine creation script below according to the OS you selected (Windows or Linux).

	1. A Windows virtual machine from an Image.

		````PowerShell
		$adminUserName = '[YOUR-USER-NAME]'
		$adminPassword = '[YOUR-PASSWORD]'
		$vmname = 'mytestvm1'
			
		New-AzureQuickVM -AdminUserName $adminUserName -Windows -ServiceName $cloudSvcName -Name $vmname -ImageName $image -Password $adminPassword -Location $dclocation
		````

	1. A Linux virtual machine from an Image. Notice that the image has changed, and the -OS switch specifies Linux as the operating system.

		````PowerShell
		$linuxuser = '[YOUR-USER-USERNAME]'
		$adminPassword = '[YOUR-PASSWORD]'
		$vmname = 'mytestvm1'
			
		New-AzureQuickVM -Linux -ServiceName $cloudSvcName -Name $vmname -ImageName $image -LinuxUser $linuxuser -Password $adminPassword -Location $dclocation
		````

	> **Note:** Specifying the **-Location** parameter on **New-AzureQuickVM** or **New-AzureVM** tells the Cmdlet to attempt to create a cloud service as a container for the virtual machines. Use this option when creating the first virtual machine and omit it when adding new virtual machines to the same cloud service.

	![New-AzureQuickVM Cmdlet](Images/new-azurequickvm-cmdlet.png?raw=true "New-AzureQuickVM Cmdlet")

	_New-AzureQuickVM Cmdlet Sample Output_

1. Once the virtual machine has been created you can inspect it using the **Get-AzureVM** Cmdlet. The following command will enumerate the details of all the virtual machines in the cloud service.

	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName 
	````

	To be more specific you can use the -Name parameter.

	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname
	````

1. The **Windows Azure PowerShell Cmdlets** support restart, stop and start operations as well as using the **Restart-AzureVM**, **Stop-AzureVM** and **Start-AzureVM** commands. 

	With the following commands you will be able to start, stop and restart your virtual machine.

	````PowerShell
	# Restart
	Restart-AzureVM -ServiceName $cloudSvcName -Name $vmname

	# Shutdown 
	Stop-AzureVM -ServiceName $cloudSvcName -Name $vmname

	# Start
	Start-AzureVM -ServiceName $cloudSvcName -Name $vmname
	````

	>**Note:** Make sure your virtual machine finished provisioning before executing these commands.

---
<a name='Exercise2'/>
### Exercise 2: Using PowerShell Cmdlets for Advanced Provisioning ###

In addition to creating a single uncustomized virtual machine, you can also configure data disks, disk cache settings, networking endpoints and automatically configure domain join settings at provisioning time while batch creating virtual machines using the **New-AzureVMConfig/New-AzureVM** cmdlet combination.

<a name='Ex2Task1' />
#### Task 1 - Performing Custom Provisioning ####

1. Run the Cmdlets below to create two new virtual machine configurations with a 50 GB data disk and a load balanced endpoint open on port 80 for HTTP traffic.

	For Windows:

	````PowerShell
	$vmname2 = 'mytestvm2'
	$vmname3 = 'mytestvm3'


	$vm2 = New-AzureVMConfig -Name $vmname2 -InstanceSize ExtraSmall -ImageName $image |
			Add-AzureProvisioningConfig -Windows -AdminUserName $adminUserName -Password $adminPassword |
			Add-AzureDataDisk -CreateNew -DiskSizeInGB 50 -DiskLabel 'datadisk1' -LUN 0 |
			Add-AzureEndpoint -Protocol tcp -LocalPort 80 -PublicPort 80 -Name 'lbweb' `
				-LBSetName 'lbweb' -ProbePort 80 -ProbeProtocol http -ProbePath '/' 

	$vm3 = New-AzureVMConfig -Name $vmname3 -InstanceSize ExtraSmall -ImageName $image |
			Add-AzureProvisioningConfig -Windows -AdminUserName $adminUserName -Password $adminPassword  |
			Add-AzureDataDisk -CreateNew -DiskSizeInGB 50 -DiskLabel 'datadisk2' -LUN 0  |
			Add-AzureEndpoint -Protocol tcp -LocalPort 80 -PublicPort 80 -Name 'lbweb' `
				-LBSetName 'lbweb' -ProbePort 80 -ProbeProtocol http -ProbePath '/'
````

	For Linux:

	````PowerShell
	$vmname2 = 'mytestvm2'
	$vmname3 = 'mytestvm3'

	$vm2 = New-AzureVMConfig -Name $vmname2 -InstanceSize ExtraSmall -ImageName $image |
			Add-AzureProvisioningConfig -Linux -LinuxUser $linuxUser -Password $adminPassword |
			Add-AzureDataDisk -CreateNew -DiskSizeInGB 50 -DiskLabel 'datadisk1' -LUN 0 |
			Add-AzureEndpoint -Protocol tcp -LocalPort 80 -PublicPort 80 -Name 'lbweb' `
				-LBSetName 'lbweb' -ProbePort 80 -ProbeProtocol http -ProbePath '/' 

	$vm3 = New-AzureVMConfig -Name $vmname3 -InstanceSize ExtraSmall -ImageName $image |
			Add-AzureProvisioningConfig -Linux -LinuxUser $linuxUser -Password $adminPassword |
			Add-AzureDataDisk -CreateNew -DiskSizeInGB 50 -DiskLabel 'datadisk2' -LUN 0 |
			Add-AzureEndpoint -Protocol tcp -LocalPort 80 -PublicPort 80 -Name 'lbweb' `
				-LBSetName 'lbweb' -ProbePort 80 -ProbeProtocol http -ProbePath '/'
	````

1. Now run the **New-AzureVM** Cmdlet to create two virtual machines using the configurations you defined in the previous step.

	````PowerShell
	New-AzureVM -ServiceName $cloudSvcName -VMs $vm2,$vm3
	````

	>**Note:** You will still need to log in to the machine and configure/format the data disk via disk manager. In the next task you will find a walk through for these steps.

1. Verify the virtual machines were created by running the following script.

	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName
	````

<a name='Ex2Task2'></a>
#### Task 2 - Post Provisioning Configuration ####

Modifying an existing virtual machine requires retrieving the current settings by calling **Get-AzureVM**, modifying them, and then calling the **Update-AzureVM** Cmdlet to save the changes.

You can hot add and remove data disks and networking endpoints. Changing disk cache settings requires a reboot as does changing the virtual machine's instance size. 

In this task you will use the **Get-AzureVM** Cmdlet to retrieve the virtual machine object and send it to the PowerShell Pipeline.

The **Add-AzureDataDisk** Cmdlet with the **CreateNew** parameter allows you to dynamically add storage to the virtual machine. In this task, we will call it twice to attach two unformatted blank VHDs to the server, each of 50 gigabytes. The **-LUN** parameter tells the order of the device being attached and the optional **-MediaLocation** parameter is used to specify the location in storage to keep the new VHDs.

**Add-AzureDataDisk** also supports the **Import** parameter to attach a disk in the disk library and **-ImportFrom** to attach a pre-existing disk in storage.

The task also adds a new endpoint for TCP port 1433 internally that is listening externally on port 2000 using the **Add-AzureEndpoint** command. 

1. Use the following script to hot add data disks and endpoints to an existing virtual machine.

	````PowerShell
	$vmname = 'mytestvm1'

	Get-AzureVM -Name $vmname -ServiceName $cloudSvcName |
		Add-AzureDataDisk -CreateNew -DiskSizeInGB 50 -DiskLabel 'datadisk1' -LUN 0 |
		Add-AzureDataDisk -CreateNew -DiskSizeInGB 50 -DiskLabel 'translogs1' -LUN 1 |
		Add-AzureEndpoint -Protocol tcp -LocalPort 1433 -PublicPort 2000 -Name 'sql' |
		Update-AzureVM 
	````

	>**Note:** To connect to SQL Server you would still need to enable 1433 on the Windows Server firewall to connect.

1. Once the **Update-AzureVM** call has completed, you will need to log in to the machine by using the following command. Use the credentials you've configured when creating the virtual machine to log in.

	>**Note:** If you created a **Linux virtual machine,** skip the next steps and follow [this tutorial](http://www.windowsazure.com/en-us/manage/linux/tutorials/virtual-machine-from-gallery#logon) instead to connect via SSH and initialize your disks. Afterwards, continue with the last step of this task.

	````PowerShell
	Get-AzureRemoteDesktopFile -ServiceName $cloudSvcName -Name $vmname -LocalPath 'C:\Temp\myvmconnection.rdp' -Launch 
	````
	> **Note:** Make sure the *C:\Temp* folder is created or change the path.

1. Once logged in, open the Disk Management tool. To do this in Windows, you can use the **WINDOWS+X** key combination and then select Disk Management from the menu.

1. The **Initialize Disk** dialog box will be open. Click **OK** to initialize the disks.

	![InitializeDisk](Images/initializedisk.png?raw=true)

	_Initializing disks_

	![uninitdisk](Images/uninitdisk.png?raw=true)

	_Virtual Machine Disks_

	> **Note:** If the disks are not online, right-click them (on the left) and click **Online**. Once the disks are online you will need to right-click on one and click **Initialize** (on the left).

1. Once the disks are initialized, you will need to right-click on the right side and select **New Simple Volume...** (RAID software is supported so those options are available as well). The **New Simple Volume** wizard will guide you to format the disks and mount them for use. Leave the default values unchanged in the wizard.

	![formatteddisks](Images/formatteddisks.png?raw=true)

	_Formatted disks_

1. You can control the disk cache settings for your data disks by calling **Set-AzureDataDisk** and configuring the HostCaching parameter. Valid values for HostCaching are _ReadOnly_, _ReadWrite_ and _None_. With the following script you are enabling Write Cache on a data disk and viewing the resulting change.

	>**Note:** This change is at the host level and will not be reflected in the disk manager. 
	> By default, write cache is disabled and read cache is enabled on data disks.


	````PowerShell
	Get-AzureVM -Name $vmname -ServiceName $cloudSvcName  |
		   Set-AzureDataDisk -HostCaching ReadWrite -LUN 0 |
		   Set-AzureDataDisk -HostCaching ReadWrite -LUN 1 |
		   Update-AzureVM

	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname | Get-AzureDataDisk
	````

	![Get-AzureVM Cmdlet Result](Images/get-azurevm-cmdlet-result.png?raw=true)

	_Get-AzureVM Cmdlet Sample Output_

<a name='Ex2Task3' />
#### Task 3 - Performing Changes That Require a Reboot ####

Some changes require the virtual machine to be **restarted** when applied. Making changes to the underlying hardware by changing the instance size using **Set-AzureRoleSize**, modifying the OS Disk cache settings with **Set-AzureOSDisk** or moving the virtual machine between subnets using **Set-Subnet** will all result in an automatic restart of the virtual machine.

1. Run the following script to disable the write disk cache, changing the write cache setting of the OS disk from _write cache enabled_ to _write cache disabled_.

	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname |
		Set-AzureDataDisk -HostCaching ReadOnly -LUN 0|
		Set-AzureDataDisk -HostCaching ReadOnly -LUN 1|
		Update-AzureVM 
	````

1. Run the following script to change the instance size of a virtual machine. The virtual machine will reboot as the new hardware is provisioned.
	
	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname |
		Set-AzureVMSize -InstanceSize Medium |
		Update-AzureVM
	````

	![Changing VM instance size](Images/changing-vm-instancesize.png?raw=true)

	_Changing VM instance size_

<a name='Ex2Task4'></a>
#### Task 4 - Managing Disk Images ####

An image is a VHD that you use as a template to create a new virtual machine. An image is a template because, unlike a running virtual machine, it doesn’t have specific settings such as the computer name and user account settings. When you create a virtual machine from an image, an operating system disk is automatically created for the new virtual machine. 

It is simple to view all of the data disks or images in the disk and image repository with PowerShell.
Running the **Get-AzureDisk** command will enumerate all of the data disks in your subscription.

1. Use the following command to retrieve all your subscriptions' disks.
	
	````PowerShell
	Get-AzureDisk
	````

1. You can use PowerShell's built in capabilities to limit the results. For instance, with this example you will be able to find a specific virtual machine's VHD image.

	````PowerShell
	$vmname = 'mytestvm2'
	Get-AzureDisk | Where { $_.AttachedTo.RoleName -eq $vmname }
	````

	> **Note:** _mytestvm2_ is created in Task 1 of this exercise.

1. Currently, when a virtual machine is removed, the underlying VHDs are not removed along with it. PowerShell allows you to clean up the underlying storage when removing a virtual machine.

	The following script removes a specific virtual machine, but first it saves its attached disks so you can later delete them.

	````PowerShell
	$vmname = 'mytestvm2'
	$vmDisks = Get-AzureDisk | Where { $_.AttachedTo.RoleName -eq $vmname }

	Remove-AzureVM -ServiceName $cloudSvcName -Name $vmname
	````

1. The following script removes the orphan disks.

	````PowerShell
	$vmDisks | foreach {
		Remove-AzureDisk -DiskName $_.DiskName -DeleteVHD
	}
	````

	> **Note:** If you get an exception saying the disk is in use when trying to delete, wait a few minutes until the virtual machine has been completely deleted from Windows Azure.

1. Similar functionality exists for managing the image repository on your subscription. With this script you will identify user created images (as opposed to images provided by Windows Azure in the Image Gallery).

	````PowerShell
	Get-AzureVMImage | Where { $_.Category -eq 'User' }
	````

<a name='Ex2Task5' />
####Task 5 - Imaging, Exporting and Importing Virtual Machine Configurations ####

Windows Azure IaaS provides the capability to customize a virtual machine, generalize it using a tool like sysprep, and then capture the virtual machine to the image library. This functionality allows you to create customized images that you can then reuse to generate multiple identical virtual machines. The steps to accomplish this using PowerShell are relatively simple.

1. Execute the following script to create a virtual machine that will be the start of the image.

	1. For Windows virtual machines.

		````PowerShell
	   $vmname = 'winvmforimg'
		New-AzureVMConfig -Name $vmname -InstanceSize Small -ImageName $image |
			Add-AzureProvisioningConfig -Windows -AdminUserName $adminUserName -Password $adminPassword |
			New-AzureVM -ServiceName $cloudSvcName 
		````

	1. For Linux virtual machines.

		````PowerShell
	   $vmname = 'linuxvmforimg'
		New-AzureVMConfig -Name $vmname -InstanceSize Small -ImageName $image |
			Add-AzureProvisioningConfig -Linux -LinuxUser $linuxuser -Password $adminPassword |
			New-AzureVM -ServiceName $cloudSvcName 
		````

1. Now you need to generalize a virtual machine for capturing an image. At this point you would customize the virtual machine with settings required for the captured image. 

	1. Connect to the virtual machine using either RDP or SSH. You can use the **Get-AzureRemoteDesktopFile** Cmdlet as shown in Task 2.

		>**Note:** Make sure your virtual machine has finished provisioning before connecting.

	1. For Windows, run sysprep from within Windows. To do this, open the **Run** dialog box and type **sysprep**. In the opened Windows Explorer, double-click the sysprep executable. Then select **Entire System Out-of-Box Experience (OOBE)**, select the **Generalize** check box, select **Shutdown**, and click **OK**.

		![sysprep](Images/sysprep.png?raw=true)

		_SysPrep_

	1. For Linux virtual machines, run the following script.

		````Bash
		sudo /usr/sbin/waagent -deprovision+user
		````

1. Run the following command to return the current status of the virtual machine. Do not proceed past this point until the status is set to **StoppedVM**.

	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname | Select InstanceStatus 
	````

1. Generate a new image using the **Save-AzureVMImage** Cmdlet. 

	````PowerShell
	Save-AzureVMImage -ServiceName $cloudSvcName -Name $vmname -NewImageName '[YOUR-NEW-VM-IMAGE-NAME]' -NewImageLabel '[YOUR-NEW-IMAGE-LABEL]'
	````

	>**Note:** The **Save-AzureVMImage** Cmdlet makes a running persistent virtual machine available as an image for reuse. For Windows virtual machines, the image should be sysprepped before capture. After performing the capture, you can delete or reprovision the virtual machine using the PostCaptureAction parameter with Delete|Reprovision value.

	> **Known issue:** Version **0.7.2.1** of the Windows Azure PowerShell has a known bug that causes the above command to return an **error message** even after being successfully run. This issue has **already been addressed** in the development branch but has not been included in the public package at the time of writing.

1. Verify the image was created by running the following script.

	````PowerShell
	Get-AzureVMImage -ImageName '[YOUR-NEW-VM-IMAGE-NAME]'
	````
 
<a name='Ex2Task6' />
#### Task 6 - Exporting and Importing Virtual Machine Configuration ####

The Windows Azure PowerShell Cmdlets provide the capability of saving the configuration of a virtual machine. 
This is useful in scenarios where you need to completely remove the virtual machine but also need to be able to easily restore it at some point. It works by understanding the fact that when you remove a virtual machine by default, the underlying data and OS disk in storage is not removed. The **Export-AzureVM** Cmdlet saves all of the configuration of the virtual machine including disk names, endpoint settings and so on, to an XML file. This allows you to delete the virtual machine and then later recreate it using the saved configuration.


1. Run the following script to export the virtual machine Configuration and remove the Deployment.

	````PowerShell
	$vmname = 'mytestvm1' 
	Export-AzureVM -ServiceName $cloudSvcName -Name $vmname -Path 'C:\Temp\mytestvm1-config.xml' 
	Remove-AzureVM -ServiceName $cloudSvcName -Name $vmname
	````

	>**Note:** This code saves the configuration of the mytestvm1 virtual machine and then removes it by removing the virtual machine.
	>Make sure you create a Temp folder within the _C:_ drive before executing the command or changing the path.

1. Once the deployment has been removed (this could take a couple of minutes), you can then recreate the virtual machine from the saved state. Run the following script to import the virtual machine configuration into a new deployment.

	````PowerShell
	Import-AzureVM -Path 'C:\Temp\mytestvm1-config.xml' | New-AzureVM -ServiceName $cloudSvcName 
	````

	>**Note:** In deployments with Virtual Networking this will result in a new IP address so it is not recommended for virtual machines that require a persistent IP such as a domain controller.

<a name='Ex2Task7' />
#### Task 7 - Managing Remote Desktop (RDP) and SSH Connectivity ####

By default, all new virtual machines created from the Windows Azure PowerShell Cmdlets will allow RDP for Windows or SSH. However, you can add or remove endpoints by using the **Remove-AzureEndpoint/Add-AzureEndpoints** Cmdlets.

To discover the ports for these endpoints you can use the **Get-AzureEndpoint** to learn the public port of the Remote Desktop (RDP) or SSH input endpoint.

1. Run the following script to get the virtual machine endpoints and check which ones are opened.

	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname | Get-AzureEndpoint
	````

1. Remove the Remote Desktop endpoint by using the following script.

	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname | Remove-AzureEndpoint –Name "RemoteDesktop" | Update-AzureVM
	````

1. Rerun the script to verify that the endpoint was removed.

	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname | Get-AzureEndpoint 
	````

	![Get-AzureEndpoint Cmdlet Output](Images/get-azureendpoint-cmdlet-output.png?raw=true)

	_Get-AzureEndpoint Cmdlet Output_

---

## Next Steps ##

To learn more about configuring Windows virtual machines on Windows Azure, please refer to the following articles:

**Technical Reference**

This is a list of articles that expands the information on the technologies explained on this lab:

- You can continue reading the Hands-on labs **Understanding Virtual Machine Imaging with Capture (PowerShell)** and **Deploy Active Directory in Windows Azure using PowerShell**.

- [Windows Azure Management Cmdlets Reference](http://aka.ms/Bnma6w): This topic provides reference information on the cmdlet sets that are included in the Windows Azure PowerShell module.

- [How to use PowerShell to set up a SQL Server virtual machine in Windows Azure](http://aka.ms/Ehtolo): In this tutorial, you can learn how to create multiple SQL Server virtual machines in the same Cloud Service by using the PowerShell cmdlets.

- [Add a Virtual Machine to a Virtual Network](http://aka.ms/Pej5x8): This tutorial walks you through the steps to create a Windows Azure storage account and virtual machine (VM) to add to a virtual network.

- [Windows Azure Virtual Networks](http://aka.ms/tj1lj3): Windows Azure Virtual Network provides you with the capability to extend your network into Windows Azure and treat deployments in Windows as a natural extension of your on-premises network.

**Development**

This is a list of useful sample scripts from [Script Center](http://aka.ms/Bv06qh) to manage Windows Azure Virtual Machines:

- [Start Windows Azure Virtual Machines on a Schedule](http://aka.ms/Dsgp6a): Demonstrates starting a single Virtual Machine or set of Virtual Machines (using a wildcard pattern) within a Cloud Service. This is done by creating scheduled tasks to start the Virtual Machine(s) on a schedule at a specified time.

- [Stop Windows Azure Virtual Machines on a Schedule](http://aka.ms/Rx7dvy): Demonstrates stopping a single Virtual Machine or set of Virtual Machines (using a wildcard pattern) within a Cloud Service. This is done by creating scheduled tasks to stop the Virtual Machine(s) on a schedule at a specified time.

- [Deploy Windows Azure VMs to an Availability Set and Load Balanced on an Endpoint](http://aka.ms/Htx61t): Deploy a specified number of Virtual Machines based on a given image name. The Virtual Machines are placed in the same availability set and load balanced on a given endpoint name.

- [Deploy Multiple Windows Azure VMs in the Same Windows Azure Virtual Network](http://aka.ms/Yg0n7j): Creates four Windows Server 2012 Virtual Machines across two separate cloud services and adds them to the same virtual network. If the virtual network indicated does not exist, it is then created.

<a name='Summary' />
## Summary ##

In this hands-on lab you learned how to configure your subscription ID and certificate to manage Windows Azure Virtual Machines. You were also shown the basics of how to provision virtual machines and modify them with hot add capabilities and changes that require rebooting, such as changing the instance size. 

In addition, you were shown how you can use the Windows Azure PowerShell Cmdlets to manage your disk and image libraries along with exporting and importing virtual machine configurations.
