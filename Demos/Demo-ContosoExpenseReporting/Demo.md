<a name="title" />
# Contoso Expense Reporting Demo #

---

<a name="Overview" />
## Overview ##

This demo illustrates an end-to-end scenario highlighting the Microsoft Azure cloud-ready data services, including Microsoft Azure SQL Database, Microsoft Azure Storage, and SQL Server 2012 in a Microsoft Azure Virtual Machine. This demo will showcase the rich and powerful features of the Microsoft Azure data services including easy provisioning of the services, fluid migration options, and the high-performing, scalable and feature-rich data options.

<a id="goals" />
### Goals ###
In this demo, you will see the following:

1. Contoso Expense Reporting application running on-premise.

1. Provisioning a virtual machine in Microsoft Azure to run SQL Server 2012.

1. Deploying the application to a Microsoft Azure Web Site.

1. Connecting the application to a Microsoft Azure SQL Database.

1. Connecting the application to use Microsoft Azure Storage.

1. Creating SQL Database Federations.

<a name="setup" />
### Setup and Configuration ###

In order to execute this demo, you need to set up your environment.

1. Open Windows Explorer and browse to the demo's **Source** folder.

1. Execute **Setup.Local.cmd** with Administrator privileges to launch the setup process that will verify all the prerequisites and install the Visual Studio code snippets for this demo.

1. If the User Account Control dialog is shown, confirm the action to proceed.

>**Note 1:** Make sure you have all the dependencies for this demo checked before proceeding.

> **Note 2:** The setup script copies the source code for this demo to a working folder that can be configured in the **Config.Local.xml** file (by default, C:\Projects). From now on, this folder will be referenced in this document as the **working folder**.

---

<a name="Demo" />
## Demo ##

Throughout the course of this session, we will show the power and flexibility of Microsoft Azure by taking an on-premises application and associated database, and migrate them to Microsoft Azure. We will first use our IaaS services to migrate the database to a virtual machine with SQL Server 2012 and the application to a Windows  Azure Cloud Service. We will then highlight our PaaS services by migrating the database to SQL Azure and extend the functionality of the application to use our Microsoft Azure storage services.

This demo contains the following segments:

