<a name="Title"></a>
# Building and Publishing Web Applications with Microsoft Azure Websites and WebMatrix 2#

---
<a name="Overview"></a>
## Overview ##

In this Hands-on Lab you will learn how to use Microsoft WebMatrix 2 to build and publish Websites in Microsoft Azure.

Microsoft WebMatrix is a free tool that allows you to create, customize and publish Websites. WebMatrix includes a range of built-in templates to make it quick and easy to get started with your web site code and provides built-in publishing support to help you easily publish your Websites to Microsoft Azure.

Following the exercises, you will create a new Web Site from the Microsoft Azure Management Portal and start to build your Web Site locally using WebMatrix. By using a WebMatrix out-of-the-box template, you will quickly create a photo gallery application and customize it by adding a Facebook Like button. Finally, you will publish the resulting web application to Microsoft Azure using WebMatrix built-in publishing support.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Create a Web Site from the Microsoft Azure Management Portal
- Download and install Microsoft WebMatrix
- Take advantage of the WebMatrix Photo Gallery template
- Publish the Photo Gallery application using WebMatrix publishing feature
- Use NuGet Packages to customize the Photo Gallery application

<a name="Prerequisites"></a>
### Prerequisites ###

- A Microsoft Azure subscription with the Websites Preview enabled - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

