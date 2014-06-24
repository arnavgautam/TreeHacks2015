<a name="demo2" />
# Puppet Labs Demo#

## Overview ##

In this demo we will show how **Microsoft Azure** integrates with configuration management systems, in this case Puppet.

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
1. Open **PowerShell** and execute the following command:

	````PowerShell
	Get-AzurePublishSettingsFile
	````

1. A browser will open. If prompted, enter your Azure credentials. Save the file to a known location and rename it **azure.publishsettings**.

	![Save Azure Settings file](Images/save-azure-settings-file.png?raw=true)
	
	_Save Azure Settings File_
	
1. Copy the file and replace the one located in the **Source\Setup\assets\publishSettings** folder of this demo.

1. Using a text editor, open the **config.xml** file located in the **Source** folder.

1. Replace the placeholder values in the **generalSettings** node with the following:
	- **azureSubscriptionName**: your Azure subscription name.
	- **location**: the location where the Azure resources will be deployed. E.g.: East US, West US, etc.
	- **adminUserName**: the admin username for the Puppet.
	- **adminPassword**: the password for the admin username.
	- **storageAccount**: the name of the storage account required for this demo. The setup scripts will automatically create a new Storage Account in Azure using this value, if it dos not exist. The name you choose must be in lower case.
1. Replace the placeholder values in the **puppetMasterSettings** node with the following:
	- **cloudServiceName**: name of the cloud service for the Master virtual machine.
	- **vmName**: name of the Master virtual machine.
	- **consoleUserName**: the user name for the master virtual machine. It will be in the form user@cloudServiceName.cloudapp.net.
	- **consolePassword**: password for the consoleUserName.

1. Replace the placeholder values in the **puppetAgentSettings** node with the following:
	- **cloudServiceName**: name of the cloud service for the Agent virtual machine.
	- **vmName**: name of the Agent virtual machine.
	
1. Save the file and close the editor.

##### Setup the Master Puppet VM #####
1. Run **Source\1.Setup.Master.cmd** using elevated permissions. A new command window will launch and start executing commands. Wait until it finishes running and when prompted, press a key to dismiss it.

	![Setup Master script](Images/setup-master-script.png?raw=true)
	
	_Setup Master script_
	
