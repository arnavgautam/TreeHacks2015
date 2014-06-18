<a name="title" />
# Load Balancing and Availability Sets #

In this demo, you will demonstrate how easy it is to set up load-balancing between two Web servers. 

You will also demonstrate how to configure an Availability set for two Web servers to ensure high availability.

The demo starts by creating two new Windows Server 2012 VMs. After starting the provisioning, you must wait 4-5 minutes before the two servers are provisioned and running.

> **Note:** To avoid a pause in the demo or exiting back to slides while the Virtual Machines provision, it is strongly encouraged to provision at least the first VM of the load balanced set.

<a id="goals" />
### Goals ###
In this demo, you will see three things:

1. How to create Virtual Machines
1.	How to configure the Microsoft Azure Load Balancer
1.	How to configure Microsoft Azure Availability Sets

<a name="technologies" />
### Key Technologies ###

This demo uses the following technologies:

- [Microsoft Azure Management Portal] [1]

[1]: https://manage.windowsazure.com/

<a name="Demo" />
## Demo ##

1. In a Microsoft Azure PowerShell window, type the following commands:

	````PowerShell
	$svcname = "ITC-" + $(Get-Random -Minimum 10000 -Maximum 99999)
	$adminUser = "[YOUR-ADMIN-USER]"
	$adminPassword = "[YOUR-ADMIN-PWD]"
	$location = "[LOCATION]"

	$vmimage = "a699494373c04fc0bc8f2bb1389d6106__Windows-Server-2012-Datacenter-201301.01-en.us-30GB.vhd"

	$iisvm1 = New-AzureVMConfig -Name "iisvm1" -InstanceSize Small -ImageName $vmimage | Add-AzureProvisioningConfig -Windows -AdminUsername $adminUser -Password $adminPassword

	$iisvm2 = New-AzureVMConfig -Name "iisvm2" -InstanceSize Small -ImageName $vmimage | Add-AzureProvisioningConfig -Windows -AdminUsername $adminUser -Password $adminPassword

	New-AzureVM -ServiceName $svcname -VMs $iisvm1, $iisvm2 -Location $location
	````

	> **Note:** It will take a few minutes for the two new Virtual Machines to be provisioned and running.

	> **Speaking Point:**

	> We start by creating a random name for a cloud service. The name must be unique (<name>.cloudapp.net), so we use a random number to ensure it does not exist yet.

	> The image name is an Azure-provided **Windows Server 2012** image. You can find all existing image names by using the following PowerShell command:
_Get-AzureVMImage | ft ImageName_

	> The last three commands create a cloud service with two new VMs, based on the Windows Server 2012 image.

	> - **Note**: Make sure to update the **$adminUser**, **$adminPassword** and **$location**. Administrator username is not available.

	> - **Note**: Make sure the specified **$location** matches the datacenter that the storage account is created in. For example "West US".

	> - **Note**: These commands require a default storage account name. You can set that with the PowerShell command:
	Set-AzureSubscription "[subname]" 
	-CurrentStorageAccount "[storagename]"

1. Notice that you can follow the progresss of the PowerShell command execution on the screen.

	![Displaying VM Creation status in PS](./Images/Displaying-VM-Creation-status-in-PS.png?raw=true "Displaying VM Creation status in PS")

	> **Speaking Point:**

	> After the Virtual Machines are created, they will automatically start up. This will take around two minutes.

1. In the Azure portal, on the **Virtual Machines** page, click he **iisvm1** name.

	![Provisioning Virtual Machines](./Images/Provisioning-Virtual-Machines.png?raw=true "Provisioning Virtual Machines")

	> **Speaking Point:**

	> Even when the Virtual Machines are being starting, you are able examine their network properties.

1. On the iisvm1 page, examine the information under **Quick Glance**. Click the back button to go back to the **Virtual Machines** page.

	![Displaying DNS and IP information in an Azure VM](./Images/Displaying-DNS-and-IP-information-in-an-Azure-VM.png?raw=true "Displaying DNS and IP information in an Azure VM")

	> **Speaking Point:**

	> Notice the public IP address, and the internal (private) IP address of the Virtual Machine. 

	> Azure assigns the private IP address to the Virtual Machine, as soon as it is started.

1. On the Virtual Machines page, wait until both **iisvm1** and **iisvm2** are running. Select **iisvm1**, and then on the toolbar, click **Connect**. Open the RDP connection, and log on with the credentials you defined in Step 1.

	![Connecting to the IISVM1 VM](./Images/Connecting-to-the-IISVM1-VM.png?raw=true "Connecting to the IISVM1 VM")

	> **Speaking Point:**

	> First let's connect to the first server to enable IIS.

1. In iisvm1, open the the **Windows PowerShell** window,
and then run the command:

	````PowerShell
