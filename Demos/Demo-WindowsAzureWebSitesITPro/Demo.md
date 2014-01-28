<a name="title" />
#Windows Azure Web Sites (PaaS)#

---
<a name="Overview" />
## Overview ##
In this demo, you will learn how to use Microsoft WebMatrix 2 to quickly build and publish Web sites in Windows Azure.  You do not need to be a developer in order to do this and it will help to illustrate the difference between Windows Azure IaaS and PaaS as well as show you one of the many non-IaaS services available on Windows Azure.

Microsoft WebMatrix is a free tool that allows you to create, customize and publish Web sites. WebMatrix includes a range of built-in templates to make it quick and easy to get started with your Web site, and it provides built-in publishing support to help you easily publish your Web sites to Windows Azure.
In this demo, we will create a Web site from the Windows Azure portal, and start to build the Web site locally using WebMatrix.

This is an example of Platform as a Service (PaaS).

<a name="Demo 1: Creating your first Windows Azure Web Site" />
## Demo 1: Creating your first Windows Azure Web Site ##

1. In the Azure portal, on the toolbar, click **New**.

	![New Web Site Button](./images/New-Web-Site-Button.png?raw=true "New Web Site Button")

	> **Speaking Point:**

	> In this demo, we will create a new Web site, and use WebMatrix to deploy an application to that Web site.

	> In Azure, this is an example of a Platform as a Service (PaaS) VM.

	> That means that we are responsible for the Web site content, but that Azure manages patching, scaling, availability, etc.


1. In the New window, Click **Compute**, click **Web Site**, and then click **Quick Create**.

	![Quick Create](./images/Quick-Create.png?raw=true "Quick Create")

	> **Speaking Point:**

	> We use the Quick Create option to quickly create a new Web site.

1. In the **URL** text box, type **Demo-NNNN**.

	> **Note:** The Web site URL must be unique within <name>.azurewebsites.net. Pick a four-digit number for NNNN that is not in use yet. If the green check mark does not appear, use a different number.

	![Web Site URL Dialog](./images/Web-Site-URL-Dialog.png?raw=true "Web Site URL Dialog")

	> **Speaking Point:**
	
	> We pick a unique name for our new Web site. The URL needs to be unique within **<name>.azurewebsites.net**.

1. In the **Region** drop-down list box, select the location where your storage account is created (for example: West US).

	![Select Region](./images/Select-Region.png?raw=true "Select Region")

1. Click **Create Web Site**.

	![Creating a Web Site](./images/Creating-a-Web-Site.png?raw=true "Creating a Web Site")

	> **Speaking Point:**
	
	> Azure will create a new Web site.

	> Notice how quickly the new Web site is provisioned. Usually this only takes a few seconds. The reason for the quick provisioning is that Azure has pre-started Web sites in the datacenter already.

