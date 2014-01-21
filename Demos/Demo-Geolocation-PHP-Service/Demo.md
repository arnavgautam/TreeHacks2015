<a name="title" />
# Creating a Mobile Geolocation Backend in PHP with Windows Azure Web Sites #

---

<a name="Overview" />
## Overview ##

This demonstration shows how to create a PHP backend for a mobile application.  In this case, the website will run in Windows Azure Website and will expose endpoints for finding tagged geocoordinates within a specific radius as well as adding new geocoordinates.

Widows Azure Websites enables developers to quickly get up and running with websites.  Websites may be developed in ASP.NET, Node.js and PHP.  In addition, websites may use SQL or MySQL for data storage.  Deployment can be accomplished in several ways including TFS, FTP, and GIT.

<a id="goals" />
### Goals ###
In this demo, you will see:

1. How easy is to create a new Web Site in Windows Azure.

1. How to add a new table to a MySQL database.

1.  How to crate a SQL Azure Storage account.

1. How to get working an existing PHP in a few minutes uploading it with GIT.

<a name="technologies" />
### Key Technologies ###

- [Windows Azure Web Sites](https://www.windowsazure.com/en-us/home/scenarios/web-sites/)
- [Windows Azure Cloud Storage](https://www.windowsazure.com/en-us/home/features/data-management/)

---

<a name="Demo" />
## Demo ##

This demo is composed of the following segments:

1. [Create a New Web Site Hosted in Windows Azure](#segment1).
1. [Preparing the MySQL Database](#segment2).
1. [Creating a Cloud Storage account](#segment3).
1. [Upload an existing PHP Website into the Windows Azure Web Site using GIT](#segment4).
1. [Configuring and updating the PHP Website](#segment5).

<a name="segment1" />
### Create a New Web Site Hosted in Windows Azure ###

> **Speaking Point**
>
> During this demo we're going to create a Windows Azure Web Site.
>
> Lets start by opening the browser and accessing to the Windows Azure Management Portal.

1. Open browser, go to the [Windows Azure Management portal](https://manage.windowsazure.com) and sign in using your **Microsoft Account** credentials associated with your subscription.

	> **Speaking Point**
	>
	> Thanks to the Quick Create option from the Windows Azure Management Portal, we can now get a Web Site working on the cloud just in seconds.

1. Click **New | Web Site | Create with Database** on the command bar.

	![Creating a new Web Site with a database ](Images/createWithDatabase.png?raw=true "Creating a new Web Site with a database")

	_Creating a new web site with a database_

1.  Provide an available URL (e.g. geolocation-test), choose to "Create a new MySQL database" and click Next.

	![Details on a new Web Site with a new database](Images/createWebsiteOne.png?raw=true "Details on a new Web Site with a new database")

	_Details for a new web site with a new database_

1.  Enter an available name for the database (e.g.  geolocationtest) or accept the default.  Agree to the terms and click the checkmark to continue.

	![Entering database details](Images/newDatabase.png?raw=true "Entering database details")

	_Entering database details_

1. Once the Web Site is created, click the link under the **URL** column to check that it is working.

	![Browsing to the new web site](Images/clickOnWebsite.png?raw=true "Browsing to the new web site")

	_Browsing to the new web site_

	![Web site running](Images/newWebsite.png?raw=true "Web site running")

	_Web site running_

<a name="segment2" />
### Preparing the MySQL Database ###

> **Speaking Point**
>
> Next, we're going to connect to our MySQL database and add a new table to it.
>
> Let's go to the dashboard panel of the Web Site we have just created.

1. Back to the Windows Azure Management Portal, click your Web Site name, under the **Name** column to access the Dashboard.

	![Accessing the Web Site Dashboard](Images/goToDashboard.png?raw=true "Accessing the Web Site Dashboard")

	_Accessing the Web Site Dashboard_

	> **Speaking Point**
	>
	> Once we're at the dashboard, we'll go to the configure tab where we can see the configuration information for our database.

1. At the dashboard, click on the Configure tab near the top.

	![Accessing the configuration information](Images/clickOnConfigure.png?raw=true "Accessing the configuration information")

	_Accessing the Configuration information_

1. Scroll down until you get to connection strings.  Copy this information for later use.

	![Accessing database configuration details](Images/databaseConfigurationInfo.png?raw=true "Accessing database configuration")

	_Accessing database configuration_

1.  Open a terminal (or command prompt if using Windows).

1.  Navigate to a directory where you can access MySQL tools (i.e. /usr/local/mysql/bin on OSX).  

1.  Start MySQL (i.e. run ./mysql on OSX) with parameters --host, --username, and --password followed by the databasename.  For example:

	_./mysql --host=us-cdbr-azure-east-a.cloudapp.net --user=myuser --password=mypassword mydatabase_

1.  Execute the following query to create the table (this query can also be found in the sqlcreate.sql file in the source directory):

	````C#
CREATE TABLE geodata (
  Id char(36) NOT NULL,
  Type smallint(6) DEFAULT NULL,
  Description varchar(200) DEFAULT NULL,
  Url varchar(400) DEFAULT NULL,
  Location point DEFAULT NULL,
  PRIMARY KEY (Id)
);
````

1.  Ensure your table was created by running the following command and ensuring that the "geodata" table is present:
	`show tables;`

> **Speaking Point**
>
> Our database now has the table we're going to use for the rest of this demo.

<a name="segment3" />
### Creating a Cloud Storage Account ###

> ***Speaking Point***
>
>The mobile applications we're going to create to connect to this web site will be storing images and videos tied to geopoints.  To store those images and videos, we're going to create a Windows Azure Storage account.

1.  Return to the Windows Azure Portal.

1.  Go to New and select Storage.  Enter an available name (e.g. geostorage).

	![Creating a New Storage Account](Images/newStorageOne.png?raw=true "Creating a New Storage Account")

	_Creating a New Storage Account_

1.  Click "Create Storage Account".

1.  After your storage account is created, click manage keys at the bottom.

	![Manage Storage Keys](Images/storageManageKeys.png?raw=true "Manage Storage Keys")

	_Manage Storage Keys_

1.  Make note of the primary access key.  This along with the name of the storage account will be used later.

	![Storage Keys](Images/storageAccessKeys.png?raw=true "Storage Keys")

	_Storage Keys_

<a name="segment4" />
### Adding Depedent Libraries and Uploading an existing PHP Website into the Windows Azure Web Site using GIT ###

> **Speaking Point**
>
> Before we can push our site up to Windows Azure, we need to pull down a couple third party libraries.  This PHP site was built using Silex which is a micro-PHP framework.  Silex allows the site to easily set up web service end points.  Let's go get that library now.

1.  Open a browser and go to http://silex.sensiolabs.org/

1.  Click the download link.

1.  Go to the bottom of the page and download the PHAR file.

1.  Place the PHAR file in the following folder structure: /source/vendor/Silex/silex.phar

	> **Speaking Point**
	>
	> Now let's get our PHP site ready to push to Windows Azure Websites.
	>
	> Lets go to the dashboard panel of the Web Site we have just created.

1. Back to the Windows Azure Management Portal, click your Web Site name, under the **Name** column to access the Dashboard.

	![Accessing the Web Site Dashboard](Images/goToDashboard.png?raw=true "Accessing the Web Site Dashboard")

	_Accessing the Web Site Dashboard_

	> **Speaking Point**
	>
	>  We can see at the Dashboard, a set of useful information about the Web Site. Such as access reports, hardware usage and database access.
	>
	> We also can find here multiple options to upload content to the Web Sites. We can do it using GIT, TFS or FTP.

	> **Speaking Point**
	>
	> To access the Web Site with an GIT, we need to specify the deployment credentials we will be using.

1. Click **Reset deployment credentials** from the **Quick Glance** section.

	![Resetting Deployment Credentials](Images/resetCredentials.png?raw=true "Resetting Deployment Credentials")

1. Enter a **User Name** and a **Password**.

	![Setting up GIT Credentials](Images/newCredentials.png?raw=true "Setting up GIT Credentials")

	_Setting up GIT Credentials_

	> **Speaking Point**
	>
	> With that done, we can now set up GIT.  

1. Click "Setup GIT Publishing" under **Quick Glance** 

	![Setup GIT Publishing](Images/setupGitPublishing.png?raw=true "Setup GIT Publishing")

	_Setup GIT Publishing_

1.  After a moment, GIT should be set up and you will see a notification that the GIT repo is ready.

	![Git is ready](Images/gitIsReady.png?raw=true "Git is ready")

	_GIT is Ready_

1.  After that image, you will see instructions for commiting your files locally.

	![Local GIT Commit instructions](Images/commitLocalFiles.png?raw=true "Local GIT Commit instructions")

	_Local GIT commit instructions_

1.  Open a terminal or command prompt.

1.  Navigate into this demo's source directory.

1.  Enter the command to initialize a git repository in your source directory:
	`git init`

1.  Add all of your local files:
	`git add .`

1.  Commit all of your local changes:
	`git commit -m "inital commit"`

	> **Speaking Point**
	>
	> Now that our files have been committed to our local git repo, we can follow the next set of instructions.  

1.  Return to the portal's GIT instructions.  See the third step to add a remote branch.

	![Adding a remote repository](Images/addGitRemote.png?raw=true "Adding a remote repository")

	_Adding a remote repository_

1.  Return to the terminal and add a remote repo for your local GIT repository:
	`git remote add azure https://azurepreviewlive@geolocationtest.scm.azurewebsites.net/geolocationtest.git	`

1.  Push all your changes to Windows Azure:
	`git push azure master`

1.  Return to your website in the browser and refresh.  The site should load.

> **Speaking Point**
>
> Now that the site is running, we just need to complete one more step.  That is to configure our site to use our MySQL database and our Windows Azure storage account.Let's do that and update Windows Azure.

<a name="segment5" />
### Configuring and updating the PHP Website ###

> **Speaking Point**
>
> We're now going to configure out site to use our MySQL Database.

1.  If you copied the database connection information, proceed to step 4, otherwise continue to 2.

1.  Return to the site configuration by going to the configuration tab again.  

	![Accessing the configuration information](Images/clickOnConfigure.png?raw=true "Accessing the configuration information")

	_Accessing the Configuration information_

1. Scroll down until you get to connection strings.  Copy this information for later use.

	![Accessing database configuration details](Images/databaseConfigurationInfo.png?raw=true "Accessing database configuration")

	_Accessing database configuration_

1.  Navigate in your source code to /source/src/geo/ and open GeoCode.php.

1.  Edit the following database configuration information to match what you copied from the configuration tab in the portal:

	````C#
//Local
    private $db_server = 'localhost';
    private $db_user   = 'phptestuser';
    private $db_password = 'phptestuser';
    private $db_name     = 'shorty';
````

1.  Now navigate to /source/src/ and open app.php.

1.  Locate the following lines and enter the account name and key from your storage account:


	````C#
//Define our storage account name and keys
define("STORAGE_ACCOUNT_NAME", "accountname");
define("STORAGE_ACCOUNT_KEY", "accountkey");
````

1.  Return to your terminal / command prompt.

1.  Use GIT to add your changes:
	`git add .`

1.  Commit your changes:
	`git commit -m "Updated database configuration"`

1.  Push your changes to Windows Azure:
	`git push azure master`

---

<a name="summary" />
## Summary ##

In this demo, you saw how to create a web site on Windows Azure, how to push your source code up using GIT, and how to configure and connect to your MySQL database.