<a name="Title" />
# Deploying a SharePoint Farm with Microsoft Azure Virtual Machines #

---

<a name="Overview" />
## Overview ##

In this hands-on lab you will learn how to create a SharePoint farm, connected with Active Directory and SQL Server.	

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

1. Use the Microsoft Azure Portal to create a Sharepoint Image.
1. Connect two virtual machines to the same cloud service for network connectivity.
1. Create and Configure SharePoint Server Farm.

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- Complete the _Deploying Active Directory_ HOL
- Complete the _Deploying SQL Server for SharePoint_ HOL
- [Microsoft Azure PowerShell CmdLets](http://msdn.microsoft.com/en-us/library/windowsazure/jj156055)
- A Microsoft Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

<a name='gettingstarted' /></a>
### Getting Started: Obtaining Subscription's Credentials ###

In order to complete this lab, you will need your subscription’s secure credentials. Microsoft Azure lets you download a Publish Settings file with all the information required to manage your account in your development environment.

<a name='GSTask1' /></a>
#### Task 1 - Downloading and Importing a Publish-settings File ####

> **Note:** If you have done these steps in a previous lab on the same computer you can move on to Exercise 1.


In this task, you will log on to the Microsoft Azure Portal and download the publish-settings file. This file contains the secure credentials and additional information about your Microsoft Azure Subscription that you will use in your development environment. Therefore, you will import this file using the Microsoft Azure Cmdlets in order to install the certificate and obtain the account information.

1.	Open Internet Explorer and browse to <https://windows.azure.com/download/publishprofile.aspx>.

1.	Sign in using the **Microsoft Account** associated with your **Microsoft Azure** account.

1.	**Save** the publish-settings file to your local file system.

	![Downloading publish-settings file](Images/downloading-publish-settings-file.png?raw=true 'Downloading publish-settings file')

	_Downloading publish-settings file_

	> **Note:** The download page shows you how to import the publish-settings file using the Visual Studio Publish box. This lab will show you how to import it using the Microsoft Azure PowerShell Cmdlets instead.

1. Search for **Microsoft Azure PowerShell** in the Start screen and choose **Run as Administrator**.

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


1.	The following script imports your publish-settings file and generates an XML file with your account information. You will use these values during the lab to manage your Microsoft Azure Subscription. Replace the placeholder with the path to your publish-setting file and execute the script.

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
		New-AzureStorageAccount -StorageAccountName '[YOUR-SUBSCRIPTION-NAME]' -Location '[DC-LOCATION]'
		````

1. Execute the following command to set your current storage account for your subscription.

	<!-- mark:1 -->
	````PowerShell
	Set-AzureSubscription -SubscriptionName '[YOUR-SUBSCRIPTION-NAME]' -CurrentStorageAccount '[YOUR-STORAGE-ACCOUNT]'
	````

---
 
<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating SharePoint Image](#Exercise1)
1. [Configuring a SharePoint Virtual Machine](#Exercise2)
1. [Configuring Load Balancing](#Exercise3)
 
Estimated time to complete this lab: **50 minutes**.
	
---

<a name="Exercise1" />
### Exercise 1: Creating SharePoint Image ###

You will now create the SharePoint Server disk image required to run this hands-on lab.

Make sure you have this image created before starting with the lab.	

<a name="Ex1Task1" />
#### Task 1 - Create a SharePoint virtual machine from an image using PowerShell####

In this task, you will create a SharePoint virtual machine from an image using PowerShell and we will join it to the domain we created in Deploying Active Directory hands-on lab. You will later use this virtual machine to configure the SharePoint Farm.

1. Navigate to the [Microsoft Azure Management Portal](https://manage.windowsazure.com) using a web browser, and sign in using your Microsoft account.

1. If you do not have the IP address of the Domain Controller Virtual Machine, Navigate to the **Microsoft Azure Portal** using a Web browser and sign in using the **Microsoft Account** associated with your Microsoft Azure account.

1. Go to **Virtual Machines**, select the virtual machine where you deployed the active directory and select the **Connect** button at the bottom panel.

1. In the virtual machine, go to **Start**, type **cmd** and press **ENTER**.

1. Type **ipconfig** and press **ENTER**. Take note of the **IPv4 address**, you will use it later on this exercise. 

1. Close the **Remote Desktop** connection.

1. Click **NETWORKS** in the left pane. Select the desired **Virtual Network** and copy its Affinity Group name. You will use this name later, to create the new Virtual Machine.

	![Virtual Networks](Images/virtual-networks.png?raw=true "Virtual Networks")
	
	_Virtual Networks_	

1. Open **Microsoft Azure PowerShell** from **Start** | **All Programs** | **Microsoft Azure** | **Microsoft Azure PowerShell**, right-click **Microsoft Azure Powershell** and choose **Run as Administrator**.

1. Execute the following command to obtain the names of the available OS Disk images. Take note of the **SharePoint** image disk name you created in the **Getting Started** section of this lab.

	<!-- mark:1 -->
	````PowerShell
	Get-AzureVMImage | Select ImageName
	````

	![Image Name list](Images/image-name-list.png?raw=true "Image Name list")

	_Image Name list_

1. Copy the SharePoint Image name and execute the following command to define the Operating System disk image name for the new Virtual Machine. In this case we are going to use _c6e0f177abd8496e934234bd27f46c5d__SharePoint-2013-Trial-4-13-2013_ but it may defer when running the Lab.
	
	````PowerShell
	$imgName = 'c6e0f177abd8496e934234bd27f46c5d__SharePoint-2013-Trial-4-13-2013'
	````

1. Set up the virtual machine's DNS settings. To do this, you will use the Virtual Machine you created in _Deploying Active Directory_ HOL. Replace the placeholders before executing the following command. Use the IP address you took note at the beginning of the exercise.
	
	````PowerShell
	$advmIP = '[AD-IP-ADDRESS]'
	$advmName = '[AD-VM-NAME]'
	# Point to IP Address of Domain Controller Created Earlier
	$dns1 = New-AzureDns -Name $advmName -IPAddress $advmIP
	````

1. Set up the two virtual machine's configuration settings to automatically join the domain in the provisioning process. Before executing the command, replace the placeholders with the administrator and domain passwords.

	````PowerShell
	$vmName1 = 'spvm1'
	$vmName2 = 'spvm2'
	$adminUserName = '[ADMIN-USER-NAME]'
	$adminPassword = '[YOUR-PASSWORD]'
	$domainPassword = '[YOUR-PASSWORD]'
	$domainUser = 'administrator'
	$FQDomainName = 'contoso.com'
	$subNet = 'Subnet-1'
	# Configuring VM to Automatically Join Domain
	$spvm1 = New-AzureVMConfig -Name $vmName1 -InstanceSize Small -ImageName $imgName | 
				Add-AzureProvisioningConfig -WindowsDomain -AdminUserName $adminUserName -Password $adminPassword `
				-Domain 'contoso' -DomainPassword $domainPassword `
				-DomainUserName $domainUser -JoinDomain $FQDomainName |
		 Set-AzureSubnet -SubnetNames $subNet
	$spvm2 = New-AzureVMConfig -Name $vmName2 -InstanceSize Small -ImageName $imgName | 
				Add-AzureProvisioningConfig -WindowsDomain -AdminUserName $adminUserName -Password $adminPassword `
				-Domain 'contoso' -DomainPassword $domainPassword `
				-DomainUserName $domainUser -JoinDomain $FQDomainName |
		 Set-AzureSubnet -SubnetNames $subNet
	````

	>**Note:** The previous command asumes that you used the proposed names for the Domain Name and the Subnets that are shown in the **Deploying Active Directory** hands on lab. You may need to update the values if you used different names.

1. Create two Virtual Machine using the Domain and DNS settings you defined in the previous steps. Replace the placeholder with a unique Service Name.

	````PowerShell
	$serviceName = '[YOUR-SERVICE-NAME]'
	$affinityGroup = 'adag'
	$adVNET = 'domainvnet'
	# New Azure VM with VNET and DNS settings
	New-AzureVM –ServiceName $serviceName -AffinityGroup $affinityGroup `
									-VMs $spvm1, $spvm2 -DnsSettings $dns1 -VNetName $adVNET
	````

	>**Note**: Make sure the location specified matches the location of the storage account you've configured in the Getting Started section. Also make sure that the service name is available to create the dns of the virtual machine.

1. Once the provisioning process finish, connect to the virtual machine using Remote Desktop and verify if it was automatically joined to your existing domain. To do so, open server manager and verify that the machine is joined to the domain.

	![Virtual machine joined to the domain](Images/vm-joined-to-the-domain.png?raw=true "Virtual machine joined to the domain")

	_Virtual machine joined to the domain_

---

<a name="Exercise2" />
### Exercise 2: Configuring a SharePoint Virtual Machine ###

<a name="Ex2Task1" />
#### Task 1 - Creating a new Farm in SharePoint####

In this task, you will configure the SharePoint virtual machine to create and a SharePoint Farm. 

1. If not already opened, navigate to the [Microsoft Azure Management Portal](https://manage.windowsazure.com)  using a web browser, and sign in using your Windows account.

1. In the **Virtual Machines** section, select the first SharePoint Virtual Machine ( _spvm1_ ) and click **Connect** to connect using **Remote Desktop**.

1. Open the **SharePoint 2013 Products Configuration Wizard**.

1. In the **Welcome to SharePoint Products** screen click next.

	>**Note**: If prompt that some services might restart during installation, click **Yes**. This second virtual machine will be used to create the SharePoint 

1. In the **Connect to a server farm** page, select **Create a new server farm** option.
	 
	![Create a new server farm](Images/create-a-new-server-farm.png?raw=true)

	_Create a new server farm_
 
1. In the **Specify Configuration Database Settings** page, complete the fields with the following information and click **Next**.

	1. **Database Server**: type the name of the computer where you installed SQL Server followed by _.contoso.com_
	
	1. **Database name**: type the name for your SharePoint configuration database. The default name is _SharePoint_Config_.

	1. **Username**: type the user name for the server farm account. Ensure that you type the user name in the format DOMAIN\user name. For testing purposes this can be the contoso\administrator account you created in the deploying Active Directory hands on lab.
	
		>**Note**: The server farm account is used to create and access your configuration database. It also acts as the application pool identity account for the SharePoint Central Administration application pool, and it is the account under which the Microsoft SharePoint Foundation Workflow Timer service runs. The SharePoint Products Configuration Wizard adds this account to the SQL Server Login accounts, the SQL Server dbcreator server role, and the SQL Server securityadmin server role. The user account that you specify as the service account must be a domain user account, but it does not need to be a member of any specific security group on your front-end Web servers or your database servers. We recommend that you follow the principle of least privilege and specify a user account that is not a member of the Administrators group on your front-end Web servers or your database servers.
		>
		>Find more information about this topic [here](http://technet.microsoft.com/en-us/library/cc287960.aspx).
	
	1. **Password**: type the user’s password.

		![Configuration Database Settings](Images/configuration-database-settings.png?raw=true)
	 
		_Configuration Database Settings_
 
1. In the **Specify Farm Security Settings** page, type a phrase that meets the minimum requirements and click **Next** to continue.

	![Farm Security Settings](Images/farm-security-settings.png?raw=true)
	 
	_Farm Security Settings_
 
	>**Note:** A passphrase is similar to a password, but it is usually longer to enhance security.

1. In the **Configure SharePoint Central Administration Web Application** page, choose _NTLM_ as **Authentication provider** and click **Next**.

	![Configure SharePoint Central Administration Web Application](Images/configure-sharepoint-central-administration-w.png?raw=true)
	 
	_Configure SharePoint Central Administration Web Application_

1. Review your configuration settings and click **Next**. Once the configuration settings are applied click **Finish**.

	![Completing the SharePoint Products Configuration Wizard](Images/completing-the-sharepoint-products-configurat2.png?raw=true)
	 
	_Completing the SharePoint Products Configuration Wizard_
 
1. Now, you will enable Anonymous Access in your SharePoint Server. To do this, open **SharePoint Central Administration**.

1. In the **Central Administration** section, under **Application Management**, click **Manage web applications** link.

	![SharePoint Central Administration](Images/sharepoint-central-administration.png?raw=true)
	 
	_SharePoint Central Administration_

1. On the top bar, click the **New** button.

	![New Site](Images/configure-sharepoint-new-site.png?raw=true)
	 
	_Web Application Management_ 

1. In the **Create New Web Application** dialog box, make sure the **port** is set to _80_ and enable Anonymous Access. Click **OK** to create the web application.

	![New Web Application](Images/configure-sharepoint-new-web-application.png?raw=true)
	 
	_Create New Web Application_ 

1. Click **OK** once the Web Application is created.

1. Select the web application recently created and click **Authentication Providers**, located in the **Web Applications** ribbon bar.

	![Authentication Providers](Images/authentication-providers.png?raw=true "Authentication Providers")

	_Authentication Providers_

1. In the **Authentication Providers** dialog, click the **Default** link.

1. In the **Edit Authentication** dialog, locate the **Anonymous Access** section and select the **Enable anonymous access** check box.

	![Edit Authentication](Images/edit-authentication.png?raw=true)
	 
	_Edit Authentication_
 
1. Back in the **Web Application Management** page, in the **Web Applications** tab, click **Anonymous Policy**.
 
1. In the **Anonymous Access Restrictions** dialog, locate **Permissions** section and select _None - No Policy_ as **Anonymous User Policy**.

	![Anonymous Policy](Images/anonymous-policy.png?raw=true)
	 
	_Anonymous Access Restrictions_ 

<a name="Ex2Task2" />
### Task 2 - Configure SharePoint to connect to the Farm ###
In this task, you will configure the SharePoint virtual machine to  connect to the SharePoint Farm. 

1. Go back to the **Microsoft Azure Portal** and go to **Virtual Machines** section.

1. Select the second SharePoint virtual machine _(spvm2)_ and click **Connect** to connect using **Remote Desktop**.

1. Open the **SharePoint 2013 Products Configuration Wizard**.

1. Follow the **SharePoint Products Configuration Wizard**. In the **Connect to a server farm** page, select **Connect to an existing server farm** option.
	  
	![SharePoint Configuration Wizard](Images/sharepoint-configuration-wizard.png?raw=true)

	_SharePoint Configuration Wizard_
 
1. In the **Specify Configuration Database Settings** page, type the name of the SQL Server instance in the **Database Server** box and click **Retrieve Database Names**.

1. In the **Database name** list, select the Configuration database’s name and click **Next**.

1. In the **Specify Farm Security Settings** page, type the **passphrase** you set in the SharePoint Server Farm and click **Next**.

1. Complete the **SharePoint Products Configuration Wizard**. Once the wizard finishes, it will launch the **Farm Configuration Wizard**. You do not need to run this wizard, close it to continue.

<a name="Verification" />
### Verification - Create a New SharePoint Site Collection ###

In this task, you will verify that the SharePoint Server was correctly configured by creating a new SharePoint Site Collection.

1. If not already connected, connect to the first SharePoint virtual machine ( _spvm1_ ) using **Remote Desktop Connection**.

1. Open **SharePoint 2013 Products Central Administrator**.

1. Create a new Site Collection. To do this, click **Create Site Collection** link under **Application Management** section within **Central Administration** page.

	![Application Management - Create Site Collections](Images/application-management---create-site-collecti.png?raw=true)
	 
	_Application Management - Create Site Collections_
 
1. In the **Create Site Collection** page, type a **Title** and a **Description** for the site collection. In the **Web Site Address** section, select _/sites/_ from the dropdown list and enter _SPFWebApp_.

	![Create Site Collection](Images/create-site-collection.png?raw=true)
	 
	_Create Site Collection_
 
1. In the **Template Selection** section, switch to **Publishing** tab and select **Publishing Portal** template. Then complete the **Primary and Secondary Site Collection Administrators**, use _contoso\[User name]_ where UserName is the one you configured in Deploying Active Directory HOL.

	![Create Site Collection(2)](Images/create-site-collection2.png?raw=true)
	 
	_Create Site Collection_
 
1. Leave the **Quota Template** with the default value and click **OK** to create the new Site Collection.

1. Once the Site Collection is ready, you will see a successfully created message. To test the site, click the URL shown.

	![Site Collection Created](Images/site-collection-created.png?raw=true)
	 
	_Site Collection Created_
 
1. If you are prompt for user and password, log on using a domain account (i.e.: The one you used for the Primary Site Collection Administrator).

 
1. Once logged on, you will see a site like the following one.

	![Site's Home Page](Images/sites-home-page.png?raw=true)
	 
	_Site’s Home Page_

1. Once in the site click **Set up Site Permissions** link 

1. In the Permissions ribbon, click **Anonymous Access**.

1. Change settings to **Entire Web Site** and click **OK**.

	![ConfigureAnonymous](Images/configureanonymous.png?raw=true)

	_Configuring anonymous access_

1. Now, test the SharePoint Farm connecting to the second SharePoint Virtual Machine _(spvm2)_. To do this, go back to the **Microsoft Azure Portal** and go to **Virtual Machines** section.

1. Select the second SharePoint virtual machine _(spvm2)_ and click **Connect** to connect using **Remote Desktop Connection**.

1. Open **SharePoint 2013 Products Central Administrator**.

1. Click in **Application Management** in the Central Administration page.

1. Under **Site Collections** click **View all sites collections** link.

1. Select the site you created in the first SharePoint server (SPFWebApp), copy the site’s URL and paste it in an Internet Explorer browser inside the Virtual Machine. If the site is working properly, you will be able to log on and access to the same home page you accessed from the first SharePoint server.

---

<a name="Exercise3" />
### Exercise 3: Configuring Load Balancing ###

<a name="Ex3Task1" />
#### Task 1 - Adding load balancing endpoints in Microsoft Azure portal ####

1. In the Microsoft Azure Portal click on the first virtual machine **SPVM1 | Endpoints | Add Endpoint** to open the endpoint create wizard.

	1. In the **Add endpoint to virtual machine** page, select the **Add Endpoint** option and then click the **right arrow** to continue.

		![add-endpoint](Images/add-endpoint.png?raw=true)

		_Adding an endpoint_

	1. In the **Specify endpoint details** page, enter _webport_ in the name field, select the **TCP** protocol, and enter _80_ in the public and private port fields. Finally, click the check to confirm the endpoint creation.

		![webendpoint](Images/webendpoint.png?raw=true)

		_Creating a web endpoint_

1. Once the web endpoint is created in the first virtual machine, you will access the second virtual machine and add a load balancing endpoint. Enter the second virtual machine dashboard, click **Endpoints** link, and click **Add Endpoint** button on the bottom bar to start the endpoint creation wizard.

	1. In the **Add endpoint to virtual machine** page, select the **Load-balance traffic on an existing endpoint** option. Then, select the **webport** endpoint from the list and click the arrow to continue.

		![Add load balancing endpoint wizard](Images/add-load-balancing-endpoint-wizard.png?raw=true "Add load balancing endpoint wizard")
		
		_Add load balancing endpoint wizard_

	1. In the **Specify endpoint details** page, define the same settings as the previous endpoint. Enter a Name (e.g. webport) and a private port (e.g. 80). Click the check to create the load balancing endpoint.

		![Load balancing endpoint details](Images/load-balancing-endpoint-details.png?raw=true "Load balancing endpoint details")

		_Load balancing endpoint details_


1. Wait until the endpoint is created, and the load balancing is enabled in both virtual machines.

1. To verify, select the endpoint in the list and click **Edit endpoint**. 

	![Edit Endpoint](Images/edit-endpoint.png?raw=true "Edit endpoint")

	_Edit Endpoint_

1. Notice that both virtual machines are configured as load-balanced machines. If you enter to the first virtual machine and edit its web endpoint, it will show the same configuration.

	![Edit endpoint details](Images/edit-endpoint-details.png?raw=true "Edit endpoint details")

	_Edit endpoint details_

1. Enter SPVM1 dashboard and locate the quick glance section. Take note of the virtual machine DNS and IP.

	![VM IP load balancing](Images/vm-ip-load-balancing.png?raw=true "VM IP load balancing")

	_Virtual machine IP load balancing_

1. Now enter SPVM2 dashboard and locate the quick glance section. Notice that the both virtual machines have the same virtual IP address and URL. That means, the load balancing is transparent for the user when a web site is retrieved. Internally, Microsoft Azure will redirect the traffic to either SPVM1 or SPVM2 hosts.

	![VM IP load balancing 2](Images/vm-ip-load-balancing-2.png?raw=true "VM IP load balancing 2")

	_Virtual machine IP load balancing 2_

1. Finally, start a new browser session and browse to the virtual machine URL. The URL should look like _http://myservice.cloudapp.net/sites/SPFWebApp_

---

<a name="summary" />
## Summary ##

In this hands-on lab you have learnt how to create a SharePoint farm, connected with Active Directory and SQL Server.

