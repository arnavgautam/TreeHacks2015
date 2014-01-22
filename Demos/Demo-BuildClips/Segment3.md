<a name="segment3" />
### Segment 3: Building N-Tier Cloud Services with Real-Time Communications ###

In this segment, you will evolve the Video web project into an n-tier solution that includes a background service for monitoring the status of encoding jobs.  We will also enable real-time updates to users for both the web and Windows 8 apps using SignalR, Web Sockets in IIS8, and the Windows Azure Service Bus.

> **Speaking Point:** Uploading a nice seven-second clip of video is good for some scenarios. But for a lot of media scenarios, you might be uploading, you know, hundreds of megabytes of video content. And it's going to take minutes or hours even to encode all of that in all the formats within the cloud. And what we're going to want to be able to do for our client experience to make it a lot richer is be able to provide real-time feedback to the users as to the status of their different encoding jobs. And to do that, we're going to take advantage of a cool library called SignalR. And what SignalR does is to maintain a continuous connection with your clients, and we can use this now to be able to provide continuous feedback from the server to the client in a very efficient, scalable way. And SignalR will scale even to hundreds of thousands or millions of client connections. Now, to make this thing even more scalable, I'm going to also, then, introduce another tier into my application. This is going to be a background service. It's going to be a non-UI service. And what it's going to do is it's going to monitor our Media Services accounts. It's going to look at all the encoding jobs that are going on within it, and it's then just going to feed messages to my Web app with SignalR, which will then broadcast it to the client. And then my UI on the client can just provide a nice, continuous update UI feedback to my users.

1. Check if IIS Express is running in the System Tray. If this is the case, right-click the IIS Express icon and select Exit (confirm to stop all the working processes).

	> **Note:** This is required to stop all running sites and prevent the Windows Azure emulator from deploying the web roles created during this segment in an unexpected port.

1. Switch to the **Visual Studio 2012** instance with the **BuildClips** Web Site project opened.

    > **Speaking Point:** Let's go back to the Web Application. Now, the first step we're going to do is we're going to convert this from being a single-tier website to, instead, being a multitier what we call cloud service inside Windows Azure. So a cloud service can have multiple tiers that run on multiple machines and kind of provide a nice way to compose them together.

1. Right click on the **BuildClips** Web project and select **Add Windows Azure Cloud Service Project**.

    > **Speaking Point:** And converting a website to be a cloud service is really easy. All I need to do is just right-click on the website and say add Windows Azure cloud service project. This is now going to add into my Visual Studio project a Windows Azure cloud service that kind of defines the overall shape and structure of my cloud service app.

	![Add windows azure cloud service project](Images/add-windows-azure-cloud-service-project.png?raw=true)

	_Add windows azure cloud service project_

1. Expand the **BuildClips.Azure** cloud service project, right-click the **BuildClips** role and select **Properties**. In the **Endpoints** tab, change the value of the Public Port column to **81**.

	> **Speaking Point:** And you'll notice it's automatically added a role, think of it like a tier, that points to my website that we built earlier. And I don't have to change any code within the website in this particular scenario. By default, our new role will be listening to port 80. Let me just change it to listen to the same port where our original website was listening.

	![Web role properties option](Images/web-role-properties-option.png?raw=true)

	_Web role properties option_

	![Web role endpoint port option](Images/change-webrole-enpoint-port.png?raw=true)

	_Web role endpoint port_

1. Right click on the **BuildClips.Azure** cloud service project and select **New Worker Role Project** to create a worker role named **BackgroundService**.

    > **Speaking Point:** We can also add additional tiers or roles to our cloud service. Let's add a Worker Role and name it BackgroundService. We will use this worker role to poll Windows Azure Media Services and check on the status of our encoding jobs.  

	![new worker role](Images/new-worker-role.png?raw=true)

	_New Worker Role_

	![Background Service Worker Role](Images/background-service-worker-role.png?raw=true)

	_Background Service Worker Role_

1. Right click on the **BackgroundService** project and select **Add Reference**. In the **Reference Manager** dialog, select the **Solutions** node and then the **BuildClips.Services** project. Click **OK**. 

    > **Speaking Point:** And you'll notice the default template just has a run method and just spins forever, sleeping and writing out trace lines. So what we're going to do, though, is we want this background worker to talk to our media service. And so to do that, we're going to add a reference to our class library that contains all of our media service references.

	![add BuildClips.Service reference](Images/add-myvideosservice-reference.png?raw=true)

	_Add reference_

	![add BuildClips.Service reference 2](Images/add-myvideosservice-reference-2.png?raw=true)

	_Select BuildClips.Service project_

