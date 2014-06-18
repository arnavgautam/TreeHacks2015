<a name="handsonlab"></a>
# Exploring Microsoft Azure Storage - for Visual Studio 2012 #
---

<a name="Overview"></a>
## Overview ##

Storage services provide persistent, durable storage in the Microsoft Azure compute emulator, and include blob and table service and the queue service. In addition, using Microsoft Azure Drives, your Microsoft Azure applications running in the cloud can use existing NTFS APIs to access a durable drive backed by blob storage.
In this lab, you will examine the basic process of working with Microsoft Azure storage on the local compute emulator, and explore some of the features that are available to developers.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

-	Use the Table Service
-	Use the Blob service
-	Use the Queue service
-	Create and read metadata
-	Use Microsoft Azure Drives

<a name="Prerequisites"></a>
### Prerequisites ###

- [Microsoft Visual Studio 2012 Express for Web] [1] or higher
- [Microsoft Azure Tools for Microsoft Visual Studio 1.8] [2]
- A Microsoft Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

[1]: http://www.microsoft.com/visualstudio/
[2]: http://www.microsoft.com/windowsazure/sdk/

> **Note:** This lab was designed for Windows 8.
	
<a name="Setup"></a>
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab’s **Source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process. This process will configure your environment and install the Visual Studio code snippets for this lab.
1. If the User Account Control dialog is shown, confirm the action to proceed.

