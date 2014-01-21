<a name="segment1" />
### Segment 1: Building and Extending Web Apps to Windows 8 ###

In this segment, we will extend the web site to include Web APIs that power a Windows 8 experience.  Finally, the web site project will be deployed to Windows Azure Web Sites and scaled using multiple paid shared instances.

1.	Launch Visual Studio 2012 as **administrator** and open the **BuildClips.sln** solution in **[working directory]\BuildClips\BuildClips.Web**. Press **F5** to run the Web application locally.

	> **Speaking Point**: In this demo, we are taking an __existing__ website and __extending__ it to Windows 8 using WebAPIs. This application uses ASP.NET 4.5 and the new WebAPIs.
	>
	> I've got my app right here. Let's go and launch this app
	>
	> **Note:** Make sure the port where the application will run is number 81, otherwise you will get an error when using Facebook login service.
	>
	>To check this, open Web project's properties and verify that **Use Local IIS Web Server** is selected and it has this value: _http://127.0.0.1:81/_

1. Log in to the application using Facebook. Provide your credentials.

	> **Speaking Point**: You can see at the top that not only do I have the ability to log in via regular ASP.NET membership, but I've got a number of additional social identity providers like a Microsoft account, Twitter or Facebook. 
	>
	> I'm going to log in via Facebook, and this is all built into the ASP.NET 4.5 API.

	![Log On using Facebook Service](Images/log-on-using-facebook-service.png?raw=true "Log On using Facebook Service")

	_Log On using Facebook_

1. If the application consent page is shown, click **Go to App**.

1. Once you are authenticated, in the application Register page click **Register** to associate your Facebook account with the Web application.

	> **Speaking Point**: Now, I'm going to log into my application here, and you'll notice it says "associate your Facebook account." I didn't actually have to create a local membership account. I hit "register" and it notices that it is, in fact, me. And it knows my name, it knows my information. I can associate multiple external accounts.

	![Associating Facebook Account](Images/associating-facebook-account.png?raw=true "Associating Facebook Account")

	_Associating Facebook Account_

1. Close the browser.

1.	Talk about Page Inspector. Open **/Views/Account/Login.cshtml** and type **Ctrl+K, Ctrl+G** to open Page Inspector. 

	> **Speaking Point**: With Page Inspector you can now see what your Razor markup will look like in the browser and make real-time edits.

	![page-inspector](Images/page-inspector.png?raw=true)

	_Page Inspector_

1.	Talk about Facebook Authentication with DotNetOpenAuth. Open **App_Start/AuthConfig.cs**

	> **Speaking Point**: We can now easily use DotNetOpenAuth for handling authentication with a variety of identity providers. In this application we are using Facebook for authentication. You can see how easy it is to set up Facebook authenticaiton in just a few lines of code. Similarly you could set up Google, Microsoft, and Twitter authentication.
	>
	> In addition to supporting Facebook authentication, we also have added a Facebook Application template in the ASP.NET Updates that makes it easy to build a Facebook App using the Facebook C# SDK.

1. Mention that we are using SQL Database for the video metadata.

1.	Discuss the use of Entity Framework 6. Under the **BuildClips.Services** Project, open **Models/VideosContext.cs** and **VideoService.cs**.

	> **Speaking Point**: After I log in, I can upload a video and the metadata for the video is stored in a SQL database. For that metadata we're using Entity Framework 6 Code First, and I'm going to take a look at the video context here.
	>
	> Note that with Code First the database and the schema are automatically generated. Additionally, using automatic migrations makes it easy to iterate your data access code and database schema.
	>
	> We are using async and await to store this. This is really important because I know this application is going to be really popular. I'm going to make sure that I do all of my database IO in a non-blocking way. I make my new video, I add it, and then I use async in a way to save those changes asynchronously. That releases the ASP.NET thread back into the pool and then lets that IO happen in a non-blocking way.

1.	Discuss how we use Blob Storage to store the videos. Under the **BuildClips.Services** Project, open **VideoStorage.cs**. Show how easy it is to upload data to the blob. Show the Upload method and discuss how blob storage is scalable.

	> **Speaking Point**: Then after that, we can even upload the video to Azure Blob Storage, also using async and await.

1. Show the new Storage Explorer in Visual Studio. 

	> **Speaking Point**:  Now, rather than running another application or going to the Azure portal, I can go into Server Explorer now and with the new Azure tools, I can see my blobs, tables, queries, all from inside Visual Studio, including my development blob and the blob in the cloud. I can even right-click on those, view the blob container. And I can see the videos that I've uploaded before.

	![storage-explorer](Images/storage-explorer.png?raw=true)

	_Storage Explorer_

	> **Note**: If you are using the Express edition of Visual Studio, you will find the Storage Explorer in the Database Explorer window.

