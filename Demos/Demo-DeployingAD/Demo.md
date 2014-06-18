<a name="title" />
# Deploying Active Directory #

---
<a name="Overview" />
## Overview ##

In this demonstration you will walk through the process of deploying a domain controller in Microsoft Azure. To deploy an Active Directory domain controller you must first configure a Microsoft Azure Virtual Network for IP persistence (non VNET deployments DHCP leases are short where a VNET deployment is perpetual). In many real world scenarios the VNET deployment would likely be connected with a site to site VPN tunnel. For this example you will only deploy a cloud only AD environment. 

<a name="technologies" />
### Key Technologies ###

- Microsoft Azure subscription - you can sign up for free trial [here][1]
- Microsoft Azure Virtual Machines 
- Microsoft Azure Virtual Networks
- [Microsoft Azure PowerShell Cmdlets][2]

[1]: http://bit.ly/WindowsAzureFreeTrial
[2]: http://go.microsoft.com/?linkid=9811175&clcid=0x409



<a name="setup" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment.

1. Download, install and configure the Microsoft Azure PowerShell cmdlets. Instructions on how to configure the cmdlets with your subscription can be found here: http://msdn.microsoft.com/en-us/library/windowsazure/jj554332.aspx

1. Download and install the latest node.js library from: http://nodejs.org 

1. Install the node.js command line tools for Microsoft Azure by running the following command at a command prompt:

	````PowerShell
	npm install azure-cli -g
	````

1. Modify the Config.Azure.xml located in **Source** folder for your Microsoft Azure Subscription. The following values are needed:
	- Target Storage Account Name, Container and Key where the demo VHDs will be copied to. 
	- The Storage Account should be in West US to allow the VHDs to copy in a timely manner.
	- Subscription Name - value can be retrieved from PowerShell by running **Get-AzureSubscription | select SubscriptionName**.
	- Unique name for the Cloud Service container that will be used when creating the domain controller in the PowerShell Demo (does not need to be pre-created).	
	- Unique name for the Cloud Service container that will be used when creating domain joined member servers using the PowerShell demo (does not need to be pre-created and must be different than the DC cloud service).

1. If you have not used Microsoft Azure PowerShell before you need to download a publish settings file. To do this, run the following script. Sign in to the Microsoft Azure Management Portal, and then follow the instructions to download your Microsoft Azure publish settings. Use your browser to save the file with extension _.publishsettings_ file to your local computer. Take a note of the location of the file.

	````PowerShell
	Get-AzurePublishSettingsFile
	````

1. Then replace the placeholder with your publish settings file’s path and execute this script.

	````PowerShell
	Import-AzurePublishSettingsFile '[YOUR-PUBLISH-SETTINGS-PATH]'   
	````

1. Run the **Setup-1.Azure.cmd** script located in **Source** folder as Administrator to copy the prepared VHDs to your storage account (the storage account must be in West US).

1.  Run the **Setup-2.Azure.cmd** script located in **Source** folder as Administrator to create the OS disk entry for the virtual machine you will use to create AD.

	> **Note:** In the script examples do not leave the [ ] brackets when replacing the tokens with your own values.


<a name="Demo" />
## Demo ##

1. Configure the demo virtual network where the demo virtual machines will be deployed to. 

1. Create a simple virtual network with an affinity group for it in the region where your cloud services will be hosted and specify the affinity group along with a single subnet network configuration.

	![simple-vnet](Images/simple-vnet.png?raw=true)

1. At the top right corner, select the **CIDR** option. Then set the Address Space Starting IP value to _10.1.0.0_ and its Address CIDR to _/16_. Finally, add a subnet named _AppSubnet_ with a Starting IP of _10.1.1.0_ and an Address CIDR of _/24_.

	![simple-vnet-2](Images/simple-vnet-2.png?raw=true)

1. Accept the defaults for the rest of the configuration.

	![simple-vnet-3](Images/simple-vnet-3.png?raw=true)

1. Run powershell_ise and and run the following script. Make sure to replace the placeholder with the path to **Config.Azure.xml** in **Source** folder. The script will provision a virtual machine from the disk that was copied over earlier. The disk already has the Active Directory role provisioned and a forest created.

	````PowerShell
	# Create VMs
	[xml]$xml = Get-Content '[AZURE-CONFIGURATION-FILE-PATH]'
	$addcvm = New-AzureVMConfig -Name 'ad-dc' -InstanceSize Small -DiskName 'ad-dcosdisk' |
          	Set-AzureOSDisk 'ReadOnly' |
          	Set-AzureSubnet 'AppSubnet' | 
  	  	Add-AzureEndpoint -Name RDP -LocalPort 3389 -Protocol tcp 
	
	New-AzureVM -ServiceName $xml.configuration.cloudServiceName1 -VMs $addcvm -AffinityGroup 'MyVNETAG' -VNETName 'SimpleVNET'
	
	````
	
	> **Speaking Point:** The script above specifies the subnet and virtual network for the virtual machine to boot up in. Since this VM will also be a domain controller you are setting the OS disk cache settings to be read only to reduce the risk of data corruption.

1. Once the virtual machine is booted RDP in using **administrator@fabrikam.com** as the login and **pass@word1** as the password.

1. From a command line run **ipconfig /all**.

1. Note the DHCP lease expiration to demonstrate that it is for all intents and purposes a static IP address. Also, save the IP address because you will now use it to configure a member server. Close the RDP.

1. In the Windows PowerShell ISE window run the following PowerShell command to retrieve available image names. You will need to insert one of these image names in the next script.

	````PowerShell
	Get-AzureVMImage | ft imagename
	````

1. Paste in the following script to create a member server joined VM. Update the $dcip variable to use the IP address of the domain controller, the $adminUserName variable with a user name of your choice, the Config.Azure.xml file path for the $xml variable and the $image variable with a Windows Server image name before running.

	````PowerShell
	$dcip = '[IP-ADDRESS-OF-DOMAIN-CONTROLLER]'
	[xml]$xml = Get-Content '[AZURE-CONFIGURATION-FILE-PATH]'
	$image = 'a699494373c04fc0bc8f2bb1389d6106__Windows-Server-2012-Datacenter-201302.01-en.us-30GB.vhd'

	$subnet = 'AppSubnet'

	$ou = 'OU=AzureVMs,DC=fabrikam,DC=com'

	$dom = 'fabrikam'
	$pass = 'pass@word1'
	$domjoin = 'fabrikam.com'
	$domuser = 'administrator'
	$adminUserName = '[YOUR-USER-NAME]'

	$domVM = New-AzureVMConfig -Name 'ad-ms1' -InstanceSize Small -ImageName $image |      
		
			Add-AzureProvisioningConfig -WindowsDomain -JoinDomain $domjoin -Domain $dom -DomainPassword $pass -AdminUserName $adminUserName -Password $pass -DomainUserName $domuser -MachineObjectOU $ou |
						 Set-AzureSubnet -SubnetNames $subnet



	$dns = New-AzureDns -Name 'ad-dc' -IPAddress $dcip
		  
	New-AzureVM -ServiceName $xml.configuration.cloudServiceName2 -AffinityGroup 'MyVNETAG' -VNetName 'SimpleVNET' -DnsSettings $dns -VMs $domVM

	````

	> **Speaking Point:** The PowerShell script above creates a new Windows Server that is domain joined at boot time. The computer account will be created in the AzureVMs Organizational Unit. By specifying the IP address of the domain controller in the DNS Settings the member server will automatically be configured to communicate to AD. 

