<a name="demo2" />
# ClipMeme Demo#

## Overview ##

In this demo, we will ...

<a name="Goals" />
### Goals ###
In this demo, you will see how to:

1. This...

<a name="Technologies" />
### Key Technologies ###
- Windows Azure subscription
- Microsoft Visual Studio 2013
- Visual Studio 2013 Update 2
- Windows Azure Webistes

<a name="Prerequisites" />
### System Prerequisites ###
- Visual Studio 2013
- [Visual Studio 2013 Update 2](http://www.microsoft.com/es-es/download/details.aspx?id=42666)
- [Web Essentials for Update 2](http://visualstudiogallery.msdn.microsoft.com/56633663-6799-41d7-9df7-0f2a504ca361)
- [Add-in to add publishing support to web projects for Azure WebJobs](http://visualstudiogallery.msdn.microsoft.com/f4824551-2660-4afa-aba1-1fcc1673c3d0)
- [Side Waffles templates](http://visualstudiogallery.msdn.microsoft.com/a16c2d07-b2e1-4a25-87d9-194f04e7a698)
- [Google Chrome](www.google.com/chrome)

<a name="Setup" />
### Setup and Configuration ###

##### Azure #####

1. Create a **Storage Account** in Azure.

1. Create a **Windows Azure Website** in **Standard** mode and enable Staging slot.

1. Enable **Web Sockets** in **Configure**.

1. Add the following settings in **Configure**:

	* AzureJobsRuntime: Storage Account connection string
	* AzureJobsData: Storage Account connection string

1. Create **Traffic Manager** and configure it. 
	
##### Solutions  #####

1. Open **ClipMeme** and build it to install NuGet Packages.

1. Publish **ClipMeme** to the Azure Website you created before.

##### Configuration Variables #####

1. Set the connection string for BlobStorage.

1. Set the default path for the _Working Directory_.

1. Set the username that will be displayed in ClipMeme.

1. Set the Azure WebSite URL.

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

	> **Speaking Point:** You can even provision a new database directly from here with let you set up your entire development environment ahead of time.
	
	![Azure Website Database Provisioning](Images/azure-website-database-provisioning.png?raw=true)

2. Browse to https://manage.windowsazure.com and log-in with your credentials.  
 
2. Click **Web sites** and verify that your site is created.

	![Azure Web Site Created](Images/azure-web-site-created.png?raw=true)

2. Go back to **Visual Studio** and open **Publish-WebApplication.ps1** file.

	> **Speaking Point:** So now my project is created and Visual Studio is provisioning Azure. But its also now creating Publishing Scripts that i can use to automate my deployment.
	
	![Publish Web Application PS1](Images/publish-web-application-ps1.png?raw=true)

2. Right-Click on the **PS1** file and click **Open with Powershell ISE**

	> **Speaking Point:** One neat feature that I think you’ll enjoy is the context menu for PowerShell. If you Right-click on the PS1 file, you will notice the new context menu ‘Open with PowerShell ISE, which allows you to open a PowerShell editor straight from Visual Studio.
	
	![Open with PowerShell ISE](Images/open-with-powershell-ise.png?raw=true)

<a name="segment2" />
### Web Essentials Features ###

>**Speeking Point**: Introduction to the scenario:

>* So for this scenario, we’re going to look at a small site that could get big fast. We are building a viral meme generator with a twist: we are using animated GIFs. 
>* Show the running application.
>* Hover over one of the images to show the animated gif with the text overlay.
	
<a name="browserlink-features" />
#### Task 1 - Browser link####

1. Place Visual Studio and Internet Explorer side by side.

1. Show that there’s a typo in the header, the word **and** is repeated bellow the **Welcome to ClipMeme** section.

	>**Note**: This also works for changes I make using the browser developer tools. I’ve decided the header looks a little too bland, and I’d prefer to change the color interactively in the browser using the F12 tools in my browser. 

1. Right-Click in the header section and select **Inspect element**. This will open the developer tools.

	![Internet Explorer - Inspect element](Images/internet-explorer---inspect-element.png?raw=true "Internet Explorer - Inspect element")
	
	_Internet Explorer - Inspect element_

1. In the **DOM Explorer** select the **header** html tag
	
	![DOM Explorer - Header section](Images/dom-explorer---header-section.png?raw=true "DOM Explorer - Header section")
	
	_DOM Explorer - Header section_

1. In the **Styles** pane, scroll until you find the **.navbar-default** selector. 

1. Double-click the background color value and change it to a different color. Show how the value is automatically updated in Visual Studio.

1. Switch to Google Chrome and show that the header color is automatically udpated in all the browsers.

	>**Speeking Point**: Oops, it looks like there is a text error in the header - we repeated the word “and”. Let’s fix that. 

1. Press the **Ctrl** key in the browser to enable the Web Essentials Browser Link Overlay. 

1. Click on the **Design** link to enable Design mode.
	
	>**Note**: Now I can fix the text right in the page and save it back to my source code in Visual Studio.

1. Show that while hovering each section of the page, Visual Studio highlights the element.
	
1. Delete the duplicated word.

	>**Note**: This works the exact same in any browser, in case we want to use IE instead.

<a name="image-sprites-not-in-keynote" />
#### Task 2 - Image Sprites####

1. We can improve performance on our site by sending a lot of images in one HTTP request, using CSS sprites. We’ll select some common images on our site and create a sprite the easy way, using Web Essentials.

1. Select the 2 share images in the **/Content/** folder, right-click, and select **Web Essentials / Create Image Sprite**…
 
1. Name the sprite **social**.
  
	>**Note**: Adding a new sprite automatically generates example CSS, LESS and SASS to include these new images using standard CSS classes. We’ll update our CSS and now we’re only making one image request for all of the images in the sprite.

	>**Review**: [TODO: Show SCSS addition and SCSS tooling in Visual Studio 2013 Update 2.]

1. Open the “**Content/Site.less**” file and do the following to use the image sprite just generated:
	
1. Add the following @import statement at the top of the file:
	
	<!-- mark:1 -->
	````CSS
	@import 'social.png.less';
	````
 
1. Within the same file modify the **.facebook** and **.twitter** classes by replacing the background-image style with the corresponding less function to retrieve the image sprite as shown below:

	<!-- mark:1-6 -->
	````CSS
	.facebook: {
		.sprite-share-facebook();
	}
	.twitter {
		.sprite-share-twitter();
	}
	````

<a name="javascript-editing" />
#### Task 3 - Javascript and AngularJS editing ####

1. Open the **/Scripts/app/controllers/modal.js** file.

1. Place the cursor below the **cleanVariables();** line and start writing **$sc**. Show that there is Intellicense for Javascript and AngualrJS.

	![Intellisence Javascript with Web Essentials](Images/intellisence-javascript-with-web-essentials.png?raw=true "Intellisence Javascript with Web Essentials")
	
	_Intellisence Javascript with Web Essentials_

1. Right-click the **/scripts/app/directives** directory and select **Add** | **New item**.
	
	![Add new item menu](Images/add-new-item-menu.png?raw=true "Add new item menu")
	
	_Add new item menu_

1. Type **directive** in the filter to narrow the list. Name the directive **appDropzone.js**.
	
	![Directive template](Images/directive-template.png?raw=true "Directive template")
	
	_Directive template_


	>**Note**: This sets me up with a basic directive with some best practices, like for instance it’s using Strict Mode. We’ve also got JsHint running in the background so it can catch some common JavaScript coding issues as I type, for instance if I try to reuse a function parameter name.
	
1. Add another parameter named **scope** to the end of the **link** function and show that **JsHint** flags it.

	![Duplicate parameter error](Images/duplicate-parameter-error.png?raw=true "Duplicate parameter error")
	
	_Duplicate parameter error_

	>**Note**: if the error does not show up in the output window, build the solution first.
 
1. Remove the **scope** parameter just added, it was simply to show the error.
	
1. Move the code from the **modal** controller to the directive you created; the code to move is highlighted with a comment that says **//TODO: refactor**. 

1. Remove the **element** and **scope** variable in the controller to avoid JSHint issues.

1. Open the **Error List** panel and show that there are two JSHIint errors.

	![Error list panel](Images/error-list-panel.png?raw=true "Error list panel")
	
	_Error list panel_

1. Fix the semicolong issue by adding the ; at the end of the line

1. Fix the comparison issue by adding another =.

	![JSHint issues fixed in directive](Images/jshint-issues-fixed-in-directive.png?raw=true "JSHint issues fixed in directive")
	
	_JSHint issues fixed in directive_

1. Demonstrate that it works.
	
1. Open **Modal.cshtml** view from the **Share** folder and update the div element with id **dropzone** as with the code below; the code to update is highlighted with a comment that says **<!--Dropzone element-->**. 
 
	````HTML
	<div app-dropzone class="drag-drop" ng-hide="fileloaded || loading">
	````

	
<a name="azure-staged-published" />
#### Task 4 - Azure Staged Published ####

1. Right-click on the **Project** and select **Publish…**

1. Go to **Profile** and show the different hosting options. 

1. Select **Windows Azure Web Sites**.
 
1. Select **build2014p-gifmeme(Staging)** and click **OK**.
 
	>**Note**: Option to launch browser when deployment is complete will be disabled to prevent interrupting next segment.

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

	> **Speaking Point:** A Webjob allows me to run background tasks.

1. Switch back to the **ClipMeme** solution in **Visual Studio**.

1. In **Solution Explorer** go to the **GifGenerator** project and open **Program.cs**. Scroll down to show the **ProcessImage** method.

	> **Speaking Point:** TODO

1. In Solution Explorer, right-click the **ClipMeme** project node and select **Add > Windows Azure Webjob...**.

	![Associating a Webjob project with your Website](Images/associating-a-webjob-project-with-your-websit.png?raw=true)
	
	_Associating a Webjob project with your Website_

	![Webjob project selector](Images/webjob-project-selector.png?raw=true)
	
	_Webjob project selector_

	> **Speaking Point:** I've already associated my Console project with my Website. When I publish my Website, the Webjob will published with it.

1. Switch to the the **Management Portal** and click the **logs** link of your WebJob.

	![Webjob Logs](Images/webjob-logs.png?raw=true)
	
	_Webjob Logs_

1. Show the logs and explain that everytime a Job runs or fails, it will add a new entry to this log.

	> **Note:** If you generated errors in your log, they will be displayed between the other logs.

<a name="segment4" />
### Azure Auto Scale, Traffic Manager and Backup ###

> **Speaking Point:** Explain that all the configuration such as SSL certificates and public domain names stay where they are, but the deployments are swapped. The site is readily available afterwards.

<a name="auto-scale" />
#### Task 1 - Auto Scale ####
1. In the Web Site, go to **Scale**.
2. Scroll down to **Scale by Metric** and select **CPU**. Change the instance count to **2** and **4**.

	> **Speaking Point:** Explain how the user can use the Target CPU threshold to automatically upscale or downscale the number of instances.
	
<a name="traffic-manager" />
#### Task 2 - Traffic Manager ####
1. Browse to the Azure Portal, and click New > Network Services > Traffic Manager > Quick Create.

	> **Speaking Point:** Explain the different Load Balancing Methods available.
	
1. Click Close, and go to the an existing Traffic Manager created.
1. Select Web Site in the Service Type combo.
1. Check the deployed Web Site to the Traffic Manager profile.

	> **Speaking Point:** Explain that no matter where the user is, they will always hit the data center closest to them.

<a name="backups" />
#### Task 3 - Backups ####
1. Go to the **Backups** section in the Web Site.
1. Enable **Automated Backup** by clicking **ON**
1. Select a storage account.

	> **Speaking Point:** Explain that the user can change the **Frequency** and the Start Date.
	
1. Select a database connection from the **Included Databases** drop-down.
1. Click **Save** to confirm.
1. Click **Backup Now** to create a backup.
1. Once completed, show how to restore a backup. Click **Restore Now**.

	> **Speaking Point:** Explain the two options to restore a backup: From a previous backup of the site or by selecting a file from the storage account. Select the second option and click the folder icon.
	
1. Browse the storage account you selected to generate the backup. Select the **websitebackups** container.
1. Select the backup file and click **OPEN**.
1. Click the arrow to continue to next page.
1. Show the options to restore a Web Site (_current_ or _new_). From the databases drop-down, select a server and show how you can set up credentials to restore the database. Click the **Automatically adjust connection strings** checkbox. **Do not Confirm changes, close the window to continue**