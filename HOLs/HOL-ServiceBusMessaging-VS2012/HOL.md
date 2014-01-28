<a name="HOLTop"></a>
# Service Bus Messaging #
---

<a name="Overview"></a>
## Overview ##

**Service Bus Messaging** contains a brand-new set of cloud-based, message-oriented-middleware technologies including a fully-featured **Message Queue** with support for arbitrary content types, rich message properties, correlation, reliable binary transfer, and grouping. Another important feature is **Service Bus Topics** which provide a set of publish-and-subscribe capabilities and are based on the same backend infrastructure as **Service Bus Queues**. A **Topic** consists of a sequential message store just like a **Queue**, but allows for many concurrent and durable **Subscriptions** that can independently yield copies of the published messages to consumers. Each **Subscription** can define a set of rules with simple expressions that specify which messages from the published sequence are selected into the Subscription.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a Queue.
- Send and Receive Messages through a Queue.
- Inspect Message Properties.
- Create Topics and Subscriptions.
- Use Subscription Filter Expressions.
- Use Subscription Filter Actions.

<a name="Prerequisites"></a>
### Prerequisites ###

You must have the following items to complete this lab:

- [Windows Azure SDK for .NET](http://www.windowsazure.com/en-us/downloads/?sdk=net)
- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

>**Note:** This lab was designed to use Windows 8 Operating System.

<a name="Setup"></a>
### Setup ###
In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process that will configure your environment and install the Visual Studio Code Snippets for this lab.

1. If the User Account Control dialog is shown, confirm the action to proceed.

 
> **Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="UsingCodeSnippets"></a>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2012 to avoid having to add it manually.

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Using Queues](#Exercise1)
1. [Using Topics and Subscriptions](#Exercise2)

Estimated time to complete this lab: **60 minutes**.

> **Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

<a name="GettingStarted"></a>
### Getting Started: Creating a Service Bus Namespace ###

To follow this lab and complete all the exercises, you first need to create a Windows Azure Service Bus namespace. Once created, it can be used for **all** of the labs that use Windows Azure Service Bus and for your own projects as well.

<a name="GettingStartedTask1"></a>
#### Task 1 - Creating your Service Bus Namespace ####

In this task, you will create a new Windows Azure Service Bus Namespace.

1. Navigate to [http://manage.windowsazure.com/](http://manage.windowsazure.com). You will be prompted for your **Microsoft Account** credentials if you are not already signed in.

1. Click **Service Bus** within the left pane.

 	![Configuring Windows Azure Service bus](./Images/Configuring-Windows-Azure-Service-bus.png?raw=true "Configuring Windows Azure Service bus")
 
	_Configuring Windows Azure Service bus_

1. Create a Service Namespace. A service namespace provides an application boundary for each application exposed through the Service Bus and is used to construct Service Bus endpoints for the application. To create a service namespace, click **Create** on the bottom bar. 

 	![Creating a New Namespace](./Images/Creating-a-New-Namespace.png?raw=true "Creating a New Namespace")
 
	_Creating a New Namespace_

1. In the **Create A Namespace** dialog, enter a name for your service **Namespace** and select a **Region** for your service to run in. Service names must be globally unique as they are hosted in the cloud and accessible by whomever you decide to grant access.

 	![Creating a new Service Namespace](./Images/Creating-a-new-Service-Namespace.png?raw=true "Creating a new Service Namespace")
 
	_Creating a new Service Namespace_

	> **Note:** It can take a few minutes while your service is provisioned.

1. Once the namespace is active, select the service's row and click **Access Key** within the bottom menu.

	![View Access Key](Images/view-access-key.png?raw=true "View Access Key")

	_View Access Key_

1. In the **Access Key** dialog, record the value shown for **Default Issuer** and **Default Key**, and click **OK**. You will need these values later when configuring your Web Role settings.

 	![Service Bus default keys](./Images/Service-Bus-default-keys.png?raw=true "Service Bus default keys")
 
	_Service Bus default keys_

 
You have now created a new Windows Azure namespace for this hands-on lab. To sign in at any time, simply navigate to the Windows Azure Management Portal, click **Sign In** and provide your **Microsoft Account** credentials.

> **Note:** In this lab you will learn how to create and make use of Service Bus Queues and Topics from an ASP.NET MVC Application. You can also create Queues and Topics from the Windows Azure Management Portal, for more information see [Appendix A: Creating Queues and Topics using Windows Azure Management Portal](#appendixA).


<a name="Exercise1"></a>
### Exercise 1: Using Queues ###

In this exercise, you will learn how to create and use a **Service Bus Queue**. You will set up a MVC 4 application to communicate with your Service Bus Namespace, create a new Queue and learn how to send and receive messages from it.

<a name="Ex1Task1"></a>
#### Task 1 - Creating a Queue ####

In this task, you will create a new Queue into your Service Bus namespace.

1. Open **Visual Studio 2012 Express for Web or higher** as Administrator.

1. Open the solution file located at **Source\Ex1-UsingQueues\Begin\Begin.sln** in the **Source** folder of this lab.

1. Build the solution in order to download and install the NuGet package dependencies. To do this, click **Build** | **Build Solution** or press **Ctrl + Shift + B**.

	>**Note:** NuGet is a Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects that use the .NET Framework.
	>
	> When you install the package, NuGet copies files to your solution and automatically makes whatever changes are needed, such as adding references and changing your app.config or web.config file. If you decide to remove the library, NuGet removes files and reverses whatever changes it made in your project so that no clutter is left.
	>
	>For more information about NuGet, visit [http://nuget.org/](http://nuget.org/).

1. Update the service definition to define the configuration settings required to access your Service Bus namespace. To do this, expand the **Roles** folder of the **UsingQueues** project in **Solution Explorer**, right-click **UsingQueues.Web**, and then select **Properties**.

	![Launching the service configuration editor](./Images/Launching-the-service-configuration-editor.png?raw=true "Launching the service configuration editor")
 
	_Launching the service configuration editor_

1. In the **Settings** tab, set _namespaceAddress_ value to the name of your Service Bus namespace, and set the _issuerName_ and _issuerKey_ values to the ones you previously copied from the [Windows Azure Management Portal](http://go.microsoft.com/fwlink/?LinkID=129428).

	![Updating settings to the UsingQueues.Web Web Role](./Images/Updating-settings-to-the-UsingQueues.Web-Web-Role.png?raw=true "Updating settings to the UsingQueues.Web Web Role")

	_Updating settings to the **UsingQueues.Web** Web Role_

1. Press **CTRL + S** to save the changes to the Web Role configuration.

1. Next, you will add the required assemblies to the **ASP.NET MVC 4** Web project to connect to the **Windows Azure Service Bus** from your application. In **Solution Explorer**, right-click on **UsingQueues.Web** project node and select **Add Reference.**

1. In the **Reference Manager** dialog, check the **System.Runtime.Serialization** assembly.  Then, select the **Extensions** assemblies from the left pane, check **Microsoft.ServiceBus** and ensure **Microsoft.WindowsAzure.ServiceRuntime** is checked as well. Click **OK** to add the references.

1. Open the **HomeController.cs** file under the **Controllers** folder in the **UsingQueues.Web** project.

1. Add the following namespace directives to declare the Service Bus and the Windows Azure supporting assemblies, and a reference to the **Models** namespace of the Web project, which you will use in the next tasks.

	(Code Snippet - _Service Bus Messaging - Ex01 - Adding Namespace Directives_ - CS)
	<!-- mark:1-4 -->
	````C#
	using Microsoft.ServiceBus;
	using Microsoft.ServiceBus.Messaging;
	using Microsoft.WindowsAzure.ServiceRuntime;
	using UsingQueues.Web.Models;
	````

1. Add two properties to the **HomeController** class to enable the communication with the Service Bus Queue.

	(Code Snippet - _Service Bus Messaging - Ex01 - Service Bus Properties_ - CS)
	<!-- mark:1-2 -->
	````C#
	private NamespaceManager namespaceManager;
	private MessagingFactory messagingFactory;
	````

1. In order to create a Queue, we have to connect to the **Service Bus Namespace** address and bind this namespace to a **MessagingFactory**. This class is in charge of creating the entities responsible for sending and receiving messages through Queues. Add a constructor for the **HomeController** by including the following code:

	(Code Snippet - _Service Bus Messaging - Ex01 - HomeController Constructor_ - CS)
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

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex1Task2"></a>
#### Task 2 - Sending a Message ####

In this task, you learn how to send a message through a Queue. You can send any serializable object as a **Message** through Queues**.** You will send a **CustomMessage** object which has its own properties and is agnostic on how the **Service Bus Queue** works or interacts with your application.

1. Create a new class under the **Models** folder of the **UsingQueues.Web** project. To do this, right-clicking the folder, select **Add** and then **Class**. In the **Add New Item** dialog, set the name of the class to _CustomMessage_.

1. Replace the entire code of the class with the following:

	(Code Snippet - _Service Bus Messaging - Ex01 - CustomMessage Class_ - CS)
	<!-- mark:1-23 -->
	````C#
	namespace UsingQueues.Web.Models
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

1. Next, you will create the method in the **HomeController** class that allows you to send your custom object to a **Queue**. Open the **HomeController.cs** file under the **Controllers** folder in the **UsingQueues.Web** project.

1. Add the following method to the class.

	(Code Snippet - _Service Bus Messaging - Ex01 - NewCustomMessage_ - CS)
	<!-- mark:1-7 -->
	````C#
	[HttpPost]
	public JsonResult SendMessage(string queueName, string message)
	{
	    QueueClient queueClient = this.messagingFactory.CreateQueueClient(queueName);
	    var customMessage = new CustomMessage() { Date = DateTime.Now, Body = message };
	    long? messagesInQueue = null;
	}
	````

1. Next, you will instantiate a **CustomMessage** object and set its **Date** property with the current date and its **Body** property with the text you receive from the UI. This way you are sending a plain text message through a Queue along with additional, useful information for processing the messages received from Queues. In order to do this, add the following bolded code inside the **SendMessage** method:

	(Code Snippet - _Service Bus Messaging - Ex01 - Send BrokeredMessage_ - CS)
	<!-- mark:7-27 -->
	````C#
	[HttpPost]
	public JsonResult SendMessage(string queueName, string message)
	{
	    QueueClient queueClient = this.messagingFactory.CreateQueueClient(queueName);
	    var customMessage = new CustomMessage() { Date = DateTime.Now, Body = message };
	    long? messagesInQueue = null;
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

	(Code Snippet - _Service Bus Messaging - Ex01 - Add Custom Properties_ - CS)
	<!-- mark:12-13 -->
	````C#
	[HttpPost]
	public JsonResult SendMessage(string queueName, string message)
	{
	    QueueClient queueClient = this.messagingFactory.CreateQueueClient(queueName);
	    var customMessage = new CustomMessage() { Date = DateTime.Now, Body = message };
	    long? messagesInQueue = null;
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

	> **Note:** You will see custom properties in action in the Exercise 2, Task 3 and 4 of this lab.

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex1Task3"></a>
#### Task 3 - Receiving Messages ####

In the previous task, we instantiate a **QueueClient** in order to send messages to a Queue. In this task you learn how to use a **QueueClient**, to receive a message from a Queue and explore the properties inside the received message.

1. If not already opened, open the **HomeController.cs** file under the **Controllers** folder in the **UsingQueues.Web** project.

1. Add the following method within **HomeController** class:

	(Code Snippet - _Service Bus Messaging - Ex01 - RetrieveMessage from a Queue_ - CS)
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
	    return this.Json(new { MessageInfo = messageInfo, MessagesInQueue = this.GetMessageCount(queueName) }, JsonRequestBehavior.AllowGet);
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

<a name="Ex1Verification"></a>
#### Verification ####

You now launch the updated application in the Windows Azure compute emulator to verify that you can create a Queue, send messages to a specific Queue and also receive messages from a Queue.

1. In **Visual Studio**, configure the cloud project **UsingQueues** as the StartUp Project. To do this, in the **Solution Explorer**, right-click on **UsingQueues** and then select **Set as StartUp Project**.

 	![Configuring StartUp Project](./Images/setting-startup-project.png?raw=true "Configuring StartUp Project")
 
	_Configuring StartUp Project_

1. Press **F5** to launch the application. The browser will show the default page of the application.

 	![UsingQueues Application Home Page](./Images/UsingQueues-Application-Home-Page.png?raw=true "UsingQueues Application Home Page")
 
	_UsingQueues Application Home Page_

1. In the panel named **Queues**, enter a Queue name in the textbox (like _MyQueue_) and click **Create**.

 	![Creating a Queue](./Images/Creating-a-Queue.png?raw=true "Creating a Queue")
 
	_Creating a Queue_

 	![The application displays a message when a Queue was successfully created](./Images/The-application-displays-a-message-when-a-Queue-was-successfully-created.png?raw=true "The application displays a message when a Queue was successfully created")
 
	_The application displays a message when a Queue was successfully created_

1. In the **Send Message** panel, select the previously created Queue from the dropdown list, enter a message in the Textbox, and click **Send**. Your message will be sent to the Queue.

 	![Sending a Message to the Queue](./Images/Sending-a-Message-to-the-Queue.png?raw=true "Sending a Message to the Queue")
 
	_Sending a Message to the Queue_

1. If not selected, in the dropdown list of the **Receive Message** panel select the Queue you used in the previous step and then click the **Retrieve First Message in Queue** button. The Message will be shown in the panel along with its custom properties.

 	![Retrieving the First Message in the Queue](./Images/Retrieving-the-First-Message-in-the-Queue.png?raw=true "Retrieving the First Message in the Queue")
 
	_Retrieving the First Message in the Queue_

1. Close Internet Explorer.

<a name="Exercise2"></a>
### Exercise 2: Using Topics and Subscriptions ###

In this exercise, you will learn to create a Topic and add Subscriptions to it. A Subscription works like a Queue but you can apply filters on it to retrieve only the messages relevant to that Subscription. When you send a Message to a Topic, all the subscriptions verify if the message has a match with its own subscription rules. If there is a match, the subscription will contain a virtual copy of the message. This is useful to avoid sending multiple messages to different subscriptions. Sending a single message to a Topic will distribute along different Subscriptions by checking Rule Expressions. Additionally, you will learn how to apply **Filter Actions** to Subscriptions to modify the **BrokeredMessage** properties of the messages that match a custom rule.

<a name="Ex2Task1"></a>
#### Task 1 - Creating a Topic and Adding Subscriptions ####

In this task, you will learn how to create a new Topic and add several subscriptions to it. For this, first you will add the necessary configurations to connect to your Service Bus namespace.

1. Open **Visual Studio 2012** as Administrator.

1. Open the **Begin.sln** solution file from **Source\Ex2-UsingTopicsAndSubscriptions\Begin\**.

1. Build the solution in order to download and install the NuGet package dependencies. To do this, click **Build** | **Build Solution** or press **Ctrl + Shift + B**.

1. Set up your own Service Bus namespace settings in the **Service Configuration** files. To do this, in **Solution Explorer** expand the **Roles** folder in the **UsingTopics** project, right-click **UsingTopics.Web** and then select **Properties**.

 	![Launching the Service Configuration editor](./Images/Launching-the-topics-service-configuration-editor.png?raw=true "Launching the Service Configuration editor")
 
    _Launching the Service Configuration editor_

1. In the **Settings** tab, set _namespaceAddress_ value to the name of your Service Bus namespace, and set the _issuerName_ and _issuerKey_ values to the ones you previously copied from the [Windows Azure Management Portal](http://go.microsoft.com/fwlink/?LinkID=129428).

 	![Updating settings to the UsingTopics.Web Web Role](./Images/Updating-settings-to-the-UsingTopics.Web-Web-Role.png?raw=true "Updating settings to the UsingTopics.Web Web Role")
 
    _Updating settings to the **UsingTopics.Web** Web Role_

1. Press **CTRL + S** to save the changes to the Web Role configuration.

1. Open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics.Web** project.

    > **Note:** In the previous Exercise, you created a connection to the Service Bus Namespace inside the Constructor method of the **HomeController** class. For the current Exercise, this was already added for you in the Solution. For more information, refer to the step 12 of Task 1 in Exercise 1. 

1. You will create a new **Topic** with two **Subscriptions**, named _AllMessages_, and _UrgentMessages_. To do this, add the following method at the end of the **HomeController** class.

	(Code Snippet - _Service Bus Messaging - Ex02 - Create Topic and subscriptions_ - CS)
	<!-- mark:1-19 -->
	````C#
	[HttpPost]
	public JsonResult CreateTopic(string topicName)
	{
	    bool success;
	    try
	    {
	        var topic = this.namespaceManager.CreateTopic(topicName);
	        var allMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "AllMessages");
	        var urgentMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "UrgentMessages");
	
	        success = true;
	    }
	    catch (Exception)
	    {
	        success = false;
	    }
	
	    return this.Json(success, JsonRequestBehavior.AllowGet);
	}
	````

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex2Task2"></a>
#### Task 2 - Using a Subscription Rule Filter Expression ####

**Rule Filters** are used in Subscriptions to retrieve messages that match certain rules. That way you can send one message to a Topic, but it _virtually_ replicates through multiple Subscriptions.

1. If not already opened, open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics.Web** project.

1. In the previous task, you created a Topic with two Subscriptions. Now, you will replace a line of that code to include a **SqlFilter** to the _UrgentMessages_ Subscription. With this filter, the _UrgentMessages_ subscription will get only the messages that match the rule **Urgent = '1'**. Replace the code you added in the previous task with the following highlighted code.

	(Code Snippet - _Service Bus Messaging - Ex02 - Create Topic and Subscriptions with Rule Filters_ - CS)
	<!-- mark:9 -->
	````C#
	[HttpPost]
	public JsonResult CreateTopic(string topicName)
	{
	    bool success;
	    try
	    {
	        var topic = this.namespaceManager.CreateTopic(topicName);
	        var allMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "AllMessages");
	        var urgentMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "UrgentMessages", new SqlFilter("Urgent = '1'"));
	   ...
	}
	````

    > **Note:** Take into account that you can use SQL92 as Filter Expressions.

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex2Task3"></a>
#### Task 3 - Using a Subscription Rule Filter Action ####

Additionally to Rule Filter Expressions, you can use **Rule Filter Actions.** With this, you can modify the properties of a **BrokeredMessage** that matches the specified rule. You will create a new **Subscription** named _HighPriorityMessages_ containing a custom **Rule Filter Action**. All messages that match the rule _Urgent = '1'_ will be sent to that **Subscription** with the property **Priority** set to _'High'_.

> **Note:** Both Filter Expressions and Filter Actions use the properties declared in the **BrokeredMessage** dictionary named **Properties**. These rules won't apply on custom objects inside the body of the **BrokeredMessage.**

1. If not already opened, open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics.Web** project.

1. Create a new **Subscription** object with a **RuleDescription**. Within this object, you can set a **Filter** and an **Action**. This way, if the **Filter** matches, the specific **Action** is applied to the **BrokeredMessage**. In the **CreateTopic** Action Method, add the highlighted code.

	(Code Snippet - _Service Bus Messaging - Ex02 - Create Subscription with Action Filter_ - CS)
	<!-- mark:11-16 -->
	````C#
	[HttpPost]
	public JsonResult CreateTopic(string topicName)
	{
	    bool success;
	    try
	    {
	        var topic = this.namespaceManager.CreateTopic(topicName);
	        var allMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "AllMessages");
	        var urgentMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "UrgentMessages", new SqlFilter("Urgent = '1'"));
	
	        var ruleDescription = new RuleDescription()
	        {
	            Filter = new SqlFilter("Important= '1' OR Priority = 'High'"),
	            Action = new SqlRuleAction("set Priority= 'High'")
	        };
	        var highPriorityMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "HighPriorityMessages", ruleDescription);
	        success = true;
	    }
	    catch (Exception)
	    {
	        success = false;
	    }
	
	    return this.Json(success, JsonRequestBehavior.AllowGet);
	}
	````

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex2Task4"></a>
#### Task 4 - Sending Messages ####

In this task, you will send messages to a Topic and verify that each message arrives to the proper Subscription. You are going to use the same approach of Exercise 1, sending a serializable object as the **Message** with custom properties on it.

1. If not already opened, open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics.Web** project.

1. In order to send a Message you will create a **TopicClient** using the **MessagingFactory.** Next, you will create a **CustomMessage,** add it to the **BrokeredMessage** and then you will set the _Urgent_, _Important_ and _Priority_ properties with the values you receive from the UI. Finally, you will use the **TopicClient** to send the message to the **Topic**. Add the following method at the end of the **HomeController** class.

	(Code Snippet - _Service Bus Messaging - Ex02 - SendMessage_ - CS)
	<!-- mark:1-31 -->
	````C#
	[HttpPost]
	public JsonResult SendMessage(string topicName, string message, bool messageIsUrgent, bool messageIsImportant)
	{
	    TopicClient topicClient = this.messagingFactory.CreateTopicClient(topicName);
	    var customMessage = new CustomMessage() { Body = message, Date = DateTime.Now };
	    bool success = false;
	    BrokeredMessage bm = null;
	
	    try
	    {
	        bm = new BrokeredMessage(customMessage);
	        bm.Properties["Urgent"] = messageIsUrgent ? "1" : "0";
	        bm.Properties["Important"] = messageIsImportant ? "1" : "0";
	        bm.Properties["Priority"] = "Low";
	        topicClient.Send(bm);
	        success = true;
	    }
	    catch (Exception)
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
	
	    return this.Json(success, JsonRequestBehavior.AllowGet);
	}
	````

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex2Task5"></a>
#### Task 5 - Receiving Messages ####

In this task, you will learn how to receive messages from a subscription. You will use a very similar logic used in Exercise 1, but in this case you will instantiate a **MessageReceiver** object from a **SubscriptionClient**.

1. If not already opened, open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics.Web** project.

1. Add the following code at the end of the **HomeController** class.

	(Code Snippet - _Service Bus Messaging - Ex02 - RetrieveMessages_ - CS)
	<!-- mark:1-34 -->
	````C#
	[HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
	public JsonResult RetrieveMessage(string topicName, string subscriptionName)
	{
	    SubscriptionClient subscriptionClient = this.messagingFactory.CreateSubscriptionClient(topicName, subscriptionName, ReceiveMode.PeekLock);
	    BrokeredMessage receivedMessage = subscriptionClient.Receive(new TimeSpan(0,0,30));
	
	    if (receivedMessage == null)
	    {
	        return this.Json(null, JsonRequestBehavior.AllowGet);
	    }
	
	    var receivedCustomMessage = receivedMessage.GetBody<CustomMessage>();

		receivedMessage.Properties["Priority"] = receivedMessage.Properties["Important"].ToString() == "1" ? "High" : "Low";

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
	    return this.Json(messageInfo, JsonRequestBehavior.AllowGet);
	}
	````

	> **Note:** In this code you are also adding additional information of the message that you will show in the UI.

1. Add the following code at the end of the **HomeController** class to retrieve the topics and subscriptions data to the View.

	(Code Snippet - _Service Bus Messaging - Ex02 - GetTopic and Subscriptions_ - CS)
	<!-- mark:1-49 -->
	````C#
	[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
	public JsonResult Subscriptions(string topicName)
	{
	    var subscriptions = this.namespaceManager.GetSubscriptions(topicName).Select(c => c.Name);
	    return this.Json(subscriptions, JsonRequestBehavior.AllowGet);
	}
	
	[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
	public JsonResult TopicsWithSubscriptions()
	{
	    var topics = this.namespaceManager.GetTopics().Select(c => c.Path).ToList();
	    var topicsToReturn = new Dictionary<string, object>();
	    topics.ForEach(c =>
	    {
	        var subscriptions = this.namespaceManager.GetSubscriptions(c).Select(d => new { Name = d.Name, MessageCount = d.MessageCount });
	        topicsToReturn.Add(c, subscriptions);
	    });
	
	    return this.Json(topicsToReturn.ToArray(), JsonRequestBehavior.AllowGet);
	}
	
	[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
	public JsonResult Filters(string topicName, string subscriptionName)
	{
	    var rules = this.namespaceManager.GetRules(topicName, subscriptionName);
	    var sqlFilters = new List<Tuple<string, string>>();
	
	    foreach (var rule in rules)
	    {
	        var expression = rule.Filter as SqlFilter;
	        var action = rule. Action as SqlRuleAction;
	
	        if (expression != null)
	        {
	            sqlFilters.Add(
	                new Tuple<string, string>(
	                    expression.SqlExpression,
	                    action != null ? action.SqlExpression : string.Empty));
	        }
	    }
	
	    return this.Json(sqlFilters.Select(t => new { Filter = t.Item1, Action = t.Item2 }), JsonRequestBehavior.AllowGet);
	}
	
	public long GetMessageCount(string topicName, string subscriptionName)
	{
	    var subscriptionDescription = this.namespaceManager.GetSubscription(topicName, subscriptionName);
	    return subscriptionDescription.MessageCount;
	}
	````

	> **Note:** These methods are used by the View to retrieve the information on Topics and Subscriptions via jQuery and AJAX.

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex2Verification"></a>
#### Verification ####

You will now launch the updated application in the Windows Azure compute emulator to verify that you can create a Topic with subscriptions, send and receive messages. You will verify that each message will go to the subscription that matches the correct filter.

1. In **Visual Studio**, configure the cloud project **UsingTopics** as the StartUp Project. To do this, in the **Solution Explorer**, right-click on **UsingTopics** and then select **Set as StartUp Project**.

 	![Configuring StartUp Project](./Images/setting-startup-project2.png?raw=true "Configuring StartUp Project")
 
	_Configuring StartUp Project_

1. Press **F5** to launch the application. The browser will show the default page of the application.

 	![UsingTopics Application Home Page](./Images/UsingTopics-Application-Home-Page.png?raw=true "UsingTopics Application Home Page")
 
	_UsingTopics Application Home Page_

1. In the panel named **Topics**, enter a topic name, for example _MyTopic_, and click **Create**.

 	![Creating a Topic](./Images/Creating-a-Topic.png?raw=true "Creating a Topic")
 
	_Creating a Topic_

 	![The application displays a message when a Topic is created](./Images/The-application-displays-a-message-when-a-Topic-is-created.png?raw=true "The application displays a message when a Topic is created")
 
	_The application displays a message when a Topic is created_

1. In the **Send Message** panel, select the previously created **Topic** from the dropdown list, enter "This is an urgent message" in the TextBox, check **Is Urgent** and click **Send.**

 
 	![Sending a message to the topic](./Images/Sending-a-message-to-the-topic.png?raw=true "Sending a message to the topic")
 
	_Sending a message to the topic_

1. Check that the message is received only by the **UrgentMessages** and the **AllMessages** subscriptions. To do this, select each subscription in the dropdown list located in the **Receive Message** panel and click **Retrieve First message in Subscription**.

 	![Retrieving a message to the AllMessages subscription](./Images/Retrieving-a-message-to-the-AllMessages-subscription.png?raw=true "Retrieving a message to the AllMessages subscription")
 
	_Retrieving a message to the AllMessages subscription_

 	![Retrieving a message to the HighPriorityMessages subscription](./Images/Retrieving-a-message-to-the-HighPriorityMessages-subscription.png?raw=true "Retrieving a message to the HighPriorityMessages subscription")
 
	_Retrieving a message to the HighPriorityMessages subscription_

 	![Retrieving a message to the UrgentMessages subscription](./Images/Retrieving-a-message-to-the-UrgentMessages-subscription.png?raw=true "Retrieving a message to the UrgentMessages subscription")
 
	_Retrieving a message to the UrgentMessages subscription_

1. Send another message to the Topic, but this time, uncheck the **Is Urgent** checkbox and check **Mark as important**. Retrieve the message from the **HighPriorityMessages** subscription and verify that the Priority is now set to **High**.

 	![Sending an important message to the Topic](./Images/Sending-an-important-message-to-the-Topic.png?raw=true "Sending an important message to the Topic")
 
 	_Sending an important message to the Topic_

---

<a name="Summary"></a>
## Summary ##

 By completing this hands-on lab, you have reviewed the basic elements of Service Bus Queues, Topics and Subscriptions. You have seen how to send and retrieve messages through a Queue and how to create Topics and Subscriptions to it. Finally, you learned how to apply Expression Filters and Rule Actions to Subscriptions to distribute your messages that matched those rules.

---

<a name="appendixA" />
## Appendix A: Creating Queues and Topics using Windows Azure Management Portal ##

In this appendix you will learn how to create Service Bus Queues and Topics using the Windows Azure Management Portal.

<a name="AppendixAQueues" />
### Creating a Queue using Windows Azure Management Portal ###


1. Make sure you have a Service Namespace created before continue. If you do not have a service namespace created, go to the [Getting Started](#GettingStarted) section of this lab.

1. If not already open, navigate to  [http://manage.windowsazure.com](http://manage.windowsazure.com/). You will be prompted for your **Microsoft Account** credentials if you are not already signed in.

1. Click **Service Bus** within the left pane and click your namespace's name to open its dashboard.

	![Service Bus Namespace Dashboard](Images/service-bus-namespace-dashboard.png?raw=true "Service Bus Namespace Dashboard")

	_Service Bus Namespace Dashboard_

1. Click **New** in the bottom menu, then select **Service Bus Queue** | **Custom Create**.

	![Creating a new Queue](Images/creating-a-new-queue.png?raw=true "Creating a new Queue")

	_Creating a new Queue_

1. In the **Add a new queue** page, select a **Queue Name** and the **Region** for your queue. Make sure the selected **Namespace** is where you want to create the Queue and click **Next**.

	![Add new Queue](Images/add-new-queue.png?raw=true "Add new Queue")

	_Add new Queue_

1. In the **Configure queue** page, update the settings according to your needs and click **Finish**.

	![Configure Queue](Images/configure-queue.png?raw=true "Configure Queue")

	_Configure Queue_ 

1. Wait until the **Queue** is created.

	![Created Queue](Images/created-queue.png?raw=true "Created Queue")

	_Created Queue_

<a name="AppendixATopics" />
### Creating a Topic using Windows Azure Management Portal ###

1. If not already open, navigate to  [http://manage.windowsazure.com](http://manage.windowsazure.com/). You will be prompted for your **Microsoft Account** credentials if you are not already signed in.

1. Click **Service Bus** within the left pane and click your namespace's name to open its dashboard.

	![Service Bus Namespace Dashboard](Images/service-bus-namespace-dashboard.png?raw=true "Service Bus Namespace Dashboard")

	_Service Bus Namespace Dashboard_

1. Click **New** in the bottom menu, then select **Service Bus Topic** | **Custom Create**.

	![Creating a new Topic](Images/creating-a-new-topic.png?raw=true "Creating a new Topic")

	_Creating a new Topic_

1. In the **Add a new topic** page, select a **Topic Name** and the **Region** for your topic. Make sure the selected **Namespace** is where you want to create the topic and click **Next**.

	![Add new Topic](Images/add-new-topic.png?raw=true "Add new Topic")

	_Add new Topic_

1. In the **Configure topic** page, update the settings according to your needs and click **Finish**.

	![Configure Topic](Images/configure-topic.png?raw=true "Configure Topic")

	_Configure Topic_

1. Wait until the **Topic** is created.

	![Created Topic](Images/created-topic.png?raw=true "Created Topic")

	_Created Topic_