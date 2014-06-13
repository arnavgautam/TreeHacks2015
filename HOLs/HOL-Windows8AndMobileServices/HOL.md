<a name="title" />
# Introduction to Building Windows Store Apps with Microsoft Azure Mobile Services #

---
<a name="Overview" />
## Overview ##
In this HOL you will learn how you can leverage Visual Studio 2012 and Microsoft Azure Mobile Services to add structured storage, push notifications and integrated authentication to your Windows Store applications.

<a name="Objectives" />
### Objectives ###
- Create a Microsoft Azure Mobile Service.
- Use the Microsoft Azure Mobile Services SDK.
- Learn how to Insert, Update, Read and Delete rows from a Mobile Service.
- Add Push Notifications to your application.
- Lock down your Mobile Service such that only authenticated users can consume it.
- Add a Scheduled Job to poll the Twitter API and send Tile updates.

<a name="technologies" />
### Prerequisites ###

- Microsoft Azure subscription with Mobile Services preview enabled - [Create a Microsoft Azure account and enable preview features][1]
- [Visual Studio Express 2012 for Windows 8](http://www.microsoft.com/visualstudio) or higher

[1]: http://www.windowsazure.com/en-us/develop/mobile/tutorials/create-a-windows-azure-account/

<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating your first Mobile Service](#Exercise1)
1. [Adding Push Notifications to your app](#Exercise2)
1. [Adding Auth to Your App and Services](#Exercise3)
1. [Adding a Scheduled Job to your Mobile Service](#Exercise4)

<a name="Exercise1" />
## Exercise 1: Creating your first Mobile Service ##

This exercise shows you how to add a cloud-based backend service to a Windows 8 app using Microsoft Azure Mobile Services.  You will create both a new mobile service and a simple _To do list_ app that stores app data in the new mobile service.

A screenshot from the completed app is below:

![Image 1](Images/image-1.png?raw=true)

<a name="create-a-new-mobile-service" />
### Task 1 - Creating a new mobile service ###
Follow these steps to create a new mobile service.

1. Log into the [Microsoft Azure Management Portal](https://manage.windowsazure.com) and navigate to Mobile Services.

1. Click the **+New** button.

	![image-2](Images/image-2.png?raw=true)

1. Expand **Compute | Mobile Service**, then click **Create**.
 
	![Image 3](Images/image-3.png?raw=true)
 
	This displays the **New Mobile Service** dialog.

1. In the **Create a mobile service** page, type a subdomain name for the new mobile service in the **URL** textbox and wait for name verification. Once name verification completes, click the right arrow button to go to the next page.
 
	![Image 4](Images/image-4.png?raw=true)

	This displays the **Specify database settings** page.

	> **Note:** As part of this exercise, you create a new SQL Database instance and server. You can reuse this new database and administer it as you would do with any other SQL Database instance. If you already have a database in the same region as the new mobile service, you can instead chooseUse existing Databaseand then select that database. The use of a database in a different region is not recommended because of additional bandwidth costs and higher latencies.

1. In **Name**, type the name of the new database, then type **Login name**, which is the administrator login name for the new SQL Database server, type and confirm the password, and click the check button to complete the process.
 
	![Image 5](Images/image-5.png?raw=true)

	> **Note:**  When the password that you supply does not meet the minimum requirements or when there is a mismatch, a warning is displayed.  We recommend that you make a note of the administrator login name and password that you specify; you will need this information to reuse the SQL Database instance or the server in the future. You have now created a new mobile service that can be used by your mobile apps.

You have now created a new mobile service that can be used by your mobile apps.

<a name="create-a-new-app" />
### Task 2 - Creating a new app ###
Once you have created your mobile service, you can follow an easy quick start in the Management Portal to either create a new Windows Store app or modify an existing app to connect to your mobile service.


1. In the Management Portal, click **Mobile Services**, and then click the mobile service that you just created.

1. In the quickstart tab, make sure that **Windows Store** is selected as the **Platform**, and expand **Create a new Windows Store app**.

	![Image 6](Images/image-6.png?raw=true)

	This displays the three easy steps to create a Windows 8 app connected to your mobile service.

	![Image 7](Images/image-7.png?raw=true)

1. If you haven't already done so, download and install [Visual Studio 2012 Express for Windows 8](http://go.microsoft.com/fwlink/?LinkId=257546&clcid=0x409) on your local computer or virtual machine.

1. Click **Create TodoItem Table** to create the table in the database.

1. Lastly, make sure that the **C#** language is selected and click **Download**.

	This downloads the project for the sample _Todo list_ application that is connected to your mobile service. Save the compressed project file to your local computer, and make a note of where you save it.

<a name="run-your-app" />
### Task 3 - Running your app ###

1. Browse to the location where you saved the compressed project files, extract the files on your computer, and open the solution file in Visual Studio 2012 Express for Windows 8.

	![Image 23](Images/image-23.png?raw=true)

1. Expand the **References** node in the project and notice that many references are missing. These references will be downloaded as nuget packages when the app is compiled the first time. The _Microsoft Azure Mobile SDK_ is now downloaded as a nuget package.

1. Press the **F5** key to build the project, download the required dependencies, and start the app.

1. In the app, type meaningful text, such as _Complete the lab_, in the **Insert a TodoItem** textbox, and then click **Save**.

	![Image 9](Images/image-9.png?raw=true)

	This sends a POST request to the new mobile service hosted in Microsoft Azure. Data from the request is inserted into the TodoItem table. Items stored in the table are returned by the mobile service, and the data is displayed in the second column in the app.

	> **Note:** You can review the code that accesses your mobile service to query and insert data, which is found in the MainPage.xaml.cs file.

1. Back in the Management Portal, click the **Data** tab and then click the **TodoItems** table and observe that the data has been successfully stored.

	![Image 10](Images/image-10.png?raw=true)

	This lets you browse the data inserted by the app into the table.

	![Image 11](Images/image-11.png?raw=true)

<a name="Explore-your-app-code" />
### Task 4 - Exploring your app code ###

In this step we explore _Todo list_ application code and see how simple the Microsoft Azure Mobile Services Client SDK makes it to interact with Microsoft Azure Mobile Services.

1. Return to the downloaded _Todo list_ application in Visual Studio 2012 Express for Windows 8.

1. In solution explorer expand the **References** folder and notice **Microsoft.WindowsAzure.Mobile**, which is the reference for the _Microsoft Azure mobile Services Client SDK_. 

1. Open **App.xaml.cs** and show the _MobileServiceClient_ class.  This is the key class provided by the Mobile Services client SDK that provides a way for your application to interact with Microsoft Azure Mobile Services. The first parameter in the constructor is the Mobile Service endpoint and the second parameter is the Application Key for your Mobile Service.

	````C#
	public static MobileServiceClient MobileService 
			= new MobileServiceClient( 
				"https://todolist.azure-mobile.net/"
				,"vIWepmcOXGPsYCJQDDcFBKsnOVxzLG52" );
	
	````

1. Open **MainPage.xaml.cs** to observe how the mobile service client is then used for Inserts, Updates, Reads and Deletes:

	The source creates a handle for operations on a table:

	````C#
	private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();		
	````
	Performs an Insert:

	<!-- mark:3;-->
	````C#
	private async void InsertTodoItem(TodoItem todoItem)
	{
		await todoTable.InsertAsync(todoItem);
		items.Add(todoItem);                        
	}
	````

	Performs an Update:

	<!-- mark:3 -->
	````C#
	private async void UpdateCheckedTodoItem(TodoItem item)
	{
		await todoTable.UpdateAsync(item);
		items.Remove(item);
	}
	````

	Performs a Read:

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

1. As an extension see if you can update the _UpdateCheckedTodoItem_ method to perform a delete rather then update operation using the todoTable.DeleteAsync(...) method.

<a name="Exercise2" />
## Exercise 2: Adding Push Notifications to your app ##

In this exercise, you will add push notifications, using the Windows Push Notification service (WNS), to the project. When completed, an insert in the mobile service todolist table will generate a push notification back to your app. 

<a name="Registering-your-app-for-push-notifications-and-configure-Mobile-Services" />
### Task 1 - Registering your app for push notifications and configure Mobile Services ###

1.	Click **Store** in the Visual Studio menu and select **Reserve App Name**. Additionally, you can find the **Reserve App Name** option under the **Project | Store** menu in some Visual Studio versions. 

	![Reserving App Name](./Images/reserving-app-name.png?raw=true)

1.	The browser will display the Windows Store page that you will use to obtain your WNS credentials. In the Submit an app section, click **App Name**.

	> **Note:** You will have to sign in using your Microsoft Account to access the Windows Store.

	![Giving your app a unique name](./Images/giving-app-name-windows-store.png?raw=true)

1.	In the App name field, insert the Package Display Name that is inside the **package.appxmanifest** file of your solution and click **Reserve app name**. Then click **Save** to confirm the reservation.

	![Reserving an app name](./Images/app-name-windows-store.png?raw=true)

	![Confirming the app name reservation](./Images/name-reservation-successful-win-store.png?raw=true)

1. Now you will have to identify your application to get a name and a publisher to insert in the **package.appxmanifest** file. In the Submit an app page, click **Advanced features**.

	![Configuring push notifications for the Notifications.Client app](./Images/app-name-reverved-completely-windows-store.png?raw=true)

1. In the Advanced features page, click **Push notifications and Live Connect services info**.

	![Advanced features page](./Images/push-notif-live-connect-service-info.png?raw=true)

1. Once in the Push notifications and Live Connect services info section, click **Identifying your app**.

	![Push notifications Overview page](./Images/identifying-your-app.png?raw=true)

1. Now we have to set the Identity Name and Publisher of our **package.appxmanifest** file with the information in Windows Store. Go back to Visual Studio, right-click the **package.appxmanifest** and select **View Code**. Replace the Name and Publisher attributes of the Identity element with the ones obtained in Windows Store. Click **Authenticating your service**.

	![Setting Identity Name and Publisher](./Images/app-identification.png?raw=true)

1. Finally you obtained a **Package Security Identifier (SID)** and a **Client secret**, which are the WNS Credentials that we need to update the Web configuration of our Notification App Server.

	![Package Security Identifier (SID) and Client secret](./Images/sid-client-secret.png?raw=true)

	> **Note:** The client secret and package SID are important security credentials. Do not share these secrets with anyone or distribute them with your app.
	
1. Log on to the [Microsoft Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

	![Image 13](Images/image-13.png?raw=true)

1. Click the **Push** tab, enter the **Client secret** and **Package SID** values obtained for WNS above, and click **Save**.

	![Image 14](Images/image-14.png?raw=true)

	> **Note:** In the following steps you will associate your application with the Windows Store. If you obtained your WNS credentials from the Windows Push Notifications & Live Connect Portal, there is no need to execute these steps.

1.	Click **Store** or **Project | Store** in the Visual Studio menu and select **Associate App with the Store**.

	![Associating App with Store](./Images/associating-app-with-store.png?raw=true)

1. In the Associate Your App with the Windows Store wizard, click **Sign In**.

	![Associating App with Store Wizard](./Images/associate-app-with-store.png?raw=true)

1. Enter your credentials and click **Sign In**.

	![Inserting your credentials to assciate your app in Windows Store](./Images/sign-in-for-association.png?raw=true)

1. In the Select an app name step, select **Notifications.Client** and click **Next**.

	![Selecting your app name](./Images/selecting-app-name.png?raw=true)

1. Take a look at the summary of the values that will be added in the manifest file. Click **Associate**. 

	![Associating your app with the Windows Store Summary](./Images/association-summary.png?raw=true)

1.	**Close** and **Save** changes to **package.appxmanifest**.

<a name="Adding-push-notifications-to-the-app" />
### Task 2 - Adding push notifications to the app ###

1. In Visual Studio open the **package.appxmanifest**, select the **Application UI** tab and ensure **toast capable** is set to _yes_.  

	> **Note:** If you wish to send Wide Tiles then you must provide a default wide tile in the Wide Logo field.

1. Right-click the TodoList project, select **Add | Class** and name it **Channel.cs**. Then insert the following properties into it:  

	````C#
	public class Channel
	{
		public int? Id { get; set; }
		public string Uri { get; set; }
	}

	````
1. Open the file **App.xaml.cs**.

1. Add the following using statements:

	````C#
	using Windows.Networking.PushNotifications;
	using Windows.Storage;
	````

1. Find the OnLaunched method and mark it to be **async** as follows.

	````C#
	protected async override void OnLaunched(LaunchActivatedEventArgs args)
	````

1. Add the following lines of code at the end of OnLaunched to request a notification channel and register it with your Mobile Services app.

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

Now that you have the client wired up to request a channel and write it to the Mobile Service you need to add a Channel table to the Mobile Service and add a server side script to send push notifications.

<a name="Inserting-data-to-receive-notifications" />
### Task 3 - Inserting data to receive notifications ###

In this task you will add a Channel table and server side scripts to send push notifications everytime someone inserts into our todolist.  

1. Return to the [Microsoft Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

1. Select the **Data** tab.

1. Click **Create** in the bottom toolbar.

	![Image 19](Images/image-19.png?raw=true)

1. In **Table name** type _Channel_, then click the check button.
		
	![Image 20](Images/image-20.png?raw=true)

1. Click the new **Channel** table and verify that there are no data rows.

1. Click the **Columns** tab and verify that there is only a single **id** column, which is automatically created for you.
This is the minimum requirement for a table in Mobile Services.

	> **Note:** When dynamic schema is enabled on your mobile service, new columns are created automatically when JSON objects are sent to the mobile service by an insert or update operation.

1. Now in the left navbar select the **TodoItem** table.

1. Click the **Script** tab and select the **Insert** Operation and replace the existing script with the following and walk through the following code.


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
					  text2: "Hello World 1",
					  text3:  "Hello World 2"
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
	> **Note:** This script executes each time an insert operation is executed on the Todoitem table.  The sendNotifications method will select all channels from the Channels table and iterate through them sending a push notification to each channel URI. Although we have only demonstrated a single toast template, the push.wns.* namespace provides simple-to-use methods required for sending toast, tile and badge updates. As you can see in this scenario we are sending a ToastText04 template that requires three lines of text. When you build your applications we would advise that you do not send toast notifications so frequently but rather only at times when there is a critical or important message to deliver to the user of your application.

	![Image 22](Images/image-22.png?raw=true)

1. In **Visual Studio** press **F5** to run the app.

1. Enter a _Todo item_ and click **Save**. The toast notification will be displayed.

	![Testing Push notifications](Images/testing-push-notifications.png?raw=true)


Next we will move on to look at how you can secure your Mobile Service endpoints.

<a name="Exercise3" />
## Exercise 3: Adding Auth to Your App and Services ##

This exercise shows how to authenticate users in Microsoft Azure Mobile Services from a Windows 8 app. In this exercise, you add authentication to the quickstart project using Microsoft Account. When successfully authenticated by a Microsoft Account your app will be able to consume your Mobile Service.

<a name="Registering-your-app" />
### Task 1 - Registering your app ###

To be able to authenticate users, you must register your Windows Store app within an Identity Provider. You must then register the obtained client secret to integrate the provider with Mobile Services.

The supported identity providers are listed below. In this exercise you will use **Microsoft Account** as the provider, nevertheless you can use the one of your preferences and you can follow the steps to register your app with that provider:

[Microsoft Account] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-microsoft-authentication/)

[Facebook login] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-facebook-authentication/)

[Twitter login] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-twitter-authentication/)

[Google login] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-google-authentication/)

1. Navigate to the [My Applications](http://go.microsoft.com/fwlink/p/?linkid=262039&clcid=0x409) page in the Live Connect Developer Center, log on with your Microsoft account if needed.

1. Click your app name in the **My applications** list.

	![Image 24](Images/image-24.png?raw=true)

1. Click **Edit settings**, then **API Settings** and make a note of the value of **Client secret**. In **Redirect domain**, enter the domain of your mobile service, in the format **https://[YOUR-SERVICE-NAME].azure-mobile.net/**, where _YOUR-SERVICE-NAME_ is the name of your mobile service, then click **Save**.

	![Image 25](Images/image-25.png?raw=true)
 
	You must provide this value to Mobile Services to be able to use Live Connect for authentication.

	> **Note:** The client secret is an important security credential. Do not share the client secret with anyone or distribute it with your app.

1. Return to the [Microsoft Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

	![Image 26](Images/image-26.png?raw=true)


1. Click the **Identity** tab, enter the **Client secret** obtained from Live Connect, and click **Save**.

	![Image 27](Images/image-27.png?raw=true)

<a name="Restrict-permissions" />
### Task 2 - Restricting permissions ###

1. In the Management Portal, click the **Data** tab, and then click the **TodoItem** table.

	![Image 28](Images/image-28.png?raw=true)

1. Click the **Permissions** tab, set all permissions to **Only authenticated users**, and then click **Save**. This will ensure that all operations against the **TodoItem** table require an authenticated user.

	![Image 29](Images/image-29.png?raw=true)

1. Return to Visual Studio 2012 and press the **F5** key to run this app; verify that an exception with a status code of 401 (Unauthorized) is raised.
This happens because the app is accessing Mobile Services as an unauthenticated user, but the _TodoItem_ table now requires authentication.

Next, you will update the app to authenticate users with your Microsoft Account before requesting resources from the mobile service.

<a name="Add-authentication" />
### Task 3 - Adding authentication to your Windows store app ###

1. In the project in Visual Studio open **MainPage.xaml.cs**.

1. Update the **OnNavigatedTo** event handler to be async and add a call to the **LoginAsync** method:
	<!-- mark:1,3 -->
	````C#
	protected async override void OnNavigatedTo(NavigationEventArgs e)
	{
		await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
		RefreshTodoItems();            
	}
	````

1. Press the **F5** key to run the app and sign into Live Connect with your Microsoft Account.

	When you are successfully logged-in, the app will run without auth errors, and you will be able to query Mobile Services and make updates to data.

<a name="Exercise4" />
## Exercise 4: Adding a Scheduled Job to your Mobile Service ##

In this exercise you will learn how to execute a script on a scheduled basis using **Microsoft Azure Mobile Services**.  In this scenario we will configure the scheduler to poll Twitter every 15 minutes and then send a Tile update with the latest tweets.

<a name="Configuring-your-windows-store-app-for-wide-tiles" />
### Task 1 - Configuring your Windows store app for Wide Tiles ###

1. In Visual Studio open your **package.appxmanifest** and select the **Application UI** tab.

1. Provide a **Wide Tile Logo** of 310x150 pixels. To do this, click **Wide Logo** in the **All Image Assets** and change the logo in the **Wide Logo** section. 

	![Image 37](Images/image-37.png?raw=true)

	> **Note:** If you do not have an image of these dimensions available you can use Microsoft Paint to quickly create one.


### Task 2 - Configuring the Mobile Services scheduler ###

1. In the **scheduler** tab, click on **Create the scheduler job** link.

	![Image 38](Images/image-38.png?raw=true)

1. Specify a name for the job and make sure the schedule frequency is set to **every 15 minutes**. Click the check mark to create the job.

	![Image 39](Images/image-39.png?raw=true)

1. Select the created job from the job list.

	![Image 40](Images/image-40.png?raw=true)

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
																					
				  push.wns.sendTileWideSmallImageAndText04(channel.uri, {
						image1src: tweet.profile_image_url,                            
						text1: '@' + tweet.from_user,
						text2: tweet.text
				  });                  
																															 
			 });
		}
	 });
	}
	````

1. Once you paste the script into the editor, click the **Save** button to store the changes to the script.

	![Image 41](Images/image-41.png?raw=true)

 
1. In Visual Studio, press **F5** to build and run the application.  This will ensure your channel URI is up to date and will ensure the Default Wide tile is now on your Start screen.

1. Go back to the Microsoft Azure Management Portal, select the **Scheduler** tab of your mobile service, and then click **Enable** in the command bar to allow the job to run.

	![Image 42](Images/image-42.png?raw=true)

1. To test your script immediately rather than wait 15 minutes for it to be scheduled, click **Run Once** in the command bar.

	![Image 43](Images/image-43.png?raw=true)

1. Return to the start screen and see the latest update on your application tile.

	![Image 44](Images/image-44.png?raw=true)


<a name="Summary"></a>
## Summary ##
By completing this hands-on lab you have learnt how to:

- Create a Microsoft Azure Mobile Service.
- Use the Microsoft Azure Mobile Services SDK.
- Learn how to Insert, Update, Read and Delete rows from a Mobile Service.
- Add Push Notifications to your application.
- Lock down your Mobile Service such that only authenticated users can consume it.
- Use the Scheduler to execute scripts at a scheduled interval.

---
