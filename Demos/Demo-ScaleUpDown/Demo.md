<a name="title" />
# Scaling Up or Down #

---
<a name="Overview" />
## Overview ##

One of the key benefits of cloud computing is the ability to increase or decrease the number of instances your application is using. In this demonstration you will show how using some simple PowerShell scripts and a task scheduler job you can easily scale an application up or demand on a schedule. 

The scenario could be an application where the majority of the users use the application between 9AM and 5PM and this scheduled scale up and down could be used to save money. 


<a name="technologies" />
### Key Technologies ###

- Microsoft Azure subscription - you can sign up for free trial [here][1]
- Microsoft Azure Virtual Machines 
- [Microsoft Azure PowerShell Cmdlets][2]

[1]: http://bit.ly/WindowsAzureFreeTrial
[2]: http://go.microsoft.com/?linkid=9811175&clcid=0x409



<a name="setup" />
### Prerequisites ###

- Complete the [Migrating a Web Farm](https://github.com/WindowsAzure-TrainingKit/DEMO-MigratingWebFarm/blob/master/Demo.md) demo.

<a name="segment1" />
### Configuring the PowerShell Scripts ###

1. Modify **Source\Assets\add.ps1** and **Source\Assets\remove.ps1** to use the subscription name in your PowerShell settings and the service name you created in the **Migrating a Web Farm** demo. 

1. Create a **C:\temp** folder and copy the _add.ps1_ and _remove.ps1_ scripts into it.

1. Open **Task Scheduler** from _Administrative Tools_. Create a task by right-clicking **Task Scheduler (Local)** and selecting **Create Task**.

	![create-task](Images/create-task.png?raw=true)

1. Name the task **Add VMS**. Ensure that _Run whether user is logged on or not_ is selected.

	![add-task](Images/add-task.png?raw=true)

1. On the **Triggers** tab add a new trigger to run at **8:30AM**.

	![add-trigger](Images/add-trigger.png?raw=true)

1. On the **Actions** tab add a new action that specifies _powershell.exe_ as the **Program/script** and _"& 'c:\temp\add.ps1'"_ as the **arguments**.

	> **Note:** ensure that you have copied the add.ps1 and remove.ps1 scripts to a C:\temp folder.

	![add-action](Images/add-action.png?raw=true)

1. When you click **OK** in the **Create task** window, you will be prompted for credentials to run the task. This way, it will be able to run without a user logged in.

1. Repeat the same steps and create a new task called **Remove VMS** that calls _remove.ps1_ at **5:30PM**.

	> **Note:** ensure that you copied the add.ps1 and remove.ps1 scripts to C:\temp folder, so the Task Scheduler can run the scheduled tasks.

1. Demonstrate that you can run the _remove.ps1_ task manually and it will export the virtual machine settings to **C:\temp** and remove them. Once completed, you can run the _add.ps1_ script to re-import the virtual machines.
