<a name="HOLTop"></a>
# Migrating ASP.NET Applications to Microsoft Azure #

---
<a name="Overview"></a>
## Overview ##

ASP.NET supports different implementations of the application providers for membership, role, profile and session management. Most providers come with a version that is based on a SQL database, or uses in-memory representations of data managed by the providers.
The Microsoft Azure samples include provider implementations that make use of scalable and reliable blob and table storage services. Additionally, the providers deal with the problem of Web applications that are hosted on a variety of different machines inside the Microsoft Azure fabric.
When you deploy your Web application in the Microsoft Azure data centers, the storage services for tables and blobs are readily available and are therefore easily accessible from your application.

> **Note:** A Visual Studio 2012 version of this Hands-on Lab can be found in the latest build of the [Microsoft Azure training kit](http://bit.ly/WindowsAzureTK) or in [GitHub](https://github.com/WindowsAzure-TrainingKit/HOL-MigratingAspNetApps-VS2012).

<a name="objectives"></a>
### Objectives ###
In this hands-on lab, you will learn how to:

- Migrate ASP.NET Web Form and MVC applications to Microsoft Azure
- Use Forms Authentication with Microsoft Azure
- Use Azure ASP.NET providers for membership, role, and session state support

The lab shows how to use these features for both ASP.NET Web Form and ASP.NET MVC applications and has an exercise dedicated to each technology. Since both exercises use the same scenario and cover the same material, choose the one that is most relevant to your needs.

<a name="prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Microsoft .NET Framework 4.0][1]
- [Microsoft Visual Studio 2010][2]
- [Microsoft Azure SDK and Microsoft Azure Tools for Microsoft Visual Studio 1.8][3]
- [ASP.NET MVC 4](http://www.asp.net/mvc/mvc4/)
- A Microsoft Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

[1]: http://go.microsoft.com/fwlink/?linkid=186916
[2]: http://msdn.microsoft.com/vstudio/products/
[3]: http://www.microsoft.com/windowsazure/sdk/

> **Note:** This lab was designed to use Windows 7 Operating System.

<a name="setup"></a>
### Setup ###
In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.

1. Right-click the **Setup.cmd** file and click **Run as administrator**. This will launch the setup process that will configure your environment and install the Visual Studio code snippets for this lab.

> **Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

<a name="usingcodesnippets"></a>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2010 to avoid having to add it manually.

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Moving a Web Application to the Cloud](#Exercise1)
1. [Using the Azure ASP.NET Providers with MVC Applications](#Exercise2)
1. [Using the Azure ASP.NET Providers with Web Form Applications](#Exercise3)

> **Note:** Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise. Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.

Estimated time to complete this lab: **45 minutes**.

<a name="Exercise1"></a>
### Exercise 1: Moving a Web Application to the Cloud ###

In this exercise, you configure a sample shopping cart application implemented with ASP.NET to run in Microsoft Azure.

<a name="Ex1Task1"></a>
#### Task 1 – Preparing the Application to Run in Microsoft Azure ####

The Azure Store is a standard ASP.NET sample that mimics a simple commerce application. It presents a list of products that users can select and add to their shopping cart.

Before you begin, you may wish to build and run the solution and become acquainted with its operation. In its initial state, the application runs outside the compute emulator.

In this task, you create a Microsoft Azure project in Visual Studio to prepare the application to run in Microsoft Azure.

1. Open Visual Studio in elevated administrator mode from **Start | All Programs | Microsoft Visual Studio 2010** by right clicking the **Microsoft Visual Studio 2010** shortcut and choosing **Run as administrator**.
1. If the **User Account Control** dialog appears, click **Yes**.
1. In the **File** menu, choose **Open** and then **Project/Solution**. In the Open Project dialog, browse to **Ex1-MovingMVCAppsToAzure\Begin\** or **Ex1-MovingWebAppsToAzure\Begin\** in the Source folder of the lab, select **Begin.sln** and click **Open**.

	> **Note:** Depending on your needs, you may wish to explore migrating ASP.NET MVC or ASP.NET Web Form applications to Microsoft Azure. The procedures in this exercise are common to both types of application. Choose the most appropriate solution.

1. Next, create a new cloud service project and add it to the solution. In the **File** menu, point to **Add** and select **New Project**. In the **Add New Project** dialog, expand Visual C# in the **Installed Templates** list and select **Cloud**. Choose the **Microsoft Azure Project** template, set the **Name** of the project to **CloudShopService**, keep the proposed location in the solution’s folder and then click **OK**.

	![Configuring the application to run in Microsoft Azure](Images/configuring-the-application-to-run-in-windows.png?raw=true)
	
	_Configuring the application to run in Microsoft Azure_
	
1. In the **New Microsoft Azure Project** dialog, click **OK** without adding new roles to the solution because you will use the existing application as a web role.
	
	![No additional roles are required](Images/no-additional-roles-are-required.png?raw=true)
	
	_No additional roles are required_

1. Associate the ASP.NET project to the cloud project. In **Solution Explorer**, right-click the **Roles** node in the **CloudShopService** project, point to **Add** and select **Web Role Project in solution**.
	
	![Associating the web role project](Images/associating-the-web-role-project.png?raw=true)
	
	_Associating the web role project_
	
1. In the **Associate with Role Project** dialog, select the **CloudShop** project and click **OK**.

	![Associating the Web Role project2](Images/associating-the-web-role-project2.png?raw=true)
	
	_Associating the web role project_
	
	> **Note:** When you associate a new role, Visual Studio updates the **ServiceDefinition.csdef** and the **ServiceConfiguration.cscfg** files. If either of these files is currently open, make sure that you save it to preserve these changes.

1. Configure a **TraceListener** to enable diagnostics logging for the application. To do this, open the **Web.config** file of the **CloudShop** project and insert a **system.diagnostics** section as shown (highlighted) below.	

	(Code Snippet – _Migrating ASP.NET Applications  - Ex1 DiagnosticMonitorTraceListener_)
	<!--mark: 3-12-->
	````XML
	<configuration>
	  ...
	  <system.diagnostics>
		<trace>
			<listeners>
				<add type="Microsoft.WindowsAzure.Diagnostics.DiagnosticMonitorTraceListener, Microsoft.WindowsAzure.Diagnostics, Version=1.8.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
      name="AzureDiagnostics">
					<filter type="" />
				</add>
			</listeners>
		</trace>
	  </system.diagnostics>
	
	</configuration>
	````
	
	
	>**Note:**These settings in the **system.diagnostics** section configure a trace listener specific to Microsoft Azure that allows the application to trace code execution using the classes and methods available in the [System.Diagnostics.Trace](http://msdn.microsoft.com/en-us/library/system.diagnostics.trace.aspx) class.
	This step is usually unnecessary for roles created in Visual Studio because they already include the necessary settings in their role templates.


1. In the **Global.asax** file of the web role, declare the following namespaces. 	
	
	<!--mark: 1,2-->
	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.ServiceRuntime;
	````
	
1. Next, find the **Application_Start** method and insert the following (highlighted) code at the start of the method to initialize the configuration settings publisher.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex1 SetConfigurationSettingPublisher_ )
	<!--mark: 3-7-->
	````C#
	protected void Application_Start(object sender, EventArgs e)
	{
		Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
		{
			configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
		});
		
	  ...
	}
	````

	>**Note:** A configuration setting publisher simplifies the retrieval of storage account configuration settings. Applications only need to set up the publisher once using **CloudStorageAccount.SetConfigurationSettingPublisher**, and then elsewhere, whenever they require access to a storage account, they only require the name of the corresponding setting to access the storage account using **CloudStorageAccount.FromConfigurationSetting**.
	
	>Web roles can define a **WebRole** class with the entry point of the role. This class contains methods that Microsoft Azure calls at various stages during the role’s lifetime, for example, the **OnStart** method is called during role start up. You can use this method to initialize the role. Note, however, that for web roles executing in full IIS mode, Internet Information Server hosts the web application in a separate process (w3wp.exe), while the role entry point executes in a different process (WaIISHost.exe). Consequently, most web role initialization tasks need to be performed in the ASP.NET **Application_Start** method.

1. For an **ASP.NET MVC project**, ensure that the **System.Web.MVC** assembly is included in the service package that you deploy to Microsoft Azure. To do this, expand the **References** node in **Solution Explorer** for the **CloudShop** project, right-click the **System.Web.MVC** assembly and select **Properties**. Change the value of the **Copy Local** setting to _True_.
	
	![Including assemblies in the service package deployed to Microsoft Azure](Images/including-assemblies-in-the-service-package-d.png?raw=true)
	
	_Including assemblies in the service package deployed to Microsoft Azure_
	
	>**Note:** In general, you need to set **Copy Local** = True for any assembly that is not installed by default in the Microsoft Azure VMs to ensure that it is deployed with your application.
	
>**Note:** Depending on the type of application that you chose for the exercise that you have just completed, you may wish to proceed with Exercise 2: Using the Azure ASP.NET Providers with MVC Applications or Exercise 3: Using the Azure ASP.NET Providers with Web Form Applications.
	
<a name="Exercise2"></a>
### Exercise 2: Using the Azure ASP.NET Providers with MVC Applications ###

In this exercise, you modify the ASP.NET MVC application to use the ASP.NET providers from the Microsoft Azure samples. You start by adding authentication to the site using the membership provider. Next, you implement the role provider to classify users and customize the products that the application offers. Finally, you configure the session state provider to store the contents of the shopping cart.

<a name="Ex2Task1"></a>
#### Task 1 – Configuring Authenticated Access to the Application ####

In this task, you configure the application to require authenticated access to the pages that implement the shopping cart. 

1. If necessary, open Visual Studio in elevated administrator mode from **Start | All Programs | Microsoft Visual Studio 2010** by right clicking the **Microsoft Visual Studio 2010** shortcut and choosing **Run as administrator**. 
1. In the **File** menu, choose **Open** and then **Project/Solution**. In the **Open Project** dialog, browse to **Ex2-UsingAzureProvidersWithMVCApps\Begin** in the **Source** folder of the lab, select **Begin.sln** and click **Open**.
1. In the **Controllers** folder, open **HomeController.cs** and decorate the class with an **Authorize** attribute. This configures the application to require authenticated access for every available action on this controller.

	<!--mark: 2-->
	````C#
	[HandleError] 
	[Authorize]
	public class HomeController : Controller
	{
		...
	}
	````

1. Save the **HomeController.cs** file.

<a name="Ex2Task2"></a>
#### Task 2 – Configuring Membership Support Using the Azure  TableStorageMembershipProvider ####

In this task, you add and configure the Azure ASP.NET providers for membership, role, and session.

1. Add the Microsoft Azure ASP.NET Providers project to the solution.  In **Solution Explorer**, right-click the **Begin** solution, point to **Add** and select **Existing Project**. Browse to **Assets\AspProviders** in the **Source** folder of the lab, select the **AspProviders.csproj** project and click **Open**.

	>**Note:** The **AspProviders** project is available as a sample. It is included as part of this training kit for your convenience and is based on the original source code found in the [MSDN Code Gallery](http://code.msdn.microsoft.com/windowsazuresamples). The project contains the implementation of ASP.NET application providers for membership, role, profile, and session state.

1. Add a reference in the web role to the **AspProviders** project. In **Solution Explorer**, right-click the **CloudShop** project node and click **Add Reference**. In the **Add Reference** dialog, switch to the **Projects** tab, select the **AspProviders** project and click **OK**.

	![Adding a reference to the sample Azure ASP.NET Providers project](Images/adding-a-reference-to-the-sample-azure-aspnet.png?raw=true)

	_Adding a reference to the sample Azure ASP.NET Providers project_
	
1. Update the service configuration to include a connection string to the Storage account where the data will be stored. In the **CloudShopService** project, expand the **Roles** node and double-click the **CloudShop** node to open the properties window for this role.

1. In the **CloudShop[Role]** properties window, select the **Settings** tab and click **Add Setting**. Set the **Name** of the new setting to _DataConnectionString_ and change the **Type** to _Connection String_. Then, in the **Value** column, click the button labeled with an ellipsis.
	
	![Creating a new configuration setting for the role](Images/creating-a-new-configuration-setting-for-the.png?raw=true)
	
	_Creating a new configuration setting for the role_
	
1. In the **Storage Account Connection String** dialog, choose the option labeled **Microsoft Azure storage emulator** and click **OK**.
 
	![Configuring a storage connection string](Images/configuring-a-storage-connection-string.png?raw=true)
	
	_Configuring a storage connection string_

1. Press **CTRL + S** to save your changes to the role configuration.

1. Open the **Web.config** file located in the root folder of the **CloudShop** project.

1. (Optional) Configure the storage account information required by the ASP.NET providers in the application configuration file. To do this, locate the \<**appSettings**> element and add the following (highlighted) configuration block. If the **appSettings** element is missing, insert it as a direct child of the \<**configuration**> element.

	>**Note:** In addition to the service configuration file, you can also configure the Azure providers in the **Web.config** file of the application. This allows you to host the application outside the Azure fabric and still take advantage of the Azure ASP.NET providers and storage. However, when the application runs in the Microsoft Azure environment, configuration settings in the service configuration file for the ASP.NET providers take precedence over those in the **Web.config** file. By using the Microsoft Azure settings, you can avoid redeploying the application when changing provider settings.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex2 DataConnectionString_)
	<!--mark: 5-->
	````XML
	<configuration>
	  ...
	  <appSettings>
		...
	    <add key="DataConnectionString" value="UseDevelopmentStorage=true"/>
	  </appSettings>
	  ...
	</configuration>	
	````

1. Configure the application to use the membership provider in the **AspProviders** project. To do this, replace the existing \<**membership**> section inside the <**system.web**> element with the following (highlighted) configuration. 

	>**Note:** The default ASP.NET MVC template in Visual Studio creates the configuration settings for the **AspNetSqlMembershipProvider**, which uses SQL Server for storage.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex2 TableStorageMembershipProvider_)
	<!--mark: 8-24-->
	````XML
	<configuration>
	  ...
	  <system.web>
	    ...
	    <authentication mode="Forms">
	      ...
	    </authentication>
	    <!-- Membership Provider Configuration -->
	    <membership defaultProvider="TableStorageMembershipProvider" userIsOnlineTimeWindow="20" hashAlgorithmType="HMACSHA256">
	      <providers>
	        <clear/>
	        <add name="TableStorageMembershipProvider"
	             type="Microsoft.Samples.ServiceHosting.AspProviders.TableStorageMembershipProvider"
	             description="Membership provider using table storage"
	             applicationName="CloudShop"
	             enablePasswordRetrieval="false"
	             enablePasswordReset="true"
	             requiresQuestionAndAnswer="false"
	             minRequiredPasswordLength="1"
	             minRequiredNonalphanumericCharacters="0"
	             requiresUniqueEmail="true"
	             passwordFormat="Hashed"/>
	      </providers>
	    </membership>
	  </system.web>
	  ...
	</configuration>
	````

	>**Important:** Before you execute the solution, make sure that the startup project and the start-up page are set. 
	>To set the startup project, in **Solution Explorer**, right-click the **CloudShopService** project and select **Set as StartUp Project**. 
	>To designate the start page, in **Solution Explorer**, right-click the **CloudShop** project and select **Properties**. In the **Properties** window, select the **Web** tab and in the **Start Action**, select **Specific Page**. For the MVC project, set the value of this field empty.

1. Press **F5** to build and run the application. An initialization procedure may be required the first time you execute an application that uses the storage emulator. If this happens, wait until the procedure is complete and review its status. Click **OK** to continue.
  
	![Development storage initialization procedure status](Images/development-storage-initialization-procedure.png?raw=true)

	_Development storage initialization procedure status_

1. Notice that the application redirects you to the logon page when it starts because the authorization settings now require authenticated access to the Home controller. The membership database is initially empty, so you first need to create an account before you can proceed. In the login page, click **Register** to access the user registration form.
 
 	![Authentication required to proceed](Images/authentication-required-to-proceed.png?raw=true)

	_Authentication required to proceed_

1. Fill in the registration form and click **Register** to create your account.
  
	![Creating a new user account](Images/creating-a-new-user-account.png?raw=true)
	
	_Creating a new user account_

1. After creating your account, the system automatically logs you in and displays the products page.  Notice your user name displayed in the upper right corner of the window.
  
	![Products page displaying the current user](Images/products-page-displaying-the-current-user.png?raw=true)

	_Products page displaying the current user_

1. Close the browser window to stop the running application.

<a name="Ex2Task3"></a>
#### Task 3 – Configuring Role Support Using the Azure TableStorageRoleProvider ####

In this task, you add role support to the application using the Azure role provider. This requires updating the registration process to capture the role of the user and configuring the settings for the role provider. To demonstrate the use of roles, you update the products page to filter the list of products based on the type of user.

1. Add code to the startup routine to initialize the roles supported by the application. The code creates two roles, _Home_ and _Enterprise_, which the application uses to classify different types of user. Open **Global.asax.cs** and insert the following (highlighted) code into the **Application_Start** method.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex2 Initialize Roles_)
	<!--mark: 6-15-->
	````C#
	protected void Application_Start()
	{
	  ...
	  this.LoadProducts();
	    
	  // Initialize the application roles 
	  if (!System.Web.Security.Roles.RoleExists("Home"))
	  {
	    System.Web.Security.Roles.CreateRole("Home");
	  }
	
	  if (!System.Web.Security.Roles.RoleExists("Enterprise"))
	  {
	    System.Web.Security.Roles.CreateRole("Enterprise");
	  }
	}
	````

1. Change the **Index** action to filter the list of products based on the type of user. In the **Controllers** folder, open the **HomeController.cs** file and insert the following (highlighted) code into the **Index** method immediately below the line that declares and initializes the **filteredProducts** variable.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex2 Filter Products_)
	<!--mark: 10-14-->
	````C#
	public ActionResult Index()
	{
	  var products = this.HttpContext.Application["Products"] as List<string>;
	  var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
	
	  // add all products currently not in session
	  var filteredProducts = products.Where(item => !itemsInSession.Contains(item));
	
	  //// Add additional filters here
	  // filter product list for home users
	  if (User.IsInRole("Home"))
	  {
	    filteredProducts = filteredProducts.Where(item => item.Contains("Home"));
	  }
	
	  return this.View(filteredProducts);
	}
	````

	>**Note:** The inserted code appends an additional filter for users in the _Home_ role that returns only items containing the text "Home".

