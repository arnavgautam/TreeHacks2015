<a name="handsonlab"></a>
# Connecting a PaaS application to an IaaS Application #

---

<a name="Overview"></a>
## Overview ##

In this lab, you will create a Virtual Machine with SQL Server installed using Microsoft Azure Management Portal. Then you will modify a sample Web application to connect to the SQL Server using a public endpoint. By the end, you will have two different hosted services with different instances communicating with each other. This type of communication is known as **Simple Mixed Mode**.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Configure a SQL Server Virtual Machine
- Connect a sample Web application with SQL Server using a public endpoint
- Deploy the sample Web application to a Cloud App in Microsoft Azure

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

<a name="Exercise1"></a>
### Exercise 1: Creating a SQL Server Virtual Machine ###

In this exercise, you will create a new Virtual Machine with SQL Server and configure a public endpoint to access it remotely.

<a name="Ex1Task1"></a>
#### Task 1 - Creating a Virtual Machine Using the Microsoft Azure Portal ####

In this task, you will create a new Virtual Machine using the Microsoft Azure Portal.

1. Go to the [Microsoft Azure Management Portal](https://manage.windowsazure.com) and sign with your Windows account credentials.

	![Log on to Microsoft Azure portal](Images/login.png?raw=true "Log on to Microsoft Azure portal")

	_Log on to Microsoft Azure Management Portal_
	
1. Click **New** and select **Compute** | **Virtual Machine** option and then **From Gallery**.

	![Creating a New virtual machine](Images/creating-a-new-vm.png?raw=true "Creating a New virtual machine")
 
	_Creating a New Virtual Machine_

1. In the **Virtual machine operating system Selection** page, click **Platform Images** on the left menu and select the **Microsoft SQL Server 2012 SP1 Evaluation Edition** OS image from the list. Click the next arrow to continue.

1. In the **Virtual machine Configuration** page, enter a **Virtual Machine Name**, provide a user name for the **New User Name** field and a password for the **New Password** and **Confirm Password** fields. Lastly, set the Virtual Machine **Size** to _Small_ and click **Next** to continue.

	>**Note:** You will use these credentials in future steps to connect to the Virtual Machine using remote desktop.

	![Virtual Machine Configuration](Images/vm-configuration.png?raw=true "Virtual Machine Configuration")
 
	_Virtual Machine Configuration_

1. In the **Virtual machine Mode** page, select **Standalone Virtual Machine** option and provide a unique name for the **DNS Name**. Then select a **Storage Account** or leave the default value _Use Automatically Generated Storage Account_, select a **Region/Affinity Group/Virtual Network** and click **Next** to continue.


 	![Selecting Virtual Machine mode](./Images/Selecting-VM-mode.png?raw=true "Selecting Virtual Machine mode")
 
	_Selecting the Virtual Machine mode_

1. In the **Virtual machine Options** page, leave the default options and click the **complete** button to create the virtual machine.

	> **Note:** It will take from 8 to 10 minutes for the Virtual Machine to complete the provisioning process.

1. In the **Virtual Machines** section, you will see the Virtual Machine you just created, showing the _provisioning_ status. Wait until it changes to _Running_ in order to continue with the following step.

1. In the **Virtual Machines** section, locate the Virtual Machine you just created and click its name. Once in the **Dashboard** page, click **Endpoints**.

1. Click **Add Endpoint**, select Add Endpoint option and then click the **Next** button to continue.

	![Adding a new Endpoint](Images/adding-a-new-endpoint.png?raw=true "Adding a new Endpoint")
	
	_Adding a new Endpoint_

1. In the **New Endpoint Details** page, set the **Name** to _sqlserver_, the **Protocol** to _TCP_, the **Public Port** to 57500 and the **Private Port** to 1433.

	![New Endpoint Details](Images/new-endpoint-details.png?raw=true "New Endpoint Details")

	_New Endpoint Details_

1. Now, you will create and attach empty data disks to store the SQL Server logs and data files, and you will also add an endpoint. To do this, in the **Virtual Machines** section, select the SQL Server Virtual Machine you created in this task.

1. In the Virtual Machine's **Dashboard**, click **Attach** in the menu at the bottom of the page and select **Attach Empty Disk**.

	![Attach Empty Disk](Images/attach-empty-disk.png?raw=true "Attach Empty Disk")

	_Attach Empty Disk_

1. In the **Attach Empty Disk** page, set the **Size** to _50_ GB and create the Disk.

1. Wait until the process to attach the disk finishes. Repeat the steps 11 to 13 to create a second disk.

1. You will see three disks for the virtual machine: one for the **OS** and other two for **Data** and **Logs**.

	> **Note:** It might take a few minutes until the data disks appear in the virtual machine's dashboard within the Azure Portal.



<a name="Ex1Task2"></a>
#### Task 2 - Configuring SQL Server 2012 Instance ####

In this task, you will install an SQL Server and configure it to enable remote access.

1. In the Microsoft Azure Management Portal menu, click **Virtual Machines** on the left menu.

 	![Microsoft Azure Portal](./Images/Windows-Azure-Portal.png?raw=true "Microsoft Azure Portal")
 
	_Microsoft Azure Portal Menu_

1. Select your virtual machine from the Virtual Machines list and click **Connect** to connect using **Remote Desktop Connection**. An RDP file will be downloaded to your local machine, which needs to be opened to launch _Remote Desktop_.

1. In the Virtual Machine, open **Server Manager** from **Start | All Programs | Administrative Tools**.

1. Expand the **Storage** node and select the **Disk Management** option.

 	![Disk Management(2)](Images/disk-management2.png?raw=true)
 
	_Disk Management_

1. After selecting Disk Management, an **Initialize Disk** dialog will be displayed. Leave the default values and click **OK**. 

	> **Note**: If the Initialize Disk dialog is not displayed when selecting Disk Management, locate the disks you created using the **Attach Empty Disk** feature from the Microsoft Azure Management Portal, right-click the first disk and select **Initialize Disk**. Leave the default values and click **OK**.

1. Right-click the first disk unallocated space and select **New Simple Volume**.

	 ![Disk Management](Images/disk-management.png?raw=true)
 
	_Disks Management_

1. Follow the **New Simple Volume Wizard**, keeping all the default values. When asked for the **Volume Label** set it to _SQLData_.

1. Wait until the process for the first disk is completed. Repeat the steps 5 to 8 but this time using the second disk. Set the **Volume Label** to _SQLLogs_.

1. The **Disk Management** list of available disks should now show the **SQLData** and **SQLLogs** disks, as in the following figure:

 	![Disks Management](./Images/Disks-Management.png?raw=true "Disks Management")
 
	_Disk Management_

1. Open **SQL Server Configuration Manager** from **Start | All Programs | Microsoft SQL Server 2012 | Configuration Tools**.

1. Expand the **SQL Server Network Configuration** node and select **Protocols for MSSQLSERVER** (this option may be different if you used a different instance name when installing SQL Server). Make sure **Shared Memory**, **Named Pipes** and **TCP/IP** protocols are enabled. To enable a protocol, right-click the protocol name and select **Enable**.

 	![Enabling SQL Server Protocols](./Images/Enabling-SQL-Server-Protocols.png?raw=true "Enabling SQL Server Protocols")
 
	_Enabling SQL Server Protocols_

1. Go to the **SQL Server Services** node and right-click the **SQL Server (MSSQLSERVER)** item and select **Restart.**

<a name="Ex1Task3"></a>
#### Task 3 - Installing the AdventureWorks Database ####

In this task, you will add the **AdventureWorks** database that will be used by the sample application in the following exercise.

1. In order to enable downloads from IE you will need to update **Internet Explorer Enhanced Security Configuration**. In the Azure Virtual Machine, open **Server Manager** from **Start | All Programs | Administrative Tools**.

1. In the **Server Manager** window, click **Configure IE ESC** within the **Security Information** section.

	![Configure Internet Explorer Enhanced Security](Images/configure-internet-explorer-enhanced-security.png?raw=true "Configure Internet Explorer Enhanced Security")
 
	_Configure Internet Explorer Enhanced Security_

1. In the **Internet explorer Enhanced Security** dialog, turn **off** enhanced security for **Administrators** and click **OK**.

 	![Internet Explorer Enhanced Security](./Images/Internet-Explorer-Enhanced-Security.png?raw=true "Internet Explorer Enhanced Security")
 
	_Internet Explorer Enhanced Security_

	>**Note:** Modifying Internet Explorer Enhanced Security configurations is not good practice and is only for the purpose of this particular lab. The correct approach should be to download the files locally and then copy them to a shared folder or directly to the virtual machine.

1. Open the SQL Server Management Studio from **Start | All Programs | Microsoft SQL Server 2012 | SQL Server Management Studio**.

1. Connect to the SQL Server 2012 default instance using your Windows Account.

1. Now, you will update the database's default locations in order to split the DATA from the LOGS. To do this, right click on your SQL Server instance and select **Properties**.

1. Select **Database Settings** from the left side pane.

1. Locate the **Database default locations** section and update the default values to point to the disks you attached in the previous task.

 	![Setting Database Default Locations](./Images/Setting-Database-Default-Locations.png?raw=true "Setting Database Default Locations")
 
	_Setting the Database Default Locations_

1. Using Windows Explorer create the following folders: **F:\Data, G:\Logs** and **G:\Backups**.

1. Restart SQL Server. To do this, go back to **SQL Server Management Studio**, and in the **Object Explorer**, right-click the server node and select **Restart**. Confirm the prompted dialog.

1. This lab uses the **AdventureWorks2012** database. Open **Internet Explorer** and browse to <http://msftdbprodsamples.codeplex.com/> to download the **SQL Server 2012** sample databases.

1. Right-click the downloaded file and select **Properties**. Click **Unblock**, and close the dialog by clicking **OK**.

1. Decompress the database files.

1. Add the **AdventureWorks2012** sample database to your SQL Server. To do this, open **SQL Server Management Studio**, connect to **(local)** using your Windows Account. Locate your SQL Server instance node and expand it.

1. Right-click **Databases** folder and select **Attach**.

	![Object Explorer - Attaching AdventureWorks Database](Images/attaching-adventureworks-database-menu.png?raw=true)
 
	_Object Explorer - Attaching the Adventureworks2012 Database_

1. In the **Attach Databases** dialog, press **Add**. Browse to the path where the Sample Databases were installed and select **AdventureWorks2012** data file. Click **OK**.

1. Now, select the AdventureWorks2012 Log's row within **database details** and click **Remove**.

 	![Attaching AdventureWorks Database](./Images/attaching-adventureworks-database.png?raw=true "Attaching AdventureWorks Database")
 
	_Attaching the AdventureWorks2012 Database_

1. Click **OK** to attach the database. 

1. Create a Full Text Catalog for the database. You will consume this feature with a MVC application you will deploy in the next exercise. To do this, expand the *Databases\AdventureWorks2012\Storage* node, right-click **Full Text Catalogs** folder and select **New Full-Text Catalog**.

	![New Full-Text Catalog](Images/new-full-text-catalog.png?raw=true "New Full-Text Catalog")
 
	_New Full-Text Catalog_

1. In the **New Full-Text Catalog** dialog, set the **Name** value to _AdventureWorksCatalog_ and press **OK**.

	![New Full-Text Catalog Name](Images/new-full-text-catalog-name.png?raw=true "New Full-Text Catalog Name")
 
	_Full-Text Catalog Creation_

1. Right-click the **AdventureWorksCatalog** and select **Properties**. Select the **Tables/Views** menu item. Add the **Production.Product** table to the **Table/View objects assigned to the Catalog** list. Lastly, select _Name_ from **Eligible columns**, and choose **English** in _Language for Word Breaker_ column and click **OK**.

	![Full-Text Catalog Properties](Images/full-text-catalog-properties.png?raw=true "Full-Text Catalog Properties")
 
	_Full-Text Catalog Properties_

1. Enable **Mixed Mode Authentication** in the SQL Server instance. To do this, in the **SQL Server Management Studio**, right-click the server instance and click **Properties**.

1. Select the **Security** page in the right side pane and then select **SQL Server and Windows Authentication mode** under **Server Authentication** section. Click **OK** to save changes.

    ![Mixed authentication mode](Images/mixed-authentication-mode.png?raw=true "Mixed authentication mode")

    _Mixed authentication mode_

1. Restart the SQL Server instance. To do this, right-click the SQL Server instance and click **Restart**.

1. Add a new user for the MVC4 application you will deploy in the following exercise. To do this, expand the **Security** folder within the SQL Server instance. Right-click the **Logins** folder and select **New Login**.

 	![Creating a New Login](./Images/create-new-login.png?raw=true "Creating a New Login")
 
	_Creating a New Login_

1. In the **General** section, set the **Login name** to _CloudShop_. Select **SQL Server authentication** option and set the **Password** to _Azure$123_.

	>**Note:** If you entered a different username or password than those suggested in this step you will need to update the web.config file for the MVC4 application that you will use in the next exercise to match those values.

1. Uncheck **Enforce password policy** option to avoid having to change the User's password the first time you log on and set the **Default database** to _AdventureWorks2012_.

	![New Login's General Settings](Images/new-logins-general-settings.png?raw=true "New Login's General Settings")
 
	_Creating a New Login_

1. Go to **User Mapping** section. Map the user to the _AdventureWorks2012_ database and click **OK**.

 	![Mapping the new User to the AdventureWorks Database](./Images/Mapping-the-new-User.png?raw=true "Mapping the new User to the AdventureWorks Database")
 
	_Mapping the new User to the AdventureWorks Database_

1. Expand the **AdventureWorks2012** database within **Databases** folder. In the **Security** folder, expand **Users** and double-click the **CloudShop** user.

1. Go to the **Membership** page, and select the _db_owner_ role checkbox for the **CloudShop** user and click **OK**.

	![Adding Database role membership to CloudShop user](./Images/Adding-Database-role-membership-to-CloudShop-user.png?raw=true "Adding Database role membership to CloudShop user")
 
	_Adding Database role membership to CloudShop user_
 
	>**Note:** The application you will deploy in the next exercise uses Universal Providers to manage sessions. The first time the application runs, the Provider will create a Sessions table within the AdventureWorks database. For that reason, you are assigning the db_owner role to the CloudShop user. Once you run the application for the first time, you can remove this role for the user as it will not need those permissions anymore.

1. Close **SQL Server Management Studio**.

1. In order to allow the MVC4 application access the SQL Server database you will need to add an **Inbound Rule** for the SQL Server requests in the **Windows Firewall**. To do this, open **Windows Firewall with Advance Security** from **Start | All Programs | Administrative Tools**.

1. Select **Inbound Rules** node, right-click it and select **New Rule**.

 	![Creating an Inbound Rule](./Images/Creating-an-Inbound-Rule.png?raw=true "Creating an Inbound Rule")
 
	_Creating an Inbound Rule_

1. In the **New Inbound Rule Wizard**, select _Port_ as the **Rule Type** and click **Next**.

	![New Inbound Rule Type](Images/new-inbound-rule-type.png?raw=true "Inbound Rule Type")
 
	_Inbound Rule's Type_

1. In the **Protocols and Ports** step, select **Specific local ports** and set its value to _1433_. Click **Next** to continue.

	![Inbound Rule's Local Port](Images/inbound-rules-local-port.png?raw=true "Inbound Rule's Local Port")
 
	_Inbound Rule's Local Port_

1. In the **Action** step, make sure the **Allow the connection** option is selected and click **Next**.

	![Inbound Rule's Action](Images/inbound-rules-action.png?raw=true "Inbound Rule's Action")
 
	_Inbound Rule's Action_

1. In the **Profile** step, leave the default values and click **Next**.

1. Finally, set the Inbound Rule's **Name** to _SQLServerRule_ and click **Finish**.

 	![New Inbound Rule](Images/new-inbound-rule.png?raw=true "New Inbound Rule")
 
	_New Inbound Rule_

1. Close the **Windows Firewall with Advanced Security** window and then close the **Remote Desktop Connection**.

<a name="Exercise2"></a>
### Exercise 2: Deploying a Simple MVC4 Application ###

In this exercise, you will configure a simple Web application to connect to the SQL Server instance you created in the previous exercise, by using a public endpoint. You will test the application using the local Azure Emulator. Then, you will publish the application to **Microsoft Azure** and run it in the Cloud.

<a name="Ex2Task1"></a>
#### Task 1 - Configuring the MVC4 Application to Connect to a SQL Server Instance ####

In this task, you will change the connection string to point to the SQL Server instance created in the previous exercise.

1. Open Visual Studio 2012 Express for Web or higher as administrator.

1. Open the solution **IaaSDeploySimpleApp.sln** located in the folder **Ex02-DeploySampleApp** under the **Source** folder of this lab.

1. Open the **Web.config** file of the **CloudShop** project and locate the **connectionStrings** node. Replace the **Data Source** attribute values with the address of the public DNS of the SQL Server Virtual Machine, the SQL Server instance name and the public port endpoint that you created in the previous exercise (For example: _vmname.cloudapp.net,57500_).

	<!--mark: 1-5-->
	````XML
	<connectionStrings>
		<add name="DefaultConnection" connectionString="Data Source=[YOUR-VM-DNS-NAME].cloudapp.net, 57500;initial catalog=AdventureWorksLT2008R2;Uid=CloudShop;Password=Azure$123;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" />
		<add name="AdventureWorksEntities" connectionString="metadata=res://*/Models.AdventureWorks.csdl|res://*/Models.AdventureWorks.ssdl|res://*/Models.AdventureWorks.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source= [YOUR-VM-DNS-NAME].cloudapp.net, 57500;initial catalog=AdventureWorksLT2008R2;Uid=CloudShop;Password=Azure$123;multipleactiveresultsets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
	</connectionStrings>
	````

1. Compile and run the solution.

	> **Note**: Make sure that the CloudShop.Azure project is set as the StartUp project before running the application.

1. In the home page, the Products list will be filled with the data from the SQL Server instance. Perform a search by entering any text and clicking the **Search** link. The products list will be filtered.

 	![Verifying that the sample application is connected to the SQL Server instance](./Images/Verifying-that-the-sample-application-is-connected-to-the-SQL-Server-instance.png?raw=true "Verifying that the sample application is connected to the SQL Server instance")
 
	_Verifying that the sample application is connected to the SQL Server instance_

1. Close the browser and go back to **Visual Studio**.

<a name="Ex2Task2"></a>
#### Task 2 - Publishing the MVC4 Application to Microsoft Azure ####

In this task you will download your account's publish settings and publish the Web Application from Visual Studio.

1. Open Internet Explorer and go to <https://windows.azure.com/download/publishprofile.aspx>.

1. **Save** the publish-settings file to your local machine.

	![downloading-publish-settings-file](Images/downloading-publish-settings-file.png?raw=true "downloading-publish-settings-file")

	_Downloading publish-settings file_

1. Switch to Visual Studio. Right-click the **CloudShop.Azure** project and select **Publish**.

 	![Publishing the Cloud Application](./Images/Publishing-the-Cloud-Application.png?raw=true "Publishing the Cloud Application")
 
	_Publishing the Cloud Application_

1. In the **Publish Microsoft Azure Application** wizard, click the **Import** button and select the publish settings file you have just downloaded. Make sure is selected in the drop-down list and click **Next**.

 	![Importing the Publishing Settings](./Images/Importing-the-Publishing-Settings.png?raw=true "Importing the Publishing Settings")
 
	_Importing the Publishing Settings_

1. In the **Microsoft Azure Publish Settings** page, expand the **Cloud Service** drop-down list and select **Create New**. In the dialog, enter a name for the cloud service and select a location. Click **OK** to continue.

 	![Creating a New Cloud Service](./Images/Creating-a-New-Cloud-Service.png?raw=true "Creating a New Cloud Service")
 
	_Creating a New Cloud Service_

1. Make sure the **Environment** is set to _Production_, the **Build configuration** is set to _Release_ and the **Service configuration** is set to _Cloud._ Click **Next**.

1. In the **Microsoft Azure Publish Summary** page, click **Publish**. Wait until the deployment is completed. Click the **Website URL** link.

 	![Deployment Activity Log](./Images/Deployment-Activity-Log.png?raw=true "Deployment Activity Log")
 
	_Deployment Activity Log_

1. The browser will show you the home page of the **Cloud Shop** sample application. In the **Search** box, write _Classic_ and click **Search**. It will show all the products that have _ProductName_ or _QuantityPerUnit_ fields that match the search criteria. The Cloud App is accessing the SQL Server using the public endpoint to retrieve the list of products.

 	![Searching Products](./Images/Searching-Products.png?raw=true "Searching Products")
 
	_Searching Products_

---

<a name="Summary"></a>
## Summary ##

By completing this hands-on lab, you have learnt how to:

- Configure a SQL Server Virtual Machine
- Connect a sample Web application with the SQL Server using a public endpoint
- Deploy a sample Web application to a Cloud App in Microsoft Azure
