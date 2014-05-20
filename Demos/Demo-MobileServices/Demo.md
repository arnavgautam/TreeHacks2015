<a name="title" />
# Mobile Services #

---

<a name="Overview" />
## Overview ##

This demo is designed to...

<a id="goals" />
### Goals ###
In this demo, you will see how to:

1. Create & Deploy a Mobile Services C# Backend
1. Integrate ADAL in a Windows Store app
1. Showcase Xamarin for iOS and its integration with ADAL

<a name="technologies" />
### Key Technologies ###

- Azure subscription- you can sign up for free trial [here][1]
- [Microsoft Visual Studio 2013][2]

[1]: http://bit.ly/WindowsAzureFreeTrial
[2]: http://www.microsoft.com/visualstudio/


<a name="setup" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment.

#### Creating an Office 365 subscription ####

If you do not have an Office 365 subscription you can do one of the following:

- If you are a **Microsoft Partner**, you have the option of requesting a demo tenant here https://www.microsoftofficedemos.com. These demo tenants expire in 90 days after the creation.

- If you have a **MSDN subscription** you can activate your **Office 365 Developer** subscription in your MSDN subscriber dashboard.

- Buy a subscription. Go to http://office.microsoft.com/en-us/business/compare-office-365-for-business-plans-FX102918419.aspx and pick the option of your choice (e.g. Office 365 Small Business).

Once you finish signin up for you Office 365 subscription, follow these steps:

1. Close the browser to clear out the authentication. Open a browser again and go to https://portal.microsoftonline.com/default.aspx.

	> **Note:** This will open the Office 365 Management Portal. It might take a few minutes until all services are provisioned.

1. Click the **Users and groups** option in the left navigation menu.

1. Click at your user name and verify that it's linked to the subscription accordingly.

	![Office Subscription Linked](Images/office-subscription-linked.png?raw=true)
	
1. Go to https://{username}-my.sharepoint.com replacing the placeholder with the username you defined before the **.onmicrosoft.com** domain (e.g.: https://myuser-my.sharepoint.com/).

1. This site is provisioned upon the first time you actually launch it on the browser. It should take a few minutes to provision. Your site should look like the following:

	![SharePoint My Site](Images/sharepoint-my-site.png?raw=true)
	
#### Setting Azure + Office 365 Subscription ####

> **Note:** If you have a MSDN subscription you get a free Azure subscription with it. Go to the msdn.com subscription dashboard and follow the **Activate Windows Azure** option.

1. Open a browser and go to http://manage.windowsazure.com.

1. When prompted, use your Office 365 credentials.

1. You will be presented with a screen asking you to get a subscription before you can start using Azure. Click the **Sign Up for Windows Azure** link.

	![Sign up for Azure](Images/sign-up-for-azure.png?raw=true)
	
1. You will be taken to a screen where you will validate your details, agree with the Terms and Privacy statement and then finish signing up. Once the registration is over, you will be able to access the Management Portal. Now Azure and Office 365 are both linked to the same user account.

#### Creating a Mobile Service and Registering your Apps in Azure AD ####

This demo requires two applications in your Azure AD: One for the Mobile Service and another for the Client App.

