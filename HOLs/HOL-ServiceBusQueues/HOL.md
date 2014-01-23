<a name="HOLTop"></a>
# Service Bus Queues #
---

<a name="Overview"></a>
## Overview ##

**Service Bus Messaging** contains a brand-new set of cloud-based, message-oriented middleware technologies including a full-featured **Message Queue** with support for arbitrary content types, rich message properties, correlation, reliable binary transfer, and grouping.

**Queues** offer First In, First Out (FIFO) message delivery to one or more competing consumers. That is, messages are typically expected to be received and processed by the receivers in the temporal order in which they were added to the queue, and each message is received and processed by only one message consumer. Using queues to intermediate between message producers and consumers provides an inherent loose coupling between both components. Because producers and consumers are not aware of each other, consumers can be upgraded without having any effect on the producer.

In this hands-on lab, you will learn how to create a Service Bus namespace using Windows Azure Management Portal. Then you will explore how to create a cloud-based application that writes and reads messages through a queue. Finally, you will see how different platforms and languages can interact seamlessly using the Advanced Messaging Queue Protocol (AMQP) with Windows Azure Service Bus Queues.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a Service Bus Namespace and queue
- Write and read nessages through a queue
- Inspect message properties
- Use AMQP to write a message to a queue with a .NET application
- Use AMQP to read a message from a queue with a Java applications

<a name="Prerequisites"></a>
### Prerequisites ###

You must have the following items to complete this lab:

- [Visual Studio Express 2013 for Web][1] or greater
- [Windows Azure Tools for Microsoft Visual Studio 2.2 (or later)][2]
- A Windows Azure subscription
	- Sign up for a [Free Trial](http://aka.ms/watk-freetrial)
	- If you are a Visual Studio Professional, Test Professional, Premium or Ultimate with MSDN or MSDN Platforms subscriber, activate your [MSDN benefit](http://aka.ms/watk-msdn) now to start development and test on Windows Azure.
	- [BizSpark](http://aka.ms/watk-bizspark) members automatically receive the Windows Azure benefit through their Visual Studio Ultimate with MSDN subscriptions.
	- Members of the [Microsoft Partner Network](http://aka.ms/watk-mpn) Cloud Essentials program receive monthly credits of Windows Azure at no charge.

[1]: http://www.microsoft.com/visualstudio/
[2]: http://www.microsoft.com/windowsazure/sdk/

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

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use within Visual Studio 2013 to avoid adding them manually.

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
### Exercise 1: Creating a Queue ###

In this exercise, you will learn how to create a Windows Azure Service Bus Queue. Queues provide messaging capabilities that enable a large and heterogeneous class of applications running on premises or in the cloud to exchange messages in a flexible, secure and reliable fashion across network and trust boundaries.

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

 	![Create A Namespace dialog box](Images/create-a-namespace-dialog-box.png?raw=true "Create A Namespace dialog box")
 
	_Create A Namespace dialog box_

	> **Note:** It can take a few minutes while your service is provisioned.

1. Once the namespace is active, select the service's row and click **Connection Information** within the bottom menu.

	![View Connection Information](Images/view-connection-information.png?raw=true "View Connection Information")

	_View Connection Information_

1. In the **Access Connection Information** dialog box, record the value shown for **Default Issuer** and **Default Key**, and click **OK**. You will need these values later when configuring your Web Role settings.

 	![Service Bus default keys](Images/service-bus-default-keys.png?raw=true "Service Bus default keys")
 
	_Service Bus default keys_

 
You have now created a new Windows Azure namespace for this hands-on lab. To sign in at any time, simply navigate to the Windows Azure Management Portal, click **Sign In** and provide your **Microsoft Account** credentials.


>**Note**: In this lab you will learn how to create and make use of Service Bus queues from Visual Studio and from an ASP.NET MVC application. You can also create queues from the Windows Azure Management Portal, for more information see [How to Manage Service Bus Messaging Entities ](http://www.windowsazure.com/en-us/documentation/articles/service-bus-manage-message-entities/)


<a name="Ex1Task2"></a>
#### Task 2 - Creating a Queue in Visual Studio ####

The Windows Azure Tools for Microsoft Visual Studio includes Server Explorer support for managing Service Bus messaging entities, including queues. In this task, you will use Server Explorer to connect to the service bus namespace you created previously and create a queue.

1. Open **Visual Studio 2013 Express for Web** (or greater) as Administrator.

1. From the menu bar, select **View** and then click **Server Explorer**.

1. In **Server Explorer**, expand the  **Windows Azure** node, right-click **Service Bus** and select **Add New Connection...**.

	![Adding new Service Bus connection](Images/adding-new-service-bus-connection.png?raw=true)

	_Adding new Service Bus connection_

1. In the **Add Connection** dialog box, make sure the **Windows Azure Service Bus** option is selected. Enter the **Namespace name**, the **Issuer Name** and the **Issuer Key** using the values obtained in the previous task. Finally, click **OK**.

	> **Note:** Alternatively, you can check the **Use connection string** checkbox and provide the service bus connection string.

	![Add Connection dialog box](Images/add-connection-dialog-box.png?raw=true)

	_Add Connection dialog box_

1. After connecting to your Service Bus namespace, your namespace should appear under **Service Bus**. Expand the Service Bus namespace node, right-click **Queues** and select **Create New Queue...**. 

	![Creating a new queue](Images/creating-a-new-queue.png?raw=true "Creating a new queue")

	_Creating a new queue_

1. In the **New Queue** dialog box, enter a name for the service bus queue in the **Name** textbox. Leave the default options and click **Save**.

	![New queue dialog box](Images/new-queue-dialog-box.png?raw=true "New queue dialog box")

	_New queue dialog box_

1. The new queue should be added to your Service Bus Namespace.

	> **Note:** You can also use the Windows Azure Tools for Microsoft Visual Studio to send and receive test messages, as well as to define subscription rules. In the next exercises, you will learn how to perform those operations from code by using the **WindowsAzure.ServiceBus** NuGet package.

	![New queue created](Images/new-queue-created.png?raw=true "New queue created")

	_New queue created_


<a name="Ex1Task3"></a>
#### Task 3 - Creating a Queue Programmatically ####
In this task, you will learn how to use the **Mircosoft.ServiceBus.NamespaceManager** class to create a new queue. For this, first you will add the necessary configurations to connect to your Service Bus namespace


1. In **Visual Studio**, open the **Begin.sln** solution file from **Source\Ex1-CreatingAQueue\Begin**.

1. Build the solution in order to download and install the NuGet package dependencies. To do this, click **Build** | **Build Solution** or press **Ctrl + Shift + B**.

	>**Note:** NuGet is a Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects that use the .NET Framework.
	>
	> When you install the package, NuGet copies files to your solution and automatically makes whatever changes are needed, such as adding references and changing your _App.config_ or _Web.config_ files. If you decide to remove the library, NuGet removes files and reverses whatever changes it made in your project so that no clutter is left.
	>
	>For more information about NuGet, visit [http://nuget.org/](http://nuget.org/).

1. Update the service definition to define the configuration settings required to access your Service Bus namespace. To do this, in **Solution Explorer** expand the **Roles** folder of the **UsingQueues.Azure** project in **Solution Explorer**, right-click **UsingQueues**, and then select **Properties**.

	![Launching the service configuration editor](Images/launching-the-service-configuration-editor.png?raw=true "Launching the service configuration editor")
 
	_Launching the service configuration editor_

1. In the **Settings** tab, set _namespaceName_ value to the name of your Service Bus namespace, and set the _issuerName_ and _issuerKey_ values to the ones you previously copied from the [Windows Azure Management Portal](http://go.microsoft.com/fwlink/?LinkID=129428).

	![Updating settings to the UsingQueues Web Role](Images/updating-settings-to-the-usingqueues-web-role.png?raw=true "Updating settings to the UsingQueues Web Role")

	_Updating settings to the **UsingQueues** Web Role_

1. Press **CTRL + S** to save the changes to the Web Role configuration.

1. Next, you will add the required assemblies to the **ASP.NET MVC 5** Web project to connect to the **Windows Azure Service Bus** from your application. In **Solution Explorer**, right-click on **UsingQueues** project node and select **Add | Reference...**.

1. In the **Reference Manager** dialog box, check the **System.Runtime.Serialization** assembly.  Then, select the **Extensions** assemblies from the left pane, check **Microsoft.ServiceBus** and ensure **Microsoft.WindowsAzure.ServiceRuntime** is checked as well. Click **OK** to add the references.

1. Open the **HomeController.cs** file under the **Controllers** folder in the **UsingQueues** project.

1. Add the following namespace directives to declare the Service Bus and the Windows Azure supporting assemblies.

	(Code Snippet - _Service Bus Messaging - Ex01 - Adding Namespace Directives_ - CS)

	<!-- mark:1-2 -->
	````C#
	using Microsoft.ServiceBus;
	using Microsoft.WindowsAzure.ServiceRuntime;
	````

1. Add the following property to the **HomeController** class to enable Namespace management with the Service Bus.

	(Code Snippet - _Service Bus Messaging - Ex01 - Service Bus Properties_ - CS)

	<!-- mark:1 -->
	````C#
	private NamespaceManager namespaceManager;
	````

1. In order to create a Queue, we have to connect to the **Service Bus Namespace** address and bind this namespace to a **MessagingFactory**. This class is in charge of creating the entities responsible for sending and receiving messages through Queues. Add a constructor for the **HomeController** by including the following code:

	(Code Snippet - _Service Bus Messaging - Ex01 - HomeController Constructor_ - CS)
	
	<!-- mark:1-10 -->
	````C#
	public HomeController()
	{
	    var baseAddress = RoleEnvironment.GetConfigurationSettingValue("namespaceName");
	    var issuerName = RoleEnvironment.GetConfigurationSettingValue("issuerName");
	    var issuerKey = RoleEnvironment.GetConfigurationSettingValue("issuerKey");
	
	    Uri namespaceAddress = ServiceBusEnvironment.CreateServiceUri("sb", baseAddress, string.Empty);
	
	    this.namespaceManager = new NamespaceManager(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
	}
	````

1. Add the following action to the **HomeController** class. This method uses the **namespaceClient** object to create a new Queue.

	(Code Snippet - _Service Bus Messaging - Ex01 - CreateQueue_ - CS)

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

1. The UI requires a way to retrieve the names of the existent queues in the Service Bus and another method to count the number of messages in a specific Queue. For this, add the following ActionMethods at the end of the **HomeController** class:

	(Code Snippet - _Service Bus Messaging - Ex01 - GetQueues and Count_ - CS)

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

<a name="Ex1Task3"></a>
#### Task 3 - Verification ####

You will now launch the updated application in the Windows Azure Compute Emulator to verify that you can create a queue.

1. In **Visual Studio**, configure the cloud project **UsingQueues.Azure** as the startup Project. To do this, in the **Solution Explorer**, right-click on **UsingQueues.Azure** and then select **Set as StartUp Project**.

 	![Configuring StartUp Project](Images/configuring-startup-project.png?raw=true "Configuring StartUp Project")
 
	_Configuring StartUp Project_

1. Press **F5** to launch the application. The browser will show the default page of the application (note that the queue you created in the previous task is displayed under **Queue Explorer**).

 	![UsingQueues Application Home Page](Images/usingqueues-application-home-page.png?raw=true "UsingQueues Application Home Page")
 
	_UsingQueues Application Home Page_

1. In **Create a Queue** section, enter _SimpleQueue_ for the queue name, and click **Create**.

	![Creating a queue](Images/creating-a-queue.png?raw=true)
 
	_Creating a queue_

1. The application calls Service Bus to create the queue. The queue is added to the queue list and a successful message is displayed

 	![Queue created](Images/queue-created.png?raw=true "Queue created")
 
	_Queue created_


1. Go back to **Visual Studio** and stop debugging.

<a name="Exercise2"></a>
### Exercise 2: Interacting with Service Bus Queues ###

In this exercise, you will learn how to use a **Service Bus Queue**. You will set up a MVC 5 application to communicate with your Service Bus Namespace and learn how to write and read messages from it.

<a name="Ex2Task1"></a>
#### Task 1 - Write a Message to the Queue ####

In this task, you learn how to write a message to a queue. You can send any serializable object as a **Message** through queues. You will send a **CustomMessage** object which has its own properties and is agnostic on how the **Service Bus Queue** works or interacts with your application.

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

1. First add the following using statement at the top of the file.

	(Code Snippet - _Service Bus Messaging - Ex02 - Adding Namespace Directives_ - CS)

	<!-- mark:1-2 -->
	````C#
	using Microsoft.ServiceBus.Messaging;
	using UsingQueues.Models;
	````

1. Add the following property to the **HomeController** class to enable messaging with the Service Bus Queue.

	(Code Snippet - _Service Bus Messaging - Ex02 - Adding Messaging Factory Property_ - CS)

	<!-- mark:1 -->
	````C#
   private MessagingFactory messagingFactory;
	````

1. Add the following highlighted code to the **HomeController** constructor to create a new instance of the **MessagingFactory** class.

	(Code Snippet - _Service Bus Messaging - Ex02 - Adding Messaging Factory Creation_ - CS)

	<!-- mark:10 -->
	````C#
	public HomeController()
	{
		var baseAddress = RoleEnvironment.GetConfigurationSettingValue("namespaceName");
		var issuerName = RoleEnvironment.GetConfigurationSettingValue("issuerName");
		var issuerKey = RoleEnvironment.GetConfigurationSettingValue("issuerKey");

		Uri namespaceAddress = ServiceBusEnvironment.CreateServiceUri("sb", baseAddress, string.Empty);

		this.namespaceManager = new NamespaceManager(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
		this.messagingFactory = MessagingFactory.Create(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
	}
	````

1. Add the following action to the class.

	(Code Snippet - _Service Bus Messaging - Ex02 - NewCustomMessage_ - CS)

	<!-- mark:1-6 -->
	````C#
	[HttpPost]
	public void SendMessage(string queueName, string messageBody)
	{
	    QueueClient queueClient = this.messagingFactory.CreateQueueClient(queueName);
	    var customMessage = new CustomMessage() { Date = DateTime.Now, Body = messageBody };
	}
	````

1. Next, you will instantiate a **CustomMessage** object and set its **Date** property with the current date and its **Body** property with the text you receive from the UI. This way you are sending a plain text message through a queue along with additional, useful information for processing the messages received from queues. In order to do this, add the following bolded code inside the **SendMessage** method:

	(Code Snippet - _Service Bus Messaging - Ex02 - Send BrokeredMessage_ - CS)

	<!-- mark:6-23 -->
	````C#
	[HttpPost]
	public void SendMessage(string queueName, string messageBody)
	{
	    QueueClient queueClient = this.messagingFactory.CreateQueueClient(queueName);
	    var customMessage = new CustomMessage() { Date = DateTime.Now, Body = messageBody };
	    BrokeredMessage bm = null;
	    
	    try
	    {
	      bm = new BrokeredMessage(customMessage);
	      queueClient.Send(bm);
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
	}
	````

1. The **BrokeredMessage** class has a property named **Properties**, which is a Dictionary of **String/Object** key value pairs. You can set your own custom pair of key-values, and use them as needed. These properties are independent of your custom object and intended to be used in the Messaging Logic. You will add two predefined properties that will be inspected when you retrieve the message from the queue. Add the following highlighted code inside the **SendMessage** method:

	(Code Snippet - _Service Bus Messaging - Ex02 - Add Custom Properties_ - CS)

	<!-- mark:11-12 -->
	````C#
	[HttpPost]
	public void SendMessage(string queueName, string messageBody)
	{
	    QueueClient queueClient = this.messagingFactory.CreateQueueClient(queueName);
	    var customMessage = new CustomMessage() { Date = DateTime.Now, Body = messageBody };
	    BrokeredMessage bm = null;
	    
	    try
	    {
	      bm = new BrokeredMessage(customMessage);
	      bm.Properties["Urgent"] = "1";
	      bm.Properties["Priority"] = "High";
	      queueClient.Send(bm);
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
	}
	````

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex2Task2"></a>
#### Task 2 - Read a Message from the Queue ####

In the previous task, we instantiate a **QueueClient** in order to write messages to a queue. In this task you learn how to use a **QueueClient**, to read messages from a queue and explore the properties inside the message.

1. Go to the **HomeController.cs** file under the **Controllers** folder in the **UsingQueues** project.

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

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex2Task3"></a>
#### Task 3 - Verification ####

You now launch the updated application in the Windows Azure compute emulator to verify that you can create a Queue, write messages to a specific queue and also read messages from a queue.

1. Press **F5** to launch the application. 

1. In **Select a Queue** section, select the previously created queue from the list of queues, enter a message in the Textbox named **Message**, and click **Send**. Your message will be sent to the Queue.

	![Sending a Message to the queue](Images/sending-a-message-to-the-queue.png?raw=true "Sending a Message to the queue")

	_Sending a Message to the queue_

1. Notice that the message count is updated in **Select a Queue** section.

	![Message count updated](Images/message-count-updated.png?raw=true "Message count updated")

	_Message count updated_

1. If not selected, in **Select a Queue** section select your queue. In **Read a Message** section click on **Receive** to read the first message from the queue. The Message will be shown in **Message Details** section.

 	![Retrieving the first message in the queue](Images/retrieving-the-first-message-in-the-queue.png?raw=true "Retrieving the first message in the queue")
 
	_Retrieving the first message in the queue_

1. Close Internet Explorer.

<a name="Exercise3"></a>
### Exercise 3: Talking across Platforms and Languages with AMQP ###

The Advanced Message Queuing Protocol (AMQP) 1.0 is an efficient, reliable, wire-level messaging protocol that you can use to build robust, cross-platform messaging applications.

Support for AMQP 1.0 in Service Bus means that you can use the queuing and publish/subscribe brokered messaging features from a range of platforms using an efficient binary protocol. 

In this exercise you will learn how to use the Service Bus brokered messaging features from .NET applications using the Service Bus .NET API. Then you will see how you can interact with different platforms and languages.

<a name="Ex3Task1"></a>
#### Task 1 - Converting write code to send via AMQP ####

In this task you will update the solution you created in last exercise to support AMQP. You will update the SendMessage action to send messages using AMQP.

1. If not already open, open **Visual Studio 2013 Express for Web or higher** as Administrator and open the solution file located at **Source\Ex3-TalkingacrossPlatformsandLanguageswithAMQP\Begin\Begin.sln**.

	>**Note:** You can continue working with the solution from Exercise 2. If using the **Begin** solution remember to update project settings as explained in Exercise 2.

1. Open **CustomMessage.cs** file located in **Models** folder.

1. Add the following directive bellow the **using System;** statement

	(Code Snippet -  _Service Bus Messaging - Ex03 - Using Statements in Custom Message class_ - CS))

	<!-- mark:1 -->
	````C#
	using System.Collections.Generic;
	````

1. Add the following Helper method that returns the object's properties in a Dictionary.
	
	(Code Snippet -  _Service Bus Messaging - Ex03 - ToDictionary helper method_ - CS))

	<!-- mark:1-7 -->
	````C#
	public Dictionary<string, object> ToDictionary()
	{
		var dictionary = new Dictionary<string, object>();
		dictionary.Add("body", this.body);
		dictionary.Add("date", this.date);
		return dictionary;
	}
	````

1. Open **Web.config** file from **UsingQueues** project.

2. Locate the **Microsoft.ServiceBus.ConnectionString** key in the **appSettings** tag and replace the placeholders with your namepsace name and issuer key. After updating the connection string, 

1. Add **;TransportType=Amqp** at the end of the connection string.

	>**Note:** When using AMQP, the connection string is appended with **;TransportType=Amqp**, which tells the client library to make its connection to Service Bus using AMQP 1.0.

1. Open **HomeController.cs** located in the **Controllers** folder in **UsingQueues** project.

1. Add the following using statements at the top of the file.

	(Code Snippet - _Service Bus Messaging - Ex03 - Adding Models Namespace Directives_ - CS)
	
	<!-- mark:1 -->
	````C#
	using System.Configuration;
	````

1. Add the following private string property before the **HomeController** constructor.
	
	(Code Snippet - _Service Bus Messaging - Ex03 - Connection String Property_ - CS)

	<!-- mark:1 -->
	````C#
	private string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
	````

1. Locate the **HomeController** constructor and replace the **MessagingFactory** creation with the following one.

	(Code Snippet - _Service Bus Messaging - Ex03 - Messaging Factory creation_ - CS)

	<!-- mark:10 -->
	````C#
	public HomeController()
	{
		var baseAddress = RoleEnvironment.GetConfigurationSettingValue("namespaceName");
		var issuerName = RoleEnvironment.GetConfigurationSettingValue("issuerName");
		var issuerKey = RoleEnvironment.GetConfigurationSettingValue("issuerKey");

		Uri namespaceAddress = ServiceBusEnvironment.CreateServiceUri("sb", baseAddress, string.Empty);

		this.namespaceManager = new NamespaceManager(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
		this.messagingFactory = MessagingFactory.CreateFromConnectionString(connectionString);
	}
	````

1. Scroll down to the **SendMessage** action and modify it as follows.

	(Code Snippet -  _Service Bus Messaging - Ex03 - SendMessage with AMQP_ - CS))

	<!-- mark:1-26 -->
	````C#
	[HttpPost]
	public void SendMessage(string queueName, string messageBody)
	{
		MessageSender sender = this.messagingFactory.CreateMessageSender(queueName);
		var customMessage = new CustomMessage() { Date = DateTime.Now, Body = messageBody };
		BrokeredMessage bm = null;

		try
		{
			bm = new BrokeredMessage(customMessage.ToDictionary());
			bm.Properties["Urgent"] = "1";
			bm.Properties["Priority"] = "High";
			sender.Send(bm);
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
	}
	````

	>**Note** The custom message is converted to a **Dictionary** with its properties as key-value pairs before being sent. The **Dictionary** type maps to the **Map** AMQP type. To check all the AMQP types mapping, click [here](http://msdn.microsoft.com/en-us/library/windowsazure/jj841075.aspx#sectionSection2).

1. Build the solution.

<a name="Ex3Task2"></a>
#### Task 2 - Creating an Oracle Linux VM with Java Runtime Environment (JRE) installed ####

In this task you will create a Linux Virtual Machine to host a Java application that reads messages from the Queue.

1. Navigate to [Windows Azure Management Portal](http://manage.windowsazure.com/). You will be prompted for your **Microsoft Account** credentials if you are not already signed in.

1. Click **NEW** at the bottom of the Page.

	![New button](Images/new-button.png?raw=true "New button")

	_New button_

1. Click on **COMPUTE** | **VIRTUAL MACHINE** | **FROM GALLERY**.

	![New VM from Gallery](Images/new-vm-from-gallery.png?raw=true "New VM from Gallery")

	_New Virtual Machine from Gallery_

1. In the **CREATE A VIRTUAL MACHINE** box, select the **Oracle WebLogic Server** image which is installed on a **Linux** server. Click the right arrow to continue.

	![Selecting the Oracle WebLogic Server image](Images/selecting-the-oracle-weblogic-server-image.png?raw=true "Selecting the Oracle WebLogic Server image")

	_Selecting the Oracle WebLogic Server image_

1. In the **Virtual Machine Configuration** page, enter a **VIRTUAL MACHINE NAME**. In the **AUTHENTICATION** section, uncheck **UPLOAD COMPATIBLE SSH KEY FOR AUTHENTICATION** and check **PROVIDE A PASSWORD**. Provide a password for the **NEW PASSWORD** and **CONFIRM** fields.  Click the right arrow to continue.

	![Configuring the Virtual Machine](Images/configuring-the-virtual-machine.png?raw=true "Configuring the Virtual Machine")

	_Configuring the Virtual Machine_

1. In the next page, enter the **CLOUD SERVER DNS NAME**, you can automatically generate a new Storage Account or select one you already own. Then, select the Region/Affinity group/Virtual Network value and select the subscription. Click the right arrow to continue.

	![Configurating the Virtual Machine Cloud Service](Images/configurating-the-virtual-machine-cloud-servi.png?raw=true "Configurating the Virtual Machine Cloud Service")

	_Configurating the Virtual Machine Cloud Service_

1. In the **Virtual Machine Options** page, make sure that there is an endpoint created named _SSH_ and click the button to create a new Virtual Machine.

	![Configurating the ports](Images/configurating-the-ports.png?raw=true "Configurating the ports")

	_Configurating the ports_

1. Wait until the Virtual Machine is created.

1. After the Virtual Machine is created, locate the row for your Virtual Machine and copy the value that is located in the **DNS NAME** columns.

<a name="Ex3Task3"></a>
#### Task 3 - Deploying the Java application to the VM ####

In this task yoy will deploy Java application which reads from the Queue to a Linux Virtual Machine.

>**Note:** You can follow [this guide](http://www.windowsazure.com/en-us/documentation/articles/service-bus-java-how-to-use-jms-api-amqp/) in order to use Service Bus & AMQP 1.0 with JAVA.
>
>You can find the source code of the Java Console Application in **Assets/SbQueueReaderCode**.

1. Open the **servicesbus.properties** file located at **Assets/JavaApp** with your favorite text editor.

1. Update the **connectionfactory.SBCF** value with your Service Bus namespace values.

	>**Note:** The format of the Connection URL is as follows:
	>
	>**amqps://[username]:[password]@[namespace].servicebus.windows.net** Where [namespace], [username] and [password] have the following meanings:
	>
	>- **[namespace]** The Service Bus namespace obtained from the Windows Azure Management Portal.
	>- **[username]** The Service Bus issuer name obtained from the Windows Azure Management Portal.
	>- **[password]** URL encoded form of the Service Bus issuer key obtained from the Windows Azure Management Portal. A useful URL-encoding utility is available at http://www.w3schools.com/tags/ref_urlencode.asp.

1. Update the **queue.QUEUE** value with the name of the queue you have created in the previous exercise. Save the file.

	>**Note:** You **do not** have to surround the name with quotation marks.

1. Copy the assets inside the **Assets/JavaApp** folder to the Virtual Machine.


	> **Note:** You can use  to copy the files to the virtual machine using the command **scp**
	>
	>	`scp -r ./JavaApp/ [your-vm-username]@[your-vm-name].cloudapp.net:/home/[your-vm-username]`
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

<a name="Ex3Task4"></a>
#### Task 4 - Verification ####

You will now launch the updated application in the Windows Azure Compute Emulator to verify that you can send a message with a .NET application and receive it from a Java application running on Linux. **Windows Azure Service Bus Queues** helps you achieve interoperability by using **AMQP**.

1. In **Visual Studio**, configure the cloud project **UsingQueues.Azure** as the startup project.

1. Press **F5** to launch the application and wait until the application retrieves the list of queues.

1. In **Select a Queue** section, select the queue you have created in the previous exercise.

1. In **Send a Message** section, enter a message in the Textbox named **Message**, and click **Send**. 

	![Sending a Message with AMQP](Images/sending-a-message-with-amqp.png?raw=true "Sending a Message with AMQP")

	_Sending the Message with AMQP_

1. Switch to your remote terminal to the Linux Virtual Machine and run the following command in order to execute the java client.

	````Bash
	java -jar sbqueuereader.jar
	````

	![Executing the java client in the VM](Images/executing-the-java-client-in-the-vm.png?raw=true "Executing the java client in the VM")
	
	_Executing the java client in the VM_

	>**Note** The application receives the message and displays the output in the console.

---

<a name="NextSteps"></a>
## Next Steps ##

To learn more about **Service Bus Queues** please refer to the following articles:

**Technical Reference**

This is a list of articles that expand on the technologies explained on this lab:

- [Windows Azure Queues and Windows Azure Service Bus Queues - Compared and Contrasted](http://aka.ms/Nofjzt): This article analyzes the differences and similarities between the two types of queues offered by Windows Azure today: Windows Azure Queues and Windows Azure Service Bus Queues. By using this information, you can compare and contrast the respective technologies and be able to make a more informed decision about which solution best meets your needs.

- [Service Bus Queues, Topics, and Subscriptions](http://aka.ms/Jed5rg): The new release of the Windows Azure Service Bus adds a set of cloud-based, message-oriented-middleware technologies including reliable message queuing and durable publish/subscribe messaging. These “brokered” messaging capabilities can be thought of as asynchronous, or decoupled messaging features that support publish-subscribe, temporal decoupling, and load balancing scenarios using the Service Bus messaging fabric. 

- [How to Manage Service Bus Messaging Entities](http://aka.ms/Yndr05): This topic describes how to create and manage your Service Bus entities using the Windows Azure Management Portal. You can use the portal to create new service namespaces or messaging entities (queues, topics, or subscriptions). You can also delete entities, or change the status of entities.

**Development**

This is a list of developer-oriented articles related to **Service Bus Queues**:

- [How to Use Service Bus Queues](http://aka.ms/Oqobpi): This guide will show you how to use Service Bus queues. The samples are written in C# and use the .NET API. The scenarios covered include creating queues, sending and receiving messages, and deleting queues.

- [How to use AMQP 1.0 with the Service Bus .NET API](http://aka.ms/Pwzyms): This how-to guide explains how to use the Service Bus brokered messaging features (queues and publish/subscribe topics) from .NET applications using the Service Bus .NET API. There is a companion how-to guide that explains how to do the same using the standard Java Message Service (JMS) API. You can use these two guides together to learn about cross-platform messaging using AMQP 1.0.

- [How to use the Java Message Service (JMS) API with Service Bus & AMQP 1.0](http://aka.ms/Yt3o96): This how-to guide explains how to use the Service Bus brokered messaging features (queues and publish/subscribe topics) from Java applications using the popular Java Message Service (JMS) API standard. There is a companion How-to guide that explains how to do the same using the Service Bus .NET API. You can use these two guides together to learn about cross-platform messaging using AMQP 1.0.

- [Service Bus Brokered Messaging .NET Tutorial](http://aka.ms/P8wfc5): The topics in this section are intended to give you an overview and hands-on experience with one of the core components of the brokered messaging capabilities of Service Bus, a feature called Queues. After you work through the sequence of topics in this tutorial, you will have an application that populates a list of messages, creates a queue, and sends messages to that queue. Finally, the application receives and displays the messages from the queue, then cleans up its resources and exits.

---

<a name="Summary"></a>
## Summary ##

 By completing this hands-on lab, you have reviewed the basic elements of Service Bus Queues. You have seen how to send and retrieve messages through a Queue. Finally, you learned how to send and receive messages through a Queue from different languages using AMQP protocol.