1. Configure the application to use the role provider in the **AspProviders** project. In the **Web.config** file, **replace** the existing \<**roleManager**> section inside the \<**system.web**> element with the following (highlighted) configuration.

	>**Note:** The default ASP.NET MVC template in Visual Studio creates the configuration settings for the **AspNetSqlRoleProvider**, which uses SQL Server for storage, and the **AspNetWindowsTokenRoleProvider**, which uses Windows groups.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex2 TableStorageRoleProvider_)
	<!--mark: 5-22-->
	````XML
	<configuration>
	  ...
	  <system.web>
	    ...
	    <!-- RoleManager Provider Configuration -->
	    <roleManager enabled="true"
	                 defaultProvider="TableStorageRoleProvider" 
	                 cacheRolesInCookie="true" 
	                 cookieName=".ASPXROLES" 
	                 cookieTimeout="30" 
	                 cookiePath="/" 
	                 cookieRequireSSL="false" 
	                 cookieSlidingExpiration="true" 
	                 cookieProtection="All">
	      <providers>
	        <clear/>
	        <add name="TableStorageRoleProvider"
	             type="Microsoft.Samples.ServiceHosting.AspProviders.TableStorageRoleProvider"
	             description="Role provider using table storage"
	             applicationName="CloudShop" />
	      </providers>
	    </roleManager>
	    ...
	  </system.web>
	  ...
	</configuration>
	````

