<a name="title" />
# Getting Started with Windows Azure Mobile Services and iOS #

---
<a name="Overview" />
## Overview ##
This demo script demonstrates how you can leverage Windows Azure Mobile Services to add structured storage and integrated authentication to your iOS applications.

> **Note:** This is a demo script that can be used as a guide for demos when presenting on Windows Azure Mobile Services.

<a name="technologies" />
### Key Technologies ###

- Windows Azure subscription - you can sign up for free trial [here][1]
- Windows Azure Mobile Services Preview enabled on your subscription
- Windows Azure Mobile Services iOS Client SDK
- Xcode

[1]: http://bit.ly/WindowsAzureFreeTrial

<a name="setup-and-configuration" />
### Setup and Configuration ###

In order to execute this demo you need to set up your environment.

1. Download and Install Xcode

1. Download and Install the Windows Azure Mobile Services SDK

<a name="Demo 1: Creating your first Mobile Service" />
## Demo 1: Creating your first Mobile Service ##

The goal of this demo is to use the quick start within the portal to quickly demonstrate the structured storage capability of Windows Azure Mobile services. The Push Notifications demo will demonstrate how you can add code to both the server and client to demonstrate the APIs used by the quick start.

<a name="create-a-new-mobile-service" />
### Create a new mobile service ###
Follow these steps to create a new mobile service.

