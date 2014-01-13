<a name="Title"></a>
# Building and Publishing ASP.NET Applications with Windows Azure Web Sites and Visual Studio 2012 #

---
<a name="Overview"></a>
## Overview ##

Web site publication and deployment has never been easier in Windows Azure. Using familiar tools such as Web Deploy or Git, and virtually no changes to the development workflow, Windows Azure Web Sites is the next step in the Microsoft Azure platform for web developers. 

In this hands-on lab, you will explore the basic elements of the **Windows Azure Web Sites** service by creating a simple [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4) application, which uses scaffolding to automatically generate the baseline of your application's CRUD (Create, Read, Update and Delete). Then, you will deploy it using Web Deploy from Microsoft Visual Studio 2012 and Git commit.

Starting from a simple model class and without writing a single line of code, you will create a controller that will contain all the CRUD operations, as well as all the necessary views. After publishing and running the solution, you will have the application database generated in your SQL Database server, together with the MVC logic and views for data manipulation.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a Web Site from the Windows Azure Management Portal
- Use Microsoft Visual Studio 2012 to build a new ASP.NET MVC 4 application
- Deploy the application using Web Deploy from Visual Studio
- Create a new Web Site with Git Repository enabled to publish the ASP.NET MVC 4 application using Git

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Microsoft Visual Studio 2012](http://msdn.microsoft.com/vstudio/products/)
- [GIT Version Control System](http://git-scm.com/download)
- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

<a name="Setup"></a>
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab's **Source** folder. 

1. Right-click the **Setup.cmd** file and click **Run as administrator**. This will launch the setup process that will configure your environment and check the dependencies.

>**Note:** Make sure you have checked all the dependencies for this lab before running the setup. 

---
<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

- [Getting Started: Creating an MVC 4 Application using Entity Framework Code First](#GettingStarted)
- [Exercise 1: Publishing an MVC 4 Application using Web Deploy](#Exercise1)
- [Exercise 2: Publishing an MVC 4 Application using Git](#Exercise2)

<a name="GettingStarted"></a>
### Getting Started: Creating an MVC 4 Application using Entity Framework Code First ###

In this section, you will create a simple ASP.NET MVC 4 web application, using MVC 4 scaffolding with Entity Framework code first to create the CRUD methods.

<a name="GettingStartedTask1"></a>
#### Task 1 – Creating an ASP.NET MVC 4 Application in Visual Studio ####

1. Open **Microsoft Visual Studio 2012** and click the **New Project** link in the start page. Otherwise use  **File** | **New** | **Project**.

	![Creating a new project](Images/new-website-vs2012.png?raw=true "Crating a new project")

	_Creating a new project_

1. Create a new **ASP.NET MVC 4 Web Application** using **.NET Framework 4.5**, **C#** and name it **MVC4Sample.Web**.

	![Creating a new ASP.NET MVC 4 Web Application](Images/mvc4-sample.png?raw=true "Creating a new ASP.NET MVC 4 Web Application")

	_Creating a new ASP.NET MVC 4 Web Application_

1. Select **Internet Application** and click **OK**.

	![Choosing Internet Application](Images/internet-application.png?raw=true "Choosing Internet Application")

	_Choosing Internet Application_

1. In the Solution Explorer, right-click **Models** and select **Add | Class** to create a customer class (POCO). Name it _Customer.cs_ and click **Add**.

1. Open the **Customer** class and insert the following properties.

	<!-- mark:10-25 -->
	````C#
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	namespace MVC4Sample.Web.Models
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

1. **Build MVC4Sample.Web** by using the **Ctrl + Shift + B** keyboard shortcut which will save the changes and build the project.

1. In the Solution Explorer, right-click the **Controllers** folder and select **Add | Controller**. 

1. Name the controller _CustomerController_ and complete the **Scaffolding options** with the following values.	
	- In the **Template** drop-down list, select the **MVC Controller with read/write actions and views, using Entity Framework** option.
	- In the **Model class** drop-down list, select the **Customer** class.
	- In the **Data Context class** list, select **\<New data context...\>**. In the dialog box displayed, replace the data context class type with **MVC4Sample.Web.Models.CustomerContext** and click **OK**.
	- In the **Views** drop-down list, make sure that **Razor (CSHTML)** is selected.

	![Adding the Customer controller with scaffolding](Images/add-customer-controller.png?raw=true "Adding the Customer controller with scaffolding")

	_Adding the Customer controller with scaffolding_
	
1. Click **Add** to create the new controller for **Customer** with scaffolding. You have generated the controller actions as well as the views. 
		
	![After creating the Customer controller with scaffolding ](Images/customer-scaffolding.png?raw=true "After creating the Customer controller with scaffolding")

	_After creating the Customer controller with scaffolding_

1. Open the **CustomerController.cs** file in the **Controllers** folder. Notice that the CRUD action methods have been generated automatically. 

	````C#
	//
	// POST: /Customer/Create

	[HttpPost]
	[ValidateAntiForgeryToken]
	public ActionResult Create(Customer customer)
	{
		if (ModelState.IsValid)
		{
			 db.Customers.Add(customer);
			 db.SaveChanges();
			 return RedirectToAction("Index");
		}

		return View(customer);
	}

	//
	// GET: /Customer/Edit/5

	public ActionResult Edit(int id = 0)
	{
		Customer customer = db.Customers.Find(id);
		if (customer == null)
		{
			 return HttpNotFound();
		}
		return View(customer);
	}
	````

	_Inside the Customer controller_

1. Do not close Visual Studio.

---

<a name="Exercise1"></a>
### Exercise 1: Publishing an MVC 4 Application using Web Deploy ###

In this exercise, you will create a new web site in the Windows Azure Management Portal and publish the application you obtained in the Getting Started section, taking advantage of the Web Deploy publishing feature provided by Windows Azure.

<a name="Ex1Task1"></a>
#### Task 1 – Creating a New Web Site from the Windows Azure Portal ####

1. Go to the [Windows Azure Management Portal](https://manage.windowsazure.com/) and sign in using the Microsoft credentials associated with your subscription.

	![Log on to Windows Azure portal](Images/login.png?raw=true "Log on to the Windows Azure portal")

	_Log on to the Windows Azure Management Portal_

1. Click **New** on the command bar.

	![Creating a new Web Site](Images/new-website.png?raw=true "Creating a new Web Site")

	_Creating a new Web Site_

1. Click **Compute**, **Web Site** and then **Quick Create**. Provide an available URL for the new web site and click **Create Web Site**.

	> **Note:** A Windows Azure Web Site is the host for a web application running in the cloud that you can control and manage. The Quick Create option allows you to deploy a completed web application to the Windows Azure Web Site from outside the portal. It does not include steps for setting up a database.

	![Creating a new Web Site using Quick Create](Images/quick-create.png?raw=true "Creating a new Web Site using Quick Create")

	_Creating a new Web Site using Quick Create_

1. Wait until the new **Web Site** is created.
	
	> **Note:** By default, Windows Azure provides domains at _azurewebsites.net_, but also gives you the possibility to set custom domains using the Windows Azure Management Portal. However, you can only manage custom domains if you are using certain Web Site modes.
	
	> Windows Azure offers 3 modes for users to run their web sites - Free, Shared, and Reserved. In Free and Shared mode, all web sites run in a multi-tenant environment and have quotas for CPU, Memory, and Network usage. You can mix and match which sites are Free (strict quotas) vs. Shared (more relaxed quotas). The maximum number of free sites may vary with your plan. The Reserved mode applies to ALL of your sites and makes them run on dedicated virtual machines that correspond to the standard Azure compute resources. You can find the Web Sites Mode configuration in the **Scale** menu of your Web Site.

	> ![Web Site Modes](Images/web-site-modes.png?raw=true "Web Site Modes")

	> If you are using **Shared** or **Reserved** mode, you will be able to manage custom domains for your Web Site. To do so, go to the **Configure** menu of your Web Site and under _domain names_ click **Manage Domains**.

	> ![Manage Domains](Images/manage-domains.png?raw=true "Manage Domains")

	> ![Manage Custom Domains](Images/manage-custom-domains.png?raw=true "Manage Custom Domains")

1. Once the Web Site is created, click the link under the **URL** column. Check that the new Web Site is working.

	![Browsing to the new web site](Images/navigate-website.png?raw=true "Browsing to the new web site")

	_Browsing to the new web site_

	![Web site running](Images/website-working.png?raw=true "Web site running")

	_Web site running_

1. Go back to the portal and click the name of the web site under the **Name** column to display the management pages.

	![Opening the web site management pages](Images/go-to-the-dashboard.png?raw=true "Opening the web site management pages")
	
	_Opening the Web Site management pages_

1. In the **Dashboard** page, under the **quick glance** section, click the **Download the publish profile** link.

	> **Note:** The _publish profile_ contains all of the information required to publish a web application to a Windows Azure website for each enabled publication method. The publish profile contains the URLs, user credentials and database strings required to connect to and authenticate against each of the endpoints for which a publication method is enabled. **Microsoft WebMatrix 2**, **Microsoft Visual Web Developer** and **Microsoft Visual Studio 2012** support reading publish profiles to automate configuration of these programs for publishing web applications to Windows Azure websites. 

	![Downloading the web site publish profile](Images/download-publish-profile.png?raw=true "Downloading the web site publish profile")
	
	_Downloading the Web Site publish profile_

1. Download the publish profile file to a known location. Further in this exercise you will see how to use this file to publish a web application to a Windows Azure Web Sites from Visual Studio.

	![Saving the publish profile file](Images/save-link.png?raw=true "Saving the publish profile")
	
	_Saving the publish profile file_

<a name="Ex1Task2"></a>
#### Task 2 – Configuring the Database Server ####

1. You will need a SQL Database server for storing the application database. You can view the SQL Database servers from your subscription in the portal at **Sql Databases** | **Servers**. If you do not have a server created, you can create one using the **Add** button at the bottom of the page. Make note of the server **NAME**, **MANAGE URL**, and **ADMINISTRATOR LOGIN**, and obtain the server's password, which is not shown in the portal. You will use this information next.
Do not create the database yet, as it will be created by Entity Framework when running the application.

	![SQL Database Server Dashboard](Images/sql-database-server-dashboard.png?raw=true "SQL Database Server Dashboard")

	_SQL Database Server Dashboard_

1. In the next task, you will test the database connection from Visual Studio. For that reason, you need to include your local IP address in the server's list of **Allowed IP Addresses**. To do that, click **Configure**, and then click the ![add-client-ip-address-ok-button](Images/add-client-ip-address-ok-button.png?raw=true) button next to the IP address labeled **CURRENT CLIENT IP ADDRESS**.

	![Adding Client IP Address](Images/add-client-ip-address.png?raw=true)

	_Adding Client IP Address_

1. Once the **Client IP Address** is added to the allowed IP addresses list, click **Save** to confirm the changes.

	![Confirm Changes](Images/add-client-ip-address-confirm.png?raw=true)

	_Confirm Changes_

<a name="Ex1Task3"></a>
#### Task 3 – Publishing an ASP.NET MVC 4 Application using Web Deploy ####

1. Go back to the MVC 4 solution. In the **Solution Explorer**,  right-click the web site project and select **Publish**.

	![Publishing the Application](Images/publishing-the-application.png?raw=true "Publishing the Application")

	_Publishing the web site_

1. In the **Profile** page, click **Import** and select the profile settings file you downloaded earlier in this exercise.

	![Importing the publish profile](Images/importing-the-publish-profile.png?raw=true "Importing the publish profile")

	_Importing publish profile_

1. In the **Connection** page, leave the imported values and click **Validate Connection**. Once the validation is completed, click **Next**.

	> **Note:** Validation is completed once you see a green checkmark appearing next to the **Validate Connection** button.

	![Validating connection](Images/validating-connection.png?raw=true "Validating connection")

	_Validating connection_

1. In the **Settings** page, under the **Databases** section, click the button next to the **CustomerContext** textbox.

	![Web deploy configuration](Images/web-deploy-configuration.png?raw=true "Web deploy configuration")

	_Web deploy configuration_

1. Configure the database connection as follows and then click **OK**:
	* In the **Server name**, type your SQL Database server URL. This is the domain name portion of the **MANAGE URL** copied earlier (for example, given _https://[yourserver].database.windows.net/_ as the MANAGE URL, enter _[yourserver].database.windows.net_).
	* In **User name**, type your server administrator login name.
	* In **Password**, type your server administrator login password.
	* Type a new database name, for example: _MVC4SampleDB_.

	![Configuring destination connection string](Images/configuring-destination-connection-string.png?raw=true "Configuring destination connection string")

	_Configuring destination connection string_

1. Copy the connection string value from **CustomerContext** to use it later. Then click **Next**.

	![Connection string pointing to SQL Database](Images/sql-database-connection-string.png?raw=true "Connection string pointing to SQL Database")

	_Connection string pointing to SQL Database_

1. In the **Preview** page, click **Publish**.

	![Publishing the web application](Images/publishing-the-web-application.png?raw=true "Publishing the web application")

	_Publishing the web application_

1. Once the publishing process finishes, your default browser will open the published web site. Verify that the web site was successfully published in Windows Azure.

	![Application published to Windows Azure](Images/application-published-to-windows-azure.png?raw=true "Application published to Windows Azure")

	_Application published to Windows Azure_

1. Go to **/Customer** to verify that the Customers views are working as expected. You can try adding a new Customer to verify it is successfully saved to the database.

	![Application Running](Images/application-running.png?raw=true "Application Running")

	_Add Customer view_

---

<a name="Exercise2"></a>
### Exercise 2: Publishing an MVC 4 Application using Git ###

In this exercise you will publish again the web application you created in exercise 1, but this time using Git.

> **Note:** If you did not execute exercise 1 you can still perform this exercise by deploying the site located in the **Source\Assets** folder of this lab.

<a name="Ex2Task1"></a>  
#### Task 1 – Setting up Git Publishing ####

1. Go back to the Windows Azure Management Portal. In the **Web Sites** section, locate the web site you created in the previous exercise and open its dashboard. To do this, click the web site's **Name**. 

1. Under the quick glance section, click **Set up deployment from source control** link.

	![Set up deployment from source control](Images/set-up-git-publishing.png?raw=true "Set up deployment from source control")

	_Set up deployment from source control_

1. Once the **Set up Deployment** window is displayed, select **Local Git repository** and click **Next**.

	![Set up Git Deployment](Images/selecting-git-source-control.png?raw=true "Set up Git Deployment")

	_Set up Git Deployment_

1. A message indicating that your Git repository is being created will appear. 

	> **Note:** You may be prompted for username and password.

	![Creating Git Repository](Images/creating-git-repository.png?raw=true "Creating Git Repository")

	_Creating Git repository_

1. Wait until your Git repository is ready to use before continue with the following task.

	![Git repository ready](Images/git-repository-ready.png?raw=true "Git repository ready")

	_Git repository is ready_

1. Copy the **Git URL** value. You will use it later in this exercise.

<a name="Ex2Task2"></a>  
#### Task 2 – Pushing the Application to Windows Azure using Git ####

1. Open the solution you have obtained in [exercise 1](#Exercise1) with Visual Studio. Alternatively, you can open the **MVC4Sample.Web** solution located in the **Source\Assets** folder of this lab.

1. Press **CTRL+SHIFT+B** to build the solution and download the NuGet package dependencies.

1. Open Web.config and update the **CustomerContext** connection string using the one obtained from [Exercise 1 - Task 3](#Ex1Task3). You can also use the following connection string replacing the placeholders.

	````XML
	<connectionStrings>
	 ...
	 <add name="CustomerContext" connectionString="Data Source=tcp:{SERVER_URL};Initial Catalog=MVC4SampleDB;User ID={SERVER_ADMIN_LOGIN};Password={PASSWORD}"
		providerName="System.Data.SqlClient" />
	</connectionStrings>
	````

1. Open a new **Git Bash** console and insert the following commands. Update the _[YOUR-APPLICATION-PATH]_ placeholder with the path of the MVC 4 solution you've created in [Exercise 1](#Exercise1). 
	
	<!-- mark:1-6 -->
	````CommandPrompt
	cd "[YOUR-APPLICATION-PATH]"
	git init
	git config --global user.email "{username@example.com}"
	git config --global user.name "{your-user-name}"
	git add .
	git commit -m "Initial commit"
	````

	![Git initialization and first commit](Images/git-initialization-and-first-commit.png?raw=true "Git initialization and first commit")

	_Git initialization and first commit_

1. Push your web site to the remote **Git** repository by running the following command. Replace the placeholder with the URL you obtained from the Windows Azure Management Portal. You will be prompted for your deployment password.

	<!-- mark:1-2 -->
	````CommandPrompt
	git remote add azure [GIT-CLONE-URL]
	git push azure master
	````

	![Pushing to Windows Azure](Images/pushing-to-windows-azure.png?raw=true "Pushing to Windows Azure")

	_Pushing to Windows Azure_

	> **Note:** When you deploy content to the FTP host or GIT repository of a Windows Azure website you must authenticate using **deployment credentials** that you create from the website’s **Quick Start** or **Dashboard** management pages.  If you don't know your deployment credentials you can easily reset them using the management portal. Open the web site **Dashboard** page and click the **Reset your deployment credentials** link. Provide a new password and click **OK**. Deployment credentials are valid for use with all Windows Azure websites associated with your subscription. 

1. In order to verify the web site was successfully pushed to Windows Azure, go back to the **Windows Azure Management Portal** and click **Web Sites**.

1. Locate your **Web Site** (where you deployed the application) and click its **Name** to see the **Dashboard**.

1. Click **Deployments** to see the **deployment history**. Verify that there is an **Active Deployment** with your _"Initial Commit"_.

	![Deployment](Images/deployment.png?raw=true "Deployment")

	_Active deployment_

1. Finally, click **Browse** on the bottom bar to go to the web site. 

	![Browse web site](Images/browse-web-site.png?raw=true "Browse web site")

	_Browse web site_

1. If the application was successfully deployed, you will see the ASP.NET MVC 4 template's default home page.

	![Application Running in Windows Azure](Images/application-published-to-windows-azure.png?raw=true "Application Running in Windows Azure")

	_Application Running in Windows Azure_
	
1. Go to **/Customer** to verify that the Customers views are working as expected. You can try adding a new Customer to verify it is successfully saved to the database.

	![Application Running](Images/application-running.png?raw=true "Application Running")

	_Add Customer view_

---
<a name="Summary"></a>
## Summary ##
In this hands-on lab, you have created a new MVC web site using MVC 4 Scaffolding and published it to Windows Azure Web Sites. Web site publication and deployment has never been easier in Windows Azure. Using familiar tools such as Web Deploy or Git, and virtually no changes to the development workflow, Windows Azure Web Sites is the next step in the Microsoft Azure platform for web developers. 
