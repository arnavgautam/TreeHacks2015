<a name="HOLTitle"></a>
# Getting Started with Microsoft Azure Mobile Services and HTML Applications #

---
<a name="Overview"></a>
## Overview ##

Microsoft Azure Mobile Services is a Microsoft Azure service offering designed to make it easy to create highly-functional mobile apps using Microsoft Azure. Mobile Services brings together a set of Microsoft Azure services that enable backend capabilities for your apps. These capabilities includes simple provisioning and management of tables for storing app data, integration with notification services, integration with well-known identity providers for authentication, among others.

The following is a functional representation of the Mobile Services architecture.

![Mobile Services Diagram](Images/mobile-services-diagram.png?raw=true "Mobile Services Diagram")

_Mobile Services Diagram_

This hands-on lab shows you how to add a cloud-based backend service to an HTML app using Microsoft Azure Mobile Services. You will create both a new mobile service and a simple To do list app that stores app data in the new mobile service. Also you will perform server side validations and user authentication using Mobile Services features.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a new Mobile service and use it as the backend storage for an HTML app
- Validate data server-side using Mobile Services Server Scripts feature
- Authenticate users with different identity providers using Mobile Services

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- One of the following Web Servers running on your computer:
	- On **Windows**: [IIS Express](1).
	- On **MacOS X**: Python, which should already be installed.
	- On **Linux**: Python. You must install the latest version of Python.