1. Press **CTRL+F5** to build and run the application without debugging. 
1. In the login page, click **Register** to access the user registration form. Notice that the registration wizard now displays a section to specify the role of the customer.  Create a new user and assign it a _Home_ customer profile.
 
	![Registration page showing role information](Images/registration-page-showing-role-information.png?raw=true)
	
	_Registration page showing role information_

1. Logged in as a _Home_ user, proceed to the products page. Notice that the list of products only includes home products.
 
	![Products page showing a filtered list of products based on role](Images/products-page-showing-a-filtered-list-of-prod.png?raw=true)
	
	_Products page showing a filtered list of products based on role_

1. Click the **Logoff** link in the upper right corner of the application window.
1. Register a new account and assign this user an _Enterprise profile_. Notice that the list of displayed products differs from that seen by a _Home_ user.

	![Products page showing Enterprise products](Images/products-page-showing-enterprise-products.png?raw=true)
	
	_Products page showing Enterprise products_
	
1. Select a product from the list and click **Add item to cart**. You may repeat the process to store additional items in the cart.
1. Click the **Checkout** link to view the contents of the shopping cart. Verify that the items you selected appear on the list.
 
	![Check out page showing the contents of the shopping cart](Images/check-out-page-showing-the-contents-of-the-sh.png?raw=true)
	
	_Checkout page showing the contents of the shopping cart_

