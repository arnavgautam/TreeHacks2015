<a name="Title" />
# Getting Started With Windows Azure Storage #

---
<a name="Overview" />
## Overview ##

In this lab, you will learn the basics of **Windows Azure Storage**, how to create and configure storage accounts and how you can programmatically access the different types of storage services. **Blobs**, **Tables**, and **Queues** are all available as part of the **Windows Azure Storage** account and provide durable storage on the Windows Azure platform. These services are accessible from both inside and outside the Windows Azure platform by using the [Windows Azure Storage Client SDK](http://msdn.microsoft.com/en-us/library/microsoft.windowsazure.storageclient.aspx), or via URI using [REST APIs]  (http://msdn.microsoft.com/en-us/library/dd179355.aspx).

You will learn how the following services work.

![storage-diagram](Images/storage-diagram.png?raw=true)

**Table Storage**

Table Storage is a collection of row-like entities, each of which can contain up to 255 properties. There is no schema that enforces a certain set of values in all the rows within a table, unlike tables in a database; it does not provide a way to represent relationships between data. Windows Azure Storage tables are more like rows within a spreadsheet application such as Excel rather than rows within a database such as SQL Database, in that each row can contain a different number of columns with data types different from other rows in the same table.

**Blob Storage**

Blobs provide a way to store large amounts of unstructured, binary data, such as video, audio, images, etc.  One of the features of blobs is streaming content such as video or audio.

**Queue Storage**

Queues provide storage for passing messages between applications. Messages stored in the queue are limited to a maximum of 8KB in size and are generally stored and retrieved on a first in, first out (FIFO) basis (although FIFO is not guaranteed). Processing messages from a queue is a two-stage process which involves getting the message, and then deleting it after it has been processed.  This pattern allows you to implement guaranteed message delivery by leaving the message in the queue until it has been fully processed.


<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

* Create a Storage Account.
* Learn the different configuration options for Geo-Replication, Monitoring and Logging.
* Access Tables, Blobs and Queues using **Windows Azure SDK 2.2** in a MVC Web Application.

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Microsoft Visual Studio Express 2013 for Web] (http://www.visualstudio.com/en-us/downloads/)
- [Windows Azure Tools for Microsoft Visual Studio 2.2 (or later)] (http://www.microsoft.com/windowsazure/sdk/)
- A Windows Azure subscription
	- Sign up for a [Free Trial](http://aka.ms/watk-freetrial).
	- If you are a Visual Studio Professional, Test Professional, Premium or Ultimate with MSDN or MSDN Platforms subscriber, activate your [MSDN benefit](http://aka.ms/watk-msdn) now to start development and test on Windows Azure.
	- [BizSpark](http://aka.ms/watk-bizspark) members automatically receive the Windows Azure benefit through their Visual Studio Ultimate with MSDN subscriptions.
	- Members of the [Microsoft Partner Network](http://aka.ms/watk-mpn) Cloud Essentials program receive monthly credits of Windows Azure at no charge.

<a name="Setup" />
### Setup ###
In order to execute the exercises in this hands-on lab, you will need to set up your environment.

1. Open a Windows Explorer window and browse to the **Source** folder of this lab.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process, which will configure your environment and install the Visual Studio code snippets for this lab.
1. If the User Account Control dialog box is shown, confirm the action to proceed.

> **Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="UsingCodeSnippets" />
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of this code is provided as Visual Studio Code Snippets, which you can access from within Visual Studio to avoid having to add it manually. 

---
<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1.	[Exercise 1 - Creating a Windows Azure Storage Account](#Exercise1)
1.	[Exercise 2 - Managing a Windows Azure Storage Account](#Exercise2)
1.	[Exercise 3 - Understanding the Windows Azure Storage Abstractions](#Exercise3)
1.	[Exercise 4 - Introducing SAS (Shared Access Signature)](#Exercise4)
1.	[Exercise 5 - Updating SAS to use Stored Access Policies](#Exercise5)

> **Note:** Each exercise is accompanied by a starting solution. These solutions are missing some code sections that are to be completed throughout each exercise and therefore will not necessarily work if you run them directly.
Inside each exercise you will also find an **End** folder with the solution you should obtain after completing the exercises. You can use this solution as a guide if you need additional help working through the exercises.

Estimated time to complete this lab: **60** minutes.

> **Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

<a name="Exercise1" />
### Exercise 1: Creating a Windows Azure Storage Account ###

This exercise describes how to create a storage account in the Windows Azure Management Portal. To store files and data in the storage services in Windows Azure, you must create a storage account in the geographic region where you want to store the data.

> **Note:** A storage account can contain up to 100 TB of blob, table, and queue data. You can create up to five storage accounts for each Windows Azure subscription.

<a name="Ex1Task1" />
#### Task 1 - Creating a Storage Account from Windows Azure Management Portal ####

In this task you will learn how to create a new Storage Account using the Windows Azure Management Portal.

1. Navigate to http://manage.windowsazure.com and sign in using the Microsoft Account associated with your Windows Azure account.

	![logging-azure-portal](Images/logging-azure-portal.png?raw=true)

	_Logging in to the Management Portal_

1. In the menu located at the bottom, select **New | Data Services | Storage | Quick Create** to start creating a new Storage Account. Enter a unique name for the account and select a **Location** from the list. Click **Create Storage Account** to continue.
	![create-storage-account-menu](Images/create-storage-account-menu.png?raw=true)

	_Creating a new Storage Account_

1.  In the **Storage** section, you will see the Storage Account you created with a _Creating_ status. Wait until it changes to _Online_ in order to continue with the following step.

	![storage-account-created](Images/storage-account-created.png?raw=true)

	_Storage Account created_

1. Click on the Storage Account name you created. You will enter the **Dashboard** page, which provides you with information about the status of the account and the service endpoints that can be used within your applications.

	![storage-account-dashboard](Images/storage-account-dashboard.png?raw=true)

	_Displaying the Storage Account Dashboard_

<a name="Exercise2" />
### Exercise 2: Managing a Windows Azure Storage Account ###

In this exercise, you will configure the common settings for your Storage Account. You will manage your **Access Keys**, enabling **Geo-Replication** and configuring **Monitoring and Logging**.

<a name="Ex2Task1" />
#### Task 1 - Enabling Geo-Replication ####

Geo-replication replicates the stored content to a secondary location to enable failover to that location in case of a major disaster in the primary location. The secondary location is in the same region but is hundreds of miles from the primary location. This is the highest level of storage durability, known as geo redundant storage (GRS). Geo-replication is turned on by default.

1.	In the Storage Account page, click the **Configure** tab in the top menu.

	![configure-storage-menu](Images/configure-storage-menu.png?raw=true)

	_Configuring Storage Account_

1.  You can choose to enable or disable it in the **Geo-Replication** section.

	![configuring-storage-georeplication](Images/configuring-storage-georeplication.png?raw=true)

	_Enabling Geo-Replication_

	> **Note:** If you turn off geo-replication, you have locally redundant storage (LRS). For locally redundant storage, account data is replicated three times within the same data center. LRS is offered at discounted rates. Be aware that if you turn off geo-replication and later change your mind, you will incur a one-time data cost to replicate your existing data to the secondary location.

<a name="Ex2Task2" />
#### Task 2 - Configuring Monitoring ####

From the **Monitoring** section, you can monitor your storage accounts in the Windows Azure Management Portal. For each storage service associated with the storage account (Blob, Queue, and Table), you can choose the level of monitoring - minimal or verbose - and specify the appropriate data retention policy.

1. In the **Configure** page, go to the **Monitoring** section.

	![configuring-storage-monitoring](Images/configuring-storage-monitoring.png?raw=true)

	_Configuring Monitoring Options_

1.	To set the monitoring level, select one of the following:

	**Minimal** - Collects metrics such as ingress/egress, availability, latency, and success percentages, which are aggregated for the Blob, Table, and Queue services.

	**Verbose** - In addition to the minimal metrics, this setting collects the same set of metrics for each storage operation in the Windows Azure Storage Service API. Verbose metrics enable closer analysis of issues that occur during application operations.

	**Off** - Turns off monitoring. Existing monitoring data is persisted through the end of the retention period.

	> **Note:** There are cost considerations when selecting monitoring. For more information, see [Storage Analytics and Billing](http://msdn.microsoft.com/en-us/library/windowsazure/hh360997.aspx).

1. To set the data retention policy, in **Retention** (in days), type the number of days that data should be retained from 1-365 days. If there is no retention policy (by entering zero value), it is up to you to delete the monitoring data. 

	> **Note:** It is recommended to set a retention policy based on how long you want to retain storage analytics data for your account so that old and unused analytics data can be deleted by the system at no cost.

1. Once Monitoring is enabled, you can customize the **Dashboard** to choose up to six metrics to plot on the metrics chart. There are nine available metrics for each service. To configure this, go to the **Dashboard** page.

	![storage-dashboard-menu](Images/storage-dashboard-menu.png?raw=true)

1.	In the **Dashboard** page, you will see the default metrics displayed on the chart. To add a different metric, click the **More** button to display the available metrics. Select one from the list.

	![adding-metrics-dashboard](Images/adding-metrics-dashboard.png?raw=true)
	
	_Adding Metrics to the Dashboard_

	> **Note:** You can hide metrics that are plotted on the chart by clearing the check box next to the metric header.

1.	By default, the chart shows trends, displaying only the current value of each metric (the **Relative** option at the top of the chart). To display a Y axis to see absolute values, select **Absolute**.

	![dashboard-absolute-values](Images/dashboard-absolute-values.png?raw=true)

	_Changing Chart values to Absolute_

1.	To change the time range displayed on the chart, select **6 hours** or **24 hours** at the top of the chart.

	![dashboard-time-ranges](Images/dashboard-time-ranges.png?raw=true)

	_Changing Chart Time Ranges_

1.	Click **Monitor** in the top menu. On the **Monitor** page, you can view the full set of metrics for your storage account.

	![storage-monitor-menu](Images/storage-monitor-menu.png?raw=true)

1.	By default, the metrics table displays a subset of the metrics that are available for monitoring. The illustration shows the default Monitor display for a storage account with verbose monitoring configured for all three services. Click the **Add Metrics** button in the bottom menu.

	![add-metrics-menu](Images/add-metrics-menu.png?raw=true)

	_Adding Metrics_

1.	In the dialog box, you can choose from a list of different types of metrics for each service. You can select the metrics you want to display in the **Monitor** table. Click **OK** to continue.

	![Select Metrics to Monitor dialog](Images/select-metrics-to-monitor-dialog.png?raw=true "Select Metrics to Monitor dialog")

	_Select Metrics to Monitor dialog_

1.	The metrics you selected will be displayed in the **Monitor** table.

	![metrics-table](Images/metrics-table.png?raw=true)

1.	You can delete a metric by selecting it and clicking the **Delete Metric** button in the bottom menu.

	![delete-metric-menu](Images/delete-metric-menu.png?raw=true)

	_Deleting a Metric_

<a name="Ex2Task3" />
#### Task 3 - Configuring Logging ####

You can save diagnostic logs for Read Requests, Write Requests, and/or Delete Requests and can set the data retention policy for each of the services. In this task you will configure logging for your storage account.

1. In the **Configure** page, go to the **Logging** section.

1.	For each service (Blob, Table or Queue), you can configure the types of request to log: Read Requests, Write Requests, and Delete Requests. You can also configure the number of days to retain the logged data. Enter zero if you do not want to set a retention policy. If you do not set a retention policy, it is up to you to delete the logs.

	![configuring-storage-logging](Images/configuring-storage-logging.png?raw=true)

	_Configuring Logging Options_

	> **Note:** The diagnostic logs are saved in a blob container named **$logs** in your storage account. For information about accessing the $logs container, see [About Storage Analytics Logging](http://msdn.microsoft.com/en-us/library/windowsazure/hh343262.aspx).


<a name="Ex2Task4" />
#### Task 4 - Managing Account Keys ####

When you create a storage account, Windows Azure generates two 512-bit storage access keys which are used for authentication when the storage account is accessed. By providing two storage access keys, Windows Azure enables you to regenerate the keys with no interruption to your storage service.

1.	In the Storage Account Dashboard, select the option **Manage Access Keys** in the bottom menu.

	![manage-keys-menu](Images/manage-keys-menu.png?raw=true)

	_Managing Access Keys_

1. You can use **Manage Keys** to copy a storage access key to use in a connection string. The connection string requires the storage account name and a key to use in authentication. Take note of the Primary access key and the storage account name which will be used in the following exercise.

	![managing-access-keys](Images/managing-access-keys.png?raw=true)

	_Copying Access Keys_

1.	By clicking the **Regenerate** button, a new Access Key is created. You should change the access keys to your storage account periodically to help keep your storage connections more secure. Two access keys are assigned to enable you to maintain connections to the storage account using one access key while you regenerate the other access key. 

	> **Note:** Regenerating your access keys affects virtual machines, media services, and any applications that are dependent on the storage account.

	In the next exercise, you will consume Windows Azure Storage services from an MVC application.

<a name="Exercise3"></a>
###Exercise 3: Understanding the Windows Azure Storage Abstractions ###

This sample application is comprised of five Views, one for each CRUD operation (Create, Read, Update, Delete) and one to list all the entities from the Table Storage. In this exercise, you will update the MVC application actions to perform operations against each storage service (Table, Blob and Queue) using **Windows Azure SDK v2.2**. You will also learn how to use the new **async** methods built into the SDK.

<a name="Ex3Task1" />
#### Task 1 - Configuring Storage Account in the Cloud Project ####

In this task you will configure the _StorageConnectionString_ of the application with the storage account you previously created using the _Access Key_ from the previous exercise.

1. Open **Visual Studio Express 2013 for Web** as Administrator.

1. In the Start Page, click **Open Project...**, then browse to the **Source\Ex3-UnderstandingStorageAbstractions\Begin\** folder of this lab and open the **Begin.sln** solution. Make sure to set the **PhotoUploader** cloud project as the default project.

1. Go to the **PhotoUploader_WebRole** located in the **Roles** folder of the **PhotoUploader** solution. Right-click it and select **Properties**.

	![Web Role Properties](Images/webrol-properties.png?raw=true "WebRol Properties")

	_Web Role Properties_

1. Go to the **Settings** tab and locate the _StorageConnectionString_ setting. Click the ellipsis next to the _UseDevelopmentStorage=true_ value.

	![Settings Tab](Images/settings-tab.png?raw=true "Settings Tab")

	_Settings Tab_

1. Select **Manually Entered Credentials** and set the **Account name** and **Account key** values from the previous exercise. 

	![Create Storage Connection String Dialog Box](Images/create-storage-connection-string-dialog.png?raw=true "Create Storage Connection String Dialog Box")

	_Create Storage Connection String Dialog Box_

1. Click **OK** to update the connection string.

1. Repeat the previous steps to configure the _StorageConnectionString_ for the **QueueProcessor_WorkerRole**.

<a name="Ex3Task2" />
#### Task 2 - Working with Table Storage ####

In this task you will update the MVC application actions to perform operations against the Table Storage. You are going to use Table Storage to save information about the uploaded photo such as its title and description.

1. Open **PhotoEntity.cs** file in the **Models** folder and add the following directives.

	````C#
	using Microsoft.WindowsAzure.Storage.Table;
	````

1. Update the **PhotoEntity** class to inherit from **TableEntity**. TableEntity has **PartitionKey** and **RowKey** properties that need to be set when adding a new row to Table Storage. To do this, add the following _constructors_ and inherit the class from **TableEntity**.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-InheritingTableEntity_)
	<!-- mark:1-11 -->
	````C#
	public class PhotoEntity : TableEntity
	{
		public PhotoEntity()
		{
		}

		public PhotoEntity(string partitionKey)
		{
			PartitionKey = partitionKey;
			RowKey = Guid.NewGuid().ToString();
		}

		...
	}
	````

1. Now you will add a new class to implement a **TableServiceContext** to interact with Table Storage. Right-click the **Models** folder and select **Add** | **Class**.

	![Add new class](Images/add-new-class.png?raw=true "Add new class")

	_Adding a new class_

1. In the **Add New Item** window, set the name of the class to **PhotoDataServiceContext.cs** and click **Add**. 

	![PhotoDataServiceContext class](Images/photodataservicecontext-class.png?raw=true "PhotoDataServiceContext class")

	_PhotoDataServiceContext class_

1. Add the following directives to the **PhotoDataServiceContext** class.

	````C#
	using System.Threading.Tasks;
	using Microsoft.WindowsAzure.Storage.Table;
	using Microsoft.WindowsAzure.Storage.Table.DataServices;
	````

1. Replace the class content with the following code: 

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-PhotoDataServiceContext_)

	````C#
	public class PhotoDataServiceContext : TableServiceContext
	{
		public PhotoDataServiceContext(CloudTableClient client)
			: base(client)
		{
		}

		public IEnumerable<PhotoEntity> GetPhotos()
		{
			return this.CreateQuery<PhotoEntity>("Photos");			
		}
	}
	````
	
	>**Note**: You need to make the class inherit from **TableServiceContext** to interact with Table Storage.

1. Now, you will add an _async_ method to retrieve a single entity from the table. Add the following code to the **PhotoDataServiceContext** class:

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-DataContextGetByIdAsync_)

	````C#
	public async Task<PhotoEntity> GetByIdAsync(string partitionKey, string rowKey)
	{
		var table = this.ServiceClient.GetTableReference("Photos");
		var operation = TableOperation.Retrieve<PhotoEntity>(partitionKey, rowKey);

		var retrievedResult = await table.ExecuteAsync(operation);
		return (PhotoEntity)retrievedResult.Result;
	}
	````

	>**Note**: The previous code uses a **TableOperation** to retrieve the photo with the specific **RowKey**. This method returns just one entity, rather than a collection, and the returned value in **TableResult.Result** is a **PhotoEntity**. The **ExecuteAsync** method implements the async programming pattern that leverages asynchronous support using the _async_ and _await_ keywords in the .NET Framework 4.5.

1.	In order to add a new entity, you can use the **Insert** table operation. Add the following code to implement it:

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-DataContextAddPhotoAsync_)

	````C#
	public async Task AddPhotoAsync(PhotoEntity photo)
	{
		var table = this.ServiceClient.GetTableReference("Photos");
		var operation = TableOperation.Insert(photo);
		await table.ExecuteAsync(operation);
	}
	````

	>**Note**: To prepare the insert operation, a **TableOperation** is created to insert the photo entity. The operation is then executed asynchronously by calling the **CloudTable.ExecuteAsync** method.

1. **Update** operations are similar to insert operations, but first we need to retrieve the entity and then use a **Replace** table operation. We will receive the retrieved entity in the _entityToUpdate_ parameter. Add the following code:

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-DataContextUpdatePhotoAsync_)

	````C#
	public async Task UpdatePhotoAsync(PhotoEntity entityToUpdate)
	{
		var table = this.ServiceClient.GetTableReference("Photos");
		var operation = TableOperation.Replace(entityToUpdate);
		await table.ExecuteAsync(operation);
	}
	````

1. To delete an entity, we need to first retrieve it from the table and then execute a **Delete** table operation. We will receive the retrieved entity in the _entityToDelete_ parameter. Add the following code to implement it:

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-DataContextDeletePhotoAsync_)

	````C#
	public async Task DeletePhotoAsync(PhotoEntity entityToDelete)
	{
		var table = this.ServiceClient.GetTableReference("Photos");
		var deleteOperation = TableOperation.Delete(entityToDelete);
		await table.ExecuteAsync(deleteOperation);
	}
	````

1. Now open the **HomeController.cs** file in the **Controllers** folder. We'll update the controller's actions to execute the table operations with the DataContext you just created. First, add the following _using_ directives.

	````C#
	using System.Threading.Tasks;
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.Storage;
	using Microsoft.WindowsAzure.Storage.Table;
	````

1. Add a private field to create a _StorageAccount_ object. This object will be used to perform operations for each storage service.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-StorageAccountVariable_)

	<!-- mark:3 -->
	````C#
	public class HomeController : Controller
	{
		private CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

		...
	}
	````

1.	In order to display the entities in the View, you will convert them to a **ViewModel** class. You are going to add two helper methods to convert from a **ViewModel** to a **Model** and viceversa. Add the following methods at the end of the class declaration.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-ViewModelHelpers_)

	````C#
	private PhotoViewModel ToViewModel(PhotoEntity photo)
	{
		return new PhotoViewModel
		{
			 PartitionKey = photo.PartitionKey,
			 RowKey = photo.RowKey,
			 Title = photo.Title,
			 Description = photo.Description                
		};
	}

	private PhotoEntity FromViewModel(PhotoViewModel photoViewModel)
	{
		var photo = new PhotoEntity(this.User.Identity.Name);
		photo.RowKey = photoViewModel.RowKey ?? photo.RowKey;
		photo.Title = photoViewModel.Title;
		photo.Description = photoViewModel.Description;
		return photo;
	}
	````

1.	You will be using a **PhotoDataServiceContext** to interact with Table Storage, so add a new **GetPhotoContext** private method at the bottom of the class to create the context.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-TableStorageGetPhotoContext_)

	````C#
	private PhotoDataServiceContext GetPhotoContext()
	{
		var cloudTableClient = this.StorageAccount.CreateCloudTableClient();
		var photoContext = new PhotoDataServiceContext(cloudTableClient);
		return photoContext;
	}
	````

1. The **Home** page will display a list of entities from Table Storage. For it to do this, replace the **Index** action to retrieve the entire list of entities from the Table Storage using the **PhotoDataServiceContext** with the following code.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-TableStorageIndex_)

	````C#
	public ActionResult Index()
	{
		var photoContext = this.GetPhotoContext();
		var photos = photoContext.GetPhotos();
		var photosViewModels = photos.Select(this.ToViewModel).ToList();
		return this.View(photosViewModels);
	}
	````

	>**Note**: You use **Select** and the **ToViewModel** methods to convert every photo **Model** to a new **ViewModel**.

1.	The **Details** view will show specific information about a particular photo. Replace the **Details** action with the following code to display the information of a single entity using the **PhotoDataServiceContext**.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-TableStorageDetails_)

	````C#
	public async Task<ActionResult> Details(string partitionKey, string rowKey)
	{
		var photoContext = this.GetPhotoContext();
		var photo = await photoContext.GetByIdAsync(partitionKey, rowKey);

		if (photo == null)
		{
			return HttpNotFound();
		}

		var viewModel = this.ToViewModel(photo);
		return this.View(viewModel);
	}
	````

1.	Replace the **Create** _POST_ action with the following code to insert a new entity in the Table Storage.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-TableStorageCreate_)

	````C#
	[HttpPost]
	public async Task<ActionResult> Create(PhotoViewModel photoViewModel, HttpPostedFileBase file, FormCollection collection)
	{
		if (!this.ModelState.IsValid)
		{
			return this.View();
		}

		var photo = this.FromViewModel(photoViewModel);
		// Save information to Table Storage
		var photoContext = this.GetPhotoContext();
		await photoContext.AddPhotoAsync(photo);

		return this.RedirectToAction("Index");
	}
	````

1.	Replace the **Edit** _GET_ Action with the following code to retrieve existing entity information from Table Storage.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-TableStorageEdit_)

	````C#
	public async Task<ActionResult> Edit(string partitionKey, string rowKey)
	{
		var photoContext = this.GetPhotoContext();
		var photo = await photoContext.GetByIdAsync(partitionKey, rowKey);

		if (photo == null)
		{
			return this.HttpNotFound();
		}

		var viewModel = this.ToViewModel(photo);
		return this.View(viewModel);
	}
	````

