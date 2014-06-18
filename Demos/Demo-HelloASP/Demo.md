<a name="HelloASP" />
# Classic ASP and FTP - Hello ASP Web Site #
---

<a name="Overview" />
## Overview ##

In this demo, you will create a Microsoft Azure Web Site, and get an ASP Legacy page running in the cloud using an FTP client.

<a id="goals" />
### Goals ###
In this demo, you will see:

1. How easy it is to create a new Web Site in Microsoft Azure.

1. How to get an existing ASP Web Site working in just a few minutes by uploading it with an FTP client.

<a name="technologies" />
### Key Technologies ###

- [Microsoft Azure Websites](https://www.windowsazure.com/en-us/home/scenarios/web-sites/)

---

<a name="Demo" />
## Demo ##

This demo is composed of the following segments:

1. [Create a New Web Site Hosted in Microsoft Azure](#segment1).
1. [Upload an ASP page to the Microsoft Azure Web Site using an FTP](#segment2).
1. [Set the ASP page as the Web Site Default Document](#segment3).

<a name="segment1" />
### Create a New Web site Hosted in Microsoft Azure ###

> **Speaking Point**
>
> During this demo we are going to create a Microsoft Azure Web Site.
>
> Lets start by opening Internet Explorer and accessing the Microsoft Azure Management Portal.

1. Open **Internet Explorer**, go to the [Microsoft Azure Management portal](https://manage.windowsazure.com) and sign in using your **Microsoft Account** credentials associated with your subscription.

	> **Speaking Point**
	>
	> Thanks to the Quick Create option from the Microsoft Azure Management Portal, we can now get a Web Site working in the cloud in just seconds.

1. Click **Compute**, **Web Site** and then **Quick Create** on the command bar. Provide an available URL (e.g. _hello-world-asp_) for the new web site and click **Create Web Site**.

	![Creating a new Web Site using Quick Create ](Images/creating-a-new-web-site-using-quick-create-op.png?raw=true "Creating a new Web Site using Quick Create")

	_Creating a new Web Site using Quick Create_

1. Once the Web Site is created, click the link under the **URL** column to make sure it is working.

	![Going to the new Web Site](Images/browsing-to-new-site.png?raw=true "Going to the new web site")

	_Going to the new Web Site_

	![Web site running](Images/web-site-running.png?raw=true "Web site running")

	_Web site running_

<a name="segment2" />
### Upload an ASP page to the Microsoft Azure Web Site using an FTP ###

> **Speaking Point**
>
> Next, we are going to get a basic ASP page working in the cloud, uploading it with an FTP.
>
> Lets go to the dashboard panel of the Web Site we have just created.

1. Back in the Microsoft Azure Management Portal, click your Web Site name, under the **Name** column to access the Dashboard.

	![Accessing the Web Site Dashboard](Images/accessing-the-web-site-dashboard.png?raw=true "Accessing the Web Site Dashboard")

	_Accessing the Web Site Dashboard_

	> **Speaking Point**
	>
	>  On the Dashboard we can see useful information about the Web Site such as access reports, hardware usage and database access.
	>
	> We also can find multiple options for uploading content to the Websites, including GIT, TFS and FTP.

1. Show the information displayed on the Web Site Dashboard.

	![Web Site Dashboard](Images/web-site-dashboard.png?raw=true "Web Site Dashboard")

	_Accessing the Web Site Dashboard_

	> **Speaking Point**
	>
	> To access the Web Site with an FTP, we need to specify the deployment credentials we will be using.

1. Click **Reset deployment credentials** in the **Quick Glance** section.

	![Resetting Deployment Credentials](Images/reset-deployment-credentials.png?raw=true "Resetting Deployment Credentials")

	_Resetting Deployment Credentials_

1. Enter a **User Name** and **Password** for the FTP.

	![Setting up FTP Credentials](Images/setting-ftp-credentials.png?raw=true "Setting up FTP Credentials")

	_Setting up FTP Credentials_

	> **Speaking Point**
	>
	> For this demo we are going to access the host with the File Explorer FTP connection, however you may use the FTP client of your preference.

1. Click the URL under **FTP Hostname** 

	![Opening Web Site FTP](Images/open-ftp-url.png?raw=true "Opening Web Site FTP")

	_Opening Web Site FTP_

1. Insert the FTP credentials previously specified.

	![Inserting FTP Credentials](Images/ftp-credentials-prompt.png?raw=true "Inserting FTP Credentials")

	_Inserting FTP Credentials_

1. Once in the FTP browser, click **Alt** and click **View | Open FTP Site in File Explorer**.

	![Opening FTP in File Explorer](Images/open-ftp-in-explorer.png?raw=true "Opening FTP in File Explorer")

	_Opening FTP in File Explorer_

1. If prompted for credentials, insert them again to access using File Explorer.

	![File Explorer FTP Credentials](Images/windows-explorer-ftp-credentials.png?raw=true "File Explorer FTP Credentials")

	_File Explorer FTP Credentials_

1. Open **/site/wwwroot/** folder.

	> **Speaking Point**
	>
	> Here we can see the Web Site content.
	>
	> Lets create a new basic ASP page.

1. Open **notepad** and write the following code:

	````HTML
	<%
	Response.Write "<html><body>"
	Response.Write "Hello world!"
	Response.Write "</body></html>"
	%>
	````

1. Name the file _Hello.asp_, save it to Desktop, and move it inside the FTP folder.

	![Hello.asp file inside the FTP](Images/helloasp-inside-ftp.png?raw=true "Hello.asp file inside the FTP")

	_Hello.asp file inside the FTP_

1. Switch to Internet Explorer and go to your Web Site URL. Then add _/hello.asp_ at the end of the root URL.

	![Hello.asp on the Cloud](Images/helloasp-on-the-cloud.png?raw=true "Hello.asp on the Cloud")

	_Hello.asp on the Cloud_

<a name="segment3" />
### Set the ASP page as the Web Site Default Document ###

> **Speaking Point**
>
>  We are now going to set up the hello.asp page as the Default Web Site page, so that when we go to the root URL our page is displayed.

1. Switch to the Microsoft Azure Management Portal and click **Configure** from the Web Site top menu.

	![Web Site Configure Options](Images/website-configure.png?raw=true "Web Site Configure Options")

	_Web Site Configure Options_

1. Scroll down to the **Default Documents** section and add **hello.asp** as a new default document.

	![Hello.asp as the Default Document](Images/helloasp-as-default-document.png?raw=true "Hello.asp as the Default Document")

	_Hello.asp as the Primary Default Document_

1. Click **Up Arrow** to make _hello.asp_ page the default document before _hostingstart.html_.

	![Hello.asp as the Primary Default Document](Images/helloasp-as-primary-default-document.png?raw=true "Hello.asp as the Primary Default Document")

	_Hello.asp as the Primary Default Document.png_

1. Click **Save** in the bottom menu to save the changes made.

	![Save Default Documents List](Images/save-default-documents.png?raw=true "Save Default Documents List")

	_Save Default Documents List_

1. Go to the Web Site root URL and confirm that _Hello.asp_ is the page displayed.

	![Hello.asp as the Default Page](Images/helloasp-default-page.png?raw=true "Hello.asp as the Default Page")

	_Hello.asp as the Default Page_

---

<a name="summary" />
## Summary ##

In this demo, you saw how to deploy your Web Site to the cloud with Microsoft Azure, and how easy it is to upload your own Web Site using an FTP client.