> **Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="CodeSnippets"></a>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2012 to avoid having to add it manually.

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Working with Tables](#Exercise1)
1. [Working with Blobs](#Exercise2)
1. [Working with Queues](#Exercise3)
1. [Working with Drives](#Exercise4)

Estimated time to complete this lab: **90 minutes**.

>**Note:** Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise. Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.
>
>When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

<a name="Exercise1"></a>
### Exercise 1: Working with Tables ###

In this exercise, you use the Microsoft Azure Table Service API to create a simple application that stores and retrieves data in structured storage. It consists of a simple chat Web application that can save, retrieve and display messages stored in a Microsoft Azure table.

Microsoft Azure tables store data as collections of entities, which are similar to rows in a database. An entity has a primary key and a set of properties composed by a name/value pair, similar to a column.

To access Microsoft Azure Table Service, you use a REST API that is compatible with [WCF Data Services][5] (formerly ADO.NET Data Services Framework). This exercise uses the [WCF Data Services Client Library][6] (formerly .NET Client Library) to read and write data to table service.

[5]: http://msdn.microsoft.com/en-us/library/cc668792.aspx
[6]: http://msdn.microsoft.com/en-us/library/cc668772.aspx

>**Note:** To reduce typing, you can right-click where you want to insert source code, select Insert Snippet, select My Code Snippets and then select the entry matching the current exercise step.

<a name="Ex1Task1"></a>
#### Task 1 - Configuring Storage Account Settings ####

In this task, you configure the settings required to make a connection to the Table Service.

1. Open Visual Studio 2012 Express for Web, or higher, elevated **as Administrator**.

1. In the **File** menu, choose **Open Project**. In the **Open Project** dialog, browse to **\\Source\\Ex1-WorkingWithTables\\Begin** and open the **Begin.sln** file.

 	![Solution Explorer showing the Microsoft Azure Chat application](Images/solutionexplorer.png?raw=true)
 
 	_Solution Explorer showing the Microsoft Azure Chat application_

	>**Note:**  The solution contains a Microsoft Azure WebRole project.

1. Make sure **RdChat** is the startup project by right-clicking it in **Solution Explorer** and selecting **Set as StartUp Project**.

 	![Setting the startup project](Images/startupproject.png?raw=true)

 	_Setting the startup project_

1. Update the service definition to define the configuration settings required to access Microsoft Azure Table service.  To do this, expand the **Roles** folder of the **RdChat** project in **Solution Explorer**, right-click **RdChat_WebRole**, and then select **Properties**.

 	![Launching the service configuration editor](Images/configurationeditor.png?raw=true)

 	_Launching the service configuration editor_

1. Select the **Settings** tab, click **Add Setting** and create a new configuration setting named _DataConnectionString_. Set its type to **Connection String**, then click the button labeled with an ellipsis and configure the storage connection string to _Microsoft Azure storage emulator_.

 	![Creating a storage connection string](Images/webroleconfiguration.png?raw=true)

	_Creating a storage connection string_

 	![Configuring a connection string to use Storage emulator](Images/storageaccountconnectionstring.png?raw=true)

 	_Configuring a connection string to use Storage emulator_

1. Press **CTRL + S** to save your configuration changes.

> **Note:** The StorageClient library uses these settings to access Microsoft Azure Storage. 
**DataConnectionString**: This is the connection string to the Window Azure account, through which we can programmatically access data storage and other functionalities in Microsoft Azure. This connection string can point to a Microsoft Azure Account in the cloud as well as the local compute emulator.
**Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString**: This is the connection string to Microsoft Azure server, same as _DataConnectionString_, however this one is dedicated for logging diagnostics.

<a name="Ex1Task2"></a>
#### Task 2 - Creating Classes to Model the Table Schema ####

When working locally against the Storage Emulator service for Table Service, you use WCF Data Services client library.

To use Microsoft Azure table service in .NET, you construct a class that models the desired schema. In addition to the properties required by your model, the class must include a **Timestamp**, a **PartitionKey** and a **RowKey** property and it must be decorated with a **DataServiceKey** _("PartitionKey", "RowKey")_ custom attribute. To simplify this, the **Microsoft.WindowsAzure.StorageClient** namespace includes a **TableServiceEntity** class that already defines the mandatory properties and attributes and can easily be derived from in your class.

In this task, you create the model where the data is stored for the Chat application.

1. Add a class to the Web role project to model the message table. To do this, in **Solution Explorer**, right-click the **RdChat_WebRole** project node, point to **Add** and select **Class**. In the **Add New Item** dialog, set the **Name** to **Message.cs** and then click **Add**.

1. Update the declaration of the Message class to derive from the **Microsoft.WindowsAzure.StorageClient.TableServiceEntity** class. 

	(Code Snippet - _ExploringStorage-Ex1-01-MessageClass-CS_)
	<!--mark: 2-->
	````C#
	public class Message 
	    : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
	{
	}	
	````

	> **Note:** The **TableServiceEntity** class is included as part of the **Microsoft.WindowsAzure.StorageClient** library. It defines the **PartititionKey, RowKey** and **TimeStamp** system properties required by every entity stored in a Microsoft Azure table.
Together, the **PartitionKey** and **RowKey** define the **DataServiceKey** that uniquely identifies every entity within a table.

1. Add a default constructor to the **Message** class that initializes its **PartitionKey** and **RowKey** properties.

	(Code Snippet - _ExploringStorage-Ex1-02-MessageConstructor-CS_)
	<!--mark: 1-5-->
	````C#
	public Message()
	{
	  PartitionKey = "a";
	  RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
	}
	````

	> **Note:** For the purposes of this exercise, you assign a fixed value to the **PartitionKey** property. In a more realistic scenario, you would choose a value that ensures load balancing across storage nodes.


1. Add two string properties to the **Message** class, **Name** and **Body**, to hold information about the chat message.

	(Code Snippet - _ExploringStorage-Ex1-03-TableSchemaProperties-CS_)
	<!--mark: 1,3-->
	````C#
	public string Name { get; set; }
	
	public string Body { get; set; }
	````
	
1. Save the **Message.cs** file.
1. Next, add a class to the Web role project to define the WCF Data Services **DataServiceContext** required to access the Messages table. To do this, in **Solution Explorer**, right-click the **RdChat_WebRole** project node, point to **Add** and select **Class**. In the **Add New Item** dialog, set the **Name** to **MessageDataServiceContext.cs** and then click **Add**.
1. In the new class file, add the following using namespace directives.

	(Code Snippet - _ExploringStorage-Ex1-04-Namespace-CS_)
	<!--mark: 1,2-->
	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.StorageClient;
	````

1. Replace the declaration of the new class to derive from the **TableServiceContext** class and include a default constructor to initialize the base class with the storage account information.
	
	(Code Snippet - _ExploringStorage-Ex1-05-MessageDataServiceContextClass-CS_)
	<!--mark: 3-10-->
	````C#
	namespace RdChat_WebRole
	{
	  public class MessageDataServiceContext
	      : TableServiceContext
	  {
	    public MessageDataServiceContext(string baseAddress, StorageCredentials credentials)
	        : base(baseAddress, credentials)
	    {
	    }
	  }
	}
	````
1. Now, add a property to the **MessageDataServiceContext** class to return a data service query for the Messages table. 

	(Code Snippet - _ExploringStorage-Ex1-06-MessagesProperty-CS_)
	<!--mark: 5-11-->
	````C#
	public class MessageDataServiceContext
	    : TableServiceContext
	{
	  ...
	  public IQueryable<Message> Messages
	  {
	    get
	    {
	      return this.CreateQuery<Message>("Messages");
	    }
	  }
	}
	````
	
1. Finally, add a method to the **MessageDataServiceContext** class to insert new messages into the table. You will use this method later when implementing the chat functionality.

	(Code Snippet - _ExploringStorage-Ex1-07-AddMessageMethod-CS_)
	<!--mark: 5-9-->
	````C#
	public class MessageDataServiceContext
		: TableServiceContext
	{
	  ...
	  public void AddMessage(string name, string body)
	  {
		this.AddObject("Messages", new Message { Name = name, Body = body });
		this.SaveChanges();
	  }
	}
	````
	
1. In the Build menu, select Build Solution.

<a name="Ex1Task3"></a>
#### Task 3 – Creating the Chat User Interface ####

In this task, you add the code necessary to store messages in a Microsoft Azure table and display them on the Web page.

1. Locate the **Application_Start** method in **Global.asax.cs** file and insert the following code (shown in **bold**) into it. This creates storage tables from **MessageDataServiceContext** that we created earlier.

	(Code Snippet - _ExploringStorage-Ex1-08-ApplicationStartMethod-CS_)

	<!--mark: 4-13-->
	````C#
	protected void Application_Start()
	{
	  ...
	  /// Create data table from MessageDataServiceContext
	  /// It is recommended the data tables should be only created once. It is typically done as a 
	  /// provisioning step and rarely in application code.
	  var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
	
	  // dynamically create the tables
	  CloudTableClient.CreateTablesFromModel(
	  	  typeof(MessageDataServiceContext),
	  	  account.TableEndpoint.AbsoluteUri,
	  	  account.Credentials);
	}
	````

	> **Note:** The code shown above creates the required tables from the model defined by the **MessageDataServiceContext** class created earlier. 
	Note that the recommendation is that data tables should only be created once. Typically, you would do this during a provisioning step and rarely in application code. The **Application_Start** method in the **Global** class is a recommended place for this initialization logic.
	To retrieve and display messages, the method creates an instance of the **MessageDataServiceContext** class and initializes it from account information available in the service configuration file **(ServiceConfiguration.cscfg)**. It binds the **Messages** property, which returns a data service query for the _Messages_ table, to a **ListView** control on the page for display. 
	Objects of type **CloudStorageAccount** represent a storage account, which contains the settings required to make a connection to the Storage Service.  Associated with a storage account are the account name, the URI of the account and a shared key, which the **CloudTableClient** helper class uses for its initialization. These settings are obtained from **ServiceConfiguration.cscfg**.

1. Make sure the following namespace directives exist at the top of the **Global.asax.cs** code file. They are for the utility storage classes and for the **ServiceRuntime** classes.

	(Code Snippet - _ExploringStorage-Ex1-09-GlobalNamespace-CS_)
	<!--mark: 1-3-->
	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.ServiceRuntime;
	using Microsoft.WindowsAzure.StorageClient;
	````
	
1. Expand the **RDChat_WebRole** node in **Solution Explorer**, then right-click **Default.aspx** and select **View Code** to open the code-behind file for the Web page that contains the UI for the chat application.

	Make sure the following namespace directives are included in the **Default.aspx.cs** code-behind file.

	(Code Snippet - _ExploringStorage-Ex1-10-Namespace-CS_)
	<!--mark: 1,2-->
	````C#
	using System.Data.Services.Client;
	using Microsoft.WindowsAzure;
	````
	
1. Locate the **SubmitButton_Click** event handler in **Default.aspx.cs** and insert the following code (shown in **bold**) into the method body to save messages entered by the user to the Table Service, then data bind messages from Table Service to the page. The method uses the **AddMessage** method, which you created earlier in the lab, to insert a new **Message** entity into the table. 

	(Code Snippet - _ExploringStorage-Ex1-11-SubmitButtonClick-CS_)
	<!--mark: 3-21-->	
	````C#
	protected void SubmitButton_Click(object sender, EventArgs e)
		{
		  var statusMessage = string.Empty;
		
		  try
		  {
			var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
			var context = new MessageDataServiceContext(account.TableEndpoint.ToString(), account.Credentials);
		
			context.AddMessage(HttpUtility.HtmlEncode(this.nameBox.Text), HttpUtility.HtmlEncode(this.messageBox.Text));
		
			this.messageList.DataSource = context.Messages;
			this.messageList.DataBind();
		  }
		  catch (DataServiceRequestException ex)
		  {
		   statusMessage = "Unable to connect to the table storage server. Please check that the service is running.<br>"
							+ ex.Message;
		   }
		
		   this.status.Text = statusMessage;
		}
	````

1. Save the all files and select **Build Solution** from the **Build** menu.

<a name="Ex1Verification"></a>
#### Verification ####

To test your service running in the Compute Emulator:

1. In Visual Studio, press **F5** to build and run the application. The compute emulator starts and a new deployment containing the **RdChat** Web Role initializes. A browser window opens to display the Microsoft Azure Chat Web page. 

	![Application after connecting successfully to the storage emulator table server](Images/application-after-connecting-successfully.png?raw=true)
	
	_Application after connecting successfully to the storage emulator table server_
	
	When you start the program in the debugger, Visual Studio automatically starts Storage Emulator. If the Chat application is unable to access the table service server, you will see an error as shown on the page. To examine the status of the service, right-click the icon in the system tray (it looks like a server) and select **Show Storage Emulator UI**.
	
	![Viewing the status of storage emulator](Images/viewing-the-status-of-storage-emulator.png?raw=true)
	
	_Viewing the status of storage emulator_
	
1. Now, test the Chat application by entering a few messages. Type your name and the text of your message and click **Submit**.

1. In Internet Explorer, press **Ctrl + N** to open a second browser window. Enter a different name and type a few more messages. Notice how the chat box updates the conversation after sending a new message.

	![Using the Microsoft Azure chat application](Images/using-the-windows-azure-chat-application.png?raw=true)
	
	_Using the Microsoft Azure chat application_
	
<a name="Exercise2"></a>
### Exercise 2: Working with Blobs ###

In this exercise, you will use the Microsoft Azure Blob Service API to create an application that saves and retrieves image data stored as blobs in Microsoft Azure storage. It consists of a simple image gallery Web site that can display, upload and remove images in Microsoft Azure storage, and allows you to enter and display related metadata. The application uses a single container to store its image content as blobs.

When you create a blob in Microsoft Azure, you associate a content type that specifies the format in which the API returns it and allows you to retrieve an image directly from the URL of the corresponding blob.

<a name="Ex2Task1"></a>
#### Task 1 – Retrieving Blob Data from Storage ####

In this task, you will create an image gallery web page to display images retrieved from Microsoft Azure storage. The provided solution consists of a web site project with a single page that contains the elements required to display images and enter metadata.  You will add the necessary functionality by editing the code-behind file.

1. Open Microsoft Visual Studio 2012 Express for Web elevated as **Administrator**.

1. In the **File** menu, choose **Open Project**. In the **Open Project** dialog, browse to **\\Source\\Ex2-WorkingWithBlobs\\Begin** and open the **Begin.sln** file.

1. Update the service definition to define the configuration settings required to access Microsoft Azure Table service.  To do this, expand the Roles folder of the **RDImageGallery** project in **Solution Explorer**, right-click **RDImageGallery_WebRole**, and then select **Properties**.

	![Setting role configuration settings](Images/setting-role-configuration-settings.png?raw=true)

	_Setting role configuration settings_
	
1. In the **Settings** tab, click **Add Setting** and create a **ConnectionString** type named _DataConnectionString_. Click the button labeled with an ellipsis and set the connection string to **Microsoft Azure storage emulator**.

	![Configuring a storage connection string](Images/configuring-a-storage-connection-string.png?raw=true)
	
	_Configuring a storage connection string_
	
	
	![Storage connection string dialog](Images/storage-connection-string-dialog.png?raw=true)
	
	_Storage connection string dialog_
	
1. Add another setting named _ContainerName_ and set its value to _gallery_.

	![Creating a setting for the container name](Images/creating-a-setting-for-the-container-name.png?raw=true)
	
	_Creating a setting for the container name_
	
	> **Note:** The container name must be a valid Domain Name System (DNS) name, conforming to the following naming rules:
	>
	> - Must start with a letter or number, and can contain only letters, numbers, and dash (-) characters.
	> - All letters must be lowercase.
	> - Must be from 3 to 63 characters long.
	> - A name cannot contain a dash next to a period.

1. Expand the **RDImageGallery_WebRole** node in **Solution Explorer**, then right-click **Default.aspx** and select **View Code** to open the code-behind file for the user interface of the image gallery. In the next steps, you will modify this file to add some of the required functionality.

1. Make sure the following namespace directives exist at the top of the code file. They are for the utility storage classes and for the **ServiceRuntime** classes.
	
	(Code Snippet - _ExploringStorage-Ex2-01-Namespace-CS_)
	<!--mark: 1-4-->
	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.ServiceRuntime;
	using Microsoft.WindowsAzure.StorageClient;
	using Microsoft.WindowsAzure.StorageClient.Protocol;
	````
	
1. For this lab, you need to store the blobs in a public container, so they are visible on the web to normal, anonymous users.  In this step, you will ensure that the container specified in **ServiceConfiguration.cscfg** exists. To do this, add the following method at the bottom of the **_Default** class.

	(Code Snippet - _ExploringStorage-Ex2-02-EnsureContainerExistsMethod-CS_)
	<!--mark: 4-12-->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
	  ...
	  private void EnsureContainerExists()
	  {
	    var container = this.GetContainer();
	    container.CreateIfNotExist();
	
	    var permissions = container.GetPermissions();
	    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
	    container.SetPermissions(permissions);
	  }
	}
	````
	
1. Next, you will create a utility method to retrieve a reference to the container created by the code in the previous step.  This method will be called in almost all operations since the container is involved with all blob operations. Add a method to create the container at the bottom of the **_Default** class. This method uses the configuration settings you entered in earlier steps.

	(Code Snippet - _ExploringStorage-Ex2-03-GetContainerMethod-CS_)
	<!--mark: 4-11-->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
	  ...
	  private CloudBlobContainer GetContainer()
	  {
	    // Get a handle on account, create a blob service client and get container proxy
	    var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
	    var client = account.CreateCloudBlobClient();
	
	    return client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName"));
	  }
	}
	````
	
1. Insert the following code (shown in **bold**) in the **Page_Load** method to initialize the container and refresh the **asp:ListView** control on the page that displays the images retrieved from storage.

	(Code Snippet - _ExploringStorage-Ex2-04-PageLoadMethod-CS_)
	<!--mark: 6-27-->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
	  ...
	  protected void Page_Load(object sender, EventArgs e)
	  {
	    try
	    {
	      if (!IsPostBack)
	      {
	        this.EnsureContainerExists();
	      }
	      
	      this.RefreshGallery();
	    }
	    catch (System.Net.WebException we)
	    {
	      this.status.Text = "Network error: " + we.Message;
	      if (we.Status == System.Net.WebExceptionStatus.ConnectFailure)
	      {
	        this.status.Text += "<br />Please check if the blob service is running at " +
	        ConfigurationManager.AppSettings["storageEndpoint"];
	      }
	    }
	    catch (StorageException se)
	    {
	      Console.WriteLine("Storage service error: " + se.Message);
	    }
	  }
	  ...
	}
	````
	
1. Add the following method at the bottom of the **_Default** class to bind the images control to the list of blobs available in the image gallery container. The code uses the **ListBlobs** method in the **CloudBlobContainer** object to retrieve a collection of **IListBlobItem** objects that contain information about each of the blobs. The images **asp:ListView** control in the page binds to these objects to display their value.

	(Code Snippet - _ExploringStorage-Ex2-05-RefreshGalleryMethod-CS_)
	<!--mark: 4-13-->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
	  ...
	  private void RefreshGallery()
	  {
	    this.images.DataSource =
	      this.GetContainer().ListBlobs(new BlobRequestOptions()
	      {
	        UseFlatBlobListing = true,
	        BlobListingDetails = BlobListingDetails.All
	      });
	    this.images.DataBind();
	  }
	}
	````
	
1. Press **F5** to build and run the application. A browser window launches and displays the contents of the image gallery. Note that at this point, the container is empty and the list view displays a “No Data Available” message. In the next task, you will implement the functionality required to store images as blobs in Microsoft Azure storage.

	>**Note:** Make sure the cloud project is set as the startup project by right-clicking it in **Solution Explorer** and selecting **Set as StartUp Project**.

	![The image gallery application displaying an empty container](Images/the-image-gallery-application.png?raw=true)

	_The image gallery application displaying an empty container_

	> **Note:** If you have not configured the storage settings correctly or if the storage service is not running, an error similar to the one shown below is displayed.
	
	![An error caused by an invalid Microsoft Azure storage configuration or a service problem](Images/error--invalid-azure-storage.png?raw=true)
	
	_An error caused by an invalid Microsoft Azure storage configuration or a service problem_
	
1. Press **SHIFT+F5** in Visual Studio to stop debugging and delete the deployment from the compute emulator.

<a name="Ex2Task2"></a>
#### Task 2 – Uploading Blob Data to Storage ####

In this task, you add functionality to the image gallery Web page to enter metadata and upload image files to Microsoft Azure storage. The page contains text controls that you can use to enter descriptive metadata for the selected image. An **asp:FileUpload** control on the page retrieves images from disk and posts them to the page, where they are stored in blob storage.