---
<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1.	[Creating a Microsoft Azure Web Site and Installing WebMatrix 2](#Exercise1)
1.	[Trying out the Photo Gallery Application](#Exercise2)
1.	[Using NuGet Packages to Customize the Photo Gallery Application](#Exercise3)


Estimated time to complete this lab: **45** minutes.

<a name="Exercise1"></a>
### Exercise 1: Creating a Microsoft Azure Web Site and Installing WebMatrix 2 ###

In this exercise, you will use the Microsoft Azure Management Portal to create a new Web Site. Then, you will download and install WebMatrix to publish the Photo Gallery template within your web site.

<a name="Ex1Task1"></a>
#### Task 1 – Creating a New Microsoft Azure Web Site ####

In this task, you will create a new Web Site from the Microsoft Azure Management Portal.

1. Open [Microsoft Azure Management Portal](https://manage.windowsazure.com) and log in with your Microsoft Azure Account's credentials.

1. Click **New** | **Web Site** | **Quick Create**. Select a unique **URL** for your web site, for example _WebMatrixWebSite_.

	![Creating a new Web Site](images/creating-a-new-web-site.png?raw=true "Creating a new Web Site")

	_Creating a new Web Site_

	> **Note:** A Microsoft Azure Web Site is the host for a web application running in the cloud that you can control and manage. The Quick Create option allows you to deploy a completed web application to the Microsoft Azure Web Site from outside the portal. It does not include steps for setting up a database.

1. Wait until the Web Site is **Running** before continuing to the next task.

<a name="Ex1Task2"></a>
#### Task 2 – Creating a Web Site using WebMatrix Templates ####

In this task, you will download and install WebMatrix  and then create a site using the Photo Gallery template.

1. In the **Microsoft Azure Management Portal**, click Websites and then click the Web Site **name** you created in the previous task to go to its **Dashboard**.

	![Websites page](images/web-sites-page.png?raw=true "Websites page")

	_Websites page_


1. In the **Dashboard** page, click **WebMatrix** from the bottom menu. 

	![Opening WebMatrix from the Portal](images/accessing-webmatrix-from-the-portal.png?raw=true "Opening WebMatrix from the Portal")

	_Opening WebMatrix from the Portal_

1. If a **Security Warning** dialog is shown, click **Run**.

	![Security Warning](images/security-warning.png?raw=true "Security Warning")

	_Security Warning_

1. It will start the installation of **Microsoft WebMatrix** using **Web Platform Installer**.

	![WebMatrix Installation using Web Platform Installer](images/webmatrix-installation-using-wpi.png?raw=true "WebMatrix Installation using Web Platform Installer")

	_WebMatrix Installation using Web Platform Installer_

	>**Note:** The Microsoft Web Platform Installer is a free tool that may be used to install a variety of Microsoft products, including WebMatrix.

1. Wait until the installation finishes.

	![WebMatrix Installed](images/webmatrix-installed.png?raw=true "WebMatrix Installed")

	_WebMatrix Installed_

1. Once the installation finishes, **WebMatrix** will be automatically launched and start downloading your site. Then, **WebMatrix** will start creating a local copy of your remote site. In this case, an empty site, so it will not take too long to complete the process.

	![Copying Files](images/copying-files.png?raw=true "Copying Files")

	_Copying Files_

1. **WebMatrix** will detect that your site is empty and will show an **Empty Site Detected** message. Click **Yes, install from the Template Gallery** to continue.

	![Install from Template Gallery](images/install-from-template-gallery.png?raw=true "Install from Template Gallery")

	_Installing from Template Gallery_

1. In the **Site from Template** page, select the **ASP.NET** tab and then the **Photo Gallery** template. Finally, set a **Site Name** (for example _AzurePhotoGallery_) and click **Next**.

	![Photo Gallery Template](images/photo-gallery-template.png?raw=true "Photo Gallery Template")

	_Photo Gallery Template_

	> **Note:** WebMatrix Website templates include pre-built pages and files for common types of websites, which you can customize to create your own site. You can pick from a number of different templates.

1. Wait until the web site installation finishes. During this process, WebMatrix will download all the necessary files for the Photo Gallery template.

	![Downloading Photo Gallery Template](images/downloading-photo-gallery-template.png?raw=true "Downloading Photo Gallery Template")

	_Downloading Photo Gallery Template_

1. Once the installation is complete, in **WebMatrix**, notice that there are four available tabs: **Site**, **Files**, **Databases** and **Reports**. 

	![Exploring Web Site](images/exploring-web-site.png?raw=true "Exploring Web Site")

	_Exploring Web Site_

	>**Note:** WebMatrix includes four integrated workspaces that help you focus on different areas of your Web site:
	> 
	> - **Site**: Monitor real-time web requests and configure your Web site server settings with the Site workspace.
	> - **Files**: Manage your files and edit your code using the code editor with syntax highlighting in the Files workspace. 
	> - **Databases**: Add and manage databases using the Database workspace.
	> - **Reports**: Generate SEO reports and optimize your web site for search engines using tools.


<a name="Ex1Task3"></a>
#### Task 3 - Publishing a Web Site from WebMatrix ####

In this task, you will publish the Photo Gallery template you created in the previous task to your Microsoft Azure Web Site using WebMatrix.

1. Click **Publish** to start publishing the **Photo Gallery** to the web site you created in the previous task.

	![Publish Web Site](images/publish-web-site.png?raw=true "Publish Web Site")

	_Publishing Web Site_

1. In the **Publish Preview** page, select **PhotoGallery.sdf** to deploy the database file along with the web site and click **Continue**.

	![Web Site Publish Preview](images/web-site-publish-preview.png?raw=true "Web Site Publish Preview")

	_Web Site Publish Preview_

	> **Note:** 	The Photo Gallery template uses a SQL Server Compact 4.0 database for storing photo data. SQL Server Compact provides a free, embedded, database engine that enables easy data storage within a single file. It does not require you to install a full database on your local development box. To learn more about SQL Server Compact download [the books online](http://www.microsoft.com/en-us/download/details.aspx?id=21880). 

1. Wait until the web site is published to Microsoft Azure. Once it finishes, click the link in the notification shown at the bottom to open the web site in a browser.

	![Publishing Complete](images/publishing-complete.png?raw=true "Publishing Complete")

	_Publishing Complete_

1. Verify that the **Photo Gallery** application has been published successfully. Do not close the browser.

	![Azure Photo Gallery Running](images/azure-photo-gallery-running.png?raw=true "Azure Photo Gallery Running")

1. Back in the **Microsoft Azure Management Portal**, go to the web site's **Dashboard** and see how the metrics chart is now reflecting the recent activity.

	![Metrics Chart](images/metrics-chart.png?raw=true "Metrics Chart")
	
	_Metrics Chart_

<a name="Exercise2"></a>
### Exercise 2: Trying out the Photo Gallery Application ###

In this task you will explore the Photo Gallery application you have published in [Exercise 1](#Exercise1). You will register as a photo gallery user and then explore the application to see how it works. You will upload some images, add comments to those pictures and see how thumbnails change according to the pictures you uploaded.

>**Note:** This exercise requires that you have the Photo Gallery application published in Microsoft Azure Websites as explained in Exercise 1.


<a name="Ex2Task1"></a>
#### Task 1 – Uploading Images and Adding Comments ####

1. Open the Photo Gallery web site you have published in the previous exercise.

	![Opening the web site](images/opening-the-web-site.png?raw=true "Opening the web site")

	_Opening the Photo Gallery web site_

	> **Note:** You can also get the published web site URL from the Microsoft Azure Portal in the site's Dashboard.

1. Click **Login** link and then click **Register** in order to register a new user. 

	![Click register](images/click-register.png?raw=true "Click register")

	_Register a new account_

1. Once in the **Account Creation** page, enter your **email**, a **password** and **password confirmation** for the new account. Then click **Register**.

	![Creating a new account](images/creating-a-new-account.png?raw=true "Creating a new account")

	_Creating a new account_

1. Once registered, click **Create a New Gallery**.

	![Creating a new gallery](images/creating-a-new-gallery.png?raw=true "Creating a new gallery")

	_Creating a new Gallery_

1. In the **New Gallery** dialog, set the Gallery's **Name** to _Photos_ and click **Create**.

	![Creating a new photo](images/creating-photos-gallery.png?raw=true "Creating a new photo")

	_Creating a new photo_

1. Once the new gallery is created, click **Upload a Photo** link in order to add a new photo to the gallery.

	![Click Upload a Photo](images/click-upload-a-photo.png?raw=true "Click Upload a Photo")

	_Uploading a new photo_

1. In the **Upload Photo** dialog, click the **Browse** and select an image file.

	![Click Browse](images/click-browse.png?raw=true "Click Browse")

	_Selecting an image_

1. Click **Upload** to add the selected image to the gallery. Notice that you can also add more than one image at a time to make the uploading process easier.

	![Select and upload a photo](images/select-and-upload-a-photo.png?raw=true "Select and upload a photo")

	_Uploading a new photo_

1. Once the image is uploaded, the site will show the new photo and its properties, allowing you to post a new comment on it. Type a comment in the **Comment** box and then click **Add Comment**.

	![Adding a comment](images/adding-a-comment.png?raw=true "Adding a comment")

	_Adding a comment_

1. Click the **Galleries** link from the navigation bar. A list with the available galleries will be displayed and you will see the **Photos** gallery showing the picture you have just uploaded.

	![Click Galleries](images/click-galleries.png?raw=true "Click Galleries")

	_Available photo galleries_

	![The new photo gallery](images/the-new-photo-gallery.png?raw=true "The new photo gallery")

	_The Photo gallery showing the new picture_

1. Return to the **Photos** gallery by clicking the gallery image. Repeat the steps to upload a second photo and add a comment to it.

1. Click **Galleries** link to display the available galleries. Note that the **Photos** gallery thumbnail has changed, showing that there are multiple photos uploaded.

	![Showing multiple images](images/showing-multiple-images.png?raw=true "Showing multiple images")

	_Thumbnail showing multiple images_

<a name="Exercise3"></a> 

### Exercise 3: Using NuGet Packages to Customize the Photo Gallery Application###

In this exercise, you will customize the Photo Gallery application you created in the previous exercise by using **NuGet Packages**.

>**Note:** This exercise requires that you have the Photo Gallery application published in Microsoft Azure Websites as explained in Exercise 1.

<a name="Ex3Task1"></a>
#### Task 1 – Installing the Facebook Helper NuGet Package####

In this task you will use **NuGet Packages** to install the **Facebook Helper** package.

1. If not already open, from the **Microsoft Azure Management Portal**, open **AzurePhotoGallery** project in **WebMatrix** using the **WebMatrix** icon within the Web Site's **Dashboard**.

	![Accessing WebMatrix from the Portal](images/accessing-webmatrix-from-the-portal.png?raw=true "Accessing WebMatrix from the Portal")

	_Accessing WebMatrix from the Portal_

1. In **WebMatrix**, click the **Files** tab.

    ![Switching to the Files tab](images/switching-to-the-files-tab.png?raw=true "Switching to the Files tab")

    _Switching to the Files tab_

1. Click **NuGet** within the ribbon bar to open the **NuGet Packages Gallery**.

	NuGet is a package management tool that allows you to download and install community-contributed code helpers for common tasks like social network integration and mobile support.

    ![Gallery icon](images/gallery-icon.png?raw=true "Gallery icon")

    _Opening NuGet Gallery_

1. In the **NuGet Gallery** dialog, select the **NuGet official package source** from the drop down list in the top-left corner.
    
    ![Selecting the Official package source](images/selecting-the-official-package-source.png?raw=true "Selecting the Official package source")
    
    _Selecting the Official package source_
    
1. In the search box, type _Facebook.Helper_. Select the **Facebook.Helper** gallery item and then click **Install**.

    ![Facebook Helper item](images/facebook-helper-item.png?raw=true "Facebook Helper item")
    
    _Selecting the Facebook.Helper item_

1. A dialog with additional information about the **Facebook.Helper** package will be shown. Click **Install** to download and install the package.
    
    ![Installing Facebook.Helper](images/installing-facebookhelper.png?raw=true "Installing Facebook.Helper")
	
    _Installing Facebook.Helper_
    
1. In the **License Agreement** dialog, click **I Accept** to complete the package installation.
    
    ![Accept the EULA](images/accept-eula.png?raw=true "Accept the EULA")
    
    _Accepting the EULA_

    At the bottom of the window, you will see the package installation status.
    
    ![Facebook Helper package status](images/facebook-helper-package-status.png?raw=true "Facebook Helper package status")
    
    _Facebook Helper package status_

1. In the **Files** workspace, expand your project and locate the new **Facebook** folder. Expand the **Facebook** folder and notice the files that have been added by the NuGet package you have just installed.

    ![Facebook folder](images/facebook-folder.png?raw=true "Facebook folder")
    
    _Facebook folder_
	
	> **Note:** The Facebook helper includes documentation for you to learn how to use all its features. You can view it by right-clicking the startHere.htm file located under the Facebook\Docs folder and selecting Launch in browser. The helper has a [CodePlex site](http://facebookhelper.codeplex.com) from where you can download a sample WebMatrix web site that shows the helper in action. This sample also shows you how to use the helper to integrate the Facebook login mechanism with your site.
	
<a name="Ex3Task2"></a>
#### Task 2 – Adding a Facebook Like Button ####

In this task you will use the **Facebook Helper** to insert a Like button in the Photo view.

1. In the **Site** workspace, expand the **Photo** folder  and double-click **View.cshtml** to open the file in the editor.

    ![Opening View.cshtml file](images/opening-viewcshtml-file.png?raw=true "Opening View.cshtml file")
    
    _Opening View.cshtml file_

1. Locate the following line of code in the HTML editor.
	
	````HTML
	<h1>@photo.FileTitle</h1>
	````

1. Add a new blank line under that line and type **`@Facebook`** in the HTML editor. Note the IntelliSense drop-down that appears to help you writing code.

    ![Intellisense for Facebook helper](images/intellisense-for-facebook-helper.png?raw=true "Intellisense for Facebook helper")
    
    _Intellisense for Facebook helper_

	> **Note:** IntelliSense provides an array of features that make language references easy to access (like List members, parameter info, complete word, etc.). The new version of WebMatrix has IntelliSense everywhere: HTML (including HTML5), CSS (including CSS3), JavaScript, C#, Visual Basic, and PHP.

1. Select **LikeButton** from the list. Make sure that the code added to the HTML document looks like the following code.

	````ASP.NET
	@Facebook.LikeButton()
	````

    ![Inserting a Facebook Like button](images/inserting-a-facebook-like-button.png?raw=true "Inserting a Facebook Like button")
    
    _Inserting a Facebook Like button_

1. Save the file and click **Publish**.

1. **Uncheck** the checkbox next to the **PhotoGallery.sdf** file to avoid re-publishing the database and preserve the data from the published site.
    
    ![Uncheck the PhotoGallery database](images/uncheck-the-photogallery-database.png?raw=true "Uncheck the PhotoGallery database")
    
    _Unchecking the PhotoGallery database option_

1. Click **Continue**. You can see the publishing status on the bottom bar.

    ![Publishing status](images/publishing-status.png?raw=true "Publishing status")
    
    _Publishing status_

1. Once publishing has completed, go back to the **AzurePhotoGallery** site or click the link at the bottom to open the site in the web browser.

    ![Publishing Complete](images/publishing-complete.png?raw=true "Publishing Complete")

	_Publishing Complete_

1. Click **Photos** gallery to view the images in the gallery.

1. Click an image from the gallery to open it and notice that the **Facebook Like** button is now visible on the page.

    ![Facebook Like button in place](images/facebook-like-button-in-place.png?raw=true "Facebook Like button in place")

    _Facebook Like button in place_

1. Click **Like** and notice the Facebook log in pop up to allow users to like or comment on the image.

    ![Facebook log in](images/facebook-authentication.png?raw=true "Facebook log in")
    
    _Facebook log in_
