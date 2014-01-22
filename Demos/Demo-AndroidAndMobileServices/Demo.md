<a name="title" />
# Getting Started with Windows Azure Mobile Services and Android #

---
<a name="Overview" />
## Overview ##
This demo script demonstrates how you can leverage Windows Azure Mobile Services to add structured storage and integrated authentication to your Android applications.

> **Note:** This is a demo script that can be used as a guide for demos when presenting on Windows Azure Mobile Services.

<a name="technologies" />
### Key Technologies ###

- Windows Azure subscription - you can sign up for free trial [here][1]
- Windows Azure Mobile Services Preview enabled on your subscription
- Windows Azure Mobile Services Android Client SDK
- Eclipse
- Android Development Tools (ADT) installed in Eclipse

[1]: http://bit.ly/WindowsAzureFreeTrial

<a name="setup-and-configuration" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment.

1. Download and install Eclipse

1. Download and install the Android Development Tools (ADT)

1. Download and install the Windows Azure Mobile Services SDK

<a name="Demo 1: Creating your first Mobile Service" />
## Demo 1: Creating your first Mobile Service ##

The goal of this demo is to use the quick start within the portal to quickly demonstrate the structured storage capability of Windows Azure Mobile services. 

<a name="create-a-new-mobile-service" />
### Create a new mobile service ###
Follow these steps to create a new mobile service.

