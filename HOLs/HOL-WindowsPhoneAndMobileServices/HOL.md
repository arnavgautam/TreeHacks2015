<a name="title" />
# Getting Started with Windows Azure Mobile Services for Windows Phone#

---
<a name="Overview" />
## Overview ##
This hands-on lab demonstrates how you can leverage Visual Studio 2012 and Windows Azure Mobile Services to add structured storage, push notifications, and integrated authentication to your Windows Phone application.

Windows Azure Mobile Services is a Windows Azure service offering designed to make it easy to create highly-functional mobile apps using Windows Azure. Mobile Services brings together a set of Windows Azure services that enable backend capabilities for your apps. These capabilities includes simple provisioning and management of tables for storing app data, integration with notification services, integration with well-known identity providers for authentication, among others.

<a name="objectives"></a>
### Objectives ###
In this hands-on lab, you will learn how to:

- Create a new Azure Mobile Service for Windows Phone 8 and an application that consumes it.
- Validate data using Server scripts.
- Adding support for push notifications to your applications.
- Supporting authentication in your application using well-known authentication providers, such as Microsoft Account or Twitter.

<a name="prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Microsoft Visual Studio 2012](http://msdn.microsoft.com/vstudio/products/)
- [Microsoft Visual Studio 2012 Update 2][1] or higher.
- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)
- Windows Azure Mobile Services Preview enabled on your subscription - For more information, see [here](http://www.windowsazure.com/en-us/develop/mobile/tutorials/create-a-windows-azure-account/#enable) 
- [Windows Azure Mobile Services Client SDK] (http://www.windowsazure.com/en-us/downloads/)
- [Windows Phone 8 SDK](http://dev.windowsphone.com/en-us/downloadsdk)

[1]: http://www.microsoft.com/en-us/download/details.aspx?id=38188

<a name="Setup"/>
### Setup ###

If you are performing this hands-on lab using Windows, you can follow these steps to check for dependencies. Otherwise, check the **Prerequisites** list above.

1. Open a Windows Explorer window and browse to the lab’s **Source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process that will configure your environment.

1. If the User Account Control dialog is shown, confirm the action to proceed.

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

- [Exercise 1: Creating your first Mobile Service](#Exercise1)
- [Exercise 2: Validating Data using Server Scripts](#Exercise2)
- [Exercise 3: Adding Push Notifications to your Application](#Exercise3)
- [Exercise 4: Adding Auth to your Application and Services](#Exercise4)

---
<a name="Exercise1" />
## Exercise 1: Creating your first Mobile Service ##

The goal of this exercise is to use the quick start within the portal to quickly demonstrate the structured storage capability of Windows Azure Mobile services. The Push Notifications exercise will demonstrate how you can add code to both the server and client to demonstrate the APIs used by the quick start.

<a name="create-a-new-mobile-service" />
### Task 1 - Creating a New Mobile Service ###

1. Log into the [Windows Azure Management Portal](https://manage.windowsazure.com) and navigate to Mobile Services.

1. At the bottom of the navigation pane, click **+NEW**.

	![New Button](Images/new-button.png?raw=true)

	_New Button_

1. Expand **Compute | Mobile Service**, then click **Create**.
 
	![Creating a New Mobile Service](Images/creating-new-mobile-service.png?raw=true)
 
	_Creating a new Mobile Service_

	This displays the **New Mobile Service** dialog.

1. In the **Create a mobile service** page, type a subdomain name for the new mobile service in the **URL** textbox and wait for name verification. Once name verification completes, click the right arrow button to go to the next page.
 
	![New Mobile Service](Images/create-mobile-service-step-1.png?raw=true)

	_New Mobile Service_

	This displays the **Specify database settings** page.

	> **Note:** As part of this hands-on lab, you create a new SQL Database instance and server. You can reuse this new database and administer it as you would any other SQL Database instance. If you already have a database in the same region as the new mobile service, you can instead choose **Use existing Database** and then select that database. The use of a database in a different region is not recommended because of additional bandwidth costs and higher latencies.

1. In **Name**, type the name of the new database, then type **Login name**, which is the administrator login name for the new SQL Database server, type and confirm the password, and click the check button to complete the process.
 
	![Specifying Database Settings](Images/create-mobile-service-step-2.png?raw=true)

	_Specifying Database Settings_

	> **Note:**  When the password that you supply does not meet the minimum requirements or when there is a mismatch, a warning is displayed.  We recommend that you make a note of the administrator login name and password that you specify; you will need this information to reuse the SQL Database instance or the server in the future. You have now created a new mobile service that can be used by your mobile apps.

You have now created a new mobile service that can be used by your mobile apps.

<a name="create-a-new-app" />
### Task 2 - Creating a New Windows Phone 8 App ###
Once you have created your mobile service, you can follow an easy quick start in the Management Portal to either create a new Windows Phone app or modify an existing app to connect to your mobile service.
In this section you will create a new Windows Phone 8 app that is connected to your mobile service.

1. After the status updates to **Ready** for your mobile service in the **Mobile Services** tab of the Management portal. Click your mobile service name to navigate into the service.

1. Click ![Image 16](Images/image-16.png?raw=true) to see the Mobile Services quick start page.

1. In the **Choose a platform** section, click **Windows Phone 8**.

	![Create a new application](Images/create-a-new-application.png?raw=true)

	_Mobile Services Quickstart Page_

1. Select **Create a new Windows Phone 8 app** and perform the three steps provided:
	- For the **Get the tools** step Install the Mobile Services SDK if you have not done so already.  The Mobile Services SDK provides a client side API that can be used within your Visual C++, XAML/C#/VB and HTML/JS Windows Phone apps.
	- Click **Create Todoitem table**.  This will create a table for the starter project.  
	- Under **Download and run app**, click **Download**.


	![Create a new application](Images/create-a-new-application-2.png?raw=true)

	_Creating a new application_

This downloads the project for the sample _To do list_ application that is connected to your mobile service. Save the compressed project file to your local computer, and make a note of where you save it.

> **Note:** You can add additional tables manually by clicking the "Data" tab and using the Create button within.

<a name="run-your-app" />
### Task 3 - Hosting and Running Your Windows Phone 8 App ###

The final stage of this exercise is to run and explore your new Windows Phone app.

1. Browse to the location where you saved the compressed project files, extract the files on your computer, and open the solution file in Visual Studio 2012.

	![Opening the Windows Phone App in VS2012](Images/opening-app-in-visual-studio.png?raw=true)
	
	_Windows Phone App in Visual Studio 2012_

1. Press the **F5** key to rebuild the project and start the app.

1. In the app, type meaningful text, such as _Complete the demo_, in the **Insert a TodoItem** textbox, and then click **Save**.

	![Running the Windows Phone App](Images/running-the-windows-phone-app.png?raw=true)
	
	_Running the Windows Phone App_

	> **Note:** Saving a TodoItem sends a POST request to the new mobile service hosted in Windows Azure. Data from the request is inserted into the TodoItem table. Items stored in the table are returned and are subsequently populated in the list within the application. Pressing referesh causes a query against the mobile service filtering back incomplete items. Checking the checkbox causes an update against the Mobile Service.

1. Back in the Management Portal, click the **Data** tab and then click the **TodoItems** table and observe that the data has been successfully stored.

	![Data Inserted by the App Into the Table](Images/data-inserted-by-the-app-into-the-table.png?raw=true)
	
	_Data inserted by the app into the table_

> **Note:** An effective way to do this portion of the exercise is to have the WP emulator docked to the left side of the screen and the TodoItem Data tab docked in the right side of the screen. Insert the item then hit the **Refresh** button in the toolbar at the bottom of the portal to see the new and updated data.


<a name="Explore-your-app-code" />
### Task 4 - Exploring Your App Code ###

In this step we will explore _To do list_ application code and see how simple the Windows Azure Mobile Services Client SDK makes it to interact with Windows Azure Mobile Services.

1. Return to the downloaded To do list application Visual Studio 2012.

1. In solution explorer **expand the references folder** and see the Windows Azure Mobile Services Client SDK reference.

1. Open **App.xaml.cs** and show the **MobileServiceClient** class.  This is the key class provided by the client SDK that provides a way for your application to interact with Windows Azure Mobile Services. The first parameter in the constructor is the Mobile Service endpoint and the second parameter is the Application Key for your Mobile Service.

	````C#
	public static MobileServiceClient MobileService 
			= new MobileServiceClient( 
				"https://todolist.azure-mobile.net/"
				,"vIWepmcOXGPsYCJvDDcFBKjnOVxzLG52" );
	
	````

1. Open **MainPage.xaml.cs** to see how the mobile service client is then used for Inserts, Updates, and Reads.

   - The TodoItem entity has the **JsonProperty** attribute that can be used to provide different names to be used within the mobile service and for persisting the data to the underlying Windows Azure SQL Database.  Note that you can also manipulate the Table that the entity is serialized to using **DataTable** attribute.  This is useful for some environments where the DBAs may require a specific table naming convention e.g tblTodoItem etc.
		
		````C#
		 public class TodoItem
		 {
			 public int Id { get; set; }

			 [JsonProperty(PropertyName = "text")]
			 public string Text { get; set; }

			 [JsonProperty(PropertyName = "complete")]
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
		private async void RefreshTodoItems()
        {
            // This code refreshes the entries in the list view be querying the TodoItems table.
            // The query excludes completed TodoItems
            try
            {
                items = await todoTable
                    .Where(todoItem => todoItem.Complete == false)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                MessageBox.Show(e.Message, "Error loading items", MessageBoxButton.OK);
            }

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

	> **Note** Additionally, there is a _todoTable.DeleteAsync(...)_ method used for removing items.

---
<a name="Exercise2" />
## Exercise 2: Validating Data using Server Scripts ##

In this exercise you will see how you can inject your own business logic within the Mobile Services CRUD operation pipeline using Server Scripts.  This is useful for scenarios like validation on the server side and implementing custom server side workflows.

<a name="updating-server-scripts" />
### Task 1 - Updating Server Scripts ###

1. In the portal select the **TodoItem** table.

1. Select the **Scripts** tab.

1. Notice that there are options in the dropdown for the Insert, Read, Update, and Delete operations. Select the **Insert** operation.

1. Add the following code to the **Insert** operation to demonstrate how to add some simple validation within a Server Script.

	````JavaScript
	function insert(item, user, request) {
    	if(item.text.length < 5){
		request.respond(statusCodes.BAD_REQUEST, 'text is too short');
    	} else {
		request.execute();
    	}
	}
	````

	This script checks the length of the **TodoItem.text** property and sends an error response when the length is less than 5 characters. Otherwise, the **execute** method is called to complete the insert.

	> **Note:** You can remove a registered script on the **Script** tab by clicking **Clear** and then **Save**.

	![Updating the Insert script](Images/updating-the-insert-script.png?raw=true)
	
	_Updating the Insert script_

1. Click **Save** to save the script into the mobile service.

<a name="updating-the-client" />
### Task 2 - Updating the Client ###

Now that the mobile service is validating data and sending error responses, you need to update your application to be able to handle error responses from validation.

1. In Visual Studio 2012 Express for Windows Phone, open the _todolist_ project.

1. Press the **F5** key to run the app, then type text shorter than 5 characters in the textbox and click **Save**. Notice that the app raises an unhandled **MobileServiceInvalidOperationException** as a result of the 400 response (Bad Request) returned by the mobile service.

1. Open the file MainPage.xaml.cs, then replace the existing **InsertTodoItem** method with the following code.

	````C#
	private async void InsertTodoItem(TodoItem todoItem)
	{
		// This code inserts a new TodoItem into the database. When the operation completes
		// and Mobile Services has assigned an Id, the item is added to the CollectionView
		try
		{
			await todoTable.InsertAsync(todoItem);
			items.Add(todoItem);
		}
		catch (MobileServiceInvalidOperationException e)
		{
			MessageBox.Show(e.Message,
					string.Format("{0} (HTTP {1})",
					e.Response.ReasonPhrase,
					(int)e.Response.StatusCode), MessageBoxButton.OK);
		}
	}
	````

	This version of the method includes error handling for the **MobileServiceInvalidOperationException** that displays the error response in a _MessageBox_.

1. Now run the application again, enter an invalid text, and click **Save**. The error message will be displayed.

	![Receiving the validation error in the client](Images/receiving-the-validation-error-in-the-client.png?raw=true)
	
	_Receiving the validation error in the client_

---
<a name="Exercise3" />
## Exercise 3: Adding Push Notifications to your Application ##

In this exercise, you will add push notifications, using the Microsoft Push Notification service (MPNS), to the project. When completed, an insert in the mobile service will generate a push notification back to your app. In this case you will use a Tile Notification as you can receive this while the app is running to generate the notification.

<a name="Add-push-notifications-to-the-app" />
### Task 1 - Adding Push Notifications to the App ###

1. Open the file **App.xaml.cs** and add the following using statement.

	```` C#
	using Microsoft.Phone.Notification;
	````

1. Add the following to **App.xaml.cs**.

	````C#
	public static HttpNotificationChannel CurrentChannel { get; private set; }

	private void AcquirePushChannel()
	{
		CurrentChannel = HttpNotificationChannel.Find("MyPushChannel");


		if (CurrentChannel == null)
		{
			CurrentChannel = new HttpNotificationChannel("MyPushChannel");
			CurrentChannel.Open();
			CurrentChannel.BindToShellTile();
		}
	}
	````

	This code acquires and stores a channel for a push notification subscription and binds it to the app's default tile.

	> **Note:** In this this hands-on lab, the mobile service sends a flip Tile notification to the device. When you send a toast notification, you must instead call the **BindToShellToast** method on the channel. To support both toast and tile notifications, call both **BindToShellTile** and **BindToShellToast**.

1. At the top of the **Application_Launching** event handler in App.xaml.cs, add the following call to the new **AcquirePushChannel** method.

	````C#
	AcquirePushChannel();
	````
	This guarantees that the **CurrentChannel** property is initialized each time the application is launched.

1. Open the project file MainPage.xaml.cs and add the following new attributed property to the **TodoItem** class.

	````C#
	[JsonProperty(PropertyName = "channel")]
	public string Channel { get; set; }
	````

	> **Note:** When dynamic schema is enabled on your mobile service, a new 'channel' column is automatically added to the **TodoItem** table when a new item that contains this property is inserted.

1. Replace the **ButtonSave_Click** event handler method with the following code:

	```` C#
	private void ButtonSave_Click(object sender, RoutedEventArgs e)
	{
		var todoItem = new TodoItem { Text = TodoInput.Text, 
		Channel = App.CurrentChannel.ChannelUri.ToString() };
		InsertTodoItem(todoItem);
	}
	````

	This sets the client's current channel value on the item before it is sent to the mobile service.

1. In the Solution Explorer, expand **Properties**, open the WMAppManifest.xml file, click the **Capabilities** tab and make sure that the **ID_CAP_PUSH_NOTIFICATION** capability is checked.

	![Adding Push Notifications support to the application](Images/adding-push-notifications-to-the-manifest.png?raw=true)
	
	_Adding Push Notifications support to the application_

	This makes sure that your app can receive push notifications.

<a name="Insert-data-to-receive-notifications" />
### Task 2 - Insert data to receive notifications ###

In this section we add a Channel table and server side scripts to send push notifications everytime someone inserts into our TodoItem table.

1. Log on to the [Windows Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

1. Click the **Data** tab and then click the **TodoItem** table.

	![Accessing the TodoItem table](Images/accessing-the-todoitem-table.png?raw=true)
	
	_Accessing the TodoItem table_


1. In **todoitem**, click the **Script** tab and select **Insert**.

	![Updating the Insert Server Script](Images/updating-the-insert-server-script.png?raw=true)
	
	_Updating the Insert Server Script_

	This displays the function that is invoked when an insert occurs in the **TodoItem** table.

1. Replace the insert function with the following code, and then click **Save**.

	```` C#
	function insert(item, user, request) {
		request.execute({
		success: function () {
			// Write to the response and then send the notification in the background
			request.respond();
			push.mpns.sendFlipTile(item.channel, {
				title: item.text
			}, {
				success: function (pushResponse) {
					console.log("Sent push:", pushResponse);
				}
			});
		}
	});
	}
	````

This registers a new insert script, which uses the [mpns object](http://go.microsoft.com/fwlink/p/?linkid=271130&clcid=0x409) to send a push notification (the inserted text) to the channel provided in the insert request.

<a name="test-push-notification-in-your-app" />
### Task 3 - Testing Push Notifications in your App ###

1. In Visual Studio, select **Deploy Solution** on the **Build** menu.

1. In the emulator, swipe to the left to reveal the list of installed apps and find the new **TodoList** app.
Tap and hold on the app icon, and then select **pin to start** from the context menu.
 
	![Pinning the App to Start](Images/pinning-the-app-to-start.png?raw=true)
	
	_Pinning the App to Start_

	This pins a tile named **TodoList** to the start menu.

1. Tap the tile named **TodoList** to launch the app.
 
	![Running the App from Start](Images/running-the-app-from-start.png?raw=true)
	
	_Running the App from Start_

1. In the app, enter the text "hello push" in the textbox, and then click **Save**.

	![Creating a new TodoItem to test push](Images/creating-a-new-todo-item.png?raw=true)
	
	_Creating a new TodoItem to test push_

	This sends an insert request to the mobile service to store the added item.

1. Press the **Start** button to return to the start menu.

	![Receiving the push notification](Images/receiving-the-push-notification.png?raw=true)
	
	_Receiving the push notification_

	Notice that the application received the push notification and that the title of the tile is now **hello push**.

---
<a name="Exercise4" />
## Exercise 4: Adding Auth to your Application and Services ##

This exercise shows you how to authenticate users in Windows Azure Mobile Services from a Windows Phone 8 app. In this demo, you add authentication to the project using Twitter and lock down the service such that only authenticated clients can Insert/Update/Delete/Read from your TodoItem table. When successfully logged in using Twitter the authenticated users will be able to consume your Mobile Service.

<a name="Register-your-app" />
### Task 1 - Register your app for authentication and configure Mobile Services ###

To be able to authenticate users, you must register your app with an identity provider. You must then register the provider-generated client secret with Mobile Services.

1. Log on to the [Windows Azure Management Portal] (https://manage.windowsazure.com/), click **Mobile Services**, and then click your mobile service.
 
1. Click the **Dashboard** tab and make a note of the **Site URL** value.

	![Getting the Mobile Service URL](Images/getting-the-mobile-service-url.png?raw=true)
	
	_Getting the Mobile Service URL_

	You may need to provide this value to the identity provider when you register your app.

1. Choose a supported identity provider from the list below and follow the steps to register your app with that provider:
	- [Microsoft Account] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-microsoft-authentication/)
	- [Facebook login] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-facebook-authentication/)
	- [Twitter login] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-twitter-authentication/)
	- [Google login] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-google-authentication/)

	Remember to make a note of the client identity and secret values generated by the provider.

	> **Security Note:** The provider-generated secret is an important security credential. Do not share this secret with anyone or distribute it with your app.

1. Back in the Management Portal, click the **Identity** tab, enter the app identifier and shared secret values obtained from your identity provider, and click **Save**.

	![Registering the authentication settings](Images/registering-the-authentication-provider.png?raw=true)
	
	_Registering the authentication settings_

	Both your mobile service and your app are now configured to work with your chosen authentication provider.

<a name="Restrict-permissions" />
### Task 2 - Restricting Permissions to Authenticated Users ###

1. In the Management Portal, click the **Data** tab, and then click the **TodoItem** table.
 
1. Click the **Permissions** tab, set all permissions to **Only authenticated users**, and then click **Save**. This will ensure that all operations against the **TodoItem** table require an authenticated user.

	![Setting permissions to the TodoItem table](Images/setting-permissions-to-the-todoitem-table.png?raw=true)
	
	_Setting permissions to the TodoItem table_
 
1. In **Visual Studio 2012 Express for Windows Phone**, open the project that you created at the beggining of this lab, if it is not already opened.

1. Press the **F5** key to run the app; verify that an unhandled exception with a status code of _401 (Unauthorized)_ is raised after the app starts.

	This happens because the app attempts to access Mobile Services as an unauthenticated user, but the _TodoItem_ table now requires authentication.

Next, you will update the app to authenticate users before requesting resources from the mobile service.

<a name="Add-authentication" />
### Task 3 - Adding Authentication to the App###

1. Open the project file **mainpage.xaml.cs** and add the following code snippet to the MainPage class.

	```` C#
	private MobileServiceUser user;
	private async System.Threading.Tasks.Task Authenticate()
	{
		while (user == null)
		{
			string message;
			try
			{
				user = await App.MobileService
				.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
				message =
				string.Format("You are now logged in - {0}", user.UserId);
			}
			catch (InvalidOperationException)
			{
				message = "You must log in. Login Required";
			}

			MessageBox.Show(message);
		}
	}
	````

	This creates a member variable for storing the current user and a method to handle the authentication process. The user is authenticated by using a Twitter login.

	> **Note:** If you are using an identity provider other than Twitter, change the value of **MobileServiceAuthenticationProvider** above to the value for your provider.

1. Delete or comment-out the existing **OnNavigatedTo** method override and replace it with the following method that handles the **Loaded** event for the page.

	````C#
	async void MainPage_Loaded(object sender, RoutedEventArgs e)
	{
		await Authenticate();
		RefreshTodoItems();
	}
	````

	This method calls the new **Authenticate** method.

1. Replace the **MainPage** constructor with the following code.

	````C#
	// Constructor
	public MainPage()
	{
		InitializeComponent();
		this.Loaded += MainPage_Loaded;
	}
	````

	This constructor also registers the handler for the **Loaded** event.

1. Press the **F5** key to run the app and sign into the app with your chosen identity provider.
When you are successfully logged-in, the app should run without errors, and you should be able to query Mobile Services and make updates to data.

	![Successfully logged in the application using Twitter](Images/logged-in-the-app-using-twitter.png?raw=true)
	
	_Successfully logged in the application using Twitter_