1. In the Web sites window, click on the link to **[http://demo-NNNN.azurewebsites.net]().**

	![Verifying-Web-Site-Creation](images/Verifying-Web-Site-Creation.png?raw=true "Verifying Web Site Creation")

	![Web-Site-Created-Dialog](images/Web-Site-Created-Dialog.png?raw=true "Web Site Created Dialog")

	> **Speaking Point:**
	
	> The new Web site is up and running, but does not contain any content yet.

	> You can use several methods to copy your site content to the new Web site, including **FTP**, **GIT**, **Team Foundation Service**, or development tools such as **Visual Studio** or **WebMatrix**. For the demo, we use WebMatrix.

1. Close the **[http://demo-NNNN.azurewebsites.net]()** browser window. In the Azure portal, select the **Demo-NNNN** Web Site, and then on the toolbar, click the **WebMatrix** icon.

	![Installing Web Matrix](./images/Installing-Web-Matrix.png?raw=true "Installing Web Matrix")

	> **Speaking Point:**

	> In the demo environment, WebMatrix is already installed. Otherwise you are prompted to download and install WebMatrix first.

1. If the WebMatrix License agreement dialog box appears, select **I agree**, and then click the check mark icon to continue. If he Application Run warning dialog box appears, click **Run**.
	
	![Running Web Matrix Installer](./images/Running-Web-Matrix-Installer.png?raw=true "Running Web Matrix Installer")

	>**Speaking Point:**

	> After we accept the license agreement, and the warning message, WebMatrix opens.

1. In the WebMatrix window, on the **File** menu, click **New**, and then click **Site from Template Gallery** or In the Download window, click **Yes, install from Template Gallery**.


	![Installing pre-built templates](./images/Installing-pre-built-templates.png?raw=true "Installing pre-built templates")

	> **Speaking Point:**

	> WebMatrix contains several pre-built templates to get started quickly with your new Web site.

	> Notice that there are tons of pre-existing options to choose from here such as a WordPress site, etc.
In the demo, we will use the **Photo Gallery** template.

1. On the Site from Template page, select **ASP.NET**, click **Photo Gallery**, and then click **Next.**

	![Choosing Photo Gallery Template](./images/Choosing-Photo-Gallery-Template.png?raw=true "Choosing Photo Gallery Template")

	> **Speaking Point:**

	> WebMatrix will download all the files related to the Photo Gallery template, and load the files in WebMatrix.

1. Select the **Files** section.

	![Ading Files to Template](./images/Ading-Files-to-Template.png?raw=true "Ading Files to Template")

	> **Speaking Point:**

	> WebMatrix has four workspaces to customize and configure your new application: Site, Files, Databases and Reports.
	
	> In this demo, we are not going to customize the sample application.

1. On the ribbon, click **Publish**. On the Publish Preview page, select the **PhotoGallery.sdf** database.

	![Publishing a Web Site](./images/Publishing-a-Web-Site.png?raw=true "Publishing a Web Site")

	> **Speaking Point:**

	> To run the application files from our new Web site, WebMatrix will upload all the needed files to the Demo-NNNN Web site.


1. On the Publish Preview page, click **Continue**.

	![Copying Files to Web Site](./images/Copying-Files-to-Web-Site.png?raw=true "Copying Files to Web Site")

	> **Speaking Point:**

	> The files are copied to the Web site.

1. When the publishing is completed, in the WebMatrix notification bar, click the URL to the Web site **[http://demo-NNNN.azurewebsites.net]()**

	![Publishing Complete](./images/Publishing-Complete.png?raw=true "Publishing Complete")

	![Verifying Web Site](./images/Verifying-Web-Site.png?raw=true "Verifying Web Site")

	> **Speaking Point:**

	> The Photo Gallery site opens.

	> This result confirms that we successfully published a site to an Azure Web site.

1. Go back to the Dashboard of the website.

	![Viewing Web Site Dashboard](./images/Viewing-Web-Site-Dashboard.png?raw=true "Viewing Web Site Dashboard")

	> **Speaking Point:**

	> Now let’s do a quick check of what’s going on under the hood here with this Website that is running as a PaaS instance.

	> Remember earlier we showed you how to create a VM (IaaS) and the fact we can connect directly to the console of that VM.  We would need to manage, patch, and secure the OS ourselves.

	> In the dashboard, you can see the statistics of CPU, Data, storage, and memory usage, but you won’t see on the toolbar at the bottom the ability to connect to the website using RDP because it is PaaS.

1. Select the **Scale** tab, Click on **Reserved** and Open on the **Instance Size** drop-down box.

	![Changing Web Site Scale Mode](./images/Changing-Web-Site-Scale-Mode.png?raw=true "Changing Web Site Scale Mode")

	> **Speaking Point:**

	> Here we can see that we have the ability to change the website mode from Free to Shared or Reserved.

	> Let’s choose **Reserved** that will dedicate compute and memory resources to this website alone behind-the-scenes.

	> You’ll also notice you have the ability to choose from a number of options with the # of cores and size of memory.  We’ll stick with the default of **Small**.

1. Click **Save**, click **Yes** to upgrade, and show in the details how this was upgraded.

	![Changing Web Site Scale Mode](./images/image-0.png?raw=true "Changing Web Site Scale Mode")

1. On the Scale tab, increase the **Instance Count** from 1 to **3**, click **Save**, and click **Yes** to continue.

	![Upscaling Instace Count](./images/Upscaling-Instace-Count.png?raw=true "Upscaling Instace Count")

	> **Speaking Point:**

	> Now let’s increase the instance count.  As you can see this is a big advantage over IaaS since you can’t simply increase the # of identical VMs to scale a service in this manner.

1. On the Scale tab, change the Web site Mode back to **Free**, click **Save**, and click **Yes** to downgrade.

	![Downscaling Web Site Mode](./images/Downscaling-Web-Site-Mode.png?raw=true "Downscaling Web Site Mode")

	> **Speaking Point:**

	> Likewise, we can scale the site back down to Free mode quickly to avoid any charges.