Install-WindowsFeature -Name  Web-WebServer
```

	![Installing IIS using PowerShell](./Images/Installing-IIS-using-PowerShell.png?raw=true "Installing IIS using PowerShell")

	> **Speaking Point:**

	> Let's use PowerShell to install the Web Server (IIS) on the first server. 

	> While the role is installed, we can go to the second server.

1. In the Azure portal, select **iisvm2**, and then on the toolbar, click **Connect**. Open the RDP connection, and log on with the credentials you defined in Step 1.

	![Connecting to the IISVM2 VM](./Images/Connecting-to-the-IISVM2-VM.png?raw=true "Connecting to the IISVM2 VM")

	> **Speaking Point:**
	
	> We also need to connect to the second server to enable IIS.

1. In iisvm2, in Server Manager, click **Add roles and features**. In the wizard, on the **Server Roles** page, select **Web Server (IIS)**, choose "**Add Features**", and keep clicking **Next** and then finally click **Install** to complete the wizard.

	![Adding roles to the VM using the Azure Portal](./Images/Adding-roles-to-the-VM-using-the-Azure-Portal.png?raw=true "Adding roles to the VM using the Azure Portal")

	> **Speaking Point:**

	> In the second server, let's use Server Manager to install the Web Server.

	> While the role installed, we can enable load-balancing on the two Virtual Machines.

1. In the Microsoft Azure PowerShell window, type the following commands:

	````Powershell
	Get-AzureVM -ServiceName $svcname -Name "iisvm1" | `
	Add-AzureEndPoint -Name "Web" `
		-Protocol "tcp" `
		-PublicPort 80 `
		-LocalPort 80 `
		-LBSetName "Web" `
		-ProbePort 80 `
		-ProbeProtocol "http" `
		-ProbePath "/" | `
	Update-AzureVM
	````

	> **Speaking Point:**

	> On the first server, we use Azure PowerShell to add the **Endpoint** for port 80. This will direct incoming traffic on a public port to a private port on the VM.
This is called **port forwarding**.

	> Notice that we provided a name for the Endpoint (**Web**), and a name for the load-balancing set (**Web**).

	> After this command, only the first server has the Endpoint defined. We also need to add the Endpoint to the second server.

1. In the Azure portal, on the Virtual machines page, click the **iisvm2** name. On the iisvm2 page, on the **Endpoints** tab, click the **Add Endpoint** icon.

	![Azure Portal - Adding an Endpoint](./Images/Azure-Portal---Adding-an-Endpoint.png?raw=true "Azure Portal - Adding an Endpoint")

	> **Speaking Point:**

	>Let's use the Azure portal to add the Endpoint to the second server.

1. In the wizard, select Load-balance traffic on existing endpoint. In the drop-down list box, select **Web**. Click the right arrow icon to continue.

	![Adding a Web Endpoint](./Images/Adding-a-Web-Endpoint.png?raw=true "Adding a Web Endpoint")

	> **Note:** if the **Web** entry does not appear, then the endpoint has not finished configuring yet on iisvm1.

	> **Speaking Point:**

	> Instead of adding an independent endpoint, we will load-balance on an existing endpoint, with load-balancing set name **Web**.

1. Specify the following details:
	
	| Field | Value |
	|--------|--------|
	| Name | **Web** |
	| Protocol | **TCP** (grayed out) |
	| Public Port | **80** (grayed out) |
	| Private Port | **80** |

	Click the check mark icon  to continue.

	![Editing Endpoint Settings](./Images/Editing-Endpoint-Settings.png?raw=true "Editing Endpoint Settings")

	![Propagating Endpoint configuration to VMs](./Images/Propagating-Endpoint-configuration-to-VMs.png?raw=true "Propagating Endpoint configuration to VMs")

	> **Speaking Point:**

	> When specifying the load-balanced endpoint, notice that the Protocol (TCP) and the Public Port (80) cannot be changed.
       
	> Also notice that you can change the private port if the other server was listening to web traffic on a separate port.  Ideally this will be the same port and this is what we are using, so we will choose port 80 for the private port.
	
	> As soon as you add it, you can see that it says "yes" under load balanced for this endpoint.

	> Before testing the new configuration, let's update the IIS header inside the VMs so we know which web server we are hitting when we load balance.

1. Switch to the existing RDP connection to **iisvm1**. Close the Windows PowerShell window.

	![Web Server IIS Successfully installed](./Images/Web-Server-IIS-Successfully-installed.png?raw=true "Web Server IIS Successfully installed")

	> **Speaking Point:**

	> The Web Server (IIS) role is successfully installed on the first server.

1. In iisvm1, in the **C:\inetpub\wwwroot** folder, right-click **iisstart.htm**, and then click **Open with/Notepad**.

	![Finding iisstart.htm on disk on IISVM1](./Images/Finding-iisstart.htm-on-disk-on-IISVM1.png?raw=true "Finding iisstart.htm on disk on IISVM1")

	> **Speaking Point:**

	> In order to recognize to which server we connect, we will edit the **iisstart.htm** file.

1. At the end of the iistart.htm file, after the **\<body\>** tag, add:
**\<H3\>IISVM1\</H3\>**. Close and save the iisstart.htm file.

	![Editing iisstart.htm on IISVM1](./Images/Editing-iisstart.htm-on-IISVM1.png?raw=true "Editing iisstart.htm on IISVM1")

	> **Speaking Point:**

	> This will display **IISVM1** on the page, when connecting to the first Web server.

