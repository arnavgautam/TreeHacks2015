<a name="segment4" />
### Segment 4: Scaling with Windows Azure Caching ###

In this segment, you will improve the scalability and performance of your Windows 8 and web applications by using the new Windows Azure Caching support.  

1. The BuildClips Web app and the Windows 8 application should be running from the previous demo.  

1. In the Windows 8 app, open the **charms bar** and select **Settings**.

	> **Speaking Point:** Our Windows 8 application has a development setting that we can enable to measure the execution time of calls to services. 

1. Click the **Profiling** option. 

1. Set **Enable Profiling** to **On**.

	> **Speaking Point:** After you enable profiling, you can see the execution time for calling out to the Web APIs in the application.  We will use Windows Azure Caching to improve the response time for these calls and improve the scalability for our application by not querying the database on each request. 

	> It should be pointed out that we are not measuring the overall response time of the request from the client application's perspective. Instead, we'll just show the impact that caching has on the service by measuring the execution time on the server. In other words, the time taken to process a request, from the moment it's received until a response is ready to be sent out to the network.

	!["Enabling profiling"](Images/enabling-profiling.png?raw=true "Enabling profiling")

	_Enabling profiling_

1. Switch to the Visual Studio 2012 instance that has the BuildClips web project already open and stop the solution.  

	> **Speaking Point:** Let's enable Windows Azure Caching for our cloud service. 

1. Expand the **BuildClips.Azure** project and double-click the **BuildClips** web role to open its **Properties** window. Switch to the **Caching** page and then select **Enable Caching** for the web role.

	> **Speaking Point:** To do this, we'll simply navigate to the properties for our web role. Here you can see on the caching tab that I can simply turn on caching support for the role.  

	![Enabling caching in Web Role properties](Images/web-role-properties.png?raw=true "Enabling caching in Web Role properties")

	_Enabling caching in Web Role properties_

	> **Speaking point:** We can tell Windows Azure to use a percentage of the available memory in the web role. 
	
1. Press **CTRL** + **S** to save the changes and close the Properties window.

1. Right-click the **solution** and then open the **NuGet Package Manager**.

	> **Speaking point:** Now we need to reference the Windows Azure Caching assemblies so we can interact with the cache from our application.  To do this, we'll simply right-click our solution to pull up the Manage NuGet Packages dialog and install the Windows Azure Caching NuGet package into our projects.

	![Manage nuget packages for solution](Images/manage-nuget-packages-for-solution.png?raw=true)

	_Manage nuget packages for solution_

1. In the **NuGet Package Manager**, expand the **Online** node and search for the **Windows Azure Caching** package.

1. Select package, click **Install** and confirm to install the package **in the three projects**. Close the package manager.
	
	![Install caching nuget](Images/install-caching-nuget.png?raw=true)

	_Install caching nuget_

1. Open **Web.config** in the **BuildClips** project. Locate the `<dataCacheClients>` element and replace the `[cache cluster role name]` placeholder with **BuildClips**. 

1. Uncomment the `<localCache>` XML element.

	> **Speaking point:** The NuGet package will also add a few configuration settings in our Web.config file.  We need to update one of the settings to tell the application to use our web role for caching. 

	<!-- mark:4,5 -->
	````XML
	  </entityFramework>
	  <dataCacheClients>
		 <dataCacheClient name="default">
			<autoDiscover isEnabled="true" identifier="BuildClips" />
			<localCache isEnabled="true" sync="TimeoutBased" objectCount="100000" ttlValue="300" />
		 </dataCacheClient>
	  </dataCacheClients>
	  <cacheDiagnostics>
		 <crashDump dumpLevel="Off" dumpStorageQuotaInMB="100" />
	  </cacheDiagnostics>
	</configuration>
````

1. Open **app.config** in the **BackgroundService** project. Locate the `<dataCacheClients>` element and replace the `[cache cluster role name]` placeholder with **BuildClips**.

1. Uncomment the `<localCache>` XML element.
 
	> **Speaking point:** We also need to do the same in the configuration for our worker role.  

	<!-- mark:4,5 -->
	````XML
	  </system.diagnostics>
	  <dataCacheClients>
		 <dataCacheClient name="default">
			<autoDiscover isEnabled="true" identifier="BuildClips" />
			<localCache isEnabled="true" sync="TimeoutBased" objectCount="100000" ttlValue="300" />
		 </dataCacheClient>
	  </dataCacheClients>
	  <cacheDiagnostics>
		 <crashDump dumpLevel="Off" dumpStorageQuotaInMB="100" />
	  </cacheDiagnostics>
	  <system.web>
````
	> **Speaking point:** Now let's write some code in our service layer to store data in Windows Azure Caching when we retrieve the list of videos from the database.