1. Do not close the browser window or navigate away from the checkout page.
1. In the task bar, right-click the compute emulator icon and select **Show Compute Emulator UI**. 
1. In the **Compute Emulator**, right-click the **CloudShopService** node and choose **Suspend**.
  
	![Suspending the service role instance](Images/suspending-the-service-role-instance.png?raw=true)
	
	_Suspending the service role instance_

1. Open a command window in elevated administrator mode, from **Start | All Programs | Accessories | Command Prompt** by right clicking the **Command Prompt** shortcut and choosing **Run as administrator**. At the command prompt, type _iisreset_.
 
	![Restarting Internet Information Server](Images/restarting-internet-information-server.png?raw=true)
	
	_Restarting Internet Information Server_

	>**Note:** The preceding two steps, _recycling the role_ and _restarting IIS_, simulate what would happen in Microsoft Azure when a role instance is restarted.

1. Go back to the **Compute Emulator** and wait until the service is **destroyed** as indicated by the instance icon turning purple. Now, start the service instance once again. To do this, right-click the **CloudShopService** node and choose **Run**, then wait for the service to start.
1. Switch back to the browser window showing the checkout page and click **Refresh**. Notice that the order now appears empty.

	>**Note:** The application is currently using inproc session state, which maintains all session state in-memory. When you stop the service instance, it discards all session state including the contents of the shopping cart. In the following task, you will configure the application to store session state in storage, which allows the application to maintain session state in the presence of restarts and across multiple machines hosting the application.

