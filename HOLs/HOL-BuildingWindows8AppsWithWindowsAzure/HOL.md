<a name="Title" />
# Building Windows 8 Applications using Windows Azure Web Sites #

---
<a name="Overview" />
## Overview ##

Apps are at the center of the Windows 8 experience. They are alive with activity and vibrant content. Users are immersed in full-screen Windows Store apps where they can focus on their content, rather than the operating system.

In this hands-on lab you will learn how to combine the fluency of Windows 8 applications with the power of Windows Azure. From a Windows Store application, you will consume an ASP.NET Web API service published in Windows Azure Web Sites, and store your data in a Windows Azure SQL Database. In addition, you will learn how to configure the Windows Push Notification Services (WNS) in your app using Windows Azure Notification Hubs to send toast notifications from your service to all registered clients.

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

- Create an ASP.NET Web API service
- Publish the service to Windows Azure Web Sites
- Create a Windows Store application that consumes the Web API service
- Add Push Notifications to the Windows Store application by using Notifications Hub

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Visual Studio Express 2013 for Web][1] or greater
- [Visual Studio Express 2013 for Windows][2] or greater
- A Windows Azure subscription
	- Sign up for a [Free Trial](http://aka.ms/watk-freetrial)
	- If you are a Visual Studio Professional, Test Professional, Premium or Ultimate with MSDN or MSDN Platforms subscriber, activate your [MSDN benefit](http://aka.ms/watk-msdn) now to start developing and testing on Windows Azure.
	- [BizSpark](http://aka.ms/watk-bizspark) members automatically receive the Windows Azure benefit through their Visual Studio Ultimate with MSDN subscriptions.
	- Members of the [Microsoft Partner Network](http://aka.ms/watk-mpn) Cloud Essentials program receive monthly Windows Azure credits at no charge.


[1]:http://www.microsoft.com/visualstudio/
[2]:http://www.microsoft.com/visualstudio/

<a name="Setup"/>
### Setup ###

In order to execute the exercises in this hands-on lab, you will need to set up your environment.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.

1. Right-click the **Setup.cmd** file and click **Run as administrator**. This will launch the setup process that will install the Visual Studio code snippets for this lab.

1. If the User Account Control dialog box is shown, confirm the action to proceed.
 
>**Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="CodeSnippets"/>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of this code is provided as Visual Studio Code Snippets, which you can access from within Visual Studio 2013 to avoid having to add it manually.

---
<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Building and Consuming an ASP.NET Web API from a Windows Store App](#Exercise1)
1. [Basic Data Binding and Data Access Using Windows Azure SQL Databases and Entity Framework Code First](#Exercise2)
1. [Adding Push Notification Support with the Windows Azure Notification Hubs](#Exercise3)

> **Note:** Each exercise is accompanied by a starting solution located in the **Begin** folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and may not work until you have completed the exercise.
>
>Inside the source code for an exercise, you will also find an **End** folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.

<a name="Exercise1" />
### Exercise 1: Building and Consuming an ASP.NET Web API from a Windows Store App ###

ASP.NET Web API is a framework that facilitates the process of building and consuming HTTP services for a wide range of clients.

In this exercise you will learn the basics of consuming an ASP.NET Web API REST service - hosted in Windows Azure Web Sites - from a Windows Store application.

You will first create a new Windows Azure Web Site in the portal to host the service. Then, you will create a new ASP.NET Web API project and publish it in Windows Azure from Visual Studio. Once the default service is published, you will create a basic Windows Store client application with a simple list to retrieve the service values.

> **Note:** If you are using Visual Studio 2013 Professional or greater, you are provided with a solution named **End.All.sln** with the Web API and the Windows Store projects together in the **Source/Ex1-BuildingWebAPI/End** folder of this lab.


<a name="Ex1Task1" />
#### Task 1 – Creating a Web API Service ####

In this task you will create a new ASP.NET Web API project and explore its components. 

>**Note:** You can learn more about ASP.NET Web API  [here](http://www.asp.net/web-api).

1. Open **Visual Studio Express 2013 for Web** and select **File | New Project...** to start a new solution.

	![New Project](Images/new-project.png?raw=true "New Project")

	_Creating a New Project_

1. In the **New Project** dialog box, select **ASP.NET Web Application** under the **Visual C# | Web** tab.

	Make sure **.NET Framework 4.5** is selected, name it _WebApi_, choose a Location and click **OK**.

	![New MVC Project](Images/new-mvc-project.png?raw=true "New MVC Project")

	_New MVC Project_

1. In the **New ASP.NET Project - WebApi** dialog box, select **Web API**.

	![New ASP.NET MVC Web API project](Images/new-aspnet-mvc-webapi-project.png?raw=true "New ASP.NET MVC Web API project")

	_New ASP.NET MVC Web API project_

1. You will now explore the structure of an ASP.NET Web API project. Notice that the structure of a Web API project is similar to an MVC project.

	![ASP.NET Web API Project](Images/aspnet-webapi-project.png?raw=true "ASP.NET Web API Project")

	_ASP.NET Web API Project_

	1. **Controllers:** A controller is an object that handles HTTP requests. If you have worked with ASP.NET MVC, you will notice that they work similarly in Web API, but controllers in Web API derive from the ApiController class instead of the Controller Class. The first major difference is that actions on Web API controllers return data instead of views.
		The New Project wizard created two other controllers for you when it created the project: Home and Values. 
	
		-The **Home** controller is responsible for serving HTML pages for the site and is not directly related to Web API. 

		-The **Values** Controller is an example of a Web API controller. 

	1. **Models:** In this folder you will place the classes that represent the data in your application. ASP.NET Web API can automatically serialize your model to JSON, XML, or some other format, and then write the serialized data into the body of the HTTP response message. 

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

1. To run the solution press **F5** or click the **Start** button located on the toolbar. The Web API template home page will open.

	> **Note:** If the web application is not displayed after the deployment, try refreshing the browser until it appears.

1. In the browser, append **api/values** to the URL to retrieve the JSON output of the sample service. You will be prompted to download a file. Click **Open**. If prompted, choose to open the file with a text editor.
	
	![Retrieving the default values](Images/retrieving-the-default-webapi-values.png?raw=true "Retrieving the default values")

	_Retrieving the default values_

1. Press **Shift + F5** to stop running the solution.

<a name="Ex1Task2" />
#### Task 2 – Adding a New Windows Azure Web Site from Server Explorer ####

1. If not already opened, open **Microsoft Visual Studio Express 2013 For Web** and then open **Server Explorer** by selecting **View | Server Explorer**.

1. In **Server Explorer**, right-click the **Windows Azure** node and select **Connect to Windows Azure...**. Sign in using the Microsoft account associated with your Windows Azure account.

	![Connect to Windows Azure](Images/connect-to-windows-azure.png?raw=true)

	_Connect to Windows Azure_

1. After sign in, the **Windows Azure** node is populated with the resources in your Windows Azure subscription.

1. Expand the **Windows Azure** node, right-click the **Web Sites** node and select **Add New Site...**.

	![Add new site](Images/add-new-website.png?raw=true)

	_Add new site_

1. In the **Create site on Windows Azure** dialog box, provide the following information:
	- In the **Site name** box, type an available name for the Web site.
	- In the **Location** drop-down list, select the region for the Web site. This setting specifies which data center your Web site will run in.
	- In the **Database server** drop-down list, select **Create new server**. Alternatively, you can select an existing SQL Server.
	- In the **Database username** and **Database password** boxes, type the administrator username and password for the SQL Server. If you select a SQL Server you have already created, you will be prompted for the password.

1. Click **Create** to create the Web site.

	![Create site on Windows Azure](Images/create-site-on-windows-azure.png?raw=true)

	_Create site on Windows Azure_

1. Wait for the new Web site to be created.

	> **Note:** By default, Windows Azure provides domains at _azurewebsites.net_ but also gives you the possibility to set custom domains using the Windows Azure Management Portal (right-click your Web site from Server Explorer and select **Open in Management Portal**). However, you can only manage custom domains if you are using certain Web site modes.
	
	> Windows Azure offers 3 modes for users to run their Web sites - **Free**, **Shared**, and **Standard**. In **Free** and **Shared** mode, all Web sites run in a multi-tenant environment and have quotas for CPU, Memory, and Network usage. You can mix and match which sites are **Free** (strict quotas) vs. **Shared** (more flexible quotas). The maximum number of free sites may vary with your plan. In **Standard** mode, you choose which sites run on dedicated virtual machines that correspond to the standard Azure compute resources. You can find the Web Sites Mode configuration in the **Scale** menu of your Web site.

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

	_Selecting the new Web site_

1. In the **Connection** page, leave the imported values and click **Validate Connection**. Once the validation is completed, click **Next**.

	> **Note:** Validation is complete once a green checkmark appears to the right of the **Validate Connection** button.

	![Validating connection](Images/validating-connection.png?raw=true)

	_Validating connection_

1. In the **Settings** page, leave the default values and click **Next**.

	![Publish Settings page](Images/publish-settings-page.png?raw=true "Publish Settings page")

	_Publish Settings page_

1. In the **Preview** page, click **Publish**.

	![Publishing a web site](Images/publishing-a-web-site.png?raw=true "Publishing a web site")
	
	_Publishing a Web site_

1. When the process is completed the published Web site will open in your default web browser. Append **api/values** to the URL to retrieve the default values and test that the Web API service is working successfully.

	![Default web service published](Images/default-web-service-published.png?raw=true "Default web service published")

	_Default web service - Published_

<a name="Ex1Task4" />
#### Task 4 – Creating a Windows Store Client Application ####

In this task you will create a blank Windows Store application that will consume the service you already have running.

1. Open **Visual Studio Express 2013 for Windows** and select **File | New Project...** to start a new solution.

1. In the **New Project** dialog box, select **Blank App** under the **Visual C# | Windows Store** application. Name it _Win8Client_ and click **OK**.

	![Add a new Windows Store application basic client project](Images/add-new-style-ui-app-basic-client-project.png?raw=true "Add new Windows Store application basic client project")

	_Add a new Windows Store application basic client project_

	> **Note:** If this is your first time creating a Windows Store application, you will be prompted to get a developer license from the Windows Store to develop this kind of application. In the Developer License window, click **I Agree**.
	
	> ![Getting a Developer License](Images/getting-developer-license.png?raw=true "Getting a Developer License")
	
	> You will be requested to sign in using your Microsoft credentials. Do so and click **Sign in**. You now have a developer license.

	> ![Inserting credentials to obtain a developer license](Images/inserting-credentials-developer-license.png?raw=true "Inserting credentials to obtain a developer license")

	> ![Developer License successfully obtained](Images/developer-license-succesfully-obtained.png?raw=true "Developer License successfully obtained")

1. In the Solution Explorer, right-click the **MainPage.xaml** file and select **View Code**.

	![View code behind MainPage.xaml](Images/main-page-view-code.png?raw=true "View code behind MainPage.xaml")

	_View code behind MainPage.xaml_

1. In **MainPage.xaml.cs**, add a reference to the **Windows.Data.Json** assembly.

	<!-- mark:1 -->
	````C#
	using Windows.Data.Json;
	````

1. Add the following method to the **MainPage.xaml.cs** class to perform an asynchronous call to the Web API service.

	The method **GetItems()** instantiates an HttpClient object, which  sends a GET message to the service URL and retrieves a response asynchronously. Then, the response is deserialized and read by the JsonArray object before generating the value list.  

	(Code Snippet - _BuildingWindows8Apps - Ex1 - GetItems_)
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

	> **Note:** If you want to read more about async methods, you can refer to [this article](http://msdn.microsoft.com/en-US/vstudio/async).

1. Replace the value of the placeholder [YOUR-WINDOWS-AZURE-SERVICE-URL] with your Windows Azure published Web site URL, found in the Web site's dashboard.

	>**Note:** If you want to test the service locally, start the service project, check its URL and its port (e.g. http://localhost:3565/) and use that value.

1. Then, override the **OnNavigatedTo** method adding a call to the GetItems method and press **CTRL+S** to save.

	(Code Snippet - _BuildingWindows8Apps - Ex1 - OnNavigatedTo_)
	<!-- mark:1-4 -->
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

1. Make sure the Windows Store application is the startup project and press **F5** to run the solution. 

	You will see that the values retrieved from the service are listed in the List Box.

	![Basic service output](Images/basic-service-output.png?raw=true "basic service output")

	_Service output_

<a name="Exercise2" />
### Exercise 2: Basic Data Binding and Data Access Using Windows Azure SQL Databases and Entity Framework Code First ###

In this exercise, you will learn how to bind your Windows Store application to an ASP.NET Web API service which uses Code First to generate the database from the model in SQL Database.

You will create a new ASP.NET Web API service and use Entity Framework Scaffolding with Code First to generate the service methods and a database in SQL Database.
Finally, you will explore and customize your Windows Store application to consume the service and show a customer list.

> **Note:** If you are using Visual Studio 2013 Professional or greater, you are provided with two solutions named **Begin.All.sln** and **End.All.sln** with the ASP.NET Web API and Windows Store projects together in the respective **Source/Ex2-DataAccess/Begin** and **Source/Ex2-DataAccess/End** folders of this lab.

**About Entity Framework Code First**

Entity Framework (EF) is an object-relational mapper (ORM) that enables you to create data access applications by programming with a conceptual application model instead of programming directly using a relational storage schema.

The Entity Framework Code First modeling workflow allows you to use your own domain classes to represent the model that EF relies on when performing querying, change tracking and when updating functions. Using the Code First development workflow, you do not need to begin your application by creating a database or specifying schema. Instead, you can begin by writing standard .NET classes that define the most appropriate domain model objects for your application, and Entity Framework will create the database for you.

>**Note:** You can learn more about Entity Framework [here](http://www.asp.net/entity-framework).

<a name="Ex2Task1" />
#### Task 1 - Creating an ASP.NET Web API Service with Entity Framework Code First and Scaffolding ####

In this task you will add Entity Framework Scaffolding and Code First to an ASP.NET Web API service. At the end of this task, you will have a basic API service that performs CRUD operations (Create, Read, Update and Delete) implemented and published in Windows Azure Web Sites.

1. Open **Visual Studio Express 2013 for Web** and open the ***WebApi.sln*** solution located in the ***Source/Ex2-DataAccess/Begin*** folder. Alternatively, you can continue with the solution that you obtained in the previous exercise.

1. Right-click the **Models** folder in the **Solution Explorer** and select **Add | Class...**. Name it **Customer.cs**.

	![Adding a model class](Images/add-model-class.png?raw=true "Add model class")

	_Adding a model class_

	> **Note:** You will start by creating a Customer model class, and your CRUD operations in the service will be automatically created using scaffolding features.

1. Replace the Customer.cs class content with the following code. Press **CTRL+S** to save the changes.

	(Code Snippet - _BuildingWindows8Apps - Ex2 - CustomerClass_)
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

1. In Solution Explorer, right-click the **WebApi** project and select **Build**.

	> **Note:** If you build the entire solution, including both projects, you will get errors because the Windows Store app client is still incomplete.

1. In Solution Explorer, right-click the **Controllers** folder of the Web API project and select **Add | New Scaffolded Item...** to open the **Add Scaffold** dialog box.

	![Adding new scaffolded item](Images/adding-new-scaffolded-item.png?raw=true "Adding new scaffolded item")

	_Adding new scaffolded item_

1. In the **Add Scaffold** dialog box, select **Web API 2 Controller with actions, using Entity Framework** and then click **Add.**

	![Selecting the Web API 2 controllers with actions](Images/selecting-the-web-api-2-controllers-with-acti.png?raw=true "Selecting the Web API 2 controllers with actions")

	_Selecting the Web API 2 controllers with actions_

1. Set **CustomersController** as the controller name, check the **Use async controller actions** option and select **Customer (WebApi.Models)** as the Model class.

	![Adding a controller with Scaffolding](Images/adding-a-controller-with-scaffolding.png?raw=true "Adding a controller with Scaffolding")

	_Adding a controller with Scaffolding_

	> **Note:** If there are no model classes to select, you need to build the project.

1. In the Data context class, select **New data context...**. 

	![Creating a new data context](Images/creating-a-new-data-context.png?raw=true "Creating a new data context")

	_Creating a new data context_

1. In the **New Data Context** dialog box, name the new data context _CustomerContext_ and click **Add**.

	![New customers context](Images/new-customers-context.png?raw=true "New customers context")

	_New customers context_

1. Click **Add** to add the controller. By using the 'API controller with read/write actions and Entity Framework' template, the CRUD operations for customers will be automatically generated in the Web API service.

1. Once the controller with scaffolding is created, open **CustomersController.cs**. Notice that the following CRUD actions were added:
	- DeleteCustomer(int id)
	- GetCustomer(int id)
	- GetCustomers()
	- PostCustomer(Customer customer)	
	- PutCustomer(int id, Customer customer)

	Notice that each operation includes the HTTP verb as a prefix in the name (Delete, Get, Post, etc.).

1. Now you will add a database initializer method in your database context to populate the database with initial data. Add the following **CustomerContextInitializer** class in the **CustomerContext.cs** file (in the **Models** folder) after the **CustomerContext** class, and save the changes.

	(Code Snippet - _BuildingWindows8Apps - Ex2 - ContextInitializer_)
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
	> Alternatively, you can use **CreateDatabaseIfNotExists\<TContext>**, which recreates and optionally re-seeds the database with data only if the database does not exist, or **DropCreateDatabaseAlways\<TContext>**, which always recreates and optionally re-seeds the database with data the first time a context is used in the application domain.
	

1. In the CustomerContextInitializer **Seed** method, add the following customers to the context to populate the database.

	(Code Snippet - _BuildingWindows8Apps - Ex2 - ContextInitializerSeed_)
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

1. In **Solution Explorer**, right-click the **WebAPI** project and select **Publish...**.

	![Publishing the service](Images/publishing-the-service.png?raw=true "Publishing the service")

	_Publishing the service_

1. In the **Preview** page, click the **Prev** button to navigate to the **Settings** page in order to configure the SQL database.

	![Navigating to the Settings Page](Images/navigating-to-the-settings-page.png?raw=true "Navigating to the Settings Page")

	_Navigating to the Settings Page_

	> **Note:** If you started from Exercise 2, you may want to refer to [Exercise 1 - Task 3](#Ex1Task3) to configure the Web site profile and connection settings.

1. In the **Settings** page, browse to the **Databases** section. If the **CustomerContext** section is not expanded, click the down arrow in the **CustomerContext** text box and select the SQL database shown in the drop-down list.

	> **Note:** The SQL Database you just selected was automatically generated when you created the Windows Azure Web Site in [Exercise 1](#Exercise1).

	![Selecting the SQL Database](Images/selecting-the-sql-database.png?raw=true)

	_Selecting the SQL Database_

1. Click **Next** and then in the **Preview** page, click **Publish**.

	![Publishing a Web site](Images/publishing-a-web-site.png?raw=true "Publishing a Web site")
	
	_Publishing a Web site_

1. When the process is completed, the published Web site will open in your default web browser. Go to **/api/customers** to retrieve the full list of customers.

	![Testing the Customers Web API](Images/testing-the-customers-web-api.png?raw=true "Testing the Customers Web API")

	_Testing the Customers Web API_

	>**Note:** Entity Framework will create the database schema the first time you run the application. You can also access the database tables in the Windows Azure portal to check that the data was added.

<a name="Ex2Task3" />
#### Task 3 - Exploring the Windows Store Application ####

In this task you will explore the Customer client application, built using a Windows Store application Grid Template. You will perform a brief lap around and learn about the main components of a Windows Store Grid application.

1. Open **Visual Studio Express 2013 for Windows** and open the **CustomerManager.sln** solution located in the **Source/Ex2-DataAccess/Begin** folder.
This is a client Windows Store application that displays customers and is based on the Visual Studio Grid template.

	>**Note:** The Grid application is one of the Visual Studio 2013 templates available for Windows Store applications and contains three pages. The first page displays a group of items in a grid layout. When a group is clicked, the second page shows the details of the selected group. Finally, when an item is selected, the third page shows the item details.

	In this solution you will find a simplified Grid template which only contains group and detail pages with a custom data model for customers.

	![CustomerManager Windows Store Application](Images/customermanager-app.png?raw=true "CustomerManager Windows Store Application")

	_CustomerManager Windows Store Application_

	The main application pages include:
	- _GroupedCustomersPage:_ shows customers in a grid layout
	- _CustomerDetailPage:_ shows customer details
	- _NewCustomerPage:_ adds a new customer

1. Expand the **ViewModel** folder and open **GroupedCustomersViewModel.cs**. 

	The XAML pages of this solution are bound against ViewModels classes that retrieve and prepare the necessary data that will be displayed.
	
	GroupedCustomersViewModel contains an ObservableCollection of CustomerViewModel and a method to retrieve the customers asynchronously (in the next exercise you will complete the CustomersWebApiClient call).


	````C#
    public class GroupedCustomersViewModel : BindableBase
    {
        public GroupedCustomersViewModel()
        {
            this.CustomersList = new ObservableCollection<CustomerViewModel>();

            this.GetCustomers();
        }

        public ObservableCollection<CustomerViewModel> CustomersList { get; set; }

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

	> **Note:** Windows Runtime now supports using ObservableCollection to set up dynamic bindings so that insertions or deletions in the collection update the UI automatically.

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

	In the XAML code of this page, each collection is bound to the ViewModel through a CollectionViewSource which points to the customers list from the ViewModel.

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

1. In **Solution Explorer**, right-click the **DataModel** project folder and select **Add | Existing Item...**. 

	![Adding an existing item](Images/adding-an-existing-item.png?raw=true "Adding an existing item")
	
	_Adding an existing item_

1. Browse to the **WebApi** project, open the **Models** folder and select **Customer.cs**. Click the arrow next to the **Add** button and click **Add as Link**.

	![Adding Customer.cs model class as a link](Images/add-customercs-as-an-existing-item.png?raw=true "Adding Customer.cs model class as a link")

	_Adding Customer.cs model class as a link_

	By adding the Customer class as a link, your application will be using the same model class from the Web API service. This class will serve as a data contract between the Web API service and the Windows Store app.

1. In **Solution Explorer**, right-click the **DataModel** project folder and select **Add | Class...**. Name it _CustomersWebApiClient.cs_. 

	This class will contain the methods that retrieve the customers from the Web API service you have already published on Windows Azure.

	![Adding a new class](Images/adding-a-new-class.png?raw=true "Adding a new class")

	_Adding a new class_

1. 	In the **CustomersWebApiClient** class, add the following using directives. 

	(Code Snippet - _BuildingWindows8Apps - Ex2 - CustomersWebApiClientNamespace_)

	<!-- mark:1-4 -->
	````C#
	using System.IO;
	using System.Net.Http;
	using System.Runtime.Serialization.Json;
	using WebApi.Models;
	````

	>**Note:** WebApi.Models references your service models, and it is necessary to use the customer class you have added as a link.
	
1. In the **CustomersWebApiClient** class, add the following **GetCustomers()** method.

	(Code Snippet - _BuildingWindows8Apps - Ex2 - CustomersWebApiClientGetCustomersMethod_)

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

1. In the **CustomersWebApiClient** class, add the following **CreateCustomer** method to post new customers to the service.

	(Code Snippet - _BuildingWindows8Apps - Ex2 - CustomersWebApiClientCreateCustomerMethod_)

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

	This method executes an asynchronous post to the Web API service, sending the new customer data serialized as JSON. 

1. Finally, you will configure the ServiceUrl value using your Web API service URL. Open **App.xaml** and locate the **ServiceURL** key. Change the key value using your service site URL.

	````XML
	<x:String x:Key="ServiceUrl">[YOUR-SERVICE-SITE-URL]/api/customers</x:String>
	````

	>**Note:** You can find your service URL in the  dashboard of your Windows Azure Web Sites.

<a name="Ex2Task5" />
#### Task 5 - A Lap around the Customer Manager Application ####

1. Press **F5** to run the solution.

	![Customer Manager Grid](Images/customer-manager-grid.png?raw=true "Customer Manager Grid")

	_Customer Manager - Grid_

1. Click on a customer to open the **Customer Details** page. Notice that you can use the arrows on the right and left sides of the screen to browse through the customers.

1. Click the upper-left arrow to go back.

	![Customer Details](Images/customer-details.png?raw=true "Customer Details")

	_Customer Details_

1. Back in the Home page, right-click to bring the application bar up and select **Add** to go to the new customer page.

	![Add button](Images/add-button.png?raw=true "Add button")

	_Add button_

1. Complete the new customer's data and click **Create**.

	![New Customer](Images/new-customer.png?raw=true "New Customer")

	_New Customer_

1. Back in the home page, you will see the new customer added.

	>**Note:** If you cannot see the new customer added, go to the details page and come back to the home page. As the CreateCustomer method is asynchronous, to avoid blocking the UI the GetCustomers method might execute before the new customer is posted to the service.

<a name="Exercise3" />
### Exercise 3: Adding Push Notification Support with the Windows Azure Notification Hubs ###

The Windows Push Notification Services (WNS) enables third-party developers to send toast, tile, and badge updates from their own Web site. This provides a mechanism to deliver new updates to your users in a power-efficient and dependable way.

The process of sending a notification involves the following steps:

1. **Request a channel.** Use the WinRT API to request a Channel URI from WNS. The Channel URI will be the unique identifier you use to send notifications to an application instance.

1. **Register the channel with your Windows Azure Web Site.** Once you have your channel you can store and associate it with any application-specific data (e.g. user profiles and such) until your services decide that it is time to send a notification to the given channel.

1. **Authenticate against WNS.** To send notifications to your channel URI you are first required to authenticate against WNS using OAuth2 to retrieve a token that will be used for each subsequent notification that you push to WNS.

1. **Push notification to channel recipient.** Once you have your channel, notification payload and WNS access token, you can perform an HttpWebRequest to post your notification to WNS for delivery to your client.

	![WNS Flow Diagram](Images/wns-flow-diagram.png?raw=true "WNS Flow Diagram")

	_WNS Flow Diagram_

In this exercise you will learn how to send a toast notification from the Web API service (Web site) using Windows Azure Notification Hubs to the registered clients (Windows Store applications) whenever a new customer is added.

Windows Azure Notification Hubs provide all the functionality of a push infrastructure that enables you to send push notifications from any backend (in the cloud or on-premises) to any mobile platform. By using Windows Azure Notification Hubs you remove the responsibility of managing channel URIs and device registration from the backend since it is handled by the service, allowing you to focus on sending the platform-independent notifications to clients.

A toast notification is a transient message to the user that contains relevant, time-sensitive information and provides quick access to related content in an app. It can appear whether you are in another app, the Start screen, the lock screen, or on the desktop. Toasts should be viewed as an invitation to return to your app to follow up on something of interest.

> **Note:** If you are using Visual Studio 2013 Professional or greater, you are provided with two solutions named **Begin.All.sln** and **End.All.sln** with the Web API and Windows Store projects together in the respective **Source/Ex3-Notifications/Begin** and **Source/Ex3-Notifications/End** folders of this lab.

<a name="Ex3Task1" />
#### Task 1 - Registering the Customer Manager Application for Push Notifications####

Before you can send notifications through WNS, you must register your application with the Windows developer center that supports the end-to-end process for submitting, certifying, and managing applications for sale in the Windows Store. When you register your application with the Dashboard, you are given credentials — a Package security identifier (SID) and a secret key — which your Web site will use to authenticate itself with WNS.

In this task you will obtain the information necessary to enable your application to communicate with WNS and Live Connect.

1. In Visual Studio, continue working with the solutions obtained from the previous exercise. If you did not execute the previous exercise, you can open **WebApi.sln** with **Visual Studio Express 2013 for Web** and **CustomerManager.sln** with **Visual Studio Express 2013 for Windows**, both located in the **Source/Ex3-Notifications/Begin** folder of this lab.

1. If you opened the **WebApi** begin solution, deploy it configuring the **CustomerContext** connection string to point to a Windows Azure SQL Database. [Exercise 2 - Task 2](#Ex2Task2) explains how to do this.

1. In the **CustomerManager** begin solution, open **Package.appxmanifest** and take note of the **Package Display Name** located in the **Packaging** tab.

	![Taking note of the Package display name](Images/taking-note-of-the-package-display-name.png?raw=true "Taking note of the Package display name")

	_Taking note of the Package display name_

    > **Note:** The package manifest is an XML document that contains the information the system needs to deploy, display, or update a Windows Store app. This information includes package identity, package dependencies, required capabilities, visual elements, and extensibility points. Every application package must include one package manifest.

1.	Click **Store** in the Visual Studio menu and select **Reserve App Name...**.

	![Reserving App Name](./Images/reserving-app-name.png?raw=true)

	_Reserving App Name in Windows Store_

1.	The browser will display the Windows Store page that you will use to obtain your WNS credentials. In the Submit an app section, click **App Name**.

	> **Note:** You will have to sign in using your Microsoft Account to access the Windows Store.

	![Giving your app a unique name](./Images/giving-app-name-windows-store.png?raw=true)

	_Giving your app a unique name_

1.	In the App name field, insert the **Package Display Name** that is inside the **Package.appxmanifest** file of your solution and click **Reserve app name**. Then click **Save** to confirm the reservation.

	![Reserving an app name](./Images/app-name-windows-store.png?raw=true)

	_Reserving an app name_

	![Confirming the app name reservation](./Images/name-reservation-successful-win-store.png?raw=true)

	_Confirming the app name reservation_

1. Now you will have to identify your application to get a name and a publisher to insert in the **Package.appxmanifest** file. In the **Submit an app** page, click **Services**.

	![Configuring push notifications for the Notifications.Client app](./Images/app-name-reverved-completely-windows-store.png?raw=true)

	_Configuring push notifications for the Notifications.Client app_

1. In the Services page, click **Live Services site**.

	![Advanced features page](./Images/push-notif-live-connect-service-info.png?raw=true)

	_Advanced features page_

1. Once in the Push notifications and Live Connect services info section, click **Identifying your app**.

	![Push notifications Overview page](./Images/identifying-your-app.png?raw=true)

	_Push notifications Overview page_

1. In the Identifying your app section, click **Authenticating your service**.

	![Setting Identity Name and Publisher](./Images/app-identification.png?raw=true)

	_Setting Identity Name and Publisher_

1. Take note of the **Package Security Identifier (SID)** and the **Client secret**, which are the WNS Credentials that are required to configure the Notification Hub.

	![Package Security Identifier (SID) and Client secret](./Images/sid-client-secret.png?raw=true)

	_Package Security Identifier (SID) and Client secret_

	> **Note:** To send notifications to this application, your application must use these exact credentials. You cannot use another application's credentials to send notifications to this application, and you cannot use these credentials to send notifications to another app.

	> **Note:** The client secret and package SID are important security credentials. Do not share these values with anyone or distribute them with your app.

<a name="Ex3Task2" />
#### Task 2 - Creating a Notification Hub ####

In this task you will create a **Windows Azure Notification Hub** using the **Windows Azure Management Portal**.

1. Log on to the **Windows Azure Management Portal**, and click **NEW** at the bottom of the screen.

1. Click on **App Services**, then **Service Bus**, then **Notification Hub**, then **Quick Create**.

	![Creating a new Notification Hub](Images/creating-a-new-notification-hub.png?raw=true "Creating a new Notification Hub")

	_Creating a new Notification Hub_

1. Type a name for your notification hub, select your desired Region, and then click **Create a new Notification Hub**.

	![Specifying your Notification Hub name](Images/specifying-your-notification-hub-name.png?raw=true "Specifying your Notification Hub name")

	_Specifying your Notification Hub name_

1. Wait until the new namespace appears as **Active** and click on it.

	![Selecting the new namespace](Images/selecting-the-new-namespace.png?raw=true "Selecting the new namespace")
	
	_Selecting the new namespace_

1. Select the **Notification Hubs** tab at the top, and then click the notification hub you just created.

	![Selecting the Notification Hub](Images/selecting-the-notification-hub.png?raw=true "Selecting the Notification Hub")

	_Selecting the Notification Hub_

1. Select the **Configure** tab at the top, enter the **Client secret** and **Package SID** values you obtained from WNS in the previous section, and then click **Save**.

	![Configuring the WNS credentials](Images/configuring-the-wns-credentials.png?raw=true "Configuring the WNS credentials")

	_Configuring the WNS credentials_

1. Select the **Dashboard** tab at the top, and then click **View Connection String** in the **quick glance** section. 

	![Viewing the connection strings](Images/viewing-the-connection-strings.png?raw=true "Viewing the connection strings")

	_Viewing the connection strings_

1. In the **Access connection information** dialog box, take note of the two connection strings.

	![Taking note of the connection strings](Images/taking-note-of-the-connection-strings.png?raw=true "Taking note of the connection strings")

	_Taking note of the connection strings_

<a name="Ex3Task3" />
#### Task 3 - Enabling Push Notifications####

In this task you will configure your application to raise toast notifications.

1. Go back to Visual Studio, open the application manifest and select the **Application** tab.

1. Find the **Notifications** section and select **Yes** in the **Toast capable** drop-down menu.

    ![Enabling toast notifications](Images/enabling-toast-notifications.png?raw=true "Enabling toast notifications")

    _Enabling toast notifications_

1. Switch to the **Capabilities** tab and select the following capabilities:
    - Internet (Client)
    - Internet (Client & Server)
    - Private Networks (Client & Server)

    ![Enabling network capabilities](Images/enabling-network-capabilities.png?raw=true "Enabling network capabilities")

    _Enabling network capabilities_

1.	Click **Store** in the Visual Studio menu and select **Associate App with the Store...**.

	![Associating App with Store](./Images/associating-app-with-store.png?raw=true)

	_Associating App with Store_

1. In the **Associate Your App with the Windows Store** wizard, click **Next**.

	![Associating App with Store Wizard](./Images/associate-app-with-store.png?raw=true)

	_Associating App with Store Wizard_

1. Type your credentials and click **Sign In**.

	![Typing your credentials to associate your app in Windows Store](./Images/sign-in-for-association.png?raw=true)

	_Typing your credentials to associate your app in Windows Store_

1. In the Select an app name step, select **CustomerManager** and click **Next**.

	![Selecting your app name](./Images/selecting-app-name.png?raw=true)

	_Selecting your app name_

1. Take a look at the summary of the values that will be added in the manifest file. Click **Associate**. 

	![Associating your app with the Windows Store Summary](./Images/association-summary.png?raw=true)

	_Associating your app with the Windows Store Summary_


<a name="Ex3Task4" />
#### Task 4 - Sending Push Notifications ####

To send a notification, the sender must be authenticated through WNS. The first step in this process occurs when you register your application with the Windows Store Dashboard. During the registration process, your application is given a Package security identifier (SID) and a secret key. This information is used by your application to authenticate with WNS.

In this task you will use the Windows Azure Notification Hub previously created and configured with your credentials (Package SID and secret key) to send push notifications to the Windows Store application.

1. Go to the **WebApi** solution and open the **Package Manager Console** from the **Tools | Library Package Manager** menu.

1. Make sure that **WebApi** is selected as **Default project**.

	![Making sure that WebApi is selected as Default project](Images/making-sure-that-webapi-is-selected-as-defaul.png?raw=true "Making sure that WebApi is selected as Default project")

1. Add a reference to the Windows Azure Service Bus SDK with the **WindowsAzure.ServiceBus** NuGet package.

	````PowerShell
	Install-Package WindowsAzure.ServiceBus
	````

1. Open the **CustomersController.cs** file from the WebApi project and add the following using directives.
    
	````C#
	using Microsoft.ServiceBus.Notifications;
	using System.Configuration;
	````

1. Add the following private method to send a toast notification about the new customers.

	(Code Snippet - _BuildingWindows8Apps - Ex3 - SendNotification_)
	<!--mark:1-17 -->
	````C#
	private async Task SendNotificationAsync(Customer customer)
	{
		 var connectionString = ConfigurationManager.AppSettings["HubConnectionString"];
		 var notificationHub = ConfigurationManager.AppSettings["HubName"];
		 NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, notificationHub);

		 var toast = "<toast>" +
							  "<visual>" +
									"<binding template=\"ToastText02\">" +
										 "<text id=\"1\">New customer added!</text>" +
										 "<text id=\"2\">" + customer.Name + "</text>" +
									"</binding>" +
							  "</visual>" +
						 "</toast>";

		 await hub.SendWindowsNativeNotificationAsync(toast);
	}
	````

	> **Note:** A channel is a unique address that represents a single user on a single device for a single application or secondary tile. Using the channel URI, an application can send a notification whenever it has an update for the user. With the **NotificationHubClient** you can send a notification to the full list of the client endpoints registered in the Notification Hub.

1. Find the **PostCustomer** function and add a call to **SendNotification** method.

    <!-- mark:13 -->
    ````C#
	// POST api/Customers
	[ResponseType(typeof(Customer))]
	public async Task<IHttpActionResult> PostCustomer(Customer customer)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		db.Customers.Add(customer);
		await db.SaveChangesAsync();

		await this.SendNotificationAsync(customer);

		return CreatedAtRoute("DefaultApi", new { id = customer.CustomerId }, customer);
	}
   ````
 
1. Open the **Web.config** file and add the following settings in the `appSettings` section. Replace the placeholders using the values that you obtained in Task 3.

    <!-- mark:2-3 -->
    ````XML
      ...
      <add key="HubConnectionString" value="[connection string with full access]"/>
      <add key="HubName" value="[hub name]"/>
    </appSettings>
    ````

1. Publish the Customers Web API service in Windows Azure. To do this, follow the steps in [Exercise 2 - Task 2](#Ex2Task2).

<a name="Ex3Task5" />
#### Task 5 - Registering the Notifications Client ####

When an application that is capable of receiving push notifications runs, it must first request a notification channel.
After the application has successfully created a channel URI, it sends it to its Notification Hub.

In this task you will register your application with the service and then register that channel in the Notification Hub.

1. Add a reference to the Windows Azure Messaging library for Windows Store using the **WindowsAzure.Messaging.Managed** NuGet package. In the Visual Studio Main Menu, click **Tools**, followed by **Library Package Manager**, followed by **Package Manager Console**. Then, in the console window type:

    ````PowerShell
	Install-Package WindowsAzure.Messaging.Managed
    ````

1. Open **App.xaml.cs** from the **CustomerManager** project and add the following using directives.

	````C#
	using Windows.Networking.PushNotifications;
	using Microsoft.WindowsAzure.Messaging;
	````

1. Add the following private method at the end of the class to retrieve the ChannelURI for the app from WNS, and then register that ChannelURI with your notification hub. Make sure to replace the **hub name** and **connection string with listen access** placeholders with the name of the notification hub (e.g. _customer-manager-hub_) and its corresponding **DefaultListenSharedAccessSignature** obtained in the previous task.

	(Code Snippet - _BuildingWindows8Apps - Ex3 - RegisterChannel_)
	<!-- mark:1-7 -->
	````C#
	private async void RegisterChannel()
	{
		 var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

		 var hub = new NotificationHub("<hub name>", "<connection string with listen access>");
		 var result = await hub.RegisterNativeAsync(channel.Uri);
	}
	````

1. Call the **RegisterChannel** method in the **OnLaunched** event.

	(Code Snippet - _BuildingWindows8Apps - Ex3 - CallRegisterChannelMethod_)

    <!-- mark:5 -->
    ````C#
    protected override async void OnLaunched(LaunchActivatedEventArgs e)
    {
        ...

        this.RegisterChannel();
    }
    ````

1. Run the Windows Store application.

1. Create a new customer ([Exercise 2 - Task 5](#Ex2Task5) explains how to do this), and notice the toast notification.

    ![Toast notification](Images/toast-notification.png?raw=true "Toast notification")

    _Toast notification_

---

<a name="NextSteps" />
## Next Steps ##

To learn more about Windows Store applications and Windows Azure Web Sites, please refer to the following articles:

**Technical Reference**

This is a list of articles that expand on the technologies explained in this lab:

- [Windows Azure Web Sites Documentation](http://aka.ms/Alwcgu): provides reference information for developing your site with .NET, PHP, Node.js or Python and hosting in Windows Azure Web Sites
- [Windows Azure Web Sites, Cloud Services, and VMs: When to use which?](http://aka.ms/Nocpe8): provides guidance on how to make an informed decision when choosing among Windows Azure Web Sites, Cloud Services, and virtual machines to host a web application
- [Create a Global Web Presence on Windows Azure Web Sites](http://aka.ms/Rvc3yr): provides a technical overview of how to host your organization's (.COM) site on Windows Azure
- [ASP.Net Web API (The official Microsoft ASP.NET Site)](http://aka.ms/Bxoypx): is a framework that makes it easy to build HTTP services that reach a broad range of clients, including browsers and mobile devices. ASP.NET Web API is an ideal platform for building RESTful applications on the .NET Framework
- [Notification Hubs Documentation](http://aka.ms/Dmg3nf): provides tutorials and guides for using Notification Hubs to send mobile push notifications from any backend (in the cloud or on-premises) to any mobile platform
- [Tiles, badges, and notifications (Windows Store apps)](http://aka.ms/Fasloi): discusses the concepts and terminologies required to design tiles (including secondary tiles and lock screen apps), badges, and toast notifications

**Development**

This is a list of developer-oriented articles related to Windows Store applications and Windows Azure Web Sites:

- [Deploy a Secure ASP.NET MVC 5 app with Membership, OAuth, and SQL Database to a Windows Azure Web Site](http://aka.ms/Gv0w5g): shows you how to build a secure ASP.NET MVC 5 web app that enables users to log in with credentials from Facebook or Google
- [Developing Windows Store apps](http://aka.ms/Ryj9xy): provides reference information for designing and developing great Windows Store apps
- [Securing a Windows Store Application and REST Web Service Using Windows Azure AD](http://aka.ms/T5ejfa): shows you how to create a simple web API resource and a Windows Store client application using the Windows Azure Authentication Library and Windows Azure AD
- [Connecting to Windows Azure Mobile Services](http://aka.ms/Uvxhgy): explores the different scenarios to enable Mobile Services in your Windows Store apps

---

<a name="Summary" />
## Summary ##

By completing this hands-on lab you have learned how to use Visual Studio 2013 to:

- Create an ASP.NET Web API service
- Publish the service to Windows Azure Web Sites
- Create a Windows Store application that consumes the Web API service
- Add Push Notifications to the Windows Store application by using Windows Azure Notification Hubs
