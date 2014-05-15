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

1. Go to **Visual Studio**.

1. Go to **File > New Project**.

1. Make sure that **C# > Cloud** is selected. Choose **Windows Azure Mobile Services** and click **OK**.

1. Explain the contents of the project template. Open and explain the following folders: 

	- **ScheduledJobs**
	- **Models**
	- **Controllers**

1. Right-click the project and select **Publish**.

1. In the dialog box, select **Mobile Services**.

1. From the dropdown list, select an existing Mobile Services.

1. Click OK. Show the different options that automatically the wizard populates. **DO NOT PUBLISH**. Click **Close** to continue.

1. Switch to the **FacilityApp** Mobile Service backend.

1. Open the **DataObjects** folder and add a new class named _FacilityRequest_.

1. Paste the snippet **snippet1**.

1. Open the **Controllers** folder and add a new **TableController** named _FacilityRequestController_.




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

Paste the snippet **snippet2**.

1. Add the attribute to authenticate with ADAL.

1. Switch to the FacilityApp solution in Visual Studio.

1. Show the Core portable class library.

1. Show FacilityServiceBase.cs, and the methods.

1. Go to Win8 client and open FacilityService.

1. Replace LoginAsync code with code snippet.

1. Launch app in the Simulator.

1. Login with the username.

1. Click Add. Add a new Facility Request.

1. Type a description.

1. Click Accept.

<a name="segment3" />
### Integrating with SharePoint ###

1. Go to the Service Project. 

1. Go to PatchFacilityRequest method, and update pasting the snippet.

1. Explain SharePoint APIs.

1. Save changes and re-publish the C# backend. Wait until its deployed

1. Switch to the app in the Simulator.

1. Select the previously created Request.

1. Update the Service Notes and click Accept.

1. Open SharePoint in the browser. Go to My Documents.

1. Open the folder Requests and selected the Word document.

1. Open the Portable Class Library properties and show the Targeting.

1. Change the target to iPhoneSimulator.

1. Switch to the Mac.

1. Show the Simulator running.

---

<a name="summary" />
## Summary ##

In this demo, you saw how to create, deploy, monitor and scale a Windows Azure Web Site. Additionally, you improved your application by adding Windows Azure Service Bus messaging capabilities.