1. In a browser, open the **Management Portal** by navigating to [https://manage.windowsazure.com](https://manage.windowsazure.com) and sign in using the Microsoft Account associated with your Windows Azure account. 

1. Go to **Virtual Machines** and verify a virtual machine with the **vmName** name set in the **puppetMasterSettings** node of the configuration file has been created. 

	![Virtual Machines Puppet Master vm created](Images/virtual-machines-puppet-master-vm-created.png?raw=true)

1. In a new browser window, open the **Puppet Dashboard**. The Puppet Dashboard URL can be obtained by pre-pending "https://" to the DNS Name indicated for the master virtual machine (e.g. https://puppetmaster.cloudapp.net). Initially, the **Puppet Dashboard** page will not be displayed.
	
	![Setup Puppet Dashboard not displayed](Images/setup-puppet-dashboard-not-displayed.png?raw=true)
	_Puppet Dashboard, initially not displayed_

1. Refresh the browser periodically by pressing **Ctrl+F5** until a login window appears. This indicates the Puppet Master VM has finished setting up. Do not log in.

	>**Note:** This step may take around 15-20 minutes to complete.

	
	_Puppet Dashboard login page, displayed after Master VM has been set up_
	
	>Keep both browser windows open after setup, as you will need them for Segment 2.

##### Setup the Puppet Agent VM #####
1. Run **Source\2.Setup.Agent.cmd** using elevated permissions. A new command window will launch and execute commands. Wait until it finishes running and when prompted, press a key to dismiss it.

	_Setup.Agent script execution_

1. Switch to the browser with the Management Portal open.

1. Refresh the **Virtual Machines** page, and verify a virtual machine with the **vmName** name set in the **puppetAgentSettings** node of the configuration file has been created. 

	_Puppet Agent and Puppet Master Virtual Machines created_

##### Reset the Puppet Agent VM #####
1. Run **Source\3.Reset.Puppet.cmd** using elevated permissions. This will launch a command window.

	_Reset.Puppet script execution_

1. When prompted, log in with the adminUserName and adminPassword credentials provided in the **generalSettings** node of the configuration file.

	![Reset Puppet login](Images/reset-puppet-login.png?raw=true)
	
	_Log in With Admin Credentials_

1. A new command window will open and start running a script. You will be prompted for the **adminUserName** password twice. When the script finishes running, you will be asked to press the Enter key to exit. This will close the command window and focus will switch back to the command window that opened in step 1.

	_Reset script_

1. Press a key to dismiss it.

1. Switch to the browser at the **Puppet Dashboard** page, displaying the login page. Log in with the **consoleUsername** and **consolePassword** credentials provided in the **puppetMasterSettings** node of the configuration file.

	_Puppet Dashboard Home Page_

1. The **Puppet Dashboard** home page should be displayed. Verify that the list in the "All" tab contains the Master Puppet VM.

	_Master Puppet VM in Puppet Dashboard_

1. Verify that in the Groups section on the left, a **Windows Servers** group is displayed.

	_Windows Servers group in Puppet Dashboard_

1. Click the **1 Node Request** link at the top of the **Puppet Dashboard** home page.

	_Node Request_

1. In the page, click **Accept** next to the Puppet Agent VM name.

	_Accept Puppet Agent Node Request_

##### Open a Remote Desktop to the Puppet Agent VM #####
1. Switch to the browser instance at the **Management Portal** and go to **Virtual Machines**.

1. Select the Puppet Agent VM and click **Connect** in the bottom bar.

	![Connect to Agent VM](Images/connect-to-agent-vm.png?raw=true)
	
	_Connect to Puppet Agent VM_

1. In the file download message, click **Open**.

	![Open Remote Desktop to Agent VM](Images/open-remote-desktop-to-agent-vm.png?raw=true)
	
	_Open Remote Desktop_

1. In the Remote Desktop Connection dialog that will open, click **Connect**.

	![Connect To Agent VM Dlg1](Images/connect-to-agent-vm-dlg1.png?raw=true)
	
	_Connect to Puppet Agent VM_

1. When prompted for credentials, enter the userName and password in the **generalSettings** node in the configuration file and click **Ok**.

	![Connect To Agent VM Credentials](Images/connect-to-agent-vm-credentials.png?raw=true)

	_Enter credentials to Puppet Agent VM_

1. In the **Remote Desktop Connection** dialog that indicates the identity of the remote computer cannot be verified, click **Yes** to connect.

	![Connect to Agent VM accept no cert](Images/connect-to-agent-vm-accept-no-cert.png?raw=true)
	
	_Accept Security_
	
1. The Remote Desktop window to the Puppet Agent VM will open. Open a **Command Prompt with Puppet**.

	![Remote Desktop to Agent VM](Images/remote-desktop-to-agent-vm.png?raw=true)
	_Start Command Prompt with Puppet in Puppet Agent VM_

1. Enter the following **puppet** command to trigger the updates.

	````
	puppet agent --onetime
	````

1. Wait until the command completes the update.

	![Agent VM Run puppet agent onetime](Images/agent-vm-run-puppet-agent-onetime.png?raw=true)
	
	_Execute puppet agent command_

	>**Note:** Keep this Remote Desktop Connection open, as you will use it in Segment 2.
	
1. Switch to the browser at the **Puppet Dashboard** page and refresh the page by pressing **Ctrl+F5**. The list in the "All" tab shall contain the Master Puppet VM and the Puppet Agent VM as well.

	_Puppet Agent and Puppet Master displaying in Puppet Dashboard_

	>**Note:** It may take some time for the Puppet Agent VM to appear in the list. Until it does, continue refreshing the page by pressing **Ctrl+F5**.

1. Click the agent node corresponding to the Puppet Agent.

1. In the Node page, click Edit.

1. Add the node to the **Windows Servers** group.

1. Click Save.

## Demo ##

This demo is composed of the following segments: 

1. [Provisioning Puppet Resources](#segment1)

1. [Using the Puppet Dashboard](#segment2)

<a name="segment1" />
### Provisioning Puppet Resources ###
In this segment we will show how we could easily create Puppet masters from within Microsoft Azure by adding a puppet master image to our platform image repository.

1. Switch to the browser at the **Management Portal** page.

1.  Go to **Virtual Machines**.

	_Virtual Machines_

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

1. Switch to the browser displaying **Puppet Dashboard** in the browser and explain the home page.

	![Puppet Dashboard home page](Images/puppet-dashboard-home-page.png?raw=true)
	
	_Puppet Dashboard Home Page_

	> **Speaking Point:** This is the normal interface for Puppet Enterprise. You can see under **Nodes** that we have a small number of machines under management, both Windows and Linux. The **Daily run status** shows colored bars with the result achieved every time a puppet agent runs: green if the agent did not need to do any extra work to update its infrastructure; blue if it had to make an actual change to bring it into sync.
	
	>In this case we're going to make changes to our Windows Servers.
	
1. Select **Windows Servers** group from the **Groups** panel on the left.
	
	![Puppet Dashboard Groups select win servers](Images/puppet-dashboard-groups-select-win-servers.png?raw=true)

	_Select Windows Servers Group_
	
1. Switch to the Agent VM **Remote Desktop** connection and open the **Task Manager** by right-clicking the taskbar.

	![Agent VM open Task Manager](Images/agent-vm-open-task-manager.png?raw=true)
	
	_Open Task Manager in Agent VM_

1. Show that the VM is a regular Windows box that is using the original **Task Manager**.

	![Agent VM Task Manager](Images/agent-vm-task-manager.png?raw=true)

	> **Speaking point:** we have an example virtual machine that is running the standard version of the Task Manager. We've heard there's a better version of the Task Manager out there, that is part of the Microsoft Sysinternals toolset. So, we want to update all our Windows machines to have the Sysinternals tools installed, so we can use the Systinternal's Process Explorer instead of the default Task Manager. 
	
1. Switch back to **Puppet Dashboard** and click **Edit**.

	![Edit Puppet Dashboard](Images/edit-puppet-dashboard.png?raw=true)
	
	_Edit Puppet Dashboard_

1. Type **microsoft-sysinternals** in the **Classes** textbox and select the option from the autocomplete dropdown.

	![Puppet Dashboard add class](Images/puppet-dashboard-add-class.png?raw=true)

	> **Speaking point:** In Puppet, the class is esentially the way of referring to the code associated to the function I do. So by adding the microsoft-sysinternals module we associate the class with the work that needs to be done to all of the machines in the group. This will propagate out to your whole infrastracture, which may take around 30 minutes. If you have a hundred thousand machines under management you probably do not want all of them hitting your server at exactly the same time. In this case though, we have the system working on a relatively tighter timeline, so it is propagated faster.

1. Click **Update** to save changes.

	![Puppet Dashboard click update](Images/puppet-dashboard-click-update.png?raw=true)
	
	_Click Update_

1. Switch back to the Agent VM **Remote Desktop** and open a **Command Prompt with Puppet**.

1. Enter the following **puppet** command to trigger the updates.

	````
	puppet agent --onetime
	````

1. Wait until the command completes the update.

	![Agent VM Run puppet agent onetime](Images/agent-vm-run-puppet-agent-onetime.png?raw=true)
	
	_Execute puppet agent command_

1. Once completed, open the **Task Manager**. 

	![Agent VM open Task Manager](Images/agent-vm-open-task-manager.png?raw=true)
	
	_Open Task Manager in Agent VM_

1. After clicking **Accept** in the **Process Explorer** license dialog, the **Process Explorer** will open (instead of the classic **Task Manager**).
	
	![Process Explorer License Dialog](Images/process-explorer-license-dialog.png?raw=true)
	
	_Accept Process Explorer License_
	
	![Agent VM Process Explorer](Images/agent-vm-process-explorer.png?raw=true)
	
	_Process Explorer opens instead of Task Manager_
	
---

<a name="summary" />
## Summary ##

In this demo, you saw how to provision Puppet Resources using the Azure Management Portal and how to use the Puppet Dashboard to manage Puppet Agents.