1. Open the **Default.aspx.cs** file in the Visual Studio text editor. To do this, right-click the **Default.aspx** file in Solution Explorer and select **View Code**.

1. Add a method at the bottom of the page to save images and their metadata as blobs in Microsoft Azure storage. The method uses the **GetBlobReference** method in the **CloudBlobContainer** object to create a blob from the image data array and the metadata properties.

	(Code Snippet - _ExploringStorage-Ex2-06-SaveImageMethod-CS_)
	<!--mark: 4-22-->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
	  ...
	  private void SaveImage(string id, string name, string description, string tags, string fileName, string contentType, byte[] data)
	  {
	    // Create a blob in container and upload image bytes to it
	    var blob = this.GetContainer().GetBlobReference(name);
	
	    blob.Properties.ContentType = contentType;
	
	    // Create some metadata for this image
	    var metadata = new NameValueCollection();
	    metadata["Id"] = id;
	    metadata["Filename"] = fileName;
	    metadata["ImageName"] = string.IsNullOrEmpty(name) ? "unknown" : name;
	    metadata["Description"] = string.IsNullOrEmpty(description) ? "unknown" : description;
	    metadata["Tags"] = string.IsNullOrEmpty(tags) ? "unknown" : tags;
	
	    // Add and commit metadata to blob
	    blob.Metadata.Add(metadata);
	    blob.UploadByteArray(data);            
	  }
	}
	````
	
1. Complete the code in the event handler for the **Upload Image** button by inserting the code (shown in **bold** below) to **upload_Click** method.

	(Code Snippet - _ExploringStorage-Ex2-07-UploadClickMethod-CS_)
	<!--mark: 6-24-->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
	  ...
	  protected void upload_Click(object sender, EventArgs e)
	  {
	    if (this.imageFile.HasFile)
	    {
	      this.status.Text = "Inserted [" + this.imageFile.FileName + "] - Content Type [" + this.imageFile.PostedFile.ContentType + "] - Length [" + this.imageFile.PostedFile.ContentLength + "]";
	
	      this.SaveImage(
	        Guid.NewGuid().ToString(),
	        this.imageName.Text,
	        this.imageDescription.Text,
	        this.imageTags.Text,
	        this.imageFile.FileName,
	        this.imageFile.PostedFile.ContentType,
	        this.imageFile.FileBytes);
	
	      this.RefreshGallery();
	    }
	    else
	    {
	      this.status.Text = "No image file";
	    }
	  }
	  ...
	}
	````

	The code retrieves metadata from the text controls and from properties in the **asp:FileUpload** control on the page, which include the content type of the posted file, its file name, and the array of bytes containing the image data. It then calls the **SaveImage** method to store the image and its metadata to Microsoft Azure storage. 

1. Press **F5** to build and run the application and open the image gallery page in a browser window.

1. Enter metadata in the **Name**, **Description** and **Tags** text boxes. To select the image file, click **Browse**, navigate to **\\Source\\Assets\\Images**, and select one of the available images. 

	![Entering metadata to store with the image in blob storage](Images/entering-metadata-to-store.png?raw=true)
	
	_Entering metadata to store with the image in blob storage_
	
1. Click **Upload Image** to post the image to the Web application. The page refreshes and the newly added image displays in the list view. A status message shows the file name, content type and size of the uploaded file. Note that at this point, no metadata is displayed for the image. In the next task, you will implement the functionality required to retrieve and display metadata for blobs stored in Microsoft Azure.

	![The image gallery showing the uploaded image](Images/the-image-gallery-showing-the-uploaded-image.png?raw=true)
	
	_The image gallery showing the uploaded image_
	
1. In Visual Studio, press **SHIFT+F5** to stop debugging and delete the deployment from the Compute Emulator.

<a name="Ex2Task3"></a>
#### Task 3 – Retrieving Metadata for Blobs in Storage ####

Blobs can have metadata attached to them. Metadata headers can be set on a request that creates a new container or blob resource, or on a request that explicitly creates a property on an existing resource. In this task, you will add functionality to the image gallery page to retrieve and display metadata associated with images stored in a Microsoft Azure container.

1. Add an event handler to retrieve metadata for each blob displayed in the list view control that displays images. To do this, go to **Default.aspx**, right-click **View Designer**, select the **images ListView** control, and in the **Properties** Window (you may need to make it visible by right-clicking the control and choosing Properties) click the **Events** button. Locate the **ItemDataBound** event in the Data category, type **OnBlobDataBound** and press **ENTER**. Alternatively, you may edit the ASP.NET markup directly to insert the required event handler.

	![Configuring the event handler to display metadata](Images/configuring-the-event-handler-to-display-meta.png?raw=true)
	
	_Configuring the event handler to display metadata_
	
1. In the code-behind file, locate the **OnBlobDataBound** method and insert the following code (shown in **bold**) that retrieves the properties for each blob bound to the list view and creates a collection that contains name / value pairs for each metadata item found. The collection is then used as a data source for an **asp:Repeater** control that displays metadata for each image.

	(Code Snippet - _ExploringStorage-Ex2-08-OnBlobDataBoundMethod-CS_)
	<!--mark: 3-41-->
	````C#
	protected void OnBlobDataBound(object sender, ListViewItemEventArgs e)
	{
	  if (e.Item.ItemType == ListViewItemType.DataItem)
	  {
	    var metadataRepeater = e.Item.FindControl("blobMetadata") as Repeater;
	    var blob = ((ListViewDataItem)e.Item).DataItem as CloudBlob;
	    
	    // If this blob is a snapshot, rename button to "Delete Snapshot"
	    if (blob != null)
	    {
	      if (blob.SnapshotTime.HasValue)
	      {
	        var delBtn = e.Item.FindControl("deleteBlob") as LinkButton;
	        
	        if (delBtn != null)
	        {
	          delBtn.Text = "Delete Snapshot";
	          var snapshotRequest = BlobRequest.Get(new Uri(delBtn.CommandArgument), 0, blob.SnapshotTime.Value, null);
	          delBtn.CommandArgument = snapshotRequest.RequestUri.AbsoluteUri;
	        }
	        
	        var snapshotBtn = e.Item.FindControl("SnapshotBlob") as LinkButton;
	        if (snapshotBtn != null)
	        {
	          snapshotBtn.Visible = false;
	        }
	      }
	      
	      if (metadataRepeater != null)
	      {
	        // bind to metadata
	        metadataRepeater.DataSource = from key in blob.Metadata.AllKeys
	                                      select new
	                                      {
	                                        Name = key,
	                                        Value = blob.Metadata[key]
	                                      };
	        metadataRepeater.DataBind();
	      }
	    }
	  }
	}
	````
	
1. Press **F5** to build and run the application. Note that the list view now displays the metadata for the image that was uploaded in the previous exercise.

	![The image gallery showing metadata retrieved from blob storage](Images/the-image-gallery-showing-metadata-retrieved.png?raw=true)
	
	_The image gallery showing metadata retrieved from blob storage_
	
1. Press **SHIFT+F5** to stop debugging and delete the deployment from the compute emulator.

<a name="Ex2Task4"></a>
#### Task 4 – Deleting Blobs from Storage ####

In this task, you will add functionality to the image gallery Web page to delete blobs containing image data from Microsoft Azure storage.

1. Update the image list view to add an **asp:LinkButton** control that is used to delete images from the gallery container. To do this, right-click **Default.aspx**, and select **View Markup** and locate the **ItemTemplate** for the images **asp:ListView** control. Uncomment the ASP.NET markup located immediately following the **blobMetadata** repeater control (shown **bolded** below).

	<!--mark: 10-14-->
	````HTML
	...
	<div class="item">
	  <ul style="width:40em;float:left;clear:left" >
	    <asp:Repeater ID="blobMetadata" runat="server">
	      <ItemTemplate>
	        <li><%# Eval("Name") %><span><%# Eval("Value") %></span></li>
	      </ItemTemplate>
	    </asp:Repeater>
	    <li>
	      <asp:LinkButton ID="deleteBlob" 
	                      OnClientClick="return confirm('Delete image?');"
	                      CommandName="Delete" 
	                      CommandArgument='<%# Eval("Uri")%>'
	                      runat="server" Text="Delete" oncommand="OnDeleteImage" />
	      ...
	    </li>
	  </ul>
	  <img src="<%# Eval("Uri") %>" alt="<%# Eval("Uri") %>" style="float:left"/>
	</div>
	...
	````

1. Add code (shown in **bold**) to **Default.aspx.cs** to implement the command handler for the _deleteBlob_ **asp:LinkButton** control. The code verifies if a blob exists in storage and deletes it.

	(Code Snippet - _ExploringStorage-Ex2-09-OnDeleteImageMethod-CS_)
	<!--mark: 6-23-->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
	  ...
	  protected void OnDeleteImage(object sender, CommandEventArgs e)
	  {
	    try
	    {
	      if (e.CommandName == "Delete")
	      {
	        var blobUri = (string)e.CommandArgument;
	        var blob = this.GetContainer().GetBlobReference(blobUri);
	        blob.DeleteIfExists();
	      }
	    }
	    catch (StorageClientException se)
	    {
	      this.status.Text = "Storage client error: " + se.Message;
	    }
	    catch (Exception)
	    {
	    }
	    
	    this.RefreshGallery();
	  }
	  ...
	}
	````
	
1. Press **F5** to build and run the application. 

1. Upload a few more images from **Assets\Images** in the **Source** folder of the Lab and click **Delete** on any of the images displayed to remove the corresponding blob from storage.

	![Adding and deleting image blobs from storage](Images/adding-and-deleting-image.png?raw=true)
	
	_Adding and deleting image blobs from storage_
	
1. Press **SHIFT+F5** to stop debugging and delete the deployment from the Compute Emulator.

<a name="Ex2Task5"></a>
#### Task 5 – Copying Blobs ####

Microsoft Azure Blob service has support for making copies of existing blobs. In this task, you will add functionality to the image gallery Web page to copy blobs containing image data from Microsoft Azure storage that you added earlier.

1.	Update the image list view to add an **asp:LinkButton** control that is used to copy images from the gallery container. Open the **Default.aspx** page in Markup mode and locate the **ItemTemplate** for the images **asp:ListView** control. Uncomment the ASP.NET markup located immediately following the delete blob link button control (shown in **bold** text below.)

	<!--mark: 16-20-->
	````HTML
	...
	<div class="item">
	  <ul style="width:40em;float:left;clear:left" >
	    <asp:Repeater ID="blobMetadata" runat="server">
	      <ItemTemplate>
	        <li><%# Eval("Name") %><span><%# Eval("Value") %></span></li>
	      </ItemTemplate>
	    </asp:Repeater>
	    <li>
	      <asp:LinkButton ID="deleteBlob" 
	                      OnClientClick="return confirm('Delete image?');"
	                      CommandName="Delete" 
	                      CommandArgument='<%# Eval("Uri")%>'
	                      runat="server" Text="Delete" oncommand="OnDeleteImage" />
	
	      <asp:LinkButton ID="CopyBlob" 
	                      OnClientClick="return confirm('Copy image?');"
	                      CommandName="Copy" 
	                      CommandArgument='<%# Eval("Uri")%>'
	                      runat="server" Text="Copy" oncommand="OnCopyImage" />
	      ...
	    </li>
	  </ul>
	  <img src="<%# Eval("Uri") %>" alt="<%# Eval("Uri") %>" style="float:left"/>
	</div>
	...
	````

