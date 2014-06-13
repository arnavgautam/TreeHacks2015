<a name="HOLTop" />
# Deploying Cloud Services in Microsoft Azure #

---
<a name="Overview" />
## Overview ##

In this hands-on lab, you will learn how to deploy your first application in Microsoft Azure. The lab walks through the process using myTODO, a simple list creation and management application built using ASP.NET MVC. The lab shows the steps required for provisioning the required components in the Microsoft Azure Management Portal, uploading the service package, and configuring the cloud service. You will see how you can test your application in a staging environment and then promote it to production once you are satisfied that it is operating according to your expectations.

![The myTODO application running in Microsoft Azure](Images/mytodo.png?raw=true)

_The myTODO application running in Microsoft Azure_

In the course of the lab, you will also examine how to deploy, upgrade, and configure Microsoft Azure applications programmatically using the Service Management API. You will use the Microsoft Azure Service Management Tools, which wraps the management API, to execute Windows PowerShell scripts that perform these operations. To complete the examination of deployment choices, you will use the Microsoft Azure Tools to deploy the application directly from Visual Studio.

During the lab, you will also learn how to provide an SSL connection to your Microsoft Azure service.

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

- Use the Microsoft Azure Management Portal to create a storage account and a cloud service
- Deploy service component packages using the Microsoft Azure Management Portal user interface
- Change configuration settings for a deployed application
- Test deployments in a separate staging environment before deployment to final production
- Use Windows PowerShell to deploy, upgrade, and configure Microsoft Azure services programmatically
- Use the Microsoft Azure Tools for publishing from Visual Studio
- Secure your Microsoft Azure application with SSL

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Visual Studio Express 2013 for Web][1] or greater

- [Microsoft Azure Tools for Microsoft Visual Studio 2.2][2] or later

