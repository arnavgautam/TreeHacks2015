<a name="Title"></a>
# Building and Publishing ASP.NET Applications with Windows Azure Web Sites #

---
<a name="Overview"></a>
## Overview ##

Web site publication and deployment has never been easier in Windows Azure. Using familiar tools such as Web Deploy or Git, and virtually no changes to the development workflow, Windows Azure Web Sites is the next step in the Microsoft Azure platform for web developers. 

In this hands-on lab, you will explore the basic elements of the **Windows Azure Web Sites** service by creating a simple **ASP.NET MVC 5** application, which uses scaffolding to automatically generate the baseline of your application's CRUD (Create, Read, Update and Delete). Then, you will deploy it using Web Deploy from Microsoft Visual Studio and finally, you will enable source control and use Git to publish directly from your local computer.

Starting from a simple model class and without writing a single line of code, you will create a controller that will contain all the CRUD operations, as well as all the necessary views. After publishing and running the solution, you will have the model generated in your SQL Database, together with the MVC logic and views for data manipulation.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a Windows Azure Web Site with Microsoft Visual Studio 2013
- Use Microsoft Visual Studio 2013 to build a new ASP.NET MVC 5 application
- Deploy an ASP.NET application to Windows Azure Web Sites using Web Deploy from Visual Studio
- Configure a Windows Azure Web Site with Git Repository enabled to publish an ASP.NET MVC 5 application using Git

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Visual Studio Express 2013 for Web][1] or greater

- [Windows Azure Tools for Microsoft Visual Studio 2.2 (or later)][2]

