<a name="Deploy-AD-in-Windows-Azure" />
# Deploy Active Directory using GUI in Windows Azure #

---

<a name="Overview" /></a>
## Overview ##

In this lab, you will provision a newly created Windows Server 2012 Virtual Machine called DC01 in Windows Azure using the Windows Azure management console in your web browser and then deploy Active Directory using Server Manager on DC01. DC01 will be the first domain controller in a new forest.

When deploying Active Directory in Windows Azure, two aspects are important to point out.

The first one is the networking configuration. Domain members and domain controllers need to find the DNS server hosting the domain DNS information. You will configure the Azure network configuration, so that the correct DNS server is configured.

Secondly, it is important to avoid the possibility of Active Directory database corruption. Active Directory assumes that it can write its database updates directly to disk. That means that you should place the Active Directory database files on a data disk that does not have write caching enabled.

<a name="Objectives" /></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Provision a data disk to a Virtual Machine
- Deploy a Domain Controller in Windows Azure

<a name="Prerequisites" /></a>
### Prerequisites ###

1. Complete the Provisioning a Windows Azure Virtual Machine HOL

>**Note:** In order to run through the complete hands-on lab, you must have network connectivity. 

<a name="Exercises" /></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Add a new data disk to the virtual machine](#Exercise1)
1. [Deploy a new domain controller in Windows Server 2012](#Exercise2)

---

<a name="Exercise1" /></a>
### Exercise 1: Add a new data disk to the virtual machine ###

You will now modify the virtual machine you already created from the "Provisioning a virtual machine" lab.  We will call this VM DC01.  We will create and provision a data disk to this existing VM which will be used in exercise 2 to place the AD database files.

Exercise 1 contains 2 tasks:

1. Attach a data disk to DC01
1. Configure a new data disk on DC01

<a name="Ex1Task1" /></a>
#### Task 1 - Attach a data disk to DC01 ####

1. In the **Virtual Machines** section of the Windows Azure portal, select the **DC01** virtual machine, and then on the bottom toolbar, click **Attach | Attach empty disk**.

	![Attaching an empty disk](./Images/attaching-an-empty-disk.png?raw=true "Attaching an empty disk")

	_Attaching an empty disk_

1. In the _Attach an empty disk to the virtual machine_ dialog box, in the **File Name** text box, type **DC01-data**.

1. In the **Size (GB)** text box, type **10**.

1. Click the check mark icon to continue. _Notice that by default a data disk does not have Read or Read Write caching enabled. For use with the Active Directory database files, we need to use a data disk without caching._

	![Completing the creation of the disk](./Images/completing-the-creation-of-the-disk.png?raw=true "Completing the creation of the disk")

	_Completing the creation of the disk_

<a name="Ex1Task2" /></a>
#### Task 2 - Configure a new data disk ####

1. In the **Virtual Machines** section of the Windows Azure portal, select the **DC01** virtual machine, and then on the toolbar, click the **Connect** icon to connect using **Remote Desktop Connection**.

	![Connecting to the DC01 Virtual Machine](./Images/connecting-to-the-dc01-vm.png?raw=true "Connecting to the DC01 Virtual Machine")

	_Connecting to the DC01 Virtual Machine_

1. Open the DC01.rdp file, and connect to the virtual machine.

	> **Note:** use the credentials that you inserted when creating the virtual machine in Task 1 of this exercise.

1. Once on the DC01 virtual machine, open the **Server Manager**. On the **Tools** menu, click **Computer Management**. _The Computer Management console opens._

	![Opening the Computer Manager console](./Images/opening-the-computer-manager-console.png?raw=true "Opening the Computer Manager console")

	_Opening the Computer Manager console_

1. In the Computer Management console, in the left pane, select **Disk Management**. _Disk Management recognizes that a new initialize disk is added to the computer, and it will show the Initialize Disk dialog box._

	![Selecting Disk Management](./Images/selecting-disk-management.png?raw=true "Selecting Disk Management")

	_Selecting Disk Management_

1. In the Initialize Disk dialog box, click **OK**. _The new Disk 2 is initialized._

	![Initializing the disk 2](./Images/initializing-the-disk-2.png?raw=true "Initializing the disk 2")

	_Initializing the disk 2_

1. On Disk 2, right-click the **Unallocated** space, and then click **New Simple Volume**. _The New Simple Volume Wizard opens._

	![Formatting the unallocated space](./Images/formating-the-unallocated-space.png?raw=true "Formatting the unallocated space")

	_Formatting the unallocated space_

1. In the new Simple Volume Wizard, click **Next**.

	![Using the Simple Volume Wizard](./Images/using-the-simple-volume-wizard.png?raw=true "Using the Simple Volume Wizard")

	_Using the Simple Volume Wizard_

1. On the Specify Volume Size page, click **Next**. _This means that the entire available space (10237 MB) will become a new volume._

	![Specifying the volume size](./Images/specifing-the-volume-size.png?raw=true "Specifying the volume size")

	_Specifying the volume size_

1. On the Assign Drive Letter or Path page, ensure drive letter **F** is selection, and then click **Next**.

	![Assigning the drive letter](./Images/assigning-the-drive-letter.png?raw=true "Assigning the drive letter")

	_Assigning the drive letter_

1. On the Format Partition page, in the **Volume Label** text box, type **AD DS Data**, and then click **Next**.

	![Specifying the volume label](./Images/specifing-the-volume-label.png?raw=true "Specifying the volume label")

	_Specifying the volume label_

1. On the Completing the New Simple Volume Wizard page, click **Finish**. _Windows will quick format the disk, and assign drive letter F:._

	![Completing the wizard](./Images/completing-the-wizard.png?raw=true "Completing the wizard")

	_Completing the wizard_

	> **Note:** if you are prompted to format the new AS DS Data disk, click **OK** in the dialog box and format the disk as NTFS.

1. Close the Computer Management console.

---

<a name="Exercise2" /></a>
### Exercise 2: Deploy a new domain controller in Windows Server 2012 ###
You have just created a base virtual machine called DC01, attached the necessary data disk, and provisioned the disk. We are going to login to DC01 to install and configure active directory and then verify the install was successful.

Exercise 2 contains 3 tasks:

1. Install the Active Directory Domain Services Role 
1. Configure the Active Directory Domain Services Role
1. Verify the Domain Controller Installed Successfully

<a name="Ex2Task1" /></a>
#### Task 1 - Install the Active Directory Domain Services Role ####

1. In the DC01 virtual machine, on the **Dashboard** page of the **Server Manager**, click **Add roles and features**.

	![Configuring the server](./Images/configuring-the-server.png?raw=true "Configuring the server")

	_Configuring the server_

1. In the Add Roles and Features Wizard, click **Next**.

1. On the Select Installation Type page, select **Role-based or feature-based installation**, and then click **Next**.

1. On the Select Destination Server page, click **Next**.

	![Selecting the destination server](./Images/selecting-the-destination-server.png?raw=true "Selecting the destination server")

	_Selecting the destination server_

1. On the Select Server Roles page, select **Active Directory Domain Services**.

1. In the Add Roles and Features dialog box, click **Add Features**.

1. Once the _Active Directory Domain Services_ role is selected, click **Next**.

	![Selecting server roles](./Images/selecting-server-roles.png?raw=true "Selecting server roles")

	_Selecting server roles_

1. On the Select Features page, click **Next**.

	![Selecting features](./Images/selecting-features.png?raw=true "Selecting features")

	_Selecting features_

1. On the Active Directory Domain Services page, click **Next**.

1. On the Confirm Installation Selection page, click **Install**.

1. Wait for the installation to complete. Do not click **Close**. _Windows is installing the Active Directory Domain Services role._

	![Waiting for the installation to complete](./Images/waiting-for-the-installation-to-complete.png?raw=true "Waiting for the installation to complete")

	_Waiting for the installation to complete_

<a name="Ex2Task2" /></a>
#### Task 2 - Configure the Active Directory Domain Services Role ####

1. When the feature installation has completed, click the link **Promote this server to a domain controller**. You can do this in the Add Roles and Features Wizard dialog box, or in the listed Server Manager flagged warning tasks.

	![Promoting the server](./Images/promoting-the-server.png?raw=true "Promoting the server")

	_Promoting the server_

1. On the Deployment Configuration page, select **Add a new forest**.

1. In the **Root domain name:** text box, type **contoso.com**. Click **Next**.

1. Leave all of the default settings and then type **Passw0rd!** for the DSRM password and click **Next**. 

	![Configuring the deployment](./Images/configuring-the-deployment.png?raw=true "Configuring the deployment")

	_Configuring the deployment_	
 
1. Ignore the warning in the DNS Options section and click **Next**.

1. On the Additional Options page, click **Next**.

1. On the Paths page, change the folders as follows and click **Next**.

	| Field | Value |
	|--------|--------|
	| Database folder | **F:\NTDS** |
	| Log files folder | **F:\NTDS** |
	| SYSVOL folder | **F:\SYSVOL** |
	
	![Specifying the paths](./Images/specifing-the-paths.png?raw=true "Specifying the paths.")

	_Specifying the paths_

	> **Note:** _The C: disk is the OS disk, and has caching enabled. The Active Directory database should not be stored on a disk that has write caching enabled. The F: disk is a data disk we added earlier, and does not have caching enabled._

1. On the Review Options page, click **Next**.

	![Reviewing the options](./Images/reviewing-the-options.png?raw=true "Reviewing the options")

	_Reviewing the options_

1. On the Prerequisites Check page, click **Install**. The computer is promoted to domain controller.

1. After a few moments, the DC01 Virtual Machine will restart. You will lose the connection to the restarting Virtual Machine.

	![Checking the prerequisites](./Images/checking-the-prerequisites.png?raw=true "Checking the prerequisites")

	_Checking the prerequisites_

<a name="Ex2Task3" /></a>
#### Task 3 - Verify the Domain Controller Installed Successfully ####
> **Note:** You will need to wait about 2-3 minutes after clicking install in the previous step for the DC01 Virtual Machine to restart in order to be able to connect.

1. In the **Virtual Machines** section of the Windows Azure portal, select the **DC01** virtual machine, and then on the toolbar, click the **Connect** icon to connect using **Remote Desktop Connection**.

	![Connecting to the DC01 Virtual Machine](./Images/connecting-to-the-dc01-vm.png?raw=true "Connecting to the DC01 Virtual Machine")

	_Connecting to the DC01 Virtual Machine_

1. Open the DC01.rdp file, and connect to the virtual machine.

	> **Note:** use the credentials that you inserted when creating the virtual machine in Task 1 of this exercise.

1. After logon, in Server Manager, on the **Tools** menu, click **Active Directory Administrative Center**. _The Active Directory Administrative Center console opens._

	![Opening the Active Directory Administrative Center console](./Images/opening-the-ad-administrative-center-console.png?raw=true "Opening the Active Directory Administrative Center console")

	_Opening the Active Directory Administrative Center console_

1. In the Active Directory Administrative Center window, in the left pane, expand **contoso (local)**, and then select **Domain Controllers**. _Notice that the domain has the DC01 domain controller listed. This result confirms that DC01 was successfully promoted to domain controller._

	![Selecting Domain Controllers](./Images/selecting-domain-controllers.png?raw=true "Selecting Domain Controllers")

	_Selecting Domain Controllers_

1. Close the Active Directory Administrative Center console.

---
<a name="Summary"/></a>
## Summary ##

In this lab, you walked through the steps of deploying a new Active Directory Domain controller in a new forest using Windows Azure virtual machines.
