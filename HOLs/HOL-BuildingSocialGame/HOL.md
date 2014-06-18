<a name="HOLTop"></a>
# Building a Social Game with Microsoft Azure #

---

<a name="Overview"></a>
## Overview ##

Building a game has multiple challenges, more if we are building social games that require multiplayers, contacting your friends, leaderboards and multiple ways to authenticate a user. In this lab, you will see how to take advantage of the **Microsoft Azure** services benefits when building a Social Game. **Microsoft Azure** offers **Access Control** service to authenticate your users using multiple Social Providers, such as **Microsoft Account** or **Facebook**. You can take advantage of **Azure Table Storage** to create a Leaderboard or to use it as a game storage, where clients access to update the game status in their browsers. Additionally, you will see how to use **Node.JS** to emit and broadcast the players game moves to other clients in real time.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Add ACS Support and Invite friends
- Enable Multi-Player using Storage Polling
- Enable Multi-Player using Node.Js
- Create a Leaderboard using Azure Table Storage

<a name="Prerequisites"></a> 
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Microsoft .NET Framework 4.0][1]
- [Microsoft Visual Studio 2010][2]
- [Microsoft Azure SDK and Microsoft Azure Tools for Microsoft Visual Studio 1.7][3]
- [ASP.NET and ASP.NET MVC 4][4]
- [Microsoft(r) Windows Identity Foundation SDK 4.0][5]
- [Microsoft(r) Windows Identity Foundation Runtime][6]
- A Microsoft Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

[1]: http://go.microsoft.com/fwlink/?linkid=186916
[2]: http://msdn.microsoft.com/vstudio/products/
[3]: http://www.microsoft.com/windowsazure/sdk/
[4]: http://www.asp.net/mvc/mvc4
[5]: http://www.microsoft.com/downloads/en/details.aspx?FamilyID=c148b2df-c7af-46bb-9162-2c9422208504
[6]: http://www.microsoft.com/downloads/en/details.aspx?FamilyID=eb9c345f-e830-40b8-a5fe-ae7a864c4d76
 
>**Note:** This hands-on lab has been designed to use Windows 7 Operating System and the latest release of the Microsoft Azure Tools for Visual Studio 2010 (version 1.7).

<a name="Setup"></a> 
### Setup ###
In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the **Source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process that will configure your environment and install the Visual Studio code snippets for this lab.

1. If the User Account Control dialog is shown, confirm the action to proceed.
 
>**Note:** Make sure you have checked all the dependencies for this lab before running the setup.

>**Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

<a name="CodeSnippets"></a> 
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2010 to avoid having to add it manually.

---

<a name="Exercises"></a> 
## Exercises ##

This hands-on lab includes the following exercises:

1. [Adding Social Providers and Inviting Friends](#Exercise1)

1. [Enable Multiplayer with Storage Polling](#Exercise2)

1. [Enable Multiplayer with Node.js](#Exercise3)

1. [Creating a Leaderboard](#Exercise4)
 
Estimated time to complete this lab: **90 minutes**.

>**Note:** Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise. Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.

<a name="GettingStarted"></a> 
### Understanding the Tic-Tac-Toe Solo Game Scenario ###

In this exercise, you will explore the single player Tic-Tac-Toe solution's structure and the logic used for the game flow. Throughout this lab, based on this example, you will update the Tic-Tac-Toe solution to support social gaming features for inviting friends, using ACS and Social Providers to authenticate the players and enabling multiplayer mode. Finally, you will generate a leaderboard by using Azure Table Storage.

<a name="GtsTask1"></a> 
#### Task 1 - Exploring Tic-Tac-Toe Solution ####

In this task, you will explore the Tic-Tac-Toe sample to identify the core libraries that uses and how do they work.

1. Start **Microsoft Visual Studio 2010** as administrator.

1. Open the **Begin.sln** solution located at **Source\Ex0-SoloGame\Begin**.

1. In the **Solution Explorer**, expand **Scripts\Game\TicTacToe** folder within **TicTacToe.Web** node to display the core libraries used in the sample.

 	![Game core libraries](./images/game-core-libraries.png?raw=true "Game core libraries")
 
	_Game core libraries_

	1. **Controllers.js**: This file handles the interaction between the view and the game logic. It includes methods like _start_, _onMove_, _updateGameStatus_, _etc_.

	1. **ViewModel.js**: This file defines the view model structure that will be used to bind the view with the controller.

		>**Note:** We are using the **Knockout** JavaScript library for binding our sample's UI with the controller logic. Knockout is a library used to simplify dynamic JavaScript UIs by applying the Model-View-View Model (MVVM) pattern. You can find more information about Knockout library in this link: [http://knockoutjs.com/](http://knockoutjs.com/).

	1. **Board.js**: This file contains the necessary logic to render the Tic-Tac-Toe board into an HTML5 canvas.

	1. **Game.js**: This class file contains the game logic, which includes player's turns, movements, existence of a winner, etc.

1. Open the **Index.cshtml** view located in the **Views\TicTacToe** folder. This view renders the Tic-Tac-Toe game and interacts with the controller. Notice that this file only contains the canvas element and the reference to the JavaScript libraries.

<a name="GtsVerification"></a> 
#### Verification - Running the Solution ####

In this task, you will run the solution and play a single-player Tic-Tac-Toe game.

>**Note:** To designate the start page, in **Solution Explorer**, right-click the **TicTacToe.Web** project and select **Properties**. In the **Properties** window, select the **Web** tab and in the **Start Action**, select **Specific Page**. Set the value of this field empty.

1. In **Visual Studio**, press **F5** to build and run the solution.

1. In the Social Gaming Sample Home page, click **Tic Tac Toe** in the menu to start a new single-player game.

 	![Running the Tic-Tac-Toe sample](images/running-the-tic-tac-toe-sample.png?raw=true)
 
	_Running the Tic-Tac-Toe sample_

1. Play a new Tic-Tac-Toe game to verify the game's flow. Then, close the browser and return to Visual Studio.

 	![Playing Tic-Tac-Toe, single player mode](images/playing-tic-tac-toe-single-player-mode.png?raw=true)
 
 	_Playing Tic-Tac-Toe, single player mode_
 
<a name="Exercise1"></a> 
### Exercise 1: Adding Social Providers and Inviting Friends ###

In this exercise, you will use Microsoft Azure Access Control Service (ACS) to enable authentication in the Tic-Tac-Toe game. The Access Control Service takes care of engaging every identity provider with its own authentication protocol, normalizing the authentication results in a protocol supported by the .NET framework tooling. In order to simplify the flow of this exercise, you will add the necessary configuration to access the existing public namespace '_local-watgames_', which is used in the **Microsoft Azure Toolkit for Social Games**. Next, you will create a view to send invitations to other users and see the list of friends you added.

>**Note:** In this exercise you will add social gaming features, but the multiplayer mode will be added in the following exercises.

<a name="Ex1Task1"></a>
#### Task 1 - Configuring ACS ####

1. Open the **Begin.sln** solution located in the folder **\Source\Ex1-ACSAndInvite\Begin**. You can alternatively continue working with the solution from Exercise 0.

1. Right-click the **TicTacToe.Web** project and select **Add Microsoft Azure Deployment Project**. This will create a Microsoft Azure deployment project that will host the **TicTacToe.Web** in the Cloud.

1. Open the **Web.config** file from the **TicTacToe.Web** project. You will next add the necessary entries to configure ACS to use the public namespace '_local-watgames_'.

	1. At the top of the file, below the **configuration** root node, add the following code to include the Windows Identity Foundation configuration section.

		<!-- mark: 1-3 -->
		````XML
		  <configSections>
		    <section name="microsoft.identityModel" type="Microsoft.IdentityModel.Configuration.MicrosoftIdentityModelSection, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" />  
		  </configSections>
		````

	1. In the **appSettings** element, add the following new entries with **key** _AcsNamespace_, _ApiUrl_, and _BlobUrl_.
	
		<!-- mark: 1-3 -->
		````XML
		<add key="AcsNamespace" value="local-watgames.accesscontrol.windows.net" />
		<add key="ApiUrl" value="http://127.0.0.1:81/" />
		<add key="BlobUrl" value="http://127.0.0.1:10000/devstoreaccount1/" />
		````

	1. Locate the **system.web** closing node and add the following code above the closing tag.

		<!-- mark: 1-5 -->
		````XML
		   <httpRuntime requestValidationType="Microsoft.Samples.SocialGames.Web.Security.WSFederationRequestValidator" />
		   <httpModules>
		     <add name="WSFederationAuthenticationModule" type="Microsoft.IdentityModel.Web.WSFederationAuthenticationModule, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" />
		     <add name="SessionAuthenticationModule" type="Microsoft.IdentityModel.Web.SessionAuthenticationModule, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" />
		   </httpModules>
		 </system.web>
		````

	1. Inside the **system.webServer** node, replace the **modules** node with the following code:

		<!-- mark: 1-4 -->
		````XML
		<modules runAllManagedModulesForAllRequests="true">
		  <add name="WSFederationAuthenticationModule" type="Microsoft.IdentityModel.Web.WSFederationAuthenticationModule, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" preCondition="managedHandler" />
		  <add name="SessionAuthenticationModule" type="Microsoft.IdentityModel.Web.SessionAuthenticationModule, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" preCondition="managedHandler" />      
		</modules>
		````

	1. At the end of the file, above the closing **configuration** node, add the **Identity Model** and the **System.Service.Model** sections copying the following code.

		<!-- mark: 1-20 -->
		````XML
		  <microsoft.identityModel>
		    <service>
		      <audienceUris>
		        <add value="http://127.0.0.1:81/" />
		      </audienceUris>
		      <federatedAuthentication>
		        <wsFederation passiveRedirectEnabled="false" requireHttps="false" issuer="https://placeholder/" realm="https://placeholder/" />
		        <cookieHandler requireSsl="false" />
		      </federatedAuthentication>
		      <certificateValidation certificateValidationMode="None" />
		      <issuerNameRegistry type="Microsoft.IdentityModel.Tokens.ConfigurationBasedIssuerNameRegistry, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35">
		        <trustedIssuers>
		          <add thumbprint="C89ECEF72C2A79809509E1E294DC68485E5042CF" name="https://local-watgames.accesscontrol.windows.net/" />
		        </trustedIssuers>
		      </issuerNameRegistry>
		    </service>
		  </microsoft.identityModel>
		  <system.serviceModel>
		    <serviceHostingEnvironment aspNetCompatibilityEnabled="true" />
		  </system.serviceModel>  
		````

	1. Press **CTRL+S** to save the file and close it.

1. Create a folder in the root of the **TicTacToe.Web** project and name it _Security_.

1. Right-click the **Security** folder and add an existing item. Browse to the **Source\Assets\Security** folder of the lab, select **WSFederationRequestValidator.cs**, and click **Add**.

<a name="Ex1Task2"></a>
#### Task 2 - Adding Social Gaming APIs ####

In this task, you will add the social gaming server APIs to enable a number of features on your game. This Server APIs (or Web APIs) are wrappers of a library named **SocialGames.Core**, which contains all the necessary logic to invite other users to your list of friends, as well as other features you will use throughout this lab. This library is based on the same library that is included in the **Microsoft Azure Toolkit for Social Games**.

>**Note:** The APIs provided in the **Assets** folder of this lab are using the same strategy and architecture than the one used in the **Microsoft Azure Toolkit for Social Games** projects. For more information, go to [https://github.com/WindowsAzure-Toolkits/wa-toolkit-games](https://github.com/WindowsAzure-Toolkits/wa-toolkit-games).

1. In the **TicTacToe.Web.Azure** cloud project, open **Properties** for the **TicTacToe.Web** role and add a new setting with **Name** _DataConnectionString_, set **Type** to _Connection String_ and for the **Value** choose **Microsoft Azure Storage Emulator**. Save and close the **Properties** page.

 	![Adding a new setting](images/adding-a-new-setting.png?raw=true)
 
	_Adding a new setting_

1. Copy the **SocialGames.Core** folder located in **\Source\Assets\** in your solution folder.

 	![Copying SocialGames.Core project in the solution folder](images/copying-socialgamescore-project-in-the-soluti.png?raw=true)
 
	_Copying SocialGames.Core project in the solution folder_

1. You will add the Social Games core library to the Solution. Right-click the solution node, select **Add** and then **Add Existing Project**. Browse to the **SocialGames.Core** folder you have just copied and select the **SocialGames.Core.csproj** file in order to add the library to your solution.

	>**Note:** The **SocialGames.Core** is composed of four main folders: Common, Entities, Helpers and Repositories. The main logic resides in the Repositories classes.

 	![SocialGames.Core project](images/socialgamescore-project.png?raw=true)
 
	_SocialGames.Core project_

1. In **TicTacToe.Web** project, add a reference to the recently added **SocialGames.Core** project.

1. Add three new folders at the root of the Web project and name them _Services_, _Extensions_ and _WebApi_ respectively.

1. Right-click the **Services** folder, select **Add** and then **Add Exiting Item**. Browse to the **Source\Assets\Services** folder of this lab and select all the files in the folder.

1. Right-click the **Extensions** folder, select **Add** and then **Add Exiting Item**. Browse to the **Source\Assets\Extensions** folder of this lab and select all the files in the folder.

1. Right-click the **WebApi** folder, select **Add** and then **Add Exiting Item**. Browse to the **Source\Assets\WebApi** folder of this lab and select all the files in the folder.

1. The Web APIs and the Extensions you added in the previous steps requires some dependencies that can be installed using the **Package Manager Console**. Click the **TicTacToe.Web** project and then select **Tools | Library Package Manager | Package Manager Console** from the Visual Studio Menu.

1. In the **Package Manager Console** window, type **Install-Package Autofac.mvc4** and wait for installation to complete. Once the process completes, close the **Package Manager Console** window.

	![Installing Autofac Package](images/installing-autofac-package.png?raw=true)

	_Installing Autofac Package_

1. Add the following references to the **TicTacToe.Web** project: 
	- **Microsoft.IdentityModel** 	
	- **Microsoft.WindowsAzure.ServiceRuntime**
	- **Microsoft.WindowsAzure.StorageClient**
	- **System.Json**
	- **System.Net.Http**
	- **System.Net.Http.Formatting**
	- **System.Net.Http.WebRequest**
	- **System.Web.Http**
	- **System.Web.Http.Common**
	- **System.Web.Http.WebHost**

1. Open **Global.asax** file and replace the namespace declarations with the following:

	(Code Snippet - _Building a Social Game - Ex1 Using Namespaces - CS_)

	<!-- mark: 1-19 -->
	````C#
	using System.Collections.Generic;
	using System.Configuration;
	using System.Web.Http;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Autofac;
	using Autofac.Integration.Mvc;
	using Microsoft.IdentityModel.Tokens;
	using Microsoft.IdentityModel.Web;
	using Microsoft.IdentityModel.Web.Configuration;
	using Microsoft.Samples.SocialGames;
	using Microsoft.Samples.SocialGames.Common.Storage;
	using Microsoft.Samples.SocialGames.Entities;
	using Microsoft.Samples.SocialGames.Extensions;
	using Microsoft.Samples.SocialGames.Repositories;
	using Microsoft.Samples.SocialGames.Web.Services;
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.ServiceRuntime;
	using TicTacToe.Web.WebApi;
	````

1. Now that you have the Web APIs in the project, we need to register the MVC routes to point to their address. Locate and replace the **RegisterRoutes** method with the following:

	(Code Snippet - _Building a Social Game - Ex1 Registering Routes - CS_)

	<!-- mark: 1-14 -->
	````C#
    public static void RegisterRoutes(RouteCollection routes)
	{
		routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

		routes.MapHttpRoute(
			name: "ActionApi",
			routeTemplate: "api/{controller}/{action}/{id}",
			defaults: new { id = RouteParameter.Optional });

		routes.MapRoute(
			"Default",
			"{controller}/{action}/{id}",
			new { controller = "Home", action = "Index", id = UrlParameter.Optional });
	}
	````

1. In the **Application_Start** method, you will setup the WCF Web Api configuration and register the core types in a Dependency Injection container using **Autofac**. To do this, replace the **Application_Start** method with the following code:

	>**Note:** For more information about **Autofac** go to [http://code.google.com/p/autofac/](http://code.google.com/p/autofac/).

	(Code Snippet - _Building a Social Game - Ex1 Application Start - CS_)

	<!-- mark: 1-37 -->
	````C#
	protected void Application_Start()
	{
		CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
		{
			string configuration = RoleEnvironment.IsAvailable ?
				RoleEnvironment.GetConfigurationSettingValue(configName) :
				ConfigurationManager.AppSettings[configName];

			configSetter(configuration);
		});

		// Setup AutoFac
		var builder = new ContainerBuilder();
		this.DependencySetup(builder);
		var container = builder.Build();

		DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

		// Setup WCF Web API Config
		var resolver = new AutofacDependencyResolver(container);
		GlobalConfiguration.Configuration.ServiceResolver.SetResolver(
			t => resolver.GetService(t), t => resolver.GetServices(t));

		AreaRegistration.RegisterAllAreas();

		RegisterGlobalFilters(GlobalFilters.Filters);
		RegisterRoutes(RouteTable.Routes);

		FederatedAuthentication.ServiceConfigurationCreated += this.OnServiceConfigurationCreated;

		// Call Initializers
		var initializers = DependencyResolver.Current.GetServices<IStorageInitializer>();
		foreach (var initializer in initializers)
		{
			initializer.Initialize();
		}
	}
	````

1. Add the following methods at the end of the file.

	(Code Snippet - _Building a Social Game - Ex1 DependencySetup - CS_)

	<!-- mark: 1-66 -->
	````C#
	protected void DependencySetup(ContainerBuilder builder)
	{
		// Cloud Storage Account
		builder.RegisterInstance<CloudStorageAccount>(CloudStorageAccount.FromConfigurationSetting("DataConnectionString"));

		// Queues
		builder.RegisterQueue<GameActionStatisticsMessage>(ConfigurationConstants.GameActionStatisticsQueue)
			.AsImplementedInterfaces();
		builder.RegisterQueue<GameActionNotificationMessage>(ConfigurationConstants.GameActionNotificationsQueue)
			.AsImplementedInterfaces();
		builder.RegisterQueue<InviteMessage>(ConfigurationConstants.InvitesQueue)
			.AsImplementedInterfaces();

		// Blobs
		builder.RegisterBlob<UserProfile>(ConfigurationConstants.UsersContainerName, true /* jsonpSupport */)
			.AsImplementedInterfaces();
		builder.RegisterBlob<UserSession>(ConfigurationConstants.UserSessionsContainerName, true /* jsonpSupport */)
			.AsImplementedInterfaces();
		builder.RegisterBlob<Friends>(ConfigurationConstants.FriendsContainerName, true /* jsonpSupport */)
			.AsImplementedInterfaces();
		builder.RegisterBlob<NotificationStatus>(ConfigurationConstants.NotificationsContainerName, true /* jsonpSupport */)
			.AsImplementedInterfaces();
		builder.RegisterBlob<Game>(ConfigurationConstants.GamesContainerName, true /* jsonpSupport */)
			.AsImplementedInterfaces();
		builder.RegisterBlob<GameQueue>(ConfigurationConstants.GamesQueuesContainerName, true /* jsonpSupport */)
			.AsImplementedInterfaces();
		builder.RegisterBlob<UserProfile>(ConfigurationConstants.GamesContainerName, true /* jsonpSupport */)
			.AsImplementedInterfaces();

		// Tables
		builder.RegisterTable<UserStats>(ConfigurationConstants.UserStatsTableName, true /* jsonpSupport */)
			.AsImplementedInterfaces();

		// Repositories
		builder.RegisterType<GameActionNotificationQueue>().AsImplementedInterfaces();
		builder.RegisterType<GameActionStatisticsQueue>().AsImplementedInterfaces();
		builder.RegisterType<GameRepository>().AsImplementedInterfaces();
		builder.RegisterType<IdentityProviderRepository>().AsImplementedInterfaces();
		builder.RegisterType<NotificationRepository>().AsImplementedInterfaces();
		builder.RegisterType<UserRepository>().AsImplementedInterfaces();
		builder.RegisterType<StatisticsRepository>().AsImplementedInterfaces();

		// Controllers
		builder.RegisterControllers(typeof(MvcApplication).Assembly);

		// Services
		builder.RegisterType<AuthService>().AsImplementedInterfaces().AsSelf();
		builder.RegisterType<EventService>().AsImplementedInterfaces().AsSelf();
		builder.RegisterType<GameService>().AsImplementedInterfaces().AsSelf();
		builder.RegisterType<HttpContextUserProvider>().AsImplementedInterfaces().AsSelf();
		builder.RegisterType<UserService>().AsImplementedInterfaces().AsSelf();

		// Api
		builder.RegisterType<AuthController>().AsImplementedInterfaces().AsSelf();
		builder.RegisterType<GameController>().AsImplementedInterfaces().AsSelf();
		builder.RegisterType<UserController>().AsImplementedInterfaces().AsSelf();
		builder.RegisterType<EventController>().AsImplementedInterfaces().AsSelf();
	}

	private void OnServiceConfigurationCreated(object sender, ServiceConfigurationCreatedEventArgs e)
	{
		var sessionTransforms = new List<CookieTransform>(new CookieTransform[] { new DeflateCookieTransform() });
		var sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());

		e.ServiceConfiguration.SecurityTokenHandlers.AddOrReplace(sessionHandler);
	}
	````

<a name="Ex1Task3"></a>
#### Task 3 - Creating the Login View ####

In this task, you will create the Login View that lists the Social Providers from Access Control.

1. Open **TicTacToeController** and add the **Authorize** decorator to the **Index** method. If an unauthorized user tries to access the Tic Tac Toe view, the application will redirect the user to the **LogOn** view.

	<!-- mark: 1 -->
	````C#
	[Authorize]
	public ActionResult Index()
	{
	    return View();
	}
	````

1. In the **Controllers** folder, add a new controller with the name _BaseController_.

1. The **BaseController** reads the values of the addresses for the Blob and API URLs and exposes to the View using the **ViewBag**. To do this, replace the class code with the following highlighted code:

	(Code Snippet - _Building a Social Game - Ex1 BaseController - CS_)

	<!-- mark: 5-14 -->
	````C#
	using System.Web.Mvc;
	
	namespace TicTacToe.Web.Controllers
	{
	    public class BaseController : Controller
	    {
	        protected override void OnActionExecuting(ActionExecutingContext filterContext)
	        {
	            this.ViewBag.BlobUrl = System.Configuration.ConfigurationManager.AppSettings["BlobUrl"];
	            this.ViewBag.ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
	
	            base.OnActionExecuting(filterContext);
	        }
	    }
	}
	````

1. Create a new controller and name it _AccountController_**.**

1. Inherit **AccountController** from the **BaseController** you created in previous steps.

	<!-- mark: 1 -->
	````C#
	public class AccountController : BaseController
	{
	...
	}
	````

1. Add the following using statements in the **AccountController**.

	(Code Snippet - _Building a Social Game - Ex1 AccountController Using- CS_)

	<!-- mark: 1-7 -->
	````C#
	using System.Threading;
	using Microsoft.IdentityModel.Claims;
	using Microsoft.IdentityModel.Protocols.WSFederation;
	using Microsoft.Samples.SocialGames.Repositories;
	using Microsoft.Samples.SocialGames.Web.Services;
	using Microsoft.Samples.SocialGames.Entities;
	using Microsoft.Samples.SocialGames;
	````

1. The **AccountController** is responsible of authenticating and reading Claims from the Identity Provider. The user Id is then saved (or updated if already exists) into a Table Storage to be used in the Game and in the Friends list. Replace the class content with the following:

	(Code Snippet - _Building a Social Game - Ex1 Implementing AccountController - CS_)

	<!-- mark: 3-76 -->
	````C#
	public class AccountController : BaseController
	{
		private readonly IUserRepository userRepository;
		private IUserProvider userProvider;
		
		public AccountController(IUserRepository userRepository, IUserProvider userProvider)
		{
		    this.userRepository = userRepository;
		    this.userProvider = userProvider;
		}
		
		public ActionResult LogOn(string returnUrl)
		{
		    return View();
		}
			
		[HttpPost]
		public ActionResult LogOn()
		{
		    if (this.Request.IsAuthenticated)
		    {
		        // Ensure user profile
		        var userId = this.GetClaimValue(ClaimTypes.NameIdentifier);
		        var userProfile = this.userRepository.GetUser(userId);
		        if (userProfile == null)
		        {
		            var loginType = this.GetClaimValue(ConfigurationConstants.IdentityProviderClaimType);
		            userProfile = new UserProfile
		            {
		                Id = userId,
		                DisplayName = Thread.CurrentPrincipal.Identity.Name ?? string.Empty,
		                LoginType = loginType,
		                AssociatedUserAccount =
		                    loginType.StartsWith("Facebook", StringComparison.OrdinalIgnoreCase) ?
		                        this.GetClaimValue(ConfigurationConstants.FacebookAccessTokenClaimType) :
		                        string.Empty
		            };
		
		            this.userRepository.AddOrUpdateUser(userProfile);
		        }
		
		        var effectiveReturnUrl = this.GetContextFromRequest();
		
		        if (!string.IsNullOrWhiteSpace(effectiveReturnUrl))
		        {
		            return Redirect(effectiveReturnUrl);
		        }
		    }
		
		    return Redirect("~/");
		}
		
		private string GetContextFromRequest()
		{
		    Uri requestBaseUrl = WSFederationMessage.GetBaseUrl(this.Request.Url);
		    var message = WSFederationMessage.CreateFromNameValueCollection(requestBaseUrl, this.Request.Form);
		
		    return message != null ? message.Context : string.Empty;
		}
		
		private string GetClaimValue(string claimType)
		{
		    if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
		    {
		        throw new InvalidOperationException("User is not authenticated");
		    }
		
		    var claimsIdentity = (IClaimsIdentity)Thread.CurrentPrincipal.Identity;
		
		    if (!claimsIdentity.Claims.Any(c => c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase)))
		    {
		        throw new InvalidOperationException("Claim not found: " + claimType);
		    }
		
		    return claimsIdentity.Claims.FirstOrDefault(c => c.ClaimType.Equals(claimType, StringComparison.OrdinalIgnoreCase)).Value;
		}
	}
	````

1. In the **AccountController**, right-click **LogOn** action method and select **Add View**. Name it _LogOn_ and leave the default values for the remaining fields.

1. Replace the entire code of **LogOn** view with the following code.

	<!-- mark: 1-75 -->
	````CSHTML
	@{
		ViewBag.Title = "Login";    
	}

	<h2>Login</h2>

	<p>
		Login using any of the following identity providers:
	</p>

	<div class="status">
	</div>

	<table id="loginsTable">
	</table> 

	<script type="text/javascript">
		function getQueryVariable(variable) {
			var query = window.location.search.substring(1);
			var vars = query.split("&");
			for (var i = 0; i < vars.length; i++) {
				var pair = vars[i].split("=");
				if (pair[0] == variable) {
					return pair[1];
				}
			}
		}

		jQuery(document).ready(function () {
			var apiURL = "@this.ViewBag.ApiUrl";
			
			$(".status").text("Loading...");

			var returnUrl = getQueryVariable("ReturnUrl");

			if (returnUrl == null || returnUrl == undefined)
				returnUrl = "";

			// Get login info
			$.ajax({
				type: "GET",
				url: apiURL + "api/auth/loginselector?returnUrl=" + returnUrl,
				dataType: "json",
				success: function (result) {
					createLoginButtons(result);
				},
				error: function (req, status, error) {
					alert(error);
				}
			});
		});

		// Create the login button for each identity provider
		function createLoginButtons(loginInfoList) {
			var loginsTable = $('#loginsTable');
			loginsTable.empty();

			$(".status").text("");

			$.each(loginInfoList, function (i, loginInfo) {
				var loginButton = $('<input type="button">');

				loginButton.attr("value", loginInfo.Name);
				loginButton.click(function () { window.location.href = loginInfo.LoginUrl; });

				var loginsTableTd = $('<td>');
				loginsTableTd.append(loginButton);

				var loginsTableTr = $('<tr>');
				loginsTableTr.append(loginsTableTd);

				loginsTable.append(loginsTableTr);
			});
		}
	</script>
	````

	The View uses JQuery to consume the Web API **AuthService** to retrieve the list of available providers in the ACS namespace. Once the view has the list, one button is generated for each provider to allow users select any of the available Identity providers.


<a name="Ex1Task4"></a>
#### Task 4 - Creating a Friends List View ####

In this task, you will create a Friends list consuming the server APIs to invite and retrieve the user's friends.

1. In order to interact with the Web APIs on client-side, you need to send a JSON request to the Web API URL. To do this, you will use two JavaScript files that encapsulate the calls to the APIs. Open the **Game** folder inside **Scripts** and add **UserService.js** and **ServerInterface.js** files from **Assets\\Scripts** folder of this lab.

1. In the **TicTacToe.Web** project, add a new View named **Friends** under the **Account** folder.

1. Replace the entire code with the following:

	<!-- mark: 1-24 -->
	````CSHTML
	@{
	    ViewBag.Title = "Your Friends";
	    Layout = "~/Views/Shared/_Layout.cshtml";
	}
	
	<script src="@Url.Content("~/Scripts/jQuery.tmpl.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/knockout-1.2.1.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/ServerInterface.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/UserService.js")" type="text/javascript"></script>
	
	<h2>Your Friends</h2>
	
	<p>Use this URL to invite other users: <span data-bind="text: inviteURL()" />
	</p>
	
	<h3>Friends</h3>
	
	<fieldset>
	    <div data-bind='template: { name: "FriendList", foreach: friends() }' />
	</fieldset>
	
	<script id='FriendList' type='text/html'>
	       ${$data.DisplayName} <br> 
	</script>
	````

1. Add the following code at the bottom of the View. This code interacts with the **UserService.js** to perform JSON requests to the Web APIs. In this case, you will request for the Friends list by calling the **UserService** Web API.

	<!-- mark: 1-43 -->	
	````CSHTML
	<script type="text/javascript">
	    var userId = "@this.ViewBag.CurrentUserId";
	    var BlobUrl = "@this.ViewBag.BlobUrl";
	    var si = new ServerInterface();
	    var user = new UserService(userId, BlobUrl, si);
	
	    function getFriends() {
	        user.getFriendsInfo(function (friends) {
	            window.viewModel.refreshFriends(friends);
	        }, function (req, status, error) {
	            var errorMessage;
	            if (req.responseText == undefined) {
	                errorMessage = "POST Error:\nreq:" + req + "\nstatus:" + status + "\nerror:" + error;
	            }
	            else {
	                errorMessage = "POST Error:\nreq:" + req.responseText + "\nstatus:" + status + "\nerror:" + error;
	            }
	            alert(errorMessage);
	        });
	    }
	</script>
	<script type="text/javascript">
	    function ViewModel() {
	        this.user = ko.observable(userId);
	        this.friends = ko.observableArray();
	        this.notifications = ko.observableArray();        
	        this.inviteURL = ko.observable(document.location.href + "/?id=" + encodeURIComponent(userId));
	    }
	
	    ViewModel.prototype.refreshFriends = function (friends) {
	        for (n in friends)
	            this.friends.push(friends[n]);
	    }
	
	    jQuery(document).ready(function () {
	        window.viewModel = new ViewModel();
	
	        ko.applyBindings(window.viewModel);
	        user.getFriendsInfo(
	            function (friends) { window.viewModel.refreshFriends(friends); },
	            function () { alert('Error Get Friends'); });   
	    });
	</script>
	````

1. Open **GameService.cs** located at the **Services** folder. Replace the contents of the method **Invite** with the following highlighted code:

	(Code Snippet - _Building a Social Game - Ex1 Invite Method - CS_)

	<!-- mark: 3-9 -->
	````C#
	public void Invite(Guid gameQueueId, dynamic formContent)
	{
		var users = formContent.users != null ?
		            ((JsonArray)formContent.users).ToObjectArray().Select(o => o.ToString()).ToList() :
		            null;
		string message = formContent.message != null ? formContent.message.Value : null;
		string url = formContent.url != null ? formContent.url.Value : null;
		
		this.gameRepository.Invite(this.CurrentUserId, gameQueueId, message, url, users);
	}
	````

1. Open the **\_Layout.cshtml** View located in the **Shared** folder under **Views**.

1. Add a new menu item that points to the recently created **Friends** view.

	<!-- mark: 5 -->
	````CSHTML
	<nav>
	    <ul id="menu">
	        <li>@Html.ActionLink("Home", "Index", "Home", new { area = "" }, null)</li>
	        <li>@Html.ActionLink("Tic Tac Toe", "Index", "TicTacToe", new { area = "" }, null)</li>
	        <li>@Html.ActionLink("Friends", "Friends", "Account", new { area = "" }, null)</li>
	    </ul>
	</nav>
	````

1. Now you will add a method in the **AccountController** to retrieve the friends to the view. The Friends View provides you with an invitation URL, which is no more than the same URL with a user Id as a parameter. If the Id is provided, the _Friend_ relationship is saved in an Azure blob container and it will appear in your friends list.

	(Code Snippet - _Building a Social Game - Ex1 AccountController Friends - CS_)

	<!-- mark: 1-16 -->
	````C#
	[Authorize]
	public ActionResult Friends(string id)
	{
	    if (!string.IsNullOrEmpty(id))
	    {
	        string currentUserId = this.userProvider.UserId;
	        string inviteUserId = id;
	
	        this.userRepository.AddFriend(currentUserId, inviteUserId);
	        this.userRepository.AddFriend(inviteUserId, currentUserId);
	        return RedirectToAction("Index", "Home");
	    }
	
	    this.ViewBag.CurrentUserId = this.userProvider.UserId;
	    return View();
	}
	````

1. Press **CTRL+SHIFT+B** to build the solution.

<a name="Ex1Verification"></a>
#### Verification ####

>**Note:** Before you execute the solution, make sure that the start-up project and the start-up page are set. 

> To set the startup project, in **Solution Explorer**, right-click the **TicTacToe.Web.Azure** project and select **Set as StartUp Project**.

> To designate the start page, in **Solution Explorer**, right-click the **TicTacToe.Web** project and select **Properties**. In the **Properties** window, select the **Web** tab and in the **Start Action**, select **Specific Page**. Set the value of this field empty.

1. Press to **F5** to run the solution.

 	![Tic-Tac-Toe Home Page](images/tic-tac-toe-home-page.png?raw=true)
 
	_Tic-Tac-Toe Home Page_

1. In the **Home** page menu, click **Tic Tac Toe**. The browser will redirect you to the **LogOn** view.

1. Select **Windows Live Id** as the identity provider.

 	![Selecting an identity provider](images/selecting-an-identity-provider.png?raw=true)
 
	_Selecting an identity provider_

1. You will be redirected to the **Microsoft Account** login page. Enter your credentials and log on. The page will redirect you back to the Tic-Tac-Toe game.

1. Now that you are authenticated, go to the **Friends** page by clicking **Friends** link in the menu.

1. As this is the first time you run the application with the Web APIs, the Friends list will be empty. Copy the Invitation URL.

 	![Copying the Invitation URL](images/copying-the-invitation-url.png?raw=true)
 
	_Copying the Invitation URL_

1. Open a new session in **Internet Explorer** or an **In-Private** session by pressing **CTRL+SHIFT+P**.

1. Paste the invitation URL in the browse and press enter. In the **LogOn** view, log on with a different account.

1. Go to the **Friends** page. In the Friends list, you will see the user id of your first account.

 	![Showing the Friend’s user id](images/showing-the-friends-user-id.png?raw=true)
 
	_Showing the Friend's user id_

1. Go back to the first browser session, and refresh the **Friends** page. You will see the user id of the second account.

<a name="Exercise2"></a>
### Exercise 2: Enable Multiplayer with Storage Polling ###

In this exercise, you will enable multiplayer for the Tic-Tac-Toe using the Storage Polling approach. This consists in querying a Blob container for game updates and rendering the result in the board. Additionally, you will learn how to invite another user to play a Tic-Tac-Toe game.

<a name="Ex2Task1"></a>
#### Task 1 - Implementing Multiplayer ####

In this task, you will add multiplayer features to the Tic Tac Toe view. To do this, you will update the client-side scripts and modify the view to interact with the game service.

1. Open the **Begin.sln** solution located in the folder **\\Source\\Ex2-AMultiplayerPolling\\Begin**. You can alternatively continue working with the solution obtained by completing Exercise 1.

1. Open the **TicTacToeController** class located in the **Controllers** folder and inherit it from **BaseController**.

	<!-- mark: 1 -->
	````C#
	public class TicTacToeController : BaseController
	{
		...
	}
	````

1. In the **TicTacToe.Web** project, add the **GameService.js** file located in the **Assets\\Scripts** folder of this lab, to the **Scripts\\Game** folder.

1. Expand the **TicTacToe** folder under **Scripts\\Game**, and add the files **Controller.js** and **ViewModel.js** located in the **Assets\\Scripts** folder of this lab. When asked, confirm to replace the old files.

1. Open the **Index.cshtml** view in the **Views\\TicTacToe** folder of the Web project.

1. Add the following references to the core client-side JavaScript files.

	<!-- mark: 3-5 -->
	````CSHTML
	<script src="@Url.Content("~/Scripts/jQuery.tmpl.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/knockout-1.2.1.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/ServerInterface.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/GameService.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/UserService.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/TicTacToe/Board.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/TicTacToe/Game.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/TicTacToe/ViewModel.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/TicTacToe/Controller.js")" type="text/javascript"></script>
	````

1. Add the following code at the beginning of the **div** element with Id **game**. This code renders the Invitation URL and the list of players that are in the queue waiting to play.

	<!-- mark: 1-32 -->
	````CSHTML
	<div style="display: none" data-bind="visible: gameQueueId() != null">
		<fieldset>
			<legend>Player</legend>
			<div>
				Welcome <b><span data-bind="text: playerName()"></span></b>
			</div>
			<div data-bind="visible: isOwner() && gameQueueId() != null && gameId() == null">
				<span>Use this URL to invite other players: <span data-bind="text: inviteURL()" />
				</span>
				<br />
				<div data-bind="visible: friends() != null && friends().length > 0">
				Your Friends
				<select id="friends" data-bind="options: friends, optionsValue: 'Id', optionsText: 'DisplayName'">
				</select>
					<a href="#" data-bind="click: inviteFriend">Invite Friend</a>
				</div>
			</div>
			<br />
			<div data-bind="visible: noPlayers() == 1 && isOwner()">
				<span>Waiting for your opponent...</span>
			</div>
		</fieldset>
	</div>
	<div style="display: none" data-bind="visible: gameQueueId() != null">
		<fieldset>
			<legend>Players</legend>
			<div data-bind='template: { name: "queueStatus", foreach: players()}' />
			<script id="queueStatus" type="text/html">
			<li><b>${$data.UserName}</b></li>
			</script>
		</fieldset>
	</div>
	````

1. Locate the **div** which contains the legend **Game**, and replace it with the following:

	<!-- mark: 1-20 -->
	````CSHTML
	<div style="display: none" data-bind="visible: gameId() != null">
		<fieldset>
			<legend>Game</legend>
			<div>
				You are <span data-bind="visible: playerColor() == TTTColor.Cross">Xs</span> <span
					data-bind="visible: playerColor() == TTTColor.Circle">Os</span>
			</div>
			<div data-bind="visible: playerColor() == currentColor()">
				Your turn!</div>
			<div data-bind="visible: playerColor() != currentColor()">
				Opponent turn!</div>
			<div data-bind="visible: isTie()">
				Tie!!</div>
			<div data-bind="visible: playerColor() == winnerColor()">
				You won!!</div>
			<div data-bind="visible: playerColor() != winnerColor() && winnerColor() != TTTColor.Empty">
				You lose!!</div>
			<canvas id="board" width="300" height="300"></canvas>
		</fieldset>
	</div>
	````

1. Locate the **script** block and insert the following code at the beginning.

	<!-- mark: 1-22 -->
	````JavaScript
	var viewModel;
	var gameQueueId = getQueryVariable("id");
	var nullGameId = "00000000-0000-0000-0000-000000000000";

	function getQueryVariable(variable) {
		var query = window.location.search.substring(1);
		var vars = query.split("&");

		for (var i = 0; i < vars.length; i++) {
			var pair = vars[i].split("=");
			if (pair[0] == variable) {
				return pair[1];
			}
		}
	}

	var apiURL = "@this.ViewBag.ApiUrl";
	var blobURL = "@this.ViewBag.BlobUrl";
	
	var si = new ServerInterface();
	var gs = new GameService(apiURL, blobURL, si);
	var user = new UserService(apiURL, blobURL, si);
	````

1. Next to the previously inserted code, add the following:

	<!-- mark: 1-21 -->
	````JavaScript
	function start(userId) {
	    // check for canvas, show an "Upgrade your browser" screen if they don't have it.
	    var canvas = document.getElementById('board');
	    if (canvas.getContext == null || canvas.getContext('2d') == null) {
	        $("#game").toggle();
	        $("#notSupported").toggle();
	
	        return;
	    }
	
	    var board = new TicTacToeBoard(canvas);
	    var game = new TicTacToeGame();
	    var controller = new TicTacToeController(viewModel, gs, board, game);
	
	    controller.setGameQueueId(gameQueueId);
	    controller.start();
	    user.getFriendsInfo(function (friends) { viewModel.friends(friends); });
	    user.getUser(userId, function () { });
	
	    window.onbeforeunload = function () { controller.finish(); };
	}
	````

1. Replace the body of the **$(function ())** method with the following.

	<!-- mark: 2-12 -->
	````JavaScript
	$(function () {
		viewModel = new TicTacToeViewModel();
		ko.applyBindings(viewModel);

		user.verify(
			function (userId) { start(userId); },
			function () {
				var newlocation = '@Url.Action("LogOn", "Account", new { Area = "", ReturnUrl = "replace-in-js" })';
				newlocation = newlocation.replace("replace-in-js", encodeURIComponent(window.location.href));
				window.location.assign(newlocation);
			}
		);
	});
	````

1. Add the following two methods to the script block.

	<!-- mark: 1-12 -->
	````JavaScript
	function inviteFriend() {
		var userId = $("#friends").val();
		var userName = $("#friends :selected").text();
		gs.inviteUser(viewModel.gameQueueId(), userId, "Invitation for Tic Tac Toe", viewModel.inviteURL(), function () { alert(userName + " was invited") });
	}

	function sgusersCallback(user) {
		if (user.DisplayName != null && user.DisplayName != "")
			viewModel.playerName(user.DisplayName);
		else
			viewModel.playerName(user.Id);
	}
	````

<a name="Ex2Task2"></a>
#### Task 2 - Adding Worker Role to Enable Game Invitations ####

In this task, you will add an existing project that will be in charge of processing invitation requests.

1. Copy the folder **\\Source\\Assets\\SocialGames.Worker** to the root of your project folder.

 	![Copying the worker to the project folder](images/copying-the-worker-to-the-project-folder.png?raw=true)
 
	_Copying the worker to the project folder_

1. Add the project **SocialGames.Worker.csproj** located in the folder you copied (**\SocialGames.Worker\\**) to the solution.

1. In the **TicTacToe.Web.Azure** cloud project, associate the recently added project as a Worker Role in the **Roles** folder.

1. Open the role **Properties** for the **SocialGames.Worker** and add a new setting with **Name** _DataConnectionString_, set the **Type** to _Connection String_ and for **Value** choose **Microsoft Azure Storage Emulator**. Save and close the **Properties** page.

<a name="Ex2Task3"></a>
#### Task 3 - Showing Game Invitation Messages from Friends ####

In this task, you will update the **Friends** view to show the invitation messages from other users.

1. Open the **Friends** view located in **Views\\Account** folder of the Web project.

1. Add the **Invitations** table below the script block with id **FriendList**.

	<!-- mark: 1-19 -->
	````JavaScript
	<h3>Invitations</h3>
	<table>
	    <thead>
	        <tr>
	            <th>Message</th>
	            <th>From</th>
	            <th>Action</th>
	        </tr>
	    </thead>
	    <tbody data-bind='template: { name: "NotificationList", foreach: notifications() }' />
	</table>
	
	<script id="NotificationList" type="text/html">
	<tr>
	       <td>${$data.Message} </td> 
	       <td>${$data.SenderName} </td>
	       <td><a href="${$data.Url}">Go</a> </td> 
	</tr>
	</script>
	````

1. Add a call to the **user.getNotifications** method at the end of **jQuery** **ready** function within the script block in order to retrieve the game invitation messages.

	<!-- mark: 9 -->
	````JavaScript
	jQuery(document).ready(function () {
		window.viewModel = new ViewModel();

		ko.applyBindings(window.viewModel);
		user.getFriendsInfo(
			function (friends) { window.viewModel.refreshFriends(friends); },
			function () { alert('Error Get Friends'); });

		user.getNotifications(userId, function () { });
	});
	````

1. Add a callback method at the end of the block script. This method is used by the **UserService** to return the invitation messages.

	<!-- mark: 1-11 -->
	````JavaScript
	function sgnotificationsCallback(data) {
		for (var n in data.Notifications) {
			var notification = data.Notifications[n];

			if (notification.SenderName == null || notification.SenderName == "")
				notification.SenderName = notification.SenderId;

			if (notification.Type == "Invite")
				viewModel.notifications.push(notification);
		}
	}
	````

<a name="Ex2Verification"></a>
#### Verification ####

>**Note:** Before you execute the solution, make sure that the start-up project and the start-up page are set. 

> To set the startup project, in **Solution Explorer**, right-click the **TicTacToe.Web.Azure** project and select **Set as StartUp Project**.

> To designate the start page, in **Solution Explorer**, right-click the **TicTacToe.Web** project and select **Properties**. In the **Properties** window, select the **Web** tab and in the **Start Action**, select **Specific Page**. Set the value of this field empty.

1. Press to **F5** to run the solution.

1. Click the **Tic Tac Toe** menu link and log on with an identity provider. You will see the current list of Players that are in the same game queue. Additionally, an invitation URL will be generated. Copy this URL.

 	![Listing players and the auto generated invitation URL](images/listing-players-and-the-auto-generated-invita.png?raw=true)
 
	_Listing players and the auto generated invitation URL_

1. Open a new session in **Internet Explorer** or an **In-Private** session by pressing **CTRL+SHIFT+P** without closing the current one.

1. Paste the invitation URL. When prompted, enter other credentials to log on. Now, the list of players will show both users. Check the first browser to verify that both users are listed as well.

 	![Playing a multiplayer Tic-Tac-Toe](images/playing-a-multiplayer-tic-tac-toe.png?raw=true)
 
	_Playing a multiplayer Tic-Tac-Toe_

1. Start playing the game. You will see how the browsers are updated after each move.

1. Restart the game by clicking the **Tic Tac Toe** menu link in the first browser. You will see a new drop-down list that shows your friends.

 	![Listing friends](images/listing-friends.png?raw=true)
 
	_Listing friends_

1. Click the **Invite Friend** link. A confirmation message will appear. Click **OK**.

 	![Invite a friend confirmation](images/invite-a-friend-confirmation.png?raw=true)
 
	_Invite a friend confirmation_

1. In the second browser, click the **Friends** link. The **Invitations** table will show a new message from the other user. The message has a link that takes you to the Tic Tac Toe page with the game queue Id. Click the **Go** link.

 	![Invitation Message](images/invitation-message.png?raw=true)
 
	_Invitation Message_

1. The link takes you to the Tic Tac Toe page and a new game will be started.

1. Close the browsers.

<a name="Exercise3"></a>
### Exercise 3: Enable Multiplayer with Node.Js ###

In this exercise, you will enable multiplayer for the Tic-Tac-Toe game using a different approach than the one from Exercise 2. Instead of querying the storage for updates, you will start a **Node.JS** Worker Role node that enables communication between Web clients by broadcasting incoming messages between connected clients. To do this, Node.JS uses a module named **Socket.IO** that opens Web Sockets between the client browser and the Node server. This way, the client browser _emits_ a message to the Server, which is then _broadcasted_ to other clients (in this case, the other player's browser).

To enable multiplayer, **Node.JS** and **Socket.IO** allows you to send the game's actions to the other player in real-time, without needing to poll the game status.

<a name="Ex3Task1"></a>
#### Task 1 - Adding Node.JS Worker Role to Enable Multiplayer Games ####

In this task, you will add an existing project that includes a Node.JS Worker Role.

1. Open the **Begin.sln** solution located in the folder **\\Source\\Ex3-UsingNodeJs\\Begin**. You can alternatively continue working with the solution obtained by completing Exercise 1.

1. Copy the folder **Source\\Assets\\SocialGames.WorkerNodeJs** to the root of your project.

 	![Copying the WorkerNodeJs project](images/copying-the-workernodejs-project.png?raw=true)
 
	_Copying the WorkerNodeJs project_

1. Add the project **SocialGames.WorkerNodeJs.csproj** located in the folder you have recently added to the Solution.

1. In the **TicTacToe.Web.Azure** cloud project, associate the recently added project as a Worker Role in the **Roles** folder.

1. Open the role **Properties** for the **SocialGames.Worker** and add a new setting with **Name** _DataConnectionString_, set the **Type** _Connection String_ and for **Value** choose **Microsoft Azure Storage Emulator**. Save and close the **Properties** page.

1. Open the **ServiceDefinition.csdef** file and replace the **WorkerRole** block with the following:

	<!-- mark: 1-24 -->
	````XML
	 <WorkerRole name="SocialGames.Worker" vmsize="Small">
	   <Imports>
	     <Import moduleName="Diagnostics" />
	   </Imports>
	   <Startup>
	     <Task commandLine="installSocketIO.cmd" executionContext="elevated" taskType="simple" />
	   </Startup>
	   <Endpoints>
	     <InputEndpoint name="HttpIn" protocol="tcp" port="8080" />
	   </Endpoints>
	   <Runtime>
	     <Environment>
	       <Variable name="PORT">
	         <RoleInstanceValue xpath="/RoleEnvironment/CurrentInstance/Endpoints/Endpoint[@name='HttpIn']/@port" />
	       </Variable>
	       <Variable name="EMULATED">
	         <RoleInstanceValue xpath="/RoleEnvironment/Deployment/@emulated" />
	       </Variable>
	     </Environment>
	   </Runtime>
	   <ConfigurationSettings>
	     <Setting name="DataConnectionString" />
	   </ConfigurationSettings>
	 </WorkerRole>
	````

	>**Note:** The Node.JS Worker Role requires a Startup task in order to install the required module **Socket.IO**. The script automatically download and install the NPM in the application root of Node.JS.

1. In the **TicTacToe.Web** project, add the **GameService.js** file located in the **Assets\NodeJS** folder of this lab, to the **Scripts\\Game** folder. Additionally, add the **socket.io.js** to the root of the **Scripts** folder of the same project.

1. Expand the **TicTacToe** folder under **Scripts\\Game**, and add the files **Controller.js** and **ViewModel.js** located in the **Assets\\NodeJS** folder of this lab. When asked, confirm to replace the old files.

1. Open **Web.config** located at the root of the project and add a new setting under the **appSettings** node. Set its **name** to _NodeJsUrl_ and its **value** to _http://127.0.0.1:8080/_.

	<!-- mark: 3 -->
	````XML
	<add key="ApiUrl" value="http://127.0.0.1:81/" />
	<add key="BlobUrl" value="http://127.0.0.1:10000/devstoreaccount1/" />
	<add key="NodeJsUrl" value="http://127.0.0.1:8080/"/>
 	````
	
<a name="Ex3Task2"></a> 
#### Task 2 -Implementing Multiplayer with Node.JS ####

In this task, you will update the client-side scripts to be able to play a game with two client browsers.

1. First, replace the **TicTacToeController** class located in the **Controllers** folder, to inherit from the **BaseController** and send the API URLs using the **ViewBag**.

	<!-- mark: 1-16 -->
	````C#
	public class TicTacToeController : BaseController
	{
		[Authorize]
		public ActionResult Index()
		{
			this.SetConfigurationData();
			return View();
		}

		private void SetConfigurationData()
		{
			this.ViewBag.BlobUrl = System.Configuration.ConfigurationManager.AppSettings["BlobUrl"];
			this.ViewBag.ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
			this.ViewBag.NodeJsUrl = System.Configuration.ConfigurationManager.AppSettings["NodeJsUrl"];
		}
	}
	````

1. Open the **Index.cshtml** view in the **Views\TicTacToe** folder of the Web project.

1. Add the following highlighted references to the core client-side JavaScript files.

	<!-- mark: 3-6 -->
	````CSHTML
	<script src="@Url.Content("~/Scripts/jQuery.tmpl.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/knockout-1.2.1.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/socket.io.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/ServerInterface.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/GameService.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/UserService.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/TicTacToe/Board.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/TicTacToe/Game.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/TicTacToe/ViewModel.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/TicTacToe/Controller.js")" type="text/javascript"></script>
	````

1. Add the following highlighted code at the beginning of the **div** element with Id **game**. This code renders the Invitation URL and the list of players that are in the queue waiting to play.

	<!-- mark: 1-32 -->
	````CSHTML
	<div style="display: none" data-bind="visible: gameQueueId() != null">
		<fieldset>
			<legend>Player</legend>
			<div>
				Welcome <b><span data-bind="text: playerName()"></span></b>
			</div>
			<div data-bind="visible: isOwner() && gameQueueId() != null && gameId() == null">
				<span>Use this URL to invite other players: <span data-bind="text: inviteURL()" />
				</span>
				<br />
				<div data-bind="visible: friends() != null && friends().length > 0">
				Your Friends
				<select id="friends" data-bind="options: friends, optionsValue: 'Id', optionsText: 'DisplayName'">
				</select>
					<a href="#" data-bind="click: inviteFriend">Invite Friend</a>
				</div>
			</div>
			<br />
			<div data-bind="visible: noPlayers() == 1 && isOwner()">
				<span>Waiting for your opponent...</span>
			</div>
		</fieldset>
	</div>
	<div style="display: none" data-bind="visible: gameQueueId() != null">
		<fieldset>
			<legend>Players</legend>
			<div data-bind='template: { name: "queueStatus", foreach: players()}' />
			<script id="queueStatus" type="text/html">
			<li><b>${$data.UserName}</b></li>
			</script>
		</fieldset>
	</div>
	````

1. Locate the **div** that contains the legend **Game**, and add the following attributes.

	<!-- mark: 1 -->
	````CSHTML
	<div style="display: none" data-bind="visible: gameId() != null">
		<fieldset>
			<legend>Game</legend>
			...
		</fieldset>
	</div>	
	````

1. Replace the **script** block with the following code.

	<!-- mark: 2-24 -->
	````JavaScript
	<script type="text/javascript">
	    var viewModel;
	    var gameQueueId = getQueryVariable("id");
	    var nullGameId = "00000000-0000-0000-0000-000000000000";
	
	    function getQueryVariable(variable) {
	        var query = window.location.search.substring(1);
	        var vars = query.split("&");
	
	        for (var i = 0; i < vars.length; i++) {
	            var pair = vars[i].split("=");
	            if (pair[0] == variable) {
	                return pair[1];
	            }
	        }
	    }
	
	    var apiURL = "@this.ViewBag.ApiUrl";
	    var blobURL = "@this.ViewBag.BlobUrl";
	    var nodeJsURL = "@this.ViewBag.NodeJsUrl";
	    var socket = io.connect(nodeJsURL);
	    var si = new ServerInterface();
	    var gs = new GameService(apiURL, blobURL, si, socket);
	    var user = new UserService(apiURL, blobURL, si);
	</script>
	````

	The previous code instantiates a new Socket pointing to the **Node.JS** server URL. Additionally, the code instantiates the **Game** and **User** services, which interacts with the Web APIs and **Socket.IO**.

1. Add the following code at the end of the **script** block.

	<!-- mark: 1-21 -->
	````JavaScript
	function start(userId) {
	
	    // check for canvas, show an "Upgrade your browser" screen if they don't have it.
	    var canvas = document.getElementById('board');
	    if (canvas.getContext == null || canvas.getContext('2d') == null) {
	        $("#game").toggle();
	        $("#notSupported").toggle();
	        return;
	    }
	
	    var board = new TicTacToeBoard(canvas);
	    var game = new TicTacToeGame();
	    var controller = new TicTacToeController(viewModel, gs, board, game);
	
	    controller.setGameQueueId(gameQueueId);
	    controller.start();
	    user.getFriendsInfo(function (friends) { viewModel.friends(friends); });
	    user.getUser(userId, function () { });
	
	    window.onbeforeunload = function () { controller.finish(); };
	}
	````

	The **start** function initializes the Tic-Tac-Toe board and sets a game queue Id. This Id identifies a game and can be used to invite a Friend to play.

1. Add the following code at the end of the **script** block.

	<!-- mark: 1-12 -->
	````JavaScript
	$(function () {
		viewModel = new TicTacToeViewModel();
		ko.applyBindings(viewModel);
		user.verify(
			function (userId) { start(userId); },
			function () {
				var newlocation = '@Url.Action("LogOn", "Account", new { Area = "", ReturnUrl = "replace-in-js" })';
				newlocation = newlocation.replace("replace-in-js", encodeURIComponent(window.location.href));
				window.location.assign(newlocation);
			}
		);
	 });
	````

1. Add the following code at the end of the **script** block. This enables to invite other users to play the current game from the view.

	<!-- mark: 1-12 -->
	````JavaScript
	function inviteFriend() {
		var userId = $("#friends").val();
		var userName = $("#friends :selected").text();
		gs.inviteUser(viewModel.gameQueueId(), userId, "Invitation for Tic Tac Toe", viewModel.inviteURL(), function () { alert(userName + " was invited") });
	}

	function sgusersCallback(user) {
		if (user.DisplayName != null && user.DisplayName != "")
			viewModel.playerName(user.DisplayName);
		else
			viewModel.playerName(user.Id);
	}
	````

<a name="Ex3Task3"></a> 
#### Task 3 - Showing Game Invitation Messages from Friends ####

In this task, you will update the Friends view to show the invitation messages from other users.

1. Open the **Friends** view located in **Views\\Account** folder of the Web project.

1. Add the **Invitations** table below the **script** block with id **FriendList**.

	<!-- mark: 1-19 -->
	````JavaScript
	<h3>Invitations</h3>
	<table>
	    <thead>
	        <tr>
	            <th>Message</th>
	            <th>From</th>
	            <th>Action</th>
	        </tr>
	    </thead>
	    <tbody data-bind='template: { name: "NotificationList", foreach: notifications() }' />
	</table>
	
	<script id="NotificationList" type="text/html">
	<tr>
	       <td>${$data.Message} </td> 
	       <td>${$data.SenderName} </td>
	       <td><a href="${$data.Url}">Go</a> </td> 
	</tr>
	</script>
	````

1. Add a call to the **user.getNotifications** method in order to retrieve the game invitation messages.

	<!-- mark: 9 -->
	````JavaScript
	jQuery(document).ready(function () {
		window.viewModel = new ViewModel();

		ko.applyBindings(window.viewModel);
		user.getFriendsInfo(
			function (friends) { window.viewModel.refreshFriends(friends); },
			function () { alert('Error Get Friends'); });

		user.getNotifications(userId, function () { });
	});
	````

1. Add a callback method at the end of the block script. This method is used by the **UserService** class to return the invitation messages.

	<!-- mark: 1-11 -->
	````JavaScript
	function sgnotificationsCallback(data) {
		for (var n in data.Notifications) {
			var notification = data.Notifications[n];

			if (notification.SenderName == null || notification.SenderName == "")
				notification.SenderName = notification.SenderId;

			if (notification.Type == "Invite")
				viewModel.notifications.push(notification);
		}
	}
	````

<a name="Ex3Verification"></a> 
#### Verification ####

>**Note:** Before you execute the solution, make sure that the start-up project and the start-up page are set. 

> To set the startup project, in **Solution Explorer**, right-click the **TicTacToe.Web.Azure** project and select **Set as StartUp Project**.

> To designate the start page, in **Solution Explorer**, right-click the **TicTacToe.Web** project and select **Properties**. In the **Properties** window, select the **Web** tab and in the **Start Action**, select **Specific Page**. Set the value of this field empty.

1. Press to **F5** to run the solution.

1. Click the **Tic Tac Toe** menu link and log on with an identity provider. You will see the current list of Players that are in the same game queue. Additionally, an invitation URL will be generated. Copy this URL.

 	![Listing players and the auto generated invitation URL](images/listing-players-and-the-auto-generated-invita2.png?raw=true)
 
	_Listing players and the auto generated invitation URL_

1. Open a new **Internet Explorer** session or an **In-Private** by pressing **CTRL+SHIFT+P** without closing the current one.

1. Paste the invitation URL. When prompted, enter other credentials to log on. Now, the list of players will show both users. Check the first browser to verify that both users are listed as well.

 	![Playing a multiplayer Tic-Tac-Toe](images/playing-a-multiplayer-tic-tac-toe2.png?raw=true)
 
	_Playing a multiplayer Tic-Tac-Toe_

1. Start playing the game. You will see how the browsers are updated after each move.

1. Restart the game by clicking the **Tic Tac Toe** menu link in the first browser. You will see a new drop-down list that shows you friends.

 	![Listing friends](images/listing-friends2.png?raw=true)
 
	_Listing friends_

1. Click the **Invite Friend** link. A confirmation message will appear. Click **OK**.

 	![Invite a friend confirmation](images/invite-a-friend-confirmation2.png?raw=true)
 
	_Invite a friend confirmation_

1. In the second browser, click the **Friends** link. The **Invitations** table will show a new message from the other user. The message has a link that takes you to the Tic Tac Toe page with the game queue Id. Click the **Go** link.

 	![Invitation Message](images/invitation-message2.png?raw=true)
 
	_Invitation Message_

1. The link takes you to the Tic Tac Toe page and a new game will be started.

1. Close the browsers.

<a name="Exercise4"></a>
### Exercise 4: Creating a Leaderboard ###

In this exercise, you will add the necessary logic to generate a Leaderboard using the User's scores and **Azure Table Storage**. Then you will be able to obtain the top users scores ordered by victories and the amount of defeats and played games of each top user.

<a name="Ex4Task1"></a>
#### Task 1 - Creating the Statistics Repository Logic ####

In this task, you will update the Statistics Repository adding the necessary code to save and retrieve statistics.

1. If you completed **Exercise 2** or **Exercise 3** you might continue with the obtained solution, otherwise start **Visual Studio 2010** as Administrator and open **Begin.sln** solution located at **Source\\Ex4-CreatingLeaderboard\\Begin\\\[NodeJs|StoragePolling\]**. You will see two begin solutions: one is using the Storage Polling approach from Exercise 2 and the other is using Node.Js from Exercise 3, choose the one you prefer for this exercise.

1. Open **UserStats** class within **Entities** folder in the **SocialGames.Core** project to check the entity structure used to save the user statistics in the Azure Table Storage.

	<!-- mark: 1-10 -->
	````C#
	public class UserStats : TableServiceEntity
	{
	    public string UserId { get; set; }
	
	    public int GameCount { get; set; }
	
	    public int Victories { get; set; }
	
	    public int Defeats { get; set; }
	}
	````

1. Update the **StatisticsRepository** class to implement the necessary code to save and retrieve user statistics. In the **Repositories** folder within **SocialGames.Core** project, open **StatisticsRepository.cs** file.

1. Add an **IAzureTable** read only property named **statsTable**. The **AzureTable** class, which implements the **IAzureTable** interface, has the necessary methods to interact with the **Azure Table Storage** (in this case, the **UserStats** table).

	(Code Snippet - _Building a Social Game - Ex4 Adding StatisticsRepository Property - CS_)

	<!-- mark: 1 -->
	````C#
	private readonly IAzureTable<UserStats> statsTable;
	````

1. Update the **StatisticsRepository** constructor to set the recently added property when the **StatisticsRepository** class is initialized.

	(Code Snippet - _Building a Social Game - Ex4 StatisticsRepository Constructor - CS_)

	<!-- mark: 3-8 -->
	````C#
	public StatisticsRepository(IAzureTable<UserStats> statsTable)
	{
	    if (statsTable == null)
	    {
	        throw new ArgumentNullException("statsTable");
	    }
	
	    this.statsTable = statsTable;
	}
	````

1. Locate **Initialize** method and update it to call the **CreateIfNotExist** method. This method will create the **UserStats** table in case if it does not already exists.

	(Code Snippet - _Building a Social Game - Ex4 StatisticsRepository Initialize - CS_)

	<!-- mark: 3 -->
	````C#
	public void Initialize()
	{
	    this.statsTable.CreateIfNotExist();
	}
	````

1. Update the **Save** method by replacing its content with the following highlighted code.

	(Code Snippet - _Building a Social Game - Ex4 StatisticsRepository Save - CS_)

	<!-- mark: 3-10 -->
	````C#
	public void Save(UserStats stats)
	{
	    stats.RowKey = EncodeKey(stats.UserId);
	    UserStats currentStat = this.statsTable.Query.Where(item => item.RowKey == stats.RowKey).FirstOrDefault();
	    if (currentStat != null)
	    {
	        this.statsTable.DeleteEntity(currentStat);
	    }
	
	    this.statsTable.AddEntity(stats);
	}
	````

1. Locate the **Retrieve** method and add the necessary code to retrieve the scores for a specific user.

	(Code Snippet - Building a Social Game - Ex4 StatisticsRepository Retrieve - CS)

	<!-- mark: 3 -->
	````C#
	public UserStats Retrieve(string userId)
	{
	    return this.statsTable.Query.Where(item => item.RowKey.Equals(EncodeKey(userId))).FirstOrDefault();
	}
	````

1. Finally, update **GenerateLeaderboard** method. This method returns a **Board** object that contains the board name and an array of Users' Scores.

	(Code Snippet - _Building a Social Game - Ex4 StatisticsRepository GenerateLeaderboard - CS_)

	<!-- mark: 3-26 -->
	````C#
	public Board GenerateLeaderboard(int focusCount)
	{
	    int id = 0;
	    var board = new Board()
	    {
	        Id = ++id,
	        Name = "Victories",
	        Scores = null
	    };
	
	    UserStats[] data = this.statsTable.Query.Take(focusCount).ToArray();
	    board.Scores = new Score[data.Count()];
	    int a = 0;
	    foreach (UserStats stats in data)
	    {
	        board.Scores[a] = new Score()
	        {
	            Id = ++a,
	            UserId = stats.UserId,
	            Victories = stats.Victories,
	            Defeats = stats.Defeats,
	            GameCount = stats.GameCount
	        };
	    }
	
	    return board;
	}
	````

1. Save changes in **StatisticsRepository.cs**.

	>**Note:** The StatisticsRepository class has two static methods: _EncodeKey_ and _DecodeKey_. You will use these methods to encode/decode the UserId when assigning it as RowKey and for querying the Azure Table filtering by RowKey. 
	
	>This is because RowKey and PartitionKey values do not support some characters that might be included in the claims that the ACS providers return. For more information, go to [http://msdn.microsoft.com/en-us/library/windowsazure/dd179338.aspx](http://msdn.microsoft.com/en-us/library/windowsazure/dd179338.aspx).

<a name="Ex4Task2"></a>
#### Task 2 - Adding Statistics Entries ####

In this task, you will update the Worker Role to save each game's statistics in Azure Table Storage.

1. In the **Solution Explorer**, locate the **SocialGames.Worker** project and expand the **Commands** folder.

1. Add **GameActionCommand** and **GameActionStatisticsCommand** classes from **Assets\\Commands** folder.

1. Open the **WorkerRole.cs** class within **SocialGames.Worker** project.

1. Update **Run** method adding the following code immediately below the task builder callback for logging errors declaration.

	(Code Snippet - _Building a Social Game - Ex4 WorkerRole Run - CS_)

	<!-- mark: 10-19 -->
	````C#
	public override void Run()
	{
	...
	    // TaskBuilder callback for logging errors
	    Action<ICommand, IDictionary<string, object>, Exception> logException = (cmd, context, ex) =>
	    {
	        Trace.TraceError(ex.ToString());
	    };
	
	    // Game Action for Statistics messages
	    Task.TriggeredBy(Message.OfType<GameActionStatisticsMessage>(account, ConfigurationConstants.GameActionStatisticsQueue))
	        .SetupContext((message, context) =>
	        {
	            context.Add("gameAction", message.GameAction);
	        })
	        .Do(container.Resolve<GameActionStatisticsCommand>())
	        .OnError(logException)
	        .Start();
	
	...
	}
	````

	In this code, you are registering the command in the job engine, which will execute these instructions asynchronously.

1. Register the new types in a Dependency Injection container using **Autofac**. To do this, add the following code at the end of **DependencySetup** method.

	(Code Snippet - _Building a Social Game - Ex4 WorkerRole DependencySetup - CS_)

	<!-- mark: 1-3 -->
	````C#
	builder.RegisterType<StatisticsRepository>().AsImplementedInterfaces();
	builder.RegisterType<GameActionCommand>();
	builder.RegisterType<GameActionStatisticsCommand>();
	````

1. Open the **GameActionStatisticsCommand** class. Locate the **Do** method and implement the following code to generate and save the statistics. The application will call this method each time a Tic-Tac-Toe game finishes.

	(Code Snippet - _Building a Social Game - Ex4 GameActionStatisticsCommand Do - CS_)

	<!-- mark: 3-17 -->
	````C#
	public override void Do(GameAction gameAction)
	{
	    if (string.IsNullOrWhiteSpace(gameAction.UserId))
	    {
	        return;
	    }
	
	    // Retrieve existent statistics and update them with the new results
	    UserStats currentStatistics = this.statisticsRepository.Retrieve(gameAction.UserId);
	    currentStatistics = UpdateCurrentStats(gameAction, currentStatistics);
	
	    // Generate a new UserStats with the updated RowKey to add in the TableStorage
	    UserStats statistics = null;
	    statistics = CreateUpdatedStats(currentStatistics);
	    statistics.PartitionKey = (int.MaxValue - statistics.Victories).ToString().PadLeft(int.MaxValue.ToString().Length);
	    statistics.RowKey = statistics.UserId;
	    this.statisticsRepository.Save(statistics);
	}
	````

	>**Note:** This solution takes advantage of how Table Storage sorts entities to ensure the top players appear first on the table; that way, retrieving the top(N) entities from the table will return the first N players in the leaderboard sorted correctly (the one with the most victories first.)

	> In Table Storage the Partition and Row Keys are strings, and records are sorted ascendant using first the Partition Key and then the Row Key. Taking this into account, if all the keys have the same amount of characters, and the top player has the smallest PartitionKey (using string comparison) it will appear first in the table.

	> To achieve this, the solution generates the partition key using the following formula: PartitionKey = int.maxValue - UserVictories, and ensures all the keys have the same length by prepending "0" to the resulting number. This guarantees that the first records correspond to those players with the most victories.

	> The downside of using this approach is that, in order to update the player's score, the record needs to be deleted and added again using the new Partition Key; otherwise the table will not be sorted correctly.

1. Add the **UpdateCurrentStats** method that updates the current User's statistics with the latest game's scores.

	(Code Snippet - _Building a Social Game - Ex4 GameActionStatisticsCommand UpdateCurrentStats - CS_)

	<!-- mark: 1-13 -->
	````C#
	private static UserStats UpdateCurrentStats(GameAction gameAction, UserStats originalStatistics)
	{
	    if (originalStatistics == null)
	    {
	        originalStatistics = new UserStats();
	        originalStatistics.UserId = gameAction.UserId;
	    }
	
	    originalStatistics.Victories += GetValue(gameAction.CommandData, "Victories");
	    originalStatistics.Defeats += GetValue(gameAction.CommandData, "Defeats");
	    originalStatistics.GameCount += GetValue(gameAction.CommandData, "GameCount");
	    return originalStatistics;
	}
	````

1. Add the **CreateUpdatedStats** method. This method generates a new **UserStats** object with the updated user's scores.

	(Code Snippet - _Building a Social Game - Ex4 GameActionStatisticsCommand CreateUpdatedStats - CS_)

	<!-- mark: 1-11 -->
	````C#
	private static UserStats CreateUpdatedStats(UserStats originalStatistics)
	{
	    var statistics = new UserStats
	    {
	        UserId = originalStatistics.UserId,
	        Victories = originalStatistics.Victories,
	        Defeats = originalStatistics.Defeats,
	        GameCount = originalStatistics.GameCount
	    };
	    return statistics;
	}
	````

<a name="Ex4Task3"></a>
#### Task 3 - Creating a Leaderboard ####

In this task, you will update the Web project to implement all the functionality added in the previous tasks to generate the leaderboard.

1. In the **Solution Explorer**, locate the **TicTacToe.Web** project and expand the **Views\\Account** folder. Add a new view named **Leaderboard.cshtml**.

1. Replace the **Leaderboard.cshtml** content with the following code.

	<!-- mark: 1-50 -->
	````CSHTML
	@{
	    ViewBag.Title = "TicTacToe Leaderboard";
	}
	
	<h2>Leaderboard</h2>
	<div>
	    <p>
	        The leaderboard shows the scores of the Tic Tac Toe game sample.</p>
	    <p class="status">
	        Loading...
	    </p>
	    <div data-bind="visible: isScoresEmpty()">
	        There are no scores yet. Start playing to gather the leaderboard records.</div>
	    <div data-bind="visible: !isScoresEmpty(), template: { name: 'board', foreach: topScores}">
	    </div> 
	    <script id="board" type="text/html">
	        <table class="style1 board" data-bind='attr: { id: "board" + Id }'>
	            <thead>
	                <tr>
	                    <th class="style1">
	                        Position
	                    </th>
	                    <th class="style1">
	                        Player
	                    </th>
	                    <th class="style1" data-bind='css: { highlight: Name == "Victories" }'>
	                        Victories
	                    </th>
	                    <th class="style1" data-bind='css: { highlight: Name == "Defeats" }'>
	                        Defeats
	                    </th>
	                    <th class="style1" data-bind='css: { highlight: Name == "GameCount" }'>
	                        Game Count
	                    </th>
	                </tr>
	            </thead>
	            <tbody data-bind='template: { name: "score", foreach: Scores }'>
	            </tbody>
	        </table>
	    </script>
	    <script id="score" type="text/html">
	        <tr class="style1" data-bind="css: { d1: Id % 2 == 0, d1: Id % 2 == 1 }">
	            <td class="style1" data-bind="css: { highlight: window.viewModel.currentUserId == UserId }, text: Id"></td>
	            <td class="left-aligned" data-bind="css: { highlight: window.viewModel.currentUserId == UserId }, text: UserName"></td>  
	            <td class="style1" data-bind="css: { highlight: window.viewModel.currentUserId == UserId }, text: Victories"></td>
	            <td class="style1" data-bind="css: { highlight: window.viewModel.currentUserId == UserId }, text: Defeats"></td>
	            <td class="style1" data-bind="css: { highlight: window.viewModel.currentUserId == UserId }, text: GameCount"></td>
	        </tr>
	    </script>
	</div>
	````

1. Add the following script highlighted references at the top of the view. The **UserService.js** library is used to communicate with the Server API UserService and retrieve the top scores for the leaderboard.

	<!-- mark: 5-8 -->
	````JavaScript
	@{
	    ViewBag.Title = "TicTacToe Leaderboard";
	}
	
	<script src="@Url.Content("~/Scripts/jquery.tmpl.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/knockout-1.2.1.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/ServerInterface.js")" type="text/javascript"></script>
	<script src="@Url.Content("~/Scripts/Game/UserService.js")" type="text/javascript"></script>
	
	<h2>Leaderboard</h2>
	````

1. Finally, add the following JavaScript code block to bind the view with the View Model.

	<!-- mark: 1-31 -->
	````JavaScript
	<script type="text/javascript">
	    var apiURL = "@this.ViewBag.ApiUrl";
	    var blobURL = "@this.ViewBag.BlobUrl";
	
	    var si = new ServerInterface();
	    var user = new UserService(apiURL, blobURL, si);
	
	    function ViewModel() {
	        this.isScoresEmpty = ko.observable(false);
	        this.currentUserId = "@this.ViewBag.CurrentUserId";
	        this.topScores = ko.observableArray();
	    }
	
	    window.topScoresCallback = function (items) {
	        window.viewModel.topScores(items);
	        window.viewModel.isScoresEmpty(window.viewModel.topScores().Scores.length == 0);
	        $(".status").hide();
	    };
	
	    $(function () {
	        window.viewModel = new ViewModel();
	
	        user.getLeaderboard(10, topScoresCallback, errorCallback);
	
	        ko.applyBindings(viewModel);
	    });
	
	    function errorCallback(er) {
	        alert("The leaderboard is not available right now, please try later.");
	    }        
	</script>
	````

1. Open **AccountController.cs** file from **Controllers** folder and add the **Leaderboard** action method.

	(Code Snippet - _Building a Social Game - Ex4 AccountController Leaderboard - CS_)

	<!-- mark: 1-6 -->
	````C#
	[Authorize]
	public ActionResult Leaderboard()
	{
	    this.ViewBag.CurrentUserId = this.userProvider.UserId;
	    return View();
	}
	````

1. Open the **UserService.cs** class from **Services** folder and update the **Leaderboard** method to return the statistics board's.

	(Code Snippet - _Building a Social Game - Ex4 UserService Leaderboard - CS_)

	<!-- mark: 3-13 -->
	````C#
	public Board Leaderboard(int count)
	{
		try
		{
			var board = this.statsRepository.GenerateLeaderboard(count);
			this.UpdateUserName(ref board);

			return board;
		}
		catch (Exception ex)
		{
			throw new ServiceException("Could not retrieve statistics from the database. " + ex.Message);
		}
	}
	````

1. Finally, add a new menu item in the **\_Layout.cshtml** view within **Views\Shared** folder.

	<!-- mark: 6 -->
	````CSHTML
	<nav>
	    <ul id="menu">
	        <li>@Html.ActionLink("Home", "Index", "Home", new { area = "" }, null)</li>
	        <li>@Html.ActionLink("Tic Tac Toe", "Index", "TicTacToe", new { area = "" }, null)</li>
	        <li>@Html.ActionLink("Friends", "Friends", "Account", new { area = "" }, null)</li>
	        <li>@Html.ActionLink("Leaderboard", "Leaderboard", "Account", new { area = "" }, null)</li>
	    </ul>
	</nav>
	````

1.  Save all the changes.

<a name="Ex4Verification"></a>
#### Verification ####

>**Note:** Before you execute the solution, make sure that the start-up project and the start-up page are set. 

> To set the startup project, in **Solution Explorer**, right-click the **TicTacToe.Web.Azure** project and select **Set as StartUp Project**.

> To designate the start page, in **Solution Explorer**, right-click the **TicTacToe.Web** project and select **Properties**. In the **Properties** window, select the **Web** tab and in the **Start Action**, select **Specific Page**. Set the value of this field empty.

1. Press to **F5** to run the solution.

1. Click **Leaderboard** in the menu and log in with your Microsoft Account or Facebook account. If this is the first time you run this verification, the leaderboard page should show a _There are no scores yet_ message. You will populate this table by playing some Tic-Tac-Toe games in the following steps.

 	![Empty Leaderboard](images/empty-leaderboard.png?raw=true)
 
	_Empty Leaderboard_

1. To start generating Scores, click **Tic Tac Toe** in the menu and play a Tic-Tac-Toe game.

 	![Generating Statistics playing Tic Tac Toe](images/generating-statistics-playing-tic-tac-toe.png?raw=true)
 
	_Generating Statistics playing Tic Tac Toe_

	>**Note:** You will need to open a second browser and join the Tic-Tac-Toe game using another user account and the invite URL provided in order to generate the game's statistics. Once you finish the game, it will save the scores in the UserStats Azure Table Storage.

1. Go back to the **Leaderboard** section and verify that the Leaderboard table shows two entries one for each user and the scores they have.

 	![Top Scores Leaderboard](images/top-scores-leaderboard.png?raw=true)
 
	_Top Scores Leaderboard_

1. Repeat the **Step 3** using different accounts and verify the updated Leaderboard table.


---

<a name="Summary" />
## Summary ##

In this hands-on lab, you have seen how to take advantage of the **Microsoft Azure** services benefits when building a Social Game.
You have seen how to add **Access Control** service support to an existing application, to allow your users authenticate using multiple Social Providers, such as **Microsoft Account** or **Facebook**.
You have also seen how to enable Multi-Player using **Storage Polling** and **Node.JS** to emit and broadcast the players game moves to other clients in real time.
Finally, you have seen how to create a leaderboard using **Azure Table Storage**.
