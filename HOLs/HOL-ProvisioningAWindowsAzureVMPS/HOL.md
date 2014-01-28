<a name="Title" />
# Provisioning a Windows Azure Virtual Machine (PowerShell) #

---

<a name="Overview" />
## Overview ##

In this lab, you will create a new virtual network and then a new Windows Server 2012 virtual machine from a gallery image using the Windows Azure PowerShell Cmdlets.  The virtual network creation is not necessary to create a new virtual machine from a gallery image, but is necessary to control the IP addresses assigned to the virtual machines or enable VPN connectivity back to a corporate on-premise network.

>**Note:** If you are following this HOL for a second time, you can skip Exercise 1 and move to Exercise 2 to create an additional virtual machine on the existing virtual network you already created.


<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

- Configure a new virtual network
- Deploy a new virtual machine from a gallery image

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Windows PowerShell 3.0] (http://microsoft.com/powershell/)
- [Windows Azure PowerShell Cmdlets](http://msdn.microsoft.com/en-us/library/windowsazure/jj156055)
- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

>**Note:** In order to run through the complete hands-on lab, you must have network connectivity.

<a name="Setup" />
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open Windows Explorer and browse to the lab's **Source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process that will configure your environment.

1. If the User Account Control dialog is shown, confirm the action to proceed.

> **Note:** Make sure you have checked all the dependencies for this lab before running the setup.

---

<a name="gettingstarted" /></a>
### Getting Started: Obtaining Subscription's Credentials ###

In order to complete this lab, you will need your subscription’s secure credentials. Windows Azure lets you download a Publish Settings file with all the information required to manage your account in your development environment.

<a name="GSTask1" /></a>
#### Task 1 - Downloading and Importing a Publish Settings file ####

> **Note:** If you have done these steps in a previous lab on the same computer you can move on to Exercise 1.

In this task, you will log on to the Windows Azure Portal and download the Publish Settings file. This file contains the secure credentials and additional information about your Windows Azure Subscription that you will use in your development environment. Therefore, you will import this file using the Windows Azure Cmdlets in order to install the certificate and obtain the account information.

1.	Open Internet Explorer and browse to <https://windows.azure.com/download/publishprofile.aspx>.

1.	Sign in using the **Microsoft Account** associated with your **Windows Azure** account.

1.	**Save** the Publish Settings file to your local file system.

	![Downloading publish-settings file](Images/downloading-publish-settings-file.png?raw=true 'Downloading publish-settings file')

	_Downloading Publish Settings file_

	> **Note:** The download page shows you how to import the Publish Settings file using the Visual Studio Publish box. This lab will show you how to import it using the Windows Azure PowerShell Cmdlets instead.

1. Search for **Windows Azure PowerShell** in the Start screen and choose **Run as Administrator**.

1.	Change the PowerShell execution policy to **RemoteSigned**. When asked to confirm press **Y** and then **Enter**.
	
	<!-- mark:1 -->
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


1.	The following script imports your Publish Settings file and generates an XML file with your account information. You will use these values during the lab to manage your Windows Azure Subscription. Replace the placeholder with the path to your Publish Setting file and execute the script.

	<!-- mark:1 -->
	````PowerShell
	Import-AzurePublishSettingsFile '[YOUR-PUBLISH-SETTINGS-PATH]'   
	````

1. Execute the following commands and take note of the Subscription name and the storage account name you will use for the exercise.

	<!-- mark:1-3 -->
	````PowerShell
	Get-AzureSubscription | select SubscriptionName
	Get-AzureStorageAccount | select StorageAccountName 
	````

1. If the preceding command do NOT return a storage account, you should create one first.
  
	1. Run the following command to determine the data center to create your storage account in. Ensure you pick a data center that shows support for PersistentVMRole. 

		````PowerShell
		Get-AzureLocation  
		````

	1. Create your storage account: 


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

	<!-- mark:1 -->
	````PowerShell
	Set-AzureSubscription -SubscriptionName '[YOUR-SUBSCRIPTION-NAME]' -CurrentStorageAccount '[YOUR-STORAGE-ACCOUNT]'
	````

<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating a new Virtual Network](#Exercise1)
1. [Creating a new virtual machine from the gallery image](#Exercise2)

Estimated time to complete this lab: **30 minutes**.

<a name="Exercise1" /></a>
### Exercise 1: Creating a new Virtual Network ###

The workload on many virtual machines requires persistent IP addresses and self-provided DNS name resolution. The default internal DNS service (iDNS) in Windows Azure is often not an acceptable solution because the IP address assigned to each virtual machine is not persistent. For this solution you will define a virtual network where you can assign the virtual machines to specific subnets.

The network configuration used for this lab defines the following:

- A Virtual Network Named **domainvnet** with an address prefix of: 10.0.0.0/16
- A subnet named **Subnet-1** with an address prefix of: 10.0.0.0/24

Exercise 1 contains 2 tasks:

1. Creating an Affinity Group 
2. Creating a new Virtual Network

<a name="Ex1Task1" /></a>
#### Task 1 - Creating an Affinity Group ####

The first task is to create an affinity group for the Virtual Network. 

1. On the Start menu, start typing **powershell ise**, and then click **Windows PowerShell ISE**. For this task and most of this HOL, we will use the PowerShell Integrated Scripting Environment.

	![Opening Windows Powershell ISE](./Images/opening-windows-powershell-ise.png?raw=true "Opening Windows Powershell ISE")

	_Opening Windows Powershell ISE_

1. In the PowerShell ISE window, type the following command:
	
	````PowerShell
	# Creates the affinity group
	New-AzureAffinityGroup -Location "[DC-LOCATION]" -Name agdomain
	```

	> **Note:** For the _[DC-LOCATION]_ placeholder above, please replace it with the exact text below (minus the number) from the datacenter closest to you:

	> 1. West US
	> 2. East US
	> 3. East Asia
	> 4. Southeast Asia
	> 5. North Europe
	> 6. West Europe

<a name="Ex1Task2" /></a>
#### Task 2 - Creating a new Virtual Network ####

The next step is to create a new virtual network to your subscription.

1. First create an XML file called **domainvnet.xml** on your local host where you are running the PowerShell ISE with the following contents:

	````XML
	<?xml version="1.0" encoding="utf-8"?>
	<NetworkConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.microsoft.com/ServiceHosting/2011/07/NetworkConfiguration">
	  <VirtualNetworkConfiguration>
	    <Dns>
	      <DnsServers>
	        <DnsServer name="DC01" IPAddress="10.0.0.4" />
	      </DnsServers>
	    </Dns>
	    <VirtualNetworkSites>
	      <VirtualNetworkSite name="domainvnet" AffinityGroup="agdomain">
	        <AddressSpace>
	          <AddressPrefix>10.0.0.0/16</AddressPrefix>
	        </AddressSpace>
	        <Subnets>
	          <Subnet name="Subnet-1">
	            <AddressPrefix>10.0.0.0/24</AddressPrefix>
	          </Subnet>
	        </Subnets>
	        <DnsServersRef>
	          <DnsServerRef name="DC01" />
	        </DnsServersRef>
	      </VirtualNetworkSite>
	    </VirtualNetworkSites>
	  </VirtualNetworkConfiguration>
	</NetworkConfiguration>
	```

	>**Note:** This XML file will create a virtual network named domainvnet within the affinity group you created earlier. It will add the Subnet-1 subnet to this virtual network and will create the DC01 DNS server.

	> The preceding xml configuration will only work if there are no other networks. If you have other networks defined, get their configuration and add it to the xml file before executing the following step. To get the current virtual network configuration you can use the following command: _(Get-AzureVNetConfig).XMLConfiguration_. 

1. In the PowerShell ISE window, type the following command to create a new virtual network using the configuration file created previously:

	````PowerShell
	# Creates the virtual network from XML file
	Set-AzureVNetConfig -ConfigurationPath [YOUR-PATH-TO-FILE]
	```

	> **Note:** Replace _[YOUR-PATH-TO-FILE]_ placeholder with the full path to the domainvnet.xml file you created in the previous step.

1. Open a browser and go to [https://manage.windowsazure.com/](https://manage.windowsazure.com/). When prompted, login with your **Windows Azure** credentials. In the Windows Azure portal, click **Networks**, and then click **domainvnet**. In the **Dashboard** tab you can see the virtual network that has been added and uses the affinity group you created earlier.

	![Verify The Virtual Network Creation](./Images/verify-the-virtual-network-creation.png?raw=true "Verify The Virtual Network Creation")

	_Verify The Virtual Network Creation_

<a name="Exercise2" /></a>
### Exercise 2: Creating a new virtual machine from the gallery image ###

You will now create a new virtual machine from a Windows Server 2012 gallery image called DC01.  If you are doing this exercise for a second time, you should name the virtual machine something other than DC01 and also ensure you connect to the existing virtual machine network when you create the virtual machine. The DC01 virtual machine will be used in various exercises.

Exercise 2 contains 1 task:

1. Creating a new virtual machine

<a name="Ex2Task1" /></a>
#### Task 1 - Creating a new Virtual Machine ####

1. In the PowerShell ISE window, type the following command:

	````PowerShell
	# Get a list of available gallery images
	Get-AzureVMImage  |  ft ImageName
	````

	![Retrieving the VM images](./Images/retrieving-the-vm-images.png?raw=true "Retriving the VM images")

	_Retrieving the virtual machine images_

	> **Note:** _The list of available image files is displayed. In this task, you will use a command to retrieve the name of the latest Windows Server 2012 image available._

1. In the PowerShell ISE window, type or copy the following commands to create a new virtual machine with the specified settings, using the virtual network created previously.

	> **Note:** Make sure to replace [YOUR-SERVICE-NAME] in the $svcname variable below with a new unique name to create a new hosted service. Additionally, replace the [YOUR-ADMIN-USERNAME] and  [YOUR-ADMIN-PASSWORD] placeholders with credentials of your choice.

	````PowerShell
	# Defines image name
	$imgname = @(Get-AzureVMImage | Where {$_.ImageName -match "106__Windows-Server-2012"})[-1].ImageName

	# Defines configuration settings
	$vmname = "DC01"
	$svcname = "[YOUR-SERVICE-NAME]"
	$adminUsername = "[YOUR-ADMIN-USERNAME]"
	$password = "[YOUR-ADMIN-PASSWORD]"

	# Defines network settings
	$subnetname = "Subnet-1"

	# Defines VM configuration, including size and subnet 
	$dcvm = New-AzureVMConfig  -Name  $vmname  -ImageName  $imgname -InstanceSize "Small"  |
		Add-AzureProvisioningConfig  -Windows -AdminUsername $adminUsername -Password  $password  |
		Set-AzureSubnet  -SubnetName  $subnetname

	# Creates new VM in the hosted service
	New-AzureVM  -ServiceName  $svcname  -AffinityGroup 'agdomain' -VNetName 'domainvnet' -VMs  $dcvm
	````

	![Executing the powershell commands](./Images/executing-the-powershell-commands.png?raw=true "Executing the powershell commands")

	_Executing the powershell commands_

	> **Note:** The **New-AzureVM** cmdlet will create a new hosted service when the -Location or -AffinityGroup parameter is specified, or will use an existing hosted service when those parameters are not specified. 

1. In the Windows Azure console, in the **Virtual Machines** section, wait a few minutes until the **DC01** virtual machine appears (or press **F5** to refresh the display).

1. Click the **DC01** name.

1. On the DC01 page, select the **Endpoints** tab to check the RDP endpoint port.

	>**Note**: Notice that by default the PowerShell cmdlet will add an RDP endpoint for port 3389. Therefore, we did not specify this endpoint configuration in the PowerShell commands.

	![Verifying RDP endpoint](./Images/rdp-endpoint-verification.png?raw=true "Executing the powershell commands")

	_Verifying RDP endpoint_

---

<a name="summary" />
## Summary ##
By completing this hands-on lab you have learned how to:

 * Configure a new virtual network
 * Deploy a new virtual machine from a gallery image

---
