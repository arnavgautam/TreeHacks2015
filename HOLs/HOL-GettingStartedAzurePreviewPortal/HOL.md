<a name="HOLTop" />
# Getting Started with the Azure Preview Portal #

---

<a name="Overview" />
## Overview ##

The new Azure Preview portal is an all-in-one, work-anywhere experience. Now you can manage websites, databases, and Visual Studio Online team projects in a reimagined UX you personalize around your work. It was built from the ground up to put your _applications_ at the center of the experience. 

This unified hub radically simplifies building, deploying, and managing your cloud resources. Imagine a single easy-to-use console built just for you —your team, your projects. It brings together all of the cloud resources, team members, and lifecycle stages of your application and provides you with a centralized place to plan, develop, test, provision, deploy, scale, and monitor those applications. This approach can help teams embrace a DevOps culture by bringing both development and operations capabilities and perspectives together in a meaningful way.

The new portal allows each user to transform the portal home page (called the _Startboard_) into their own customized dashboard. Stay on top of the things that matter most by pinning them to your **Startboard**. Resize parts to show more or less data. Drill in for all the details. And see insights (and opportunities) across apps and resources. New components include the following:

* **Simplified Resource Management**. Rather than managing standalone resources such as Microsoft Azure Web Sites, Visual Studio Projects or databases, customers can now create, manage and analyze their entire application as a single resource group in a unified, customized experience, greatly reducing complexity while enabling scale. Today, the new Azure Manager is also being released through the latest Azure SDK for customers to automate their deployment and management from any client or device. 

* **Integrated billing**. A new integrated billing experience enables developers and IT pros to take control of their costs and optimize their resources for maximum business advantage.

* **Gallery**. A rich gallery of application and services from Microsoft and the open source community, this integrated marketplace of free and paid services enables customers to leverage the ecosystem to be more agile and productive.

* **Visual Studio Online**. Microsoft announced key enhancements through the Microsoft Azure Preview Portal, available Thursday. This includes Team Projects supporting greater agility for application lifecycle management and the lightweight editor code-named “Monaco” for modifying and committing Web project code changes without leaving Azure. Also included is Application Insights, an analytics solution that collects telemetry data such as availability, performance and usage information to track an application’s health. Visual Studio integration enables developers to surface this data from new applications with a single click.

<a name="Objectives" />
### Objectives ###
In this hands-on lab, you will learn how to:

- Create a **Web Site + DB**
- Set up continuous integration using Team Project
- Customize and organize your Startboard
- [Optional] Create a new Resource Group using Azure Resource Manager (PowerShell)

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Visual Studio 2013 Ultimate][1]
- [Azure PowerShell][2]

[1]: http://www.microsoft.com/visualstudio/
[2]: http://go.microsoft.com/fwlink/p/?linkid=320376&amp;clcid=0x409

---

<a name="Exercises" />
## Exercises ##
This hands-on lab includes the following exercises:

