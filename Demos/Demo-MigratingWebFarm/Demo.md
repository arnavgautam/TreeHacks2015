<a name="title" />
# Migrating a Web Farm #

---
<a name="Overview" />
## Overview ##

Microsoft Azure allows you to easily migrate an application that currently runs on-premises or in another virtualization provider straight to the cloud. The demo is implemented by showing the user how to user PowerShell to provision multiple virtual machines complete with load balanced endpoints and data disks. From there you will show the audience how to configure a SQL Server and deploy an MVC4 web application for the web farm. 

<a name="technologies" />
### Key Technologies ###

- Microsoft Azure subscription - you can sign up for free trial [here][1]
- Microsoft Azure Virtual Machines 
- [Microsoft Azure PowerShell Cmdlets][2]

[1]: http://bit.ly/WindowsAzureFreeTrial
[2]: http://go.microsoft.com/?linkid=9811175&clcid=0x409

<a name="setup" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment.

1. Download, install and configure the Microsoft Azure PowerShell cmdlets. Instructions on how to configuration the cmdlets with your subscription can be found here: http://msdn.microsoft.com/en-us/library/windowsazure/jj554332.aspx

1. Download and install the latest node.js library from: http://nodejs.org 

1. Then install the node.js command line tools for Microsoft Azure by running the following command at an elevated command prompt:

	````PowerShell
	npm install azure -g
	````

	> **Note:** Alternatively, you can install the **Microsoft Azure Cross-platform Command Line Tools** from Web Platform Installer (which will install node.js)

1. Modify the **Config.Azure.xml** for your Microsoft Azure Subscription. The following values are needed:
	- Target Storage Account Name, Container (does not need to be pre-created) and Key where the demo VHDs will be copied to (just the storage account name not the url). 
	- The Storage Account should be in **West US** to allow the VHDs to copy in a timely manner.
	- Subscription Name - value can be retrieved from Microsoft Azure PowerShell by running **Get-AzureSubscription | select SubscriptionName**.
	- Unique name for the Cloud Service container that will be used when creating the virtual machines using the setup script (does not need to be pre-created).
	- Unique name for the Cloud Service container that will be used when creating the virtual machines using the PowerShell demo (does not need to be pre-created).


1. If you have not used Microsoft Azure PowerShell before you need to download a publish settings file. To do this, run the following script. Sign in to the Microsoft Azure Management Portal, and then follow the instructions to download your Microsoft Azure publishing settings. Use your browser to save the file as a .publishsettings file to your local computer. Note the location of the file.

	````PowerShell
	Get-AzurePublishSettingsFile
	````

1. Then replace the placeholder with your publish-setting file’s path and execute this script.

	````PowerShell
	Import-AzurePublishSettingsFile '[YOUR-PUBLISH-SETTINGS-PATH]'   
	````

1. From an elevated prompt run the **Setup-1.Azure.cmd** script to copy the prepared VHDs to your storage account. 

1. From an elevated prompt run the **Setup-2.Azure.cmd** script to create the starter virtual machines for this demo. 

