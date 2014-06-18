<a name="title" />
# Mobile Services Demo #

---

<a name="Overview" />
## Overview ##

Since Mobile Services was launched, you have seen some strong adoption and some great apps built on top of the platform, across both the consumer and enterprise space. Mobile Services lets you easily add a cloud-hosted back end to your mobile app, regardless of what client platform you are using. 
In this demo, you will see an exciting new set of features that makes Mobile Services even more compelling, especially in the enterprise space. You will see how to build a .NET back end locally, publishing it to the cloud, adding authentication using the **Active Directory Authentication Library** (ADAL), integrating with SharePoint and then building a cross-platform client with **Xamarin**.

<a id="goals" />
### Goals ###
In this demo, you will see how to:

1. Create a Mobile Services C# Backend
1. Integrate ADAL in a Windows Store app
1. Showcase Xamarin for iOS and its integration with ADAL

<a name="technologies" />
### Key Technologies ###

- [Microsoft Visual Studio 2013][2]
- [Visual Studio 2013 Update 2][3]
- [Microsoft Azure Mobile Services][6]
- [Active Directory Authentication Library][4] (ADAL)
- Mac iOS 7.0 (A Mac computer is required to run Xamarin)
- [Xamarin Studio for iOS][5]
	
[2]: http://www.microsoft.com/visualstudio/
[3]: http://www.microsoft.com/en-us/download/details.aspx?id=42666
[4]: http://msdn.microsoft.com/en-us/library/jj573266.aspx
[5]: http://xamarin.com/
[6]: http://azure.microsoft.com/en-us/develop/mobile/

<a name="setup" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment. The following are one-time instructions you need to execute in order to prepare the demo. Once completed, there is no need to execute these steps again; you can simply run **Reset.cmd** located in the **Setup** folder to clear the database and SharePoint files to restart the demo.

You need a Mac computer with iOS 7.0 in order to run the iOS client using Xamarin. Make sure **XCode**, **Git** and **Xamarin for iOS** are installed on the box.

#### Task 1 - Creating an Office 365 Subscription ####

If you do not have an Office 365 subscription you can do one of the following:

- If you are a **Microsoft Partner**, you have the option of requesting a demo tenant at https://www.microsoftofficedemos.com. These demo tenants expire 90 days after creation.

- If you have a **MSDN subscription**, you can activate your **Office 365 Developer** subscription in your MSDN subscriber dashboard.

- Buy a subscription. Go to http://office.microsoft.com/en-us/business/compare-office-365-for-business-plans-FX102918419.aspx and choose the option that best suits your needs (e.g. Office 365 Small Business).

Once you finish signing up for your **Office 365** subscription, follow these steps:

1. Close the browser to clear out the authentication. Open the browser again and go to https://portal.microsoftonline.com/default.aspx.

	> **Note:** This will open the Office 365 Management Portal. It might take a few minutes before all services are provisioned.

1. Click the **Users and groups** option in the left navigation menu.

1. Click your user name and verify that it is linked to the subscription accordingly.

	![Office Subscription Linked](Images/office-subscription-linked.png?raw=true)
	
1. Go to https://{username}-my.sharepoint.com replacing the placeholder with the username you defined before in the **.onmicrosoft.com** domain (e.g.: https://myuser-my.sharepoint.com/).

1. This site is provisioned the first time you actually launch it in the browser. After a few minutes, you should see the following:

	![SharePoint My Site](Images/sharepoint-my-site.png?raw=true)
	
#### Task 2 - Setting Azure + Office 365 Subscription ####

> **Note:** If you have a MSDN subscription, a free Azure subscription is included. Go to the msdn.com subscription dashboard and follow the **Activate Microsoft Azure** option.

1. Open a browser and go to http://manage.windowsazure.com.

1. When prompted, enter your Office 365 credentials.

1. You will see a screen asking you to get a subscription before you can start using Azure. Click **Sign Up for Microsoft Azure**.

	![Sign up for Azure](Images/sign-up-for-azure.png?raw=true)
	
1. You will be taken to a screen where you will validate your details, agree with the Terms and Privacy statement and then finish signing up. Once registered, you will be able to access the Management Portal. Now Azure and Office 365 are both linked to the same user account.

#### Task 3 - Creating a Mobile Service and Registering your Apps in Azure AD ####

