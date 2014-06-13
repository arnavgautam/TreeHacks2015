<a name="handsonlab"></a>
# Microsoft Azure Websites and Virtual Machines using ASP.NET and SQL Server #

---

<a name="Overview"></a>
## Overview ##

In this lab, you will learn how to create ASP.NET web applications that connect to virtual machines running in Microsoft Azure. First, you will create a virtual machine with SQL Server 2012 installed using the Microsoft Azure Management Portal and configure it to allow external connections. Then, you will create a simple ASP.NET MVC 4 web application using Entity Framework that accesses the database in the SQL Server virtual machine. The web application will take advantage of Full-Text Search features in SQL Server 2012 to search for contacts data.  You will complete the lab by deploying the application to Microsoft Azure Websites using Visual Studio.

> **Note:** A Visual Studio 2012 version of this Hands-on Lab can be found in the latest build of the [Microsoft Azure training kit](http://bit.ly/WindowsAzureTK) or in [GitHub](https://github.com/WindowsAzure-TrainingKit/HOL-ASPNETAzureWebSitesVMSQL-VS2012).

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create and configure a SQL Server 2012 Virtual Machine in Microsoft Azure
- Create a public endpoint for the virtual machine to allow web applications
- Create an ASP.NET web application using Entity Framework that connects to the SQL Server Virtual Machine
- Configure full-text search in SQL Server 2012 database and perform full-text search queries from a web application
- Publish the web application to Microsoft Azure Websites using Visual Studio publishing feature

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Microsoft Visual Studio 2010](http://msdn.microsoft.com/vstudio/products/) with Service Pack 1
- [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4)
- A Microsoft Azure subscription with the Websites and Virtual Machines Preview enabled - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)
- [Microsoft Web Publish for Visual Studio 2010 (June 2012)] (http://www.microsoft.com/web/gallery/install.aspx?appid=WebToolsExtensionPublishingVS2010_Only_1_0)

> **Note:** This lab was designed to use Windows 7 Operating System.

<a name="Setup"></a>
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab's **source** folder.

1. Execute **Setup.cmd** with Administrator privileges to launch the setup process that will configure your environment and install the Visual Studio code snippets for this lab.

1. If the User Account Control dialog is shown, confirm the action to proceed.

>**Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="UsingCodeSnippets"></a>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2010 to avoid having to add it manually. 


---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Setup Windows Server Virtual Machine with SQL Server 2012](#Exercise1)

1. [Creating an MVC 4 Web Application that Connects to the SQL Server 2012 Virtual Machine](#Exercise2)

Estimated time to complete this lab: **60 minutes**.

>**Note:** Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise. Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.
>
>When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.


<a name="Exercise1"></a>
### Exercise 1: Creating a SQL Server 2012 Virtual Machine ###

In this exercise, you will create a new Virtual Machine with SQL Server and configure a public endpoint in order to access it remotely. Then you will connect to the database using Remote Desktop and create and configure a database in the server. 

You will also create a database in the SQL Server that will be used by the MVC 4 application you will create in [Exercise 2](#Ex2).

<a name="Ex1Task1"></a>
#### Task 1 - Creating a Virtual Machine Using the Microsoft Azure Portal ####

In this task, you will create a new Virtual Machine using the Microsoft Azure Portal.

A virtual machine in Microsoft Azure is a server in the cloud that you can control and manage. After you create a virtual machine in Microsoft Azure, you can start, stop, and delete it whenever you need to, and you can access the virtual machine just as you do with a server in your office.

1. Go to the [Microsoft Azure Management Portal] (https://manage.windowsazure.com) and sign in using your Windows account.

1. Click **New** and select **Virtual Machine** option and then **Quick Create**. Select the **SQL Server 2012 Evaluation** virtual machine and type a **DNS Name** for the virtual machine that is available (for example, _SQLServer12_). Also provide a password. Finally, click **Create Virtual Machine**.

	>**Note:** You will use the password in future steps to connect to the virtual machine using remote desktop.

	![Creating a New Virtual Machine](images/creating-a-new-vm.png?raw=true "Creating a New Virtual Machine")
 
	_Creating a New Virtual Machine_

1. In the **Virtual Machines** section, you will see the Virtual Machine you created displaying a _provisioning_ status. Wait until it changes to _Running_ in order to continue with the following step.

	>**Note:** Please notice that the provisioning process might take a considerable amount of time.

	![New virtual machine running ](images/vm-running.png?raw=true "New virtual machine running")
 
	_New virtual machine running_

1. In the **Virtual Machines** section, locate the Virtual Machine you just created and click on its name. Once in the **Dashboard** page, click **Endpoints**.

	Virtual machines use endpoints to communicate within Microsoft Azure and with other resources on the Internet. All virtual machines that you create in Microsoft Azure can automatically communicate with other virtual machines in the same cloud service or virtual network. However, you need to add an endpoint to a machine for other resources on the Internet, like web applications, or other virtual networks to communicate with it.

1. Click **Add Endpoint**, select **Add Endpoint** and then click the next button to continue.

	In this lab you will create a web application that communicates to the SQL Server in the virtual machine, so you will now create an endpoint for allowing external connections.

	![Adding a new Endpoint](images/adding-a-new-endpoint.png?raw=true "Adding a new Endpoint")
	
	_Adding a new Endpoint_


1. In the **Specify endpoint details** page, set the **Name** to _sqlserver_, the **Protocol** to _TCP_, the **Public Port** to 57501 and the **Private Port** to 1433.
	
	You can choose other public port number if you decide so, but make note of it as you will use it when creating the web application in exercise 2. The Internet Assigned Numbers Authority (IANA) suggests the range 49152 to 65535 for dynamic or private ports. The 1433 private port matches the default port used by SQL Server to accept incoming TCP connections.

	![New Endpoint Details](images/new-endpoint-details.png?raw=true "New Endpoint Details")

	_New Endpoint Details_

	> **Note:** An endpoint is defined with a public port and a private port. The public port is used to access the virtual machine from the Internet. The private port is used to control internal access to the virtual machine through the firewall. Later in the lab you will configure the firewall to allow connections in the private port. The public and private ports can be the same. In this case, the private port is set to the default port where SQL Server accepts incoming TCP connections.

	>You can associate specific ports and a protocol to endpoints. In this case the web application you will create in exercise 2 will communicate with the SQL Server using the TCP protocol. Notice that the TCP protocol also includes HTTP and HTTPS communication.

1. Wait until the endpoint configuration is complete.

<a name="Ex1Task2"></a>
#### Task 2 - Configuring the Virtual Machine and SQL Server 2012 for External Access ####

In this task, you will install an SQL Server and configure it to enable external access. This means, you will make sure the SQL Server is has the TCP/IP protocol enabled and you will configure the virtual machine firewall for allowing external connections to the SQL Server port.

1. In the Microsoft Azure Management Portal, click **Virtual Machines** on the left menu.

 	![Microsoft Azure Portal](./images/Windows-Azure-Portal.png?raw=true "Microsoft Azure Portal")
 
	_Microsoft Azure Portal_

1. Click the row of the virtual machine you created in the previous task from the virtual machines list and click **Connect** from the command bar below to connect using a Remote Desktop Connection. Download and open the _.rdp_ file when prompted.

	> **Note**: You can access the programs running on a virtual machine by remotely connecting to it. For example, a virtual machine running Windows Server 2008 R2 uses a Remote Desktop connection, and a virtual machine running Linux uses a Secure Shell (SSH) connection.

1. Click **Connect** when warned about the publisher of the remote connection.

1. When prompted for credentials, type the password you've configured when creating the virtual machine. Wait until the remote connection is ready.

 	![Entering virtual machine credentials](./images/entering-vm-credentials.png?raw=true "Microsoft Azure Portal")
 
	_Entering virtual machine credentials_

1. When warned about the identity of the remote computer, click **Yes** to connect.

1. Once connected to the virtual machine, open **SQL Server Configuration Manager** from **Start | All Programs | Microsoft SQL Server 2012 | Configuration Tools**.

1. Expand the **SQL Server Network Configuration** node and select **Protocols for MSSQLSERVER**. Make sure **Shared Memory**, **Named Pipes** and **TCP/IP** protocols are enabled. To enable a protocol, right-click the protocol name and select **Enable**.

 	![Enabling SQL Server Protocols](./images/Enabling-SQL-Server-Protocols.png?raw=true "Enabling SQL Server Protocols")
 
	_Enabling SQL Server Protocols_

	> **Note:** For more information about the SQL Server Network Configuration see http://msdn.microsoft.com/en-us/library/ms189083.aspx.

1. Go to the **SQL Server Services** node and right-click the **SQL Server (MSSQLSERVER)** item and select **Restart.**

1. To allow internet applications to access the SQL Server instance in the virtual machine through the Windows Firewall, you must configure a firewall rule. To do this, you will need to add an **Inbound Rule** for the SQL Server requests in the **Windows Firewall**. To do this, open **Windows Firewall with Advance Security** from **Start | All Programs | Administrative Tools**.

	> **Note:** By default, Microsoft Windows enables the Windows Firewall, which closes port 1433 to prevent Internet computers from connecting to a default instance of SQL Server on any computer. Connections to the default instance using TCP/IP are not possible unless you reopen port 1433. For more information about SQL Server and the Window Firewall see http://msdn.microsoft.com/en-us/library/ms175043.aspx

1. Right-click **Inbound Rules** node and select **New Rule**.

 	![Creating an Inbound Rule](./images/Creating-an-Inbound-Rule.png?raw=true "Creating an Inbound Rule")
 
	_Creating an Inbound Rule_

1. In the **New Inbound Rule Wizard**, select _Port_ as **Rule Type** and click **Next**.

	![New Inbound Rule Type](images/new-inbound-rule-type.png?raw=true "Inbound Rule Type")
 
	_Inbound Rule's Type_

1. In **Protocols and Ports** step, select **Specific local ports** and set its value to _1433_. Click **Next** to continue.

	![Inbound Rule's Local Port](images/inbound-rules-local-port.png?raw=true "Inbound Rule's Local Port")
 
	_Inbound Rule's Local Port_

1. In the **Action** step, make sure the **Allow the connection** option is selected and click **Next**.

	![Inbound Rule's Action](images/inbound-rules-action.png?raw=true "Inbound Rule's Action")
 
	_Inbound Rule's Action_

1. In the **Profile** step, leave the default values and click **Next**.

1. Finally, set the Inbound Rule's **Name** to _SQL Server Rule_ and click **Finish**.

 	![New Inbound Rule](images/new-inbound-rule.png?raw=true "New Inbound Rule")
 
	_New Inbound Rule_

1. Close **Windows Firewall with Advanced Security** window.

<a name="Ex1Task3"></a>
#### Task 3 - Creating the Web Application Database in the Virtual Machine####

In this task, you will create and populate the sample database that will be used by the MVC 4 application.

1. In the virtual machine, open SQL Server Management Studio from **Start | All Programs | Microsoft SQL Server 2012 | SQL Server Management Studio**. In the **Connect to Server** dialog, click **Connect**, to connect to the SQL Server using Windows Authentication.

1. Press **CTRL+N** to open a new query window. Go to the **\Source\Assets** folder of this lab and copy all the content of the **CreateDatabase.sql** file. Then paste the content of the file in the new query window.

	> **Note:** Remote desktop connections allow you to copy and paste across different machines.

1. Press **F5** to run the script. This will create the **ContactManagerDb** database, with only one _Contacts_ table. It will also populate the _Contacts_ table with sample data.

	![Running the create database script](images/running-the-create-database-script.png?raw=true)

	_Running the create database script_

	Now you will configure the database table to allow full-text search queries. You will use this feature in the web application you will build in the next exercise.

	> **Note:** Full-Text Search in SQL Server lets users and applications run full-text queries against character-based data in SQL Server tables. You can learn more about full-text search on the following article: http://msdn.microsoft.com/en-us/library/ms142497.aspx

1. Create a Full Text Catalog for the database. To do this, in Object Explore, expand **Storage** node within **ContactManagerDb** database.

	A full-text catalog is a logical concept required for configuring full-text search. It groups a set of full-text indexes.

1. Right-click **Full Text Catalogs** folder and select **New Full-Text Catalog**.

	![New Full-Text Catalog](images/new-full-text-catalog.png?raw=true "New Full-Text Catalog")
 
	_New Full-Text Catalog_

1. In the **New Full-Text Catalog** dialog, set the **Name** value to _ContactManagerCatalog_ and press **OK**.

	![New Full-Text Catalog Name](images/new-full-text-catalog-name.png?raw=true "New Full-Text Catalog Name")
 
	_Full-Text Catalog Name_

1. Right-click the **ContactManagerCatalog** and select **Properties**. Select the **Tables/Views** menu item. Add the **Contacts** table to the **Table/View objects assigned to the Catalog** list. Check _FirstName_, _LastName_ and _Company_ from **eligible columns** and click **OK**.

	This will add the FirstName, LastName and Company columns to the full-text catalog, allowing you to perform full-text queries on the text in the columns.

	![Full-Text Catalog Properties](images/full-text-catalog-properties.png?raw=true "Full-Text Catalog Properties")
 
	_Full-Text Catalog Properties_

1. Add a new user to connect to the database with the web application you will deploy in the following exercise. To do this, expand **Security** folder within the SQL Server instance node in Object Explorer. Right-click **Logins** folder and select **New Login**.

 	![Creating a New Login](./images/create-new-login.png?raw=true "Creating a New Login")
 
	_Creating a New Login_

1. In the **General** section, type a **Login name**, for example _TestUser_. Select **SQL Server authentication** option and set the **Password**.

	>**Note:** Make note of the username and password as you will need them to update the web.config file for the MVC 4 application you will use in the next exercise to match those values.

1. Uncheck **Enforce password policy** option to avoid having to change the User's password the first time you log on and set the **Default database** to _ContactManagerDb_.

	![New Login's General Settings](images/new-logins-general-settings.png?raw=true "New Login's General Settings")
 
	_Creating a New Login_

1. Go to **User Mapping** section. Map the user to the _ContactManagerDb_ database, select the **db_owner** role for the login, and click **OK**.

 	![Mapping the new User to the AdventureWorks Database](./images/Mapping-the-new-User.png?raw=true "Mapping the new User to the ContactManagerDb Database")
 
	_Mapping the new User to the ContactManagerDb Database_

1. Now you need to make sure the database server has SQL Server Authentication enabled, allowing connections with username and password from SQL Login. To do this, in Object Explorer, right-click the database server node and click **Properties** to open the server properties. 

	> **Note:** To learn more about SQL Server Authentication modes check out this article http://technet.microsoft.com/en-us/library/ms144284.aspx

1. Select the **Security** page and make sure the **Server authentication** is set to **SQL Server and Windows Authentication mode**.

	![Enabling SQL Server Authentication](./images/enabling-sqlserver-authentication.png?raw=true "Enabling SQL Server Authentication")
 
	_Enabling SQL Server Authentication_

1. To apply the change you need to restart SQL Server service. To do this, open **SQL Server Configuration Manager** from **Start | All Programs | Microsoft SQL Server 2012 | Configuration Tools**. Go to the **SQL Server Services** node and right-click the **SQL Server (MSSQLSERVER)** item and select **Restart.**

1. Finally, you will create a _stored procedure_ that will be used by the web application in order to perform searches using **Full Text Search**. To do so, copy and run the following code in a new query window.

	````T-SQL
	USE [ContactManagerDb]
GO
CREATE PROCEDURE SearchContacts
	(@searchQuery nvarchar(4000))
AS
BEGIN
	SET NOCOUNT ON;
    
	SELECT * 
	FROM [dbo].Contacts
	WHERE CONTAINS((FirstName, LastName, Company), @searchQuery);
END
GO
````
1. Close the **SQL Server Management Studio** and close the remote desktop connection.

---

<a name="Exercise2"></a>
### Exercise 2: Creating an MVC 4 Web Application that Connects to the SQL Server 2012 Virtual Machine ###

In this exercise you will create a simple ASP.NET MVC 4 Web application that will connect to the SQL Server created previously using a public endpoint. By the end of the exercise, you will deploy the application to Microsoft Azure Websites using Visual Studio publishing feature and Web Deploy.

>**Note:** To reduce typing, you can right-click where you want to insert source code, select Insert Snippet, select My Code Snippets and then select the entry matching the current exercise step.

<a name="Ex2Task1"></a>
#### Task 1 - Creating the Web Application ####

In this task you will create a simple MVC 4 Web application.

1. Open Microsoft Visual Studio 2010 from **Start | All Programs | Microsoft Visual Studio 2010 | Microsoft Visual Studio 2010**.

1. Open the **Begin.sln** starting solution from the **Source\Ex2-CreatingMVCWebSite\Begin** folder of this lab.

1. Press **CTRL+SHIFT+B** to build the solution and download the NuGet packages required.

1. Create a new class under the **Models** folder of the ContactManager.Web project. To do this, right-click the folder, select **Add** and then **Class**. In the **Add New Item** dialog, set the class name to **Contact**.

	<!-- build to download dependencies -->

1. Replace all the code in the class with following code.

	(Code Snippet - Websites and Virtual Machines using ASP.NET - _Ex2 Contact class_)

	````C#
	namespace ContactManager.Web.Models
	{
		 using System.ComponentModel;
		 using System.ComponentModel.DataAnnotations;

		 public class Contact
		 {        
			  public int Id { get; set; }

			  [Required]
			  [DisplayName("First Name")]
			  public string FirstName { get; set; }

			  [DisplayName("Last Name")]
			  [Required]        
			  public string LastName { get; set; }

			  [DataType(DataType.EmailAddress)]
			  [StringLength(50)]
			  public string Email { get; set; }

			  [StringLength(50, MinimumLength = 3, ErrorMessage = "Must have a minimum length of 3.")]
			  public string Company { get; set; }

			  [DisplayName("Business Phone")]
			  [DataType(DataType.PhoneNumber)]        
			  public string BusinessPhone { get; set; }

			  [DisplayName("Mobile Phone")]
			  [DataType(DataType.PhoneNumber)]        
			  public string MobilePhone { get; set; }

			  [StringLength(50, MinimumLength = 3, ErrorMessage = "Must have a minimum length of 3.")]
			  [DataType(DataType.Text)]
			  public string Address { get; set; }

			  [StringLength(50, MinimumLength = 3, ErrorMessage = "Must have a minimum length of 3.")]
			  [DataType(DataType.Text)]
			  public string City { get; set; }

			  [StringLength(10)]
			  [DataType(DataType.Text)]        
			  public string Zip { get; set; }
		 }
	}
	````
	
	> **Note:** This class uses data annotations to provide more information to the Entity Framework (and to MVC 4) about the classes and the database to which they map to. For example, you can specify that a property be used as the primary key, or you can also set the length of a text field, which will override the default length. These data annotations will also serve as validation rules for your model.
> The Entity Framework Code First allows you to use the System.ComponentModel.DataAnnotations namespace to provide additional information about classes, properties and validation rules. 

1. Create a new class under the **Models** folder. To do this, right-click the  **Models** folder and select **Add | Class**. In the **Add New Item** dialog, set the class name to **ContactContext**.

1. Replace all the code in the class with the following code.

	(Code Snippet - Websites and Virtual Machines using ASP.NET - _Ex2 ContactContext class_)

	````C#
	namespace ContactManager.Web.Models
	{
		 using System.Collections.Generic;
		 using System.Data.Entity;
		 using System.Data.SqlClient;		 

		 public class ContactContext : DbContext
		 {
			  public ContactContext()
					: base("ContactManagerDb")
			  {
			  }

			  public DbSet<Contact> Contacts { get; set; }
		 }
	}
````

	> **Note:** The _ContactContext_ class above is the context class used to map the _Contact_ class to/from the database. It derives from the DbContext base class, provided by Entity Framework, and exposes a _DbSet_ property for the root entity of the model. This set is automatically initialized when the _ContactContext_ class instance is created.
> DbContext wraps the ObjectContext and exposes the most commonly used features of ObjectContext by using more simplified and intuitive API.

1. Now you will create a method for calling the stored procedure that performs the **full text search**. To do this, add the highlighted code at the end of the **ContactContext** class.

	(Code Snippet - Websites and Virtual Machines using ASP.NET - _Ex2 SearchContacts method_)

	<!-- mark:5-9 -->
	````C#
	public class ContactContext : DbContext
	{
		...
		
		public IEnumerable<Contact> SearchContacts(string searchQuery)
		{            
			return this.Database.SqlQuery<Contact>("EXECUTE [dbo].[SearchContacts] @searchQuery", new SqlParameter("searchQuery", searchQuery ?? string.Empty));
		}
	}
	````
	
	> **Note:** the **DbContext.Database.SqlQuery()** method offers a way to execute a SQL command against the database and map the returning result set to a strongly typed object or a list of strongly typed objects that has properties that match the names of the columns returned from the query.

1. In **Solution Explorer**, right-click the **Controllers** folder and select **Add | Controller**.

1. In the **Add Controller** dialog, set the controller name to **ContactController**, select the **Empty controller** template and click **Add**.

	![Adding a controller](images/adding-a-controller.png?raw=true "Adding a controller")

	_Adding a controller_

1. Add the following using statements.

	````C#
	using ContactManager.Web.Models;	
	````

1. Add the following code at the end of the **ContactController** class to create a method that will return a list  of contacts based on search criteria.

	(Code Snippet - Websites and Virtual Machines using ASP.NET - _Ex2 List Contacts Controller Method_)

	````C#
	// GET: /Contact/List
	public ActionResult List(string searchQuery)
	{
		 IEnumerable<Contact> contacts;

		 using (ContactContext context = new ContactContext())
		 {
			  if (!string.IsNullOrEmpty(searchQuery))
			  {
					contacts = context.SearchContacts(searchQuery).ToList();
			  }
			  else
			  {
					contacts = context.Contacts.ToList();
			  }
		 }

		 return View(contacts);
	}
	````

1. Build the project. To do this, right-click on the application's project in **Solution Explorer** and select **Build**.

	![Building the project](images/build-application.png?raw=true "Building the project")

	_Building the project_

1. Now you will create a new view using the **Scaffolding** MVC feature. Right-click the **List** method you just created and select **Add View...** from the context menu.

	![Adding a new view](images/adding-a-new-view.png?raw=true "Adding a new view")

	_Adding a new view_

1. In the **Add View** dialog, check the **Create a strongly-typed view** option and select **Contact** from the _Model class_ list. From the **Scaffold template:** drop-down list, select the **List** option. Click **Add** to create the view.

	![New view properties](images/new-view-properties.png?raw=true "New view properties")

	_New view properties_

1. Remove the following code from the **List** view.

	<!-- strike:1-5 -->
	````HTML
	<h2>List</h2>

	<p>
		 @Html.ActionLink("Create New", "Create")
	</p>
````

1. Add the highlighted code to the **List**
 view you just created to add the title and the search textbox. 

	<!-- mark:5-25 -->
	````HTML
	@{
		 ViewBag.Title = "List";
	}

	<section class="featured">
		 <div class="content-wrapper">
			  <hgroup class="title">
					<h1>Contact Manager Sample</h1>
					<h2>for Microsoft Azure Websites</h2>
			  </hgroup>
			  <p>
				  Create and search for contacts using Full-Text Search in SQL Server 2012 - try queries like &quot;Joh*&quot; (with the quotation marks) and john OR smith OR contoso
			  </p>
		 </div>
	</section>

	@using (Html.BeginForm())
	{
		 <fieldset class="search">
			  <label>Search Contacts by First Name/Last Name/Company: </label>
			  @Html.TextBox("searchQuery")
			  <input type="submit" value="Search" />
		 </fieldset>
	}

	<h3>Results</h3>
	<table>
		 <tr>
			  <th>
					@Html.DisplayNameFor(model => model.FirstName)
	...
	````

1. Remove the highlighted lines of code, as the action links will not be used in this sample.
	<!-- strike:30-34 -->
	````CSHTML
	@foreach (var item in Model) {
		 <tr>
			  <td>
					@Html.DisplayFor(modelItem => item.FirstName)
			  </td>
			  <td>
					@Html.DisplayFor(modelItem => item.LastName)
			  </td>
			  <td>
					@Html.DisplayFor(modelItem => item.Email)
			  </td>
			  <td>
					@Html.DisplayFor(modelItem => item.Company)
			  </td>
			  <td>
					@Html.DisplayFor(modelItem => item.BusinessPhone)
			  </td>
			  <td>
					@Html.DisplayFor(modelItem => item.MobilePhone)
			  </td>
			  <td>
					@Html.DisplayFor(modelItem => item.Address)
			  </td>
			  <td>
					@Html.DisplayFor(modelItem => item.City)
			  </td>
			  <td>
					@Html.DisplayFor(modelItem => item.Zip)
			  </td>
			  <td>
					@Html.ActionLink("Edit", "Edit", new { id=item.Id }) |
					@Html.ActionLink("Details", "Details", new { id=item.Id }) |
					@Html.ActionLink("Delete", "Delete", new { id=item.Id })
			  </td>
		 </tr>
	}
	````


1. Add the highlighted code to show a message when there are no results.

	<!-- mark:3-10 -->
	````HTML
		 </tr>
	}
	@if (Model.Count() == 0)
	{
		 <tr>
			  <td colspan="10">
					No contacts found
			  </td>
		 </tr>
	}

	</table>
	````

1. Add the following code at the end of the view.

	````HTML
<p>
    @Html.ActionLink("Create New", "Create")
</p>
````

1. Press **CTRL+SHIFT+S** to save the changes.

1. Open the **ContactController** class and add the following code to create methods that will handle the **Create** Action.

	(Code Snippet - Websites and Virtual Machines using ASP.NET - _Ex2 Create Contact Controller Method_)

	````C#
	// GET: /Contact/Create
	public ActionResult Create()
	{
		 return View();
	}
			  
	// POST: /Contact/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public ActionResult Create(Contact contact)
	{
		 if (!ModelState.IsValid)
		 {
			  return View(contact);
		 }

		 using (ContactContext context = new ContactContext())
		 {
			  context.Contacts.Add(contact);

			  context.SaveChanges();
		 }

		 return this.RedirectToAction("List");
	}
	````

1. Right-click the **Create** method you just created and select **Add View...** from the context menu.

	![Adding a new view](images/adding-a-new-view2.png?raw=true "Adding a new view")

	_Adding a new view_

1. In the **Add View** dialog, check the **Create a strongly-typed view** option and select **Contact** from the _Model class_ list. From the **Scaffold template:** list select the **Create** option. Click the **Add** button the create the view.

	![New view properties](images/new-view-properties2.png?raw=true "New view properties")

	_New view properties_

1. Add the **AntiForgeryToken** validation as shown below.
	
	<!-- mark: 5 -->
	````HTML
	...

	@using (Html.BeginForm()) {
		 @Html.ValidationSummary(true)
		 @Html.AntiForgeryToken()

		 <fieldset>
			  <legend>Contact</legend>

	...
	````
	
	> **Note:** The **AntiForgery** token renders a hidden token which is validated during post back, ensuring that your app is protected against cross-site request forgery.

1. Press **CTRL+SHIFT+S** to save the changes.

1. Open the **ContactController** class and replace the **Index** method with the following code to show the contacts list when you load the site.

	(Code Snippet - Websites and Virtual Machines using ASP.NET - _Ex2 Index Controller Method_)

	````C#
	// GET: /Contact/
	public ActionResult Index()
	{
		 return this.RedirectToActionPermanent("List");
	}
	````
1. Finally, you will configure the **connection string** which will give **Entity Framework** information regarding your SQL Server and your database. To do this, open the **Web.config** file and locate the following code within the **configuration** section. Replace the **{yourServerAdress}** token with your SQL Server Virtual Machine public URL, **{yourPort}** token with the port number you assigned to your virtual machine public endpoint and **{username}** and **{password}** tokens with the data of the database user you created in Exercise 1. 
	
	<!-- mark:3 -->
	````XML
	<connectionStrings>    
	<!-- SQL Azure Connection String -->
		<add name="ContactManagerDb" connectionString="Data Source={yourServerAdress},{yourPort};Initial Catalog=ContactManagerDb;User Id={username};Password={password};" providerName="System.Data.SqlClient" />       
	</connectionStrings>
	````

	> **Note:** You can grab your server address from Microsoft Azure Portal by going to **Virtual Machines**, then clicking on your virtual machine name and then going to **Dashboard**. Once there, under **Quick glance**, locate **DNS Name** 
> and copy the URL without the _http://_ prefix.
> 
> ![SQL Server Virtual Machine Dashboard](images/sql-server-vm-dashboard.png?raw=true "SQL Server Virtual Machine Dashboard")
> 
> _SQL Server Virtual Machine Dashboard_

1. Press **F5** to run the solution and verify the site is working properly.

	![Application running locally](images/application-running-locally.png?raw=true "Application running locally")

	_Application running locally_


<a name="Ex2Task2"></a>
#### Task 2 - Publishing the Web Application using Visual Studio ####

In this task you will publish using Microsoft Azure Websites the ASP.NET MVC 4 application you have created on the previous task. To do this, you will use Visual Studio web publishing to deploy the web application through Web Deploy.

1. Go to the [Microsoft Azure Management Portal](https://manage.windowsazure.com) and sign with your Windows account credentials.

	![Log on to Microsoft Azure portal](images/login.png?raw=true "Log on to Microsoft Azure portal")

	_Log on to Microsoft Azure Management Portal_

1. Click **New** on the command bar. Click **Web Site** and then **Quick Create**. Provide a URL for the new web site that is available (e.g. _contactmanager_) and click **Create Web Site**. You can leave the **Region** by default.

	> **Note:** A Microsoft Azure Web Site is the host for a web application running in the cloud that you can control and manage. The Quick Create option allows you to deploy a completed web application to Microsoft Azure Websites from outside the portal. It does not include steps for setting up a database.

	![Creating a new Web Site using Quick Create](images/create-website.png?raw=true "Creating a new Web Site using Quick Create")

	_Creating a new web site using Quick Create_

	> **Note:** The **Quick Create** option doesn't give you options for setting up a database. In this lab you don't require it as you are using a Virtual Machine with SQL Server 2012 as database server.

1. Wait until the new web site is created. In the **Websites** list, click the new Web Site under the Name column to access the **Dashboard**.

1. In the **Dashboard** page, under the **quick glance** section, click **Download publish profile**.

	> **Note:** The _publish profile_ contains all of the information required to publish a web application to a Microsoft Azure Web Site for each enabled publication method. The publish profile contains the URLs, user credentials and database strings required to connect to and authenticate against each of the endpoints for which a publication method is enabled. **Microsoft Visual Studio** supports reading publish profiles to automate configuration of these programs for publishing web applications to Microsoft Azure Websites. 

	![Downloading the web site publish profile](images/download-publish-profile.png?raw=true "Downloading the web site publish profile")
	
	_Downloading the web site publish profile_

1. Download the publish profile file to a known location. Later on in this exercise you will see how to use this file to publish a web application to Microsoft Azure Websites from Visual Studio.

	![Saving the publish profile file](images/save-link.png?raw=true "Saving the publish profile")
	
	_Saving the publish profile file_

1. If not already opened, open the MVC 4 application you obtained in the previous task. In the **Solution Explorer**,  right-click the web site project and select **Publish**.

	![Publishing the web site](images/publishing-the-web-site.png?raw=true "Publishing the web site")

	_Publishing the web site_

1. In the **Profile** page, click **Import** and select the profile settings file you downloaded earlier in this Exercise. Click **Next**.

	![Importing the Profile Settings File](images/importing-the-profile-settings-file.png?raw=true "Importing the Profile Settings File")

	_Importing the Profile Settings File_

1. In the **Connection** page, leave the imported values and click **Next**.

	![Setting up Web Deploy connection](images/setting-up-web-deploy-connection.png?raw=true "Setting up Web Deploy connection")

	_Setting up Web Deploy connection_

1. In the **Settings** page, leave the default values and click **Next**.

	![Setting up additional Settings](images/setting-up-additional-settings.png?raw=true "Setting up additional Settings")

	_Setting up additional Settings_

1. In the **Publish** page, click **Publish** to begin the application publishing process.

	![Publish web application preview page](images/publish-web-application-preview-page.png?raw=true "Publish web application preview page")

	_Publish web application preview page_

	> **Note:** If this is the first time you deploy the web site, you will be prompted to accept a certificate. After the message appears, click **Accept**.
	>
	>![Publish web application certificate](images/publish-web-application-certificate.png?raw=true "Publish web application certificate")

1. Once the publishing process finishes, switch back to the Microsoft Azure Management Portal.

1. In the **Dashboard** page of the web site, under the **quick glance** section, click the **Site URL** link in order to browse to your published web site.

	![Opening the Published Site](images/opening-the-published-site.png?raw=true "Opening the Published Site")

	_Opening the published web site_

1. Verify that the web site was successfully published in Microsoft Azure and then close the browser.

	![Application published to Microsoft Azure](images/application-published-to-windows-azure.png?raw=true "Application published to Microsoft Azure")

	_Application published to Microsoft Azure Websites_

---

<a name="Summary"></a>
## Summary ##

In this lab, you have learned how to create ASP.NET web applications that connect to virtual machines running in Microsoft Azure. First, created a virtual machine with SQL Server 2012 installed using the Microsoft Azure Management Portal and configured it to allow external connections. Then, you created a simple ASP.NET MVC 4 web application using Entity Framework that accessed the database in the SQL Server virtual machine. The web application took advantage of Full-Text Search features in SQL Server 2012 to search for contact data.  You completed the lab by deploying the application to Microsoft Azure Websites using Visual Studio.
