<a name="demo2" />
# Puppet Labs Demo#

## Overview ##

In this demo...

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


## Demo ##

This demo is composed of the following segments: 

1. [Provisioning Puppet Resources](#segment1)

1. [Using the Puppet Dashboard](#segment2)

<a name="segment1" />
### Provisioning Puppet Resources ###

1. Open the Management Portal and go to **Virtual Machines**.

1. Click **New**, select **Virtual Machine** and then **From Gallery**.

1. Select **Puppet Labs** from the left panel and show the **Puppet Master** image.

1. Select **Windows Server** from the left panel and select the **Windows Server 2012 R2 Datacenter** image. Click the right arrow to continue.

1. Enter a virtual machine name, set an administrator username and its password. Click the right arrow to continue.

1. Leave the default values and click the right arrow to continue.

1. Check the **Puppet Enterprise Agent** option and type the address the of **Puppet Master** instance.

1. Close the wizard without completing it.

<a name="segment2" />
### Using the Puppet Dashboard ###

1. Switch to **Puppet Dashboard** in the browser and explain the home page.

1. Select **Windows Servers** group from the left **Groups** panel.

1. Switch to the Agent VM **Remote Desktop** connection and open the **Task Manager** by right-clicking the taskbar.

1. Show that the VM is using the original **Task Manager**.

1. Switch back to **Puppet Dashboard** and click **Edit**.

1. Type **microsoft-sysinternals** in the **Classes** textbox and select the option from the autocomplete dropdown.

1. Click **Update** to save changes.

1. Go back to the **Remote Desktop** and open a **Command Prompt**.

1. Enter the following **puppet** command to trigger the updates.

	````Command Prompt
	puppet agent --onetime
	````

1. Wait until the command completes the update.

1. Once completed, open the **Task Manager**. It will open the **Process Explorer** instead of the classic **Task Manager**.
	
---

<a name="summary" />
## Summary ##

In this demo, you saw how to 