<a name="demo2" />
# Puppet Labs Demo#

## Overview ##

In this demo...
Integration with configuration management systems, in this case Puppet.

<a name="Goals" />
### Goals ###
In this demo, you will see how to:

 1. Provision Puppet Resources using the Azure Management Portal
 1. Use the Puppet Dashboard to manage Puppet Agents
 
<a name="Technologies" />
### Key Technologies ###
- Microsoft Azure subscription
- Puppet Labs

<a name="Prerequisites" />
### System Prerequisites ###
- [Azure PowerShell Cmdlets](http://go.microsoft.com/?linkid=9811175&clcid=0x409)
- [Git Bash](http://www.git-scm.com/download/win)

<a name="Setup" />
### Setup and Configuration ###

##### Updating Configuration Variables #####
1. Using a text editor, open the **config.xml** file located in the **Source** folder.

1. Replace the placeholder values in the **generalSettings** node with the following:
	- **azureSubscriptionName**: your Azure subscription name.
	- **location**: the location where the Azure resources will be deployed. E.g.: East US, West US, etc.
	- **adminUserName**: the admin username for the Puppet.
	- **adminPassword**: the password for the admin username.
	- **storageAccount**: the name of the storage account required for this demo. The setup scripts will automatically create a new Storage Account in Azure using this value, if it dos not exist. The name you choose must be in lower case.
1. Replace the placeholder values in the **puppetMasterSettings** node with the following:
	- **cloudServiceName**:
	- **vmName**:
	- **consoleUserName**:
	- **consolePassword**:

1. Replace the placeholder values in the **puppetAgentSettings** node with the following:
	- **cloudServiceName**:
	- **vmName**:
	
1. Save the file and close the editor.

1. Run **Reset.cmd** using elevated permissions.

	> **Note:** It might take a few minutes to provision all the resources in Azure.


## Demo ##

This demo is composed of the following segments: 

1. [Provisioning Puppet Resources](#segment1)

1. [Using the Puppet Dashboard](#segment2)

<a name="segment1" />
### Provisioning Puppet Resources ###
In this segment we will show how we could easily create Puppet masters from within Microsoft Azure by adding a puppet master image to our platform image repository.

1. Open the Management Portal by using a web browser to navigate to [https://manage.windowsazure.com](https://manage.windowsazure.com) and sign in using the Microsoft Account associated with your Windows Azure account.

1.  Go to **Virtual Machines**.

1. In the bottom toolbar, click **New**, select **Compute | Virtual Machine** and then **From Gallery**.

	![Create New Virtual Machine From Gallery](Images/create-new-virtual-machine-from-gallery.png?raw=true)
	
	_Create New Virtual Machine From Gallery_
	
1. Select **Puppet Labs** from the left panel and show the **Puppet Master** image.

	![Puppet Labs VM image template](Images/puppet-labs-vm-image-template.png?raw=true)
	
	_Puppet Labs Image Template_

	> **Speaking Point:** By clicking the Puppet Labs section we should be able to launch a Puppet Enterprise Puppet Master server right inside of Microsoft Azure.
	We've also made it easy to create Puppet agents, machines running the Puppet Agent that connect to a Puppet Master. This is what we're going to do now. 

1. Select **Windows Server** from the left panel and select the **Windows Server 2012 R2 Datacenter** image. Click the right arrow to continue.

	![Windows Server VM Image Template](Images/windows-server-vm-image-template.png?raw=true)

1. Enter a virtual machine name (e.g. puppet), set an administrator username and its password. Click the right arrow to continue.

	![New Virtual Machine configuration](Images/new-virtual-machine-configuration.png?raw=true)
	
	_Virtual Machine Configuration, step 2_

1. Leave the default values and click the right arrow to continue.

	![Virtual Machine Configuration screen 3](Images/virtual-machine-configuration-screen-3.png?raw=true)
	
	_Virtual Machine Configuration, step 3_

1. Check the **Puppet Enterprise Agent** option, and once the **Puppet Master Server** field appears type the address the of **Puppet Master** instance (e.g. puppetmaster.cloudapp.net).

	![Install Puppet Agent](Images/install-puppet-agent.png?raw=true)

	_Install Puppet Enterprise Agent and Provide Puppet Master Server_
	
	>**Speaking Point:** the final step is to install the VM Agent. If we have the VM Agent installed we can use that same agent technology to inject other code into that VM. And the one we'll inject in this demo is Puppet. At this point we just tell it where the puppet master is. When the virtual machine is provisioned, the puppet agent is going to launch and connect to the puppet master and I'll be able to manage it from there and deploy code into it.
	
1. Close the wizard without completing it.

<a name="segment2" />
### Using the Puppet Dashboard ###

In this segment we're going to show how to deploy code into a virtual machine on Azure from a Puppet Master.

1. Switch to **Puppet Dashboard** in the browser and explain the home page.



	> **Speaking Point:** This is the normal interface for Puppet Enterprise. You can see under **Nodes** that we have a small number of machines under management. The **Daily run status** shows colored bars with the result achieved every time a puppet agent runs: green if the agent did not need to do any extra work to update its infrastructure; blue if it had to make an actual change to bring it into sync.
	
	>In this case we're going to make changes to our Windows Servers.
	
1. Select **Windows Servers** group from the **Groups** panel on the left.
	
1. Switch to the Agent VM **Remote Desktop** connection and open the **Task Manager** by right-clicking the taskbar.

1. Show that the VM is using the original **Task Manager**.

	> **Speaking point:** we have an example virtual machine that is running the standard version of the Task Manager. We've heard there's a better version of the Task Manager out there, so we're going to see what it takes to update the machines to use the new Task Manager.
	
1. Switch back to **Puppet Dashboard** and click **Edit**.

1. Type **microsoft-sysinternals** in the **Classes** textbox and select the option from the autocomplete dropdown.

	> **Speaking point:** In Puppet, the class is esentially the way of referring to the code associated to the function I do. So by adding the microsoft-sysinternals module we associate the class with the work that needs to be done to all of the machines in the group. This will propagate out to your whole infrastracture, which may take around 30 minutes. If you have a hundred thousand machines under management you probably do not want all of them hitting your server at exactly the same time. In this case though, we have the system working on a relatively tighter timeline, so it is propagated faster.

1. Click **Update** to save changes.

1. Switch back to the Agent VM **Remote Desktop** and open a **Command Prompt**.

1. Enter the following **puppet** command to trigger the updates.

	````Command Prompt
	puppet agent --onetime
	````

1. Wait until the command completes the update.

1. Once completed, open the **Task Manager**. It will open the **Process Explorer** instead of the classic **Task Manager**.
	
---

<a name="summary" />
## Summary ##

In this demo, you saw how to provision Puppet Resources using the Azure Management Portal and how to use the Puppet Dashboard to manage Puppet Agents.
