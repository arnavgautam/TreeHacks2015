<a name="HOLTop" />
# Getting Started with the Azure Preview Portal #

---

<a name="Overview" />
## Overview ##

The new Azure Preview portal is an all-in-one, work-anywhere experience. Now you can manage websites, databases, and Visual Studio Online team projects in a reimagined UX you personalize around your work. This unified hub radically simplifies building, deploying, and managing your cloud resources. Imagine a single easy-to-use console built just for you —your team, your projects. Now craft your very own best-in-class toolset by adding fully integrated capabilities from Microsoft, partners, and the open source community. Organize your portal to custom-fit your work, and your workstyle. Stay on top of the things that matter most by pinning them to your **Startboard**. Resize parts to show more or less data. Drill in for all the details. And see insights (and opportunities) across apps and resources. New components include the following:

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

[1]: http://www.microsoft.com/visualstudio/

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
### Exercise 1: Creating a Web Site + DB ###

In this exercise...

1. Open **Visual Studio Express 2013 for Web** .

<a name="Exercise3" />
### Exercise 3 : Creating Azure Environments using Azure Resource Manager ###


<a name="Ex3Task1" />
#### Task 1 - Downloading Resource Group Tempalte ####

1. Open Azure Powershell console.

1. Execute the following command to change from the _Azure_ module to the _Azure Resource Manager_ module.

	````PowerShell
	Switch-AzureMode AzureResourceManager
	````

	![Switch-AzureMode](Images/switch-azuremode.png?raw=true "Switch-AzureMode")
	
	_Switch-AzureMode command_
	
	 >**Note**: The AzureResourceManager module, introduced in Azure PowerShell version 0.8.0, lets you manage your resources in an entirely new way. Instead of creating individual resources and trying to use them together, begin by imagining the service you want to create, such as a web portal, a blog, a photo gallery, a commerce site, or a wiki.

	>Select a resource group template for the service, including one of dozens in the Azure template gallery, or create your own. Each template provides a model of a complex service, complete with the resources that you need to support the service. Then use the template to create a resource group and its resources, and deploy and manage the related resources as a unit.

	>Beginning in version 0.8,0, the Azure PowerShell installation includes the Azure and AzureResourceManager modules, and AzureProfile, a module of cmdlets common to both modules. The Azure and AzureResourceManager modules are not designed to work together in the same session.

	>When you use the Azure PowerShell cmdlets, the Azure module is imported into the session by default. To remove the Azure module from the session and import the AzureResourceManager and AzureProfile modules, use the Switch-AzureMode cmdlet.

1. Execute the following command to authenticate to Microsoft Azure and download the subsctiptions associated with the account.

	````PowerShell
	Add-AzureAccount
	````

1. Enter your credentials and click sign-in

	![Sign in to Windows Azure dialog box](Images/sign-in-to-windows-azure-dialog-box.png?raw=true "Sign in to Windows Azure dialog box")
	
	_Sign in to Windows Azure dialog box_

1. Once the authentication process completes, one of your subscription is set as the default subsctiption.

	![Add-AzureAccount](Images/add-azureaccount.png?raw=true "Add-AzureAccount")
	
	_Add-AzureAccount command_
	
1. Execute the following command to get a list of resources from the group gallery templates

	````PowerShell	
	Get-AzureResourceGroupGalleryTemplate
	````

	>**Note**: A resource group template is a JSON string that defines a resource group for a complex entity, such as a web portal, a blog, a photo gallery, a commerce site, or a wiki. The template defines the resources that are typically needed for the entity, such as web sites, database servers, databases and storage accounts, and includes parameters for user-defined values, such as the names and properties of the resources. To create a resource group with a template, just identify the template and provide values for its parameters.

1. Use the following command to review the gallery template and it's properties.

	````PowerShell
	Get-AzureResourceGroupGalleryTemplate -IdentityMicrosoft.WebSiteSQLDatabase.0.1.0-preview1	
	````

	![Get-AzureRourceGroupGalleryTemplate](Images/get-azurerourcegroupgallerytemplate.png?raw=true "Get-AzureRourceGroupGalleryTemplate")
	
	_Get-AzureRourceGroupGalleryTemplate command_
	
1. Download the simple web site creation template.

	````PowerShell
	Save-AzureResourceGroupGalleryTemplate -Identity Microsoft.WebSiteSQLDatabase.0.1.0-preview1 -Path G:\Azure\Templates	
	````
	![Save-AzureResourceGroupGalleryTeamplate](Images/save-azureresourcegroupgalleryteamplate.png?raw=true "Save-AzureResourceGroupGalleryTeamplate")
	
	_Save-AzureResourceGroupGalleryTeamplate command_
	
	>**Note**: The Save-AzureResourceGroupGalleryTemplate cmdlet to save the Microsoft.PhotoGallery.0.1.0-preview1 gallery template as a JSON file in the path that you specify.

TODO:  Introduce some sections of the file and insert some field values
	
<a name="anchor-name-here" />
#### Task 2 - Creating a Resource Group from a Custom Template ####

In this task you will update the JSON file from the Simple website template and use the Azure cmdlets to create the new Resource Group in Microsoft Azure.

1. Open the JSON file you downloaded in the previous task in Visual Studio and locate the **resources** section with **name** _paramters('database')_

	![Resources section in template.json file](Images/resources-section-in-templatejson-file.png?raw=true "Resources section in template.json file")
	
	_Resources section in template.json file_

1. Update the properties of the resource to create a Business edition with maxSizeBytes of 10 GB. The properties section should look like the one below.

	![Properties section update in template.json](Images/properties-section-update-in-templatejson.png?raw=true "Properties section update in template.json")
	
	_Properties section update in template.json_

1. Save the file and switch back to Azure PowerShell.

1. Switch to **AzureMode** using the following command

	````PowerShell
	Switch-AzureMode AzureServiceManagement
	````
	
1. Replace the _[STORAGE NAME]_ placeholder and execute the following command to create a new storage account. Make sure that the storage name you selected is unique.

	````PowerShell
	New-AzureStorageAcount -StorageAccountName [STORAGE NAME] -Location "West US"
	````

1. Switch back to **AzureResourceManager** mode using the following command

	````PowerShell
	SwitchMode AzureResourceManager
	````
	
1. Replace the placeholdares and execute the following command to create a new resource group using the custom template. Make sure to replace the _[STORAGE NAME]_ placeholder with the storage account you have created in the previous step.

	````PowerShell
	New-AzureResourceGroup -Location [LOCATION] -Name [RESOURCE-GROUP-NAME] -TemplateFile [JSON-File-Path]  –StorageAccountName [STORAGEACCOUNT] -siteName [WEBSITENAME] -hostingPlanName TestPlan -siteLocation "North Europe" -serverName [SERVERNAME] -serverLocation "West US" -administratorLogin Admin01 -databaseName [DATABASENAME] -Verbose
	````

	> **Note**: When you enter the command, you are prompted for the missing mandatory parameter, administratorLoginPassword. And, when you type the password, the secure string value is obscured. This strategy eliminates the risk of providing a password in plain text.
	
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

1. Mavigate to the Resouce Group and check that there is a website and a sql database...

	![Custom Resource Group](Images/custom-resource-group.png?raw=true "Custom Resource Group")
	
	_Custom Resource Group_
	
1. Click the delete button in the resource group to delete it.

	![Delete button](Images/delete-button.png?raw=true "Delete button")
	
	_Delete button_



---

<a name="Summary" />
## Summary ##

In this hands-on lab, ...
