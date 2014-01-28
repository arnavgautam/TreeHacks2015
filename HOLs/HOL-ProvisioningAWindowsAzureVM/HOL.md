<a name="Provisioning a Windows Azure VM" />
# Provisioning a Windows Azure VM #

---
<a name="Overview" /></a>
## Overview ##
In this lab, you will create a new virtual network and then a new Windows Server 2012 VM from a gallery image using the Windows Azure management console in your web browser.  The virtual network creation is not necessary to create a new virtual machine from a gallery image, but is necessary to control the IP addresses assigned to the virtual machines or enable VPN connectivity back to a corporate on-premise network.

>**Note:** If you are following this HOL for a second time, you can skip Exercise 1 and move to Exercise 2 to create an additional VM on the existing virtual network you already created.

<a name="Objectives" /></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Configure a new virtual network
- Deploy a new virtual machine from a gallery image

<a name="Prerequisites" /></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- A Windows Azure subscription [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

>**Note:** In order to run through the complete hands-on lab, you must have network connectivity. 


---
<a name="Exercises" /></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Create a new Virtual Network](#Exercise1)
1. [Create a new Windows Server 2012 VM from gallery image](#Exercise2)


Estimated time to complete this lab: 30 minutes

<a name="Exercise1" /></a>
### Exercise 1: Create a new Virtual Network ###

The workload on many VMs requires persistent IP addresses and self-provided DNS name resolution. The default internal DNS service (iDNS) in Windows Azure is often not an acceptable solution because the IP address assigned to each virtual machine is not persistent. For this solution you will define a virtual network where you can assign the virtual machines to specific subnets. 

The network configuration used for this lab defines the following:

- A Virtual Network Named domainvnet with an address prefix of: 10.0.0.0/16
- A subnet named AppSubnet with an address prefix of: 10.0.10.0/24
- A local DNS server called DC01 with an ip address of: 10.0.10.4

Exercise 1 contains 1 task:

1. Creating a new Virtual Network

<a name="Ex1Task1" /></a>
#### Task 1 - Creating a new Virtual Network ####

The first task is to create a new Virtual Network to your subscription with a new affinity group.

1. Open a browser a go to [https://manage.windowsazure.com/](https://manage.windowsazure.com/). When prompted, login with your **Windows Azure** credentials. In the Windows Azure Portal, click **New**, select **Networks** | **Virtual Network** and then click **Custom Create**.

	![Virtual Network custom create](images/virtual-network-custom-create.png?raw=true)

	_Virtual Network custom create_

1. On the Virtual Network Details page, in the Name text box, type _domainvnet_. Select _Create a new affinity group_ option from the **affinity group** dropdown list. Name the affinity group _adag_ and choose the region which is closest to you. Click the arrow button to continue.

	![creating a new virtual network](images/creating-a-new-virtual-network.png?raw=true)

	_Creating a new virtual network_

1. At the top right corner, select the **CIDR** option. Then set the Address Space Starting IP value to _10.0.0.0_ and its Address CIDR to _/16_. Finally, add a subnet named _AppSubnet_ with a Starting IP of _10.0.10.0_ and an Address CIDR of _/24_. Click the arrow button to continue to the next step.

	![Adding an address space and subnets](images/adding-an-address-space-and-subnets.png?raw=true)

	_Adding an address space and subnets_

1. On the DNS Servers and Local Network page, under **DNS Servers**, select **Specify an new DNS server**, and then in the **Name** text box, type **DC01**. In the **IP address** text box, type **10.0.10.4**. Click the plus-sign to define the DNS server address. Click the check mark icon to continue.
	> **Note:**	This setting specifies that Azure will assign 10.0.10.4 as DNS server to VMs in this subnet.

	![Creating the Virtual Network](images/creating-the-virtual-network.png?raw=true "Creating the Virtual Network")

	_Creating the Virtual Network_

<a name="Exercise2" /></a>
### Exercise 2: Create a new virtual machine from the gallery image ###

You will now create a new virtual machine from a Windows Server 2012 gallery image called DC01.  If you are following this exercise after the first time, you should name the virtual machine something other than DC01 and also ensure you connect to the existing virtual machine network when you create the VM. The DC01 virtual machine will be used in various exercises.

Exercise 2 contains 1 task:

1. Create a new virtual machine

<a name="Ex2Task1" /></a>
#### Task 1 - Create a new Virtual Machine ####

1. In the Windows Azure portal, select the **Virtual Machines** section.

	![Selecting the Virtual Machines section](./images/selecting-the-virtual-machines-section.png?raw=true "Selecting the Virtual Machines section")

	_Selecting the Virtual Machines section_

1. On the toolbar, click the **New** icon. In the New dialog window, select **Virtual Machine**, and then click **From Gallery**.

	![Creating a virtual machine from gallery](./images/creating-vm-from-gallery.png?raw=true "Creating a virtual machine from gallery")

	_Creating a virtual machine from gallery_

1. In the Create Virtual Machine dialog window, select **Platform Images**, select the **Windows Server 2012 Datacenter** image, and then click the right arrow icon to continue.

	![Selecting the platform image](./images/selecting-the-platform-image.png?raw=true "Selecting the platform image")

	_Selecting the platform image_

1. In the **Virtual Machine Name** text box, type **DC01**. 

>**Note:** If you are running through this exercise after the first time to create an additional VM, then specify a different name than DC01. 

1. In the **New Password** and **Confirm** text, boxes type **Passw0rd!**.

1. In the **Size** drop-down box, ensure that **Small** is selected, and then click the right arrow icon to continue.

	![Creating the virtual machine](./images/creating-the-virtual-machine.png?raw=true "Creating the virtual machine")

	_Creating the virtual machine_


1. The DNS name needs to be unique so for this lab, be creative. You can select an existing **Storage Account** or automatically generate a new one. Select the domainvnet Virtual Network from the drop down list **Region/Affinity Group/Virtual Network**. Click the arrow to continue. 
	
	>**Note:** If you are running through this exercise after the first time to create an additional VM, then make sure to check the option **Connect to an existing Virtual Machine**. 

	![Selecting the Virtual Network for the Virtual Machine](images/selecting-the-virtual-network-for-the-virtual.png?raw=true)

	_Selecting the Virtual Network for the Virtual Machine_

1. On the Virtual machine options page, click the check mark icon to complete the wizard. _The new virtual machine is being created._

	![Complete the VM creation process](./images/complete-the-vm-creation-process.png?raw=true "Complete the VM creation process")

	_Complete the VM creation process_

<a name="summary" />
## Summary ##

By completing this hands-on lab you have learned how to:

 * Configure a new virtual network
 * Deploy a new virtual machine from a gallery image

---