1. Open **VideoService.cs** in the service library and replace the current implementation of the **GetAll** method with the following code.
	
	> **Speaking point:** To do this, let's update our GetAll method so that we use caching.  Here, for the sake of time, I'll use a code snippet to pull in a bit of code.  This code will get a reference to our cache and check to see if the video metadata has already been cached. If it has been cached, then we will use the cached data.  However, it the data is not in cache, we will proceed to query the database and add the results to our cache, so they are available for the next request. In a more realistic scenario, retrieving information from the database would involve going out into the network but since we are running everything locally, you can see that we have intentionally made this more expensive by introducing an arbitrary delay.  

	(Code Snippet - _VideoService.cs - GetAll - Caching_)
	<!-- mark:3-15 -->
	````C#
	public IQueryable<Video> GetAll()
	{
		 var dataCache = new DataCache();
		 var videos = dataCache.Get("videoList") as IEnumerable<Video>;

		 if (videos == null)
		 {
			  videos = this.context.Videos.OrderByDescending(v => v.Id);

			  dataCache.Put("videoList", videos.ToList());

			  Thread.Sleep(500);
		 }

		 return videos.AsQueryable();
	}
    ````

1. Select the **DataCache** type and press **CTRL+.** to add the using statement.

1. Now add the following highlighted code in the **Publish** method to invalidate the cache when a video is updated.

	> **Speaking point:** So our app is now updated to cache the videos. However, we do need to also update the cache whenever videos change.  As we saw previously, we have a worker role that is watching for changes to the video status.  So let's update the Publish method that the worker role invokes to remove the data from the cache whenever the video changes, causing the next request to reload data from the database. Strictly speaking, we should also apply code to invalidate the cache in the methods that create or delete a video but we'll skip this change for the time being.

	(Code Snippet - _VideoService.cs - Publish - Caching_)
	<!-- mark:6-7 -->
	````C#
	public void Publish(int id)
   {
		...
		this.context.SaveChanges();

		var cache = new DataCache();
		cache.Remove("videoList");
	}
	````

1. Open **WebApiConfig.cs** in the **App_Start** folder of the **BuildClips** project and insert the following highlighted code into the **Register** method. Then place the cursor over the **ApiExecutionProfiler** type and press **CTRL+.** to add the using directive.

	(Code Snippet - _WebApiConfig.cs - Register - ApiExecutionProfiler_)
	<!-- mark:8-9 -->
	````C#
	public static void Register(HttpConfiguration config)
   {
       config.Routes.MapHttpRoute(
           name: "DefaultApi",
           routeTemplate: "api/{controller}/{id}",
           defaults: new { id = RouteParameter.Optional });

       Action clearCache = () => new Microsoft.ApplicationServer.Caching.DataCache().Clear();
       config.MessageHandlers.Add(new ApiExecutionProfiler(clearCache));
	}
	````
	> **Speaking point:** For this demo, we'll update the Web API configuration to insert a message handler that will profile the execution of each service call. The handler will also allow clearing the data cache on demand. This is not something that you would normally have in your applications but we'll use this feature to test out caching.

1. Press **F5** to run the application in the compute emulator.

1. Switch back to the Windows 8 application.

	> **Speaking point:** Now let's switch back to our Windows 8 app that is already running.  

1. Right-click the application to open the action bar and click **Refresh** to update the list of videos. Point out the response time for the request. 

	> **Speaking point:** Let's refresh our list of videos. Since this is our first request following a cold start of the application, the measured time also includes the warm-up time for the cache, database connection initialization, and other factors that make it unsuitable for representing the cache miss scenario accurately, so we'll skip this first reading.

	![Refreshing the video list](Images/refresh.png?raw=true "Refresh")

	_Refreshing the video list_

1. In the Windows 8 app, open the **charms bar** and select **Settings**. 

1. Now, select the **Profiling** option and click **Clear Cache**.

	![Clear cache](Images/clear-cache.png?raw=true "Clear Cache")

	_Clear cache_

	> **Speaking point:** Let's start from a known state by clearing the cache. This will remove all items from the cache.  

1. Now refresh the list again. 

	> **Speaking point:** Let's refresh our list of videos. Since the cache is now empty, the service will need to go back to the database to retrieve the list of videos and then it will add them to the cache. Notice that it's taking a significant amount of time to do this. 

	![Response time with cache miss](Images/response-time-slow.png?raw=true "Response time")

	_Response time with cache miss_

1. Refresh the list of videos again one or more times to show that caching response time has decreased.

	> **Speaking point:** Now you can see that the response time has significantly improved. 

	![Response time with cache hit](Images/response-time-fast.png?raw=true "Response time")

	_Response time with cache hit_

1. Clear the cache again to force a cache miss and repeat the sequence.

1. Stop both the Web and Windows 8 applications.
