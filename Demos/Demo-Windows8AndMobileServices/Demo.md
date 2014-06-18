<a name="title" />
# Getting Started with Microsoft Azure Mobile Services for Windows Store Apps#

---
<a name="Overview" />
## Overview ##
This demo script demonstrates how you can leverage Visual Studio 2012 and Microsoft Azure Mobile Services to add structured storage, push notifications, integrated authentication and scheduled jobs to your Windows Store application.

> **Note:** This is a demo script that can be used as a guide for demos when presenting on Microsoft Azure Mobile Services.  If you are looking for a Hands on Lab with step by step screenshots please refer to the HOL section of the Microsoft Azure Training Kit.

<a name="technologies" />
### Key Technologies ###

- Microsoft Azure subscription - you can sign up for free trial [here][1]
- Microsoft Azure Mobile Services Preview enabled on your subscription
- Microsoft Azure Mobile Services Client SDK
- Visual Studio 2012 

[1]: http://bit.ly/WindowsAzureFreeTrial

<a name="setup-and-configuration" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment.

1. Download and Install Windows 8

1. Download and Install Visual Studio 2012

1. Download and Install the Microsoft Azure Mobile Services SDK

1. **Code Snippets** have been provided that you can add for use in Visual Studio 2012. You can find them in  /Source/Assets/ClientSnippets

<a name="Demo 1: Creating your first Mobile Service" />
## Demo 1: Creating your first Mobile Service ##

The goal of this demo is to use the quick start within the portal to quickly demonstrate the structured storage capability of Microsoft Azure Mobile services. The Push Notifications demo will demonstrate how you can add code to both the server and client to demonstrate the APIs used by the quick start.

<a name="create-a-new-mobile-service" />
### Create a new mobile service ###
Follow these steps to create a new mobile service.

