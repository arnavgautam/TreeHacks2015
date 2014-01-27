<a name="HOLTop"></a>
# Service Bus Topics#
---

<a name="Overview"></a>
## Overview ##

**Windows Azure Service Bus Messaging** contains a brand-new set of cloud-based, message-oriented-middleware technologies including a fully-featured **Service Bus queue** with support for arbitrary content types, rich message properties, correlation, reliable binary transfer, and grouping. Another important feature is **Service Bus topics** which provide a set of publish-and-subscribe capabilities and are based on the same backend infrastructure as **Service Bus queues**. A **topic** consists of a sequential message store just like a **queue**, but allows for many concurrent and durable **subscriptions** that can independently yield copies of the published messages to consumers. Each **subscription** can define a set of rules with simple expressions that specify which messages from the published sequence are selected into the subscription.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a Service Bus Namespace
- Create Topics and Subscriptions
- Send Messages to a Topic
- Receive Messages from Subscriptions 
- Use Subscription Filter Expressions
- Use Subscription Filter Actions

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

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2013 to avoid having to add it manually.

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating a Topic and Adding Subscriptions](#Exercise1)
1. [Sending and Receiving Messages](#Exercise2)
1. [Using a Subscription Rule Filter Expression and Rule Filter Actions](#Exercise3)

Estimated time to complete this lab: **40 minutes**.

> **Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

<a name="Exercise1"></a>
### Exercise 1: Creating a Topic and Adding Subscriptions ###

In this exercise, you will learn how to create a Windows Azure Service Bus topic and add subscriptions to it. Topics and subscriptions provide a one-to-many form of communication, in a "publish/subscribe" pattern. Useful for scaling to very large numbers of recipients, each published message is made available to each subscription registered with the topic.

<a name="Ex1Task1"></a>
#### Task 1 - Creating your Service Bus Namespace ####

To work with Service Bus topics and subscriptions, you first need to create a Windows Azure Service Bus namespace. Once created, it can be used for **all** the labs that use Windows Azure Service Bus and for your own projects as well.

1. Navigate to [Windows Azure Management Portal](http://manage.windowsazure.com). You will be prompted for your **Microsoft Account** credentials if you are not already signed in.

1. Click **Service Bus** within the left pane.

	![Configuring Windows Azure Service Bus](Images/configuring-windows-azure-service-bus.png?raw=true)
 
	_Configuring Windows Azure Service Bus_

1. Create a Service Namespace. A service namespace provides an application boundary for each application exposed through the Service Bus and is used to construct Service Bus endpoints for the application. To create a service namespace, click **Create** on the bottom bar. 

 	![Creating a new namespace](Images/creating-a-new-namespace.png?raw=true)
 
	_Creating a new namespace_

1. In the **Create A Namespace** dialog box, enter a name for your service **Namespace** and select a **Region** for your service to run in. Service names must be globally unique as they are hosted in the cloud and accessible by whomever you decide to grant access.

 	![Create A Namespace Dialog Box](Images/create-a-namespace-dialog-box.png?raw=true)
 
	_Create A Namespace dialog box_

	> **Note:** It can take a few minutes while your service is provisioned.

1. Once the namespace is active, select the service's row and click **Connection Information** within the bottom menu.

	![Connection information](Images/connection-information.png?raw=true)

	_View connection information_

1. In the **Access connection information** dialog box, record the value shown for **Default Issuer** and **Default Key**, and click **OK**. You will need these values later when connecting to Service Bus from Visual Studio and when configuring your web role settings.

 	![Service Bus default issuer and key](Images/service-bus-default-issuer-and-key.png?raw=true)
 
	_Service Bus default keys_

You have now created a new Windows Azure Service Bus namespace for this hands-on lab. To sign in at any time, simply navigate to the Windows Azure Management Portal, click **Sign In** and provide your **Microsoft Account** credentials.

> **Note:** In this lab you will learn how to create and make use of Service Bus topics and subscriptions from Visual Studio and from an ASP.NET MVC application. You can also create topics and subscriptions from the Windows Azure Management Portal, for more information see [How to Manage Service Bus Messaging Entities](http://www.windowsazure.com/en-us/documentation/articles/service-bus-manage-message-entities/).

<a name="Ex1Task2"></a>
#### Task 2 - Creating a Topic and Adding Subscriptions in Visual Studio ####

The Windows Azure Tools for Microsoft Visual Studio includes Server Explorer support for managing Service Bus messaging entities, including topics and subscriptions. In this task, you will use Server Explorer to connect to the Service Bus namespace you created previously, create a topic and add a subscription to it.

1. Open **Visual Studio 2013 Express for Web** (or greater) as Administrator.

1. From the menu bar, select **View** and then click **Server Explorer**.

1. In **Server Explorer**, expand the  **Windows Azure** node, right-click **Service Bus** and select **Add New Connection...**.

	![Adding new Service Bus connection](Images/adding-new-service-bus-connection.png?raw=true)

	_Adding new Service Bus connection_

1. In the **Add Connection** dialog box, make sure the **Windows Azure Service Bus** option is selected. Enter the **Namespace name**, the **Issuer Name** and the **Issuer Key** using the values obtained in the previous task. Finally, click **OK**.

	> **Note:** Alternatively, you can select the **Use connection string** checkbox and provide the service bus connection string.

	![Add Connection dialog box](Images/add-connection-dialog-box.png?raw=true)

	_Add Connection dialog box_

1. After connecting to your Service Bus namespace, your namespace should appear under **Service Bus**. Expand the Service Bus namespace node, right-click **Topics** and select **Create New Topic...**. 

	![Creating new Topic](Images/creating-a-new-topic.png?raw=true)

	_Creating new topic_

1. In the New Topic dialog box, enter a name for the service bus topic in the **Name** textbox. Leave the default options and click **Save**.

	![New Topic dialog box](Images/new-topic-dialog-box.png?raw=true)

	_New Topic dialog box_

1. The new topic should be added under **Topics**. Expand the topic node, right-click  **Subscriptions** and select **Create New Subscription...**.

	![Creating new subscription](Images/creating-new-subscription.png?raw=true)

	_Creating new subscription_

1. In the **New Subscription** dialog box, enter a name for the subscription in the **Name** textbox. Leave the default options and click **Save**.

	![New Subscription dialog box](Images/new-subscription-dialog-box.png?raw=true)

1. The new subscription should be added to your topic.

	> **Note:** You can also use the Windows Azure Tools for Microsoft Visual Studio to send and receive test messages, as well as to define subscription rules. In the next exercises, you will learn how to perform those operations from code by using the **WindowsAzure.ServiceBus** NuGet package.

	![New subscription created](Images/new-subscription-created.png?raw=true)

	_New subscription created_

<a name="Ex1Task3"></a>
#### Task 3 - Creating a Topic and Adding Subscriptions Programmatically ####

In this task, you will learn how to use the **Mircosoft.ServiceBus.NamespaceManager** class to create a new topic and add several subscriptions to it. For this, first you will add the necessary configurations to connect to your Service Bus namespace.

1. In **Visual Studio**, open the **Begin.sln** solution file from **Source\Ex1-CreatingATopicAndAddingSubscriptions\Begin\**.

1. Build the solution in order to download and install the NuGet package dependencies. To do this, in **Solution Explorer** right-click the solution node and select **Build Solution** or press **Ctrl + Shift + B**.

	>**Note:** NuGet is a Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects that use the .NET Framework.
	>
	> When you install the package, NuGet copies files to your solution and automatically makes whatever changes are needed, such as adding references and changing your app.config or web.config file. If you decide to remove the library, NuGet removes files and reverses whatever changes it made in your project so that no clutter is left.
	>
	>For more information about NuGet, visit [http://nuget.org/](http://nuget.org/).

1. Update the service configuration to define the configuration settings required to access your Service Bus namespace. To do this, in **Solution Explorer** expand the **Roles** folder in the **UsingTopics** project, right-click **UsingTopics** and then select **Properties**.

 	![Launching the Service Configuration editor](Images/launching-the-service-configuration-editor.png?raw=true)
 
    _Launching the Service Configuration editor_

1. In the **Settings** tab, set _namespaceName_ value to the name of your Service Bus namespace, and set the _issuerName_ and _issuerKey_ values to the ones you previously copied from the [Windows Azure Management Portal](http://go.microsoft.com/fwlink/?LinkID=129428).

	![Updating settings to the UsingTopics web role](Images/updating-settings-to-the-usingtopics-web-role.png?raw=true)

    _Updating settings to the **UsingTopics** web role_

1. Press **CTRL + S** to save the changes to the Web Role configuration.

1. Next, you will add the required assemblies to the **ASP.NET MVC 5** Web project to connect to **Windows Azure Service Bus** from your application. In **Solution Explorer**, right-click the **UsingTopics** project node and select **Add | Reference...**.

1. In the **Reference Manager** dialog box, check the **System.Runtime.Serialization** assembly. Then, select the **Extensions** assemblies from the left pane, check **Microsoft.ServiceBus** and ensure **Microsoft.WindowsAzure.ServiceRuntime** is checked as well. Click **OK** to add the references.

1. Open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics** project.

1. Add the following namespace directives to declare the Service Bus and the Windows Azure supporting assemblies.

	(Code Snippet - _Service Bus Topics - Ex01 - Adding Namespace Directives_ - CS)
	<!-- mark:1-2 -->
	````C#
	using Microsoft.ServiceBus;
	using Microsoft.WindowsAzure.ServiceRuntime;
	````
1. Add the following property to the **HomeController** class to enable the communication with the Service Bus Namespace service.

	(Code Snippet - _Service Bus Topics - Ex01 - NamespaceManager Property_ - CS)
	<!-- mark:1-1 -->
	````C#
	private NamespaceManager namespaceManager;
	````

1. In order to create a topic, you have to connect to the **Service Bus Namespace** address and bind this namespace to a **NamespaceManager**. This class is in charge of creating the entities responsible for sending and receiving messages through topics. Add a constructor for the **HomeController** by including the following code:

	(Code Snippet - _Service Bus Topics - Ex01 - HomeController Constructor_ - CS)

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

1. You will use the **namespaceManager** object to create a new **topic** with a **subscription** named _AllMessages_. To do this, add the following method at the end of the **HomeController** class.

	(Code Snippet - _Service Bus Topics - Ex01 - Create Topic and subscription_ - CS)

	<!-- mark:1-8 -->
	````C#
	[HttpPost]
	public JsonResult CreateTopic(string topicName)
	{
		 var topic = this.namespaceManager.CreateTopic(topicName);
		 var allMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "AllMessages");

		 return this.Json(topicName, JsonRequestBehavior.AllowGet);
	}
	````

1. The UI requires a way to retrieve the names of the existing topics, as well as the subscriptions of a given topic. Add the following code at the end of the **HomeController** class to retrieve the topics and subscriptions data to the view.

	(Code Snippet - _Service Bus Topics - Ex01 - Get Topics and Subscriptions_ - CS)

	<!-- mark:1-13 -->
	````C#
	[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
	public JsonResult Topics()
	{
		 var topics = this.namespaceManager.GetTopics().Select(c => c.Path);
		 return this.Json(topics, JsonRequestBehavior.AllowGet);
	}

	[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
	public JsonResult Subscriptions(string topicName)
	{
		 var subscriptions = this.namespaceManager.GetSubscriptions(topicName).Select(c => new { Name = c.Name, MessageCount = c.MessageCount });
		 return this.Json(subscriptions, JsonRequestBehavior.AllowGet);
	}
	````

	> **Note:** These methods are used by the view to retrieve the information on topics and subscriptions via jQuery and AJAX.

1. Press **CTRL + S** to save the changes to the Controller.

<a name="Ex1Task4"></a>
#### Task 4 - Verification ####
You will now launch the updated application in the Windows Azure compute emulator to verify that you can create a topic with subscriptions.

1. In **Visual Studio**, configure the cloud project **UsingTopics.Azure** as the StartUp Project. To do this, in **Solution Explorer** right-click the **UsingTopics.Azure** project node and then select **Set as StartUp Project**.

	![Configuring StartUp project](Images/configuring-startup-project.png?raw=true)

	_Configuring StartUp project_

1. Press **F5** to launch the application. The browser will show the default page of the application (note that the topic you created in the previous task is displayed under **Topic Explorer**).

	![Service Bus Topics application home page](Images/service-bus-topics-application-home-page.png?raw=true)

	_Service Bus Topics application home page_

1. In the **Create a Topic** section, enter _simpletopic_ for the topic name, and click **Create**.

	![Creating a topic](Images/creating-a-topic.png?raw=true)

	_Creating a topic_

1. The application calls Service Bus to create the topic. The topic is added to the topics list and a successful message is displayed.

	![Topic created](Images/topic-created.png?raw=true)

	_Topic created_

1. Select the new topic from the topic list. The application will retrieve the subscriptions associated to the topic from Service Bus.

	![Retrieving topic subscriptions](Images/retrieving-topic-subscriptions.png?raw=true)

	_Retrieving topic subscriptions_

1. Go back to **Visual Studio** and stop debugging.

<a name="Exercise2"></a>
### Exercise 2: Sending and Receiving Messages ###

In Exercise 1, you added the necessary code to the application in order to create Windows Azure Service Bus topics and subscriptions. You will now update the application to send messages to a topic and receive the messages that arrive to the subscriptions.

<a name="Ex2Task1"></a>
#### Task 1 - Sending Messages ####

In this task, you will send messages to a Service Bus topic. You can send any serializable object as a **Message** through topics. You will send a **CustomMessage** object which has its own properties and is agnostic on how the Service Bus topic works or interacts with your application.

1. In **Visual Studio**, open the **Begin.sln** solution file from **Source\Ex2-SendingAndReceivingMessages\Begin\**. Alternatively, you may continue with the solution that you obtained after completing the previous exercise.

1. If not already opened, open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics** project.

1. Create a new class under the **Models** folder of the **UsingTopics** project. To do this, right-click the folder, select **Add** and then **Class**. In the **Add New Item** dialog box, set the name of the class to _CustomMessage_.

1. Replace the entire code of the class with the following code.

	(Code Snippet - _Service Bus Topics - Ex02 - CustomMessage Class_ - CS)
	<!-- mark:1-23 -->
	````C#
	namespace UsingTopics.Models
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

1. Open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics** project.

1. Add the following namespace directive to access the Service Bus messaging classes.

	(Code Snippet - _Service Bus Topics - Ex02 - Adding Namespace Directives_ - CS)

	<!-- mark:1-2 -->
	````C#
	using Microsoft.ServiceBus.Messaging;
	using UsingTopics.Models;
	````

1. Add the following property to the **HomeController** class to enable access to the Service Bus messaging capabilities.

	(Code Snippet - _Service Bus Topics - Ex02 - MessagingFactory Property_ - CS)
	<!-- mark:1-1 -->
	````C#
	private MessagingFactory messagingFactory;
````

1. In order to send a message you have to bind the **Service Bus Namespace** address to a **MessagingFactory**. This is the anchor class used for run-time operations to send and receive messages to and from topics or subscriptions. Add the following code at the end of the **HomeController** constructor.

	(Code Snippet - _Service Bus Topics - Ex02 - Creating MessagingFactory_ - CS)
	<!-- mark:5-5 -->
	````C#
	public HomeController()
	{
		...

		this.messagingFactory = MessagingFactory.Create(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
	}
	````

1. Next, you will create a **CustomMessage,** add it to the **BrokeredMessage** and then you will set the _Urgent_ and _Important_ properties with the values you receive from the UI. Finally, you will use the **TopicClient** to send the message to the topic. Add the following method at the end of the **HomeController** class.

	(Code Snippet - _Service Bus Topics - Ex02 - SendMessage_ - CS)
	<!-- mark:1-18 -->
	````C#
	[HttpPost]
	public void SendMessage(string topicName, string messageBody, bool isUrgent, bool isImportant)
	{
		 TopicClient topicClient = this.messagingFactory.CreateTopicClient(topicName);
		 var customMessage = new CustomMessage() { Body = messageBody, Date = DateTime.Now };
		 var bm = new BrokeredMessage(customMessage);
		 bm.Properties["Urgent"] = isUrgent ? "1" : "0";
		 bm.Properties["Important"] = isImportant ? "1" : "0";

		 // Force message priority to "Low". Subscription filters will change the value automatically if applies
		 bm.Properties["Priority"] = "Low";
		 topicClient.Send(bm);

		 if (bm != null)
		 {
			  bm.Dispose();
		 }
	}
	````

1. Press **CTRL + S** to save the changes to the Controller class.

<a name="Ex2Task2"></a>
#### Task 2 - Receiving Messages ####

In the previous task, you instantiate a **TopicClient** in order to send messages to a topic. In this task you will learn how to use the **SubscriptionClient** to receive messages from a subscription and explore the properties inside the received message.

1. If not already opened, open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics** project.

1. Add the following code at the end of the **HomeController** class.

	(Code Snippet - _Service Bus Topics - Ex02 - RetrieveMessages_ - CS)
	<!-- mark:1-35 -->
	````C#
	[HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
	public JsonResult RetrieveMessage(string topicName, string subscriptionName)
	{
		 SubscriptionClient subscriptionClient = this.messagingFactory.CreateSubscriptionClient(topicName, subscriptionName, ReceiveMode.PeekLock);
		 BrokeredMessage receivedMessage = subscriptionClient.Receive(new TimeSpan(0, 0, 30));

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

		 var subscription = this.namespaceManager.GetSubscription(topicName, subscriptionName);

		 return this.Json(new { MessageInfo = messageInfo, MessagesInSubscription = subscription.MessageCount }, JsonRequestBehavior.AllowGet);
	}
	````

	> **Note:** In this code you are also adding additional information of the message that you will show in the UI.

1. Press **CTRL + S** to save the changes to the Controller class.

<a name="Ex2Task3"></a>
#### Task 3 - Verification ####
You will now run the application again to verify that you can send messages to a topic and receive messages from a subscription.

1. In **Visual Studio**, press **F5** to run the application.

1. Select the previously created topic. In the **Send a message section**, type _This is a test message_ in the **Message** textbox and click **Send**.

	![Sending a message](Images/sending-a-message.png?raw=true)

	_Sending a message_

1. Check that the message arrived to the **AllMessages** subscription. You should see that the message counter of the subscription is incremented to **1**.

	![Message arrived to subscription](Images/message-arrived-to-subscription.png?raw=true)

	_Message arrived to subscription_

1. Select the subscription in the **Receive a message** section and click **Receive** to retrieve the message. 

	![_Retrieving a message_](Images/retrieving-a-message.png?raw=true)

	_Retrieving a message_

1. Verify that the message has been received.

	![Message retrieved from the subscription](Images/message-retrieved-from-the-subscription.png?raw=true)

	_Message retrieved from the subscription_

1. Go back to **Visual Studio** and stop debugging.

<a name="Exercise3"></a>
### Exercise 3: Using a Subscription Rule Filter Expression and Rule Filter Actions ###

In this exercise, you will apply filters on subscriptions to retrieve only the messages relevant to that subscription. When you send a message to a topic, all the subscriptions verify if the message has a match with its own subscription rules. If there is a match, the subscription will contain a virtual copy of the message. This is useful to avoid sending multiple messages to different subscriptions. Sending a single message to a topic will distribute along different subscriptions by checking **rule expressions**. Additionally, you will learn how to apply **filter actions** to subscriptions to modify the **BrokeredMessage** properties of the messages that match a custom rule.

<a name="Ex3Task1"></a>
#### Task 1 - Using a Subscription Rule Filter Expression ####

**Rule filters** are used in subscriptions to retrieve messages that match certain rules. That way you can send one message to a topic, but it _virtually_ replicates through multiple subscriptions.

1. In **Visual Studio**, open the **Begin.sln** solution file from **Source\Ex3-UsingSubscriptionRules\Begin\**. Alternatively, you may continue with the solution that you obtained after completing the previous exercise.

1. If not already opened, open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics** project.

1. In the previous task, you created a topic with one subscription. Now, you will update the **CreateTopic** method to add a new _UrgentMessages_ subscription. This subscription will include a **SqlFilter** to get only the messages that match the rule _Urgent = '1'_. Add the following highlighted code in the **CreateTopic** action method.

	(Code Snippet - _Service Bus Topics - Ex03 - Add Subscription with Rule Filter_ - CS)
	<!-- mark:6 -->
	````C#
	[HttpPost]
	public JsonResult CreateTopic(string topicName)
	{
		var topic = this.namespaceManager.CreateTopic(topicName);
		var allMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "AllMessages");
		var urgentMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "UrgentMessages", new SqlFilter("Urgent = '1'"));
		
		return this.Json(topicName, JsonRequestBehavior.AllowGet);
	}
	````

    > **Note:** Take into account that you can use SQL92 as filter expressions.

1. Press **CTRL + S** to save the changes to the Controller class.

<a name="Ex3Task2"></a>
#### Task 2 - Using a Subscription Rule Filter Action ####

Additionally to rule filter expressions, you can use **rule filter actions.** With this, you can modify the properties of a **BrokeredMessage** that matches the specified rule. You will create a new subscription named _HighPriorityMessages_ containing a custom rule filter action. All messages that match the rule _Urgent = '1'_ will be sent to that subscription with the **Priority** property set to _High_.

> **Note:** Both filter expressions and filter actions use the properties declared in the **BrokeredMessage** dictionary named **Properties**. These rules won't apply on custom objects inside the body of the **BrokeredMessage.**

1. If not already opened, open the **HomeController.cs** file under the **Controllers** folder in the **UsingTopics** project.

1. Create a new **subscription** object with a **RuleDescription**. Within this object, you can set a **filter** and an **action**. This way, if the **filter** matches, the specific **action** is applied to the **BrokeredMessage**. Add the highlighted code to the **CreateTopic** action method.

	(Code Snippet - _Service Bus Topics - Ex03 - Add Subscription with Action Filter_ - CS)
	<!-- mark:7-14 -->
	````C#
	[HttpPost]
	public JsonResult CreateTopic(string topicName)
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

		 return this.Json(topicName, JsonRequestBehavior.AllowGet);
	}
	````

1. Add the following action method to retrieve the subscription filters for a given subscription to the view.

	(Code Snippet - _Service Bus Topics - Ex03 - Get Subscription Filters_ - CS)
	<!-- mark:1-22 -->
	````C#
	[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
	public JsonResult Filters(string topicName, string subscriptionName)
	{
		var rules = this.namespaceManager.GetRules(topicName, subscriptionName);
		var sqlFilters = new List<Tuple<string, string>>();

		foreach (var rule in rules)
		{
			var expression = rule.Filter as SqlFilter;
			var action = rule.Action as SqlRuleAction;

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

	````

1. Press **CTRL + S** to save the changes to the Controller class.

<a name="Ex3Task3"></a>
#### Task 3 - Verification ####

You will now run the updated application one more time to verify that each message sent will go to the subscription that matches the correct filter.

1. In **Visual Studio**, press **F5** to launch the application.

1. In the **Create a Topic** section, enter _topicwithrules_ for the topic name, and click **Create**.

1. Select the previously created topic from the topic list. In the **Send a message section**, type _This is an urgent message_ in the **Message** textbox, select the **Urgent** checkbox and click **Send**.
 
 	![Sending an urgent message to the topic](Images/sending-an-urgent-message-to-the-topic.png?raw=true)
 
	_Sending an urgent message to the topic_

1. Check that the message is received only by the **UrgentMessages** and the **AllMessages** subscriptions. Alternatively, you can select each subscription and click **Receive** to verify that the message is retrieved.

	![Urgent message arriving to UrgentMessages and AllMessages subscriptions](Images/urgent-message-arriving.png?raw=true)

	_Urgent message arriving to UrgentMessages and AllMessages subscriptions_

1. Send another message to the topic, but this time, unselect the **Urgent** checkbox and select the **Mark as Important** checkbox.

 	![Sending an important message to the topic](Images/sending-an-important-message-to-the-topic.png?raw=true)
 
 	_Sending an important message to the Topic_

1. Retrieve the message from the **HighPriorityMessages** subscription and verify that the **Priority** is now set to _High_.

	![Important message received from the HighPriorityMessages subscription](Images/important-message-received.png?raw=true)

	_Important message received from the HighPriorityMessages subscription_

---

<a name="NextSteps" />
## Next Steps ##

To learn more about **Service Bus Topics** and **Service Bus Messaging** please refer to the following articles:

**Technical Reference**

This is a list of articles that expand on the technologies explained on this lab:

- [An Introduction to Service Bus Topics article on the AppFabrik Team Blog](http://aka.ms/Qfzy2g): provides an introduction to the publish/subscribe capabilities offered by Service Bus Topics.

- [Service Bus Queues, Topics, and Subscriptions](http://aka.ms/Jed5rg): the new release of the Windows Azure Service Bus adds a set of cloud-based, message-oriented-middleware technologies including reliable message queuing and durable publish/subscribe messaging. These “brokered” messaging capabilities can be thought of as asynchronous, or decoupled messaging features that support publish-subscribe, temporal decoupling, and load balancing scenarios using the Service Bus messaging fabric. 

- [Partitioned Service Bus Queues and Topics](http://aka.ms/Sy2ssi): whereas a conventional queue or topic is handled by a single message broker and stored in one messaging store, a partitioned queue or topic is handled by multiple message brokers and stored in multiple messaging stores. This means that the overall throughput of a partitioned queue or topic is no longer limited by the performance of a single message broker or messaging store

- You can continue reading the **Service Bus Queues** hands-on lab.

**Development**

This is a list of developer-oriented articles related to **Service Bus Topics**:

- [How to Use Service Bus Topics/Subscriptions](http://aka.ms/Hz4ja5): will show you how to use Service Bus topics and subscriptions. The samples are written in C# and use the .NET API.

- [Creating Applications that Use Service Bus Topics and Subscriptions](http://aka.ms/U0fsy9): offers an introduction to the publish/subscribe capabilities offered by Service Bus topics.

- [SqlFilter Class](http://aka.ms/Fugit9): represents a filter which is a composition of an expression and an action that is executed in the pub/sub pipeline

---

<a name="Summary" />
## Summary ##

 By completing this hands-on lab, you have reviewed the basic elements of Service Bus topics and subscriptions. You have seen how to create topics and subscriptions, send messages to a topic and receive messages from subscriptions. Finally, you learned how to apply expression filters and rule actions to subscriptions to distribute your messages that matched those rules.