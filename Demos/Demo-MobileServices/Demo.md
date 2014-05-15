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

1. Go to Start and open **Visual Studio 2013**.

2. Click on **File**, hover on the **New** menu, and click **New Project**.

	![File > New > Project](Images/new-project.png?raw=true)
	
	_Creating new Project_

3. Make sure that **Visual C# > Cloud** is selected in the **Templates** list. Choose **Windows Azure Mobile Services**, type **MobileService** as the name for the project, and click **OK**.

	![Windows Azure Mobile Services Project](Images/windows-azure-mobile-services-project.png?raw=true)
	
	_Cloud > Windows Azure Mobile Service project_

4. Select the **Windows Azure Mobile Service** template and click **OK**.

	![New ASP.NET Project](Images/new-aspnet-project.png?raw=true)
	
	_New ASP.NET Project_

5. Explain the contents of the project template. Open and explain each one of the following folders: 

	- **Controllers**
	- **Models**
	- **ScheduledJobs**

	![Project Template](Images/project-template.png?raw=true)
	
	_Mobile Service Project Template_
	
6. Right-click the project and select **Publish**.

	![Publish](Images/publish.png?raw=true)
	
	_Publish Mobile Service Project to Azure_

7. In the dialog box, select **Mobile Services**, click **Sign In** in the dialog box, and sign in with your credentials.

8. From the dropdown list, select an existing Mobile Services.

9. Click OK. Show the different options that automatically the wizard populates. **DO NOT PUBLISH**. Click **Close** to continue.

10. Switch to the **FacilityApp** Mobile Service backend.

1. Open the **DataObjects** folder and add a new class named _FacilityRequest_.

1. Paste the snippet **snippet1**.

1. Open the **Controllers** folder and add a new **TableController** named _FacilityRequestController_.

1. Paste the snippet **snippet2**.

1. Add the attribute to authenticate with ADAL.

<a name="segment2" />
### Integrating ADAL in a Windows Store App ###


<a name="segment3" />
### Integrating ADAL and Xamarin for iOS ###

---

<a name="summary" />
## Summary ##

In this demo, you saw how to create, deploy, monitor and scale a Windows Azure Web Site. Additionally, you improved your application by adding Windows Azure Service Bus messaging capabilities.
