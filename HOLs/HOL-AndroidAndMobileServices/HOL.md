<a name="title" />
# Getting Started With Microsoft Azure Mobile Services and Android #

---
<a name="Overview" />
## Overview ##
Microsoft Azure Mobile Services is a Microsoft Azure service offering designed to make it easy to create highly-functional mobile apps using Microsoft Azure. Mobile Services brings together a set of Microsoft Azure services that enable backend capabilities for your apps. These capabilities includes simple provisioning and management of tables for storing app data, integration with notification services, integration with well-known identity providers for authentication, among others.

The following is a functional representation of the Mobile Services architecture:

![Mobile Services Diagram](Images/mobile-services-diagram.png?raw=true)

_Mobile Services Diagram_

In this hands-on lab you will learn how to add a cloud-based backend service to an Android app using Microsoft Azure Mobile Services. You will create both a new mobile service and a simple _TodoList_ app that stores app data in the new mobile service. Also you will perform server side validation, send push notifications and add user authentication using Mobile Services features.

<a name="Objectives" />
### Objectives ###
- Create a Microsoft Azure Mobile Service
- Store app data in the new mobile service
- Validate data server-side using Mobile Services Server Scripts feature
- Add support for push notifications to your applications
- Authenticate users with different identity providers using Mobile Services