1. Add code (shown in **bold**) to **Default.aspx.cs** to implement the command handler for the _copyBlob_ **asp:LinkButton** control. The code creates a copy of a blob based on an existing blob. It also updates the *“ImageName”* attribute in its metadata to reflect that it is a copy.
 
	(Code Snippet - _ExploringStorage-Ex2-10-OnCopyImageMethod-CS_)
	<!--mark: 6-31-->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
	  ...
	  protected void OnCopyImage(object sender, CommandEventArgs e)
	  {
	    if (e.CommandName == "Copy")
	    {
	      // Prepare an Id for the copied blob
	      var newId = Guid.NewGuid();
	
	      // Get source blob
	      var blobUri = (string)e.CommandArgument;
	      var srcBlob = this.GetContainer().GetBlobReference(blobUri);
	
	      // Create new blob
	      var newBlob = this.GetContainer().GetBlobReference(newId.ToString());
	
	      // Copy content from source blob
	      newBlob.CopyFromBlob(srcBlob);
	
	      // Explicitly get metadata for new blob
	      newBlob.FetchAttributes(new BlobRequestOptions { BlobListingDetails = BlobListingDetails.Metadata });
	
	      // Change metadata on the new blob to reflect this is a copy via UI
	      newBlob.Metadata["ImageName"] = "Copy of \"" + newBlob.Metadata["ImageName"] + "\"";
	      newBlob.Metadata["Id"] = newId.ToString();
	      newBlob.SetMetadata();
	
	      // Render all blobs
	      this.RefreshGallery();
	    }
	  }
	  ...
	}
	````
	
1. Press **F5** to build and run the application.

	![Copying image blobs from storage](Images/copying-image-blobs-from-storage.png?raw=true)
	
	_Copying image blobs from storage_

1. Upload a few more images from **Source\Assets\Images** and click **Copy** on any of the images displayed to make a copy of the corresponding blob from storage.

1. Click **OK** to confirm the copy operation. You should see a copy of the image has been created with **ImageName** metadata stating it is a copy.

	![Verification](Images/verification.png?raw=true)
	
	_Verification_
	
1. Press **SHIFT+F5** to stop debugging and delete the deployment from the Compute Emulator.

<a name="Ex2Task6"></a>
#### Task 6 – Taking Blob Snapshots ####

Microsoft Azure Blob service has support for taking snapshots of blobs. The different between a snapshot and a copy is that snapshots are read-only and the original blob maintains a relationship to its snapshots; blob copies on the other hand are editable. Once a snapshot has been taken for a blob, this source blob can no longer be deleted. Before a source blob can be deleted, all of its snapshots must be deleted first.

In this task, you add functionality to take a snapshot of a blob that contains image data from Microsoft Azure storage.

1. Update the image list view to add an **asp:LinkButton** control that is used to snapshot images from the gallery container. Open the **Default.aspx** page in Markup mode and locate the **ItemTemplate** for the images **asp:ListView** control. Uncomment the ASP.NET markup located immediately following the copy blob link button control (shown in **bold**).

	<!--mark: 21-25-->
	````HTML
	<div class="item">
	  <ul style="width:40em;float:left;clear:left" >
	    <asp:Repeater ID="blobMetadata" runat="server">
	      <ItemTemplate>
	        <li><%# Eval("Name") %><span><%# Eval("Value") %></span></li>
	      </ItemTemplate>
	    </asp:Repeater>
	    <li>
	      <asp:LinkButton ID="deleteBlob" 
	                      OnClientClick="return confirm('Delete image?');"
	                      CommandName="Delete" 
	                      CommandArgument='<%# Eval("Uri")%>'
	                      runat="server" Text="Delete" oncommand="OnDeleteImage" />
	
	      <asp:LinkButton ID="CopyBlob" 
	                      OnClientClick="return confirm('Copy image?');"
	                      CommandName="Copy" 
	                      CommandArgument='<%# Eval("Uri")%>'
	                      runat="server" Text="Copy" oncommand="OnCopyImage" />
	
	      <asp:LinkButton ID="SnapshotBlob" 
	                      OnClientClick="return confirm('Snapshot image?');"
	                      CommandName="Snapshot" 
	                      CommandArgument='<%# Eval("Uri")%>'
	                      runat="server" Text="Snapshot" oncommand="OnSnapshotImage" />
	    </li>
	  </ul>
	  <img src="<%# Eval("Uri") %>" alt="<%# Eval("Uri") %>" style="float:left"/>
	</div>
	````

1. Add code (shown in **bold**) to **Default.aspx.cs** to implement the command handler for the _snapshotBlob_ **asp:LinkButton control**. The code gets the source blob and takes a snapshot of it.
 
	(Code Snippet - _ExploringStorage-Ex2-11-OnSnapshotImageMethod-CS_)
	<!--mark: 6-18-->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
	  ...
	  protected void OnSnapshotImage(object sender, CommandEventArgs e)
	  {
	    if (e.CommandName == "Snapshot")
	    {
	      // Get source blob
	      var blobUri = (string)e.CommandArgument;
	      var srcBlob = this.GetContainer().GetBlobReference(blobUri);
	
	      // Create a snapshot
	      var snapshot = srcBlob.CreateSnapshot();
	
	      this.status.Text = "A snapshot has been taken for image blob:" + srcBlob.Uri + " at " + snapshot.SnapshotTime;
	
	      this.RefreshGallery();
	    }
	  }
	  ...
	}
	````

1. Press **F5** to build and run the application.

1. Click **Snapshot** on any of the images displayed to take a snapshot the corresponding blob from storage.

	![Taking a snapshot of image blobs from storage](Images/taking-a-snapshot-of-image-blobs-from-storage.png?raw=true)
	
	_Taking a snapshot of image blobs from storage_
	
1. Click **OK** to confirm the snapshot operation. You will see a status update confirming that a snapshot has been taken.

1. Attempt to delete the **original** blob from which the snapshot was taken.

	![Attempting to delete the original blob](Images/cannot-delete-snapshot-error.png?raw=true)
	
	_Attempting to delete the original blob_
	
1.	You will see a status update confirms that the blob cannot be deleted.

	![Cannot Delete Snapshot error2](Images/cannot-delete-snapshot-error2.png?raw=true)
	
	_Cannot Delete Snapshot error_
	
	> **Note:** To delete a blob that contains snapshots, all of its snapshots must be deleted first (that functionality is not provided in this solution).

<a name="Exercise3"></a>
### Exercise 3: Working with Queues ###

In this exercise, you create a simple Web application to send messages to a Microsoft Azure queue. A Worker role in the solution retrieves the messages and writes them to the compute emulator log.

Queue service is a great way to send messages between front-end roles and worker roles. A queue can contain an unlimited number of messages, each of which can be up to 64 KB in size. Messages are pushed to the end of the queue and popped from the front of the queue.

<a name="Ex3Task1"></a>
#### Task 1 – Creating the Initial Solution ####

In this task, you create and configure the initial solution to work with queues in Microsoft Azure.

1. Open Visual Studio 2012 Express for Web elevated as **Administrator**.

1. From the **File** menu, choose **New Project**. 

1. In the **New Project** dialog, expand the **Visual C#** language in the Installed Templates list and select **Cloud**. Choose the **Microsoft Azure Cloud Service** project template, set the **Name** of the project to **RdStorage**, set the location to **Ex3-WorkingWithQueues\Begin** in the **Source** folder of the lab, change the solution name to **Begin**, and ensure that **Create directory for solution** is checked. Click **OK** to create the project.

	![Creating a WindowsAzure Project](Images/creating-a-windowsazure-project.png?raw=true)
	
	_Creating a WindowsAzure Project_
	
1. In the **New Microsoft Azure Project** dialog, select **ASP.NET Web Role** from the list of available roles and click the arrow **(>)** to add an instance of this role to the solution. Change the name of the role to **RdStorage_WebRole**.  To do this, select the role in the right panel, click the pencil icon and enter the new name.  Do not close the dialog. You will add a second role in the next step.

	![Adding a Web Role to the Microsoft Azure project](Images/adding-a-web-role-to-the-windows-azure-projec.png?raw=true)
	

	*Adding a Web Role to the Microsoft Azure project*
	
1. Next, add a second role to the solution. Choose a **Worker Role** and change its name to **RdStorage_WorkerRole**. 	
	
	![Adding a new Worker Role to the cloud service project ](Images/adding-a-new-worker-role-to-the-cloud-service.png?raw=true)
	
	*Adding a new Worker Role to the cloud service project*
	
1. Click **OK** to close the **New Microsoft Azure Project** dialog and create the solution.

1. Right-click each role under the **Roles** folder from the **RdStorage** cloud project.  Choose **Properties**.

1. In the **Settings** tab, click **Add Setting** and create a **ConnectionString** type called _DataConnectionString_. Click the button labeled  with an ellipsis and set the connection string to **Use storage emulator**. Repeat for each role in your project.
	
	![Creating a storage connection string](Images/creating-a-storage-connection-string.png?raw=true)
	
	_Creating a storage connection string_
	
	![Configuring a connection string to use Compute Emulator](Images/configuring-a-connection-string-to-use-comput.png?raw=true)
	
	_Configuring a connection string to use Compute Emulator_

<a name="Ex3Task2"></a>
#### Task 2 – Sending Messages to the Queue ####

In this task, you implement the **RdStorage_WebRole** web application to send messages to the queue.

1. Open the **Default.aspx** page of the **RdStorage_WebRole** web site. Delete the existing content inside the _BodyContent_ **Content** control.

1. Add an **asp:TextBox** control to the page. Change the **ID** for the **TextBox** to **txtMessage**. You may wish to format the page to look good, but this is not required. Also, add an **asp:Button** control just after the **TextBox** inserted previously. Change the **ID** of the button to **btnSend** and set the **Text** to **Send message**. Your code should look similar to the following:

	<!--mark: 20,21-->
	````C#
	<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RdStorage_WebRole._Default" %>

	<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Modify this template to jump-start your ASP.NET application.</h2>
            </hgroup>
            <p>
                To learn more about ASP.NET, visit <a href="http://asp.net" title="ASP.NET Website">http://asp.net</a>.
                The page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET.
                If you have any questions about ASP.NET visit
                <a href="http://forums.asp.net/18.aspx" title="ASP.NET Forum">our forums</a>.
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
		<asp:TextBox ID="txtMessage" runat="server"></asp:TextBox>
		<asp:Button ID="btnSend" runat="server" Text="Send message" />
</asp:Content>
	````

1. Open the code-behind file for the **Default.aspx** page. To do this, right-click **Default.aspx**, and select View Code.

