<a name="title" />
# Event Buddy - Microsoft Azure Mobile Services #

---

<a name="Overview"/>
## Overview ##

In this demo you will start with a disconnected application that manages events and sessions to later connect it utilizing **Microsoft Azure Mobile Services** to provide structured storage for events and sessions. In order to use authentication within the application, you will add **Twitter** (or **Facebook**) to your application and services. Following this you will upload session decks to **SkyDrive** and finish by sending Live Tiles using push notifications every time an attendee rates a session.

> **Note:** This demo was designed for **Windows 8** and **Visual Studio Professional** or higher editions. However, it supports Visual Studio Express editions. In that case, the Windows 8 application must be opened with **Visual Studio 2012 Express for Windows 8** and the Windows Phone 8 application must be opened with **Visual Studio 2012 Express for Windows Phone**.

<a name="goals" />
### Goals ###
This demo covers:

1. [Getting Started with Mobile Services to Store Data](#Segment1)
1. [Structured Storage: Connecting your app using Mobile Services](#Segment2)
1. [Authentication, Authorization, and Service-Side Scripts](#Segment3) 
1. [Upload a file to SkyDrive](#Segment4)
1. [Sending Push Notifications for Live Tiles to draw users back to your application](#Segment5) 

<a name="KeyTechnologies" />
### Key Technologies ###
This demo uses the following technologies:

- [Windows Phone SDK 8.0](https://dev.windowsphone.com/en-us/downloadsdk)
- [Live SDK for Windows and Windows Phone](http://msdn.microsoft.com/en-us/live/ff621310)
- [Microsoft Azure Mobile Services SDK for Windows 8](http://go.microsoft.com/fwlink/?LinkId=257545&clcid=0x409)
- [Microsoft Azure Management Portal](http://manage.windowsazure.com/)

<a name="Setup" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment. The setup for this demo is composed of three parts:

 - [Setup for the cloud environment](#SetupCloudEnvironment) - This consists of a set of steps that must be performed only one time for setting up the demo in your Microsoft Azure account.

 - [Setup for the local environment](#SetupLocalEnvironment) - This consists of a set of steps that must be performed only one time for setting up the demo in your local machine.

 - [Reset](#Reset) - The reset steps include a series of manual and automated steps that must be performed before you execute the demo each time.  


<a name="SetupCloudEnvironment" />
#### Setup Cloud Environment - ONE TIME ONLY ####

1. Register the Windows 8 Store app in the Windows Store and associate it using Visual Studio

	Register the application in the Windows Store by following the steps from the section **Register your app for the Windows Store** in this tutorial:
https://www.windowsazure.com/en-us/develop/mobile/tutorials/get-started-with-push-dotnet/#register

	> **Note:** Take note of the **Client Secret** and **Package SID** since you will need them later to configure Push notifications on the Microsoft Azure Mobile Service.

	![Push Notifications Settings](images/push-notification-settings.png?raw=true "Push Notifications Settings")

	Open the **EventBuddy** solution located in {demo_directory}\source\code\begin\WindowsStoreApp. In Visual Studio, right click on the project and select **Store** -> **Associate App with the Store** from the context menu.

	![Associate App with the Store from Visual Studio](images/associate-app-with-store-vs.png?raw=true "Associate App with the Store from Visual Studio")

	Go through the wizard and select the app that you previously registered in the store.
  
	![Select an application](images/associate-app-with-store-select-app-vs.png?raw=true "Select an application")

	![Finish association process](images/associate-app-with-store-finish-vs.png?raw=true "Finish association process")

	> **Note:** You will be required to login with the Microsoft Account that you used to create your Windows Store Account.

	Save the solution and close Visual Studio

1. Create a new **Mobile Service** from the Microsoft Azure Management Portal.

	To do this, log into the [Microsoft Azure Management Portal](https://manage.windowsazure.com) and navigate to Mobile Services and click **New**.
	
	![New Button](images/new-button.png?raw=true)

	Expand **Compute | Mobile Service**, then click **Create**.
 
	![Create new Mobile Service](images/create-new-mobile-service.png?raw=true)
 
	This displays the **New Mobile Service** dialog.

	In the **Create a mobile service** page, type a subdomain name for the new mobile service in the **URL** textbox (e.g: _eventbuddyservice_) and wait for name verification. Once name verification completes, select _Create a new SQL Database_ in the **Database** dropdown list and click the right arrow button to go to the next page.
 
	![New Mobile Service step 1](images/new-mobile-service-step-1.png?raw=true)

	This displays the **Specify database settings** page.

	> **Note:** As part of this demo, you create a new SQL Database instance and server. You can reuse this new database and administer it as you would do with any other SQL Database instance. If you already have a database in the same region as the new mobile service, you can instead choose _Use existing Database_ and then select that database. The use of a database in a different region is not recommended because of additional bandwidth costs and higher latencies.

	In **Name**, type the name of the new database, then type **Login Name**, which is the administrator login name for the new SQL Database server, type and confirm the password, and click the check button to complete the process.
 
	![New Mobile Service step 2](images/new-mobile-service-step-2.png?raw=true)

	> **Note:**  When the password that you supply does not meet the minimum requirements or when there is a mismatch, a warning is displayed.  We recommend that you make a note of the administrator login name and password that you specify; you will need this information to reuse the SQL Database instance or the server in the future.

	You have now created a new mobile service that can be used by your mobile apps.

1. Configure authentication using Twitter.

	> **Note:** Details on how to configure **Mobile Services** authentication with identity providers such as **Twitter** and **Facebook** can be found at https://www.windowsazure.com/en-us/develop/mobile/tutorials/get-started-with-users-dotnet/.

	Create a new Twitter app in https://dev.twitter.com/apps/new

	Configure the Twitter app with the Mobile Service URL.  The URL will be in the format https://{MobileServiceName}.azure-mobile.net/, where {MobileServiceName} is the name of the new Mobile Service you created earlier.  

	![Twitter Callback URL](images/configure-mobile-service-url-twitter.png?raw=true "Twitter Callback URL")

	> **Note:** Take into account that you can choose to authenticate with **Facebook** instead of **Twitter**. In that case, Facebook applications can be created at https://developers.facebook.com/apps.

	> Once the application is created, under _Select how your app integrates with Facebook_, choose **Website with Facebook Login** and insert the mobile service URL in **Site URL**.
	
	> ![Facebook Site URL](images/configure-mobile-service-url-facebook.png?raw=true "Facebook Site URL")	

1. Back in the Microsoft Azure Portal go to the **Identity** tab from the Mobile Service and configure the corresponding settings, such as the application's consumer key and secret.

1. Configure Push Notifications.

	In the **Push** tab of the Mobile Service configure the **Client Secret** and **Package SID** values that you previously obtained when registering the app in the Windows Store.

	![Configure Push Notifications](images/configure-push-notifications.png?raw=true "Configure Push Notifications")

1. Create the Channel table. In the **Data** tab of the Mobile Service, create a table named **Channel**. Set permissions for Insert, Update, Delete, and Read to **"Anybody with the application key"**. 

	![Create Channel Table](images/create-channel-table.png?raw=true "Create Channel Table")

1. Open the table by clicking its name. Browse to the **Script** tab and update the **Insert** script to the following code:

	````JavaScript
	function insert(item, user, request) {
		 item.userId = user.userId;
		 request.execute();
	}
	````

	![Channel Table Insert Script](images/channel-table-insert-script.png?raw=true "Channel Table Insert Script")

<a name="SetupLocalEnvironment" />
#### Setup Local Environment - ONE TIME ONLY ####

1. Install dependencies.

	Open a File Explorer window and browse to the demo's **source** folder. Execute **Setup.cmd** with Administrator privileges to launch the setup process that will configure your environment. Make sure that all dependencies are installed before proceeding to the next step.

	![Installing dependencies](images/installing-dependencies.png?raw=true "Installing dependencies")
	
	_Installing dependencies_

1. Configure the Mobile Service URL and access key.

	Get the **Mobile Service URL** and **Mobile Service Key** values. Browse to your Mobile Service dashboard, copy the service URL and click **Manage Keys** on the bottom bar.

	![Mobile Service URL](images/mobile-service-settings-dashboard.png?raw=true "Mobile Service URL")

	_Mobile Service URL_

	Now copy the **Application Key** value.

	![Mobile Service Access Key](images/mobile-service-settings-keys.png?raw=true "Mobile Service Access Key")

	_Mobile Service Access Key_

	Open the file **Config.Azure.xml** and update the **mobileServiceUrl** and **mobileServiceKey** values.

	````XML
	<configuration>
		<mobileServiceUrl>https://[Mobile Service Name].azure-mobile.net/</mobileServiceUrl>
		<mobileServiceKey>[Mobile Service Key]</mobileServiceKey>
	</configuration>
	````

1. Configure the package name of the Windows Store application.

	Open the **EventBuddy** solution located in {demo_directory}\source\code\begin\WindowsStoreApp. In Visual Studio, double click on the file **Package.appxmanifest** and go to the **Packaging** tab. Copy the **Package name** value.

	![Package Name in application manifest](images/package-name-app-manifest.png?raw=true "Package Name in application manifest")

	Open the file **Config.Local.xml** and update the **packagename** value with the one you obtained in the previous step.
	
	````XML
    <appSetup>
      <packagename>[Package Name in App Manifest]</packagename>
      <appxPath>\WindowsStoreApp\EventBuddy\AppPackages\EventBuddy_1.0.0.0_AnyCPU_Test</appxPath>
    </appSetup>
	````

1. Configure the setup script to use the **Windows Phone Emulator** or a **Windows Phone Device**

	Open the file **Config.Local.xml** and update the **usePhoneEmulator** setting to **true** if you plan to run the application within the Windows Phone Emulator, if you will use a Windows Phone Device you can leave the setting with **false** value.
	
	````XML
    <settings>
        <!-- Change this setting to true if you plan to run the application within Visual Studio Phone Emulator -->
        <usePhoneEmulator>false</usePhoneEmulator>
    </settings>
	````

1. Display the IE **Favorites** bar. To do this, open Internet Explorer, right-click on the favorites star and select **Favorites bar**

	![Display Favorites bar](images/display-favorites-bar.png?raw=true "Display Favorites bar")

1. Follow the steps described in the section **Reset Environment**.

<a name="Reset" />
### Reset Environment ###

1. If you previously executed the demo, delete the following tables (and their contents) in Microsoft Azure Mobile Services.
	- Event table
	- Rating table
	- Session table

	![Delete Tables](images/delete-tables.png?raw=true "Delete Tables")

1. Run the script **Reset.Azure.cmd** as Administrator.

	![Running reset Azure script](images/running-reset-azure-script.png?raw=true "Running reset Azure script")

1. Run the script **Reset.Local.cmd** as Administrator.

	![Running reset Local script](images/running-reset-local-script.png?raw=true "Running reset Local script") 

1. Switch to the **Internet Explorer** instance started by the reset script and log in the **Microsoft Azure Management Portal**.

1. Deploy the Windows Phone 8 app to a **Windows Phone** device.

	> **Note:** If you configured the **usePhoneEmulator** setting to true you don't need to follow these steps. A new instance of Visual Studio will be automatically opened with the Windows Phone 8 app solution and you'll only need to run the app (F5) to start the emulator. 

	Before you continue, please make sure that you have all the prerequisites to deploy an application to a Windows Phone device. Detailed information on this subject can be found in [Deploying and testing apps on your Windows Phone](http://msdn.microsoft.com/en-US/library/windowsphone/develop/gg588378\(v=vs.105\).aspx).

	Make sure the device is connected to the development computer, turned on, and unlocked.

	In Visual Studio on the Standard toolbar, select **Device.**

	![Selecting Phone Device](images/selecting-phone-device.png?raw=true "Selecting Phone Device") 

	To deploy without running the application, on the **Build** menu, click **Deploy Solution**.

	![Deploying to Windows Phone device](images/deploying-phone-app.png?raw=true "Deploying to Windows Phone device") 

1. Start the Event Buddy app in Visual Studio. If you are prompted to uninstall the existing app, click yes and continue.

	![reset-uninstall-app](images/reset-uninstall-app.png?raw=true)
	
1. Start the demo on the Windows 8 start screen.

<a name="knownissues"/>
### Known Issues ###

1. In Segment 5, when trying to send a push notification using the Windows Phone 8 application, the client might not receive the notification. This is likely caused by (the user) not having registered the application in the Windows Store as part of the demo setup.

	If you review your Mobile Services logs you might find the following error:

	_Error in script '/table/Rating.insert.js'. Error: The cloud service is not authorized to send a notification to this URI even though they are authenticated._ 

	Follow the steps in  the section [Setup for the cloud environment](#SetupCloudEnvironment) to register your application with the Windows Store to resolve the issue.

1. If you try to upload a file to SkyDrive and you receive a message saying that there was an error while logging in to Skydrive, it is likely caused by (the user) not having registered the application in the Windows Store as part of the demo setup.

	Follow the steps in  the section [Setup for the cloud environment](#SetupCloudEnvironment) to register your application with the Windows Store to resolve the issue.

3. When uploading a file with the Windows 8 application, the link returned by the Skydrive API to download it **expires in 1 hour**. After that time, you won't be able to download the file with your Windows Phone client.

<a name="Demo" /> 
## Demo ##

<a name="OpeningStatement" />
### Opening Statement ###

> **Speaking Point:** The best mobile apps are connected to the cloud. With Microsoft Azure Mobile Services, any developer can add the cloud services he needs into an application. And what I really love about mobile services is how quickly you can go from idea to execution.
> 
> I'm going to prove this to you now by doing a demonstration and connecting an offline application in just a few minutes.

<a name="Segment1" />
### Segment 1: Getting Started with Mobile Services to store data ###

> **Speaking Point:** Here's an application I wanted to share with you. It's called Event Buddy. It's the sort of application you might use if you run local community events or code camps or conferences.

1. Launch the Event Buddy app from the Windows Start screen

	![start-before-demo](images/start-before-demo.png?raw=true)

	> **Speaking Point:** I can create an event here. Let's call it Redmond Code Camp. Let me save that and then I can drill into that event and add sessions.

1. On the Event Buddy home page, click **Add Event**.

	![eventbuddy-home](images/eventbuddy-home.png?raw=true)

1. Enter an event name and press **Save Event**.

	![EventBuddy-AddEvent](images/eventbuddy-addevent.png?raw=true)

1. Select the event to view the **Event Details**.
	
	![EventBuddy-selectevent](images/eventbuddy-selectevent.png?raw=true)

	> **Speaking Point:** Let's create a session on Microsoft Azure and save that.

1. Swipe up to show that we can also add sessions.

	![EventBuddy-sessions-commandbar](images/eventbuddy-sessions-commandbar.png?raw=true)

1. Navigate back to the first screen and show that our data is gone.
	
	![EventBuddy-datagone](images/eventbuddy-datagone.png?raw=true)

	> **Speaking Point:** And this will be a really useful application if you run such events. However, it's inherently limited in its potential right now because this is not connected, doesn't use cloud services, the data is all local and just in memory. 

<a name="Segment2" />
### Segment 2: Structured Storage: Connecting your app using Mobile Services ###

> **Speaking Point:** The end user experience for this application can be enriched by adding backend services to create scenarios and experiences that span multiple devices and users. I am now going to show you how you can do this with Microsoft Azure Mobile Services.

1. Switch to the Microsoft Azure Management Portal

	> **Speaking Point:** So let's switch over to the Microsoft Azure portal. And you'll see here we're on the mobile services tab. And creating a new mobile service is incredibly easy. I click new, compute, mobile service, create. I enter the name of my mobile service, and this guide will take me through the steps, just two steps necessary. Only takes about 15 seconds.   

1. Click on the New link in the lower left corner and select Compute -> Mobile Service -> Create from the fly out menu.

	![portal-create-mobileservice](images/portal-create-mobileservice.png?raw=true)

	> **Speaking Point:** For the sake of time, I've created one for our demo named **EventBuddyDemo**.  

1. Close the **New Mobile Service** dialog.

	> **Note:**  The pre-existing Mobile Service is to save time.  It has a Channel table, Push Notifications and Twitter authentication configured to avoid lots of cut/paste context switching during the demo.  

1. Navigate to the Mobile Services section of the management portal and show that we have a Mobile Service created already.

	![portal-existing-mobileservice](images/portal-existing-mobileservice.png?raw=true)


	> **Speaking Point:** When I enter the Mobile Service, I'm greeted by our quick start, which is a short tutorial to help developers get their apps connected. 

1. Select the mobile service you created when setting up the demo and show the dashboard for the mobile service.

	![portal-mobileservice-dashboard](images/portal-mobileservice-dashboard.png?raw=true)

	> **Speaking Point** Here you can see the tables that we can access through Mobile Services. We have already created a Channel table which we will use a bit later in the demo. Let's create an Event table. 

1. Navigate to the **Data** Tab and add table named **Event**.

	![portal-newtable-event2](images/portal-newtable-event2.png?raw=true)

1. Click **OK** to close the confirmation panel.


	> **Speaking Point** In just a few seconds our table is created and can be accessed through Microsoft Azure Mobile Services.
	![portal-createtable-confirmation](images/portal-createtable-confirmation.png?raw=true)

1. Navigate back to the quick start page.

	> **Speaking Point:** So we're building a Windows Store app right now. So let's have a look here. You see we support two flows, connecting a new Windows Store app, and connecting an existing app. Now, since I already have an app, I'm going to choose that latter flow.

1. Select **Windows Store** and then select **Connect an Existing Windows Store app**

	> **Speaking Point:** Let's copy this code to the clipboard because I'm going to use that in a little while.

1. Copy the **MobileServiceClient** code for C#.

	![portal-quickstart](images/portal-quickstart.png?raw=true)

	> **Speaking Point:** I'm going to switch over to Visual Studio, and we're going to connect this project.

1. Return to Visual Studio and stop the app. 

1. Open the **App.xaml.cs** if it is not already open.
	
	> **Speaking Point:** So I'm going to paste in the code that we took from the quick start, and that's going to bootstrap the application and point it at the mobile service.

1. Paste in the **MobileServiceClientCode**.

	````C#
	public static MobileServiceClient MobileService = new MobileServiceClient(
		"https://{mobile-service-name}.azure-mobile.net/", 
		"{mobile-service-key}"
	);
	````

	> **Speaking Point:** With one line of code we can simply establish a connection to Microsoft Azure Mobile Services from our Windows Store app.

1. Select the **MobileServiceClient** from the above code and add a using statement for the **Microsoft.WindowsAzure.MobileServices** namespace by entering ***CTRL + .*** and then Enter in the context menu. 


	> **Speaking Point:** And then over in the events page, I'm going to add the code necessary to insert data into Microsoft Azure.

1. Open **EventsPage.xaml.cs**

1. Navigate to the **SaveEvent** method and locate _//TODO: save the new event_.  Replace it with the following code.  

	(Code Snippet - _wamsEventInsert_)

	````C#
	await App.MobileService.GetTable<Event>().InsertAsync(item);
	````

	> **Speaking Point:** Then I want to get a table that I'm going to work with. And in this case, I'm going to work with the event table.

1. Navigate to the **LoadEvents** method and locate the comment _//TODO: query for existing events_.  Replace it with the following code.

	(Code Snippet - _wamsEventQuery_)

	````C#
	Events = await App.MobileService.GetTable<Event>().ToEnumerableAsync();
	````

	> **Speaking Point:** So let's run this application and see what's different about it.

1. Run the application by starting it from Visual Studio.

	> **Speaking Point:** I'm going to add an event and save that. Now that data is in Azure.

1. Create a new Event by clicking on the **Add Event** link on the home page in the Event Buddy Windows Store app. Enter an event name and press **Save Event** in the Add Event dialog. 

	![eventbuddy-addevent-connected](images/eventbuddy-addevent-connected.png?raw=true)

1. After saving the event, leave the Event Buddy Windows Store application running and return to the Microsoft Azure Management portal.

	> **Speaking Point:** And to prove that to you, I'm going to switch back to the portal, I'm going to drill into the event table, and right here in the browser we can see the data's stored in Microsoft Azure.

1. Navigate to the Data view for the Mobile Service and select the Event table. Show that the event has been saved.	
		![portal-data-event](images/portal-data-event.png?raw=true)
	
	> **Transition:** Now that we have our data in the cloud, we may also want to authenticate our users and add logic that is executed on the server side.

<a name="Segment3" />
### Segment 3: Authentication, Authorization, and Service-Side Scripts ###

> **Speaking Point:** You'll see that we've preconfigured this service to work with Microsoft accounts, to work with Facebook, Twitter and Google. And, actually, logging in on the client is equally simple.

1. In the Microsoft Azure Management Portal click on the **Identity** tab.

	![portal-identity](images/portal-identity.png?raw=true)

1. Scroll down the page to see the **Twitter** Settings.

	![portal-identity-twitter](images/portal-identity-twitter.png?raw=true)

	> **Speaking Point:** I'm going to add a session table to store our session data. But, I only want authenticated users to be able to work with the data inside this table, and I can do that by simply setting policy right here in the browser.

1. Navigate to the **Data** tab and click **Add** to add a new table.

1. Create a new table named **Session** and set the permissions to require **Only Authenticated Users**.

	![portal-newtable-session](images/portal-newtable-session.png?raw=true)

	> **Speaking Point:** So I'm going to save that, and this authentication mechanism works in harmony with our support for multiple identity providers. Let me just stop the app from running.

1. Switch back to Visual Studio and Stop debugging (press **Shift+F5**). 

	> **Speaking Point:** I'll go to our login page and enter the line of code it takes to log in via Twitter. I call LoginAsync and pass in Twitter, and that's going to log the user in.

1. Open the **Login.xaml.cs** file.

1. Locate the **LoginTwitter** method and add the following code.
	
	(Code Snippet - _wamsLoginTwitter_)

	````C#
	await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
	````

	> **Note:** Please take into account that if you choose to authenticate with Facebook, instead of executing the previous step add the following code in the LoginFacebook method.

	(Code Snippet - _wamsLoginFacebook_)

	````C#
	await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
	````


	> **Speaking Point:** Now, before I show you that, let's just finish doing our data code for the session table. So I need to insert the save, load and update session code.

1. Open **SessionsPage.xaml.cs** 

1. Navigate to the **SaveSession** method and locate _//TODO: Save Session_. Replace the comment with the following code:

	(Code Snippet - _wamsSessionInsert_)

	````C#
	await App.MobileService.GetTable<Session>().InsertAsync(item);
	````

	> **Speaking Point:** And we'll add the necessary code to retrieve the sessions.

1. Navigate to the **LoadSessions** method and locate _//TODO: Query Sessions for event_. Replace the comment with the following code:

	(Code Snippet - _wamsSessionQuery_)

	````C#
	Sessions = await App.MobileService.GetTable<Session>().Where(e => e.EventId == Event.Id).ToEnumerableAsync();
	````

1. Navigate to the **UpdateSession** method and locate _//TODO: Update Session_. Replace the comment with the following code:

	(Code Snippet - _wamsSessionUpdate_)

	````C#
	await App.MobileService.GetTable<Session>().UpdateAsync(item);
	````


	> **Speaking Point:** So let's run the application again, but this time I'm going to log in. So when I click this button here, that's going to sign me in by Twitter. It's going to execute that line of code we pasted before. I want you to notice that automatically it pops the UI, it logs me into Twitter, and the service is now aware of my identity. I'm logged into my mobile service.

1. Press F5 to run the application from Visual Studio.

1. Right click or swipe up to view the app bar.  Select **Login** from the app bar and select Twitter from the list of login options.

	![eventbuddy-login-options](images/eventbuddy-login-options.png?raw=true)

1. You should see the Twitter login page load in the center of the application.  Enter your Twitter username and password and click **Sign In**.

	![eventbuddy-login-twitter](images/eventbuddy-login-twitter.png?raw=true)

1. Click on the existing Event created in the previous Segment.

1. Right click or use the swipe gesture to expose the bottom AppBar.

	> **Speaking Point:** Let's add a session and call it Mobile Services. I can add any Twitter handle here. I'm going to enter my own, and then I'm going to save the session. But before I do, I want to show you my favorite feature of Mobile Services, which is the ability to execute code securely on the server.

1. Add a session name and speaker name (which should be their twitter handle) in the New Session window.  **DO NOT press save**.

	![eventbuddy-add-session](images/eventbuddy-add-session.png?raw=true)

1. Leave the Event Buddy app running, and switch back to the Microsoft Azure Management Portal.  

	> **Speaking Point:** Let's switch back to the portal. And I'm going to go to the data tab here. If I drill into that session table, you'll see we have this script tab at the top. So I'm going to drill into that, and it allows me to alter JavaScript that runs securely on the server whenever a data operation changes. So you can see I can set a script on insert, update, delete and read.

1. Navigate to the **Data** tab if it's not already selected.  Select the **Session** table and select the **Script** tab. 

	![portal-data-session-script](images/portal-data-session-script.png?raw=true)

	> **Speaking Point:** Let me just copy that to the clipboard and paste this in. Let me explain what this is going to do. So there's a function that's going to be invoked whenever someone performs an insert. And it gets past the item that's being inserted. It also gets past the user, and that object has been verified and authenticated on the server so we can trust the data inside that object.

1. Update the Insert script to the below code. Use the Internet Explorer Favorite labeled **Session** on the Favorites bar to copy the script code to the clipboard. You can then simply paste the script into the Mobile Services Script Editor.  

	> **Speaking Point:** Instead of making you wait while I type in this script, I'll copy the script I wrote earlier to the clipboard and paste it in.

	> This JavaScript will simply go out to Twitter's APIs, retrieve the profile picture for the speaker using the speaker's twitter account, and store the URL of the picture with the session record.   We need to have set up a Twitter app for our Mobile Service in order to make an authenticated call to the Twitter API.  If you didn't set up a Twitter app before for authentication, do so now.  You'll need to replace the twitterConsumerKey and twitterConsumerSecret values with those from the Twitter dev portal.  Additionally, if the user is not signed in with Twitter, you'll need to place your own user token and secret into the script.

	````JavaScript
	function insert(item, user, request) { 
      item.userId = user.userId; 
 
      if (item.speaker) { 
          // get these from https://dev.twitter.com/apps
            var twitterConsumerKey = 'YourAppsConsumerKey';
            var twitterConsumerSecret = 'YourAppsConsumerSecret';
             
            // This works for users signed in with Twitter auth, otherwise
            // you can use your own values for this from https://dev.twitter.com/apps 
            // these are on the same page under "your access token" section
            var identities = user.getIdentities();
            var userToken = identities.twitter.accessToken;
            var userSecret = identities.twitter.accessTokenSecret;
             
            var OAuth = require('OAuth');
            var oauth = new OAuth.OAuth(
                  'https://api.twitter.com/oauth/request_token',
                  'https://api.twitter.com/oauth/access_token',
                  twitterConsumerKey,
                  twitterConsumerSecret,
                 '1.0A',
                  null,
                  'HMAC-SHA1'
                );
            
            oauth.get("https://api.twitter.com/1.1/users/show.json?screen_name=" + item.speaker, userToken, userSecret, 
            function(error, data) {
                //console.log("error: ", error);
                console.log("data: ", data);
                
                if (data) {                    
                    var json = JSON.parse(data);
                    if (json.profile_image_url) {
                        var biggerImg = json.profile_image_url.replace("normal", "bigger");
                        item.img = biggerImg;
                        request.execute();
                    }
                } else {
                    item.img = "Assets/NoProfile.png"; 
                    request.execute(); 
                }
            });                                
      } 
      else { 
             item.img = "Assets/NoProfile.png"; 
             request.execute(); 
      } 
} 
	````

	> **Note:** Take into account that in order to show an Update and retrieve a new Twitter handler picture, you must paste the above script into the **Update** operation, but renaming the function to _update_. This way, whenever you change a Session with a different Twitter handle, the new picture URL will be retrieved and saved.

1. Press the **Save** button to save the changes to the script and wait for the confirmation that the changes have been saved.

	![portal-session-script-save](images/portal-session-script-save.png?raw=true)

	> **Speaking Point:** Just like that in a matter of seconds we've changed the behavior of the mobile services using some simply JavaScript and our changes are now live in Microsoft Azure.    

1. Switch back to the running Event Buddy Windows Store app and now press Save to create the new session. 

	![eventbuddy-session-save](images/eventbuddy-session-save.png?raw=true)

	> **Speaking Point:** Now let's switch back to our Event Buddy Windows Store app and create the new session record.     

	> **Note:** If the script execution was successful, you should see the session added and the Twitter picture of the speaker displayed with the session details. 

	![eventbuddy-session-list](images/eventbuddy-session-list.png?raw=true)

	> **Speaking Point:** We can now see that when our session insert was performed our service side script executed, retrieved the speaker photo from Twitter and updated the speaker profile picture. 	

<a name="Segment4" />
### Segment 4: Using SkyDrive to Store Presentations  ###

> **Speaking Point:** The next thing I want to share with you is how to integrate SkyDrive in this scenario. So I'm going to click on the mobile service, edit the session, and then upload a file. 

1. Select the session that you just created and click **Edit** at the bottom AppBar.

	![editing-session](images/editing-session.png?raw=true)

1. Once in the Edit Session window, click **Upload file**.

	![uploading-file](images/uploading-file.png?raw=true)

	> **Note:** If you are not logged in with a Microsoft account, a dialog will be displayed asking for your credentials to connect to SkyDrive. Insert your credentials and click **Save**. Then you will be prompted with a consent message asking for permission to edit data in your SkyDrive account. Go ahead and accept the consent message in order to choose the file to upload.

	> ![login-microsoft-account](images/login-microsoft-account.png?raw=true)

1. In the file picker select the deck that you want to upload and press **Open**. Then save the session.

	> **Speaking Point:** Let's upload the slide for my presentation later.

	![choosing-file-to-upload](images/choosing-file-to-upload.png?raw=true)

1. Demonstrate how you can now open the slides directly from SkyDrive using Windows Phone.

	> **Note:** Alternatively, if you don't have a Windows Phone 8, you can use the Windows Phone emulator with the Windows Phone 8 solution provided.

	In your Windows Phone 8, browse to the Session you created before in the Windows Store application.
	
	> **Speaking Point:** Here I have a Windows Phone 8, which has the EventBuddy application already installed. Now I'm going to log in and enter the session that I've created earlier. Notice that there is a new document. Let's open it, and we will be able to see the slides I've uploaded previously right on the phone.

	Once in the Session, click **View** to open the slides.

	![viewing-deck-mobile](images/viewing-deck-mobile.png?raw=true)

<a name="Segment5" />
### Segment 5: Push Notifications  ###

> **Speaking Point:** I now want to allow attendees to be able to rate sessions at the conference and once they rate a session I want the speaker to receive a live tile update that shows what that rating was.

1. With the Event Buddy Windows Store app running, switch back to the Microsoft Azure Management portal.  Navigate to the Mobile Service view.  

1. Select the **Push** tab for your mobile service and show the push credentials from the Windows Store.

	> **Speaking Point:** Configuring our Mobile Service to support push notifications is very easy.  After going to the Windows Store to turn on push notifications, we can simply include our secret for connecting to the Windows Notification Service here. 

	![portal-push-configuration](images/portal-push-configuration.png?raw=true)

	> **Speaking Note:** I'm going to add a new table now to store those ratings. And I'm going to make it secure, so let's set this to be authenticated users only. 

1. Select the Data tab and Add table named **Rating** setting permissions to **Only Authenticated Users**.  

	![portal-newtable-rating](images/portal-newtable-rating.png?raw=true)

1. Close the confirmation when the table is created.

1. Update the Insert Script to the below code in order to rate a session. Use the Internet Explorer Favorite labeled **Rating** on the Favorites bar to copy the code to the clipboard. 

	````JavaScript
	function insert(item, user, request) {
		 request.execute({
			  success: function() {
					request.respond();
					sendNotifications(item, user);
			  }
		 });
	}

	function sendNotifications(item, user) {
		 var sql = "SELECT DISTINCT c.uri, s.name FROM Channel c " + 
		 "INNER JOIN Session s ON c.userId = s.userId AND s.id = ?";
		 console.log(sql, item.sessionId);
		 mssql.query(sql, [item.sessionId], {
			  success: function(results) {
					console.log(results);
					if (results.length === 0)
					{
						 return;
					}
					var channels = results.map(function(r) { return r.uri });
					push.wns.sendTileWideSmallImageAndText04(channels, {
						 image1src: item.imageUrl,
						 text1: item.rating + " stars",
						 text2: item.raterName + " just rated your session '" + results[0].name + "'"
					},
					{ success: console.log });
			  }
		 });
	}
	````
	> **Speaking Point:** Once again instead of making you wait while I type in this script, I'll copy the script I wrote earlier to the clipboard and paste it in.  

	> With this script, we are going to execute some code on the service side whenever an Insert occurs.  In this script we have a bit of SQL to look up the Channel URI and name for the current user from our Channel table. This Channel information is used by Windows Notification Services to send a push message to a specific user. Then once we have the Channel information, with one line of code we will call out to the Windows Notification Service and send a push notification to the client application. 

1. Click the **Save** button to save the changes to the server-side script. 

	![portal-rating-script](images/portal-rating-script.png?raw=true)

	> **Speaking Point:** The following steps vary a little depending on whether you are using a Windows Phone 8 device or the Windows Phone emulator as part of Visual Studio.  

1. Press the Windows Key to show the Start screen and make sure that the EventBuddy tile is in clear view without scrolling.

	![start-before-notification](images/start-before-notification.png?raw=true)

1. If you are using the Windows Phone emulator, switch to the instance of Visual Studio that has the EventBuddy.WindowsPhone solution and then press F5 to start the app in the emulator. 	

	> **Note:** You will need to login to Twitter or Facebook on the Windows Phone app. When using the emulator press **Pause Break** to enable keyboard input. This demo segment uses Twitter as the default login provider.

1. If you are using a device to demo Windows Phone, start the Event Buddy Windows Phone 8 app.  

1. In the Event Buddy Windows Phone 8 app, select the event that you created earlier. Then select a session to view the session details.

	![phone-events](images/phone-events.png?raw=true)

1. Rate a session by selecting 1-5 stars. 

	![phone-session-rating](images/phone-session-rating.png?raw=true)

1. If you are using the Windows Phone 8 emulator, press the Windows Key to return to the **Start** screen.  

1. Notice that the Event Buddy tile is updated with the session and rating information.   

	![start-push-notification](images/start-push-notification.png?raw=true)

<a name="Closing" />
### Closing ###

In just a few minutes you've seen how a developer can quickly connect an offline application to the cloud to store data and add support for authentication. You've seen how easily you can connect different devices to the same services and interact with them using push notifications.