1.	Replace the **Edit** _POST_ action with the following code to update an existing entity in the table storage.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-TableStoragePostEdit_)

	````C#
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> Edit(PhotoViewModel photoViewModel, FormCollection collection)
	{
		if (!ModelState.IsValid)
		{
			return this.View();
		}

		var photoContext = this.GetPhotoContext();
		var entityToUpdate = await photoContext.GetByIdAsync(photoViewModel.PartitionKey, photoViewModel.RowKey);

		if (entityToUpdate == null)
		{
			return this.HttpNotFound();
		}

		// Update entity information from ViewModel
		entityToUpdate.Title = photoViewModel.Title;
		entityToUpdate.Description = photoViewModel.Description;
		await photoContext.UpdatePhotoAsync(entityToUpdate);

		return this.RedirectToAction("Index");
	}
	````

	>**Note**: Remember that you need to retrieve the entity from Table Storage first to perform an update.

1.	Replace the **Delete** _GET_ Action with the following code to retrieve existing entity data from Table Storage.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-TableStorageDelete_)

	````C#
	public async Task<ActionResult> Delete(string partitionKey, string rowKey)
	{
		var photoContext = this.GetPhotoContext();
		var photo = await photoContext.GetByIdAsync(partitionKey, rowKey);

		if (photo == null)
		{
			return this.HttpNotFound();
		}

		var viewModel = this.ToViewModel(photo);
		return this.View(viewModel);
	}
	````