1.	Back in Visual Studio, open **/Controllers/VideosController.cs**.

	> **Speaking Point**: Now, I want to be able to access these videos from an API, from a Web API. I'm going to take those videos and expose them out to a Windows 8 application. This is an existing ASP.NET app, but I can still add a Web API to it. I can use Web API inside of Web forms or MVC, it's all one ASP.NET.
	>
	> In this example I've got a get and a post, and you'll notice on this post that I'm not taking the video as a parameter, I'm not using model binding in this instance because I could be uploading potentially many gigabytes of videos. I want to use an asynchronous model here as well. I can bring in many, many gigabytes of video without tying up that thread, again, allowing me to scale.

1. Implement **Get** method for retrieving all the existing videos. Code snippet shortcut is **VideosControllercsGet**.

	(Code Snippet - _VideosController.cs - Get_)
	<!-- mark:1-5 -->
	````C#
	// GET /api/videos
	public IQueryable<Video> Get()
	{
		return this.service.GetAll();
	}
	````

	> **Speaking Point**: So here I'm calling read as multipart async on the post. I've currently got a get that lets me get one video, but I want to be able to get multiple videos. So here is a simple get method for my ASP.NET Web API controller. This is going to return those videos as an IQueryable, so if someone visits slash API slash videos, it's going to let me get those.

1.	Next, add the **[Queryable]** attribute to the **Get()** method as shown below.

	<!-- mark:2 -->
	````C#
	// GET /api/videos
	[Queryable]
	public IQueryable<Video> Get()
	{
		return this.service.GetAll();
	}
	````

	> **Speaking Point**: But I might want to be able to query them a little bit more from the query string. Simply returning IQueryable and adding the [Queryable] attribute is all you need to enable OData queries on your REST service.

1. Expand the **Areas** folder and show the **HelpPage** area. This area contains the Views to render the Web API Help Pages.

	> **Speaking Point**: Just like SOAP applications had WSDL to help you understand those Web services, Web APIs and RESTful APIs have online documentation that I don't want to have to write myself.

	![WebAPI HelpPage Area](Images/webapi-helppage-area.png?raw=true)

	_WebAPI Help Page Area_

