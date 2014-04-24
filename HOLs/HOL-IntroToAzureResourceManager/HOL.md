<a name="HOLTop" />
# Introduction to Azure Resource Manager #

---

<a name="Overview" />
## Overview ##

A growing expectation of any cloud offering is the ability to automate the deployment and management of infrastructure components, like virtual machines, networks, storage, and databases - and enable customers to build and manage higher level applications on top of these in a rich DevOps friendly way. The Azure Resource Manager, provides a **Language** (Cloud Service Template Language) that allows a declarative, parameterized description (a **Template**) of a set of related resources, so that they may be deployed and managed as a unit. The Templates are text-based (JSON), making it easy to use them with source code control systems like TFS and Git.

<a name="Objectives" />
### Objectives ###
In this hands-on lab, you will learn how to:

- Create a new Resource Group using Azure Resource Manager
- Use every section of the Top-level Template Structure
- Create more advanced templates
- Configure Firewall Rules, Alerts, and Autoscaling

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

1. [Getting familiar with the Cloud Service Manager Top-level Template Structure](#Exercise1)
1. [Advanced Template Configuration](#Exercise2)
1. [Firewall Rules, Alerts and Autoscale Settings](#Exercise3)

Estimated time to complete this lab: **XX minutes**

>**Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Each predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in the steps that you should take into account.

<a name="Exercise1" />
### Exercise 1: Getting familiar with the Cloud Service Manager Top-level Template Structure ###

In this exercise you will learn about the CSM Top-level Template structure, you will explore the different sections of it, learning about its usage, and how to construct them. 

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

In the following tasks you will examine the most important sections of this template structure. These are Resources, Parameters, Tags, and Variables. 

<a name="Ex1Task1" />
#### Task 1 – Understanding the Resources Section ####

In this task, you will learn about the Resource section, which is ued for defining all the resources that will be in your resource group.

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

> The _name_ property identifies a resource and it is required.

> The _type_ property defines the type of the resource in the form _"\<ResourceProviderNamespace\>/\<ResourceTypeName\>"_. This type will be used to lookup the Resource Provider that the Orchestrator will call to create or update the resources. This property is required.

> The _location_ property specifies the georegion, datacenter, or private cluster where the resource will be deployed. If not specified, the default value is the Location of the resourceGroup

> The _dependsOn_ property specify that the processing of a specific resource depends on another resource.

> The _properties_ property is a bag containing name/value pairs. Every resource can have a section with properties. These are the settings that describe or configure the resource. 

1. Open your preferred text editor and paste the following Top-level Template Structure.

    ````JavaScript
    {
      "$schema": "http://schema.management.azure.com/schemas/2014-04-01-preview/deploymentTemplate.json#",
      "contentVersion" : "1.0",
      "parameters": { 
      },
      "variables": {
      },
      "resources": 
      [
      ],
    }
    ````

2. You will create your custom template by adding  a website resource. To do this, locate the **resources** section, and below the comment add the following code. This code will add a very simple website to your resource group. Before adding the code, replace every _\<Your-Site-Name\>_ tag with a name of your choice, for example: _MyTestWebSite_.

	> **Note:** Take into account that Azure Websites names must be unique. Therefore you must choose a name that has not been taken yet. To avoid duplication you can append a random number to the end or your desired name.
	
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
	
3. In the **location** property of the resource, replace the _\<Your-Location\>_ value with _East Us_ or another Datacenter location.

	> Take note of the location you will use here, as it is recommended, but not mandatory, that all the other resources of the resource group are hosted in the same location.

4. Choose a name for your hosting plan name, and replace the  _\<Your-Hosting-Plan-Name\>_ tag with the choosen name in the code created in the previous step.

5. The following code will create a **Hosting Plan** that will be used in the creation of the website. Replace the _\<Your-Hosting-Plan-Name\>_ tag with the name choose in the previous step and paste this code in the resource sections of your template. Preferably, paste it before the Website resource.
	
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

6. As you may have noticed, the Hosting Plan resource needs to be created before the creation of the Website. This means that the Website depends on the Hosting Plan. Add the **dependsOn** property in the Website resource to indicate this dependency. The property is highlighted in the following code.

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

7. Add a new resource to the list, this time a SQL Server. To do this, add the following code in the resource section.

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

8. Replace the _\<Your-Server-Name\>_ placeholder with a name for your SQL Server.

	> **Note:** Keep in mind that the SQL Server name must be all lowercase, you can use numbers, and the hypen symbol.

9. Replace the _\<Your-Location\>_ placehodlder with the location you used in the previous resources. Also, replace the _\<Admin-User\>_ and _\<Admin-Password\>_ placeholders with your preferred credentials for this server.
	
10. Save the template file locally with a **.json** extension. For example, _myTemplate.json_.

11. Open Azure PowerShell.

12. Replace the _[STORAGE NAME]_ placeholder and execute the following command to create a new storage account. Make sure that the storage name you selected is unique.

	````PowerShell
	New-AzureStorageAccount -StorageAccountName [STORAGE NAME] -Location "East US"
	````

13. Switch mode to **AzureResourceManager** using the following command.

	````PowerShell
	SwitchMode AzureResourceManager
	````

14. Replace the placeholders and execute the following command to create your new resource group using the custom template. Make sure to replace the _[STORAGE NAME]_ placeholder with the storage account you have created in the previous step.

	````PowerShell
	New-AzureResourceGroup -Location [LOCATION] -Name [RESOURCE-GROUP-NAME] -TemplateFile [JSON-File-Path]  –StorageAccountName [STORAGEACCOUNT] -Verbose
	````

	![New-AzureResourceGroup command](Images/new-azureresourcegroup-command.png?raw=true "New-AzureResourceGroup command")
	
	_New-AzureResourceGroup command_
	
15. Open Internet Explorer and browse to the [Azure Portal](http://azure.portal.com)

16. Click the **Browse** button from the Hub Menu on the left side of the window.

17. In the **Browse** menu, click **Resource groups**.

18. Notice that in the Resource groups pane, there is a list of resources. Check that your resource group was created. Navigate to the Resource Group and check that there is the website with the names you defined in the template.

	![Resource Group in the azure portal](Images/resource-group-in-the-azure-portal.png?raw=true "Resource Group in the azure portal")
	
	_Resource Group in the azure portal_
	

<a name="Ex1Task2" />
#### Task 2 – Understanding the Parameters Section ####

In this task, you will learn how to use parameters in your templates. Using parameters makes changing some repetitive values easier. It also allows you to create templates that contains values that should be prompted to the user, for example, resource names or even credentials that can be masked when entered.

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

Then to use a defined resource in the template, you specify it in the following way.

````JavaScript
[parameters("Parameter-Name")]
````

1. Open the template that you created in the previous exercise.

2. Let's parametrize the name of the resources. Start with the Website name. To do this, in the parameters section of the template, define the following parameter.

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
    
    > **Note:** You should make two replacements.
    
    > **Note:** As this parameter is of string type, it is recommended that you use simple quotes when specifying the parameter name to escape the double quotes that define the whole string.
    
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

5. Replace all the hardcoded values for the paramerters defined in the previous step with calls to the parameter values. Your resulting template should look like the following code, in which the replacements are highlighted.

    <!-- mark:5,7,9,19,25,29,31,34 -->
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
    
6. Look at the definition of the Website resource, and locate the **dependsOn** property. Notice that the value uses a predefined part (_Microsoft.Web/serverFarms/_) and then the value that you chose for the hosting plan name. For these cases, you can use the **concat** operator: [concat('value1', 'value2')]. Use this operator to append the hosting plan name parameter to the fixed path, as shown in the following code.

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
    
7. You may have notice that altough the administrator login password is a string, we did not create a parameter for it, as it is not recommended to enter the passwords or sensitive information in plain text when executing the template. For this reason, there is a special type: **secureString**, which will mask the user input. First, define the following parameter in the parameters section.

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
    ````

9. There are some parameters that may accept only a set of values. You can specify this, and also if there is a default value when defining the parameter. Let's do this with the **sku** and **workerSize** values of the hosting plan resource. Add te following parameter definition in the parameters section.

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
    
	> **Note:** The **allowedValues** property is used to specify the values that are valid for the parameter, and the **defaultValue** property defines the value that will be used when the parameter is not specified. Specifying a default value, means that the parameter is optional.
	
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

11. Save the template file.

12. Open Azure PowerShell.

13. Switch mode to **AzureResourceManager**, if you are not already in this mode, using the following command.

	````PowerShell
	SwitchMode AzureResourceManager
	````

14. Replace the placeholders and execute the following command to create or update your resource group using the custom template. Make sure to replace the _[STORAGE NAME]_ placeholder with the storage account that you created in the previous exercise. Notice that we forced the sku parameter to be _Standard_.

	````PowerShell
	New-AzureResourceGroup -StorageAccountName [STORAGEACCOUNT] -TemplateFile [JSON-File-Path] -ResourceGroupName [RESOURCE-GROUP-NAME] -Location [LOCATION] -siteName [YOUR-SITE-NAME] -hostingPlanName [YOUR-HOSTING-PLAN-NAME] -siteLocation [YOUR-SITE-LOCATION] -serverName [YOUR-SQL-SERVER-NAME] -serverLocation [YOUR-SERVER-LOCATION] -administratorLogin [YOUR-ADMINISTRATOR-LOGIN] -sku Standard
	````

	<IMAGEN>
	_The PowerShell script is running_
	
	> **Note:** When writing the command in PowerShell if you press the _TAB_ key after specifying the Template file, the parameters defined in it will be listed.

	> **Note:** Notice that the **sku** property was changed to use the _Standard_ value. This is required for the following exercises and may incur in costs in your subscriptions.

15. The **administratorLoginPassword** was not specified in the previous command, as this parameter will be prompted to the user.

	<IMAGEN>
	_The Administrator Login Password is prompted to the user and the input is masked_

	> **Note:** If you use the same values as the ones used in the first task, the resources will not be created again. However if you change some properties in the script, the affected resources will be updated. For example, when we run the template in previous steps we updated the **sku** property, in this case if everything but this property remains the same, only that resource will be updated.

<a name="Exercise2" />
### Exercise 2 : Advanced Template Configuration  ###

In this exercise you will ...

<a name="Ex2Task1" />
#### Task 1 - Nested Resources  ####

Inside the Resource section, each resource can have a list of resources that are part of this service. The resource allows 5 levels deep of resources an a total ammount of 100 resources.

In this task you will add child resources to the resources you have created in the previous exercise. You will create a database in the SQL Server you have created and then you will create a configuration in the WebSite to reference the database.

1. If not already opened, open the custom JSON you started creating in Exercise 1. 

1. Locate the SQL Server resource in the **resources** section.

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

1. Add a new property to the resource called **resources**. The resources properties is a list of resources inside the service.

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

1. Add a new resource in the resources property you have just defined to create a database inside the SQL Server. The resource should look like below

	<!-- mark:12-15 -->
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

	>**Note**: The resoruce you have just added defines a new database in the same location as the server which is specified when executing the command. The name of the databse is defined with the name of the site with the __db_ prefix.

1. To allow the Azure services, you need to add a firewall rule to allow Azure IPs 0.0.0.0. To do so, add the following highlighted resource to the SQL Server resource.

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


	
1. Now you will add some configuration properties to the database to define the **edition** of the SQL Database, the collation and the maximum size.

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



	>**Note**: **Collations** define rules that sort and compare data. Make a thoughtful choice of collation, based on application needs, at the time you create your database. Note that a collation cannot be changed after database creation. The default collation is **SQL_Latin1_General_CP1_CI_AS**: Latin dictionary, code page 1 (CP1), case-insensitive (CI), and accent-sensitive (AS).
	
	> Use the **Max Size** property to specify an upper limit for the database size. Insert and update transactions that exceed the upper limit will be rejected because the database will be in read-only mode. Changing the **Max Size** property of a database does not directly affect the charges incurred for the database. Charges are based on actual size.

1. Now, you will add a configuration resource in the website and add a connection string to the database you have created.

1. Locate the website resource in the **resoruces** section. Add a new **resources** property inside the website.

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

1. Add a new resource in the **resources** property you have just defined to create a config inside the Website. The resource should look like below.

	<!-- mark:12-14 -->
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
1. In the **config** resource add a new property to called **connectionstring** which will have the connection string to the database you have included in the previous step.

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

	>**Note**: Notice that the connection string value is the combination of various strings and properties, which include the **serverName**, **siteName**, **administratorLogin** and **administratorPassowrd** parameters.

1. Save the file and switch back to Azure PowerShell

1. TODO: Verification (check if we 

<a name="Ex2Task2" />
#### Task 2 - Adding Dependencies  ####

The Template Engine will read the template, evaluate the dependencies between resources and construct a graph that it will use determine the order of deployment. When there are not dependencies between resources, the orchestrator will try to deploy the resources in parallel. Dependencies can be found by looking at where one resource gets values from another resource via Resource Expressions. 

Sometimes there are dependencies that are not obvious from these references. There's a property in the resource template were the user can explicitly declare a dependency. The property is called **dependsOn**.

In this task you will learn how to set depenednency between resources by setting the **dependsOn** property in a resource.

1. Locate the database resource inside the SQL Server resource.

1. Add a new property named **depenedsOn** to explicitly declare a dependency from the database to the SQL Server.
	
	<!-- mark:16-18 -->
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
			}
		]
	}
	````

1. Add the following highlighted line to the **dependsOn** property to set the dependency to the SQL Server.


	<!-- mark:4 -->
	````JavaScript
	...
	"apiVersion": "2.0",
	"dependsOn": [
		"[concat('Microsoft.Sql/servers/', parameters('serverName'))]"
	],
	...
	````
	
1. Locate the config resource in the website

1. Add a new property named **depenedsOn** to explicitly declare a dependency from the config resource to the webiste.

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
	
1. TODO Verification	
	
<a name="Exercise3" />
### Exercise 3 : Firewall Rules, Alerts and Autoscale Settings ###

INTRO..........

<a name="Ex3Task1" />
#### Task 1 - Configuring Firewall Rules ####

In this task you will ...

1. Open Azure PowerShell console.

<a name="Ex3Task2" />
#### Task 2 - Enabling Autoscaling ####

In this task you will ...

1. Open Azure PowerShell console.

<a name="Ex3Task3" />
#### Task 3 - Setting up Alerts ####

In this task you will ...

1. Open Azure PowerShell console.

---

<a name="Summary" />
## Summary ##

Summary