1.	Replace the **DeleteConfirmed** action with the following code to delete an existing entity from the table.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-TableStorageDeleteConfirmed_)
	
	````C#
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteConfirmed(string partitionKey, string rowKey)
	{
		var photoContext = this.GetPhotoContext();
		var photo = await photoContext.GetByIdAsync(partitionKey, rowKey);

		if (photo == null)
		{
			return this.HttpNotFound();
		}

		await photoContext.DeletePhotoAsync(photo);
		return this.RedirectToAction("Index");
	}
	````

1. In order to be able to work with Table Storage, we first need to have the table created. Data tables should only be created once. Typically, you would do this during a provisioning step and rarely in application code. The **Application_Start** method in the **Global.asax** class is recommended for this initialization logic. To complete the step, open **Global.asax.cs** and add the following using directives.

	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.Storage;
	using Microsoft.WindowsAzure.Storage.Table;
	````

1. Add the following code at the end of the **Application_Start** method to ensure that whenever you start the application, the _Photos_ table is created.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-TableStorageAppStart_)

	<!-- mark:8-11 -->
	````C#
	protected void Application_Start()
	{
		AreaRegistration.RegisterAllAreas();
		FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
		RouteConfig.RegisterRoutes(RouteTable.Routes);
		BundleConfig.RegisterBundles(BundleTable.Bundles);

		var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
		var cloudTableClient = storageAccount.CreateCloudTableClient();
		var table = cloudTableClient.GetTableReference("Photos");
		table.CreateIfNotExists();
	}
	````