1.	Press **F5** again to run the application and use the following address to navigate the Web API  Help Page:   [http://127.0.0.1:81/help](http://127.0.0.1:81/help).
	
	> **Speaking Point**: So I'm going to type in "slash help" in the URL there, and we're going to auto-generate documentation for the Web API, including the new API that I just added. If I click on that, I can even see in a sample response format that shows me whatever the JSON would look like when that gets returned.

	![WebAPI Help Page](Images/webapi-help-page.png?raw=true "WebAPI Help Page")

	_WebAPI Help Page Running_

1. Close the browser.

1.	Open the **Windows Azure Management** portal.

	> **Speaking Point**: So I've got this application set up. And now I'm going to go into the Azure portal and I can make a new website.

1.	Create a new Web Site with database, using the **Create with Database** option.

	![portal create with database](Images/portal-create-with-database.png?raw=true "WebAPI Help Page")

	_Creating a new Web Site with a database from the portal_

1. In the **Create Web site** page, set the website name by typing, for example, **_buildclipsdemo_** in the **URL** and select to create a new SQL database.

	![portal create web site](Images/portal-create-web-site.png?raw=true)

	_Creating the Web Site_

1. In the **Specify Database settings** page, choose to create a **New SQL Database server**, providing an administrator **username** and **password**.

	![portal database settings](Images/portal-database-settings.png?raw=true)

	_Creating the database_

1.	Navigate to your new web site in the **Windows Azure Management** portal and download the publishing profile.

	> **Speaking Point**: I'm going to download the publish profile and the publish profile is going to give me all the information that I could potentially need to publish this.

	![portal downloadprofile](Images/portal-downloadprofile.png?raw=true)

	_Downloading the publish profile_

1. Go back to the Web application solution within **Visual Studio**.

1.	Right-click the **BuildClips** project and select **Publish**. 

	![Clicking on the Publish menu](Images/clicking-on-the-publish-menu.png?raw=true)

	_Right-click the **BuildClips** project and select **Publish**_

1. Import the downloaded publish settings file.

	> **Speaking Point**: I'm going to go and import that publish setting. So when I hit import, it's going to update this dialog with all the information that I need, all the passwords, all the user names, every technical detail that's required. Including that entity framework connection string.

	![publish web](Images/publish-web.png?raw=true)

	_Publishing the web site_

1. In the **Connection** page, click **Next**.

	![publish connection](Images/publish-connection.png?raw=true)
	
	_Configuring the publish method_

1. In the **Settings** page, click **Next**.

	![publish settings](Images/publish-settings.png?raw=true)
	
	_Configuring the database connection string for the web site_

1. In the **Preview** page, click **Publish**.

	> **Speaking Point**: So I'm going to hit publish on that. It's going to build and start publishing. 

	![publish process](Images/publish-process.png?raw=true)

	_Publish process_

1.	Once the publishing process finishes, go to the **Scale** tab in the portal and change the **Web Site Mode** to _Shared_. Change the number of instances to 3.

	> **Speaking Point**: Now that my application is deployed and because I know this is going to be popular I'm going to go over to scale. In scale here, I can go and select not just that I want a free website, even though I get ten free websites, but I could say shared and have multiple instances or reserved and have even four cores and three instances of that. So I've got a lot of flexibility in the way I can scale my website in Azure.

	![portal sharedmode](Images/portal-sharedmode.png?raw=true)

	_Changing the **Web Site Mode** and the number of instances_

1. Click **Save** in the bottom bar and select **Yes** to agree with the billing impact.

	![Billing impact message](Images/billing-impact-message.png?raw=true)

	_Billing impact message_

1. Go to the **Configure** tab in the portal and open the **Manage Domains**.

	> **Speaking Point**: Talk about how Web Sites support CNAMES and A Records for applications running in Shared and Reserved modes.

	![portal manage domains](Images/portal-managedomains.png?raw=true)

	_Manage domains using the Windows Azure portal_

1. Navigate to the recently deployed application and upload a video that will be now stored in the cloud. You can use any of the videos from the **[working dir]\Assets\videos** folder.

	> **Speaking Point:** So now, let's upload a new video to my recently deployed application. 

	![Upload a video](Images/upload-video.png?raw=true)

	_Upload a video_

1.	Open the **BuildClips.Win8App.sln** solution located at **[working dir]\BuildClips\BuildClips.Win8App** in a new instance of **Visual Studio**.

	> **Speaking Point:** So now I'm going to consume that from Windows 8. And since I've been doing a lot of work in JavaScript and curly braces lately, I wrote the entire application in JavaScript. So this is a native Win8 application in the store that is written in JavaScript.

1.	Show the code for calling the Web API. In the **webapi.js** file, highlight **getVideos()** method. Additionally, in the **data.js** file, highlight the **GetVideosOnSuccess()** method.
	
	<!-- mark:3-4 -->
	````JavaScript
   WinJS.Namespace.define("WebApi", {
      getVideos: function getVideos(onSuccess, onError) {
         WinJS.xhr({ url: url, responseType: "json" })
            .done(onSuccess, onError);
      },
      ...
   });
   ````

	<!-- mark:4,8-11 -->
	````JavaScript
   function GetVideosOnSuccess(result) {
        setProfilingTime('GetAll', new Date().getTime() - timeStamp);

        var videos = JSON.parse(result.responseText);

        list.length = 0;

        videos.forEach(function (video, i) {
            list.push(getListItem(video));
        });

        Data.itemsRetrieved = true;
    }
   ````

	> **Speaking Point:** I'm going to look at my WebAPI.js. And here I'm going to be calling the WinJS.XHR helper function, and I'm just going to go and call that Web API that I created, return all of those videos, and then bind them to a list inside of my store application.

1.	Press **F5** to run the Windows 8 Application.

1. Login with your **Facebook** or **Twitter** account.

	![Log into the Windows 8 App using Facebook Service](Images/win8-log-on-using-facebook-service.png?raw=true "Log into the Windows 8 App using Facebook Service")
	
	_Log into the Windows 8 App using Facebook Service_

1. If you chose to authenticate using Facebook and the _Application Consent_ page is shown, click **Allow**. If you have chosen to authenticate using Twitter, and the _Authorize Application_ page is shown, click **Authorize App** to continue.

1. Click on the video to show that it is consuming the published Web API.

	![Select the previously uploaded video](Images/win8-select-video.png?raw=true "Select the previously uploaded video")
	
	_Select the previously uploaded video_

1. Then reproduce the uploaded video to validate everything is working as expected.

	![Video player showing the previously uploaded video](Images/win8-play-uploaded-video.png?raw=true "Video player showing the previously uploaded video")
	
	_Video player showing the previously uploaded video_