1. Right-click the **BackgroundService** project, select **Manage NuGet Packages** and install the **Windows Azure Media Services .NET SDK** NuGet package.

    > **Speaking Point:** And I'm also going to install the same Media Services library we added previously to our web site, but this time to our new worker role.

	![Install Media Services Package](Images/install-package-mediaservices-project.png?raw=true)
	_Install Media Services Package_

1. Within the same dialog window, install the **Microsoft ASP.NET SignalR Client** NuGet package.

	> **Note:** To find the NuGet search the term **SignalR Client** and make sure you have selected the option **Include Prerelease**


	> **Speaking Point:** Because we're going to be firing messages with SignalR, I'm going to add in a reference to the ASP.NET SignalR client library to this project as well.

	![Install SignalR Client Package](Images/install-package-signalr-client.png?raw=true)

	_Install SignalR Client Package_

1. Replace the configuration file with a new one that has the Media Service connection string values already defined. To do this, right-click the **BackgroundService** project and select **Add | Existing Item**. In the **Add Existing Item** dialog window, select  the file type filter to **All Files (*)**, and then select the **app.config** file inside **[working directory]\Assets\Segment3\BackgroundService**. Finally, click **Add** and then confirm to replace the existing file.

	> **Speaking Point:** To complete all these  configurations in our worker, let me add to the project a pre-baked configuration file with our media service connection values already in place, so we don't need to spend any time doing this.

	![add app.config](Images/add-appconfig.png?raw=true)

	_Add the app.config file_

1. Open the **WorkerRole.cs** file, select the entire **Run** method and insert the following code snippet. Place the cursor over the **HubConnection** type and press **CTRL+.** to add the using statement. Do the same with **VideoService** and **JobStatus** on the lines below.

	> **Speaking Point:** There we go. And then all I'm going to do is just replace this run method here with the following code.

	(Code Snippet - _WorkerRole.cs - Run_)

	<!-- mark:1-26 -->
	````C#
  public override void Run()
  {
		// This is a sample worker implementation. Replace with your logic.
		Trace.WriteLine("BackgroundService entry point called", "Information");

		// Connect to SignalR
		var connection = new HubConnection(CloudConfigurationManager.GetSetting("ApiBaseUrl"));
		var proxy = connection.CreateHubProxy("Notifier");
		connection.Start().Wait();

		while (true)
		{
			 Thread.Sleep(5000);

			 var service = new VideoService();
			 Trace.WriteLine("Getting Media Services active jobs", "Information");
			 var activeJobs = service.GetActiveJobs();

			 foreach (var video in activeJobs.ToList())
			 {
                 proxy.Invoke(
                        "VideoUpdated", 
                        (video.JobStatus == JobStatus.Completed) ? service.Publish(video.Id) : video);
			 }
		}
  }
````

1. Highlight the lines that perform the connection to the SignalR Hub at the beginning of the Run method.

	> **Speaking Point:** And what this is doing is it is connecting first to SignalR and getting what's called a proxy to a hub, which I'll explain more in a little bit.

1. Highlight the main loop that performs the publishing and the call to the proxy.Invoke() method.
	
	> **Speaking Point:** And then it's just also going to do a loop repetitively, sleeping for 5 seconds, and then every five seconds it will wake up. It's going to connect to my video service, and when the job is done, it's then going to publish it and then continuously it's going to be sending updates to my SignalR hub by just invoking the video updated method. That's basically all I need to do in that project.

1. Create a **Hubs** folder in the **BuildClips** project. Right-click the new folder and select **Add |  New Item** and select the SignalR Hub Class template inside Web. Name the class **Notifier.cs** and click **Add**.

    > **Speaking Point:** In my middle tier, then, inside my ASP.NET app, all I need to do is define it as a hub, called notifier. To do this, we'll base on the SignalR Hub Item template that comes with the ASP.NET Fall 2012 Update, which takes care of installing the SignalR core libraries we need.

	![add notifier](Images/add-notifier.png?raw=true)

	_Add the notifier hub_

