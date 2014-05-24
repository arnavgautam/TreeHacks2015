<a name="title" />
# Mobile Services Demo #

---

<a name="Overview" />
## Overview ##

Since Mobile Services was launched, you have seen some strong adoption and some great apps built on top of the platform, both across the consumer and enterprise space. Mobile Services lets you easily add a cloud-hosted back end to your mobile app, regardless of what client platform you are using. 
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
- [Windows Azure Mobile Services][6]
- [Active Directory Authentication Library][4] (ADAL)
- [Xamarin Studio for iOS][5]
	
[2]: http://www.microsoft.com/visualstudio/
[3]: http://www.microsoft.com/en-us/download/details.aspx?id=42666
[4]: http://msdn.microsoft.com/en-us/library/jj573266.aspx
[5]: http://xamarin.com/
[6]: http://azure.microsoft.com/en-us/develop/mobile/

<a name="setup" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment. The following are one-time instructions you need to execute in order to prepare the demo. Once completed, there is no need to execute these steps again, you can simply run **Reset.cmd** located in the **Setup** folder to clear the database and SharePoint files to restart the demo.

#### Task 1 - Creating an Office 365 subscription ####

If you do not have an Office 365 subscription you can do one of the following:

- If you are a **Microsoft Partner**, you have the option of requesting a demo tenant here https://www.microsoftofficedemos.com. These demo tenants expire in 90 days after the creation.

- If you have a **MSDN subscription** you can activate your **Office 365 Developer** subscription in your MSDN subscriber dashboard.

- Buy a subscription. Go to http://office.microsoft.com/en-us/business/compare-office-365-for-business-plans-FX102918419.aspx and pick the option of your choice (e.g. Office 365 Small Business).

Once you finish signing up for your **Office 365** subscription, follow these steps:

1. Close the browser to clear out the authentication. Open a browser again and go to https://portal.microsoftonline.com/default.aspx.

	> **Note:** This will open the Office 365 Management Portal. It might take a few minutes until all services are provisioned.

1. Click the **Users and groups** option in the left navigation menu.

1. Click at your user name and verify that it's linked to the subscription accordingly.

	![Office Subscription Linked](Images/office-subscription-linked.png?raw=true)
	
1. Go to https://{username}-my.sharepoint.com replacing the placeholder with the username you defined before the **.onmicrosoft.com** domain (e.g.: https://myuser-my.sharepoint.com/).

1. This site is provisioned upon the first time you actually launch it on the browser. It should take a few minutes to provision. Your site should look like the following:

	![SharePoint My Site](Images/sharepoint-my-site.png?raw=true)
	
#### Task 2 - Setting Azure + Office 365 Subscription ####

> **Note:** If you have a MSDN subscription you get a free Azure subscription with it. Go to the msdn.com subscription dashboard and follow the **Activate Windows Azure** option.

1. Open a browser and go to http://manage.windowsazure.com.

1. When prompted, use your Office 365 credentials.

1. You will be presented with a screen asking you to get a subscription before you can start using Azure. Click the **Sign Up for Windows Azure** link.

	![Sign up for Azure](Images/sign-up-for-azure.png?raw=true)
	
1. You will be taken to a screen where you will validate your details, agree with the Terms and Privacy statement and then finish signing up. Once the registration is over, you will be able to access the Management Portal. Now Azure and Office 365 are both linked to the same user account.

#### Task 3 - Creating a Mobile Service and Registering your Apps in Azure AD ####