<a name="technologies" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Android SDK ADT Bundle][2] (includes Eclipse IDE, Android Developer Tools and the latest Android platform)
- [Java Runtime Environment (JRE) or Java Development Kit (JDK)](http://www.oracle.com/technetwork/java/javase/downloads/index.html)
- Android 4.2 or higher
- An active [Google account](accounts.google.com) (for sending Push Notifications)
- A Microsoft Azure subscription account that has the Microsoft Azure Mobile Services feature enabled

	>**Note:** If you don't have an account, you can create a free trial account in just a couple of minutes. For details, see [Microsoft Azure Free Trial](http://aka.ms/WATK-FreeTrial).
	If you have an existing account but need to enable the Microsoft Azure Mobile Services preview, see [Enable Microsoft Azure preview features](http://www.windowsazure.com/en-us/develop/mobile/tutorials/create-a-windows-azure-account/#enable).

[2]: https://go.microsoft.com/fwLink/?LinkID=280125&clcid=0x409

<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating Your First Mobile Service](#Exercise1)
1. [Server Scripts and Structured Storage](#Exercise2)
1. [Getting Started with Push Notifications](#Exercise3)
1. [Getting Started with Authentication](#Exercise4)

Estimated time to complete this lab: **60** minutes.

---

<a name="Exercise1" />
## Exercise 1: Creating Your First Mobile Service ##

This exercise shows you how to add a cloud-based backend service to an Android app using Microsoft Azure Mobile Services. You will create both a new mobile service and a simple _TodoList_ app that stores app data in the new mobile service. Lastly, you will explore your app code to see how it interacts with Microsoft Azure Mobile Services.

<a name="creating-a-new-mobile-service" />
### Task 1 - Creating a New Mobile Service ###
Follow these steps to create a new mobile service.

1. Log into the [Microsoft Azure Management Portal](https://manage.windowsazure.com) and navigate to Mobile Services.

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

	> **Note:** As part of this hands-on, you create a new SQL Database instance and server. You can reuse this new database and administer it as you would any other SQL Database instance. If you already have a database in the same region as the new mobile service, you can instead choose Use existing Database and then select that database. The use of a database in a different region is not recommended because of additional bandwidth costs and higher latencies.

1. In **Name**, type the name of the new database, then type **Login name**, which is the administrator login name for the new SQL Database server, type and confirm the password, and click the check button to complete the process.
 
	![Specifying Database Settings](Images/create-mobile-service-step-2.png?raw=true)

	_Specifying Database Settings_

	> **Note:**  When the password that you supply does not meet the minimum requirements or when there is a mismatch, a warning is displayed.  We recommend that you make a note of the administrator login name and password that you specify; you will need this information to reuse the SQL Database instance or the server in the future.

You have now created a new mobile service that can be used by your mobile apps.

<a name="creating-a-new-android-app" />
### Task 2 - Creating a New Android App ###
Once you have created your mobile service, you can follow an easy quick start in the Management Portal to either create a new app or modify an existing app to connect to your mobile service.

In this task you will create a new Android app that is connected to your mobile service.

1. In the Management Portal, click **Mobile Services**, and then click the mobile service that you just created.

1. In the quick start tab, select **Android** under **Choose platform** and expand **Create a new Android app**.

	![Creating an Android App](Images/create-android-app.png?raw=true)

	_Creating an Android App_

	This displays the three easy steps to create an Android app connected to your mobile service.

	![Steps to Create Android App](Images/create-android-app-steps.png?raw=true)

	_Steps to Create Android App_

1. If you haven't already done so, download and install the [Android SDK ADT Bundle](https://go.microsoft.com/fwLink/?LinkID=280125&clcid=0x409) on your local computer or virtual machine.

1. Click **Create TodoItem table** to create a table to store app data.

1. Under **Download and run your app**, click **Download**.

	This downloads the project for the sample _TodoList_ application that is connected to your mobile service. Save the compressed project file to your local computer, and make a note of where you saved it.

<a name="hosting-and-running-your-android-app" />
### Task 3 - Hosting And Running Your Android App ###

In this task you will build and run your new app.

1. Browse to the location where you downloaded the Android SDK ADT Bundle. Inside the **eclipse** folder, run **eclipse.exe**.

	> **Note:** Make sure you installed the Java Runtime Environment (JRE) or Java Development Kit (JDK) before running Eclipse.

1. In the **Workspace Launcher** window, select a workspace and click **OK**.

	![Selecting a workspace](Images/selecting-workspace.png?raw=true)

	_Selecting a Workspace_

1. Once Eclipse (Java-ADT) IDE is opened, click **File** and then **Import**.

	![Importing the Downloaded Project](Images/importing-downloaded-project.png?raw=true)

	_Importing the Downloaded Project_

1. In the **Import** window, expand **Android**, click **Existing Android Code into Workspace**, and then click **Next**.

	![Selecting an Import Source](Images/selecting-import-source.png?raw=true)

	_Selecting an Import Source_

1. Click **Browse**, browse to the location of the expanded project files, click **OK**, make sure that the **ToDoActivity** project is checked, then click **Finish**.

	![Selecting Project to Import](Images/selecting-project-to-import.png?raw=true)

	_Selecting Project to Import_

	This imports the project files into the current workspace.

	![Imported Project](Images/imported-project.png?raw=true)

	_Imported Project_

1. To be able to run the project in the Android emulator, you must define at least one Android Virtual Device (AVD). You will now use the Android Virtual Device Manager to create and manage these devices. To do this, go to **Window** and the click **Android Virtual Device Manager**.

	![Opening Android Virtual Device Manager](Images/opening-avd.png?raw=true)

	_Opening Android Virtual Device Manager_

1. In the **Android Virtual Device Manager** window, click **New**.

	![Creating a New Android Virtual Device](Images/creating-new-AVD.png?raw=true)

	_Creating a New Android Virtual Device_

1. Fill the **AVD Name** field with a name for the AVD, select a **Device**, set the **Target** to _Android 4.2.2_ and then click **OK**. Then close the Android Virtual Device Manager window.

	![Creating a New Android Virtual Device](Images/creating-new-AVD-2.png?raw=true)

	_Creating a New Android Virtual Device_

1. In Eclipse, from the **Run** menu, click **Run** to start the project in the Android emulator.

	![Starting the Project](Images/starting-the-project.png?raw=true)

	_Starting the Project_

1. In the **Run As** window, select **Android Application** and click **OK**.

	![Selecting a Way to Run the App](Images/selecting-a-way-to-run-the-app.png?raw=true)

	_Selecting a Way to Run the App_

1. In the app, type meaningful text, such as _Sign-up for the free trial_, and then click **Add**. Do the same with other texts such as _Create the mobile service_ and _Complete the hands-on lab_.

	![Application Running](Images/application-running.png?raw=true)

	_Application Running_

	This sends POST requests to the new mobile service hosted in Microsoft Azure. Data from the request is inserted into the TodoItem table. Items stored in the table are returned by the mobile service, and then data is displayed in the list.

1. Back in the Microsoft Azure Management Portal, go to the mobile service you created earlier. Click the **Data** tab and then click the **TodoItem** table.

	![Accessing the TodoItem Table](Images/acessing-todo-item-table.png?raw=true)

	_Accessing the TodoItem Table_

	This lets you browse the data inserted by the app into the table. Notice that the _Complete_ value of all the todo items is false because you didn't complete any of them yet.

	![Browsing the Todo Item Table](Images/browsing-todo-item-table.png?raw=true)

	_Browsing the Todo Item Table_

	> **Note:** Mobile Services simplifies the process of storing data in a SQL Database. By default, you don’t need to predefine the schema of tables in your database. Mobile Services automatically adds columns to a table based on the data you insert. To change this dynamic schema behavior, use the Dynamic Schema setting on the Configure tab. It is recommended that you disable dynamic schema support before publicly releasing your app.

<a name="create-a-new-mobile-service" />
### Task 4 - Exploring Your App Code ###

In this task you will explore _TodoList_ application code and see how simple the Microsoft Azure Mobile Services Client SDK makes it to interact with Microsoft Azure Mobile Services.

1. Return to the downloaded _TodoList_ application in Eclipse.

1. In Package Explorer expand your project folder, and then expand the **libs** folder. Here you will find the Microsoft Azure Mobile Services jars.  

	**Note:** you may also add the Microsoft Azure Mobile Services framework to any existing Android project by dropping these jar files in that project's **libs** folder.

	![Mobile Services Jar Files](Images/mobile-services-jars.png?raw=true)

	_Mobile Services Jar Files_

1. Expand **src** and then **com.example.todolist** and open **ToDoActivity.java**. Check the **onCreate** method.  Near the top of this method you will find the creation of the **MobileServiceClient** object, which gets passed the application URL string and the Application Key.   

	````java
	mClient = new MobileServiceClient("https://todolist.azure-mobile.net/",
				"uCcQgxzBlZjJcABztxMqxFQQJSXJzs63", this).withFilter(new ProgressFilter());
	````

1. Look lower in the **onCreate** method, specifically when setting **mToDoTable** variable. This shows how to instantiate a **MobileServiceTable** for a specific table in Mobile Services, in this case the **TodoItem** one.

	````java
	mToDoTable = mClient.getTable(ToDoItem.class);
	````

1. Let's see how the mobile service client is then used for Insert, Update and Read operations.
	- Performing an **Insert** in the **addItem** method:		
	<!-- mark:11 -->
	````java
	public void addItem(View view) {
		...

		// Create a new item
		ToDoItem item = new ToDoItem();

		item.setText(mTextNewToDo.getText().toString());
		item.setComplete(false);
		
		// Insert the new item
		mToDoTable.insert(item, new TableOperationCallback<ToDoItem>() {
			...
		});

		...
	}
	````
	> **Note:** To add additional columns to the table, simply send an insert request including the new properties from your app with dynamic schema enabled. Once a column is created, its data type cannot be changed by Mobile Services. Insert or update operations fail when the type of a property in the JSON object cannot be converted to the type of the equivalent column in the table.
	- Performing an **Update** in the **checkItem** method:		
	<!-- mark:7 -->
	````java
	public void checkItem(ToDoItem item) {
		...

		// Set the item as completed and update it in the table
		item.setComplete(true);
		
		mToDoTable.update(item, new TableOperationCallback<ToDoItem>() {
			...
		});
	}
	````
	- Performing a **Read** in the **refreshItemsFromTable** method:
	<!-- mark:4 -->
	````java
	private void refreshItemsFromTable() {
		// Get the items that weren't marked as completed and add them in the
		// adapter
		mToDoTable.where().field("complete").eq(_val_(false)).execute(new TableQueryCallback<ToDoItem>() {
			...
		});
	}
	````
	
	> **Note:** As you see, all of the methods to send or request data from Mobile Services are handled asynchronously and expect a callback to be passed in to handle whatever should be executed when the server request is complete.

---

<a name="Exercise2" />
## Exercise 2: Validating Data Using Server Scripts ##

This exercise shows you how to leverage server scripts in Microsoft Azure Mobile Services. Server scripts are registered in a mobile service and can be used to perform a wide range of operations on data being inserted and updated, including validation and data modification. In this exercise, you will define and register server scripts that validate and modify data. Because the behavior of server side scripts often affects the client, you will also update your Android app to take advantage of these new behaviors.

> **Note:** For more information on server scripts check this [How-to](How-to <http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/work-with-server-scripts/>).

<a name="adding-validation" />
### Task 1 - Adding validation ###
It is always a good practice to validate the length of data that is submitted by users. First, you register a script that validates the length of string data sent to the mobile service and rejects strings that are too long, in this case longer than 10 characters.

1. Log into the [Microsoft Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

	![Accessing Your Mobile Service](Images/accessing-your-mobile-service.png?raw=true)

	_Accessing Your Mobile Service_

1. Click the **Data** tab and then click the **TodoItem** table.

	![Accessing the TodoItem Table](Images/acessing-todo-item-table.png?raw=true)

	_Accessing the TodoItem Table_

1. Click **Script** and then select the **Insert** operation. Replace the existing script with the following function, and then click **Save**.

	````javascript
	function insert(item, user, request) {
		 if (item.text.length > 10) {
			  request.respond(statusCodes.BAD_REQUEST, 'Text length must be 10 characters or less.');
		 } else {
			  request.execute();
		 }
	}
	````

	> **Note:** These scripts must call execute or respond to make sure that a response is returned to the client. When a script has a code path in which neither of these functions is invoked, the operation may become unresponsive. Additionally, notice that Mobile Services does not preserve state between script executions. Every time a script executes, a new global context is created in which the script is executed.

	This script checks the length of the text property and sends an error response when the length exceeds 10 characters. Otherwise, the execute method is called to complete the insert.

	> **Note:** You can remove a registered script on the **Script** tab by clicking **Clear** and then **Save**.

	![Adding Insert Script in TodoItem Table](Images/adding-insert-script-todo-item-table.png?raw=true)

	_Adding Insert Script in TodoItem Table_

<a name="updating-the-client" />
### Task 2 - Updating the client ###
Now that the mobile service is validating data and sending error responses, you need to verify that your app is correctly handling error responses from validation.

1. In Eclipse, open the **TodoList** project.

1. In the **ToDoActivity.java** file, locate the **addItem** method and replace the call to the _createAndShowDialog_ method with the following code.

	````java
	createAndShowDialog(exception.getCause().getMessage(), "Error");
	````

	This displays the error message returned by the mobile service.

1. From the **Run** menu, click **Run** to start the app, then type text longer than 10 characters in the textbox and click the **Add** button. Notice that error is handled and the error message is displayed to the user.

	![Application Displaying Error](Images/app-displaying-error.png?raw=true)

	_Application Displaying Error_

---

<a name="Exercise3" />
## Exercise 3: Getting Started with Push Notifications ##

This exercise shows you how to use Microsoft Azure Mobile Services to send push notifications to an Android app. In this hands-on lab you add push notifications using the Google Cloud Messaging (GCM) service to the quick start project. When complete, your mobile service will send a push notification each time a record is inserted.

<a name="register-your-app-for-push-notifications" />
### Task 1 - Register Your App for Push Notifications ###

1.	Navigate to the [Google apis](http://go.microsoft.com/fwlink/p/?linkid=268303&clcid=0x409) web site, sign-in with your Google account credentials, and then click **Create project**.

	![Google Apis Page](Images/google-apis.png?raw=true)

	_Google Apis Page_

	> **Note:** When you already have an existing project, you are directed to the **Dashboard** page after login. To create a new project from the Dashboard, expand **API Project**, click **Create** under **Other projects**, then enter a project name and click **Create project**.

	> ![Creating New Project In Google Apis](Images/new-project-google-apis.png?raw=true)

	> _Creating New Project In Google Apis_

1.	In the **Overview** section of your project, make a note of the integer value in the **Project Number** field.
Later in this exercise you will set this value as the SENDER_ID variable in the client.

	![Project Number](Images/project-number.png?raw=true)

	_Project Number_

1.	On the [Google apis](http://go.microsoft.com/fwlink/p/?linkid=268303&clcid=0x409) page, click **Services**, then click the toogle to turn on **Google Cloud Messaging for Android** and accept the terms of service.

1.	Click **API Access**, and then click **Create new Server key...**

	![Creating New Server Key](Images/creating-new-server-key.png?raw=true)

	_Creating New Server Key_

1.	In **Configure Server Key for {your-project-name}**, click **Create**.

	![Configuring Server Key](Images/configuring-server-key.png?raw=true)

	_Configuring Server Key_

1.	Make a note of the **API key** value.

	![Api Key](Images/api-key.png?raw=true)

	_Api Key_

Next, you will use this API key value to enable Mobile Services to authenticate with GCM and send push notifications on behalf of your app.

<a name="configure-mobile-services-to-send-push-requests" />
### Task 2 - Configure Mobile Services to Send Push Requests ###

1.	Log on to the [Microsoft Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

	![Accessing Your Mobile Service](Images/accessing-your-mobile-service.png?raw=true)

	_Accessing Your Mobile Service_

1.	Click the **Push** tab, enter the **API Key** value obtained from GCM in the previous task, and then click **Save**.

	![Configuring Push Notifications in Your Mobile Service](Images/configuring-push-notifications-in-mobile-service.png?raw=true)

	_Configuring Push Notifications in Your Mobile Service_
	
You mobile service is now configured to work with GCM to send push notifications.

<a name="add-push-notifications-to-your-app" />
### Task 3 - Add Push Notifications to Your App ###

1.	In Eclipse, click **Window**, then click **Android SDK Manager**.

1.	In the Android SDK Manager, expand **Extras**, ensure that **Google Cloud Messaging for Android Library** is installed, make a note of the **SDK Path**, click **Install Package**, select **Accept License** to accept the license, then click **Install**.

	![Enabling Google Cloud Messaging for Android SDK](Images/enabling-gcm-for-android.png?raw=true)

	_Enabling Google Cloud Messaging for Android SDK_

1.	Repeat the previous step to install the **Google APIs** for the current version of Android.

1.	Browse to the SDK path, and copy the **gcm.jar** file from the **\extras\google\gcm\gcm-client\dist** subfolder into the **\libs** project subfolder. Then in **Package Explorer**, right-click the **libs** folder and click **Refresh**.

	The gcm.jar library file is now shown in your project.

	![GCM Jar File](Images/gcm-jar.png?raw=true)

	_GCM Jar File_

1.	Open the project file **AndroidManifest.xml** with the Android Common XML Editor and add the following new permissions after the existing uses-permission element.

	````xml
	<permission android:name="{my_app_package}.permission.C2D_MESSAGE" android:protectionLevel="signature" />
	<uses-permission android:name="{my_app_package}.permission.C2D_MESSAGE" /> 
	<uses-permission android:name="com.google.android.c2dm.permission.RECEIVE" />
	<uses-permission android:name="android.permission.GET_ACCOUNTS" />
	<uses-permission android:name="android.permission.WAKE_LOCK" />
	````

1.	Add the following code into the application element.

	````xml
	<receiver android:name="com.google.android.gcm.GCMBroadcastReceiver" android:permission="com.google.android.c2dm.permission.SEND">
		<intent-filter>
			<action android:name="com.google.android.c2dm.intent.RECEIVE" />
			<action android:name="com.google.android.c2dm.intent.REGISTRATION" />
			<category android:name="{my_app_package}" />
		</intent-filter>
	</receiver>
	<service android:name=".GCMIntentService" />
	````

1.	In the code inserted in the previous two steps, replace _**my_app_package**_ with the name of the app package for your project, which is the value of the **manifest.package** attribute (_e.g.: com.example.todolist_).

1.	Open the file **ToDoItem.java** inside the **src** folder and add the following code to the **TodoItem** class.

	````java
	@com.google.gson.annotations.SerializedName("channel")
	private String mRegistrationId;

	public String getRegistrationId() {
		 return mRegistrationId;
	}

	public final void setRegistrationId(String registrationId) {
		 mRegistrationId = registrationId;
	}
	````

	This code creates a new property that holds the registration ID.

	> **Note:** When dynamic schema is enabled on your mobile service, a new 'channel' column is automatically added to the TodoItem table when a new item that contains this property is inserted.

1.	Open the file **ToDoActivity.java**, and add the following import statement.

	````java
	import com.google.android.gcm.GCMRegistrar;
	````

1.	Add the following private variables to the class, where _\<SENDER_ID\>_ is the project ID assigned by Google to your app in the first task.

	````java
	private String mRegistationId;
	public static final String SENDER_ID = "<SENDER_ID>";
	````

1.	In the **onCreate** method, add this code before the **MobileServiceClient** class is instantiated.

	````java
	GCMRegistrar.checkDevice(this);
	GCMRegistrar.checkManifest(this);
	mRegistationId = GCMRegistrar.getRegistrationId(this);
	if (mRegistationId.equals("")) {
		 GCMRegistrar.register(this, SENDER_ID);
	}
	````

	This code gets the registration ID for the device.

1.	Add the following line of code to the **addItem** method before the call to _mToDoTable.insert_.

	````java
	item.setRegistrationId(mRegistationId.equals("") ? GCMIntentService.getRegistrationId() : mRegistationId);
	````

	This code sets the _registrationId_ property of the item to the registration ID of the device.

1.	In the **Package Explorer**, right-click the app package, click **New** and then select **Class**.

1.	In **Name** type _GCMIntentService_, in **Superclass** type _com.google.android.gcm.GCMBaseIntentService_ and then click **Finish**

	![New GCMIntentService Class](Images/new-gcm-intent-service-class.png?raw=true)

	_New GCMIntentService Class_

	This creates the new **GCMIntentService** class.

1.	Add the following import statements to the brand new GCMIntentService class.

	````java
	import android.app.Notification;
	import android.app.NotificationManager;
	import android.support.v4.app.NotificationCompat;
	````

1.	Now add the following static variable and constructor.

	````java
	private static String sRegistrationId;

	public static String getRegistrationId() {
		 return sRegistrationId;
	}

	public GCMIntentService(){
		 super(ToDoActivity.SENDER_ID);
	}
	````

	This code invokes the Superclass constructor with the app SENDER_ID value of the app.

1.	Replace the existing **onMessage** and **onRegistered** method overrides with the following code.

	````java
	@Override
	protected void onMessage(Context context, Intent intent) {

	NotificationCompat.Builder mBuilder =
			  new NotificationCompat.Builder(this)
					.setSmallIcon(R.drawable.ic_launcher)
					.setContentTitle("New todo item!")
					.setPriority(Notification.PRIORITY_HIGH)
					.setContentText(intent.getStringExtra("message"));
	NotificationManager mNotificationManager =
		 (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
	mNotificationManager.notify(0, mBuilder.build());


	}

	@Override
	protected void onRegistered(Context context, String registrationId) {
		 sRegistrationId = registrationId;
	}
	````

	> **Note:** In this task, only the **onMessage** and **onRegistered** overrides are implemented. In a real-world app you should consider implementing all four method overrides.

	Your app is now updated to support push notifications.

<a name="updating-registered-insert-script-in-management-portal" />
### Task 4 - Updating the Registered Insert Script in the Management Portal ###

1.	In the Management Portal, click the **Data** tab and then click the **TodoItem** table.

	![Accessing the TodoItem Table](Images/acessing-todo-item-table.png?raw=true)

	_Accessing the TodoItem Table_

1.	Once in **TodoItem**, click the **Script** tab and select the **Insert** operation. Replace the insert function with the following code, and then click **Save**.

	````javascript
	function insert(item, user, request) {
		 request.execute({
			  success: function() {
					// Write to the response and then send the notification in the background
					request.respond();
					push.gcm.send(item.channel, item.text, {
						 success: function(response) {
							  console.log('Push notification sent: ', response);
						 }, error: function(error) {
							  console.log('Error sending push notification: ', error);
						 }
					});
			  }
		 });
	}
	````

	This registers a new insert script, which uses the [gcm object](http://go.microsoft.com/fwlink/p/?linkid=282645&clcid=0x409) to send a push notification (the inserted text) to the device provided in the insert request.

	![Updating Insert Script for Push Notifications](Images/updating-insert-script-for-push-notifications.png?raw=true)

	_Updating Insert Script for Push Notifications_

<a name="testing-push-notifications-in-your-app" />
### Task 5 - Testing Push Notifications in Your App ###
	
1.	Restart Eclipse. In the **Package Explorer**, right-click the project and click **Properties**. Then select **Android** and check **Google APIs**. Click **OK**.

	![Adding Google APIs to Project Properties](Images/adding-google-apis-to-project-properties.png?raw=true)

	_Adding Google APIs to Project Properties_
	
	This targets the project for the Google APIs.

1.	From the **Window** menu, select **Android Virtual Device Manager**. Then select your device and click **Edit**.

	![Editing Android Virtual Device](Images/editing-avd.png?raw=true)

	_Editing Android Virtual Device_

1.	Select **Google APIs** in **Target** and click **OK**. Close the Android Virtual Device Manager.

	![Selecting Android Virtual Device Target](Images/selecting-avd-target.png?raw=true)

	_Selecting Android Virtual Device Target_
	
	This targets the AVD to use Google APIs.

	> **Note:** When you run this app in the emulator, make sure that you use an Android Virtual Device (AVD) that supports Google APIs.
	
1.	From the **Run** menu, then click **Run** to start the app.

1.	In the app, type meaningful text, such as _A new Mobile Service task_ and then click the **Add** button.

	![Adding Meaningful Text](Images/adding-meaningful-text.png?raw=true)

	_Adding Meaningful Text_
	
1.	Verify that a notification is received.

	![Push Notification Received](Images/push-notification-received.png?raw=true)

	_Push Notification Received_

---

<a name="Exercise4" />
## Exercise 4: Getting Started with Authentication ##

In this exercise you will see how to authenticate users in Microsoft Azure Mobile Services from your app. You will add authentication to your project using an identity provider that is supported by Mobile Services. After being successfully authenticated and authorized by Mobile Services, the user ID value is displayed.

<a name="registering-your-app-for-authentication-and-configure-mobile-services" />
### Task 1 - Registering Your App for Authentication and Configure Mobile Services ###

To be able to authenticate users, you must register your app with an identity provider. You must then register the provider-generated client secret with Mobile Services.

1.	Log into the [Microsoft Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your mobile service.

	![Accessing Your Mobile Service](Images/accessing-your-mobile-service.png?raw=true)

	_Accessing Your Mobile Service_

1.	Click the **Dashboard** tab and make a note of the **Mobile Service URL** value.
You may need to provide this value to the identity provider when you register your app.

	![Mobile Service Dashboard](Images/dashboard.png?raw=true "Dashboard")

	_Mobile Service Dashboard_

1.	Choose a supported identity provider from the list below and follow the steps to register your app with that provider. Remember to make a note of the client identity and secret values generated by the provider.
	* [Microsoft Account](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-microsoft-authentication/)
	* [Facebook login ](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-facebook-authentication/)
	* [Twitter login ](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-twitter-authentication/)
	* [Google login ](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-google-authentication/)

	> **SECURITY NOTE**: The provider-generated secret is an important security credential. Do not share this secret with anyone or distribute it with your app.

1.	Back in the Management Portal, click the **Identity** tab, enter the app identifier and shared secret values obtained from your identity provider, and click **Save**.

	![TodoList Identity Tab](Images/todolist-identity.png?raw=true "Todolist identity")

	_Todolist Identity Tab_

	Both your mobile service and your app are now configured to work with your chosen authentication provider.

<a name="restricting-permissions-to-authenticated-users" />
### Task 2 - Restricting Permissions to Authenticated Users ###

1.	In the Microsoft Azure Management Portal, click the **Data** tab, and then click the **TodoItem** table.

	![Accessing the TodoItem Table](Images/acessing-todo-item-table.png?raw=true)

	_Accessing the TodoItem Table_

1.	Click the **Permissions** tab, set all permissions to **Only authenticated users**, and then click **Save**. This will ensure that all operations against the **TodoItem** table require an authenticated user.

	![Setting Permissions to the TodoItem Table](Images/permissions-todo-item-table.png?raw=true "Google cloud messaging settings")

	_Setting Permissions to the TodoItem Table_

1.	In Eclipse, from the **Run** menu, click **Run** to start the app. Verify that an unhandled exception with a status code of 401 (Unauthorized) is raised after the app starts.

	![Unauthorized Exception](Images/unauthorized-exception.png?raw=true "Google cloud messaging settings")

	_Unauthorized Exception_

	This happens because the app attempts to access Mobile Services as an unauthenticated user, but the_TodoItem_ table now requires authentication.
Next, you will update the app to authenticate users before requesting resources from the mobile service.

<a name="adding-authentication-to-the-app" />
###Task 3 - Adding Authentication to the App ###

1.	In the Package Explorer in Eclipse, open the **ToDoActivity.java** file and add the following import statements.

	````java
	import com.microsoft.windowsazure.mobileservices.MobileServiceUser;
	import com.microsoft.windowsazure.mobileservices.MobileServiceAuthenticationProvider;
	import com.microsoft.windowsazure.mobileservices.UserAuthenticationCallback;
	````

1.	Add the following method to the **ToDoActivity** class.

	````java
	private void authenticate() {
	// Login using the Google provider.
	mClient.login(MobileServiceAuthenticationProvider.Facebook,
        new UserAuthenticationCallback() {

            @Override
            public void onCompleted(MobileServiceUser user,
                    Exception exception, ServiceFilterResponse response) {


                if (exception == null) {
                    createAndShowDialog(String.format(
                                    "You are now logged in - %1$2s",
                                    user.getUserId()), "Success");
                    createTable();
                } else {
                    createAndShowDialog("You must log in. Login Required", "Error");
                }
            }
        });
	}
	````

	This creates a new method to handle the authentication process. The user is authenticated by using a Facebook login. A dialog will be displayed showing the ID of the authenticated user. You cannot proceed without a positive authentication.

	>**Note**: If you are using an identity provider other than Facebook, change the value passed to the login method above to one of the following: MicrosoftAccount, Google, or Twitter.

1.	In the **onCreate** method, add the following line of code after the code that instantiates the _MobileServiceClient_ object.

	````java
	authenticate();
	````

	This call starts the authentication process.

1.	In the **onCreate** method, move the remaining code after _authenticate()_ to a new **createTable** method, which looks like the following code.

	````java
	private void createTable() {
		// Get the Mobile Service Table instance to use
		mToDoTable = mClient.getTable(ToDoItem.class);

		mTextNewToDo = (EditText) findViewById(R.id.textNewToDo);

		// Create an adapter to bind the items with the view
		mAdapter = new ToDoItemAdapter(this, R.layout.row_list_to_do);
		ListView listViewToDo = (ListView) findViewById(R.id.listViewToDo);
		listViewToDo.setAdapter(mAdapter);

		// Load the items from the Mobile Service
		refreshItemsFromTable();
	}
	````

1.	From the **Run** menu, then click **Run** to start the app and sign in with your chosen identity provider.	When you are successfully logged-in, the app should run without errors, and you should be able to query Mobile Services and make updates to data.

	![Logging Into the Application](Images/logging-into-the-application.png?raw=true)

	_Logging Into the Application_