1. Switch to the existing RDP connection to **iisvm2**. Click **Close** to close the Roles and Features wizard.

	![Finishing IIS installation using Feature Wizard](./Images/Finishing-IIS-installation-using-Feature-Wizard.png?raw=true "Finishing IIS installation using Feature Wizard")

	> **Speaking Point:**

	> The Web Server (IIS) role is successfully installed on the second server.

1. In iisvm2, in the **C:\inetpub\wwwroot** folder, right-click **iisstart.htm**, and then click **Open with/Notepad**.

	![Finding iisstart.htm on disk on IISVM2](./Images/Finding-iisstart.htm-on-disk-on-IISVM2.png?raw=true "Finding iisstart.htm on disk on IISVM2")

	> **Speaking Point:**

	> We will make the same change to the iistart.htm file on this server.

1. At the end of the iistart.htm file, after the **\<body\>** tag, add:
**\<H3\>IISVM2\</H3\>**. Close and save the iisstart.htm file.

	![Editing iisstart.htm on IISVM2](./Images/Editing-iisstart.htm-on-IISVM2.png?raw=true "Editing iisstart.htm on IISVM2")

	> **Speaking Point:**

	> This will display **IISVM2** on the page, when connecting to the second Web server.

1. In the Azure console, on the Virtual machine pages, click the **iisvm1** name. On the dashboard page, copy the **DNS** name.

	![Locating DNS name of a VM in the Azure Portal](./Images/Locating-DNS-name-of-a-VM-in-the-Azure-Portal.png?raw=true "Locating DNS name of a VM in the Azure Portal")

	> **Speaking Point:**

	> Under Quick glance, notice the DNS name. For example: **itc-32548.cloudapp.net**.

1. In Internet Explorer, browse to **http://itc-NNNN.cloudapp.net**

	![Iisstart.htm page on IISVM1](./Images/Iisstart.htm-page-on-IISVM1.png?raw=true "Iisstart.htm page on IISVM1")

	> **Note:** replace NNNN with the number used by the service name.
	
	> **Speaking Point:**

	> The browser displays the iisstart.htm file from IISVM1. Notice the **IISVM1** text on the home page.

1. Press **Ctrl-F5** a couple times to refresh the Web page.

	![Iisstart.htm page on IISVM2](./Images/Iisstart.htm-page-on-IISVM2.png?raw=true "Iisstart.htm page on IISVM2")

	> **Speaking Point:**

	> The browser displays the iisstart.htm file from IISVM2.
	
	> This result confirms the load-balancing between the two Web servers.

1. In the Azure portal, on the **issvm1** page, on the **Configure** tab, in the **Availability Set** drop-down list box, select **Create availability set**. In the text box, type **webavset**.

	![Changing availability set of IISVM1 VM using the Microsoft Azure Portal](./Images/Changing-availability-set-of-IISVM1-VM-using-the-Windows-Azure-Portal.png?raw=true "Changing availability set of IISVM1 VM using the Microsoft Azure Portal")

	> **Speaking Point:**

	> Let's switch our focus to **Availability Sets**.

	> Servers in the same availability set, are placed on different racks in the datacenter to ensure high availability in case a failure affects a particular server rack.

1. On the toolbar, press **Save** to save the changes. Press **Yes** to confirm that **iisvm1** could be restarted.

	![Saving Changes](./Images/Saving-Changes.png?raw=true "Saving Changes")

	> **Speaking Point:**

	> When saving the changes, the portal warns that server **iisvm1** could be restarted (when moving to a different server rack).

1. Wait until iisvm1 is configured. On the Virtual machines page, click the **iisvm2** name.

	> **Speaking Point:**

	> Let's update the Availability Set for iisvm2 as well.

1. On the **issvm2** page, on the **Configure** tab, in the **Availability Set** drop-down list box, select **webavset**.

	![Changing availability set of IISVM2 VM using the Microsoft Azure Portal](./Images/Changing-availability-set-of-IISVM2-VM-using-the-Windows-Azure-Portal.png?raw=true "Changing availability set of IISVM2 VM using the Microsoft Azure Portal")

	> **Speaking Point:**

	> By selecting the same Availability Set for **iisvm2**, both servers will not be placed in the same rack.

1. On the toolbar, press **Save** to save the changes. Press **Yes** to confirm that **iisvm2** could be restarted.

	![Saving Changes on IISVM2](./Images/Saving-Changes-on-IISVM2.png?raw=true "Saving Changes on IISVM2")

	> **Speaking Point:**

	> Once this is finished, you will now see icons with both of the VMs listed in the availability set.  This ensures the **99.95%** availability for your virtual machines.

	> Notice that iisvm1 and issvm2 may indicate "**Update in progress**" for a few minutes. This indicates that one of the two virtual machines is moving to another server rack to ensure both virtual machines are on different racks.

---

<a name="summary" />
## Summary ##

In this demonstration, you learned how to configure the load balancer and availability sets to have a highly scalable and available application.
