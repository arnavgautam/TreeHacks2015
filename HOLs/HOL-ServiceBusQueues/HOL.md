<a name="HOLTop"></a>
# Service Bus Queues #
---

<a name="Overview"></a>
## Overview ##

**Service Bus Messaging** contains a brand-new set of cloud-based, message-oriented-middleware technologies including a fully-featured **Message Queue** with support for arbitrary content types, rich message properties, correlation, reliable binary transfer, and grouping.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a Queue.
- Send and Receive Messages through a Queue.
- Inspect Message Properties.
- Use AMQP protocol to send messages to a Queue.
- Receive messages from the Queue with a Java applications using AMQP

<a name="Prerequisites"></a>
### Prerequisites ###

You must have the following items to complete this lab:

- [Windows Azure SDK for .NET](http://www.windowsazure.com/en-us/downloads/?sdk=net)
- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

>**Note:** This lab was designed to use Windows 8.1 Operating System.

<a name="Setup"></a>
### Setup ###
In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process that will configure your environment and install the Visual Studio Code Snippets for this lab.

1. If the User Account Control dialog is shown, confirm the action to proceed.

 
> **Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="UsingCodeSnippets"></a>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blowcks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2013 to avoid having to add it manually.

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating a Queue](#Exercise1)
1. [Interacting with Service Bus Queues](#Exercise2)
1. [Talking Across Platforms and Languages with AMQP](#Exercise3)

Estimated time to complete this lab: **60 minutes**.

> **Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

<a name="Exercise1"></a>
### Exercise 1: Creating a Service Bus Namespace ###

To follow this lab and complete all the exercises, you first need to create a Windows Azure Service Bus namespace. Once created, it can be used for **all** of the labs that use Windows Azure Service Bus and for your own projects as well.

<a name="Ex1Task1"></a>
#### Task 1 - Creating your Service Bus Namespace ####

In this task, you will create a new Windows Azure Service Bus Namespace.

1. Navigate to [Windows Azure Management Portal](http://manage.windowsazure.com/). You will be prompted for your **Microsoft Account** credentials if you are not already signed in.

1. Click **Service Bus** within the left pane.

 	![Configuring Windows Azure Service bus](Images/configuring-windows-azure-service-bus.png?raw=true "Configuring Windows Azure Service bus")
 
	_Configuring Windows Azure Service bus_

1. Create a Service Namespace. A service namespace provides an application boundary for each application exposed through the Service Bus and is used to construct Service Bus endpoints for the application. To create a service namespace, click **Create** on the bottom bar. 

 	![Creating a New Namespace](Images/creating-a-new-namespace.png?raw=true "Creating a New Namespace")
 
	_Creating a New Namespace_

1. In the **Create A Namespace** dialog, enter a name for your service **Namespace** and select a **Region** for your service to run in. Service names must be globally unique as they are hosted in the cloud and accessible by whomever you decide to grant access.

 	![Creating a new Service Namespace](Images/creating-a-new-service-namespace.png?raw=true "Creating a new Service Namespace")
 
	_Creating a new Service Namespace_

	> **Note:** It can take a few minutes while your service is provisioned.

1. Once the namespace is active, select the service's row and click **Connection Information** within the bottom menu.

	![View Connection Information](Images/view-connection-information.png?raw=true "View Connection Information")

	_View Connection Information_

1. In the **Connection Information** dialog, record the value shown for **Default Issuer** and **Default Key**, and click **OK**. You will need these values later when configuring your Web Role settings.

 	![Service Bus default keys](Images/service-bus-default-keys.png?raw=true "Service Bus default keys")
 
	_Service Bus default keys_

 
You have now created a new Windows Azure namespace for this hands-on lab. To sign in at any time, simply navigate to the Windows Azure Management Portal, click **Sign In** and provide your **Microsoft Account** credentials.

> **Note:** In this lab you will learn how to create and make use of Service Bus Queues from an ASP.NET MVC Application. You can also create Queues and Topics from the Windows Azure Management Portal, for more information see [Appendix A: Creating Queues and Topics using Windows Azure Management Portal](#appendixA).

 
<a name="Exercise2"></a>
### Exercise 2: Interacting with Service Bus Queues ###

In this exercise, you will learn how to create and use a **Service Bus Queue**. You will set up a MVC 5 application to communicate with your Service Bus Namespace, create a new Queue and learn how to send and receive messages from it.

<a name="Ex2Task1"></a>
#### Task 1 - Creating a Queue ####

In this task, you will create a Queue in your Service Bus namespace.

1. Open **Visual Studio 2013 Express for Web or higher** as Administrator.

1. Open the solution file located at **Source\Ex2-InteractingwithServiceBusQueues\Begin\Begin.sln** in the **Source** folder of this lab.

1. Build the solution in order to download and install the NuGet package dependencies. To do this, click **Build** | **Build Solution** or press **Ctrl + Shift + B**.

	>**Note:** NuGet is a Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects that use the .NET Framework.
	>
	> When you install the package, NuGet copies files to your solution and automatically makes whatever changes are needed, such as adding references and changing your app.config or web.config file. If you decide to remove the library, NuGet removes files and reverses whatever changes it made in your project so that no clutter is left.
	>
	>For more information about NuGet, visit [http://nuget.org/](http://nuget.org/).

1. Update the service definition to define the configuration settings required to access your Service Bus namespace. To do this, expand the **Roles** folder of the **UsingQueues** project in **Solution Explorer**, right-click **UsingQueues.Web**, and then select **Properties**.

	![Launching the service configuration editor](Images/launching-the-service-configuration-editor.png?raw=true "Launching the service configuration editor")
 
	_Launching the service configuration editor_

1. In the **Settings** tab, set _namespaceAddress_ value to the name of your Service Bus namespace, and set the _issuerName_ and _issuerKey_ values to the ones you previously copied from the [Windows Azure Management Portal](http://go.microsoft.com/fwlink/?LinkID=129428).

	![Updating settings to the UsingQueues.Web Web Role](Images/updating-settings-to-the-usingqueuesweb-web-r.png?raw=true "Updating settings to the UsingQueues.Web Web Role")

	_Updating settings to the **UsingQueues** Web Role_

1. Press **CTRL + S** to save the changes to the Web Role configuration.

1. Next, you will add the required assemblies to the **ASP.NET MVC 5** Web project to connect to the **Windows Azure Service Bus** from your application. In **Solution Explorer**, right-click on **UsingQueues** project node and select **Add Reference.**

1. In the **Reference Manager** dialog, check the **System.Runtime.Serialization** assembly.  Then, select the **Extensions** assemblies from the left pane, check **Microsoft.ServiceBus** and ensure **Microsoft.WindowsAzure.ServiceRuntime** is checked as well. Click **OK** to add the references.

1. Open the **HomeController.cs** file under the **Controllers** folder in the **UsingQueues** project.

1. Add the following namespace directives to declare the Service Bus and the Windows Azure supporting assemblies, and a reference to the **Models** namespace of the Web project, which you will use in the next tasks.

	(Code Snippet - _Service Bus Messaging - Ex02 - Adding Namespace Directives_ - CS)

	<!-- mark:1-3 -->
	````C#
	using Microsoft.ServiceBus;
	using Microsoft.ServiceBus.Messaging;
	using Microsoft.WindowsAzure.ServiceRuntime;
	````

1. Add two properties to the **HomeController** class to enable the communication with the Service Bus Queue.

	(Code Snippet - _Service Bus Messaging - Ex02 - Service Bus Properties_ - CS)

	<!-- mark:1-2 -->
	````C#
	private NamespaceManager namespaceManager;
	private MessagingFactory messagingFactory;
	````

1. In order to create a Queue, we have to connect to the **Service Bus Namespace** address and bind this namespace to a **MessagingFactory**. This class is in charge of creating the entities responsible for sending and receiving messages through Queues. Add a constructor for the **HomeController** by including the following code:

	(Code Snippet - _Service Bus Messaging - Ex02 - HomeController Constructor_ - CS)

	<!-- mark:1-11 -->
	````C#
	public HomeController()
	{
	    var baseAddress = RoleEnvironment.GetConfigurationSettingValue("namespaceAddress");
	    var issuerName = RoleEnvironment.GetConfigurationSettingValue("issuerName");
	    var issuerKey = RoleEnvironment.GetConfigurationSettingValue("issuerKey");
	
	    Uri namespaceAddress = ServiceBusEnvironment.CreateServiceUri("sb", baseAddress, string.Empty);
	
	    this.namespaceManager = new NamespaceManager(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
	    this.messagingFactory = MessagingFactory.Create(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
	}
	````

1. Add the following method to the **HomeController** class. This method uses the **namespaceClient** object to create a new Queue.

	(Code Snippet - _Service Bus Messaging - Ex02 - CreateQueue_ - CS)

	<!-- mark:1-13 -->
	````C#
	[HttpPost]
	public JsonResult CreateQueue(string queueName)
	{
	    try
	    {
	        var queueDescription = this.namespaceManager.CreateQueue(queueName);
	        return this.Json(queueDescription, JsonRequestBehavior.AllowGet);
	    }
	    catch (Exception)
	    {
	        return this.Json(false, JsonRequestBehavior.AllowGet);
	    }
	}
	````

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex2Task2"></a>
#### Task 2 - Sending a Message ####

In this task, you learn how to send a message through a Queue. You can send any serializable object as a **Message** through Queues**.** You will send a **CustomMessage** object which has its own properties and is agnostic on how the **Service Bus Queue** works or interacts with your application.

1. Create a new class under the **Models** folder of the **UsingQueues** project. To do this, right-clicking the folder, select **Add** and then **Class**. In the **Add New Item** dialog, set the name of the class to _CustomMessage_.

1. Replace the entire code of the class with the following:

	(Code Snippet - _Service Bus Messaging - Ex02 - CustomMessage Class_ - CS)

	<!-- mark:1-23 -->
	````C#
	namespace UsingQueues.Models
	{
	    using System;
	
	    [Serializable]
	    public class CustomMessage
	    {
	        private DateTime date;
	        private string body;
	
	        public DateTime Date
	        {
	            get { return this.date; }
	            set { this.date = value; }
	        }
	        
	        public string Body
	        {
	            get { return this.body; }
	            set { this.body = value; }
	        }
	    }
	}
	````

1. Press **CTRL + S** to save the changes.

1. Next, you will create the method in the **HomeController** class that allows you to send your custom object to a **Queue**. Open the **HomeController.cs** file under the **Controllers** folder in the **UsingQueues** project.

1. Firt add the following using statement on top of the file.

	(Code Snippet - _Service Bus Messaging - Ex02 - Adding Models Namespace Directives_ - CS)

	<!-- mark:1 -->
	````C#
	using UsingQueues.Models;
	````

1. Add the following method to the class.

	(Code Snippet - _Service Bus Messaging - Ex01 - NewCustomMessage_ - CS)
	<!-- mark:1-6 -->
	````C#
	[HttpPost]
	public JsonResult SendMessage(string queueName, string message)
	{
	    QueueClient queueClient = this.messagingFactory.CreateQueueClient(queueName);
	    var customMessage = new CustomMessage() { Date = DateTime.Now, Body = message };
	}
	````

1. Next, you will instantiate a **CustomMessage** object and set its **Date** property with the current date and its **Body** property with the text you receive from the UI. This way you are sending a plain text message through a Queue along with additional, useful information for processing the messages received from Queues. In order to do this, add the following bolded code inside the **SendMessage** method:

	(Code Snippet - _Service Bus Messaging - Ex02 - Send BrokeredMessage_ - CS)

	<!-- mark:6-26 -->
	````C#
	[HttpPost]
	public JsonResult SendMessage(string queueName, string message)
	{
	    QueueClient queueClient = this.messagingFactory.CreateQueueClient(queueName);
	    var customMessage = new CustomMessage() { Date = DateTime.Now, Body = message };
	    BrokeredMessage bm = null;
	    
	    try
	    {
	      bm = new BrokeredMessage(customMessage);
	      queueClient.Send(bm);
	      messagesInQueue = this.GetMessageCount(queueName);
	    }
	    catch
	    {
	      // TODO: do something
	    }
	    finally
	    {
	      if (bm != null)
	      {
	        bm.Dispose();
	      }
	    }
	    
	    return this.Json(messagesInQueue, JsonRequestBehavior.AllowGet);
	}
	````

1. The **BrokeredMessage** class has a property named **Properties**, which is a Dictionary of **String/Object** key value pairs. You can set your own custom pair of key-values, and use them as needed. These properties are independent of your custom object and intended to be used in the Messaging Logic. You will add two predefined properties that will be inspected when you retrieve the message from the Queue. Add the following highlighted code inside the **SendMessage** method:

	(Code Snippet - _Service Bus Messaging - Ex02 - Add Custom Properties_ - CS)

	<!-- mark:11-12 -->
	````C#
	[HttpPost]
	public JsonResult SendMessage(string queueName, string message)
	{
	    QueueClient queueClient = this.messagingFactory.CreateQueueClient(queueName);
	    var customMessage = new CustomMessage() { Date = DateTime.Now, Body = message };
	    BrokeredMessage bm = null;
	    
	    try
	    {
	      bm = new BrokeredMessage(customMessage);
	      bm.Properties["Urgent"] = "1";
	      bm.Properties["Priority"] = "High";
	      queueClient.Send(bm);
	      messagesInQueue = this.GetMessageCount(queueName);
	    }
	    catch
	    {
	      // TODO: do something
	    }
	    finally
	    {
	      if (bm != null)
	      {
	        bm.Dispose();
	      }
	    }
	
	    return this.Json(messagesInQueue, JsonRequestBehavior.AllowGet);
	}
	````

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex2Task3"></a>
#### Task 3 - Receiving Messages ####

In the previous task, we instantiate a **QueueClient** in order to send messages to a Queue. In this task you learn how to use a **QueueClient**, to receive a message from a Queue and explore the properties inside the received message.

1. If not already opened, open the **HomeController.cs** file under the **Controllers** folder in the **UsingQueues** project.

1. Add the following method within **HomeController** class:

	(Code Snippet - _Service Bus Messaging - Ex02 - RetrieveMessage from a Queue_ - CS)

	<!-- mark:1-32 -->
	````C#
	[HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
	public JsonResult RetrieveMessage(string queueName)
	{
	    QueueClient queueClient = this.messagingFactory.CreateQueueClient(queueName, ReceiveMode.PeekLock);
	    BrokeredMessage receivedMessage = queueClient.Receive(new TimeSpan(0,0,30));
	
	    if (receivedMessage == null)
	    {
	        return this.Json(null, JsonRequestBehavior.AllowGet);
	    }
	
	    var receivedCustomMessage = receivedMessage.GetBody<CustomMessage>();
	
	    var brokeredMsgProperties = new Dictionary<string, object>();
	    brokeredMsgProperties.Add("Size", receivedMessage.Size);
	    brokeredMsgProperties.Add("MessageId", receivedMessage.MessageId.Substring(0, 15) + "...");
	    brokeredMsgProperties.Add("TimeToLive", receivedMessage.TimeToLive.TotalSeconds);
	    brokeredMsgProperties.Add("EnqueuedTimeUtc", receivedMessage.EnqueuedTimeUtc.ToString("yyyy-MM-dd HH:mm:ss"));
	    brokeredMsgProperties.Add("ExpiresAtUtc", receivedMessage.ExpiresAtUtc.ToString("yyyy-MM-dd HH:mm:ss"));
	
	    var messageInfo = new
	    {
	        Label = receivedMessage.Label,
	        Date = receivedCustomMessage.Date,
	        Message = receivedCustomMessage.Body,
	        Properties = receivedMessage.Properties.ToArray(),
	        BrokeredMsgProperties = brokeredMsgProperties.ToArray()
	    };
	
	    receivedMessage.Complete();
	    return this.Json(new { MessageInfo = messageInfo }, JsonRequestBehavior.AllowGet);
	}
	````

1. The UI requires a way to retrieve the names of the existent queues in the Service Bus and another method to count the number of messages in a specific Queue. For this, add the following ActionMethods at the end of the **HomeController** class:

	(Code Snippet - _Service Bus Messaging - Ex02 - GetQueues and Count_ - CS)

	<!-- mark:1-12 -->
	````C#
	[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
	public JsonResult Queues()
	{
	    var queues = this.namespaceManager.GetQueues().Select(c => new { Name = c.Path, Messages = c.MessageCount }).ToArray();
	    return this.Json(queues, JsonRequestBehavior.AllowGet);
	}
	
	public long GetMessageCount(string queueName)
	{
	    var queueDescription = this.namespaceManager.GetQueue(queueName);
	    return queueDescription.MessageCount;
	}
	````

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex2Verification"></a>
#### Verification ####

You now launch the updated application in the Windows Azure compute emulator to verify that you can create a Queue, send messages to a specific Queue and also receive messages from a Queue.

1. In **Visual Studio**, configure the cloud project **UsingQueues** as the StartUp Project. To do this, in the **Solution Explorer**, right-click on **UsingQueues** and then select **Set as StartUp Project**.

 	![Configuring StartUp Project](Images/configuring-startup-project.png?raw=true "Configuring StartUp Project")
 
	_Configuring StartUp Project_

1. Press **F5** to launch the application. The browser will show the default page of the application.

 	![UsingQueues Application Home Page](Images/usingqueues-application-home-page.png?raw=true "UsingQueues Application Home Page")
 
	_UsingQueues Application Home Page_

1. In the section named **1. Create a Queue**, enter a Queue name in the textbox (like _MyQueue_) and click **Create**.

 	![Creating a Queue](Images/creating-a-queue.png?raw=true)
 
	_Creating a Queue_

 	![The application displays a message when a Queue was successfully created](Images/the-application-displays-a-message-when-a-que.png?raw=true "The application displays a message when a Queue was successfully created")
 
	_The application displays a message when a Queue was successfully created_

1. In the **2. Select a Queue** panel, select the previously created Queue from the list of Queues, enter a message in the Textbox named **Message**, and click **Send**. Your message will be sent to the Queue.

 	![Sending a Message to the Queue](Images/sending-a-message-to-the-queue.png?raw=true "Sending a Message to the Queue")
 
	_Sending a Message to the Queue_

1. If not selected, in the dropdown list of the **Receive Message** panel select the Queue you used in the previous step and then click the **Retrieve First Message in Queue** button. The Message will be shown in the panel along with its custom properties.

 	![Retrieving the First message in the Queue](Images/retrieving-the-first-message-in-the-queue.png?raw=true "Retrieving the First message in the Queue")
 
	_Retrieving the First Message in the Queue_

1. Close Internet Explorer.

<a name="Exercise3"></a>
### Exercise 3: Talking across Platforms and Languages with AMQP ###

The Advanced Message Queuing Protocol (AMQP) 1.0 is an efficient, reliable, wire-level messaging protocol that you can use to build robust, cross-platform, messaging applications.

Support for AMQP 1.0 in Service Bus means that you can use the queuing and publish/subscribe brokered messaging features from a range of platforms using an efficient binary protocol. 

In this exercise you will learn how to use the Service Bus brokered messaging features from .NET applications using the Service Bus .NET API. And then you will see how you can interact with different platforms and Languages.

<a name="Ex3Task1"></a>
#### Task 1 - Converting write code to send via AMQP ####

In this task you will update the solution you created in last exercise to support AMQP. You will update the send message functionallity to send messages using AMQP.

1. If not already open, open **Visual Studio 2013 Express for Web or higher** as Administrator and open the solution file located at **Source\Ex3-TalkingacrossPlatformsandLanguageswithAMQP\Begin\Begin.sln**.

	>**Note:** You can continue working with the solution you were working on exercise 2.

1. Open **Web.config** file from **UsingQueue** project.

2. Locate the **Microsoft.ServiceBus.ConnectionString** key in the **appSettings** tag and replace the placeholders with your namepsace ane and issuer key. After updating the connection string, add at the end of the connection string the **;TransportType=Amqp**.

	>**Note:** When using AMQP, the connection string is appended with **;TransportType=Amqp**, which tells the client library to make its connection to Service Bus using AMQP 1.0.

1. Now, open **HomeController.cs** located in the **Controllers** folder.

1. Add the following using statements at the top of the file.

	(Code Snippet - _Service Bus Messaging - Ex03 - Adding Models Namespace Directives_ - CS)

	````C#
	using System.Configuration;
	````

1. Add the following private string property before the **HomeController** constructor.
	
	(Code Snippet - _Service Bus Messaging - Ex03 - Connection String Property_ - CS)
	````C#
	private string connectionStringKey = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
	````

1. Update the **HomeController** constructor and replace the MessagingFactory creation with the following one

	(Code Snippet - _Service Bus Messaging - Ex03 - Messaging Factory creation_ - CS)

	<!-- mark:10 -->
	````C#
	public HomeController()
	{
		var baseAddress = RoleEnvironment.GetConfigurationSettingValue("namespaceAddress");
		var issuerName = RoleEnvironment.GetConfigurationSettingValue("issuerName");
		var issuerKey = RoleEnvironment.GetConfigurationSettingValue("issuerKey");

		Uri namespaceAddress = ServiceBusEnvironment.CreateServiceUri("sb", baseAddress, string.Empty);

		this.namespaceManager = new NamespaceManager(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
		this.messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);
	}
	````

1. Scroll down to the **SendMessage** action and replace it with the following implementation.

	(Code Snippet -  _Service Bus Messaging - Ex03 - SendMessage with AMQP_ - CS))

	<!-- mark:1-23 -->
	````C#
	[HttpPost]
	public void SendMessage(string queueName, string messageBody, bool isUrgent, bool isFollowUp)
	{
		MessageSender sender = this.messagingFactory.CreateMessageSender(queueName);
		var customMessage = new CustomMessage() { Date = DateTime.Now, Body = messageBody };
		BrokeredMessage bm = null;

		try
		{
			bm = new BrokeredMessage(customMessage.ToDictinary());
			bm.Properties["Urgent"] = "1";
			bm.Properties["Priority"] = "High";
			sender.Send(bm);
		}
		catch { }
		finally
		{
			if (bm != null)
			{
				bm.Dispose();
			}
		}
	}

	````

1. Open **CustomMessage.cs** file located in **Models** folder.

1. Add the following Helper method to return the object properties in a Dictionary.
	
	(Code Snippet -  _Service Bus Messaging - Ex03 - ToDictionary helper method_ - CS))

	<!-- mark:1-7 -->
	````C#
	public Dictionary<string, object> ToDictinary()
	{
		var dictionary = new Dictionary<string, object>();
		dictionary.Add("body", this.body);
		dictionary.Add("date", this.date);
		return dictionary;
	}
	````


<a name="Ex3Task2"></a>
#### Task 2 - Creating an Oracle Linux VM with Java Runtime Environment (JRE) installed ####

In this task you will create a Linux Virtual Machine with Java already installed which you are going to use then to deploy a Java application that reads messages from the Queue.

1. Navigate to [Windows Azure Management Portal](http://manage.windowsazure.com/). You will be prompted for your **Microsoft Account** credentials if you are not already signed in.

1. Click **NEW** at the bottom of the Page.

	![New button](Images/new-button.png?raw=true "New button")

	_New button_

1. Click on **COMPUTE** | **VIRTUAL MACHINE** | **FROM GALLERY**.

	![New VM from Gallery](Images/new-vm-from-gallery.png?raw=true "New VM from Gallery")

	_New Virtual Machine from Gallery_

1. In the **CREATE A VIRTUAL MACHINE** box, select the **Oracle WebLogic Server** image.

	![Selecting the Oracle WebLogic Server image](Images/selecting-the-oracle-weblogic-server-image.png?raw=true "Selecting the Oracle WebLogic Server image")

	_Selecting the Oracle WebLogic Server image_

1. Provided a password.

	![Configuring the Virtual Machine](Images/configuring-the-virtual-machine.png?raw=true "Configuring the Virtual Machine")

	_Configuring the Virtual Machine_

1. Configurate a region 

	![Configurating the Virtual Machine Cloud Service](Images/configurating-the-virtual-machine-cloud-servi.png?raw=true "Configurating the Virtual Machine Cloud Service")

	_Configurating the Virtual Machine Cloud Service_

1. Configurate the ports.

	![Configurating the ports](Images/configurating-the-ports.png?raw=true "Configurating the ports")

	_Configurating the ports_

1. Wait until the Virtual Machine is created

1. After the Virtual Machine is created, locate the row for your Virtual Machine and copy the value that is located in the **DNS NAME** columns.

<a name="Ex3Task3"></a>
#### Task 3 - Deploying the Java application to the VM ####

In this task yoy will deploy Java application which reads from the Queue to a Linux Virtual Machine. 

>**Note** You can follow [this](http://www.windowsazure.com/en-us/documentation/articles/service-bus-java-how-to-use-jms-api-amqp/) guide in order to user Service Bus & AMQP 1.0 with JAVA.

1. Open the **servicesbus.properties** file located at **Assets/JavaApp** with your favorite text editor.

1. Update the **connectionfactory.SBCF** value with your Service Bus namespace values.

	>**Note:** The format of the Connection URL is as follows:
	>
	>**amqps://[username]:[password]@[namespace].servicebus.windows.net** Where [namespace], [username] and [password] have the following meanings:
	>
	>- **[namespace]** The Service Bus namespace obtained from the Windows Azure Management Portal.
	>- **[username]** The Service Bus issuer name obtained from the Windows Azure Management Portal.
	>- **[password]** URL encoded form of the Service Bus issuer key obtained from the Windows Azure Management Portal.

1. Update the **queue.QUEUE** value with the name of the queue you have created in the previous exercise (avoid the quotations). Save the file.

1. Copy the assets inside the **Assets/JavaApp** folder to the Virtual Machine.


	> **Note:** You can use  to copy the files to the virtual machine using the command **scp**
	>
	>	`scp -r ./JavaApp/ azureuser@[your-vm-name].cloudapp.net:/home/azureuser`
	>
	>	![Copying the Java queue client to the VM](Images/copying-the-java-queue-client-to-the-vm.png?raw=true "Copying the Java queue client to the VM")
	>
	>	_Copying the Java queue client to the VM_

1. Connect to the Virtual Machine using ssh using the **DNS NAME** of the Virtual Machine you created in the previous task

	![Connecting to the VM using PuTTY client](Images/connecting-to-the-vm-using-putty-client.png?raw=true "Connecting to the VM using PuTTY client")

	_Connecting to the VM using PuTTY client_

1. Verify that the files were uploaded. To do so, make sure the **JavaApp** folder exists and change the directory to the **JavaApp** folder to see that the two files are there.

	![Verifying that the files were uploaded correctly](Images/verifying-that-the-files-were-uploaded-correc.png?raw=true "Verifying that the files were uploaded correctly")

	_Verifying that the files were uploaded correctly_

> **Note:** Do not close the ssh conection as you will use it in the next task.

<a name="Ex3Verification"></a>
#### Verification ####

You now launch the updated application in the Windows Azure compute emulator to verify that you can send a message to a Queue with a .NET application with AMQP protocol and receive the message from a Java application running on linux.

1. In **Visual Studio**, configure the cloud project **UsingQueues** as the StartUp Project.

1. Press **F5** to launch the application adn wait until the application retrieves the list of Queues.

1. In the **2. Select a Queue** panel, select the Queue you have created in the previous exercise.

1. In the **3. Send a Message panel**, enter a message in the Textbox named **Message**, and click **Send**. 

	![Sending the Message with AMQP](Images/sending-the-message-with-amqp.png?raw=true "Sending the Message with AMQP")

	_Sending the Message with AMQP_

1. Switch to the PuTTY client connected to the Linux Virtual Machine and run the following command in order to execute the java client.

	````Bash
	java -jar sbqueuereader.jar
	````

	![Executing the java client in the VM](Images/executing-the-java-client-in-the-vm.png?raw=true "Executing the java client in the VM")
	
	_Executing the java client in the VM_

	>**Note** Notice how the application receives the message and displays the output in the console.

---

## Next Steps ##

To learn more about **Service Bus Queues**, please refer to the following articles which expand on the technologies explained in this lab:

- [How to Use Service Bus Queues](https://www.windowsazure.com/en-us/documentation/articles/service-bus-dotnet-how-to-use-queues/): This guide will show you how to use Service Bus queues. The samples are written in C# and use the .NET API. The scenarios covered include creating queues, sending and receiving messages, and deleting queues.

- [How to use AMQP 1.0 with the Service Bus .NET API](http://www.windowsazure.com/en-us/documentation/articles/service-bus-dotnet-advanced-message-queuing/): This how-to guide explains how to use the Service Bus brokered messaging features (queues and publish/subscribe topics) from .NET applications using the Service Bus .NET API. There is a companion how-to guide that explains how to do the same using the standard Java Message Service (JMS) API. You can use these two guides together to learn about cross-platform messaging using AMQP 1.0.

- [How to use the Java Message Service (JMS) API with Service Bus & AMQP 1.0](http://www.windowsazure.com/en-us/documentation/articles/service-bus-java-how-to-use-jms-api-amqp/): This how-to guide explains how to use the Service Bus brokered messaging features (queues and publish/subscribe topics) from Java applications using the popular Java Message Service (JMS) API standard. There is a companion How-to guide that explains how to do the same using the Service Bus .NET API. You can use these two guides together to learn about cross-platform messaging using AMQP 1.0.


---

<a name="Summary"></a>
## Summary ##

 By completing this hands-on lab, you have reviewed the basic elements of Service Bus Queues. You have seen how to send and retrieve messages through a Queue. Finally, you learned how to send and receive messages through a Queue from different languages using AMQP protocol.