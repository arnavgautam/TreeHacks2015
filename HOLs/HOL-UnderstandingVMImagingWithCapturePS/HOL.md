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

- [Windows PowerShell 3.0] (http://microsoft.com/powershell/)
- [Windows Azure PowerShell Cmdlets](http://msdn.microsoft.com/en-us/library/windowsazure/jj156055)
- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)
- Complete the _Provisioning a Windows Azure Virtual Machine (PowerShell)_ HOL.

<a name="Setup" />
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open Windows Explorer and browse to the lab's **Source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process that will configure your environment.

1. If the User Account Control dialog is shown, confirm the action to proceed.

> **Note:** Make sure you have checked all the dependencies for this lab before running the setup. 

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

In order to complete this lab, you will need your subscription’s secure credentials. In the following task, you will download the Publish Settings file with all the information required to manage your account in your development environment.

<a name="GSTask1" />
#### Task 1 - Downloading and Importing a Publish Settings File ####

In this task, you will log on to the Windows Azure portal and download the Publish Settings file. This file contains the secure credentials and additional information about your Windows Azure Subscription to use in your development environment. Then, you will import this file using the Windows Azure Cmdlets in order to install the certificate and obtain the account information.

1.	Open Internet Explorer and go to <https://windows.azure.com/download/publishprofile.aspx> and sign in using your Microsoft Account credentials.

1.	**Save** the Publish Settings file to your local machine.

	![Downloading publish-settings file](Images/downloading-publish-settings-file.png?raw=true "Downloading publish-settings file")

	_Downloading Publish Settings file_

	> **Note:** The download page shows you how to import the Publish Settings file using Visual Studio Publish box. This lab will show you how to import it using the Windows Azure PowerShell Cmdlets instead.

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

1.	The following script imports your Publish Settings file and generates an XML file with your account information. You will use these values during the lab to manage your Windows Azure subscription. Replace the placeholder with your publish-setting file's path and execute the script.

	````PowerShell
	Import-AzurePublishSettingsFile '[YOUR-PUBLISH-SETTINGS-PATH]'
	````

1. Execute the following commands and take note of the subscription name and a storage account name you will use for the exercise. Also make note of the location of the storage account.

	````PowerShell
	Get-AzureSubscription | select SubscriptionName
	Get-AzureStorageAccount | select StorageAccountName, Location 
	````

1. If you do **not** have a storage account already created you can use for this exercise you should create one first by following these steps.  

	1. Run the following to determine the data center to create your storage account in. Ensure you pick a data center that shows support for **PersistentVMRole**. 
	
		````PowerShell
		Get-AzureLocation  
		````
	
	1. Create your storage account.
	
		````PowerShell
		New-AzureStorageAccount -StorageAccountName '[YOUR-STORAGE-ACCOUNT]' -Location '[DC-LOCATION]'
		````

		> **Note:** For the _[DC-LOCATION]_ placeholder above, please replace it with the exact text below (minus the number) from the datacenter you chose in the previous step:

		> 1. West US
		> 2. East US
		> 3. East Asia
		> 4. Southeast Asia
		> 5. North Europe
		> 6. West Europe

1. Execute the following command to set your current storage account for your subscription.


	````PowerShell
	Set-AzureSubscription -SubscriptionName '[YOUR-SUBSCRIPTION-NAME]' -CurrentStorageAccount '[YOUR-STORAGE-ACCOUNT]'
	````

---

<a name="Exercise1" />
###Exercise 1: Customizing and Generalizing the Virtual Machine###

In this exercise we are going to customize the Virtual Machine by enabling the Web Server role in Windows Server 2012. 

<a name="Ex1Task1" />
#### Task 1 - Enabling Web Server role ####

1. Go to the **Virtual Machines** page within the Windows Azure Management portal and select the Virtual Machine you created by following the _Provisioning a Windows Azure Virtual Machine (PowerShell)_ HOL.

1. Click on the Virtual Machine name to open its page and click on **Dashboard**. Locate and take note of the DNS.

	![Virtual Machine DNS](Images/virtual-machine-dns.png?raw=true)

	_Virtual Machine DNS_

1.  Now click on **Endpoints** and take note of the public port in the remote PowerShell endpoint that was created when you provisioned the virtual machine.

	![Virtual Machine Endpoints](Images/virtual-machine-endpoints.png?raw=true)

	_Virtual Machine Endpoints_

1. In Windows Azure PowerShell, type the following command to access remotely to the virtual machine. Replace [YOUR-VM-DNS] and [YOUR-ENDPOINT-PORT] placeholders with the values obtained in the previous steps. Replace [YOUR-VM-USERNAME] with the administrator username provided when you created the virtual machine.

	````PowerShell
	Enter-PSSession -ComputerName '[YOUR-VM-DNS]' -Port [YOUR-ENDPOINT-PORT] -Authentication Negotiate -Credential '[YOUR-VM-USERNAME]' -UseSSL -SessionOption (New-PSSessionOption -SkipCACheck -SkipCNCheck)
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

In this step we will run the System Preparation (Sysprep) tool to generalize the image which will allow multiple virtual machines to be created off it, all having the same customized settings, including Web Server role enabled.

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

In this exercise you are going to use the capture feature for virtual machines to create a new image based off of an existing virtual machine (the one previously created). The example below uses the Windows Azure PowerShell cmdlets but this is also possible in the Windows Azure Management Portal.

>**Note:** Before proceeding ensure the virtual machine you sysprepped is Off (its status should be _Stopped_).

<a name="Ex2Task1" />
#### Task 1 - Saving an Image in the Image Library ####

1. Start **Windows Azure PowerShell**.

1. Run the following command to list all the service names of your subscription.

	````PowerShell
	Get-AzureService | Select ServiceName
	````
	>**Note:** You should see in the list the service name you specified when creating the virtual machine.

1. Run the following command to return the current status of the virtual machine. Do not proceed past this point until the status is set to **StoppedVM**. Make sure you replace the plaholders accordingly, using the virtual machine name and service name from the list obtained previously.

	````PowerShell
	$cloudSvcName = '[YOUR-SERVICE-NAME]'
	$vmname = '[YOUR-VM-NAME]'
	Get-AzureVM -ServiceName $cloudSvcName -Name $vmname  | Select InstanceStatus 
	````

1. Using the **Save-AzureVMImage** cmdlet you can take the sysprepped virtual machine and capture it as a new re-usable image.

	````PowerShell
	Save-AzureVMImage -ServiceName $cloudSvcName -Name $vmname -NewImageLabel 'custombaseimg' -NewImageName 'custombaseimg'
	````

	![Save-AzureVMImage Cmdlet Output](Images/save-azurevmimage-cmdlet-output.png?raw=true)

	_Save-AzureVMImage Cmdlet Output_

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

<a name="Summary" />
## Summary ##
By completing this hands-on lab you have learned how to:

 - Customize and Capture a virtual machine to the image library
 - Provision New Virtual Machines based off of the image
 - Navigate to a virtual machine through HTTP 

---
