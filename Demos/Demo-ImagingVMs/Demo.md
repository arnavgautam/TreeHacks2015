<a name="title" />
# Imaging Virtual Machines #

---
<a name="Overview" />
## Overview ##

Windows Azure Virtual Machines allow you to customize a virtual machine by installing software or making configuration changes and then saving the result as a generalized image. In this demonstration you will show how to generalize a customized Windows virtual machine and capture it as an image. You will then show how you can create multiple identical virtual machines from the same image.

<a name="technologies" />
### Key Technologies ###

- Windows Azure subscription - you can sign up for free trial [here][1]
- Windows Azure Virtual Machines 
- [Windows Azure PowerShell Cmdlets][2]

[1]: http://bit.ly/WindowsAzureFreeTrial
[2]: http://go.microsoft.com/?linkid=9811175&clcid=0x409

<a name="setup" />
### Setup and Configuration ###

To complete this demonstration you need a virtual machine that you will use to create an image, you can use the **iisvm2** virtual machine created in the [Migrating a Web Farm](https://github.com/WindowsAzure-TrainingKit/DEMO-MigratingWebFarm/blob/master/Demo.md) demo, or create one using the following steps.

1.	Create a new **Windows Server 2012 Datacenter** virtual machine in the **Windows Azure Management** portal using the gallery. Set the virtual machine name to _iisvm2_.

1. On the Virtual Machines page, wait until **iisvm2** is running. Select it and then on the toolbar, click **Connect**. Open the RDP connection, and log on with the credentials you used to create it.

1. In _iisvm2_, open the the **Windows PowerShell** window,
and then run the command:

	````PowerShell
Install-WindowsFeature -Name  Web-WebServer
```

	![Installing IIS using PowerShell](./Images/Installing-IIS-using-PowerShell.png?raw=true "Installing IIS using PowerShell")

1. Wait until the IIS installation is completed.

<a name="GettingStarted" />
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

1. Execute the following command to set your current storage account for your subscription.


	````PowerShell
	Set-AzureSubscription -SubscriptionName '[YOUR-SUBSCRIPTION-NAME]' -CurrentStorageAccount '[YOUR-STORAGE-ACCOUNT]'
	````

---
<a name="Demo" />
## Demo ##


<a name="creatinganimage" />
### Creating a Customized Image ###

1. RDP into **iisvm2** and click **Start -> Run**. Type in _sysprep_ and press **ENTER**.

1. In the opened window, right-click on _sysprep.exe_ and select **Run as administrator**.

	![SysPrep](Images/sysprep.png?raw=true)

1.  Select **Enter System Out-of-Box Experience (OOBE)**, check **Generalize** and select **Shutdown**.

1. Press **OK** and wait for the virtual machine to show the **Stopped** status in the Windows Azure Management portal.

	>**Note**: It takes from 8 to 10 minutes to stop the virtual machine.

1. Highlight the virtual machine and press the **Capture** button in the toolbar.

	![capture](Images/capture.png?raw=true)

6. When the capture dialog appears, name the new image **webappimg** and check **I have run sysprep in the virtual machine**.

	![capture-img-name](Images/capture-img-name.png?raw=true)

7. Once the virtual machine has completed capturing, note that _iisvm2_ is no longer listed and show the available images by clicking **Images** on the virtual machine's listing page. 

	![imagelist](Images/imagelist.png?raw=true)

8. The final step of the demo is to show how to create multiple virtual machines from the image. Paste in and run the following powershell script to create three identical virtual machines using the image name.

	````PowerShell
	Set-AzureSubscription '[SUBSCRIPTION-NAME]' -CurrentStorageAccount '[STORAGE-ACCOUNT-NAME]' 
	$adminUser = "[ADMIN-USER]"
	$adminPassword = "[ADMIN-PWD]"
	$cloudService = '[SERVICE-NAME]'
	
	$iisvm2 = New-AzureVMConfig -Name 'iisvm2' -InstanceSize Small -ImageName 'WebAppImg' |
		Add-AzureEndpoint -Name web -LocalPort 80 -PublicPort 80 -Protocol tcp -LBSetName lbweb -ProbePath '/' -ProbeProtocol http -ProbePort 80 |
		Add-AzureProvisioningConfig -Windows  -AdminUserName $adminUser -Password $adminPassword
	
	$iisvm3 = New-AzureVMConfig -Name 'iisvm3' -InstanceSize Small -ImageName 'WebAppImg' |
		Add-AzureEndpoint -Name web -LocalPort 80 -PublicPort 80 -Protocol tcp -LBSetName lbweb -ProbePath '/' -ProbeProtocol http -ProbePort 80 |
		Add-AzureProvisioningConfig -Windows  -AdminUserName $adminUser -Password $adminPassword
	
	
	$iisvm4 = New-AzureVMConfig -Name 'iisvm4' -InstanceSize Small -ImageName 'WebAppImg' |
	    Add-AzureEndpoint -Name web -LocalPort 80 -PublicPort 80 -Protocol tcp -LBSetName lbweb -ProbePath '/' -ProbeProtocol http -ProbePort 80 |
	    Add-AzureProvisioningConfig -Windows  -AdminUserName $adminUser -Password $adminPassword
	
	New-AzureVM -ServiceName $cloudService -VMs $iisvm2, $iisvm3, $iisvm4 
	````


	> **Note:** Ensure to replace the subscription name and storage account name with the ones where the image was stored. Additionally, replace the service name you used  in the **Migrating a Web Farm** demo and provide an admin username and password. If you created the virtual machine from scratch use a new service name of your preference, but in this case you may need to specify the **Location** parameter in the **New-AzureVM** command to the same location where the image resides.

1. The three virtual machines based on the image will be created. You can see them in the virtual machines view of the management portal.

	![VMlist](Images/vmlist.png?raw=true)