1. Close the browser window to stop the application.

<a name="Ex2Task4"></a>
#### Task 4 – Configuring Session Support Using the Azure TableStorageSessionProvider ####

Microsoft Azure can potentially host a Web role on multiple machines inside the fabric, which makes in-memory session state unsuitable for such an environment. In contrast, the session state provider in the **AspProviders** project uses table storage to store configuration information about the session and blob storage to store the session state itself. 
In this task, you configure the application to use the Azure session state provider.

1. Configure the application to use the session provider in the **AspProviders** project. To do this, in the **Web.config** file of the **CloudShop** project, replace the sessionState section with the following (highlighted) configuration block as a direct child of the <**system.web**> element.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex2 TableStorageSessionStateProvider_)
	<!--mark: 5-14-->
	````XML
	<configuration>
	  ...
	  <system.web>
	    ...
	    <!-- SessionState Provider Configuration -->
	    <sessionState mode="Custom"
	                  customProvider="TableStorageSessionStateProvider">
	      <providers>
	        <clear/>
	        <add name="TableStorageSessionStateProvider"
	             type="Microsoft.Samples.ServiceHosting.AspProviders.TableStorageSessionStateProvider"
	             applicationName="CloudShop" />
	      </providers>
	    </sessionState>
	    ...
	  </system.web>
	  ...
	</configuration>
	````

1. Press **CTRL+F5** to build and run the application without debugging.
1. Log in and navigate to the products page. Select one or more products from the list and click **Add item to cart**. Repeat the process to store additional items in the cart.
1. Click the **Checkout** link to view the contents of the shopping cart. Verify that the items you selected appear on the list.
1. Do not close the browser window or navigate away from the checkout page.
1. In the task bar, right-click the compute emulator icon and select **Show Compute Emulator UI**. 
1. In the **Compute Emulator**, right-click the **CloudShopService** node and choose **Suspend**. Wait until the service is **destroyed** as indicated by the instance icon turning purple. 
1. Open a command window in elevated administrator mode, from **Start | All Programs | Accessories | Command Prompt** by right clicking the **Command Prompt** shortcut and choosing **Run as administrator**. At the command prompt, type _iisreset_.
1. Now, restart the service instance once again. To do this, right-click the **CloudShopService** node and choose **Run**, then wait for the service to start.
1. Switch back to the browser window showing the checkout page and click **Refresh**. Notice that the order is intact. This confirms that the session state can persist through application restarts when using the Azure provider.
1. Close the browser window to stop the application.

<a name="Exercise3"></a>
### Exercise 3: Using the Azure ASP.NET Providers with Web Form Applications ###

In this exercise, you modify the ASP.NET Web Form application to use the ASP.NET providers from the Microsoft Azure samples. You start by adding authentication to the site using the membership provider. Next, you implement the role provider to classify users and customize the products that the application offers. Finally, you configure the session state provider to store the contents of the shopping cart.

<a name="Ex3Task1"></a>
#### Task 1 – Configuring Authenticated Access to the Application ####
In this task, you configure the application to require authenticated access to the pages that implement the shopping cart. 

1. If necessary, open Visual Studio in elevated administrator mode from **Start | All Programs | Microsoft Visual Studio 2010** by right clicking the **Microsoft Visual Studio 2010** shortcut and choosing **Run as administrator**. 
1. In the **File** menu, choose **Open** and then **Project/Solution**. In the **Open Project** dialog, browse to **Ex3-UsingAzureProvidersWithWebApps\Begin** in the **Source** folder of the lab, select **Begin.sln** and click **Open**.
1. Configure authorization for the **Store** folder to require authenticated access. Open the **Web.config** file of the **CloudShop** project and insert the following (highlighted) configuration block as a direct child of the <**configuration**> element.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex3 Configuring Authorization_)
	<!--mark: 4-11-->
	````XML
	<?xml version="1.0"?>
	<configuration>
	  ...
	  <location path="Store">
	    <system.web> 
	      <authorization>
	        <deny users="?"/>
	        <allow users="*"/>
	      </authorization>
	    </system.web>
	  </location>
	</configuration>
	````

1. Press **CTRL + S** to save the **Web.config** file.
	
<a name="Ex3Task2"></a>
#### Task 2 – Configuring Membership Support Using the Azure TableStorageMembershipProvider ####
In this task, you add and configure the Azure ASP.NET providers for membership, role, and session.

