<a name="title" />
# Websites - CloudSurvey#

---

<a name="Overview" />
## Overview ##

This demonstration shows how to create a Web Site using the Azure command-line tool, enabling it for Git deploy and finally deploy it to Azure using the Git commands.

> **Note:** In order to run through this complete demo, you must have network connectivity and a Microsoft account.

<a id="goals" />
### Goals ###
In this demo, you will see how to:

1. Create a Web Site with Git deploy enabled using the Azure command-line tool.
1. Configure a Git repository to associate it to an Azure Web Site.
1. Deploy a Web Site to Azure using Git.
1. View the deployments history in the management portal.

<a name="prerequisites" />
### Prerequisites ###

This demo uses the following technologies:

- [Git Source Control](http://git-scm.com/)
- [Node.js](http://nodejs.org/#download)
- Microsoft Azure Command-Line Tools

	> **Note:** If you do not have Microsoft Azure Command-Line Tools installed, open a command prompt with administrator privileges and run the following command:
	> 
	> `npm install azure-cli -g`
	> 
	> By using -g, Microsoft Azure Command-Line Tools will install on your machine globally. That means, you will be able to execute azure commands from any location.

<a name="setup" />
### Setup and Configuration ###

To run the demo, you need to have configured a SQL Database in Azure and install and configure the Azure Command-Line tool.

1. Run the **Setup.Local.cmd** script located at the **\Source** folder. This script will perform the following tasks:
	* Copy the demo solution to a working directory.
	* Configure the SQL Azure connection string in the demo solution.
	* Install Microsoft Azure SDK for Node.js.
	* Download and import the publish profile file from your Azure account.

<a name="segment1" />
### Publishing an application into a Microsoft Azure Web Site ###

1. Open a Git Bash command line and CD to the folder where you have the CloudSurvey application (in this case *c:/projects/cloudsurvey*).

	![Git Bash command line](Images/git-bash-command-line.png?raw=true "Git Bash command line")

	_Git Bash command line_

	> **Speaking point:**
	> To create our Azure Web Site, we are going to use the Azure Command Line Tool, that runs under Node.js, so it perfectly works in Windows, Mac and Linux.
	> 
	> It is really simple to use, just by writing **Azure** in a command line console we should get it working.

1. Execute the **azure** command.

	![Azure Git Bash command line](Images/azure-git-bash-command-line.png?raw=true "Azure Git Bash command line")

	_Azure Git Bash command line_

	> **Speaking point:**
	> Let's run the command to create an Azure Web Site and specify, for this case, that we want to create a Git repository within that Web Site.
	>
	> This way, we don't need to run additional commands to initialize the Git repository or add the Git remote, since these two tasks will be done automatically by the command line tool.

1. Run the following command to create the Microsoft Azure hosted site.

	```CommandPrompt
	azure site create --git
	```

	Provide a site name when prompted, for example, _CloudSurveyApp_.

	![Creating a new Web Site using the Command-Line Tool](Images/new-web-site-cli.png?raw=true "Creating a new Web Site using the Command-Line Tool")

	_Creating a new Web Site using the Command-Line Tool_

	> **Note:** If you experience any issues while executing this step, you can create the Web Site and configure the Git repository as explained in <a href=https://www.windowsazure.com/en-us/develop/nodejs/tutorials/create-a-website-(mac)>this link</a>.

	> **Speaking point:**
	> Now let's add the files to the Git repository and push them, in order to get them running on the cloud.

1. Go back to the command prompt and execute the following commands. When prompted, provide your deployment credentials.

	```CommandPrompt
	git add .
	git commit -m "initial commit"
	git push azure master
	```
	![Pushing the site files](Images/push-site.png?raw=true "Pushing the site files")

	_Pushing the site files_

	> **Speaking point:**
	> Let's check that the Git deployment was successful, by opening the Azure Management Portal and browsing to the Web Site Deployments page.

1. Run the following command to open the site in the Microsoft Azure Management portal and provide your Microsoft Account credentials associated with the subscription to sign in.

	```CommandPrompt
	azure site portal
	```

1. Click your site in the Web Site list and open the **Deployments** page of the site. Check out the latest deployment.

	![Web Site deployments](Images/site-deployments.png?raw=true "Web Site deployments")

	_Web Site deployments_

	> **Speaking point:**
	> Let's check now that the website is actually up and running.

1. Execute the following command to browse to the web site.

	```CommandPrompt
	azure site browse
	```

	![Published web site](Images/website-working-cli.png?raw=true "Published web site")

	_Published web site_

	> **Note:**
	> You can create a new user in the application by browsing to: _/Account/Register_

---

<a name="summary" />
## Summary ##

In this demo, you saw how to create a Web Site using the Azure command-line tool, enable it for Git deploy and finally deploy it to Azure using the Git commands.
