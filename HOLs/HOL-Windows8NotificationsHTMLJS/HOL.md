<a name="HOLTop" />
# Sending Windows 8 Push Notifications using Windows Azure and the Windows Push Notification Service (JS) #
---

<a name="Overview" />
## Overview ##
In this hands-on lab, you will learn how to deploy a version of the [Windows Azure Toolkit for Windows 8](http://watwindows8.codeplex.com/) to Windows Azure and then use this deployment to send notifications to your client application via the [Windows Push Notification Service (WNS)] (http://msdn.microsoft.com/en-us/library/windows/apps/hh465460\(v=vs.85\).aspx). By the end of this lab you will have a fully functional portal capable of sending Toast, Tile and Badge notifications to your Windows 8 Style UI client application.

![Windows Azure Toolkit for Windows 8 delivering a notification via WNS](./Images/windows-azure-toolkit-for-windows-8-deliverin.png?raw=true)

_Windows Azure Toolkit for Windows 8 delivering a notification via WNS_

If you would like to learn more about how this lab works please see [this video](http://channel9.msdn.com/Events/TechDays/TechDays-2012-Belgium/272).

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

- Use the Windows Azure Management Portal to create storage accounts and hosted service components.
- Use the Windows Push Notification and Live Connect Portal to request credentials for use with WNS.
- Deploy web site using Web Deploy.
- Configure a Windows 8 Style UI client to receive notifications.
- Test sending notifications to your client app via WNS using the Windows Azure Toolkit for Windows 8 portal.

<a name="Prerequisites" />
### Prerequisites ###

You must have the following items to complete this lab:

- [Visual Studio Express 2012 for Web][1] or greater.
- [Visual Studio 2012 Express for Windows 8][2] or greater.
- A Windows Azure subscription with the Websites Preview enabled - [sign up for a free trial][3]

[1]:http://www.microsoft.com/visualstudio/
[2]:http://msdn.microsoft.com/en-us/windows/apps/hh852659
[3]:http://aka.ms/WATK-FreeTrial

>**Note:** This lab was designed to use Windows 8 Operating System.

<a name="Setup" />
### Setup ###
In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab’s **source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process that will configure your environment.

1. If the User Account Control dialog is shown, confirm the action to proceed.

>**Note:** Make sure you have checked all the dependencies for this lab before running the setup.

---

<a name="Exercises" />
## Exercises ##
This hands-on lab includes the following exercises:

*	[Getting Started: Deploying the Notification App Server Using Web Deploy](#GettingStarted)
*	[Exercise 1: Configure a Windows 8 Style UI Client application for Push Notifications](#Exercise1)
*	[Exercise 2: Sending Push Notifications](#Exercise2)

Estimated time to complete this lab: **45 minutes**.

<a name="GettingStarted" />
### Getting Started: Deploying the Notification App Server Using Web Deploy###

In this exercise, you deploy the notification app server to Windows Azure using Web Deploy. To do this, you provision the required service components at the management portal, request credentials from the WNS and Live Connect portal and deploy to Windows Azure using Web Deploy.   

<a name="GSTask1" />
#### Task 1 – Creating a Storage Account and Web Site ####

The application you deploy in this exercise requires a Web Site and a Storage Account. In this task, you create a new storage account to allow the application to persist its data. In addition, you define a Web Site to host the notification app server.

1. Navigate to [<https://manage.windowsazure.com>](<https://manage.windowsazure.com>) using a Web browser and sign in using the Microsoft Account associated with your Windows Azure account.

	![Signing in to the Windows Azure platform Management portal](./Images/signing-in-to-the-windows-azure-platform-mana.png?raw=true)

	_Signing in to the Windows Azure platform Management portal_

1. First, you will create the **Storage Account** that the application will use to store its data. In the Windows Azure Management Portal, click **New** | **Data Services** | **Storage** | **Quick Create**.

1. Set a unique **URL**, for example _notificationapp_, and select **Create Storage Account**.
 
	![Creating a new storage account](./Images/creating-a-new-storage-account.png?raw=true)

	_Creating a new storage account_

	> **Note:** The URL used for the storage account corresponds to a DNS name and is subject to standard DNS naming rules. Moreover, the name is publicly visible and must therefore be unique. The portal ensures that the name is valid by verifying that the name complies with the naming rules and is currently available. A validation error will be shown if you enter a name that does not satisfy the rules.
	>
	> ![URL Validation](./Images/url-validation.png?raw=true)

1. Wait until the Storage Account is created. Click your storage account's name to go to its **Dashboard**.

	![Storage Accounts page](./Images/storage-accounts-page.png?raw=true "Storage Accounts page")

	_Storage Accounts page_

1.	Click **Manage Keys** at the bottom of the page in order to show the storage account's access keys.

	![Manage Keys](./Images/manage-keys-option.png?raw=true "Manage Keys")

	_Manage Storage Account Keys_

1. Copy the **Storage Acount Name** and the **Primary access key**. You will use these values later on to configure the application.

	![Manage Storage Account Keys](./Images/manage-storage-account-keys.png?raw=true "Manage Storage Account Keys")

	_Manage Storage Account Keys_

	>**Note:** The **Primary Access Key** and **Secondary Access Key** both provide a shared secret that you can use to access storage. The secondary key gives the same access as the primary key and is used for backup purposes. You can regenerate each key independently in case either one is compromised.

1. Go back to the portal home page, and select **Websites**.

1. Select **New**, then select **Compute** | **Web Site** from the list and then **Quick Create**.

1. Choose a name for your Web Site and then select **Create Web Site**.

	![Creating a new Web Site](./Images/creating-a-new-web-site.png?raw=true)
	
	_Creating a new Web Site_

<a name="GSTask2" />
#### Task 2 – Updating the application with your Storage Account Name and Key ####
In this task, you will update the Connection String values within the web configuration file using the Storage Account you created in the previous task.

1. In a new instance of **Visual Studio 2012**, open **Services.sln** located in the **Source/Assets/Server/Notifications.Backend** folder.  This is the Notification App Server.

1. Open **Web.config**.  In the **appsettings** section, replace the value of the **DataConnectionString** setting using the recommended format as shown in the commented lines in the snippet below.

	````XML
	<!--
		When deploying to Windows Azure, replace the DataConnectionString setting with the
		connection string for your Windows Azure Storage account. For example:
		"DefaultEndpointsProtocol=https;AccountName={your storage account name};AccountKey={your storage account key}"
	-->
	<add key="DataConnectionString" value="DefaultEndpointsProtocol=https;AccountName={your storage account};AccountKey={your storage account key}"/>
	````

1. Press **CTRL+SHIFT+B** to build the solution.

<a name="GSTask3" />
#### Task 3 – Requesting WNS Credentials and updating the Web.config ####
In this task, you will obtain the Windows Push Notification Services (WNS) credentials and use them to update the Web Configuration file.

1.	In a new instance of **Visual Studio 2012**, open your existing HTML5/JS Windows 8 Style UI application or create a new application by clicking **File | New Project | Installed Templates | JavaScript | Windows Store | Blank App**. 

	> **Note:** If it's the first time you create a Windows Syle UI application you will be prompted to get a developer license for Windows 8 to develop this kind of applications. In the Developer License window, click **I Agree**.
	
	> ![Getting a Developer License](Images/getting-developer-license.png?raw=true "Getting a Developer License")
	
	> You will be requested to sign in using your Microsoft credentials. Do so and click **Sign In**. Now you have a developer license.

	> ![Inserting credentials to obtain a developer license](Images/inserting-credentials-developer-license.png?raw=true "Inserting credentials to obtain a developer license")

	> ![Developer License successfully obtained](Images/developer-license-succesfully-obtained.png?raw=true "Developer License successfully obtained")

1.	In solution explorer open your **package.appxmanifest** and select the **Packaging** tab.  We will use the **Package Display Name** for creating your **WNS** Credentials.

	![Opening package.appxmanifest](./Images/opening-packageappxmanifest.png?raw=true)

	_Opening package.appxmanifest_

1.	Click **Store** in the Visual Studio menu and select **Reserve App Name**.

	![Reserving App Name](./Images/reserving-app-name.png?raw=true)

	_Reserving App Name in Windows Store_

1.	The browser will display the Windows Store page that you will use to obtain your WNS credentials. In the Submit an app section, click **App Name**.

	> **Note:** You will have to sign in using your Microsoft Account to access the Windows Store.

	![Giving your app a unique name](./Images/giving-app-name-windows-store.png?raw=true)

	_Giving your app a unique name_

1.	In the App name field, insert the Package Display Name that is inside the **package.appxmanifest** file of your solution and click **Reserve app name**. Then click **Save** to confirm the reservation.

	![Reserving an app name](./Images/app-name-windows-store.png?raw=true)

	_Reserving an app name_

	![Confirming the app name reservation](./Images/name-reservation-successful-win-store.png?raw=true)

	_Confirming the app name reservation_

1. Now you will have to identify your application to get a name and a publisher to insert in the **package.appxmanifest** file. In the Submit an app page, click **Advanced features**.

	![Configuring push notifications for the Notifications.Client app](./Images/app-name-reverved-completely-windows-store.png?raw=true)

	_Configuring push notifications for the Notifications.Client app_

1. In the Advanced features page, click **Push notifications and Live Connect services info**.

	![Advanced features page](./Images/push-notif-live-connect-service-info.png?raw=true)

	_Advanced features page_

1. Once in the Push notifications and Live Connect services info section, click **Identifying your app**.

	![Push notifications Overview page](./Images/identifying-your-app.png?raw=true)

	_Push notifications Overview page_

1. Now we have to set the Identity Name and Publisher of our **package.appxmanifest** file with the information in Windows Store. Go back to Visual Studio, right-click the **package.appxmanifest** and select **View Code**. Replace the Name and Publisher attributes of the Identity element with the ones obtained in Windows Store. Click **Authenticating your service**.

	![Setting Identity Name and Publisher](./Images/app-identification.png?raw=true)

	_Setting Identity Name and Publisher_

1. Finally we obtained a **Package Security Identifier (SID)** and a **Client secret**, which are the WNS Credentials that we need to update the Web configuration of our Notification App Server.

	![Package Security Identifier (SID) and Client secret](./Images/sid-client-secret.png?raw=true)

	_Package Security Identifier (SID) and Client secret_

1.	Switch to the Notification App Server, open **Web.config** file and replace _[YOUR_WNS_PACKAGE_SID]_ with the **Package Security Identifier (SID)**  and _[YOUR_WNS_CLIENT_SECRET]_ with the **Client secret** that you obtained either from the Windows Store or from the Windows Push Notifications & Live Connect Portal.

	![Updating Web.config with WNS Credentials](./Images/updating-webconfig-with-wns-cred.png?raw=true)

	_Updating Web.config with WNS Credentials_

	> **Note:** Ensure you have not copied a white space on the start or end of the values in the **Web.config** and that the **Package SID** and **Client Secret** were pasted into the correct fields.

1.	Your Notification App Server is now ready to deploy to Windows Azure. Note if your account is limited to one core.

<a name="GSTask4" />
#### Task 4 – Deploy your Notification App Server to Windows Azure using Web Deploy####

In this task, you will deploy the Notification App Server to Windows Azure using Web Deploy.

1. In the Windows Azure Portal, select **Websites**, and then select your Web Site to open the **Dashboard**.  In the **Dashboard** page, under the **quick glance** section, click the **Download publish profile** link and save the file to a known location. You will use theses settings later to publish the web site from Visual Studio.

	> **Note:** The _publish profile_ contains all of the information required to publish a web application to a Windows Azure website for each enabled publication method. The publish profile contains the URLs, user credentials and database strings required to connect to and authenticate against each of the endpoints for which a publication method is enabled. **Microsoft Visual Studio** supports reading publish profiles to automate the publishing configuration for web applications to Windows Azure Websites.

	![Downloading the publish profile](./Images/download-publish-profile.png?raw=true "Downloading the publish profile")

	_Downloading the publish profile_

1. In Visual Studio's Solution Explorer, right-click the **Notification App Server** project node and select **Publish** to open the Publish Web wizard.

	![Publishing the service](./Images/publishing-the-service.png?raw=true "Publishing the service")

	_Publishing the service_

1. In the **Profile** page, click the **Import** button and select your publishing profile file. Click **Next**.

	![Publising profile profile selection](./Images/publishing-profile-profile-selection.png?raw=true)

	_Selecting a publishing profile file_

1. In the **Connection page**, leave the imported values and click **Next**.

	![Publishing profile imported](./Images/publishing-profile-imported.png?raw=true "Publishing profile imported")

	_Publishing profile imported_

1. In the **Settings** page, leave the default values and click **Next**.

	![Publishing profile, Settings page](./Images/publishing-profile-settings-page.png?raw=true "Publishing profile, Settings page")

	_Publishing profile - Settings_

1. In the **Preview** page, click **Publish**.

	![Publishing Profile - Preview Page](./Images/publishing-profile-preview-page.png?raw=true "Publishing Profile - Preview Page")

	_Publishing Profile - Preview Page_	

1.	Navigate to your deployed web site to confirm that it is up and running.

	![Notification App Server portal running in Windows Azure](./Images/notification-app-server-portal-running-in-win.png?raw=true)

	_Notification App Server portal running in Windows Azure_

<a name="Exercise1" />
### Exercise 1: Configure a Windows 8 Style UI Client application for Notifications ###

In this exercise, you will configure your client application to request a notification channel from the WNS and register this channel with your Notification App Server running in Windows Azure.

<a name="Ex1Task1" />
#### Task 1 – Configuring the package.appmanifest for Push Notifications ####

In this task, you will update the package.appmanifest to receive Wide Tile notifications using Visual Studio 2012.

1.	Return to your Windows 8 Style UI client application in **Visual Studio 2012 Express for Windows 8**.

1.	In **Solution Explorer** double click **package.appmanifest**.

	![Updating your client app with WNS credentials](./Images/updating-your-client-app-with-wns-credentials.png?raw=true)

	_Updating your client app with WNS credentials_

1. In order to enable your application to receive **Wide Tile** notifications, click **Wide Logo** in the **All Image Assets** pane. In the **Scaled Assets** section, click the **...** button located below the **Scale 100** image, and navigate to the **Assets/Client** folder. Then, select **widelogo.png** and click **Open**.

	![Adding a wide logo to your application](./Images/adding-a-wide-logo-to-your-application.png?raw=true)

	_Adding a wide logo to your application_

1.	Scroll down and change the **Toast Capable** dropdown to **Yes.**

	![Configuring your client application to allow Toast Notifications](./Images/configuring-your-client-application-to-allow.png?raw=true)

	_Configuring your client application to allow Toast Notifications_

	> **Note:** In the following steps of this task you will associate your application with the Windows Store. If you obtained your WNS credentials from the Windows Push Notifications & Live Connect Portal, there is no need to execute these steps.

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

1.	**Close** and **Save** changes to **package.appmanifest**.

<a name="Ex1Task2" />
#### Task 2 – Updating the Client Codebase for Notifications ####

In this task, you will update your client application to be able to send push notifications using the Notification App Server.

1.	In Solution Explorer **Right click** the **js** folder and select **Add | Existing Item**.

1.	Browse to the **Source/Assets/Client/JS**  folder, select **notifications.js** and click **Add**.

	![Add notifications.js to your client application](./Images/add-notificationsjs-to-your-client-applicatio.png?raw=true)

	_Add notifications.js to your client application_

1. Open **notifications.js** file and update the url to point to the correct endpoint.  Update the **serverUrl** value _[YOUR_DNS_NAME]_ in http://_[YOUR_WEBSITE_DOMAIN]_/endpoints.  You obtain the **DNS** value from the web site you created in the **Windows Azure Management Portal** in the previous exercise.

	`var serverUrl = "http://[YOUR_WEBSITE_DOMAIN]/endpoints";`


1.	Open **default.html** within the Solution Explorer and add a **script reference** to _/js/notifications.js_ and a **div** tag with id _statusMessage_.

	<!-- mark:15,18 -->
	````HTML
	<!DOCTYPE html>
	<html>
	<head>
		 <meta charset="utf-8">
		 <title>Application21</title>

		 <!-- WinJS references -->
		 <link href="//Microsoft.WinJS.0.6/css/ui-dark.css" rel="stylesheet">
		 <script src="//Microsoft.WinJS.0.6/js/base.js"></script>
		 <script src="//Microsoft.WinJS.0.6/js/ui.js"></script>

		 <!-- Application21 references -->
		 <link href="/css/default.css" rel="stylesheet">
		 <script src="/js/default.js"></script>
		 <script src="/js/notifications.js"></script>
	</head>
	<body>
		  <div id="statusMessage"></div>
	</body>
	</html>
	````

1.	Open **default.js** within **js** folder, and add a call to the **openNotificationsChannel()** of **notifications.js**.

	![Adding a call to openNotificationsChannel() to ensure your channel is requested from WNS and Registered with your Notification App Server](./Images/adding-a-call-to-opennotificationschannel-to.png?raw=true)

	_Adding a call to openNotificationsChannel() to ensure your channel is requested from WNS and Registered with your Notification App Server_

1.	Save all the changes from **File** | **Save All** (or by pressing **Ctrl + Shift + S**).

1.	In the **Build** menu, click **Build Solution** to ensure your builds.

1.	Open **notifications.js** and locate the **openNotificationsChannel()** method. This method will create a Push Notification Channel for the Notification App Server.

<a name="Exercise2" />
### Exercise 2: Sending Push Notifications ###

This section describes how to run your client application and send notifications to it through the notification app server deployed to Windows Azure in the previous exercises.  

<a name="Ex2Task1" />
#### Task 1 – Running the Notification enabled Windows 8 Style UI App ####

In this task, you will run the client application you created in the previous exercise to create a channel for the WNS and register it with the Notification App Server.

1.	Open the **Notifications.Client.sln** Style UI App located under **Source/Ex2-SendingPushNotifications/Begin** folder. Alternatively, you may continue with the solution that you obtained after completing the previous exercise.

	> **Note:** If you chose to open the solution in the Begin folder, you will have to open the **notifications.js** file and update the url to point to the correct endpoint.  Update the **serverUrl** value _[YOUR_DNS_NAME]_ in http://_[YOUR_WEBSITE_DOMAIN]_/endpoints. You obtain the **DNS** value from the web site you created in the **Windows Azure Management Portal**.

	>	`var serverUrl = "http://[YOUR_WEBSITE_DOMAIN]/endpoints";`

1.	Once the solution has opened press **F5**. Due the configuration you made previously when the application launches it will call **openNotificationsChannel()** method. This will request a channel from **WNS** and submit it to the **Notifications App Server** you deployed to Windows Azure.  In the **statusMessage** div, you will see that the **Channel URI** was sent successfully to your service.

	![Client output after successful channel request from WNS and registering with notification app server](./Images/client-output-after-successful-channel-reques.png?raw=true)

	_Client output after successful channel request from WNS and registering with notification app server_

<a name="Ex2Task2" />
#### Task 2 – Sending Push Notifications using the ASP .NET MVC 4 Portal ####

Now that a channel has been successfully requested from WNS and registered with your Notification App Server we can now start to send notifications through the portal.

1.	Switch to the Web browser and log into your deployed application (e.g: http://_[YOUR_SUBDOMAIN]_/.cloudapp.net), using the following credentials:
	1. User Name: **admin**
	1. Password: ![password](./Images/password.png?raw=true) (with a zero)

1.	Once logged in, additional menu options are displayed to allow you to send push notifications and manage the images (blobs) used when sending notifications.

	![Notification App Server Home Page](./Images/notification-app-server-home-page.png?raw=true)

	_Notification App Server Home Page_

1.	Click the **Push Notifications** menu option. Here you will see the channel that you requested using the sample Windows 8 Style UI app that registered with your Web Role.

	![Pushing Notifications](./Images/pushing-notifications.png?raw=true)
	
	_Pushing Notifications_

	> **Note:** If you re-register your channel from your client app while this page is open it is worthwhile refreshing the page to ensure that you have captured the latest channel uri.

1.	You can now send your first _toast_ notification to this channel. Click **Send Notification** button to open the notifications template dialog window.

1.	Select **Toast** in the first drop-down list and then select the **ToastImageAndText01** template.

1.	Configure the template column with your Square **WindowsAzureLogo.png** and type some text into **Regular text** as per below:

	![Selecting notification type and template](./Images/selecting-notification-type-and-template.png?raw=true)

	_Selecting notification type and template_

1.	Click **Send** and observe that the Web portal indicates that the message was successfully delivered to **WNS** and the _Toast_ notification arrives to the Client application.

	![Notification sent confirmation](./Images/notification-sent-confirmation.png?raw=true)

	_Notification sent confirmation_

	>**Note:**  If you are getting a 403 unauthorized or no notifications showing up please try refreshing the page to ensure that the channel that you are sending to is refreshed.  Following this, please check that you created your WNS credentials with the correct CN from your package.appxmanifest and that you have configured your package.appxmanifest and Web.config correctly.

1.	Now you will see how to send a _Tile_ notification. Select **Tile** in the first drop-down and then select the **TileWideImageAndText01** template.

1.	Configure the template column with:
	1.	Your wide logo **WindowsAzureLogoWide.png**.
	1.	Custom text to be displayed in **Large Text** and **Regular Text**.
	1.	Click **Send**.

		![Pushing a Tile notification](./Images/pushing-a-tile-notification.png?raw=true)

		_Pushing a Tile notification_

1.	Return to **Start** by pressing the **Windows key** ( ![start](./Images/start.png?raw=true)) and observe that your _Tile_ notification has now been delivered and is being displayed.

	![Delivered Tile notification](./Images/delivered-tile-notification.png?raw=true)
	
	_Delivered Tile notification_

1.	Now you will see how to send a _Badge_ notification. Select **Badge** in the first drop-down list and then select the **Glyph** template.

1.	In the template column select the **NewMessage** option and click **Send**.

	![Pushing a Badge notification](./Images/pushing-a-badge-notification.png?raw=true)

	_Pushing a Badge notification_

1.	Return to **Start** by pressing the **Windows key** ( ![start](./Images/start.png?raw=true)) and observe that your _Tile_ has now the _Badge_ updated to see the NewMessage Glyph type:

	![Updated Tile notification with a Badge](./Images/updated-tile-notification-with-a-badge.png?raw=true)

	_Updated Tile notification with a Badge_

	>**Note:** This concludes the overview of how to send Toast, Tile and Badge notifications using the Windows Azure Toolkit for Windows 8. As an exercise, it’s recommended to spend some time both exploring the rich set of templates available to each of the different notification types and how you can use blob storage to store your image assets for notifications.

---

<a name="Summary" />
## Summary ##
By completing this Hands-On Lab you have learned how to:

-	 Use the Windows Azure Management Portal to create storage accounts and web site components.
-	Use the WNS and Live Connect Portal to request credentials for use with WNS.
-	Deploy a web site using Web Deploy.
-	Configure a Windows 8 Style UI client to receive notifications.
-	Test sending notifications to your client app via WNS using the Windows Azure Toolkit for Windows 8 portal.

If you would like the full codebase for the **Notification App Server** to update for your own applications please download the **Windows Azure Training Kit for Windows 8** (http://watwindows8.codeplex.com).

---