1. Add the Microsoft Azure ASP.NET Providers project to the solution.  In **Solution Explorer**, right-click the **Begin** solution, point to **Add** and select **Existing Project**. Browse to **Assets\AspProviders** in the **Source** folder of the lab, select the **AspProviders.csproj** project and click **Open**.

	>**Note:** The **AspProviders** project is available as a sample. It is included as part of this training kit for your convenience and is based on the original source code found in the [MSDN Code Gallery](http://code.msdn.microsoft.com/windowsazuresamples). The project contains the implementation of ASP.NET application providers for membership, role, profile, and session state.

1. Add a reference in the web role to the **AspProviders** project. In **Solution Explorer**, right-click the **CloudShop** project node and click **Add Reference**. In the **Add Reference** dialog, switch to the **Projects** tab, select the **AspProviders** project and click **OK**.
    
	![Adding a reference to the sample Azure ASP.NET Providers project](Images/adding-a-reference-to-the-sample-azure-aspnet.png?raw=true)
	
	_Adding a reference to the sample Azure ASP.NET Providers project_

1. Update the service configuration to include a connection string to the storage account where the data will be stored. In the **CloudShopService** project, expand the **Roles** node and double-click the **CloudShop** node to open the properties window for this role. 
1. In the **CloudShop[Role]** properties window, select the **Settings** tab and click **Add Setting**. Set the **Name** of the new setting to _DataConnectionString_ and change the **Type** to _Connection String_. Then, in the **Value** column, click the button labeled with an ellipsis.
 
	![Creating a new configuration setting for the role](Images/creating-a-new-configuration-setting-for-the.png?raw=true)
	
	_Creating a new configuration setting for the role_

1. In the **Storage Account Connection String** dialog, choose the option labeled **Microsoft Azure storage emulator** and click **OK**.
 
	![Configuring a storage connection string](Images/configuring-a-storage-connection-string.png?raw=true)
	
	_Configuring a storage connection string_

1. Press **CTRL + S** to save your changes to the role configuration.
1. Open the **Web.config** file located in the root folder of the **CloudShop** project.
1. (Optional) Configure the storage account information required by the ASP.NET providers in the application configuration file. To do this, locate the <**appSettings**> element, which should be empty, and replace it with the following (highlighted) configuration block. If the **appSettings** element is missing, insert it as a direct child of the \<**configuration**> element.

	>**Note:** In addition to the service configuration file, you can also configure the Azure providers in the **Web.config** file of the application. This allows you to host the application outside the Azure fabric and still take advantage of the Azure ASP.NET providers and storage. However, when the application runs in the Microsoft Azure environment, configuration settings in the service configuration file for the ASP.NET providers take precedence over those in the **Web.config** file. By using the Microsoft Azure settings, you can avoid redeploying the application when changing provider settings.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex3 DataConnectionString_)
	<!--mark: 3-5-->
	````XML
	<configuration>
	  ...
	  <appSettings>
	    <add key="DataConnectionString" value="UseDevelopmentStorage=true"/>
	  </appSettings>
	  ...
	</configuration>
	````

1. Configure the application to use the membership provider in the **AspProviders** project. To do this, **replace** the existing \<**membership**> section inside the \<**system.web**> element with the following (highlighted) configuration.

	>**Note:** The default ASP.NET Web Application template in Visual Studio creates the configuration settings for the **AspNetSqlMembershipProvider**, which uses SQL Server for storage.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex3 TableStorageMembershipProvider_)
	<!--mark: 9-26-->
	````XML
	<configuration>
	  ...
	  <system.web>
	    ...
	    <authentication mode="Forms">
	      ...
	    </authentication>

	    <!-- Membership Provider Configuration -->
	    <membership defaultProvider="TableStorageMembershipProvider" userIsOnlineTimeWindow="20" hashAlgorithmType="HMACSHA256">
	      <providers>
	        <clear/>
	        <add name="TableStorageMembershipProvider"
		     type="Microsoft.Samples.ServiceHosting.AspProviders.TableStorageMembershipProvider"
	             description="Membership provider using table storage"
	             applicationName="CloudShop"
	             enablePasswordRetrieval="false"
	             enablePasswordReset="true"
	             requiresQuestionAndAnswer="false"
	             minRequiredPasswordLength="1"
	             minRequiredNonalphanumericCharacters="0"
	             requiresUniqueEmail="true"
	             passwordFormat="Hashed"/>
	      </providers>
	    </membership>

	  </system.web>
	  ...
	</configuration>
	````


	>**Important:** Before you execute the solution, make sure that the start-up project and the start-up page are set. 
	> To set the startup project, in **Solution Explorer**, right-click the **CloudShopService** project and select **Set as StartUp Project**. 
	
	>To designate the start page, in **Solution Explorer**, right-click the **CloudShop** project and select **Properties**. In the **Properties** window, select the **Web** tab and in the **Start Action**, select **Specific Page**. For the Web Form project, set the value of this field to **Store/Products.aspx**.

1. Press **F5** to build and run the application. An initialization procedure may be required the first time you execute an application that uses the storage emulator. If this happens, wait until the procedure is complete and review its status. Click **OK** to continue.
  
	![Development storage initialization procedure status](Images/development-storage-initialization-procedure.png?raw=true)

	_Development storage initialization procedure status_

1. Notice that the application redirects you to the log in page when it starts because the authorization settings now require authenticated access to the Home controller. The membership database is initially empty, so you first need to create an account before you can proceed. In the log in page, click **Register** to access the user registration form.
 
	![Authentication required to proceed](Images/authentication-required-to-proceed2.png?raw=true)

	_Authentication required to proceed_

1. Fill in the registration form and click **Create User** to register your account.
  
	![Creating a new user account](Images/creating-a-new-user-account2.png?raw=true)

	_Creating a new user account_

1. Once you complete the wizard, the application displays a confirmation message. Click **Continue** to proceed to the products page.
 
	![Account successfully created](Images/account-successfully-created2.png?raw=true)

	_Account successfully created_

1. After creating your account, the system automatically logs you in and displays the products page.  Notice your user name displayed in the upper right corner of the window.
 
	![Products page displaying the current user](Images/products-page-displaying-the-current-user2.png?raw=true)

	_Products page displaying the current user_

1. Close the browser window to stop the running application

<a name="Ex3Task3"></a>
#### Task 3 – Configuring Role Support Using the Azure TableStorageRoleProvider ####

In this task, you add role support to the application using the Azure role provider. This requires updating the registration process to capture the role of the user and configuring the settings for the role provider. To demonstrate the use of roles, you update the products page to filter the list of products based on the type of user.

1. Update the registration process to assign a role to the user. Open **Register.aspx** in the **Account** folder and insert the following (highlighted) markup to add a new step in the **CreateUserWizard** control.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex3 Role WizardStep_)
	<!--mark: 3-8-->
	````ASP.NET
		...
        <WizardSteps>
            <asp:WizardStep>
                <div>
                    Choose a customer profile:</div>
                <asp:RadioButtonList ID="roles" runat="server" RepeatDirection="Vertical" RepeatLayout="Flow"
                    CssClass="role" />
            </asp:WizardStep>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
            </asp:CompleteWizardStep>
        </WizardSteps>
		...
	````


