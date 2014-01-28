<a name="HOLTop"></a>
# Introduction to Windows Azure Virtual Machines #

---

<a name="Overview"></a>
## Overview ##

Using Windows Azure as your Infrastructure as a Service (IaaS) platform, will enable you to create and manage your infrastructure quickly, provisioning and accessing any host ubiquitously. Grow your business through the cloud-based infrastructure, reducing the costs of licensing, provisioning and backup.

In this hands-on Lab, you will learn how to deploy a simple ASP.NET MVC 4 Web application to a Web server hosted in Windows Azure, using SQL Server and configuring load balancing.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a Web Farm using Windows Azure Management Portal
- Configure Load Balancing in IIS
- Deploy a Simple MVC4 Application that consumes SQL Server Features
- Create a Virtual Machine with SQL Server Full-Text Search feature to be consumed by the MVC Application

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating Virtual Machines for IIS](#Exercise1)
1. [Creating a SQL Server Virtual Machine](#Exercise2)
1. [Deploying a Simple MVC4 Application](#Exercise3)

Estimated time to complete this lab: **45 minutes**.

<a name="Exercise1"></a>
### Exercise 1: Creating Virtual Machines for IIS ###

In this exercise, you will learn how to create a Virtual Machine in Windows Azure. Then, you will configure an Internet Information Server adding roles to use later on in this lab.

<a name="Ex1Task1"></a>
#### Task 1 - Creating IIS Virtual Machines ####

In this task, you will provision a Virtual Machine and configure the Load Balancing to host an MVC4 application.

1. Open Internet Explorer and browse to [https://manage.windowsazure.com/](https://manage.windowsazure.com/) to enter the Windows Azure portal. Then, log in with your credentials.

1. In the menu located at the bottom, select **New | Compute | Virtual Machine | From Gallery** to start creating a new virtual machine.
	 
	![Creating a new Virtual Machine](Images/creating-a-new-virtual-machine.png?raw=true)

	_Creating a new Virtual Machine_
 
1. In the **Virtual Machine OS Selection** page, click **Platform Images** on the left menu and select the **Windows Server 2008 R2 SP1** OS image from the list. Click the arrow to continue.	

1. In the **Virtual Machine Configuration** page, leave the version release date by default (latest). Enter the Virtual Machine Name (i.e. "iisvm1"), provide a user name for the **New User Name** field and a password for the **New Password** and **Confirm Password** fields. This password needs to contain three of these – lower case characters, uppercase characters, numbers and special characters. Make sure you remember your choice. Lastly, set the Virtual Machine **Size** to _Small_ and click **Next** to continue.

	![Configuring a Custom Virtual Machine](Images/creating-a-vm-configuration.png?raw=true)
	 
	_Creating a Virtual Machine - Configuration_
 
	>**Note:** It is suggested to use secure passwords for admin users, as Windows Azure virtual machines could be accessible from the Internet knowing just their DNS.
	>
	>You can also read this document on the Microsoft Security website that will help you select a secure password:  [http://www.microsoft.com/security/online-privacy/passwords-create.aspx](http://www.microsoft.com/security/online-privacy/passwords-create.aspx)
 
1. In the **Virtual Machine Mode** page, select **Standalone Virtual Machine**, enter the **DNS Name**, select a **Storage Account** or leave the default value _Use Automatically Generated Storage Account_, and select a **Region/Affinity Group/Virtual Network**. Click the **right arrow** to continue. 

	![Configuring a Custom Virtual Machine, Virtual Machine Mode](Images/creating-a-vm-vm-mode.png?raw=true)
 
	_Creating a Virtual Machine - Virtual Machine Mode_

1. In the **Virtual Machine Options** page, leave the default values and click the **Finish** button to create a new Virtual Machine.

	![Creating a Virtual Machine - Virtual Machine Options](Images/creating-a-vm--vm-options.png?raw=true "Creating a Virtual Machine - Virtual Machine Options")

	_Creating a Virtual Machine - Virtual Machine Options_

1. In the **Virtual Machines** section, you will see the Virtual Machine you created with a _Starting (provisioning)_ status. Wait until it changes to _Running_ in order to continue with the following step as you will need a provisioned Virtual Machine on the following steps.

 	![Creating Virtual Machine for IIS Web Farm](./Images/creating-vm-for-iis-web-farm.png?raw=true "Creating Virtual Machine for IIS Web Farm")
 
	_Creating Virtual Machine for IIS Web Farm_

	> **Note:** It will take from 8 to 10 minutes for the Virtual Machine to complete the provisioning process.

1. You will now add the second Virtual Machine for the IIS Load Balancing. In the portal, select **New | Compute | Virtual Machine | From Gallery**. 

1. In the **Virtual Machine OS Selection** page, click **Platform Images** on the left menu and select the **Windows Server 2008 R2 SP1, February** OS image from the list. Click the **arrow** to continue.	

1. In the **Virtual Machine Configuration** page, set the version release date to **February 12, 2013**. Enter the **Virtual Machine Name** (i.e. "_iisvm2_"), a **User Name**, a **Password** and the **Size**. Click the **right arrow** to continue.
 
1. In the **Virtual Machine Mode** page, select **Connect to existing Virtual Machine** and choose the first virtual machine you created from the drop down list. Select a **Storage Account** or leave the default value _Use Automatically Generated Storage Account_ and click the **right arrow** to continue. This step adds the new virtual machine to the cloud service created in the previous step. This allows the virtual machines to be on the same network.


	![Configuring a Custom Virtual Machine, Virtual Machine Mode](Images/creating-a-vm-vm-mode2.png?raw=true)
	 
	_Creating a Virtual Machine - Virtual Machine Mode_

 
1. In the **Virtual Machine Options** page, leave the default values and click the button to create a new Virtual Machine.

1. Wait until the second Virtual Machine is created. You can check the Virtual Machine status from the Virtual Machines section within the portal.

	> **Note:** It will take from 8 to 10 minutes for the virtual machine to complete the provisioning process.

1. After creating the second virtual machine, you will create an endpoint in the port 80 in the Virtual Machine you created first. To do this, click on the first Virtual Machine Name (_iisvm1_) to go to the **Dashboard** page and then click **Endpoints**. Click **Add Endpoint** on the bottom pane. 

	![Selecting Add Endpoint in the dashboard](Images/adding-a-new-endpoint-dashboard.png?raw=true "Selecting Add Endpoint in the dashboard")

	_Selecting Add Endpoint in the dashboard_

1. Make sure that **Add Endpoint** option is selected and then click the **right arrow** button to continue.

	![Adding a new Endpoint](Images/adding-a-new-endpoint.png?raw=true "Adding a new Endpoint")

	_Adding a new Endpoint_

1. In the **Specify endpoint details** page, set the **Name** to _webport_, the **Protocol** to _TCP_ and the **Public Port** and **Private Port** to _80_. Click the button to create the endpoint. Wait until the Endpoint is created before continue to the following steps.

	![New Endpoint Details](Images/new-endpoint-details.png?raw=true "New Endpoint Details")

	_New Endpoint Details_

	> **Note:** It will take some minutes to create a new endpoint.

1. Now, create a new Endpoint in the second Virtual Machine in order to enable Load Balancing between both Virtual Machines. To do this, click **Virtual Machines** and then select the second Virtual Machine you created. Then, click **Endpoints**.

1. Click **Add Endpoint**, select **Load Balance Traffic On An Existing Endpoint** option. Select the endpoint you created for the first Virtual Machine from the drop down list and then click the **right arrow** to continue.

	![Load Balance Traffic On An Existing Endpoint](Images/load-balance-traffic-an-an-existing-endpoint.png?raw=true "Load Balance Traffic An Existing Endpoint")

	_Load Balance Traffic On An Existing Endpoint_

1. In the **New Endpoint Details** page, set the **Name** to _webport_ and the **Private Port** to _80_. Click the button to create the endpoint. 

	> **Note:** It will take some minutes to create a new endpoint.

1. In the **Virtual Machines** section, click on the first Virtual Machine Name (_iisvm1_) and then click **Endpoints**. 

1. Select the **webport** endpoint you have created. Make sure the **Load Balancer** column value is **Yes**. 

	![Verification: enabling IIS Load Balancing](Images/creating-load-balancing-endpoint-1.png?raw=true)

	_Verification: enabling IIS Load Balancing_

1. Click the **Edit Endpoint** button in the bottom bar to enter the endpoint details and verify the load balancing is enabled. Repeat this step in the second Virtual Machine.

	![Verification: enabling IIS Load Balancing, details](Images/creating-load-balancing-endpoint-2.png?raw=true)

	_Verification: enabling IIS Load Balancing, details_

<a name="Ex1Task2"></a>
#### Task 2 - Configuring IIS Virtual Machines ####

In this task, you will configure the IIS Virtual Machines by adding the necessary roles to deploy the MVC application.

1. In the Portal, click **Virtual Machines** on the left menu.

1. You will see a list with your existing Virtual Machines. Select the first one you created in Task 1 and click the **Connect** button in the bottom bar. If you used the proposed name, this Virtual Machine should be named **iisvm1**.

1. You will be asked to download the remote desktop settings file. Click **Open** and log on using the credentials you defined when creating the Virtual Machine.

1. In the Azure Virtual Machine, open **Server Manager** from **Start | Administrative Tools**.

1. In the **Server Manager** window, select **Roles** node.

 	![Server Manager](./Images/Server-Manager.png?raw=true "Server Manager")
 
	_Server Manager_

1. Click **Add Roles** link.

 	![Adding Server Roles](./Images/Adding-Server-Roles.png?raw=true "Adding Server Roles")
 
	_Adding Server Roles_

1. The **Add Roles Wizard** will appear.

1.  In the **Before You Begin** page, read the content and click **Next**.

1. In the **Select Server Roles** page, check the **Application Server** and **Web Server (IIS)**. A warning will show, informing the Required Role Services that are missing. Click **Add Required Features** to install them and then click **Next**.

	![Add Roles Wizard(2)](Images/add-roles-wizard2.png?raw=true)

	_Add Roles Wizard_

1. The **Application Server** page provides a brief introduction about Application Server's capabilities. Click **Next** when you complete reading it.

1. In the **Select Role Services** page for **Application Server**, select **Web Server (IIS)** **Support** and make sure **.NET Framework 3.5.1** is selected. It will prompt a dialog warning about missing Required Role Services. Click **Add Required Role Services** to install them and then click **Next**

	![Add Roles Wizard(3)](Images/add-roles-wizard3.png?raw=true)

	_Add Roles Wizard_

1. The **Web Server (IIS)** page provides a brief introduction about Web Server (IIS) capabilities. Click **Next** when you complete reading.

1. The **Select Role Services** page for **Web Server (IIS)** page will display the selected role services that will be installed. Click **Next**.

	![Add Roles Wizard(4)](Images/add-roles-wizard4.png?raw=true)

	_Add Roles Wizard_

1. In the **Confirm Installation Selections** page, make sure the displayed services that will be installed are the ones you have selected (.NET Framework 3.5.1 support and IIS), and then click **Install**.

	![Add Roles Wizard](./Images/Add-Roles-Wizard.png?raw=true "Add Roles Wizard")

	_Add Roles Wizard_

	> **Note:** It will take some minutes to complete the installation.

1. Close the **Remote Desktop Connection**.

	Repeat this task on the second Virtual Machine to install IIS, starting from step 4. If you used the proposed name, the second Virtual Machine should be named **iisvm2**.

<a name="Exercise2"></a>
### Exercise 2: Creating a SQL Server Virtual Machine ###

In this exercise, you will create a new Virtual Machine and learn how to install SQL Server. You will add disk images to the existing Virtual Machine in order to split the data from the logs generated by SQL Server.

<a name="Ex2Task1"></a>
#### Task 1 - Creating a SQL Server Virtual Machine ####

In this task, you will create a new Virtual Machine using the Windows Azure portal in the same Cloud App you deployed the IIS Virtual Machines.

1. In the menu located at the bottom, select **New | Compute | Virtual Machine | From Gallery** to start creating a new virtual machine.
 
1. In the **Virtual Machine OS Selection** page, click **Platform Images** on the left menu and select the **Microsoft SQL Server 2012** image from the list. Click the arrow to continue.	

1. In the **Virtual Machine Configuration** page, enter the **Virtual Machine Name** (i.e. "_sqlvm1_"), a **User Name**, a **Password** and the **Size**. Click the **right arrow** to continue.
 
1. In the **Virtual Machine Mode** page, select **Connect to existing Virtual Machine** and choose the first IIS Virtual Machine you created from the drop down list (_iisvm1_). Click the **right arrow** to continue.
 
1. In the **Virtual Machine Options** page, leave the default values and click the button to create a new Virtual Machine.

1. In the **Virtual Machines** section, you will see the Virtual Machine you created with a _provisioning_ status. Wait until it changes to _Running_ in order to continue with the following step.

	> **Note:** It will take from 8 to 10 minutes for the Virtual Machine to complete the provisioning process.

<a name="Ex2Task2"></a> 
#### Task 2 - Attaching Empty Disk Images ####

In this task, you will create two empty data disks and attach them to an existing Virtual Machine using the Windows Azure Management Portal. You will use these data disks to split SQL Server Data and Logs.

1. Now, you will create and attach empty data disks to store the SQL Server logs and data files, and you will also add an endpoint. To do this, in the **Virtual Machines** section, select the SQL Server Virtual Machine you created in the previous task.

1. In the Virtual Machine's **Dashboard**, click **Attach** in the menu at the bottom of the page and select **Attach Empty Disk**.

	![Attach Empty Disk](Images/attach-empty-disk.png?raw=true "Attach Empty Disk")

	_Attach Empty Disk_

1. In the **Attach Empty Disk** page, set the **Size** to _50_ GB and create the Disk.

1. Wait until the process to attach the disk finishes. Repeat the steps 1 to 3 to create a second disk.

1. Open the Virtual Machine's **Dashboard**. You will see three disks: one for the **OS** and other two for **Data** and **Logs**.

	> **Note:** It might take a few minutes until the data disks appear in the Virtual Machine's dashboard within the Azure Portal.

 	![Attached Data Disks](./Images/Attached-Data-Disks.png?raw=true "Attached Data Disks")
 
	_Attached Data Disks_

1. Finally, you need to format the disks in order to access them from the Virtual Machine. To do this, click **Connect** to connect to the Virtual Machine using **Remote Desktop connection**.

1. It will ask you to download the remote desktop settings file. Click **Open** and log on using the credentials you defined when creating the Virtual Machine.

1. In the virtual machine, open **Server Manager** from **Start | Administrative Tools**.

1. Expand **Storage** node and select **Disk Management** option.

 	![Disk Management(2)](Images/disk-management2.png?raw=true)
 
	_Disks Management_

1. The **Initialize Disk** dialog will appear. Leave the default values and click **OK**.

1. Right-click the first disk unallocated space and select **New Simple Volume**.

 	![Disk Management](Images/disk-management.png?raw=true)
 
	_Disks Management_

1. Follow the **New Simple Volume Wizard**. When asked for the **Volume Label** use _SQLData_.

1. Wait until the process for the first disk is completed. Repeat the steps 11 to 12 but this time using the second disk. Set the **Volume Label** to _SQLLogs_.

1. The **Disk Management** list of available disks should now show the **SQLData** and **SQLLogs** disks like in the following figure:

 	![Disks Management](./Images/Disks-Management.png?raw=true "Disks Management")
 
	_Disks Management_

	> **Note:** Do not close the **Remote Desktop Connection**. You will use it in the following task.


<a name="Ex2Task3"></a> 
#### Task 3 - Configuring SQL Server in the Virtual Machine ####

In this task, you will configure SQL Server 2012. You will create the database that will be used by the MVC4 application and add Full-Text Search capabilities to it. Additionally, you will create a SQL Server user for the MVC4 website.

1. Open Windows Explorer and create the following folders: **F:\Data, G:\Logs** and **G:\Backups**.

1. Open the SQL Server Management Studio from **Start | All Programs | Microsoft SQL Server 2012 | SQL Server Management Studio**.

1. Connect to the SQL Server 2012 default instance using your Windows Account.

1. Now, you will update the database's default locations in order to split the DATA from the LOGS. To do this, right click on your SQL Server instance and select **Properties**.

1. Select **Database Settings** from the left side pane.

1. Locate the **Database default locations** section and update the default values to point to the disks you attached in the previous task and then click "Ok".

 	![Setting Database Default Locations](./Images/Setting-Database-Default-Locations.png?raw=true "Setting Database Default Locations")
 
	_Setting Database Default Locations_

1. Restart SQL Server. In the **Object Explorer**, right-click on the server node and select **Restart**. 

1.	In order to enable downloads from Internet Explorer you will need to update **Internet Explorer Enhanced Security Configuration**. In the Azure Virtual Machine, open **Server Manager** from **Start | Administrative Tools | Server Manager**.

1. In the **Server Manager**, click **Configure IE ESC** within **Security Information** section.

 	![Configuring IE ESC](./Images/configuring-internet-explorer-enhanced-security-configuration.png?raw=true "Configuring IE ESC")
 
	_Configuring IE ESC_

1. In the **Internet explorer Enhanced Security** configuration, turn **off** the enhanced security for **Administrators** and click **OK**.

	![Internet Explorer Enhanced Security(2)](Images/internet-explorer-enhanced-security2.png?raw=true)
	 
	_Internet Explorer Enhanced Security_
 
	>**Note:** Modifying **Internet Explorer Enhanced Security** configurations is not good practice and is only for the purpose of this particular lab. The correct approach should be to download the files locally and then copy them to a shared folder or directly to the Virtual Machine.

1. This lab uses the **AdventureWorks2012** database. Open an **Internet Explorer** browser and go to <http://msftdbprodsamples.codeplex.com/> to download  the **SQL Server 2012** sample databases. Once on the page click on **AdventureWorks Databases – 2008, 2008R2 and 2012** and then download Adventure Works 2012 Data File. Download the file to F:\Data.

	>**Note:** The **AdventureWorks2012** database can also be downloaded as a .zip file. If you choose this format, right-click the file to open its properties window and then click **Unblock**. Then, extract the database to F:\Data.

1. Add the **AdventureWorks2012** sample database to your SQL Server. To do this, in the **SQL Server Management Studio**, locate your SQL Server instance node and expand it. Right click the **Databases** folder and select **Attach**.

	![Attaching the database](Images/attaching-adventureworks-database-menu.png?raw=true)

	_Attaching the database_

1. In the **Attach Databases** dialog, press **Add**. Browse to the data disk and select the Adventure Works 2012 data file.

1. Select the **AdventureWorks2012** Log entry and click **Remove**.

 	![Removing AdventureWorks2012 Log entry](Images/removing-adventureworks-log-entry.png?raw=true)
 
	_Removing AdventureWorks2012 Log entry_

1. Press **OK** to add the database.

1. In the **Databases** folder, locate the new **AdventureWorks2012** database and explore its tables.

 	![AdventureWorks Sample Database](./Images/adventureworks-sample-database.png?raw=true "Northwind Sample Database")
 
	_AdventureWorks Sample Database_

1. Expand **Storage** node within **AdventureWorks2012** database, right-click **Full Text Catalogs** folder and select **New Full-Text Catalog**.

	> **Note:** You are creating a Full Text Catalog for the database that will be used later by the MVC application. 

 	![Create New Full-Text Catalog(2)](Images/create-new-full-text-catalog2.png?raw=true)
 
	_Create New Full-Text Catalog_

1. In the New Full-Text Catalog dialog, set the **Name** value to _AdventureWorksCatalog_ and press **OK**.

 	![Create New Full-Text Catalog(3)](Images/create-new-full-text-catalog3.png?raw=true)
 
	_Create New Full-Text Catalog_

1. Check that the Full-Text Catalog you created appears in the **Full-Text Catalogs** folder.

 	![Create New Full-Text Catalog(4)](Images/create-new-full-text-catalog5.png?raw=true)
 
	_Create New Full-Text Catalog_

1. Right-click **AdventureWorksCatalog** and select **Properties**. In the **Full-Text Catalog Properties** dialog, switch to **Tables/Views** page.

1. Add the **Production.Product** table to the **Table/View objects assigned to the Catalog** list. Then, check the _Name_ column and click **OK**.

 	![Create New Full-Text Catalog(5)](Images/create-new-full-text-catalog4.png?raw=true)
 
	_Create New Full-Text Catalog_

1. Add a new user for the MVC4 application you will deploy in the following exercise. To do this, expand **Security** folder within the SQL Server instance. Right-click **Logins** folder and select **New Login**.

 	![Creating a New Login(2)](Images/creating-a-new-login2.png?raw=true)
 
	_Creating a New Login_

1. In the **General** section, set the **Login name** to _CloudShop._ Select **SQL Server authentication** option and set the **Password** to _Azure$123_.

	> **Note:** If you enter a different username or password than those suggested in this step, do not forget in the next exercise to update the web.config file of the MVC4 application to match those values.

1. Unselect **Enforce password policy** checkbox to avoid having to change the password the first time you log on, and set the **Default database** to _AdventureWorks2012_.

 	![Creating a New Login](./Images/Creating-a-New-Login.png?raw=true "Creating a New Login")
 
	_Creating a New Login_

1. Click **User Mapping** on the left pane. Select the map checkbox in the _AdventureWorks2012_ database row and click **OK**.

 	![Mapping the new User to the AdventureWorks Database](./Images/mapping-new-user-database-2.png?raw=true "Mapping the new User to the AdventureWorks Database")
 
	_Mapping the new User to the AdventureWorks Database_

1. Expand **AdventureWorks2012** database within **Databases** folder. In the **Users** folder under **Security**, double-click **CloudShop** user.

1. Select the **Membership** page, and select the _db_owner_ role checkbox for the **CloudShop** user and click **OK**.

 	![Adding Database role membership to CloudShop user](./Images/Adding-Database-role-membership-to-CloudShop-user.png?raw=true "Adding Database role membership to CloudShop user")
 
	_Adding Database role membership to CloudShop user_

	> **Note:** The application you will deploy in the next exercise uses Universal Providers to manage sessions. The first time the application runs, the provider will create the Sessions table within the  database. For that reason, you are assigning a db_owner role to the CloudShop user. Once you run the application for the first time, you can remove this role as these permissions will not be needed.

1. Now, enable **Mixed Mode Authentication** to the SQL Server instance. To do this, in the **SQL Server Management Studio**, right-click the server instance and click **Properties**.

1. Click **Security** in the left side pane and then select **SQL Server and Windows Authentication mode** under **Server Authentication** section. Click **OK** to save changes.

1. Restart the SQL Server instance. To do this, right-click the SQL Server instance and click **Restart**.

1. Close the **SQL Server Management Studio**.

1. In order to allow the MVC4 application access the SQL Server database you will need to add an **Inbound Rule** for the SQL Server requests in the **Windows Firewall**. To do this, open **Windows Firewall with Advanced Security** from **Start | Administrative Tools**.

1. Select **Inbound Rules** node, right-click it and select **New Rule** to open the **New Inbound Rule Wizard**.

 	![Creating an Inbound Rule](./Images/Creating-an-Inbound-Rule.png?raw=true "Creating an Inbound Rule")

	_Creating an Inbound Rule_

1. In the **Rule Type** page, select **Port** and click **Next**.

	![New Inbound Rule Wizard](Images/new-inbound-rule-wizard2.png?raw=true)
 
	_New Inbound Rule Wizard_

1. In **Protocol and Ports** page, leave TCP selected, select **Specific local ports,** and set its  value to _1433_. Click **Next** to continue.

 	![New Inbound Rule Wizard](Images/new-inbound-rule-wizard.png?raw=true)
 
	_New Inbound Rule Wizard_

1. In the **Action** page, make sure that **Allow the connection** is selected and click **Next**.

 	![Protocol and Ports(3)](Images/new-inbound-rule-wizard3.png?raw=true)
 
	_Protocol and Ports_

1. In the **Profile** page, leave the default values and click **Next**.

1. In the **Name** page, set the Inbound Rule's **Name** to _SQLServerRule_ and click **Finish**

 	![New Inbound Rule Wizard(4)](Images/new-inbound-rule-wizard4.png?raw=true)
 
	_New Inbound Rule Wizard_

1. Close **Windows Firewall with Advanced Security** window.

	> **Note:** Make sure the Named Pipes and TCP/IP protocols are enabled for the Server Instance. You can verify this by going to the SQL Server Configuration Manager and within SQL Server Network Configuration node check that these protocols' status are set to enable.
	>
	> Remember to restart the SQL Server instance after enabling a protocol.

1. Close the **Remote Desktop Connection**.

<a name="Exercise3"></a> 
### Exercise 3: Deploying a Simple MVC4 Application ###

In this exercise, you will learn how to deploy a simple ASP.NET MVC4 application in the IIS of the Azure Virtual Machine you have previously configured.

>**Note:** To make this solution highly available, you need to configure the SQL Servers in an availability set and set up SQL Server Mirroring between the instances.

<a name="Ex3Task1"></a>
#### Task 1 - Deploying a Simple MVC4 Application ####

In this task, you will deploy the MVC4 application to the IIS Virtual Machines.

1. In the Azure Portal, Click **Virtual Machines** on the left menu.

1. You will see a list with your existing Virtual Machines. Select the first one you created in Exercise 1 and click **Connect**. If you used the proposed name, this Virtual Machine's should be named **iisvm1**.

1. You will be prompted to download the remote desktop client. Click **Open** and log on using the Admin credentials you defined when creating the Virtual Machine.

1. You need to install **.NET Framework 4.0** before deploying the MVC4 application. In order to do that, you will enable downloads from IE update **Internet Explorer Enhanced Security Configuration**.

	1. In the Azure Virtual Machine, open Server Manager from **Start | Administrative Tools**.

	1. In the **Server Manager**, click **Configure IE ESC** **within Security Information** **section**.

 		![Internet Explorer Enhanced Security(3)](Images/internet-explorer-enhanced-security3.png?raw=true)
 
		_Internet Explorer Enhanced Security_

	1. In the **Internet explorer Enhanced Security** dialog, turn **off** enhanced security for **Administrators** and click **OK**.

 		![Internet Explorer Enhanced Security](./Images/Internet-Explorer-Enhanced-Security.png?raw=true "Internet Explorer Enhanced Security")
 
		_Internet Explorer Enhanced Security_

		> **Note:** Modifying Internet Explorer Enhanced Security configurations is not a good practice and it only for the purpose of this particular lab. The correct approach would be to download the files locally and then copy them to a shared folder or directly to the Virtual Machine.

1. Now that you have permissions to download files, open an **Internet Explorer** browser session and navigate to [http://go.microsoft.com/fwlink/?linkid=186916](http://go.microsoft.com/fwlink/?linkid=186916). Download and install **.NET Framework 4.0**.

1. Once **.Net Framework 4.0** installation finishes, open **wwwroot** folder located at **C:\inetpub\** and copy the file **CloudShop.zip** located in **Source\Assets\CloudShop** folder of this lab. To do this, copy **CloudShop.zip** (**Ctrl + C**) and paste it (**Ctrl + V**) in the Virtual Machine's **wwwroot** folder. Extract all files to **C:\inetpub\wwwroot\CloudShop** folder.

 	![wwwroot folder](./Images/wwwroot-folder.png?raw=true "wwwroot folder")
 
	_wwwroot folder_

1. Open with **Notepad** the **Web.config** file located in **C:\inetpub\wwwroot\CloudShop**. Replace the connection strings placeholder with the name of your SQL Server (by default, is the Virtual Machine's name).

	<!--mark: 1-4-->
	````XML
	<connectionStrings>
	    <add name="AdventureWorksEntities" connectionString="metadata=res://*/Models.AdventureWorks.csdl|res://*/Models.AdventureWorks.ssdl|res://*/Models.AdventureWorks.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=[ENTER YOUR SQL SERVER NAME];initial catalog=AdventureWorks2012;Uid=CloudShop;Password=Azure$123;multipleactiveresultsets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
	    <add name="DefaultConnection" connectionString="Data Source=[ENTER YOUR SQL SERVER NAME];initial catalog=AdventureWorks2012;Uid=CloudShop;Password=Azure$123;MultipleActiveResultSets=True" providerName="System.Data.SqlClient" />
	</connectionStrings>
	````

1. Open the **Internet Information Services (IIS) Manager** from **Start | Administrative Tools**.

1. In the **Connections** pane, expand **Default Web Site** within your IIS Server's node. You will see the **CloudShop** folder you copied in the **wwwroot** folder.

 	![IIS Manager](./Images/IIS-Manager.png?raw=true "IIS Manager")
 
	_IIS Manager_

1. Right-click **CloudShop** folder and select **Convert to Application**.

 	![IIS Manager Convert to Application](Images/iis-manager-convert-to-application.png?raw=true)
 
	_IIS Manager - Convert to Application_

1. In the **Add Application** dialog, click **OK**.

 	![Add Application dialog](./Images/Add-Application-dialog.png?raw=true "Add Application dialog")
 
	_Add Application dialog_

1. Finally, select the **Application Pools** node and double-click **DefaultAppPool** application pool.

 	![Updating Default Application Pool](./Images/Updating-Default-Application-Pool.png?raw=true "Updating Default Application Pool")
 
	_Updating Default Application Pool_

1. In the **Edit Application Pool** dialog, change the **.Net Framework** version to **v4.0** and click **OK**.

 	![Editing Application Pool](./Images/Editing-Application-Pool.png?raw=true "Editing Application Pool")
 
	_Editing Application Pool_

1. Now, the **DefaultAppPool's** .Net Framework should be **v4.0** instead of v2.0.

 	![Updating Default Application Pool](./Images/Updating-Default-Application-Pool-Updated.png?raw=true "Updating Default Application Pool")
 
	_Updating Default Application Pool_

1. Close the **Internet Information Server (IIS) Manager** window.

1. Close the **Remote Desktop Connection**.

1. Repeat this task in the second Virtual Machine you created in **Exercise 1 -Task 1**. If you used the proposed name, this Virtual Machine should be named **iisvm2**.

<a name="Verification"></a> 
#### Verification ####

In this task, you will test the Cloud Shop MVC4 application you deployed in the previous task.

1. In your local machine, open **Internet Explorer**.

1. Go to http://[**YOUR-SERVICE-NAME**].cloudapp.net/CloudShop. The Service Name is the one you used when creating the IIS Virtual Machines (you can also check it in the Azure Portal, within Virtual Machine's dashboard).

 	![MVC4 Application running in the Web Farm](./Images/MVC4-Application-running-in-the-Web-Farm.png?raw=true "MVC4 Application running in the Web Farm")
 
	_MVC4 Application running in the Web Farm_

1. In the **Search** box, write _Classic_ and click **Search**. It will show all the products that have a product name that match the search criteria.

 	![Searching Products(2)](Images/searching-products2.png?raw=true)
 
	_Searching Products_

--- 

<a name="summary" />
## Summary ##

In this hands-on Lab, you have learnt how to deploy a simple ASP.NET MVC 4 Web application to a Web server hosted in Windows Azure, using SQL Server and configuring load balancing.