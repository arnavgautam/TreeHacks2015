<a name="Title" />
# Understanding Virtual Machine Imaging with Capture (PowerShell) #

---

<a name="Overview" />
## Overview ##
In this hands-on lab you will walk through creating a customized virtual machine that is customized with Web Server role enabled. You will then learn how to generalize it and save it is an image so that all new virtual machines provisioned from it will have Web Server role enabled by default.

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

- Customize and generalize a virtual machine
- Save the image to the image library
- Provision New Virtual Machines based off of the image

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
 - Members of the [Microsoft Partner Network](<http://aka.ms/watk-mpn>) Cloud Essentials program receive monthly credits of Windows Azure at no charge.
- A Windows Server 2012 virtual machine
 - Follow the [Quickly create a virtual machine](http://msdn.microsoft.com/en-us/library/windowsazure/jj835085.aspx#bk_Quick) section of the [Create or Delete Virtual Machines Using Windows Azure Cmdlets](http://msdn.microsoft.com/en-us/library/windowsazure/jj835085.aspx) how to guide to create a Windows virtual machine (make sure to pick a Windows Server 2012 image from the images list).


---

<a name="Exercises" />
## Exercises ##
The following exercises make up this hands-on lab:

1. [Customizing and Generalizing the Virtual Machine](#Exercise1)
1. [Saving an Image in the Image Library](#Exercise2)
1. [Provisioning New Virtual Machines based on an Image](#Exercise3)

Estimated time to complete this lab: **60 minutes**.

<a name="gettingstarted" />
### Getting Started: Obtaining Subscription's Credentials ###

In order to complete this lab, you will need your subscription’s secure credentials. Windows Azure lets you download a Publish Settings file with all the information required to manage your account in your development environment.

<a name="GSTask1" />
#### Task 1 - Downloading and Importing a Publish Settings File ####

> **Note:** If you have already done these steps on the same computer, you can move on to Exercise 1.

In this task, you will log on to the Windows Azure portal and download the Publish Settings file. This file contains the secure credentials and additional information about your Windows Azure Subscription to use in your development environment. Then, you will import this file using the Windows Azure Cmdlets in order to install the certificate and obtain the account information.

1. Start **Windows Azure PowerShell** with administrator privileges by selecting **Run as Administrator**.

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

1. Execute the following command to download the subscription information. This command will open a web page on the Windows Azure Management Portal.

	````PowerShell
	Get-AzurePublishSettingsFile
	````

1. Sign in using the **Microsoft Account** associated with your **Windows Azure** account.

1.	**Save** the Publish Settings file to your local machine.

	![Downloading publish-settings file](Images/downloading-publish-settings-file.png?raw=true "Downloading publish-settings file")

	_Downloading Publish Settings file_

1.	The following script imports your Publish Settings file and generates an XML file with your account information. You will use these values during the lab to manage your Windows Azure subscription. Replace the placeholder with your publish-setting file's path and execute the script.

	````PowerShell
	Import-AzurePublishSettingsFile '[YOUR-PUBLISH-SETTINGS-PATH]'
	````

	> **Note:** It is recommended that you delete the publishing profile that you downloaded using _Get-AzurePublishSettingsFile_ after you import those settings. Because the management certificate includes security credentials, it should not be accessed by unauthorized users. If you need information about your subscriptions, you can get it from the Windows Azure Management Portal or the Microsoft Online Services Customer Portal.

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

---

<a name="Exercise1" />
###Exercise 1: Customizing and Generalizing the Virtual Machine###

In this exercise you will customize the virtual machine by enabling the Web Server role in Windows Server 2012. 

<a name="Ex1Task1" />
#### Task 1 - Enabling Web Server role ####

1. Start **Windows Azure PowerShell**.

1. Run the following command to list all the service names of your subscription.

	````PowerShell
	Get-AzureService | Select ServiceName
	````
	>**Note:** You should see in the list the service name corresponding to the virtual machine.

1. Run the following command to obtain the DNS name of the virtual machine. Make sure you replace the placeholders accordingly, using the virtual machine name and the service name from the list obtained previously.

	````PowerShell
	$cloudSvcName = '[YOUR-SERVICE-NAME]'
	$vmname = '[YOUR-VM-NAME]'
	$dnsName = (Get-AzureVM -ServiceName $cloudSvcName -Name $vmname).DNSName.split('/')[2]
	````

1. Now execute the following command to obtain the public port of the remote PowerShell endpoint created when the virtual machine was provisioned.

	````PowerShell
	$winRmHTTPsPort = (Get-AzureVM -ServiceName $cloudSvcName -Name $vmname | Get-AzureEndpoint -Name "WinRmHTTPs").Port
	````

1. Type the following command to access remotely to the virtual machine. Replace [YOUR-VM-USERNAME] with the administrator username provided when the virtual machine was created.

	````PowerShell
	Enter-PSSession -ComputerName $dnsName -Port $winRmHTTPsPort -Authentication Negotiate -Credential '[YOUR-VM-USERNAME]' -UseSSL -SessionOption (New-PSSessionOption -SkipCACheck -SkipCNCheck)
	````
	>**Note:** When prompted, login with the administrator password.

1. You should now be at a prompt with the host name to the left.

1. Type the following command to install the Web Server role in the virtual machine.

	````PowerShell
	Install-WindowsFeature -Name Web-Server -IncludeManagementTools
	````

1. Once the process is finished, type the following command to verify the role was installed successfully.

	````PowerShell
	Get-WindowsFeature
	````

	![Role installed](Images/powershell-get-features.png?raw=true)

	_Role installed_

<a name="Ex1Task2" />
#### Task 2 - Generalizing the Machine with SysPrep ####

In this step you will run the System Preparation (Sysprep) tool to generalize the image which will allow multiple virtual machines to be created off it, all having the same customized settings, including Web Server role enabled.

The System Preparation (Sysprep) tool is used to change Windows images from a specialized to a generalized state, and back. A generalized image can be deployed on any computer. A specialized image is targeted to a specific computer. For example, when you use the Sysprep tool to generalize an image, Sysprep removes all system-specific information and resets the computer. You must generalize a Windows image before you capture and deploy the image. 

1. In the PowerShell remote session, type the following command.

	````PowerShell
	$command = "C:\Windows\System32\Sysprep\Sysprep.exe /generalize /oobe /shutdown /quiet"
	Invoke-Expression -command "$command"
	````

1. The Sysprep utility will run in the background. Type _exit_ to leave the remote session.

	>**Note:** When running _exit_ command you might get an access denied error. This is because sysprep is already running. Ignore the error and continue.

---

<a name="Exercise2" />
### Exercise 2: Saving an Image in the Image Library ###

In this exercise you will use the capture feature for virtual machines to create a new image based off of an existing virtual machine (the one generalized in the previous exercise). The example below uses the Windows Azure PowerShell cmdlets but this is also possible in the Windows Azure Management Portal.

>**Note:** Before proceeding ensure the virtual machine you sysprepped is Off (its status should be _Stopped_).

<a name="Ex2Task1" />
#### Task 1 - Saving an Image in the Image Library ####

1. In the **Windows Azure PowerShell** window used in the previous exercise, run the following command to return the current status of the virtual machine. Do not proceed past this point until the status is set to **StoppedVM**.

	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname | Select InstanceStatus 
	````

1. Using the **Save-AzureVMImage** cmdlet you can take the sysprepped virtual machine and capture it as a new re-usable image.

	````PowerShell
	Save-AzureVMImage -ServiceName $cloudSvcName -Name $vmname -NewImageLabel 'custombaseimg' -NewImageName 'custombaseimg'
	````

	![Save-AzureVMImage Cmdlet Output](Images/save-azurevmimage-cmdlet-output.png?raw=true)

	_Save-AzureVMImage Cmdlet Output_

	> **Known issue:** Version **0.7.2.1** of the Windows Azure PowerShell has a known bug that causes the above command to return an **error message** even after being successfully run. This issue has **already been addressed** in the development branch but has not been included in the public package at the time of writing.

---

<a name="Exercise3" />
### Exercise 3: Provisioning New Virtual Machines based on an Image ###

In this exercise you are going to create a new virtual machine using the image you created in the previous exercise.

<a name="Ex3Task1" />
#### Task 1 - Creating a Virtual Machine Based on an Image ####

1. The following command will return the current list of all operating systems in the gallery. Do not proceed after this step if **custombaseimg** does not appear in the return value.

	````PowerShell
	Get-AzureVMImage | Select ImageName
	````

	![Get-AzureVMImage Cmdlet Output](Images/get-azurevmimage-cmdlet-output.png?raw=true)

	_Get-AzureVMImage Cmdlet Output_

1. Run the following commands to create a new virtual machine based on the previously created image and configure an endpoint to listen on port 80 for browsing to the virtual machine through HTTP. Replace the placeholders with the administrator credentials before running the commands.

	````PowerShell
	$vmname = 'customizedvm1'
	$adminUsername = '[YOUR-ADMIN-USERNAME]'
	$password = '[YOUR-ADMIN-PASSWORD]'
	$imgname = 'custombaseimg'
	
	$vm = New-AzureVMConfig -Name $vmname -InstanceSize Small -ImageName $imgname |
		Add-AzureProvisioningConfig -Windows -AdminUsername $adminUsername -Password $password | 
		Add-AzureEndpoint -LocalPort 80 -PublicPort 80 -Protocol tcp -Name 'Http' |
			New-AzureVM -ServiceName $cloudSvcName
	````

	> **Note:** The previous command will deploy the virtual machine using the cloud service created when you provisioned the virtual machine. If you receive an error saying _'The disk's VHD must be in the same account as the VHD of the source image'_, is probably because the previously created virtual machine is in a different account than the one configured with the **Set-AzureSubscription** cmdlet. Try using the Set-AzureSubscription cmdlet to set the current storage account to the same account where the virtual machine was created.

<a name="Ex3Task2" />
#### Task 2 - Navigating to the IIS default web page ####

1. Run the following command to return back the status of your virtual machine. Wait until _InstanceStatus_ is **ReadyRole** before proceeding to the next step.

	````PowerShell
	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname
	````

1. Run again the same command from the previous step but this time take note of the _DNSName_ property.

	![Get-AzureVM Cmdlet Output](Images/get-azurevm-cmdlet-output.png?raw=true)

	_Get-AzureVM Cmdlet Output_

1. Open a web browser and navigate to the DNS address obtained in the previous step.




	![IIS default web page](Images/ie-iis.png?raw=true)

	_IIS default web page_

	>**Note:** Since the virtual machine was created using your custom image with Web Server role enabled, you get the IIS default web page when you browse to the virtual machine.

---

## Next Steps ##

To learn more about configuring Windows virtual machines on Windows Azure, please refer to the following articles:

**Technical Reference**

This is a list of articles that expands the information on the technologies explained on this lab:

- You can continue reading the Hands-on lab **Deploy Active Directory in Windows Azure using PowerShell**.

- [Windows Azure Management Cmdlets Reference](http://aka.ms/bnma6w): This topic provides reference information on the cmdlet sets that are included in the Windows Azure PowerShell module.

- [How to Capture an Image of a Virtual Machine Running Linux](http://aka.ms/W8k4yu): If you want to create multiple virtual machines that are set up the same way, you can capture an image of a configured virtual machine and use that image as a template. This article shows you how to capture a virtual machine running Linux.

- [How to use PowerShell to set up a SQL Server virtual machine in Windows Azure](http://aka.ms/ehtolo): In this tutorial, you can learn how to create multiple SQL Server virtual machines in the same Cloud Service by using the PowerShell cmdlets.

- [Add a Virtual Machine to a Virtual Network](http://aka.ms/pej5x8): This tutorial walks you through the steps to create a Windows Azure storage account and virtual machine (VM) to add to a virtual network.

- [Windows Azure Virtual Networks](http://aka.ms/tj1lj3): Windows Azure Virtual Network provides you with the capability to extend your network into Windows Azure and treat deployments in Windows as a natural extension of your on-premises network.

**Development**

This is a list of useful sample scripts from [Script Center](http://aka.ms/bv06qh) to manage Windows Azure Virtual Machines:

- [Create a New Windows Azure VM Image Based on a Stock Image and a WebPI App](http://aka.ms/I190m6): Creates a new Virtual Machine image based on a given stock image. Installs an application from the Web Platform Installer, then the image is saved to the user's subscription to be used for provisioning future Virtual Machines.

- [Deploy Windows Azure VMs to an Availability Set and Load Balanced on an Endpoint](http://aka.ms/htx61t): Deploy a specified number of Virtual Machines based on a given image name. The Virtual Machines are placed in the same availability set and load balanced on a given endpoint name.

- [Deploy a SQL Server VM with Striped Disks in Windows Azure](http://aka.ms/Fb3jkp): Creates a virtual machine based on SQL Server 2012 on Windows Server 2012 gallery image. Adds four disks to the VM and stripes them into two pools. Formats them for use, creates a database, putting the data files on one volume and the log files on the other.

- [Deploy Multiple Windows Azure VMs in the Same Windows Azure Virtual Network](http://aka.ms/yg0n7j): Creates four Windows Server 2012 Virtual Machines across two separate cloud services and adds them to the same virtual network. If the virtual network indicated does not exist, it is then created.

---

<a name="Summary" />
## Summary ##
By completing this hands-on lab you have learned how to:

 - Customize and Capture a virtual machine to the image library
 - Provision New Virtual Machines based off of the image
 - Navigate to a virtual machine through HTTP 

---
