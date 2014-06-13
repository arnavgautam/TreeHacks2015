<a name="title" />
# Twitter Reader Demo #

---

<a name="Overview" />
## Overview ##

Microsoft Azure Caching is a distributed cache that can store your application's data in memory to improve application responsiveness, performance, and scale. It provides a low latency caching layer that allows data to be shared across multiple machines in a Microsoft Azure cloud service. Any data put into the cache by one server can be accessed by another server.

The cache is designed to dynamically grow and shrink allowing you to easily increase and decrease the cache size depending on the application's needs.

When you enable multiple caching role nodes in your application, Microsoft Azure Caching supports high availability. Data placed in the cache is automatically copied to at least two other nodes so that the information exists in multiple servers and can be recovered even if one of the nodes crashes.

Microsoft Azure Caching allows your application to take advantage of the full AppFabric API. Moreover, it also supports memcached--a popular open source caching protocol--so any code written against it can be used without changes.

In this demo, we will explore the caching capabilities built into Microsoft Azure and specifically, its low latency in-memory distributed cache.

<a id="goals" />
### Goals ###
In this demo, you will see how to:

1. Enable caching in your Microsoft Azure cloud service

1. Update your application to read from and write to the cache

1. Improve the responsiveness of your application

<a name="technologies" />
### Key Technologies ###
- Microsoft Visual Studio 2012
- [Microsoft Azure Caching][1]

[1]: http://msdn.microsoft.com/en-us/library/windowsazure/hh914153

<a name="setup" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment.

1. Open Windows Explorer and browse to the demo's **Source** folder.

1. Execute the **Setup.Local.cmd** file with Administrator privileges to launch the setup process that will verify all the prerequisites and install the Visual Studio code snippets for this demo.

1. If the User Account Control dialog is shown, confirm the action to proceed.

>**Note 1:** Make sure you have all the dependencies for this demo checked before proceeding.

> **Note 2:** The setup script copies the source code for this demo to a working folder that can be configured in the **Config.Local.xml** file (by default, C:\Projects). From now on, this folder will be referenced in this document as the **working folder**.

---

<a name="Demo" />
## Demo ##

This demo contains the following segments:

1. [Improving Performance using Microsoft Azure Caching](#segment1)

<a name="segment1" />
### Improving Performance using Microsoft Azure Caching ###

1. Launch Visual Studio 2012 **as an administrator**.

1. Click File | Open | Project/Solution and open the **Begin.sln** solution file located at  **[working folder]\TwitterReader\Begin**.

1. Press **F5** to run the application.

	![Running the application](Images/running-the-application.png?raw=true "Running the application")

	_Running the application_

	> **Speaking Point**
	>
	> I will demonstrate the caching features using a simple app named TwitterReader that basically reads tweets from Twitter in real time and shows them in a browser window.


1. Click the **Twitter Feed** link. Observe the slow response time.

	> **Speaking Point**
	>
	> A timer on the page shows how long it takes to render the page. As you can see, the elapsed time is variable, but it usually takes more than a second to retrieve the information from Twitter. In practice, taking this long to call another service can quickly become a bottleneck when you have many users accessing the application.

1. Stop the application.

	![Retrieving Tweets](Images/retrieving-twitts.png?raw=true "Retrieving Tweets")

	_Retrieving Tweets_

1. Open the **TwitterReader** role's properties page. To do this, double-click the **TwitterReader** role inside the **Roles** folder.

1. In the role properties window, click the **Caching** tab and then select **Enable Caching (Preview Release)**.

	![Enabling Caching in a Microsoft Azure Role](Images/enabling-caching-in-windows-azure-role.png?raw=true "Enabling Caching in Microsoft Azure Role")

	_Enabling Caching in a Microsoft Azure Role_

	> **Speaking Point**
	>
	> Instead of retrieving the feed from Twitter each time, we could store the tweets in a cache and only invalidate the data periodically, say every 5 or 10 minutes. Using the distributed caching capability built into Microsoft Azure this is really easy to do. I can just edit the settings for my web role and enable the distributed cache. All I need to do is click the “Enable caching” checkbox. This sets up a cache in the VMs and allows me to choose how much memory I want to use. 

1. In Solution Explorer, right-click the **TwitterReader** project, and then point to **Manage NuGet Packages**.

1. Ensure **Online** is selected in the left pane. In the Search box, type _Microsoft.WindowsAzure.Caching_ and then press **Enter**.

1. Now, in the results list, select **Microsoft Azure Caching Preview** and click **Install**.

1. In the **License Acceptance** dialog, click **I Accept** to complete the installation.

	![Microsoft Azure Caching Preview NuGet Package](Images/windows-azure-caching-preview-nuget-package.png?raw=true "Microsoft Azure Caching Preview NuGet Package")

	_Microsoft Azure Caching Preview NuGet Package_

	> **Speaking Point** 
	>
	> I'm now going to use the NuGet package manager to add the caching assemblies to my .NET solution. 

1. Open the **Web.config** file, locate the **dataCacheClients** section and replace **[cache cluster role name]** with _TwitterReader_.

1. Next, uncomment the **localCache** element in the same section.

	![Updating TwitterReader Web.config](Images/twitterreader-webconfig.png?raw=true "Updating TwitterReader Web.config")

	_Updating the TwitterReader Web.config file_

	> **Speaking Point** 
	>
	> Next, I need to update the cache configuration in my Web.config file to point to the app. TwitterReader is the name where my cache is located and I am also going to enable the local cache option. 


1. Open **TwitterFeedController.cs** in the **Controllers** folder.

1. Locate the **getTweets** method and remove the entire method body, replacing it with the following highlighted code.

	<!-- strike:3;mark:5-13 -->
	````C#
	Tweets getTweets(string name)
	{
		return TwitterFeed.GetTweets(name);

		DataCache cache = new DataCache();
		Tweets entries = cache.Get(name) as Tweets;
		if (entries == null)
		{
			 entries = TwitterFeed.GetTweets(name);
			 cache.Add(name, entries);
		}

		return entries;
	}
	````

	> **NOTE**: You can use the Visual Studio code snippet included in this demo.

1. Include the following namespace declaration for **DataCache**.
	
	<!-- mark:1 -->
	````C#
	using Microsoft.ApplicationServer.Caching;
	````

	> **Speaking Point** 
	>
	> Now, we need to update the application to read from and write to the cache. As you see, the controller currently always reads the tweets directly from Twitter.
I am going to replace that code with one that checks whether or not the tweets are already in the cache. If so, it just returns them; otherwise, it gets them from Twitter and then adds them to the distributed cache.

1. Press **F5** to build and run the solution.

1. Show how the initial load is just as slow. Then, refresh the browser and see that it is now much faster as the cache kicks in.

	![Application Running with Caching](Images/application-running-with-caching.png?raw=true "Application Running with Caching")

	_Application Running with Caching Enabled_

	> **Speaking Point** 
	>
	> And now, I can rerun my application. The first time, because the data has yet to be cached, the application needs to retrieve the tweets by reading the Twitter feed, so it takes several seconds to render the page. If I now refresh the page, you can see how the cache kicks in and the page renders in less than a millisecond. This is because the application is now taking advantage of the distribute cache to pull all the information from the in-memory cache, waiting every ten minutes or so before retrieving fresh data from Twitter.

---

<a name="summary" />
## Summary ##

In this demo, you saw how to take advantage of Microsoft Azure Caching to enable a distributed in-memory cache that can improve your application's responsiveness, performance, and scale. 
