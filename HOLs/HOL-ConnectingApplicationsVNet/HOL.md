<a name="handsonlab"></a>
# Connecting a PaaS application to an IaaS Application with a Virtual Network #

---

<a name="Overview"></a>
## Overview ##

In this lab, you will create a virtual machine with SQL Server installed using Microsoft Azure Management Portal. Then you will modify and deploy a sample Web application to a new Cloud Service. By the end, you will communicate the Cloud Service and the SQL Server virtual machine through a Virtual Network.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Directly connect a Web Role to a SQL Server running in a virtual machine through a simple virtual network
- Configure a SQL Server virtual machine
- Update and deploy the sample Web application to a Cloud App in Microsoft Azure

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Visual Studio Express 2012 for Web](http://www.microsoft.com/visualstudio/) or greater
- [Microsoft Azure Tools for Microsoft Visual Studio 1.8](http://www.microsoft.com/windowsazure/sdk/)
- A Microsoft Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

- [Creating a SQL Server Virtual Machine](#Exercise1)

- [Deploying a Simple MVC4 Application](#Exercise2)

 
Estimated time to complete this lab: **45 minutes**.

---

<a name="GettingStarted"></a>
### Getting Started - Configuring Virtual Networking ###

For this lab, you will define a virtual network where you can assign the virtual machines to specific subnets. 

<a name="GettingStartedTask1"></a>
#### Task 1 - Creating a new Virtual Network with a new affinity group ####

The first task is to create a new Virtual Network to your subscription with a new affinity group.

1. Open a browser and go to [https://manage.windowsazure.com/](https://manage.windowsazure.com/). When prompted, login with your **Microsoft Azure** credentials. In the Microsoft Azure Portal, click **New**, select **Networks** | **Virtual Network** and then click **Custom Create**.

	![Virtual Network custom create](Images/virtual-network-custom-create.png?raw=true)

	_Virtual Network custom create_

1. Set a Name for the virtual network, for example _MyVNET_. Select _Create a new affinity group_ option from the **affinity group** dropdown list, set a name for it, for example _myag_, and choose a region. Click the arrow button to continue.

	![creating a new virtual network](Images/creating-a-new-virtual-network.png?raw=true)

	_Creating a new virtual network_

1. At the top right corner, select the **CIDR** option. Then set the Address Space Starting IP value to _192.168.0.0_ and its Address CIDR to _/16_. Finally, add a subnet named _AppSubnet_ with a Starting IP of _192.168.1.0_ and an Address CIDR of _/24_. Click the arrow button to continue to the next step.

	![Adding an address space and subnets](Images/adding-an-address-space-and-subnets.png?raw=true)

	_Adding an address space and subnets_

1. Leave default settings for DNS and click the finish button.

	![Creating the Virtual Network](Images/creating-the-virtual-network.png?raw=true "Creating the Virtual Network")

	_Creating the Virtual Network_

---

<a name="Exercise1"></a>
### Exercise 1: Creating a SQL Server Virtual Machine ###

In this exercise, you will create a new virtual machine with SQL Server and configure a public endpoint in order to access it remotely.

<a name="Ex1Task1"></a>
#### Task 1 - Creating a Virtual Machine Using Microsoft Azure Portal ####

In this task, you will create a new virtual machine using the Microsoft Azure Portal.

1. Navigate to the **Microsoft Azure Portal** using a Web browser and sign in using the **Microsoft Account** associated with your Microsoft Azure account.

1. Click **New** and select **Virtual Machine** option and then **From Gallery**.

	![Creating a New Virtual Machine](Images/creating-a-new-vm.png?raw=true "Creating a New Virtual Machine")
 
	_Creating a New Virtual Machine_

1. In the **Virtual machine operating system selection** page, click **Platform Images** on the left menu and select the **SQL Server 2012 SP1** OS image from the list. Click the arrow to continue.

1. In the **Virtual machine Configuration** page, enter a **Virtual Machine Name**, provide a user name for the **New User Name** field and a password for the **New Password** and **Confirm Password** fields. Lastly, set the Virtual Machine **Size** to _Small_ and click **Next** to continue.

	>**Note:** You will use these credentials in future steps to connect to the virtual machine using remote desktop.

	![Virtual Machine Configuration](Images/vm-configuration.png?raw=true "Virtual Machine Configuration")
 
	_Virtual Machine Configuration_

1. In the **Virtual machine mode** page, select **Standalone Virtual Machine** option and provide a unique name for the **DNS Name**. Finally, select a **Storage Account** or leave the default value _Use an automatically generated storage account_ and then select the Virtual Network you created previously from the **Region/Affinity Group/Virtual Network** list. Select the **APPSUBNET** virtual network subnet from the Virtual Network Subnet list and click **Next** to continue.

	![Selecting Virtual Machine mode](Images/selecting-vm-mode.png?raw=true "Selecting Virtual Machine mode")

	_Setting the Virtual Machine Mode_

1. In the **Virtual machine options** page, leave the default values and click the finish button to create the new virtual machine.

	![Virtual Machine Options](Images/vm-options.png?raw=true "Virtual Machine Options")

	_Setting the Virtual Machine Options_

1. In the **Virtual Machines** section, you will see the virtual machine you created displaying a _provisioning_ status. Wait until it changes to _Running_ in order to continue.

	> **Note:** It will take from 8 to 10 minutes for the virtual machine to complete the provisioning process.

1. Now, you will create and attach empty data disks to store the SQL Server logs and data files, and you will also add an endpoint. To do this, in the **Virtual Machines** section, select the SQL Server virtual machine you created in this task.

1. In the virtual machine's **Dashboard**, click **Attach** in the menu at the bottom of the page and select **Attach Empty Disk**.

	![Attach Empty Disk](Images/attach-empty-disk.png?raw=true "Attach Empty Disk")

	_Attach Empty Disk_

1. In the **Attach Empty Disk** page, set the **Size** to _50_ GB and create the Disk.

1. Wait until the attach disk process finishes. Repeat the steps 9 and 10 to create a second disk.

1. You will see three disks for the virtual machine: one for the **OS** and other two for **Data** and **Logs**.

	> **Note:** It might take a few minutes until the data disks appear in the virtual machine's dashboard within the Microsoft Azure Portal.

<a name="Ex1Task2"></a>
#### Task 2 - Configuring SQL Server 2012 Instance ####

In this task, you will set up SQL Server and configure it to enable remote access.

1. In the Microsoft Azure Management Portal, click **Virtual Machines** on the left menu.

 	![Microsoft Azure Portal](./Images/Windows-Azure-Portal.png?raw=true "Microsoft Azure Portal")
 
	_Microsoft Azure Portal_

1. Select your virtual machine from the virtual machines list and click **Connect** to connect using **Remote Desktop Connection**.

	> **Note:** use the credentials that you inserted when creating the virtual machine in the previous task.

1. In the virtual machine, open **Server Manager** from **Start | All Programs | Administrative Tools**.

1. Expand **Storage** node and select **Disk Management** option.

 	![Disk Management(2)](Images/disk-management2.png?raw=true)
 
	_Disks Management_

1. After selecting Disk Management, an **Initialize Disk** dialog will be displayed. Leave the default values and click **OK**. 

	> **Note**: If the Initialize Disk dialog is not displayed when selecting Disk Management, locate the disks you created using the **Attach Empty Disk** feature from the Microsoft Azure Management Portal, right-click the first disk and select **Initialize Disk**. Leave the default values and click **OK**.

1. Right-click the first disk unallocated space and select **New Simple Volume**.

 	![Disk Management](Images/disk-management.png?raw=true)
 
	_Disks Management_

1. Follow the **New Simple Volume Wizard**. When asked for the **Volume Label** use _SQLData_.

1. Wait until the process for the first disk is completed. Repeat the steps 6 to 8 but this time using the second disk. Set the **Volume Label** to _SQLLogs_.

1. The **Disk Management** list of available disks should now show the **SQLData** and **SQLLogs** disks like in the following figure:

 	![Disks Management](./Images/Disks-Management.png?raw=true "Disks Management")
 
	_Disks Management_

1. Open **SQL Server Configuration Manager** from **Start | All Programs | Microsoft SQL Server 2012 | Configuration Tools**.

1. Expand the **SQL Server Network Configuration** node and select **Protocols for MSSQLSERVER** (this option might change if you used a different instance name when installing SQL Server). Make sure **Shared Memory**, **Named Pipes** and **TCP/IP** protocols are enabled. To enable a protocol, right-click the protocol name and select **Enable**.

 	![Enabling SQL Server Protocols](./Images/Enabling-SQL-Server-Protocols.png?raw=true "Enabling SQL Server Protocols")
 
	_Enabling SQL Server Protocols_

1. Select the **SQL Server Services** node and right-click the **SQL Server (MSSQLSERVER)** item and select **Restart.**

<a name="Ex1Task3"></a>
#### Task 3 - Installing the AdventureWorks Database ####

In this task, you will add the **AdventureWorks** database that will be used by the sample application in the following exercise.

1. In order to enable downloads from IE you will need to update **Internet Explorer Enhanced Security Configuration**. In the Microsoft Azure virtual machine, open **Server Manager** from **Start | All Programs | Administrative Tools**.

1. In the **Server Manager** window, click **Configure IE ESC** within the **Security Information** section.

	![Configure Internet Explorer Enhanced Security](Images/configure-internet-explorer-enhanced-security.png?raw=true "Configure Internet Explorer Enhanced Security")
 
	_Configure Internet Explorer Enhanced Security_

1. In the **Internet explorer Enhanced Security Configuration** dialog, turn **off** enhanced security for **Administrators** and click **OK**.

 	![Internet Explorer Enhanced Security](./Images/Internet-Explorer-Enhanced-Security.png?raw=true "Internet Explorer Enhanced Security")
 
	_Internet Explorer Enhanced Security_

	>**Note:** Modifying Internet Explorer Enhanced Security configurations is not good practice and is only for the purpose of this particular lab. The correct approach should be to download the files locally and then copy them to a shared folder or directly to the virtual machine.

1. Open the SQL Server Management Studio from **Start | All Programs | Microsoft SQL Server 2012 | SQL Server Management Studio**.

1. Connect to the SQL Server 2012 default instance using your Windows Account.

1. Now, you will update the database's default locations in order to split the DATA from the LOGS. To do this, right click on you SQL Server instance and select **Properties**.

1. Select **Database Settings** from the left side pane.

1. Locate the **Database default locations** section and update the default values to point to the disks you attached in the previous task.

 	![Setting Database Default Locations](./Images/Setting-Database-Default-Locations.png?raw=true "Setting Database Default Locations")
 
	_Setting Database Default Locations_

1. Using Windows Explorer create the following folders: **F:\Data, G:\Logs** and **G:\Backups**.

1. Restart SQL Server. In the **Object Explorer**, right-click on the server node and select **Restart**. 

1. This lab uses the **AdventureWorks2012** database. Open an **Internet Explorer** browser and go to <http://msftdbprodsamples.codeplex.com/> to download  the **SQL Server 2012** sample databases. Once on the page click _Download AdventureWorks Databases – 2008, 2008R2 and 2012_ and then download _AdventureWorks2012_Database.zip_ file. Download it to F:\Data.

1. Extract the files from the zip one into F:\Data, then right click the database file and open the properties. Click **Unblock**.

1. Add the **AdventureWorks2012** sample database to your SQL Server. To do this, open **SQL Server Management Studio**, connect to **(local)** using your Windows Account. Locate your SQL Server instance node and expand it.

1. Right click **Databases** folder and select **Attach**.

	![Object Explorer - Attaching AdventureWorks2012 Database](Images/attaching-adventureworks-database-menu.png?raw=true)
 
	_Object Explorer - Attaching Adventureworks2012 Database_

1. In the **Attach Databases** dialog, press **Add**. Browse to the data disk and select the Adventure Works 2012 data file.

1. Now, select the AdventureWorks2012 Log's row within **database details** and click **Remove**.

 	![Attaching AdventureWorks Database](./Images/attaching-adventureworks-database.png?raw=true "Attaching AdventureWorks Database")
 
	_Attaching AdventureWorks2012 Database_

1. Click **OK** to attach the database. 

1. Create a Full Text Catalog for the database. You will consume this feature with an MVC application you will deploy in the next exercise. To do this, expand **Storage** node within **AdventureWorks2012** database.

1. Right-click **Full Text Catalogs** folder and select **New Full-Text Catalog**.

	![New Full-Text Catalog](Images/new-full-text-catalog.png?raw=true "New Full-Text Catalog")
 
	_New Full-Text Catalog_

1. In the **New Full-Text Catalog** dialog, set the **Name** value to _AdventureWorksCatalog_ and press **OK**.

	![New Full-Text Catalog Name](Images/new-full-text-catalog-name.png?raw=true "New Full-Text Catalog Name")
 
	_Full-Text Catalog Name_

1. Right-click the **AdventureWorksCatalog** and select **Properties**. Select the **Tables/Views** menu item. Add the **Production.Product** table to the **Table/view objects assigned to the catalog** list. Check _Name_ from **Eligible columns**, select **English** in _Language for Word Breaker_ column and click **OK**.

	![Full-Text Catalog Properties](Images/full-text-catalog-properties.png?raw=true "Full-Text Catalog Properties")
 
	_Full-Text Catalog Properties_

1. Enable **Mixed Mode Authentication** to the SQL Server instance. To do this, in the **SQL Server Management Studio**, right-click the server instance and click **Properties**.

1. Select the **Security** page in the left side pane and then select **SQL Server and Windows Authentication mode** under **Server Authentication** section. Click **OK** to save changes.

    ![Mixed authentication mode](Images/mixed-authentication-mode.png?raw=true "Mixed authentication mode")

    _Mixed authentication mode_

1. Restart the SQL Server instance. To do this, right-click the SQL Server instance and click **Restart**.

1. Add a new user for the MVC4 application you will deploy in the following exercise. To do this, expand **Security** folder within the SQL Server instance. Right-click **Logins** folder and select **New Login**.

 	![Creating a New Login](./Images/create-new-login.png?raw=true "Creating a New Login")
 
	_Creating a New Login_

1. In the **General** section, set the **Login name** to _CloudShop_. Select **SQL Server authentication** option and set the **Password** to _Azure$123_.

	>**Note:** If you entered a different username or password than those suggested in this step you will need to update the web.config file for the MVC 4 application you will use in the next exercise to match those values

1. Uncheck **Enforce password policy** option to avoid having to change the User's password the first time you log on and set the **Default database** to _AdventureWorks2012_.

	![New Login's General Settings](Images/new-logins-general-settings.png?raw=true "New Login's General Settings")
 
	_Creating a New Login_

1. Go to **User Mapping** section. Map the user to the _AdventureWorks2012_ database and click **OK**.

 	![Mapping the new User to the AdventureWorks2012 Database](./Images/Mapping-the-new-User.png?raw=true "Mapping the new User to the AdventureWorks Database")
 
	_Mapping the new User to the AdventureWorks2012 Database_

1. Expand **AdventureWorks2012** database within **Databases** folder. In the **Security** folder, expand **Users** and double-click **CloudShop** user.

1. Select the **Membership** page, and select the _db_owner_ role checkbox for the **CloudShop** user and click **OK**.

	![Adding Database role membership to CloudShop user](./Images/Adding-Database-role-membership-to-CloudShop-user.png?raw=true "Adding Database role membership to CloudShop user")
 
	_Adding Database role membership to CloudShop user_
 
	>**Note:** The application you will deploy in the next exercise uses Universal Providers to manage sessions. The first time the application run the Provider will create a Sessions table within AdventureWorks2012 database. For that reason, you are assigning db_owner role for the CloudShop user. Once you run the application for the first time, you can remove this role for the user as it will not need those permissions anymore.

1. Close the **SQL Server Management Studio**.

1. In order to allow the MVC4 application access the SQL Server database you will need to add an **Inbound Rule** for the SQL Server requests in the **Windows Firewall**. To do this, open **Windows Firewall with Advance Security** from **Start | All Programs | Administrative Tools**.

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

1. Close **Windows Firewall with Advanced Security** window and then close the **Remote Desktop Connection**.

---

<a name="Exercise2"></a>
### Exercise 2: Deploying a Simple MVC4 Application ###

In this exercise, you will configure a simple Web application to connect to the SQL Server instance you created in the previous exercise and publish the application to **Microsoft Azure** and run it in the Cloud.

<a name="Ex2Task1"></a>
#### Task 1 - Configuring the MVC4 Application to Connect to an SQL Server Instance ####

In this task, you will change the connection string to point to the SQL Server instance created in the previous exercise and update the configuration settings to enable network communication between the Web Role and the SQL Server instance.

1. Navigate to the **Microsoft Azure Portal** using a Web browser and sign in using the **Microsoft Account** associated with your Microsoft Azure account.

1. In the left side pane, click on **Virtual Machines** and locate the SQL Server virtual machine you created in the previous exercise. Select the virtual machine and then click **Connect**. 

1. When prompted to save or open the .rdp file, click **Open** and then log on using the Admin credentials you defined when you created the virtual machine.

1. Once logged on, open a **Command Prompt** window and type **ipconfig** to display the machine's IP configuration.

1. Take note of the machine's ip address as you will use it later.

	![Getting the IP address](Images/getting-the-ip-address.png?raw=true "Getting the IP address")

	_Getting the IP address_

1. Open Visual Studio Express 2012 for Web as administrator.

1. Open the solution **IaasDeploySimpleApp.sln** located in the folder **Ex02-DeploySampleApp** under the **Source** folder of this lab.

1. Compile the solution in order to download the required packages.

1. Open the **Web.config** file and locate the **connectionStrings** node. Replace the **Data Source** attribute values with the IP address of the  SQL Server virtual machine you copied in step 5.

	<!--mark: 1-5-->
	````XML
	<connectionStrings>
		<add name="DefaultConnection" connectionString="Data Source=[ENTER-IP-ADDRESS];initial catalog=AdventureWorks2012;Uid=CloudShop;Password=Azure$123;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" />
		<add name="AdventureWorksEntities" connectionString="metadata=res://*/Models.AdventureWorks.csdl|res://*/Models.AdventureWorks.ssdl|res://*/Models.AdventureWorks.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=[ENTER-IP-ADDRESS];initial catalog=AdventureWorks2012;Uid=CloudShop;Password=Azure$123;multipleactiveresultsets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
	</connectionStrings>
	````

1. In order to enable communication between the Web Role and the SQL Server virtual machine, you need configure the Web Role to connect to the same **Virtual Network** as the SQL Server virtual machine. To do so, open the **ServiceConfiguration.Cloud.cscfg** under the **CloudShop.Azure** project and add the highlighted  configuration:

	<!--mark: 9-18-->
	````XML
<?xml version="1.0" encoding="utf-8"?>
<ServiceConfiguration serviceName="CloudShop.Azure" xmlns="http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration" osFamily="3" osVersion="*" schemaVersion="2012-10.1.8">
  <Role name="CloudShop">
    <Instances count="1" />
    <ConfigurationSettings>
      <Setting name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value="UseDevelopmentStorage=true" />
    </ConfigurationSettings>
  </Role>
  <NetworkConfiguration>
    <VirtualNetworkSite name="MyVNET" />
    <AddressAssignments>
      <InstanceAddress roleName="CloudShop">
        <Subnets>
          <Subnet name="AppSubnet" />
        </Subnets>
      </InstanceAddress>
    </AddressAssignments>
  </NetworkConfiguration>
</ServiceConfiguration>
````

<a name="Ex2Task2"></a>
#### Task 2 - Publishing the MVC4 Application to Microsoft Azure ####

In this task, you will publish the Web Application to Microsoft Azure using Visual Studio.

1. In Visual Studio, right-click the **CloudShop.Azure** project and select **Package**.

	![Packaging the Cloud Application](Images/packaging-the-cloud-application.png?raw=true "Packaging the Cloud Application")

	_Packaging the Cloud Application_

1. In the **Package Microsoft Azure Application** dialog, make sure that _Service Configuration_ is set to **Cloud** and _Build Configuration_ is set to **Release**. Then click the **Package** button.

	![Package Microsoft Azure Application dialog](Images/package-windows-azure-application-dialog.png?raw=true "Package Microsoft Azure Application dialog")

	_Package Microsoft Azure Application dialog_

1. Wait to the package process to finish to continue with the next step.

1. Navigate to the **Microsoft Azure Portal** using a Web browser and sign in using the **Microsoft Account** associated with your Microsoft Azure account.

1. Click the **New** link located at the bottom of the page, select **Compute** | **Cloud Service** and then **Custom Create**.

1. In the **Create a cloud service** window, enter **CloudShop** in the **Url** field, select **myag** from the **Region or Affinity Group** selection list and check the **Deploy a Cloud Service package now** option.

	![New Cloud Service](Images/new-cloud-service5.png?raw=true "New Cloud Service")

	_New Cloud Service_

1. In the **Publish your cloud service** window, enter a name for the new deployment (for instance **CloudShop**). Enter the location for your package and configuration files (usually under the bin\Release\app.publish folder of your cloud project) and check the **Deploy even if one or more roles contain a single instance** option. Then click the **Finish** button.

	![Publish your cloud service](Images/new-cloud-service4.png?raw=true "New Cloud Service")

	_Publish your cloud service_

1. Wait until your new _Cloud Service_ is deployed and provisioned.

1. Once the _Cloud Service_ status gets to **Created** click on the service's name and go to the **Dashboard** page. Once there, click the **Site Url** link in the **quick glance** pane.

	![Cloud Service Dashboard](Images/cloud-service-dashboard.png?raw=true "Cloud Service Dashboard")

	_Cloud Service Dashboard_

1. The browser will show you the home page of the **CloudShop** sample application. In the **Search** box, write _Classic_ and click **Search**. It will show all the products that match the search criteria. The Cloud App is accessing the SQL Server using the public endpoint to retrieve the list of products.

 	![Searching Products](./Images/Searching-Products.png?raw=true "Searching Products")
 
	_Searching Products_

---

<a name="Summary"/></a>
## Summary ##

By completing this hands-on lab, you have learnt how to:

- Directly connect a Web Role to a SQL Server running in a virtual machine through a simple virtual network
- Configure a SQL Server virtual machine
- Update and deploy the sample Web application to a Cloud App in Microsoft Azure
