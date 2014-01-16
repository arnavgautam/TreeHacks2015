<a name="HOLTop"></a>
# Service Bus Topics#
---

<a name="Overview"></a>
## Overview ##

// TODO: Review Overview

**Service Bus Topics** contains a brand-new set of cloud-based, message-oriented-middleware technologies including a fully-featured **Message Queue** with support for arbitrary content types, rich message properties, correlation, reliable binary transfer, and grouping. Another important feature is **Service Bus Topics** which provide a set of publish-and-subscribe capabilities and are based on the same backend infrastructure as **Service Bus Queues**. A **Topic** consists of a sequential message store just like a **Queue**, but allows for many concurrent and durable **Subscriptions** that can independently yield copies of the published messages to consumers. Each **Subscription** can define a set of rules with simple expressions that specify which messages from the published sequence are selected into the Subscription.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create Topics and Subscriptions.
- Use Subscription Filter Expressions.
- Use Subscription Filter Actions.

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

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2013 to avoid having to add it manually.

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating a Topic and Adding Subscriptions](#Exercise1)
1. [Using a Subscription Rule Filter Expression and Rule Filter Actions](#Exercise2)
1. [Sending and Receiving Messages](#Exercise3)

Estimated time to complete this lab: **60 minutes**.

> **Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

<a name="Exercise1"></a>
### Exercise 1: Creating a Topic and Adding Subscriptions ###

In this exercise, you will learn how to create a Windows Azure Service Bus topic and add subscriptions to it. Topics and subscriptions provide a one-to-many form of communication, in a “publish/subscribe” pattern. Useful for scaling to very large numbers of recipients, each published message is made available to each subscription registered with the topic.

<a name="Ex1Task1"></a>
#### Task 1 - Creating your Service Bus Namespace ####

To work with Service Bus topics and subscriptions, you first need to create a Windows Azure Service Bus namespace. Once created, it can be used for **all** of the labs that use Windows Azure Service Bus and for your own projects as well.

1. Navigate to [http://manage.windowsazure.com/](http://manage.windowsazure.com). You will be prompted for your **Microsoft Account** credentials if you are not already signed in.

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

1. In the **Access connection information** dialog box, record the value shown for **Default Issuer** and **Default Key**, and click **OK**. You will need these values later when configuring your Web Role settings.

 	![Service Bus default issuer and key](Images/service-bus-default-issuer-and-key.png?raw=true)
 
	_Service Bus default keys_

You have now created a new Windows Azure namespace for this hands-on lab. To sign in at any time, simply navigate to the Windows Azure Management Portal, click **Sign In** and provide your **Microsoft Account** credentials.

> **Note:** In this lab you will learn how to create and make use of Service Bus  topics and subscriptions from Visual Studio and from an ASP.NET MVC Application. You can also create topics and subscriptions from the Windows Azure Management Portal, for more information see [How to Manage Service Bus Messaging Entities](http://www.windowsazure.com/en-us/documentation/articles/service-bus-manage-message-entities/).

<a name="Ex2Task1"></a>
#### Task 1 - Creating a Topic and Adding Subscriptions Programmatically ####

In this task, you will learn how to create a new topic and add several subscriptions to it. For this, first you will add the necessary configurations to connect to your Service Bus namespace.

1. Open **Visual Studio 2013 Express for Web** as Administrator.

1. Open the **Begin.sln** solution file from **Source\Ex1-CreatingATopicAndAddingSubscriptions\Begin\**.

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


<a name="Exercise2"></a>
### Exercise 2: Using a Subscription Rule Filter Expression and Rule Filter Actions ###

In this exercise, you will apply filters on subscriptions to retrieve only the messages relevant to that subscription. When you send a message to a topic, all the subscriptions verify if the message has a match with its own subscription rules. If there is a match, the subscription will contain a virtual copy of the message. This is useful to avoid sending multiple messages to different subscriptions. Sending a single message to a topic will distribute along different subscriptions by checking **Rule Expressions**. Additionally, you will learn how to apply **Filter Actions** to subscriptions to modify the **BrokeredMessage** properties of the messages that match a custom rule.


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

<a name="Exercise2"></a>
### Exercise 3: Sending and Receiving Messages ###

In this...

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