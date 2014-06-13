<a name="title" />
# Using CSUpload #

---
<a name="Overview" />
## Overview ##
CSUpload allows you to upload VHDs as data disks or bootable OS disks. In this demonstration you will create a data disk and show how you can upload it to Microsoft Azure and also demonstrate how to attach it to an existing virtual machine.


<a name="technologies" />
### Key Technologies ###

- Microsoft Azure subscription - you can sign up for free trial [here][1]
- Microsoft Azure Virtual Machines 
- [CSUpload.exe part of the Microsoft Azure 1.8 SDK][2]

[1]: http://bit.ly/WindowsAzureFreeTrial
[2]: http://www.microsoft.com/windowsazure/sdk/

<a name="GettingStarted" />
### Getting Started: Obtaining Subscription's Credentials ###

In order to complete this lab, you will need your subscription’s secure credentials. In the following task, you will download the Publish Settings file with all the information required to manage your account in your development environment.

<a name="GSTask1" />
#### Task 1 - Downloading and Importing a Publish Settings File ####

In this task, you will log on to the Microsoft Azure portal and download the Publish Settings file. This file contains the secure credentials and additional information about your Microsoft Azure Subscription to use in your development environment. Then, you will import this file using the Microsoft Azure Cmdlets in order to install the certificate and obtain the account information.

1.	Open Internet Explorer and go to <https://windows.azure.com/download/publishprofile.aspx> and sign in using your Microsoft Account credentials.

1.	**Save** the Publish Settings file to your local machine.

	![Downloading publish-settings file](Images/downloading-publish-settings-file.png?raw=true "Downloading publish-settings file")

	_Downloading Publish Settings file_

	> **Note:** The download page shows you how to import the Publish Settings file using Visual Studio Publish box. This lab will show you how to import it using the Microsoft Azure PowerShell Cmdlets instead.

1. Start **Microsoft Azure PowerShell** with administrator privileges by selecting **Run as Administrator**.

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

1.	The following script imports your Publish Settings file and generates an XML file with your account information. You will use these values during the lab to manage your Microsoft Azure subscription. Replace the placeholder with your publish-setting file's path and execute the script.

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
<a name="segment1" />
### Creating and Uploading a VHD as a Data Disk ###

1. Launch Control Panel -> System and Security -> Administrative Tools -> Computer Management -> Disk Management

1. Right click on Disk Management and select Create VHD.

	![CreateVHD](Images/createvhd.png?raw=true)

1. Give the name and location of the vhd (mydatadisk.vhd). Specify 50 MB for the size (small so you can demonstrate uploading). Note also that Microsoft Azure only supports VHD and Fixed size. 

	![CreateVHDProps](Images/createvhdprops.png?raw=true)

1. Right click on the disk and select initialize disk.

	![InitializeDisk](Images/initializedisk.png?raw=true)

	> **Note:** In the **Initialize Disk** dialog, select the **MBR (Master Boot Record)** partition style.

1. From there right click and create a new simple volume. Accept the defaults for the dialog questions.

	![new-simple-volume](Images/new-simple-volume.png?raw=true)

1. The disk status should be healthy and it will be mounted on your machine.

	![finished-disk](Images/finished-disk.png?raw=true)

1. Using Windows Explorer copy a file into the disk. 

1. Once the file is copied over right click on the drive in disk manager and select **Detach VHD.**

	![detach-vhd](Images/detach-vhd.png?raw=true)

1. Open \Source\Assets\UploadDisk.cmd and modify the following values
	- [SUBSCRIPTION ID]
	- [STORAGE ACCOUNT NAME]
	- [CERT THUMB PRINT]

	> **Note:** You can retrieve the certificate thumbprint value from the Microsoft Azure PowerShell cmdlets configuration settings file (C:\Users\\[YOUR-USER-NAME]\AppData\Roaming\Microsoft Azure Powershell\DefaultSubscriptionData.xml). Ensure the thumbprint comes from the same subscription that contains your storage account. Additionally, if you stored the VHD file in a different path, update that value.
	
	> The paths to the commands in the cmd file depend on the Azure SDK version, therefore if you	 have a different version other than 1.8 make sure you update the following line: `C:\Program Files\Microsoft SDKs\Microsoft Azure\.NET SDK\2012-10\bin\csupload.exe`

1. Run UploadDisk.cmd and in the output note how csupload.exe detects empty blocks in the file. It does not upload them for efficiency. 

1. Once the upload has completed open IISVM1 in the Microsoft Azure Management portal. 

	>**Note:** The _IISVM1_ virtual machine is used in other demos of this training kit, you can use that one or any other virtual machine for this demo purposes.

1. Click attach and then attach disk. 

	![attachdisk](Images/attachdisk.png?raw=true)

1. Select mydatadisk.vhd in the dropdown list.

	![select-mydatadisk](Images/select-mydatadisk.png?raw=true)

1. Click the checkbox to attach the disk.

1. RDP into IISVM1 and you should see the uploaded volume as an attached disk. 

1. Explain that the same concept could work for uploading OS disks. The only difference is there is a -OS flag on the csupload.exe command line (Windows or Linux) that tells Microsoft Azure whether the disk is bootable or not. 