1. Add an event handler for the event that fires when the wizard creates the user. Add an **OnCreatedUser** attribute to the **CreateUserWizard** control and set the name of the event handler to **OnCreatedUser**.
 
	![Adding an event handler for the CreatedUser event](Images/adding-an-event-handler-for-the-createduser-e.png?raw=true)

	_Adding an event handler for the CreatedUser event_

1. In **Solution Explorer**, right-click **Register.aspx.cs** and then select **View Code** to open its code-behind file. Insert the following code to define the **OnCreatedUser** event handler.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex3 OnCreatedUser_)
	<!--mark: 1-8-->
	````C#
	protected void OnCreatedUser(object sender, EventArgs e)
	{
	  var list = (RadioButtonList)this.CreateUserWizard1.WizardSteps[0].FindControl("roles");

	  System.Web.Security.Roles.AddUserToRole(
	                             this.CreateUserWizard1.UserName,
	                             list.SelectedItem.Text);
	}
	````


	>**Note:** The code retrieves the role selected in the wizard and then adds the user to this role using the configured role provider.

1. Insert the following highlighted code into the body of the **Page_Load** method to initialize the **CreateUserWizard** control using the roles defined by the application.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex3 Page_Load_)
	<!--mark: 4-16-->
	````C#
	protected void Page_Load(object sender, EventArgs e)
	{
	  this.CreateUserWizard1.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];

	  if (!this.IsPostBack)
	  {
	    var list = (RadioButtonList)this.CreateUserWizard1.WizardSteps[0].FindControl("roles");

	    list.DataSource = System.Web.Security.Roles.GetAllRoles().OrderByDescending(a => a);
	    list.DataBind();

	    if (list.Items.Count > 0)
	    {
	      list.Items[0].Selected = true;
	    }
	  }
	}
	````

1. Add code to the start-up routine to initialize the roles supported by the application. The code creates two roles, _Home_ and _Enterprise_, which the application uses to classify different types of user. Open **Global.asax.cs** and insert the following (highlighted) code into the **Application_Start** method.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex3 Initialize Roles_)
	<!--mark: 6-15-->
	````C#
	protected void Application_Start(object sender, EventArgs e)
	{
	  ...
	  this.LoadProducts();
	    
	  // Initialize the application roles 
	  if (!System.Web.Security.Roles.RoleExists("Home"))
	  {
	    System.Web.Security.Roles.CreateRole("Home");
	  }

	  if (!System.Web.Security.Roles.RoleExists("Enterprise"))
	  {
	    System.Web.Security.Roles.CreateRole("Enterprise");
	  }
	}
	````


1. Change the product page to filter the list of products based on the type of user. Open the  **Products.aspx.cs** code-behind file in the  **Store** folder and insert the following (highlighted) code into the **Page_Init** method, immediately below the line that declares and initializes the **filteredProducts** variable.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex3 Page_Init_)
	<!--mark: 10-14-->
	````C#
	protected void Page_Init(object sender, EventArgs e)
	{
	  var products = this.Application["Products"] as List<string>;
	  var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
		   
	  // add all products currently not in session
	  var filteredProducts = products.Where(item => !itemsInSession.Contains(item));

	  //// Add additional filters here
	  // filter product list for home users
	  if (User.IsInRole("Home"))
	  {
	    filteredProducts = filteredProducts.Where(item =>  item.Contains("Home"));
	  }

	  foreach (var product in filteredProducts)
	  {
	    this.products.Items.Add(product);
	  }
	}

	````

	>**Note:** The inserted code appends an additional filter for users in the _Home_ role that returns only items containing the text _"Home"_.

1. Configure the application to use the role provider in the  **AspProviders** project. In the  **Web.config** file, **replace** the existing  \<**roleManager**> section inside the \<**system.web**> element with the following (highlighted) configuration.

	>**Note:** The default ASP.NET Web Application template in Visual Studio creates the configuration settings for the  **AspNetSqlRoleProvider**, which uses SQL Server for storage, and the  **AspNetWindowsTokenRoleProvider**, which uses Windows groups.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex3 TableStorageRoleProvider_)
	<!--mark: 5-22-->
	````XML
	<configuration>
	  ...
	  <system.web>
	    ...
	    <!-- RoleManager Provider Configuration -->
	   <roleManager enabled="true"
	                 defaultProvider="TableStorageRoleProvider" 
	                 cacheRolesInCookie="true" 
	                 cookieName=".ASPXROLES" 
	                 cookieTimeout="30" 
	                 cookiePath="/" 
	                 cookieRequireSSL="false" 
	                 cookieSlidingExpiration="true" 
	                 cookieProtection="All">
	      <providers>
	        <clear/>
	        <add name="TableStorageRoleProvider"
	             type="Microsoft.Samples.ServiceHosting.AspProviders.TableStorageRoleProvider"
             description="Role provider using table storage"
	             applicationName="CloudShop" />
	      </providers>
	    </roleManager>
	    ...
	  </system.web>
	  ...
	</configuration>
	````

1. Press **CTRL+F5** to build and run the application without debugging.
1. In the logon page, click  **Register** to access the user registration form. Notice that the registration wizard now displays a section to specify the role of the customer.  Create a new user and assign it a _Home_ customer profile.
 
	![Registration page showing role information](Images/registration-page-showing-role-information2.png?raw=true)

	_Registration page showing role information_

1. Logged in as a _Home_ user, proceed to the products page. Notice that the list of products only includes home products.
 
	![Products page showing a filtered list of products based on role](Images/products-page-showing-a-filtered-list-of-prod2.png?raw=true)
	
	_Products page showing a filtered list of products based on role_