- A web browser that supports HTML5
- A Microsoft Azure subscription account that has the Microsoft Azure Mobile Services feature enabled

	>**Note:** If you don't have an account, you can create a free trial account in just a couple of minutes. For details, see [Microsoft Azure Free Trial](http://aka.ms/WATK-FreeTrial).
	If you have an existing account but need to enable the Microsoft Azure Mobile Services preview, see [Enable Microsoft Azure preview features](http://www.windowsazure.com/en-us/develop/mobile/tutorials/create-a-windows-azure-account/#enable).

[1]: http://www.microsoft.com/web/gallery/install.aspx?appid=IISExpress

<a name="Setup"/>
### Setup ###

If you are performing this hands-on lab using Windows, you can follow these steps to check for dependencies. Otherwise, check the **Prerequisites** list above.

1. Open a Windows Explorer window and browse to the lab’s **Source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process that will configure your environment.

1. If the User Account Control dialog is shown, confirm the action to proceed.

---
<a name="Exercises"/>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating Your First Mobile Service](#Exercise1)
1. [Validating Data Using Server Scripts](#Exercise2)
1. [Getting Started with Authentication](#Exercise3)

Estimated time to complete this lab: **45** minutes.

<a name="Exercise1" />
## Exercise 1: Creating Your First Mobile Service ##

In this exercise use the quick start within the portal to quickly demonstrate the structured storage capability of Microsoft Azure Mobile services.

A screenshot from the completed app is below:

![completed-app](Images/completed-app.png?raw=true "Completed HTML Application")


<a name="Ex1Task1" />
### Task 1 - Creating a New Mobile Service ###
Follow these steps to create a new mobile service.

1. Log into the [Microsoft Azure Management Portal](https://manage.windowsazure.com) and navigate to Mobile Services.

1. At the bottom of the navigation pane, click **NEW**.

	![New Button](Images/new-button.png?raw=true)

	_New Button_

1. Expand **Compute | Mobile Service**, then click **Create**. This displays the **New Mobile Service** dialog.
 
	![Creating a New Mobile Service](Images/creating-new-mobile-service.png?raw=true)
 
	_Creating a new Mobile Service_	

1. In the **Create a mobile service** page, type a subdomain name for the new mobile service in the **URL** textbox and wait for name verification. Once name verification completes, select **Create a new SQL database instance** and click the right arrow button to go to the next page.
 
	![New Mobile Service](Images/create-mobile-service-step-1.png?raw=true)

	_New Mobile Service_

	This displays the **Specify database settings** page.

	> **Note:** As part of this exercise, you create a new SQL Database instance and server. You can reuse this new database and administer it as you would any other SQL Database instance. If you already have a database in the same region as the new mobile service, you can instead chooseUse existing Databaseand then select that database. The use of a database in a different region is not recommended because of additional bandwidth costs and higher latencies.

1. In **Name**, leave the default database name and select **New SQL database server**. Then type **Login name**, which is the administrator login name for the new SQL Database server, type and confirm the password, and click the check button to complete the process.
 
	![Specifying Database Settings](Images/create-mobile-service-step-2.png?raw=true)

	_Specifying Database Settings_

	> **Note:** When the password that you supply does not meet the minimum requirements or when there is a mismatch, a warning is displayed. We recommend that you make a note of the administrator login name and password that you specify; you will need this information to reuse the SQL Database instance or the server in the future. You have now created a new mobile service that can be used by your mobile apps.

Wait until the mobile servies is ready. You have now created a new mobile service that can be used by your mobile apps.

![Mobile Service Ready](Images/mobile-service-ready.png?raw=true "Mobile Service Ready")

_Mobile Service Ready_

<a name="Ex1Task2" />
### Task 2 - Creating a New HTML App ###

Once you have created your mobile service, you can follow an easy quickstart in the Microsoft Azure Management Portal to either create a new app or modify an existing app to connect to your mobile service.

In this task you will create a new HTML app that is connected to your mobile service.

1. In the Management Portal, click the mobile service that you just created.

2. In the quickstart tab, click **HTML/JavaScript** under **Choose platform** and expand **Create a new HTML app**.

	![New Mobile Service](Images/new-mobile-service.png?raw=true "New Mobile Service")

	_New Mobile Service_

3. Three steps are displayed to create and host an HTML app connected to your mobile service. Click **Create TodoItems table** to create a table to store app data.

	![Creating a New HTML App](Images/creating-a-new-html-app.png?raw=true "Creating a New HTML App")

	_Creating a New HTML App_

4. Under **Download and run application**, click **Download**.

This downloads the web site files for the sample To do list application that is connected to your mobile service. Save the compressed file to your local computer, and make a note of where you save it.

<a name="Ex1Task3" />
### Task 3 - Hosting and Running your HTML App ###

The final stage of this exercise is to host and run your new app on your local computer.

1. Browse to the location where you saved the compressed project files, extract the files on your computer, and launch one of the following command files from the **server** subfolder, depending on your operating system.
	- On Windows: **launch-windows**
	- On Mac OS X: **launch-mac.command**
	- On Linux: **launch-linux.sh**

	> **Note:** On a Windows computer, type `R` when PowerShell asks you to confirm that you want to run the script. Your web browser might warn you to not run the script because it was downloaded from the internet. When this happens, you must request that the browser proceed to load the script.

	This starts a web server on your local computer to host the new app.

1. Open the URL [http://localhost:8000/](http://localhost:8000/) in a web browser to start the app.

	![Running the App](Images/running-the-app.png?raw=true "Running the App")

	_Running the app_

1. In the app, type meaningful text, such as _Complete the hands-on lab_, in **Enter new task**, and then click **Add**.

	![Creating a New Task](Images/creating-a-new-task.png?raw=true "Creating a New Task")

	_Creating a New Task_

	This sends a POST request to the new mobile service hosted in Microsoft Azure. Data from the request is inserted into the TodoItem table. Items stored in the table are returned by the mobile service, and the data is displayed in the second column in the app.

	> **Note:** In the next task you will review the code that accesses your mobile service to query and insert data, which is found in the app.js file.

1. Back in the Management Portal, click the **Data** tab of your mobile service. This lets you browse the data inserted by the app into the table. You can also add additional tables manually by using the **Create** button.

	![Mobile Services Data](Images/mobile-services-data.png?raw=true "Mobile Services Data")

	_Mobile Services Data_

1. Then click the **TodoItems** table, to see the table data. Notice that the TodoItem table now contains data, with id values generated by Mobile Services, and that columns have been automatically added to the table to match the TodoItem class in the app.

	![TodoItems Table Data](Images/todoitems-table-data.png?raw=true "TodoItems Table Data")

	_TodoItems Table Data_

	>**Note:** Mobile Services simplifies the process of storing data in a SQL Database. By default, you don’t need to predefine the schema of tables in your database. Mobile Services automatically adds columns to a table based on the data you insert. To change this dynamic schema behavior, use the Dynamic Schema setting on the Configure tab. It is recommended that you disable dynamic schema support before publicly releasing your app.
	>
	> ![dynamic schema](Images/dynamic-schema.png?raw=true "dynamic schema")

<a name="Ex1Task4" />
### Task 4 - Exploring your App Code ###

In this task you will explore To do list application code and see how simple the Microsoft Azure Mobile Services Client SDK makes it to interact with Microsoft Azure Mobile Services.

1. Open **index.html** using a text editor. The file is located in the folder where you've extracted the application files. The file imports **MobileServices.Web-1.0.0.min.js** from your mobile services web page and the **app.js** file that is located in the same folder of **index.hml**
	
	````HTML
	<script src='//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js'></script>
	<script src='https://todolist.azure-mobile.net/client/MobileServices.Web-1.0.0.min.js'></script>
	<script src='app.js'></script>
	````

1. Open **app.js** using a text editor. Check the MobileServiceClient class. This is the key class provided by the client SDK that provides a way for your application to interact with Microsoft Azure Mobile Services. The first parameter in the constructor is the Mobile Service endpoint and the second parameter is the Application Key for your Mobile Service.

	Additionally, the **getTable()** function creates a proxy object (todoItemTable) for the SQL Database TodoItem.
 
	````JavaScript
	$(function() {
		var client = new WindowsAzure.MobileServiceClient('https://todolist.azure-mobile.net/', '[MOBILE-SERVICES-KEY]'),
		todoItemTable = client.getTable('todoitem');
		...
	}
	````

1. Let's see how the mobile service client is then used for Inserts, Updates, Query operations. The **where()** function creates a query that is executed with the **read()** operation.

	````JavaScript
   function refreshTodoItems() {
		var query = todoItemTable.where({ complete: false });

		query.read().then(function(todoItems) { 
			...
		}
	}
	````

1. The insert is performed using the **Insert** function over the table proxy, receiving a JSON object. 

	````JavaScript
	$('#add-item').submit(function(evt) {
		 var textbox = $('#new-item-text'),
			  itemText = textbox.val();
		 if (itemText !== '') {
			  todoItemTable.insert({ text: itemText, complete: false }).then(refreshTodoItems);
		 }
		 textbox.val('').focus();
		 evt.preventDefault();
	});
	````

	>**Note:** To add additional columns to the table, simply send an insert request including the new properties from your app with dynamic schema enabled. Once a column is created, its data type cannot be changed by Mobile Services. Insert or update operations fail when the type of a property in the JSON object cannot be converted to the type of the equivalent column in the table.

1. And finally, the update is performed using the **Update** function. 

	````JavaScript
    $(document.body).on('change', '.item-text', function() {
        var newText = $(this).val();
        todoItemTable.update({ id: getTodoItemId(this), text: newText });
    });
	````

	> **Note:** Most of the methods to send or request data from Mobile Services are handled asynchronously and expect a callback to be passed in to handle whatever should be executed when the server request is complete.

---

<a name="Exercise2"/>
## Exercise 2: Validating Data Using Server Scripts ##

This exercise shows you how to leverage server scripts in Microsoft Azure Mobile Services. Server scripts are registered in a mobile service and can be used to perform a wide range of operations on data being inserted and updated, including validation and data modification. In this exercise, you will define and register server scripts that validate and modify data. Because the behavior of server side scripts often affects the client, you will also update your HTML app to take advantage of these new behaviors.

This exercise requires that you've completed [Exercise 1](#Exercise1).

>**Note:** For more information on server scripts check this [reference] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/work-with-server-scripts/).


<a name="Ex2Task1" />
###Task 1 - Adding Server-Side Validation###

It is always a good practice to validate the length of data that is submitted by users. In this task, you will register a script that validates the length of string data sent to the mobile service and rejects strings that are too long, in this case longer than 10 characters.

1.	Log into the [Microsoft Azure Management Portal] (https://manage.windowsazure.com/), click **Mobile Services**, and then click your app.

	![Viewing the Mobile Services](Images/viewing-the-mobile-services.png?raw=true "Viewing the Mobile Services")

	_Viewing the Mobile Services_
1.	Click the **Data** tab, then click the **TodoItem** table.
 
	![Opening the Data Tab](Images/opening-the-data-tab.png?raw=true "Opening the Data Tab")
	
	_Opening the Data Tab_

1.	Click **Script**, then select the **Insert** operation.
 
	![Insert Operation Script](Images/insert-operation-script.png?raw=true "Insert Operation Script")

	_Insert Operation Script_

	> **Note:** You can remove a registered script on the **Script** tab by clicking **Clear** and then **Save**.

1.	Replace the existing script with the following function, and then click **Save**. This script checks the length of the **TodoItem.text** property and sends an error response when the length exceeds 10 characters. Otherwise, the **execute** function is called to complete the insert.

	````JavaScript
	function insert(item, user, request) {
		 if (item.text.length > 10) {
			  request.respond(statusCodes.BAD_REQUEST, {
					error: "Text cannot exceed 10 characters"
			  });
		 } else {
			  request.execute();
		 }
	}
	````

	> **Note:** These scripts must call execute or respond to make sure that a response is returned to the client. When a script has a code path in which neither of these functions is invoked, the operation may become unresponsive. Additionally, notice that Mobile Services does not preserve state between script executions. Every time a script executes, a new global context is created in which for the script is executed.

<a name="Ex2Task2" />
###Task 2 - Handling Validation Errors###

Now that the mobile service is validating data and sending error responses, you need to update your app to be able to handle error responses from validation.

1.	Open the file **app.js** with a text editor, then replace the **$('#add-item').submit()** event handler with the following code.

	````JavaScript
	$('#add-item').submit(function(evt) {
		 var textbox = $('#new-item-text'),
			  itemText = textbox.val();
		 if (itemText !== '') {
			  todoItemTable.insert({ text: itemText, complete: false })
					.then(refreshTodoItems, function(error){
					alert(JSON.parse(error.request.responseText).error);
			  });
		 }
		 textbox.val('').focus();
		 evt.preventDefault();
	});
	````

1. Run one of the following command files from the **server** subfolder of the project that you modified when you completed [Exercise 1](#Exercise1). This starts a web server on your local computer to host the app. If the command file is already opened, close it before executing the command again.
	- On Windows: **launch-windows**
	- On Mac OS X: **launch-mac.command**
	- On Linux: **launch-linux.sh**

	> **Note:** On a Windows computer, type `R` when PowerShell asks you to confirm that you want to run the script. Your web browser might warn you to not run the script because it was downloaded from the internet. When this happens, you must request that the browser proceed to load the script.

1.	In a web browser, navigate to [http://localhost:8000/](http://localhost:8000/), then type text in _Add new task_ and click **Add**. Notice that the operation fails and error handling displays the error response in a dialog.

	![Validation Message](Images/validation-message.png?raw=true "Validation Message")
	_Validation Message_

---

<a name="Exercise3"/>
## Exercise 3: Getting Started with Authentication ##

This exercise shows you how to authenticate users in Microsoft Azure Mobile Services from your HTML app. In this exercise, you add authentication to the quickstart project using an identity provider that is supported by Mobile Services. After being successfully authenticated and authorized by Mobile Services, the user ID value is displayed.

This exercise requires that you've completed [Exercise 1](#Exercise1).

<a name="Ex1Task1" />
###Task 1 - Register your app for authentication and configure Mobile Services###

To be able to authenticate users, you must register your app with an identity provider. You must then register the provider-generated client secret with Mobile Services.

1.	Log on to the [Microsoft Azure Management Portal] (https://manage.windowsazure.com/), click **Mobile Services**, and then click your mobile service.

	![Viewing the Mobile Services](Images/viewing-the-mobile-services.png?raw=true "Viewing the Mobile Services")

	_Viewing the Mobile Services_

2.	Click the **Dashboard** tab and make a note of the **Site URL** value. You may need to provide this value to the identity provider when you register your app.

	![Getting the Site URL](Images/getting-the-site-url.png?raw=true "Getting the Site URL")
	
	_Getting the Site URL_

3.	Choose a supported identity provider from the list below and follow the steps to register your app with that provider. Remember to make a note of the client identity and secret values generated by the provider using the links below.
	- [Microsoft Account] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-microsoft-authentication/)
	- [Facebook login](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-facebook-authentication/)
	- [Twitter login] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-twitter-authentication/)
	- [Google login] (http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-google-authentication/)

	> **Note:** The provider-generated secret is an important security credential. Do not share this secret with anyone or distribute it with your app.

4.	Back in the Management Portal, click the **Identity** tab, enter the app identifier and shared secret values obtained from your identity provider, and click **Save**.

	![Identity Settings](Images/identity-settings.png?raw=true "Identity Settings")

	_Identity Settings_

Both your mobile service and your app are now configured to work with your chosen authentication provider.

<a name="Ex1Task2" />
###Task 2 - Restricting Permissions to Authenticated Users###

1.	In the Management Portal, click the **Data** tab, and then click the **TodoItem** table.
 
	![Opening the Table](Images/opening-the-table.png?raw=true "Opening the Table")
	_Opening the Table_

2.	Click the **Permissions** tab, set all permissions to **Only authenticated users**, and then click **Save**. This will ensure that all operations against the **TodoItem** table require an authenticated user.
 
	![Table Permissions](Images/table-permissions.png?raw=true "Table Permissions")

	_Table Permissions_

3.	Run one of the following command files from the **server** subfolder of the project that you modified when you completed [Exercise 1](#Exercise1). This starts a web server on your local computer to host the new app. If the command file is already opened, close it before executing the command again.
	- On Windows: **launch-windows**
	- On Mac OS X: **launch-mac.command**
	- On Linux: **launch-linux.sh**

	> **Note:** On a Windows computer, type `R` when PowerShell asks you to confirm that you want to run the script. Your web browser might warn you to not run the script because it was downloaded from the internet. When this happens, you must request that the browser proceed to load the script.

1. Open the URL [http://localhost:8000/](http://localhost:8000/) in a web browser to start the app. The data fails to load and the _Loading..._ message does not disappear. This happens because the app attempts to access Mobile Services as an unauthenticated user, but the _TodoItem_ table now requires authentication.

1.	Optionally, you can open the script debugger for your web browser and reload the page. Verify that an access denied error occurs.

	![Access is denied Error](Images/access-is-denied-error.png?raw=true "Access is denied Error")

	_Access is denied Error_

Next, you will update the app to allow authentication before requesting resources from the mobile service.

<a name="Ex1Task3" />
###Task 3 - Adding Authentication to the App###

1.	Open the file **index.html** from the HTML app, locate the **H1** element and under it, add the following code snippet.

	````HTML
	<div id="logged-in">
		 You are logged in as <span id="login-name"></span>.
		 <button id="log-out">Log out</button>
	</div>
	<div id="logged-out">
		 You are not logged in.
		 <button>Log in</button>
	</div>
	````

	This enables you to login to Mobile Services from the page.

	>**Note:** Because the login is performed in a popup, you should invoke the login method from a button's click event. Otherwise, many browsers will suppress the login window.

1.	In the **app.js** file, locate the line of code at the very bottom of the file that calls to the **refreshTodoItems** function, and replace it with the following code. 

	````JavaScript
	function refreshAuthDisplay() {
		 var isLoggedIn = client.currentUser !== null;
		 $("#logged-in").toggle(isLoggedIn);
		 $("#logged-out").toggle(!isLoggedIn);


	if (isLoggedIn) {
		 $("#login-name").text(client.currentUser.userId);
		 refreshTodoItems();
		}
	}

	function logIn() {
		 client.login("facebook").then(refreshAuthDisplay, function(error){
			  alert(error);
		 });
	}

	function logOut() {
		 client.logout();
		 refreshAuthDisplay();
		 $('#summary').html('<strong>You must login to access data.</strong>');
	}

	// On page init, fetch the data and set up event handlers
	$(function () {
		 refreshAuthDisplay();
		 $('#summary').html('<strong>You must login to access data.</strong>');
		 $("#logged-out button").click(logIn);
		 $("#logged-in button").click(logOut);
	});
	````

	This creates a set of functions to handle the authentication process. The user is authenticated by using a Facebook login.

	> **Note:** If you are using an identity provider other than Facebook, change the value passed to the login method above to one of the following: _microsoftaccount, facebook, twitter, or google_.

1.	Go back to the browser where your HTML app is running, and refresh the page. When you are successfully logged in, the app should run without errors, and you should be able to query Mobile Services and make updates to data.

	![Logged in with Facebook](Images/logged-in-with-facebook.png?raw=true "Logged in with Facebook")

	_Logged in with Facebook_

	> **Note:** When you use Internet Explorer, you may receive the error after login: _Cannot reach window opener. It may be on a different Internet Explorer zone._ This occurs because the pop-up runs in a different security zone (internet) from localhost (intranet). This only affects apps during development using localhost. As a workaround, open the Security tab of Internet Options, click Local Intranet, click Sites, and disable Automatically detect intranet network. Remember to change this setting back when you are done testing.

