<a name="Title" />
# Building Windows 8.1 Applications using Windows Azure Web Sites #

---
<a name="Overview" />
## Overview ##

Apps are at the center of the Windows 8.1 experience. They’re alive with activity and vibrant content. Users are immersed in your full-screen, Windows Store apps, where they can focus on their content, rather than on the operating system.

In this hands-on lab you will learn how to combine the fluency of Windows 8.1 applications with the power of Windows Azure: From a Windows Store application, you will consume an ASP.NET MVC 5 Web API service published in Windows Azure Web Sites, and store your data in a Windows Azure SQL Database. In addition, you will learn how to configure the Windows Push Notification Services (WNS) in your app to send toast notifications from your service to all the registered clients.

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

- Create an ASP.NET MVC 5 Web API service
- Publish the service to Windows Azure Web Sites
- Create a Windows Store application that consumes the Web API service
- Add Push Notifications to the Windows Store application by using WNS Recipe

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Visual Studio Express 2013 for Web][1] or greater
- [Visual Studio 2013 Express for Windows 8.1][2] or greater
- A Windows Azure subscription
	- Sign up for a [Free Trial](http://aka.ms/watk-freetrial)
	- If you are a Visual Studio Professional, Test Professional, Premium or Ultimate with MSDN or MSDN Platforms subscriber, activate your [MSDN benefit](http://aka.ms/watk-msdn) now to start development and test on Windows Azure.
	- [BizSpark](http://aka.ms/watk-bizspark) members automatically receive the Windows Azure benefit through their Visual Studio Ultimate with MSDN subscriptions.
	- Members of the [Microsoft Partner Network](http://aka.ms/watk-mpn) Cloud Essentials program receive monthly credits of Windows Azure at no charge.


[1]:http://www.microsoft.com/visualstudio/
[2]:http://msdn.microsoft.com/en-us/windows/apps/hh852659

<a name="Setup"/>
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.

1. Right-click the **Setup.cmd** file and click **Run as administrator**. This will launch the setup process that will install the Visual Studio code snippets for this lab.

1. If the User Account Control dialog is shown, confirm the action to proceed.
 
>**Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="CodeSnippets"/>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2013 to avoid having to add it manually.

---
<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Building and Consuming an ASP.NET Web API from a Windows Store App](#Exercise1)
1. [Basic Data Binding and Data Access Using Windows Azure SQL Databases and Entity Framework Code First](#Exercise2)
1. [Adding Push Notification Support to your Windows Store Application](#Exercise3)

> **Note:** Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise.
>
>Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.

<a name="Exercise1" />
### Exercise 1: Building and Consuming an ASP.NET Web API from a Windows Store App ###

ASP.NET Web API is a new framework from MVC 5 that facilitates to build and consume HTTP services for a wide range of clients.

In this exercise you will learn the basics of consuming an ASP.NET MVC 5 Web API REST service - hosted in Windows Azure Web Sites - from a Windows Store application. 

For that purpose, you will first create a new Azure Web Site in the portal to host the service. Then, you will create a new ASP.NET MVC 5 Web API project and publish it in Windows Azure from Visual Studio. Once the default service is published, you will create a basic Windows Store client application with a simple list to retrieve the service values.

> **Note:** If you are using Visual Studio 2013 Professional or higher, you are provided with a solution named **End.All.sln** with the Web Api and the StyleUI projects together in the **Source/Ex1-BuildingWebAPI/End** folder of this lab.


<a name="Ex1Task1" />
#### Task 1 – Creating an MVC 5 Web API Service ####

In this task you will create a new MVC 5 Web API project and explore its components. 

>**Note:** You can learn more about ASP.NET Web API  [here](http://www.asp.net/web-api).

1. Open **Visual Studio Express 2013 for Web** and select **File | New Project...** to start a new solution.

	![New Project](Images/new-project.png?raw=true "New Project")

	_Creating a New Project_

1. In the **New Project** dialog, select **ASP.NET Web Application** under the **Visual C# | Web** tab.

	Make sure the **.NET Framework 4.5** is selected, name it _WebApi_, choose a Location and click **OK**.

	![New MVC Project](Images/new-mvc-project.png?raw=true "New MVC Project")

	_New MVC Project_

1. In the **New ASP.NET Project - WebApi** dialog, select **Web API**.

	![New ASP.NET MVC Web API project](Images/new-aspnet-mvc-webapi-project.png?raw=true "New ASP.NET MVC Web API project")

	_New ASP.NET MVC Web API project_

1. You will now explore the structure of an ASP.NET Web API project. Notice that the structure of a Web API project is similar to an MVC project.

	![ASP.NET Web API Project](Images/aspnet-webapi-project.png?raw=true "ASP.NET Web API Project")

	_ASP.NET Web API Project_

	1. **Controllers:**  A controller is an object that handles HTTP requests. If you have worked with ASP.NET MVC, you will notice that they work similarly in Web API, but controllers in Web API derive from the ApiController instead of Controller Class. The first major difference is that actions on Web API controllers return data instead of views.
		The New Project wizard created two other controllers for you when it created the project: Home and Values. 
	
		-The **Home** controller is responsible for serving HTML pages for the site, and is not directly related to Web API. 

		-The **Values** Controller is an example of a Web API controller. 

	1. **Models**: In this folder you will place the classes that represent the data in your application. ASP.NET Web API can automatically serialize your model to JSON, XML, or some other format, and then write the serialized data into the body of the HTTP response message. 

	1. **Routing:** To determine which action to invoke, the framework uses a routing table configured in App_Start/RouteConfig.cs. The project template creates a default HTTP route named "Default".
		
		````C#
		routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
		````

		When the Web API framework receives an HTTP request, it tries to match the URI against one of the route templates in the routing table. Once a matching route is found, Web API selects the controller and the action. For instance, these URIs match the default route:

		-/api/values/

		-/api/values/1
			
			
		>**Note:** The reason for using an 'api' prefix in the route is to prevent collisions with ASP.NET MVC routing, which manages views in the same namespace. That way, you can have a "values" view and a "values" action method at the same time. However, you can change the default prefix and use your own routes. 

1. Press **F5** to run the solution or, alternatively, click the **Start** button located on the toolbar to run the solution. The Web API template home page will open.

	> **Note:** If the web application is not displayed after the deployment, try refreshing the browser a couple of times.

1. In the browser, go to **/api/values** to retrieve the JSON output of the sample service. 

	In the browser, you will be prompted to download a file. Click **Open**. If prompted, choose to open the file with a text editor.
	
	![Retrieving the default  values](Images/retrieving-the-default-webapi-values.png?raw=true "Retrieving the default values")

	_Retrieving the default values_

<a name="Ex1Task2" />
#### Task 2 – Adding a New Windows Azure Web Site from Server Explorer ####

1. Open **Microsoft Visual Studio Express 2013 For Web** and then open **Server Explorer** by selecting **View | Server Explorer**.

1. In **Server Explorer**, right-click the **Windows Azure** node and select **Connect to Windows Azure...**. Sign in using the Microsoft account associated with your Windows Azure account.

	![Connect to Windows Azure](Images/connect-to-windows-azure.png?raw=true)

	_Connect to Windows Azure_

1. After sign in, the **Windows Azure** node is populated with the resources in your Windows Azure subscription.

1. Expand the **Windows Azure** node, right-click the **Web Sites** node and select **Add New Site...**.

	![Add new site](Images/add-new-website.png?raw=true)

	_Add new site_

1. In the **Create site in Windows Azure** dialog box, provide the following information:
	- In the **Site name** box, enter an available name for the Web site.
	- In the **Location** drop-down list, select the region for the web site. This setting specifies which data center your Web site will run in.
	- In the **Database server** drop-down list, select **Create new server**. Alternatively, you can select an existing SQL Server.
	- In the **Database username** and **Database password** boxes, enter the administrator username and password for the SQL Server. If you selected a SQL Server you have created previously, you will be prompted for the password.

1. Click **Create** to create the web site.

	![Create site on Windows Azure](Images/create-site-on-windows-azure.png?raw=true)

	_Create site on Windows Azure_

1. Wait for the new Web site to be created.

	> **Note:** By default, Windows Azure provides domains at _azurewebsites.net_ but also gives you the possibility to set custom domains using the Windows Azure Management Portal (right-click your Web site from Server Explorer and select **Open Management Portal**). However, you can only manage custom domains if you are using certain Web site modes.
	
	> Windows Azure offers 3 modes for users to run their Web sites - Free, Shared, and Standard. In Free and Shared mode, all Web sites run in a multi-tenant environment and have quotas for CPU, Memory, and Network usage. You can mix and match which sites are Free (strict quotas) vs. Shared (more flexible quotas). The maximum number of free sites may vary with your plan. In Standard mode, you choose which sites run on dedicated virtual machines that correspond to the standard Azure compute resources. You can find the Web Sites Mode configuration in the **Scale** menu of your Web site.

	> ![Web Site Modes](Images/web-site-modes.png?raw=true "Web Site Modes")

	> If you are using **Shared** or **Standard** mode, you will be able to manage custom domains for your Web site by going to your Web site’s **Configure** menu and clicking **Manage Domains** under _domain names_.

	> ![Manage Domains](Images/manage-domains.png?raw=true "Manage Domains")

	> ![Manage Custom Domains](Images/manage-custom-domains.png?raw=true "Manage Custom Domains")

1. Once the Web site is created, it will be displayed in Server Explorer under the **Web Sites** node. Right-click the new Web site and select **Open in Browser** to check that the Web site is running.

	![Browsing to the new web site](Images/browsing-to-the-new-web-site.png?raw=true)

	_Browsing to the new Web site_

	![Web site running](Images/website-working.png?raw=true "Web site running")

	_Web site running_

<a name="Ex1Task3" />
#### Task 3 – Publishing the Web API Service to Windows Azure Web Sites ####

1. In **Solution Explorer**, right-click the Web site project and select **Publish...**.

	![Publishing the service](Images/publishing-the-service.png?raw=true "Publishing the service")

	_Publishing the service_

1. In the **Profile** page, click **Import...** to import the publish profile.

	![Publishing profile selection](Images/publishing-profile-profile-selection.png?raw=true)
	
	_Selecting a publishing profile_

1. In the **Import Publish Settings** dialog box, select the **Import from a Windows Azure Web Site** option. If not already signed in, click the **Sign In...** button and sign in using the Microsoft account associated with your Windows Azure account.

1. Select your Web site from the drop-down list, and then click **OK**.

	![Selecting the new website](Images/selecting-the-new-website.png?raw=true "Selecting the new website")

	_Selecting the new website_

1. In the **Connection** page, leave the imported values and click **Validate Connection**. Once the validation is completed, click **Next**.

	> **Note:** Validation is complete once a green checkmark appears to the right of the **Validate Connection** button.

	![Validating connection](Images/validating-connection.png?raw=true)

	_Validating connection_

1. In the **Settings** page, leave the default values and click **Next**.

	![Publish Settings page](Images/publish-settings-page.png?raw=true "Publish Settings page")

	_Publish Settings page_

1. In the **Preview** page, click **Publish**.

	![Publishing a web site](Images/publishing-a-web-site.png?raw=true "Publishing a web site")
	
	_Publishing a web site_

1. When the process is completed the published web site will be opened in your default web browser. Go to **/api/values** to retrieve the default values and test that the Web API service is working successfully.

	![Default web service published](Images/default-web-service-published.png?raw=true "Default web service published")

	_Default web service - Published_

<a name="Ex1Task4" />
#### Task 4 – Creating a Windows Store Client Application ####

In this task you will create a blank Windows Store application that will consume the service you have already running.

1. Open **Visual Studio Express 2013 for Windows 8.1** and select **File | New Project...** to start a new solution.

1. In the **New Project** dialog, select the **Blank** application under the **Visual C# | Windows Store** applications. Name it _Win8Client_ and click **OK**.

	![Add a new Windows Store application basic client project](Images/add-new-style-ui-app-basic-client-project.png?raw=true "Add new Windows Store application basic client project")

	_Add a new Windows Store application basic client project_

	> **Note:** If it's the first time you create a Windows Syle UI application you will be prompted to get a developer license for Windows 8.1 to develop this kind of applications. In the Developer License window, click **I Agree**.
	
	> ![Getting a Developer License](Images/getting-developer-license.png?raw=true "Getting a Developer License")
	
	> You will be requested to sign in using your Microsoft credentials. Do so and click **Sign In**. Now you have a developer license.

	> ![Inserting credentials to obtain a developer license](Images/inserting-credentials-developer-license.png?raw=true "Inserting credentials to obtain a developer license")

	> ![Developer License successfully obtained](Images/developer-license-succesfully-obtained.png?raw=true "Developer License successfully obtained")

1. In the Solution Explorer, right-click **MainPage.xaml** file and select **View Code**.

	![View code behind MainPage.xaml](Images/main-page-view-code.png?raw=true "View code behind MainPage.xaml")

	_View code behind MainPage.xaml_

1. In **MainPage.xaml.cs**, add a reference to the **Windows.Data.Json** assembly.

	<!-- mark:1 -->
	````C#
	using Windows.Data.Json;
	````

1. Add the following method to the **MainPage.xaml.cs** class, to perform an asynchronous call to the Web API service.

	GetItem() instantiates an HttpClient object, which  sends a GET message to the service URL and retrieves a response asynchronously. Then, the response is deserialized and read by the JsonArray object before generating the value list. 
	
	> **Note:** If you want to read more about async methods you can check out [this article](http://msdn.microsoft.com/en-US/vstudio/async). 

	(Code Snippet - _Building Windows 8.1 Apps - Ex1 - GetItems_)
	<!-- mark:1-22 -->
	````C#
	public async void GetItems()
	{
		 var serviceURI = "[YOUR-WINDOWS-AZURE-SERVICE-URI]/api/values";

		 using (var client = new System.Net.Http.HttpClient())
		 using (var response = await client.GetAsync(serviceURI))
		 {
			  if (response.IsSuccessStatusCode)
			  {
					var data = await response.Content.ReadAsStringAsync();
					var values = JsonArray.Parse(data);

					var valueList = from v in values
										 select new
										 {
											  Name = v.GetString()
										 };

					this.listValues.ItemsSource = valueList;
			  }
		 }
	}
	````

1. Replace the value of the placeholder **[YOUR-WINDOWS-AZURE-SERVICE-URL]** with your Windows Azure published web site URL. You can check that value in the web site's dashboard.

	>**Note:** If you want to test the service locally, start the service project, check its URL and its port (e.g. http://localhost:3565/) and use that value.

1. Then, in the **OnNavigateTo** method, add a call to the GetItem method and press **CTRL+S** to save.

	<!-- mark:3 -->
	````C#
	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		this.GetItems();
	}
	````

1. Open **MainPage.xaml** and add the following controls in the grid to display the service values.

	You will add a listbox that will bind against a values list.

	<!-- mark:2-9 -->
	````XML
	<Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">        
	  <ListBox x:Name="listValues" HorizontalAlignment="Left" Width="250" ScrollViewer.VerticalScrollBarVisibility="Visible">
			<ListBox.ItemTemplate>
				 <DataTemplate>
					  <TextBlock Text="{Binding Name}" Width="200" Height="25" FontSize="18" />         
				 </DataTemplate>
			</ListBox.ItemTemplate>
	  </ListBox>
	  <TextBlock HorizontalAlignment="Left" Margin="265,10,0,0" TextWrapping="Wrap" Text="Web API default service values" VerticalAlignment="Top" FontSize="25"/>
	</Grid>
	````

1. Make sure the Windows Store application is the start up project and press **F5** to run the solution. 

	You will see that the values retrieved from the service are listed in the List Box

	![Basic service output](Images/basic-service-output.png?raw=true "basic service output")

	_Service output_

<a name="Exercise2" />
### Exercise 2: Basic Data Binding and Data Access Using Windows Azure SQL Databases and Entity Framework Code First ###

In this exercise, you will learn how to bind your Windows Store application to an ASP.NET Web API service which is using Code First to generate the database from the model in SQL Database.

You will create a new ASP.NET Web API service and use Entity Framework Scaffolding with Code First to generate the service methods and a database in SQL Database.
Finally, you will explore and customize your Windows Store application to consume the service and show a customer list.

> **Note:** If you are using Visual Studio 2013 Professional or higher, you are provided with two solutions named **Begin.All.sln** and **End.All.sln** with the Web Api and the StyleUI projects together in the **Source/Ex2-DataAccess/Begin** and **Source/Ex2-DataAccess/End** respective folders of this lab.

**About Entity Framework Code First**

Entity Framework (EF) is an object-relational mapper (ORM) that enables you to create data access applications by programming with a conceptual application model instead of programming directly using a relational storage schema.

The Entity Framework Code First modeling workflow allows you to use your own domain classes to represent the model that EF relies on when performing querying, change tracking and when updating functions. Using the Code First development workflow, you do not need to begin your application by creating a database or specifying schema!. Instead, you can begin by writing standard .NET classes that define the most appropriate domain model objects for your application, and Entity Framework will create the database for you.

>**Note:** You can learn more about Entity Framework [here](http://www.asp.net/entity-framework).

<a name="Ex2Task1" />
#### Task 1 - Creating an ASP.NET Web API Service with Entity Framework Code First and Scaffolding ####

In this task you will add Entity Framework Scaffolding and Code First to an ASP.NET Web API service. At the end of this task, you will have a basic API service that performs CRUD operations (Create, Read, Update and Delete) implemented and published in Windows Azure Web Sites.

1. Open **Visual Studio Express 2013 for Web** and open the ***WebApi.sln*** solution located under ***Source/Ex2-DataAccess/Begin*** folder. Alternatively, you may continue with the solution that you obtained after completing the previous exercise.

1. Right-click the **Models** folder in the **Solution Explorer** and select **Add | Class**. Name it **Customer.cs**.

	![Adding a model class](Images/add-model-class.png?raw=true "Add model class")

	_Adding a model class_

	> **Note:** You will start by creating a Customer model class, and your CRUD operations in the service will be automatically created using scaffolding features.

1. Replace the Customer.cs class content with the following code. Press **CTRL+S** to save the changes.

	(Code Snippet - _Building Windows 8.1 Apps - Ex2 - Customer class_)
	<!-- mark:1-21 -->
	````C#
	namespace WebApi.Models
	{
		 public class Customer
		 {
			  public int CustomerId { get; set; }

			  public string Name { get; set; }

			  public string Phone { get; set; }

			  public string Address { get; set; }

			  public string Company { get; set; }

			  public string Title { get; set; }

			  public string Email { get; set; }

			  public string Image { get; set; }
		 }
	}
	````

1. In Solution Explorer, right-click the WebApi project and select **Build**.

	> **Note:** If you build the entire solution you will get errors as the Style UI app client is still incomplete.

1. In Solution Explorer, right-click the **Controllers** folder of the Web API project and select **Add | Controller** to open the **Add Controller** dialog.

	![Adding new scaffolded item](Images/adding-new-scaffolded-item.png?raw=true "Adding new scaffolded item")

	_Adding new scaffolded item_

1. In the **Add Scaffold** dialog, select **Web API 2 Controller with actions, using Entity Framework** and then click **Add.**

	![Selecting the Web API 2 controllers with actions](Images/selecting-the-web-api-2-controllers-with-acti.png?raw=true "Selecting the Web API 2 controllers with actions")

	_Selecting the Web API 2 controllers with actions_

1. Set **CustomersController** as the controller name. In the **Scaffolding options**, select the **API controller with read/write actions, using Entity Framework** Template, and **Customer** as the Model class.

	![Adding a controller with Scaffolding](Images/adding-a-controller-with-scaffolding.png?raw=true "Adding a controller with Scaffolding")

	_Adding a controller with Scaffolding_

	> **Note:** If you do not have model classes to select you need to build the project.

1. In the Data context class, select **New data context**. Name the new data context _CustomerContext_ and click **OK**.

	![New customers context](Images/new-customers-context.png?raw=true "New customers context")

	_New customers context_

1. Click **Add** to add the controller. By using the 'API controller with read/write actions and Entity Framework' template, the CRUD operations for customers will be automatically generated in the Web API service.

1. Once the controller with scaffolding is created, open **CustomersController.cs**. Notice that the following CRUD actions were added:
	- DeleteCustomer(int id)
	- GetCustomer(int id)
	- GetCustomers()
	- PostCustomer(Customer customer)	
	- PutCustomer(int id, Customer customer)

	Notice that each operation has the HTTP verb as a prefix in the name (Delete, Get, Post, etc.).

1. Now, you will add a database initializer method in your database context to populate the database with initial data. Add the following **CustomerContextInitializer** class in **CustomerContext.cs** file (under the **Models** folder) after **CustomerContext** class, and save the changes.

	(Code Snippet - _Building Windows 8.1 Apps - Ex2 - Context Initializer_)
	<!-- mark:26-31 -->
	````C#
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Web;

	namespace WebApi.Models
	{
		 public class CustomerContext : DbContext
		 {
			  // You can add custom code to this file. Changes will not be overwritten.
			  // 
			  // If you want Entity Framework to drop and regenerate your database
			  // automatically whenever you change your model schema, please use data migrations.
			  // For more information refer to the documentation:
			  // http://msdn.microsoft.com/en-us/data/jj591621.aspx
		 
			  public CustomerContext() : base("name=CustomerContext")
			  {
			  }

			  public System.Data.Entity.DbSet<WebApi.Models.Customer> Customers { get; set; }
		 
		 }

		 public class CustomerContextInitializer : DropCreateDatabaseIfModelChanges<CustomerContext>
		 {
			  protected override void Seed(CustomerContext context)
			  {
			  }
		 }
	}
	````

	>**Note:** Code First allows us to insert into our database by using a database initializer and overriding the Seed method. In this case the class inherits from **DropCreateDatabaseIfModelChanges\<TContext\>**, where TContext is CustomerContext.
	>
	> The **DropCreateDatabaseIfModelChanges\<TContext\>** class is an implementation of **IDatabaseInitializer\<TContext\>** that will delete, recreate, and optionally reseed the database with data only if the model has changed since the database was created. This is achieved by writing a hash of the store model to the database when it is created and then comparing that hash with one generated from the current model.
	> 
	> Alternatively, you can use **CreateDatabaseIfNotExists\<TContext>**, which recreates and optionally re-seeds the database with data only if the database does not exist, or **DropCreateDatabaseAlways\<TContext>**, which always recreates and optionally re-seeds the database with data the first time that a context is used in the application domain.
	

1. In the CustomerContextInitializer **Seed** method, add the following customers to the context to populate the database with customers.

	(Code Snippet - _Building Windows 8.1 Apps - Ex2 - Context Initializer Seed_)
	<!-- mark:3-102 -->
	````C#
	protected override void Seed(CustomerContext context)
	{
		context.Customers.Add(new Customer
		{
			 Name = "Catherine Abel", Email = "catherine.abel@vannuys.com",
			 Company = "Van Nuys", Phone = "541 555 0100",
			 Address = "1 Microsoft Way, Redmond, WA, 98052",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.Customers.Add(new Customer
		{
			 Name = "Kim Branch", Email = "kim.branch@contoso.com",
			 Company = "Contoso", Phone = "541 555 0100",
			 Address = "1 Microsoft Way, Redmond, WA, 98052",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.Customers.Add(new Customer
		{
			 Name = "Frances Adams", Email = "frances.adams@contoso.com",
			 Company = "Contoso", Phone = "541 555 0100",
			 Address = " 1 Microsoft Way, Redmond, WA, 98052",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.Customers.Add(new Customer
		{
			 Name = "Mark Harrington", Email = "mark.harrington@datum.com",
			 Company = "A. Datum Corporation", Phone = "541 555 0100",
			 Address = "1 Microsoft Way, Redmond, WA, 98052",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.Customers.Add(new Customer
		{
			 Name = "Keith Harris", Email = "keith.harris@adventureworks.com", 
			 Company = "Adventure Works", Phone = "541 555 0100",
			 Address = "1 Microsoft Way, Redmond, WA, 98052",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.Customers.Add(new Customer
		{
			 Name = "Roger Harui", Email = "roger.harui@baldwinmuseum.com",
			 Company = "Baldwin Museum of Art", Phone = "541 555 0100",
			 Address = "1 Microsoft Way, Redmond, WA, 98052",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.Customers.Add(new Customer
		{
			 Name = "Pilar Pinilla", Email = "pilar.pinilla@blueyonderairlines.com",
			 Company = "Blue Yonder Airlines", Phone = "541 555 0100",
			 Address = "1 Microsoft Way, Redmond, WA",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.Customers.Add(new Customer
		{
			 Name = "Kari Hensien", Email = "kari.hensien@citypowerlight.com",
			 Company = "City Power & Light", Phone = "541 555 0100",
			 Address = "1 Microsoft Way, Redmond, WA",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.Customers.Add(new Customer
		{
			 Name = "Johny Porter", Email = "johny.porter@cohowinery.com",
			 Company = "Coho Winery", Phone = "541 555 0100",
			 Address = "1 Microsoft Way, Redmond, WA",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.Customers.Add(new Customer
		{
			 Name = "Peter Brehm", Email = "peter.brehm@cohowinery.com",
			 Company = "Coho Winery", Phone = "541 555 0100",
			 Address = "1 Microsoft Way, Redmond, WA",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.Customers.Add(new Customer
		{
			 Name = "John Smith", Email = "john.smith@contoso.com",
			 Company = "Contoso", Phone = "541 555 0100",
			 Address = "1 Microsoft Way, Redmond, WA",
			 Image = "Assets/CustomerPlaceholder.png",
			 Title = "Sales"
		});

		context.SaveChanges();
	}
	````

1. Open **Global.asax.cs** and add a reference to **WebApi.Models** and **System.Data.Entity**.

	<!-- mark:1-2 -->
	````C#
	using WebApi.Models;
	using System.Data.Entity;
	````

1. Add the database initializer in the **Application_Start** method in Global.asax.cs.

	<!-- mark:5 -->
	````C#
	protected void Application_Start()
	{
		...
	
		Database.SetInitializer<CustomerContext>(new CustomerContextInitializer());
	}
	````

<a name="Ex2Task2" />
#### Task 2 - Publishing the Customers Web API Service to Windows Azure ####

In this task you will publish the updated Web API service in Windows Azure Web Sites replacing the connection string to use a SQL Database.

1. In **Solution Explorer**, right-click the Web API service project and select **Publish...**.

	![Publishing the service](Images/publishing-the-service.png?raw=true "Publishing the service")

	_Publishing the service_

1. In the **Preview** page, click **Prev** button to navigate to the **Settings** page in order to configure the SQL database.

	![Navigating to the Settings Page](Images/navigating-to-the-settings-page.png?raw=true "Navigating to the Settings Page")

	_Navigating to the Settings Page_

1. In the **Settings** page, under the **Databases** section, if the **CustomerContext** section is not expanded, click the down arrow in the **CustomerContext** textbox, and select the SQL database shown in the drop-downlist.

	> **Note:** The SQL Database you just selected was automatically generated when you created the Windows Azure Web Site in [Exercise 1](#Exercise1).

	![Selecting the SQL Database](Images/selecting-the-sql-database.png?raw=true)

	_Selecting the SQL Database_

1. Click **Next** and then in the **Preview** page, click **Publish**.

	![Publishing a web site](Images/publishing-a-web-site.png?raw=true "Publishing a web site")
	
	_Publishing a web site_

1. When the process is completed the published web site will be opened in your default web browser. Go to **/api/customers** to retrieve the full list of customers.

	![Testing the Customers Web API](Images/testing-the-customers-web-api.png?raw=true "Testing the Customers Web API")

	_Testing the Customers Web API_

	>**Note:** Entity Framework will create the database schema the first time you run the application. You can also access the database tables in Windows Azure portal and check if the data was added.

<a name="Ex2Task3" />
#### Task 3 - Exploring the Windows Store Application ####

In this task you will explore the Customer client application, built using a Windows Store application Grid Template. You will perform a brief lap around and learn about the main components of a Style UI Grid application.

1. Open **Visual Studio Express 2013 for Windows 8.1** and open the **CustomerManager.sln** solution located under **Source/Ex2-DataAccess/Begin** folder.

1. This is a client Windows Store application that displays customers. It is based on the Visual Studio Grid template.

	>**Note:** The Grid application is one of the Visual Studio 2013 available templates for Windows Store applications, which contains three pages. The first page displays a group of items in a grid layout. When a group is clicked, the second page shows the details of the selected group. Finally, when an item is selected, the third page shows the item details.

	In this solution you will find a simplified Grid template, which only contains group and detail pages with a custom data model for customers.

	![CustomerManager Windows Store Application](Images/customermanager-app.png?raw=true "CustomerManager Windows Store Application")

	_CustomerManager Windows Store Application_

	The main application pages are the following ones:
	- _GroupedCustomersPage:_ Shows the customers in a grid layout
	- _CustomerDetailPage:_ Shows customer's details
	- _NewCustomerPage:_ Adds a new customer

1. Expand the **ViewModels** folder and open **GroupedCustomersViewModel.cs**. 

	The XAML pages of this solution are bound against ViewModels classes that retrieve and prepare the necessary data that will be displayed.
	
	GroupedCustomersViewModel contains an ObservableCollection of CustomerViewModel and a method to retrieve the customers asynchronously (in the next exercise you will complete the CustomersWebApiClient call).


	````C#
    public class GroupedCustomersViewModel : BindableBase
    {
        public ObservableCollection<CustomerViewModel> CustomersList { get; set; }

        public GroupedCustomersViewModel()
        {
            this.CustomersList = new ObservableCollection<CustomerViewModel>();

            this.GetCustomers();
        }

        private async void GetCustomers()
        { 
            IEnumerable<Customer> customers = await CustomersWebApiClient.GetCustomers();

            foreach (var customer in customers)
            {
                this.CustomersList.Add(new CustomerViewModel(customer));                
            }        
        }
    }
	````

	> **Note:** The Windows Runtime now supports using ObservableCollection to set up dynamic bindings so that insertions or deletions in the collection update the UI automatically.

	The **GroupedCustomersPage.xaml.cs** code-behind declares, initializes and binds the view model as follows.

	<!--mark:4,8-11-->
	````C#
	public sealed partial class GroupedCustomersPage : Page
	{
		private NavigationHelper navigationHelper;
		private GroupedCustomersViewModel viewModel = new GroupedCustomersViewModel();

		...

		public GroupedCustomersViewModel ViewModel
		{
			get { return this.viewModel; }
		}

		...
	}
	````

	In the XAML code of this page, each collection is bound to the ViewModel through a CollectionViewSource, that points to the customers list from the ViewModel.

	````XML
	<Page.Resources>
	  <CollectionViewSource
			x:Name="groupedItemsViewSource"
			Source="{Binding CustomersList}"            
			IsSourceGrouped="false" />
	</Page.Resources>

	...	
	````

	Then, each of the page elements (lists, grids, etc.) use the defined collection view source and bind to specific properties.

	<!-- mark:7,16,19-20 -->
	````XML
	<GridView
		 x:Name="itemGridView"
		 AutomationProperties.AutomationId="ItemGridView"
		 AutomationProperties.Name="Grouped Items"
		 Grid.RowSpan="2"
		 Padding="116,137,40,46"
		 ItemsSource="{Binding Source={StaticResource groupedItemsViewSource}}"
		 SelectionMode="None"
		 IsSwipeEnabled="false"
		 IsItemClickEnabled="True"
		 ItemClick="CustomerItem_Click">
		 <GridView.ItemTemplate>
			  <DataTemplate>
					<Grid HorizontalAlignment="Center" Width="300" Height="225" Margin="0,0,0,0">
						 <Border Background="{ThemeResource ListViewItemPlaceholderBackgroundThemeBrush}">
							  <Image Source="{Binding ImagePath}" Stretch="UniformToFill" AutomationProperties.Name="{Binding Title}"/>
						 </Border>
						 <StackPanel VerticalAlignment="Bottom" Background="{ThemeResource ListViewItemOverlayBackgroundThemeBrush}">
							  <TextBlock Text="{Binding Name}" Foreground="{ThemeResource ListViewItemOverlayForegroundThemeBrush}" Style="{StaticResource TitleTextBlockStyle}" Height="60" Margin="15,0,15,0"/>
							  <TextBlock Text="{Binding Company}" Foreground="{ThemeResource ListViewItemOverlaySecondaryForegroundThemeBrush}" Style="{StaticResource CaptionTextBlockStyle}" TextWrapping="NoWrap" Margin="15,0,15,10"/>
						 </StackPanel>
					</Grid>
			  </DataTemplate>
		 </GridView.ItemTemplate>
		 <GridView.ItemsPanel>
			  <ItemsPanelTemplate>
					<ItemsWrapGrid GroupPadding="0,0,70,0"/>
			  </ItemsPanelTemplate>
		 </GridView.ItemsPanel>
	</GridView>
	````

<a name="Ex2Task4" />
#### Task 4 - Integrating the Web API Service with the Windows Store Application ####

In this task you will bind your Windows Store Application against your customer's model retrieving data from the Web API service. You will start by configuring the binding, and then you will modify the application to call the service asynchronously and display the customers.

1. Right-click the **DataModel** project folder in the solution explorer and select **Add | Existing Item**. 

	![Adding an existing item](Images/adding-an-existing-item.png?raw=true "Adding an existing item")
	
	_Adding an existing item_

1. Browse to the **WebApi** project, open the **Models** folder and select **Customer.cs**. Click the arrow next to the **Add** button and click **Add as Link**.

	![Adding Customer.cs model class as a link](Images/add-customercs-as-an-existing-item.png?raw=true "Adding Customer.cs model class as a link")

	_Adding Customer.cs model class as a link_

	By adding the Customer class as a link, your application will be using the same model class from the Web API service. This class will serve as data contract between the Web API service and the Style UI app.

1. Right-click the **DataModel** project folder in the Solution Explorer and select **Add | Class**. Name it _CustomersWebApiClient.cs_. 

	This class will contain the methods that retrieve the customers from the Web API service you have already published on Windows Azure.

	![Adding a new class](Images/adding-a-new-class.png?raw=true "Adding a new class")

	_Adding a new class_

1. 	Open the **CustomersWebApiClient.cs** class and add the following using directives. 

	(Code Snippet - _Building Windows 8.1 Apps - Ex2 - CustomersWebApiClient namespace_)

	<!-- mark:1-4 -->
	````C#
	using System.IO;
	using System.Net.Http;
	using System.Runtime.Serialization.Json;
	using WebApi.Models;
	````

	>**Note:** WebApi.Models references your service models, and it is necessary to use the customer class you have added as a link.
	
1. In the CustomersWebApiClient class, add the following **GetCustomers()** method.

	(Code Snippet - _Building Windows 8.1 Apps - Ex2 - CustomersWebApiClient GetCustomers Method_)

	<!-- mark:1-18 -->
	````C#
	public static async Task<IEnumerable<Customer>> GetCustomers()
	{            
		object serviceUrl;
		App.Current.Resources.TryGetValue("ServiceUrl", out serviceUrl);

		using (HttpClient client = new HttpClient())
		{
			 HttpResponseMessage response = await client.GetAsync(serviceUrl as string);

			 response.EnsureSuccessStatusCode();

			 using (var stream = await response.Content.ReadAsStreamAsync())
			 {                    
				  DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<Customer>));
				  return serializer.ReadObject(stream) as IEnumerable<Customer>;                                       
			 }                
		}            
	}
	````

	This method performs an asynchronous call to the Web API service. After the data is retrieved, the method uses the [DataContractJSonSerializer](http://msdn.microsoft.com/en-us/library/system.runtime.serialization.json.datacontractjsonserializer(v=vs.110\).aspx) to read the array of customers, using the Customers data contract defined.

	The Web API service URL is retrieved from a resources dictionary in App.xaml.

1. Now add the **CreateCustomer** method to post new customers to the service.

	(Code Snippet - _Building Windows 8.1 Apps - Ex2 - CustomersWebApiClient CreateCustomer Method_)

	<!-- mark:1-21 -->
	````C#
	public static async void CreateCustomer(Customer customer)
	{
		object serviceUrl;
		App.Current.Resources.TryGetValue("ServiceUrl", out serviceUrl);

		using (HttpClient client = new HttpClient())
		{
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Customer));
			
			using (MemoryStream stream = new MemoryStream())
			{                    
				serializer.WriteObject(stream, customer);                    
				stream.Seek(0, SeekOrigin.Begin);
				
				var json = new StreamReader(stream).ReadToEnd();

				var response = await client.PostAsync(serviceUrl as string, new StringContent(json, Encoding.UTF8, "application/json"));
				response.EnsureSuccessStatusCode();
			}
		}
	}
	````

	This method executes an asynchronous post to the Web API service, sending the customer serialized in JSON. 

1. Finally, you will configure the ServiceUrl value using your Web API service URL. Open **App.xaml** and locate the **ServiceURL** key. Change the key value using your service site URL.

	````XML
	<x:String x:Key="ServiceUrl">[YOUR-SERVICE-SITE-URL]/api/customers</x:String>
	````

	>**Note:** You can find your service URL in the  dashboard of your Windows Azure Web Sites.

<a name="Ex2Task5" />
#### Task 5 - A Lap Around the Customer Manager Application ####

1. Press **F5** to run the solution.

	![Customer Manager Grid](Images/customer-manager-grid.png?raw=true "Customer Manager Grid")

	_Customer Manager - Grid_

1. Click on a customer to open the **Customer Details** page. Notice that you can use the arrows on the right and left sides of the screen to browse through the customers.

1. Click the upper left arrow to go back.

	![Customer Details](Images/customer-details.png?raw=true "Customer Details")

	_Customer Details_

1. Back in the Home page, right-click to bring the application bar up and select **Add** to go to the new customer page.

	![Add button](Images/add-button.png?raw=true "Add button")

	_Add button_

1. Complete the new customer's data and click **Create**.

	![New Customer](Images/new-customer.png?raw=true "New Customer")

	_New Customer_

1. Back in the home page, you will see the new customer added.

	>**Note:** If you cannot see the new customer added, go to the details page and come back to the home page. As the CreateCustomer method is asynchronous, to avoid blocking the UI, the GetCustomers method might execute before the new customer is posted to the service.

<a name="Exercise3" />
### Exercise 3: Adding Push Notification Support to your Windows Store Application ###

The Windows Push Notification Services (WNS) enables third-party developers to send toast, tile, and badge updates from their own web site. This provides a mechanism to deliver new updates to your users in a power-efficient and dependable way.

The process of sending a notification requires few steps:

1. **Request a channel.** Utilize the WinRT API to request a Channel Uri from WNS. The Channel Uri will be the unique identifier you use to send notifications to an application instance.

1. **Register the channel with your Windows Azure Web Site.** Once you have your channel you can then store your channel and associate it with any application specific data (e.g user profiles and such) until your services decide that it’s time to send a notification to the given channel.

1. **Authenticate against WNS.** To send notifications to your channel URI you are first required to Authenticate against WNS using OAuth2 to retrieve a token to be used for each subsequent notification that you push to WNS.

1. **Push notification to channel recipient.** Once you have your channel, notification payload and WNS access token you can then perform an HttpWebRequest to post your notification to WNS for delivery to your client.

	![WNS Flow Diagram](Images/wns-flow-diagram.png?raw=true "WNS Flow Diagram")

	_WNS Flow Diagram_

In this exercise you will learn how to send a toast notification from the Web API service (Web Site) to the registered clients (Windows Store applications) whenever a new customer is added.

A toast notification is a transient message to the user that contains relevant, time-sensitive information and provides quick access to related content in an app. It can appear whether you are in another app, the Start screen, the lock screen, or on the desktop. Toasts should be viewed as an invitation to return to your app to follow up on something of interest.

> **Note:** If you are using Visual Studio 2013 Professional or higher, you are provided with two solutions named **Begin.All.sln** and **End.All.sln** with the Web Api and the StyleUI projects together in the **Source/Ex3-Notifications/Begin** and **Source/Ex3-Notifications/End** respective folders of this lab.

<a name="Ex3Task1" />
#### Task 1 - Registering the Customer Manager Application for Push Notifications####

Before you can send notifications through WNS, you must register your application with the Windows developer center that supports the end-to-end process for submitting, certifying, and managing applications for sale in the Windows Store. When you register your application with the Dashboard, you are given credentials — a Package security identifier (SID) and a secret key — which your Web Site will use to authenticate itself with WNS.

In this task you will obtain the information that will be needed to enable your application to communicate with WNS and Live Connect.

1. In Visual Studio, continue working with the solutions obtained from the previous exercise. If you did not executed the previous exercise you can open **WebApi.sln** with **Visual Studio Express 2013 for Web** and **CustomerManager.sln** with **Visual Studio Express 2013 for Windows 8.1**, both located in the **Source/Ex3-Notifications/Begin** folder of this lab.

1. If you opened the **WebApi** begin solution, open **Web.config** and configure the **CustomerContext** connection string to point to a Windows Azure SQL Database. You can use the connection string below replacing the placeholders. [Exercise 2 - Task 3](#Ex2Task3) instructs how to do this. Then build the solution.

	````XML
	<add name="CustomerContext" connectionString="Server=tcp:[SERVER_URL],1433;Database=CustomersDB;User ID=[SERVER_ADMIN_LOGIN];Password=[SERVER_ADMIN_PASSWORD];Trusted_Connection=False;Encrypt=True;Connection Timeout=30;" providerName="System.Data.SqlClient" />
	````

1. In the **CustomerManager** begin solution, open **Package.appxmanifest**.

    > **Note:** The package manifest is an XML document that contains the info the system needs to deploy, display, or update a Windows Store app. This info includes package identity, package dependencies, required capabilities, visual elements, and extensibility points. Every application package must include one package manifest.

1.	Click **Store** in the Visual Studio menu and select **Reserve App Name**.

	![Reserving App Name](./Images/reserving-app-name.png?raw=true)

	_Reserving App Name in Windows Store_

1.	The browser will display the Windows Store page that you will use to obtain your WNS credentials. In the Submit an app section, click **App Name**.

	> **Note:** You will have to sign in using your Microsoft Account to access the Windows Store.

	![Giving your app a unique name](./Images/giving-app-name-windows-store.png?raw=true)

	_Giving your app a unique name_

1.	In the App name field, insert the Package Display Name that is inside the **Package.appxmanifest** file of your solution and click **Reserve app name**. Then click **Save** to confirm the reservation.

	![Reserving an app name](./Images/app-name-windows-store.png?raw=true)

	_Reserving an app name_

	![Confirming the app name reservation](./Images/name-reservation-successful-win-store.png?raw=true)

	_Confirming the app name reservation_

1. Now you will have to identify your application to get a name and a publisher to insert in the **Package.appxmanifest** file. In the Submit an app page, click **Advanced features**.

	![Configuring push notifications for the Notifications.Client app](./Images/app-name-reverved-completely-windows-store.png?raw=true)

	_Configuring push notifications for the Notifications.Client app_

1. In the Advanced features page, click **Push notifications and Live Connect services info**.

	![Advanced features page](./Images/push-notif-live-connect-service-info.png?raw=true)

	_Advanced features page_

1. Once in the Push notifications and Live Connect services info section, click **Identifying your app**.

	![Push notifications Overview page](./Images/identifying-your-app.png?raw=true)

	_Push notifications Overview page_

1. Now we have to set the Identity Name and Publisher of our **Package.appxmanifest** file with the information in Windows Store. Go back to Visual Studio, right-click the **Package.appxmanifest** and select **View Code**. Replace the Name and Publisher attributes of the Identity element with the ones obtained in Windows Store. Click **Authenticating your service**.

	![Setting Identity Name and Publisher](./Images/app-identification.png?raw=true)

	_Setting Identity Name and Publisher_

1. Finally we obtained a **Package Security Identifier (SID)** and a **Client secret**, which are the WNS Credentials that we need to update the Web configuration of our Notification App Server.

	![Package Security Identifier (SID) and Client secret](./Images/sid-client-secret.png?raw=true)

	_Package Security Identifier (SID) and Client secret_

	> **Note:** To send notifications to this application, your Web Site must use these credentials exactly. You cannot use another Web Site credentials to send notifications to this application, and you cannot use these credentials to send notifications to another app.


<a name="Ex3Task2" />
#### Task 2 - Enabling Push Notifications####

In this task you will configure your application  to be capable of raising toast notifications. Then, you will create the necessary classes required for sending and receiving push notifications in both the WebApi and the CustomerManager.StyleUI solutions.

1. Go back to Visual Studio, open the application manifest and select the **Application.UI** tab.

1. Find the **Notifications** section and set **Yes** for **Toast capable**.

    ![Enabling toast notifications](Images/enabling-toast-notifications.png?raw=true "Enabling toast notifications")

    _Enabling toast notifications_

1. Switch to the **Capabilities** tab and mark the following capabilities:
    - Internet (Client)
    - Internet (Client & Server)
    - Private Networks (Client & Server)

    ![Enabling network capabilities](Images/enabling-network-capabilities.png?raw=true "Enabling network capabilities")

    _Enabling network capabilities_

	> **Note:** In the following steps you will associate your application with the Windows Store. If you obtained your WNS credentials from the Windows Push Notifications & Live Connect Portal, you can skip the next 5 steps.

1.	Click **Store** in the Visual Studio menu and select **Associate App with the Store**.

	![Associating App with Store](./Images/associating-app-with-store.png?raw=true)

	_Associating App with Store_

1. In the Associate Your App with the Windows Store wizard, click **Sign In**.

	![Associating App with Store Wizard](./Images/associate-app-with-store.png?raw=true)

	_Associating App with Store Wizard_

1. Enter your credentials and click **Sign In**.

	![Inserting your credentials to assciate your app in Windows Store](./Images/sign-in-for-association.png?raw=true)

	_Inserting your credentials to assciate your app in Windows Store_

1. In the Select an app name step, select **Notifications.Client** and click **Next**.

	![Selecting your app name](./Images/selecting-app-name.png?raw=true)

	_Selecting your app name_

1. Take a look at the summary of the values that will be added in the manifest file. Click **Associate**. 

	![Associating your app with the Windows Store Summary](./Images/association-summary.png?raw=true)

	_Associating your app with the Windows Store Summary_

1. Go to the **WebApi** solution and open the **Package Manager Console** from the **Tools | Library Package Manager** menu.

1. In **Default project** make sure **WebApi** is selected.

1. Execute the following command to install the packages required for WNS Recipe.

    ````PowerShell
	Install-Package WnsRecipe
    ````

    > **Note:** The Windows Push Notification Service Recipe (**WnsRecipe**) is a push notification server-side helper library that provides an easy way to send all three types of push notification messages supported by Windows Push Notification Services (WNS): Tile, Toast, and Badge.

1. Inside the **Models** folder, create a new class and name it **Channel**. Replace its content with the following one:

	````C#
	namespace WebApi.Models
	{
		 using System.Runtime.Serialization;

		 [DataContract]
		 public class Channel
		 {
			  [DataMember]
			  public int? Id { get; set; }

			  [DataMember]
			  public string Uri { get; set; }
		 }
	}
	````

1. Open **CustomerContext.cs** from the **Models** folder and add the following line of code after the declaration of the Customers DbSet in the CustomerContext class.

	(Code Snippet - _Building Windows 8.1 Apps - Ex3 - Channels DbSet_)
	
	<!-- mark:7 -->
	````C#
	public class CustomerContext : DbContext
	{
		...

		public DbSet<Customer> Customers { get; set; }

		public DbSet<Channel> Channels { get; set; }
	}
	````

1. Now create a new class inside the **Controllers** folder named **ChannelController** and add the following using directives.

	````C#
	using System.Web.Http;
	using WebApi.Models;
	````

1. Make the ChannelController inherit from the **ApiController** class by adding the following highlighted code:

	<!-- mark:1 -->
	````C#
	public class ChannelController : ApiController
	{
	}
	````

1. Insert the following member to have the CustomerContext available to perform the necessary operations with Entity Framework.
	
	(Code Snippet - _Building Windows 8.1 Apps - Ex3 - Customer Context Instantiation_)

	<!-- mark:3 -->
	````C#
	public class ChannelController : ApiController
	{
		private CustomerContext db = new CustomerContext();
	}
	````

1. Add the following **Create** method to process the create channel requests that you will implement later in this exercise in the Store app.

	(Code Snippet - _Building Windows 8.1 Apps - Ex3 - Create Channel Method_)

	<!-- mark:5-24 -->
	````C#
	public class ChannelController : Controller
	{
		...
		
		//
		// POST: /Channel/Create
		public Channel Create(Channel channel)
		{
			Channel ch = null;

			if (ModelState.IsValid)
			{
				 ch = db.Channels.Find(channel.Id);

				 if (ch == null)
				 {
					  db.Channels.Add(channel);
					  db.SaveChanges();
					  return channel;
				 }
			}

			return ch;
		}
	}
	````

1. Add a **Dispose** method at the end of the controller.

	(Code Snippet - _Building Windows 8.1 Apps - Ex3 - Channel Controller Dispose Method_)

	<!-- mark:5-9 -->
	````C#
	public class ChannelController : Controller
	{
		...
		
		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
	````

1. Switch back to the **CustomerManager.StyleUI** solution and add a link to the _Channel_ class that you created early in the WebApi solution. To do this, right-click the **DataModel** folder and select **Add | Existing Item**. Browse to the **Source/Ex3-Notifications/Begin/WebApi/Models** folder and select **Channel.cs**. Now, click the down arrow next to the _Add_ button and select **Add As Link**.

	![Adding a Link to the Channel Entity](Images/adding-link-to-channel-entity.png?raw=true "Adding a Link to the Channel Entity ")

	_Adding a Link to the Channel Entity_
	
1. Inside the **DataModel** folder, add a new class named **ChannelWebApiClient** with the following using directives.

	````C#
	using System.IO;
	using System.Net.Http;
	using System.Runtime.Serialization.Json;
	using WebApi.Models;
	````

1. Insert the following **RegisterChannel** method that will call the ChannelController of the WebApi to create a new channel for the current application.

	(Code Snippet - _Building Windows 8.1 Apps - Ex3 - Register Channel Method_)

	````C#
	public static async Task<Channel> RegisterChannel(Channel channel)
	{
		object channelServiceUrl;
		App.Current.Resources.TryGetValue("ChannelServiceUrl", out channelServiceUrl);

		using (HttpClient client = new HttpClient())
		{
			 DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Channel));

			 using (MemoryStream stream = new MemoryStream())
			 {
				  serializer.WriteObject(stream, channel);
				  stream.Seek(0, SeekOrigin.Begin);

				  var json = new StreamReader(stream).ReadToEnd();

				  var response = await client.PostAsync(channelServiceUrl as string + "/create", new StringContent(json, Encoding.UTF8, "application/json"));
				  response.EnsureSuccessStatusCode();

				  using (var responseStream = await response.Content.ReadAsStreamAsync())
				  {
						DataContractJsonSerializer responseSerializer = new DataContractJsonSerializer(typeof(Channel));
						return responseSerializer.ReadObject(responseStream) as Channel;
				  }
			 }
		}
	}
	````

<a name="Ex3Task3" />
#### Task 3 - Sending Push Notifications ####

To send a notification, the Web Site must be authenticated through WNS. The first step in this process occurs when you register your application with the Windows Store Dashboard. During the registration process, your application is given a Package security identifier (SID) and a secret key. This information is used by your Web Site to authenticate with WNS.

The WNS authentication scheme is implemented using the client credentials profile from the [OAuth 2.0](http://go.microsoft.com/fwlink/?linkid=226787) protocol. The Web Site authenticates with WNS by providing its credentials (Package SID and secret key). In return, it receives an access token. This access token allows a Web Site to send a notification. The token is required with every notification request sent to the WNS.

1. Open the **CustomersController.cs** file from the WebApi project and add the following using directives.
    
    ````C#
	using System.Configuration;
	using NotificationsExtensions;
	using NotificationsExtensions.ToastContent;
    ````

1. Add the following private method to send a toast notification about the new customers.

	(Code Snippet - _Building Windows 8.1 Apps - Ex3 - SendNotification_)

	````C#
	private void SendNotification(Customer customer)
	{
		var clientId = ConfigurationManager.AppSettings["ClientId"];
		var clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
		var tokenProvider = new WnsAccessTokenProvider(clientId, clientSecret);
		var notification = ToastContentFactory.CreateToastText02();

		notification.TextHeading.Text = "New customer added!";
		notification.TextBodyWrap.Text = customer.Name;

		var channels = db.Channels;

		foreach (var channel in channels)
		{
			 var result = notification.Send(new Uri(channel.Uri), tokenProvider);
		}
	}
	````

	> **Note:** A channel is a unique address that represents a single user on a single device for a single application or secondary tile. Using the channel URI, the Web Site can send a notification whenever it has an update for the user. With the **NotificationServiceContext** we can get the full list of the client endpoints registered with the Web Site.

1. Find the **PostCustomer** function and add a call to **SendNotification** method.

	(Code Snippet - _Building Windows 8.1 Apps - Ex3 - Call to Send Notification_)

    <!-- mark:9 -->
    ````C#
	// POST api/Customers
	public HttpResponseMessage PostCustomer(Customer customer)
	{
		if (ModelState.IsValid)
		{
			db.Customers.Add(customer);
			db.SaveChanges();

			this.SendNotification(customer);

			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, customer);
			response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = customer.CustomerId }));
			return response;
		}
		else
		{
			return Request.CreateResponse(HttpStatusCode.BadRequest);
		}
	}
    ````
 
1. Open the **Web.config** file and add the following settings in the `appSettings` section. Replace the placeholders using the values that you obtained either from the Windows Store or from the Windows Push Notifications & Live Connect Portal.

    <!-- mark:2-3 -->
    ````XML
      ...
      <add key="ClientId" value="[Package Security Identifier (SID)]"/>
      <add key="ClientSecret" value="[Client secret]"/>
    </appSettings>
    ````

    > **Note:** For demo purposes we simply store these values in the Web.config file, but the Package security identifier SID and client secret should be securely stored. Disclosure or theft of this information could enable an attacker to send notifications to your users without your permission or knowledge.

1. Publish the Customers Web API service in Windows Azure. To do this, follow the steps in [Exercise 2, Task 3](#Ex2Task3).

<a name="Ex3Task4" />
#### Task 4 - Registering the Notifications Client ####

When an application that is capable of receiving push notifications runs, it must first request a notification channel.
After the application has successfully created a channel URI, it sends it to its Web Site, together with any app-specific metadata that should be associated with this URI.

In this task you will call the ChannelController of the WebApi app to request the channel and register your application with the service when it is launched and unregister it when it is suspended. 

1. Open **App.xaml.cs** from the **CustomerManager.StyleUI** project and add the following using directives.

	````C#
	using CustomerManager.StyleUI.DataModel;
	using WebApi.Models;
	using Windows.Networking.PushNotifications;
	using Windows.Storage;
	````

1. Add the following private method at the end of the class to register a channel that will be used to send push notifications.
    
	(Code Snippet - _Building Windows 8.1 Apps - Ex3 - Register and Unregister Channel methods_)

	<!-- mark:5-18 -->
    ````C#
	sealed partial class App : Application
	{
		...

		private async void RegisterChannel()
		{
			var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

			if (ApplicationData.Current.LocalSettings.Values["ChannelId"] == null)
			{
				 var channelDTO = await ChannelWebApiClient.RegisterChannel(new Channel
				 {
					  Uri = channel.Uri
				 });

				 ApplicationData.Current.LocalSettings.Values["ChannelId"] = channelDTO.Id;
			}
		}
	}
    ````

1. Call the **RegisterChannel** method in the **OnLaunched** event.

	(Code Snippet - _Building Windows 8.1 Apps - Ex3 - Call Register Channel method_)

    <!-- mark:5 -->
    ````C#
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        ...

        this.RegisterChannel();
    }
    ````

1. Open **App.xaml** and locate the **ServiceURL** key. Make sure the **ServiceUrl** key value has the URL of the Web API service URL deployed.
	
	````XML
	<x:String x:Key="ServiceUrl">[YOUR-SERVICE-SITE-URL]/api/customers</x:String>
	````

1. Add a new key named **ChannelServiceUrl**. To do this, insert the following line right after the _ServiceUrl_ one, replacing the _[YOUR-SERVICE-SITE-URL]_ with the one of the Web API service deployed as you did in the previous step.

	````XML
	<x:String x:Key="ChannelServiceUrl">[YOUR-SERVICE-SITE-URL]/api/channel</x:String>
	````

1. Run the Windows Store application.

1. Create a new customer and notice the toast notification.

    ![Toast notification](Images/toast-notification.png?raw=true "Toast notification")

    _Toast notification_

---

<a name="NextSteps" />
## Next Steps ##

TBC

---

<a name="Summary" />
## Summary ##

By completing this hands-on lab you have learnt how to use Visual Studio 2013 to:

- Create an ASP.NET MVC 4 Web API service
- Publish the service to Windows Azure Web Sites
- Create a Windows Store application that consumes the Web API service
- Add Push Notifications to the Windows Store application by using WNS Recipe
