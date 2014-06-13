<a name="Title" />
# Understanding Virtual Machine Imaging with Capture #
---

<a name="Overview" />
## Overview ##

In this hands-on lab you will walk through creating a customized virtual machine that is customized with Web Server role enabled. You will then learn how to generalize it and save it as an image so that all new virtual machines provisioned from it will have Web Server role enabled by default.

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

- Customize and generalize a virtual machine
- Save the image to the image library
- Provision New Virtual Machines based off of the image

<a name="Prerequisites" />
### Prerequisites ###

You must have the following items to complete this lab:

- A Microsoft Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)
- Complete the _Provisioning a Microsoft Azure Virtual Machine_ HOL.

<a name="Exercises" />
## Exercises ##
The following is required to complete this hands-on lab:

1. [Customizing and Generalizing the Virtual Machine](#Exercise1)
2. [Saving an Image in the Image Library](#Exercise2)
3. [Provisioning New Virtual Machines based on an Image](#Exercise3)

Estimated time to complete this lab: **60 minutes**

<a name="Exercise1"></a>
### Exercise 1: Customizing and Generalizing the Virtual Machine ###

In this exercise we are going to customize the Virtual Machine by enabling the Web Server role in Windows Server 2012. 

<a name="Ex1Task1" />
#### Task 1: Enabling Web Server role####

1. Go to the **Virtual Machines** page within the Microsoft Azure Management portal and select the Virtual Machine you created by following the _Provisioning a Microsoft Azure Virtual Machine_ HOL.

	![Virtual Machine Created](Images/virtual-machine-created.png?raw=true "Virtual Machine Selected")

	_Virtual Machine Created_

1. In the bottom menu, click **Connect** to download an _.rdp_ file to connect to the Virtual Machine using Remote Desktop Connection.

	![Connect](Images/connect.png?raw=true)

	_Connecting to the Virtual Machine_

	>**Note:** When asked for Login, write the same credentials you used while creating the Virtual Machine.

1. If not already opened, open the Server Manager from the Quick Launch bar and click **Add roles and features**.

	![Add roles and features](Images/add-roles.png?raw=true)

	_Add roles and features_

1. Click **Next** in the _Before you begin_ screen.

1. In the _Installation Type_ screen select **Role-based or feature-based installation** and click **Next**.

1. In the following screen, make sure your server is selected and click **Next**.

1. In the _Server Roles_ screen, look for **Web Server (IIS)** role and mark the checkbox. When prompted for addtitional features click **Add features**.

	![Select Web Server role](Images/add-roles-select-role.png?raw=true)

	_Select Web Server role_

1. Click **Next** in the _Features_ and _Web Server Role (IIS)_ screens.

1. In the _Role Services_ screen leave the default options and click **Next**.

	![Select Role Services role](Images/add-roles-web-server-services.png?raw=true)

	_Select Role Services_

1. In the _Confirmation_ screen click **Install**. Once the process is finished click **Close**.

<a name="Ex1Task2" />
#### Task 2: Generalizing the Machine with SysPrep ####

In this step we will run sysprep to generalize the image. It will allow multiple virtual machines to be created having the same customized settings (Web Server role enabled).

1. On the Start menu, start typing _run_, and then click Run.

	![Open Run dialog](Images/start-run.png?raw=true)

	_Open Run dialog_

1. In the **Run** box write _c:\Windows\System32\sysprep\sysprep.exe_ and press Enter.

1. In the **System Cleanup Action** select **Generalize** checkbox and in the **Shutdown Options** select **Shutdown** and click **OK**.

	![sysprep](Images/sysprep.png?raw=true "sysprep")

	_Sysprep dialog_

---

<a name="Exercise2" />
###Exercise 2: Saving an Image in the Image Library###

In this exercise you are going to use the capture feature of Microsoft Azure IaaS to create a new image based off of an existing virtual machine (the previously created one).

>**Note:** Before proceeding, ensure the **DC01** Virtual Machine is off. Wait until the sysprep finishes and turns off the Virtual Machine

<a name="Ex2Task1" />
#### Task 1: Saving an Image in the Image Library ####

1. Open the Microsoft Azure Portal and click **Virtual Machines**.

1. Select the Virtual Machine you prepared in the previous exercise.

	![Opening Virtual Machine Dashboard](Images/opening-virtual-machine-dashboard.png?raw=true "Opening Virtual Machine Dashboard")

	_Opening Virtual Machine Dashboard_

1. Click **Capture** at the bottom of the screen.

	![Capturing Image](Images/capturing-image.png?raw=true "Capturing Image")

	_Capturing Image_

1. In the **Capture virtual machine** page, set the **Image Name** and select **I have run Sysprep on the virtual machine**, then store it as an image.

	![capturedlg](Images/capturedlg.png?raw=true)

	_Image Details_

---

<a name="Exercise3"></a>
### Exercise 3: Provisioning New Virtual Machines based on an Image ###

In this exercise you are going to create a new virtual machine using the image you created in exercise 2. 

> **Note:** Before proceeding wait until the image finishes provisioning. You can switch to the **Images** tab under **Virtual Machines** to check the status of the image.

<a name="Ex3Task1" />
#### Task 1: Creating a Virtual Machine Based on an Image ####

1. Log in to the Microsoft Azure Portal: https://manage.windowsazure.com.

1. Click **New** | **Compute** | **Virtual Machine** | **From Gallery**.
	![createvmfromimage](Images/createvmfromimage.png?raw=true)

	_Creating Virtual Machine from Image_

1. In the **Virtual Machine operating system selection** switch to **MY IMAGES** and select the image you created previously.

	![myimagetab](Images/virtualmachineimageselection.png?raw=true)

	_Virtual Machine operating system selection_

1. Set the **Virtual Machine Name** to _customizedvm1_ and complete the administrator username and password.

	![myimagetab](Images/virtualmachinename.png?raw=true)

	_Virtual Machine configurtation_

1. Set the **DNS Name** to a new unique name and click **Next**.

	![myimagetab](Images/virtualmachinedns.png?raw=true)

	_Virtual Machine mode_

1. Leave the **Virtual Machine options** with the default value and complete the operation.

<a name="Ex3Task2" />
#### Task 2: Creating an Endpoint to Allow Traffic to the Virtual Machine ####

1. Open the Microsoft Azure Portal from https://manage.windowsazure.com and click **Virtual Machines**.

2. Click on the **customizedvm1** Virtual Machine to open its **Dashboard** and then click **Endpoints**.

	![Virtual Machine Endpoints](Images/virtual-machine-endpoints.png?raw=true "Virtual Machine Endpoints")

	_Virtual Machine Endpoints_

4. Click **Add Endpoint** towards the bottom of the page, select **Add Endpoint** option and click **Next**. 

5. In the **Specify Endpoint Details** page, use the following settings.

	![add-endpoint](Images/add-endpoint.png?raw=true)

	>**Note:** Before proceeding, ensure the endpoint configuration is complete.

<a name="Ex3Task3" />
#### Task 3: Navigating to the IIS default web page ####

1. Log in to the Microsoft Azure Portal and go to your virtual machine's **Dashboard**.

1. In the **Dashboard** page, locate and take note of the DNS.

	![Obtaining VM information](Images/obtaining-vm-information.png?raw=true "Obtaining Virtual Machine information")

	_Obtaining Virtual Machine information_

1. Open a web browser and navigate to the DNS address from the previous step. In this case _http://customizedvm1.cloudapp.net/_.

	![IIS default web page](Images/ie-iis.png?raw=true "IIS default web page")

	_IIS default web page_

	>**Note:** Since the virtual machine was created using your custom image with Web Server role enabled, you get the IIS default web page when you browse to the virtual machine.

---

<a name="Summary" />
## Summary ##
By completing this Hands-on Lab you have learned how to:
 
 - Customize and capture a virtual machine to the image library
 - Provision New Virtual Machines based off of the image
 - Navigate to a virtual machine through HTTP
