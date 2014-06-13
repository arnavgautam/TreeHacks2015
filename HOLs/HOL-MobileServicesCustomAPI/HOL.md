<a name="HOLTitle"></a>
# Creating a Microsoft Azure Mobile Service with a Custom API #

---
<a name="Overview"></a>
## Overview ##

Microsoft Azure Mobile Services enables you to define custom business logic that runs on the server. This logic is provided as JavaScript code that is stored and executed on the server. In Mobile Services, a server script is either registered to an insert, read, update, or delete operation on a given table or is assigned to a scheduled job. Server scripts have a main function along with optional helper functions. The signature of the main function depends on the whether the script is registered as a custom API, to a table operation, or to run as a scheduled job.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a mobile service and a custom API.
- Call the custom API from a browser.
- Access data, send pull notifications, and manage authentication from custom APIs.
- Integrate custom APIs with a Windows Store Application.

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Visual Studio Express 2012 for Windows 8](http://www.microsoft.com/visualstudio/) or higher
- A Microsoft Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

<a name="Setup"/>
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab’s **Source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process that will configure your environment and install the Visual Studio code snippets for this lab.

1. If the User Account Control dialog is shown, confirm the action to proceed.

>**Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.
>
>Make sure you have checked all the dependencies for this lab before running it.

<a name="UsingCodeSnippets"/>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2012 to avoid having to add it manually.

---
<a name="Exercises"/>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Exercise 1: Creating a Mobile Service and Enabling Custom API Support](#Exercise1)
2. [Exercise 2: Accessing Data, Pull Notifications and performing Authentication from your Custom API](#Exercise2)
3. [Exercise 3: Building a Windows 8 Client using your Custom API](#Exercise3)

Estimated time to complete this lab: **60** minutes.

>**Note:** Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise. Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.

<a name="Exercise1"></a>
### Exercise 1: Creating a Mobile Service and Enabling Custom API Support ###

In this exercise, you will create a new Mobile services and a Custom API for that service, which will be consumed in a Windows Store application.

<a name="Ex1Task1"></a>
#### Task 1 – Creating a new Mobile Service ####

In this task, you will create a new Mobile Service using the Microsoft Azure Portal.

1. Log into the [Microsoft Azure Management Portal](https://manage.windowsazure.com) and navigate to Mobile Services.

2. Click **+New** in the bottom toolbar.

	![The New item button](Images/image-2.png?raw=true)

	_The New item button_

3. Expand **Compute | Mobile Service**, then click **Create**.
 
	![Creating a new Mobile Service](Images/image-3.png?raw=true)
 
	_Creating a new Mobile Service_

4. In the **Create a mobile service** page, type a subdomain name for the new mobile service in the **URL** textbox and wait for name verification. Once name verification completes, click the right arrow button to go to the next page.
 
	![Create a Mobile Service dialog](Images/image-4.png?raw=true)

	_Create a Mobile Service dialog_

	> **Note:** As part of this exercise, you create a new SQL Database instance and server. You can use this new database and administer it as you would do with any other SQL Database instance. If you already have a database in the same region as the new mobile service, you can instead choose **Use existing Database** and then select that database. The use of a database in a different region is not recommended because of additional bandwidth costs and higher latencies.

5. In the **Specify database settings** dialog, type the name of the new database or leave the default value in the **Name** field. Select **New SQL Database Server** as the **Server**, and then type **Login name**, which is the administrator login name for the new SQL Database server, type and confirm the password, and click the check button to complete the process.
 
	![Specify database settings dialog](Images/image-5.png?raw=true)

	_Specify database settings dialog_

	> **Note:**  When the password that you supply does not meet the minimum requirements or when there is a mismatch, a warning is displayed.  We recommend that you make a note of the administrator login name and password that you specify; you will need this information to reuse the SQL Database instance or the server in the future.

You have now created a new mobile service that can be used by your mobile apps.

<a name="create-a-new-app" />
#### Task 2 - Downloading a Sample Windows Store App ####
Once you have created your mobile service, you can follow an easy quick start in the Management Portal to either create a new Windows Store app or modify an existing app to connect to your mobile service.

1. In the Management Portal, click **Mobile Services**, and then click the mobile service that you just created.

2. In the quick start tab, make sure that **Windows Store** is selected as the **Platform**, and expand **Create a new Windows Store app**.

	![The get started page](Images/image-6.png?raw=true)
	
	_The get started page_

	This displays the three easy steps to create a Windows 8 app connected to your mobile service.

3. If you haven't already done so, download and install [Visual Studio 2012 Express for Windows 8](http://go.microsoft.com/fwlink/?LinkId=257546&clcid=0x409) on your local computer or virtual machine.

4. Click **Create TodoItem Table** to create the table in the database.

5. Lastly, make sure that the **C#** language is selected and click **Download**.

	![Downloading the sample Windows Store app](Images/image-7.png?raw=true)

	_Downloading the sample Windows Store app_

	This downloads the project for the sample _Todo list_ application that is connected to your mobile service. Save the compressed project file to your local computer, and make a note of where you saved it.

6. Open **Visual Studio Express 2012 for Windows 8** and open the sample solution you just downloaded from your mobile service.

6. Press **F5** to run the application.

7. In the application, add two _todo items_ and save them by clicking **Save**. 

	![Running the Windows Store application](Images/image-8.png?raw=true)

	_Running the Windows Store application_


<a name="Ex1Task3"></a>
#### Task 3 – Creating a Custom API for your mobile service ####

In this task, you will create a custom API that will be consumed by your Windows Store application.

1. In the **Azure Management Portal**, browse to the **Mobile Services** section and click your recently created mobile service.

2. At the top of the screen click **API**.

3. Click the **Create** button to create a new custom API.

	![Creating a new custom API](Images/image-9.png?raw=true)

	_Creating a new custom API_

4. In the **Create a new custom API** dialog, complete the **API Name** field and set the **Get Permission** list to **Everyone**. Finally, click the checkmark button.

	![Create a new custom API dialog](Images/image-10.png?raw=true)

	_Create a new custom API dialog_

	> **Note:** Setting the GET operation permission to everyone allows you to call this operation in the API without sending credentials.

5. Once the custom API is created, click its name. You will be able to edit the custom script.

	![Editing the custom API code](Images/image-11.png?raw=true)

	_Editing the custom API code_


6. Add the following code to the custom script.

	````JavaScript
	exports.get = function(request, response) {
		var table = request.service.tables.getTable('TodoItem');
		table.read({ success: function(results)
			{
				if (results.length > 0) {
					response.send(200, results);
				}
				else
				{
					response.send(200,"No records");
				}     
			}
		});
	};
	````

7. Click **Save** to save the script modifications.

	> **Note:** Mobile Services does not preserve state between script executions. Every time a script executes, a new global context is created in which the script is executed. Do not define any state variables in your scripts. If you need to store state from one request to another, create a table in your mobile service in which to store your state, and then read and write your state to the table.

8. Open a browser window and browse to your mobile service API URL. E.g. _https://{your mobile service URL}/api/mycustomapi_.

	> **Note:** By default, Internet Explorer will not display the response of the API on the Window, it will download a _.json_ file with the response content. In order to display the JSON directly from Internet Explorer, go to the **Assets** folder of this lab and execute **enable-json-ie.cmd** script to enable JSON visualization. To disable it, execute **disable-json-ie.cmd**. The following steps will assume that you have JSON visualization enabled.

	![The custom API GET response](Images/image-12.png?raw=true)

	_The custom API GET response_

	> **Note:** You can get the mobile service URL from the dashboard page of your mobile service in the Azure Management Portal.

	Notice that the API returns the table data in JSON format.

---

<a name="Exercise2"></a>
### Exercise 2: Accessing Data, Pull Notifications and performing Authentication from your Custom API ###

In this exercise, you are going to create custom APIs that manipulates data, and others that send pull notifications. Lastly, you will edit the permissions of the APIs to restrict its access.

<a name="Ex2Task1"></a>
#### Task 1 – Manipulating Data ####

In this task, you will create a custom API that is used to query and add data into the mobile service database.

1. In the **Azure Management Portal**, browse to the **Mobile Services** section and click your recently created mobile service.

2. At the top of the screen click **API**.

3. Click the **Create** button from the bottom toolbar to create a new custom API.

	![Creating a new custom API](Images/new-custom-api-2.png?raw=true)

	_Creating a new custom API_

4. In the **Create a new custom API** dialog, type _TodoItems_ in the **API Name** field and leave the permissions configuration as it is. Finally, click the checkmark button.

	![Create a new custom API dialog](Images/image-13.png?raw=true)

	_Create a new custom API dialog_

5. Once the custom API is created, click its name. You will be able to edit the custom script.

6. Replace the existing script with the following code.

	````JavaScript
	exports.post = function(request, response) {
		var table = request.service.tables.getTable('TodoItem');
		var newItem = { text: request.query.text, complete: false };
		table.insert(newItem);
    
		response.send(200, newItem);
	};
	````
	
	This code responds POST requests, and inserts a new record in the _TodoItem_ table, using the text passed as a parameter through query string.

	> **Note:** To receive parameters passed through query string you will use _request.query.parameterName_. For parameters passed through POST you will be receiving the parameters in the request body: _request.body.parameterName_.

7. Insert the following code, after the code inserted in the previous step.

	````JavaScript
	exports.get = function(request, response) {
		var table = request.service.tables.getTable('TodoItem');
		table.where({ complete: false })
			.read({ success: function(results)
				{
					if (results.length > 0) {
						response.send(200, results);
					}
					else
					{
						response.send(200,"No records");
					}     
				}
			});
	};
	````

	This script handles GET requests and returns the records that are not yet completed in JSON format.


	![Editing the custom API code](Images/image-14.png?raw=true)

	_Editing the custom API code_

8. Click **Save** to save the script modifications.

> **Note:** You are going to test this functionality at a later stage.

<a name="Ex2Task2"></a>
#### Task 2 – Sending Pull Notifications ####

In this task, you will create a custom API that is used to get pull notifications for your mobile service application.

1. In the **Azure Management Portal**, create a new Custom API for your Mobile Service.

4. In the **Create a new custom API** dialog, type _notifications_ in the **API Name** field and change **Get permission** to **Everyone**. Finally, click the checkmark button. This creates the new API with public GET access.

	![Create a new custom API dialog](Images/image-15.png?raw=true)

	_Create a new custom API dialog_

5. Click the new _notifications_ entry in the API table.
 
6. Click the **Scripts** tab and replace the existing code with the following:
	
	````Javascript
	exports.get = function(request, response) {
		var wns = require('wns');
		var todoItems = request.service.tables.getTable('TodoItem');
		todoItems.where({
			complete: false
		}).read({
			success: sendResponse
	});

	function sendResponse(results) {
		var tileText = {
			text1: "My todo list"
		};
		var i = 0;
		console.log(results)
		results.forEach(function(item) {
			tileText["text" + (i + 2)] = item.text;
			i++;
		});
		var xml = wns.createTileSquareText01(tileText);
		response.set('content-type', 'application/xml');
		response.send(200, xml);
		}
	};
	````

	This code returns the top 3 uncompleted items from the TodoItem table, then loads them into a JSON object passed to the **wns.createTileSquareText01** function. This function returns the following tile template XML:

	````XML
	<tile>
		<visual>
			<binding template="TileSquareText01">
				<text id="1">My todo list</text>
				<text id="2">Task 1</text>
				<text id="3">Task 2</text>
				<text id="4">Task 3</text>
			</binding>
		</visual>
	</tile>
	````

	The **exports.get** function is used because the client will send a GET request to access the tile template.

	> **Note:** This custom API script uses the [Node.js WNS module](http://go.microsoft.com/fwlink/p/?linkid=306750&clcid=0x409), which is referenced by using the **require** function. This module is different from the [wns object] (http://go.microsoft.com/fwlink/p/?linkid=260591&clcid=0x409) returned by the [push object] (http://msdn.microsoft.com/en-us/library/windowsazure/jj554217.aspx), which is used to send push notifications from server scripts.
To get the **push** object inside a custom API script, you can use **request.services.push**.

1. Click **Save** from the bottom toolbar to save the script.

In the following exercise, you will modify the Windows Store application to start periodic notifications that update the live tile by requesting the new custom API.

<a name="Ex2Task3"></a>
#### Task 3 – Authentication ####

In this task, you will learn how to use authentication in custom APIs.

1. Click the **API** tab of your mobile service.

2. Click the **mycustomapi** API. You will be able to edit the custom script.

3. Replace the **exports.get** method with the following code.

	````Javascript
	exports.get = function (request, response) {
		var result = { userlevel: request.user.level, userName: request.user.userId };
		response.send(200, result)
	};
	````
	
	This code returns the user level and the user ID as a JSON object. The level of authentication, can be one of the following:

	**admin**: the master key was included in the request.

	**anonymous**: a valid authentication token was not provided in the request.

	**authenticated**: a valid authentication token was provided in the request.

	The User ID property returns a value when the user is authenticated. When a user is not authenticated, this property returns **undefined**.
	
	Using the user object you can add logic to allow or restrict access to your users at the API level.

	![Editing the custom API](Images/image-16.png?raw=true)

	_Editing the custom API_


4. Press **CTRL + S** to save the script.

5. Open a browser window and browse to your mobile service API URL. E.g. _https://{your mobile service URL}/api/mycustomapi_. Do not close the browser windows after using it.

	![The custom API GET response that shows the user level](Images/image-17.png?raw=true)

	_The custom API GET response that shows the user level_

	> **Note:** Querying the API from the browser is a request that does not have any authentication header, therefore the userId is an undefined value and it is not returned in the resulting JSON.

6. Go back to the Azure Management Portal, to the _mycustomapi_ page and click the **Permissions** tab.

7. In the **Get Permission** list, select _Anybody with the Application Key_ and click **Save**.

	![Changing the scripts GET permission](Images/image-18.png?raw=true)

	_Changing the script's GET permission_

8. Go back to the browser window that you used to query the mobile service API, and press **CTRL + F5** to refresh.

	![Querying the Mobile Service without credentials is now forbidden](Images/image-19.png?raw=true)

	_Querying the Mobile Service without credentials is now forbidden_

	This shows you another layer where you can enforce authentication, portal level authentication. Your custom APIs will not be executed unless the portal level security is met.

---

<a name="Exercise3"></a>
### Exercise 3: Building a Windows 8 Client using your Custom API ###

In this exercise, you will wire up your Windows Store application to the different APIs that you have created in the previous exercises. 

<a name="Ex3Task1"></a>
#### Task 1 – Invoking the Custom API to add new data ####

In this task, you will call the custom API created in exercise 2 using the Mobile Services SDK to add new items to the _todoitems_ table. 

1. Open **Visual Studio Express 2012 for Windows 8** and open the sample solution you downloaded in the first exercise of this lab.

2. Open the **MainPage.xaml** file.

3. In the second nested **Grid** element, add a new **RowDefinition** element. 

	<!-- mark:9 -->
	````XML
	<Grid Background="White">
		<Grid Margin="50,50,10,10">
			<Grid.ColumnDefinitions>
				<ColumnDefinition Width="*" />
				<ColumnDefinition Width="*" />
			</Grid.ColumnDefinitions>
			<Grid.RowDefinitions>
				<RowDefinition Height="Auto" />
				<RowDefinition Height="Auto" />
				<RowDefinition Height="*" />
			</Grid.RowDefinitions>
			...
		</Grid>
	</Grid>
	````
4. Find the **Grid** element that is located in the Row 1 and Column 1, and add the **RowSpan** attribute with a value of _2_. This is shown in the following code.

	````XML
	<Grid Grid.Row="1" Grid.Column="1" RowSpan="2">
	````

5. Add the following code after the grid that you edited in the previous step. This will insert a new section in the _MainPage_ that will be used for adding a new todo item by invoking the custom API.

	<!-- mark:4-17 -->
	````XML
	<Grid Grid.Row="1" Grid.Column="1" RowSpan="2">
		...
	</Grid>
	<Grid Grid.Row="2">
		<StackPanel>
			<local:QuickStartTask Number="3"
				Title="Insert a TodoItem using the Custom API"
				Description="Enter some text below and click Save to insert a new todo item into your database using the Custom API" />
			<StackPanel Orientation="Horizontal"
					Margin="72,0,0,0">
				<TextBox Name="ApiTextInput"
					Margin="5" MinWidth="300"></TextBox>
				<Button Name="ApiButtonSave"
					Click="ApiButtonSave_Click">Save</Button>
			</StackPanel>
		</StackPanel>
	</Grid>
	````

6. Open the **MainPage.xaml.cs** file and add the following using directive.

	````C#
	using System.Net.Http;
	````

6. Inside the **MainPage** class, add the following event handler.


	(Code Snippet - _MobileServicesCustomAPI_ - _Ex3-ApiButtonSaveClickEventHandler_)
	<!-- mark:4-12 -->
	````C#
	public sealed partial class MainPage : Page
	{
		...
		private async void ApiButtonSave_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(ApiTextInput.Text))
			{
				var parameters = new Dictionary<string, string> { { "text", ApiTextInput.Text } };
				await App.MobileService.InvokeApiAsync("todoitems", HttpMethod.Post, parameters);
				RefreshTodoItems();
			}
		}
	}
	````

	The preceding code verifies that the new todo item is not empty and then it creates a new parameter dictionary using that text. Using the **InvokeApiAsync** method of the **MobileServiceClient** class the custom API is invoked. This method receives as parameters the Custom API name, the HTTP method, and the parameters that will be passed by query string. Finally, the todo items list is refreshed.

	In this case the _HTTP Verb_ is **POST** as new inserts are performed by POST requests as defined when the custom API was created.

7. Press **F5** to run the solution.

8. In _section 3_ of the application, enter a new todo item description and click **Save**.

	![Inserting a new todo item using the Custom API](Images/image-20.png?raw=true)
	
	_Inserting a new todo item using the Custom API_

9. Notice that the item inserted through the API is added to the list. You may need to click the **Refresh** button to see that the item was added.

	![The new item is added to the list](Images/image-21.png?raw=true)
	
	_The new item is added to the list_

	> **Note:** As this custom API is invoked using the Mobile Services SDK, the application key is passed automatically in every request. Therefore this custom API works with the permissions set to **Anyone with the Application Key**.

<a name="Ex3Task2"></a>
#### Task 2 – Using pull notifications to update the application live tile  ####

In this task, you will call the custom API to retrieve live tile updates for your Windows Store application.

1. Continue working in **Visual Studio Express 2012 for Windows 8** with the sample Mobile Services solution.

2. Open the **App.xaml.cs** file.

3. Add the following using statement at the top of the class.

	````C#
	using Windows.UI.Notifications;
	````

4. Locate the **OnLauched** method, and add the following code at the end of it.

	(Code Snippet - _MobileServicesCustomAPI_ - _Ex3-UpdatingLiveTiles_)
	<!-- mark:4-7 -->
	````C#
	protected override void OnLaunched(LaunchActivatedEventArgs args)
	{
		...
		TileUpdateManager.CreateTileUpdaterForApplication().StartPeriodicUpdate(
			new System.Uri(MobileService.ApplicationUri, "/api/notifications"),
			PeriodicUpdateRecurrence.HalfHour
		);
	}
	````
	The preceding code turns on period notifications to request tile template data from the **notifications** custom API. You can select a **PeriodicUpdateRecurrance** value that best matches the update frequency of your data.


5. Press the **F5** key to run the app again. This will turn on periodic notifications.

6. Make sure at least one item is displayed. If there are no items, type text in **Insert a TodoItem**, and then click **Save**.

7. Navigate to the **Start** screen, locate the live tile for the app, and notice that item data is now displayed in the tile.

	![The live tile receiving updates from the custom API](Images/image-22.png?raw=true)
	
	_The live tile receiving updates from the custom API_


	> **Note:** In this case the custom API is used by the **TileUpdateManager** class to obtain tile data. As this request is not done through the custom API, this script needs to have permissions set to **Everyone**, otherwise it will not work.

---

<a name="summary"></a>
## Summary ##

By completing this hands-on lab you have learned how to create Mobile Services custom APIs, access to data, create pull notifications and use authentication. Lastly, you have learned how to wire up mobile services custom APIs to a Windows Store application.