1.	Press **F5** and run the application.

	![Index Home Page](Images/index-home-page.png?raw=true "Index Home Page")

	_Index Home Page_

1. You will be presented with a login page. You will first have to click **"Register"** to create a user.

	![Register a new user](Images/register-a-new-user.png?raw=true "Register a new user")

	_Register a new user_

1. Click the **Create** link to create a new entity.

1. Complete the **Title** and **Description** fields and click **Create** to submit the form.

	![Create Image Form](Images/create-image-form.png?raw=true "Create Image Form")

	_Create Image Form_

	> **Note**: You can leave the **Image** file input field empty in this exercise.

1. Close the browser to stop the application.

<a name="Ex3Task3" />
#### Task 3 - Working with Blobs ####

In this task you will configure the MVC application to upload images to Blob Storage. 

1.	Open **HomeController.cs** and add the following directives to work with Blobs.

	````C#
	using Microsoft.WindowsAzure.Storage.Blob;
	````

1. Add the following helper method at the end of the class. It will allow you to retrieve the blob container from the storage account that will be used to store the images.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-BlobStorageGetBlobContainer_)
	
	````C#
	private CloudBlobContainer GetBlobContainer()
	{
		var cloudBlobClient = this.StorageAccount.CreateCloudBlobClient();
		var container = cloudBlobClient.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName"));
		return container;
	}
	````

1.	Now, you will update the **Create** action of the **HomeController** to upload an image to a blob. You will save the blob reference name in the table to reference it in the future. To do this, add the following code in the **Create** _POST_ action method.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-BlobCreate_)

	<!-- mark:7-19 -->
	````C#
	[HttpPost]
	public async Task<ActionResult> Create(PhotoViewModel photoViewModel, HttpPostedFileBase file, FormCollection collection)
	{
		...
		var photo = this.FromViewModel(photoViewModel);

		if (file != null)
		{
			// Save file stream to Blob Storage
			var blob = this.GetBlobContainer().GetBlockBlobReference(file.FileName);
			blob.Properties.ContentType = file.ContentType;
			await blob.UploadFromStreamAsync(file.InputStream);
			photo.BlobReference = file.FileName;
		}
		else
		{
			this.ModelState.AddModelError("File", new ArgumentNullException("file"));
			return this.View(photoViewModel);
		}
		...
	}
	````

1.	In the **Details** view, you will need to display the image that was stored in the blob container. To do this, you need to update the **Details** action to retrieve the URL using the **Blob Reference** name that was saved when creating a new entity. Add the highlighted code.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-BlobDetails_)

	<!-- mark:6-9 -->
	````C#
	public async Task<ActionResult> Details(string partitionKey, string rowKey)
	{
		...

		var viewModel = this.ToViewModel(photo);
		if (!string.IsNullOrEmpty(photo.BlobReference))
		{
			viewModel.Uri = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference).Uri.ToString();
		}

		return this.View(viewModel);
	}	
	````

1.	Add the same line of code for the **Edit** _GET_ action to get the image when editing.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-BlobEdit_)
	<!-- mark:6-9 -->
	````C#
	public async Task<ActionResult> Edit(string partitionKey, string rowKey)
	{
		...

		var viewModel = this.ToViewModel(photo);
		if (!string.IsNullOrEmpty(photo.BlobReference))
		{
			viewModel.Uri = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference).Uri.ToString();
		}

		return this.View(viewModel);
	}	
	````

1. Add the same for the **Delete** _GET_ Action to get the image when deleting.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-BlobDelete_)

	<!-- mark:6-9 -->
	````C#
	public async Task<ActionResult> Delete(string partitionKey, string rowKey)
	{
		...

		var viewModel = this.ToViewModel(photo);
		if (!string.IsNullOrEmpty(photo.BlobReference))
		{
			viewModel.Uri = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference).Uri.ToString();
		}

		return this.View(viewModel);
	}	
	````
	
1.	To delete the blob from the container, you will use the blob reference name to retrieve the container and perform a _delete_ operation. To do this, add the following code to the **DeleteConfirmed** action.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-BlobPostDelete_)

	<!-- mark:7-13 -->
	````C#
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteConfirmed(string partitionKey, string rowKey)
	{
		...

		//Deletes the Image from Blob Storage
		if (!string.IsNullOrEmpty(photo.BlobReference))
		{
			var blob = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference);
			await blob.DeleteIfExistsAsync();
		}

		return this.RedirectToAction("Index");
	}
	````

1. As you did with the _Photos_ table in the previous task, you also need to initialize the **Blob** container at application start. Open **Global.asax.cs** and add the following using directive.

	````C#
	using Microsoft.WindowsAzure.Storage.Blob;
	````

1. Add the following code at the end of the **Application_Start** method to ensure the blob container is created.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-BlobStorageAppStart_)

	<!-- mark:5-10 -->
	````C#
	protected void Application_Start()
	{
		...
		table.CreateIfNotExists();
		var cloudBlobClient = storageAccount.CreateCloudBlobClient();
		var container = cloudBlobClient.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName"));
		if (container.CreateIfNotExists())
		{
			container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
		}
	}
	````

1.	Press **F5** to run the application, and then log in if you are not.

1. Browse for an image, insert a title and a description for it and then click **Create** to perform the upload.

	![Upload image](Images/upload-image.png?raw=true "Upload image")

	_Upload image_

	> **Note:** You can use one of the images included in this lab in the **Assets** folder.

1. Go to the **Details** page to check that the image uploaded successfully and then close the browser.

<a name="Ex3Task4" />
#### Task 4 - Working with Queues ####

In this task, you will use queues to simulate a notification service, where a message is sent to a worker role for processing.

1.	Open **HomeController.cs** and add the following directive.

	````C#
	using Microsoft.WindowsAzure.Storage.Queue;
	````

1.	You will add the following helper method at the end of the class to retrieve the **Cloud Queue** object.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-QueueHelper_)

	````C#
	private CloudQueue GetCloudQueue()
	{
		var queueClient = this.StorageAccount.CreateCloudQueueClient();
		var queue = queueClient.GetQueueReference("messagequeue");
		return queue;
	}
	````

1.	To send notification that a new photo has been uploaded, you must insert a message to the **Queue** with the specific text to be displayed. Add the following highlighted code in the **Create** _POST_ action method.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-QueueSendMessageCreate_)

	<!-- mark:8-10 -->
	````C#
	[HttpPost]
	public async Task<ActionResult> Create(PhotoViewModel photoViewModel, HttpPostedFileBase file, FormCollection collection)
	{
		...

		await photoContext.AddPhotoAsync(photo);

		// Send create notification
		var msg = new CloudQueueMessage("Photo Uploaded");
		await this.GetCloudQueue().AddMessageAsync(msg);

		return this.RedirectToAction("Index");
	}	
	````

1.	To send notification that a photo was deleted, you must insert a message to the **Queue** with the specific text to be displayed. Add the following highlighted code to the **DeleteConfirmed** action method.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-QueueSendMessageDelete_)

	<!-- mark:7-9 -->
	````C#
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> DeleteConfirmed(string partitionKey, string rowKey)
	{
		...

		// Send delete notification
		var msg = new CloudQueueMessage("Photo Deleted");
		await this.GetCloudQueue().AddMessageAsync(msg);

		return this.RedirectToAction("Index");
	}
	````

1. As you did with **Table** and **Blob**, you need to create the Queue at application start if it doesn't exist. Open **Global.asax.cs** and add the following using directive.

	````C#
	using Microsoft.WindowsAzure.Storage.Blob;
	````

1. Add the following code at the end of the **Application_Start** method to ensure the queue is created.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-QueueAppStart_)

	<!-- mark:8-10 -->
	````C#
	protected void Application_Start()
	{
		...
		if (container.CreateIfNotExists())
		{
			container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
		}
		var cloudQueueClient = storageAccount.CreateCloudQueueClient();
		var queue = cloudQueueClient.GetQueueReference("messagequeue");
		queue.CreateIfNotExists();
	}
	````

1. Open the **WorkerRole.cs** file located in the **QueueProcessor_WorkerRole** project.

1. The worker role will read the **Queue** for notification messages. First, you need to get a queue reference. To do this, add the following highlighted code in the **Run** method.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex3-QueueWorkerAccount_)
	<!-- mark:6-11 -->
	````C#
	public override void Run()
	{
		// This is a sample worker implementation. Replace with your logic.
		Trace.TraceInformation("QueueProcessor_WorkerRole entry point called", "Information");

		// Initialize the account information
		var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
		
		// retrieve a reference to the messages queue
		var queueClient = storageAccount.CreateCloudQueueClient();
		var queue = queueClient.GetQueueReference("messagequeue");

		while (true)
		{
			Thread.Sleep(10000);
			Trace.TraceInformation("Working", "Information");			
		}
	}
	````

