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
    "$schema": "<http://schemas.microsoft.org/azure/deploymentTemplate?api-version=2014-01-01>",
    "location" : "<location>",
    "contentVersion" : "1.0",
    "parameters": { 
      // name/value pairs representing the template inputs
    },
    "variables": {
      // arbitrary JSON data used for constants and metadata
      // a JSON dictionary
      // Can be complex values like JSON arrays and objects
    },
    "tags":
    {
      // JSON name/value pairs that will be attached as runtime metadata
      // to the resourcegroup
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

1. Open your preferred text editor and copy & paste the Top-level Template Structure located in the Introduction of this exercise.

2. In the **location** property, replace the _\<location\>_ value with _EastUs_.

3. You will create your custom template by adding resources websites with SQL Database template to use the site name parameter with the __db_ prefix as the database name. Remove the _databaseName_ parameter as it is no longer required.

4. Locate the **resources** section, and below the comment add the following code. This code will add a very simple website to your resource group. Before adding the code, replace every _\<YourWebSite\>_ tag with a name of your choice, for example: _MyTestWebSite_.

	> **Note:** Take into account that Azure Websites names must be unique. Therefore you must choose a name that has not been taken yet. To avoid duplication you can append a random number to the end or your desired name.

	````JavaScript
	...
	"resources": [
      	{
      	"apiVersion": "2014-04-01",
      	"name": "<YourSiteName>",
      	"type": "Microsoft.Web/Sites",
      	"location": "EastUs",
      	"properties": {
        	"name": "<YourSiteName>",
        	"serverFarm": "<YourHostingPlanName>"
      	},
	],
	...
	````
5. Choose a name for your hosting plan name, and replace the  _\<YourHostingPlanName\>_ tag with the choosen name in the code created in the previous step.

6. The following code will create a **Hosting Plan** that will be used in the creation of the website. Replace the _\<YourHostingPlanName\>_ tag with the name choose in the previous step and paste this code in the resource sections of your template.
	
	````JavaScript
    	{
      	"apiVersion": "2014-04-01",
      	"name": "<YourHostingPlanName>",
      	"type": "Microsoft.Web/serverFarms",
      	"location": "EastUs",
      	"properties": {
        	"name": "<YourHostingPlanName>",
        	"sku": "Free",
        	"workerSize": "0",
        	"numberOfWorkers": 1
      	}
	````

7. As you may have noticed, the Hosting Plan resource needs to be created before the creation of the Website. This means that the Website depends on the Hosting Plan. Add the **dependsOn** property in the Website resource to indicate this dependency. The property is highlighted in the following code.

	<!-- mark:6-8 -->
	````JavaScript
    {
        "apiVersion": "2014-04-01",
        "name": "<YourSiteName>",
        "type": "Microsoft.Web/Sites",
        "location": "EastUs",
        "dependsOn": [
            "Microsoft.Web/serverFarms/<YourHostingPlanName>"
        ],
        "properties": {
        "name": "<YourSiteName>",
        "serverFarm": "<YourHostingPlanName>"
     	},
	````
	
8. Save the template file locally with a **.json** extension. For example, _myTemplate.json_

9. Open Azure PowerShell.

10. Replace the _[STORAGE NAME]_ placeholder and execute the following command to create a new storage account. Make sure that the storage name you selected is unique.

	````PowerShell
	New-AzureStorageAcount -StorageAccountName [STORAGE NAME] -Location "East US"
	````

11. Switch mode to **AzureResourceManager** using the following command.

	````PowerShell
	SwitchMode AzureResourceManager
	````

12. Replace the placeholders and execute the following command to create your new resource group using the custom template. Make sure to replace the _[STORAGE NAME]_ placeholder with the storage account you have created in the previous step.

	````PowerShell
	New-AzureResourceGroup -Location [LOCATION] -Name [RESOURCE-GROUP-NAME] -TemplateFile [JSON-File-Path]  –StorageAccountName [STORAGEACCOUNT] -Verbose

13. Open Internet Explorer and browse to the [Azure Portal](http://azure.portal.com)

14. Click the **Browse** button from the Hub Menu on the left side of the window.

15. In the **Browse** menu, click on **Resource groups**.

16. Notice that in the Resource groups pane, there is a list of resources. Check that your resource group was created.

17. Navigate to the Resource Group and check that there is the website with the names you defined in the template.

<a name="Ex1Task2" />
#### Task 2 – Understanding the Parameters Section ####

In this task, you will .....

1. ....

<a name="Ex1Task3" />
#### Task 3 – Understanding the Tags Section ####

In this task, you will .....

1. ....

<a name="Ex1Task4" />
#### Task 4 – Understanding the Variables Section ####

In this task, you will .....

1. ....

<a name="Exercise2" />
### Exercise 2 : Advanced Template Configuration  ###

In this exercise you will ...

<a name="Ex2Task1" />
#### Task 1 - Nested Resources  ####

Inside the Resource section, each resource can have a list of resources that are part of this service. The resource allows 5 levels deep of resources an a total ammount of 100 resources.

In this task you will add child resources to the resources you have created in the previous exercise. You will create a database in the SQL Server you have created and then you will create a configuration in the WebSite to reference the database.

1. If not already opened, open the custom JSON you started creating in Exercise 1. 

1. Locate the SQL Server resource in the **resources** section.

	````JSON
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
	````JSON
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
	````JSON
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
	
1. Now you will add some configuration properties to the database to define the **edition** of the SQL Database, the collation and the maximum size.

	<!-- mark:16-20 -->
	````JSON
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
	````JSON
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

1. Add a new resource in the **resources** property you have just defined to create a config inside the Website. The resource should look like below
	<!-- mark:12-14 -->
	````JSON
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
	````JSON
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
	````JSON
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

1. 


	<!-- mark:4 -->
	````JSON
	...
	"apiVersion": "2.0",
	"dependsOn": [
		"[concat('Microsoft.Sql/servers/', parameters('serverName'))]"
	],
	...
	
	````
	
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