1. [Creating a Web Site + DB](#Exercise1)
1. [Setting Up Continuous Integration using Team Project](#Exercise2)
1. [[Optional] Creating a Resource Group using Azure Resource Manager](#Exercise3)

Estimated time to complete this lab: **30 minutes**

>**Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Each predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in the steps that you should take into account.

<a name="Exercise1" />
### Exercise 1: Creating a Web Site + SQL ###

Historically, managing a resource (a user-managed entity such as a database server, database or web site,) in Microsoft Azure required you to perform operations against one resource at a time. When developing for the cloud today, we are oftentimes managing _individual resources_ (databases, storage, cloud services, virtual machines, and so on). It’s left up to us as cloud developers and IT professionals to piece these resources together in some meaningful way and manage them over time. In the Microsoft Azure Preview portal was designed to bring together all of the individual resources of an application into a consolidated view. Resource group is a new concept in Azure that serves as the lifecycle boundary for every resource contained within it. 

In this exercise, you will learn how to create a new Web Site and a SQL Server using the Azure Preview portal.

<a name="Ex1Task1" />
#### Task 1 – Creating a Website + SQL ####

In this task, you will login to the Azure Preview portal and create a new Web Site and SQL Server.

1. Open a browser and navigate to http://portal.azure.com. Log in using your credentials.

1. The first thing you will see is the **Startboard**. This is your home page, where you can see dynamic data and all the details you care about your resources. You can customize it as you see fit.

	> **Note:** You can right click on the tiles of the startboard to customize it. You can pin or unpin tiles and change their size.
	
	![Startboard](Images/startboard.png?raw=true)
	
	_Your Home Page: The Startboard_

1. On the left side, you will see the **Hub Menu**. This is your navigation menu to access all of your resources and options. Click the **New** button at the bottom of the **Hub Menu**.

	![Creating a new resource](Images/creating-a-new-resource.png?raw=true)
	
	_Creating a new resource_

1. A panel is displayed with different options. You can choose one of the options to create a new resource. In this case, you will select **Website + SQL**.

	![Selecting Website + SQL](Images/selecting-website-sql.png?raw=true)
	
	_Selecting Website + SQL_
	
	A _blade_ is opened. Blades are your entry point to discover insights, perform actions, and build applications. This particular _blade_ collects input from you to create a new **Website + SQL**.

	> **Note:** For more information about _blades_, you can click the **Tour** tile on your **Startboard**. On the **Tour** blade, scroll down to the bottom and click **Learn more**. A new blade is opened with further information. You can continue the **Tour** to learn the basics of Blades, Commands, Lenses and more.
	
	> ![Tour](Images/tour.png?raw=true)

1. When you are create an application that consists of a few resources working together (like in this example, a Website + SQL) it is always created in its own resource group, so you can manage the lifecycle of all related assets using the resource group. Choose a name for the **Resource Group**, for example _MyResourceGroup_, and click the **Website** option.

	> **Note:** Resources group names can only contain letters, numbers, periods, underscores and dashes.

	![New Resource Group](Images/new-resource-group.png?raw=true)
		
	_New Resource Group_

1. Another blade is opened which displays the options to create a new **Website**. Select an URL for your Website, for example _mynewazurewebsite_. Take into account that this name must be unique. Click the **Web Hosting Plan** option.

	![Changing the Web Hosting Plan](Images/changing-the-web-hosting-plan.png?raw=true)
	
	_Changing the Web Hosting Plan_

1. In the _Choose your pricing tier_ blade, choose the hosting plan that fits your needs and click **Select**. Web hosting plans represent a set of features and capacity that you can share across your web sites. Web hosting plans support a few pricing tiers (e.g. Free, Shared, Basic, and Standard) where each tier has its own capabilities. There are a couple of difference between these tiers. Plans in the Free and Shared tier provide sites with a shared infrastructure, meaning that your sites share resources with other customers' sites. Web hosting plans in the Basic and Standard tier provider sites with a dedicated infrastructure, meaning that only the site or sites you choose to associate with this plan will be running on those resources. At this tier you can configure your web hosting plan to use one or more virtual machine instances.

	> **Note:** For all tiers except 'Shared' you pay one price for the web hosting plan based on the tier and your chosen capacity and there is no additional charge for each site that uses the plan. Shared web hosting plans are different. Due to the nature of the shared infrastructure you are charged separately for each site in the plan. 
	
	![Selecting a Web Hosting Plan](Images/selecting-a-web-hosting-plan.png?raw=true)
	
	_Selecting a Web Hosting Plan_

1. Click **OK** to go back to the **Website** blade. You can change or leave the default location for the Website. Click **OK** to go to the previous blade.

1. Click **Database** to change the settings for your new database.

	![Changing your database settings](Images/changing-your-database-settings.png?raw=true)

	_Changing your database settings_

1. Set a name for the database, e.g. _mywebsite-db_, and click the **Server** option.

	![Database Settings](Images/database-settings.png?raw=true)	
	
	_Database Settings_


1. Enter the **server admin login** and a **password**. Click **OK** to go back to the Database blade and click **OK** to close it.
	
	![Configuring the Database Server](Images/configuring-the-database-server.png?raw=true)

	_Configuring the Database Server_
	
1. Now you are ready to create your resource group. Click **Create**.

	![Configured Resource Group](Images/configured-resource-group.png?raw=true)
	
	_Configured Resource Group_

1. You can see when the new resource group is created by accessing the **Notifications**. On the **Hub Menu**, click **Notifications**.

	![Notifications](Images/notifications.png?raw=true)
	
	_Notifications_
	
1. Once completed, you can click the notification to open the resource group blade.

	![Created Resource Group Notification](Images/created-resource-group-notification.png?raw=true)
	
	_Created Resource Group Notification_

1. You created your new resoure group, which includes a Website and SQL Server database.

	![New Resource Group Blade](Images/new-resource-group-blade.png?raw=true)
	
	_New Resource Group Blade_

<a name="Exercise2" />
### Exercise 2 :  Creating a team project and using Continuous Deployment

In this exercise you will create a new a new Team Project, and enable continuous integration in the project. Therefore when you commit any changes to the project repository, an automatic deployment will be fired. Additionally, you will see how to update the code from within the portal and commit those changes, triggering an automatic build.

<a name="Ex2Task1" />
#### Task 1 - Creating a team project ####

In this task you will learn how to create a **Team Project** using the new Azure Portal.

1. In the Azure Portal, click **New** and select **Team Project**.
  
	![Creating a new Team Project](Images/creating-a-new-team-project.png?raw=true)

	_Creating a New Team Project_
 
1. In the **New Team Project** blade, enter a name for the Team Project.

	![New Team Project Blade](Images/new-team-project-blade.png?raw=true)

	_New Team Project Blade_
 
1. Click the **Version Control** tab to see the different options you have. 

	![Checking the Version Control options](Images/checking-the-version-control-options.png?raw=true)

	_Checking the Version Control options_

1. Select **GIT** and click **Select**. Additionally, you can check the available **Process Template** options.

	> **Note:** The process templates define the set of work item types, queries and reports that you will use to plan an track your project. The available **Process Templates** are: Scrum 2013, Agile 2013, and CMMI 2013.

1. Click the **Account** tab, and the click **Create New**. Enter the name for the new _visualstudio.com_ account. Click **OK** to confirm the settings.
 
	![Creating a new Visual Studio Online Account](Images/creating-a-new-visual-studio-online-account.png?raw=true)

	_Creating a new Visual Studio Online Account_

1. Enter the name for the new _.visualstudio.com_ account. For example, _MyTestTeamProject_. Click **OK** to confirm the account, and then click **Create** to create the team project.
 
1. Wait till the new **Team Project** is created. You will see the summary of it.
 
	![The Team Project is Created](Images/the-team-project-is-created.png?raw=true)
	
	_The Team Project is Created_

<a name="Ex2Task2" />
#### Task 2 - Enabling Continuous Integration ####

In this task, you will set up continuous deployment in your Team Project. Then you will check in code to see how an automatic deployment is triggered.

1. In the Azure portal, click your pinned team project and scroll down the blade to see the **Set up Continuous Deployment** part and click on it.
 
	![Setting Up Continuous Deployment](Images/setting-up-continuous-deployment.png?raw=true)

	_Setting Up Continuous Deployment_

1. Click the **Website** tab and choose the website you created in a previous exercise. Click **Select** to continue.

	![Selecting the Website to Deploy](Images/selecting-the-website-to-deploy.png?raw=true)

	_Selecting the Website to Deploy_
 
1. In the **Repository** tab, select your repository and click **Select**.

	![Selecting a Repository](Images/selecting-a-repository.png?raw=true)

	_Selecting a Repository_
 
1. Lastly, in the **Branch** tab make sure that the **default** branch is selected and click **Select**.
	
1. Click create in the **Set up deployment** blade.

	![The deployment is configured](Images/the-deployment-is-configured.png?raw=true)
 
	_The deployment is configured_
	
	Notice the message that says that a build will begin after code is added to your project.
 
1. Click the **Add code to your repository** part, to open the **Repository** part.
 
	![Adding code to your repository](Images/adding-code-to-your-repository.png?raw=true)
	
	_The Repository blade_

1. Click the **Clone** command. You will get information about how to clone your repository.
 
	![Getting Information on how to clone the repository](Images/getting-information-on-how-to-clone-the-repos.png?raw=true)
	
	_Getting information on how to clone the repository_

1. Go back to the **Team Project** blade and click the **Open in Visual Studio** part. This will add the team project automatically to Visual Studio and will open it.

	> **Note:** You will be prompted to accept that you want to open the application link with Visual Studio.

1. In Visual Studio's **Team Explorer** double-click the added team project, and click **Clone Repository**.

1. You can modify the local path of your repository to the folder of your choice if you wish. Click **Clone**.

	![Cloning Your Repository](Images/cloning-your-repository.png?raw=true)
	
	_Cloning Your Repository_
 
1. Once the repository is cloned successfully, at the bottom of the **Team Explorer** pane, click **New** in the **Solutions** section.

	![Creating a New Solution in Source Control](Images/creating-a-new-solution-in-source-control.png?raw=true)
	
	_Creating a New Solution in Source Control_
 
1. Select **Visual C# / Web / ASP.NET Web Application** in the **New Project** dialog. Enter _MyTestWebApplication_ as the Solution Name, and click **OK**. Notice that the **Add to Source Control** option is selected by default.

	![Creating a new Project](Images/creating-a-new-project.png?raw=true)
	
	_Creating a new Project_
 
1. Select **MVC** in the **New ASP.NET Project** dialog, and click **OK**.

	![Selecting the ASP.NET Project type](Images/selecting-the-aspnet-project-type.png?raw=true)
 
	_Selecting the ASP.NET Project type_
	
1. Your project will be created. Press **CTRL+SHIFT+B** to build it.

1. In the **Team Explorer** pane, click **Changes**.
Enter a commit message and verify that all the solutions files are included in the commit. Click **Commit**.
 
	![Committing the Changes](Images/commiting-the-changes.png?raw=true)
	
	_Committing the Changes_

1. Once the commit is created locally, click **Sync** to share these changes with the server.
 
1. In **Unsynced Commits**, you will see the list of local commits that will be uploaded to the server. Click the **Sync** button to do so. 

	![Syncing the changes in the server](Images/syncing-the-changes-in-the-server.png?raw=true)
	
	_Syncing the changes in the server_

1. When the syncing is complete, go to the Azure portal. Open your pinned team project part. After some time you will see that the commit you made is automatically deployed.

	![Checking the status of the team project](Images/checking-the-status-of-the-team-project.png?raw=true)
	
	_Checking the status of the team project_
 
1. Click the **Latest Build** part. This will display details of the automatic deploy.

	![Latest build Details](Images/latest-build-details.png?raw=true)

	_Latest build Details_

1. In this blade, click the **Browse** command, to go to the deployed website.
 
	![Browsing the deployed Site](Images/browsing-the-deployed-site.png?raw=true)
	
	_Browsing the deployed Site_

<a name="Ex2Task3" />
#### Task 3 - Updating the code from the Azure Portal ####

In this task, you will modify your code from the Azure Portal and then commit those changes. This is useful if you want to make a quick fix in your code from a device that do not have Visual Studio installed. After the commit, as continuous deployment is enabled, a deployment will be fired and you will see the updated site live.

1. In the Azure Portal, click your pinned Team Project.

1. In the **Team Project** blade, click the **Branches** part, where the commits are shown.

1. In the **Repository** blade, click the **Code** part.

	![Browsing the Code of the Team Project](Images/editing-the-code-of-the-team-project.png?raw=true)
	
	_Browsing the Code of the Team Project_

1. In the code blade, browse to _MyTestWebApplication\MyTestWebApplication\Views\Home\Index.cshtml_ file.

	![editing a file](Images/editing-a-file.png?raw=true)
	
	_Editing a file_
 
	Notice that the code of the file will be displayed in a new blade.

1. Click **Edit** to modify the code online. Update the line where the **H1** tags are, replacing _“ASP.NET”_ with _“My Updated App from the portal”_.
 
	![The edited Index file](Images/the-edited-index-file.png?raw=true)
	
	_The edited Index file_

1. Click the **Commit** command, to commit your changes.
In the **Commit** blade, enter a comment and click **OK**.
 
	![Committing the Changes from the portal](Images/commiting-the-changes-from-the-portal.png?raw=true)
	
	_Committing the Changes from the portal_

1. Go back to the **Team Project** blade and note that an additional commit has been generated after we edited and checked in the code from the portal.

	![New commit generated](Images/new-commit-generated.png?raw=true)
	
	_New Commit generated_
 
1. In the **Build Definitions** part, you can see the progress of the build that was triggered due to the commit.
 
	![Build Definitions blade](Images/build-definitions-blade.png?raw=true)
	
	_Build Definitions blade_

1. In the **Latest Build** part, click the link of the deployed website to open it.

	![Opening the website details blade](Images/opening-the-website-details-blade.png?raw=true)
	
	_Opening the website details blade_
 
1. In the **Website** blade, click the **Browse** command to open the WebSite.
 
	![The updated website](Images/the-updated-website.png?raw=true)
	
	_The Updated Website_
	
<a name="Exercise3" />
### Exercise 3 : Creating Azure Environments using Azure Resource Manager ###

Azure Resource Manager introduces an entirely new way of thinking about your Azure resources. Instead of creating and managing individual resources, you begin by imagining a complex service, such as a blog, a photo gallery, a SharePoint portal, or a wiki. You use a template -- a resource model of the service -- to create a resource group with the resources that you need to support the service. Then, you can manage and deploy that resource group as a logical unit.

In this exercise, you learn how to use Azure PowerShell with Resource Manager for Microsoft Azure. You will go through the process of downloading, updating and creating a resource group for a Web site with a SQL database.

<a name="Ex3Task1" />
#### Task 1 - Downloading Resource Group Template ####

In this task you will you will use Azure PowerShell for Azure Resources to list the available templates from the gallery and then download one JSON template for creating a Web site and a SQL Database.

1. Open Azure PowerShell console.

1. Execute the following command to change from the _Azure_ module to the _Azure Resource Manager_ module.

	````PowerShell
	Switch-AzureMode AzureResourceManager
	````

	![Switch-AzureMode](Images/switch-azuremode.png?raw=true "Switch-AzureMode")
	
	_Switch-AzureMode command_
	
	 >**Note**: The **AzureResourceManager** module, introduced in Azure PowerShell version 0.8.0, lets you manage your resources in an entirely new way. Instead of creating individual resources and trying to use them together, begin by imagining the service you want to create, such as a web portal, a blog, a photo gallery, a commerce site, or a wiki.

	>Select a resource group template for the service, including one of dozens in the Azure template gallery, or create your own. Each template provides a model of a complex service, complete with the resources that you need to support the service. Then use the template to create a resource group and its resources, and deploy and manage the related resources as a unit.

	>Beginning in version 0.8,0, the Azure PowerShell installation includes the Azure and **AzureResourceManager** modules, and **AzureProfile**, a module of cmdlets common to both modules. The Azure and **AzureResourceManager** modules are not designed to work together in the same session.

	>When you use the Azure PowerShell cmdlets, the Azure module is imported into the session by default. To remove the Azure module from the session and import the **AzureResourceManager** and **AzureProfile** modules, use the **Switch-AzureMode** cmdlet.

1. Execute the following command to authenticate to Microsoft Azure and download the subsctiptions associated with the account.

	````PowerShell
	Add-AzureAccount
	````

1. In the **Sign-in to Windows Azure** dialog box, enter your **Microsoft Account** and **Password** and click sign-in

	![Sign in to Windows Azure dialog box](Images/sign-in-to-windows-azure-dialog-box.png?raw=true "Sign in to Windows Azure dialog box")
	
	_Sign in to Windows Azure dialog box_

1. Once the authentication process completes, one of your subscription is set as the default subscription.

	![Add-AzureAccount](Images/add-azureaccount.png?raw=true "Add-AzureAccount")
	
	_Add-AzureAccount command_
	
	>**Note**: The **Set-AzureSubscription** cmdlet configures common settings including subscription ID, management certificate, and custom endpoints. The settings are stored in a subscription data file in the user’s profile or in a user specified file. Multiple subscription data sets are supported and identified by a subscription name. To select a subscription and make it current, use the Select-AzureSubscription cmdlet. 
	
1. Execute the following command to get a list of resources from the group gallery templates.

	````PowerShell	
	Get-AzureResourceGroupGalleryTemplate
	````

	>**Note**: A resource group template is a JSON string that defines a resource group for a complex entity, such as a web portal, a blog, a photo gallery, a commerce site, or a wiki. The template defines the resources that are typically needed for the entity, such as web sites, database servers, databases and storage accounts, and includes parameters for user-defined values, such as the names and properties of the resources. To create a resource group with a template, just identify the template and provide values for its parameters.

1. You can review the gallery template and its properties, such as icons and screenshots. Use the **Get-AzureResourceGroupGalleryTemplate** command to review the **Microsoft.WebSiteSQLDatabase.0.1.0-preview1** template and its properties.

	````PowerShell
	Get-AzureResourceGroupGalleryTemplate -Identity Microsoft.WebSiteSQLDatabase.0.1.0-preview1	
	````

	![Get-AzureRourceGroupGalleryTemplate](Images/get-azurerourcegroupgallerytemplate.png?raw=true "Get-AzureRourceGroupGalleryTemplate")
	
	_Get-AzureRourceGroupGalleryTemplate command_
	
1.  To save a gallery template as a JSON file, use the **Save-AzureResourceGroupGalleryTemplate** cmdlet. Download the **Microsoft.WebSiteSQLDatabase.0.1.0-preview1** template executing the following command.

	````PowerShell
	Save-AzureResourceGroupGalleryTemplate -Identity Microsoft.WebSiteSQLDatabase.0.1.0-preview1 -Path [FILE-PATH]	
	````
	![Save-AzureResourceGroupGalleryTeamplate](Images/save-azureresourcegroupgalleryteamplate.png?raw=true "Save-AzureResourceGroupGalleryTeamplate")
	
	_Save-AzureResourceGroupGalleryTeamplate command_

1. Open the File Explorer in the path were you saved the template and check that the file was correctly saved.

<a name="Ex3Task2" />
#### Task 2 - Creating a Resource Group from a Custom Template ####

In this task you will update the JSON file from the Simple website template and use the Azure cmdlets to create the new Resource Group in Microsoft Azure.

1. Open the JSON file you downloaded in the previous task in Visual Studio and locate the **parameters** section.

1. You will create your custom template by updating the websites with SQL Database template to use the site name parameter with the _\_db_ prefix as the database name. To do so, remove the parameter _databaseName_ as it is no longer required.

1. Locate the **resources** section with type _databases_ and replace the **name** property with the following code.

	<!-- mark:4 -->
	````JSON
	...
	resources": [
        {
          "name": "[concat(parameters('siteName'), '_db')]",
          "type": "databases",
          "location": "[parameters('serverLocation')]",
          ...
        },
	...
	````
1. Locate the **resource** section with **type** _config_ and replace the **ConnectionString** property with the following.

	<!-- mark:12 -->
	````JSON
	"resources": [
        {
          "apiVersion": "2014-04-01",
          "type": "config",
          "name": "web",
          "dependsOn": [
            "[concat('Microsoft.Web/Sites/', parameters('siteName'))]"
          ],
          "properties": {
            "connectionStrings": [
              {
                "ConnectionString": "[concat('Data Source=tcp:', reference(concat('Microsoft.Sql/servers/', parameters('serverName'))).fullyQualifiedDomainName, ',1433;Initial Catalog=', parameters('siteName'), '_db', ';User Id=', parameters('administratorLogin'), '@', parameters('serverName'), ';Password=', parameters('administratorLoginPassword'), ';')]",
                "Name": "DefaultConnection",
                "Type": 2
              }
            ]
          }
        }
	````

	>**Note**: Notice that we only replaced the **parameters('databaseName')** with **parameters('siteName'), '_db'** in the connection string.
	
1. Save the file and switch back to Azure PowerShell.

1. Switch to **AzureMode** using the following command.

	````PowerShell
	Switch-AzureMode AzureServiceManagement
	````
	
1. Replace the _[STORAGE NAME]_ placeholder and execute the following command to create a new storage account. Make sure that the storage name you selected is unique.

	````PowerShell
	New-AzureStorageAcount -StorageAccountName [STORAGE NAME] -Location "West US"
	````

1. Switch back to **AzureResourceManager** mode using the following command.

	````PowerShell
	SwitchMode AzureResourceManager
	````
	
1. Replace the placeholders and execute the following command to create a new resource group using the custom template. Make sure to replace the _[STORAGE NAME]_ placeholder with the storage account you have created in the previous step.

	````PowerShell
	New-AzureResourceGroup -Location [LOCATION] -Name [RESOURCE-GROUP-NAME] -TemplateFile [JSON-File-Path]  –StorageAccountName [STORAGEACCOUNT] -siteName [WEBSITENAME] -hostingPlanName TestPlan -siteLocation "North Europe" -serverName [SERVERNAME] -serverLocation "West US" -administratorLogin Admin01 -databaseName [DATABASENAME] -Verbose
	````

	> **Note**: When you enter the command, you are prompted for the missing mandatory parameter, **administratorLoginPassword**. And, when you type the password, the secure string value is obscured. This strategy eliminates the risk of providing a password in plain text.
	
1. Enter the **administratorLoginPassword** and press **Enter**.

	![New-AzureResourceGroup](Images/new-azureresourcegroup.png?raw=true "New-AzureResourceGroup")
	
	_New-AzureResourceGroup command_
	
1. Open Internet Explorer and browse the [Azure Portal](http://azure.portal.com)

1. Click the **Browse** button from the menu on the left side of the window.

	![Browse button](Images/browse-button.png?raw=true "Browse button")
	
	_Browse button_

1. In the **Browse** menu, click on **Resource groups**.

	![Resource groups](Images/resource-groups.png?raw=true "Resource groups")
	
	_Resource groups_
	
1. Notice that in the Resource groups pane, there is a list of resource. Check that your resource group was created.

	![Resources groups list](Images/resources-groups-list.png?raw=true "Resources groups list")
	
	_Resources groups list_

1. Navigate to the Resouce Group and check that there is a website and a SQL database with the namesyou defined in PowerShell.

	![Custom Resource Group](Images/custom-resource-group.png?raw=true "Custom Resource Group")
	
	_Custom Resource Group_
	
1. Click the delete button in the resource group to delete it.

	![Delete button](Images/delete-button.png?raw=true "Delete button")
	
	_Delete button_

---

<a name="Summary" />
## Summary ##

In this hands-on lab, ...
