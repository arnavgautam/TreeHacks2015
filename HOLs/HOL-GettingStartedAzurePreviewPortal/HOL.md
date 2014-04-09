<a name="HOLTop" />
# Getting Started with the Azure Preview Portal #

---

<a name="Overview" />
## Overview ##

TBC

<a name="Objectives" />
### Objectives ###
In this hands-on lab, you will learn how to:

- TBC

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Visual Studio Express 2013 for Web][1] or greater

[1]: http://www.microsoft.com/visualstudio/

<a name="Setup" />
### Setup ###
In order to run the exercises in this hands-on lab, you will need to set up your environment first.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.
1. Right-click on **Setup.cmd** and select **Run as administrator** to launch the setup process that will configure your environment and install the Visual Studio code snippets for this lab.
1. If the User Account Control dialog is shown, confirm the action to proceed.

> **Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="CodeSnippets" />
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of this code is provided as Visual Studio Code Snippets, which you can access from within Visual Studio 2013 to avoid having to add it manually. 

>**Note**: Each exercise is accompanied by a starting solution located in the **Begin** folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and may not  work until you have completed the exercise. Inside the source code for an exercise, you will also find an **End** folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.

---

<a name="Exercises" />
## Exercises ##
This hands-on lab includes the following exercises:

1. [A](#Exercise1)
1. [B](#Exercise2)

Estimated time to complete this lab: **[TBC] minutes**

>**Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Each predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in the steps that you should take into account.

<a name="Exercise1" />
### Exercise 1: A ###

In this exercise, ...

<a name="Ex1Task1" />
#### Task 1 – A ####

In this task 

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