1. Log into the [Microsoft Azure Management Portal](https://manage.windowsazure.com) and navigate to Mobile Services in the 

1. Click the **+New** button then click **Mobile Service**, **Create** 

1. On the **Create a Mobile Service** dialog provide a unique subdomain in the **URL** field.  Once verification that the subdomain is unique proceed with the next steps.

1. Select either to use an existing database or new database and region. 

	> **Speaking Point:** If you select an existing database Microsoft Azure Mobile Services will separate multiple Mobile Service tenants by schema. 

1. Complete the remainder of the database settings in the Create Mobile Service wizard

You have now created a new mobile service that can be used by your mobile apps.

<a name="create-a-new-app" />
### Create a new app ###
Once you have created your mobile service, you can follow an easy quick start in the Management Portal to either create a new Windows Store app or modify an existing app to connect to your mobile service.

1. After the status updates to **Ready** for your mobile service in the **Mobile Services** tab of the Management portal. Click on your mobile service name to navigate into the service.

1. Click on ![Image 16](Images/image-16.png?raw=true) to see the Mobile Services quick start page.

1. Select **Create a new Windows 8 application** and perform the three steps provided
	- For the **Get the tools** step Install the Mobile Services SDK if you have not done so already.  The Mobile Services SDK provides a client side API that can be used within your Visual C++, XAML/C#/VB and HTML/JS Windows Store apps.
	- Click **Create Todoitem table**.  This will create a table for the starter project.  
	- In the Download and run your app step select your desired app language, in this case **C#**, and click **Download**.

	> **Note:** This downloads the project for the sample _To do list_ application that is connected to your mobile service. Save the compressed project file to your local computer, and make a note of where you save it.

	> **Speaking Point:** You can add additional tables manually by clicking the "Data" tab and using the Create button within.  We will revisit adding tables through the Data tab further on in the Push Notification demo. 

<a name="run-your-app" />
### Run your app ###

The final stage of this tutorial is to run and explore your new Windows Store app.

1. Browse to the location where you saved the compressed project files, expand the files on your computer, and open the solution file in Visual Studio 2012 Express for Windows 8.

1. Press the **F5** key to rebuild the project and start the app.

1. In the app, type meaningful text, such as _Complete the demo_, in the **Insert a TodoItem** textbox, and then click **Save**.

	> **Speaking Point:** Explain this sends a POST request to the new mobile service hosted in Microsoft Azure. Data from the request is inserted into the TodoItem table. Items stored in the table are returned by the mobile service, and the data is displayed in the second column in the app.

1. Back in the Management Portal, click the **Data** tab and then click the **TodoItems** table and observe that the data as been successfully stored

<a name="Explore-your-app-code" />
### Explore your app code ###

In this step we explore _To do list_ application code and see how simple the Microsoft Azure Mobile Services Client SDK makes it to interact with Microsoft Azure Mobile Services.

1. Return to the downloaded To do list application Visual Studio 2012

1. In solution explorer **expand the references folder** and show the Microsoft Azure Mobile Services Client SDK reference.  

	> **Speaking Point:** you may also add references to the Microsoft Azure Mobile Services Client SDK from any Windows Store app.

1. Open App.xaml.cs and show the MobileServiceClient class.  This is the key class provided by the client SDK that provides a way for your application to interact with Microsoft Azure Mobile Services. The first parameter in the constructor is the Mobile Service endpoint and the second parameter is the Application Key for your Mobile Service.

	````C#
	public static MobileServiceClient MobileService 
			= new MobileServiceClient( 
				"https://cloudnick.azure-mobile.net/"
				,"vIWepmcOXGPsYCJQDDcFBKsnOVxzLG52" );
	
	````

1. Open **MainPage.xaml.cs** to see how the mobile service client is then used for Inserts, Updates, Reads and **show** the following:
	* Creating a handle for operations on a table
		<!-- mark:3;-->
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
	- Performing an Update
		<!-- mark:3 -->
		````C#
		private async void UpdateCheckedTodoItem(TodoItem item)
		{
			await todoTable.UpdateAsync(item);
			items.Remove(item);
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

1. Note existence of todoTable.DeleteAsync(...) method

<a name="Demo 2: Adding Push Notifications to your app" />
## Demo 2: Adding Push Notifications to your app ##

In demo, you add push notifications, using the Windows Push Notification service (WNS), to the quickstart project. When complete, an insert in the mobile service will generate a push notification back to your app. 

<a name="Register-your-app-for-push-notifications-and-configure-Mobile-Services" />
### Register your app for push notifications and configure Mobile Services ###

1. Navigate to the [Windows Push Notifications & Live Connect](http://go.microsoft.com/fwlink/?LinkID=257677&clcid=0x409) page, login with your Microsoft account if needed, and then follow the instructions to register your app. 

	> **Speaking Point:** It's important that the CN that you supply in the Wizard matches that in your package.appxmanifest

1. At the end of the registration process for your app you will be provided with WNS Credentials.  Keep the page open or make a note of the **Package Name**, **Client Secret** and **Package SID**.  
	
1. Log on to the [Microsoft Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

1. Click the **Push** tab, enter the **Client secret** and **Package SID** values obtained for WNS above, and click **Save**.

<a name="Add-push-notifications-to-the-app" />
### Add push notifications to the app ###

1. In Visual Studio open the **package.appxmanifest**, select the packaging tab and copy the _Package Name_ from your WNS Credentials you recieved in the Windows Push Notifications & Live Connect portal and paste it into the Package name field in visual studio.

1. In the package.appxmanifest now select the **Application UI** tab and ensure **toast capable** is set to yes.  

	> **Speaking Point:** If you wish to send Wide Tiles then you must provide a default wide tile in the Wide Logo field.

1. Open the file **App.xaml.cs** 

1. Add a **Channel.cs** class as follows.  

	````C#
	public class Channel
	{
		public int? Id { get; set; }
		public string Uri { get; set; }
	}

	````
	> **Note:** You can install and use the code snippet _wamschannelclass_ located in /Source/Assets/ClientSnippets to perform this task

1. add the following using statement:

	````C#
	using Windows.Networking.PushNotifications;
	````

1. Find the OnLaunched method and mark it to be **async** as follows

	````C#
	protected async override void OnLaunched(LaunchActivatedEventArgs args)
	````

1. Add the following lines of code to request a notification channel and register it with your Mobile Services app

	````C#
	var ch = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
	var channelDTO = new Channel()
				  {
						Id = ApplicationData.Current.LocalSettings.Values["ChannelId"] as int?,
						Uri = ch.Uri
				  };

	if (ApplicationData.Current.LocalSettings.Values["ChannelId"] == null)
	{
		 await MobileService.GetTable<Channel>().InsertAsync(channelDTO);
		 ApplicationData.Current.LocalSettings.Values["ChannelId"] = channelDTO.Id;
	}
	else
	{
		 await MobileService.GetTable<Channel>().UpdateAsync(channelDTO);
	}
	````

	> **Note:** You can install and use the code snippet _wamschannelrequest_ located in /Source/Assets/ClientSnippets to perform this task

Now that we have the client wired up to request a channel and write it to our Mobile Service we now need to add a Channel table to our Mobile Service and add a server side script to send push notifications.

<a name="Insert-data-to-receive-notifications" />
### Insert data to receive notifications ###

In this section we add a Channel table and server side scripts to send push notifications everytime someone inserts into our todolist.  

> **Speaking Point:** from a demo perspective and in ensuring it keeps the duration of the presentation within an 1 I would suggest for the demo you keep the server scripts somewhere that you can copy and paste them in, then walk through what they are doing as opposed to typing them out.

1. Log on to the [Microsoft Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

1. Select the **Data** tab

1. Click **+ Create** in the bottom toolbar

1. In **Table name** type _Channel_, then click the check button.

1. Click the new **Channel** table and verify that there are no data rows.

1. Click the **Columns** tab and verify that there is only a single **id** column, which is automatically created for you.
This is the minimum requirement for a table in Mobile Services.

	> **Speaking Point:** When dynamic schema is enabled on your mobile service, new columns are created automatically when JSON objects are sent to the mobile service by an insert or update operation.

1. Select the **TodoItem** table 

1. Click the **Script** tab and select the **Insert** Operation and replace the existing script with the following and walk through the following code

	````JavaScript
	function insert(item, user, request) {		 
		 request.execute({
			  success: function(){
					request.respond();
					sendNotifications(item);
			  },
			  error: function(err){
					request.respond(500, "Error");
			  }
		 });
	}

	function sendNotifications(item){               		 
	  var channelTable = tables.getTable('Channel'); 
	  channelTable.read({ 
		 success: function(channels){
			 channels.forEach(function(channel){  
									
				 push.wns.sendToastText04(channel.Uri, {
					  text1: item.text,
					  text2: "text line 2",
					  text3:  "text line 3"
				 }, {
					  success: function(response){                                               
						  console.log(response);
					  },                                   
					  error: function(err){                                               
							console.error(err);                       
					  }                    
				 });
			});
	  }        
 });    
}

	````
	> **Note:** This script is located in the _/Source/Assets/ServerSnippets_ folder
	

	> **Speaking Point:** This script executes as a each time a the insert operation is executed on the Todoitem table.  The sendNotifications method we select all channels from the Channels table and iterate through them sending a push notification to each channel uri.  While we have only demonstrated toast the push.wns.* namespace provides simple to use methods required for sending toast, tile and badge updates. As you can see in this scenario we are sending a ToastText04 template which requires three lines of text.  When you build your applications we would advise that you do not send toast notifications so frequently but rather only at times when there is a critical or important message to deliver the user of your application.

Next we will move on to look at how you can secure your Mobile Service endpoints using Live Connect

<a name="Demo 3: Adding Auth to Your App and Services" />
## Demo 3: Adding Auth to Your App and Services ##

This demo shows you how to authenticate users in Microsoft Azure Mobile Services from a Windows 8 app. In this demo, you add authentication to the quickstart project using Live Connect. When successfully authenticated by Live Connect, a logged-in the application will be able to consume your Mobile Service.

<a name="Register-your-app" />
### Register your app ###

To be able to authenticate users, you must register your Windows 8 app at the Live Connect Developer Center. You must then register the client secret to integrate Microsoft Account with Mobile Services.

1. Navigate to the [Windows Push Notifications & Live Connect](http://go.microsoft.com/fwlink/?LinkID=257677&clcid=0x409) page, log on with your Microsoft account if needed, and then follow the instructions to register your app.

	> **Note:** In this example we will use Microsoft Account.  If you wish to demonstrat Choose a supported identity provider from the list below and follow the steps to register your app with that provider:
	[Microsoft Account](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-microsoft-authentication/),
	[Facebook login](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-facebook-authentication/), 
	[Twitter login](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-twitter-authentication/),
	[Google login](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-google-authentication/)


1. To enable auth you must now Navigate to the [My Apps dashboard](http://go.microsoft.com/fwlink/?LinkId=262039&clcid=0x409) in Live Connect Developer Center and click on your app in the **My applications** list.

	**Note:**  This step is easier to demo if you leave the Live Connect portal open after doing the Push Notification demo script above

1. Click **Edit settings**, then **API Settings** and make a note of the value of **Client ID** and **Client secret**.
 
	You must provide this value to Mobile Services to be able to use Live Connect for authentication.

1. In **Redirect domain**, enter the domain of your mobile service, in the format **https://****service-name****.azure-mobile.net/**, where _service-name_ is the name of your mobile service, then click **Save**.

1. Log on to the [Microsoft Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

1. Click the **Identity** tab, enter the **Client secret** obtained from Live Connect, and click **Save**.

<a name="Restrict-permissions" />
### Restrict permissions ###

1. In the Management Portal, click the **Data** tab, and then click the **TodoItem** table.

1. Click the **Permissions** tab, set all permissions to **Only authenticated users**, and then click **Save**. This will ensure that all operations against the **TodoItem** table require an authenticated user.

1. Return to Visual Studio 2012 aand press the **F5** key to run this quickstart-based app; verify that an exception with a status code of 401 (Unauthorized) is raised.
This happens because the app is accessing Mobile Services as an unauthenticated user, but the _TodoItem_ table now requires authentication.

Next, you will update the app to authenticate users with a Microsoft Account before requesting resources from the mobile service.

<a name="Add-authentication" />
### Add authentication to your app###

1. Now that your Authentication provider is now configured lets wire up the application. Return to Visual Studio and select **MainPage.xaml.cs**

1. Locate the **OnNavigatedTo** and the existing **OnNavigatedTo** method override with the following method that calls the new **Authenticate** method:

	````C#
	protected override async void OnNavigatedTo(NavigationEventArgs e) 
	{ 
		await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
		RefreshTodoItems();
	}

	````

	> **Note:** Both your mobile service and your app are now configured to work with your chosen authentication provider.

1. Press the F5 key to run the app and sign into the app with your chosen identity provider.

	> **Note:** When you are successfully logged-in, the app will run without errors, and you will be able to query Mobile Services and make updates to data.

<a name="Exercise4" />
## Exercise 4: Adding a Scheduled Job to your Mobile Service ##

In this demo you learn how to execute script on a scheduled basis using **Microsoft Azure Mobile Services**.  In this scenario we will configure the scheduler to poll Twitter every 15 minutes and then send a Tile update with the latest tweets.


### Task 1 - Configure your Windows store app for Wide Tiles ###
1. In Visual Studio Open your **package.appxmanifest**

1. Select the Application UI tab

1. Provide a Wide Tile Logo of 310x150 pixels.  

	> **Note:** Note if you do not have an image of these dimensions available you can use Microsoft Paint to quickly create one


### Task 2 - Configure the Mobile Services scheduler ###

1. Create the scheduler job that will send push notifications to registered clients every 15 minutes with the latest Twitter updates for a particular twitter handle.

1. Specify a name for the job and make sure the schedule frequency is set to **every 15 minutes**. Click the check mark to create the job.

1. Select the created job from the job list.

1. Select the **Script** tab and paste the code snippet below that both polls Twitter and then composes a push notification to update your start screens tile using push.wns.*
 
	````JavaScript
	function CheckFeed() {
		 getUpdatesAndNotify();
	}

	var request = require('request');
	function getUpdatesAndNotify() {  
		 request('http://search.twitter.com/search.json?q=@cloudnick&rpp=2', 
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
																							
						  push.wns.sendTileWideSmallImageAndText04(channel.Uri, {
								image1src: tweet.profile_image_url,                            
								text1: '@' + tweet.from_user,
								text2: tweet.text
						  });                  
																																	 
					 });
				}
			 });
	}
	````
1. Once you paste the script into the editor, click the **Save** button to store the changes to the script
 
1. In Visual Studio, press **F5** to build and run the application.  This will ensure your channel URI is up to date and will ensure the Default Wide tile is now on your Start screen

1. Go back to the Microsoft Azure Management Portal, select the **Scheduler** tab of your mobile service, and then click **Enable** in the command bar to allow the job to run.

1. To test your script immediately rather than wait 15 minutes for it to be scheduled, click **Run Once** in the command bar.

1. Return to the start screen and see the latest update on your application tile