1. In the [Management Portal](http://manage.windowsazure.com/) create a new Mobile Service. You can select a new Free Database or choose an existing one.

	![Creating a Mobile Service](Images/creating-a-mobile-service.png?raw=true)
	
1. Once created, click the Mobile Service and go to **Identity**.

1. Scroll down to the **Azure Active Directory** identity provider section and copy the **APP URL** listed there.

1. Go to **Active Directoy**.

1. Select your **Default Directory** from the list and go to **Applications**.

1. Click **Add** and select **Add an application my organization is developing**.

1. Type a name, for example _mymobileservice_, and select **Web Application and/or Web API**. Click next to continue.

	![Creating a Web Application in AD](Images/creating-a-web-application-in-ad.png?raw=true)

1. Paste the Mobile Service URL in the **SIGN-ON URL** and **APP ID URI** field. Click ok to create the app.

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

#### Associate your Client App to the Windows Store ####
	
1. Open the **FacilityApp.sln** solution in Visual Studio.

1. Right-click the **FacilityApp.UI.Windows** project and select **Associate App with the Store...**.

1. Sign into your **Dev Center** account.

1. Enter the app name you want to reserve and click **Reserve**.

1. Select the new app name and click **Next**.

1. Click **Associate** to associate the app with the store name.

1. Log into you [Windows Dev Center Dashboard](http://go.microsoft.com/fwlink/p/?linkid=266734&clcid=0x409) and click **Edit** on the app.

1. Then click **Services**.

1. Then click **Live Services Site**.

1. Copy your package SID from the top of the page.

<a name="Demo" />
## Demo ##

This demo is composed of the following segments:

1. [Creating and Deploying a Mobile Services C# Backend](#segment1).
1. [Integrating ADAL in a Windows Store App](#segment2).
1. [Integrating ADAL and Xamarin for iOS](#segment3).

<a name="segment1" />
### Creating and Deploying a Mobile Services C# Backend ###

1. Go to **Start** and open **Visual Studio 2013**.

2. Click on **File**, hover on the **New** menu, and click **New Project**.

	![File > New > Project](Images/new-project.png?raw=true)
	
	_Creating new Project_

3. Make sure that **Visual C# > Cloud** is selected in the **Templates** list. Choose **Windows Azure Mobile Services**, type **MobileService** as the name for the project, and click **OK**.

	![Windows Azure Mobile Services Project](Images/windows-azure-mobile-services-project.png?raw=true)
	
	_Cloud > Windows Azure Mobile Service project_

4. Select the **Windows Azure Mobile Service** template and click **OK**.

	![New ASP.NET Project](Images/new-aspnet-project.png?raw=true)
	
	_New ASP.NET Project_
	
	> **Speaking Point:** Mention any .NET language can be used to build the Mobile Service right from VS, and the framework is built on top of ASP.NET Web API, which means we get leverage the power of NuGet and all our existing skills and code.

5. Explain the contents of the project template. Open and explain each one of the following folders: 

	- **Controllers**
	- **Models**
	- **ScheduledJobs**

	![Project Template](Images/project-template.png?raw=true)
	
	_Mobile Service Project Template_

	> **Speaking Point:** We have a simple structure, with the corresponding model and the controller to expose that model to the world in a way that all our cross-platform clients understand. Also, it wouldn't be Mobile Services without great support for scheduled jobs.
	
6. Press **F5** to run the Mobile Services back-end.

	![Mobile Service Back-End running](Images/mobile-service-back-end-running.png?raw=true)
	
	_Mobile Service Back-End running_

	> **Speaking Point:** We have support for local development. We have a documentation page with information about the API, and a test client inside the browser to try it out. Local and remote debugging now work great with Mobile Services.
	We're going to build a powerful line of business app, where we can report facilities issues, and then the facilities department can use it to take care of it.
	
7. Right-click the **MobileService** project, select **Add** and click **New Folder**. Type **Common** as the name for the new folder and press Enter.

8. Right-click the **Common** folder, select **Add**, and click **Class..**. Type **Enum.cs** as the name for the class and click **Add**.

	![Adding Enum.cs class](Images/adding-enumcs-class.png?raw=true)
	
	_Adding Enum.cs class_
	
9. Replace the **Enum** class in the **Enum.cs** with the following snippet.
	<!-- mark:1-5 -->
	````C#
	public enum RoomType
	{
		Office,
		Auditorium,
	}
	````

10. Right-click the **DataObjects** folder, select **Add**, and click **Class..** in order to add a new class. Name it as _FacilityRequest.cs_ and click **Add**.

	![Add new FacilityRequest class](Images/add-new-facilityrequest-class.png?raw=true)
	
	_Adding FacilityRequest.cs class_

11. Replace the _FacilityService_ class in VS with the following snippet.
	<!-- mark:1-28 -->
	````C#
	using Common;
	
	public class FacilityRequest : EntityData
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
	````

	> **Speaking Point:** By default we use Entity Framework backed by a SQL Server database, but there's a number of backend choices such as MongoDB and Table Storage.

12. Right-click the **Controllers** folder, select **Add**, and click **Controller**. Select **Windows Azure Mobile Services Table Controller** as the Scaffold and add a new **TableController** named _FacilityRequestController_. Select **FacilityRequest** as the Model class, and **MobileServiceContext** as the Data Context class.

	![Add Controller](Images/add-controller.png?raw=true)
	
	_Adding a Controller_

	> **Speaking Point:** We have first class support for Mobile Services Table Controller right in the scaffolding dialog.

<a name="segment2" />
### Integrating with ADAL ###

1. In the **FacilityRequestController** paste the following highlighted code after the namespace declaration.

	(Code Snippet - _authattrib_)

	<!-- mark:3-5 -->
	````C#
	namespace MobileService.Controllers
	{
		using Microsoft.WindowsAzure.Mobile.Services.Security;
		
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
	
8. Open **FacilityServiceBase.cs** and explain the advatages of the integration with the **Mobile Services SDK**. 

	![Mobile Services SDK integration](Images/mobile-services-sdk-integration.png?raw=true)
	
	_Mobile Services SDK integration_

	> **Speaking Point:** This SDK gives us some easy access methods such as **ReadAsync** that loads up all the facility requests from the server.

9. Go to the Windows 8.1 client project and open the **FacilityService.cs** file under **Services**.

	![FacilityService in W8 client](Images/facilityservice-in-w8-client.png?raw=true)
	
	_FacilityService in W8 client_

	> **Speaking Point:** What this app is still missing is support for authentication. So let's go ahead and do that.

10. Replace the code inside the **LoginAsync** method with the following highlighted code.

	(Code Snippet - _authclient_)
	<!-- mark:3-14 -->
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

2. Locate the **PatchFacilityRequest** method and paste the following highlighted code replacing the one inside that method.

	(Code Snippet - _sharepoint_)
	<!-- mark:3-31 -->
	````C#
	public async Task<FacilityRequest> PatchFacilityRequest(string id, Delta<FacilityRequest> patch)
	{
		if (SharePointProvider.SharePointUri == null)
			Services.Settings.TryGetValue("SharePointUri", out SharePointProvider.SharePointUri);

            var facilityRequest = patch.GetEntity();

            var sharepointUri = SharePointProvider.SharePointUri + string.Format(@"/getfolderbyserverrelativeurl('Documents')/Folders('Requests')/Files/Add(url='{0}.docx', overwrite=true)",
                                    facilityRequest.DocId);

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

            await SharePointProvider.UploadFile(sharepointUri, document, token, activeDirectoryClientId);

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
