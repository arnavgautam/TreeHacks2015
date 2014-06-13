<a name="Title" />
# Creating and Configuring a SQL Server 2012 Database in a Microsoft Azure Virtual Machine #
---

<a name="Overview" />
## Overview ##

In this hands-on lab you will learn how to create and configure a SQL Server 2012 Database.	

You will start by provisioning the Virtual Machine using the Microsoft Azure portal, and then you will configure the SQL Server instance.

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

1. Create a virtual machine using the Microsoft Azure portal
1. Configure a SQL Server 2012 Virtual Machine

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- A Microsoft Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1. [Creating and configuring Windows Server Virtual Machine with SQL Server 2012 using the Microsoft Azure portal](#Exercise1)

Estimated time to complete this lab: **30 minutes**.

<a name="Exercise1" />
### Exercise 1: Creating and configuring Windows Server Virtual Machine with SQL Server 2012 using the Microsoft Azure portal ###

You will now create the Windows Server Virtual Machine and configure SQL Server.

<a name="Ex1Task1" />
#### Task 1 - Creating the Virtual Machine in the Microsoft Azure Portal ####

1. Navigate to [http://manage.windowsazure.com/](http://manage.windowsazure.com/) using a Web browser and sign in using the Microsoft Account associated with your Microsoft Azure account.

	![Sign in](Images/sign-in.png?raw=true "Sign in")

	_Sign in_

1. Click the **New** link located at the bottom of the page and select **Compute** | **Virtual Machine** | **From Gallery**.

	![New VM from gallery](Images/new-vm-from-gallery.png?raw=true "New VM from gallery")

	_New Virtual Machine from gallery_

1. In the **Virtual Machine Operating System Selection** page, click **Platform Images** and then select **Microsoft SQL Server 2012 SP1 Standard Edition** from the OS list. Then click **Next**.

	![VM OS Selection](Images/vm-os-selection.png?raw=true "VM OS Selection")

	_Virtual Machine Operating System Selection_

1. In the **Virtual machine Configuration** page, set the virtual machine's name to _SqlServer2012VM_, provide a user name for the **New User Name** field and a password for the **New Password** and **Confirm Password** fields. Lastly, set the Virtual Machine **Size** to _Medium_ and click **Next** to continue.

	![New VM Configuration](Images/new-vm-configuration.png?raw=true "New VM Configuration")

	_New Virtual Machine Configuration_

1. In the **Virtual Machine Mode** page, leave mode as _Standalone Virtual Machine_,  set the **DNS Name** to _SQLServerVM1_, and the desired region. Then click **Next**.

	![Configuring the VM mode](Images/configuring-the-vm-mode.png?raw=true "Configuring the VM mode")

	_Configuring the Virtual Machine Mode_

1. In the **Virtual Machine Options** page, leave the default values and click **Finish** to complete the operation.

	![Virtual Machine Options](Images/vm-options.png?raw=true "Virtual Machine Options")

	_Setting the Virtual Machine Options_

1. In the **Virtual Machines** section, you will see the Virtual Machine you created in the _provisioning_ status. Wait until its status changes to _Running_ in order to continue with the next task.

	![VM Provisioned](Images/vm-provisioned.png?raw=true "VM Provisioned")

	_Virtual Machine Provisioned_

<a name="Ex1Task2" />
#### Task 2 - Configuring Disks for SQL Server ####

1. In the **Microsoft Azure Portal**, click the _SQLServerVM1_ virtual machine name that you created in Task 1 to open its Dashboard.

	![Opening VM Dashboard](Images/opening-vm-dashboard.png?raw=true "Opening VM Dashboard")

	_Opening the Virtual Machine Dashboard_

1. At the bottom menu, select **Attach** | **Attach Empty Disk**. 

	![attachemptydisk](Images/attachemptydisk.png?raw=true)

	_Attaching a Disk_

1. In the **Attach Empty Disk to Virtual Machine** page, set the disk **Size** to _50 GB_ and click **Finish**.

	![Attach Empty Disk to Virtual Machine](Images/attach-empty-disk-to-virtual-machine.png?raw=true "Attach Empty Disk to Virtual Machine")
	
	_Attaching an Empty Disk to Virtual Machine_

1. Wait until the disk has been provisioned and then repeat the previous steps to create and attach a second empty disk.

	![Two Empty Disks Attached](Images/two-empty-disks-attached.png?raw=true "Two Empty Disks Attached")

	_Two Empty Disks Attached_

1. You should have two 50 GB data disks attached to your virtual machine. 

	![Disks Section in Dashboard](Images/disks-section-in-dashboard.png?raw=true "Disks Section in Dashboard")

	_Disks Section in the Dashboard_

1. At the bottom menu, click **Connect** and log in to the virtual machine using Remote Desktop (RDP). Use the credentials that you inserted when creating the virtual machine in the previous task to log in.

	![Connect to the VM](Images/connect-to-the-vm.png?raw=true "Connect to the VM")

	_Connect to the Virtual Machine_

1. Once logged in, start **Computer Management** from **Start** | **Administrative Tools** and under the **Storage** node, click **Disk Management**.

1. You will be prompted to initialize the attached disks. Leave the default options, and click **OK**.

	![initializedisk](Images/initializedisk.png?raw=true)

	_The Initialize Disk dialog_

1. Once the disks are initialized you will need to right-click on the right side and select **New Simple Volume** (software RAID is also supported, thus those are available options as well). The **New simple volume** wizard will allow you to format the disks and mount them for use.
    
	![initializeddisks](Images/initializeddisks.png?raw=true)

	_Initialized Disks_

1. Configure Database Default Location. To do this, launch **SQL Server Management Studio** from **Start | All Programs | Microsoft SQL Server 2012 | SQL Server Management Studio**. 

1. Connect to the Server using the default information.

1. Right-click the server name and select **Properties**. Then select **Database Settings**.

1. Specify the new data disks for the default data, logs and backup folders, as seen in the following screenshot, and click **OK** to close.

    ![dbsettings](Images/dbsettings.png?raw=true)

	_Database Settings_

1. Restart the server, by right-clicking its name and selecting **Restart**.

<a name="Ex1Task3" />
#### Task 3 - Updating the SQL Server Network Configuration ####

1. If you are not already connected, connect to the _SQLServerVM_ virtual machine. To do this, in the **Microsoft Azure Portal**,open the _SQLServerVM_ virtual machine **Dashboard**. 

	![Opening VM Dashboard](Images/opening-vm-dashboard.png?raw=true "Opening VM Dashboard")

	_Opening Virtual Machine Dashboard_

1. Then, click **Connect** and log in to the virtual machine using Remote Desktop (RDP). Use the Administrator credentials to log on.

	![Connect to the VM](Images/connect-to-the-vm.png?raw=true "Connect to the VM")

	_Connecting to the Virtual Machine_


1. In the Virtual Machine, open **SQL Server Configuration Manager** from **Start | All Programs | Microsoft SQL Server 2012 | Configuration Tools**.

1. Expand the **SQL Server Network Configuration** node and select **Protocols for MSSQLServer** (this option might change if you used a different instance name when installing SQL Server). Make sure **Shared Memory**, **Named Pipes** and the **TCP/IP** protocols are enabled. To enable a protocol, right-click the Protocol Name and select **Enable**.

	![Enabling SQL Server Protocols](Images/enabling-sql-server-protocols.png?raw=true "Enabling SQL Server Protocols")

	_Enabling SQL Server Protocols_

1. Restart the service by going to **SQL Server Services**, right-click on **SQL Server** and click **Restart**.

1. Close the **SQL Server Configuration Manager**.

1. Open **Windows Firewall with Advanced Security** from **Start | All Programs | Administrative Tools**.

1. Select and right-click **Inbound Rules** node. Choose **New Rule** from the context menu to open the **New Inbound Rule Wizard**.

	![Creating an Inbound Rule](./Images/Creating-an-Inbound-Rule.png?raw=true "Creating an Inbound Rule")
 
	_Creating an Inbound Rule_

1. In the **Rule Type** page, select **Port** and click **Next**.

	![New Inbound Rule Wizard](Images/new-inbound-rule-wizard2.png?raw=true)
 
	_New Inbound Rule Wizard_

1. In the **Protocols and Ports** page, leave TCP selected, select **Specific local ports,** and set its  value to _1433_. Click **Next** to continue.

 	![New Inbound Rule Wizard](Images/new-inbound-rule-wizard.png?raw=true)
 
	_New Inbound Rule Wizard_

1. In the **Action** page, make sure that **Allow the connection** is selected and click **Next**.

 	![New Inbound Rule Wizard(3)](Images/new-inbound-rule-wizard3.png?raw=true)
 
	_New Inbound Rule Wizard_

1. In the **Profile** page, leave the default values and click **Next**.

1. In the **Name** page, set the Inbound Rule's **Name** to _SQLServerRule_ and click **Finish**

 	![New Inbound Rule Wizard(4)](Images/new-inbound-rule-wizard4.png?raw=true)
 
	_New Inbound Rule Wizard_

1. Close **Windows Firewall with Advanced Security** window.

1. Now, you can start taking advantage of your **SQL Server 2012** hosted in a Virtual Machine in the cloud. 

1. Close the **Remote Desktop Connection**.

<a name="summary" />
## Summary ##

In this lab you learned how to create and configure a SQL Server 2012 Database by provisioning a Virtual Machine using the Microsoft Azure portal and then configuring the SQL Server instance. You can easily add additional virtual machines and use the connect to Virtual Machine feature to have connectivity between them.
