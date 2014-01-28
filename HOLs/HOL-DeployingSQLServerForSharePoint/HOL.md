<a name="Title" />
# Configuring SQL Server 2012 for SharePoint in Windows Azure #

---
<a name="Overview" />
## Overview ##

In this hands-on lab you will learn how to create and configure a virtual machine running SQL Server 2012 as part of the deploying a SharePoint farm hands-on lab.

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

1. Create a virtual machine using the Windows Azure portal that is hosted within the same cloud service as another virtual machine.
1. Configure a SQL Server 2012

<a name="Setup" />
### Setup ###

The Windows Azure PowerShell Cmdlets are required for this lab. If you have not configured them yet, see the **Automating VM Management** hands-on lab in the **Automating Windows Azure with PowerShell** module. 

>**Note:** In order to run through the complete hands-on lab, you must have network connectivity. 

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- Complete the Deploying Active Directory hands-on lab
- [Windows Azure PowerShell CmdLets](http://msdn.microsoft.com/en-us/library/windowsazure/jj156055)
- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)


<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating and configuring Windows Server Virtual Machine with SQL Server 2012 using the Windows Azure portal](#Exercise1)
 
Estimated time to complete this lab: **30 minutes**.

---
<a name='gettingstarted' />
### Getting Started: Obtaining Subscription's Credentials ###

In order to complete this lab, you will need your subscription’s secure credentials. Windows Azure lets you download a Publish Settings file with all the information required to manage your account in your development environment.

<a name='gettingstartedTask1' />
#### Task 1 - Downloading and Importing a Publish-settings File ####

> **Note:** If you have done these steps in a previous lab on the same computer you can move on to Exercise 1.

In this task, you will log on to the Windows Azure Portal and download the publish-settings file. This file contains the secure credentials and additional information about your Windows Azure Subscription to use in your development environment. Then, you will import this file using the Windows Azure Cmdlets in order to install the certificate and obtain the account information.

1.	Open an Internet Explorer browser and go to <https://windows.azure.com/download/publishprofile.aspx>.

1.	Sign in using the credentials associated with your Windows Azure account.

1.	**Save** the publish-settings file to your local machine.

	![Downloading publish-settings file](Images/downloading-publish-settings-file.png?raw=true 'Downloading publish-settings file')

	_Downloading publish-settings file_

	> **Note:** The download page shows you how to import the publish-settings file using Visual Studio Publish box. This lab will show you how to import it using the Windows Azure PowerShell Cmdlets instead.

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

	
1.	The following script imports your publish-settings file and generates an XML file with your account information. You will use these values during the lab to manage your Windows Azure Subscription. Replace the placeholder with your publish-setting file’s path and execute the script.

	````PowerShell
	Import-AzurePublishSettingsFile '[YOUR-PUBLISH-SETTINGS-PATH]'   
	````

1. Execute the following commands and take note of the subscription name and a storage account name you will use for the exercise. Also make note of the location of the storage account.

	````PowerShell
	Get-AzureSubscription | select SubscriptionName
	Get-AzureStorageAccount | select StorageAccountName, Location 
	````

	> **Note:** If you do **not** have a storage account already created you can use for this exercise you should create one first by following these steps.  

	> 1. Run the following to determine the data center to create your storage account in. Ensure you pick a data center that shows support for **PersistentVMRole**. 
	>
	>	````PowerShell
	>	Get-AzureLocation  
	>	````
	>
	> 2. Create your storage account.
	>
	>	````PowerShell
	>	New-AzureStorageAccount -StorageAccountName '[YOUR-STORAGE-ACCOUNT]' -Location '[DC-LOCATION]'
	>	````

1. Execute the following command to set your current storage account for your subscription.


	````PowerShell
	Set-AzureSubscription -SubscriptionName '[YOUR-SUBSCRIPTION-NAME]' -CurrentStorageAccount '[YOUR-STORAGE-ACCOUNT]'
	````

---
<a name="Exercise1" />
### Exercise 1: Creating and configuring Windows Server Virtual Machine with SQL Server 2012 ###

You will now create the Windows Server virtual machine and configure SQL Server. You will automatically provision a new virtual machine that is joined to the Active Directory domain at boot.

1. If you do not have the IP address of the Domain Controller virtual machine, navigate to http://manage.windowsazure.com/ using a Web browser and sign in using the **Microsoft Account** associated with your Windows Azure account.

1. Go to **Virtual Machines**, select the virtual machine where you deployed the AD and select the **Connect** button at the bottom panel.

1. In the virtual machine, go to **Start**, type **cmd** and press **ENTER**.

1. Type **ipconfig** and press **ENTER**. Take note of the **IPv4 address**, you will use it later on this exercise. Close the **Remote Desktop** connection.

	![IP Address](Images/ip-address.png?raw=true "IP Address")

	_IP Address_

1. Open **Windows Azure PowerShell**. Start typing **Windows Azure PowerShell** in the **Start** screen, right-click **Windows Azure Powershell** and choose **Run as Administrator**.

1. Execute the following command to obtain the names of the available OS Disk images. Take note of the **SQL Server 2012 Standard Edition** image disk name. This image is a Windows Server 2008 R2 that has a SQL Server 2012 Standard Edition already installed.

	<!-- mark:1 -->
	````PowerShell
	Get-AzureVMImage | Select ImageName
	````

1. Execute the following command to define the OS disk image name for the new Virtual Machine.
	
	<!-- mark:1 -->
	````PowerShell
	$imgName = '[OS-IMAGE-NAME]'
	````

1. Set up the Virtual Machine's DNS settings. To do this, you will use the virtual machine you created in **Deploying Active Directory in Windows Azure** hands-on lab, were you configured the Active Directory. Replace the placeholders before executing the following command. Use the IP address you copied at the beginning of the exercise.
	
	<!-- mark:1-4 -->
	````PowerShell
	$advmIP = '[AD-IP-ADDRESS]'
	$advmName = '[AD-VM-NAME]'
	# Point to IP Address of Domain Controller Created Earlier
	$dns1 = New-AzureDns -Name $advmName -IPAddress $advmIP
	````


1. Set up the new Virtual Machine's configuration settings to automatically join the domain in the provisioning process. Before executing the command, replace the placeholders with the administrator and domain credentials.

	<!-- mark:1-12 -->
	````PowerShell
	$vmName = 'SqlServer2012VM'
	$adminUsername = '[YOUR-ADMIN-USERNAME]'
	$adminPassword = '[YOUR-PASSWORD]'
	$domainPassword = '[YOUR-PASSWORD]'
	$domainUser = 'administrator'
	$FQDomainName = 'contoso.com'
	$subNet = 'AppSubnet'
	# Configuring VM to Automatically Join Domain
	$advm1 = New-AzureVMConfig -Name $vmName -InstanceSize Small -ImageName $imgName | 
				Add-AzureProvisioningConfig -WindowsDomain -AdminUserName $adminUsername -Password $adminPassword `
				-Domain 'contoso' -DomainPassword $domainPassword `
				-DomainUserName $domainUser -JoinDomain $FQDomainName |
		 Set-AzureSubnet -SubnetNames $subNet
	````

	>**Note:** The previous command asumes that you used the proposed names for the Domain Name and the Subnets that are shown in the **Deploying Active Directory** hands on lab. You may need to update the values if you used different names.

1. Create a new Virtual Machine using the Domain and DNS settings you defined in the previous steps. Replace the placeholder with a unique Service Name.

	````PowerShell
	$serviceName = '[YOUR-SERVICE-NAME]'
	$affinityGroup = 'adag'
	$adVNET = 'ADVNET'
	# New Cloud Service with VNET and DNS settings
	New-AzureVM –ServiceName $serviceName -AffinityGroup $affinityGroup -VMs $advm1 -DnsSettings $dns1 -VNetName $adVNET
	````

	>**Note:** The previous command asumes that you used the proposed names for the Domain Name and the Subnets that are shown in the **Deploying Active Directory** hands on lab. You may need to update the values if you used different names.

1. Once the provisioning proces finish, connect to the VM using Remote Desktop and verify if it was automatically joined to your existing domain.

<a name="Ex1Task2" />
#### Task 2 - Configuring Disks for SQL Server ####

1. In the **Windows Azure Portal**, select the _SQLServer2012VM_ virtual machine you created in Task 1, and click **Attach**.

     ![attachemptydisk](Images/attachemptydisk.png?raw=true)

	_Attaching an empty disk_

1. Select **Attach empty disk** and select 50 GB 

1. Wait until the disk has been provisioned and repeat.

1. You should now have 2 50 GB data disks attached to this virtual machine. 

1. Click connect and login to the virtual machine using RDP. 

1. Once logged in, open **Computer Management** from | **Start** | **Administrative Tools** and under **Storage** click **Disk Management**.

1. The **Initialize Disk** dialog will be shown. Leave the default options and click **OK**.

    ![initializedisk](Images/initializedisk.png?raw=true)

	_Initialize Disk_

1. Once the disks are initialized you will then need to right click on the unallocated disks and select **New Simple Volume** (software RAID is also support so those are options are available as well). Follow the wizard using the default options. Do this for each disk. The **New Simple Volume** dialog will allow you to format the disks and mount them for use.
    ![initializeddisks](Images/initializeddisks.png?raw=true)

	_Disk management_

1. You will now configure database default location. To do this, launch SQL Server Management Studio from **Start | All Programs | Microsoft SQL Server 2012 | SQL Server Management Studio**.

1. Connect to the Server using the default information.

1. Right click on the server name, click **Properties** and click **Database Settings**.

1. Specify the new data disks for the default data, logs and backup folders and click **OK** to close.

    ![dbsettings](Images/dbsettings.png?raw=true)

	_Database default locations_


1. Using Windows Explorer create the following folders: **F:\Data, G:\Logs** and **G:\Backup**.

1. Restart the service to apply the changes, by right-clicking the service name and selecting **Restart**.

<a name="Ex1Task3" />
#### Task 3 - Updating SQL Server Network Configuration ####

1. In the **Windows Azure Portal**, select the _SQLServer2012VM_ virtual machine you created in Task 1, and click **Connect**, if you are not already logged into this machine.

1. Open the remote desktop file and log on using the Administrator credentials you defined when creating the virtual machine.

1. Open **SQL Server Configuration Manager** from **Start | All Programs | Microsoft SQL Server 2012 | Configuration Tools**.
1. Expand the **SQL Server Network Configuration** node and select **Protocols for MSSQLServer** (this option might change if you used a different instance name when installing SQL Server). Make sure **Shared Memory**, **Named Pipes** and **TCP/IP** protocols are enabled. To enable a protocol, right-click the Protocol Name and select **Enable**.

	![Enabling SQL Server Protocols](Images/enabling-sql-server-protocols.png?raw=true "Enabling SQL Server Protocols")

1. Close the **SQL Server Configuration Manager**.

<a name="Ex1Task4" />
#### Task 4 - Adding an inbound rule in Windows Firewall ####

1. In order to allow SharePoint access the SQL Server database you will need to add an **Inbound Rule** for the SQL Server requests in the **Windows Firewall**. To do this, open **Windows Firewall with Advance Security** from **Start | All Programs | Administrative Tools**.

1. Select **Inbound Rules** node, right-click it and select **New Rule**.

 	![Creating an Inbound Rule](./Images/Creating-an-Inbound-Rule.png?raw=true "Creating an Inbound Rule")
 
	_Creating an Inbound Rule_

1. In the **New Inbound Rule Wizard**, select _Port_ as **Rule Type** and click **Next**.

	![New Inbound Rule Type](Images/new-inbound-rule-type.png?raw=true "Inbound Rule Type")
 
	_Inbound Rule's Type_

1. In **Protocols and Ports** step, select **Specific local ports** and set its value to _1433_. Click **Next** to continue.

	![Inbound Rule's Local Port](Images/inbound-rules-local-port.png?raw=true "Inbound Rule's Local Port")
 
	_Inbound Rule's Local Port_

1. In the **Action** step, make sure **Allow the connection** option is selected and click **Next**.

	![Inbound Rule's Action](Images/inbound-rules-action.png?raw=true "Inbound Rule's Action")
 
	_Inbound Rule's Action_

1. In the **Profile** step, leave the default values and click **Next**.

1. Finally, set the Inbound Rule's **Name** to _SQLServerRule_ and click **Finish**.

 	![New Inbound Rule](Images/new-inbound-rule.png?raw=true "New Inbound Rule")
 
	_New Inbound Rule_

1. Close **Windows Firewall with Advanced Security** 

<a name="Ex1Task5" />
#### Task 5 - Joining the Active directory Domain ####

1. In the remote desktop connection, from **Start**, right-click **Computer** and select **Properties**. 

1. Click **Advanced System Settings** and switch to **Computer Name** tab. Then click **Change**.

1. Select **Domain**, and enter _contoso.com_, if it is not already there. If you changed it, when prompted use contoso\administrator and allow the reboot.

	1. Once rebooted login via remote desktop again 

	> **Note:** You will receive the following error which you can safely ignore.

	![domjoinerror](Images/domjoinerror.png?raw=true)

1. Start Microsoft SQL Server Management Studio

1. Right click on **Logins** folder under **Security** and click **New login**

1. Complete the **Login name** with _contoso\administrator_  and in **Server Role** tab specify the _sysadmin_ server role.

1. Click **OK** to close the dialog box.

<a name="summary" />
## Summary ##

In this lab you learned how to create and configure a SQL Server 2012 Database by provisioning a Virtual Machine in the Windows Azure portal and then applying the configuration in SQL Server. You also learned how to join a virtual machine to an existing cloud service and join a domain with that virtual machine.