- A Windows Azure subscription
	- Sign up for a [Free Trial](http://aka.ms/watk-freetrial)
	- If you are a Visual Studio Professional, Test Professional, Premium or Ultimate with MSDN or MSDN Platforms subscriber, activate your [MSDN benefit](http://aka.ms/watk-msdn) now to start development and test on Windows Azure.
	- [BizSpark](http://aka.ms/watk-bizspark) members automatically receive the Windows Azure benefit through their Visual Studio Ultimate with MSDN subscriptions.
	- Members of the [Microsoft Partner Network](http://aka.ms/watk-mpn) Cloud Essentials program receive monthly credits of Windows Azure at no charge.

[1]: http://www.microsoft.com/visualstudio/
[2]: http://www.microsoft.com/windowsazure/sdk/

---
<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

- [Exercise 1: Creating a Windows Azure Web Site with Visual Studio](#Exercise1)
- [Exercise 2: Creating an MVC 5 Application with Entity Framework](#Exercise2)
- [Exercise 3 Publishing an MVC 5 Application using Web Deploy](#Exercise3)
- [Exercise 4: Publishing an MVC 5 Application using Git](#Exercise4)

<a name="Exercise1"></a>
### Exercise 1: Creating a Windows Azure Web Site with Visual Studio ###

In this exercise, you will take advantage of the Windows Azure tools for Visual Studio to create a Windows Azure web site. Through **Server Explorer**, you will connect to Windows Azure by signing in with your Microsoft Account, and then create a Windows Azure web site and its associated SQL database.

#### Task 1 – Adding a New Windows Azure Web Site from Server Explorer ####

1. Open **Microsoft Visual Studio Express 2013 For Web** and then open **Server Explorer** by selecting **View | Server Explorer**.

1. In **Server Explorer**, right-click the **Windows Azure** node and select **Connect to Windows Azure...**. Sign in using the Microsoft account associated with your Windows Azure account.

	![Connect to Windows Azure](Images/connect-to-windows-azure.png?raw=true)

	_Connect to Windows Azure_

1. After sign in, the **Windows Azure** node is populated with the resources in your Windows Azure subscription.

1. Expand the **Windows Azure** node, right-click the **Web Sites** node and select **Add New Site**.

	![Add new site](Images/add-new-website.png?raw=true)

	_Add new site_

1. In the **Create site in Windows Azure** dialog box, provide the following information:
	- In the **Site name** box, enter an available name for the web site.
	- In the **Location** drop-down list, select the region for the web site. This setting specifies which data center your web site will run in.
	- In the **Database server** drop-down list, select **Create new server**. Alternatively, you can select an existing SQL Server.
	- In the **Database username** and **Database password** boxes, enter the administrator username and password for the SQL Server. If you selected a SQL Server you have created previously, you will be prompted for the password of the previous SQL Server account you created.

1. Click **Create** to create the web site.

	![Create site on Windows Azure](Images/create-site-on-windows-azure.png?raw=true)

	_Create site on Windows Azure_

1. Wait for the new web site to be created.

	> **Note:** By default, Windows Azure provides domains at _azurewebsites.net_, but also gives you the possibility to set custom domains using the Windows Azure Management Portal (right-click your web site from Server Explorer and select **Open Management Portal**). However, you can only manage custom domains if you are using certain Web Site modes.
	
	> Windows Azure offers 3 modes for users to run their web sites - Free, Shared, and Standard. In Free and Shared mode, all web sites run in a multi-tenant environment and have quotas for CPU, Memory, and Network usage. You can mix and match which sites are Free (strict quotas) vs. Shared (more relaxed quotas). The maximum number of free sites may vary with your plan. In Standard mode, you choose which sites run on dedicated virtual machines that correspond to the standard Azure compute resources. You can find the Web Sites Mode configuration in the **Scale** menu of your Web Site.

	> ![Web Site Modes](Images/web-site-modes.png?raw=true "Web Site Modes")

	> If you are using **Shared** or **Standard** mode, you will be able to manage custom domains for your Web Site. To do so, go to the **Configure** menu of your Web Site and under _domain names_ click **Manage Domains**.

	> ![Manage Domains](Images/manage-domains.png?raw=true "Manage Domains")

	> ![Manage Custom Domains](Images/manage-custom-domains.png?raw=true "Manage Custom Domains")

1. Once the web site is created, it will be displayed in Server Explorer under the **Web Sites** node. Right-click the new web site and select **Open in Browser** to check that the web site is running.

	![Browsing to the new web site](Images/browsing-to-the-new-web-site.png?raw=true)

	_Browsing to the new web site_

	![Web site running](Images/website-working.png?raw=true "Web site running")

	_Web site running_

<a name="Exercise2"></a>
### Exercise 2: Creating an MVC 5 Application with Entity Framework ###

In this exercise, you will create a simple ASP.NET MVC 5 web application, using ASP.NET scaffolding with Entity Framework to create the CRUD methods.

<a name="GettingStartedTask1"></a>
#### Task 1 – Creating an ASP.NET MVC 5 Application in Visual Studio ####

1. In **Microsoft Visual Studio Express 2013 For Web**, click the **New Project** link in the start page. Otherwise use  **File** | **New Project**.

	![Creating a new project](Images/creating-a-new-project.png?raw=true)

	_Creating a new project_

1. Create a new **ASP.NET Web Application** using **.NET Framework 4.5**, **C#** and name it **MVCSample.Web**. Click **OK** to continue.

	![Creating a new ASP.NET Web Application](Images/creating-a-new-aspnet-web-application.png?raw=true)

	_Creating a new ASP.NET Web Application_

1. Select the **MVC** template and make sure that **Authentication** is set to **Individual User Accounts**. Click **OK** to continue.

	![Choosing MVC project template](Images/choosing-mvc-project-template.png?raw=true)

	_Choosing MVC project template_
	
1. In the Solution Explorer, right-click **Models** and select **Add | Class** to create a customer class (POCO). Name it _Customer.cs_ and click **Add**.

1. Open the **Customer** class and insert the following properties.

	<!-- mark:10-25 -->
	````C#
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;

	namespace MVCSample.Web.Models
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

1. **Build MVCSample.Web** by using the **Ctrl + Shift + B** keyboard shortcut which will save the changes and build the project.

1. In **Solution Explorer**, right-click the **Controllers** folder and select **Add | Controller**. 

1. In the **Add Scaffold** dialog box, select the **MVC 5 Controller with views, using Entity Framework** scaffolding type and click **Add**.

	![Choosing scaffolding option](Images/scaffolding-options.png?raw=true)

	_Choosing scaffolding option_

1. Complete the scaffolding options in the **Add Controller** dialog box with the following values.
	- In the **Controller name** box, type _CustomerController_.
	- Check the **Use async controller actions** checkbox.
	- In the **Model class** drop-down list, select the **Customer** class.
	- In the **Data Context class** field, click **\<New data context...\>**. In the dialog box displayed, replace the data context class type with **MVCSample.Web.Models.CustomerContext** and click **OK**.
	- In the **Views** section, make sure that all checkboxes are checked.

	![Adding the Customer controller with scaffolding](Images/add-customer-controller.png?raw=true)

	_Adding the Customer controller with scaffolding_
	
1. Click **Add** to create the new controller for **Customer** with scaffolding. Visual Studio will then generate the controller actions, the Customer data context and the views. 
		
	![After creating the Customer controller with scaffolding](Images/customer-scaffolding.png?raw=true)

	_After creating the Customer controller with scaffolding_

1. Open the **CustomerController.cs** file in the **Controllers** folder. Notice that the CRUD action methods have been generated automatically. 

	> **Note:** By checking the **Use async controller actions** checkbox from the scaffolding options in the previous steps, Visual Studio generates asynchronous action methods for all actions that involve access to the Customer data context. It is recommended to use asynchronous action methods for long-running, non-CPU bound requests to avoid blocking the Web server from performing work while the request is being processed.

	````C#
	...

	// POST: /Customer/Create
	// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
	// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> Create([Bind(Include="CustomerId,Name,Phone,Address,Company,Title,Email,Image")] Customer customer)
	{
		if (ModelState.IsValid)
		{
			 db.Customers.Add(customer);
			 await db.SaveChangesAsync();
			 return RedirectToAction("Index");
		}

		return View(customer);
	}

	// GET: /Customer/Edit/5
	public async Task<ActionResult> Edit(int? id)
	{
		if (id == null)
		{
			 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		}
		Customer customer = await db.Customers.FindAsync(id);
		if (customer == null)
		{
			 return HttpNotFound();
		}
		return View(customer);
	}

	...
	````

	_Generated code for the Customer controller_

---

<a name="Exercise3"></a>
### Exercise 3: Publishing an MVC 5 Application using Web Deploy ###

In this exercise, you will publish the application you obtained in [Exercise 2](#Exercise2) to Windows Azure Web Sites by making use of the Web Deploy publishing feature provided by Visual Studio.

#### Task 1 – Publishing an ASP.NET MVC 5 Application using Web Deploy ####

1. Open the solution you have obtained in [Exercise 2](#Exercise2) with Visual Studio. Alternatively, you can open the **MVCSample.Web** solution located in the **Source\Assets** folder of this lab.

1. In **Solution Explorer**, right-click the web site project and select **Publish...**.

	![Publishing the web site](Images/publishing-the-web-site.png?raw=true)

	_Publishing the web site_

1. In the **Profile** page, click **Import...** to import the publish profile.

	![Importing publish profile](Images/importing-the-publish-profile.png?raw=true)

	_Importing publish profile_

1. In the **Import Publish Settings** dialog box, select the **Import from a Windows Azure Web Site** option. If not already signed in, click the **Sign In...** button and sign in using the Microsoft account associated with your Windows Azure account.

1. Select your web site from the drop-down list, and then click **OK**.

	![Importing publish profile from web site](Images/importing-publish-profile-from-web-site.png?raw=true)

	_Importing publish profile from web site_

1. In the **Connection** page, leave the imported values and click **Validate Connection**. Once the validation is completed, click **Next**.

	> **Note:** Validation is completed once you see a green checkmark appearing next to the **Validate Connection** button.

	![Validating connection](Images/validating-connection.png?raw=true)

	_Validating connection_

1. In the **Settings** page, under the **Databases** section, click the down arrow icon of the **CustomerContext** textbox and select the SQL database shwown in the drop-down list.

	> **Note:** The SQL Database you just selected was automatically generated by Visual Studio when you created the Windows Azure Web Site in [Exercise 1](#Exercise1).

	![Selecting the SQL Database](Images/selecting-the-sql-database.png?raw=true)

	_Selecting the SQL Database_

1. In the **Preview** page, click **Publish**.

	![Publishing the web application](Images/publishing-the-web-application.png?raw=true)

	_Publishing the web application_

1. When the publishing process completes, your default browser will open the published web site. Verify that the web site was successfully published in Windows Azure.

	![Application published to Windows Azure](Images/application-published-to-windows-azure.png?raw=true)

	_Application published to Windows Azure_

1. Go to **/Customer** to verify that the Customers views are working as expected. You can try adding a new Customer to verify it is successfully saved to the database.

	![Application Running](Images/customer-view.png?raw=true "Application Running")

	_Customer view_

---

<a name="Exercise2"></a>
### Exercise 4: Publishing an MVC 5 Application using Git ###

In this exercise you will publish again the web application you created in [Exercise 2](#Exercise2), but this time using Git.

> **Note:** If you did not execute [Exercise 2](#Exercise2) you can still perform this exercise by deploying the site located in the **Source\Assets** folder of this lab.

<a name="Ex2Task1"></a>  
#### Task 1 – Setting up Git Publishing ####

1. In Visual Studio, right-click your web site in **Server Explorer** and select **Open in Management Portal**. Sign in using the Microsoft credentials associated with your subscription.

	![Opening the Web Site in Managemen Portal](Images/opening-the-managemen-portal.png?raw=true)

	_Opening the web site in Management Portal_

1. In the **Dashboard** page, click **Set up deployment from source control** link under the **quick glance** section.

	![Set up deployment from source control](Images/set-up-git-publishing.png?raw=true "Set up deployment from source control")

	_Set up deployment from source control_

1. Once the **Set up Deployment** window is displayed, select **Local Git repository** and click **Next**.

	![Set up Git Deployment](Images/selecting-git-source-control.png?raw=true "Set up Git Deployment")

	_Set up Git Deployment_

1. A message indicating that your Git repository is being created will appear. 

	> **Note:** You may be prompted for the deployment credentials (a username and password).

	![Creating Git Repository](Images/creating-git-repository.png?raw=true "Creating Git Repository")

	_Creating Git repository_

1. Wait until your Git repository is ready to use before continue with the following task.

	![Git repository ready](Images/git-repository-ready.png?raw=true "Git repository ready")

	_Git repository is ready_

1. Copy the **Git URL** value. You will use it later in this exercise.

<a name="Ex2Task2"></a>  
#### Task 2 – Pushing the Application to Windows Azure using Git ####

1. Open the solution you have obtained in [exercise 1](#Exercise1) with Visual Studio. Alternatively, you can open the **MVCSample.Web** solution located in the **Source\Assets** folder of this lab.

1. Press **CTRL+SHIFT+B** to build the solution and download the NuGet package dependencies.

1. Open Web.config and update the **CustomerContext** connection string using the one obtained from [Exercise 1 - Task 3](#Ex1Task3). You can also use the following connection string replacing the placeholders.

	````XML
	<connectionStrings>
	 ...
	 <add name="CustomerContext" connectionString="Data Source=tcp:{SERVER_URL};Initial Catalog={DATABASE_NAME};User ID={SERVER_ADMIN_LOGIN};Password={PASSWORD}"
		providerName="System.Data.SqlClient" />
	</connectionStrings>
	````

1. Open a new **Git Bash** console and insert the following commands. Update the _[YOUR-APPLICATION-PATH]_ placeholder with the path to the MVC solution you've created in [Exercise 2](#Exercise2). 
	
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

	![Customer view](Images/customer-view.png?raw=true)

	_Add Customer view_

---
<a name="Summary"></a>
## Summary ##
In this hands-on lab, you have created a new MVC web site using MVC 5 Scaffolding and published it to Windows Azure Web Sites. Web site publication and deployment has never been easier in Windows Azure. Using familiar tools such as Web Deploy or Git, and virtually no changes to the development workflow, Windows Azure Web Sites is the next step in the Microsoft Azure platform for web developers. 