1. Open **Notifier.cs** file from the **Hubs** folder. Update the content of the class with the method from below. Then, place the cursor over the **Video** type and press **CTRL+.**).

	> **Speaking Point:** So let me do some small changes to this helper method. When this method gets called by the background service, so it's going to be firing messages to it, it's just broadcasting that message to any client that's listening on the hub, and this works with both browsers as well as devices like Windows 8.

	(Code Snippet - _Notifier.cs - VideoUpdated_)
	<!-- mark:3-6 -->
	````C#
	 public class Notifier : Hub
	 {
        public void VideoUpdated(Video video)
        {
            Clients.All.onVideoUpdate(video);
        }
	 }
	````

1. Open **Index.cshtml** in the **Views\Home** folder and insert the following highlighted code into the **Scripts** section.

	(Code Snippet - _Index.cshtml - SignalRNotifications_)
	<!-- mark:2-16 -->
	````C#
@section Scripts {
        <script src="@Url.Content("~/Scripts/jquery.signalR-1.0.0-alpha2.min.js")"></script>
        <script src="@Url.Content("~/signalr/hubs")" type="text/javascript"></script>
        <script>
            $(function () {
                var connection = $.hubConnection();
                var hub = connection.createHubProxy("Notifier");
                hub.on("onVideoUpdate", function (video) {
                    if (video.ThumbnailUrl) {
                        $("#video_" + video.Id).css("background", "url(" + video.ThumbnailUrl + ") no-repeat top left");
                    }
                });

                connection.start();
            });
        </script>

        <script>
            $(function () {
                ...
            });
        </script>
	````


	> **Speaking Point:** We'll update the view to listen for notifications from the SignalR hub and update the video item list whenever an encoding job is ready.  

	> **Note:** Make sure that the script reference to **jquery.signalR-1.0.0-alpha2.min.js** in the code from above matches the script file located inside the **Scripts** folder. The name of this script may change when SignalR is released.

1. Go to the **Windows Azure management portal**. Click **Service Bus** within the left pane. To create a service namespace, click **Create** on the bottom bar. 

	> **Speaking Point:** In order to deliver messages through SignalR at scale, we need to connect it to a backend messaging service.  Windows Azure Service Bus provides a secure and reliable messaging service that can power SignalR and these real-time experiences. Within the Windows Azure Management Portal, we can create a new Service Bus namespace.  

	![Creating a new Service Namespace](Images/service-bus-add-namespace.png?raw=true)

	_Creating a new Service Namespace_

1. Close the **Create a Namespace** Dialog.

1. Now show how we have another **Service Bus** namespace already created.

	> **Speaking Point:** I've already created a namespace named myvideos that we'll use for this demo. 

	![Service Bus in Azure Mgmt Portal](Images/service-bus-in-azure-mgmt-portal.png?raw=true)

1. Right-click the  **BuildClips** project and select **Manage NuGet Packages**. Install the **Microsoft.AspNet.SignalR.ServiceBus** NuGet package. Make sure you have selected the option **Include Prerelease**

    > **Speaking Point:** We will be using the Windows Azure Service Bus to enable delivery of the messages at scale, so let's install the SignalR Windows Azure Service Bus NuGet. 

	![Install Service Bus NuGet UI](Images/install-servicebus-nuget.png?raw=true)

	_Install Service Bus backplane for SignalR NuGet_

1. Select the **Updates** tab and update the **Microsoft.AspNet.SignalR** package to the latest version.

	> **Speaking Point:** Let me now update the SignalR NuGet packages installed by the **SignalR Hub Class** template. Because the ASP.NET Fall 2012 update is still in a preview stage, the hub item template is currently referencing an outdated version of the package.

	![Update SignalR Nugets](Images/update-signalr-nugets.png?raw=true)

	_Update SignalR Nugets_

1. Open the **Global.asax** file in the **BuildClips** project and **type** the following namespaces.

	<!-- mark:1-3-->
	````C#
	using Microsoft.AspNet.SignalR;
	using Microsoft.AspNet.SignalR.ServiceBus;
	````

1. Add the following code at the beginning of the *Application_Start* method to connect the web application with the service bus. Place the cursor over the **RoleEnvironment** and press **CTRL+.** to add the using statement.

	> **Speaking Point:** Now let's connect SignalR to the Service Bus.  When our web application starts, we need to establish a connection to the Service Bus.  We just need to provide SignalR with the Service Bus namespace and key.  

	(Code Snippet - _Global.asax - ServiceBusConfig_)
	<!-- mark:3-7-->
	````C#
    protected void Application_Start()
    {
		// SignalR backplane using the Windows Azure Service Bus
		GlobalHost.DependencyResolver.UseWindowsAzureServiceBus(
				  ConfigurationManager.AppSettings["ServiceBusConnectionString"],
				  topicCount: 5,
				  instanceCount: RoleEnvironment.CurrentRoleInstance.Role.Instances.Count);
		...
	}
	````

1. Press **F5** to start the Cloud Services solution in the compute emulator. 

    > **Speaking Point:** Finally, I can simply press F5 and start running my entire cloud service locally.  This will start the Windows Azure Compute Emulator, which provides a local environment that simulates a multi-tier environment in the cloud.  This is great for debugging and building apps before you deploy them.  

1. Right-click the emulator icon and select **Show Compute Emulator UI**. Show the tracing messages.

    > **Speaking Point:** Let me open the emulator UI so you can see it in action. Here you can see that our application has started.  BuildClips is our web role and BackgroundService is our worker role. 

	![Compute Emulator UI](Images/compute-emulator-ui.png?raw=true)

	_Compute Emulator_

1. Close the compute emulator.


1. Switch to the **Visual Studio 2012** instance with the **Windows 8** project open.

	> **Speaking Point:** Let's go back to the Windows Store application and add the code to connect to SignalR.

1. Open the **Package Manager Console** and install the SignalR Nuget using the following command:
	
	````
	Install-Package Microsoft.AspNet.SignalR.JS -Pre
	````

	> **Speaking Point:** After adding the NuGet package for SignalR, we need to link our client application with the SignalR hub from the Web app. Let's add a class that will serve this purpose by creating a Hub Proxy when the video is still being updated.

1. Right-click the **js** folder and select **Add | New Item**. Select the **JavaScript File** template and name the file **Notifications.js**.

1. Add the following code into **Notifications.js**.

	> **Speaking Point:** So let me add the code to connect to that SignalR hub.

	(Code Snippet - _Notifications.js - SignalRNotifications_)
	<!-- mark:1-23 -->
	````Javascript
(function () {
    	"use strict";

    	var connection = null;

    	WinJS.Namespace.define("Notifications", {
        	connect: connect
    	});

    	function connect() {
        	if (connection == null) {
            	connection = $.hubConnection(Configuration.ApiBaseUrl);
            	var hub = connection.createHubProxy("Notifier");
            	hub.on("onVideoUpdate", function (video) {
                	Data.updateVideoItem(video);
            	});

            	connection.start({ waitForPageLoad: false });
        	}
    	}

    	connect();
})();
	````

1. The above code will display the tracing message when the message for SignalR arrives. Highlight the main parameters of the code:	
	- **Notifier**: The hub name
	- **onVideoUpdate**: The callback method

	> **Speaking Point:** So it's just a few lines of JavaScript, just connecting to the notifier hub, and then I'm just handling inside this Windows 8 the on-video update message, and then when it does it, it's just updating some UI.

	![windows8-signalr-notification](Images/windows8-signalr-notification.png?raw=true)

	_SignalR notification in the Windows 8 app_

1. Open **videoList.html** from the **pages/videoList** folder and add the following script references at the end of the head section.

	> **Speaking Point:** So let's now reference this new JavaScript code from the video list page where the UI updates will take place. As you can see, we are also referencing the SignalR JS library from the NuGet package we just installed.

	(Code Snippet - _videoList.html - SignalRScriptReferences_)
	<!-- mark:1-2 -->
	````HTML
      <script src="/Scripts/jquery.signalR-1.0.0-alpha2.min.js"></script>
      <script src="/js/notifications.js"></script>
   </head>
	````

	> **Note:** Make sure that the script reference to **jquery.signalR-1.0.0-alpha2.min.js** in the code from above matches the script file located inside the **Scripts** folder. The name of this script may change when SignalR is released.

1. Open the **config.js** file inside the **js** folder, and modify the **ApiBaseUrl** property to point to **http://127.0.0.1:81/**.

	> **Speaking Point:** And we are almost done. We just need to make the Win 8 app to point back to our local Web API endpoint, which is now hosted in our new Web app role.

	![modify Web API endpoint](Images/modify-webapi-endpoint.png?raw=true)

	_Modify Web API Endpoint_

1.	Press **F5** to start the Windows 8 application from **Visual Studio**.

	> **Speaking Point:** And now when I go ahead and run this application, what I will see is sort of a view of my app here.

1. Log in to the app with Facebook.

	![Windows 8 app login with Facebook](Images/windows8-facebook-login.png?raw=true)

	_Windows 8 app login with Facebook_

1. Right-click on the screen and click the **Upload** button.

	> **Speaking Point:** I can go ahead now and upload a new video. This is running on my dev machine so I don't have a camera.

	![Upload Video Button](Images/windows8-upload-button.png?raw=true)
	
	_Upload Video Button_

1. Enter a title, description, select some tags, and upload a video from the **[working dir]\Assets\videos** folder.

	![Uploading a Video](Images/windows8-upload-video.png?raw=true)
	
	_Uploading a Video_

	> **Speaking Point:**
	>  This is running on my dev machine so I don't have a camera. So I can select a video. Going to upload it now to my local emulated environment that's running, again, on my local development machine. It's then calling out to Windows Azure Media Services in the cloud, and so it's going to be, again, making another REST call out to Windows Azure Media Services, which is then going to store it, kick off the encoding tasks on it.

1. Once upload is completed, **go back to the video list page** and after a few seconds you will see the "encoding progress bar" in the new video, and the _last updated_ label being refreshed periodically. 

	> **Note:** And now when I go back here, you'll notice that our UI is showing a placeholder for that video. And if you look closely at this time stamp here, what you should see its' changing about every five seconds. And that's basically because our background service is waking up, checking on the status, firing messages to our ASP.NET app, which is then broadcasting it to all the listening clients that are connected.

	![Video encoding notification thumbnail](Images/video-encoding-notification-thumbnail.png?raw=true)
	
	_Video encoding notification_

1. Switch to the **Visual Studio** instance with the **BuildClips** Web application.

	> **Note:** And the beauty about this architecture is it will work not just with one machine on my local desktop, but if I have thousands or hundreds of thousands of clients connected, they'll all get those real-time updates, and I can kind of provide a nice user experience for them to use. So we've built our app now. We can go ahead and deploy it into Windows Azure. 

1. Right-click the **BuildClips.Azure** cloud project and select **Publish**. Click **Import** and open the publish settings file located in **Downloads**.

	> **Speaking Point:** Doing that is pretty easy. You saw how we could right-click and publish a website. I can now do the same thing using Windows Azure Cloud Services. I already imported the publish settings file.

	![Publishing the Cloud Service](Images/cloud-service-publish-signin.png?raw=true "Publishing the Cloud Service")

	_Publishing the Cloud Service_

1. In the Settings page, choose the already created **BuildClips** cloud service and click **Next**.

	> **Speaking Point:** All you do is just right-click on the cloud service and I can then pick what part of the world I want to deploy it into.

	![Visual Studio publish cloud service settings](Images/cloud-service-publish.png?raw=true)

	_Visual Studio publish cloud service settings_

1. Talk about the publishing workflow (do not click **Publish**).

	> **Speaking Point:** When I go ahead and hit publish, Visual Studio packages that up into what's called a service package file. It's going to be like a zip file, and uploads it into Windows Azure. And then what Windows Azure's going to do is it's going to find appropriate servers to run within the datacenter, automatically select them for me, image them with whatever operating system or dependencies I need, and then install my application on them. And once that application is deployed on it, it will automatically wire up a network load balancer and start sending traffic to the application. And the beauty about cloud services is it's fully automated for me. I don't have to manually set up the machines. All that is handled for me by the core platform.

	![Visual Studio publish cloud service summary](Images/cloud-service-publish-summary.png?raw=true)

	_Visual Studio publish cloud service summary_

	> **Note:** In order to publish the service, you first need to change the database connection string to point to an Azure SQL Database (in both web.config and app.config files of the web and worker roles respectively) and also modify the _ApiBaseUrl_ in the app.config file to point to your Windows Azure Cloud Service address (_http://{your-cloud-service}.cloudapp.net_):

