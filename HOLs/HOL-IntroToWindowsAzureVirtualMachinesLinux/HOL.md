<a name="handsonlab" />
# Introduction to Windows Azure Virtual Machines (Linux) #

---

<a name="Overview" />
## Overview ##

Using Windows Azure as your IaaS platform will enable you to create and manage your infrastructure quickly, provisioning and accessing any host ubiquitously. Grow your business through the cloud-based infrastructure, reducing the costs of licensing, provisioning and backup.

In this hands-on lab, you will learn how to use the Windows Azure IaaS platform to provision Linux based Virtual Machines in the cloud and manage it remotely. 

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to: 

- Create Linux virtual machines in Windows Azure
- Install and configure an Apache web server with MySQL 
- Install and configure Drupal CMS

<a name="Prerequisites" />
### Prerequisites ###

- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

> **Note:** This lab was designed to use **openSUSE** Linux distribution when creating the new Virtual Machine in Windows Azure.

---
 
<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating a Linux Virtual Machine in Windows Azure](#Exercise1)

1. [Installing and Configuring the Virtual Machine](#Exercise2)

1. [Installing and Configuring Drupal](#Exercise3)
 
Estimated time to complete this lab: **45 minutes**.

---

<a name="Exercise1" />
### Exercise 1: Creating a Linux Virtual Machine in Windows Azure ###

In this exercise, you will learn how to provision a Linux Virtual Machine in the Azure portal.
	
<a name="Ex1Task1" />
#### Task 1 - Creating a New Linux Virtual Machine ####

1. Open Internet Explorer and browse [https://manage.windowsazure.com](https://manage.windowsazure.com) to enter the Windows Azure portal. Then, log in with your credentials.
1. In the menu located at the bottom, select **Compute** | **New Virtual Machine | From Gallery** to start creating a new virtual machine.
	 
	![Creating a new Virtual Machine](Images/creating-a-new-virtual-machine.png?raw=true)

	_Creating a new Virtual Machine_
 
1. In the **Virtual Machine operating system selection** page, click **Platform Images** on the left menu and select the **openSUSE** OS image from the list. Click the arrow to continue.	

	![Selecting openSUSE from the image list](Images/creating-a-vm-suse.png?raw=true)	
	 
	_Selecting openSUSE from the image list_

1. In the **Virtual Machine Configuration** page, enter a **Virtual Machine Name**. In the **Authentication** section, uncheck **Upload compatible ssh key for authentication** and check **Provide password**. Provide a password for the **New Password** and **Confirm Password** fields. Lastly, set the Virtual Machine **Size** to _Small_ and click **Next** to continue.

	![Configuring a Custom Virtual Machine](Images/creating-a-vm-configuration.png?raw=true)	
	 
	_Creating a Virtual Machine - Configuration_
 
	>**Note:** It is suggested to use secure passwords for admin users, as Windows Azure virtual machines could be accessible from the Internet knowing just their DNS.

	>You can also read this document on the Microsoft Security website that will help you select a secure password:  [http://www.microsoft.com/security/online-privacy/passwords-create.aspx](http://www.microsoft.com/security/online-privacy/passwords-create.aspx)
 
1. In the **Virtual Machine Mode** page, select **Standalone Virtual Machine**, enter the **DNS Name**, you can automatically generate a new Storage Account or select one you already own. Then, select the **Region/Affinity group/Virtual Network** value  and select the **subscription**. Click the **right arrow** to continue. 

	![Configuring a Custom VM, VM Mode](Images/creating-a-vm-vm-mode.png?raw=true)
	 
	_Creating a Virtual Machine - Virtual Machine Mode_
 
1. In the **Virtual Machine Options** page, click the button to create a new Virtual Machine.

	![Creating a VM - VM Options](Images/creating-a-vm--vm-options.png?raw=true "Creating a Virtual Machine - Virtual Machine Options")

	_Creating a Virtual Machine - Virtual Machine Options_
 
1. In the **Virtual Machines** section, you will see the Virtual Machine you created with a _provisioning_ status. Wait until it changes to _Running_ in order to continue with the following step.

	![Creating Linux VM](Images/creating-linux-vm.png?raw=true)
	 
	_Creating Linux Virtual Machine_

1. Now, you will create the endpoints required to manage the Virtual Machine. To do this, select the Virtual Machine to go to the **Dashboard** page and then click **Endpoints**.

1. Click **Add Endpoint**, select **Add Endpoint** option and then click the **right arrow** to continue.

	![Adding a new Endpoint](Images/adding-a-new-endpoint.png?raw=true "Adding a new Endpoint")

	_Adding a new Endpoint_

1. In the **Specify the details of the endpoint** page, set the **Name** to _webport_, the **Protocol** to _TCP_ and the **Public Port** and **Private Port** to _80_.

	![New Endpoint Details](Images/new-endpoint-details.png?raw=true "New Endpoint Details")

	_Specify Endpoint Details_
	
	> **Note:** This endpoint enables the port 80 that will be used by the Apache Server in the following tasks.

<a name="Ex1Task2" />
#### Task 2 - Verification: Connecting to the Virtual Machine by using a SSH client ####

Now that you have provisioned and configured a Linux Virtual Machine, you will connect by using an SSH client.

>**Note:** You can download Putty, a free SSH client for Windows, here: [http://www.putty.org/](http://www.putty.org/)


1.	In the Windows Azure Portal, select the Linux Virtual Machine from the list to enter the **Dashboard**. Take note of the **SSH Details** field at the "quick glance" section, which is the public address you will use to access and connect to the virtual machine using the SSH client.

    ![SSH Endpoint](Images/ssh-endpoint.png?raw=true "SSH Endpoint")

    _SSH Endpoint_

1. Open the Putty client (or any other SSH client) and create a new connection to the Virtual Machine, using address and port from the previous step.

	![Connecting to the Linux Virtual Machine via Putty Client](Images/connecting-to-the-linux-vm-via-putty-client.png?raw=true)
	 
	_Connecting to the Linux Virtual Machine via Putty Client_

	> **Note:** You can verify the public port for the SSH connection in the **Endpoints** section of the Virtual Machine. When using Putty, make sure you enable TCP Keepalives and set it with a value greater than 5.
 
1. Use the Virtual Machine credentials to login.

	> **Note:** When completing the password, the cursor won't move

	![Logging in to the Linux Virtual Machine](Images/logging-in-to-the-linux-virtual-machine.png?raw=true)

	_Logging in to the Linux Virtual Machine_

1. Execute the following command to impersonate with **Administrator** rights.

	````Linux
	sudo su -
	````

	![Logging in with Administrator rights](Images/logging-in-with-administrator-rights.png?raw=true)

	_Logging in with Administrator rights_

---

<a name="Exercise2" />
### Exercise 2: Installing and Configuring the Virtual Machine ###

In this exercise, you will learn how to install and configure a Web Server in the Linux Virtual Machine by using a SSH client. First, you will install the Apache Web server and the MySQL database server by using Yast2 application. Then, you will configure the Virtual Machine and create an example database. 

>**Note:** If you have not run Exercise 1, make sure you have the following items ready before proceeding with Exercise 2:
  
> - A Linux Virtual Machine created in Windows Azure Portal.
> - A TCP Endpoint enabled with private port 22.
> - A TCP Endpoint enabled in port 80.

<a name="Ex2Task1" />
#### Task 1 - Installing and Configuring Apache and MySQL ####

In this task, you will install and configure an Apache HTTP Server and MySQL Database Management System.

1. In the terminal, execute the following commands to install the required packages.

	````Linux
	zypper install -t pattern lamp_server
	````

	>**Note**: If you get the following prompt "Do you want to reject the key, trust temporarily, or trust always? [r/t/a/?]", press **A** and then **Enter**. Pattern Lamp server installs only the needed packages for Lamp server.

1. Install the following packages in order to run PHP, MySQL (MariaDB distribution) and Apache2 in openSUSE.

	````Linux
	zypper install apache2 apache2-doc apache2-mod_php5 apache2-example-pages mariadb php5-gd php-db php5-mysql php5-json php5-dom php5-mbstring 
	````

1. In order to start MySQL service, execute the following commands.

	````Linux
	systemctl enable mysql.service 
	systemctl start mysql.service
	````

1. Execute the following commands in order to start Apache too.

	````Linux
	systemctl enable apache2.service
	systemctl start apache2.service
	````

	![Starting Apache and MySQL service](Images/starting-apache-and-mysql-service.png?raw=true "Starting Apache and MySQL service")

	_Starting Apache and MySQL service_


#### Task 2 - Validating Apache and MySQL are running ####

In this task you will check the status of MySQL and Apache service.

1. If not open, open SSL client and connect to the Virtual Machine

1. Run the following command to check MySQL Service status
	
	````Linux
	service mysql status 
	````

	![MySQL Service Status](Images/mysql-service-status.png?raw=true "MySQL Service Status")

	_MySQL Service Status_

1. Run the following command to check Apache Service status
	
	````Linux
	service apache2 status 
	````

	![Apache Server Status](Images/apache-server-status.png?raw=true "Apache Server Status")

	_Apache Server Status_

1. Open **Internet Explorer** and browse the **DNS name** of the Virtual Machine to ensure apache is accessible through the Web.
	
	![Browser checking status](Images/browser-checking-status.png?raw=true "Browser checking status")

	_Apache working_

---

<a name="Exercise3" />
### Exercise 3 - Installing and Configuring Drupal ###

In this exercise, you will install and configure the Drupal CMS in the Linux virtual machine. At the end of the exercise, you will be able to host a Drupal CMS website. 

>**Note:** Before starting this exercise, make sure you have completed Exercise 2.

<a name="Ex3Task1" />
#### Task 1 - Installing and Configuring Drupal ####

In this task, you will install and configure a Drupal portal on your Windows Azure Linux Virtual Machine. Additionally, you will create an empty database to be used by Drupal CMS.

1. Open the root websites folder and create a folder named **Drupal** by executing the following.

	````Linux
	cd /srv/www/htdocs
	mkdir drupal
	cd drupal
	````

1.	Execute the following command to install **wget**.
	
	````Bash
	zypper install wget
	````

1. Download and extract **Drupal**.

	````Linux
	wget http://drupal.org/files/projects/drupal-7.22.tar.gz
	tar -xzf drupal-7.22.tar.gz --strip-components=1 
	````

	![Downloading Drupal](Images/downloading-drupal.png?raw=true)

	_Downloading Drupal_

1. Copy the **default.settings.php** file, located in the **sites/default** directory, and rename the copied file to **settings.php**. Additionally, give the web server write privileges to the configuration file.

	````Linux
	cp sites/default/default.settings.php sites/default/settings.php
	chmod a+w sites/default/settings.php
	chmod a+w sites/default
	````

	![Creating the configuration file and granting permissions](Images/creating-the-configuration-file-and-granting.png?raw=true)

	_Creating the configuration file and granting permissions_

1. To complete the installation, create an empty database for **Drupal** in **MySQL**. Execute the following:

	````Linux
	mysqladmin create drupaldb
	````

1. Execute **mysql** command. At the MySQL prompt execute the following query. Replace **username** and **password** with your user account.

	````Linux
	GRANT SELECT, INSERT, UPDATE, DELETE, CREATE, DROP, INDEX, ALTER, CREATE TEMPORARY TABLES, LOCK TABLES	
	ON drupaldb.*
	TO 'username'@'localhost' IDENTIFIED BY 'password';
	````

	![Granting permissions in MySQL](Images/granting-permissions-in-mysql.png?raw=true)

	_Granting permissions in MySQL_
	
1. Open Internet Explorer and locate the virtual machine DNS Name. Browse to _http://[YOUR-DNS-URL]/drupal_ to complete Drupal installation.
 
	![Opening Drupal for the first time](Images/opening-drupal-for-the-first-time.png?raw=true)

	_Opening Drupal for the first time_ 

	>**Note:**  You can find more details about Drupal configuration in the official documentation ([http://drupal.org/documentation/install/run-script](http://drupal.org/documentation/install/run-script)).

1. In the Set up Database page, enter the name of the database you have created in Task 1 (‘drupaldb’), and the **username** and **password**. 
	 
	![Opening Drupal for the first time(2)](Images/opening-drupal-for-the-first-time2.png?raw=true)

	_Opening Drupal for the first time_
 
1. In the **Configure site** website, enter a user name, an e-mail address and a password to create a new **site maintenance account**.

	![Configuring a Drupal account](Images/configuring-a-drupal-account.png?raw=true)
	 
	_Configuring a Drupal account_

1. Click the **Visit your new site** link to verify that the Drupal home page loads. 

	![Drupal CMS home page](Images/drupal-cms-home-page.png?raw=true)
	 
	_Drupal CMS home page_
	
---

<a name='Summary' />
## Summary ##

In this hands-on lab, you have learnt how to use the Windows Azure IaaS platform to provision Linux based Virtual Machines in the cloud and manage it remotely.