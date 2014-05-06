<a name="HOLTop" />
# Understanding Azure Resource Manager #

---

<a name="Overview" />
## Overview ##

A growing expectation of any cloud offering is the ability to automate the deployment and management of infrastructure components, like virtual machines, networks, storage, and databases - and enable customers to build and manage higher level applications on top of these in a rich and friendly DevOps environment. The Azure Resource Manager (ARM) provides a **Language** (Azure Resource Manager Template Language) that allows a declarative, parameterized description (a **Template**) of a set of related resources, so that they may be deployed and managed as a unit. The Templates are text-based (JSON), making it easy to use them with source code control systems like TFS and Git.

<a name="Objectives" />
### Objectives ###
In this hands-on lab, you will learn how to:

- Create a new Resource Group using Azure Resource Manager
- Use the different sections of the Top-level Template Structure
- Create more advanced templates
- Configure Firewall Rules, Alerts, and Autoscaling
- Deploy a Website using the MSDeploy extension
- Remove Resources and Resource Groups

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Visual Studio 2013 Ultimate Update 2][1]
- [Azure PowerShell][2]

[1]: http://www.microsoft.com/visualstudio/
[2]: http://go.microsoft.com/fwlink/p/?linkid=320376&amp;clcid=0x409

---

<a name="Exercises" />
## Exercises ##
This hands-on lab includes the following exercises:

1. [Getting Started with the ARM Top-level Template Structure](#Exercise1)
1. [Advanced Template Configuration](#Exercise2)
1. [Cleaning up Resources using ARM](#Exercise3)

Estimated time to complete this lab: **45 minutes**

>**Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Each predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in the steps that you should take into account.

<a name="Exercise1" />
### Exercise 1: Getting Started with the ARM Top-level Template Structure ###

In this exercise you will learn about the **Azure Resource Manager** Template Language, its Top-level Template structure, and you will explore its different sections, learning about its usage and construction. 

<a name="Ex1Task1" />
#### Task 1 – Introduction to the ARM Template Language ####

In this task you will learn about the **ARM** Template Language and its Top-level Template structure.

This **Language** allows a declarative, parameterized description of a set of related resources so that they may be deployed and managed as a unit. There is a service that reads these Templates and orchestrates the creation of the resources they describe. Tools like _Visual Studio 2013 Update 2_ can read and write these Templates and even provide IntelliSense. The Templates are text-based (JSON), making it easy to use them with source code control systems like TFS and Git.

At a glance, the following is the Top-level Template structure.

````JavaScript
{
	"$schema": "http://schema.management.azure.com/schemas/2014-04-01-preview/deploymentTemplate.json#",
	"contentVersion" : "1.0",
	"parameters": { 
		// name/value pairs representing the template inputs
	},
	"variables": {
		// arbitrary JSON data used for constants and metadata
		// a JSON dictionary
		// Can be complex values like JSON arrays and objects
	},
	"resources": 
	[
		// An array of JSON nodes representing resources
	      {
			// A Resource
		},
		{
			// Another Resource
		}
	],
	"outputs": {
		// JSON declaration of output of the template
		// exact format below
	}    
}
````

1. Open your preferred text editor and paste the following Top-level Template Structure. You will use this to start constructing your own template.

	````JavaScript
	{
		"$schema": "http://schema.management.azure.com/schemas/2014-04-01-preview/deploymentTemplate.json#",
		"contentVersion" : "1.0",
		"parameters": { 
		},
		"variables": {
		},
		"resources": [
		]
	}
    ````
    
2. Save the template file locally with a **.json** extension. For example, _myTemplate.json_.

3. Open the JSON file you just created in _Visual Studio 2013 Update 2_.

4. In order to showcase Intellisense capabilities, in the **parameters** section, create a new parameter named _paramName_ as shown in the following code.

    <!-- mark:5-6 -->
    ````JavaScript
	{
		"$schema": "http://schema.management.azure.com/schemas/2014-04-01-preview/deploymentTemplate.json#",
		"contentVersion" : "1.0",
		"parameters": { 
			"paramName" : {
			}
		},
		"variables": {
		},
		"resources": 
		[
		]
	}
	````

5. Inside the curly brackets of the _paramName_ parameter, type **t** and wait for Visual Studio to provide you with IntelliSense options. You can also press **CTRL + D** to force Visual Studio to display IntelliSense options.

	![Visual Studio 2013 Update 2 providing Intellisense](Images/visual-studio-2013-update-2-providing-intelli.png?raw=true)
	
	_Visual Studio 2013 Update 2 providing IntelliSense for the ARM template_

6. Remove the _paramName_ parameter so the **parameters** section is empty.
	
	> **Note:** ARM provides a gallery of already created templates. These templates are useful for the most common resource group scenarios, such as _Websites with SQL_. To get the list of available templates in the gallery, use the following Azure PowerShell command while you are in _AzureResourceManager_ Azure mode.

	>	````PowerShell	
	Get-AzureResourceGroupGalleryTemplate
	````
	> Using the **-Identity** switch, you can review a gallery template and its properties, including icons and screenshots.
	> To save a template from the gallery, you can use the **Save-AzureResourceGroupGalleryTemplate** command, specifying the template name and the path where the file will be saved.

	>	````PowerShell
	Save-AzureResourceGroupGalleryTemplate -Identity [TEMPLATE-NAME] -Path [FILE-PATH]	

<a name="Ex1Task2" />
#### Task 2 – Understanding the Resources Section ####

In this task, you will learn about the Resource section, used for defining all the resources included in your resource group.

The **Resources** collection contains a JSON array of **Resource** objects. Each Resource object has the following top-level structure.

````JavaScript
{
	"name": "<resourcename>",
	"type": "<ResourceProviderNamespace/ResourceTypeName>",
	"location": "<location>",
	"dependsOn": 
	[
		// JSON array of resourcename strings
	],
	"tags":
	{
		// JSON name/value pairs that will be attached as runtime metadata
	},
	"properties":
	{
		// Settings for the resource, defined by the Resource Provider
	},
}
````

> **Note:** The following is the description of the different properties of the resource top-level structure.

> The _name_ property identifies a resource and is required.

> The _type_ property defines the type of the resource in the form _"\<ResourceProviderNamespace\>/\<ResourceTypeName\>"_. This type will be used to look up the Resource Provider that the Orchestrator will call to create or update the resources. This property is required.

> The _location_ property specifies the georegion, datacenter, or private cluster where the resource will be deployed. If not specified, the default value is the Location of the resourceGroup

> The _dependsOn_ property specifies that the processing of a specific resource depends on another resource.

> The _properties_ property is a bag containing name/value pairs. Every resource can have a section with properties. These are the settings that describe or configure the resource. 

1. You will create your custom template by adding  a website resource. To do this, locate the **resources** section, and below the comment add the following code. This code will add a very simple website to your resource group. Before adding the code, replace every _\<Your-Site-Name\>_ tag with a name of your choice, for example: _MyTestWebSite_, and every _\<Your-Location\>_ with the location of your choice (e.g.: _East US_).

	> **Note:** Take into account that Azure Websites names must be unique. Therefore, you must choose a name that has not previously been used. To avoid duplication, you can append a random number to the end or your desired name.
	
	<!-- mark:2-11 -->
	````JavaScript
	"resources": [
		{
			"apiVersion": "2014-04-01",
			"name": "<Your-Site-Name>",
			"type": "Microsoft.Web/Sites",
			"location": "<Your-Location>",
			"properties": {
				"name": "<Your-Site-Name>",
				"serverFarm": "<Your-Hosting-Plan-Name>"
			}
		},
	],
	````
	
2. In the **location** property of the resource, replace the _\<Your-Location\>_ value with _East Us_ or other Datacenter location.

	> Take note of the location you will use here. It is recommended, but not mandatory, that all the other resources of the resource group be hosted in the same location.

3. Choose a name for your hosting plan name, and replace the  _\<Your-Hosting-Plan-Name\>_ tag with the chosen name in the code created in the previous step.

4. The following code will create a **Hosting Plan** that will be used in the creation of the website. Add a new resource and replace the _\<Your-Hosting-Plan-Name\>_ tag with the name chosen in the previous step and paste this code in the resource sections of your template. Preferably, paste it before the Website resource.
	
	````JavaScript
	{
		"apiVersion": "2014-04-01",
		"name": "<Your-Hosting-Plan-Name>",
		"type": "Microsoft.Web/serverFarms",
		"location": "<Your-Location>",
		"properties": {
			  "name": "<Your-Hosting-Plan-Name>",
			  "sku": "Free",
			  "workerSize": "0",
			  "numberOfWorkers": 1
		}
	},
	````

5. As you may have noticed, the Hosting Plan resource needs to be created before the creation of the Website. This means that the Website depends on the Hosting Plan. Add the **dependsOn** property in the Website resource to indicate this dependency. This property is highlighted in the following code.

    <!-- mark:6-8 -->
	````JavaScript
	{
		"apiVersion": "2014-04-01",
		"name": "<YourSiteName>",
		"type": "Microsoft.Web/Sites",
		"location": "<Your-Location>",
		"dependsOn": [
			"Microsoft.Web/serverFarms/<YourHostingPlanName>"
		],
		"properties": {
			"name": "<Your-Site-Name>",
			"serverFarm": "<Your-Hosting-Plan-Name>"
		}
	},
	````

6. Add a new resource to the list, this time a **SQL Server**. To do this, add the following code in the resource section.

	````JavaScript
	{
		"name": "<Your-Server-Name>",
		"type": "Microsoft.Sql/servers",
		"location": "<Your-Location>",
		"apiVersion": "2.0",
		"properties": {
			"administratorLogin": "<Admin-User>",
			"administratorLoginPassword": "<Admin-Password>"
		}
	}
	````

7. Replace the _\<Your-Server-Name\>_ placeholder with a name for your SQL Server.

	> **Note:** Keep in mind that the SQL Server name must be all lowercase, you can use numbers, and the hyphen symbol.

8. Replace the _\<Your-Location\>_ placeholder with the location you used in the previous resources. Also, replace the _\<Admin-User\>_ and _\<Admin-Password\>_ placeholders with your preferred credentials for this server.
	
9. Save the template file.

10. Open **Azure PowerShell**.

11. If you have not added an Azure Account yet, type the following command:

	````PowerShell
	Add-AzureAccount
	````

12. The PowerShell command should open a dialog. Enter your Azure credentials and log in.

13. Replace the _[STORAGE NAME]_ placeholder and execute the following command to create a new storage account. Make sure that the storage name you selected is unique.

	You can skip this step if you prefer to use an already existing storage account of your own instead of creating a new one.

	````PowerShell
	New-AzureStorageAccount -StorageAccountName [STORAGE NAME] -Location "East US"
	````
	> **Note:** Take into account that the Storage Account Name must be all lowercase.

13. Switch mode to **AzureResourceManager** using the following command. In this mode, you will have access to the Cmdlets related to **Azure Resource Manager**.

	````PowerShell
	Switch-AzureMode AzureResourceManager
	````

	> **Note:** You can switch back to the Azure module executing _Switch-AzureMode AzureServiceManagement_.
	
14. Replace the placeholders and execute the following command to create your new resource group using the custom template. Make sure to replace the _[STORAGE NAME]_ placeholder with the storage account you created in the previous step.

	````PowerShell
	New-AzureResourceGroup –StorageAccountName [STORAGEACCOUNT] -TemplateFile [JSON-File-Path] -Name [RESOURCE-GROUP-NAME] -Location [LOCATION]
	````

	![New-AzureResourceGroup command](Images/new-azureresourcegroup-command.png?raw=true "New-AzureResourceGroup command")
	
	_New-AzureResourceGroup command_
	
15. Open Internet Explorer and browse to the [Azure Portal](http://azure.portal.com)

16. Click the **Browse** button from the Hub Menu on the left side of the window.

17. In the **Browse** menu, click **Resource groups**.

18. Notice that in the **Resource** groups pane, there is a list of resources. Check that your resource group was created. Navigate to the Resource Group and check that there is a website with the names you defined in the template.

	![Resource Group in the azure portal](Images/resource-group-in-the-azure-portal.png?raw=true "Resource Group in the azure portal")
	
	_Resource Group in the azure portal_

<a name="Ex1Task3" />
#### Task 3 – Understanding the Parameters Section ####

In this task, you will learn how to use parameters in your templates. Using parameters makes changing repetitive values easier. It also allows you to create templates that contain values that should be prompted to the user, for example, resource names or even credentials that can be masked when entered.

Parameters are defined in the **parameters** section of the Top-level template, using the following pattern.

````JavaScript
"<Parameter-Name>": {
	"type": "<Parameter-Type>",
	"allowedValues": [
		"value1",
		"value2",
		"value3"
	],
	"defaultValue": "value0"
},
````

To use a defined resource in the template, you specify it in the following way.

````JavaScript
[parameters("Parameter-Name")]
````

1. Open the template that you created in the previous exercise.

2. Let's parametrize the name of the resources. Start with the Website name by defining the following parameter in the parameters section of the template.

	<!-- mark:2-4 -->
	````JavaScript
	"parameters": {
		"siteName": {
			"type": "string"
		},
	}
	````

3. Now, locate the two instances where you hardcoded the name of the site, and replace them with the following code.

    ````JavaScript
    [parameters('siteName')]
    ````
    
    > **Note:** As this is a string type parameter, it is recommended that you use simple quotes when specifying the parameter name to escape the double quotes that define the whole string.
    
4. Continue defining the following simple string parameters. Simple string parameters can be used for the hosting plan name, the site location, the server name, server location, and the administration login.

	<!-- mark:5-19 -->
	````JavaScript
	"parameters": {
		"siteName": {
			"type": "string"
		},
		"hostingPlanName": {
			"type": "string"
		},
		"siteLocation": {
			"type": "string"
		},
		"serverName": {
			"type": "string"
		},
		"serverLocation": {
			"type": "string"
		},
		"administratorLogin": {
			"type": "string"
		},
	}
	````

5. Replace all the hardcoded values for the parameters defined in the previous step with calls to the parameter values. Your resulting template should look like the following code, in which the replacements are highlighted.

    <!-- mark:5,7,9,17,19,25,29,31,34 -->
    ````JavaScript
	"resources": 
	[
	{
		"apiVersion": "2014-04-01",
		"name": "[parameters('hostingPlanName')]",
		"type": "Microsoft.Web/serverFarms",
		"location": "[parameters('siteLocation')]",
		"properties": {
			"name": "[parameters('hostingPlanName')]",
			"sku": "Free",
			"workerSize": "0",
			"numberOfWorkers": 1
		}
	},
	{
		"apiVersion": "2014-04-01",
		"name": "[parameters('siteName')]",
		"type": "Microsoft.Web/Sites",
		"location": "[parameters('siteLocation')]",
		"dependsOn": [
			"Microsoft.Web/serverFarms/MyHostingPlan"
		],
		"properties": {
			"name": "[parameters('siteName')]",
			"serverFarm": "[parameters('hostingPlanName')]"
		}
	},
	{
		"name": "[parameters('serverName')]",
		"type": "Microsoft.Sql/servers",
		"location": "[parameters('serverLocation')]",
		"apiVersion": "2.0",
		"properties": {
			"administratorLogin": "[parameters('administratorLogin')]",
			"administratorLoginPassword": "Passw0rd!"
		}
	}
	],
	````
    
6. Look at the definition of the Website resource, and locate the **dependsOn** property. Notice that the value uses a predefined part (_Microsoft.Web/serverFarms/_) plus the value that you chose for the hosting plan name. For these cases, you can use the **concat** operator: [concat('value1', 'value2')]. Use this operator to append the hosting plan name parameter to the fixed path, as shown in the following code.

    <!-- mark:7 -->
    ````JavaScript
	{
		"apiVersion": "2014-04-01",
		"name": "[parameters('siteName')]",
		"type": "Microsoft.Web/Sites",
		"location": "[parameters('siteLocation')]",
		"dependsOn": [
			"[concat('Microsoft.Web/serverFarms/', parameters('hostingPlanName'))]"
		],
		"properties": {
			"name": "[parameters('siteName')]",
			"serverFarm": "[parameters('hostingPlanName')]"
		}
	},
    ````

    > **Note:** As the **concat** operator already uses brackets (**[]**), you do not need to use them when specifying the parameter.
    
7. You may have noticed that although the administrator login password is a string, we did not create a parameter for it, as it is not recommended to enter the passwords or sensitive information in plain text when executing the template. For this reason, there is a special type: **secureString**, which will mask the user input. First, define the following parameter in the parameters section.

	````JavaScript
	"administratorLoginPassword": {
		"type": "securestring"
	},
	````

8. Now replace the hardcoded password with the parameter usage expression, as shown in the following code.

    <!-- mark:8 -->
    ````JavaScript
	{
		"name": "[parameters('serverName')]",
		"type": "Microsoft.Sql/servers",
		"location": "[parameters('serverLocation')]",
		"apiVersion": "2.0",
		"properties": {
			"administratorLogin": "[parameters('administratorLogin')]",
			"administratorLoginPassword": "[parameters('administratorLoginPassword')]"
		}
	}
    ````

9. There are some parameters that may accept only a set of values. You can specify this as well as any default values when defining the parameter. Let's do this with the **sku** and **workerSize** values of the hosting plan resource. Add the following parameter definitions in the parameters section.

    ````JavaScript
	"sku": {
		"type": "string",
		"allowedValues": [
			"Free",
			"Shared",
			"Basic",
			"Standard"
		],
		"defaultValue": "Free"
	},
	"workerSize": {
		"type": "string",
		"allowedValues": [
			"0",
			"1",
			"2"
		],
		"defaultValue": "0"
	},
	````
    
	> **Note:** The **allowedValues** property is used to specify the values that are valid for the parameter, and the **defaultValue** property defines the value that will be used when the parameter is not specified. Specifying a default value means that the parameter is optional.
	
10. In the template, locate the hosting plan resource and replace the hardcoded **sku** and **workerSize** values with the parameter reference. This is shown in the following code.
    <!-- mark:8-9 -->
	````JavaScript
	{
		"apiVersion": "2014-04-01",
		"name": "[parameters('hostingPlanName')]",
		"type": "Microsoft.Web/serverFarms",
		"location": "[parameters('siteLocation')]",
		"properties": {
			"name": "[parameters('hostingPlanName')]",
			"sku": "[parameters('sku')]",
			"workerSize": "[parameters('workerSize')]",
			"numberOfWorkers": 1
		}
	},
	````

11. Save the template file and switch back to **PowerShell**.

12. Replace the placeholders and execute the following command to create or update your resource group using the custom template. Make sure to replace the _[STORAGE NAME]_ placeholder with the storage account that you created in the previous exercise.

	````PowerShell
	New-AzureResourceGroup -StorageAccountName [STORAGEACCOUNT] -TemplateFile [JSON-File-Path] -ResourceGroupName [RESOURCE-GROUP-NAME] -Location [LOCATION] -siteName [YOUR-SITE-NAME] -hostingPlanName [YOUR-HOSTING-PLAN-NAME] -siteLocation [YOUR-SITE-LOCATION] -serverName [YOUR-SQL-SERVER-NAME] -serverLocation [YOUR-SERVER-LOCATION] -administratorLogin [YOUR-ADMINISTRATOR-LOGIN]
	````

	![The PowerShell script is running](Images/the-powershell-script-is-running.png?raw=true "The PowerShell script is running")
	
	_The PowerShell script is running_
	
	> **Note:** If you press the _TAB_ key after specifying the Template file when writing the command in PowerShell, the parameters defined in it will be listed.

13. The **administratorLoginPassword** was not specified in the previous command, as this parameter will be prompted to the user.

	![The Administrator Login Password is prompted to the user and the input is masked](Images/the-administrator-login-password-is-prompted.png?raw=true "The Administrator Login Password is prompted to the user and the input is masked")
	
	_The Administrator Login Password is prompted to the user and the input is masked_

	> **Note:** If you use the same values as the ones used in the first task, the resources will not be created again. However, if you change any properties in the script, the affected resources will be updated.

<a name="Exercise2" />
### Exercise 2: Advanced Template Configuration  ###

In this exercise you will dig deeper into more options that the **ARM** template provides to define your resources. You will learn how to create nested resources, how to configure alerts based on your resource metrics, and even how to autoscale your resources based on these metrics.

<a name="Ex2Task1" />
#### Task 1 - Adding Nested Resources ####

Inside the Resource section, each resource can have a list of resources that are part of this service. The resource allows 5 levels of nested resources and a total amount of 100 resources.

In this task you will add child resources to the resources you created in the previous exercise. You will create a database in the SQL Server already generated and then a configuration in the WebSite to reference the database.

1. If not already open, open the custom JSON file you started in Exercise 1.

2. Locate the **SQL Server** resource in the **resources** section.

	````JavaScript
	{
		"name": "[parameters('serverName')]",
		"type": "Microsoft.Sql/servers",
		"location": "[parameters('serverLocation')]",
		"apiVersion": "2.0",
		"properties": {
			"administratorLogin": "[parameters('administratorLogin')]",
			"administratorLoginPassword": "[parameters('administratorLoginPassword')]"
		},
	}
	````

3. Add a new property to the resource called **resources**. The **resources** property is a list of resources inside the service.

	<!-- mark:10-13 -->
	````JavaScript
	{
		"name": "[parameters('serverName')]",
		"type": "Microsoft.Sql/servers",
		"location": "[parameters('serverLocation')]",
		"apiVersion": "2.0",
		"properties": {
			"administratorLogin": "[parameters('administratorLogin')]",
			"administratorLoginPassword": "[parameters('administratorLoginPassword')]"
		}
		,
		"resources": [
		]
	}
	````

4. Add a new resource in the **resources** property you have just defined to create a database inside the SQL Server. The nested resource should look like the following:

	<!-- mark:11-16 -->
	````JavaScript
	{
		"name": "[parameters('serverName')]",
		"type": "Microsoft.Sql/servers",
		"location": "[parameters('serverLocation')]",
		"apiVersion": "2.0",
		"properties": {
			"administratorLogin": "[parameters('administratorLogin')]",
			"administratorLoginPassword": "[parameters('administratorLoginPassword')]"
		},
		"resources": [
			{
				"name": "[concat(parameters('siteName'), '_db')]",
				"type": "databases",
				"location": "[parameters('serverLocation')]",
				"apiVersion": "2.0",
			}
		]
	},
	````

	>**Note**: The resource you have just added defines a new database in the same location as the server which is specified when executing the command. The name of the database is defined with the name of the site with the __db_ prefix.
	
5. Now you will add some configuration properties to the database to define the **edition** of the SQL Database: the **collation** and the **maximum size**.

	<!-- mark:16-20 -->
	````JavaScript
	{
		"name": "[parameters('serverName')]",
		"type": "Microsoft.Sql/servers",
		"location": "[parameters('serverLocation')]",
		"apiVersion": "2.0",
		"properties": {
			"administratorLogin": "[parameters('administratorLogin')]",
			"administratorLoginPassword": "[parameters('administratorLoginPassword')]"
		},
		"resources": [
			{
				"name": "[concat(parameters('siteName'), '_db')]",
				"type": "databases",
				"location": "[parameters('serverLocation')]",
				"apiVersion": "2.0",
				"properties": {
					"edition": "Web",
					"collation": "SQL_Latin1_General_CP1_CI_AS",
					"maxSizeBytes": "1073741824"
				}
			}
		]
	}
	````


	>**Note**: **Collations** define rules that sort and compare data. Before choosing a collation, carefully consider your application's needs when it comes to creating your database. Note that a collation cannot be changed after database creation. The default collation is **SQL_Latin1_General_CP1_CI_AS**: Latin dictionary, code page 1 (CP1), case-insensitive (CI), and accent-sensitive (AS).
	
	> Use the **Max Size** property to specify an upper limit for the database size. Insert and update transactions that exceed the upper limit will be rejected because the database will be in read-only mode. Changing the **Max Size** property of a database does not directly affect the charges incurred by the database. Charges are based on actual size.

	
6. To allow the Azure services, you need to add a firewall rule to allow Azure IPs 0.0.0.0. To do so, add the following highlighted resource to the SQL Server resource.

	<!-- mark:24-33 -->
	````JavaScript
	...
	"resources": [
		{
			"name": "[parameters('serverName')]",
			"type": "Microsoft.Sql/servers",
			"location": "[parameters('serverLocation')]",
			"apiVersion": "2.0",
			"properties": {
				"administratorLogin": "[parameters('administratorLogin')]",
				"administratorLoginPassword": "[parameters('administratorLoginPassword')]"
			},
			"resources": [
				{
					"name": "[concat(parameters('siteName'), '_db')]",
					"type": "databases",
					"location": "[parameters('serverLocation')]",
					"apiVersion": "2.0",
					"properties": {
						"edition": "Web",
						"collation": "[parameters('collation')]",
						"maxSizeBytes": "1073741824"
					}
				}
				,{
					"apiVersion": "2.0",
					"location": "[parameters('serverLocation')]",
					"name": "AllowAllWindowsAzureIps",
					"properties": {
						"endIpAddress": "0.0.0.0",
						"startIpAddress": "0.0.0.0"
					},
					"type": "firewallrules"
				}
			]
		},
	...
	````

7. Now, you will add a configuration resource to the website and a connection string to the database you have created. To do this, locate the website resource in the **resources** section. Add a new **resources** property inside the website, as shown in the following code.

	<!-- mark:10-12 -->
	````JavaScript
	{
		"apiVersion": "2014-04-01",
		"name": "[parameters('siteName')]",
		"type": "Microsoft.Web/Sites",
		"location": "[parameters('siteLocation')]",
		"properties": {
			"name": "[parameters('siteName')]",
			"serverFarm": "[parameters('hostingPlanName')]"
		}
		,
		"resources": [
		]
	}
	````

8. Add a new resource in the **resources** property you have just defined to create a config inside the Website. The resource should look like the following code.

	<!-- mark:11-15 -->
	````JavaScript
	{
		"apiVersion": "2014-04-01",
		"name": "[parameters('siteName')]",
		"type": "Microsoft.Web/Sites",
		"location": "[parameters('siteLocation')]",
		"properties": {
			"name": "[parameters('siteName')]",
			"serverFarm": "[parameters('hostingPlanName')]"
		},
		"resources": [
			{
				"apiVersion": "2014-04-01",
				"type": "config",
				"name": "web",
			}
		]
	}
	````
9. In the **config** resource, add a new property called **connectionstring** which will have the connection string to the database you have included in the previous step.

	<!-- mark:15-23 -->
	````JavaScript
	{
		"apiVersion": "2014-04-01",
		"name": "[parameters('siteName')]",
		"type": "Microsoft.Web/Sites",
		"location": "[parameters('siteLocation')]",
		"properties": {
			"name": "[parameters('siteName')]",
			"serverFarm": "[parameters('hostingPlanName')]"
		},
		"resources": [
			{
				"apiVersion": "2014-04-01",
				"type": "config",
				"name": "web",
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
		]
	}
	````

	>**Note**: Notice that the connection string value is the combination of various strings and properties, which include the **serverName**, **siteName**, **administratorLogin** and **administratorPassword** parameters.

	The **Template Engine** will read the template, evaluate the dependencies between resources and construct a graph which will determine the order of deployment. When there are not dependencies between resources, the orchestrator will try to deploy the resources in parallel. Dependencies can be found by looking at where one resource gets values from another resource via _Resource Expressions_. 

	Sometimes there are dependencies that are not obvious from these references. There is a property in the resource template where the user can explicitly declare a dependency. The property is called **dependsOn**.

10. Locate the resources inside the SQL Server resource and add the **dependsOn** property to explicitly declare a dependency.
	
	<!-- mark:16-18,28-30 -->
	````JavaScript
	{
		"name": "[parameters('serverName')]",
		"type": "Microsoft.Sql/servers",
		"location": "[parameters('serverLocation')]",
		"apiVersion": "2.0",
		"properties": {
			"administratorLogin": "[parameters('administratorLogin')]",
			"administratorLoginPassword": "[parameters('administratorLoginPassword')]"
		},
		"resources": [
			{
				"name": "[concat(parameters('siteName'), '_db')]",
				"type": "databases",
				"location": "[parameters('serverLocation')]",
				"apiVersion": "2.0",
				"dependsOn": [
					
				],
				"properties": {
					"edition": "Web",
					"collation": "[parameters('collation')]",
					"maxSizeBytes": "1073741824"
				}
			},{
				"apiVersion": "2.0",
				"location": "[parameters('serverLocation')]",
				"name": "AllowAllWindowsAzureIps",
				"dependsOn": [
					
				],
				"properties": {
					"endIpAddress": "0.0.0.0",
					"startIpAddress": "0.0.0.0"
				},
				"type": "firewallrules"
			}
		]
	}
	````

11. Add the following highlighted line to the **dependsOn** property to set the dependency on the SQL Server.

	<!-- mark:4 -->
	````JavaScript
	...
	"apiVersion": "2.0",
	"dependsOn": [
		"[concat('Microsoft.Sql/servers/', parameters('serverName'))]"
	],
	...
	````
	
12. Locate the config resource in the website and add the **dependsOn** property to explicitly declare the dependency of the Website on the configuration resource.

	<!-- mark:6-8 -->
	````JavaScript
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
	]
	````
	
13. Save the template file and switch back to **PowerShell**.

14. Replace the placeholders and execute the following command to create or update your resource group using the custom template. Make sure to replace the _[STORAGE NAME]_ placeholder with the storage account that you created in the previous exercise.

	````PowerShell
	New-AzureResourceGroup -StorageAccountName [STORAGEACCOUNT] -TemplateFile [JSON-File-Path] -ResourceGroupName [RESOURCE-GROUP-NAME] -Location [LOCATION] -siteName [YOUR-SITE-NAME] -hostingPlanName [YOUR-HOSTING-PLAN-NAME] -siteLocation [YOUR-SITE-LOCATION] -serverName [YOUR-SQL-SERVER-NAME] -serverLocation [YOUR-SERVER-LOCATION] -administratorLogin [YOUR-ADMINISTRATOR-LOGIN]
	````
	
	![Creating the database and configuring the website](Images/creating-the-database-and-configuring-the-web.png?raw=true "Creating the database and configuring the website")
	
	_Creating the database and configuring the website_
	
<a name="Ex2Task2" />
#### Task 2 - Configuring Alerts ####

In this task you will add a new **Alert** as a new resource in the JSON template. You can configure different types of alerts depending on the metric you want to be notified of. For example, in this task you will create a new alert that will send you an email when a threshold of 2000 Requests (or greater) is reached for your website.

2. Edit the JSON template file you created in the previous exercises.

3. Add the following resource at the end of the template.

	````JavaScript
	,{
		"apiVersion": "2014-04",
		"name": "[concat('Requests-', parameters('siteName'))]",
		"type": "microsoft.insights/alertrules",
		"location": "[parameters('siteLocation')]",                        
		"dependsOn": [
			"[concat('Microsoft.Web/sites/', parameters('siteName'))]"
		],
		"properties": {
			
		}
	}
	````

	As you can see, this section is using a parameter to set up a dynamic name for the rule. In this case it is appending the _siteName_ parameter to the "Requests-" text. The resulting alert name would be something similar to "Requests-MyAzureWebsite". The resource is of type **microsoft.insights/alertrules** and has a single dependency on the Website that you created in the first exercise.
	
3. This alert needs a rule to define the conditions to send a notification email to the Service owner and co-admins of the Azure account. You will define a new condition where the "Requests" metric must not exceed a value of 2000 in a time frame of 15 minutes. Paste the following highlighted code block inside the **properties** property.

	<!-- mark:10-22 -->
	````JavaScript
	{
		"apiVersion": "2014-04",
		"name": "[concat('Requests-', parameters('siteName'))]",
		"type": "microsoft.insights/alertrules",
		"location": "[parameters('siteLocation')]",                        
		"dependsOn": [
			"[concat('Microsoft.Web/sites/', parameters('siteName'))]"
		],
		"properties": {
			"name": "[concat('Requests-', parameters('siteName'))]",
			"description": "[concat(parameters('siteName'), ' requests threshold exceeded.')]",
			"isEnabled": true,
			"condition": {
				"odata.type": "Microsoft.WindowsAzure.Management.Monitoring.Alerts.Models.ThresholdRuleCondition",
				"dataSource": {
					"odata.type": "Microsoft.WindowsAzure.Management.Monitoring.Alerts.Models.RuleMetricDataSource",
					"resourceUri": "[concat(resourceGroup().id, '/providers/Microsoft.Web/sites/', parameters('siteName'))]",
					"metricName": "Requests"
				},
				"threshold": 2000.0,
				"windowSize": "PT15M"
			},
		}
	}
	````

	 To trigger a notification, the alert must meet the condition settings. Inside the **condition** property for this example, you are configuring the following values:
	 
	 - **odata.type**: This condition uses a **ThresholdRuleCondition** which requires a value to meet the condition.
	 - **dataSource**: Inside this property there are 3 more properties to configure. The **metricName** is the type of metric that the alert will be watching. In this case you are using **Requests** but you can choose a different one. For example, to listen for HTTP 500 error codes, you need to set the **metricName** value to **Http5xx**. Additionally, you need to set the **resourceUri** which will identify the Website that the alert is currently watching.
	 - **threshold**: This value is related to the **metricName** you specified. In this example, we are using a value of 2000 for the **Requests** metric.
	 - **windowSize**: This property indicates the time span to monitor the metric data specified in the **dataSource** property. In this code block, you are using a time frame of 15 minutes.

4. Now that you have the alert condition set, you need to set an **action**. You can configure the alert to send an email to the service administrator and co-admins, including additional emails if you want. To do this, add the following highlighted code block to the resource.

	<!-- mark:23-27 -->
	````JavaScript
	{
		"apiVersion": "2014-04",
		"name": "[concat('Requests-', parameters('siteName'))]",
		"type": "microsoft.insights/alertrules",
		"location": "[parameters('siteLocation')]",                        
		"dependsOn": [
			"[concat('Microsoft.Web/sites/', parameters('siteName'))]"
		],
		"properties": {
			"name": "[concat('Requests-', parameters('siteName'))]",
			"description": "[concat(parameters('siteName'), ' requests threshold exceeded.')]",
			"isEnabled": true,
			"condition": {
				"odata.type": "Microsoft.WindowsAzure.Management.Monitoring.Alerts.Models.ThresholdRuleCondition",
				"dataSource": {
					"odata.type": "Microsoft.WindowsAzure.Management.Monitoring.Alerts.Models.RuleMetricDataSource",
					"resourceUri": "[concat(resourceGroup().id, '/providers/Microsoft.Web/sites/', parameters('siteName'))]",
					"metricName": "Requests"
				},
				"threshold": 2000.0,
				"windowSize": "PT15M"
			},
			"action": {
				"odata.type": "Microsoft.WindowsAzure.Management.Monitoring.Alerts.Models.RuleEmailAction",
				"sendToServiceOwners": true,
				"customEmails": []
			}
		}
	}	
	````

	To send an email to the service owner and co-admins, set the **sendToServiceOwners** property to **true**. To add additional emails, write them inside the **customEmails** array.

5. Save the template and go back to **PowerShell**.

6. Run the **New-AzureResourceGroup** Cmdlet and wait until the Resource Group is updated. Once completed, open the Azure Preview portal.

	![Alert rule created](Images/alert-rule-created.png?raw=true "Alert rule created")
	
	_Alert rule created_

7. Click the **Browse** button in the **Hub Menu** and select **Resource Groups**. Select the resource group you created in the first exercise.

8. In the **Resource Map**, select the website.

9. In the Website blade, scroll down to the **Operations** part and select **Alert Rules**.

	![Alert rules in website blade](Images/alert-rules-in-website-blade.png?raw=true "Alert rules in website blade")
	
	_Alert rules in website blade_

10. You will see the alert you created in the list. Select the rule to display its properties. Check that the settings match those you specified in your template.
	
	![Alert created in the portal](Images/alert-created-in-the-portal.png?raw=true "Alert created in the portal")
	
	_Alert created in the portal_

<a name="Ex2Task3" />
#### Task 3 - Configuring Autoscaling Settings ####

In this task you will add an **Autoscaling setting** to your hosting plan. With this setting you can automatically define a rule to scale up your Website when the CPU metric is above 80% and a scale-down rule that will decrease the number of instances when the CPU goes below 60%.

> **Note:** In order to enable the autoscale setting, the pricing tier of the Hosting Plan must be set to **Standard**. When updating the Resource Group using PowerShell you need to set the **sku** parameter to _Standard_.

1. Add the following resource to the template.

	<!-- mark:1-14 -->
	````JavaScript
	,{
		"apiVersion": "2014-04",
		"name": "[concat(parameters('hostingPlanName'), '-', resourceGroup().name)]",
		"type": "microsoft.insights/autoscalesettings",
		"location": "[parameters('siteLocation')]",		
		"dependsOn": [
			"[concat('Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]"
		],
		"properties": {			
			"enabled": true,
			"name": "[concat(parameters('hostingPlanName'), '-', resourceGroup().name)]",
			"targetResourceUri": "[concat(resourceGroup().id, '/providers/Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]",
		}
	}
	````

	> **Note:** Notice the **dependsOn** property. The autoscale setting resource depends on the **Hosting Plan** configured for the server farm.
	
2. First you will add the **profiles** property, which establishes the minimum and maximum number of instances to perform the autoscaling. In this case, you will set a minimum of 2 instances and a maximum value of 4. The default number of instances that the website will start is 2.

	<!-- mark:13-22 -->
	````JavaScript
	{
		"apiVersion": "2014-04",
		"name": "[concat(parameters('hostingPlanName'), '-', resourceGroup().name)]",
		"type": "microsoft.insights/autoscalesettings",
		"location": "[parameters('siteLocation')]",		
		"dependsOn": [
			"[concat('Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]"
		],
		"properties": {			
			"enabled": true,
			"name": "[concat(parameters('hostingPlanName'), '-', resourceGroup().name)]",
			"targetResourceUri": "[concat(resourceGroup().id, '/providers/Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]",
			"profiles": [
				{
					"name": "Default",
					"capacity": {
						"minimum": "2",
						"maximum": "4",
						"default": "2"
					},
				}
			]
		}
	}	
	````

3. Now, add a scale-up rule that will increase the number of instances when the CPU threshold is greater than 80%. To do this, add the **rules** property inside **profiles**, as shown in the highlighted code.

	<!-- mark:21-39 -->
	````JavaScript
	{
		"apiVersion": "2014-04",
		"name": "[concat(parameters('hostingPlanName'), '-', resourceGroup().name)]",
		"type": "microsoft.insights/autoscalesettings",
		"location": "East US",		
		"dependsOn": [
			"[concat('Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]"
		],
		"properties": {			
			"enabled": true,
			"name": "[concat(parameters('hostingPlanName'), '-', resourceGroup().name)]",
			"targetResourceUri": "[concat(resourceGroup().id, '/providers/Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]",
			"profiles": [
				{
					"name": "Default",
					"capacity": {
						"minimum": "2",
						"maximum": "4",
						"default": "2"
					},
					"rules": [
						{
							"metricTrigger": {
								"metricName": "CpuPercentage",
								"metricResourceUri": "[concat(resourceGroup().id, '/providers/Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]",
								"timeGrain": "PT1M",
								"statistic": "Average",
								"timeWindow": "PT10M",
								"timeAggregation": "Average",
								"operator": "GreaterThan",
								"threshold": 80.0
							},
							"scaleAction": {
								"direction": "Increase",
								"type": "ChangeCount",
								"value": "1",
								"cooldown": "PT10M"
							}
						},
					]
				}
			]
		}
	}
	````
	
	Inside the **rules** property, you define the different scale-up and scale-down rules you need to autoscale your Website. Each rule is composed of 2 properties: **metricTrigger** and **scaleAction**. In a **metricTrigger** you are defining a _CpuPercentage_ metric with a threshold of _80_ and using the operator _GreaterThan_. The **timeWindow** property is set to 10 minutes, i.e. every 10 minutes the rule will verify that the CPU percentage average did not exceed the 80% threshold. If it does, it will execute the action specified in the **scaleAction** property.
	
	The **scaleAction** property is defined by its direction. As this is a _scale-up_ rule, the direction value is **Increase**. The action will increase a single instance each time it is invoked with a cooldown period of 10 minutes (it will not execute again before that period of time).
	
4. To add a scale-down rule, you need to add a new rule to the **rules** property, but in this case you will set the **direction** to **Decrease**. Additionally, define a rule that will execute only when the CPU average percentage is below 60%. This is highlighted in the following code.

	<!-- mark:40-57 -->
	````JavaScript
	{
		"apiVersion": "2014-04",
		"name": "[concat(parameters('hostingPlanName'), '-', resourceGroup().name)]",
		"type": "microsoft.insights/autoscalesettings",
		"location": "East US",		
		"dependsOn": [
			"[concat('Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]"
		],
		"properties": {			
			"enabled": true,
			"name": "[concat(parameters('hostingPlanName'), '-', resourceGroup().name)]",
			"targetResourceUri": "[concat(resourceGroup().id, '/providers/Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]",
			"profiles": [
				{
					"name": "Default",
					"capacity": {
						"minimum": "2",
						"maximum": "4",
						"default": "2"
					},
					"rules": [
						{
							"metricTrigger": {
								"metricName": "CpuPercentage",
								"metricResourceUri": "[concat(resourceGroup().id, '/providers/Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]",
								"timeGrain": "PT1M",
								"statistic": "Average",
								"timeWindow": "PT10M",
								"timeAggregation": "Average",
								"operator": "GreaterThan",
								"threshold": 80.0
							},
							"scaleAction": {
								"direction": "Increase",
								"type": "ChangeCount",
								"value": "1",
								"cooldown": "PT10M"
							}
						},
						{
							"metricTrigger": {
								"metricName": "CpuPercentage",
								"metricResourceUri": "[concat(resourceGroup().id, '/providers/Microsoft.Web/serverfarms/', parameters('hostingPlanName'))]",
								"timeGrain": "PT1M",
								"statistic": "Average",
								"timeWindow": "PT1H",
								"timeAggregation": "Average",
								"operator": "LessThan",
								"threshold": 60.0
							},
							"scaleAction": {
								"direction": "Decrease",
								"type": "ChangeCount",
								"value": "1",
								"cooldown": "PT1H"
							}
						}
					]
				}
			]
		}
	}	
	````

	> **Note:** Another difference when comparing with the scale-up rule is that the time period to scale down an instance is 1 hour.

5. Save the template and go back to **PowerShell**.

6. Run the **New-AzureResourceGroup** Cmdlet. Set the **sku** value to **Standard**. Once completed, open the Azure Preview portal.

	![Autoscaling rule created](Images/autoscaling-rule-created.png?raw=true "Autoscaling rule created")
	
	_Autoscaling rule created_

7. Click the **Browse** button in the **Hub Menu** and select **Resource Groups**. Select the resource group you created in the first exercise.

8. In the **Resource Map**, select the website.

9. In the Website blade, scroll down to the **Usage** part and select **Scale**. You will see the autoscale setting you specified in the template.

	![Scale part in website blade](Images/scale-part-in-website-blade.png?raw=true "Scale part in website blade")
	
	_Scale part in website blade_

10. Notice that the **Instance Range** is between _2_ and _4_.

	![Scale blade](Images/scale-blade.png?raw=true "Scale blade")

	_Scale blade_
	
<a name="Ex2Task4" />
#### Task 4 - Configuring MSDeploy ####

In this task you will learn how to deploy a Website as part of the ARM template. 

1. In Azure PowerShell, navigate to the **Source/Assets** folder of the lab

2. Execute the following command to change from **Azure Resource Manager** to **Azure Service Management** mode.

	````PowerShell
	Switch-AzureMode AzureServiceManagement
	````

3. Replace the _[STORAGE-ACCOUNT-NAME]_ placeholder with the Account you created in the previous exercise and execute the following script to create a packages container in your storage account and update the zip file named **mywebsite** to the storage account.
	
	````PowerShell
	$storageAccountName = "[STORAGE-ACOUNT-NAME]"
	$fqName = ".\mywebsite.zip"
	$fileName = "mydeployedwebsite.zip"
	$ContainerName = "package"
	$storageAccountKey = Get-AzureStorageKey $storageAccountName | %{ $_.Primary }
	$context = New-AzureStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $storageAccountKey
	New-AzureStorageContainer $ContainerName -Permission Container -Context $context
	Set-AzureStorageBlobContent -Blob $fileName  -Container $ContainerName -File $fqName -Context $context -Force
	````
	
	![Uploaded zip file to blob storage](Images/uploaded-zip-file-to-blob-storage.png?raw=true "Uploaded zip file to blob storage")
	
	_Uploaded zip file to blob storage_
	
4. Once the zip file is uploaded, replace the _[STORAGE-FILE-URL]_ placeholder with the file's URL in Blob storage in the following snippet and add the **MSDeploy** extension to the website resources after the **config** resource.

	<!-- mark:1-16 -->
	````JavaScript
	,{
		"apiVersion": "01-01-2014",
		"name": "MSDeploy",
		"type": "Extensions",
		"dependsOn": [
			"[concat('Microsoft.Web/Sites/', parameters('siteName'))]",
			"[concat('Microsoft.Sql/servers/', parameters('serverName'), '/databases/', parameters('siteName'), '_db')]"
		],
		"properties": {
				"packageUri": "[STORAGE-FILE-URL]",
				"setParameters": {
					"Application Path": "[parameters('siteName')]",
					"Connection String": "[concat('Data Source=tcp:', reference(concat('Microsoft.Sql/servers/', parameters('serverName'))).fullyQualifiedDomainName, ',1433;Initial Catalog=', parameters('siteName'), '_db', ';User Id=', parameters('administratorLogin'), '@', parameters('serverName'), ';Password=', parameters('administratorLoginPassword'), ';')]"
				}
			}
	}
	````
	
	>**Note** The storage URL should look like the following. HTTP://[ACCOUNT-NAME].blob.core.windows.net/[CONTAINER]/[FILENAME]. If you left the variables as shown in the script below, _[CONTAINER]_ should be **package** and _[FILENAME]_ should be **mydeployedwebsite.zip**.

5. Save the template and go back to **PowerShell**.

6. Run the **New-AzureResourceGroup** Cmdlet and wait until the Resource Group is updated. Once completed, open Internet Explorer.

	![New-AzureResourceGroup command with MSDeploy task added](Images/new-azureresourcegroup-command-with-msdeploy.png?raw=true "New-AzureResourceGroup command with MSDeploy task added")
	
	_New-AzureResourceGroup command with MSDeploy task added_

7. Navigate to **HTTP://[YOUR-SITE-NAME].azurewebsites.net** where _[YOUR-SITE-NAME]_ is the same name you set in the command.

	![HTTP://[YOUR-SITE].azurewebsites.net](Images/httpyour-siteazurewebsitesnet.png?raw=true "HTTP://[YOUR-SITE].azurewebsites.net")
	
	_HTTP://[YOUR-SITE-NAME].azurewebsites.net_


<a name="Exercise3" />
### Exercise 3: Cleaning up Resources using ARM ###

In this exercise you will learn how to delete resources and resource groups by deleting the resource group that was created in the previous exercises.

<a name="Ex3Task1" />
#### Task 1 - Removing Resources and Resource Groups  ####

In this task you will learn about deleting resources and deleting resource groups using Azure PowerShell.

1. Open Azure Powershell if it is not already open.

2. Ensure that your Powershell session is in _AzureResourceManager_ mode. If not, execute the following command to enter in this mode.

	````PowerShell
	Switch-AzureMode AzureResourceManager
	````

3. Use the following command to list all the available resource groups. 

	````PowerShell
	Get-AzureResourceGroup
	```` 

	![Listing the Resource Groups of your suscription](Images/listing-the-resource-groups-of-your-suscripti.png?raw=true)
	
	_Listing the Resource Groups of your subscription_
	
4. From the list of resource groups in your subscription, choose the one created in the previous exercises and execute the following command that will list all the resources of that resource group. Without parameters, **Get-AzureResource** gets all resources in your Azure subscription

	````PowerShell
	Get-AzureResource -ResourceGroupName [YOUR-AZURE-RESOURCE-GROUP-NAME]
	```` 
	
	![Listing the resources of a Resource Group](Images/listing-the-resources-of-a-resource-group.png?raw=true)
	
	_Listing the resources of a Resource Group_

5. Choose one resource from the list of resources that will be deleted, for example, the Website. To get more information about the chosen resource, specify its name. When you use the **Name** parameter to get a particular resource, the **ResourceGroupName**, **ResourceType**, and **APIVersion** parameters are required.

	````PowerShell
	Get-AzureResource -Name [YOUR-AZURE-RESOURCE-NAME] -ResourceGroupName [YOUR-AZURE-RESOURCE-GROUP-NAME] -ResourceType "Microsoft.Web/sites" -ApiVersion 2014-04-01
	```` 
	
	![Seeing the details of the Website resource](Images/seeing-the-details-of-the-website-resource.png?raw=true)
	
	_Seeing the details of the Website resource_
	
6. Delete the selected resource. To delete a resource from the resource group, use the **Remove-AzureResource** cmdlet. This cmdlet deletes the resource but does not delete the resource group.

	````PowerShell
	Remove-AzureResource -Name [YOUR-AZURE-RESOURCE-NAME] -ResourceGroupName [YOUR-AZURE-RESOURCE-GROUP-NAME] -ResourceType Microsoft.web/sites -ApiVersion 2014-04-01
	```` 
	
	> **Note:** By default, **Remove-AzureResource** prompts you for confirmation. To suppress the prompt, use the **Force** parameter.

 
7. Type **'Y'** and press **Enter** to confirm the deletion.

8. List all the resources from your resource group to verify that the specified resource was deleted by issuing the following command.

	````PowerShell
	Get-AzureResource -ResourceGroupName [YOUR-AZURE-RESOURCE-GROUP-NAME]
	```` 
	
	![The Website resource is deleted](Images/the-website-resource-is-deleted.png?raw=true)
	
	_The Website resource is deleted_
	
	> **Note:** Notice that the Hosting Plan resource was also deleted as it was dependent on the Website.


9. The **Remove-AzureResourceGroup** cmdlet deletes a resource group and its resources from your subscription. By default, **Remove-AzureResourceGroup** prompts you for confirmation. To suppress the prompt, use the **Force** parameter. Delete the resource group by using the following command.

	````PowerShell
	Remove-AzureResourceGroup -ResourceGroupName [YOUR-AZURE-RESOURCE-GROUP-NAME]
	```` 
	
10. Type **'Y'** and press **Enter** to confirm the deletion.

	![Removing the Resource Group](Images/removing-the-resource-group.png?raw=true)
	
	_Removing the Resource Group_

11. You can verify that the resource group was successfully deleted by using the following command. Notice that the deleted resource group is no longer in your subscription.

	````PowerShell
	Get-AzureResourceGroup
	```` 
	
	![The Resource Group was deleted](Images/the-resource-group-was-deleted.png?raw=true)
	
	_The Resource Group was deleted_
	
---

<a name="Summary" />
## Summary ##

In this Hands-on Lab you learned about **Azure Resource Manager** (ARM) and the ARM template language to define resources and their dependencies. You learned how to parameterize the template, add dependencies, configure alerts, and autoscaling settings to your resources.

Additionally, you learned how to remove specific resources from a **Resource Group** and how to delete an entire **Resource Group** using the Azure PowerShell Cmdlets.