1. In the [Management Portal](http://manage.windowsazure.com/), create a new Mobile Service. You can choose between a new Free Database and an existing one. Make sure the **.NET (PREVIEW)** option is selected for the **Backend** drop-down list.

	![Creating a Mobile Service](Images/creating-a-mobile-service.png?raw=true)
	
1. Once created, click the Mobile Service and go to **Identity**.

1. Scroll down to the **Azure Active Directory** identity provider section and copy the **APP URL** listed there.

1. Go to **Active Directory**.

1. Select your directory from the list and go to **Applications**.

1. Click **Add** and select **Add an application my organization is developing**.

1. Type a name, for example _mymobileservice_, and select **Web Application and/or Web API**. Click the arrow button to continue.

	![Creating a Web Application in AD](Images/creating-a-web-application-in-ad.png?raw=true)

1. Paste the Mobile Service URL in the **SIGN-ON URL** and **APP ID URI** field. Click the check button to create the app.

	![Configuring App Properties](Images/configuring-app-properties.png?raw=true)
	
1. Click **Manage Manifest** in the menu and select **Download Manifest**.

	![Download Manifest](Images/download-manifest.png?raw=true)
	
1. Open the application manifest file with **Visual Studio**. At the top of the file, find the app permissions line.

	````JSON
	"appPermissions": [],
	````

1. Replace that line with the following app permissions and save the file.

	````JSON
	"appPermissions": [
	    {
		"claimValue": "user_impersonation",
		"description": "Allow the application access to the mobile service",
		"directAccessGrantTypes": [],
		"displayName": "Have full access to the mobile service",
		"impersonationAccessGrantTypes": [
		    {
			"impersonated": "User",
			"impersonator": "Application"
		    }
		],
		"isDisabled": false,
		"origin": "Application",
		"permissionId": "b69ee3c9-c40d-4f2a-ac80-961cd1534e40",
		"resourceScopeType": "Personal",
		"userConsentDescription": "Allow the application full access to the mobile service on your behalf",
		"userConsentDisplayName": "Have full access to the mobile service"
	    }
	],
	````
	
1. In the Management Portal, click **Manage Manifest** and select **Upload Manifest**. Select the file you just updated and upload the manifest.

1. Scroll down to the **permissions to other applications** section and grant permissions to **Office 365 SharePoint Online**. Select **Edit or delete users' files** from the **Delegated Permissions** drop-down list.

	![Permissions for the Mobile Service App](Images/permissions-for-the-mobile-service-app.png?raw=true)

1. In the **Keys** section, choose a duration from the drop-down list to create a new key. Click **Save**.

1. Take note of the generated key value; you will use it later to update the Mobile Service configuration value named **Active Directory Client Secret**.

	![Generating Secret Key](Images/generating-secret-key.png?raw=true)

#### Task 4 - Associate your Client App to the Windows Store ####
	
1. Open the **FacilityApp\Begin\FacilityApp.sln** solution located under the **Source** folder in Visual Studio.

1. Right-click the **FacilityApp.UI.Windows** project and select **Associate App with the Store...** under **Store** sub-menu.

1. Sign into your **Dev Center** account.

1. Enter the app name you want to reserve and click **Reserve**.

1. Select the new app name and click **Next**.

1. Click **Associate** to associate the app with the store name.

1. Log in to your [Windows Dev Center Dashboard](http://go.microsoft.com/fwlink/p/?linkid=266734&clcid=0x409) and click **Edit** on the app.

1. Then click **Services**.

1. Then click **Live Services Site**.

1. Copy your package **SID** from the top of the page.

1. Switch to the **Management Portal** and go to your AD.

1. Go to **Applications** and click **Add**. Select **Add an application my organization is developing**.

1. Type a name for the client app (e.g.: _facilityappclient_) and select **Native Client Application**. Click next to continue.

	![Creating client AD app](Images/creating-client-ad-app.png?raw=true)
	
1. In the **Redirect URI** field, paste the package **SID** you copied in a previous step. Click the check button to continue.

	![Client App Package SID](Images/client-app-package-sid.png?raw=true)

1. Click the **Configure** tab for the native application and take note of the **Client ID**.

	![Copying the Client ID](Images/copying-the-client-id.png?raw=true)

1. In the **Redirect URIs** section, add the Mobile Services URL. E.g.: https://{mobileservice-name}.azure-mobile.net/.
	
1. Scroll down to the **permissions to other applications** section and grant full access to the mobile service application that you registered earlier. Click **Save**

	![Granting permissions to the Client App](Images/granting-permissions-to-the-client-app.png?raw=true)
	
#### Task 5 - Setting up Configuration Variables ####

1. In the Management Portal, open your Mobile Service and go to **Configure**.

1. Scroll down to the **app settings** section.

1. Add the following settings (take into account that the name of each setting is case sensitive):

	* **SharePointUri**: the SharePoint user's personal site targeting the API address. The URL usually has the following form **https://{domain}-my.sharepoint.com/personal/{username}_{domain}_onmicrosoft_com/_api/web**. For example: https://dpe-my.sharepoint.com/personal/admin_dpe_onmicrosoft_com/_api/web.
	* **SharePointResource**: the base URL of SharePoint's Personal sites collection. E.g.: https://{domain}-my.sharepoint.com
	* **Authority**: the Azure AD authority. Use https://login.windows.net/common/oauth2/authorize
	* **ActiveDirectoryClientId**: the Id of the Mobile Service application registered in the Azure AD
	* **ActiveDirectoryClientSecret**: the secret of the Mobile Service application registered in the Azure AD

1. Click **Save**.

1. Browse to the **Source** folder of this demo and open the file **Config.xml**.

1. The starting solutions will be copied to the **C:\Demos\Source** folder. If you want to change the default directory, update the element **solutionWorkingDir** in **localPaths**.

1. Update the values under **clientSettings** in the XML file to configure your solutions:

	* **AadAuthority**: the Azure AD authority. Use https://login.windows.net/common/oauth2/authorize
	* **AadRedirectResourceURI**: the Mobile Service AAD login URI. You can find this value under **Azure Active Directory** in your Mobile Service's **Identity** tab
	* **AppRedirectLocation**: the Mobile Service URI
	* **AadClientId**: the Id of your native client app registered in your AD
	* **AppKey**: the Mobile Service key. You can retrieve this value by clicking **Manage Keys** in your Mobile Service
	* **MobSvcUri**: the Mobile Service URI
	* **SharePointResource**: the root URL for the personal sites of your SharePoint domain. E.g.: http://{domain}-my.sharepoint.com/
	* **SharePointUri**: the SharePoint user's personal site targeting the API address. The URL usually has the following form **https://{domain}-my.sharepoint.com/personal/{username}_{domain}_onmicrosoft_com/_api/web**. For example: https://dpe-my.sharepoint.com/personal/admin_dpe_onmicrosoft_com/_api/web.
	* **SharePointUser**: Full qualified name for the Office 365 user. E.g.: admin@dpe.onmicrosoft.com.
	
1. The following values are displayed in the Windows Store app. These settings configure the Username and the default location of the device, simulating Geolocation inside the app. You can replace them with a real location (e.g.: the location where the demo will be presented).
	
	* **UserName**: the first name of the User that is displayed on the Windows Store app
	* **UserSurname**: the last name of the User that is displayed on the Windows Store app
	* **BuildingFRVM**: the building name
	* **RoomFRVM**: the room number
	* **CityFRVM**: the city name
	* **StreetFRVM**: the street name
	* **StateFRVM**: the state where the city is located
	* **ZipFRVM**: the zip code

1. Under **windowsAzureSubscription**, update the values of the Mobile Service SQL Server (you can find these values in your Mobile Service configuration):

	* **sqlserver**: the SQL Server address. E.g.: {server}.database.windows.net
	* **db**: the database name.
	* **sqlUsername**: the server administrator username
	* **sqlPassword**: the server administrator password
	* **sqlTable**: the database table including the schema name. Use the following format: **{mobileservice-name}.facilityrequests**

1. Under the **SharePoint** element set the values to connect your SharePoint:

	* **baseUrl**: the SharePoint user's personal site. The URL usually has the following form **https://{domain}-my.sharepoint.com/personal/{username}_{domain}_onmicrosoft_com/**. For example: https://dpe-my.sharepoint.com/personal/admin_dpe_onmicrosoft_com/
	* **username**: Full qualified name for the Office 365 user. E.g.: admin@dpe.onmicrosoft.com
	* **password**: the password for the Office 365 user
	* **folderName**: the folder in the Personal Sites documents list where the app will upload files. Leave the default value, **Requests**

1. Save and close the file.

1. If you want to replace the map image that is displayed on the Windows Store client app, replace the file located in **Source\Setup\assets\image**.

1. Run **Reset.cmd** in the **Setup** folder to execute the reset scripts. These scripts configure the settings files for each client app, remove any records in the Mobile Service SQL database, and delete all the files in the **Requests** folder in SharePoint. Remember to add a firewall rule for your machine to access the SQL database in Azure.

	> **Note:** You can execute **Reset.cmd** any time you need to reset the demo. Since you already configured Azure AD and the Mobile Service, you only need to execute the reset scripts to reset the demo to a starting point.

#### Task 6 - First Run

Follow these steps to run the **FacilityRequests** app to adjust the correct Simulator's resolution display and orientation.

1. Go to the **MAC OS**, type **Xamarin** in the **Spotlight** control and open **Xamarin.iOS Build Host** application.

	![Xamarin.iOS Build Host app](Images/xamarinios-build-host-app.png?raw=true)
	
1. Click **Pair** and take note of the generated **PIN** code. You will use it later in this section.

	![Xamarin.iOS Build Host pin code](Images/xamarinios-build-host-pin-code.png?raw=true)
	
1. Go back to Windows and open the **FacilityApp.sln** solution located in your demo folder (by default **C:\Demos\Source**).

1. A Xamarin dialog box will appear to _pair_ Visual Studio with a Mac. Click **Continue**.

	![Pairing Visual Studio with Xamarin](Images/pairing-visual-studio-with-xamarin.png?raw=true)

1. In the next dialog box, select the Mac instance where Xamarin Studio is installed and click **Connect**.

1. The **Xamarin Pair With** screen will pop up requesting a PIN code. Write the PIN code generated in the previous step and click **Pair**.

	![Pairing with Xamarin in Visual Studio](Images/pairing-with-xamarin-in-visual-studio.png?raw=true)

1. A successfull pairing message pops up. Click **Finish**.

1. Set **FacilityApp.UI.Windows** as the startup project and run the app using the Simulator.

1. Click the **resolution** button and set its value to **12'' 1280 X 800 (16:10, 100%)**.

	![Changing the simulator resolution](Images/changing-the-simulator-resolution.png?raw=true)
	
1. Change the orientation of the Simulator by rotating it clockwise 90 degrees.

	![Rotating the simulator clockwise](Images/rotating-the-simulator-clockwise.png?raw=true)

1. Your Simulator is now adjusted. Stop the app in Visual Studio.
	
	![Simulator running](Images/simulator-running.png?raw=true)
	
1. Open the solutions **FacilityApp** and **MobileServices** in Visual Studio. Compile both solutions to ensure that all NuGet packages are downloaded. Open a new Visual Studio instance but do not open any solutions. You will start presenting using this instance.

1. In the **Source\Assets** folder of this demo you will find the following files: **apiDefinition.cs** and **StructsAndEnums.cs**. Transfer these two files to a working directory in your iOS computer. These files are used in the optional segment **Creating Your Own ADAL Binding Library**.

1. In the Mac computer, open **Terminal** and clone the ADAL for iOS library from the GitHub repository: https://github.com/MSOpenTech/azure-activedirectory-library-for-ios

<a name="Demo" />
## Demo ##

This demo is composed of the following segments:

1. [Creating a Mobile Services C# Backend](#segment1).
1. [Integrating ADAL in a Windows Store App](#segment2).
1. [Integrating ADAL and Xamarin for iOS](#segment3).
1. [Creating Your Own ADAL Binding Library (Optional)](#segment4).

<a name="segment1" />
### Creating a Mobile Services C# Backend ###

1. In Visual Studio, click **File > New > Project...**.

	![File > New > Project](Images/new-project.png?raw=true)
	
	_Creating new Project_

3. Make sure that **Visual C# > Cloud** is selected in the **Templates** list. Choose **Microsoft Azure Mobile Services** and click **OK**.

	![Microsoft Azure Mobile Services Project](Images/windows-azure-mobile-services-project.png?raw=true)
	
	_Cloud > Microsoft Azure Mobile Service project_

4. Select the **Microsoft Azure Mobile Service** template and explain how this template can be used. **Do not click OK**. Click **Cancel**.

	![New ASP.NET Project](Images/new-aspnet-project.png?raw=true)

	_New ASP.NET Project_
	
	> **Speaking Point:** Mention that any .NET language can be used to build the Mobile Service right from VS, and that the framework is built on top of ASP.NET Web API, which means we can leverage the power of NuGet and all our existing skills and code.
	
5. Switch to the **MobileService** solution.

5. Explain the contents of the project template. Open and explain each one of the following folders: 

	- **Controllers**
	- **Models**
	- **ScheduledJobs**

	![Project Template](Images/project-template.png?raw=true)
	
	_Mobile Service Project Template_

	> **Speaking Point:** We have a simple structure, with the corresponding model and the controller to expose the model to the world in a way that all our cross-platform clients understand. Also, it wouldn't be Mobile Services without great support for scheduled jobs.

7. Open **TodoItemController.cs** in the **Controllers**	folder and add a breakpoint at the beginning of the **GetAllTodoItem** method.

6. Press **F5** to run the Mobile Services back-end.

	![Mobile Service Back-End running](Images/mobile-service-back-end-running.png?raw=true)
	
	_Mobile Service Back-End running_

	> **Speaking Point:** We have support for local development. We have a documentation page with information about the API, and a test client inside the browser to try it out. Local and remote debugging now work great with Mobile Services.

1. Click the **Try it out** link.

	![Try out link](Images/try-out-link.png?raw=true)

1. Select **GET tables/TodoItem** from the list.

1. Click the **try this out** button at the top of the screen. Click **send** in the dialog box. The debugger will be hit in Visual Studio.

1. Stop the app.
	
	> **Speaking Point:** We're going to build a powerful line-of-business app, where we can report facilities issues, which the facilities department can use to find solutions.
	
7. Right-click the **DataObjects** folder, select **Add**, and click **Class** in order to add a new class. Name it _FacilityRequest.cs_ and click **Add**.

	![Add new FacilityRequest class](Images/add-new-facilityrequest-class.png?raw=true)
	
	_Adding FacilityRequest.cs class_

8. Replace the _FacilityRequest_ class in VS with the following snippet.
	
	(Code Snippet - _facilityrequest_)
	<!-- mark:1-49 -->
	````C#
	namespace MobileService.DataObjects
	{
		using System;
		using Microsoft.WindowsAzure.Mobile.Service;
		using Microsoft.WindowsAzure.Mobile.Service.Tables;
	
		public class FacilityRequest : EntityData, ITableData
		{
			public string User { get; set; }

			public RoomType RoomType { get; set; }

			public string Building { get; set; }

			public string Room { get; set; }

			public string GeoLocation { get; set; }
			
			public string Zip { get; set; }

			public string Street { get; set; }

			public string State { get; set; }

			public string City { get; set; }

			public string BTLEId { get; set; }

			public string BeforeImageUrl { get; set; }

			public string AfterImageUrl { get; set; }

			public string ProblemDescription { get; set; }

			public string ServiceNotes { get; set; }

			public string DocId { get; set; }

			public DateTimeOffset RequestedDate { get; set; }

			public DateTimeOffset CompletedDate { get; set; }
		}
	
		public enum RoomType
		{
			Office,
			Auditorium,
		}
	}
	````

	> **Speaking Point:** By default we use Entity Framework backed by a SQL Server database, but there are a number of backend choices such as MongoDB and Table Storage.

9. Right-click the **Controllers** folder, select **Add**, and click **Controller**. Select **Microsoft Azure Mobile Services Table Controller** as the Scaffold and add a new **TableController** named _FacilityRequestController_. Select **FacilityRequest** as the Model class, and **MobileServiceContext** as the Data Context class.

	![Add Controller](Images/add-controller.png?raw=true)
	
	_Adding a Controller_

	> **Speaking Point:** We have first class support for Mobile Services Table Controller right in the scaffolding dialog.

<a name="segment2" />
### Integrating with ADAL and Deploying to Microsoft Azure Mobile Services###

1. Right-click the **MobileService** project and click **Manage NuGet Packages...**.

	![Manage NuGet Packages](Images/manage-nuget-packages.png?raw=true)
	
	_Manage NuGet Packages_

2. Select the **Installed Packages** tab on the left side of the dialog box and search for **ADAL**. In the results, verify that the **Active Directory Authentication Library** is installed in the solution. Note that this is a Prerelease version. Click **Close**.

	![ADAL Library](Images/adal-library.png?raw=true)
	
	_Active Directory Authentication Library is installed_

	> **Speaking Point:** We use the ADAL (Active Directory Authentication Library) to easily provide authentication functionality for a .NET client and Windows Store application.
	
3. In the **FacilityRequestController**, paste the following highlighted code after the namespace declaration.

	(Code Snippet - _authattrib_)
	<!-- mark:3-6 -->
	````C#
	namespace MobileService.Controllers
	{
		using Microsoft.WindowsAzure.Mobile.Service.Security;
		using MobileService.Common.Providers;
		
		[AuthorizeLevel(AuthorizationLevel.User)]
		public class FacilityRequestController : TableController<FacilityRequest>
		{
			...
		}
	}	
	````

	> **Speaking Point:** Let's assume for a moment that our company has already federated our on-premise Active Directory with Azure. Adding authentication to our API is as easy as adding an attribute to our controller.
	
2. Right-click the project and select **Publish...**.

	![Publish](Images/publish.png?raw=true)
	
	_Publish Mobile Service Project to Azure_

3. In the dialog box, select **Microsoft Azure Mobile Services**. Click **Sign In** in the dialog box, and sign in with your credentials.

	![Sign In to Microsoft Azure](Images/sign-in-to-windows-azure.png?raw=true)
	
	_Sign In to Microsoft Azure_

4. From the dropdown list, select an existing Mobile Service previously created for the demo.

	![Select Microsoft Azure Mobile Service](Images/select-windows-azure-mobile-service.png?raw=true)
	
	_Select Microsoft Azure Mobile Service_
	
	> **Speaking Point:** We can use an existing service, or create a new one right from VS. Let's pick one we've already created.

5. Click **OK**. Show the different options that the wizard automatically populates. Click **Publish** to continue.

	![Publish Web Mobile Service](Images/publish-web-mobile-service.png?raw=true)
	
	_Microsoft Azure Mobile Service Settings_

	> **Speaking Point:** We will deploy this to Mobile Services, which provides a first class hosting environment for our APIs.

6. Switch to the **FacilityApp** solution in Visual Studio.

7. Expand the **Core** project and explain the multiplatform advantages of a Portable Class Library. 

	![Core Portable Class Library](Images/core-portable-class-library.png?raw=true)
	
	_Core Portable Class Library_

	> **Speaking Point:** The Portable Class Library allows us to reuse our code across a variety of client platforms.
	
8. Open **FacilityServiceBase.cs** and explain the advantages of the integration with **Mobile Services SDK**. 

	![Mobile Services SDK integration](Images/mobile-services-sdk-integration.png?raw=true)
	
	_Mobile Services SDK integration_

	> **Speaking Point:** This SDK gives us several easy access methods such as **ReadAsync** that loads all the facility requests from the server.

9. Go to the **FacilityApp.UI.Windows (Windows 8.1)** client project and open the **FacilityService.cs** file under **Services**.

	![FacilityService in W8 client](Images/facilityservice-in-w8-client.png?raw=true)
	
	_FacilityService in W8 client_

	> **Speaking Point:** What this app is still missing is support for authentication. So let's go ahead and do that.

10. Replace the content of the **LoginAsync** method with the following highlighted code snippet.

	(Code Snippet - _authclient_)
	<!-- mark:3-21 -->
	````C#
	public override async Task<string> LoginAsync(bool clearCache, string authorityId, string redirectUri, string resourceId, string clientId)
	{
	    var context = new AuthenticationContext(authorityId);
	    var result = await context.AcquireTokenAsync(resourceId, clientId);

	    // Build our token
	    var token = JObject.FromObject(new
	    {
		access_token = result.AccessToken,
	    });

	    // Request access to Azure Mobile Services
	    await MobileServiceClientProvider.MobileClient.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, token);

		var authContext = new AuthenticationContext(ConfigurationHub.ReadConfigurationValue("AadAuthority"), false);

		// Get the sharepoint token
		var authenticationResult = await authContext.AcquireTokenByRefreshTokenAsync(result.RefreshToken, ConfigurationHub.ReadConfigurationValue("AadClientID"), ConfigurationHub.ReadConfigurationValue("SharePointResource"));
		State.SharePointToken = authenticationResult.AccessToken;

	    return result.AccessToken;
	}	
	````

	> **Speaking Point:** We can take advantage of the Active Directory authentication library which gives us a native login experience on all clients. We can pass the authentication token to the Mobile Services back-end so that the user is logged in to both places.
	
11. Set the **FacilityApp.UI.Windows (Windows 8.1)** project as the startup project for the solution, make sure the **Simulator** option is selected, and launch the client app.

	![Run in Simulator](Images/run-in-simulator.png?raw=true)
	
	_Run in Simulator_

12. The Simulator will launch and start the application. When prompted, log in using your AD credentials.

	![Login with AD](Images/login-with-ad.png?raw=true)
	
	_Log in with AD_

	> **Speaking Point:** Once signed in, it's going to call out to our on-premise Active Directory and start pulling graph information about our user.
	
13. Click the **Add** button to add a new Facility Request.

	![Add a new Request](Images/add-a-new-request.png?raw=true)
	
	_Add a new Request_

14. Enter a description in the **Description of the Problem** and click **Accept**.

	![Accepting a Request](Images/accepting-a-request.png?raw=true)
	
	_Accepting a Request_

	> **Speaking Point:** Once saved, the facility request will be safely stored in the Mobile Services back-end.
	
	> **Note:** Do not close the Simulator or stop the app in Visual Studio. You will continue using it in the next segment.

<a name="segment3" />
### Integrating with SharePoint ###

We've added authentication with Active Directory, but what our app users would really like is integration with all other enterprise services, including SharePoint and Office365. For example, the facilities department might want to create a document in their SharePoint site for every request they receive. It's easy to build that for them with Mobile Services.

1. Switch to the **MobileService** backend project and open the **FacilityRequestController** class. 

	![FacilityRequestController](Images/facilityrequestcontroller.png?raw=true)
 
	_FacilityRequestController_

1. Locate the **PatchFacilityRequest** method and replace it with the following snippet.

	(Code Snippet - _sharepoint_)
	<!-- mark:1-31 -->
	````C#
	public async Task<FacilityRequest> PatchFacilityRequest(string id, Delta<FacilityRequest> patch)
	{
		var sharePointUri = SharePointProvider.SharePointUri;
		if (sharePointUri == null)
                Services.Settings.TryGetValue("SharePointUri", out sharePointUri);

		SharePointProvider.SharePointUri = sharePointUri;
		var facilityRequest = patch.GetEntity();

		sharePointUri = SharePointProvider.SharePointUri + string.Format(@"/getfolderbyserverrelativeurl('Documents')/Folders('Requests')/Files/Add(url='{0}.docx', overwrite=true)", facilityRequest.DocId);

		string authority;
		string sharePointResource;
		string activeDirectoryClientId;
		string activeDirectoryClientSecret;

		Services.Settings.TryGetValue("Authority", out authority);
		Services.Settings.TryGetValue("SharePointResource", out sharePointResource);
		Services.Settings.TryGetValue("ActiveDirectoryClientId", out activeDirectoryClientId);
		Services.Settings.TryGetValue("ActiveDirectoryClientSecret", out activeDirectoryClientSecret);

		var token = await SharePointProvider.RequestAccessToken((ServiceUser)this.User, authority, sharePointResource, activeDirectoryClientId, activeDirectoryClientSecret);

		var document = SharePointProvider.BuildDocument(facilityRequest);

		await SharePointProvider.UploadFile(sharePointUri, document, token, activeDirectoryClientId);

		return await this.UpdateAsync(id, patch);
	}	
	````

	> **Speaking Point:** The method is called every time a facility request is updated, so we can take advantage of the Active Directory authentication token to call the new set of Office365 REST APIs, allowing us to generate the document on the fly and post it straight to SharePoint.

3. Save the changes and go through publishing the **C# backend** once again.

	![Publishing Mobile Service Backend Changes](Images/publishing-mobile-service-backend-changes.png?raw=true)
	
	_Publishing Mobile Service Backend Changes_

4. Switch to the client app in the **Simulator** and select the previously created **Facility Request** item from the list.

5. Update the **Service Notes** field and click **Accept**.

	![Updating the Facility Request](Images/updating-the-facility-request.png?raw=true)
	
	_Updating the Facility Request_

	> **Speaking Point:** Once we click Accept, the request will go through Mobile Services and now call out to SharePoint and generate the document.

6. Open **SharePoint** in the browser and go to **OneDrive**. Select **My Documents** from the left panel.

	> **Speaking Point:** We can verify this by browsing to the company SharePoint site. We will find a new document generated in our documents list.

7. Open the **Requests** folder and select the **Word** document created just now.

	![Created Word document in SharePoint](Images/created-word-document-in-sharepoint.png?raw=true)
	
	_Created Word document in SharePoint_

8. Switch back to the **FacilityApp** project in **Visual Studio** and open the **Portable Class Library** properties. Focus on the **Targeting** section and explain that this class library is using Xamarin to integrate with **iOS**.

	![Multiple targets in Portable Class](Images/multiple-targets-in-portable-class.png?raw=true)
	
	_Multiple targets in Portable Class_

9. Open the **Misc** folder and show that the **iOS** project is in the same solution in **Visual Studio**.

	![iOS Project in Visual Studio](Images/ios-project-in-visual-studio.png?raw=true)
	
	_iOS Project in Visual Studio_

10. Change the **build** target from **Any CPU** to **iPhoneSimulator**. Make sure the **FacilityApp.UI.IOS** client app is selected to run and press **F5** to run the app.

	![Run as iPhoneSimulator](Images/run-as-iphonesimulator.png?raw=true)
	
	_Run as iPhoneSimulator_

11. Switch to the Mac and show the **Xamarin Build Host**. Explain the pairing feature to connect Visual Studio with iOS.

12. Wait until the Simulator is displayed. Show the app running with the same Facility Request you created using the Windows 8 client app.

<a name="segment4" />
### Creating Your Own ADAL Binding Library ###

1. Switch to the iOS 7.0 computer.

1. Open **Terminal** and browse to the folder where the ADAL source code is located. Navigate to the **ADALiOS** folder.

	> **Speaking Point:** The first thing we need to do is compile the ADAL source code we cloned from GitHub. We'll generate three files for each type of architecture. Then we'll generate a universal binary with those 3 files.

1. Execute the following command to build the library for each specific architecture.

	````Bash
	xcodebuild -project ADALiOS.xcodeproj -target ADALiOS -sdk iphonesimulator -configuration Release clean build
	````

	![Compiling ADAL iOS](Images/compiling-adal-ios.png?raw=true)
	
	_Compiling the ADAL library for iOS_
	
1. Rename the generated file (**build/Release-iphonesimulator/libADALiOS.a**) as **libADALiOS-i386.a**.

1. Execute the previous **xcodebuild** command, but use the flag _armv7_.

	````Bash
	xcodebuild -project ADALiOS.xcodeproj -target ADALiOS -sdk iphoneos -arch armv7 -configuration Release clean build
	````

1. Now rename the generated file (**build/Release-iphoneos/libADALiOS.a**) as **libADALiOS-armv7.a**.

1. Execute **xcodebuild** using the flag _armv7s_.

	````Bash
	xcodebuild -project ADALiOS.xcodeproj -target ADALiOS -sdk iphoneos -arch armv7s -configuration Release clean build
	````

1. Rename the last generated file (**build/Release-iphoneos/libADALiOS.a**) as **libADALiOS-armv7s.a**.

	> **Speaking Point:** Now that we have the 3 libraries generated, we'll merge them using the **lipo** command to create a universal binary.

1. Create a new folder called **build/Binaries** and copy the files generated and renamed in the previous steps there.


1. Change into the **build/Binaries** folder and execute the following command specifying the 3 files you generated before.

	````Bash
	lipo -create -output libADALiOS.a libADALiOS-i386.a libADALiOS-armv7.a libADALiOS-armv7s.a
	````
	
	![Creating an universal binary](Images/creating-an-universal-binary.png?raw=true)
	
	_Creating a universal binary_
	
	> **Speaking Point:** With this library we'll create the Binding project in Xamarin to link the native iOS ADAL library and use it in Visual Studio.
	
1. Open **Xamarin Studio** and click the **New...** button.

	![Creating a new project in Xamarin](Images/creating-a-new-project-in-xamarin.png?raw=true)
	
	_Creating a new project in Xamarin_

1. In the **New Solution** dialog box, select **iOS Binding Project**, name it **ADALBinding** and click **OK**.

	![Selecting iOS Binding Project Template](Images/selecting-ios-binding-project-template.png?raw=true)
	
	_Selecting iOS Binding Project Template_

1. Right-click the **ADALBinding** solution node, select **Add** and then **Add Files...**.

1. Browse to the folder where you generated the universal binary and select the file. When prompted, select **Copy the file to the directory**.

	![Adding the universal binary to Xamarin](Images/adding-the-universal-binary-to-xamarin.png?raw=true)
	
	_Adding the universal binary to Xamarin_

	> **Speaking Point:** We included our generated binary to the project. But we still need to indicate how Xamarin will expose those native methods. We have two files here - the apiDefinition and the StructsAndEnums. These files define how Xamarin invokes ADAL's native methods. We'll replace them with the ones I have here, which already define the required methods we need.
	
1. Replace the **apiDefinition.cs** and **StructsAndEnums.cs** files  with those located in your working directory for this demo.

1. Right-click the solution and select **Build ADALBinding** to generate the DLL.

	> **Speaking Point:** We can import this generated library into our solution in Visual Studio and start using it like a simple .NET library. When we compile our app for iOS, Xamarin will generate the method calls to the native library.

---

<a name="summary" />
## Summary ##

In this demo, you saw how to build a .NET Mobile Service back-end locally, publish it to the cloud, add authentication with Active Directory, integrate with SharePoint, and then build a cross-platform client with Xamarin.
