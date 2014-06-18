<a name="title" />
# Service Bus Relay #

---

<a name="Overview" />
## Overview ##

This demonstration shows how to configure Service Bus Relay service.

The Service Bus Relay service enables you to build hybrid applications that run in both a Microsoft Azure datacenter and your own on-premises enterprise environment. The Service Bus relay facilitates this by enabling you to securely expose Windows Communication Foundation (WCF) services that reside within a corporate enterprise network to the public cloud, without having to open up a firewall connection or requiring intrusive changes to a corporate network infrastructure.


> **Note:** In order to run through this complete demo, you must have network connectivity and a Microsoft account.

<a id="goals" />
### Goals ###
In this demo, you will see how to:

1.	Configure an application to consume Service Bus Relay services.

1. Test the application using an iPhone emulator.

<a name="technologies" />
### Key Technologies ###

This demo uses the following technologies:

- [Microsoft Azure Management Portal] [1]

[1]: https://manage.windowsazure.com/

<a name="setup" />
### Setup and Configuration ###

1. Make sure you have installed [**Web Matrix 2**](http://www.microsoft.com/web/webmatrix/) with the [**iPhone simulator extension**](http://extensions.webmatrix.com/packages/iPhoneSimulator/) or another iPhone simulator or emulator.

1. Go to <https://manage.windowsazure.com/>

1. To use **Service Bus**, you need to access the previous management portal version. In order to do this, hover the mouse pointer over **Preview** in the main page header and click **Take me to the previous portal**.

1. Click **Service Bus, Access Control and Caching** and select **Service Bus**.

1. Click the **New** button to create a new Service Namespace.

1. In the **Create a new Service Namespace** dialog, enter a valid namespace and select a Country/Region and the Subscription where you want to create the namespace. Ensure you have selected "Service Bus" from the "Available Services" list and click **Create Namespace**.

	![Create a new Service Namespace](Images/create-a-new-service-namespace.png?raw=true "Create a new Service Namespace")

	_Create a new Service Namespace_

1. Once your Service Namespace is created select it and click **View** in the **Default Key** property.

	![Default Key](Images/default-key.png?raw=true "Default Key")

	_Default Key_

1. Take note of the default issuer name and key.

1. Open the file located at **\Source\Config.Local.xml**.

1. Replace the values with your service bus namespace, issuer name and issuer key.

1. Run the **Setup.Local.cmd** script as an administrator.

> **Note:** The setup script copies the source code for this demo to a working folder that can be configured in the **Config.Local.xml** file (by default, C:\Projects). From now on, this folder will be referenced in this document as the **working folder**.

---

<a name="Demo" />
## Demo ##

This demo is composed of the following segments:

1. [Service Bus Relay](#segment1)

<a name="segment1" />
### Service Bus Relay ###

1. Open **Visual Studio 2012** as administrator.

1. Click File | Open | Project/Solution and open the **ServiceBusRelay.Console.sln** solution file located at  **[working folder]\ServiceBusRelay-Console**.
	
	> **Speaking Point**
	>
	> Let's open ServiceBusRelay Console application. This is a simple application that shows how to expose an on-premises SQL Database using Service Bus Relay services.

1. Press **F5** to run the console application. 

	![Service Bus Relay console Running](Images/service-bus-relay-console-running.png?raw=true "Service Bus Relay console Running")

	_Service Bus Relay console Running_

	> **Speaking Point**
	>
	> Now, we'll run the console application. 
	>
	>The Service Bus allows a Windows Communication Foundation-based (WCF) application to listen to a public network address, even if the application is located behind a NAT or network firewall. This functionality can be used to help applications communicate with each other, regardless of network specific structure. Through the use of the Service Bus as an intermediary you can interconnect different applications without the need to write and maintain complex logic and code to traverse networks.


1. Open a new instance of **Visual Studio 2012** as administrator.

1. Open the **ServiceBusRelay.Web.sln** solution file located at **[working folder]\ServiceBusRelay-Web**.

1. Make sure the cloud project is set as startup project. To do this, right-click **ServiceBusRelay.Web.Azure** and select **Set as StartUp Project**.

1. Press **F5** to run the application.

1. Copy the URL from the browser window.

	![Service Bus Relay web Running](Images/service-bus-relay-web-running.png?raw=true "Service Bus Relay web Running")

	_Service Bus Relay web Running_

	> **Speaking Point**
	>
	> Now we'll open a second application, this time a website that consumes the information that it is being exposed through the console application using Service Bus Relay services.

1. Open **Web Matrix 2**, select the Service Bus Relay site and run it using the iPhone simulator.

	![Running the application using iPhone simulator](Images/running-the-application-using-iphone-simulato.png?raw=true "Running the application using iPhone simulator")

	_Running the application using iPhone simulator_

	> **Speaking Point**
	>
	> Let's access the application from an iPhone simulator and make a call to one of the exposed methods.

1. Click **Customers**.

1. Switch to the running console application and notice the call received.

	![GetCustomers received call](Images/getcustomers-received-call.png?raw=true "GetCustomers received call")

	_GetCustomers received call_

	> **Speaking Point**
	>
	> If we switch back to the console application, you will notice how the GetCustomers request was logged.

---

<a name="summary" />
## Summary ##

In this demonstration, you saw how to implement Service Bus Relay services.