1. Log into the [Windows Azure Management Portal](https://manage.windowsazure.com) and navigate to Mobile Services in the left side navigation.

1. Click the **+New** button then click **Compute**, **Mobile Service**, **Create** 

1. On the **Create a Mobile Service** dialog provide a unique subdomain in the **URL** field.  Once verification that the subdomain is unique proceed with the next steps.

1. Select either to use an existing database or new database and region. **Speaking Point:** If you select an existing database Windows Azure Mobile Services will separate multiple Mobile Services by schema (which is sort of prepended to the table name). 

1. Complete the remainder of the database settings in the Create Mobile Service wizard

You have now created a new mobile service that can be used by your mobile apps.

<a name="create-a-new-app" />
### Create a new app ###
Once you have created your mobile service, you can follow an easy quick start in the Management Portal to either create a new Android app or modify an existing app to connect to your mobile service.

1. After the status updates to **Ready** for your mobile service in the **Mobile Services** tab of the Management portal. Click on your mobile service name to navigate into the service.

1. Click on ![Image 16](Images/image-16.png?raw=true) to see the Mobile Services quick start page.

1.  Under **CHOOSE PLATFORM** select **Android**.

1. Select **Create a new Android app** and perform the three steps provided
	- For the **Get the tools** step, you can skip downloading the SDK as it will come in the libs folder of the Quick Start app.
	- Click **Create Todoitem table**.  This will create a table for the starter project.  

	- In the Download and run your app step click **Download**.

	- After downloading and unzipping, you'll need to go into the File menu and choose Import.  Browse to your unzipped project.

This downloads the project for the sample _To do list_ application that is connected to your mobile service. Save the compressed project file to your local computer, and make a note of where you save it.

> **Speaking Point:** You can add additional tables manually by clicking the "Data" tab and using the Create button within.

<a name="run-your-app" />
### Run your app ###
The final stage of this tutorial is to run and explore your new Android App.

1. Start your emulator from Eclipse.

1. Right click on your project in the **Package Explorer** and choose **Run As** and **Android Application**

1. In the app, type meaningful text, such as _Complete the demo_, in the **Add a ToDo item** textbox, and then click **Add**.

	**Speaking Point:** Explain this sends a POST request to the new mobile service hosted in Windows Azure. Data from the request is inserted into the TodoItem table. Items stored in the table are returned by the mobile service, and the data is displayed in the second column in the app.  The schema is inferred by the items sent over.  You can also talk about how it uses JSON for it's data format.

1. Back in the Management Portal, click the **Data** tab and then click the **TodoItems** table and observe that the data as been successfully stored

<a name="Explore-your-app-code" />
### Explore your app code ###

In this step we explore _To do list_ application code and see how simple the Windows Azure Mobile Services Client SDK makes it to interact with Windows Azure Mobile Services.

1. Return to the downloaded To do list application in Eclipse

1. In Project Explorer expand your project folder, and then expand the **libs** folder and show the Windows Azure Mobile Services jars.  **Speaking Point:** you may also add the Windows Azure Mobile Services framework to any existing Android projects by dropping these jars in that project's **libs** folder.

1. Expand **src** and then **com.example.yourproject**.

1. Open **ToDoActivity.java** and show the **onCreate** method.  Near the top of this method highlight the creation of the **MobileServiceClient** object which is passed the Application URL string and the Application Key:   

	````java
	mClient = new MobileServiceClient("https://mymobileservice.azure-mobile.net/",
				"myapplicationkey", this).withFilter(new ProgressFilter());
	
	````

1. Look lower in the **onCreate** method at setting **mToDoTable**.  This shows how to instantiate a **MobileServiceTable** for a specific table in Mobile Serivces:

	````java
	mToDoTable = mClient.getTable(ToDoItem.class);
	
	````

1.  Continue on ath look at the following to see how Inserts, Updates, and Reads are done:

	- Performing an Insert

		````java
			mToDoTable.insert(item, new TableOperationCallback<TodoItem>{ ...
		````
	- Performing an Update

		````java
			mToDoTable.update(item, new TableOperationCallback<ToDoItem>() {...
		````
	- Performing a Read

		````
			mToDoTable.where().field("complete").eq(val(false)).execute(new TableQueryCallback<ToDoItem>() {...
		````


**Speaking Point:** As you see, all of the methods to send or request data from Mobile Services are handled asynchronously and expect a callback to be passed in to handle whatever should be executed when the server request is complete. 

Next we will move on to look at how you can secure your Mobile Service endpoints using Facebook

<a name="Demo 2: Adding Auth to Your App and Services" />
## Demo 2: Adding Auth to Your App and Services ##

This demo shows you how to authenticate users in Windows Azure Mobile Services from an Android app. In this demo, you add authentication to the quickstart project using Facebook. When successfully authenticated by Facebook, a logged-in the application will be able to consume your Mobile Service.

<a name="Register-your-app" />
### Register your app ###

To be able to authenticate users, you must register your Android app at the Facebook Developer Center. 

1. Navigate to the [Facebook Developer Center](https://developers.facebook.com) page, log on with your Facebook account if needed, and then follow the instructions to create your app.

1. For the ***Callback URL*** enter the URL of your Mobile Service.  This can be found on the ***Dashboard*** tab in the portal under ***Mobile Service URL*** on the right side.

1. Log on to the [Windows Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

1. Click the **Identity** tab, enter the **App ID** and the **App Secret** obtained in the Facebook developer center, and click **Save**.

<a name="Restrict-permissions" />
### Restrict permissions ###

1. In the Management Portal, click the **Data** tab, and then click the **TodoItem** table.

1. Click the **Permissions** tab, set all permissions to **Only authenticated users**, and then click **Save**. This will ensure that all operations against the **TodoItem** table require an authenticated user. This also simplifies the scripts in the next tutorial because they will not have to allow for the possibility of anonymous users

1. Return to Eclipse and click the ***Run*** button to rerun the quickstart-based app; verify that no data is returned and show the error output in logcat.
This happens because the app is accessing Mobile Services as an unauthenticated user, but the _TodoItem_ table now requires authentication.

Next, you will update the app to authenticate users with Facebook before requesting resources from the mobile service.

<a name="Add-authentication" />
### Add authentication ###

1.  In the Package Explorer in Eclipse, open **ToDoActivity.java** and add the following import statements

	````
	import com.microsoft.windowsazure.mobileservices.MobileServiceUser;
	import com.microsoft.windowsazure.mobileservices.MobileServiceAuthenticationProvider;
	import com.microsoft.windowsazure.mobileservices.UserAuthenticationCallback;
	````
	
1.  Add the following **authenticate** method to the **ToDoActivity** class

	````
	private void authenticate() {
		//Login using the Facebook provider
		mClient.login(MobileServiceAuthenticationProvider.Facebook,
			new UserAuthenticationCallback() {
				@Override
				public void onCompleted(MobileServiceUser user, Exception exception, 
											ServiceFilterResponse resonse) {
					if (exception == null) {
						createAndShowDialog(String.format("You are now logged in - %1$2s", 
											user.getUserId()), "Success");
						createTable();
					} else {
						createAndShowDialog("You must log in.  Login required", "Error");
					}
				}
			});
	}
	````

1.  In the **onCreate** method, add the following code after you create your **MobileServiceClient**

	````java
	authenticate();
	````

1.  Take the code that is now after your call to **authenticate()** out of the **onCreate** method and place it in a new **createTable** method

	````java
	private void createTable() {
		//Get the Mobile Service Table instance to use
		mToDoTable = mClient.getTable(ToDoItem.class);

		mTextNewToDo = (EditText) findViewById(R.id.textNewToDo);
	
		//Create an adapter to bind the items with the view
		mAdapter = new ToDoItemAdapter(this, R.layout.row_list_to_do);
		ListView listViewToDo = (ListView) findViewById(R.id.listViewToDo);
		listViewToDo.setAdapter(mAdapter);

		//Load the items from the Mobile Service
		refreshItemsFromTable();
	}

	````

1. Re-run your app and log in through Facebook.  

When you are successfully logged-in, the app should run without errors, and you should be able to query Mobile Services and make updates to data.