1. Now, add the following code inside the **while** block to read messages from the queue.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex3-QueueReadingMessages_)

	<!-- mark:9-18 -->
	````C#
	public override void Run()
	{		
		...

		while (true)
		{
			Thread.Sleep(10000);
			Trace.TraceInformation("Working", "Information");		
			if (queue.Exists())
			{
				var msg = queue.GetMessage();

				if (msg != null)
				{
					Trace.TraceInformation(string.Format("Message '{0}' processed.", msg.AsString));
					queue.DeleteMessage(msg);
				}
			}	
		}
	}
	````

	> **Note:** The worker process will try to get a message from the queue every 10 seconds using the **GetMessage** method. If there are messages in the queue, they will be shown them in the Compute Emulator log.

1. Press **F5** to run the application, and log in if you have not already. Once the browser has opened, upload a new image.

1. Open the **Compute Emulator**. To do so, right-click the Windows Azure icon tray and select **Show Compute Emulator UI**.

	![Windows Azure Tray Icon](Images/windows-azure-tray-icon.png?raw=true "Windows Azure Tray Icon")

	_Windows Azure Tray Icon_

1. Select the worker role instance. Wait until the process reads the message from the queue.

	![Worker role processing the queue](Images/worker-role-processing-the-queue.png?raw=true "Worker role processing the queue")

	_Worker role processing the queue_

<a name="Ex3Task5" />
#### Task 5 - Verification with Visual Studio####

In this task, you will use Visual Studio to inspect the Windows Azure Storage Account.

1. If not already opened, open **Visual Studio Express 2013 for Web**.

1. Go to the **View** menu, and open **Server Explorer**.

1. In the Database Explorer pane, right-click **Windows Azure** and select **Connect to Windows Azure**.

	![Server Explorer](Images/server-explorer.png?raw=true "Server Explorer")

	_Server Explorer_

1. Enter your **Live ID credentials** associated with the Storage Account you are using and sign in to Windows Azure.

1. Expand the **Storage** drop-down and within it your account name. Notice that there is an entry for Tables, Blobs and Queues.

1. Expand the **Tables** container. You will see the **Photos** table under it.

	![Photos Table in Database Explorer](Images/photos-table-in-database-explorer.png?raw=true "Photos Table in Database Explorer")

	_Photos Table in Database Explorer_

1. Double-click the **Photos** table or right-click it and select **View Table**.

	![Photos Table](Images/photo-table.png?raw=true "Photos Table")

	_Photos Table_

	> **Note**: Here you can see the data you created in the previous task. Notice the blob reference column. This column stores the name of the image saved in the Blob Storage.

1. Expand the **Blobs** container. Double-click the **gallery** blob or right-click it and select **View Blob Container**. 

	![Gallery Blob Container](Images/gallery-blob-container.png?raw=true "Gallery Blob Container")

	_Gallery Blob Container_


<a name="Exercise4" />
### Exercise 4: Introducing SAS (Shared Access Signature) ###

Shared Access Signatures allows granular access to tables, queues, blob containers, and blobs. A SAS token can be configured to allow executing only specific operations on a given storage, and for a certain amount of time or without any limit. Allowed operations could be many of: read, write, delete or list. And the storage could be a specific table, key range within a table, queue, blob or blob container. The SAS token appears as part of the resource's URI as a series of query parameters.

In this exercise you will learn how to use Shared Access Signatures with the three storage abstractions: Tables, Blobs, and Queues.

>**Note:** This sample application does not follow all best practices for using Shared Access Signature for simplicity's sake. In a production environment you will typically have a service that generates the SAS for your application.

<a name="Ex4Task1" />
#### Task 1 - Adding SAS at Table level  ####

In this task you will learn how to create SAS for Windows Azure Tables. You as the account owner can create SAS when you want to grant access to your storage without sharing your account key, and also restricting the operations in several ways.

You can grant access to an entire table, to a table range (for example, to all the rows under a particular partition key), or to some specific rows. Additionally, you can grant execution rights only to specific methods such as _Query_, _Add_, _Update_, _Delete_. Finally, you can specify the SAS token start and expiry time.

1. Continue working with the end solution of the previous exercise or open the solution located at _Source/Ex04-IntroducingSAS/Begin_. Remember to run Visual Studio **as administrator** to be able to use the Windows Azure Storage Emulator.

1. Press **F5** to run the application, and register a new user. Notice that you are able see what you have uploaded with the previous user.

	![Index without SAS](Images/index-without-sas.png?raw=true "Index without SAS")

	_Index without SAS_

1. In the next steps you will use **Shared Access Signature** to restrict the information your users will be able to see.

1. First, create a folder named **Services** in the root of your project. Right-click **PhotoUploader_WebRole** project, and under **Add** click **New Folder**. Type **Services** and click _Enter_.

1. Right-click the **Services** folder and select **Add** | **Class**.

1. In the **Add New Item** window, set the name of the class to **SasService.cs** and click **Add**.

1. Add the following _using_ statements at the top of the file.

	<!-- mark:1-5 -->
	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.Storage;
	using Microsoft.WindowsAzure.Storage.Auth;
	using Microsoft.WindowsAzure.Storage.Table;
	````

1. Add the **StorageAccount** field to the class and initialize it with your **StorageConnectionString**.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex4-SasServiceStorageAccount_)

	<!-- mark:3 -->
	````C#
	public class SasService
	{
		private static CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
	}
	````

1. Create a new method called **GetSasForTable**.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex4-SasServiceGetSasForTable_)

	<!-- mark:5-23 -->
	````C#
	public class SasService
	{
		...

		public static string GetSasForTable(string username)
		{
			var cloudTableClient = StorageAccount.CreateCloudTableClient();
			var policy = new SharedAccessTablePolicy()
			{
				SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
				Permissions = SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Delete | SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Update
			};

			var sasToken = cloudTableClient.GetTableReference("Photos").GetSharedAccessSignature(
				policy: policy,
				accessPolicyIdentifier: null,
				startPartitionKey: username,
				startRowKey: null,
				endPartitionKey: username,
				endRowKey: null);

			return sasToken;
		}
	}
	````

	> **Note**: This method takes the **username** passed as argument and creates a SAS for the _Photos_ table. This SAS will grant the specified permissions only to the rows with a partition equal to "**username**". Finally, it returns the SAS in string format.

1. Open the **HomeController.cs** file under the _Controllers_ folder, and add the following field.

	(Code Snippet - _GettingStartedWindowsAzureStorage - Ex4-HomeControllerUriTable_)

	<!-- mark:4 -->
	````C#
	public class HomeController : Controller
	{
		private CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
		private Uri uriTable = new Uri("http://127.0.0.1:10002/devstoreaccount1");

		...
	}
       
	````

	>**Note**: Replace _http://127.0.0.1:10002/devstoreaccount1_ with your storage account table URI in order to work against Windows Azure.

1. Add the following _using_ statements to the **HomeController.cs** file.

	````C#
	using Microsoft.WindowsAzure.Storage.Auth;
	using PhotoUploader_WebRole.Services;
	````

1. Scroll down to the **GetPhotoContext** method and update the **cloudTableClient** creation with the following code. 

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex4-NewCloudTableClientCall_)

	<!-- mark:3-4 -->
	````C#
	private PhotoDataServiceContext GetPhotoContext()
	{
		var sasToken = SasService.GetSasForTable(this.User.Identity.Name);
		var cloudTableClient = new CloudTableClient(this.uriTable, new StorageCredentials(sasToken));
		var photoContext = new PhotoDataServiceContext(cloudTableClient);
		return photoContext;
	}
	````

1. Run the solution by pressing **F5**.

1. Log in with the new user you created at the beginning of this task, and you will notice that you no longer see the other user's photos.

	![Index with SAS](Images/index-with-sas.png?raw=true "Index with SAS")

	_Index with SAS_

1. Add a **new entry** to check that everything is working as expected.

1. You can also log in with **the other user** to see that your photos are still there, but not the newest entry.

<a name="Ex4Task2" />
#### Task 2 - Adding SAS at Blob level  ####

In this task you will learn how to create SAS for Azure Blobs. SAS tokens can be used on blobs to read, update and delete the specified blob, and in blob containers to list its contents, create, read, update and delete blobs in it.

1. Open the **SasService.cs** file and add the following _using_ statements.
	
	````C#
	using Microsoft.WindowsAzure.Storage.Blob;
	using System.Globalization;
	````

1. Create a new method called **GetReadonlyUriWithSasForBlob** with the following code.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex4-GetReadonlyUriWithSasForBlobMethod_)

	````C#
	public static string GetReadonlyUriWithSasForBlob(string blobName)
	{
		var cloudBlobClient = StorageAccount.CreateCloudBlobClient();
		var blob = cloudBlobClient.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName")).GetBlockBlobReference(blobName);
		var sas = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
		{
			Permissions = SharedAccessBlobPermissions.Read,
			SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
			SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(2),
		});

		return string.Format(CultureInfo.InvariantCulture, "{0}{1}", blob.Uri, sas);
	}
	````

