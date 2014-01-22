<a name="demo2" />
# Demo : BUILD Clips#

## Overview ##

In this demo, we will show how to build and deploy an ASP.NET web site that enables users to browse, play, and upload their own personal videos.  We will then extend the web site to include Web APIs that power a Windows 8 experience.  Finally, the web site project will be deployed to Windows Azure Web Sites and scaled using multiple paid shared instances.

<a name="Goals" />
### Goals ###
In this demo, you will see how to:

1. Extend a Web application to communicate with a Windows 8 application
1. Add Windows Azure Media Services to upload and encode videos
1. Add real-time communication between Web and Windows 8 apps using SignalR
1. Scale an application using Windows Azure Caching
1. Deploy and manage Windows Azure apps using New Relic (optional)

<a name="Technologies" />
### Key Technologies ###

- ASP.NET MVC 4 Web API
- Windows Azure Media Services
- Windows Azure Caching
- Windows Azure Add-ons

<a name="Prerequisites" />
### System Prerequisites ###
- Visual Studio 2012 Express for Web
- Visual Studio 2012 Express for Windows 8
- [ASP.Net Fall 2012 Update] (http://www.asp.net/vnext/overview/fall-2012-update)
- [Player Framework for Windows 8 (v1.0)](http://playerframework.codeplex.com/releases/view/97333)
- [Smooth Streaming Client SDK](http://visualstudiogallery.msdn.microsoft.com/04423d13-3b3e-4741-a01c-1ae29e84fea6)
- [Windows Azure Tools for Microsoft Visual Studio 1.8](http://www.microsoft.com/windowsazure/sdk/)

<a name="Setup" />
### Setup and Configuration ###

In order to execute this demo, you first need to set up your environment by completing the following tasks: 

1. [Creating a Service Bus Namespace for the SignalR Backplane](#setup1)

1. [Creating Storage Account for Media](#setup2)

1. [Creating a Media Services Account](#setup3)

1. [Downloading the Publish Settings File for the Subscription](#setup4)

1. [Configuring the Windows Azure Web Site](#setup5)

1. [Configuring Identity Providers](#setup6)

1. [Running the Setup Scripts](#setup7)

1. [Deploying a Cloud Service and Configuring New Relic (optional)](#setup8)

As you proceed with these manual configuration tasks, you will be required to update information in two configuration files named **Config.Local.xml** and **Config.Azure.xml** that you will find in the **source** folder of the demo. These files are used by the setup scripts to configure the demo and includes, among other settings, storage, database, and service bus connection strings, working directory, cloud service names, media services account credentials, and identity provider settings.

In the **source** folder, you will find several scripts that carry out different setup and cleanup tasks including:
 
- **Setup.cmd**: verifies dependencies, creates the working directory and copies the source files to this directory, and updates the configuration of the solutions with the configured settings. You typically run this script once, before running the demo for the first time.
- **Cleanup.cmd**: this script allows you to clean up some of the resources used for this demo, including deleting the SQL database used by the Azure web site, removing the working directory and local database and resetting the storage emulator.

	> **Note:** This script will not remove the following assets, which need to be removed manually:

	> - Windows Azure Web Site
	> - Windows Azure Media Services account
	> - Storage account used by Media Services
	> - Service Bus namespace

- **Reset.cmd**: executes the cleanup script to reset the environment and runs a reduced setup that does not verify dependencies. This script prepares the environment for running the demo again.
- **Setup.Deployment.cmd**: configures the solution used by segment #5, creates a Storage account for diagnostics, creates a cloud service, creates a new Azure SQL database and deploys the solution to Windows Azure.
- **Cleanup.Deployment.cmd**: deletes the cloud service, the Storage account for diagnostics and the Azure SQL database created by **Setup.Deployment.cmd**.

<a name="setup1" />
**Creating a Service Bus Namespace for the SignalR Backplane**

To create a service namespace:

1. Go to the **Windows Azure Management Portal**.

1. In the navigation pane, select **SERVICE BUS** and then click **CREATE** in the command bar.

1. In the **CREATE A NAMESPACE** dialog box, enter the **NAMESPACE NAME**, select a **REGION**, and then click the check mark to confirm the action.

	> **Note:** Make sure to select the same region for all the assets that you create in Windows Azure for this demo, typically the one closest to you.

	![Creating a new Service Namespace](Images/service-bus-add-namespace.png?raw=true)

	_Creating a new Service Namespace_

1. Select the newly created namespace, click **ACCESS KEY** in the command bar, and then copy the **CONNECTION STRING** setting to the clipboard.

	![Service Bus Namespace Access Key](Images/access-key-servicebus-namespace.png?raw=true)

	_Service Bus Namespace Access Key_

1. Now, open the **Config.Local.xml** file in the **source** folder, locate the **serviceBusConnectionString** setting in the **appSettings** section, and then paste the contents of the clipboard, replacing its current value. Alternatively, you may replace the individual placeholders for the namespace name and namespace key, which you can also obtain as a result of the previous step.

<a name="setup2" />
**Creating Storage Account for Media**

To create the storage account:

1. Go to the **Windows Azure Management Portal**.

1. In the navigation pane, select **STORAGE**, click **NEW** in the command bar, and then **QUICK CREATE**.

1. Enter a unique subdomain for the **URL** of the storage account that you will use to store your media, select a **REGION/AFFINITY GROUP**, and then click the **CREATE STORAGE ACCOUNT** check mark.

	> **Note:** Make sure to select the same region for all the assets that you create in Windows Azure for this demo, typically the one closest to you.

	![Creating a new Storage Account](Images/storage-account-create.png?raw=true)

	_Creating a new Storage Account_

1. Select the newly created storage account, click **MANAGE KEYS** in the command bar, and then copy the value of the **STORAGE ACCOUNT NAME** and **PRIMARY ACCESS KEY** settings.

	![Storage Account Access Keys](Images/storage-account-access-keys.png?raw=true)

	_Storage Account Access Keys_

1. In the **appSettings** section of the **Config.Local.xml** file, locate the **storageAccountConnectionString** setting and replace the placeholders for account name and account key with the corresponding values obtained in the previous step.

<a name="setup3" />
**Creating a Media Services Account**

To create a new Media Services account:

1. Go to the **Windows Azure Management Portal**.

1. In the navigation pane, select **MEDIA SERVICES**, click **NEW** in the command bar, and then **QUICK CREATE**.

1. Enter the **NAME** of the service, select a **REGION**, select the **STORAGE ACCOUNT** that you created previously to hold your media from the drop-down list, and then click the **CREATE MEDIA SERVICE** check mark.

	> **Note:** Make sure to select the same region for all the assets that you create in Windows Azure for this demo, typically the one closest to you.

	![Creating the Media Service](Images/create-media-service.png?raw=true)

	_Creating the Media Services account_

1. Select the newly created service, click **MANAGE KEYS** in the command bar, and then copy the value of the **MEDIA SERVICE ACCOUNT NAME** and **PRIMARY MEDIA SERVICE ACCESS KEY** settings.

	![Media Service Access Keys](Images/media-service-access-keys.png?raw=true)

	_Media Service Access Keys_

1. In the **appSettings** section of the **Config.Local.xml** file, locate the **mediaServicesAccountName** and **mediaServicesAccountKey** settings and replace the placeholders with the corresponding values obtained in the previous step.

<a name="setup4" />
**Downloading the Publish Settings File for the Subscription**

1. Go to [https://windows.azure.com/download/publishprofile.aspx]() to download the publish settings file for your subscription. Save the file to your **Downloads** folder. You will need this file during the demo, as well as for the setup scripts.

1. In the **Config.Azure.xml** file, perform the following steps:
	- Locate the **publishSettingsFilePath** setting and replace the placeholders with the file path where the publish settings file is located.
	- Locate the **subscriptionName** setting and replace the placeholders with the name of your Azure subscription.

<a name="setup5" />
**Configuring the Windows Azure Web Site**

1. In the **websiteName** setting of the **Config.Azure.xml** file, replace the placeholders with the name of the Windows Azure Web Site you will create in [Building and Extending Web Apps to Windows 8](#segment1).

	> **Note:** This information is required by the cleanup scripts to remove the underlying SQL Azure database used by the web site.


<a name="setup6" />
**Configuring Identity Providers**

The application used in this demo allows users to log in using one of several configured identity providers. To configure them:

1. Choose one or more identity providers from the list below and follow the steps to register your app with that provider. Remember to make a note of the client identity and secret values generated by a provider. 
	- [Facebook] [1]
	- [Twitter] [2]

[1]: https://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-facebook-authentication/
[2]: https://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-twitter-authentication/

	Note that you need to create at least two entries in each provider, one for running the application locally using [http://127.0.0.1:81]() as the return (or callback) URL and the other for the URL of the site when deployed to Windows Azure Web Sites (e.g. [http://{YOUR-SITE-NAME}.azurewebsites.net/]()). 

	>**Important:** Make sure that the URL for the Windows Azure Web Sites scenario that you specify to the identity provider is available when you deploy the site during the demo by choosing a site name that is unlikely to be in use. Alternatively, you may create the site in advance to reserve its name and, during the demo, simply walk through the process without creating the site. 
	
	In addition, an (optional) segment in this demo requires you to deploy the application as a cloud service. If you intend to complete this segment, you also need to configure a third entry for the cloud service's URL (e.g. [http://{YOUR-CLOUD-SERVICE-NAME}.cloudapp.net]()). Use the cloud service name that you created earlier, as described in [Creating a Cloud Service](#setup4).

	> **Note:** Currently, the Windows 8 application only supports authentication using **Facebook** or **Twitter** accounts

1. In the **Config.Local.xml** file, enter the information generated by the identity provider for each configured deployment scenario, local, web site, and cloud service. In each of the subsections of the **appSettings** section, **local**, **website**, and **cloudService**, enter the corresponding application ID (or consumer key) and application secret (or consumer secret) pairs returned by the identity provider. Also, in the **appSettings** section locate the **website** subsection and replace the placeholder in the **apiBaseUrl** setting with the name of the site. 

1. Save the **Config.Local.xml** file.

<a name="setup7" />
**Running the Setup Scripts**

Once you have completed the previous tasks and updated the **Config.Local.xml** and the **Config.Azure.xml** files with the necessary settings, you may now run the setup scripts that will first copy the solutions to the working folder and then configure them.

1. Ensure that you have saved any changes to the **Config.Local.xml** and **Config.Azure.xml** files.

1. Run the **Setup.cmd** script that you will find in the **source** folder of the demo's install location.

<a name="setup8" />
**Deploying the Application as a Cloud Service and Configuring New Relic (optional)**

The following procedure sets up the deployment used for the final and optional segment of this demo, [Deploying and Managing Windows Azure Apps](#segment5). It shows how to acquire the New Relic (free) add-on from the Windows Azure Store, configure it for the solution, and deploy the application to a cloud service.

1. In the **Windows Azure Management Portal**, click **New** and then **Store**.

1. Select the **New Relic** from the list of available add-ons.

	![Adding New Relic Add-On](Images/adding-new-relic-add-on.png?raw=true "Adding New Relic Add-On")

	_Adding New Relic Add-On_

1. Select the **Standard (FREE)** plan, enter a **NAME** for the add-on, and then click **Next**.
	![Personalize-new-relic-add-on](Images/personalize-new-relic-add-on.png?raw=true)

	_Personalize New Relic Add-On_

1. Once created, select the add-on and click **Connection Info** in the command bar. Copy the value of the **License Key** to the clipboard.

	![New Relic Connection Info](Images/NewRelic-ConnectionInfo.png?raw=true "New Relic Connection Info")

	![Azure License Key](Images/NewRelic-License-Key-Azure.png?raw=true "Azure License Key")

	_Azure license key_

1. In Visual Studio, open the **BuildClips.sln** stored in the working directory, inside the **BuildClips.Web\BuildClipsDeploy** folder.

1. Open the **Package Manager Console**, select **BuildClips** in the **Default Project** drop-down list, and install the **NewRelicWindowsAzure** NuGet package using the following command:

		Install-Package NewRelicWindowsAzure

	![Adding New Relic dependencies](Images/NewRelicWindowsAzure-package.png?raw=true "Adding New Relic dependencies")

	_Adding New Relic dependencies_

1. When prompted, enter the obtained **License Key** and the **Name** chosen when creating the add-on.

	![Setting New Relic License Key](Images/NewRelic-License-Key.png?raw=true "Setting New Relic License Key")

	_Setting New Relic license key_

1. Open the **_Layout.cshtml** file in the **Views\Shared** folder and add the following lines to enable Browser-side metrics on New Relic dashboard.

	<!-- mark:6,11 -->
````HTML
<html lang="en">
	<head>
		...
		@Scripts.Render("~/bundles/modernizr")
		@Scripts.Render("~/bundles/jquery", "~/bundles/jqueryui")
		@Html.Raw(NewRelic.Api.Agent.NewRelic.GetBrowserTimingHeader())
	</head>
	<body>
		...
		@RenderSection("scripts", required: false)
        @Html.Raw(NewRelic.Api.Agent.NewRelic.GetBrowserTimingFooter())
    </body>
</html>
````
1. Repeat the previous step with the **_Layout.Login.cshtml** file in the **Views\Shared** folder.

	<!-- mark:5,10 -->
````HTML
<html lang="en">
	<head>
		...
		@Scripts.Render("~/bundles/modernizr")
		@Html.Raw(NewRelic.Api.Agent.NewRelic.GetBrowserTimingHeader())
	</head>
	<body>
		...
		@RenderSection("scripts", required: false)
        @Html.Raw(NewRelic.Api.Agent.NewRelic.GetBrowserTimingFooter())
    </body>
</html>
````

1. Close **Visual Studio**.

1. In the **Config.Azure.xml** file, perform the following steps:
	- Locate the **cloudService** section and replace the placeholder in the **name** setting with the name of the cloud service you want to create. Additionally, specify the region in the **location** setting. 
	- Locate the **sqlDatabase** section and replace the placeholders for the **name**, **username** and **password** settings with the name of the database you want to create, and the admin user and password for the SQL database server.  Additionally, specify the region in the **location** setting.
	- In the **storageAccounts**, locate the **diagnosticsStorageAccount** subsection and replace the placeholder for the **name** setting with the name of the storage account you want to create. Additionally, specify the region in the **location** setting.

	> **Note:** The setup script executed in the following steps will automatically create the assets specified above. Make sure to select the same region for all the assets that you create in Windows Azure for this demo, typically the one closest to you.

1. Save the **Config.Azure.xml** file.

1. Run the **Setup.Deployment.cmd** script that you will find in the **source** folder of the demo's install location. This script will automatically create the cloud service, the Storage account for diagnostics and the Azure SQL database. Finally, it will configure the necessary connection strings in the solution, and deploy the cloud service.

1. Wait for the setup to complete.

1. Finally, go to the deployed application's URL and sign in.

	> Note: You may sign in using one of the registered identity providers. Alternatively, you can register as a local user by going to /Account/Register.

1. In the home page, select **Upload** in the navigation bar, and then upload a few sample videos from the **[installdir]\setup\assets\videos** directory. This will exercise the application and generate metrics that can be displayed by New Relic.

<a name="Demo" />
## Demo ##
This demo is composed of the following segments:

1. [Building and Extending Web Apps to Windows 8](#segment1)
1. [Windows Azure Media Services](#segment2)
1. [Building N-Tier Cloud Services with Real-Time Communications](#segment3)
1. [Scaling with Windows Azure Caching](#segment4)
1. [Deploying and Managing Windows Azure apps](#segment5)

<a name="segment1" />
### Segment 1: Building and Extending Web Apps to Windows 8 ###

In this segment, we will extend the web site to include Web APIs that power a Windows 8 experience.  Finally, the web site project will be deployed to Windows Azure Web Sites and scaled using multiple paid shared instances.

1.	Launch Visual Studio 2012 as **administrator** and open the **BuildClips.sln** solution in **[working directory]\BuildClips\BuildClips.Web**. Press **F5** to run the Web application locally.

	> **Speaking Point**: In this demo, we are taking an __existing__ website and __extending__ it to Windows 8 using WebAPIs. This application uses ASP.NET 4.5 and the new WebAPIs.
	>
	> I've got my app right here. Let's go and launch this app
	>
	> **Note:** Make sure the port where the application will run is number 81, otherwise you will get an error when using Facebook login service.
	>
	>To check this, open Web project's properties and verify that **Use Local IIS Web Server** is selected and it has this value: _http://127.0.0.1:81/_

1. Log in to the application using Facebook. Provide your credentials.

	> **Speaking Point**: You can see at the top that not only do I have the ability to log in via regular ASP.NET membership, but I've got a number of additional social identity providers like a Microsoft account, Twitter or Facebook. 
	>
	> I'm going to log in via Facebook, and this is all built into the ASP.NET 4.5 API.

	![Log On using Facebook Service](Images/log-on-using-facebook-service.png?raw=true "Log On using Facebook Service")

	_Log On using Facebook_

1. If the application consent page is shown, click **Go to App**.

1. Once you are authenticated, in the application Register page click **Register** to associate your Facebook account with the Web application.

	> **Speaking Point**: Now, I'm going to log into my application here, and you'll notice it says "associate your Facebook account." I didn't actually have to create a local membership account. I hit "register" and it notices that it is, in fact, me. And it knows my name, it knows my information. I can associate multiple external accounts.

	![Associating Facebook Account](Images/associating-facebook-account.png?raw=true "Associating Facebook Account")

	_Associating Facebook Account_

1. Close the browser.

1.	Talk about Page Inspector. Open **/Views/Account/Login.cshtml** and type **Ctrl+K, Ctrl+G** to open Page Inspector. 

	> **Speaking Point**: With Page Inspector you can now see what your Razor markup will look like in the browser and make real-time edits.

	![page-inspector](Images/page-inspector.png?raw=true)

	_Page Inspector_

1.	Talk about Facebook Authentication with DotNetOpenAuth. Open **App_Start/AuthConfig.cs**

	> **Speaking Point**: We can now easily use DotNetOpenAuth for handling authentication with a variety of identity providers. In this application we are using Facebook for authentication. You can see how easy it is to set up Facebook authenticaiton in just a few lines of code. Similarly you could set up Google, Microsoft, and Twitter authentication.
	>
	> In addition to supporting Facebook authentication, we also have added a Facebook Application template in the ASP.NET Updates that makes it easy to build a Facebook App using the Facebook C# SDK.

1. Mention that we are using SQL Database for the video metadata.

1.	Discuss the use of Entity Framework 6. Under the **BuildClips.Services** Project, open **Models/VideosContext.cs** and **VideoService.cs**.

	> **Speaking Point**: After I log in, I can upload a video and the metadata for the video is stored in a SQL database. For that metadata we're using Entity Framework 6 Code First, and I'm going to take a look at the video context here.
	>
	> Note that with Code First the database and the schema are automatically generated. Additionally, using automatic migrations makes it easy to iterate your data access code and database schema.
	>
	> We are using async and await to store this. This is really important because I know this application is going to be really popular. I'm going to make sure that I do all of my database IO in a non-blocking way. I make my new video, I add it, and then I use async in a way to save those changes asynchronously. That releases the ASP.NET thread back into the pool and then lets that IO happen in a non-blocking way.

1.	Discuss how we use Blob Storage to store the videos. Under the **BuildClips.Services** Project, open **VideoStorage.cs**. Show how easy it is to upload data to the blob. Show the Upload method and discuss how blob storage is scalable.

	> **Speaking Point**: Then after that, we can even upload the video to Azure Blob Storage, also using async and await.

1. Show the new Storage Explorer in Visual Studio. 

	> **Speaking Point**:  Now, rather than running another application or going to the Azure portal, I can go into Server Explorer now and with the new Azure tools, I can see my blobs, tables, queries, all from inside Visual Studio, including my development blob and the blob in the cloud. I can even right-click on those, view the blob container. And I can see the videos that I've uploaded before.

	![storage-explorer](Images/storage-explorer.png?raw=true)

	_Storage Explorer_

	> **Note**: If you are using the Express edition of Visual Studio, you will find the Storage Explorer in the Database Explorer window.

1.	Back in Visual Studio, open **/Controllers/VideosController.cs**.

	> **Speaking Point**: Now, I want to be able to access these videos from an API, from a Web API. I'm going to take those videos and expose them out to a Windows 8 application. This is an existing ASP.NET app, but I can still add a Web API to it. I can use Web API inside of Web forms or MVC, it's all one ASP.NET.
	>
	> In this example I've got a get and a post, and you'll notice on this post that I'm not taking the video as a parameter, I'm not using model binding in this instance because I could be uploading potentially many gigabytes of videos. I want to use an asynchronous model here as well. I can bring in many, many gigabytes of video without tying up that thread, again, allowing me to scale.

1. Implement **Get** method for retrieving all the existing videos. Code snippet shortcut is **VideosControllercsGet**.

	(Code Snippet - _VideosController.cs - Get_)
	<!-- mark:1-5 -->
	````C#
	// GET /api/videos
	public IQueryable<Video> Get()
	{
		return this.service.GetAll();
	}
	````

	> **Speaking Point**: So here I'm calling read as multipart async on the post. I've currently got a get that lets me get one video, but I want to be able to get multiple videos. So here is a simple get method for my ASP.NET Web API controller. This is going to return those videos as an IQueryable, so if someone visits slash API slash videos, it's going to let me get those.

1.	Next, add the **[Queryable]** attribute to the **Get()** method as shown below.

	<!-- mark:2 -->
	````C#
	// GET /api/videos
	[Queryable]
	public IQueryable<Video> Get()
	{
		return this.service.GetAll();
	}
	````

	> **Speaking Point**: But I might want to be able to query them a little bit more from the query string. Simply returning IQueryable and adding the [Queryable] attribute is all you need to enable OData queries on your REST service.

1. Expand the **Areas** folder and show the **HelpPage** area. This area contains the Views to render the Web API Help Pages.

	> **Speaking Point**: Just like SOAP applications had WSDL to help you understand those Web services, Web APIs and RESTful APIs have online documentation that I don't want to have to write myself.

	![WebAPI HelpPage Area](Images/webapi-helppage-area.png?raw=true)

	_WebAPI Help Page Area_

1.	Press **F5** again to run the application and use the following address to navigate the Web API  Help Page:   [http://127.0.0.1:81/help](http://127.0.0.1:81/help).
	
	> **Speaking Point**: So I'm going to type in "slash help" in the URL there, and we're going to auto-generate documentation for the Web API, including the new API that I just added. If I click on that, I can even see in a sample response format that shows me whatever the JSON would look like when that gets returned.

	![WebAPI Help Page](Images/webapi-help-page.png?raw=true "WebAPI Help Page")

	_WebAPI Help Page Running_

1. Close the browser.

1.	Open the **Windows Azure Management** portal.

	> **Speaking Point**: So I've got this application set up. And now I'm going to go into the Azure portal and I can make a new website.

1.	Create a new Web Site with database, using the **Create with Database** option.

	![portal create with database](Images/portal-create-with-database.png?raw=true "WebAPI Help Page")

	_Creating a new Web Site with a database from the portal_

1. In the **Create Web site** page, set the website name by typing, for example, **_buildclipsdemo_** in the **URL** and select to create a new SQL database.

	![portal create web site](Images/portal-create-web-site.png?raw=true)

	_Creating the Web Site_

1. In the **Specify Database settings** page, choose to create a **New SQL Database server**, providing an administrator **username** and **password**.

	![portal database settings](Images/portal-database-settings.png?raw=true)

	_Creating the database_

1.	Navigate to your new web site in the **Windows Azure Management** portal and download the publishing profile.

	> **Speaking Point**: I'm going to download the publish profile and the publish profile is going to give me all the information that I could potentially need to publish this.

	![portal downloadprofile](Images/portal-downloadprofile.png?raw=true)

	_Downloading the publish profile_

1. Go back to the Web application solution within **Visual Studio**.

1.	Right-click the **BuildClips** project and select **Publish**. 

	![Clicking on the Publish menu](Images/clicking-on-the-publish-menu.png?raw=true)

	_Right-click the **BuildClips** project and select **Publish**_

1. Import the downloaded publish settings file.

	> **Speaking Point**: I'm going to go and import that publish setting. So when I hit import, it's going to update this dialog with all the information that I need, all the passwords, all the user names, every technical detail that's required. Including that entity framework connection string.

	![publish web](Images/publish-web.png?raw=true)

	_Publishing the web site_

1. In the **Connection** page, click **Next**.

	![publish connection](Images/publish-connection.png?raw=true)
	
	_Configuring the publish method_

1. In the **Settings** page, click **Next**.

	![publish settings](Images/publish-settings.png?raw=true)
	
	_Configuring the database connection string for the web site_

1. In the **Preview** page, click **Publish**.

	> **Speaking Point**: So I'm going to hit publish on that. It's going to build and start publishing. 

	![publish process](Images/publish-process.png?raw=true)

	_Publish process_

1.	Once the publishing process finishes, go to the **Scale** tab in the portal and change the **Web Site Mode** to _Shared_. Change the number of instances to 3.

	> **Speaking Point**: Now that my application is deployed and because I know this is going to be popular I'm going to go over to scale. In scale here, I can go and select not just that I want a free website, even though I get ten free websites, but I could say shared and have multiple instances or reserved and have even four cores and three instances of that. So I've got a lot of flexibility in the way I can scale my website in Azure.

	![portal sharedmode](Images/portal-sharedmode.png?raw=true)

	_Changing the **Web Site Mode** and the number of instances_

1. Click **Save** in the bottom bar and select **Yes** to agree with the billing impact.

	![Billing impact message](Images/billing-impact-message.png?raw=true)

	_Billing impact message_

1. Go to the **Configure** tab in the portal and open the **Manage Domains**.

	> **Speaking Point**: Talk about how Web Sites support CNAMES and A Records for applications running in Shared and Reserved modes.

	![portal manage domains](Images/portal-managedomains.png?raw=true)

	_Manage domains using the Windows Azure portal_

1. Navigate to the recently deployed application and upload a video that will be now stored in the cloud. You can use any of the videos from the **[working dir]\Assets\videos** folder.

	> **Speaking Point:** So now, let's upload a new video to my recently deployed application. 

	![Upload a video](Images/upload-video.png?raw=true)

	_Upload a video_

1.	Open the **BuildClips.Win8App.sln** solution located at **[working dir]\BuildClips\BuildClips.Win8App** in a new instance of **Visual Studio**.

	> **Speaking Point:** So now I'm going to consume that from Windows 8. And since I've been doing a lot of work in JavaScript and curly braces lately, I wrote the entire application in JavaScript. So this is a native Win8 application in the store that is written in JavaScript.

1.	Show the code for calling the Web API. In the **webapi.js** file, highlight **getVideos()** method. Additionally, in the **data.js** file, highlight the **GetVideosOnSuccess()** method.
	
	<!-- mark:3-4 -->
	````JavaScript
   WinJS.Namespace.define("WebApi", {
      getVideos: function getVideos(onSuccess, onError) {
         WinJS.xhr({ url: url, responseType: "json" })
            .done(onSuccess, onError);
      },
      ...
   });
   ````

	<!-- mark:4,8-11 -->
	````JavaScript
   function GetVideosOnSuccess(result) {
        setProfilingTime('GetAll', new Date().getTime() - timeStamp);

        var videos = JSON.parse(result.responseText);

        list.length = 0;

        videos.forEach(function (video, i) {
            list.push(getListItem(video));
        });

        Data.itemsRetrieved = true;
    }
   ````

	> **Speaking Point:** I'm going to look at my WebAPI.js. And here I'm going to be calling the WinJS.XHR helper function, and I'm just going to go and call that Web API that I created, return all of those videos, and then bind them to a list inside of my store application.

1.	Press **F5** to run the Windows 8 Application.

1. Login with your **Facebook** or **Twitter** account.

	![Log into the Windows 8 App using Facebook Service](Images/win8-log-on-using-facebook-service.png?raw=true "Log into the Windows 8 App using Facebook Service")
	
	_Log into the Windows 8 App using Facebook Service_

1. If you chose to authenticate using Facebook and the _Application Consent_ page is shown, click **Allow**. If you have chosen to authenticate using Twitter, and the _Authorize Application_ page is shown, click **Authorize App** to continue.

1. Click on the video to show that it is consuming the published Web API.

	![Select the previously uploaded video](Images/win8-select-video.png?raw=true "Select the previously uploaded video")
	
	_Select the previously uploaded video_

1. Then reproduce the uploaded video to validate everything is working as expected.

	![Video player showing the previously uploaded video](Images/win8-play-uploaded-video.png?raw=true "Video player showing the previously uploaded video")
	
	_Video player showing the previously uploaded video_


<a name="segment2" />
### Segment 2: Windows Azure Media Services ###

In this segment, you will configure Media Services in the Windows Azure Management portal and then you will update the application to submit an encoding job to Media Services when a video is uploaded.

> **Important:**  Before proceeding with this segment you will either have to:

> - Use the begin solutions of segment #2, located in **[working directory]\BuildClipsMedia**, instead of continuing with the end solutions from segment #1.

> or

> - Continue with the solution from the previous segment after removing the **Microsoft.AspNet.WebApi.OData** NuGet package from the **BuildClips** project and the **[Queryable]** attribute from the **Get()** method in the **VideosController.cs** file. 

> The **Microsoft.AspNet.WebApi.OData** (required by segment #1) and **Microsoft.WindowsAzure.MediaServices** (required by this segment) are both pre-release software and are currently not compatible with each other.

--

> **Speaking point:** What I'm going to do now is scale that app even further. The first way we're going to do that is by integrating another service called Windows Azure Media Services. And what Media Services allows you to do is very easily ingest, encode or transcode video, and then set up a streaming end point that you can use in order to stream your video to the cloud. Now, instead of storing it directly to storage, we're going to fire it off to Media Services, which will then encode it for us automatically, and then we're going to stand up a media streaming endpoint, which is going to allow our clients to be able to go ahead and stream it from a scalable back end.
And the beauty about Windows Azure Media Services is that it exposes a REST API that makes it really easy for you as developers to integrate within your applications, and it takes care of all the infrastructure necessary for us. So we don't have to spin up VMs, we don't have to manage our own encoders or streaming servers. Instead, Windows Azure Media Services does all that for us.

1. Start in the Windows Azure Management Portal, in the **All Items** node.

	> **Speaking Point:**
	> So let's go ahead and look at some code and see how we do it. So you can see here this is the Windows Azure portal. Earlier, you saw how we could create a website.

	![](Images/portal-all-items.png?raw=true)

1. Show how to create a new Media Service by navigating to **New | App Services | Media Service**.  Enter the name of a new media service and select your pre-created storage account. **Do not click create**, just a talking point.

	> **Speaking Point:**
	> The cool thing here is I'm going to now just take advantage and click on this app services category, and I can easily create a media service. So this is a service I can use from within my application. And I can give it a name. I can choose where in the world I want to run it, and I just go ahead and create. And in about 45 seconds, I'll then have a media service of my own that I can then program against and use for all my media scenarios.

	![Quick Create Media Service](Images/quick-create-media-service.png?raw=true "Quick Create Media Service")
 
1. In the media services list on the left, select your  pre-created media service. Show the dashboard. Click the **Content** link to show the list of jobs.

	> **Speaking Point:**
	> Now, I happen to have one already created with some content that we uploaded earlier. So I'm going to click on it here, and you can see us drill in here. So, for example, I have a dashboard view here because I can program it with REST, I can download our SDK and then I just use our little manage keys button down here in order to get my developer key, and then I can just start coding against the service. Also, I can just go ahead and click this content link at the top, I can see I have a couple of jobs and videos already uploaded here.

	![Show Media service contents](Images/show-mediaservice-content.png?raw=true "Show Media service contents")

1. Click **Upload** in the command bar and browse to **[working directory]\Assets\videos** folder. Select a video and click Ok to start uploading the new video.

	> **Speaking Point:**
	> With the new Windows Azure Management Portal, I can simply click on Upload on the command bar and browse to a video file that I have on my local machine. And this is now going to upload the video directly from my dev machine up in terms of the Windows Azure Media Service account. And once it's uploaded, then, I can go ahead and do various jobs on it.

	![Upload video from portal](Images/upload-video-from-portal.png?raw=true "Upload Video from portal")

1. Select the video from the content list and click **Encode**. Highlight the options in the **Preset** list-box. Show that the encoding job begins.

	> **Speaking Point:**
	> Once the file is uploaded, I can then easily encode the file into multiple formats and support multiple devices. You can target Silverlight and Flash, you can target HTML5, you can even target iOS. I'll start to encode this video file using a preset encoding profile that will target playback in HTML5 browsers including IE, Chrome, and Safari.   

	![Encode video from portal](Images/encode-video-from-portal.png?raw=true "Encode video from portal")

	> **Speaking Point:**
	> So Windows Azure Media Services will automatically take that job, put it on dedicated machines that we run, and spin up and do that encoding job for you. Here you can see that my encoding job has started.  It will take a few minutes for this short video to finish encoding. In addition to uploading content and submitting encoding jobs from the new Windows Azure Management portal, I can also submit jobs to Windows Azure Media Services programmatically.

	![Encoding job starting](Images/encoding-job-starting.png?raw=true "Encoding job starting")

1. Select a previous video that is already encoded (ones starting with _JobOutputAsset_ prefix and currently not published). Click **Publish** and confirm to publish the video.

	> **Speaking Point:**
	> Once something is encoded, I can then choose to publish it. This is going to create a unique streaming end-point URL that I can use for this particular asset. And then I can go ahead and embed that within my application and start playing it directly. And then there's a media server that we're managing with Media Services that's doing all the back-end streaming for you. Since encoding will take a few minutes, here you can see a video that I encoded earlier. Once encoding is complete, I'll publish the video out to Windows Azure Storage. The publishing process makes the video available so it can be consumed from multiple applications and experiences.

	![Confirm Publish](Images/confirm-publish.png?raw=true)

1. Click the **Play** button to watch the video streaming.

	> **Speaking Point:**
	> And then the cool thing is when you actually want to play something, you can just go ahead, click on a video, click play, and even directly within the browser here, you can go ahead and test out your video, including with adaptive streaming. So very easy way you can test out and kind of learn how to use the product and quickly see the status of different jobs that you're working on.

	> **Note:**
	> Even when the publish operation seems to be done, it might take a few more seconds for the video to be really ready - 30-45 secs.  

	![Playing smooth streaming video](Images/playing-smooth-streaming-video.png?raw=true)

1. Switch to the Visual Studio 2012 instance that has the **BuildClips** Web Project already opened.

	> **Speaking Point:** 
	> But the cool thing about Media Services, again, isn't that you can do this all manually, it's the fact that you can actually code against it and just send REST calls and do all this from within your apps. Let's go ahead and do that. So I'm going to flip into Visual Studio. And what you're seeing here is that same project we were working on just a few minutes ago. So that same ASP.NET project.

1. Install the Media Services NuGet. To do this, right click the **BuildClips** solution  and select **Manage NuGet packages for Solution**. Search for **Windows Azure Media Services SDK** package and click **Install**. Make sure both projects are selected and click **Ok**. Click **I Accept**. Once the package installation is completed, click **Close**.

	> **Speaking Point:**
	> Now, I'm going to go ahead and click on manage NuGet and I'm just going to use the NuGet package manager in .NET in order to install a little SDK library that is going to provide a nice .NET-managed object model on top of those REST end points. And so this is just installing it within my solution, makes it a little bit easier for me to code against.   

	![Install the Media Services NuGet for solution](Images/install-package-mediaservices-solution.png?raw=true)

1. Right click the **Helpers** folder and select **Add Existing item**. Select the **MediaServicesHelper.cs** file located at **[working dir]\Assets\Segment2\Helpers** and click **Add**.

	> **Speaking Point:** So I'm going to add now a pre-cooked helper class that will provide some minor extensions to the Media Services SDK library, which are intended to help us on interfacing with our video service.

1. Open **VideoService.cs** and scroll down to the **CreateVideoAsync** method. Replace the **TODO** comment and the next four lines with the code below.
	
	(Code Snippet - _VideoService.cs - Create_)
	<!-- mark:1-13 -->
	````C#
	// Create an instance of the CloudMediaContext
	var mediaContext = new CloudMediaContext(
									 CloudConfigurationManager.GetSetting("MediaServicesAccountName"), 
									 CloudConfigurationManager.GetSetting("MediaServicesAccountKey"));

	// Create the Media Services asset from the uploaded video
	var asset = mediaContext.CreateAssetFromStream(name, title, type, dataStream);

	// Get the Media Services asset URL
	var videoUrl = mediaContext.GetAssetVideoUrl(asset);

	// Launch the smooth streaming encoding job and store its ID
	var jobId = mediaContext.ConvertAssetToSmoothStreaming(asset, true);
	````

	> **Speaking Point:** There we go. And then I'm just going to update some of the code I showed you earlier so that instead of storing that media inside a storage account, we're instead going to pass it off to Media Services to both store and code and publish. And doing that is really easy. So what I'm going to do here is just replace these four lines of code. This code right here. And what all this code is doing is connecting to my Windows Azure Media Services account. It's going to create a new asset in Media Services from the stream that was uploaded, get back a URL for it, and then kick off an encoding job to convert it to smooth streaming. Just a couple lines of code I can do that.

1. Place the cursor over the **CloudMediaContext** type and press **CTRL+.** to add the using statement. Do the same with **CloudConfigurationManager** on the next line.

1. Next, in the **Publish** method, replace the **TODO** comment and the next three lines of code with the following snippet.

	> **Speaking Point:** 
	> And then when I want to publish it so that people can stream it, I can just go ahead and call video services publisher, and this is then going to just call and publish that video on the job object that was encoded and get me back a URL that I can now pass off to my clients to play.

	(Code Snippet - _VideoService.cs - Publish_)
	<!-- mark:1-15 -->
	````C#
    var mediaContext = new CloudMediaContext(
                                     CloudConfigurationManager.GetSetting("MediaServicesAccountName"),
                                     CloudConfigurationManager.GetSetting("MediaServicesAccountKey"));

    string encodedVideoUrl, thumbnailUrl;
    if (mediaContext.PublishJobAsset(video.JobId, out encodedVideoUrl, out thumbnailUrl))
    {
        video.EncodedVideoUrl = encodedVideoUrl;
        video.ThumbnailUrl = thumbnailUrl;
        video.JobId = null;

        this.context.SaveChanges();
	}

	return video;
	````
1. Finally, insert a new method named **GetActiveJobs** into the class.

	> **Speaking Point:** 
	> Finally, let me add a new method to the video service that we'll be using shortly to retrieve the list of encoding jobs that are in progress and completed. This will allow me, for example, to run a background process to check the active jobs, report the status of the jobs in progress and also publish the videos already encoded.

	(Code Snippet - _VideoService.cs - GetActiveJobs_)
	<!-- mark:1-26 -->
	````C#
    public IEnumerable<Video> GetActiveJobs()
    {
        var activeJobs = this.context.Videos.Where(v => !string.IsNullOrEmpty(v.JobId));

        if (activeJobs.Any())
        {
            var mediaContext = new CloudMediaContext(
                                             CloudConfigurationManager.GetSetting("MediaServicesAccountName"),
                                             CloudConfigurationManager.GetSetting("MediaServicesAccountKey"));

            foreach (var video in activeJobs)
            {
                var job = mediaContext.GetJob(video.JobId);
                if (job != null)
                {
                    // The video status will be Encoding unless the encoding job is finished or error
                    video.JobStatus = (job.State == JobState.Finished || job.State == JobState.Error)
                                        ? JobStatus.Completed : JobStatus.Encoding;

                    yield return video;
                }
            }
        }

        yield break;
    }
	````

1. Right-click the **BuildClips** project and select **Publish**. 

	> **Note:** Please note that if you chose to open the begin solution for segment #2 you will need to import again the Web Site publish settings file.

	> **Speaking Point:** Now, that was basically all the code we needed to do. I can then right-click and publish this back to the cloud. One of the things that we support with Windows Azure websites is a very nice incremental publishing story.

	![Publish web site](Images/publish-web-site.png?raw=true)

1. Click the **Settings** page, expand **File Publish Options** and check the **Remove additional files at destination** option. Then click **Publish**.

	> **Speaking Point:** 
	> So before we start publishing, I'm going to configure the publishing to remove all files that won't be required anymore. And so you can see here, instead of having to redeploy the entire app, it's just showing me the differences between what's on my local dev machine and then what's in the cloud, so it makes deployment a lot faster. Those changes are now already live. 

	![Publish web site](Images/publish-web-settings.png?raw=true)

1. Once the app is deployed, switch the focused window to Internet Explorer to show the live running instance of the app and navigate to **http://{yourdeployedwebapp}.azurewebsites.net/help** in order to show the Web Api Help Page.

	> **Speaking Point:** 
	> So remember we have those Web APIs that we exposed earlier. We can also, then, hit this with a Windows 8 client.

	![](Images/webapi-help-page.png?raw=true)

1. Switch to the Windows 8 App.

	> **Speaking Point:** 
	> So let's go ahead and switch to the Windows 8 app. 

1. In the video list page, right-click on the screen and click the **Upload** button.

	![Upload Video Button](Images/windows8-upload-button.png?raw=true)

	> **Speaking Point:**
	> I'll upload a video that I have on my local machine.

1. Enter a title, description and select some tags, and then upload a video from the **[working dir]\Assets\videos** folder using the file picker.

	![Uploading a Video](Images/windows8-upload-video.png?raw=true)

	> **Speaking Point:**
	> Let me capture a title and description for my video, and then I'll press upload to pick the video. We're going to hit open, and this is now uploading this off to Windows Azure. We're going to use the exact same REST URL we published earlier with our Web APIs in the middle tier. And this is going to talk to that Web service on the back end, it's going to then fire off a REST call to Windows Azure Media Services and it's going to kick off an encoding job, which is then going to start encoding it. And then once it's done, and you can see it just completed, it will be able to be played through our streaming server in the cloud.

1. Switch back to the **Windows Azure Management Portal** and navigate to your Media Service account.

	> **Speaking Point:**
	> Now that my video is uploaded, let's switch back to the Windows Azure Management Portal.  

1. Go to the **Content** section of the media service and show the video uploaded and the running encoding job.

	> **Speaking Point:**
	> I'm going to go back into the portal, click the content tab, and you can see that my video has been, and hey, we've got two new files that have just shown up. One is the file we uploaded, and one is an adaptive streaming job that we just kicked off. And if I go ahead and hit play, what I should be able to do inside the portal is see all of you. 

	![Encoding the video](Images/encoding-the-video.png?raw=true)


<a name="segment3" />
### Segment 3: Building N-Tier Cloud Services with Real-Time Communications ###

In this segment, you will evolve the Video web project into an n-tier solution that includes a background service for monitoring the status of encoding jobs.  We will also enable real-time updates to users for both the web and Windows 8 apps using SignalR, Web Sockets in IIS8, and the Windows Azure Service Bus.

> **Speaking Point:** Uploading a nice seven-second clip of video is good for some scenarios. But for a lot of media scenarios, you might be uploading, you know, hundreds of megabytes of video content. And it's going to take minutes or hours even to encode all of that in all the formats within the cloud. And what we're going to want to be able to do for our client experience to make it a lot richer is be able to provide real-time feedback to the users as to the status of their different encoding jobs. And to do that, we're going to take advantage of a cool library called SignalR. And what SignalR does is to maintain a continuous connection with your clients, and we can use this now to be able to provide continuous feedback from the server to the client in a very efficient, scalable way. And SignalR will scale even to hundreds of thousands or millions of client connections. Now, to make this thing even more scalable, I'm going to also, then, introduce another tier into my application. This is going to be a background service. It's going to be a non-UI service. And what it's going to do is it's going to monitor our Media Services accounts. It's going to look at all the encoding jobs that are going on within it, and it's then just going to feed messages to my Web app with SignalR, which will then broadcast it to the client. And then my UI on the client can just provide a nice, continuous update UI feedback to my users.

1. Check if IIS Express is running in the System Tray. If this is the case, right-click the IIS Express icon and select Exit (confirm to stop all the working processes).

	> **Note:** This is required to stop all running sites and prevent the Windows Azure emulator from deploying the web roles created during this segment in an unexpected port.

1. Switch to the **Visual Studio 2012** instance with the **BuildClips** Web Site project opened.

    > **Speaking Point:** Let's go back to the Web Application. Now, the first step we're going to do is we're going to convert this from being a single-tier website to, instead, being a multitier what we call cloud service inside Windows Azure. So a cloud service can have multiple tiers that run on multiple machines and kind of provide a nice way to compose them together.

1. Right click on the **BuildClips** Web project and select **Add Windows Azure Cloud Service Project**.

    > **Speaking Point:** And converting a website to be a cloud service is really easy. All I need to do is just right-click on the website and say add Windows Azure cloud service project. This is now going to add into my Visual Studio project a Windows Azure cloud service that kind of defines the overall shape and structure of my cloud service app.

	![Add windows azure cloud service project](Images/add-windows-azure-cloud-service-project.png?raw=true)

	_Add windows azure cloud service project_

1. Expand the **BuildClips.Azure** cloud service project, right-click the **BuildClips** role and select **Properties**. In the **Endpoints** tab, change the value of the Public Port column to **81**.

	> **Speaking Point:** And you'll notice it's automatically added a role, think of it like a tier, that points to my website that we built earlier. And I don't have to change any code within the website in this particular scenario. By default, our new role will be listening to port 80. Let me just change it to listen to the same port where our original website was listening.

	![Web role properties option](Images/web-role-properties-option.png?raw=true)

	_Web role properties option_

	![Web role endpoint port option](Images/change-webrole-enpoint-port.png?raw=true)

	_Web role endpoint port_

1. Right click on the **BuildClips.Azure** cloud service project and select **New Worker Role Project** to create a worker role named **BackgroundService**.

    > **Speaking Point:** We can also add additional tiers or roles to our cloud service. Let's add a Worker Role and name it BackgroundService. We will use this worker role to poll Windows Azure Media Services and check on the status of our encoding jobs.  

	![new worker role](Images/new-worker-role.png?raw=true)

	_New Worker Role_

	![Background Service Worker Role](Images/background-service-worker-role.png?raw=true)

	_Background Service Worker Role_

1. Right click on the **BackgroundService** project and select **Add Reference**. In the **Reference Manager** dialog, select the **Solutions** node and then the **BuildClips.Services** project. Click **OK**. 

    > **Speaking Point:** And you'll notice the default template just has a run method and just spins forever, sleeping and writing out trace lines. So what we're going to do, though, is we want this background worker to talk to our media service. And so to do that, we're going to add a reference to our class library that contains all of our media service references.

	![add BuildClips.Service reference](Images/add-myvideosservice-reference.png?raw=true)

	_Add reference_

	![add BuildClips.Service reference 2](Images/add-myvideosservice-reference-2.png?raw=true)

	_Select BuildClips.Service project_

1. Right-click the **BackgroundService** project, select **Manage NuGet Packages** and install the **Windows Azure Media Services .NET SDK** NuGet package.

    > **Speaking Point:** And I'm also going to install the same Media Services library we added previously to our web site, but this time to our new worker role.

	![Install Media Services Package](Images/install-package-mediaservices-project.png?raw=true)
	_Install Media Services Package_

1. Within the same dialog window, install the **Microsoft ASP.NET SignalR Client** NuGet package.

	> **Note:** To find the NuGet search the term **SignalR Client** and make sure you have selected the option **Include Prerelease**


	> **Speaking Point:** Because we're going to be firing messages with SignalR, I'm going to add in a reference to the ASP.NET SignalR client library to this project as well.

	![Install SignalR Client Package](Images/install-package-signalr-client.png?raw=true)

	_Install SignalR Client Package_

1. Replace the configuration file with a new one that has the Media Service connection string values already defined. To do this, right-click the **BackgroundService** project and select **Add | Existing Item**. In the **Add Existing Item** dialog window, select  the file type filter to **All Files (*)**, and then select the **app.config** file inside **[working directory]\Assets\Segment3\BackgroundService**. Finally, click **Add** and then confirm to replace the existing file.

	> **Speaking Point:** To complete all these  configurations in our worker, let me add to the project a pre-baked configuration file with our media service connection values already in place, so we don't need to spend any time doing this.

	![add app.config](Images/add-appconfig.png?raw=true)

	_Add the app.config file_

1. Open the **WorkerRole.cs** file, select the entire **Run** method and insert the following code snippet. Place the cursor over the **HubConnection** type and press **CTRL+.** to add the using statement. Do the same with **VideoService** and **JobStatus** on the lines below.

	> **Speaking Point:** There we go. And then all I'm going to do is just replace this run method here with the following code.

	(Code Snippet - _WorkerRole.cs - Run_)

	<!-- mark:1-26 -->
	````C#
  public override void Run()
  {
		// This is a sample worker implementation. Replace with your logic.
		Trace.WriteLine("BackgroundService entry point called", "Information");

		// Connect to SignalR
		var connection = new HubConnection(CloudConfigurationManager.GetSetting("ApiBaseUrl"));
		var proxy = connection.CreateHubProxy("Notifier");
		connection.Start().Wait();

		while (true)
		{
			 Thread.Sleep(5000);

			 var service = new VideoService();
			 Trace.WriteLine("Getting Media Services active jobs", "Information");
			 var activeJobs = service.GetActiveJobs();

			 foreach (var video in activeJobs.ToList())
			 {
                 proxy.Invoke(
                        "VideoUpdated", 
                        (video.JobStatus == JobStatus.Completed) ? service.Publish(video.Id) : video);
			 }
		}
  }
````

1. Highlight the lines that perform the connection to the SignalR Hub at the beginning of the Run method.

	> **Speaking Point:** And what this is doing is it is connecting first to SignalR and getting what's called a proxy to a hub, which I'll explain more in a little bit.

1. Highlight the main loop that performs the publishing and the call to the proxy.Invoke() method.
	
	> **Speaking Point:** And then it's just also going to do a loop repetitively, sleeping for 5 seconds, and then every five seconds it will wake up. It's going to connect to my video service, and when the job is done, it's then going to publish it and then continuously it's going to be sending updates to my SignalR hub by just invoking the video updated method. That's basically all I need to do in that project.

1. Create a **Hubs** folder in the **BuildClips** project. Right-click the new folder and select **Add |  New Item** and select the SignalR Hub Class template inside Web. Name the class **Notifier.cs** and click **Add**.

    > **Speaking Point:** In my middle tier, then, inside my ASP.NET app, all I need to do is define it as a hub, called notifier. To do this, we'll base on the SignalR Hub Item template that comes with the ASP.NET Fall 2012 Update, which takes care of installing the SignalR core libraries we need.

	![add notifier](Images/add-notifier.png?raw=true)

	_Add the notifier hub_

1. Open **Notifier.cs** file from the **Hubs** folder. Update the content of the class with the method from below. Then, place the cursor over the **Video** type and press **CTRL+.**).

	> **Speaking Point:** So let me do some small changes to this helper method. When this method gets called by the background service, so it's going to be firing messages to it, it's just broadcasting that message to any client that's listening on the hub, and this works with both browsers as well as devices like Windows 8.

	(Code Snippet - _Notifier.cs - VideoUpdated_)
	<!-- mark:3-6 -->
	````C#
	 public class Notifier : Hub
	 {
        public void VideoUpdated(Video video)
        {
            Clients.All.onVideoUpdate(video);
        }
	 }
	````

1. Open **Index.cshtml** in the **Views\Home** folder and insert the following highlighted code into the **Scripts** section.

	(Code Snippet - _Index.cshtml - SignalRNotifications_)
	<!-- mark:2-16 -->
	````C#
@section Scripts {
        <script src="@Url.Content("~/Scripts/jquery.signalR-1.0.0-alpha2.min.js")"></script>
        <script src="@Url.Content("~/signalr/hubs")" type="text/javascript"></script>
        <script>
            $(function () {
                var connection = $.hubConnection();
                var hub = connection.createHubProxy("Notifier");
                hub.on("onVideoUpdate", function (video) {
                    if (video.ThumbnailUrl) {
                        $("#video_" + video.Id).css("background", "url(" + video.ThumbnailUrl + ") no-repeat top left");
                    }
                });

                connection.start();
            });
        </script>

        <script>
            $(function () {
                ...
            });
        </script>
	````


	> **Speaking Point:** We'll update the view to listen for notifications from the SignalR hub and update the video item list whenever an encoding job is ready.  

	> **Note:** Make sure that the script reference to **jquery.signalR-1.0.0-alpha2.min.js** in the code from above matches the script file located inside the **Scripts** folder. The name of this script may change when SignalR is released.

1. Go to the **Windows Azure management portal**. Click **Service Bus** within the left pane. To create a service namespace, click **Create** on the bottom bar. 

	> **Speaking Point:** In order to deliver messages through SignalR at scale, we need to connect it to a backend messaging service.  Windows Azure Service Bus provides a secure and reliable messaging service that can power SignalR and these real-time experiences. Within the Windows Azure Management Portal, we can create a new Service Bus namespace.  

	![Creating a new Service Namespace](Images/service-bus-add-namespace.png?raw=true)

	_Creating a new Service Namespace_

1. Close the **Create a Namespace** Dialog.

1. Now show how we have another **Service Bus** namespace already created.

	> **Speaking Point:** I've already created a namespace named myvideos that we'll use for this demo. 

	![Service Bus in Azure Mgmt Portal](Images/service-bus-in-azure-mgmt-portal.png?raw=true)

1. Right-click the  **BuildClips** project and select **Manage NuGet Packages**. Install the **Microsoft.AspNet.SignalR.ServiceBus** NuGet package. Make sure you have selected the option **Include Prerelease**

    > **Speaking Point:** We will be using the Windows Azure Service Bus to enable delivery of the messages at scale, so let's install the SignalR Windows Azure Service Bus NuGet. 

	![Install Service Bus NuGet UI](Images/install-servicebus-nuget.png?raw=true)

	_Install Service Bus backplane for SignalR NuGet_

1. Select the **Updates** tab and update the **Microsoft.AspNet.SignalR** package to the latest version.

	> **Speaking Point:** Let me now update the SignalR NuGet packages installed by the **SignalR Hub Class** template. Because the ASP.NET Fall 2012 update is still in a preview stage, the hub item template is currently referencing an outdated version of the package.

	![Update SignalR Nugets](Images/update-signalr-nugets.png?raw=true)

	_Update SignalR Nugets_

1. Open the **Global.asax** file in the **BuildClips** project and **type** the following namespaces.

	<!-- mark:1-3-->
	````C#
	using Microsoft.AspNet.SignalR;
	using Microsoft.AspNet.SignalR.ServiceBus;
	````

1. Add the following code at the beginning of the *Application_Start* method to connect the web application with the service bus. Place the cursor over the **RoleEnvironment** and press **CTRL+.** to add the using statement.

	> **Speaking Point:** Now let's connect SignalR to the Service Bus.  When our web application starts, we need to establish a connection to the Service Bus.  We just need to provide SignalR with the Service Bus namespace and key.  

	(Code Snippet - _Global.asax - ServiceBusConfig_)
	<!-- mark:3-7-->
	````C#
    protected void Application_Start()
    {
		// SignalR backplane using the Windows Azure Service Bus
		GlobalHost.DependencyResolver.UseWindowsAzureServiceBus(
				  ConfigurationManager.AppSettings["ServiceBusConnectionString"],
				  topicCount: 5,
				  instanceCount: RoleEnvironment.CurrentRoleInstance.Role.Instances.Count);
		...
	}
	````

1. Press **F5** to start the Cloud Services solution in the compute emulator. 

    > **Speaking Point:** Finally, I can simply press F5 and start running my entire cloud service locally.  This will start the Windows Azure Compute Emulator, which provides a local environment that simulates a multi-tier environment in the cloud.  This is great for debugging and building apps before you deploy them.  

1. Right-click the emulator icon and select **Show Compute Emulator UI**. Show the tracing messages.

    > **Speaking Point:** Let me open the emulator UI so you can see it in action. Here you can see that our application has started.  BuildClips is our web role and BackgroundService is our worker role. 

	![Compute Emulator UI](Images/compute-emulator-ui.png?raw=true)

	_Compute Emulator_

1. Close the compute emulator.


1. Switch to the **Visual Studio 2012** instance with the **Windows 8** project open.

	> **Speaking Point:** Let's go back to the Windows Store application and add the code to connect to SignalR.

1. Open the **Package Manager Console** and install the SignalR Nuget using the following command:
	
	````
	Install-Package Microsoft.AspNet.SignalR.JS -Pre
	````

	> **Speaking Point:** After adding the NuGet package for SignalR, we need to link our client application with the SignalR hub from the Web app. Let's add a class that will serve this purpose by creating a Hub Proxy when the video is still being updated.

1. Right-click the **js** folder and select **Add | New Item**. Select the **JavaScript File** template and name the file **Notifications.js**.

1. Add the following code into **Notifications.js**.

	> **Speaking Point:** So let me add the code to connect to that SignalR hub.

	(Code Snippet - _Notifications.js - SignalRNotifications_)
	<!-- mark:1-23 -->
	````Javascript
(function () {
    	"use strict";

    	var connection = null;

    	WinJS.Namespace.define("Notifications", {
        	connect: connect
    	});

    	function connect() {
        	if (connection == null) {
            	connection = $.hubConnection(Configuration.ApiBaseUrl);
            	var hub = connection.createHubProxy("Notifier");
            	hub.on("onVideoUpdate", function (video) {
                	Data.updateVideoItem(video);
            	});

            	connection.start({ waitForPageLoad: false });
        	}
    	}

    	connect();
})();
	````

1. The above code will display the tracing message when the message for SignalR arrives. Highlight the main parameters of the code:	
	- **Notifier**: The hub name
	- **onVideoUpdate**: The callback method

	> **Speaking Point:** So it's just a few lines of JavaScript, just connecting to the notifier hub, and then I'm just handling inside this Windows 8 the on-video update message, and then when it does it, it's just updating some UI.

	![windows8-signalr-notification](Images/windows8-signalr-notification.png?raw=true)

	_SignalR notification in the Windows 8 app_

1. Open **videoList.html** from the **pages/videoList** folder and add the following script references at the end of the head section.

	> **Speaking Point:** So let's now reference this new JavaScript code from the video list page where the UI updates will take place. As you can see, we are also referencing the SignalR JS library from the NuGet package we just installed.

	(Code Snippet - _videoList.html - SignalRScriptReferences_)
	<!-- mark:1-2 -->
	````HTML
      <script src="/Scripts/jquery.signalR-1.0.0-alpha2.min.js"></script>
      <script src="/js/notifications.js"></script>
   </head>
	````

	> **Note:** Make sure that the script reference to **jquery.signalR-1.0.0-alpha2.min.js** in the code from above matches the script file located inside the **Scripts** folder. The name of this script may change when SignalR is released.

1. Open the **config.js** file inside the **js** folder, and modify the **ApiBaseUrl** property to point to **http://127.0.0.1:81/**.

	> **Speaking Point:** And we are almost done. We just need to make the Win 8 app to point back to our local Web API endpoint, which is now hosted in our new Web app role.

	![modify Web API endpoint](Images/modify-webapi-endpoint.png?raw=true)

	_Modify Web API Endpoint_

1.	Press **F5** to start the Windows 8 application from **Visual Studio**.

	> **Speaking Point:** And now when I go ahead and run this application, what I will see is sort of a view of my app here.

1. Log in to the app with Facebook.

	![Windows 8 app login with Facebook](Images/windows8-facebook-login.png?raw=true)

	_Windows 8 app login with Facebook_

1. Right-click on the screen and click the **Upload** button.

	> **Speaking Point:** I can go ahead now and upload a new video. This is running on my dev machine so I don't have a camera.

	![Upload Video Button](Images/windows8-upload-button.png?raw=true)
	
	_Upload Video Button_

1. Enter a title, description, select some tags, and upload a video from the **[working dir]\Assets\videos** folder.

	![Uploading a Video](Images/windows8-upload-video.png?raw=true)
	
	_Uploading a Video_

	> **Speaking Point:**
	>  This is running on my dev machine so I don't have a camera. So I can select a video. Going to upload it now to my local emulated environment that's running, again, on my local development machine. It's then calling out to Windows Azure Media Services in the cloud, and so it's going to be, again, making another REST call out to Windows Azure Media Services, which is then going to store it, kick off the encoding tasks on it.

1. Once upload is completed, **go back to the video list page** and after a few seconds you will see the "encoding progress bar" in the new video, and the _last updated_ label being refreshed periodically. 

	> **Note:** And now when I go back here, you'll notice that our UI is showing a placeholder for that video. And if you look closely at this time stamp here, what you should see its' changing about every five seconds. And that's basically because our background service is waking up, checking on the status, firing messages to our ASP.NET app, which is then broadcasting it to all the listening clients that are connected.

	![Video encoding notification thumbnail](Images/video-encoding-notification-thumbnail.png?raw=true)
	
	_Video encoding notification_

1. Switch to the **Visual Studio** instance with the **BuildClips** Web application.

	> **Note:** And the beauty about this architecture is it will work not just with one machine on my local desktop, but if I have thousands or hundreds of thousands of clients connected, they'll all get those real-time updates, and I can kind of provide a nice user experience for them to use. So we've built our app now. We can go ahead and deploy it into Windows Azure. 

1. Right-click the **BuildClips.Azure** cloud project and select **Publish**. Click **Import** and open the publish settings file located in **Downloads**.

	> **Speaking Point:** Doing that is pretty easy. You saw how we could right-click and publish a website. I can now do the same thing using Windows Azure Cloud Services. I already imported the publish settings file.

	![Publishing the Cloud Service](Images/cloud-service-publish-signin.png?raw=true "Publishing the Cloud Service")

	_Publishing the Cloud Service_

1. In the Settings page, choose the already created **BuildClips** cloud service and click **Next**.

	> **Speaking Point:** All you do is just right-click on the cloud service and I can then pick what part of the world I want to deploy it into.

	![Visual Studio publish cloud service settings](Images/cloud-service-publish.png?raw=true)

	_Visual Studio publish cloud service settings_

1. Talk about the publishing workflow (do not click **Publish**).

	> **Speaking Point:** When I go ahead and hit publish, Visual Studio packages that up into what's called a service package file. It's going to be like a zip file, and uploads it into Windows Azure. And then what Windows Azure's going to do is it's going to find appropriate servers to run within the datacenter, automatically select them for me, image them with whatever operating system or dependencies I need, and then install my application on them. And once that application is deployed on it, it will automatically wire up a network load balancer and start sending traffic to the application. And the beauty about cloud services is it's fully automated for me. I don't have to manually set up the machines. All that is handled for me by the core platform.

	![Visual Studio publish cloud service summary](Images/cloud-service-publish-summary.png?raw=true)

	_Visual Studio publish cloud service summary_

	> **Note:** In order to publish the service, you first need to change the database connection string to point to an Azure SQL Database (in both web.config and app.config files of the web and worker roles respectively) and also modify the _ApiBaseUrl_ in the app.config file to point to your Windows Azure Cloud Service address (_http://{your-cloud-service}.cloudapp.net_):


<a name="segment4" />
### Segment 4: Scaling with Windows Azure Caching ###

In this segment, you will improve the scalability and performance of your Windows 8 and web applications by using the new Windows Azure Caching support.  

1. The BuildClips Web app and the Windows 8 application should be running from the previous demo.  

1. In the Windows 8 app, open the **charms bar** and select **Settings**.

	> **Speaking Point:** Our Windows 8 application has a development setting that we can enable to measure the execution time of calls to services. 

1. Click the **Profiling** option. 

1. Set **Enable Profiling** to **On**.

	> **Speaking Point:** After you enable profiling, you can see the execution time for calling out to the Web APIs in the application.  We will use Windows Azure Caching to improve the response time for these calls and improve the scalability for our application by not querying the database on each request. 

	> It should be pointed out that we are not measuring the overall response time of the request from the client application's perspective. Instead, we'll just show the impact that caching has on the service by measuring the execution time on the server. In other words, the time taken to process a request, from the moment it's received until a response is ready to be sent out to the network.

	!["Enabling profiling"](Images/enabling-profiling.png?raw=true "Enabling profiling")

	_Enabling profiling_

1. Switch to the Visual Studio 2012 instance that has the BuildClips web project already open and stop the solution.  

	> **Speaking Point:** Let's enable Windows Azure Caching for our cloud service. 

1. Expand the **BuildClips.Azure** project and double-click the **BuildClips** web role to open its **Properties** window. Switch to the **Caching** page and then select **Enable Caching** for the web role.

	> **Speaking Point:** To do this, we'll simply navigate to the properties for our web role. Here you can see on the caching tab that I can simply turn on caching support for the role.  

	![Enabling caching in Web Role properties](Images/web-role-properties.png?raw=true "Enabling caching in Web Role properties")

	_Enabling caching in Web Role properties_

	> **Speaking point:** We can tell Windows Azure to use a percentage of the available memory in the web role. 
	
1. Press **CTRL** + **S** to save the changes and close the Properties window.

1. Right-click the **solution** and then open the **NuGet Package Manager**.

	> **Speaking point:** Now we need to reference the Windows Azure Caching assemblies so we can interact with the cache from our application.  To do this, we'll simply right-click our solution to pull up the Manage NuGet Packages dialog and install the Windows Azure Caching NuGet package into our projects.

	![Manage nuget packages for solution](Images/manage-nuget-packages-for-solution.png?raw=true)

	_Manage nuget packages for solution_

1. In the **NuGet Package Manager**, expand the **Online** node and search for the **Windows Azure Caching** package.

1. Select package, click **Install** and confirm to install the package **in the three projects**. Close the package manager.
	
	![Install caching nuget](Images/install-caching-nuget.png?raw=true)

	_Install caching nuget_

1. Open **Web.config** in the **BuildClips** project. Locate the `<dataCacheClients>` element and replace the `[cache cluster role name]` placeholder with **BuildClips**. 

1. Uncomment the `<localCache>` XML element.

	> **Speaking point:** The NuGet package will also add a few configuration settings in our Web.config file.  We need to update one of the settings to tell the application to use our web role for caching. 

	<!-- mark:4,5 -->
	````XML
	  </entityFramework>
	  <dataCacheClients>
		 <dataCacheClient name="default">
			<autoDiscover isEnabled="true" identifier="BuildClips" />
			<localCache isEnabled="true" sync="TimeoutBased" objectCount="100000" ttlValue="300" />
		 </dataCacheClient>
	  </dataCacheClients>
	  <cacheDiagnostics>
		 <crashDump dumpLevel="Off" dumpStorageQuotaInMB="100" />
	  </cacheDiagnostics>
	</configuration>
````

1. Open **app.config** in the **BackgroundService** project. Locate the `<dataCacheClients>` element and replace the `[cache cluster role name]` placeholder with **BuildClips**.

1. Uncomment the `<localCache>` XML element.
 
	> **Speaking point:** We also need to do the same in the configuration for our worker role.  

	<!-- mark:4,5 -->
	````XML
	  </system.diagnostics>
	  <dataCacheClients>
		 <dataCacheClient name="default">
			<autoDiscover isEnabled="true" identifier="BuildClips" />
			<localCache isEnabled="true" sync="TimeoutBased" objectCount="100000" ttlValue="300" />
		 </dataCacheClient>
	  </dataCacheClients>
	  <cacheDiagnostics>
		 <crashDump dumpLevel="Off" dumpStorageQuotaInMB="100" />
	  </cacheDiagnostics>
	  <system.web>
````
	> **Speaking point:** Now let's write some code in our service layer to store data in Windows Azure Caching when we retrieve the list of videos from the database.

1. Open **VideoService.cs** in the service library and replace the current implementation of the **GetAll** method with the following code.
	
	> **Speaking point:** To do this, let's update our GetAll method so that we use caching.  Here, for the sake of time, I'll use a code snippet to pull in a bit of code.  This code will get a reference to our cache and check to see if the video metadata has already been cached. If it has been cached, then we will use the cached data.  However, it the data is not in cache, we will proceed to query the database and add the results to our cache, so they are available for the next request. In a more realistic scenario, retrieving information from the database would involve going out into the network but since we are running everything locally, you can see that we have intentionally made this more expensive by introducing an arbitrary delay.  

	(Code Snippet - _VideoService.cs - GetAll - Caching_)
	<!-- mark:3-15 -->
	````C#
	public IQueryable<Video> GetAll()
	{
		 var dataCache = new DataCache();
		 var videos = dataCache.Get("videoList") as IEnumerable<Video>;

		 if (videos == null)
		 {
			  videos = this.context.Videos.OrderByDescending(v => v.Id);

			  dataCache.Put("videoList", videos.ToList());

			  Thread.Sleep(500);
		 }

		 return videos.AsQueryable();
	}
    ````

1. Select the **DataCache** type and press **CTRL+.** to add the using statement.

1. Now add the following highlighted code in the **Publish** method to invalidate the cache when a video is updated.

	> **Speaking point:** So our app is now updated to cache the videos. However, we do need to also update the cache whenever videos change.  As we saw previously, we have a worker role that is watching for changes to the video status.  So let's update the Publish method that the worker role invokes to remove the data from the cache whenever the video changes, causing the next request to reload data from the database. Strictly speaking, we should also apply code to invalidate the cache in the methods that create or delete a video but we'll skip this change for the time being.

	(Code Snippet - _VideoService.cs - Publish - Caching_)
	<!-- mark:6-7 -->
	````C#
	public void Publish(int id)
   {
		...
		this.context.SaveChanges();

		var cache = new DataCache();
		cache.Remove("videoList");
	}
	````

1. Open **WebApiConfig.cs** in the **App_Start** folder of the **BuildClips** project and insert the following highlighted code into the **Register** method. Then place the cursor over the **ApiExecutionProfiler** type and press **CTRL+.** to add the using directive.

	(Code Snippet - _WebApiConfig.cs - Register - ApiExecutionProfiler_)
	<!-- mark:8-9 -->
	````C#
	public static void Register(HttpConfiguration config)
   {
       config.Routes.MapHttpRoute(
           name: "DefaultApi",
           routeTemplate: "api/{controller}/{id}",
           defaults: new { id = RouteParameter.Optional });

       Action clearCache = () => new Microsoft.ApplicationServer.Caching.DataCache().Clear();
       config.MessageHandlers.Add(new ApiExecutionProfiler(clearCache));
	}
	````
	> **Speaking point:** For this demo, we'll update the Web API configuration to insert a message handler that will profile the execution of each service call. The handler will also allow clearing the data cache on demand. This is not something that you would normally have in your applications but we'll use this feature to test out caching.

1. Press **F5** to run the application in the compute emulator.

1. Switch back to the Windows 8 application.

	> **Speaking point:** Now let's switch back to our Windows 8 app that is already running.  

1. Right-click the application to open the action bar and click **Refresh** to update the list of videos. Point out the response time for the request. 

	> **Speaking point:** Let's refresh our list of videos. Since this is our first request following a cold start of the application, the measured time also includes the warm-up time for the cache, database connection initialization, and other factors that make it unsuitable for representing the cache miss scenario accurately, so we'll skip this first reading.

	![Refreshing the video list](Images/refresh.png?raw=true "Refresh")

	_Refreshing the video list_

1. In the Windows 8 app, open the **charms bar** and select **Settings**. 

1. Now, select the **Profiling** option and click **Clear Cache**.

	![Clear cache](Images/clear-cache.png?raw=true "Clear Cache")

	_Clear cache_

	> **Speaking point:** Let's start from a known state by clearing the cache. This will remove all items from the cache.  

1. Now refresh the list again. 

	> **Speaking point:** Let's refresh our list of videos. Since the cache is now empty, the service will need to go back to the database to retrieve the list of videos and then it will add them to the cache. Notice that it's taking a significant amount of time to do this. 

	![Response time with cache miss](Images/response-time-slow.png?raw=true "Response time")

	_Response time with cache miss_

1. Refresh the list of videos again one or more times to show that caching response time has decreased.

	> **Speaking point:** Now you can see that the response time has significantly improved. 

	![Response time with cache hit](Images/response-time-fast.png?raw=true "Response time")

	_Response time with cache hit_

1. Clear the cache again to force a cache miss and repeat the sequence.

1. Stop both the Web and Windows 8 applications.

<a name="Segment5" />
### Segment 5: Deploying and Managing Windows Azure apps ###

>**Note:** This segment is optional. Before executing these steps, make sure you have completed the procedure described in the **Setup and Configuration** section (see [Deploying a Cloud Service and Configuring New Relic (optional)](#setup9)) to properly set up and deploy your application.

In this segment, you will monitor and manage the Windows Azure Cloud Services that power the Windows 8 application. You will also show how to provision a partner service (New Relic) from the new Windows Azure Store.

1. Switch to the Windows Azure Management portal. By default, you will see the Media Services content page from the previous demos.

1. Open the previously deployed **BuildClips** cloud service.

	>**Speaking Point:** Let's go back to the BuildClips cloud service. Every cloud project in Azure has this nice dashboard view where we can see the overall health of the application. We can actually see individual machines, CPU, network and memory access, as well as the aggregate view of it. It displays separate information for both front-end and back-end roles.

	![Windows Azure Dashboard ](Images/windows-azure-dashboard.png?raw=true "Windows Azure Dashboard ")

	_Windows Azure dashboard_

1. Show that you have Production and Staging environments and how you can swap between them.

	>**Speaking Point:** We are having deployments in Production and in Staging. The Swap Deployment operation initiates a virtual IP swap between staging and production deployment environments for a service. If the service is currently running in the staging environment, it will be swapped to the production environment. If it is running in the production environment, it will be swapped to staging.

	![portal swap environments ](Images/portal-swap-environments.png?raw=true)

	_Swapping environments using the portal_

1. On the **Scale** page, show how you can scale up the number of web and worker role instances independently.

	>**Speaking Point:** The initial number or roles is determined by the **ServiceConfiguration** file that we uploaded. The **Instances** setting controls the number of roles that Windows Azure starts and is used to scale the service.

	![portal scale](Images/portal-scale.png?raw=true)

	_Scaling using the portal_

1. Go to the **MONITOR** page. Select **ADD METRICS** in the command bar to show how you can add a metric to the dashboard.

	>**Speaking Point:** In the monitor page, we can monitor key performance metrics for the cloud services in the portal, and customize what we monitor so we can meet our needs in order to check how our application is working.

	![portal add metrics](Images/portal-add-metrics.png?raw=true)

	_Adding new metrics to Windows Azure portal_

	![portal show metrics](Images/portal-show-metrics.png?raw=true)

	_Windows Azure portal metrics_

1. Navigate to the **Windows Azure Store** menu by clicking on **New** | **Store**. Scroll over all the available add-ons to show the multiple options that users have for extending their apps.

	>**Speaking Point:** The management portal makes it incredibly easy in the same way that we could create a new website, create a new virtual machine or mobile service, we can just go inside the portal and say new - store, and this will go ahead and bring up a list of services provided from Microsoft partners.

	>Here we can see services ranging from IP address checking, creating MySQL databases, MongoDB databases, or adding monitoring tools like New Relic. You can do it all directly from this portal. You can click on any of these and purchase them in your Windows Azure subscription without having to enter a new credit card or payment methods; you can use the existing payment mechanisms.

1. Select **New Relic** from the list.

	>**Speaking Point:** We will just try out the **New Relic** add-on as an example. It provides additional rich monitoring and outside-in monitoring support that we can take advantage of now easily within Windows Azure.

	![Adding New Relic Add-On](Images/adding-new-relic-add-on.png?raw=true "Adding New Relic Add-On")

	_Adding New Relic Add-On_

1. Click **Next** to show the **Personalize Add-on page**.

	>**Speaking Point:** They're actually offering a free edition of their standard package that's available to all Windows Azure customers, and we can run it, actually, on any number of servers.

	![Personalize-new-relic-add-on](Images/personalize-new-relic-add-on.png?raw=true "Personalize New Relic Add-On")

	_Personalize New Relic Add-On_

1. Do not complete the purchase. Close the window.

	>**Speaking Point:** I will now close this window since I have already configured my application to work with New Relic.

1. Navigate to the **Add-ons** section of the **Windows Azure Management portal** and select your previously created **New Relic** service. Show the dashboard options that the service offers.

	>**Speaking Point:** This is the service dashboard. We can see at a high level what's going on with the service. We can get the connection info, so if we need for example, the developer key in order to set up New Relic in our application and allow it to log data and send it to New Relic, we can do that. We can also upgrade it to the professional account later, if we want to.

	![New Relic Azure Dashboard](Images/new-relic-azure-dashboard.png?raw=true "New Relic Azure Dashboard")

	_New Relic Azure Dashboard_

1. Click **MANAGE** in the command bar to go to the **New Relic** dashboard and show it.

	> **Speaking Point:** By clicking this "manage" button, it will now do a single sign-on for us into the New Relic management portal for the service that they provide. Now, in a single view, I can actually drill in through the New Relic portal into my Windows Azure apps and services that are running in my cloud.

	![New Relic Dashboard](Images/newrelic-1-overview.png?raw=true "New Relic Dashboard")

	_New Relic dashboard_

1. Click on the **buildclips** application to drill into the service.

	>**Speaking Point:** We can see how our app server is doing from a health perspective. Once we drill down into an application, we can observe a quick overview of our application server including server processing time, our KPI scores as well as system throughputs.

	![Service Overview](Images/newrelic-n1.png?raw=true "Application Overview")

	_Service overview_

	>**Note:** Click on the **App Server** button to switch to the App Server view, if this is not the current view (New Relic remembers your last selection).

1. Click on the **Browser** button to go to the Browser view.

	>**Speaking Point:** They have some pretty cool features that allow us to see how browsers are doing, so we can actually see a browser view of responsiveness of the app in real time as well as how it's performing from various geographic locations around the country and around the world.

	>Two most important response times in performance tuning are shown in the upper-left corner - client perceived response time and server processing time. Client perceived response time is important because that's what our customers care about. Server processing time is useful when you try to fine-tune performances of specific server APIs.

	![Browser view](Images/newrelic-n2.png?raw=true "Browser view")

	_Browser view_

1. Click on the **Map** tab.

	>**Speaking Point:** This is a different system view that shows our dependencies on external services. If we recall the presentation slide just now, we can see this map matches with the architecture diagram pretty well.

	![Map view](Images/newrelic-3-map.png?raw=true "Map view")

	_Map view_
