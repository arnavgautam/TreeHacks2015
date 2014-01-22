<a name="title" />
# Getting Started with Windows Azure Mobile Services for Windows Phone#

---
<a name="Overview" />
## Overview ##
This demo script demonstrates how you can leverage Visual Studio 2012 and Windows Azure Mobile Services to add structured storage, push notifications, integrated authentication and scheduled jobs to your Windows Phone application.

> **Note:** This is a demo script that can be used as a guide for demos when presenting on Windows Azure Mobile Services and Windows Phone.  Given this is a demo script and not a HOL screenshots have been excluded to keep printed format short.  The corresponding PowerPoint presentation can be found here - [Presentation: Developing Windows Phone apps with Windows Azure Mobile Services](https://github.com/WindowsAzure-TrainingKit/Presentation-WindowsPhoneAndWindowsAzureMobileServices)

<a name="technologies" />
### Key Technologies ###

- Windows Azure subscription - you can sign up for free trial [here][1]
- Windows Azure Mobile Services Preview enabled on your subscription
- Windows Azure Mobile Services Client SDK
- Visual Studio 2012 
- [Windows Phone 8 SDK](http://dev.windowsphone.com/en-us/downloadsdk)

[1]: http://bit.ly/WindowsAzureFreeTrial

<a name="setup-and-configuration" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment.

1. Download and Install Windows 8

1. Download and Install Visual Studio 2012

1. Download and Install the Windows Phone 8 SDK

1. Download and Install the Windows Azure Mobile Services SDK

1. **Code Snippets** have been provided that you can add for use in Visual Studio 2012. You can find them in  /Source/Assets/ClientSnippets

<a name="Demo 1: Structured Storage - Creating your first Mobile Service" />
## Demo 1: Structured Storage - Creating your first Mobile Service ##

The goal of this demo is to use the quick start within the portal to quickly demonstrate the structured storage capability of Windows Azure Mobile services. The Push Notifications demo will demonstrate how you can add code to both the server and client to demonstrate the APIs used by the quick start.

<a name="create-a-new-mobile-service" />
### Create a new mobile service ###
Follow these steps to create a new mobile service.

1. Log into the [Windows Azure Management Portal](https://manage.windowsazure.com) and navigate to Mobile Services in the 

1. Click the **+New** button then click **Mobile Service**, **Create** 

1. On the **Create a Mobile Service** dialog provide a unique subdomain in the **URL** field.  Once verification that the subdomain is unique proceed with the next steps.

1. Select either to use an existing database or new database and region. **Speaking Point:** If you select an existing database Windows Azure Mobile Services will separate multiple Mobile Service tenants by schema. 

1. Complete the remainder of the database settings in the Create Mobile Service wizard

You have now created a new mobile service that can be used by your mobile apps.

<a name="create-a-new-app" />
### Create a new app ###
Once you have created your mobile service, you can follow an easy quick start in the Management Portal to either create a new Windows Phone app or modify an existing app to connect to your mobile service.

1. After the status updates to **Ready** for your mobile service in the **Mobile Services** tab of the Management portal. Click on your mobile service name to navigate into the service.

1. Click on ![Image 16](Images/image-16.png?raw=true) to see the Mobile Services quick start page.

1. Select **Create a new Windows Phone 8 application** and perform the three steps provided
	- For the **Get the tools** step Install the Mobile Services SDK if you have not done so already.  The Mobile Services SDK provides a client side API that can be used within your Visual C++, XAML/C#/VB and HTML/JS Windows Phone apps.
	- Click **Create Todoitem table**.  This will create a table for the starter project.  

	- In the Download and run your app step select your desired app language, in this case **C#**, and click **Download**.

This downloads the project for the sample _To do list_ application that is connected to your mobile service. Save the compressed project file to your local computer, and make a note of where you save it.

> **Speaking Point:** You can add additional tables manually by clicking the "Data" tab and using the Create button within.  We will revisit adding tables through the Data tab further on in the Push Notification demo. 

<a name="run-your-app" />
### Run your app ###

The final stage of this tutorial is to run and explore your new Windows Phone app.

1. Browse to the location where you saved the compressed project files, expand the files on your computer, and open the solution file in Visual Studio 2012.

1. Press the **F5** key to rebuild the project and start the app.

1. In the app, type meaningful text, such as _Complete the demo_, in the **Insert a TodoItem** textbox, and then click **Save**.

	**Speaking Point:** Explain this sends a POST request to the new mobile service hosted in Windows Azure. Data from the request is inserted into the TodoItem table. Items stored in the table are returned and are subsequently populated in the list within the app.  Pressing referesh causes a query against the mobile service filtering back incomplete items. Checking the checkbox causes an update against the Mobile Service

1. Back in the Management Portal, click the **Data** tab and then click the **TodoItems** table and observe that the data as been successfully stored

> **Note:** an effective way to do this portion of the demo is to have the WP emulator docked to the left side of the screen and the TodoItem Data tab docked in the right side of the screen. Insert the item then hit the refresh button in the toolbar at the bottom of the portal to see the new+updated data.


<a name="Explore-your-app-code" />
### Explore your app code ###

In this step we explore _To do list_ application code and see how simple the Windows Azure Mobile Services Client SDK makes it to interact with Windows Azure Mobile Services.

1. Return to the downloaded To do list application Visual Studio 2012

1. In solution explorer **expand the references folder** and show the Windows Azure Mobile Services Client SDK reference.  **Note:** While the dowloaded app already includes the reference you should show where to find the Windows Azure Mobile Services Client SDK referece if they already have an existing Windows Phone app under development.

1. Open App.xaml.cs and show the MobileServiceClient class.  This is the key class provided by the client SDK that provides a way for your application to interact with Windows Azure Mobile Services. The first parameter in the constructor is the Mobile Service endpoint and the second parameter is the Application Key for your Mobile Service.

	````C#
	public static MobileServiceClient MobileService 
			= new MobileServiceClient( 
				"https://cloudnick.azure-mobile.net/"
				,"vIWepmcOXGPsYCJQDDcFBKsnOVxzLG52" );
	
	````

1. Open **MainPage.xaml.cs** to see how the mobile service client is then used for Inserts, Updates, Reads and **show** the following:

   - Show the TodoItem entity and explain how the DataMember attribute can be used to provide different names to be used within the mobile service and for persisting the data to the underlying Windows Azure SQL Database.  Note that you can also manipulate the Table that the entity is serialized to using DataTable attribute.  This is useful for some environments where the DBAs may require a specific table naming convention e.g tblTodoItem etc.
		
		````C#
		 public class TodoItem
		 {
			 public int Id { get; set; }

			 [DataMember(Name = "text")]
			 public string Text { get; set; }

			 [DataMember(Name = "complete")]
			 public bool Complete { get; set; }
		 }
		````


	- Creating a handle for operations on a table

		````C#
		private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();
		
		````
	- Performing an Insert
		<!-- mark:3;-->
		````C#
		private async void InsertTodoItem(TodoItem todoItem)
		{
			await todoTable.InsertAsync(todoItem);
			items.Add(todoItem);                        
		}

		````
	- Performing a Read
		<!-- mark:3-4 -->
		````C#
		private void RefreshTodoItems()
		{
			items = todoTable
				 .Where(todoItem => todoItem.Complete == false)
				 .ToCollectionView();
			ListItems.ItemsSource = items;
		}
		````

	- Performing an Update
		<!-- mark:3 -->
		````C#
		private async void UpdateCheckedTodoItem(TodoItem item)
		{
			await todoTable.UpdateAsync(item);
			items.Remove(item);
		}
		````

	- Note existence of todoTable.DeleteAsync(...) method

<a name="Demo 2: Server Scripts" />
## Demo 2: Server Scripts and Structured Storage ##
> **Speaking Note:** In this demo we will demonstrate how you can inject your own business logic within the Mobile Services CRUD operation pipeline using Server Scripts.  This is useful for scenarios like validation on the server side and implementing custom server side workflows.

1. In the portal Select the Select the **TodoItem Table**

1. Select the Scripts tab

1. Show that there are dropdowns for Insert/Read/Update/Delete

1. Select the **Insert** Dropdown and explain the item, user and request params

1. Add the following code to the Insert operation to demonstrate how one would add some simple validation within a Server Script.

	````JavaScript
	function insert(item, user, request) {
    	if(item.text.length < 5){
		request.respond(statusCodes.BAD_REQUEST, 'text is too short');
    	} else {
		request.execute();
    	}
	}
	````
	> **Note:** The above code snippet is also available in /Source/Assets/ServerSnippets/InsertScript.TodoitemTable.ValidateDemo.txt  

<a name="Demo 3: Adding Push Notifications to your app" />
## Demo 3: Adding Push Notifications to your app ##

In demo, you add push notifications, using the Microsoft Push Notification service (MPNS), to the quickstart project. When complete, an insert in the mobile service will generate a push notification back to your app. In this case we are choosing to use a Tile Notification as we can receive this while the app is running to generate the notification.  Later we will look at Toast Notifications with scheduled scripts.

<a name="Add-push-notifications-to-the-app" />
### Add push notifications to the app ###

1. In Visual Studio open the **Properties/WMAppManifest.xml** file, select the Capabilities tab and Check ID_CAP_PUSH_NOTIFICATION.

1. In the package.appxmanifest now select the **Application UI** tab and ensure **toast capable** is set to yes.  **Speaking Point:** If you wish to send Wide Tiles then you must provide a default wide tile in the Wide Logo field.

1. Open the file **App.xaml.cs** 

1. Replace the _Application_Launching_ and _Application_Activated methods with the following code snippet.  

> **Note:** This is a rather large code snippet.  Please ensure you walk though the code with the attendees.  The key points to note are that you Request a Channel with the HttpChannelNotification Class and then Register it with Mobile Services


````C#
private void Application_Launching(object sender, LaunchingEventArgs e)
{
	RequestAndRegister();
}

private void Application_Activated(object sender, ActivatedEventArgs e)
{
	RequestAndRegister();
}

private async void RequestAndRegister()
{
	var channel = RequestChannel();
	await RegisterChannel(channel.ChannelUri); 
}
	
private HttpNotificationChannel RequestChannel()
{
	string channelName = "MyPushChannel";
	
	var channel = HttpNotificationChannel.Find(channelName);

	if (channel == null)
	{
		channel = new HttpNotificationChannel(channelName);

		//register for events
		channel.ChannelUriUpdated += channel_ChannelUriUpdated;
		channel.ErrorOccurred += channel_ErrorOccurred;
		channel.HttpNotificationReceived += channel_HttpNotificationReceived;
		channel.ShellToastNotificationReceived += channel_ShellToastNotificationReceived;                
		channel.Open();         
	}
	else
	{
		channel.ChannelUriUpdated += channel_ChannelUriUpdated;
		channel.ErrorOccurred += channel_ErrorOccurred;
		channel.HttpNotificationReceived += channel_HttpNotificationReceived;
		channel.ShellToastNotificationReceived += channel_ShellToastNotificationReceived;
	}

	if(!channel.IsShellTileBound)
		channel.BindToShellTile(); //TODO list of channel Uri

	if(!channel.IsShellToastBound)
		channel.BindToShellToast();

	return channel;
}      

private async Task RegisterChannel(Uri channelUri)
{
	if (channelUri != null)
	{
		var channel = new Channel
		{
			Id = IsolatedStorageSettings.ApplicationSettings.Contains("ChannelId")
									? IsolatedStorageSettings.ApplicationSettings["ChannelId"] as int?
									: null,
			Uri = channelUri.ToString()
		};

		//if first time registering channel
		if (!channel.Id.HasValue)
		{
			await App.MobileService.GetTable<Channel>().InsertAsync(channel);
			IsolatedStorageSettings.ApplicationSettings["ChannelId"] = channel.Id;
		}
		else
		{
			await App.MobileService.GetTable<Channel>().UpdateAsync(channel);
		}
	}
}



private class Channel
{
	public int? Id { get; set; }
	public string Uri { get; set; }
}

void channel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
{

}

void channel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
{
		
}

async void channel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
{
	await RegisterChannel(e.ChannelUri);
}

void channel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
{

}

````
> **Note:** You can install and use the code snippet _wpchannelrequest_ located in /Source/Assets/ClientSnippets to perform this task.  Note you may also remove the events for HttpNotificationReceived and ShellToastNotificationReceived if you do not want to handler Raw and Toast notifications respectively when the application is in the foreground
 
Now that we have the client wired up to request a channel and write it to our Mobile Service we now need to add a Channel table to our Mobile Service and add a server side script to send push notifications.

1. Now pin the tile of the application to the start screen on your Phone such that we can see when a notification is delivered.

<a name="Insert-data-to-receive-notifications" />
### Insert data to receive notifications ###

In this section we add a Channel table and server side scripts to send push notifications everytime someone inserts into our TodoItem table.  **Note:** from a demo perspective and in ensuring it keeps the duration of the presentation within an 1 I would suggest for the demo you keep the server scripts somewhere that you can copy and paste them in, then walk through what they are doing as opposed to typing them out.

1. Log on to the [Windows Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

1. Select the **Data** tab

1. Click **+ Create** in the bottom toolbar

1. In **Table name** type _Channel_, then click the check button.

1. Click the new **Channel** table and verify that there are no data rows.

1. Click the **Columns** tab and verify that there is only a single **id** column, which is automatically created for you.
This is the minimum requirement for a table in Mobile Services.

	**Speaking Point:** When dynamic schema is enabled on your mobile service, new columns are created automatically when JSON objects are sent to the mobile service by an insert or update operation.

1. Select the **TodoItem** table 

1. Click the **Script** tab and select the **Insert** Operation and replace the existing script with the following and walk through the following code

	````JavaScript
	function insert(item, user, request) {
	request.execute({
		success: function(){
		request.respond();
		sendNotifications(item);
		}
	});
	}
	
	function sendNotifications(item){               
		var channelTable = tables.getTable('Channel'); 
			channelTable.read({ 
					success: function(channels){
						channels.forEach(function(channel){
							
							push.mpns.sendFlipTile(channel.Uri,{
									title: item.text
							});
							
						});
					}        
		});    
	}

	````
**Note:** This script is located in the file _/Source/Assets/ServerSnippets/InsertScript.TodoitemTable.PushDemo.txt_ folder

**Speaking Point:** This script executes as a each time a the insert operation is performed on the Todoitem table.  The sendNotifications method we select all channels from the Channels table and iterate through them sending a push notification to each channel uri.  While we have only demonstrated a basic example of the sendFlipTile method in the push.mpns.* there are a number of other overloads and parameters you can use to send tile, toast and raw notifications.  You can find all supported push.mpns.* functions and parameters [here ](http://msdn.microsoft.com/en-us/library/windowsazure/jj871025.aspx)

Next we will move on to look at how you can secure your Mobile Service endpoints using Twitter

<a name="Demo 4: Adding Auth to Your App and Services" />
## Demo 4: Adding Auth to Your App and Services ##

This demo shows you how to authenticate users in Windows Azure Mobile Services from a Windows Phone 8 app. In this demo, you add authentication to the quickstart project using Twitter and lock down the service such that only authenticated clients can Insert/Update/Delete/Read from your TodoItem table. When successfully logged in using Twitter the authenticated users will be able to consume your Mobile Service.

<a name="Register-your-app" />
### Register your app ###

To be able to authenticate users using Twitter, you must register your Mobile Service within the Twitter developer portal. Following this you must then register the twitter secrets with your Mobile Service.

1. Navigate to the [Twitter developer portal](https://dev.twitter.com/) and log on with your Twitter Account if needed, and then follow the instructions to register your app.

> **Note:** In this example we will use Twitter.  If you wish to demonstrate another then choose supported identity provider from the list below and follow the steps to register your app with that provider:
[Microsoft Account](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-microsoft-authentication/),
[Facebook login](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-facebook-authentication/), 
[Twitter login](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-twitter-authentication/),
[Google login](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-google-authentication/)

1. Once logged in to the [Twitter developer portal](https://dev.twitter.com/)  **click your username** in the top right and select **My Applications**

1. Press **Create a new Application**

1. Provide an arbitrary Name, Description and Website

1. For the Callback Url field enter the domain of your mobile service, in the format **https://****service-name****.azure-mobile.net/**, where _service-name_ is the name of your mobile service, then click **Save**.

1. Press **Create your Twitter Application**.  On success this will provide you with a Consumer Key and Consumer Secret.

> **Note:** I generally dock the Twitter browser Left and dock the Windows Azure Portal Right for the next two steps

1. Select the **Identity Tab** in the Windows Azure Portal for your Mobile Service

1. Copy the Twitter Consumer Key and Consumer Secret to the correspond Mobile Service Portal Twitter settings fields and click **save**

<a name="Restrict-permissions" />
### Restrict Table permissions ###

1. In the Management Portal, click the **Data** tab, and then click the **TodoItem** table.

1. Click the **Permissions** tab, set all permissions to **Only authenticated users**, and then click **Save**. This will ensure that all operations against the **TodoItem** table require an authenticated user.

1. Return to Visual Studio 2012 and press the **F5** key to run the app again; verify that now an exception with a status code of 401 (Unauthorized) is raised.
This happens because the app is accessing Mobile Services as an unauthenticated user, but the _TodoItem_ table now requires authentication.

Next, you will update the app to authenticate users with a Microsoft Account before requesting resources from the mobile service.

<a name="Add-authentication" />
### Add authentication to your app###

1. Now that your Authentication provider is now configured lets wire up the application. Return to Visual Studio and select **MainPage.xaml.cs**

1. In the constructor for MainPage.xaml.cs add an event handler for the Page Loaded event
<!-- mark:4 -->
````C#
  public MainPage()
  {
		InitializeComponent();
		this.Loaded += MainPage_Loaded;
  }
````


1. Remove **OnNavigatedTo** and replace the MainPage_Loaded handler with the following:

	````C#
	async void MainPage_Loaded(object sender, RoutedEventArgs e)
	{
		if (App.MobileService.CurrentUser == null)
		{
			 await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
		}

		RefreshTodoItems();
	}

	````
> **Note:** You can install and use the code snippet _wpauthenticate_ located in /Source/Assets/ClientSnippets to perform this task. 

1. Press the F5 key to run the app and sign into the app with your chosen identity provider.  **Note:** that you should probably move the emulator out of view for the password entry.

> **Note:** When you are successfully logged-in, the app will run without errors, and you will be able to query Mobile Services and make updates to data now that the user is authenticated.

<a name="Exercise5" />
## Exercise 5: Adding a Scheduled Job to your Mobile Service ##

In this demo you learn how to execute script on a scheduled basis using **Windows Azure Mobile Services**.  In this scenario we will configure the scheduler to poll Twitter every 15 minutes and then send a Toast notification with the latest tweet directed at a given alias.


### Task 1 - Close the Windows Phone app ###
1.  Close the Windows Phone application so that you can see the toast arrive

### Task 2 - Configure the Mobile Services scheduler ###

1. Create the scheduler job that will send push notifications to registered clients every 15 minutes with the latest Twitter updates for a particular twitter handle.

1. Specify a name for the job and make sure the schedule frequency is set to **every 15 minutes**. Click the check mark to create the job.

1. Select the created job from the job list.

1. Select the **Script** tab and paste the code snippet below that both polls Twitter and then composes a push notification to update your start screens tile using push.mpns.*
 
	````JavaScript
	function checkTweets() {
		  getUpdatesAndNotify();
	}

	var request = require('request');
	function getUpdatesAndNotify() {  
		  request('http://search.twitter.com/search.json?q=@cloudnick&rpp=1', 
			function tweetsLoaded (error, response, body) {
				 var results = JSON.parse(body).results;

				 if(results){
						 results.forEach(function visitResult(tweet){
							  sendNotifications(tweet);
						 });
				 }             
			});
	}

	function sendNotifications(tweet){    
	var channelTable = tables.getTable('Channel');

	channelTable.read({
		 success: function(channels) {
				channels.forEach(function(channel) { 
										
					  push.mpns.sendToast(channel.Uri, {                  
							text1: '@' + tweet.from_user,
							text2: tweet.text                                    
					  });                                    
				});
		 }
	 });
	}
	````

**Note:** This script is located in the file _/Source/Assets/ServerSnippets/Scheduler.CheckTweets.txt_ folder

1. Once you paste the script into the editor, click the **Save** button to store the changes to the script

> **Note:** Note for next few steps of the demo I like to have both the Emulator (docked left) and Portal (docked right) visible

1. In the **Scheduler** tab of your mobile service  click **Enable** in the command bar to allow the job to run every 15 minutes

1. To test your script immediately rather than wait 15 minutes for its first scheduled run, click the **Run Once** button in the command bar.

1. In the emulator you should see a toast delivered. 
**Note:** an interesting alternative to this demo would be to send a push.mpns.sendFlipTile with the users twitter profile which is also returned in the tweet object.