1. Add another method called **GetSasForBlobContainer** with the following code

	(Code Snippet - _GetSasForBlobContainer_ - _Ex4-GetSasForBlobContainerMethod_)

	````C#
	public static string GetSasForBlobContainer()
	{
		var cloudBlobClient = StorageAccount.CreateCloudBlobClient();
		var container = cloudBlobClient.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName"));
		if (container.CreateIfNotExists())
		{
			container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
		}

		var sas = container.GetSharedAccessSignature(new SharedAccessBlobPolicy()
		{
			Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Delete,
			SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
			SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(2),
		});

		return sas;
	}
	````

1. Open the _Index.cshtml_ located in the _Views\Home_ folder, and add the following code to add the share link to the photos actions.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex4-IndexViewUpdateWithShareLink_)

	<!-- mark:5,11 -->
	````CSHTML
	<td>
		@Html.ActionLink("Edit", "Edit", new { partitionKey = photo.PartitionKey, rowKey = photo.RowKey }) |
		@Html.ActionLink("Details", "Details", new { partitionKey = photo.PartitionKey, rowKey = photo.RowKey }) |
		@Html.ActionLink("Delete", "Delete", new { partitionKey = photo.PartitionKey, rowKey = photo.RowKey }) |
		@Html.ActionLink("Share", "Share", new { partitionKey = photo.PartitionKey, rowKey = photo.RowKey })
	</td>
	````

	> **Note:** Notice that the **ActionLink** is calling the Share action passing the partition and row keys as parameters.

1. Open the **HomeController.cs** file, located in the _Controllers_ folder, and add the following field.

	<!-- mark:3 -->
	````C#
	private CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
	private Uri uriTable = new Uri("http://127.0.0.1:10002/devstoreaccount1");
	private Uri uriBlob = new Uri("http://127.0.0.1:10000/devstoreaccount1");
	````

1. Scroll down to the **GetBlobContainer** method and replace it with the following implementation that uses SAS.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex4-NewCloudBlobClientCall_)
	
	<!-- mark:1-6 -->
	````C#
	private CloudBlobContainer GetBlobContainer()
	{
		var sasToken = SasService.GetSasForBlobContainer();
		var client = new CloudBlobClient(this.uriBlob, new StorageCredentials(sasToken));
		return client.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName"));
	}
	````

1. Create a new **Share** action in the _HomeController_ class.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex4-ShareAction_)

	<!-- mark:1-26 -->
	````C#
	public async Task<ActionResult> Share(string partitionKey, string rowKey)
	{
		var photoContext = this.GetPhotoContext();

		var photo = await photoContext.GetByIdAsync(partitionKey, rowKey);
		if (photo == null)
		{
			return this.HttpNotFound();
		}

		var sas = string.Empty;
		if (!string.IsNullOrEmpty(photo.BlobReference))
		{
			var blobBlockReference = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference);
			sas = SasService.GetReadonlyUriWithSasForBlob(blobBlockReference.Name);
		}

		if (!string.IsNullOrEmpty(sas))
		{
			return View("Share", null, sas);
		}

		return RedirectToAction("Index");
	}
	````

	The preceding code gets the blob reference by using the partition and row keys, and calls the **GetReadonlyUriWithSasForBlob** method passing the blob name as parameter. In this case, the SAS is created with **Read** permissions.

1. You will now add the corresponding view to the **Share** action you just created. To do so, right click in the **Home** folder under **Views**, go to **Add** and select **Existing Item...**.

1. Browse to the _Assets/Ex4-IntroducingSAS_ folder, select the **Share.cshtml** file and click **Add**.

1. Run the solution by pressing **F5**.

1. Log into the application. If you do not have a user, register to create one.

1. If you haven't uploaded images yet, upload one image now.

1. Click the **Share** link, next to one of the uploaded photos. You will navigate to the _Share_ page.

	![Generating a link to share a blob](Images/sharing-a-blob-page.png?raw=true "Generating a link to share a blob")

	_Generating a link to share a blob_

1. Copy the provided link and open it in your browser. You will be able to see the image from your browser.

	![Opening a shared blob](Images/opening-a-shared-blob.png?raw=true "Opening a shared blob")

	_Opening a shared blob_

1. Wait two minutes (time it takes for this SAS token to expire) and press **Ctrl+F5** to make a full page refresh. As the token is no longer valid, you will not be able to see the image and an error will be displayed.

	![Opening an expired share link](Images/opening-an-expired-share-link.png?raw=true "Opening an expired share link")

	_Opening an expired share link_

	>**Note:** If you don't get the expected error, Internet Explorer may be serving the image from the cache. You can avoid hitting the cache by accessing in _Private Mode_ (press _Ctrl+Shift+P_).

<a name="Ex4Task3" />
#### Task 3 - Adding SAS at Queue level  ####

In this task you will use SAS at queue level to restrict access to the Queue Storage. SAS can enable Read, Add, Process, and Update permissions on a queue.

1. Open the **SasService.cs** file and add the following _using_ statements.
	
	````C#
	using Microsoft.WindowsAzure.Storage.Queue;
	````

1. Create a new method called **GetAddSasForQueues** with the following code.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex4-GetAddSasForQueues_)

	````C#
	public static string GetAddSasForQueues()
	{
		var cloudQueueClient = StorageAccount.CreateCloudQueueClient();
		var policy = new SharedAccessQueuePolicy() 
		{ 
			 Permissions = SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Read, 
			 SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15) 
		};

		var sasToken = cloudQueueClient.GetQueueReference("messagequeue").GetSharedAccessSignature(policy, null);
		return sasToken;
	}
	````
1. Open the **HomeController.cs** file under the _Controllers_ folder, and add the following field.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex4-UriQueueProperty_)

	<!-- mark:6 -->
	````C#
	public class HomeController : Controller
	{
		private CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
		private Uri uriTable = new Uri("http://127.0.0.1:10002/devstoreaccount1");
		private Uri uriBlob = new Uri("http://127.0.0.1:10000/devstoreaccount1");
		private Uri uriQueue = new Uri("http://127.0.0.1:10001/devstoreaccount1");
		...
	}
	````

	>**Note**: Replace _<http://127.0.0.1:10001/devstoreaccount1>_ with your storage account table URI in order to work against Windows Azure.

1. Scroll down to the **GetCloudQueue** method and update the **queueClient** creation with the following code. 

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex4-NewQueueClientCall_)

	<!-- mark:3-4 -->
	````C#
	private CloudQueue GetCloudQueue()
	{
		var sasToken = SasService.GetAddSasForQueues();
		var queueClient = new CloudQueueClient(this.uriQueue, new StorageCredentials(sasToken));
		var queue = queueClient.GetQueueReference("messagequeue");
		return queue;
	}

	````

1. In the **QueueProcessor_WorkerRole** project, open the **WorkerRole** class and add the following _using_ statements.

	````C#
	using Microsoft.WindowsAzure.Storage.Queue;
	using Microsoft.WindowsAzure.Storage.Auth;
	````

1. Add the following fields at the start of the class to save the **queue URI** and the **expiration time** of the queue SAS token. Keep in mind that you can replace the local storage URI with your Azure Queue Storage URL.


	<!-- mark:3-4 -->
	````C#
	public class WorkerRole : RoleEntryPoint
	{
		private DateTime serviceQueueSasExpiryTime;
		private Uri uri = new Uri("http://127.0.0.1:10001/devstoreaccount1");

		...
	}
	````

1. Create a new private method called **GetProcessSasForQueues** using the following code.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex4-GetProcessSasForQueues_)

	````C#
	public string GetProcessSasForQueues()
	{
		var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
		var queue = storageAccount.CreateCloudQueueClient().GetQueueReference("messagequeue");
		queue.CreateIfNotExists();
		this.serviceQueueSasExpiryTime = DateTime.UtcNow.AddMinutes(15);
		return queue.GetSharedAccessSignature(new SharedAccessQueuePolicy() { Permissions = SharedAccessQueuePermissions.ProcessMessages | SharedAccessQueuePermissions.Read | SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Update, SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15) }, null);
	}
	````

	This method gets a reference to the application's queue and generates a SAS token that has permissions to process, read, add, and update messages.

1. Now update the **Run** method by replacing its body with the following code.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex4-RunMethodUpdate_)

	````C#
	public override void Run()
	{
		// This is a sample worker implementation. Replace with your logic.
		Trace.TraceInformation("QueueProcessor_WorkerRole entry point called", "Information");

		// Initialize the account information
		var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

		// retrieve a reference to the messages queue
		var queueClient = new CloudQueueClient(this.uri, new StorageCredentials(this.GetProcessSasForQueues()));
		var queue = queueClient.GetQueueReference("messagequeue");

		while (true)
		{
			Thread.Sleep(10000);
			Trace.TraceInformation("Working", "Information");
			if (queue.Exists())
			{
				if (DateTime.UtcNow.AddMinutes(1) >= this.serviceQueueSasExpiryTime)
				{
					queueClient = new CloudQueueClient(this.uri, new StorageCredentials(this.GetProcessSasForQueues()));
					queue = queueClient.GetQueueReference("messagequeue");
				}

				var msg = queue.GetMessage();

				if (msg != null)
				{
					Trace.TraceInformation(string.Format("Message '{0}' processed.", msg.AsString));
					queue.DeleteMessage(msg);
				}
			}
		}
	}
	````

