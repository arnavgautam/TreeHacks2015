<a name="HOLTop" />
# Building Windows Azure Cloud Services with Cache Service #

---

<a name="Overview" />
## Overview ##

Windows Azure Cache Service provides a distributed, cost-effective in-memory cache for your Cloud Services. With Cache Service enabled on your Cloud Services roles, you can utilize spare memory on your service hosts as high performance cache to improve response time and system throughput. And because the cache hosts are collocated with your Cloud Service roles, you get optimal access time by avoiding external service calls. In this lab, you will learn how easy it is to enable Cache Service on your Cloud Services roles, and how to use Cache Service to provide high performance in-memory caching to your Cloud Services.

<a name="Objectives" />
### Objectives ###
In this hands-on lab, you will learn how to:

- Easily and quickly enable Cache service.
- Use Cache Service for your Asp.Net session state.
- Cache reference data from Windows Azure SQL Database in Cache Service.
- Create a reusable and extensible caching layer for your Cloud Services.

During this lab, you will explore how to use these features in a simple Asp.Net MVC4 application.

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Microsoft Visual Studio 2012 Express for Web][1] or higher
- [Windows Azure Tools for Microsoft Visual Studio 2.0][2]
- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

[1]: http://www.microsoft.com/visualstudio/
[2]: http://www.windowsazure.com/en-us/develop/downloads/
[3]: http://aka.ms/WATK-FreeTrial
[4]: http://www.microsoft.com/en-us/download/details.aspx?id=38188

<a name="Setup" />
### Setup ###
In order to run the exercises in this hands-on lab you need to set up your environment first.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.
1. Right-click on **Setup.cmd** and select Run as Administrator to launch the setup process that will configure your environment and install the Visual Studio code snippets for this lab.
1. If the User Account Control dialog is shown, confirm the action to proceed.

> **Note:** Make sure you have checked all the dependencies for this lab before running the setup.

> This lab requires a Windows Azure SQL Database to start. To build the Northwind2 database automatically, the **Setup.cmd** file will prompt you with your Windows Azure SQL Database account information. Remember to update the NorthwindEntities connection string in the application's configuration file to point to your database for each solution.