1. Add the following namespace directives at the top of the file.

	(Code Snippet - _ExploringStorage-Ex3-01-Namespace-CS_)
	<!--mark: 1-3-->
	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.StorageClient;
	using Microsoft.WindowsAzure.ServiceRuntime;
	````

1. Right-click **Default.aspx**, select **View Designer** and double-click the **Send message** button. Alternatively, you may edit the ASP.NET markup directly to insert the required event handler. Add the following code (shown in **bold**) to the btnSend_Click event to initialize the account information:
	
	(Code Snippet - _ExploringStorage-Ex3-02-WebRoleCreateAccount-CS_)
	<!--mark: 6,7-->
	````C#
	public partial class _Default : System.Web.UI.Page
	{
	  ...
	  protected void btnSend_Click(object sender, EventArgs e)
	  {
	    // initialize the account information
	    var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
	  }
	}
	````

1. Next, add the following code (shown in **bold**) immediately after the code inserted in the previous step to obtain an instance of the **QueueStorage** helper object and create the message queue if it does not exist.

	(Code Snippet - _ExploringStorage-Ex3-03-WebRoleCreateQueue-CS_)
	<!--mark: 5-8-->
	````C#
	protected void btnSend_Click(object sender, EventArgs e)
	{
	  ...
	
	  // retrieve a reference to the messages queue
	  var queueClient = storageAccount.CreateCloudQueueClient();
	  var queue = queueClient.GetQueueReference("messagequeue");
	  queue.CreateIfNotExist();
	}
	````

1. Add the following code (shown in **bold**) immediately after the code inserted in the previous step to put the message entered by the user into the queue.

	(Code Snippet - _ExploringStorage-Ex3-04-WebRoleAddMessage-CS_)
	<!--mark: 5-8-->
	````C#
	protected void btnSend_Click(object sender, EventArgs e)
	{
	  ...
	
	  // add the message to the queue
	  var msg = new CloudQueueMessage(this.txtMessage.Text);
	  queue.AddMessage(msg);
	  this.txtMessage.Text = string.Empty;
	}
	````

1. Open the **WebRole.cs** file from the **RDStorage_WebRole** project.

1. Append the following namespace to the existing directives at the top of the code file.

	(Code Snippet - _ExploringStorage-Ex3-05-Namespace-CS_)
	<!--mark: 1-->
	````C#
	using Microsoft.WindowsAzure.StorageClient;
	````

<a name="Ex3Task3"></a>
#### Task 3 – Retrieving Messages from the Queue ####

In this task, you update the worker role to retrieve messages from the queue and show them in the compute emulator log.

1. Open the **Global.asax.cs** file from the **RdStorage_WebRole** project.

1. Add the following code (shown in **bold**) to the **Application_Start** method to initialize the account information.

	(Code Snippet - _ExploringStorage-Ex3-06-InitializeAccount-CS_)
	<!--mark: 6-9-->	
	````C#
	void Application_Start(object sender, EventArgs e)
	{
		// Code that runs on application startup
		BundleConfig.RegisterBundles(BundleTable.Bundles);
        AuthConfig.RegisterOpenAuth();	  
		CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
		{
			configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
		});
	}
	````

1. Make sure the following namespace directives exist on the top of the code files.

	(Code Snippet - _ExploringStorage-Ex3-07-Namespace-CS_)
	<!--mark: 1,2-->
	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.ServiceRuntime;
	````
	
1. Open the **WorkerRole.cs** file from the **RdStorage_WorkerRole** project.

1. Add the following highlighted code to the **OnStart** method directly above **return base.OnStart()** in WorkerRole.cs to initialize the account information.
 
	(Code Snippet - _ExploringStorage-Ex3-08-InitializeAccount-CS_)
	<!--mark: 9-12-->
	````C#
	public override bool OnStart()
	{
	  // Set the maximum number of concurrent connections 
	  ServicePointManager.DefaultConnectionLimit = 12;

	  // For information on handling configuration changes
	  // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
	  
	  CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
	  {
		configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
	  });

	  return base.OnStart();
	}
	````
	
1. Make sure the following namespace directives exist on the top of the code files.

	(Code Snippet - _ExploringStorage-Ex3-09-Namespace-CS_)
	<!--mark: 1-6-->
	````C#
	using System.Diagnostics;
	using System.Threading;
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.Diagnostics;
	using Microsoft.WindowsAzure.ServiceRuntime;
	using Microsoft.WindowsAzure.StorageClient;
	````

1. In the **Run** method, obtain an instance of the **QueueStorage** helper object and retrieve a reference to the _messages_ queue. To do this, add the following code (shown in **bold**) and remove the following lines of code (shown in ~~strikethrough~~) that simulate the worker role latency..

	(Code Snippet - _ExploringStorage-Ex3-10-WorkerGetQueue-CS_)
	<!--mark: 12-17; strike: 6-10-->
	````C#
	public override void Run()
	{
	  // This is a sample worker implementation. Replace with your logic.
	  Trace.WriteLine("RdStorage_WorkerRole entry point called", "Information");

	  while (true)
	  {
	    Thread.Sleep(10000);
	    Trace.WriteLine("Working", "Information");
	  }

	  // initialize the account information
	  var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

	  // retrieve a reference to the messages queue
	  var queueClient = storageAccount.CreateCloudQueueClient();
	  var queue = queueClient.GetQueueReference("messagequeue");
	}
	````
	