1. Login to each of the virtual machines and configure [CloudXPlorer](http://clumsyleaf.com/downloads/cloudxplorer.zip) to use the storage account specified during the **Setup-1.Azure.cmd** script. You will use this tool later to download the database backup file and the web application.

	> **Note:** CloudXPlorer is already installed in the IIS virtual machine.

	![Pre-provisioned Virtual Machines](Images/pre-provisioned-virtual-machines.png?raw=true "Pre-provisioned Virtual Machines")

	> **Note:** In the script examples do not leave the [ ] brackets when replacing the tokens with your own values.

<a name="Demo" />
## Demo ##

<a name="segment1" />
### Segment 1: Creating a Web Farm using PowerShell ###

1. Run **powershell_ise.exe** (open the Run dialog using WINDOWS+R) and paste in the following script. Replace the subscription placeholders.

	````PowerShell
	Import-Module 'C:\Program Files (x86)\Microsoft SDKs\Microsoft Azure\PowerShell\Azure\Azure.psd1' 
	Set-AzureSubscription [SUBSCRIPTION-NAME] -CurrentStorageAccount [STORAGE-ACCOUNT-NAME]
	Select-AzureSubscription [SUBSCRIPTION-NAME]
 	
	$iisimage = '' 
	$sqlimage = ''  	
	````

1. In the console window run the following PowerShell commands to retrieve available image names.

	````PowerShell
	Import-Module 'C:\Program Files (x86)\Microsoft SDKs\Microsoft Azure\PowerShell\Azure\Azure.psd1' 
	Get-AzureVMImage | ft imagename
	````
   	![powershell_ise_get_azurevmimage](Images/powershellisegetazurevmimage.png?raw=true)

1.  Update the variables **$iisimage** and **$sqlimage** to contain images for Windows Server and SQL Server using the list above. The following command is shown as an example. 

	````PowerShell
	$iisimage = 'a699494373c04fc0bc8f2bb1389d6106__Win2K8R2SP1-Datacenter-201301.01-en.us-30GB.vhd' 
	$sqlimage = 'fb83b3509582419d99629ce476bcb5c8__Microsoft-SQL-Server-2012SP1-Standard-CY13SU04-SQL11-SP1-CU3-11.0.3350.0-B' 
	````

1. Paste in the following PowerShell script to provision two front end web servers.

	````PowerShell
	$iisvm1 = New-AzureVMConfig -Name 'iisvm1' -InstanceSize Small -ImageName $iisimage |
		Add-AzureEndpoint -Name web -LocalPort 80 -PublicPort 80 -Protocol tcp `
    	-LBSetName web -ProbePath '/' -ProbeProtocol http -ProbePort 80 |
		Add-AzureProvisioningConfig -Windows -Password 'pass@word1' -AdminUserName 'iisadmin'
	
	$iisvm2 = New-AzureVMConfig -Name 'iisvm2' -InstanceSize Small -ImageName $iisimage  |
		Add-AzureEndpoint -Name web -LocalPort 80 -PublicPort 80 -Protocol tcp `
    	-LBSetName web -ProbePath '/' -ProbeProtocol http -ProbePort 80 |
		Add-AzureProvisioningConfig -Windows -Password 'pass@word1' -AdminUserName 'iisadmin'
	````

	> **Note:** Explain how the above script also configures port 80 to be load balanced and monitored for availability.

1. Paste in the following PowerShell script to provision a SQL Server with two data disks (one for data and the other for transaction logs).

	````PowerShell
	$sqlvm1 = New-AzureVMConfig -Name 'sqlvm1' -InstanceSize Medium -ImageName $sqlimage |
		Add-AzureDataDisk -CreateNew -DiskSizeInGB 100 -DiskLabel 'data' -LUN 0 |
		Add-AzureDataDisk -CreateNew -DiskSizeInGB 100 -DiskLabel 'logs' -LUN 1 |
		Add-AzureProvisioningConfig -Windows -Password 'pass@word1' -AdminUserName 'iisadmin'
	````
	
	> **Note:** Explain how the above script configures the additional data disks.

1. Paste in the following code that actually creates the cloud service and virtual machines. Replace the placeholder with a unique service name. Ensure that the -Location parameter matches the data center to where your storage account has been created in. 

	````PowerShell
	New-AzureVM -ServiceName '[SERVICE-NAME]' -VMs $iisvm1, $iisvm2, $sqlvm1 -Location 'West US'
	````

1. Press **F5** to run the script. This will provision the web farm you are about to show how to configure.

	> **Note:** You will not actually use the virtual machines just provisioned. Instead, you will use the virtual machines that were previously provisioned as part of the setup. 

<a name="segment2" />
### Segment 2: Virtual Machine Configuration - Configuring SQL Server ###

1. Next use the Microsoft Azure Portal to login via remote desktop to the **SQLVM1** virtual machine that was created by the setup script. Use _sqladmin_ as username and _pass@word1_ as the password.

1. Start **SQL Server Management Studio** and connect to the default server.

1. Right-click on the server instance and select **Properties**.

1. In **Security**, change the server authentication to **SQL Server and Windows Authentication mode** and click **OK**. 

	![sql-security](Images/sql-security.png?raw=true)

1. Restart SQL Server by right-clicking on the server name and selecting **Restart**.

1. Configure data and transaction disks by launching **Computer Management** (Start | Administrative Tools | Computer Management) and then selecting **Disk Management** under the _Storage_ node. 

1. When Disk Management opens, you will be prompted to initialize the disks. Click **OK**.

	![InitializeDisks](Images/initializedisks.png?raw=true)

1. Right-click on each disk and select **New Simple Volume**.

1. Accept the defaults for everything but volume label and drive letter. Set the volume label of the first disk to **Data** and use **F:** as the drive letter. Set the volume label of the second disk to **Logs** and use **G:** as the drive letter.

	![Disks Initialization](Images/disks-initialization.png?raw=true "Disks Initialization")

1. Once the disks are configured, create a folder in the **F:** drive called **Data** and a folder in the **G:** drive called **Logs**. 

1. Use [CloudXplorer](http://clumsyleaf.com/downloads/cloudxplorer.zip) to download the **AdventureWorks.bak** database backup file from the storage account you've configured for the lab (in **Config.Azure.xml**) to the **F:** drive. 

	> **Note:** Using storage tools such as CloudXplorer shows how to transfer migrated data to the cloud.

1. Switch back to **SQL Server Management Studio**. Right-click on **Databases** and click **Restore database**. 

	![RestoreDB](Images/restoredb.png?raw=true)

1. Select **Device** and browse to **F:\AdventureWorks.bak**.

	![RestoreDB2](Images/restoredb2.png?raw=true)

1. Click the **Files** menu item on the left to show that the restore is being split across the disks.

	![RestoreDB3](Images/restoredb3.png?raw=true)

1. Next step is to create a user account for the SQL Database. Under **Security**, right-click **Logins** and select **New Login**.

	![CreateLogin1](Images/createlogin1.png?raw=true)

1. Name the login **sqluser**, select **SQL Server Authentication** and set the password to _pass@word1_. Ensure to uncheck **User must change password at next login** and select **AdventureWorks2012** as the default database.

	![createlogin2](Images/createlogin2.png?raw=true)

1. Select **User Mapping** and check **AdventureWorks2012**. Click **OK**.

	![createlogin3](Images/createlogin3.png?raw=true)

1. Expand the **AdventureWorks2012** database and then **Security**. Inside **Users**, right-click **sqluser** and select **Properties**.

1. Select **Membership** and modify the database role membership by checking **db_owner**, **db_datareader** and **db_datawriter**. Click **OK**.

	![createlogin4](Images/createlogin4.png?raw=true)

1. To allow connectivity from the web servers, open **Windows Firewall with Advanced Security** under **Administrative tools**.

1. Add a new inbound rule of type **Port** and click **Next**.

	![fw_rule1](Images/fwrule1.png?raw=true)

1. Specify **1433** for the port. Click **Next**.
	
	![fw_rule2](Images/fwrule2.png?raw=true)

1. Accept the defaults for the remaining steps except for naming the rule. Set the name to **SQL** and click **Finish**.

At this point the SQL Server is ready to serve connections for the database. 

<a name="segment3" />
### Segment 3: Virtual Machine Configuration - Configuring the Web Farm ###

1. Open a remote desktop connection into **IISVM1** virtual machine. Use _iisadmin_ as username and _pass@word1_ as the password.

1. Use [CloudXplorer](http://clumsyleaf.com/downloads/cloudxplorer.zip) to download **AzureStore.zip** from the storage account you've configured for the lab (in **Config.Azure.xml**) to the file system.

1. Unzip the contents of the files into **C:\InetPub\wwwroot**.

1. Open **IIS Manager** and change the .NET Framework version of the _DefaultAppPool_ to **.NET Framework 4.0**.

	![defaultapppool](Images/defaultapppool.png?raw=true)

1. Repeat the same steps for the **IISVM2** virtual machine. 

1. Browse the site by getting the URL from the **DNS Name** in dashboard of any virtual machine in the cloud service to test the site's functionality.

	![Azure Store Application](Images/azure-store-application.png?raw=true "Azure Store Application")

