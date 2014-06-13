<a name="title" />
# Service Bus Message Me #

---

<a name="Overview" />
## Overview ##

The Microsoft Azure Service Bus provides a hosted, secure, and widely available infrastructure for widespread communication, large-scale event distribution, naming, and service publishing.

This demo is designed to show how the Microsoft Azure Service Bus messaging can be used by applications to exchange messages in a loosely coupled way for improved scale and resiliency. This is a fun example that consists of a simple ASP.NET MVC Mobile Web Application that sends messages to a Service Bus queue and a console application that reads from the queue and displays the messages. 


<a id="goals" />
### Goals ###
In this demo, you will see how to:

1.	Create & Deploy Microsoft Azure Web Site
1.	Monitoring & Scaling an existing Microsoft Azure Web Site
1. Improve your application adding Microsoft Azure Service Bus capabilities

<a name="technologies" />
### Key Technologies ###

- Microsoft Azure subscription with the Websites enabled - you can sign up for free trial [here][1]
- [Microsoft Visual Studio 2012][2]
- [ASP.NET MVC 4][3]


[1]: http://bit.ly/WindowsAzureFreeTrial
[2]: http://www.microsoft.com/visualstudio/11/en-us
[3]: http://www.asp.net/mvc/mvc4

<a name="setup" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment.

1. Go to <https://manage.windowsazure.com/>

1. To use **Service Bus**, you need to access the previous management portal version. In order to do this, hover the mouse pointer over **Preview** in the main page header and click **Take me to the previous portal**.

1. Click **Service Bus, Access Control and Caching** and select **Service Bus**.

1. Click the **New** button to create a new Service Namespace.

1. In the **Create a new Service Namespace** dialog, enter a valid namespace and select a Country/Region and the Subscription where you want to create the namespace. Ensure you have selected "Service Bus" from the "Available Services" list and click **Create Namespace**.

	![Create a new Service Namespace](Images/create-a-new-service-namespace.png?raw=true "Create a new Service Namespace")

	_Create a new Service Namespace_

1. Once your Service Namespace is created select it and click **View** in the **Default Key** property.

	![Default Key](Images/default-key.png?raw=true "Default Key")

	_Default Key_

1. Take note of the default issuer name and key.

1. Open Windows Explorer and browse to the demo's **Source** folder.

1. Open Config.Local.xml file and replace the placeholders with your Service Bus endpoint URL and issuer and key values.

1. Execute the **Setup.Local.cmd** file with Administrator privileges to launch the setup process that will configure your environment and install the Visual Studio code snippets for this demo.

1. If the User Account Control dialog is shown, confirm the action to proceed.

1. Execute the **Setup.Azure.cmd** file with Administrator privileges to initialize the Azure Service Bus queue required for the demo.

>**Note 1:** Make sure you have checked all the dependencies for this demo before running the setup.

>**Note 2:** The setup script copies the source code for this demo to a working folder that can be configured in the **Config.Local.xml** file (by default, C:\Projects). From now on, this folder will be referenced in this document as the **working folder**.

---

<a name="Demo" />
## Demo ##

This demo is composed of the following segments:

1. [Create, Deploy, and Scale Microsoft Azure Web Site](#segment1).
1. [Add Services Bus and Send Messages](#segment2).


<a name="segment1" />
### Create, Deploy, and Scale Microsoft Azure Web Site ###

1. Launch Visual Studio 2012. Click the Start button and type _Visual Studio_, then press **ENTER**.

	![Launching Visual Studio](Images/launching-visual-studio.png?raw=true "Launching Visual Studio")

	_Launching Visual Studio_

	>**Speaking Point:** 
	>
	> During this demo we're going to build a mobile Web application that we'll be hosted in the cloud using Microsoft Azure. For building this application we'll use ASP.net and Visual Studio 2012.
	>
	>Let's start by launching Visual Studio 2012.
	>

1. In the **New Project** dialog box, select **C# | Web** under templates and then select **ASP.NET MVC 4 Web Application** in the right pane. Make sure the target framework of the project is 4.0. Enter a name for the application and then click **OK**.

	![New ASP.NET Application](Images/new-aspnet-application.png?raw=true "New ASP.NET Application")

	_New ASP.NET Application_

1. In the New ASP.NET ASP MVC 4 project's templates, select **Mobile Application**.

	![Selecting Mobile Application Template](Images/selecting-mobile-application-template.png?raw=true "Selecting Mobile Application Template")

	_Selecting Mobile Application Template_

	>**Speaking Point:** 
	>
	> If I pick the mobile application template, this is going to give me a project that has all the files I need in order to build a Web application that works against any type of phone or tablet device.

1. Press F5 to run the application.

	>**Speaking Point:** 
	>
	> I will now run the application locally, Visual Studio 2012 lets me run it using a desktop browser or using new third-party emulators.
	> You can see here a very simple app, with a nice navigation and animation UI that works with a variety of different phones and tablets.

1. Show the application UI.

	![Application Running Locally](Images/application-running-locally.png?raw=true)

	_Application Running Locally_

1. Open Microsoft Azure Management Portal.

	![Microsoft Azure Management Portal Home Page](Images/windows-azure-management-portal-home-page.png?raw=true "Microsoft Azure Management Portal Home Page")

	_Microsoft Azure Management Portal Home Page_

	>**Speaking Point:** 
	>
	>Now, let's switch to the Microsoft Azure Management Portal. I'm going to create a Web Site, which is a new Microsoft Azure feature.
	> I can choose which data center I want to create it in. This is going to provision for me a website that I can use to deploy any Web-based application.

1. Within Microsoft Azure Portal, click **New** | **Compute** | **Web Site** | **Click Create**. Enter a descriptive title (e.g. _MessageMeDemo_) and then click **Create Web Site**.

	![Creating a new Web Site](Images/creating-a-new-web-site.png?raw=true "Creating a new Web Site")

	_Creating a new Web Site_

1. Wait until the Web Site finishes provisioning.

	![Provisioning the new Web Site](Images/provisioning-the-new-web-site.png?raw=true "Provisioning the new Web Site")

	_Provisioning the new Web Site_

1. Once the Web Site's state changes to **Running** open the Web Site's **Dashboard** and scroll down to the **Quick Glance** section. Click **Download Publish Profile**.

	![Web Site's Dashboard](Images/web-sites-dashboard.png?raw=true "Web Site's Dashboard")

	_Web Site's Dashboard_

	>**Speaking Point:** 
	>
	> You'll notice how fast it is now inside Microsoft Azure. We have just stood up a new website in one of Azure data centers. We can see the website's Dashboard page, which doesn't show any monitoring data yet, because I don't actually have my website deployed.
	>
	> Let's deploy the application to the website I've just created. For this purpose, we could choose between different approaches like using standard FTP tools and just copy the bits up; use our Team Foundation Server to link the project to the web site and any time someone checks in source-code into that project automatically do a build, run your unit tests and, if they succeed, deploy the project into Microsoft Azure; or, what will be our choice, directly use Visual Studio IDE for the deployment.

1. Save the **Publish Profile** file to your computer.

	![Downloading Publish Settings](Images/downloading-publish-settings.png?raw=true "Downloading Publish Settings")

	_Downloading Publish Settings_

	>**Speaking Point:** 
	>
	> From the management portal, I'll download a publishing profile from the Dashboard page.
	> This is just an XML file that has all of my publish settings which I will use to set up Visual Studio so we can just directly deploy within the IDE.

1. Go back to the Visual Studio instance, and stop the application (if it is still running). Right click the project and click **Publish**.

	![Publishing Application](Images/publishing-application.png?raw=true "Publishing Application")

	_Publishing Application_

	>**Speaking Point:** 
	>
	> Setting up Visual Studio for deploying my website is really easy, all we have to do is right-click the project, select Publish and then import the Publish settings file we've just download from the portal. Then I'll just click Publish and wait a moment until the deployment finishes.

1. In the **Profile** page, click **Import** in order to import the recently downloaded publish profile.

	![Importing Publish Settings File](Images/importing-publish-settings-file.png?raw=true "Importing Publish Settings File")

	_Importing Publish Settings File_

1. Browse to your Publish Profile's file location, select the file and click **Open**.

	![Selecting the Publish Settings file](Images/selecting-the-publish-settings-file.png?raw=true "Selecting the Publish Settings file")

	_Selecting the Publish Settings file_

1. In the **Connection** page, click **Publish**.

	![Connection Page](Images/connection-page.png?raw=true "Connection Page")

	_Connection Page_

1. Once the Publishing process finishes, the site will be opened in the browser.

	![Published Web Site](Images/published-web-site.png?raw=true "Published Web Site")

	_Published Web Site_

	>**Speaking Point:** 
	>
	>This is going to cause Visual Studio to package up our files, upload them into Microsoft Azure data center and after that launch a browser with our application.
	>
	> You can see here, our application running in Microsoft Azure.

1. Now that we have our Website running in the cloud, we'll go back to the Microsoft Azure Portal. Click **Monitor**.

	![Web Site's Monitor](Images/web-sites-monitor.png?raw=true "Web Site's Monitor")

	_Web Site's Monitor_

	>**Speaking Point:** 
	>
	> If we go back to the portal we can see dashboard statistics around the application I've just deployed. One of the things you'll notice here is a number of statistics in terms of number of request that have hit so, I actually start getting real-time statistics around this deployment.

1. In the Microsoft Azure Portal, click **Scale** and update the number of instances to _3_. Click **Save** to reflect the changes.

	![Increasing Web Site's instances](Images/increasing-web-sites-instances.png?raw=true "Increasing Web Site's instances")

	_Increasing Web Site's instances_

	>**Speaking Point:** 
	>
	> Now, that we have our application running I'd like to increase the number of instances it has. 
	> All we have to do is switch to Scale section and set the number of instances to 3. This will automatically update our website adding 2 more instances.

<a name="segment2" />
### Add Services Bus and Send Messages ###

1. Switch back to Visual Studio.

1. Select **References** and click **Manage NuGet Packages**.

	![Manage NuGet Packages](Images/manage-nuget-packages.png?raw=true "Manage NuGet Packages")

	_Manage NuGet Packages_

	>**Speaking Point:** 
	>
	> The Service Bus is another very interesting feature of Microsoft Azure, a really powerful messaging capability that allows me to link code running in the cloud with code running on premise.
	>
	> Let's make this application a little bit richer; I'll now extend this application by including some additional capabilities to it.
	> I'll start by adding Service Bus references using the package manager in .NET. in order to get all the APIs that I need to program.

1. Select **NuGet official package source** and search for **WindowsAzure.ServiceBus** package.

1. Select **Microsoft Azure Service Bus** and click **Install**.

	![Microsoft Azure Service Bus NuGet Package](Images/windows-azure-service-bus-nuget-package.png?raw=true "Microsoft Azure Service Bus NuGet Package")

	_Microsoft Azure Service Bus NuGet Package_

1. Read and **Accept** the **License Acceptance**.

	![Service Bus License Acceptance](Images/service-bus-license-acceptance.png?raw=true "Service Bus License Acceptance")

	_Service Bus License Acceptance_

1. Open **HomeController.cs** file and replace the content of the class with the code snippet titled _Send Message Action_.

	![HomeController Class](Images/homecontroller-class.png?raw=true "HomeController Class")

	_HomeController Class_

	>**Speaking Point:** 
	>
	> I'll modify my application so anyone can send me messages which we'll be shown in real time. We're going to write some server code that's going to take that message, put it in what's called a message object, and stick it in what's called a Service Bus queue.

1. Click **QueueClient** tooltip and add namespace **Microsoft.ServiceBus.Messaging**

	![HomeController Class - Queue Client](Images/homecontroller-class-queue-client.png?raw=true "HomeController Class - Queue Client")

	_HomeController Class - Queue Client_

1. Open the Web.Config file.

1. In the appSettings section, locate the **Microsoft.ServiceBus.ConnectionString** and replace the placeholders with your Service Bus credentials. 

	![Web.Config file](Images/webconfig-file.png?raw=true "Web.Config file")

	_Web.Config file_

	>**Speaking Point:** 
	>
	> Now, I'll need to update the ServiceBus connection string with my account's credentials.

1. Open the **Home/Index.cshtml** View.

1. Replace all code with code snippet  titled _Home View_.

	![Updating Index View](Images/updating-index-view.png?raw=true "Updating Index View")

	_Updating Index View_

	>**Speaking Point:** 
	>
	> Finally, I'm going to modify this Web app to have a nice little HTML form on the front end. I want this app to be a little social interactive application where all of you can send me messages that we'll be displayed on the screen. 

1. Repeat the steps to publish the updated web site.

	![Re-publishing the web site](Images/re-publishing-the-web-site.png?raw=true "Re-publishing the web site")

	_Re-publishing the web site_

	>**Speaking Point:** 
	>
	> Before trying out our new social application, let's redeploy it to our Microsoft Azure website.
	> From Visual Studio we can re deploy applications; this means that instead of redeploying all the application when we want to actually supply an update, we can just go ahead and hit **redeploy**. 
	> When I do that, it will just deploy the incremental changes to the existing website application.

1. Once the publishing process finishes, the site will be opened in your default browser.

	![Message Me application published](Images/message-me-application-published.png?raw=true "Message Me application published")

	_Message Me application published_

1. Open the **MessageReceiver** console application from **Assets\MessageReceiver** located in the working folder.

1. Open the app.config file.

1. In the appSettings section, locate the **Microsoft.ServiceBus.ConnectionString** and replace the placeholders with your Service Bus credentials. 

1. Press **F5** to run the client application.

	![Message Me application published](Images/console-application-running.png?raw=true "Console Application Running")

	_Console Application Running_

	>**Speaking Point:** 
	>
	> Before testing the Web site, I'll run a simple client application I've already created. This client is a console application that just receives and shows messages stored in a specific queue.

	![Message Me application published](Images/console-application-receiving-messages-from-website.png?raw=true "Console Application receiving messages from Web Site")

	_Console Application receiving messages from Web Site_

	>**Speaking Point:** 
	>
	> Now that we have everything in place, let's start sending messages to the queue and verifying the client application shows them in real time.
	>
	>You can also go to [YOUR-WEBSITE-URL], and send me your own real time messages.

---

<a name="summary" />
## Summary ##

In this demo, you saw how to create, deploy, monitor and scale a Microsoft Azure Web Site. Additionally, you improved your application by adding Microsoft Azure Service Bus messaging capabilities.