1. Log into the [Windows Azure Management Portal](https://manage.windowsazure.com) and navigate to Mobile Services in the left side navigation.

1. Click the **+New** button then click **Compute**,**Mobile Service**, **Create** 

1. On the **Create a Mobile Service** dialog provide a unique subdomain in the **URL** field.  Once verification that the subdomain is unique proceed with the next steps.

1. Select either to use an existing database or new database and region. **Speaking Point:** If you select an existing database Windows Azure Mobile Services will separate multiple Mobile Service tenants by schema. 

1. Complete the remainder of the database settings in the Create Mobile Service wizard

You have now created a new mobile service that can be used by your mobile apps.

<a name="create-a-new-app" />
### Create a new app ###
Once you have created your mobile service, you can follow an easy quick start in the Management Portal to either create a new Windows Store app or modify an existing app to connect to your mobile service.

1. After the status updates to **Ready** for your mobile service in the **Mobile Services** tab of the Management portal. Click on your mobile service name to navigate into the service.

1. Click on ![Image 16](Images/image-16.png?raw=true) to see the Mobile Services quick start page.

1.  Under **CHOOSE PLATFORM** select **IOS**.

1. Select **Create a new iOS app** and perform the three steps provided
	- For the **Get the tools** step Install the iOS SDK if you have not done so already.  The iOS SDK provides a client side API that can be used within your iOS apps.
	- Click **Create Todoitem table**.  This will create a table for the starter project.  

	- In the Download and run your app step click **Download**.

This downloads the project for the sample _To do list_ application that is connected to your mobile service. Save the compressed project file to your local computer, and make a note of where you save it.

> **Speaking Point:** You can add additional tables manually by clicking the "Data" tab and using the Create button within.

<a name="run-your-app" />
### Run your app ###
The final stage of this tutorial is to run and explore your new iOS App.


1. Browse to the location where you saved the compressed project files, expand the files on your computer, and open the project file in Xcode.

1. Press the **RUN** button in the top left to compile and run your app.

1. In the app, type meaningful text, such as _Complete the demo_, in the **Enter text to create a new item** textbox, and then click **+**.

	**Speaking Point:** Explain this sends a POST request to the new mobile service hosted in Windows Azure. Data from the request is inserted into the TodoItem table. Items stored in the table are returned by the mobile service, and the data is displayed in the second column in the app.  The schema is inferred by the items sent over.

1. Back in the Management Portal, click the **Data** tab and then click the **TodoItems** table and observe that the data as been successfully stored

<a name="Explore-your-app-code" />
### Explore your app code ###

In this step we explore _To do list_ application code and see how simple the Windows Azure Mobile Services Client SDK makes it to interact with Windows Azure Mobile Services.

1. Return to the downloaded To do list application in Xcode

1. In Project Navigator **expand the Frameworks folder** and show the Windows Azure Mobile Services framework.  **Speaking Point:** you may also add the Windows Azure Mobile Services framework to any existing iOS projects.

1. Open TodoService.m and show the TodoService class.  This class demonstrates how to interact with a Mobile Serivce data table in an iOS app.  In the **init** method, you'll see the **MSClient** constructor which is passed the Application URL string and the Application Key:

	````objective-c
	MSClient *newClient = [MSClient clientWithApplicationURLString:@"https://iostest.azure-mobile.net/"
                                withApplicationKey:@"WBipYLNMvHhmmHduKgzkuErzjZynpf79"];
	
	````
1. Look lower in the **init** method at setting **self.table**.  This shows how to instantiate a **MSTable** for a specific table in Mobile Serivces:

	````objective-c
	self.table = [_client getTable:@"TodoItem"];
	
	````

1.  Continue on ath look at the following to see how Inserts, Updates, and Reads are done:

	- Performing an Insert

	````
		[self.table insert:item completion:^(NSDictionary *result, NSError *error) {...}];
	````
	- Performing an Update

		````
			[self.table update:mutable completion:^(NSDictionary *item, NSError *error) {...}];
		````
	- Performing a Read

		````
			[self.table readWhere:predicate completion:
				^(NSArray *results, NSInteger totalCount, NSError *error) {...}];
		````


**Speaking Point:** As you see, all of the methods to send or request data from Mobile Services are handled asynchronously and expect a callback block to be passed in to handle whatever should be executed when the server request is complete. 

Next we will move on to look at how you can secure your Mobile Service endpoints using Facebook

<a name="Demo 2: Adding Auth to Your App and Services" />
## Demo 2: Adding Auth to Your App and Services ##

This demo shows you how to authenticate users in Windows Azure Mobile Services from an iOS app. In this demo, you add authentication to the quickstart project using Facebook. When successfully authenticated by Facebook, a logged-in the application will be able to consume your Mobile Service.

<a name="Register-your-app" />
### Register your app ###

To be able to authenticate users, you must register your iOS app at the Facebook Developer Center. 

1. Navigate to the [Facebook Developer Center](https://developers.facebook.com) page, log on with your Facebook account if needed, and then follow the instructions to create your app.

1. For the ***Callback URL*** enter the URL of your Mobile Service.  This can be found on the ***Dashboard*** tab in the portal under ***Mobile Service URL*** on the right side.

1. Log on to the [Windows Azure Management Portal](https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

1. Click the **Identity** tab, enter the **App ID** and the **App Secret** obtained in the Facebook developer center, and click **Save**.

<a name="Restrict-permissions" />
### Restrict permissions ###

1. In the Management Portal, click the **Data** tab, and then click the **TodoItem** table.

1. Click the **Permissions** tab, set all permissions to **Only authenticated users**, and then click **Save**. This will ensure that all operations against the **TodoItem** table require an authenticated user. This also simplifies the scripts in the next tutorial because they will not have to allow for the possibility of anonymous users

1. Return to Xcode and tap the ***Run*** button to rerun the quickstart-based app; verify that no data is returned and show the error output in the debug console.
This happens because the app is accessing Mobile Services as an unauthenticated user, but the _TodoItem_ table now requires authentication.

Next, you will update the app to authenticate users with Facebook before requesting resources from the mobile service.

<a name="Add-authentication" />
### Add authentication ###
	
1.  In Xcode open the **TodoListController.m** file.  In the **viewDidLoad** method, remove the following code:

	````
	[self.todoService refreshDataOnSuccess:^{
		[self.tableView reloadData];
	}];
	````

1.  After the **viewDidLoad** method, add the following code:

	````
	 -(void)viewDidAppear:(BOOL)animated {
		 // If user is already logged in, no need to ask for auth
		 if (todoService.client.currentUser == nil)
		 {
				  // We want the login view to be presented after the this run loop has completed
				  // Here we use a delay to ensure this.
				  [self performSelector:@selector(login) withObject:self afterDelay:0.1];
			 }
		}


	-(void) login
		{
			 UINavigationController *controller =


			[self.todoService.client
			 loginViewControllerWithProvider:@"facebook"
			 completion:^(MSUser *user, NSError *error) {


			 if (error) {
						NSLog(@"Authentication Error: %@", error);
						// Note that error.code == -1503 indicates
						// that the user cancelled the dialog
			 } else {
				  // No error, so load the data
				  [self.todoService refreshDataOnSuccess:^{
						[self.tableView reloadData];
				  }];
			 }


			 [self dismissViewControllerAnimated:YES completion:nil];
		}];


		[self presentViewController:controller animated:YES completion:nil];


		}                                
	````

1. Re-run your app and log in through Facebook.  

When you are successfully logged-in, the app should run without errors, and you should be able to query Mobile Services and make updates to data.