> Remember to configure the firewall setting of your Windows Azure SQL Database account. You can enable a list of IP addresses that can access your Windows Azure SQL Database Server. The firewall will deny all connections by default, so **be sure to configure your allow list** so you can connect to the database. Changes to your firewall settings can take a few moments to become effective. For more information, see [How to Create and Configure a SQL Database](http://www.windowsazure.com/en-us/documentation/articles/sql-database-create-configure).

>![SQL database setup](Images/sql-database-setup.png?raw=true "Windows Azure SQL Database setup")

>_Windows Azure SQL Database setup_


<a name="CodeSnippets" />
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2013 to avoid having to add it manually. 

>**Note**: Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise. Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.

---

<a name="Exercises" />
## Exercises ##
This hands-on lab includes the following exercises:

1. [Enable Cache service for Session State](#Exercise1)
1. [Caching Data with Windows Azure Caching](#Exercise2)
1. [Caching common data patterns with WACEL](#Exercise3)

Estimated time to complete this lab: **60 minutes**.

>**Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

<a name="Exercise1" />
### Exercise 1: Enable Cache service for Session State ###

In this exercise, you will explore the use of the session state provider for Cache service as the mechanism for out-of-process storage of session state data. For this purpose, you will use **Cloud Shop**, a sample shopping cart application implemented with ASP.NET MVC4. You will run this application in the compute emulator and then modify it to take advantage of the Windows Azure Cache service as the back-end store for the ASP.NET session state. You will start with a begin solution and explore the sample using the default ASP.NET in-proc session state provider. Next, you will add references to the Cache assemblies and configure the session state provider to store the contents of the shopping cart in the distributed cache cluster provided by Cache service.

<a name="Ex1Task1" />
#### Task 1 - Running the Cloud Shop Sample Site in the Compute Emulator ####

In this task, you will run the Cloud Shop application in the compute emulator using the default session state provider; you will change that provider to take advantage of the Windows Azure Cache service later on.

1. Start **Microsoft Visual Studio 2013 Express for Web** as administrator.
1. Open the **Begin** solution located at **Source\\Ex1-CacheSessionState\\Begin**.

	>**Important:** 	Before you execute the solution, make sure that the startup project is set. For MVC projects, the start page must be left blank.

	>To set the startup project, in **Solution Explorer**, right-click the **CloudShop.Azure** project and select **Set as StartUp Project**.
	
	>To set the start page, in **Solution Explorer**, right-click the **CloudShop** project and select **Properties**. In the **Properties** window, select the **Web** tab and in the **Start Action**, select **Specific Page**. Leave the value of this field blank.

1. In the **Web.config** file, update the _NorthwindEntities_ connection string to point to your database. Replace **[YOUR-SQL-DATABASE-SERVER-ADDRESS]**, **[SQL-DATABASE-USERNAME]**, and **[SQL-DATABASE-PASSWORD]** in the connectionStrings section with the Windows Azure SQL Database server name, administrator username and administrator password that you registered at the portal and used for creating the database during setup.

	>**Note:** Make sure that you followed the instructions of the setup section to create a copy of the Northwind2 database in your own Windows Azure SQL Database account and configure your Windows Azure SQL Database firewall settings.

1. Press **CTRL** + **F5** to build and run the application without debugging in the compute emulator. 

	>**Note:** Make sure that you run the application without debugging. With debugging mode you won't be able to recycle the web role

1. Explore the main page of the application, the **Products** page, which displays a list of products obtained from a Windows Azure SQL Database.

	![Cloud Shop products page](Images/azure-store-products-page.png?raw=true "Cloud Shop products page")

	_Cloud Shop products page_

1. Select a product from the list and click **Add item to cart**. You may repeat the process to store additional items in the shopping cart.

1. Click the **Checkout** link to view the contents of the cart. Verify that the items you selected appear on the list. These items are stored in the current session.

	![Checkout page showing the contents of the shopping cart](Images/checkout-page-showing-the-contents-of-the-sho.png?raw=true "Checkout page showing the contents of the shopping cart")

	_Checkout page showing the contents of the shopping cart_

1. Navigate back to **Products** page.

1. Click on **Recycle** link. This link forces the web role to be recycled. Once you click on the link, the Products page will turn blank.

1. In the **Compute Emulator**, observe how the web role is recycled by the emulator:

	![Suspending the service role instance](Images/suspending-the-service-role-instance.png?raw=true "Suspending the service role instance")

	_Web role recycled_

1. Go back to the browser. Remove */Home/Recycle* from the address bar, and then press **Enter** to reload the site. The **Products** page should come back normal after a short delay.

1. Navigate to **Checkout** page. Notice that the order now appears empty.

	>**Note:** The application is currently using in-proc session state, which maintains the session state in-memory. When you stop the service instance, it discards all session state including the contents of the shopping cart. In the following task, you will configure the application to store session state using Windows Azure Caching as the storage mechanism, which allows the application to maintain the session state in the presence of restarts and across multiple role instances hosting the application.

1. Close the browser window to stop the application.

<a name="Ex1Task2" />
#### Task 2 - Adding a dedicated caching role ####
In this task, you will add a new worker role that serves as a dedicated cache host. All other web roles and worker roles in the Cloud Service will be able to access the Cache service hosted by this role. You can set up multiple dedicated worker roles within your Cloud Service. In addition, you can also enable Cache service on any of the existing roles and allocate certain percentage of virtual machine memory to be used as cache. 

1. In solution explorer, expand **CloudShop.Azure** node, and then right-click on **Roles**. Then, select **Add** | **New Worker Role Project...**.

1. In **Add New .NET Framework 4.5 Role Project** dialog, select **Cache Worker Role** template. Name the role as **CacheWorkerRole**, and then click **Add**.

	>**Note:** All Cache hosts in your Cloud Service share their runtime states via a Windows Azure Blob Storage. By default, a cache worker role is configured to use development storage. You can change this setting in the **Caching** tab on the role property page. 

1. Press **CTRL + SHIFT + S** to save all the items.

<a name="Ex1Task3" />
#### Task 3 - Configuring Session State Using Windows Azure Cache service ####

In this task, you will change the Session State provider to take advantage of the Windows Azure Cache as the storage mechanism. This requires adding the appropriate assemblies to the **CloudShop** project and then updating the corresponding configuration in the **Web.config** file. 

1. In Visual Studio 2013 Express for Web, open **Package Manager Console** from **Tools** | **Library Package Manager** | **Package Manager Console** menu.

1. Make sure that **CloudShop** is selected in the **Default project** drop-down list. Issue the following command to install the Nuget package for Cache service:
 
	````PowerShell
	Install-package Microsoft.WindowsAzure.Caching
	````
   
1. Open the **Web.config** file located in the root folder of the **CloudShop** project.
1. In the **dataCacheClient** tab, change **[cache cluster role name]** to **CacheWorkerRole**.

	<!--mark: 3-->
	```` XML
	<dataCacheClients>
      <dataCacheClient name="default">
       <autoDiscover isEnabled="true" identifier="CacheWorkerRole" />
      <!--<localCache isEnabled="true" sync="TimeoutBased" objectCount="100000" ttlValue="300" />-->
      </dataCacheClient>
  </dataCacheClients>
	  ...
	````

1. Add a new session state provider configuration under System.Web tag:  

	<!--mark: 3-9-->
	````XML
	<system.Web>
	...
	<sessionState mode="Custom" customProvider="NamedCacheBProvider">
      <providers>
        <add cacheName="default" name="NamedCacheBProvider" 
             dataCacheClientName="default" applicationName="MyApp" 
             type="Microsoft.Web.DistributedCache.DistributedCacheSessionStateStoreProvider, Microsoft.Web.DistributedCache" />
      </providers>
    </sessionState>
	...
	</system.web>
   ````

	>**Note:** You can replace the commented settings that were added when installing **Microsoft.WindowsAzure.Caching** package.
 
1. Press **CTRL + S** to save your changes to the **Web.config** file.

<a name="Ex1Task4"></a>
#### Task 4 - Verification ####

1. Press **Ctrl + F5** to build and run the application. Wait for the browser to launch and show the **Products** page.

1. Select one product from the list and click **Add item to cart**. Repeat the process to store additional items in the cart.

1. Click the **Checkout** link to view the contents of the shopping cart. Verify that the items you selected appear on the list.

1. Navigate back to **Products** page and click on **Recycle** link.

1. Observe the web role getting recycled in **Show Compute Emulator UI**. 

1. Go back to browser, remove */Home/Recycle* from address, and then press Enter to reload the site.

1. **Products** page should load correctly. Navigate to **Checkout** page. Notice that the order is intact. This confirms that with the Windows Azure Caching provider, the session state is stored outside the role instance and can persist through application restarts.

	> **Note:** You should infer from the verification that, if your application is hosted in multiple servers or Windows Azure role instances, and a load balancer distributes requests to the application, clients would continue to have access to their session data regardless of which instance responds to the request.

1. Close the browser window to stop the application.

<a name="Exercise2" />
### Exercise 2: Caching Data with Windows Azure Caching ###

This exercise will show you how to use Windows Azure Caching to cache results from queries to Windows Azure SQL Database. You will continue with a solution based on the one used for the previous exercise. The only difference is in the home page, which has been updated to show the elapsed time to retrieve the list of products in the catalog, and now has a button to enable or disable the use of the cache.

During the exercise, you will update the data access code with a trivial implementation of caching. It uses the canonical pattern, in which the code checks the cache first to retrieve the results of a query and, if there is no data available, executes the query against the database to cache the results.

<a name="Ex2Task1" />
#### Task 1 - Caching Data Retrieved from the SQL Database ####

To make use of Windows Azure Caching, you first need to create a **DataCacheFactory** object. This object determines the cache cluster connection information, which is set programmatically or by reading settings from the configuration file. Typically, you create an instance of the factory class and use it for the lifetime of the application. To store data in the cache, you request a **DataCache** instance from the **DataCacheFactory** and then use it to add or retrieve items from the cache.
In this task you will update the data access code to cache the result of queries to Windows Azure SQL Database using Windows Azure Caching. 

1. Start **Microsoft Visual Studio 2013 Express for Web** as administrator.
1. Open the **Begin** solution located at **Source\\Ex2-CachingData\\Begin**.

	>**Important:** Before you execute the solution, make sure that the startup project is set. For MVC projects, the start page must be left blank. 
	> To set the startup project, in **Solution Explorer**, right-click the **CloudShop.Azure** project and then select **Set as StartUp Project**. 
	> To set the start page, in **Solution Explorer**, right-click the **CloudShop** project and select **Properties**. In the **Properties** window, select the **Web** tab and in the **Start Action**, select **Specific Page**. Leave the value of this field blank.

1. In the **Web.config** file, update the _NorthwindEntities_ connection string to point to your database. Replace **[YOUR-SQL-DATABASE-SERVER-ADDRESS]**, **[SQL-DATABASE-USERNAME]**, and **[SQL-DATABASE-PASSWORD]** with the Windows Azure SQL Database server name, administrator username and administrator password that you registered at the portal and used for creating the database during setup.

	> **Note:** 	Make sure that you followed the instructions of the setup section to create a copy of the Northwind2 database in your own Windows Azure SQL Database account and configure your Windows Azure SQL Database firewall settings.

1. Open the **ProductsRepository.cs** file in the **Services** folder of the **CloudShop** project.
1. Add the following _using_ directives.

	<!--mark: 4-5-->
	````C#
	using System.Collections.Generic;
	using System.Linq;
	using CloudShop.Models;
	using System;
	using Microsoft.ApplicationServer.Caching;
	...
	````

1. In the **ProductsRepository** class, add the following highlighted code to define a constructor and declare a static member variable for a **DataCacheFactory** object instance, in addition to a boolean instance variable to control the use of the cache.

	(Code Snippet - _BuildingAppsWithCachingService - Ex2 - ProductsRepositoryConstructor_)
	<!--mark: 3-9-->
	````C#
	public class ProductsRepository : IProductRepository	
	{
		private static DataCacheFactory cacheFactory = new DataCacheFactory();
		private bool enableCache = false;

		public ProductsRepository(bool enableCache)
		{
			this.enableCache = enableCache;
		}

		public List<string> GetProducts()
		{
			...
		}
	}
	````

	> **Note:** The **DataCacheFactory** member is declared as static and is used throughout the lifetime of the application.

1. Locate the **GetProducts** method and insert the following (highlighted) code at the very beginning.
	
	(Code Snippet - _BuildingAppsWithCachingService - Ex2 - GetProductsReadCache_)
	<!--mark: 6-28-->
	````C#
	public class ProductsRepository : IProductRepository
	{
		...
		public List<string> GetProducts()
		{
			DataCache dataCache = null;
			if (this.enableCache)
			{
				try
				{
					dataCache = cacheFactory.GetDefaultCache();
					var products = dataCache.Get("products") as List<string>;
					if (products != null)
					{
						products[0] = "(from cache)";
						return products;
					}
				}
				catch (DataCacheException ex)
				{
					if (ex.ErrorCode != DataCacheErrorCode.RetryLater)
					{
						throw;
					}

					// ignore temporary failures
				}
			}

			using (NorthwindEntities context = new NorthwindEntities())
			{
				var query = from product in context.Products
							select product.ProductName;
				var products = query.ToList();
				return products;
			}
		}
	}
	````

	>**Note:** The inserted code uses the **DataCacheFactory** object to return an instance of the default cache object and then attempts to retrieve an item from this cache using a key with the value "_products_". If the cache contains an object with the requested key, it inserts a new entry to indicate that the list was retrieved from the cache and then returns it. The code treats temporary failures from the Windows Azure Caching service as a cache miss so that it can retrieve the item from its data source instead.

1. Next, add the following (highlighted) code block to the **GetProducts** method, immediately before the line that returns the **products** list at the end of the method.

	(Code Snippet - _BuildingAppsWithCachingService - Ex2 - GetProductsWriteCache_)
	<!-- mark:12-17 -->
	````C#
	public class ProductsRepository : IProductRepository
	{
		...
		public List<string> GetProducts()
		{
			...
			using (NorthwindEntities context = new NorthwindEntities())
			{
				var query = from product in context.Products
							select product.ProductName;
				var products = query.ToList();
				products.Insert(0, "(from data source)");

				if (this.enableCache && dataCache != null)
				{
					dataCache.Add("products", products, TimeSpan.FromSeconds(30));
				}

				return products;
			}
		}
	}
	````

	>**Note:** The inserted code stores the result of the query against the data source into the cache and sets its expiration policy to purge the item from the cache after 30 seconds.

<a name="Ex2Task2" />
#### Task 2 - Measuring the Data Access Latency ####

In this task, you will update the application to allow control of the use of the cache from the UI and to display the time required to retrieve catalog data, allowing you to compare the latency of retrieving data from the cache and the time required to access the data source.

1. Open the **HomeController.cs** file in the **Controllers** folder and add the **System.Diagnostics** using directive at the top of the file.
	
	<!-- mark:1 -->
	````C#
	using System.Diagnostics;
	````

1. Find the **Index** action, locate the lines that instantiate a new **ProductsRepository** and call its **GetProducts** method, and replace them with the highlighted code, as shown below.

	(Code Snippet - _BuildingAppsWithCachingService - Ex2 - GetProductsLatency_)
	<!-- mark:6-13 -->
	````C#
	public class HomeController : Controller
	{
		...                       
		public ActionResult Index()
		{
			bool enableCache = (bool)this.Session["EnableCache"];

			// retrieve product catalog from repository and measure the elapsed time
			Services.IProductRepository productRepository = new Services.ProductsRepository(enableCache);
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			var products = productRepository.GetProducts();
			stopWatch.Stop();

			// add all products currently not in session
			var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
			var filteredProducts = products.Where(item => !itemsInSession.Contains(item));

			IndexViewModel model = new IndexViewModel()
			{
				Products = filteredProducts
			};

			return this.View(model);
		}
		...
	}
	````

1. In the same method, locate the code that creates a new **IndexViewModel** instance and replace the **model** initialization with the following (highlighted) code block.

	(Code Snippet - _BuildingAppsWithCachingService - Ex2 - IndexViewModelInitialization_)
	<!--mark: 22-25-->
	````C#
	public class HomeController : Controller
	{
		...                       
		public ActionResult Index()
		{
			bool enableCache = (bool)this.Session["EnableCache"];

			// retrieve product catalog from repository and measure the elapsed time
			Services.IProductRepository productRepository =
			new Services.ProductsRepository(enableCache);
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			var products = productRepository.GetProducts();
			stopWatch.Stop();

			// add all products currently not in session
			var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
			var filteredProducts = products.Where(item => !itemsInSession.Contains(item));

			IndexViewModel model = new IndexViewModel()
			{
				Products = filteredProducts,
				ElapsedTime = stopWatch.ElapsedMilliseconds,
				IsCacheEnabled = enableCache,
				ObjectId = products.GetHashCode().ToString()
			};

			return this.View(model);
		}
		...
	}
	````

	>**Note:** The data added to the view model provide the time taken to load the product catalog from the repository, a flag to indicate whether the cache is enabled, and an identifier for the catalog object returned by the call to **GetProducts**. The view displays the object ID to allow you to determine whether the instance returned by the call to the repository has changed. This feature will be used later in the exercise when you enable the local cache.


1. Add a new action method to the **HomeController** to enable or disable the cache from the UI of the application.

	(Code Snippet - _BuildingAppsWithCachingService - Ex2 - EnableCacheMethod_)
	<!--mark: 4-8-->
	````C#
	public class HomeController : Controller
	{
		...
		public ActionResult EnableCache(bool enabled)
		{
			this.Session["EnableCache"] = enabled;
			return this.RedirectToAction("Index");
		}
	}
	````

1. Press **Ctrl** + **F5** to build and launch the application in the compute emulator.

	>**Note:** Ideally, you should test the code in Windows Azure. When you execute the application in the compute emulator, consider that accessing the Windows Azure SQL Database data source and the Windows Azure Caching require executing requests to resources located outside the bounds of your own network. Depending on your geographic location, both requests may exhibit a relatively high latency, which may overshadow the difference between the cached and non-cached scenarios. Once you deploy the application to Windows Azure, it is co-located in the same data center as the Windows Azure Caching service in Windows Azure SQL Database. As the latency is much lower, the results should be more significant.

1. When you start the application, the cache is initially disabled. Refresh the page and notice the elapsed time displayed under the **Cache settings for Cloud Shop** section that indicates the time required to retrieve the product catalog. Also that the first item in the list indicates that the application retrieved the product catalog from the data source.

	>**Note:** You may need to refresh the page several times to obtain a stable reading. The value shown for the first request may be greater because ASP.NET needs to compile the page.

	![Running the application without the cache](Images/running-the-application-without-the-cache.png?raw=true "Running the application without the cache")

	_Running the application without the cache_

1. Observe the **Object ID** indicator shown above the product catalog and notice how it changes every time you refresh the page indicating that the repository returns a different object for each call.

1. Now, click the **Enable** button in **Enable Cache** and wait for the page to refresh. Notice that the first item in the list indicates that it was still necessary for the application to retrieve the product catalog from the data source because the information has yet to be cached.

1. Click **Products**, or refresh the page in the browser. This time, the application retrieves the product data from Windows Azure Cache Service and the elapsed time should be lower. Confirm that the first item in the list indicates that the source of the information is the cache.

	![Running the application with the cache enabled](Images/running-the-application-with-the-cache-enable.png?raw=true "Running the application with the cache enabled")

	_Running the application with the cache enabled_

1. Close the browser.

<a name="Ex2Task3"></a>
#### Task 3 - Enabling the Local Cache ####

When using Windows Azure Cache Service, you have the option of using a local cache that allows objects to be cached in-memory at the client. When the application requests the object, the cache client checks whether the object resides in the local cache. If so, the reference to the object is returned immediately without contacting the cache service. If it does not exist, the object is retrieved from the cache service. In this task, you will enable the local cache and then compare the access time with the remote case.

1. Open the **ProductsRepository.cs** file in the **Services** folder of the **CloudShop** project.

	>**Note:** Make sure your solution is not running before editing the files.

1. In the **ProductsRepository** class, replace the current member fields and the constructor with the following code, to add the logic for managing the localCache configuration.

	(Code Snippet - _BuildingAppsWithCachingService - Ex2 - ProductsRepositoryWithLocalCache_)
	<!--mark: 2-34-->
	````C#
	...	
	private static DataCacheFactory cacheFactory;
	private static DataCacheFactoryConfiguration factoryConfig;
	private bool enableCache = false;
	private bool enableLocalCache = false;
	
	public ProductsRepository(bool enableCache, bool enableLocalCache)
	{
	    this.enableCache = enableCache;
	    this.enableLocalCache = enableLocalCache;
	
	    if (enableCache)
	    {
	        if (enableLocalCache && (factoryConfig == null || !factoryConfig.LocalCacheProperties.IsEnabled))
	        {
	            TimeSpan localTimeout = new TimeSpan(0, 0, 30);
	            DataCacheLocalCacheProperties localCacheConfig = new DataCacheLocalCacheProperties(10000, localTimeout, DataCacheLocalCacheInvalidationPolicy.TimeoutBased);
	            factoryConfig = new DataCacheFactoryConfiguration();
	
	            factoryConfig.LocalCacheProperties = localCacheConfig;
	            cacheFactory = new DataCacheFactory(factoryConfig);
	        }
	        else if (!enableLocalCache && (factoryConfig == null || factoryConfig.LocalCacheProperties.IsEnabled))
	        {
	            cacheFactory = null;
	        }
	    }
	
	    if (cacheFactory == null)
	    {
	        factoryConfig = new DataCacheFactoryConfiguration();
	        cacheFactory = new DataCacheFactory(factoryConfig);
	    }
	} 
	...
	````

1. Open the **HomeController.cs** file in the **Controllers** folder and find the **Index** action. Locate the code that instantiates a new **ProductsRepository** and replace those lines with the following highlighted code:

	(Code Snippet - _BuildingAppsWithCachingService - Ex2 - ProductsRepositoryLocalCache_)
	<!--mark: 7-10-->
	````C#
	public class HomeController : Controller
	{
		...                       
		public ActionResult Index()
		{
			bool enableCache = (bool)this.Session["EnableCache"];
			bool enableLocalCache = (bool)this.Session["EnableLocalCache"];

			// retrieve product catalog from repository and measure the elapsed time
			Services.IProductRepository productRepository = new Services.ProductsRepository(enableCache, enableLocalCache);
			Stopwatch stopwatch = new Stopwatch();
			stopWatch.Start();
			var products = productRepository.GetProducts();
			...
		}
	````

1. In the same method, locate the code that creates a new **IndexViewModel** and add the following highlighted property.
	<!-- mark:9 -->
	````C#                  
	public ActionResult Index()
	{
		...
		IndexViewModel model = new IndexViewModel()
		{
			Products = filteredProducts,
			ElapsedTime = stopWatch.ElapsedMilliseconds,
			IsCacheEnabled = enableCache,
			IsLocalCacheEnabled = enableLocalCache,
			ObjectId = products.GetHashCode().ToString()
		};

		return this.View(model);
	}
	````

1. Add a new action method to the **HomeController** to enable or disable the local cache from the UI of the application.

	(Code Snippet - _BuildingAppsWithCachingService - Ex2 - EnableLocalCacheMethod_)
	<!--mark: 4-8-->
	````C#
	public class HomeController : Controller
	{
		...
		public ActionResult EnableLocalCache(bool enabled)
		{
			this.Session["EnableLocalCache"] = enabled;
			return RedirectToAction("Index");
		}
	}
	````

1. Open **Index.cshtml** file in the **Views\Home** folder and add the following highlighted code above the **elapsedTime** div.

	(Code Snippet - _BuildingAppsWithCachingService - Ex2 - EnableLocalCacheButton_)
	<!--mark: 13-23-->
	````HTML
	<span class="h4">Cache settings for Cloud Shop</span>
	<p><b>Instance ID:</b> @Model.InstanceId</p>
	<p><b>Object ID:</b> @Model.ObjectId</p>
	<p><b>Cache Management:</b></p>
	@if (!Model.IsCacheEnabled)
	{
		<a href="@Url.Action("EnableCache", "Home", new { enabled = true })" class="btn btn-success"><span class="glyphicon glyphicon glyphicon-flash"></span> Enable Cache</a>
	}
	else
	{
		<a href="@Url.Action("EnableCache", "Home", new { enabled = false })" class="btn btn-danger"><span class="glyphicon glyphicon glyphicon-off"></span> Disable Cache</a>
	}
	@if (Model.IsCacheEnabled)
	{
		if (!Model.IsLocalCacheEnabled)
		{
			<a href="@Url.Action("EnableLocalCache", "Home", new { enabled = true })" class="btn btn-success"><span class="glyphicon glyphicon glyphicon-flash"></span> Enable Local Cache</a>
		}
		else
		{
			<a href="@Url.Action("EnableLocalCache", "Home", new { enabled = false })" class="btn btn-danger"><span class="glyphicon glyphicon glyphicon-off"></span> Disable Local Cache</a>
		}
	}
	<br />
	<br />
	<div id="elapsedTime">Elapsed time: @Model.ElapsedTime.ToString() milliseconds.</div>
	````

1. Press **Ctrl** + **F5** to build and launch the application in the compute emulator.
 
1. When you start the application, the cache option is initially disabled and the local cache option is hidden (it will be shown once you enable it). Enable cache and then the local cache. 

1. Refresh the page several times until the elapsed time stabilizes. Notice that the reading is now significantly lower, possibly under a millisecond, showing that the application now retrieves the data from the local in-memory cache. 

	![Using the local cache](Images/using-the-local-cache.png?raw=true "Using the local cache")

	_Using the local cache_

1. Observe that, each time you refresh the page, the **Object ID** shown above the product catalog remains constant indicating that the repository now returns the same object each time.

	>**Note:** 	This is an important aspect to consider. Previously, with the local cache disabled, changing an object retrieved from the cache had no effect on the cached data and subsequent fetches always returned a fresh copy. Once you enable the local cache, it stores references to in-memory objects and any changes to the object directly affect the cached data. 
You should be aware of this when using the cache in your own applications and consider that, after changing a cached object and later retrieving the same object from the cache, it may or may not include these changes depending on whether it is returned by the local or remote cache.

1. Wait for at least 30 seconds and then refresh the page one more time. Notice that the elapsed time is back to its original value and that the object ID has changed, showing that the cached item has expired and been purged from the cache due to the expiration policy set on the object when it was stored.

<a name="Exercise3" />
### Exercise 3: Caching common data patterns with WACEL ###
This exercise will show you how to use  ....


<a name="Ex3Task1" />
#### Task 1 - Retrieving data from Azure Storage Tables using WACEL ####

In this task you will learn how to use WACEL as a high-level data structure used on top of Windows Azure Table. To do so, you will first add a new View to the application that shows a list of customers from a Company.

>**Note:** WACEL provides implementation of high-level data structures that can be shared among your services and application. For more information, browse [http://wacel.codeplex.com/](http://wacel.codeplex.com/)

1. Start **Microsoft Visual Studio 2013 Express for Web** as administrator.

1. Open the **Begin** solution located at **Source\Ex2-CachingCommonDataPatternWithWACEL\Begin**.

	>**Important:** Before you execute the solution, make sure that the startup project is set. For MVC projects, the start page must be left blank. To set the startup project, in **Solution Explorer**, right-click the **CloudShop.Azure** project and then select Set as StartUp Project. To set the start page, in **Solution Explorer**, right-click the **CloudShop** project and select **Properties**. In the **Properties** window, select the Web tab and in the **Start Action**, select **Specific Page**. Leave the value of this field blank.

1. Right click in the **Home** folder inside **Views** in the **CloudShop** project and select **Add** | **Existing Item...**.

	![Add existing item](Images/add-existing-item.png?raw=true "Add existing item")	

	_Add existing item_

1. Select the file **table.cshtml** located in the **Assets** folder of the lab.

1. Open the **HomeController** and add the following code to return the View you added in the previous step.

	<!-- mark:1-4 -->
	````C#
	public ActionResult Table()
	{
		return View();
	}
	````

1. Open **Package Manager Console**. To do so, go to the **Tools** menu. Then go to the **Library Package Manager** and click on **Package Manager Console**.

1. Execute _Install-Package WACEL_ to install **Windows Azure Cache Extension Library (WACEL)** dependency to the project. Make sure that **CloudShop** project is set as default project.
	
	![WACEL nuget installed](Images/wacel-nuget-installed.png?raw=true "WACEL nuget installed")

	_WACEL nuget installed_

1. Right click in the **Models** folder and go to **Add** | **Class**.

	![Add class](Images/add-class.png?raw=true "Add class")
	
	_Add class_

1. Name it _Customer.cs_ and click **OK**.

	![Add new item dialog box](Images/add-new-item-dialog-box.png?raw=true "Add new item dialog box")

	_Add new item dialog box_

1. Replace the **Customer** implementation with the following code

	<!-- mark:1-19 -->
	````C#
	namespace CloudShop.Models
	{
		 using System;

		 public class Customer
		 {
			  public string Id { get; set; }

			  public string Company { get; set; }
			  
			  public string Name { get; set; }
			  
			  public double Value { get; set; }
			  
			  public string Comment { get; set; }
			  
			  public DateTime ContractDate { get; set; }
		 }
	}
	````

1. Add another class called **TableViewModel** in the **Models** folder.

1. Replace the **TableViewModel** class implementation with the following code.

	<!-- mark:1-11 -->
	````C#
	namespace CloudShop.Models
	{
		 using System.Collections.Generic;

		 public class TableViewModel
		 {
			  public List<Customer> Customers { get; set; }

			  public long ElapsedTime { get; set; }
		 }
	}
	````

1. Right click in the **Controllers** folder and go to **Add** | **Web API Class Controller (v2)**.

	![Add Web API Controller](Images/add-web-api-controller.png?raw=true "Add Web API Controller")

	_Add Web API controller_

1. In the **Add Scaffold** dialog box, select **WebAPI 2 Controller - Empty** and then click **Add**

	![Add Scaffold dialog box](Images/add-scaffold-dialog-box.png?raw=true "Add Scaffold dialog box")

	_Add Scaffold dialog box_
	

1. In the **Specify Name for Item** dialog box, set the name of the controller to _TableDataController_ and click **OK**.

	![Specify Name for Item dialog box](Images/specify-name-for-item-dialog-box.png?raw=true "Specify Name for Item dialog box")

	_Specify Name for Item dialog box_

1. Add the following namespace directives to the Web API controller.

	<!-- mark:1-4 -->
	````C#
	using CloudShop.Models;
	using System.Diagnostics;
	using Microsoft.Ted.Wacel;
	using Microsoft.WindowsAzure;
	````

1. Add the following action to the **TableDataController** to retrieve the list of customers and the elapsed time of the call.
	
	<!-- mark:1-16 -->
	````C#
	[HttpGet]
	public TableViewModel GetTable(string partition, string startId, string endId)
	{
		Stopwatch stopWatch = new Stopwatch();
		stopWatch.Start();

		Table<Customer> table = new Table<Customer>("Company", "Id", CloudConfigurationManager.GetSetting("StorageClient"), "customers");
		var customers = table.List(startId, endId, partition, partition).ToList();

		stopWatch.Stop();
		return new TableViewModel()
		{
			Customers = customers,
			ElapsedTime = stopWatch.ElapsedMilliseconds
		};
	}
	````

1. Now, in the **Solution Explorer**, expand the **Roles** folder and right click on **Properties**.

	![WebRole properties](Images/webrole-properties.png?raw=true "WebRole properties")

	_WebRole properties_

1. In the **Settings** tab, add a new setting named _StorageClient_. Set the type to _Connection String_ and set the value to _UseDevelopmentStorage=true_

	![StorageClient setting](Images/storageclient-setting.png?raw=true "StorageClient setting")

	_StorageClient setting_

1. Press **F5** to run the application.

1. Click on the **Customers** link on the Top bar.

	![Customers link](Images/customers-link.png?raw=true "Customers link")

	_Customers link_

1. Wait until the table is field with a list of Customers. Notice the time it took to return the data from Windows Azure Tables.

	![Customer list using WACEL without caching](Images/customer-list-without-caching.png?raw=true "Customer list using WACEL without caching")

	_Customer list using WACEL without caching_

1. Switch from **Company0** to **Company1** in the drop down list in order to show the Customers from Company 1

	![Customer list from Company 1](Images/customer-list-from-company-1.png?raw=true "Customer list from Company 1")

	_Customer list from Company 1_

In the next task you will update the solution to include caching included in WACEL

<a name="Ex3Task2" />
#### Task 2 - Adding caching support to WACEL Cloud Tables ####

In this task you will update the Web API to include caching provided by WACEL when querying the Customer table.

1. Open the **TableDataController** located in the **Controllers** folder.

1. Add the following namepsace directive to the top of the file.
	
	<!-- mark:1 -->
	````C#
	using Microsoft.ApplicationServer.Caching;
	````

1. Update the **GetTable** method in order to add a new **DataCache** parameter to the **Table** constructor.

	<!-- mark:1 -->
	````C#
	Table<Customer> table = new Table<Customer>("Company", "Id", CloudConfigurationManager.GetSetting("StorageClient"), "customers", new DataCache("customers"));
	````
	
	>**Note:** TBC

1. Now, in the **Solution Explorer**, expand the **Roles** folder and right click on the **CacheWorkerRole** and click on **properties**.

	![CacheWorkerRole properties](Images/cacheworkerrole-properties.png?raw=true "CacheWorkerRole properties")

	_CacheWorkerRole properties_
	
1. In the **Caching** tab, add a new **Named Cache** called _customers_
	
	![Customers Named Cache](Images/customers-named-cache.png?raw=true "Customers Named Cache")

	_Customers Named Cache_

	>**Note:** The name must match the string you passed as argument to the DataCache in the Web API controller.

1. Press **F5** to run the application again.

1. Switch between **Company0** and **Company1** to retrieve the list of Customers from Company 1.

1. Switch back to **Company0**. Notice how the **Elapsed time** has decressed. It decressed because we are using WACEL Caching implementation with In-Role Caching.

	![Customers list with WACEL and caching](Images/customers-list-with-wacel-and-caching.png?raw=true "Customers list with WACEL and caching")

	_Customers list with WACEL and caching_

---

<a name="NextSteps" />
## Next Steps ##

TBC

---

<a name="Summary" />
## Summary ##

In this hands-on lab, you explored the use of the Windows Azure Cache Service. You saw how to configure session state to be cached across a cache cluster, allowing sessions to be preserved in the presence of restarts and across multiple role instances hosting the application. In addition, you learnt the basics of data caching with Windows Azure and in particular, how to cache the results of queries to a Windows Azure SQL Database. Finally, you looked at a reusable caching layer implementation that will allow you to add a caching layer to your applications in a very simple manner.