- [Microsoft Azure PowerShell Cmdlets](http://msdn.microsoft.com/en-us/library/windowsazure/jj156055)

- A Microsoft Azure subscription
	- Sign up for a [Free Trial](http://aka.ms/watk-freetrial)
	- If you are a Visual Studio Professional, Test Professional, Premium or Ultimate with MSDN or MSDN Platforms subscriber, activate your [MSDN benefit](http://aka.ms/watk-msdn) now to start developing and testing on Microsoft Azure.
	- [BizSpark](http://aka.ms/watk-bizspark) members automatically receive the Microsoft Azure benefit through their Visual Studio Ultimate with MSDN subscriptions.
	- Members of the [Microsoft Partner Network](http://aka.ms/watk-mpn) Cloud Essentials program receive monthly Microsoft Azure credits at no charge.

[1]: http://www.microsoft.com/visualstudio/
[1]: http://www.visualstudio.com/downloads/download-visual-studio-vs
[2]: http://www.microsoft.com/windowsazure/sdk/

> **Note:** This lab was designed for Windows 8.1.

> When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

---

<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Deploying an Application Using the Microsoft Azure Management Portal](#Exercise1)
1. [Using PowerShell to Manage Microsoft Azure Applications](#Exercise2)
1. [Using Visual Studio to Publish Applications](#Exercise3)
1. [Securing Microsoft Azure with SSL](#Exercise4)

Estimated time to complete this lab: **90 minutes**.

> **Note:** Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise. Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.

---

<a name="Exercise1" />
### Exercise 1: Deploying an application using the Microsoft Azure Management Portal ###

In this exercise, you deploy the myTODO application to Microsoft Azure using the Microsoft Azure Management Portal. To do this, you will provision the required service components at the management portal, upload the application package to the staging environment and configure it. You will then execute the application in this test environment to verify its operation. Once you are satisfied that it operates according to your expectations, you will promote the application to production.

<a name="Ex1Task1" />
#### Task 1 - Creating a Storage Account, a Cloud Service and a SQL Database ####

The application you deploy in this exercise requires a Cloud Service, a SQL Database and a Storage Account. In this task, you will create a new SQL Database  to allow the application to persist its data. In addition, you will define a Cloud Service to host your web application, and a Storage Account to store the diagnostic data collected by the application.

1. Navigate to [http://manage.windowsazure.com](http://manage.windowsazure.com) using a Web browser and sign in using the Microsoft Account associated with your Microsoft Azure account.

	![Signing in to the Microsoft Azure Management portal](Images/signing-in-to-the-windows-azure-management-po.png?raw=true)

	_Signing in to the Microsoft Azure Management portal_

1. First, create an **Affinity Group** where your services will be deployed. In the Microsoft Azure Management Portal menu, click **Settings**.

	![Select Settings](Images/select-settings.png?raw=true)

	_Select Settings_

1. In the **Settings** page, click **Affinity Groups**.

	![Settings page](Images/settings-page.png?raw=true)

	_Settings page_

1. Click **Add** button in order to create a new **Affinity Group**.

	![Add Affinity Group](Images/add-affinity-group.png?raw=true)

	_Add Affinity Group_

1. In the **Create Affinity Group** dialog box, enter a **Name** (e.g. _MyAffinityGroup_) a **Description** and the **Region** for your new group. Click the **Tick** button to continue.

	![Affinity Group details](Images/affinity-group-details.png?raw=true)

	_Affinity Group details_

	>**Note:** The reason that you are creating a new affinity group is to deploy both the cloud service and storage account to the same location, thus ensuring high bandwidth and low latency between the application and the data it depends on.

1. Now, you will create the **Storage Account** that the application will use to store the diagnostic data. In the Microsoft Azure Management Portal, click **New** | **Data Services** | **Storage** | **Quick Create**.

1. Set a unique **URL**  (e.g. _storagemytodo_) and select the _Affinity Group_ you previously created. Click **Create Storage Account** button to continue.

	![Creating a new storage account](Images/creating-a-new-storage-account.png?raw=true)

	_Creating a new storage account_

	> **Note:** The URL used for the storage account corresponds to a DNS name and is subject to standard DNS naming rules. Moreover, the name is publicly visible and must therefore be unique. The portal ensures that the name is valid by verifying that the name complies with the naming rules and is currently available. A validation error will be shown if you enter a name that does not satisfy the rules.
	>
	> ![URL Validation](./Images/url-validation.png?raw=true)

1. Wait until the storage account is created. Select your storage account and click **Manage Access Keys** at the bottom of the page in order to show the storage account's access keys.

	![Manage Storage Account keys](Images/manage-storage-account-keys.png?raw=true)

	_Manage Storage Account keys_

1. Copy the **Storage account name**, and the **Primary access key** values. You will use these values later on to configure the application.

	![Retrieving the storage access keys](Images/retrieving-the-storage-access-keys.png?raw=true)

	_Retrieving the storage access keys_

	>**Note:** The **Primary Access Key** and **Secondary Access Key** provide a shared secret that you can use to access storage. The secondary key gives the same access as the primary key and is used for backup purposes. You can regenerate each key independently in case either one is compromised.

1. Next, create the **Cloud Service** that executes the application code. Click **New** | **Compute** | **Cloud Service** | **Quick Create**.

1.	Select a **URL** for your cloud service (e.g. _servicemytodo_) and the **Affinity Group** where you created the storage account. Click **Create Cloud Service** to continue. Microsoft Azure uses the URL value to generate the endpoint URLs for the cloud service.

	![Creating a new Cloud Service](Images/creating-a-new-cloud-service.png?raw=true)

	_Creating a new Cloud Service_

	>**Note:** The portal ensures that the name is valid by verifying that the name complies with the naming rules and is currently available. A validation error will be shown if you enter name that does not satisfy the rules.
	>
	>![URL prefix validation](./Images/url-prefix-validation.png?raw=true)
	>
	> By choosing the same affinity group you used for your storage account, you ensure that the cloud service is deployed to the same data center.

1.	Wait until your cloud service is created to continue.

	![Cloud Service Created](./Images/cloud-service-created.png?raw=true "Cloud Service Created")

	_Cloud Service Created_

1. Finally, create the **SQL Database** that the application will use to store its data. Click **New | Data Services | SQL Database | Quick Create**. 

1. Enter a **Database Name** (e.g. _dbmytodo_), select **New SQL database server** in the **Server** drop-down list and select the same **Region** specified for the affinity you created previously. Finally, Enter a **Login Name** (e.g. _SQLAdmin_) and a **Password** for the database administrator.

	![Creating a SQL Database](Images/creating-a-sql-database.png?raw=true)

1. Wait until the database is created. Go to the **SQL Databases** page and click your database to go to the **Quick Start** page. Take note of the **Server** information under the **Connect to your database** section. You will use this value later on to configure the application.

	![SQL Database server information](Images/sql-database-server-information.png?raw=true)

1. Do not close the browser window, you will use the portal for the next task.

<a name="Ex1Task2" />
#### Task 2 - Publishing the Application to the Microsoft Azure Management Portal ####

A Cloud Service is a service that hosts your code in the Microsoft Azure environment. It has two separate deployment slots: **staging** and **production**. The staging deployment slot allows you to test your service in the Microsoft Azure environment before you deploy it to production.

In this task, you will create a service package for the myTODO application and then deploy it to the staging environment using the Microsoft Azure Management Portal.

1. Launch **Microsoft Visual Studio Express 2013 for Web** (or greater) as Administrator.

1. In the **File** menu, select **Open Project** and browse to **Ex1-DeployingWithWAZPortal\Begin** in the **Source** folder of the lab. Select **MyTodo.sln** and click **Open**.

	The solution contains the following projects:

	- **MyTodo**. A standard cloud service project configured to support a single web role named **MyTodo.WebUx**.
	- **MyTodo.WebUx**. A web role that hosts the myTODO ASP.NET MVC application in Microsoft Azure.

1. Build the solution in order to download and install the NuGet package dependencies. To do this, click **Build** | **Build Solution** or press **Ctrl** + **Shift** + **B**.

	>**Note:** NuGet is a Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects that use the .NET Framework.
	>
	> When you install the package, NuGet copies files to your solution and automatically makes whatever changes are needed, such as adding references and changing your _App.config_ or _Web.config_ files. If you decide to remove the library, NuGet removes files and reverses whatever changes it made in your project so that no clutter is left.
	>
	>For more information about NuGet, visit [http://nuget.org/](http://nuget.org/).

1. Ensure that the **System.Web.Mvc** assembly is included in the service package that you deploy to Microsoft Azure.  To do this, expand the **References** node in **Solution Explorer** for the **MyTodo.WebUx** project, right-click the **System.Web.Mvc** assembly and select **Properties**.

	To add the assembly to the service package, in the **Properties** window for the **System.Web.Mvc** assembly, if **Copy Local** setting is set to _False_ then change it to _True_.

	![Including assemblies in the service package deployed to Microsoft Azure](./Images/including-assemblies-in-the-service-package-d.png?raw=true "Including assemblies in the service package deployed to Microsoft Azure")

	_Including assemblies in the service package deployed to Microsoft Azure_

	>**Note:** In general, you need to set **Copy Local = True** for any assembly that is not installed by default in the Microsoft Azure VMs to ensure that it is deployed with your application. 

1. Next, change the size of the virtual machine that will host the application. To do this, in **Solution Explorer**, expand the **Roles** node of the **MyTodo** project and then double-click the **MyTodo.WebUX** role to open its properties window. In the **Configuration** page, locate the **VM** Size setting under the **Instances** category and select the **Extra small** size from the drop-down list.

	![Configuring the size of the virtual machine for the deployment](Images/configuring-vm-depl-size.png?raw=true)

	_Configuring the size of the virtual machine (VM) for the deployment_

	>**Note:** When you create your service model, you can specify the size of the virtual machine (VM) to which to deploy instances of your role, depending on its resource requirements. The size of the VM determines the number of CPU cores, the memory capacity, the local file system size allocated to a running instance, and the network throughput.

1. To configure the storage account, open the **ServiceConfiguration.Cloud.cscfg** file located in the **MyTodo** project. Replace the placeholder labeled \[YOUR\_ACCOUNT\_NAME\] with the **Storage Account Name** that you chose when you configured the storage account in Task 1.

1. Next, replace the placeholder labeled \[YOUR\_ACCOUNT\_KEY\] with the **Primary Access Key** value that you recorded earlier, when you created the storage account in Task 1.

	![Configuring the storage account connection string](Images/configuring-storage-account-connection.png?raw=true)

	_Configuring the storage account connection string_

1. Now, set the version of the Microsoft Azure Guest Operating System that should run your service on the virtual machine. To do this, edit the **osVersion** attribute from the **ServiceConfiguration** root element and set its value to _WA-GUEST-OS-3.10_201312-01_, as shown in the figure below.

	>**Note:** The value used for **osVersion** here is to illustrate that you can select which release of the guest OS runs your application.

	![Configuring the version of the guest operating system that runs the application in the VM](Images/configuring-guestos.png?raw=true)

	_Configuring the version of the guest operating system that runs the application in the VM_

	>**Note:** Microsoft Azure runs a guest operating system into which your service application will be deployed. This guest operating system is regularly updated. While rare, there is some chance that updated guest operating system versions may introduce breaking changes in your application. By setting the **osVersion** attribute, you ensure that your application runs in a version of the Microsoft Azure guest operating system that is compatible with the version of the Microsoft Azure SDK with which you developed it. You may then take the time to test each new **osVersion** prior to running it in your production deployment.
	
	>To configure the operating system version, you need to edit the service definition file directly because the current release of the Microsoft Azure Tools for Microsoft Visual Studio does not support setting this attribute through its user interface. 
	
	>Microsoft Azure offers an auto-upgrade feature, that automatically upgrades your service to use the latest OS version whenever it becomes available, thus ensuring that your service runs in an environment with the latest security fixes. This is the default mode if you omit an **osVersion** when you deploy your service. To change an existing service to auto-upgrade mode, set the **osVersion** attribute to the value "*".
	
	>For information on available versions of the Microsoft Azure guest operating system, see [Microsoft Azure Guest OS Versions and SDK Compatibility Matrix](http://msdn.microsoft.com/en-us/library/ee924680.aspx).

1. Press **CTRL + S** to save the changes.

1. Configure the connection string of the SQL Database to store the application data. To do this, open the **Web.config** file located in the **MyTodo.WebUx** project. Replace the placeholder labeled \[YOUR\_DATABASE\_SERVER\] with the **SQL Database server** information that you recorded earlier, when you configured the SQL database in Task 1.

1. Next, configure the placeholders labeled \[YOUR\_DATABASE\_NAME\], \[YOUR\_USER\_NAME\] and \[YOUR\_USER\_PASSWORD\] with the **database name**, the **login name** and the **login password** that you entered when you configured the SQL database in Task 1.

	![Configuring the database connection string](Images/configuring-the-database-connection-string.png?raw=true)

	_Configuring the database connection string_

1. Press **CTRL + S** to save the changes.

1. To create a service package, right-click the cloud service project and select **Package**. 

1. In the **Package Microsoft Azure Application** dialog box, ensure that **Service configuration** value is set to _Cloud_. Then click **Package** and wait until Visual Studio creates it. Once the package is ready, a window showing the folder that contains the generated files should open. Do not close this window, you will use the package later in this task.

	![Creating a service package in Visual Studio](Images/creating-a-service-package.png?raw=true)

	_Creating a service package in Visual Studio_

1.	At the portal, locate the Cloud Service you previously created and click its name in order to go to the **Quick Start** page.

1. Click on the **Dasbboard** tab to navigate to the dashboard view.

1. Make sure **Staging** tab is selected and click **Upload a new staging deployment**.

	![Uploading the application to Microsoft Azure](Images/uploading-the-application-to-windows-azure.png?raw=true)

	_Uploading the Application to Microsoft Azure_

	>**Note:** A Cloud Service is a service that runs your code in the Microsoft Azure environment. It has two separate deployment slots: staging and production. The staging deployment slot allows you to test your service in the Microsoft Azure environment before you deploy it to production.

1.	In the **Upload a package** dialog box, enter a label to identify the deployment at the **Deployment Name**; for example, use _MyTodo-v1_.

	>**Note:** The management portal displays the label in its user interface for staging and production, which allows you to identify the version currently deployed in each environment.

1.	Under **Package** click **From Local** button, navigate to the folder where Visual Studio generated the package in the prior steps and then select **MyTodo.cspkg** file.

	>**Note:** The _.cspkg_ file is an archive file that contains the binaries and files required to run a service.

1.	Now, under **Configuration** click **From Local** button and select **ServiceConfiguration.Cloud.cscfg** file within the same folder.

	>**Note:** The _.cscfg_ file contains configuration settings for the application, including the instance count and configuration for the web role, and the storage account settings that you modified previously.

1.	Finally, check **Deploy even if one or more roles contain a single instance**. Then click the **Tick** button to start the deployment.

	![Configuring the service package deployment](Images/configuring-service-package-deployment.png?raw=true)

	_Configuring the service package deployment_
 
1.	Notice that the package begins to upload and that the portal shows the status of the deployment to indicate its progress.

	![Uploading a service package to the Microsoft Azure Management Portal](./Images/uploading-a-service-package-to-the-windows-az.png?raw=true)

	_Uploading a service package to the Microsoft Azure Management Portal_

1. Wait until the deployment process finishes, which may take several minutes. At this point, you have already uploaded the package and you can view the deployment status on the dashboard. When the deployment reaches its **Running** state, the portal assigns a **DNS name** to the deployment that includes a unique identifier. Shortly, you will access this URL to test the application and determine whether it operates correctly in the Microsoft Azure environment, but first you need to configure it.

	>**Note:** During deployment, Microsoft Azure analyzes the configuration file and copies the service to the correct number of machines, and starts all the instances. Load balancers, network devices and monitoring are also configured during this time.

	![Application being deployed](Images/package-successfully-uploaded.png?raw=true)

	_Application being deployed_

<a name="Ex1Task3" />
#### Task 3 – Configuring the Application to Increase Number of Instances ####

Before you can test the deployed application, you need to configure it. In this task, you change the service configuration already deployed to manually increase the number of instances.

1. In the **Microsoft Azure Management Portal**, go to the **Cloud Services** page and click your **MyTodo** service name to open the service **Quick Start** page.
 
	![Configuring application settings](./Images/configuring-app-settings.png?raw=true "Configuring application settings")

	_Configuring application settings_

1. Click **Scale** in order to increase the number of roles your application has.

1. Make sure that the **Scale by Metric** option of the **MyTodo.WebUx** role is set to **None**.

	> **Note:** You can also configure your cloud service to automatically increase or decrease the number of role instances based on the **average CPU usage** of each instance, or the **number of queue messages** each instance should support. For more details, see [Automatically scale an application running Web Roles, Worker Roles, or Virtual Machines](http://www.windowsazure.com/en-us/documentation/articles/cloud-services-how-to-scale#autoscale).

	> Additionally, you can schedule automatic scaling of your application by configuring schedules for different times. For more details, see [Schedule the scaling of your application](http://www.windowsazure.com/en-us/documentation/articles/cloud-services-how-to-scale#schedule).


	![Scale by Metric option disabled](Images/scale-by-metric-option-disabled.png?raw=true)
	
	_Scale by Metric option disabled_

1. In the **Scale** page, make sure **Staging** tab is selected and update the number of roles to _2_.

	![Scaling cloud service](Images/scaling-cloud-service.png?raw=true)

	_Scaling Cloud Service_
 
	>**Note:** The initial number or roles is determined by the **ServiceConfiguration.cscfg** file that you uploaded earlier, when you deployed the package in Task 2.

	>**Note:** The **Instances** setting controls the number of roles that Microsoft Azure starts and is used to scale the service. For a token-based subscription —currently only available in countries that are not provisioned for billing— this number is limited to a maximum of two instances. However, in the commercial offering, you can change it to any number that you are willing to pay for.

1. Click **Save** to update the configuration and wait for the **Cloud Service** to apply the new settings.
 
	![Updating the number of role instances](./Images/updating-number-role-instances.png?raw=true "Updating the number of role instances")

	_Updating the number of role instances_

	>**Note:** The portal displays a "Changing the scale settings..." message while the settings are applied.

<a name="Ex1Task4" />
#### Task 4 – Testing the Application in the Staging Environment ####

In this task, you run the application in the staging environment and access its Web Site URL to test that it operates correctly.

1. In the **Cloud Services** page, go to your MyTodo service **Dashboard** and then click the **Site URL** link.

	![Running the application in the staging environment](Images/running-app-staging.png?raw=true)

	_Running the application in the staging environment_

	>**Note:** The address URL is shown as _\<guid\>.cloudapp.net_, where \<_guid_\> is some random identifier. This is different from the address where the application will run once it is in production. Although the application executes in a staging area that is separate from the production environment, there is no actual physical difference between staging and production – it is simply a matter of where the load balancer is connected.

1. Click **Register** to create a local account.

	![Application running in the staging environment](Images/application-running-staging.png?raw=true)

	_Application running in the staging environment_

1. Complete the account details by entering a user name, email address, and password and then click **Register**. 

	>**Note:**  Account information is stored in the SQL database created earlier. Data is not shared between to-do lists.

	![Creating a new account](Images/application-new-account.png?raw=true)

	_Creating a new account_

1. Next, the application enumerates the lists that you have currently defined. A sample todo list is provided by default for new users.

	![Application ready to be used](Images/application-ready.png?raw=true)

	_Application ready to be used_

1. If you wish to explore the application, update the existing to-do list, or create a new to-do list and enter some items.

<a name="Ex1Task5" />
#### Task 5 – Promoting the Application to Production ####

Now that you have verified that the service is working correctly in the staging environment, you are ready to promote it to final production.  When you deploy the application to production, Microsoft Azure reconfigures its load balancers so that the application is available at its production URL.

1. In the **Cloud Services** page, click your MyTodo service **name**. Then click in the **Dashboard** tab and select **Staging**. Finally, click **Swap** button from the bottom menu.

	![Promoting the application to the production slot](Images/promoting-app-prod.png?raw=true)

	_Promoting the application to the production slot_

1. In the **VIP Swap** dialog box, click **Yes** to swap the deployments between staging and production.

	![Promoting the application to the production deployment](Images/promoting-app-deploy.png?raw=true)
	
	_Promoting the application to the production deployment_

1. Once the transition finishes, switch to **Production** tab and click the **Site URL** link to open the production site in a browser window and notice the URL in the address bar.
 
	![Application running in the production environment](Images/application-running-production.png?raw=true)

	_Application running in the production environment_

	>**Note:** If you visit the production site shortly after its promotion, the DNS name might not be ready. If you encounter a DNS error (404), wait a few minutes and try again. Keep in mind that Microsoft Azure creates DNS name entries dynamically and that the changes might take few minutes to propagate.

	>**Note:** Even when a deployment is in a suspended state, Microsoft Azure still needs to allocate a virtual machine (VM) for each instance and charge you for it. Once you have completed testing the application, you need to remove the deployment from Microsoft Azure to avoid an unnecessary expense. To remove a running deployment, go to your cloud service **Dashboard** page, select the deployment slot where the service is currently hosted, staging or production, and then click **Stop** on the bottom menu. Once the service has stopped, click **Delete** to remove it.

<a name="Exercise2" />
### Exercise 2: Using PowerShell to manage Microsoft Azure Applications ####

Typically, during its lifetime, an application undergoes changes that require it to be re-deployed. In the previous exercise, you saw how to use the Microsoft Azure Management Portal to deploy applications. As an alternative, the Service Management API provides programmatic access to much of the functionality available through the Management Portal. Using the Service Management API, you can manage your storage accounts, SQL databases and cloud services, your service deployments, and your affinity groups.

The Microsoft Azure Service Management PowerShell Cmdlets wrap the Microsoft Azure Service Management API. These cmdlets make it simple to automate the deployment, upgrade, and scaling of your Microsoft Azure application. By pipelining commands, you compose complex scripts that use the output of one command as the input to another.

In this exercise, you will learn how to deploy and upgrade a Microsoft Azure application using the Azure Services Management Cmdlets. 

<a name="Ex2Task1" />
#### Task 1 - Downloading and Importing a Publish-settings File ####

In this task, you will log on to the Microsoft Azure Portal and download the publish-settings file. This file contains the secure credentials and additional information about your Microsoft Azure Subscription to use in your development environment. Then, you will import this file using the Microsoft Azure Cmdlets in order to install the certificate and obtain the account information.

1. Run **Microsoft Azure PowerShell** as Administrator.

1.	Change the PowerShell execution policy to **RemoteSigned**. When asked to confirm press **Y** and then **Enter**.
	
	<!-- mark:1 -->
	````PowerShell
	Set-ExecutionPolicy RemoteSigned
	````

	> **Note:** The Set-ExecutionPolicy cmdlet enables you to determine which Windows PowerShell scripts (if any) will be allowed to run on your computer. Windows PowerShell has four different execution policies:
	>
	> - _Restricted_ - No scripts can be run. Windows PowerShell can be used only in interactive mode.
	> - _AllSigned_ - Only scripts signed by a trusted publisher can be run.
	> - _RemoteSigned_ - Downloaded scripts must be signed by a trusted publisher before they can be run.
	> - _Unrestricted_ - No restrictions; all Windows PowerShell scripts can be run.
	>
	> For more information about Execution Policies refer to this TechNet article: <http://technet.microsoft.com/en-us/library/ee176961.aspx>

1. Execute the following command to download the subscription information. This command will open a web page on the Microsoft Azure Management Portal.

	````PowerShell
	Get-AzurePublishSettingsFile
	````

1. Sign in using the Microsoft Account associated with your Microsoft Azure account.

1.	**Save** the Publish Settings file to your local machine.

	![Downloading publish-settings file](Images/downloading-publish-settings-file.png?raw=true "Downloading publish-settings file")

	_Downloading Publish Settings file_

1.	The following script imports your publish-settings file and persists this information for later use. You will use these values during the lab to manage your Microsoft Azure Subscription. Replace the placeholder with your publish-setting file’s path and execute the script.

	<!-- mark:1 -->
	````PowerShell
	Import-AzurePublishSettingsFile '[YOUR-PUBLISH-SETTINGS-PATH]'
	````

1.  Execute the following commands to determine your subscription and storage account name.

	<!-- mark:1-2 -->
    ````PowerShell
    Get-AzureSubscription | select SubscriptionName
    Get-AzureStorageAccount | select StorageAccountName 
    ````

1.  Execute the following commands to set your current storage account for your subscription.

	<!-- mark:1 -->
	````PowerShell
	Set-AzureSubscription -SubscriptionName '[YOUR-SUBSCRIPTION-NAME]' -CurrentStorageAccount '[YOUR-STORAGE-ACCOUNT]' 
	````

<a name="Ex2Task2" />
#### Task 2 – Configuring the Application ####

In this task, you will configure the application using your SQL database and storage account information and generate a package to publish it using Microsoft Azure PowerShell CmdLets.

1. If it is not already open, launch **Microsoft Visual Studio Express 2013 for Web** (or greater) as Administrator.

1. In the **File** menu, choose **Open Project** and browse to **Ex2-DeployingWithPowerShell\Begin** in the **Source** folder of the lab. Select **MyTodo.sln** and click Open.

	> **Note:** Alternatively, you may continue with the solution that you completed during Exercise 1. You can dismiss this task if you selected to continue with the solution from Exercise 1.

1. Configure the storage account connection strings. To do this, expand the **Roles** node in the **MyTodo** project and double-click the **MyTodo.WebUX** role. In the role properties window, select the **Settings** tab and select _Cloud_ from the **Service Configuration** drop-down list. Then select the _Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString_ setting, ensure that the **Type** is set to _Connection String_, and then click the button labeled with an ellipsis.

	![Defining storage account connection settings](Images/defining-connection-settings.png?raw=true)

	_Defining storage account connection settings_

1. In the **Create Storage Connection String** dialog box, select the **Manually entered credentials** option. Enter your storage **Account Name** and storage **Account Key** and click **OK**.
 
	![Configuring the storage account name and account key](./Images/defining-connection-settings-2.png?raw=true "Configuring the storage account name and account key")
	
	_Configuring the storage account name and account key_

	>**Note:** This information is available in the **Dashboard** section of your storage account in the Microsoft Azure Management Portal. You used the same settings in Exercise 1, when you deployed and configured the application. 

1. Press **CTRL + S** to save your changes. 

1. Next, open the **Web.config** file located in the **MyTodo.WebUx** project. Replace the placeholders with the **SQL Database server**, **database name**, **login name** and **login password** of your SQL database.

	![Configuring the database connection string](Images/configuring-the-database-connection-string.png?raw=true)

	_Configuring the database connection string_

1. Press **CTRL + S** to save the changes.

1. To create a service package, right-click the cloud service project and select **Package**. In the **Package Microsoft Azure Application** dialog, ensure that **Service configuration** value is set to _Cloud_. Click **Package** and then wait until Visual Studio creates it. Once the package is ready, Visual Studio opens a window showing the folder that contains the generated files.

<a name="Ex2Task3" />
#### Task 3 – Uploading a Service Package Using Windows PowerShell ####

In the previous exercise, you uploaded the service package for the myTODO application using the Microsoft Azure Management Portal. In this task, you deploy the package using the Microsoft Azure Service Management PowerShell cmdlets instead. 

1. If not already open, open a **Microsoft Azure PowerShell** command prompt as Administrator.

1. Change the current directory to the location where you generated the service package for the myTODO application in the previous task. For your facility you might want to copy the packages into a shorter URL path (e.g. _C:\HOL\_).

1. Next, enter the command shown below. Use the following command line arguments ensuring that you replace the parameter placeholders with the settings that apply to your service account.

	<!-- mark:1-5 -->
	````PowerShell
	$serviceName = '[YOUR-SERVICE-NAME-LOWER-CASE]'
	$packageLocation = '[PACKAGE-LOCATION]'
	$configurationLocation = '[CONFIGURATION-LOCATION]'
	$deploymentLabel = 'MyTodo-v2'
	New-AzureDeployment -ServiceName $serviceName -Package $packageLocation -Configuration $configurationLocation -Slot 'Staging' -Label $deploymentLabel -DoNotStart
	````

	|                                |   |
	|--------------------------------|---|
	| [YOUR-SERVICE-NAME-LOWER-CASE] | The service name chosen when configuring the cloud service URL in Exercise 1, not its Service Label. |
	| [PACKAGE-LOCATION]             | A path to a local file or the URL of the blob in your Storage Account that contains the service package. |
	| [CONFIGURATION-LOCATION]       | A path to a local file or the public URL of the blob that contains the service configuration file. |
	| [YOUR-DEPLOYMENT-LABEL]        | The deployment label. |

	>**Note:** The command shown above uses the **New-AzureDeployment** cmdlet to upload a service package and create a new deployment in the staging environment. It assigns a "MyTodo-v2" label to identify this deployment.

1. Press **ENTER** to execute the command and wait until the **New-AzureDeployment** command finishes.

	![Deploying a new service package to Microsoft Azure using PowerShell](./Images/command-line-deploying-powershell.png?raw=true "Deploying a new service package to Microsoft Azure using PowerShell")

	_Deploying a new service package to Microsoft Azure using PowerShell_

1. In the Microsoft Azure Management Portal, open the Cloud Service's **Dashboard** page and notice how the deployment for the staging environment shows its status with the message "**Updating deployment...**" in the UI. It may take a few seconds for the service status to refresh. Wait for the deployment operation to complete and display the status as **Stopped**.

	![Deployment Stopped](./Images/deployment-stopped.png?raw=true "Deployment Stopped")

	_Deployment Stopped_

	>**Note:** Normally, you will not use the management portal to view the status and determine the outcome of your deployment operations. Here, it is shown to highlight the fact that you can use the management API to execute the same operations that are available in the management portal. In the next task, you will see how to use a cmdlet to wait for the operation to complete and retrieve its status.

1. Keep Microsoft Visual Studio and the PowerShell console open. You will need them for the next task.

<a name="Ex2Task4" />
#### Task 4 – Upgrading a Deployment Using Windows PowerShell ####

In this task, you use the Microsoft Azure PowerShell cmdlets to upgrade an existing deployment. First, you will change the original solution by making minor changes to its source code to produce an updated version of the application. Next, you will build the application and create a new service package that contains the updated binaries. Finally, you will re-deploy the package to Microsoft Azure using the management cmdlets. 

1. Go back to Microsoft Visual Studio. 

1. Open the layout view of the application for editing. To do this, in **Solution Explorer**, double-click **Index.cshtml** in the **Views\Home** folder of the **MyTodo.WebUx** project.

1. Insert a new caption in the footer area of the page. Go to the bottom of the layout view and update the copyright notice with the text "(_Deployed with the PowerShell CmdLets_)" as shown below.

	<!-- mark:4 -->
	````HTML
	...
	<hr />
	<footer>
		<p>&copy; @DateTime.Now.Year - myTODO (Deployed with the PowerShell CmdLets)</p>
	</footer>
	...
	````

1. Generate a new service package. To do this, in **Solution Explorer**, right-click the cloud service project and select **Package**. In the **Package Microsoft Azure Application** dialog, ensure that **Service configuration** value is set to _Cloud_. Click **Package** and then wait until Visual Studio creates it. Once the package is ready, Visual Studio opens a window showing the folder that contains the generated files. 

1. Switch to the PowerShell console and enter the command shown below, specifying the settings that apply to your service account where indicated by the placeholder parameters. Do **not** execute the command yet.

	<!-- mark:1-4 -->
	````PowerShell
	$packageLocation = '[PACKAGE-LOCATION]'
	$configurationLocation = '[CONFIGURATION-LOCATION]'
	$deploymentLabel = 'MyTodo-v21'
	Get-AzureService -ServiceName $serviceName | Get-AzureDeployment -Slot staging | Set-AzureDeployment -Package $packageLocation -Configuration $configurationLocation -Upgrade -Label $deploymentLabel
	````

	>**Note:** The command line shown above concatenates a sequence of cmdlets. First, it obtains a reference to the cloud service using **Get-AzureService**, then it uses **Get-AzureDeployment** to retrieve its _staging_ environment, and finally it uploads the package using **Set-AzureDeployment**. This is done to illustrate how to compose a complex command using the PowerShell pipeline. In fact, in this particular case, you could instead provide all the required information to **Set-AzureDeployment** to achieve the same result. For example:

	>> Set-AzureDeployment -Upgrade -ServiceName $serviceName -Package $packageLocation -Configuration $configurationLocation -Slot 'Staging' -Label $deploymentLabel 

1. Press **ENTER** to execute the command. Wait until the deployment process finishes, which may take several minutes. When the operation ends, it displays a message with the outcome of the operation; if the deployment completes without errors, you will see the "Succeeded" message.

	![PowerShell console showing the status of the package deployment operation](./Images/command-line-powershell-status.png?raw=true "PowerShell console showing the status of the package deployment operation")

	_PowerShell console showing the status of the package deployment operation_

	>**Note:** Deploying a package using the steps described above does not change its state. If the service is not running prior to deployment, it will remain in that state. To start the service, you need to update its deployment status using the **Set-AzureDeployment** cmdlet.

1. To change the status of the service to a running state, in the **PowerShell** console, enter the following command.

	<!-- mark:1 -->
	````PowerShell
	Set-AzureDeployment -Status -ServiceName $serviceName -NewStatus 'Running' -Slot 'Staging'
	````

1. Finally, swap the deployments in the staging and production environments. To do this, in the **PowerShell** console, use the **Get-Deployment** and **Move-Deployment** cmdlets, as shown below.

	<!-- mark:1 -->
	````PowerShell
	Get-AzureService -ServiceName $serviceName | Get-AzureDeployment -Slot staging | Move-AzureDeployment
	````

<a name="Ex2Task5" />
#### Task 5 - Verification ####

Now that you have deployed your updated solution to Window Azure, you are ready to test it. You will launch the application to determine if the deployment succeeded and ensure that the service is working correctly and is available at its production URL. 

1. In your Cloud Service **Dashboard** page within the management portal, click the **Web Site URL** link to open the production site in a browser window. Notice the footer of the page. It should reflect the updated text that you entered in the last task.

	![New deployment showing the updated footer text](Images/new-deployment.png?raw=true)

	_New deployment showing the updated footer text_

	>**Note:** If you visit the production site shortly after its promotion, the DNS name might not be ready. If you encounter a DNS error (404), wait a few minutes and try again. Keep in mind that Microsoft Azure creates DNS name entries dynamically and that the changes might take few minutes to propagate.

<a name="Exercise3" />
### Exercise 3: Using Visual Studio to Publish Applications ####

Previously, you learnt how to deploy applications to Microsoft Azure using the Management Portal and a set of PowerShell cmdlets. If you are a developer, you may find it more convenient during your development cycle to deploy your applications directly from Visual Studio. 

The first time you publish a service to Microsoft Azure using Visual Studio, you need to create the necessary credentials to access your account. For doing this, you will use the PublishSettings file you downloaded in the previous exercise. 

Once you set up your account information in Visual Studio, you can publish your current solution in the background with only a few mouse clicks.

In this exercise, you will set up the credentials to authenticate with the Microsoft Azure Management Service and then publish the myTODO application from Visual Studio.


<a name="Ex3Task1" />
#### Task 1 – Preparing the Solution for Publish ####

When you publish your service using Visual Studio, the Microsoft Azure Tools upload the service package and then automatically start it. You will not have a chance to update the configuration settings before the service starts. Therefore, you must configure all the necessary settings before you publish the service.

In this task, you will update the storage connection strings to point to your SQL database and your storage account.

1. If it is not already open, launch **Microsoft Visual Studio Express 2013 for Web** (or greater) as Administrator.

1. In the **File** menu, choose **Open Project** and browse to **Ex3-DeployingWithVisualStudio\Begin** in the **Source** folder of the lab. Select **MyTodo.sln** and click Open.

	>**Note:** This is the same solution deployed earlier except for a legend in its footer area to indicate that it was deployed using **Visual Studio**.

1. In **Solution Explorer**, expand the **Roles** node in the **MyTodo** project, double-click the **MyTodo.WebUx** role to open its properties window, and then switch to the **Settings** tab.

1. Configure the storage account connection strings. To do this, expand the **Roles** node in the **MyTodo** project and double-click the **MyTodo.WebUX** role. In the role properties window, select the **Settings** tab and select _Cloud_ from the **Service Configuration** drop-down list. Then select the _Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString_ setting, ensure that the **Type** is set to _Connection String_, and then click the button labeled with an ellipsis.

	![Defining storage account connection settings](./Images/defining-connection-settings.png?raw=true "Defining storage account connection settings")

	_Defining storage account connection settings_

1. In the **Create Storage Account Connection String** dialog box, select the **Manually entered credentials** option. Complete your storage **Account Name** and storage **Account Key** and click **OK**.

	![Configuring the storage account name and account key](./Images/defining-connection-settings-2.png?raw=true "Configuring the storage account name and account key")

	_Configuring the storage account name and account key_

	>**Note:** This information is available in the **Dashboard** section of your storage account in the Microsoft Azure Management Portal. You used the same settings in Exercise 1, when you deployed and configured the application. 

1. Press **CTRL + S** to save your changes. 

1. Next, open the **Web.config** file located in the **MyTodo.WebUx** project. Replace the placeholders with the **SQL Database server**, **database name**, **login name** and **login password** of your SQL database.

	![Configuring the database connection string](Images/configuring-the-database-connection-string.png?raw=true)

	_Configuring the database connection string_

1. Press **CTRL + S** to save the changes.

<a name="Ex3Task2" />
#### Task 2 – Publishing a Service with the Microsoft Azure Tools ####

In this task, you will configure a set of credentials that provide access to your Microsoft Azure account. Visual Studio saves this information and allows you to reuse the credentials whenever you need to publish a service, without requiring you to enter your credentials again.

Then, you will use these credentials to publish the myTODO application directly from Visual Studio.

1. In **Solution Explorer**, right-click the **MyTodo** cloud project and select **Publish**.

1. In the **Publish Microsoft Azure Application** dialog box, click the subscription drop-down list and select **Manage...**.

	![Manage subscriptions](Images/manage-subscriptions.png?raw=true)

	_Manage subscriptions_

1. In the **Manage Microsoft Azure Subscriptions** dialog box, select the **Certificates** tab and click **Import...**.

	![Import Subscription](Images/import-subscription.png?raw=true)

	_Import Subscription_

1. In the **Import Microsoft Azure Subscriptions** dialog box, click **Browse...**, locate the publish-settings file you downloaded in the previous exercise, select it and click **Open**. Then click **Import**.

	> **Note:** It is recommended that you delete the publish-settings file after you import those settings. Because the management certificate includes security credentials, it should not be accessed by unauthorized users. If you need information about your subscriptions, you can get it from the Microsoft Azure Management Portal or the Microsoft Online Services Customer Portal.

	![Select and import subscription file](Images/import-subscription-2.png?raw=true)

	_Select and import subscription file_

1. Click **Close**. Back in the **Publish Microsoft Azure Application** dialog box, select the subscription created from the publish-settings file and click **Next**.
 
	![Signing in](Images/waz-sign-in.png?raw=true)
	
	_Signing in_

1. In the **Common Settings** tab, notice that the dialog populates the drop-down list labeled **Cloud Service** with the information for all the services configured in your Microsoft Azure account. Select the cloud service in this list where you wish to deploy the application.

1. Make sure the **Environment** is set to _Production_ and the **Build Configuration** to _Release_. Also, set the **Service Configuration** to _Cloud_.
 
	![Deployment Common Settings](Images/deployment-common-settings.png?raw=true)
	
	_Deployment Common Settings_

1. Click the **Advanced Settings** tab. Update the **Deployment Label** to _MyTodo_ and select the check box labeled **Append date and time** to identify the deployment in the Developer Portal UI.

1. Like with the cloud services, the drop-down list labeled **Storage account** is populated with all the storage services that you have configured in your Microsoft Azure account. To publish a service, Visual Studio first uploads the service package to storage in Microsoft Azure, and then publish the service from there. Select the storage service that you wish to use for this purpose and click **Next**.
 
	![Deployment Advanced Settings](Images/deployment-advanced-settings.png?raw=true)e)
	
	_Deployment Advanced Settings_

	>**Note:** The **Enable Remote Debugger for all roles** option allows you to attach the Visual Studio debugger to a running cloud service in Microsoft Azure. The remote debugging feature is available in Visual Studio Professional edition (or greater).

	> The Ultimate edition of Visual Studio provides an option to enable **IntelliTrace**. With IntelliTrace, you can capture detailed trace logs of your running service in the cloud that you can download to your desktop to perform historical debugging. This can be invaluable when troubleshooting issues that occur during role start up.

1. Review the Summary information. If everything is OK, click **Publish** to start the deployment process.

	![Starting Deployment](Images/start-deployment.png?raw=true)

	_Starting Deployment_

	>**Note:** At the top of the dialog box, you will find a Target Profile drop down list. Once you configured your deployment settings, you can save them as a new profile and use it later without having to complete all the fields again.

1. If the slot that you chose is already occupied by a previous deployment, Visual Studio warns you and asks for confirmation before it replaces it. Click **Replace** if you are certain that the current deployment is no longer needed and can be overwritten. Otherwise, click **Cancel** and repeat the operation choosing a different deployment slot.

	![Overwriting an existing deployment](./Images/overwrite-existing-deployment.png?raw=true)

	_Overwriting an existing deployment_

1. After you start a deployment, you can examine the Microsoft Azure activity log window to determine the status of the operation. If this window is not visible, in the **View** menu, point to **Other Windows**, and then select **Microsoft Azure Activity Log**.

1. By default, the log shows a descriptive message and a progress bar to indicate the status of the deployment operation. 

	![Viewing summary information in the Microsoft Azure activity log](Images/waz-activity-summary.png?raw=true)

	_Viewing summary information in the Microsoft Azure activity log_

1. To view detailed information about the deployment operation in progress, click the arrow on the left side of the activity log entry.
Notice that the additional information provided includes the deployment slot, **Production** or **Staging**, the **Website URL**, the **Deployment ID**, and a **History** log that shows state changes, including the time when each change occurred. 

	![Viewing detailed information about a deployment operation](Images/detailed-deployment-information.png?raw=true)

	_Viewing detailed information about a deployment operation_

1. Wait for the deployment operation to complete, which may take several minutes. While this is happening, you can examine the **History** panel on the right side to determine the status of the deployment. For a successful deployment, it should resemble the following sequence.

	![Deployment operation history log](Images/deployment-operation-log.png?raw=true)

	_Deployment operation history log_

1. Once the deployment operation is complete, in the **Microsoft Azure Activity Log**, click the **Website URL** link for the completed operation to open the application in your browser and ensure that it is working properly. Notice the legend in the copyright at the bottom of the page indicating that this is the version that you deployed with Visual Studio.

	![Running the application deployed with Visual Studio](Images/running-deployment.png?raw=true)

	_Running the application deployed with Visual Studio_

<a name="Exercise4" />
### Exercise 4: Securing Microsoft Azure with SSL ###

In this exercise, you will enable SSL to secure the myTODO application. This involves creating a self-signed certificate for server authentication and uploading it to the Microsoft Azure portal. With the certificate in place, you will add a new HTTPS endpoint to the service model and assign the certificate to this endpoint. You will complete the exercise by deploying the application to Microsoft Azure one more time and then access it using its HTTPS endpoint.

<a name="Ex4Task1" />
#### Task 1 – Adding an HTTPS Endpoint to the Application ####

In this task, you will update the service model of myTODO to add an HTTPS endpoint and then you test the application in the compute emulator.

1. If it is not already open, launch **Microsoft Visual Studio Express 2013 for Web** (or greater) as Administrator.

1. In the **File** menu, choose **Open Project** and browse to **Ex4-SecuringAppWithSSL\Begin** in the **Source** folder of the lab. Select **MyTodo.sln** and click Open.

1. In **Solution Explorer**, expand the **Roles** node in the **MyTodo** project, and then double-click the **MyTodo.WebUx** role to open its properties window.

1. Switch to the **Endpoints** tab. In the existing endpoint entry, set the **Protocol** field to _HTTPS_, enter the _443_ value for the **Public Port** field and leave the **Name** field unchanged. Do not set the SSL certificate at this time; you will do that later in the exercise.

	![Adding an HTTPs endpoint to the application](Images/adding-https-endpoint.png?raw=true)

	_Adding an HTTPs endpoint to the application_

1. Now, select the HTTPS endpoint as the one to use when you launch the application in the browser when you are debugging it. To do this, right-click the **MyTodo.WebUx** role in the **MyTodo** project, point to **Launch in Browser**, and ensure that only **HTTPS** is selected.

	![Selecting the endpoint used to debug the application](Images/selecting-debug-endpoint.png?raw=true)

	_Selecting the endpoint used to debug the application_

1. You will now test the application locally. Press **F5** to build and launch the application in the compute emulator. Notice that the browser indicates that there is a problem with the certificate. Ignore the warning and click **Continue to this website**.

	![Certificate error when testing in the compute emulator](./Images/certificate-error-computer-emulator.png?raw=true "Certificate error when testing in the compute emulator")

	_Certificate error when testing in the compute emulator_

	>**Note:** When testing your application in the local development environment using SSL, you do not need to configure a certificate. Instead, the compute emulator handles this requirement by using its own certificate. However, the certification authority for the certificate that it uses is not trusted, hence the warning. You may safely ignore the warning while you test your application locally.

	>If you wish, you can remove the warning by installing the certificate in the **Trusted Root Certification Authorities** certificate store. Note, however, that this has security implications that you need to evaluate before you proceed.

	>To remove the warning, open the **Microsoft Management Console** by pressing Windows key and searching for **MMC**. Add an instance of the **Certificates** snap-in and configure it to manage certificates for the **Computer** account. Expand the **Personal\Certificates** store and locate a certificate issued to 127.0.0.1. To ensure that you have the right certificate, view its properties to verify that the **Subject** and **Issuer** fields identify the certificate as belonging to the compute emulator. To trust the certificate, simply drag and drop the certificate from the **Personal** certificate store into the **Trusted Root Certification Authorities** certificate store.

	> ![Certificate used by the compute emulator to implement SSL](./Images/computer-emulator-certificate.png?raw=true "Certificate used by the compute emulator to implement SSL")

	>_Certificate used by the compute emulator to implement SSL_

1. After you access the home page, notice that the address bar shows that you are now accessing the HTTPS endpoint.

	![Accessing the HTTPS endpoint in the compute emulator](Images/accessing-endpoints.png?raw=true)

	_Accessing the HTTPS endpoint in the compute emulator_

1. Close the browser window. You will now create a self-signed certificate and deploy the application to Microsoft Azure.

1. Do not close the project in Visual Studio. You will need it shortly.

<a name="Ex4Task2" />
#### Task 2 – Creating a Self-Signed Certificate ####

In this task, you will create a self-signed certificate that you can upload to the Microsoft Azure Developer Portal to configure an SSL endpoint for your application.

>**Note:** if you are unable to use Internet Information Services (IIS) Manager in your environment, you may skip this task. Instead, you can find a self-signed certificate that you can use among the lab’s resources.

>To install the certificate, open Windows Explorer, browse to **Assets** in the **Source** folder of the lab and then double-click the **MyTodoCertificate.pfx** file to install the certificate using the **Certificate Import Wizard**. Select **Local Machine** as the store location and use "password1" (without the quotation marks) as the password. Use default values for all other options.

>**Important:** You should only use this certificate to complete the steps in the exercise. Do not use the certificate in your production deployments.

1. Start Internet Information Services Manager. To do this, click the Windows button and type "iis" in the search box and then click **Internet Information Services (IIS) Manager** in the list of installed programs.

	![Launching Internet Information Services (IIS) Manager](Images/iis-manager-launch.png?raw=true)
	
	_Launching Internet Information Services (IIS) Manager_

1. In the **Connections** pane of the Internet Information Services (IIS) Manager console, select the top-level node corresponding to your computer. Next, locate the **IIS** category in the middle pane and double-click **Server Certificates**.

	![Managing certificates with Internet Information Services (IIS) Manager](./Images/iis-managing-certificates.png?raw=true "Managing certificates with Internet Information Services \(IIS\) Manager")

	_Managing certificates with Internet Information Services (IIS) Manager_

1. In the **Server Certificates** page, click **Create Self-Signed Certificate** in the **Actions** pane.

	![Creating a self-signed certificate in the Internet Information Services (IIS) Manager](./Images/iis-creating-self-signed-certificate.png?raw=true "Creating a self-signed certificate in the Internet Information Services \(IIS\) Manager")

	_Creating a self-signed certificate in the Internet Information Services (IIS) Manager_

1. In the **Specify Friendly Name** page of the **Create Self-Signed Certificate** wizard, enter a name to identify your certificate —this can be any name, for example, **MyTodoCertificate**—, and then click **OK**.
 
	![Specifying a name for the certificate](./Images/iis-specifying-certificate-name.png?raw=true "Specifying a name for the certificate")
	
	_Specifying a name for the certificate_

1. Now, right-click the newly created certificate and select **Export** to store the certificate in a file. 

	![Server certificates page showing the new self-signed certificate](./Images/iis-server-certificates.png?raw=true "Server certificates page showing the new self-signed certificate")

	_Server certificates page showing the new self-signed certificate_

1. In the **Export Certificate** dialog box, enter the name of a file in which to store the certificate for exporting, type a password and confirm it, and then click **OK**. Take a note of the password. You will require it later on, when you upload the certificate to the portal.

	![Exporting the certificate to a file](./Images/iis-exporting-certificate.png?raw=true)

	_Exporting the certificate to a file_

<a name="Ex4Task3" />
#### Task 3 – Adding the Certificate to the Service Model of the Application ####

Previously, when you tested SSL access to the application in your local environment, you were able to do so without specifying a certificate by taking advantage of the certificate managed by the compute emulator. In this task, you will configure the application to use the self-signed certificate that you created in Internet Information Services (ISS) Manager.

1. Switch back to Visual Studio. If you closed the project, you will need to reopen it from **Ex4-SecuringAppWithSSL\Begin** in the **Source** folder of the lab.

1. In **Solution Explorer**, expand the **Roles** node in the **MyTodo** project, double-click the **MyTodo.WebUx** role to open its properties window, and then switch to the **Certificates** tab.

1. In the **Certificates** page, click **Add Certificate**. Complete the **Name** field with a value that identifies the certificate that you are adding, for example, use _SSL_. Ensure that the **Store Location** is set to _LocalMachine_ and the **Store Name** is set to _My_ and then click the button labeled with an ellipsis, to the right of the **Thumbprint** column.

	![Adding a certificate for the service](Images/adding-certificate.png?raw=true)

	_Adding a certificate for the service_

1. In the **Select a certificate** dialog, select the self-signed certificate that you created earlier and then click **OK**. 

	![Choosing a certificate for the service](./Images/selecting-certificate.png?raw=true "Choosing a certificate for the service")

	_Choosing a certificate for the service_

1. Notice that the dialog populates the **Thumbprint** column with the corresponding value from the certificate.

	![Adding a certificate to the service model of the application](Images/adding-certificate-service-model.png?raw=true)

	_Adding a certificate to the service model of the application_

1. Now, switch to the **Endpoints** tab and, in the **HTTPS** input endpoints section, expand the **SSL certificate name** drop down list and select the certificate that you added to the service in the previous step.

	![Choosing a certificate to use for the HTTPS endpoint](Images/choosing-certificate-https-endpoint.png?raw=true)

	_Choosing a certificate to use for the HTTPS endpoint_

1. Press **CTRL+S** to save the changes to the configuration.

<a name="Ex4Task4" />
#### Task 4 – Uploading the Certificate to the Microsoft Azure Management Portal ####

In this task, you will upload the self-signed certificate created previously to the Microsoft Azure Management Portal.

1. Navigate to [http://manage.windowsazure.com](http://manage.windowsazure.com) using a Web browser and sign in using your Microsoft account.

1. In the **Cloud Services** page, click your cloud service's **name** to go to the service's **Quick Start** page.

1. Click **Certificates** and then **Upload a certificate**.

	![Adding a new certificate](Images/adding-a-new-certificate.png?raw=true)

	_Adding a new Certificate_

1. In the **Upload Certificate** dialog, click **Browse**, and then navigate to the location where you stored the certificate exported in the previous task. Enter the password specified when you exported the certificate, confirm it and then click the **Tick**.

	![Creating a certificate for the service](Images/creating-certificate-service.png?raw=true)

	_Creating a certificate for the service_

<a name="Ex4Task5" />
#### Task 5 - Verification ####

In this task, you will deploy the application to Microsoft Azure and access its HTTPS endpoint to verify that your enabled SSL successfully.

1. Switch back to Visual Studio. If you closed the project, you will need to reopen it from **Ex4-SecuringAppWithSSL\Begin** in the **Source** folder of the lab.

1. In **Solution Explorer**, expand the **Roles** node in the **MyTodo** project, double-click the **MyTodo.WebUx** role to open its properties window, and then switch to the **Settings** tab.

1. Configure the storage account connection strings. To do this, expand the **Roles** node in the **MyTodo** project and double-click the **MyTodo.WebUX** role. In the role properties window, select the **Settings** tab and select _Cloud_ from the **Service Configuration** drop-down list. Then select the _Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString_ setting, ensure that the **Type** is set to _Connection String_, and then click the button labeled with an ellipsis.

	![Defining storage account connection settings](./Images/defining-connection-settings.png?raw=true "Defining storage account connection settings")

	_Defining storage account connection settings_

1. In the **Storage Account Connection String** dialog, select the **Manually entered credentials** option. Complete your storage **Account Name** and storage **Account Key** and click **OK**.

	![Configuring the storage account name and account key](./Images/defining-connection-settings-2.png?raw=true "Configuring the storage account name and account key")

	_Configuring the storage account name and account key_

	>**Note:** This information is available in the **Dashboard** section of your storage account in the Microsoft Azure Management Portal. You used the same settings in Exercise 1, when you deployed and configured the application. 

1. Press **CTRL + S** to save the changes.

1. Next, open the **Web.config** file located in the **MyTodo.WebUx** project. Update the _DefaultConnection_ entry with the following highlighted code, replacing the placeholders with the **SQL Database server**, **database name**, **login name** and **login password** of your SQL database.

	<!--mark: 3-8-->
	````XML
	...
	<connectionStrings>
		<add name="DefaultConnection" providerName="System.Data.SqlClient"
			connectionString="Server=tcp:[YOUR_DATABASE_SERVER];
			Database=[YOUR_DATABASE_NAME];User ID=[YOUR_USER_NAME];
			Password=[YOUR_USER_PASSWORD];
			Trusted_Connection=False;
			Encrypt=True;Connection Timeout=30;" />
	</connectionStrings>
	...
````

1. Press **CTRL + S** to save the changes.

1. Publish and deploy the application once again to the Microsoft Azure environment using your preferred method, by choosing among the Microsoft Azure Developer Portal, the Microsoft Azure Service Management PowerShell Cmdlets, or the Microsoft Azure Tools for Visual Studio. Refer to Exercises 1, 2, and 3 for instructions on how to carry out the deployment with any of these methods.

	>**Note:** The service configuration now specifies an additional endpoint for HTTPS, therefore, you cannot simply upgrade the current deployment and instead, you need to re-deploy the application. This is mandatory whenever you change the topology of the service.

1. Once you have deployed the application, wait until its status is shown as **Running** (or the deployment is shown as **Completed** when deploying with Visual Studio).

1. Now, browse to the HTTPS endpoint (e.g. _https://servicemytodo.cloudapp.net_). Once again, you will observe a certificate error because the certificate authority for the self-signed certificate is not trusted. You may ignore this error.

	![Accessing the HTTPS endpoint in Microsoft Azure](Images/accessing-https-endpoint.png?raw=true)

	_Accessing the HTTPS endpoint in Microsoft Azure_

	>**Note:** For your production deployments, you can purchase a certificate for your application from a trusted authority and use that instead.

<a name="Ex4Task6" />
#### Task 6 – Configuring a CNAME Entry for DNS Resolution (Optional) ####

When you deploy your application, Microsoft Azure assigns it a URL of the form _http://\[servicemytodo\].cloudapp.net_, where [_servicemytodo_] is the public name that you chose for your cloud service at the time of creation. While this URL is completely functional, there are many reasons why you might prefer to use a URL in your own domain to access the service. In other words, instead of accessing the application as _http://servicemytodo.cloudapp.net_, use your own organization's domain name instead, for example _http://servicemytodo.fabrikam.com_.

One way to map the application to your own domain is to set up a CNAME record in your own DNS system pointing at the host name in Azure. A CNAME provides an alias for any host record, including hosts in different domains. Thus, to map the application to the _fabrikam.com_ domain, you can create the following record in your DNS.

| **Organization's domain**  | **Alias** | **Application's domain**   |
|----------------------------|-----------|----------------------------|
| servicemytodo.fabrikam.com | CNAME     | servicemytodo.cloudapp.net |

The procedure for doing this varies depending on the details of your DNS infrastructure. For external domain registrars, you can consult their documentation to find out the correct procedure for setting up a CNAME. For additional information on this topic, see [Custom Domain Names in Microsoft Azure](http://blog.smarx.com/posts/custom-domain-names-in-windows-azure).
As an example, this task briefly shows how you set up an alias using Microsoft DNS on Windows Server 2008. 

>**Note:** Windows DNS Server should be installed on the Windows Server 2008. You can enable the DNS Server role on the Server Manager.

1. Open DNS Manager from **Start | Administrative Tools | DNS**.

1. In DNS Manager, expand the **Forward Lookup Zones** node, then right-click the zone where you intend to set up the alias and select **New Alias (CNAME)**.Notice that if you do not have any zone, you must create one prior the alias creation.
 
	![Updating a lookup zone to create an alias](./Images/creating-alias.png?raw=true "Updating a lookup zone to create an alias")
	
	_Updating a lookup zone to create an alias_

1. In the **New Resource Record** dialog, enter the alias name that you would like to use to access the application hosted in Azure, for example, _servicemytodo_. Then, type in the fully qualified domain name of your application that Azure assigned to your application, for example, _\[servicemytodo\].cloudapp.net_. Click **OK** to create the record.
 
	![Creating an alias for the myTODO application in Azure](./Images/creating-alias-app.png?raw=true "Creating an alias for the myTODO application in Azure")
	
	_Creating an alias for the myTODO application in Azure_

1. In the DNS Manager console, review the contents of the updated zone to find the newly created CNAME record.
 
	![Updated lookup zone showing the new alias for the application](./Images/lookup-zone-alias.png?raw=true "Updated lookup zone showing the new alias for the application")
	
	_Updated lookup zone showing the new alias for the application_

1. Now, open a command prompt and type the following command to verify that the alias is set up correctly and maps to the address of the application in Microsoft Azure. 

	<!--mark: 1-->
	````CommandPrompt
	nslookup <youralias>
	````
 
	![Verifying the domain alias](./Images/command-prompt-alias-verification.png?raw=true "Verifying the domain alias")

	_Verifying the domain alias_
	

	You will now be able to access the application using the alias.

---

<a name="NextSteps" />
## Next Steps ##

To learn more about deploying Cloud Services in Microsoft Azure, please refer to the following articles:

**Technical Reference**

This is a list of articles that expand on the technologies explained in this lab:

- [Use the Microsoft Azure SDK Tools to Package, Run, and Deploy an Application](http://aka.ms/Jb4mkm): explores the tools in the Microsoft Azure SDK to run, test, debug, and fine-tune your application before you deploy it as a cloud service to Microsoft Azure.

- [How to Monitor Cloud Services](http://aka.ms/Ifhdz0): explains how to monitor key performance metrics for your cloud services in the Microsoft Azure Management Portal.

- [Debugging Cloud Services](http://aka.ms/Leav14): explores the different techniques provided by the Microsoft Azure Tools for Microsoft Visual Studio and the Microsoft Azure SDK to debug a Microsoft Azure application.

- [Set Up a Remote Desktop Connection for a Role in Microsoft Azure](http://aka.ms/Hy6l4h): after you create a cloud service that is running your application, you can remotely access a role instance to configure settings in the virtual machine or troubleshoot issues.

- [Run Startup Tasks in Microsoft Azure](http://aka.ms/Aoxmbq): you can use startup tasks to perform operations before a role starts. Operations that you might want to perform include installing a component, registering COM components, setting registry keys, or starting a long running process.

- [Configure an SSL Certificate on an HTTPS Endpoint](http://aka.ms/Tydm1x): enumerates the steps to add an HTTPS endpoint to a role in your Microsoft Azure service and associate it with an SSL certificate.

**Development**

This is a list of developer-oriented articles related to deploying Cloud Services in Microsoft Azure:

- [Microsoft Azure Management Cmdlets Reference](http://aka.ms/G9j3a3): describes the cmdlet in the .7.1 version of the Microsoft Azure PowerShell module. Microsoft Azure PowerShell is the module for Windows PowerShell that you can use to control and automate the deployment and management of your workloads in Microsoft Azure.

---

<a name="Summary" />
## Summary ##

By completing this hands-on lab, you learnt how to create a storage account, a SQL database and a cloud service at the Microsoft Azure Management Portal. Using the management portal, you deployed a service package that contained the application binaries, configured its storage and defined the number of instances to run. 

You also saw how to achieve this programmatically using the Service Management API and in particular, how to use the Microsoft Azure Service Management cmdlets to deploy, update, and manage applications using Windows PowerShell.

As a developer, you saw how to use Microsoft Azure Tools in Visual Studio to publish your solution in the background, while you continue with your development tasks.
Finally, you learnt how to upload certificates using the management portal and use SSL to secure your Microsoft Azure application.
