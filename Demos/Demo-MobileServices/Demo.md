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

1. Go to <https://manage.windowsazure.com/>

1. ...

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

7. Right-click the **DataObjects** folder, select **Add**, and click **Class..** in order to add a new class. Name it as _FacilityRequest.cs_ and click **Add**.

	![Add new FacilityRequest class](Images/add-new-facilityrequest-class.png?raw=true)
	
	_Adding FacilityRequest.cs class_

8. Replace the _FacilityRequest_ class in VS with the following snippet.
	<!-- mark:1-36 -->
	````C#
	using Microsoft.WindowsAzure.Mobile.Service;
	
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
	
	public enum RoomType
	{
		Office,
		Auditorium,
	}
	````

	> **Speaking Point:** By default we use Entity Framework backed by a SQL Server database, but there's a number of backend choices such as MongoDB and Table Storage.

9. Right-click the **Controllers** folder, select **Add**, and click **Controller**. Select **Windows Azure Mobile Services Table Controller** as the Scaffold and add a new **TableController** named _FacilityRequestController_. Select **FacilityRequest** as the Model class, and **MobileServiceContext** as the Data Context class.

	![Add Controller](Images/add-controller.png?raw=true)
	
	_Adding a Controller_

	> **Speaking Point:** We have first class support for Mobile Services Table Controller right in the scaffolding dialog.

<a name="segment2" />
### Integrating with ADAL ###

1. Right-click the MobileService project and click **Manage NuGet Packages**.

	![Manage NuGet Packages](Images/manage-nuget-packages.png?raw=true)
	
	_Manage NuGet Packages_

2. Select the **Online** tab at the left of the dialog, be sure to choose **Include Prerelease** in the combo box at the top, and search for **ADAL**. In the results click install for the **Active Directory Authentication Library**. Click **I Accept** for the License dialog.

	![Adding Active Directory Authentication Library](Images/adding-active-directory-authentication-librar.png?raw=true)
	
	_Adding Active Directory Authentication Library_

3. In the **FacilityRequestController** paste the following highlighted code after the namespace declaration.

	(Code Snippet - _authattrib_)

	<!-- mark:3-5 -->
	````C#
	namespace MobileService.Controllers
	{
		using Microsoft.WindowsAzure.Mobile.Service.Security;
		
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