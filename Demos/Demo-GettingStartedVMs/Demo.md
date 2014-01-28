<a name="title" />
# Getting Started with Windows Azure Virtual Machines #

---

<a name="Overview" />
## Overview ##
This demonstration shows how to get started creating and configuring Windows Azure Virtual Machines.

> **Note:** In order to run through this complete demo, you must have network connectivity and a Microsoft account.

<a id="goals" />
### Goals ###
In this demo, you will see three things:

1.	Virtual Machine Creation
1.	Virtual Machine Configuration 
1.	Virtual Machine Connectivity 

<a name="technologies" />
### Key Technologies ###

This demo uses the following technologies:

- [Windows Azure Management Portal] [1]

[1]: https://manage.windowsazure.com/

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- A Windows Azure subscription [sign up for a free trial](http://aka.ms/WATK-FreeTrial)
- Putty SSH Client for Windows [Putty home page](http://www.putty.org/)

<a name="setup" />
### Setup and Configuration ###

1.	Create a new Windows Server 2012 virtual machine.
	1.	Create Empty Disk of 1023 GB.
	1.	RDP into the machine and initialize and format the disk.
	1.	Launch **Windows Firewall with Advanced Security**.
	1.	Under **Inbound Rules** enable both **File and Printer Sharing Echo Request Rules**.

---

<a name="Demo" />
## Demo ##

This demo is composed of the following segments:

1. [Virtual Machine Creation](#segment1)
1. [Virtual Machine Configuration](#segment2)
1. [Virtual Machine Connectivity](#segment3)

<a name="segment1" />
### Virtual Machine Creation ###

1.	Show how to create a new Windows Server 2012 Datacenter virtual machine in the Windows Azure Management portal using the gallery experience but actually do not create it (skip the last wizard step and use the Virtual Machine created in the setup section).

	![VM OS Selection](Images/vm-os-selection.png?raw=true "VM OS Selection")

	_Virtual Machine Operating System Selection_

	> **Speaking Point**: Note that Windows Azure supports creation from images or disks. Note that the images can be custom images created by the owner of the subscription or they can be platform images.

	> Note that Windows Azure now supports various Linux distros.


<a name="segment2" />
### Virtual Machine Configuration ###

1. RDP into the virtual machine created in the Setup section, start **Computer Management** and go to **Disk Management**.

	> **Speaking Point**: Note that you have 1TB of storage. How do you get more? 

1. In the portal click **Attach Empty Disk**.

1. Type _1023_ and switch back to the disk manager.

	> **Speaking Point**: Note that you just dynamically added an additional 1TB in storage.
	> An X-Large virtual machine can have up to 16x1TB disks attached (Small is only 2x1TB disks).

	![Attaching an Empty Disk](Images/attaching-an-empty-disk.png?raw=true)

	_Attaching an Empty Disk_

1. Create a new Virtual Machine based on SUSE Linux distribution.

	![Creating a Linux VM](Images/creating-a-linux-vm.png?raw=true)

	_Creating a Linux Virtual Machine_

1. Change from stand-alone to connect to existing Virtual Machine and select the running Windows Server 2012 Virtual Machine. This will put the Linux Virtual Machine on the same network as the server 2012 Virtual Machine. Leave the default values and complete the wizard.

	![VM Mode](Images/vm-mode.png?raw=true "VM Mode")

	_Virtual Machine Mode_

	> **Note:** It takes 2-3 mins to boot - talk about another slide

<a name="segment3" />
### Virtual Machine Connectivity ###

1. Use Putty or another SSH client and connect to the Linux Virtual Machine.

	![SSH Details](Images/ssh-details.png?raw=true "SSH Details")

	_SSH Details_

1. Ping the Windows Server 2012 virtual machine demonstrate connectivity.

	![Ping to the new VM](Images/ping-to-the-new-vm.png?raw=true "Ping to the new VM")
	
	_Pinging to the new Virtual Machine_

---

<a name="summary" />
## Summary ##

In this demonstration, you have seen how to provision a virtual machine and dynamically add additional storage to the Virtual Machine. You have also seen how to add additional virtual machines to the same cloud service to establish direct network connectivity and name resolution.