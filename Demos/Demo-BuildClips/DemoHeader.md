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

$$$segment1

$$$segment2

$$$segment3

$$$segment4

$$$segment5