1. [Contoso Expense Reporting Application Overview](#segment1).
1. [Migrating to Microsoft Azure with Virtual Machines & SQL Server 2012](#segment2).
1. [Microsoft Azure SQL Database](#segment3). 
1. [Microsoft Azure SQL Federation](#segment4). 
1. [Microsoft Azure Storage](#segment5). 

<a name="segment1" />
### Contoso Expense Reporting On-Premises ###

1. In Visual Studio, open the **Expenses.Web.sln** solution located in the working folder of this demo.
1. Press **F5** to launch the application in the Development Server.

	![Expense reporting application running locally](Images/expense-reporting-app-running-locally.png?raw=true "Expense reporting application running locally")

	_Expense reporting application running locally (on-premises)_

	>**Speaking Point**
	>
	> We’re first going to log in as a user and submit a simple expense report.
	>
	> As part of this, we’re going to highlight some of the challenges the company has today with the system, including the challenge of attaching receipts and verifying expenses.

1. Click **Login as user**.

	![Log in as User](Images/log-in-as-user.png?raw=true "Log in as User")

	_Log in as a regular user_

	>**Speaking Point**
	>
	> We’re first going to log in as a user to illustrate some of the pain points we have with this application in submitting expense reports and highlight some of the areas where this application can be expanded and improved.

1. Select **My Reports** in the toolbar. 

	![Expense reporting application dashboard](Images/expense-reporting-app-dashboard.png?raw=true "Expense reporting application dashboard")

	_Expense reporting application dashboard_

1. In the **My Reports** view, click **Add New Report**.

	![Application Dashboard](Images/application-dashboard.png?raw=true "Application Dashboard")

	_Viewing the list of expense reports_

	>**Speaking Point**
	>
	> In my Dashboard view, I can see expenses I have submitted and are awaiting approval from my manager, as well as those that have been approved and rejected.

1.	Enter the following information:
	- Name
	- Purpose


1. Next, click **Add New Expense** and fill in the following fields:
	- Category
	- Description
	- Merchant
	- Billed Amount
	- Transaction Amount

	![Expense Reporting - New Report](Images/expense-reporting-new-report.png?raw=true "Expense Reporting - New Report")

	_Adding a new report_

	>**Speaking Point**
	>
	> Highlight the fact that because of limitations on-premises, receipts are still submitted manually and are not physically associated or attached with the Expense report.

1. Click **Save Draft**.

	![Expense Reporting - Save Draft](Images/expense-reporting-save-draft.png?raw=true "Expense Reporting - Save Draft")

	_Saving a draft report_

	>**Speaking Point**
	>
	> We are going to save this expense report as a draft because we will come back to it several times during this demo.

1. Show **Visual Studio**. 
1. Open the **Solution Explorer** window.

	![Application's Solution Explorer](Images/applications-solution-explorer.png?raw=true "Application's Solution Explorer")

	_The Expense Report application in Solution Explorer_

	>**Speaking Point**
	>
	> Standard MVC web application. Common to most of you. Most have running in enterprise.

<a name="segment2" />
### Migrating to Microsoft Azure with Virtual Machines & SQL Server 2012 ###

1. Open the Web browser and go to the Microsoft Azure Portal at <http://windows.azure.com>

1. Sign in with your Live ID.


	![Sign in](Images/sign-in.png?raw=true "Microsoft Azure Portal Sign in")

	_Microsoft Azure Portal sign in_

	>**Speaking Point**
	>
	> The first step in migrating our application to Microsoft Azure is to create a VM.
	>
	> Let us first navigate to the Microsoft Azure portal and get signed in.
	>
	> We use Live ID for authentication on the portal, so let’s log on.

1. In the portal, click **New** | **Virtual Machine** | **From Gallery**.

	![Creating Virtual Machine from Gallery](Images/creating-virtual-machine-from-gallery.png?raw=true "Creating Virtual Machine from Gallery")

	_Creating a virtual machine from the Gallery_

	>**Speaking Point**
	>
	> Now that we’re logged in, creating a virtual machine via the portal is easy. Let’s select the virtual machines option in the navigation pane.
	>
	> At least one virtual machine will be created, but we’re going to create a new virtual machine that has SQL Server 2012. To do that, we’re going to select the **FROM GALLERY** option.
	>
	>You can use **Quick Create** if you only need one virtual machine that doesn’t need to be load-balanced or joined to a virtual network or
**From Gallery** if you have more complex solution. With the latter, you have more flexibility in how the machine is used in advanced scenarios.

1.	In the **CREATE VIRTUAL MACHINE** dialog, select the **Microsoft SQL Server 2012 Evaluation** option.
1.	Click the right arrow (Next).


	![Selecting Virtual Machine OS version](Images/selecting-virtual-machine-os-version.png?raw=true "Selecting Virtual Machine OS version")

	_Selecting an OS version for the virtual machine_

	>**Speaking Point**
	>
	> Notice all the different VM’s that we have in our gallery. As you can see, we have many VMs that include Linux, as well as several Server 2008 R2 and Server 2012 VMs.
	>
	>We also have, as you can see, a SQL Server 2012 VM. This VM is running SQL Server 2012 Enterprise Evaluation edition which is the VM we want to create, so let’s select that one.

1.	The second page of the **Create Virtual Machine** wizard is the **VM Configuration**. Enter the following:
	- **Virtual Machine Name**: _ContosExpenseVM_
	- **Password**: _Passw0rd!_

1.	Enter the password for the administrator account, then re-enter the password for confirmation.
1. Select _Medium_ or larger as the **SIZE** of the VM.

	![VM Configuration page](Images/vm-configuration-page.png?raw=true "VM Configuration page")

	_VM configuration page_

	>**Speaking Point**
	>
	> Now what we need to do is give our virtual machine a name. This is the portal display name.
	>
	>We also need to supply an administrator password. 
	>
	>More  importantly, however, we also need to select the size of the VM. There are several size options:
	>
	>	- Extra Small
	>	- Small
	>	- Medium
	>	- Large
	>	- Extra Large
	>
	>These sizes differ, as we can see, in the amount of CPU cores and memory allotted to the VM. It is important that the VM we select will suffice for our needs.

1.	The third page of the **CREATE VIRTUAL MACHINE** wizard is the **VM Mode.** Select the **STANDALONE VIRTUAL MACHINE** option.
1.	Enter a _unique_ **DNS NAME** for the virtual machine.
1.	For the **STORAGE ACCOUNT**, leave the default at **Use Automatically Generated Storage Account**.
1. Choose a suitable **REGION/AFFINITY GROUP/VIRTUAL NETWORK** where the VM will be deployed.

	![VM Mode page](Images/vm-mode-page.png?raw=true "VM Mode page")

	_VM Mode page_

	>**Speaking Point**
	>
	> Now, we need to select the type of virtual machine. In other words, is this going to be new standalone virtual machine, or are we connecting to an existing virtual machine? With the new IaaS domain features, we have the ability to join this VM to an existing domain. However, for the purposes of this demo, we’ll simply create a standalone VM.
	>
	> Next, we need to supply a DNS name for this VM. 
	> 
	> These VMs are stored in blob storage and we need to specify whether our VM will use a Microsoft Azure account tied to our subscription, if we created one, or use an automatically generated storage account. Since we haven’t created a storage account yet, we’ll use the automatically generated account.
Lastly, we need to specify in which region to create the VM. 


1.	The fourth and final page of the **CREATE VIRTUAL MACHINE** wizard is the **VM Options.** 
1.	Select **Create Availability Set**, and set the **AVAILABILITY SET NAME** to **MyAvailSet**.
1. Click Finish (check mark).

	![VM Options page](Images/vm-options-page.png?raw=true "VM Options page")

	_VM Options page_

	>**Speaking Point**
	>
	> Lastly, we need to select an availability set in which to create the VM. 
	>
	> An availability set is a group of machines that are deployed across multiple locations (commonly called fault domains). Availability sets are used to protect from outages.
	> 
	>So, we’ll select the **Create Availability Set** option and name the availability set as _MyAvailSet_.
	>
	>Great, we are done! We can see in the portal that our VM is being provisioned.

1. For expediency, you may now switch over to a virtual machine that you have previously provisioned. This VM must be configured with:

	- Microsoft SQL Server 2012
	- An endpoint configured for TCP port 1433 to enable remote access to the SQL Server instance
	- TCP port 1433 opened in the Windows Firewall
	- SQL Server authentication enabled in SQL Server
	- An additional SQL Server login configured with SQL Server authentication and assigned to the _sysadmin_ server role

	Otherwise, you need to complete the following steps to configure the VM.

1.	In the **Microsoft Azure Portal**, select the VM you created previously.
1.	Select the **ENDPOINTS** view, and then click **ADD ENDPOINT** in the command bar.
1.	In the **ADD ENDPOINT** dialog, select **Add Endpoint** and then click the right arrow to go to the next page.

	![Add Endpoint to Virtual Machine page](Images/add-endpoint-to-virtual-machine-page.png?raw=true "Add Endpoint to Virtual Machine page")

	_Adding an endpoint to the virtual machine_

1.	In the second page of the **ADD ENDPOINT** dialog, enter a **NAME** for the endpoint, select **TCP** as the **PROTOCOL**, and specify _1433_ for both the **PUBLIC PORT** and **PRIVATE PORT** . 

	![Add Endpoint To Virtual Machine page](Images/add-endpoint-to-virtual-machine-page2.png?raw=true "Add Endpoint To Virtual Machine page")

	_Configuring the virtual machine endpoint_

	>**Speaking Point**
	>
	> Highlight the fact that in order to gain access to SQL Server inside the VM from external, we needed to create an **Endpoint** for port 1433.
	>
	>**Virtual machines** use endpoints to communicate within Microsoft Azure and with other resources on the Internet
	>
	>Creating the endpoint allows us to access the VM remotely to connect to SQL Server.

	![Endpoints Section](Images/endpoints-section.png?raw=true "Endpoints Section")

	_Viewing the endpoints configured for the virtual machine_

1.	Ensure that you are on the **DASHBOARD** page.

1. In the **DASHBOARD**, highlight and copy the **DNS** name of the virtual machine to the clipboard.

	![Virtual Machine's DNS](Images/virtual-machines-dns.png?raw=true "Virtual Machine's DNS")

	_Virtual machine's DNS_

	>**Speaking Point**
	>
	> However, before we connect to the VM, we need to grab some information from it. 
	>
	> In order to connect to SQL Server in the VM, we need its DNS name. We’ll be using it shortly to connect to SQL Server.

1.	Now, click **Connect** on the command bar.
1.	Save the RDP file to the desktop or another suitable location.
1. Minimize the browser.

	![Connect to the Virtual Machine](Images/connect-to-the-virtual-machine.png?raw=true "Connect to the Virtual Machine")

	_Connecting to the virtual machine_

	>**Speaking Point**
	>
	> We’re going to connect to this VM and to do that, we simply select the VM in the portal and click **CONNECT**, which downloads a Remote Desktop Connection file and asks us to open it or save it to disk. 
	>
	> For our existing VM, we will save this shortcut to our desktop.

1.	On the desktop, double-click the RDP file.
2.	In the **Windows Security** dialog, enter the password for the Administrator account.
	- Password: Passw0rd!
1. Click **OK**.

	![Connecting Virtual Machine - Entering Credentials](Images/connecting-virtual-machine-entering-credent.png?raw=true "Connecting Virtual Machine - Entering Credentials")

	_Entering the credentials for the Remote Desktop Connection_

	>**Speaking Point**
	>
	> Let’s log into the virtual machine by entering the password we configured earlier. 

1.	Inside the remote desktop session, click **Run** on the **Start** menu, type **WF.msc**, and then click **OK**.
1.	In the **Windows Firewall with Advanced Security** management console, select **Inbound Rules** in the left pane, and then click **New Rule** in the action pane.
1.	In the **Rule Type** page, select **Port**, and then click **Next**.
1.	In the **Protocol and Ports** page, select **TCP**. Select **Specific local ports**, and then type the port number of the instance of the Database Engine, **1433** for the default instance. Click **Next**.
1.	In the **Action** page, select **Allow the connection**, and then click **Next**.
1.	In the **Profile** page, select any profiles that describe the computer connection environment when you want to connect to the Database Engine, and then click **Next**.
1.	In the **Name** page, type a name and description for this rule, and then click **Finish**.

	![New Inbound Firewall Rule](Images/new-inbound-firewall-rule.png?raw=true"New Inbound Firewall Rule")

	_Creating a firewall rule for SQL Server access_

	>**Speaking Point**
	> 
	> To gain access to SQL Server in the VM, we need to open port 1433 on the Windows Firewall to allow inbound connections.

1.	Now, **in the remote desktop session**, open SQL Server Management Studio and log in to SQL Server using Windows Authentication.

	![Management Studio's Object Explorer](Images/management-studios-object-explorer.png?raw=true "Management Studio's Object Explorer")

	_Management Studio's Object Explorer_

	>**Speaking Point**
	>
	> Call out the fact that is indeed SQL Server 2012, FULL FUNCTIONALITY.

1. Expand the **Databases** node to illustrate that there are no user databases.
1. Right-click the (root) server node and then select **Properties**.
1. In the **Server Properties** dialog, select the **Security** page.
1. Under **Server authentication**, select the option labeled **SQL Server and Windows Authentication mode** and then click **OK**.

	![Enabling SQL Server Authentication](Images/enabling-sql-server-authentication.png?raw=true "Enabling SQL Server Authentication")

	_Enabling SQL Server Authentication_

	>**Speaking Point**
	> 
	> For this demo, we will enable SQL Server authentication mode and create login that we can use to connect remotely.

1. Now, right-click the (root) server node again and select **Restart** to allow the change in authentication mode to take effect.

1. Expand the **Security** node, right-click **Logins**, and then select **New Login**.
1. In the **Login - New** dialog, select the **General** page, type a new **Login name**, for example _demouser_, select **SQL Server authentication**, and clear the option labeled **User must change password at next login**.

	![New SQL Server Login](Images/sql-server-create-login.png?raw=true"New SQL Server Login")

	_Adding a new SQL Server login_

1. Now, switch to the **Server Roles** page, enable the **sysadmin** role, and then click **OK**.

	![Assigning the sysadmin role](Images/assign-sysadmin-role-to-login.png?raw=true"Assigning the sysadmin role")

	_Assigning the sysadmin role to the SQL Server login_

	>**Speaking Point**
	>
	> For simplicity, we also need to create a new SQL Server login with which to connect to SQL Server in the VM.

1. Minimize the remote desktop session for the VM.

1.	**In your local machine (on-premises)**, open Management Studio for SQL Server 2012.
1.	In the **Connect to Server** dialog, log into the on-premise SQL Server using Windows Authentication.

	![Connecting to on-premises SQL Server](Images/connecting-to-on-premises-sql-server.png?raw=true "Connecting to on-premises SQL Server")

	_Connecting to the on-premises SQL Server_

	>**Speaking Point**
	>
	> Our first task is to connect to our on-premises SQL Server. This is where our source database is coming from.

1. Expand the database node and point to the database that is going to be migrated to the VM. 
1.	Create a second SQL Server connection. In the **Server name** field, paste in the DNS name of the VM copied earlier from the portal.
1. Choose **SQL Server Authentication** as the authentication method, and then type _demouser_ and _Passw0rd!_ for the login name and password.

	![Creating to SQL Server using VM's DNS Name](Images/creating-to-sql-server-using-vms-dns-name.png?raw=true "Creating to SQL Server using VM's DNS Name")

	_Connecting to the SQL Server in Microsoft Azure using the VM's DNS name_

	>**Speaking Point**
	>
	> We also need to connect to SQL Server in the VM. 
	>
	>This is as easy as specifying the DNS name of the VM. 
	>
	>We copied this name to the clipboard earlier.

1.	You should now have two connections, one for the on-premises SQL Server and one for the server running in the VM.
1.	In the on-premises connection, right-click the database to export and select **Tasks** -> **Export Data-Tier Application** in the context menu.
1. In the **Introduction** screen, click **Next**.

	![Exporting Data-tier Application](Images/exporting-data-tier-application.png?raw=true "Exporting Data-tier Application")

	_Exporting the data-tier application_

	>**Speaking Point**
	>
	> We actually have several ways of getting our database from on-premises to the VM, including the Generate Scripts wizard or backup/restore. 
	>
	> **However, new in Sql Server 2012 is the ability to export and import a database using what are known as BACPACs**.

1.	In the **Export Settings** page, select the option labeled **Save to local disk**.
1.	Click **Browse**.
1.	In the **Save As** dialog, go to the _C:\DAC Packages_ folder or another suitable location, set the filename to **Expenses**, and then click **Save**.
1. Click **Next** to go to the **Summary Page**.
1. Click **Finish**.

	![Exporting bacpac file](Images/exporting-bacpac-file.png?raw=true "Exporting bacpac file")

	_Exporting the bacpac file_

	>**Speaking Point**
	>
	> **Highlight the following:**
	>
	>	- BACPACs - schema and data
	>	- Save TO Microsoft Azure BLOB storage
	>
	> We will now import the bacpac into the SQL Server running in the VM.

1.	In the SQL/VM connection in Object Explorer, right-click the **Databases** node and select **Import Data-Tier Application**.
1. In the **Introduction** page, click **Next**.

	![Importing Data-tier Application](Images/importing-data-tier-application.png?raw=true "Importing Data-tier Application")

	_Importing the data-tier application_

	>**Speaking Point**
	>
	> Again, we go back to SQL Server 2012, but this time we select our destination server, which is SQL Server running in the VM.

1.	In the **Import Settings** page, click **Browse** and go to the folder where you previously saved the bacpac file.
1. Select the bacpac file and click **Open**.
1. Click **Next**.
1. In the **Database Settings** page, click **Next**.
1. In the **Summary** page, click **Finish**.

	![Importing bacpac file](Images/importing-bacpac-file.png?raw=true "Importing bacpac file")

	_Importing the bacpac file_

	>**Speaking Point**
	>
	> Now, we’re going to import from the exported bacpac file. 
	>
	> Again, highlight the following:
	>
	>	- BACPACs - schema and data
	>	- Import FROM Microsoft Azure BLOB storage
	>
	> POINT: No need to create the database! This process will create it for you!**

1.	Maximize the remote desktop session to the VM and in SSMS, refresh the **Databases** node.
1. Show the newly created database and, in particular, expand its **Tables** node.
1. In Visual Studio, open the **Web.config** file.
1.	Remove the existing connection string.
1.	Press **CTL+K, CTL+X** and select **My XML Snippets**.
1. Insert the **VMConnectionString** snippet and replace the placeholders in the connection string with the host name of your SQL Server VM, and the user name and password of the SQL Server login that you created previously.

	![Updating the connection string within Web.config](Images/updating-connection-string-in-webconfig.png?raw=true "Updating the connection string within Web.config")

	_Updating the connection string in Web.config_

	>**Speaking Point**
	>
	> We are going to deploy the application to Microsoft Azure, but we now want it to connect to our database in the VM, so the first thing we need to do is modify the connection string.

1. Press **F5**.

	<!-- TODO: Add application's screenshot -->

	>**Speaking Point**
	>
	> Let’s build our application to ensure we’re good.

1.	Go back to the portal.
1.	Click **NEW** | **WEB SITE** | **QUICK CREATE**. 
1. Enter **ContosoExpense** for the **URL** or, if this DNS name is already in use, choose another suitable name.
1.	Choose the **REGION** where the site will be created.
1. Click **Create Web Site**.

	![Creating a new Web Site](Images/creating-a-new-web-site.png?raw=true "Creating a new Web Site")

	_Creating a new Web site_

	>**Speaking Point**
	>
	> **Quick Create** when you don’t need database connectivity or you will be setting up a database separately.
	>
	> **From Gallery** when you want to quickly build a web site from one of several available templates.
	>
	> **Custom Create** a website if you plan to deploy a completed web application to Window Azure and you want to simultaneously set up a database for use with your website.
	>
	> We need to specify the URL through which this app will be accessed, as well as the region where the application will be hosted.
	>
	> POINT: **Notice how fast our Web site was provisioned**.

1. Once provisioned, select the newly created web site **NAME** in the list.

	![Selecting Web Site's name](Images/selecting-web-sites-name.png?raw=true "Selecting Web Site's name")

	_Managing the Web site_

1. In the **DASHBOARD**, click the **Download publish profile** link.

	![Downloading Publish Profile](Images/downloading-publish-profile.png?raw=true "Downloading Publish Profile")

	_Downloading the publish profile_

1. Save the file to the desktop or another suitable location.

	>**Speaking Point**
	>
	> POINT: All of the information required to publish a Web application to a Microsoft Azure website is stored in an XML file known as a **publish profile**. The publish profile contains the URLs, user credentials and database strings required to connect to and authenticate against each of the endpoints for which a publication method is enabled.

1.	Go back to **Visual Studio**, right-click the **Web Project** and then select **Publish**.
1.	In the **Publish Web** dialog, click the **Import** button, browse to locate the **.PublishSettings** file that you downloaded previously, and then click **Open**.

	![Importing the publish settings file](Images/importing-publish-settings-file.png?raw=true "Importing the publish settings file")

	_Importing the publish settings file_

	>**Speaking Point**
	>
	> POINT: **MAXIMUM COMPATIBILITY!**

1.	Click **Next**.
1.	Click **Publish**.
1. The web site will automatically load after the deployment is finished.

	![Publish Web Application Page](Images/publish-web-application-page.png?raw=true "Publish Web Application Page")

	_Publishing the Web application_

	>**Speaking Point**
	>
	> The **Web Deploy** settings will be automatically populated according to the settings in the **.PublishSettings** file.
	>
	> **POINT:** With a few clicks and using familiar tools we have deployed both the app and the database to the cloud with NO CODE except for changing the connection string. 

<a name="segment3" />
#### Microsoft Azure SQL Database ####

1.	Back in to the Microsoft Azure portal, select the **SQL DATABASES** option in the navigation pane.
1. Click **New**.

	![Azure Portal SQL Databases](Images/azure-portal-sql-databases.png?raw=true "Azure Portal SQL Databases")

	_SQL Databases in the portal_

	>**Speaking Point**
	>
	> We have multiple options for creating servers and databases. Previously, we clicked NEW on the navigation bar.
	> Here, we simply need to create a server, so we'll take a different route and go back to the navigation bar and select SQL Databases.

1.	Select the **SERVERS** menu option.

	![Servers Section](Images/servers-section.png?raw=true "Servers Section")

	_Servers view_

	>**Speaking Point**
	>
	> From here, we’ll click the **SERVERS** option because we simply need to provision a server.

1. Now, click **ADD**.
1.	In the **SQL Database server settings** page, enter the following:
	- **LOGIN NAME**:  _AzureAdmin_
	- **LOGIN PASSWORD**: _Passw0rd!_
1. Choose a **REGION** that matches the region used by the web site that you created previously.
1.	Ensure that the option labeled **Allow Microsoft Azure services to access the server** is selected.

	![Database server settings](Images/database-server-settings.png?raw=true "Database server settings")

	_Database server settings_

1.	Click **OK**.
1. Once the server is provisioned, click its Name on the list.

	>**Speaking Point**
	>
	> The admin account is much like the _sa_ account in the on-premises SQL Server. This account is the main administrator for the Microsoft Azure SQL Database server.
	> You can create other accounts and assign different permissions just like you can on-premises.

1. Click the **CONFIGURE** menu option.

	![Configure Option](Images/configure-option.png?raw=true "Configure Option")

	_Configure Option_

	>**Speaking Point**
	>
	> With our server provisioned, we need to configure a few things.

1.	Add a firewall rule for the current IP address. You can click the arrow shown next to the **CURRENT CLIENT IP ADDRESS** field to do this automatically.

	![Add Firewall Rule](Images/add-firewall-rule.png?raw=true "Add Firewall Rule")

	_Adding a firewall rule for the current location_

1. Click **Save**.

	>**Speaking Point**
	>
	> One of the great features of WA SQL Database is its built-in security. Firewall rules help protect your data by preventing all access to your server until you specify which computers have permission.
	>
	>Firewall rules grant access based on the originating IP addresses of each request.
	>
	> Let's save the changes.

1. In the portal, select the **DASHBOARD** menu option and copy the **FQDN** of the server to the clipboard.

	![Manage URL](Images/manage-url.png?raw=true "Manage URL")

	_Manage URL_

	>**Speaking Point**
	>
	> Unchanged is how to connect to WA SQL DB; via a DNS endpoint.

1.	Go back to SQL Server Management Studio.
1.	In **Object Explorer**, click **Connect -> Database Engine**.
1.	Select _SQL Server Authentication_ as the authentication method and enter the following credentials:
	- **Login**: _AzureAdmin_
	- **Password**: _Passw0rd!_

	![Connecting to Azure SQL Database](Images/connecting-to-azure-sql-database.png?raw=true "Connecting to Azure SQL Database")

	_Connecting to Microsoft Azure SQL Database_

	>**Speaking Point**
	>
	> We now have every relational database option in a single tool!
	>
	>	- On-premises
	>	- SQL Server in a VM
	>	- Microsoft Azure SQL Database

1.	We now need to import the database into the Microsoft Azure SQL Database.
1.	In Object Explorer, right-click the **Databases** node for the Microsoft Azure SQL Database connection and select **Import Data-Tier Application**.
1. In the **Introduction** page, click **Next**.


	![Import Data-tier Application](Images/import-data-tier-application2.png?raw=true "Import Data-tier Application")

	_Importing a data-tier application_

	>**Speaking Point**
	>
	> Now, let’s use the same bacpac that we used earlier to also import the data into a Microsoft Azure SQL Database.

1.	In the **Import Settings** page, click **Browse** to locate and select the saved **Expenses** bacpac file and then click **Open**.
1. Click **Next**.

	![Importing the bacpac file](Images/importing-bacpac-file2.png?raw=true "Importing the bacpac file")

	_Importing the bacpac file_

	>**Speaking Point**
	>
	> POINT: COMPATIBILTY ON EXPORT.

1.	In the **Database Settings** page, enter the following:
	- **New Database Name**: _Expenses_
	- **Edition/ Maximum database size**: Keep Default
1. Click **Next**.
1. In the **Summary** page, click **Finish**.

	![Specify Settings for the new SQL Database](Images/specify-settings-for-the-new-sql-database.png?raw=true "Specify Settings for the new SQL Database")

	_Configuring the new SQL Database_

	>**Speaking Point**
	>
	> Highlight the fact that it is important to know the size of the database so you can select the right edition and size.
	> Also point out that this process will create the database for you.

1.	In the Portal, select the **Databases** option in the **Navigation** pane.
1. Select the database **NAME**.	
	
	![SQL Databases section](Images/sql-databases-section.png?raw=true "SQL Databases section")

	_SQL Databases section_

1. In the **DASHBOARD**, click **Show Connection Strings**.

	![Show Connection Strings](Images/show-connection-strings.png?raw=true "Show Connection Strings")

	_Viewing the connection strings_

1. Highlight and copy the ADO.NET connection string.

	![Connection String](Images/connection-string.png?raw=true "Connection String")

	_Connection String_


1.	In Visual Studio, open the **Web.config** file.
1.	Remove the existing connection string 
1.	Press **CTL+K, CTL+X** and select **My XML Snippets**.
1.	Insert the **AzureConnectionString** and replace the placeholder with the connection string that you copied from the portal. 
1. Remember to update the password in the new connection string:

	![Updating Web.config with Azure Connection String](Images/updating-webconfig-with-azure-connection-stri.png?raw=true "Updating Web.config with Azure Connection String")

	_Updating Web.config with the Azure connection string_

	>**Speaking Point**
	>
	> We now need to deploy the application to Microsoft Azure, but we now want it to connect to our database in the VM, so the first thing we need to do is modify the connection string.

1. Right-click the **Web Project** and then select **Publish**.

	![Re-deploying application](Images/re-deploying-application.png?raw=true "Re-deploying application")

	_Re-deploying the application_

	>**Speaking Point**
	>
	> Simply by changing the connection string and redeploying, I'm  running completely in Azure managed services. 


<a name="segment4" />
### Microsoft Azure SQL Federation ###

1. In SQL Server Management Studio, expand the SQL Azure Connection in Object Explorer and select the **Databases** node.

	![Object Explorer showing the SQL Azure Database connection](Images/management-studios-object-explorer-.png?raw=true "Object Explorer showing the SQL Azure Database connection")

	_Object Explorer showing the SQL Azure Database connection_

	>**Speaking Point**
	>
	> The database we have been using up to this point is a single Azure SQL Database. Since we've just discussed SQL Federations, we’re going to illustrate how to create a federated version of our expense report database.
	> In SSMS, we need to create a new database from which we will create our federations. 

1.	Right-click the **Databases** node, and select **New Database** to open a new query window connected to the _master_ database. 
1. In the new query window, replace the placeholder for the database name with the name _ContosoFed_ and execute the query.

	![Creating a new database](Images/creating-a-new-database.png?raw=true "Creating a new database")

	_Creating a new database_

	>**Speaking Point**
	>
	> Let’s open a new query window and create a new database. We’ll call this database ContosoFed. 
	>
	> We need to make sure that the connection for this query window is connected to the master database.

1.	Once the database is created, in SSMS, open the **ContosoExpenseFed_DB.sql** script located in the **Assets\Federations** folder. Ensure that the query window connection is for the new database.
1. Execute the script.

	![Create Federation script](Images/create-federation-script.png?raw=true "Create Federation script")

	_Create Federation script_

	>**Speaking Point**
	>
	> Before we run the script that will create our federation and federated objects, let’s spend a minute looking at the script and Federation syntax. 
	>
	>	- CREATE FED statement
	>	- USE FED statement
	>	- Table changes for FEDs
	>	- Use of GUIDS and why
	>
	> With our root database created, let’s run the script that will create our federation and associated federated objects.

1. In Object Explorer in SSMS, expand the **Federations** node of the new database and right-click the **UserExpense_Federation**.

	![Azure SQL Database Connection](Images/azure-sql-database-connection.png?raw=true "Azure SQL Database Connection")
	
	_Microsoft Azure SQL Database connection_

	>**Speaking Point**
	>
	> SQL Server 2012 now includes the ability to work directly with Federations.
	>
	> Talking Points:
	>
	>	- Connect to a specific Fed
	>	- The ability to connect and query a specific federation
	>	- Split - Scale the database by partitioning the database via the designated key.

1.	Now, open the **ContosoExpenseFed_Split.sql** script located in the **Assets\Federations** folder. Ensure that the query window connection is for the new database.
1.	Execute the first statement.
1.	Execute the 2nd set of statements (the DMV’s).
1. Execute the SELECT queries.

	![Execute Federation Script](Images/execute-federation-script.png?raw=true "Execute Federation Script")

	_Executing the Federation script_

	>**Speaking Point**
	>
	> Walk through the examples of:
	>
	>	- Looking at the metadata
	>	- Split operation
	>	- Querying a specific fed mem

1.	Back in the portal, select **SQL DATABASES** from the navigation menu.
1.	Select the **Databases** option.
1. Click the federated database **NAME** to view its properties.

	![SQL Databases page](Images/sql-databases-page.png?raw=true "SQL Databases page")

	_Managing the ContosoFed SQL Database_

	>**Speaking Point**
	>
	> The Management Portal for SQL Azure is a lightweight and easy-to-use database management tool. It allows you to conveniently manage your SQL Azure databases and to quickly develop, deploy, and manage your data-driven applications in the cloud.

1. In the **Dashboard** for the **ContosoFed** database, click the **MANAGE URL** link.

	![Manage URL](Images/manage-url2.png?raw=true "Manage URL")

	_Accessing the database management portal_

1.	Enter the following credentials:
	1.	**User**: _AzureAdmin_
	1.	**Password**: Passw0rd!
1. Click **Log on**.

	![Log in to Azure SQL Database portal](Images/log-in-to-azure-sql-database-portal.png?raw=true "Log in to Azure SQL Database portal")

	_Microsoft Azure SQL Database portal_

1. In the **Summary Page** of the SQL Database portal, click the **right arrow** in the Federations section to show all federation members.

	![SQL Portal showing federations](Images/sql-portal-showing-federations.png?raw=true "SQL Portal showing federations")

	_Microsoft Azure SQL Database portal showing federations_

	>**Speaking Point**
	>
	> Database sharding is a technique for horizontally partitioning data across multiple physical servers to provide application scale-out. Microsoft Azure SQL Database combined with database sharding techniques provides for virtually unlimited scalability of data for an application.

1.	In the **Federation Member** grid, click the grey area.
1.	In the **UserExpense_Federation** dialog, select the **Split** option and set the value to **40**.
1. Click **Split**.

	![Federation Member](Images/federation-member.png?raw=true "Federation Member")

	_Splitting a federation member_

	>**Speaking Point**
	>
	> Here, we’re going to specify the value on which to split the federation, thus creating a second federation member.

1.	Go back to SSMS
1. Talk through the **USE FEDERATION** statements to query the different **Federation Members**.


<a name="segment5" />
### Microsoft Azure Storage ###

1. Open the **Microsoft Azure Management portal** and select the **STORAGE ACCOUNTS** option in the **Navigation Pane**.

	![Managing storage accounts](Images/selecting-the-storage-accounts.png?raw=true "Managing storage accounts")

	_Managing storage accounts_

	>**Speaking Point**
	>
	> There are three options for unstructured and non-relational data storage in Microsoft Azure: **Blob**, **Table**, and **Queue** services.
	>
	> A storage account is scoped to a primary geographic region and is configured by default to seamlessly replicate itself to a secondary region in case of a major failure in the primary region.

1. Click **New | STORAGE ACCOUNT | QUICK CREATE**.	

	![Quick Create Storage Account](Images/quick-create-storage-account.png?raw=true "Quick Create Storage Account")

	_Quick Create a Storage Account_

1. In **CREATE A NEW STORAGE ACCOUNT**, enter **contosoexpense** for the storage account name or, if this name is already in use, choose a different name.
1. Choose a **REGION/AFFINITY GROUP** that matches the one used by the web site that you created previously.

	![Creating the Storage Account](Images/creating-the-storage-account.png?raw=true "Creating the Storage Account")

	_Creating the storage account_

	>**Speaking Point**
	>
	> You can specify either a **geographic region** or an **affinity group** for your storage. By specifying an affinity group, you can co-locate your cloud apps in the same data center with your storage
	>
	> Geo-replication is turned on by default. During geo-replication, your data is replicated to a secondary location, at no cost to you, so that your storage fails over seamlessly to a secondary location in the event of a major failure that can't be handled in the primary location. The secondary location is assigned automatically, and can't be changed.

1. Click **CREATE STORAGE ACCOUNT**.	

1. Open **Visual Studio** and go to **Tools | Library Package Manager | Package Manager Console**.

	>**Speaking Point**
	>
	> By running the following command in the **Package Manager Console**, we are adding the Microsoft Azure Storage Client libraries to the project.

1. In the **Package Manager Console**, type: _Install-Package WindowsAzure.Storage_.

	![Using the Package Manager Console](Images/using-the-package-manager-console.png?raw=true "Using the Package Manager Console")

	_Installing the Microsoft Azure Storage Client library_

1. In Solution Explorer, right-click the **Helpers** folder, point to **Add** and select **Existing Item**. 
1. Browse to the **Assets\storage** folder, select **StorageHelper.cs** and click **Add**.

	![Adding the StorageHelper class to the project](Images/add-storage-helper-class.png?raw=true "Adding the StorageHelper class to the project")

	_Adding the StorageHelper class to the project_

	>**Speaking Point**
	>
	> We'll also add a helper class that handles the creation of blob containers and sets their access permission.

1. Open **Views | Reports | EditorTemplates | ExpenseReportDetail.cshtml**

	>**Speaking Point**
	>
	> Let’s add the code to show the **Attach Receipt** link/icon for each expense line item.

1. Insert a new line after the comment that reads "**Attach Link Here**" and type the text **attachreceiptlink** in this line. Then, press **TAB** to install the corresponding code snippet.

	![Editing ExtenseReportDetail.cshtml](Images/editing-extensereportdetailcshtml.png?raw=true "Editing ExtenseReportDetail.cshtml")

	_Editing the ExpenseReportDetail template_

1. Open **Views | Reports | Edit.cshtml**

	>**Speaking Point**
	>
	> Next, let’s add the **Submit Receipt** form.

1. Insert a new line after the comment that reads "**Attach Form Here**" and type the text **attachreceiptform** in this line. Then, press **TAB** to install the corresponding code snippet.	

	![Editing Edit.cshtml](Images/editing-editcshtml.png?raw=true "Editing Edit.cshtml")

	_Updating the Edit view_

1. Open **Controllers | ReportsController**

	>**Speaking Point**
	>
	> Next, let’s add the code to **save the receipt to blob storage**.

1. In the **ReportsController** class, Insert a new line immediately below the **Summary** action method, type **attachreceiptmethod** and press **TAB** to install the code snippet.

	![Editing ReportsController.cs](Images/editing-reportscontrollercs.png?raw=true "Editing ReportsController.cs")

	_Inserting the AttachReceipt action_

1. Go back to the **Azure Management portal** and select **STORAGE** from the navigation pane.

	![Managing storage in the Management Portal](Images/managing-storage-in-the-management-portal.png?raw=true "Managing storage in the Management Portal")

	_Managing storage in the Management Portal_

1. Select the newly created storage account **NAME** in the list of storage accounts.

	![Selecting the Storage Account Name](Images/selecting-the-storage-account-name.png?raw=true "Selecting the Storage Account Name")

	_Managing the storage account_

1. Select the **CONFIGURE** menu option.

	![Selecting Configure Option](Images/selecting-configure-option.png?raw=true "Selecting Configure Option")

	_Configuring the storage account_

	>**Speaking Point**
	>
	> The **Minimal** monitoring value collects metrics such as ingress/egress, availability, latency, and success percentages summarized at the Blob, Table, and Queue service level.
	>
	> The **Verbose** monitoring value collects metrics at the API operation level in addition to the service-level metrics. Verbose metrics enable closer analysis of issues that occur during application operations.

1. In the **monitoring** section, set the monitoring level of  **BLOBS** to **MINIMAL**.

	![Selecting Monitoring Blobs Option](Images/selecting-monitoring-blobs-option.png?raw=true "Selecting Monitoring Blobs Option")

	_Configuring storage monitoring options_

1. In the command bar, click **SAVE** to save the changes to the monitoring configuration.

	![Saving the monitoring configuration](Images/save-monitoring-configuration.png?raw=true "Saving the monitoring configuration")

	_Saving the monitoring configuration_

1. Next, in the command bar, click **MANAGE KEYS**.

	![Managing the storage account keys](Images/clicking-manage-keys.png?raw=true "Clicking the storage account keys")

	_Managing the storage account keys_

	>**Speaking Point**
	>
	> We need to retrieve our **account key**. To do this, we go back to the portal, select our storage account, and click **MANAGE KEYS**.

1. Copy the value of the **PRIMARY ACCESS KEY** to the clipboard.

	![Copying the Primary Access Key](Images/copying-the-primary-access-key.png?raw=true "Copying the Primary Access Key")

	_Copying the primary access key_

1. Open the **Web.config** file and find the comment that reads “**Microsoft Azure Storage Account**” within the **appSettings** section.

	>**Speaking Point**
	>
	> Let’s add the storage account snippet. This information allows us to securely access our storage services.

1. Press **CTL+K**, **CTL+X** and select **My XML Snippets**.
1. Insert the **StorageAccountInfo** snippet and replace the storage account name with the name of your storage account and the key with the value you copied from the portal.

	![Editing Web.config values](Images/editing-webconfig-values.png?raw=true "Editing Web.config values")

	_Configuring the storage account settings_

	>**Speaking Point**
	>
	> Every account has a set of unique keys. We need to update the key in our web.config with the key or the account we just generated.

1. Right-click the **Web Project** and select **Publish** from the context menu. Follow the wizard to redeploy the updated files.

	![Redeploying the application](Images/redeploying-the-application.png?raw=true "Redeploying the application")

	_Redeploying the application_

	>**Speaking Point**
	>
	> Finally, let’s redeploy the application.

1. In the expense App, log in as a user and modify the expense report to attach a receipt.

1. Click the **Attach receipt** icon.

	![Clicking Attach Receipt](Images/attach-receipt.png?raw=true "Clicking Attach Receipt")

	_Attaching a new receipt_

1. In the **Attach Receipt** dialog, click **Browse**.

	![Clicking Browse Receipt](Images/clicking-browse-receipt.png?raw=true "Clicking Browse Receipt")

	_Attach Receipt dialog_

1. Go to the **Assets\receipts** folder and select _Receipt.png_.

	![Submitting a receipt](Images/submitting-a-receipt.png?raw=true "Submitting a Receipt")

	_Submitting a receipt_

1. In the **Attach Receipt** dialog, click **Submit**.	

1. Finally, click **Save and Submit** to submit the expense report.

	![Submitting an expense](Images/saving-and-submitting.png?raw=true "Submitting an expense")

	_Submitting an expense_

1. Log off as a user.

1. Now, log in as a manager, approve the expense report and view the receipt.

	>**Speaking Point**
	>
	> Types of data store and manage. Query with familiar tools.

1. Back in the portal, click the **MONITOR** option.	

	![Portal Monitor Option](Images/portal-monitor-option.png?raw=true "Portal Monitor Option")

	_Monitoring the storage account_

	>**Speaking Point**
	>
	> Hopefully there will be some data.

---
<a name="summary" />
## Summary ##

In this demo, you learned how to:

1. Deploy applications to Microsoft Azure Websites.

1. Provision a virtual machine in Microsoft Azure to run SQL Server 2012.

1. Connect applications to a Microsoft Azure SQL Database.

1. Connect applications to use Microsoft Azure Storage.

1. Create SQL Database Federations.