1. Next, add the following highlighted code to retrieve messages and write them to the compute emulator log. The message is then removed from the queue.

	(Code Snippet - _ExploringStorage-Ex3-11-WorkerGetMessages-CS_)
	<!--mark: 5-20-->
	````C#
	public override void Run()
	{
	  ...
	  
	  // retrieve messages and write them to the compute emulator log
	  while (true)
	  {
		Thread.Sleep(10000);

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
	
> **Note:** The worker process will try to get a message from the queue every 10 seconds using the GetMessage method. If there are messages in the queue, it will show them in the Compute Emulator log.

<a name="Ex3Verification"></a>
#### Verification ####

To test your service running in the compute emulator:

1. In Visual Studio, press **F5** to build and run the application.

	>**Note:** Make sure the cloud project is set as the startup project by right-clicking it in **Solution Explorer** and selecting **Set as StartUp Project**.

1. Open the compute emulator UI. To do this, right-click its icon located in the system tray and select **Show Compute Emulator UI**. (The icon is an azure colored Window.)

	![Showing the compute emulator UI](Images/showing-the-compute-emulator-ui.png?raw=true)

	_Showing the compute emulator UI_

1. Expand the tree to show the **Worker** instance log. 

1. Switch back to Windows Internet Explorer.  Ensure that the default page is shown, enter a message, and click **Send message**.

	![Default Web page](Images/default-web-page.png?raw=true)

	_Default Web page_

1. Change back to the Compute Emulator UI.  You should see the message logged in the worker role log.

	![Worker log showing the message](Images/worker-log-showing-the-message.png?raw=true)

	_Worker log showing the message_

	> **Note:** Because of the worker sleep time, it may take several seconds to show the message. 

<a name="Exercise4"></a>
### Exercise 4: Working with Drives ###
	
A Microsoft Azure Drive is an NTFS formatted virtual hard disk (VHD) file that is stored in a page blob. You can mount this VHD into a Microsoft Azure Compute instance to provide persistent storage exposed to applications via the Windows file system. The content of an Azure Drive will persist even if the compute role to which it is mounted is recycled.

In this exercise, you take an existing application that makes use of regular Windows file system APIs to access information in local disk storage and run it as a Microsoft Azure service.  You will see that by using a Microsoft Azure Drive, no changes to the code are necessary to run the same application in the Azure cloud and have it access information stored in Azure storage.

In the first part of the exercise, you execute the original application in the ASP.NET development server to familiarize yourself with its operation. Next, you create a cloud service project, associate the application as a Web role, and run it in the compute emulator using simulated Azure Drives. Finally, you create a VHD on your local machine, upload it to blob storage, deploy the application to Microsoft Azure and mount the drive in a Microsoft Azure instance.
	
<a name="Ex4Task1"></a>
#### Task 1 - Exploring the PhotoAlbum Application####

PhotoAlbum is a sample application that uses standard file system APIs to obtain a directory listing of the contents of its image store. In this task, you briefly examine the application and then configure the location of the image store to a folder in your machine to run the application locally.

1. Open Visual Studio 2012 Express for Web or higher in elevated administrator mode. 

	> **Note:** You are not required to use elevated administrator mode to run the application using the ASP.NET Web development server. However, you will need to do this later, when you launch the application in the compute emulator.

1. If the **User Account Control** dialog appears, click **Continue**.

1. Open the begin solution provided for this exercise. In the **File** menu, choose **Open Project**. Then, in the **Open Project** dialog, navigate to **\\Source\\Ex4-WorkingWithDrives\\Begin** and open the **Begin.sln** solution.

1.	Take a minute to browse the files in the application. First, double-click **Default.aspx** in **Solution Explorer** to open this file. Notice that the page uses a **GridView** control to display a directory listing of the image store and that its data source is a **LinqDataSource** control bound to a **PhotoAlbumDataSource** context object.

1.	Next, open the **PhotoAlbumDataSource.cs** file to examine the context object class used by the **LinqDataSource**.  Notice that the **Files** property used by the data source control returns a collection of [FileInfo][7] objects and that it uses standard file system APIs to enumerate PNG and JPEG files in the image store directory. 

[7]:http://msdn.microsoft.com/en-us/library/system.io.fileinfo.aspx

1. Now, open the **Global.asax** file and see that it contains an **ImageStorePath** property that returns the location of the image store folder and that the **Application_Start** event handler initializes this property with a path that it retrieves from the application settings.

1. Before you run the application, you need to configure the location of the image store. In **Solution Explorer**, double-click **Web.config** to open this file in the text editor. In the **appSettings** section, locate the _ImageStorePath_ setting and update its value to the path of the **Sample Pictures** folder in your machine.

	![Configuring the location of the image store](Images/configuring-the-location-of-the-image-store.png?raw=true)

	_Configuring the location of the image store_


	> **Note:** The sample pictures library is typically available in most Windows installations and is located at _"%PUBLIC%\Pictures\Sample Pictures"_. It contains a small set of image files. If this folder is not available in your environment, you may substitute it with any folder that contains a suitable collection of JPEG or PNG image files, for example, those found in _Assets\Images_ in the _Source_ folder of this lab.

	> Note that _%PUBLIC%_ is an environment variable that points to the location of _Public_ in the User profiles folder. You must expand the variable to the proper value when you configure the _ImageStorePath_ setting (e.g. _C:\\Users\\Public\\Pictures\\Sample Pictures_).

1. Press **F5** to build and run the PhotoAlbum application. Notice that the default page shows a listing of the files contained in the image store. Also, notice that the path to the image store is the folder on your machine that you set up in the application configuration file.

	>**Note:** Make sure the cloud project is set as the startup project by right-clicking it in **Solution Explorer** and selecting **Set as StartUp Project**.

	![Running the PhotoAlbum application locally ](Images/running-the-photoalbum-application-locally.png?raw=true)

	_Running the PhotoAlbum application locally_

1. Close the browser window. You have now seen that the application uses standard file system APIs to access the files in its store. In the next task, you will update the application to run as a cloud service.

<a name="Ex4Task2"></a>
#### Task 2 - Using a Microsoft Azure Drive to Move the Application to the Cloud####

When moving the application to Microsoft Azure, the natural choice is to relocate the image store to blob storage. Regardless, the application expects its images to be stored in the file system. You can always update the code that accesses the images in the store and change it to use the Blob service APIs instead. For a simple application such as this one, this would not be too challenging, but it can represent a barrier for a more complex application. Using Microsoft Azure Drives enables you to move the application to the cloud without any changes to its code, other than mounting the drive on a page blob.

In this task, you update the application to run as a Microsoft Azure cloud service and to use a Microsoft Azure Drive for its image store.

1. Add a cloud service project to the solution. In **Solution Explorer**, right-click the root solution node, point to **Add** and then select **New Project**. 

1. In the **Add** **New Project** dialog, expand **Visual C#** in the **Installed Templates** list and select **Cloud**. Choose the **Microsoft Azure Cloud Service** template, set the **Name** of the project to **PhotoAlbumService**, leave the proposed location unchanged and then click **OK**.

	![Creating a Microsoft Azure Project ](Images/creating-a-windows-azure-project.png?raw=true)

	_Creating a Microsoft Azure Project_

1. In the **New Microsoft Azure Project** dialog, click **OK** without adding any new roles. You will use the existing application as a web role.

1. Add a reference to the Microsoft Azure support assemblies. In **Solution Explorer**, right-click the **PhotoAlbum** project and select **Add Reference**. In the **Reference Manager** dialog, search for **Microsoft.WindowsAzure.CloudDrive**, **Microsoft.WindowsAzure.Diagnostics**, **Microsoft.WindowsAzure.ServiceRuntime**, and **Microsoft.WindowsAzure.StorageClient** assemblies, select them and then click **OK**.

	![Adding a reference to the Microsoft Azure support assemblies](Images/adding-a-reference-to-the-windows-azure-suppo.png?raw=true)

	_Adding a reference to the Microsoft Azure support assemblies_

	
1. Now, in **Solution Explorer**, right-click the **Roles** node in the **PhotoAlbumService** project, point to **Add** and then select **Web Role Project in solution**.

1. In the **Associate with Role Project** dialog, select the **PhotoAlbum** project and click **OK**.

1. You will now configure the web role. To do this, double-click the **PhotoAlbum** role under the **Roles** node in the **PhotoAlbumService** project.

1. In the **PhotoAlbum \[Role\]** properties window, switch to the **Settings** tab and then click **Add Setting**. Set the **Name** of the new setting to _DataConnectionString_, set the **Type** as _Connection String_, and then click the button labeled with an ellipsis located on the right side of the **Value** column. In the **Storage Connection String** dialog, choose the option labeled **Microsoft Azure storage emulator** and click **OK**.

1. Add a second setting to configure the URL of the cloud drive in blob storage. For this setting, set the **Name** to _ImageStoreBlobUri_, the **Type** as _String_, and the **Value** as _mydrives/SamplePictures.vhd_. 

	> **Note:** The _ImageStoreBlobUri_ setting identifies a Blob service URI and is case sensitive. Make sure that you enter the value exactly as shown.

	![Configuring the Web role settings](Images/configuring-the-web-role-settings.png?raw=true)

	_Configuring the Web role settings_

1. Switch to the **Local Storage** tab and then click **Add Local Storage**. Set the **Name** of the new storage setting to _LocalDriveCache_, set the **Size** to _120_ and leave **Clean on Role Recycle** unselected.

	![Configuring local storage for the Web role to cache Azure drive contents](Images/configuring-local-storage-for-the-web-role-to.png?raw=true)

	_Configuring local storage for the Web role to cache Azure drive contents_

	> **Note:** When **Clean on Role Recycled** is disabled, the contents of the cache will persist even if the role instance is recycled.
	
	> Due to the way that the system allocates local storage resources, part of the requested space is unavailable for cache usage. Beginning with Microsoft Azure Guest OS 1.8, the entire Microsoft Azure Drive cache is allocated up-front and if there is not enough space in the local resource for the cache, the call to Mount will fail. To avoid this issue when configuring the size of the local resource destined for the drive cache, specify an additional 20MB to the required size. For example, to use a 1000MB cache size, set the size of the local resource to 1020MB.

	
1. Press **CTRL+S** to save the changes to the role configuration.

1. Configure a trace listener to output diagnostics information to the Microsoft Azure log. To do this, double-click **Web.config** in **Solution Explorer** to open this file and insert the following **system.diagnostics** section into the configuration after configSections, as shown below.

	(Code Snippet - _ExploringStorage-Ex4-01-DiagnosticMonitorTraceListener_)
	<!--mark: 5-14-->
	````XML
	<configuration>
	  <configSections>
		...
	  </configSections>
	  <system.diagnostics>
		<trace>
			<listeners>
			<add type="Microsoft.WindowsAzure.Diagnostics.DiagnosticMonitorTraceListener, Microsoft.WindowsAzure.Diagnostics, Version=1.8.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
          name="AzureDiagnostics">
				<filter type="" />
			</add>
			</listeners>
		</trace>
	  </system.diagnostics>
	  ...
	</configuration>
	````
	
1. Add a class to the Web role project to manage its initialization and shutdown. In **Solution Explorer**, right-click the **PhotoAlbum** project, point to **Add** and then select **Existing Item**. In the **Add Existing Item** dialog, browse to **Ex4-WorkingWithDrives\Assets**, select **WebRole.cs** and then click **Add**.

	> **Note:** The **WebRole.cs** file contains a standard **RoleEntryPoint** derived class, similar to the one generated when you select a new Microsoft Azure Web Role project template in Visual Studio.


1. Locate the **Application_Start** method in **Global.asax.cs** file and insert the following code (shown in **bold**) to this method. 

	(Code Snippet - _ExploringStorage-Ex4-02-ApplicationStartMethod-CS_)
	<!--mark: 12-59-->
	````C#
	protected void Application_Start(object sender, EventArgs e)
{
		// Code that runs on application startup
		BundleConfig.RegisterBundles(BundleTable.Bundles);
		AuthConfig.RegisterOpenAuth();

		if (imageStorePath == null)
		{
			ImageStorePath = WebConfigurationManager.AppSettings["ImageStorePath"];
		}
		
		// initialize storage account configuration setting publisher
		CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
		{
			string connectionString = RoleEnvironment.GetConfigurationSettingValue(configName);
			configSetter(connectionString);
		});

		try
		{
			// initialize the local cache for the Azure drive
			LocalResource cache = RoleEnvironment.GetLocalResource("LocalDriveCache");
			CloudDrive.InitializeCache(cache.RootPath + "cache", cache.MaximumSizeInMegabytes);

			// retrieve storage account 
			CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

			// retrieve URI for the page blob that contains the cloud drive from configuration settings 
			string imageStoreBlobUri = RoleEnvironment.GetConfigurationSettingValue("ImageStoreBlobUri");

			// unmount any previously mounted drive.
			foreach (var drive in CloudDrive.GetMountedDrives())
			{
				var mountedDrive = new CloudDrive(drive.Value, account.Credentials);
				mountedDrive.Unmount();
			}
  
			// create the Microsoft Azure drive and its associated page blob
			CloudDrive imageStoreDrive = account.CreateCloudDrive(imageStoreBlobUri);

			if (CloudDrive.GetMountedDrives().Count() == 0)
			{                  
				try
				{
					imageStoreDrive.Create(16);
				}
				catch (CloudDriveException)
				{
					// drive already exists
				}
			}

			// mount the drive and initialize the application with the path to the image store on the Azure drive
			Global.ImageStorePath = imageStoreDrive.Mount(cache.MaximumSizeInMegabytes / 2, DriveMountOptions.None);
		}
		catch (CloudDriveException driveException)
		{
			Trace.WriteLine("Error: " + driveException.Message);
		}
	}
	````

	> **Note:** The preceding code retrieves the path to the local storage that you defined earlier, when you configured the Web role, and then uses this path to initialize the drive cache and set the maximum amount of local disk space it will use. Next, it creates a **CloudDrive** object specifying the URL of the page blob, which you also defined earlier in the role configuration settings. Finally, it mounts the formatted page blob to a drive letter for the Microsoft Azure application to start using. 
	
	> Notice that the cache assigned to the drive is only half the total amount of storage reserved for the cache. Later in the exercise, you will create a second drive and assign the remainder to that drive.
	
1. Make sure the following namespace directives exist at the top of the **Global.asax.cs** code file.

	(Code Snippet - _ExploringStorage-Ex4-03-GlobalNamespace-CS_)
	<!--mark: 1-4-->
	````C#
	using System.Diagnostics;
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.ServiceRuntime;
	using Microsoft.WindowsAzure.StorageClient;
	````
 
1. Next, insert the following highlighted code into the **Application_End** method to unmount the Microsoft Azure Drive when the Web role shuts down.  Place the code at the start of the method.
	
	(Code Snippet - _ExploringStorage-Ex4-04-ApplicationEndMethod-CS_)
	<!--mark: 3-7-->
	````C#
	protected void Application_End(object sender, EventArgs e)
	{
		// obtain a reference to the cloud drive and unmount it
		CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
		string imageStoreBlobUri = RoleEnvironment.GetConfigurationSettingValue("ImageStoreBlobUri");
		CloudDrive imageStoreDrive = account.CreateCloudDrive(imageStoreBlobUri);
		imageStoreDrive.Unmount();
	}
	````

	> **Note:** The code above retrieves a reference to the previously mounted cloud drive and then unmounts it. 

1. The application is now ready to run as a Microsoft Azure service. Press **F5** to build and launch the application in the Compute Emulator. When the application starts, it displays the contents for the Microsoft Azure Drive, which is initially empty. Notice that the page shows that the Image Store Drive is mapped to a drive letter. Keep the browser window open for the moment.

	![Running the application in the compute emulator with an empty image store drive](Images/running-the-application-in-the-compute-emulat.png?raw=true)

	_Running the application in the compute emulator with an empty image store drive_
	
1. Next, you determine the location of the folder used by storage emulator to simulate the cloud drive. To display the compute emulator UI, right-click the Microsoft Azure tray icon and then select **Show Storage Emulator UI**.

	![Showing the Compute Emulator UI](Images/viewing-the-status-of-storage-emulator.png?raw=true)

	_Showing the Storage Emulator UI_

1. In the Storage Emulator UI, open the **File** menu and select **Open Azure Drive Folder in Windows Explorer**. 	

	![Opening the Azure Drive simulation folder](Images/opening-the-azure-drive-simulation-folder.png?raw=true)

	_Opening the Azure Drive simulation folder_

	> **Note:** When running locally, the storage emulator does not use blob storage to simulate the cloud drive. Instead, it maps the drive to a local folder. From the storage emulator UI, you can open a Windows Explorer window pointing at the temporary folder used by storage emulator to store simulated Microsoft Azure drives.

1. Inside the Azure Drive folder, navigate to **devstoreaccount1\mydrives\SamplePictures.vhd**. Note that this path matches the URI of the blob in the storage emulator.

1. Now, open the **Start** menu, search for **Pictures** to open your pictures library and then double-click the **Sample Pictures** folder to open a window with sample image files.

	> **Note:** The sample pictures library is typically present in most Windows installations and is located at _"%PUBLIC%\\Pictures\\Sample Pictures"_. It contains a small set of image files. If this folder is not available in your environment, you may substitute it with any folder that contains a suitable collection of JPEG or PNG image files, for example, those found in _Assets\\Images_ in the _Source_ folder of this lab.

	![Sample pictures](Images/sample-pictures-library-in-windows.png?raw=true)

	_Sample pictures_

1. Copy one or more files from the pictures library to the simulated cloud drive that you determined previously.

1. Switch back to the browser window showing the contents of the image store and refresh the page. Notice that the updated page shows the files that you copied in the previous step.

	![Application showing the updated contents of the Microsoft Azure Drive](Images/application-showing-the-updated-contents-of-t.png?raw=true)

	_Application showing the updated contents of the Microsoft Azure Drive_

1. Close the browser window.

<a name="Ex4Task3"></a>
#### Task 3 - Creating a New Drive in the Cloud####

In this task, you update the application to create a new drive in the cloud, mount it, and then copy the contents of the original drive into it.

1. In **Solution Explorer**, right-click **Default.aspx** and then select **View Markup**.

1. Insert the highlighted markup inside the body of the page, between the **h1** and **h2** heading tags, as shown below.
	
	(Code Snippet - _ExploringStorage-Ex4-05-MountedDrives Panel_)
	<!--mark: 9-15-->
	````HTML
	...
	<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
		<style type="text/css">
			...
		</style>
	  
		<div>
			<h1>PhotoAlbum</h1>        
			<asp:Panel ID="SelectDrive" runat="server" Visible="false">
			<asp:LinkButton ID="NewDrive" runat="server" Text="New Drive" onclick="NewDrive_Click" CssClass="newdrive" />
			Mounted Drives: 
			<asp:DropDownList ID="MountedDrives" runat="server" AutoPostBack="true"
							DataTextField="Name" DataValueField="Value"
							OnSelectedIndexChanged="MountedDrives_SelectedIndexChanged" />
			</asp:Panel>
			<h3>Image Store Drive: (<%=this.CurrentPath%>)</h3>
			<asp:GridView DataSourceID="LinqDataSource1" AutoGenerateColumns="False" 
	...
	````

	> **Note:** The markup above displays a drop down list that enumerates the drives that the web role has mounted as well as a link button that triggers the creation of a new Microsoft Azure drive.


1. In **Solution Explorer**, right-click **Default.aspx** and then select **View Code** to open its code-behind file.

1. Add the following namespace directives at the top of the file to declare the Microsoft Azure supporting assemblies.
	
	(Code Snippet - _ExploringStorage-Ex4-06-AzureNamespaces-CS_)
	<!--mark: 1-4-->
	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.Diagnostics;
	using Microsoft.WindowsAzure.ServiceRuntime;
	using Microsoft.WindowsAzure.StorageClient;
	````

1. Locate the **Page_PreRender** method and insert the following highlighted code at the end of the method, as shown below.
	
	(Code Snippet - _ExploringStorage-Ex4-07-Page_PreRender-CS_)
	<!--mark: 5-19-->
	````C#
	protected void Page_PreRender(object sender, EventArgs e)
	{
	  this.GridView1.Columns[this.GridView1.Columns.Count - 1].Visible = this.CurrentPath != Global.ImageStorePath;
	  
	  if (RoleEnvironment.IsAvailable)
	  {
	    this.MountedDrives.DataSource = from item in CloudDrive.GetMountedDrives()
										select new
										{
											Name = item.Key + " => " + item.Value,
											Value = item.Key
										};
	   
		this.MountedDrives.DataBind();
		this.MountedDrives.SelectedValue = this.CurrentPath;
		this.SelectDrive.Visible = true;
	   
		this.NewDrive.Text = this.MountedDrives.Items.Count < 2 ? "New Drive" : "Delete Drive";
	  }
	}
	````
	
	>**Note:** The preceding code populates a drop down list with the drives currently mounted by the web role. The drop down shows the mapping between the page blob URI and the corresponding drive letter for the mounted drive.
 
1. Add code to implement an event handler for the **New Drive** link button. To do this, paste the following code into the **_Default** class.
	
	(Code Snippet - _ExploringStorage-Ex4-08-NewDrive_Click-CS_)
	<!--mark: 1-45-->
	````C#
	protected void NewDrive_Click(object sender, EventArgs e)
	{
	  if (RoleEnvironment.IsAvailable)
	  {
		// retrieve storage account
		CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

		// build page blob URI for the new cloud drive by changing the extension in the original URI
		string imageStoreBlobUri = RoleEnvironment.GetConfigurationSettingValue("ImageStoreBlobUri");
		string cloneStoreBlobUri = Path.ChangeExtension(imageStoreBlobUri, "bak");                

		// create drive and its associated page blob
		CloudDrive clonedDrive = account.CreateCloudDrive(cloneStoreBlobUri);
		if (this.MountedDrives.Items.Count < 2)
		{
		  try
		  {
			clonedDrive.Create(16);
		  }
		  catch (CloudDriveException)
		  {
			// cloud drive already exists
		  }

		  // mount the drive and retrieve its path
		  LocalResource cache = RoleEnvironment.GetLocalResource("LocalDriveCache");
		  string clonedStorePath = clonedDrive.Mount(cache.MaximumSizeInMegabytes / 2, DriveMountOptions.None);

		  // copy the contents from the original drive to the new drive                
		  foreach (string sourceFileName in Directory.GetFiles(Global.ImageStorePath, "*.*").Where(name => name.EndsWith(".jpg") || name.EndsWith(".png")))
		  {
			string destinationFileName = Path.Combine(clonedStorePath, Path.GetFileName(sourceFileName));
			File.Copy(sourceFileName, destinationFileName, true);
		  }

		  this.SelectImageStore(clonedStorePath);
		}
		else
		{
		  clonedDrive.Unmount();
		  clonedDrive.Delete();
		  this.SelectImageStore(Global.ImageStorePath);
		}
	  }
	}
	````
	
	>**Note:** The preceding code checks if it has already mounted a second drive and if not, it creates a new NTFS formatted cloud drive and its associated page blob. It then copies the contents of the originally mounted drive into this second drive. If the drive already exists, it first unmounts it and then deletes it.

1. Finally, add an event handler for the **SelectedIndexChanged** event of the mounted drives drop down list. To do this, insert the following method into the **_Default** class.
	
	(Code Snippet - _ExploringStorage-Ex4-09-MountedDrives_SelectedIndexChanged-CS_)
	<!--mark: 1-4-->
	````C#
	protected void MountedDrives_SelectedIndexChanged(object sender, EventArgs e)
	{
	  this.SelectImageStore(this.MountedDrives.SelectedValue);
	}
	````
 

	>**Note:** The preceding code triggers a redirection that will refresh the contents of the page and show the directory of the newly selected drive.

1. You are now ready to test the changes to the solution. Press **F5** to launch the application in the compute emulator. When the applications starts, notice that the **Mounted Drives** drop down list shows the letter assigned to the drive and the corresponding URL of the page blob that provides backing storage.

	![Application showing currently mounted drives](Images/application-showing-currently-mounted-drives.png?raw=true)

	_Application showing currently mounted drives_

1. Click the **New Drive** link to create a new drive and copy the contents of the original drive into it. After you create the drive, the page refreshes and shows the contents of the new drive. Notice that the drop down list of mounted drives now shows the second drive.

	![Application showing the newly created drive](Images/application-showing-the-newly-created-drive.png?raw=true)

	_Application showing the newly created drive_

1. Delete one or more files in the new drive by clicking the **Delete** link.

	![Deleting files in the second drive](Images/deleting-files-in-the-second-drive.png?raw=true)

	_Deleting files in the second drive_

1.	Now, choose the original drive in the **Mounted Drives** drop down list and verify that its contents are intact.
1. Finally, click **Delete Drive** to unmount the drive and delete its corresponding page blob.

	![Unmounting the drive and deleting it](Images/unmounting-the-drive-and-deleting-it.png?raw=true)

	_Unmounting the drive and deleting it_

1. Close the browser window. You will now deploy and test the application in Microsoft Azure.

<a name="Ex4Task4"></a>
#### Task 4 - Creating an NTFS Formatted VHD on Your Local Machine####

So far, you have explored Microsoft Azure Drives using the simulated environment provided by the compute emulator. When you deploy the application to the Microsoft Azure environment, you need a mechanism to upload the information used by the application to blob storage. One alternative is creating a Virtual Hard Drive (VHD) locally in your machine, copying the required information, and then uploading the VHD file to a Microsoft Azure page blob.

In this task, you create an NTFS-formatted VHD file to contain sample images that you can upload to Microsoft Azure Storage and then use as the backing store for a Microsoft Azure Drive.

>**Note:** This task is optional and is dependent on a feature that is currently available only in Windows 7 and Windows Server 2008 R2. If you do not have these operating system versions, you may skip this task and use the pre-built VHD included in the lab's _Assets_ folder instead.
 
>**Important:** To complete the remaining tasks in this exercise, you require a valid Microsoft Azure subscription.
For more information, visit the [Microsoft Azure Portal][9].

[9]:http://www.microsoft.com/windowsazure/

	
1. Open the **Disk Management** console. To do this, press **Start**, type **diskmgmt.msc**, and then click **diskmgmt.msc**.

	![Launching the Disk Management console](Images/launching-the-disk-management-console.png?raw=true)

	_Launching the Disk Management console_

	
1. In the Disk Management console, open the **Action** menu and select **Create VHD**.

1. In the **Create and Attach Virtual Hard Disk** dialog, click **Browse** and navigate to **\\Source\\Ex4-WorkingWithDrives**, set the file name to **SamplePictures.vhd** and click **Save**. Next, set the **Virtual hard disk size** to _16 MB_,  the **Virtual hard disk format** to **VHD** and the **Virtual hard disk type** to **Fixed size**, and then click **OK** to create and attach the virtual hard disk.

	![Creating a virtual hard disk (VHD)](Images/creating-a-virtual-hard-disk-vhd.png?raw=true)

	_Creating a virtual hard disk (VHD)_


1. Before you can use the new disk, you need to initialize it. To do this, right-click the disk icon for the newly created disk in the lower pane of the **Disk Management** console and select **Initialize Disk**.

	![Initializing the virtual hard disk (VHD)](Images/initializing-the-virtual-hard-disk-vhd.png?raw=true)

	_Initializing the virtual hard disk (VHD)_

1. In the **Initialize Disk** dialog, make sure to select the disk that corresponds to the attached VHD, set the partition style to **MBR (Master Boot Record)**, and then click **OK**.

	![Configuring disk initialization options](Images/configuring-disk-initialization-options.png?raw=true)

	_Configuring disk initialization options_

	
1. Next, right-click the partition area in the attached virtual hard disk identified as unallocated and select **New Simple Volume**.

	![Creating a volume in the virtual hard disk (VHD)](Images/creating-a-volume-in-the-virtual-hard-disk-vh.png?raw=true)

	_Creating a volume in the virtual hard disk (VHD)_

1. In the **New Simple Volume Wizard**, click **Next** to dismiss the Welcome page.

	![New Simple Volume Wizard welcome page](Images/new-simple-volume-wizard-welcome-page.png?raw=true)

	_New Simple Volume Wizard welcome page_

1. Leave the **Simple volume size** unchanged-it should match the **Maximum disk space**-and click **Next** to proceed.

	![Specifying the size of the disk volume](Images/specifying-the-size-of-the-disk-volume.png?raw=true)

	_Specifying the size of the disk volume_

1. Next, assign a suitable drive letter and click **Next**.

	![Assigning a drive letter to the volume](Images/assigning-a-drive-letter-to-the-volume.png?raw=true)

	_Assigning a drive letter to the volume_

1. Now, choose to format the new partition. Set the **File system** to _NTFS_, leave the default **Allocation unit size** unchanged and set the **Volume label** to _SamplePictures_. Make sure that you check the option labeled **Perform a quick format** and leave the **Enable file and folder compression** option turned off. Click **Next** to proceed.

	![Formatting the new disk partition](Images/formatting-the-new-disk-partition.png?raw=true)

	_Formatting the new disk partition_

	> **Note:** Currently, Microsoft Azure Drives require an NTFS-formatted Virtual Hard Drive (VHD). 

1. Finally, verify the information presented by the wizard in its summary screen and click **Finish** to create the new volume.

	![Completing the New Simple Volume Wizard](Images/completing-the-new-simple-volume-wizard.png?raw=true)

	_Completing the New Simple Volume Wizard_

	
1. Wait for the formatting process to complete-it should only take a few seconds. If enabled, **AutoPlay** then prompts you to view the files in the newly attached disk. If that is the case, click **Open folder to view files**. Otherwise, right-click the volume in the Disk Management console and select **Open** to show the contents of the new VHD drive. Leave this new window open for the time being.

1. Now, open the **Start** menu, select **Pictures** to open your pictures library and then browse to the **Sample Pictures** folder to view the sample image files.
	
	>**Note:** If this folder is not available in your environment, you may substitute it with any folder that contains a suitable collection of JPEG or PNG image files, for example, those found in _Assets\\Images_ in the _Source_ folder of this lab.
 
1. Next, copy and paste all the files from the **Sample Pictures** folder into the first window to copy all the images contained in this folder into the VHD drive. If the drive becomes full, do not worry. You only require a few files to test the application.

1. Switch back to the Disk Management console, right-click the attached disk-make sure to point to the disk and not the partition area-and then select **Detach VHD**.

	
	![Preparing to detach the virtual hard disk (VHD) file](Images/preparing-to-detach-the-virtual-hard-disk-vhd.png?raw=true)

	_Preparing to detach the virtual hard disk (VHD) file_

1. In the **Detach Virtual Hard Disk** dialog, make sure not to check the option labeled **Delete the virtual hard disk file after removing the disk** and then click **OK**.

	![Detaching a virtual hard disk](Images/detaching-a-virtual-hard-disk.png?raw=true)

	_Detaching a virtual hard disk_

1. You are now ready to upload the virtual hard disk (VHD) file to Microsoft Azure Storage.

<a name="Ex4Task5"></a>
#### Task 5 - Deploying the Application and Uploading the Drive to Microsoft Azure####

In this task, you upload the NTFS-formatted Virtual Hard Drive (VHD) created previously to a Microsoft Azure Page Blob. The lab material includes a tool that you can use for this purpose.

1. Before you access your Microsoft Azure Storage account to upload the VHD file, you need to determine the name and primary key of the account. To obtain your storage account information, sign in at the Microsoft Azure Management Portal <http://manage.windowsazure.com/>.

1. Click **Storage** and then select your account from the list. Take note of the account **name**.

	![Viewing Microsoft Azure storage accounts](Images/viewing-windows-azure-storage-accounts.png?raw=true "Viewing Microsoft Azure storage accounts")

	_Viewing storage accounts_

1. Click **Manage Keys** within the bottom menu and take note of the storage account's **Primary Key** (you will use this value later in this exercise).

	![Manage keys](Images/manage-keys.png?raw=true "Manage keys")

	_Manage Keys_

	![Viewing Microsoft Azure storage account information](Images/viewing-windows-azure-storage-account-informa.png?raw=true)

	_Viewing Microsoft Azure storage account information_

1. Next, open a command prompt and change the current directory to _\\Source\\Assets\\VHDUpload_.

1. At the command prompt, type the following command line replacing _\<vhdFilePath\>_ with the path to the VHD file created in task 4, and **\<accountName\>** and **\<accountKey\>** with the name and the primary access key of your Microsoft Azure storage account , respectively. 

	>**Note:** If you are currently on a platform that does not support creating and mounting VHD files and you were unable to complete the previous task, use the **SamplePictures.vhd** file located in the same folder as the tool instead.

	<!--mark: 1-->
	````CommandPrompt
	VHDUPLOAD <vhdFilePath> mydrives/SamplePictures.vhd <accountName> <accountKey>
	````

	>**Note:** The Blob service URI that specifies the location of the .vhd file in blob storage is case sensitive. Make sure that you enter the value exactly as shown.	 	

1. Press **Enter** to start uploading the virtual hard disk (VHD) file to blob storage. Wait for the process to complete; it may take several minutes

	![Uploading a VHD file to a Microsoft Azure page blob](Images/uploading-a-vhd-file-to-a-windows-azure-page.png?raw=true)

	_Uploading a VHD file to a Microsoft Azure page blob_

1. Now that you have uploaded the VHD file to Azure Storage, you are ready to deploy the application.

1. Switch back to Visual Studio.

1. First, you need to update the web role configuration with your storage account information. To do this, expand the **Roles** node in the **PhotoAlbumService** project and double-click the **PhotoAlbum** role to open its properties window. 

1. In the **PhotoAlbum \[Role\]** window, switch to the **Settings** tab, locate the _Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString_ setting and then click the button labeled with an ellipsis, on the right edge of the row. In the **Storage Connection String** dialog, choose **Manually entered credentials** and enter the **Account name** and **Account key** for your Microsoft Azure storage account. In the **Connection** area, make sure that the **Use HTTP** option is selected, and click **OK**.

1. Repeat the previous step to configure the _DataConnectionString_ setting. Enter the same information used previously except for the **Connection** setting, which should be set to **Use HTTP**.

	![Configuring the storage account for the cloud drive to use HTTP endpoints](Images/configuring-the-storage-account-for-the-cloud.png?raw=true)

	_Configuring the storage account for the cloud drive to use HTTP endpoints_

	> **Note:** Microsoft Azure Drives only support HTTP endpoints currently.

1. Now that you have configured the application with the settings for your storage account, you can proceed to deploy it. To create a service package, right-click the **PhotoAlbumService** cloud service project and select **Package**. 

	>**Note:** For additional information about deploying your application to Microsoft Azure, see the **Microsoft Azure Deployment** hands-on lab in this training kit. In particular, Exercise 3 of this lab discusses deploying applications from Visual Studio.

	![Creating a service package in Visual Studio](Images/creating-a-service-package-in-visual-studio.png?raw=true)

	_Creating a service package in Visual Studio_

1. In the **Package Microsoft Azure Application** dialog box, click **Package** to generate the package. This opens Windows Explorer to a folder within your solution folders that contains the generated package. Although you could use the integrated deployment feature in the Microsoft Azure Tools to publish the service to Microsoft Azure directly from Visual Studio, in this lab, you deploy it using the Microsoft Azure Management Portal.

1. To deploy the service package, go to the [Management Portal][10] and sign in. 

[10]:https://manage.windowsazure.com/

1. At the portal, select the project where you will deploy your application. If you have not previously created a service, you will need to create one at this time; otherwise, you may use an existing service. In that case, skip the following steps and go directly to step 20. 

1. Create the compute component that executes the application code. To do this, click **New** | **Compute** | **Cloud Service** | **Custom Create**.

	![Creating a new cloud service](Images/creating-a-new-cloud-service.png?raw=true "Creating a new cloud service")

	_Creating a new cloud service_

1. In the **Create a cloud service** dialog, select a unique **URL** prefix for your service and then choose an available **Region/Affinity Group** from the list.

1. Select **Deploy a Cloud Service package now** and continue to the next step.

	![Create Your Cloud Service](Images/create-your-cloud-service.png?raw=true "Create Your Cloud Service")

	_Create a cloud service_

1. In the **Publish your cloud service** page, select a **Deployment Name** and then, select the Package **(.cspkg)** and Configuration **(.cscfg)** files that you generated in Visual Studio: you can find them in the file system location indicated by the Windows Explorer window that opened when you published the package.

1. Finally, set the **Environment** where you want to deploy the application _(Production or Staging)_, select **Deploy even if one or more roles contain a single instance** and finish the wizard.

	![Publish your cloud service](Images/publish-your-cloud-service.png?raw=true "Publish your cloud service")

	_Publish your cloud service_


	> **Note:** A Microsoft Azure virtual machine runs a guest operating system into which your service application is deployed. To support the Microsoft Azure Drive feature, the operating system version must be compatible with the Microsoft Azure SDK version 1.8. For information on available versions of the Microsoft Azure guest operating system, see [http://msdn.microsoft.com/en-us/library/ee924680(v=MSDN.10).aspx](http://msdn.microsoft.com/en-us/library/ee924680\(v=MSDN.10\).aspx).
	
	> In general, it is recommended to use the latest available OS to take advantage of new features and security fixes. If you do not specify a version in your configuration file, the OS upgrade method is set to automatic and Microsoft Azure automatically upgrades your VMs to the latest release of the guest OS, once it becomes available. For this lab, you will not choose a specific version and instead use automatic upgrade mode to ensure that the application runs under a guest OS that supports Microsoft Azure Drives.


1. Wait for the transfer to complete. The status of the service should show **Created** when completed.

1. Now, click **URL** link within your recently deployed service to open the application in your browser. The application should behave in essentially the same manner as it did when you deployed it locally. However, notice that the URL of the page blob for the mounted drive is now pointing to the page blob where you uploaded the VHD file.

	![Running the application in the Microsoft Azure environment](Images/running-the-application-in-the-windows-azure.png?raw=true)

	_Running the application in the Microsoft Azure environment_

---
	
<a name="Summary"></a>
## Summary ##

In this lab, you have learned how to work with the Microsoft Azure Storage using tables, blobs, queues and drives.

Tables store data as collections of entities. Entities are similar to rows. An entity has a primary key and a set of properties. A property is a name/value pair, similar to a column.

Blobs can store any binary data. In this lab, you have used the Blob service to store and show images and associate metadata using a Web application.

Using queues, you have seen how to dispatch simple messages (with string, xml or binary content). This is an excellent way of enabling communication between web and worker roles.

Finally, you explored the use of Microsoft Azure Drives that allow you to read and write data to blob storage using standard file system functions.