1. Click the  **Logout** link in the upper left corner of the application window.
1. Register a new account and assign this user an _Enterprise_ profile. Notice that the list of displayed products differs from that seen by a _Enterprise_ user.

	![Products page showing Enterprise products](Images/products-page-showing-enterprise-products2.png?raw=true)

	_Products page showing Enterprise products_

1. Select a product from the list and click **Add item to cart**. You may repeat the process to store additional items in the cart.
1. Click the **Checkout** link to view the contents of the shopping cart. Verify that the items you selected appear on the list.
 
	![Check out page showing the contents of the shopping cart](Images/check-out-page-showing-the-contents-of-the-sh2.png?raw=true)
	
	_Checkout page showing the contents of the shopping cart_

1. Do not close the browser window or navigate away from the checkout page.
1. In the task bar, right-click the compute emulator icon and select **Show Compute Emulator UI**. 
1. In the **Compute Emulator**, right-click the **CloudShopService** node and choose **Suspend**.
  
	![Suspending the service role instance](Images/suspending-the-service-role-instance.png?raw=true)
	
	_Suspending the service role instance_	

1. Open a command window in elevated administrator mode, from **Start | All Programs | Accessories | Command Prompt** by right clicking the **Command Prompt** shortcut and choosing **Run as Administrator**. At the command prompt, type _iisreset_.
 
	![Restarting Internet Information Server](Images/restarting-internet-information-server.png?raw=true)
	
	_Restarting Internet Information Server_

	>**Note:** These two steps, recycling the role and restarting IIS, simulate what would happen in Microsoft Azure when a role instance is restarted.

1. Go back to the **Compute Emulator** and wait until the service is **destroyed** as indicated by the instance icon turning purple. Now, restart the service instance once again. To do this, right-click the **CloudShopService** node and choose **Run**, then wait for the service to start.
1. Switch back to the browser window showing the checkout page and click **Refresh**. Notice that the order now appears empty.

	>**Note:** The application is currently using inproc session state, which maintains all session state in-memory. When you stop the service instance, it discards all session state including the contents of the shopping cart. In the following task, you will configure the application to store session state in storage, which allows the application to maintain session state in the presence of restarts and across multiple machines hosting the application.

1. Close the browser window to stop the application.

<a name="Ex3Task4"></a>
#### Task 4 – Configuring Session Support Using the Azure TableStorageSessionProvider ####

Microsoft Azure can potentially host a Web role on multiple machines inside the fabric, which makes in-memory session state unsuitable for such an environment. In contrast, the session state provider in the **AspProviders** project uses table storage to store configuration information about the session and blob storage to store the session state itself. 
In this task, you configure the application to use the Microsoft Azure session state provider.

1. Configure the application to use the session provider in the **AspProviders** project. To do this, in the **Web.config** file of the **CloudShop** project, add the sessionState section with the following (highlighted) configuration block as an immediate child of the <**system.web**> element.

	(Code Snippet – _Migrating ASP.NET Applications  - Ex3 TableStorageSessionStateProvider_)
	<!--mark: 5-14-->
	````XML
	<configuration>
	  ...
	  <system.web>
		...
		<!-- SessionState Provider Configuration -->
		<sessionState mode="Custom"
					  customProvider="TableStorageSessionStateProvider">
		  <providers>
			<clear/>
			<add name="TableStorageSessionStateProvider"
				 type="Microsoft.Samples.ServiceHosting.AspProviders.TableStorageSessionStateProvider"
				 applicationName="CloudShop" />
		  </providers>
		</sessionState>
		...
	  </system.web>
	  ...
	</configuration>
	````

1. Press **CTRL+F5** to build and run the application without debugging.
1. Log in and navigate to the products page. Select one or more products from the list and click **Add item to cart**. Repeat the process to store additional items in the cart.
1. Click the **Checkout** link to view the contents of the shopping cart. Verify that the items you selected appear on the list.
1. Do not close the browser window or navigate away from the checkout page.
1. In the task bar, right-click the compute emulator icon and select **Show Compute Emulator UI**. 
1. In the **Compute Emulator**, right-click the **CloudShopService**node and choose **Suspend**. Wait until the service is **destroyed** as indicated by the instance icon turning purple. 
1. Open a command window in elevated administrator mode, from **Start | All Programs | Accessories | Command Prompt** by right clicking the **Command Prompt** shortcut and choosing **Run as administrator**. At the command prompt, type _iisreset_.
1. Now, restart the service instance once again. To do this, right-click the **CloudShopService** node and choose **Run**, then wait for the service to start.
1. Switch back to the browser window showing the checkout page and click **Refresh**. Notice that the order is intact. This confirms that the session state can persist through application restarts when using the Azure provider.
1. Close the browser window to stop the application.

> **Note:** If you want to deploy an ASP.NET Web Forms application in Microsoft Azure, you need to set up the Start Page in the web role project. To do this, open the Web.Config file and add the following lines at the end of the System.WebServer section:

>```` XML
>	<system.webServer>
>	 . . .
>	 <defaultDocument>
>		   <files>
>				<add value="Store/Products.aspx" />
>		   </files>
>	 </defaultDocument>
>	</system.webServer>
>````
> For more information about this issue, please visit this [msdn blog](http://blogs.msdn.com/b/cesardelatorre/archive/2010/07/22/how-to-set-a-default-page-to-a-windows-azure-web-role-app-silverlight-asp-net-etc.aspx).

---

<a name="Summary"></a>
## Summary ##

By completing this hands-on lab, you saw the changes that are necessary to run an existing ASP.NET application in the Microsoft Azure environment. You explored authentication and how to use membership, role, and session state providers that are based on scalable and reliable blob and table storage services to handle applications running on multiple machines inside the Microsoft Azure fabric.