1. In the [Management Portal](http://manage.windowsazure.com/) create a new Mobile Service. You can choose between a new Free Database and an existing one. Make sure the **.NET (PREVIEW)** option is selected for the **Backend** drop-down list.

	![Creating a Mobile Service](Images/creating-a-mobile-service.png?raw=true)
	
1. Once created, click the Mobile Service and go to **Identity**.

1. Scroll down to the **Azure Active Directory** identity provider section and copy the **APP URL** listed there.

1. Go to **Active Directory**.

1. Select your directory from the list and go to **Applications**.

1. Click **Add** and select **Add an application my organization is developing**.

1. Type a name, for example _mymobileservice_, and select **Web Application and/or Web API**. Click next to continue.

	![Creating a Web Application in AD](Images/creating-a-web-application-in-ad.png?raw=true)

1. Paste the Mobile Service URL in the **SIGN-ON URL** and **APP ID URI** field. Click OK to create the app.

	![Configuring App Properties](Images/configuring-app-properties.png?raw=true)
	
1. Click **Manage Manifest** from the menu and select **Download Manifest**.

	![Download Manifest](Images/download-manifest.png?raw=true)
	
1. Open the application manifest file with **Visual Studio**. At the top of the file find the app permissions line that looks as follows:

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

1. Open **Configure** and scroll down to the **Client ID** section. Take note of the Client ID, you will use it later to update a configuration value named **AadClientId**.

1. Scroll down to the **permissions to other applications** section and grant permissions to **Office 365 SharePoint Online**. Select **Edit or delete users' files** from the **Delegated Permissions** drop-down list.

	![Permissions for the Mobile Service App](Images/permissions-for-the-mobile-service-app.png?raw=true)

1. In the **Keys** section choose a duration from the drop-down list to create a new key. Click **Save**.

1. Take note the generated key value, you will use it later.

#### Task 4 - Associate your Client App to the Windows Store ####
	
1. Open the **Begin\FacilityApp.sln** solution located under the **Source** folder in Visual Studio.

1. Right-click the **FacilityApp.UI.Windows** project and select **Associate App with the Store...** under **Store** sub-menu.

1. Sign into your **Dev Center** account.

1. Enter the app name you want to reserve and click **Reserve**.

1. Select the new app name and click **Next**.

1. Click **Associate** to associate the app with the store name.

1. Log into you [Windows Dev Center Dashboard](http://go.microsoft.com/fwlink/p/?linkid=266734&clcid=0x409) and click **Edit** on the app.

1. Then click **Services**.

1. Then click **Live Services Site**.

1. Copy your package **SID** from the top of the page.

1. Switch to the **Management Portal** and go to your AD.

1. Go to **Applications** and click **Add**. Select **Add an application my organization is developing**.

1. Type a name for the client app (e.g.: _facilityappclient_) and select **Native Client Application**. Click next to continue.

	![Creating clent AD app](Images/creating-clent-ad-app.png?raw=true)
	
1. In the **Redirect URI** field, paste the package **SID** you copied in a previous step. Click OK to continue.

	![Client App Package SID](Images/client-app-package-sid.png?raw=true)

1. Click the **Configure** tab for the native application and take note of the **Client ID**.

	![Copying the Client ID](Images/copying-the-client-id.png?raw=true)

1. In the **Redirect URIs** section add the Mobile Services URL. E.g.: https://{mobileservice-name}.azure-mobile.net/.
	
1. Scroll down to the **permissions to other applications** section and grant full access to the mobile service application that you registered earlier. Click **Save**

	![Granting permissions to the Client App](Images/granting-permissions-to-the-client-app.png?raw=true)
	
#### Task 5 - Setting up Configuration Variables ####

1. In the Management Portal, open your Mobile Service and go to **Configure**.

1. Scroll down to the **app settings** section.

1. Add the following settings (take into account that the name of each setting is case sensitive):

	* **SharePointUri**: The SharePoint user's personal site targeting the API address. The URL usually has the following form **https://{domain}-my.sharepoint.com/personal/{username}_{domain}_onmicrosoft_com/_api/web**. For example: https://dpe-my.sharepoint.com/personal/admin_dpe_onmicrosoft_com/_api/web.
	* **SharePointResource**: The base URL of SharePoint's Personal sites collection. E.g.: https://{domain}-my.sharepoint.com
	* **Authority**: The Azure AD authority. Use https://login.windows.net/common/oauth2/authorize.
	* **ActiveDirectoryClientId**: The Id of the Mobile Service application registered in the Azure AD.
	* **ActiveDirectoryClientSecret**: The secret of the Mobile Service application registered in the Azure AD.

1. Click **Save**.

1. Browse to the **Setup** folder of this demo and open the file **Config.xml**.

1. The starting solutions will be copied to the **C:\Demos\Source** folder. If you want to change the default directory, update the element **solutionWorkingDir** in **localPaths**.

1. Update the values under **clientSettings** in the XML file to configure your solutions:

	* **AadAuthority**: The Azure AD authority. Use https://login.windows.net/common/oauth2/authorize.
	* **AppRedirectLocation**: The Mobile Service URI.
	* **AadRedirectResourceURI**: The Mobile Service AAD login URI. You can find this value under **Azure Active Directory** in your Mobile Service's **Identity** tab.
	* **AadClientId**: The Id of your native client app registered in your AD.
	* **AppKey**: The Mobile Service key. You can retrieve this value by clicking **Manage Keys** in your Mobile Service.
	* **MobSvcUri**: The Mobile Service URI.
	* **SharePointResource**: It is the root URL for the personal sites of your SharePoint domain. E.g.: http://{domain}-my.sharepoint.com/
	* **SharePointUser**: Full qualified name for the Office 365 user. E.g.: admin@dpe.onmicrosoft.com.
	
1. The following values are displayed on the Windows Store app. These settings configure the Username and the default location of the device, simulating Geolocation inside the app. You can replace them with a real location (e.g.: the location where the demo will be presented).	
	
	* **UserName**: The first name of the User that is displayed on the Windows Store app.
	* **UserSurname**: The last name of the User that is displayed on the Windows Store app.
	* **BuildingFRVM**: The building name.
	* **RoomFRVM**: The room number.
	* **CityFRVM**: The city name.
	* **StreetFRVM**: The street name.
	* **StateFRVM**: The state where the city is located.
	* **ZipFRVM**: The zip code.

1. Under **windowsAzureSubscription**, update the values of the Mobile Service SQL Server (you can find these values in your Mobile Service configuration):

	* **sqlserver**: The SQL Server address. E.g.: {server}.database.windows.net.
	* **db**: The database name.
	* **sqlUsername**: The server administrator username.
	* **sqlPassword**: The server administrator password.
	* **sqlTable**: The database table including the schema name. Use the following format: **{mobileservice-name}.facilityrequests**.

1. Under the **sharepoint** element set the values to connect your SharePoint:

	* **baseUrl**: The SharePoint user's personal site. The URL usually has the following form **https://{domain}-my.sharepoint.com/personal/{username}_{domain}_onmicrosoft_com/**. For example: https://dpe-my.sharepoint.com/personal/admin_dpe_onmicrosoft_com/.
	* **username**: Full qualified name for the Office 365 user. E.g.: admin@dpe.onmicrosoft.com.
	* **password**: The password for the Office 365 user.
	* **folderName**: The folder in the personal's site documents where the app will upload files. Leave the default value **Requests**.

1. Save and close the file.

1. Run **Reset.cmd** in the **Setup** folder to execute the reset scripts. These scripts configure the settings files for each client app, removes any record in the Mobile Service SQL database and deletes all the files in the **Requests** folder in SharePoint. Remember to add a firewall rule for your machine to access the SQL database in Azure.

	> **Note:** You can execute **Reset.cmd** any time you need to reset the demo. As you already configured Azure AD and the Mobile Service you only need to execute the reset scripts to reset the demo to a starting point.

#### Task 6 - First Run

Follow these steps to run the **FacilityRequests** app to adjust the correct Simulator's resolution display and orientation.

1. Open the **FacilityApp.sln** solution located in your demo folder (by default **C:\Demos\Source**).

1. The **Xamarin Pair With** screen will pop up requesting a PIN.

	![Xamarin Pair With screen](Images/xamarin-pair-with-screen.png?raw=true)
	
1. Go to the **MAC OS**, type **Xamarin** in the **Spotlight** control and open **Xamarin.iOS Build Host** application.

	![Xamarin.iOS Build Host app](Images/xamarinios-build-host-app.png?raw=true)
	
1. Click **Pair** and take note of the generated **PIN** code.

	![Xamarin.iOS Build Host pin code](Images/xamarinios-build-host-pin-code.png?raw=true)

1. Go back to Visual studio, write the PIN code in the **Xamarin Pair With** screen and click **Pair**.

	![Pairing with Xamarin in Visual Studio](Images/pairing-with-xamarin-in-visual-studio.png?raw=true)

1. A succesfull pairing message pops up. Click **Finish**.

1. Set **FacilityApp.UI.Windows** as the startup project and run the app using the Simulator.

1. Click on the **resolution** button and set its value to **12'' 1280 X 800 (16:10, 100%)**.

	![Changing the simulator resolution](Images/changing-the-simulator-resolution.png?raw=true)
	
1. Change the orientation of the Simulator by rotating it clockwise 90 degreess.

	![Rotating the simulator clockwise](Images/rotating-the-simulator-clockwise.png?raw=true)

1. Your Simulator is now adjusted. Stop the app in Visual Studio.
	
	![Simulator running](Images/simulator-running.png?raw=true)
	
1. Open the solutions **FacilityApp** and **MobileServices** in Visual Studio. Compile both solutions to ensure that all NuGet packages are downloaded. Open a new Visual Studio instance but do not open any solutions. You will start presenting using this instance.
	
<a name="Demo" />
## Demo ##

This demo is composed of the following segments:

1. [Creating a Mobile Services C# Backend](#segment1).
1. [Integrating ADAL in a Windows Store App](#segment2).
1. [Integrating ADAL and Xamarin for iOS](#segment3).

<a name="segment1" />
### Creating a Mobile Services C# Backend ###

1. In Visual Studio, click on **File**, hover on the **New** menu, and click **New Project**.

	![File > New > Project](Images/new-project.png?raw=true)
	
	_Creating new Project_

3. Make sure that **Visual C# > Cloud** is selected in the **Templates** list. Choose **Windows Azure Mobile Services** and click **OK**.

	![Windows Azure Mobile Services Project](Images/windows-azure-mobile-services-project.png?raw=true)
	
	_Cloud > Windows Azure Mobile Service project_

4. Select the **Windows Azure Mobile Service** template and explain how this template can be used. **Do not click OK**. Click **Cancel**.

	![New ASP.NET Project](Images/new-aspnet-project.png?raw=true)

	_New ASP.NET Project_
	
	> **Speaking Point:** Mention any .NET language can be used to build the Mobile Service right from VS, and the framework is built on top of ASP.NET Web API, which means we get leverage the power of NuGet and all our existing skills and code.
	
5. Switch to the **MobileService** solution.

5. Explain the contents of the project template. Open and explain each one of the following folders: 

	- **Controllers**
	- **Models**
	- **ScheduledJobs**

	![Project Template](Images/project-template.png?raw=true)
	
	_Mobile Service Project Template_

	> **Speaking Point:** We have a simple structure, with the corresponding model and the controller to expose that model to the world in a way that all our cross-platform clients understand. Also, it wouldn't be Mobile Services without great support for scheduled jobs.

7. Open **TodoItemController.cs** in the **Controllers**	folder and add a breakpoint at the beginning of the **GetAllTodoItem** method.

6. Press **F5** to run the Mobile Services back-end.

	![Mobile Service Back-End running](Images/mobile-service-back-end-running.png?raw=true)
	
	_Mobile Service Back-End running_

	> **Speaking Point:** We have support for local development. We have a documentation page with information about the API, and a test client inside the browser to try it out. Local and remote debugging now work great with Mobile Services.

1. Click the **Try out** link.

	![Try out link](Images/try-out-link.png?raw=true)

1. Select **GET tables/TodoItem** from the list.

1. Click the **try this out** button at the top of the screen. Click **send** on the dialog box. The debugger will be hit in Visual Studio.

1. Stop the app.
	
	> **Speaking Point:** We're going to build a powerful line of business app, where we can report facilities issues, and then the facilities department can use it to take care of it.
	
7. Right-click the **DataObjects** folder, select **Add**, and click **Class..** in order to add a new class. Name it as _FacilityRequest.cs_ and click **Add**.

	![Add new FacilityRequest class](Images/add-new-facilityrequest-class.png?raw=true)
	
	_Adding FacilityRequest.cs class_

8. Replace the _FacilityRequest_ class in VS with the following snippet.

	
	(Code Snippet - _facilityrequest_)
	<!-- mark:1-41 -->
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

	> **Speaking Point:** By default we use Entity Framework backed by a SQL Server database, but there's a number of backend choices such as MongoDB and Table Storage.

9. Right-click the **Controllers** folder, select **Add**, and click **Controller**. Select **Windows Azure Mobile Services Table Controller** as the Scaffold and add a new **TableController** named _FacilityRequestController_. Select **FacilityRequest** as the Model class, and **MobileServiceContext** as the Data Context class.

	![Add Controller](Images/add-controller.png?raw=true)
	
	_Adding a Controller_

	> **Speaking Point:** We have first class support for Mobile Services Table Controller right in the scaffolding dialog.

<a name="segment2" />
### Integrating with ADAL and Deploying to Windows Azure Mobile Services###

1. Right-click the **MobileService** project and click **Manage NuGet Packages**.

	![Manage NuGet Packages](Images/manage-nuget-packages.png?raw=true)
	
	_Manage NuGet Packages_

2. Select the **Installed Packages** tab at the left of the dialog and search for **ADAL**. In the results verify the **Active Directory Authentication Library** is installed in the solution. Note this is a Prerelease version. Click **Close**.

	![ADAL Library](Images/adal-library.png?raw=true)
	
	_Active Directory Authentication Library is installed_

	> **Speaking Point:** We use the ADAL (Active Directory Authentication Library) to easily provide authentication functionality for a .NET client and Windows Store application.
	
3. In the **FacilityRequestController** paste the following highlighted code after the namespace declaration.

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
	
2. Right-click the project and select **Publish**.

	![Publish](Images/publish.png?raw=true)
	
	_Publish Mobile Service Project to Azure_

3. In the dialog box, select **Mobile Services**, click **Sign In** in the dialog box, and sign in with your credentials.

	![Sign In to Windows Azure](Images/sign-in-to-windows-azure.png?raw=true)
	
	_Sign In to Windows Azure_

4. From the dropdown list, select an existing Mobile Service, previously created for the demo.

	![Select Windows Azure Mobile Service](Images/select-windows-azure-mobile-service.png?raw=true)
	
	_Select Windows Azure Mobile Service_
	
	> **Speaking Point:** We can use an existing service, or create a new one right from VS. Let's pick one we created previously.

5. Click **OK**. Show the different options that the wizard automatically populates. Click **Publish** to continue.

	![Publish Web Mobile Service](Images/publish-web-mobile-service.png?raw=true)
	
	_Windows Azure Mobile Service Settings_

	> **Speaking Point:** We deploy this to Mobile Services, which provides a first class hosting environment for our APIs.

6. Switch to the **FacilityApp** solution in Visual Studio.

7. Expand the **Core** project and explain the multiplatform advantages of a Portable Class Library. 

	![Core Portable Class Library](Images/core-portable-class-library.png?raw=true)
	
	_Core Portable Class Library_

	> **Speaking Point:** The Portable Class Library allows us to reuse our code across a variety of client platforms.
	
8. Open **FacilityServiceBase.cs** and explain the advantages of the integration with the **Mobile Services SDK**. 

	![Mobile Services SDK integration](Images/mobile-services-sdk-integration.png?raw=true)
	
	_Mobile Services SDK integration_

	> **Speaking Point:** This SDK gives us some easy access methods such as **ReadAsync** that loads up all the facility requests from the server.

9. Go to the Windows 8.1 client project and open the **FacilityService.cs** file under **Services**.

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

	> **Speaking Point:** We can take advantage of the Active Directory authentication library, which gives us a native login experience on all clients. We can pass the authentication token to the Mobile Services back-end, so the user is logged in to both places.
	
11. Set the Windows 8.1 project as the startup project for the solution, make sure the **Simulator** option is selected, and launch the client app.

	![Run in Simulator](Images/run-in-simulator.png?raw=true)
	
	_Run in Simulator_

12. The Simulator will launch and start the application. When prompted, login using your AD credentials.

	![Login with AD](Images/login-with-ad.png?raw=true)
	
	_Login with AD_

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

We've added authentication with Active Directory, but what our app users would really want is integration with all other enterprise services, including SharePoint and Office365. For example, the facilities department might want to create a document in their SharePoint site for every request they receive. It's easy to build that for them with Mobile Services.

1. Switch to the **MobileService** backend project and open the **FacilityRequestController** class. 

![FacilityRequestController](Images/facilityrequestcontroller.png?raw=true)
	
	_FacilityRequestController_

2. Locate the **PatchFacilityRequest** method and replace it with the following snippet.

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

		string headerUri;
		Services.Settings.TryGetValue("HeaderUri", out headerUri);
		var document = SharePointProvider.BuildDocument(facilityRequest, headerUri);

		await SharePointProvider.UploadFile(sharePointUri, document, token, activeDirectoryClientId);

		return await this.UpdateAsync(id, patch);
	}	
	````

	> **Speaking Point:** The method is called every time a facility request is updated, so we can take advantage of the Active Directory authentication token to call to the new set of Office365 REST APIs, which allows us to generate the document on-the-fly and post it straight to SharePoint.

3. Save changes and go through publishing the **C# backend** once again.

	![Publishing Mobile Service Backend Changes](Images/publishing-mobile-service-backend-changes.png?raw=true)
	
	_Publishing Mobile Service Backend Changes_

4. Switch to client app in the **Simulator** and select the previously created **Facility Request** item from the list.

5. Update the **Service Notes** field and click **Accept**.

	![Updating the Facility Request](Images/updating-the-facility-request.png?raw=true)
	
	_Updating the Facility Request_

	> **Speaking Point:** Once we press Accept, the request will go through Mobile Services and now call out to SharePoint and generate the document.

6. Open **SharePoint** in the browser and go to **OneDrive**. Select **My Documents** from the left panel.

	> **Speaking Point:** We can verify it browsing to the company SharePoint site, we can find a new generated document with our company identity.

7. Open the **Requests** folder and select the **Word** document created a few seconds ago.

	![Created Word document in SharePoint](Images/created-word-document-in-sharepoint.png?raw=true)
	
	_Created Word document in SharePoint_

8. Switch back to **Visual Studio** and open the **Portable Class Library** properties. Make focus on the **Targeting** section and explain that this class library is using Xamarin to integrate with **iOS**.

	![Multiple targets in Portable Class](Images/multiple-targets-in-portable-class.png?raw=true)
	
	_Multiple targets in Portable Class_

9. Open the **Misc** folder and show that the **iOS** project is in the same solution in **Visual Studio**.

	![iOS Project in Visual Studio](Images/ios-project-in-visual-studio.png?raw=true)
	
	_iOS Project in Visual Studio_

10. Change the **build** target from **Any CPU** to **iPhoneSimulator**. Make sure the iOS client app is selected for running and press **F5** to run the app.

	![Run as iPhoneSimulator](Images/run-as-iphonesimulator.png?raw=true)
	
	_Run as iPhoneSimulator_

11. Switch to the Mac and show the **Xamarin Build Host**. Explain the pairing feature to connect Visual Studio with iOS.

12. Wait until the Simulator is displayed. Show the app running with the same Facility Request you created using the Windows 8 client app.

---

<a name="summary" />
## Summary ##

In this demo, you saw building a .NET Mobile Service back-end locally, publishing it to the cloud, adding authentication with Active Directory, integrating with SharePoint, and then building a cross-platform client with Xamarin.