1. Press **F5** to run the application. Once the browser is opened, upload a new image.

1. Open the **Compute Emulator**. To do so, right-click the Windows Azure icon tray and select **Show Compute Emulator UI**.

	![Windows Azure Tray Icon](Images/windows-azure-tray-icon.png?raw=true "Windows Azure Tray Icon")

	_Windows Azure Tray Icon_

1. Log in to the application and upload a new photo. Wait until the process reads the message from the queue and shows the _"Photo uploaded"_ message.

	![New processed message](Images/logged-user-can-add-messages-to-the-queue.png?raw=true "New processed message")

	_New processed message_

<a name="Exercise5" />
### Exercise 5: Updating SAS to use Stored Access Policies ###

A Stored Access Policy provides **an additional level of control** over Shared Access Signatures on the server side. Establishing a Stored Access Policy serves to group Shared Access Signatures and to provide additional restrictions for signatures that are bound by the policy. You can use a stored access policy to change the start time, expiry time, or permissions for a signature, or to revoke it after it has been issued.

A stored access policy gives you greater control over Shared Access Signatures you have released. Instead of specifying the signature's lifetime and permissions on the URL, you can specify these parameters within the Stored Access Policy on the blob, container, queue, or table that is being shared. To change these parameters for one or more signatures, you modify only the Stored Access Policy, rather than reissuing the signatures. You can also quickly revoke the signature by modifying the Stored Access Policy, for example in case of a security breach.


<a name="Ex5Task1" />
#### Task 1 - Updating table security to use Stored Access Policy ####

In this task you will update table security to use Stored Access Policy.

1. Continue working with the end solution of the previous exercise or open the solution located at _/Source/Ex5-UpdatingSASToUseStoredAccessPolicies/Begin_. Remember to run Visual Studio **as administrator** to be able to use the Windows Azure storage emulator.

1. Update **Global.asax.cs** to set the Stored Access Policies for Table Storage.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-TableStorageStoredAccessPoliciesTables_)

	<!-- mark:6-11 -->
	````C#
	protected void Application_Start()
  {
		...
		queue.CreateIfNotExists();

		TablePermissions tp = new TablePermissions();
		tp.SharedAccessPolicies.Add("readonly", new SharedAccessTablePolicy { Permissions = SharedAccessTablePermissions.Query, SharedAccessExpiryTime =  System.DateTime.UtcNow.AddMinutes(15) });
		tp.SharedAccessPolicies.Add("edit", new SharedAccessTablePolicy { Permissions = SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Update, SharedAccessExpiryTime =  System.DateTime.UtcNow.AddMinutes(15) });
		tp.SharedAccessPolicies.Add("admin", new SharedAccessTablePolicy { Permissions = SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Update | SharedAccessTablePermissions.Delete, SharedAccessExpiryTime =  System.DateTime.UtcNow.AddMinutes(15) });
		tp.SharedAccessPolicies.Add("none", new SharedAccessTablePolicy { Permissions = SharedAccessTablePermissions.None, SharedAccessExpiryTime =  System.DateTime.UtcNow.AddMinutes(15) });
		table.SetPermissions(tp);
  }
	````

1. Open the **SasService.cs** class located in the **Services** folder and replace the **GetSasForTable** method with the following code.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-GetSasForTableImplementation_)

	<!-- mark:5-23 -->
	````C#
	public class SasService
	{
		...

		public static string GetSasForTable(string username, string policyName)
		{
			var cloudTableClient = StorageAccount.CreateCloudTableClient();
			var policy = new SharedAccessTablePolicy()
			{
				SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
				Permissions = SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Delete | SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Update
			};

			var sasToken = cloudTableClient.GetTableReference("Photos").GetSharedAccessSignature(
				policy:  new SharedAccessTablePolicy(),
				accessPolicyIdentifier: policyName,
				startPartitionKey: username,
				startRowKey: null,
				endPartitionKey: username,
				endRowKey: null);

			return sasToken;
		}
	}
	````

1. Scroll down to the **GetPhotoContext** method and update the **sasToken** creation with the following code.  

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-GetPhotoContext_)
	
	<!-- mark:3 -->
	````C#
	private PhotoDataServiceContext GetPhotoContext()
	{
		var sasToken = SasService.GetSasForTable(this.User.Identity.Name, "admin");
		var cloudTableClient = new CloudTableClient(this.uriTable, new StorageCredentials(sasToken));
		var photoContext = new PhotoDataServiceContext(cloudTableClient);
		return photoContext;
	}
	````

<a name="Ex5Task2" />
#### Task 2 - Updating blob security to use stored access policy ####

1. Open the **Global.asax.cs** file and locate the **Application_Start** method. Set the Stored Access Policies for Blob Storage by adding the following code at the end of the method.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-BlobStorageStoredAccessPolicy_)
	<!-- mark:6-8 -->
	````C#
	protected void Application_Start()
	{
		...
		table.SetPermissions(tp);

		BlobContainerPermissions bp = new BlobContainerPermissions();
		bp.SharedAccessPolicies.Add("read", new SharedAccessBlobPolicy { Permissions = SharedAccessBlobPermissions.Read, SharedAccessExpiryTime = System.DateTime.UtcNow.AddMinutes(60) });
		container.SetPermissions(bp);
	}
	````

	>**Note**: Replace the _<http://127.0.0.1:10002/devstoreaccount1>_ with your storage account table URI in order to work against Windows Azure if not already replaced.

1. Open the **SasService.cs** class and replace the _GetReadonlyUriWithSasForBlob_ method with the following implementation. Notice you only added a new **policyId** parameter and passed it to _GetSharedAccessSignature_.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-GetSasForBlobWithStoredAccessPolicy_)
	
	````C#
	public static string GetReadonlyUriWithSasForBlob(string blobName, string policyId)
	{
		var cloudBlobClient = StorageAccount.CreateCloudBlobClient();
		var blob = cloudBlobClient.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName")).GetBlockBlobReference(blobName);
		var sas = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
		{
			Permissions = SharedAccessBlobPermissions.Read,
			SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
			SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(2),
		}, policyId);

		return string.Format(CultureInfo.InvariantCulture, "{0}{1}", blob.Uri, sas);
	}
	````

1. Open the **HomeController.cs** class and scroll down to the _Share_ method. Replace the _GetSasForBlob_ method call with the new implementation.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-ShareActionWithStoredAccessPolicy_)

	<!-- mark:10 -->
	````C#
	[HttpGet]
	public ActionResult Share(string partitionKey, string rowKey)
	{
		...

		string sas = string.Empty;
		if (!string.IsNullOrEmpty(photo.BlobReference))
		{
			 CloudBlockBlob blobBlockReference = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference);
			 sas = SasService.GetReadonlyUriWithSasForBlob(blobBlockReference.Name, "read");
		}

		if (!string.IsNullOrEmpty(sas))
		{
			 return View("Share", null, sas);
		}

		return RedirectToAction("Index");
	}
	````

<a name="Ex5Task3" />
#### Task 3 - Updating queue security to use stored access policies ####

1. Open the **Global.asax.cs** file and add the following using statements.

	````C#
	using Microsoft.WindowsAzure.Storage.Queue;
	using Microsoft.WindowsAzure.Storage.Queue.Protocol;
	````

1. Update the **Application_Start** method to set the Stored Access Policies for Queues Storage. You will also add new **resize** metadata and set it to _true_.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-QueueStorageWithStoredAccessPolicy_)

	<!-- mark:5-13 -->
	````C#
	protected void Application_Start()
	{
		...
		container.SetPermissions(bp);

		QueuePermissions qp = new QueuePermissions();
		qp.SharedAccessPolicies.Add("add", new SharedAccessQueuePolicy { Permissions = SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Read, SharedAccessExpiryTime = System.DateTime.UtcNow.AddMinutes(15) });
		qp.SharedAccessPolicies.Add("process", new SharedAccessQueuePolicy { Permissions = SharedAccessQueuePermissions.ProcessMessages | SharedAccessQueuePermissions.Read, SharedAccessExpiryTime = System.DateTime.UtcNow.AddMinutes(15) });
		queue.SetPermissions(qp);
		queue.Metadata.Add("Resize", "true");
		queue.SetMetadata();
	}
	````

1. Open the **SasService.cs** file located in the **Service** folder and locate the _GetAddSasForQueues_ method. Replace the _GetSharedAccessSignature_ method call with the following code.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-GetAddSasForQueuesWithStoredAccessPolicy_)

	<!-- mark:10 -->
	````C#
	public static string GetAddSasForQueues()
	{
		var cloudQueueClient = StorageAccount.CreateCloudQueueClient();
		var policy = new SharedAccessQueuePolicy() 
		{ 
			Permissions = SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Read, 
			SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15) 
		};

		var sasToken = cloudQueueClient.GetQueueReference("messagequeue").GetSharedAccessSignature(policy, "add");
		return sasToken;
	}
	````

