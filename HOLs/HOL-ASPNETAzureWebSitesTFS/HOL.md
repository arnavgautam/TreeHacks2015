<a name="Title"></a>
# Publishing ASP.NET Applications with Microsoft Azure Websites and Team Foundation Service #

---
<a name="Overview"></a>
## Overview ##

In this hands-on lab you will learn how to link a Microsoft Azure Web Site to a Team Foundation Service (TFS) repository in order to check in your changes and automatically reflect those updates in your web site.

Following the exercises, you will create a new **Web Site** from the **Microsoft Azure Management Portal** and enable TFS Publishing.
Finally, you will create a simple MVC4 application in **Microsoft Visual Studio 2012** and perform a check in to your TFS server. You will notice how your changes are automatically reflected in your Microsoft Azure web site after updating the TFS repository.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a web site from the Microsoft Azure Management Portal
- Link a Microsoft Azure Web Site to a new or existing TFS Repository
- Connect a Visual Studio web application to TFS
- Perform a check in from Microsoft Visual Studio 2012 to TFS and reflect the changes in your web site.

<a name="Prerequisites"></a>
### Prerequisites ###

- [Microsoft Visual Studio 2012 Express for Web][1] or greater

- A Microsoft Azure subscription with the Websites Preview enabled - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

[1]: http://www.microsoft.com/visualstudio/

> **Note:** This lab was designed for Windows 8.

<a name="Setup"></a>
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab’s **Source** folder.

1. Right click the **Setup.cmd** file and click **Run as administrator**. This will launch the setup process that will check the necessary dependencies.

>**Note:** Make sure you have checked all the dependencies for this lab before running the setup. 

---
<a name="Exercises"></a>
## Exercises ##

This Hands-on Lab includes the following exercises:

1.	[Create a Microsoft Azure Web Site](#Exercise1)
1.	[Connecting a Microsoft Azure Web Site to a TFS Project](#Exercise2)
1.	[Connecting Microsoft Visual Studio 2012 to a TFS Repository](#Exercise3)

Estimated time to complete this lab: **30** minutes.

<a name="Exercise1"></a>
### Exercise 1: Create a Microsoft Azure Web Site ###

In this exercise, you will use the **Microsoft Azure Management Portal** to create a new Web Site. In the following exercises you will use this web site to publish an MVC4 application using TFS check-in operation.

<a name="Ex1Task1"></a>
#### Task 1 – Creating a new Web Site ####

In this task, you will create a new Web Site from the Microsoft Azure Management Portal.

1. Open [Microsoft Azure Management Portal](https://manage.windowsazure.com) and log in with your Microsoft Azure Account's credentials.

1. Click **New** | **Compute** | **Web Site** | **Quick Create**. Select a unique **URL** for your web site, for example _WebSitesAndTFS_.

	![Creating a new Web Site](Images/creating-a-new-web-site.png?raw=true "Creating a new Web Site")

	_Creating a new Web Site_

1. In the **Microsoft Azure Management portal**, click **Websites** and then click the **name** of your web site to go to its **Dashboard**.

	![Websites page](Images/web-sites-page.png?raw=true "Websites page")

	_Websites page_

1. This is your web site's **Dashboard** page, from where you will be able to manage your web site's settings and find all the information related to it. Do not close this window since you will use it in the next exercise.

	![site-dashboard](Images/web-sites-dashboard.png?raw=true "Site Dashboard")

	_Web Site's Dashboard_

<a name="Exercise2"></a>
### Exercise 2: Connecting a Microsoft Azure Web Site to a TFS Project ###

In this exercise you will link your Microsoft Azure Web Site to a TFS Project. By doing this, you will be able to check in your changes and automatically update your Web Site.

<a name="Ex2Task1"></a>
#### Task 1 – Connecting a Microsoft Azure Web Site to a TFS Project ####

In this task you will connect your Microsoft Azure Web Site to a TFS Project. You can both create a new account and project in TFS or use an existing one.

1. If not already open, open the [Microsoft Azure Management Portal](https://manage.windowsazure.com) and go to your web site's **Dashboard**.

1. Click **Set up TFS publishing** link at the bottom of the page.

	![Setting up TFS publishing](Images/setting-up-tfs-publishing.png?raw=true "Setting up TFS publishing")

	_Setting up TFS publishing_

1.	Now, you will configure the TFS account you will link to your Web Site. If you already have a TFS account, go directly to **step 12** of this task, otherwise, click **Create a TFS account now**.

	>**Note:** If you choose to use an existing user, make sure you have a Team Project available.

	![Creating a new TFS account](Images/creating-a-new-tfs-account.png?raw=true "Creating a new TFS account")

	_Creating a new TFS account_

1. In the **Account Creation** page, select _Windows Live ID_ as your **Identity Provider** and set the **Server URL** (e.g. _WebSitesAndTFSServer_) for your TFS Server. Click **Create Account** to continue.

	![TFS New account registration](Images/tfs-new-account-registration.png?raw=true "TFS New account registration")

	_TFS New account registration_

1. If you are not logged on with the Identity Provider you selected, TFS will ask you to sign in.

1. Once in the TFS account home page, click **Create a team project** in order to generate a new TFS project.

	![TFS Server Home Page](Images/create-a-team-project.png?raw=true "TFS Server Home Page")

	_TFS Server Home Page_

1. In the **Create New Team Project** dialog, set the **Project Name** to _WebSitesAndTFSProject_, add a **Description** for your project and leave the **Process Template** with its default value. Then, click **Create Project**.

	![Creating a new team project](Images/creating-a-new-team-project.png?raw=true "Creating a new team project")

	_Creating a new team project_

1. Wait until the TFS project is created.

	![Project Creation Process](Images/project-creation-process.png?raw=true "Project Creation Process")

	_Project Creation Process_

1.	Once the creation process finishes, click **Navigate to Project**. You will see the TFS Project's Home Page.

	![TFS Project Page](Images/tfs-project-page.png?raw=true "TFS Project Page")

	_TFS Project Page_

1.	Copy the TFS Server's **URL** prefix and close the window.

1.	Go back to the web site's **Dashboard**, paste the TFS Server's URL prefix into the **Existing TFS User** field and click **Authorize Now**.

	![TFS Account URL](Images/tfs-account-information.png?raw=true "TFS Account URL")

	_TFS Account URL_

1.	A new window will be prompted requesting for permission to link the Web Site project to the TFS Project. Click **Accept** to allow your Web Site use the specified TFS account.

	![Requesting TFS Project Access](Images/requesting-tfs-project-access.png?raw=true "Requesting TFS Project Access")

	_Requesting TFS Project Access_

1.	In the **Set Up Publishing From Project** page, choose the TFS Project from the **Select Team Project to Publish** drop-down list.

	![Choosing Your TFS Project](Images/choosing-your-tfs-project.png?raw=true "Choosing Your TFS Project")

	_Choosing Your TFS Project_

1.	Finally, in the **Deployments** page, you will see a message showing that the TFS project has been successfully linked.

	![TFS Project Linked](Images/tfs-project-linked.png?raw=true "TFS Project Linked")

	_TFS Project Linked_


<a name="Exercise3"></a>
### Exercise 3: Connecting Microsoft Visual Studio 2012 to a TFS Repository ###

In this exercise, you will create a simple MVC4 Application, connect it to the TFS Repository and check-in your files only using **Microsoft Visual Studio Express 2012 for Web**.

<a name="Ex3Task1"></a>
#### Task 1 – Connecting Microsoft Visual Studio 2012 to a TFS Repository ####

In this task, you will create an MVC4 Application with Microsoft Visual Studio Express 2012 for Web, connect it with the TFS Repository and a check in your files into the TFS Repository.

1.	In your web site's **Dashboard** page, locate the **Visual Studio** icon and click it. Clicking this icon will open up Microsoft Visual Studio Express 2012 for Web and will connect it directly to the TFS repository you linked in the previous exercise.

	![Opening Visual Studio](Images/opening-visual-studio.png?raw=true "Opening Visual Studio")

	_Opening Visual Studio_

1.	Visual Studio may launch a dialog requesting you to log in to your TFS project. If so, enter the credentials you used when creating the TFS project.

1.	Create a new **ASP.NET MVC 4** project in Microsoft Visual Studio Express 2012 for Web. To do this, select **File** | **New Project**.

1. In the **New Project** dialog, select **ASP.NET MVC 4 Web Application** within **Visual C# Web** templates and set _.NET Framework 4.5_ as your **target framework**. Name it _MyMVC4App_ and click **OK**.

	![New ASP.NET MVC 4 Project](Images/new-aspnet-mvc-4-project.png?raw=true "New ASP.NET MVC 4Project")

	_New ASP.NET MVC 4 Project_

1.	In the **New ASP.NET MVC 4 Project dialog**, select the **Internet Application** template and click **OK**.
	
	![MVC4 Project Template](Images/mvc4-project-template.png?raw=true "MVC4 Project Template")
	
	_MVC ASP.NET MVC 4_

1.	In the **Solution Explorer**, right-click the solution and select **Add Solution to Source Control**.

	![Adding Solution to Source Control](Images/adding-solution-to-source-control.png?raw=true "Adding Solution to Source Control")

	_Adding Solution to Source Control_

1.	In the **Add Solution to Source Control** dialog, select your team project node in order to add the web project to the team project and click **OK**.

	![Adding Solution to Team Project](Images/adding-solution-to-team-project.png?raw=true "Adding Solution to Team Project")

	_Adding Solution to Team Project_

1.	Once the project is added to source control, the Solution Explorer will reflect that files need to be checked into TFS.

	![Project Added to TFS](Images/project-added-to-tfs.png?raw=true "Project Added to TFS")

	_Project Added to TFS_

1.	In Microsoft Visual Studio 2012, switch to **Team Explorer** tab and select **Pending Changes**.

	![Team Explorer](Images/team-explorer.png?raw=true "Team Explorer")

	_Team Explorer_

1. In the **Team Explorer - Pending Changes** pane, enter a **Comment** for the check in, and click **Check In** to commit the files.

	![Team Explorer Pending Changes](Images/team-explorer-pending-changes.png?raw=true "Team Explorer Pending Changes")

	_Team Explorer Pending Changes_

1. If a **Check-in Confirmation** dialog appears, click **Yes**.
	
1. Wait until the Check in finishes. It will show a success message at the top of the pane.

	![Check In Complete](Images/check-in-complete.png?raw=true "Check In Complete")

	_Check In Complete_

<a name="Ex3Task2"></a>
#### Task 2 – Checking in changes to TFS and updating the Web Site ####

In this task, you will update your MVC 4 application and check in your changes to TFS. Additionally, you will create a new **Build Definition** in TFS to automatically build and update your Web Site.

1.	In the **Solution Explorer**, open the **_Layout.cshtml** file located at **Views/Shared** folder. Replace the line of code that reads _your logo here_ with _My TFS Built Application_ and save the changes.

	![Updating Layout View](Images/updating-layout-view.png?raw=true "Updating Layout View")

	_Updating Layout View_

1.	Switch to the **Team Explorer** tab and **Check In** the changes using a new **Comment**.

	![Updating Layout View](Images/updating-layout-view2.png?raw=true "Updating Layout View")
	
	_Updating Layout View_
	
1. Open **Team Explorer**'s menu and select **Builds** to see the builds status from Visual Studio.

	![Switching to the Build details](Images/switching-to-builds.png?raw=true "Switching to the Build details")

	_Switching to the Build details_

1.	Go back to the **TFS Online** application. To do this, locate **TFS Project** link within your web site's **Dashboard**.

	![Accessing TFS Project](Images/accessing-tfs-project.png?raw=true "Accessing TFS Project")

	_Accessing TFS Project_

1. In the **TFS Online** application, click **Build** tab. You should see that a new build has been queued. Wait until the build is complete (you may need to manually refresh the page to reflect the changes).

	![Build Queued](Images/build-queued.png?raw=true "Build Queued")
	
	_Build Queued_

	![Build completed](Images/build-completed.png?raw=true "Build completed")

	_Build Completed_

	>**Note:** The TFS build will automatically compile and publish your changes to your web site, making it much more easier to update your Microsoft Azure Websites.

1.	Go back to the **Microsoft Azure Management Portal**, and go to your site's **Dashboard** page. Click **Browse** at the bottom of the **Dashboard** page. 

	![Browse the Web Site](Images/browse-the-web-site.png?raw=true "Browse the Web Site")

	_Browse the Web Site_

1. The web site will be opened, reflecting the changes you committed to your TFS Project.

	![Application Running](Images/application-running.png?raw=true "Application Running")

	_Application Running_

---

<a name="Summary"></a>
## Summary ##


In this hands-on lab you learned how to link a Microsoft Azure Web Site to a Team Foundation Service (TFS) repository in order to check in your changes and automatically reflect those updates in your web site. You created a new web site from the Microsoft Azure Management Portal and enabled TFS Publishing to it. You created a simple MVC4 application in Visual Studio 2012, and then performed a check-in to your TFS server. You demonstrated how your changes are automatically reflected in your Microsoft Azure web site after updating the TFS repository.
