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