1. Open the **HomeController.cs** file located in the _Controllers_ folder and locate the **Create** _POST_ method.

1. Update the code inside the **if** block with the following in order to **add metadata** to the blob reference before uploading it.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-BlobMetadata_)
	<!-- mark:11-16 -->
	````C#
	[HttpPost]
	public async Task<ActionResult> Create(PhotoViewModel photoViewModel, HttpPostedFileBase file, FormCollection collection)
	{
		...

		if (file != null)
		{
			// Save file stream to Blob Storage
			var blob = this.GetBlobContainer().GetBlockBlobReference(file.FileName);
			blob.Properties.ContentType = file.ContentType;
			var image = new System.Drawing.Bitmap(file.InputStream);
			if (image != null)
			{
				blob.Metadata.Add("Width", image.Width.ToString());
				blob.Metadata.Add("Height", image.Height.ToString());
			}

			await blob.UploadFromStreamAsync(file.InputStream);
			photo.BlobReference = file.FileName;
		}
		else
		{
			this.ModelState.AddModelError("File", new ArgumentNullException("file"));
			return this.View(photoViewModel);
		}

		...
	}
	````

	>**Note:** The **Bitmap** class allows you to easily read the _Width_ and _Height_ properties of the uploaded image.

1. On the **QueueProcessor_WorkerRole** project, open the **WorkerRole.cs** file.

1. Add the following using statements.

	````C#
	using Microsoft.WindowsAzure.Storage.Queue.Protocol;
	using Microsoft.WindowsAzure.Storage.Blob;
	````

1. Add the following field to the **WorkerRole** class to store the _CloudBlobContainer_

	````C#
	private CloudBlobContainer container;
	````

1. Create a new method called **CreateCloudBlobClient** in order to create the container and also set the _container_ variable. To do that, insert the following code.
	
	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-CreateCloudBlobClientImplementation_)

	````C#
	private void CreateCloudBlobClient()
	{
		var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

		CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
		this.container = blobStorage.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName"));
	}
	````

1. In the **OnStart** method, call the **CreateCloudBlobClient** method you have just created.
	
	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-CreateCloudBlobClientCall_)

	<!-- mark:5 -->
	````C#
	public override bool OnStart()
	{
		...

		this.CreateCloudBlobClient();

		return base.OnStart();
	}
	````

1. Scroll down to the **GetProcessSasForQueues** method. Replace the entire method with the following code.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-QueueSharedAccessSignatureWithStoredAccessPolicyInWorkerRole_)

	````C#
	public string GetProcessSasForQueues()
	{
		var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
		var client = storageAccount.CreateCloudQueueClient();
		var queue = client.GetQueueReference("messagequeue");
		queue.CreateIfNotExists();

		QueuePermissions qp = new QueuePermissions();
		qp.SharedAccessPolicies.Add("process", new SharedAccessQueuePolicy { Permissions = SharedAccessQueuePermissions.ProcessMessages | SharedAccessQueuePermissions.Read, SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15) });
		queue.SetPermissions(qp);

		var token = queue.GetSharedAccessSignature(
						new SharedAccessQueuePolicy(),
						"process");
		this.serviceQueueSasExpiryTime = DateTime.UtcNow.AddMinutes(15);
		return token;
	}
	````

1. Add the following code to the **Run** method in the **WorkerRole** class in order to display the properties and metadata saved in the WebRole. Replace the  **if (msg != null) { ... }** block with the following code.

	(Code Snippet - _GettingStartedWindowsAzureStorage_ - _Ex5-RunMethodUpdate_)

	<!-- mark:11-36 -->
	````C#
		public override void Run()
		{
			...

			 while (true)
            {
                ...
                if (queue.Exists())
                {
                    ...
                    if (msg != null)
                    {
                        queue.FetchAttributes();

                        var messageParts = msg.AsString.Split(new char[] { ',' });
                        var message = messageParts[0];
                        var blobReference = messageParts[1];

                        if (queue.Metadata.ContainsKey("Resize") && string.Equals(message, "Photo Uploaded"))
                        {
                            var maxSize = queue.Metadata["Resize"];

                            Trace.TraceInformation("Resize is configured");

                            CloudBlockBlob outputBlob = this.container.GetBlockBlobReference(blobReference);

                            outputBlob.FetchAttributes();

                            Trace.TraceInformation(string.Format("Image ContentType: {0}", outputBlob.Properties.ContentType));
                            Trace.TraceInformation(string.Format("Image width: {0}", outputBlob.Metadata["Width"]));
                            Trace.TraceInformation(string.Format("Image hieght: {0}", outputBlob.Metadata["Height"]));
                        }

                        Trace.TraceInformation(string.Format("Message '{0}' processed.", msg.AsString));
                        queue.DeleteMessage(msg);
                    }
                }
            }
		}
	````

1. Go the the Cloud project, right-click the **QueueProcessor_WorkerRole** role located under the **Roles** folder and select **Properties**.

	![WorkerRole Properties](Images/workerrole-properties.png?raw=true "WorkerRole Properties")

	_WorkerRole Properties_

1. Click the **Settings** tab and add a new setting named **ContainerName** of type _String_ and value "**gallery**".

	![Settings tab](Images/settings-tab2.png?raw=true "Settings tab")

	_Settings tab_

1. Press **Ctrl** + **S** to save the settings.


<a name="Ex5Task4" />
#### Task 4 - Verification ####

1. Press **F5** to start debugging the solution.

	>**Note**: The Windows Azure Emulator should start.

1. Log into the application with the user you created in Exercise 3.

1. Click the **Share** link in one of the photos you have uploaded. If you have not, you can do it now.

	![Sharing a photo with Stored Access Policy](Images/sharing-a-photo-with-stored-access-policy.png?raw=true "Sharing a photo with Stored Access Policy")

	_Sharing a photo with Stored Access Policy_

	>**Note**: Notice how there's a new parameter in the query string named _si_ that has the value _read_ which is the Signed Identifier.

1. Go back to the **Index** page and click on **Create**.

1. Upload a new image of your choice.

1. Open the _Compute Emulator_ and check that the **Properties** and **Metadata** are logged by the Worker Role.

	![Compute Emulator logs in worker role](Images/compute-emulator-logs-in-worker-role.png?raw=true "Compute Emulator logs in worker role")

	_Compute Emulator logs in worker role_

---

## Next Steps ##

To learn more about **Windows Azure Storage**, please refer to the following articles which expand the information on the technologies explained on this lab:

- [Shared Access Signatures, Part 1: Understanding the SAS Model](http://aka.ms/Fkjgzx): In Part 1 of this tutorial on shared access signatures, you will see an overview of the SAS model and review SAS best practices.

- [Shared Access Signatures, Part 2: Create and Use a SAS with the Blob Service](http://aka.ms/Xsyztd): Part 2 of this tutorial walks you through the process of creating SAs with the Blob service.

- [Introducing Table SAS (Shared Access Signature), Queue SAS and update to Blob SAS](http://aka.ms/Yaqi4e): In this blog POST, we will see usage scenarios for these features along with sample code.

- [How to use the Table Storage Service](http://aka.ms/Yf1l4c): This guide will show you how to perform common scenarios using the Windows Azure Table Storage Service.

- [How to use the Windows Azure Blob Storage Service in .NET](http://aka.ms/Uguxow): This guide will demonstrate how to perform common scenarios using the Windows Azure Blob storage service.

- [Browsing Storage Resources with Server Explorer](http://aka.ms/Gouc3c): This guide will show you how to display data from your local storage emulator account and also from storage accounts that you've created for Windows Azure by using the Windows Azure Storage node in Server Explorer.

- [Storage Services REST API Reference](http://aka.ms/V84kea): The reference of the Storage Services REST API gives insight into the underlying HTTP-based infrastructure that supports Windows Azure Storage.

- [Set and Retrieve Properties and Metadata](http://aka.ms/Nd1yuv): This guide will show you how to set and retrieve properties and metadata from blobs and blob containers.

- [New Azure Tools in Visual Studio 2013 (Video)](http://aka.ms/Uh25d9): In this episode Paul Yuknewicz, Dennis Angeline and Boris Scholl show how the Windows Azure SDK 2.2 adds new levels of productivity to Visual Studio for cloud development.

- [Getting Started with Windows Azure, the SDK, and Visual Studio (Video)](http://aka.ms/Y1ln6z): In this episode you will be guided through deploying an Azure Web Site and debugging it in the cloud.

---

<a name="summary" />
## Summary ##

By completing this hands-on lab you have learned how to:

* Create a Storage Account.
* Enable Geo-Replication.
* Configure Monitoring metrics for your account.
* Configure Logging for each service.
* Consume Storage Services from a Web Application.
* Create and consume Shared Access Signatures.
* Enhance Shared Access Signatures by using Stored Access Policies.

---

