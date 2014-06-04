<a name="demo2" />
# ClipMeme Demo#

## Overview ##

In this demo, we will showcase the Visual studio and azure VM and Web Site provisioning features, **PowerShell** script generation and editing to provision multiple geographic deployments using Traffic Manager, and background services using both VM’s and Web Jobs. It also reinforces our momentum with Visual Studio (with Web Essentials) as a great tool for creating powerful **HTML5** applications backed by **ASP.NET**.

<a name="Goals" />
### Goals ###
In this demo, you will see how to:

 1. Provision Azure through Visual Studio
 1. Work with the new Web Essentials Features, Image Sprites and Angular Js
 1. Create Azure Webjobs
 1. Backup, Scale and control site traffic using Traffic Manager

<a name="Technologies" />
### Key Technologies ###
- Windows Azure subscription
- Microsoft Visual Studio 2013
- Visual Studio 2013 Update 2
- Windows Azure Websites

<a name="Prerequisites" />
### System Prerequisites ###
- [Azure PowerShell Cmdlets](http://go.microsoft.com/?linkid=9811175&clcid=0x409)
- [Azure Cross-platform Command Line Tools](http://go.microsoft.com/fwlink/?LinkID=275464&clcid=0x409)
- [Visual Studio 2013](http://www.visualstudio.com/downloads/download-visual-studio-vs)
- [Visual Studio 2013 Update 2](http://www.microsoft.com/es-es/download/details.aspx?id=42666)
- [Web Essentials for Update 2](http://visualstudiogallery.msdn.microsoft.com/56633663-6799-41d7-9df7-0f2a504ca361)
- [Add-in to add publishing support to web projects for Azure WebJobs](http://visualstudiogallery.msdn.microsoft.com/f4824551-2660-4afa-aba1-1fcc1673c3d0)
- [SideWaffle templates](http://visualstudiogallery.msdn.microsoft.com/a16c2d07-b2e1-4a25-87d9-194f04e7a698)
- [Google Chrome](www.google.com/chrome)

<a name="Setup" />
### Setup and Configuration ###

##### Azure #####

1. Open **PowerShell** and execute the following command.

	````PowerShell
	Get-AzurePublishSettingsFile
	````

1. A browser will open. When prompted, enter your Azure credentials. Save the file to a known location.

1. Rename the file **azure.publishsettings**.

1. Copy the file and replace the one located in the **Source\Setup\assets\publishSettings** folder of this demo.
	
1. Open using a text editor the **config.xml** file located in the **Source** folder.

1. Replace the placeholder values with the following:

	- **solutionWorkingDir**: The default directory where the solutions and assets will be copied for the demo. You can leave the default directory or choose a different one.
	- **DisplayName**: It is the username that is displayed on ClipMeme's home screen.
	- **EnvironmentSubscriptionName**: It is your Azure subscription name.
	- **EnvironmentPrimaryLocation**: It is the location where the Azure resources will be deployed. E.g.: East US, West US, etc.
	- **StorageAccountName**: The name of the storage account required for ClipMeme. The setup scripts will automatically create a new Storage Account in Azure using this value. The name you choose must be unique and in lower case.
	- **WebsiteName**: The name of the Azure Website for ClipMeme. The setup scripts will automatically create an Azure Website, scale it to **Standard** mode and create a _staging_ slot.
	- **TrafficManagerProfile**: The name of the Azure Traffic Manager profile. The setup scripts will automatically create a profile to use in the demo.
	
1. Save the file and close the editor.

1. Run **Reset.cmd** using elevated permissions.
	
##### Solutions  #####

1. Open **ClipMeme** and build it to install NuGet Packages.

1. Publish **ClipMeme** to the Azure Website you created before.

##### Configuration Variables #####

1. Run **Reset.cmd**.

1. Open **Visual Studio** and the **ClipMeme** solution.

##### First Run #####

1. Open a browser and go Management Portal > Web Site.

1. Open a browser and go Webjob **Dashboard**.

## Demo ##

This demo is composed of the following segments: 

1. [Visual Studio and Azure Provisioning](#segment1)

1. [Web Essentials Features](#segment2)

1. [Azure Web Jobs](#segment3)

1. [Azure Auto Scale, Traffic Manager and Backup](#segment4)

<a name="segment1" />
###Visual Studio and Azure Provisioning###

1. In Visual Studio, click **File > New > Project...** 

	![Create New Project](Images/create-new-project.png?raw=true)

1. Click **Web** and select **ASP .NET Web Application**. Click **OK**.

	![New ASP .NET Web Application](Images/new-asp-net-web-application.png?raw=true)

1. Select **Empty** template and check **Create remote resources** checkbox. Click **OK**.

	> **Speaking Point:** As a new thing, as you can see, we made it really easy for you to provision both **Azure Web Sites** and **Virtual Machines** directly from this dialog. 
	
	![Web Site Empty Template](Images/web-site-empty-template.png?raw=true)

2. Select **Create new server**. Provide **Database Username** and **Database Password**. Click **OK**.

	> **Speaking Point:** You can even provision a new database directly from here. It will let you set up your entire development environment ahead of time.
	
	![Azure Website Database Provisioning](Images/azure-website-database-provisioning.png?raw=true)

2. Browse to https://manage.windowsazure.com and log-in with your credentials.  
 
2. Click **Web sites** and verify that your site is created.

	![Azure Web Site Created](Images/azure-web-site-created.png?raw=true)

2. Go back to **Visual Studio** and open **Publish-WebApplication.ps1** file.

	> **Speaking Point:** So now the project is created and Visual Studio is provisioning Azure. But it is also now creating Publishing Scripts that we can use to automate deployments.
	
	![Publish Web Application PS1](Images/publish-web-application-ps1.png?raw=true)

2. Right-Click on the **PS1** file and click **Open with PowerShell ISE**

	> **Speaking Point:** One neat feature is the context menu for PowerShell. If you Right-click on the PS1 file, you will notice the new context menu ‘Open with PowerShell ISE, which allows you to open a PowerShell editor straight from Visual Studio.
	
	![Open with PowerShell ISE](Images/open-with-powershell-ise.png?raw=true)

<a name="segment2" />
### Web Essentials Features ###

>**Speaking Point**: Introduction to ClipMeme scenario:

>* We are going to look at a small site that could get big fast. We are building a viral meme generator with a twist: we are using animated GIFs. 
>* Show the running application.
>* Hover over one of the images to show the animated gif with the text overlay.
	
<a name="browserlink-features" />
#### Task 1 - Browser link####

1. Place Visual Studio and Internet Explorer side by side.

	>**Note**: This also works for changes we make using the browser developer tools. I’ve decided the header looks a little too bland, and I’d prefer to change the color interactively in the browser using the **F12** tools in my browser. 

1. Right-click in the header section and select **Inspect element**. This will open the developer tools.

	![Internet Explorer - Inspect element](Images/internet-explorer---inspect-element.png?raw=true "Internet Explorer - Inspect element")
	
	_Internet Explorer - Inspect element_

1. In the **DOM Explorer** select the **header** html tag
	
	![DOM Explorer - Header section](Images/dom-explorer---header-section.png?raw=true "DOM Explorer - Header section")
	
	_DOM Explorer - Header section_

1. Press the **Ctrl** key in the browser to enable the **Web Essentials Browser Link Overlay**. 

1. Click on **F12 Auto sync** from the Web Essentials toolbox in Internet Explorer to enable updating in the browser.

	![F12 Auto Sync](Images/f12-auto-sync.png?raw=true "F12 Auto Sync")
	
	_F12 Auto Sync_
	
1. In the **Styles** pane, scroll until you find the **.navbar-default** selector. 

1. Double-click the **background-color** value and change it to a different color (e.g. Azure). Show how the value is automatically updated in Visual Studio.

	![Automatic update background color](Images/automatic-update-background-color.png?raw=true "Automatic update background color")

	_Automatic update background color_
	
	![Final background color](Images/final-background-color.png?raw=true "Final background color")
	
	_Final background color_
	
1. Switch to **Google Chrome** and show that the header color is automatically updated in all the browsers.

	![Google Chrome with background color updated](Images/google-chrome-with-background-color-updated.png?raw=true "Google Chrome with background color updated")
	
	_Google chrome with background color updated_

1. Show that there is duplicated **and** in the text.

	![Typo - Duplicate and](Images/typo---duplicate-and.png?raw=true "Typo - Duplicate and")
	
	_Typo - Duplicate and_

	>**Speaking Point**: Oops, it looks like there is a text error in the header - we repeated the word **and**. Let’s fix that. 
	
1. Press the **Ctrl** key in the browser to enable the **Web Essentials Browser Link Overlay**. 

1. Click on the **Design** link to enable **Design mode**.
	
	![Design mode link](Images/design-link.png?raw=true "Design mode link")
	
	_Design mode link_

	>**Note**: Now we can fix the text right in the page and save it back to my source code in Visual Studio.

1. Show that while hovering each section of the page, Visual Studio highlights the element.
	
	![Hovering in the browser updates in Visual Studio](Images/hovering-in-the-browser-updates-in-visual-stu.png?raw=true "Hovering in the browser updates in Visual Studio")
	
	_Hovering in the browser updates in Visual Studio_

1. Click on the text with the typo and delete the duplicated word.

	![Removed duplicate word](Images/removed-duplicate-word.png?raw=true "Removed duplicate word")
	
	_Removed duplicate word_

	>**Note**: This works the exact same in any browser, in case we want to use IE instead.

<a name="image-sprites-not-in-keynote" />
#### Task 2 - Image Sprites####

1. Switch back to Visual Studio.

	>**Speaking Point**: We can improve performance on our site by sending a lot of images in one HTTP request, using CSS sprites. We will select some common images on our site and create a sprite the easy way, using Web Essentials.


1. In the Solution Explorer, expand the **Content** folder and select **share-facebook.png** and share-twitter.png holding the **Ctrl** key.

1. Right-click on the images and select **Web Essentials | Create Image Sprite...**…

	![Create image sprite](Images/create-image-sprite.png?raw=true "Create image sprite")
	
	_Create image sprite_

1. In the **Save As** dialog box, name the sprite **social.sprite** and click **Save**.

	![Save as dialog box](Images/save-as-dialog-box.png?raw=true "Save as dialog box")
	
	_Save as dialog box_
  
	>**Note**: Adding a new sprite automatically generates example CSS, LESS and SASS to include these new images using standard CSS classes. We’ll update our CSS and now we’re only making one image request for all of the images in the sprite.

1. Open the “**Content/Site.less**” file and do the following to use the image sprite just generated:
	
1. Add the following **@import** statement at the top of the file:
	
	<!-- mark:1 -->
	````CSS
	@import 'social.png.less';
	````
 
1. Within the same file modify the **.facebook** and **.twitter** classes by replacing the background-image style with the corresponding less function to retrieve the image sprite as shown below:

	<!-- mark:1-11 -->
	````CSS
	.facebook {
		.sprite-Content-share-facebook();
		width: 24px;
		height: 24px;
	}

	.twitter {
		.sprite-Content-share-twitter();
		width: 24px;
		height: 24px;
	}
	````

1. In Internet Explorer, open **Developer tools** and switch to the **Network** tab.

1. Click on **Play** to start recording the network traffic and refresh the application.

1. Check there is a request to **/Content/Social.png**. Select it and click on **Details** to check the details of the request.

	![Request details](Images/request-details.png?raw=true "Request details")
	
	_Request details_

1. Click on **Response body** to check that both icons come in the same image.
	
	![Response body](Images/response-body.png?raw=true "Response body")
	
	_Response body_

<a name="javascript-editing" />
#### Task 3 - Javascript and AngularJS editing ####

1. Open the **/Scripts/app/controllers/modal.js** file.

1. Place the cursor below the **cleanVariables();** line and start writing **$sc**. Show that there is Intellisense for JavaScript and AngualrJS.

	![Intellisence Javascript with Web Essentials](Images/intellisence-javascript-with-web-essentials.png?raw=true "Intellisence Javascript with Web Essentials")
	
	_Intellisense JavaScript with Web Essentials_

1. Open the **Error List** panel and show that there are two JSHIint errors.

	![Error list panel](Images/error-list-panel.png?raw=true "Error list panel")
	
	_Error list panel_

1. Double-click on each issue and it will take you to the corresponding line.

	![Before fixing the error](Images/before-fixing-the-error.png?raw=true "Before fixing the error")

	_Before fixing the error_

1. Fix the issues, save the file and show that there are no more issues.

	![After fixing the error](Images/after-fixing-the-error.png?raw=true "After fixing the error")
	
	_After fixing the error_

1. Close the **Error List** panel and show that querying the DOM from a controller is not a good practise in AngularJS so we need to refactor the code by adding a new directive.
	
1. Right-click the **/scripts/app/directives** directory and select **Add** | **New item**.
	
	![Add new item menu](Images/add-new-item-menu.png?raw=true "Add new item menu")
	
	_Add new item menu_

1. Type **directive** in the filter to narrow the list. Name the directive **appDropzone.js**.
	
	![Directive template](Images/directive-template.png?raw=true "Directive template")
	
	_Directive template_


	>**Note**: This sets us up with a basic directive with some best practices, like for instance it’s using Strict Mode. We’ve also got JsHint running in the background so it can catch some common JavaScript coding issues as we type, for instance if we try to reuse a function parameter name.
	
1. Add another parameter named **scope** to the end of the **link** function and show that **JsHint** flags it.

	![Duplicate parameter error](Images/duplicate-parameter-error.png?raw=true "Duplicate parameter error")
	
	_Duplicate parameter error_
 
1. Remove the **scope** parameter just added, it was simply to show the error.
	
1. Move the code from the **modal** controller to the directive you created; the code to move is highlighted with a comment that says **//TODO: refactor to a directive**. 

1. Remove the **element** and **scope** variable in the controller to avoid JSHint issues.
	
	![Removed variables from modal controller](Images/removed-variables-from-modal-controller.png?raw=true "Removed variables from modal controller")
	
	_Removed variables from modal controller_

	>**Note:** Don't remove the **scope.init** declaration as it is required for AngularJS.
	
1. Open **Modal.cshtml** view from the **Share** folder and update the div element with id **dropzone** as with the code below; the code to update is highlighted with a comment that says **<!--Dropzone element-->**. 
 
	````HTML
	<div app-dropzone class="drag-drop" ng-hide="fileloaded || loading">
	````

1. Press **Ctrl** + **Shift** + **S** to save all the files.
	
<a name="azure-staged-published" />
#### Task 4 - Azure Staged Published ####

1. In the **Solution Explorer**, right-click on the **Project** and select **Publish…**

	![Publish](Images/publish.png?raw=true "Publish")
	
	_Publish_

1. Go to **Profile** and show the different publish targets. 

	![Publish Web dialog box](Images/publish-web-dialog-box.png?raw=true "Publish Web dialog box")
	
	_Publish Web dialog box_

1. Select **Windows Azure Web Sites**.
 
	![Select existing Web Site dialog box](Images/select-existing-web-site-dialog-box.png?raw=true "Select existing Web Site dialog box")
	
	_Select Existing Web Site dialog box_

1. Select the **Staging** web site and click **OK**.


<a name="segment3" />
### Azure Web Jobs ###

1. Switch back to the production site of **ClipMeme** and click the **Create Meme** button.

	![Creating a new meme](Images/creating-a-new-meme.png?raw=true)
	
	_Creating a new meme_

1. Drag the animated GIF from your working directory into the _drop zone_ in the browser. Enter a caption and click **Submit**.

	![Submitting a new meme](Images/submitting-a-new-meme.png?raw=true)
	
	_Submitting a new meme_

1. Wait until the meme is fully created (the image legend will change from _Processing_ to your caption).

	![Processing the meme](Images/processing-the-meme.png?raw=true)
	
	_Processing the meme_

1. Switch to the **ClipMeme Azure Website** in the Management Portal and go to **WEBJOBS**.

	![Azure Webjobs](Images/azure-webjobs.png?raw=true)
	
	_Azure Webjobs_

	> **Speaking Point:** A Webjob allowsus to run background tasks in the same context as the Web Site.

1. Switch back to the **ClipMeme** solution in **Visual Studio**.

1. In **Solution Explorer** go to the **GifGenerator** project and open **Program.cs**. Scroll down to show the **ProcessImage** method.

	> **Speaking Point:** Using the WebJobs SDK it makes really easy to listen to any events that is happening in any of the Azure resources.

1. In Solution Explorer, right-click the **ClipMeme** project node and select **Add > Windows Azure Webjob...**.

	![Associating a Webjob project with your Website](Images/associating-a-webjob-project-with-your-websit.png?raw=true)
	
	_Associating a Webjob project with your Website_

	![Webjob project selector](Images/webjob-project-selector.png?raw=true)
	
	_Webjob project selector_

	> **Speaking Point:**We've already associated my Console project with my Website. The next time we publish the Website, the Webjob will be published with it.

1. Switch to the **Management Portal** and click the **logs** link of your WebJob.

	![Webjob Logs](Images/webjob-logs.png?raw=true)
	
	_Webjob Logs_

1. Show the logs and explain that every time a Job runs or fails, it will add a new entry to this log.

	> **Note:** If you generated errors in your log, they will be displayed between the other logs.

<a name="segment4" />
### Azure Backup, Auto Scale and Traffic Manager ###

<a name="backups" />
#### Task 1 - Backups ####
1. Go to the **Backups** section in the Web Site.

	![WebSite Backup Section](Images/website-backup-section.png?raw=true)

1. Enable **Automated Backup** by clicking **ON**

	![Enable Automated Backup](Images/enable-automated-backup.png?raw=true)

1. Select a storage account from the dropdown list.

	> **Speaking Point:** The user can change the **Frequency** and also the **Start Date** for performing a backup.

	![WebSite Storage accountand Frequency](Images/website-storage-accountand-frequency.png?raw=true)
	
1. Select a database connection from the **Included Databases** drop-down.

	![Selecting Backup Database](Images/selecting-backup-database.png?raw=true)
	
1. Click **Save** to confirm.
1. Click **Backup Now** to create a backup.

	![Backing up a Web Site](Images/backing-up-a-web-site.png?raw=true)
	
	_Backing up a Web Site_

1. Once completed, show how to restore a backup. Click **Restore Now**.

	![Restoring a Database](Images/restoring-a-database.png?raw=true)

	> **Note:** Explain the two options to restore a backup: From a previous backup of the site or by selecting a file from the storage account. Select the second option and click the folder icon.
	
1. Click the folder icon to browse the storage account file.

	![Browsing storage account file](Images/browsing-storage-account-file.png?raw=true)
	
1. Select the storage account used to generate the backup. Click the **websitebackups** container and select the backup file. Click **OPEN**.

	![Restoring backup file](Images/restoring-backup-file.png?raw=true)
	
1. Click the arrow to continue to next page.

1. Show the options to restore a Web Site (_current_ or _new_). From the databases drop-down, select a server and show how you can set up credentials to restore the database. Click the **Automatically adjust connection strings** checkbox. **Do not Confirm changes, close the window to continue**

	![Restore Web Site options](Images/restore-web-site-options.png?raw=true)


<a name="auto-scale" />
#### Task 2 - Auto Scale ####
1. In the Azure Portal, click Web Sites, browse to the Web Site and click **Scale**.

	![Web Site Scale](Images/web-site-scale.png?raw=true)

1. Make sure the **Instance Size** is set to **Medium** or **Large**.

	![Scale instance size](Images/scale-instance-size.png?raw=true)

1. Scroll down to **Scale by Metric** and select **CPU**.

	![CPU Scaling](Images/cpu-scaling.png?raw=true)
	
1. Change the **Instance Count** slider to **2** and then to **4**. 

	> **Note:** Explain how the user can use the Target CPU threshold to automatically upscale or downscale the number of instances. This allows to spend only as much as needed for the service.
	
	![Updating instance count](Images/updating-instance-count.png?raw=true)
	
1. Click **Discard**.

<a name="traffic-manager" />
#### Task 3 - Traffic Manager ####
1. Browse to the Azure Portal, and click **New** > **Network Services** > **Traffic Manager** > **Quick Create**.

	![Traffic Manager creation](Images/traffic-manager-creation.png?raw=true)
	
	_Traffic Manager creation_

	> **Speaking Point:** We can optimize the Web Site for **Performance**, **Round Robin**, or **Failover**. Failover is the scenario where we set up a primary node and in case of failure, traffic is routed automatically by Traffic Manager to secondary nodes. Since we want to optimize for Performance, we'll select this method. No matter where the user is, they will always hit the data center closest to them.
		
1. Click **Close**, and go to the Traffic Manager already created.
1. Select **Endpoints**

	![Traffic Manager Endpoints](Images/traffic-manager-endpoints.png?raw=true)
	
1. Click **Add**.
	
	![Adding Traffic Manager endpoints](Images/adding-traffic-manager-endpoints.png?raw=true)

1. Select **Web Site** in the **Service Type** combo. Check the deployed Web Site and add it to the Traffic Manager profile. Click **Ok**.
	
	![Add Web Site to the Traffic Manager](Images/add-web-site-to-the-traffic-manager.png?raw=true)
	
	_Add Web Site to the Traffic Manager_

	> **Speaking Point:** This ensures that no mather where you are in the world, you are going to always hit the datacenter closest to your location.
	
1. Switch to **ClipMeme** site and show were are you served from.
	
---

<a name="summary" />
## Summary ##

In this demo, you saw how to easelly create a development environment upfront. Use some of the new Visual Studio Features to create beautifull modern Web Applications, backup and restore them, deploy them to staging for Testing, then onto production and scale it World wide.
