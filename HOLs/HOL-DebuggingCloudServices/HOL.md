<a name="HOLTop"></a>
# Debugging Applications in Windows Azure #
---
<a name="Overview"></a>
## Overview ##

Using Visual Studio, you can debug applications in your local machine by stepping through code, setting breakpoints, and examining the value of program variables. For Windows Azure applications, the compute emulator allows you to run the code locally and debug it using these same features and techniques, making this process relatively straightforward.

Ideally, you should take advantage of the compute emulator and use Visual Studio to identify and fix most bugs in your code, as this provides the most productive environment for debugging. Nevertheless, some bugs might remain undetected and will only manifest themselves once you deploy the application to the cloud. These are often the result of missing dependencies or caused by differences in the execution environment. The tools in Visual Studio can help you debug these errors if you enable remote debugging when you publish your service and then attach the debugger to a role instance.

However, there are some scenarios for which you should not use remote debugging. For example, if you start a remote debugging session with a cloud service hosted in a production environment, clients using the production service might be adversely affected. Instead, you need to rely on debugging information written to logs in order to diagnose and troubleshoot application failures. Windows Azure provides comprehensive diagnostic facilities that allow you to capture information from different sources, including Windows Azure application logs, IIS logs, failed request traces, Windows event logs, custom error logs, and crash dumps. The availability of this diagnostic information relies on the Windows Azure Diagnostics Monitor to collect data from individual role instances and transfer this information to Windows Azure storage for aggregation. Once the information is in storage, you can retrieve it and analyze it.

Sometimes an application may crash before it is able to produce logs that can help you determine the cause of the failure. With **IntelliTrace** debugging, a feature available in the Visual Studio 2013 Ultimate edition, you can log extensive debugging information for a role instance while it is running in Windows Azure. This lab shows you how to enable IntelliTrace for an Azure deployment to debug role start up failures.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will:

- Learn what features are available in Visual Studio to debug cloud services

- Use a simple **TraceListener** to log directly to table storage and a viewer to retrieve these logs

- Understand how to enable and use IntelliTrace to trace and debug applications

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Visual Studio Express 2013 for Web][1] or later

- [Windows Azure Tools for Microsoft Visual Studio 2.2][2] or later

- A Windows Azure subscription
	- Sign up for a [Free Trial](http://aka.ms/watk-freetrial)
	- If you are a Visual Studio Professional, Test Professional, Premium or Ultimate with MSDN or MSDN Platforms subscriber, activate your [MSDN benefit](http://aka.ms/watk-msdn) now to start development and test on Windows Azure.
	- [BizSpark](http://aka.ms/watk-bizspark) members automatically receive the Windows Azure benefit through their Visual Studio Ultimate with MSDN subscriptions.
	- Members of the [Microsoft Partner Network](http://aka.ms/watk-mpn) Cloud Essentials program receive monthly credits of Windows Azure at no charge.


[1]: http://www.microsoft.com/visualstudio/
[2]: http://www.microsoft.com/windowsazure/sdk/

>**Note:** This lab was designed for Windows 8.

<a name="Setup"/>
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.

1. Right-click the **Setup.cmd** file and click **Run as administrator**. This will launch the setup process that will install the Visual Studio code snippets for this lab.

1. If the User Account Control dialog is shown, confirm the action to proceed.
 
>**Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="UsingCodeSnippets"/>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2013 to avoid having to add it manually.

---
<a name="Exercises"/>
## Exercises ##

This hands-on lab includes the following exercises:

- [Debugging a Cloud Service in Visual Studio](#Exercise1)

- [Adding Diagnostic Trace](#Exercise2)

- [Using IntelliTrace to Diagnose Role Start-Up Failures](#Exercise3) (Optional for Visual Studio 2013 Ultimate edition)

Estimated time to complete this lab: **40 minutes**.

<a name="Exercise1"></a>
### Exercise 1: Debugging a Cloud Service in Visual Studio ###

In this exercise, you will debug a simple cloud service application locally from Visual Studio by using the Windows Azure compute emulator. Then, you will learn how to use the Visual Studio tools to attach the debugger to the application when it is running in Windows Azure.

The cloud service application that you will use for this exercise simulates an online car insurance policy calculator. It has a single form where users can enter details about their vehicle and then submit the form to obtain an estimate on their insurance premium. Behind the scenes, the controller action that processes the form uses a separate assembly to calculate premiums based on the input from the user. The assembly contains a bug that causes it to raise an exception for input values that fall outside the expected range.

<a name="Ex1Task1"></a>
#### Task 1 - Debugging the Fabrikam Insurance Application on the Local Computer ####

In this task, you will build and run the Fabrikam Insurance application in the Windows Azure compute emulator so you can test and debug the cloud service before you deploy it.

1. Open Visual Studio in elevated administrator mode by right clicking the **Microsoft Visual Studio Express 2013 for Web** shortcut and choosing **Run as administrator**.

1. In the **File** menu, choose **Open Project...**, browse to **Ex1-DebuggingInVisualStudio** in the **Source** folder of the lab, select **Begin.sln** in the **Begin** folder and then click **Open**.

1. Set the start action of the project. To do this, in **Solution Explorer**, right-click the **FabrikamInsurance** project and then select **Properties**. In the properties window, switch to the **Web** tab and then, under **Start Action**, select the **Specific Page** option. Leave the page value blank.

 	![Configuring the start action of the project](Images/configuring-the-start-action-of-the-project.png?raw=true "Configuring the start action of the project")

	_Configuring the start action of the project_

1. You are now ready to test the Windows Azure Project application. To launch the application in the compute emulator, set the **FabrikamInsurance.Azure** cloud project as the Startup project and press **F5**. Wait until the deployment completes and the browser opens the **Auto Insurance Quotes** page.

1. To explore its operation, complete the form by choosing any combination of values from the **Vehicle Details** drop down lists and then click **Calculate** to obtain a quote for the insurance premium. Notice that after you submit the form, the page refreshes and shows the calculated amount.

	![Exploring the Fabrikam Insurance application](Images/exploring-the-fabrikam-insurance-application.png?raw=true "Exploring the Fabrikam Insurance application")
  
	_Exploring the Fabrikam Insurance application_

1. Once you have verified that everything works in the compute emulator, you will now cause an exception by making the application process bad data that it does not handle correctly. To do this, change the values used for the calculation by setting the **Make** to "_PORSCHE"_ and the **Model** to "_BOXSTER (BAD DATA)"_.

  	![Choosing make and model for the insurance premium calculation](Images/choosing-make-and-model-for-the-insurance-premium-calculation.png?raw=true "Choosing make and model for the insurance premium calculation")

	_Choosing make and model for the insurance premium calculation_


1. Click **Calculate** to re-submit the form with new values. Notice that an unhandled exception occurs and execution halts in the Visual Studio debugger at the line that caused the error. 

 	![Unhandled exception in the application caused by bad data](Images/unhandled-exception-in-the-application-caused-by-bad-data.png?raw=true "Unhandled exception in the application caused by bad data")

	_Unhandled exception in the application caused by bad data_

	>**Note:** Within the Visual Studio debugger, you are able to step through code, set breakpoints, and examine the value of program variables. Debugging applications hosted in the compute emulator provides the same experience that you typically have when debugging other programs to which you can attach the Visual Studio debugger. Using the debugger under these conditions is covered extensively and will not be explored here. For more information, see [Debugging in Visual Studio](http://msdn.microsoft.com/en-us/library/sc65sadd.aspx).

1. Press **F5** to continue execution and let ASP.NET handle the exception. Notice that the unhandled exception handler provides details about the exception, including the line in the source code that raised the exception. 

 	![ASP.NET default unhandled exception handler ](Images/aspnet-default-unhandled-exception-handler.png?raw=true "ASP.NET default unhandled exception handler ")
  
	_ASP.NET default unhandled exception handler_

	>**Note:** Unhandled exceptions are typically handled by ASP.NET, which can report the error in its response including details about an error and the location in the source code where the exception was raised. However, for applications that are available publicly, exposing such information is not recommended to prevent unnecessary disclosure of internal details about the application that may compromise its security. Instead, errors and other diagnostics output should be written to a log that can only be retrieved after proper authorization. 

	>You can configure how information is displayed by ASP.NET when an unhandled error occurs during the execution of a Web request. 

	>In this case, the unhandled exception error page includes full details for the error because the default mode for the **customErrors** element is _remoteOnly_ and you are accessing the page locally. When you deploy the application to the cloud and access it remotely, the page shows a generic error message instead.

1. Go back to Visual Studio and press **SHIFT + F5** to stop debugging and shut down the application.

<a name="Ex1Task2"></a>
#### Task 2 - Debugging the Fabrikam Insurance Application in Windows Azure ####

In this task, you will deploy the Fabrikam insurance application to Windows Azure and enable remote debugging when publishing the service. This will allow you to attach the Visual Studio debugger to the deployed cloud service.

1. In **Solution Explorer**, right-click the **FabrikamInsurance.Azure** cloud project and select **Publish**.
In the **Publish Windows Azure Application** dialog, click **Sign In** and sign in using the Microsoft account associated with your Windows Azure account. 

	![Sign in to see your subscriptions](Images/sign-in-to-see-your-subscriptions.png?raw=true)

	_Sign in to see your subscriptions_

1. The subscription drop down list will be populated with your subscriptions. Select a subscription and click **Next**.

1. In the **Common Settings** tab, click the drop down list labeled **Cloud Service** and select **\<Create New...\>**. Enter the name for your cloud service (e.g. _fabrikaminsuranceservice_), select the location of the service and click **OK**.

	![Create the cloud service](Images/create-the-cloud-service.png?raw=true)

	_Create the cloud service_

1. Make sure the cloud service you just created is selected. Then, click the drop down list labeled **Build configuration** and select **Debug**.

	> **Note:** For simplicity, you will debug the cloud service deployed to the **Production** environment. In a real-world scenario, you should work in the **Staging** environment when performing remote debugging.

	![Deployment common settings](Images/deployment-common-settings-debugging.png?raw=true)

	_Deployment common settings_

1. Click the **Advanced Settings** tab. In the list labeled **Storage account** select **\<Create New...\>**. Enter the name for your storage service (e.g. _fabrikaminsurancestorage_), select the location you selected for the cloud service and click **OK**.

	![Create the storage account](Images/create-the-storage-account.png?raw=true)

1. Make sure the storage service you just created is selected. Then, check the check box labeled **Enable Remote Debugger for all roles** and click **Next**.

	![Deployment advanced settings](Images/deployment-advanced-settings-debugging.png?raw=true)

1. Review the Summary information. If everything is OK, click **Publish** to start the deployment process.

	![Starting deployment](Images/starting-deployment-debugging.png?raw=true)

	_Starting deployment_

1. After you start a deployment, you can examine the Windows Azure activity log window to determine the status of the operation. If this window is not visible, in the **View** menu, point to **Other Windows**, and then select **Windows Azure Activity Log**.

	![Windows Azure Activity Log](Images/windows-azure-activity-log-debugging.png?raw=true)

	_Windows Azure Activity Log_

1. You can examine the **History** panel on the right of the **Windows Azure Activity Log** window to determine the status of the deployment. Wait for the deployment to complete (you should see a **Complete** message).

	![Deployment completed](Images/deployment-completed-debugging.png?raw=true)

	_Deployment completed_

1. In the **Windows Azure Activity Log** window, click **Open in Server Explorer** under the deployment entry of your cloud service.

	![Viewing the cloud service in Server Explorer](Images/viewing-the-cloud-service-in-server-explorer.png?raw=true)

	_Viewing the cloud service in Server Explorer_

1. In the **Windows Azure Compute** node in **Server Explorer**, right-click the node labeled as **FabrikamInsurance**, and then select **Attach Debugger**. 

	![Attach the debugger to the role](Images/attach-the-debugger-to-the-role.png?raw=true)

	_Attach the debugger to the role_

	> **Note:** When you select a role, the Visual Studio debugger attaches to each instance of that role. The debugger will break on a breakpoint for the first role instance that runs that line of code and meets any conditions of that breakpoint. If you select an instance, the debugger attaches to only that instance and breaks on a breakpoint only when that specific instance runs that line of code and meets the breakpoint's conditions.

1. The debugger automatically attaches to the appropriate host process for your role. Depending on what the role is, the debugger attaches to w3wp.exe, WaWorkerHost.exe, or WaIISHost.exe. To verify the process to which the debugger is attached, expand the instance node in **Server Explorer**.

	![Processes to which the debugger is attached](Images/process-the-debugger-is-attached-to.png?raw=true)

	_Processes to which the debugger is attached_

	> **Note:** To identify the processes to which the debugger is attached, open the **Processes** dialog box by selecting then **Debug** menu option, then pointing to **Windows** and then **Processes**.

1. Open a browser window, and navigate to **http://[your-cloud-service-name].cloudapp.net/**. Complete the form making sure that you choose "_PORSCHE"_ for the **Make** of the vehicle and "_BOXSTER (BAD DATA)_" for the **Model**.

1. Just like in the local scenario, an unhandled exception occurs and execution halts in the Visual Studio debugger at the line that caused the error. But this time, Visual Studio is debugging the code running in the remote process.

 	![Unhandled exception in the deployed application](Images/unhandled-exception-in-the-application-caused-by-bad-data.png?raw=true)

	_Unhandled exception in the deployed application_

	> **Note:** After the debugger attaches to a role or an instance, you can debug as usual. However, you should avoid long stops at breakpoints when remote debugging because Windows Azure treats a process that is stopped for longer than a few minutes as unresponsive and stops sending traffic to that instance. If you stop too much time, the Remote Debugging Monitor (_msvsmon_._exe_) that runs on your role instance shuts down and terminates your debugging session.

1. Press **F5** to continue execution and let ASP.NET handle the exception. Notice that this time the application shows a generic error page instead of the exception details that you saw earlier when running the application locally. This is because the default mode for the **customErrors** element is _remoteOnly_, and the application is now running in Windows Azure.

	![Generic error page](Images/generic-error-page.png?raw=true)

	_Generic error page_

1. To detach the debugger from all processes in your instance or role, press **SHIFT+F5** or right-click the role or instance node that you're debugging in **Server Explorer**, and then select **Detach Debugger**.

<a name="Exercise2"></a>
### Exercise 2: Adding Diagnostic Trace ###

Because Windows Azure Diagnostics is oriented towards operational monitoring and has to cater for gathering information from multiple role instances, it requires that diagnostic data first be transferred from local storage in each role to Windows Azure storage, where it is aggregated. The Diagnostics Monitor -a process running on your role instances- then performs scheduled transfers to copy logging data to Windows Azure storage at regular intervals.

In this exercise, you will debug the Fabrikam insurance application by configuring Windows Azure Diagnostics. To produce diagnostic data, you will instrument the application to write its trace information using standard methods in the System.Diagnostics namespace. Finally, you will take advantage of the tools provided by Visual Studio to retrieve and display the contents of the diagnostics table.

<a name="Ex2Task1"></a>
#### Task 1 - Adding Tracing Support to the Application ####

To debug the application once you deploy it to a production environment, you should write debugging information to the logs in order to diagnose an application failure.

In this task, you will configure Windows Azure Diagnostics in the Fabrikam Insurance application and instrument the application to trace diagnostics data.

1. If not already open, open Visual Studio in elevated administrator mode by right clicking the **Microsoft Visual Studio Express 2013 for Web** shortcut and choosing **Run as administrator**.

1. In the **File** menu, choose **Open Project**, browse to **Ex2-AddingDiagnosticTrace** in the **Source** folder of the lab, select **Begin.sln** in the **Begin** folder and then click **Open**.

	> **Note:** Alternatively you can continue working with the solution obtained after completing the previous exercise.

1. In **Solution Explorer**, right-click the **FabrikamInsurance** role located in the **FabrikamInsurance.Azure** project and select **Properties**. Choose the **Configuration** tab.

	![Opening properties for the role](Images/opening-properties-for-the-role.png?raw=true "Opening properties for the role")

	_Opening properties for the role_

1. In the **Diagnostics** section, make sure that the **Enable Diagnostics** check box is selected.

	![Making sure that diagnostics are enabled](Images/making-sure-that-diagnostics-are-enabled.png?raw=true "Making sure that diagnostics are enabled")

	_Making sure that diagnostics are enabled_

1. To customize the settings, select the **Custom plan** option, and then click the **Edit** button.

	> **Note:** Among the available options (**Errors only**, **All information**, and **Custom plan**), **Errors only** is the default option and requires the least amount of storage because it doesn’t transfer warnings or tracing messages. All information transfers the most information and is, therefore, the most expensive option.

	![Selecting the custom plan](Images/selecting-the-custom-plan.png?raw=true "Selecting the custom plan")

	_Selecting the custom plan_

1. In the **Application logs** tab of the **Diagnostics configuration** dialog select **All** as **Log level** in order to log all trace messages (not only the application errors).

	![Selecting the All log level for Application logs](Images/selecting-the-all-log-level-for-application-l.png?raw=true "Selecting the All log level for Application logs")

	_Selecting the All log level for Application logs_

1.  Make sure that the **Transfer period** is set to **1 minute**. The Diagnostic Monitor will transfer the application logs from the role instances to Windows Azure storage in this time interval.

	![Application logs transfer period](Images/application-logs-transfer-period.png?raw=true)

1. Click **OK** to close the dialog.

1. To instrument the application and write diagnostics information to the error log, add a global error handler to the application. To do this, open the **Global.asax.cs** file located in the **FabrikamInsurance** project and insert the following method into the **MVCApplication** class.

	(Code Snippet - WindowsAzureDebugging-Ex2-Application_Error-CS)
	<!-- mark:5-9 -->
	````C#
	public class MvcApplication : System.Web.HttpApplication
	{
	  ...
	
	  protected void Application_Error()
	  {
	    var lastError = Server.GetLastError();
	    System.Diagnostics.Trace.TraceError(lastError.Message);
	  }
	}
	````

	> **Note:** The **Application_Error** event is raised to catch any unhandled ASP.NET errors while processing a request. The event handler shown above retrieves a reference to the unhandled exception object using **Server.GetLastError** and then uses the **TraceError** method of the **System.Diagnostics.Trace** class to log the error message. 

	>Note that the **Trace** object outputs the message to each listener in its **Listeners** collection, which by default contains an instance of the **DefaultTraceListener** class. The role project templates provided by the Windows Azure Tools for Microsoft Visual Studio already include the settings required in the **Web.config** or **App.config** file  of the role to use the **DiagnosticMonitorTraceListener** to write to the Windows Azure diagnostics log. When using this type of trace listener, the logs are gathered locally in each role. The Diagnostic Monitor then copies the information to the storage service account configured in the _ServiceConfiguration.cscfg_ file (transferred trace data is located in the _WADLogsTable_ of the storage account).

1. Open the **QuoteController.cs** file in the **Controllers** folder of the **FabrikamInsurance** project and add the following method. 

	(Code Snippet - WindowsAzureDebugging-Ex2-Controller OnException method-CS)
	<!-- mark:6-9 -->
	````C#
	[HandleError]
	public class QuoteController : Controller
	{
	  ...
	
	  protected override void OnException(ExceptionContext filterContext)
	  {
	    System.Diagnostics.Trace.TraceError(filterContext.Exception.Message);
	  }
	}
	````

	> **Note:** The **OnException** method is called when an unhandled exception occurs during the processing of an action in a controller. For MVC applications, unhandled errors are typically caught at the controller level, provided they occur during the execution of a controller action and that the action (or controller) has been decorated with a **HandleErrorAttribute**. To log exceptions in controller actions, you need to override the **OnException** method of the controller because the **Application_Error** is bypassed when the error-handling filter catches the exceptions. 

	> By default, when an action method with the **HandleErrorAttribute** attribute throws any exception, MVC displays the **Error** view that is located in the **~/Views/Shared** folder.

1. In addition to error logging, tracing can also be useful for recording other significant events during the execution of the application. For example, for registering whenever a given controller action is invoked. To show this feature, insert the following (highlighted) tracing statement at the start of the **Calculator** method to log a message whenever this action is called. 

	<!-- mark:7 -->
	````C#
	public class QuoteController : Controller
	{
	  ...
	  
	  public ActionResult Calculator()
	  {
	  	System.Diagnostics.Trace.TraceInformation("Calculator called...");
	    QuoteViewModel model = new QuoteViewModel();
	    this.PopulateViewModel(model, null);
	    return this.View(model);
	  }
	
	  ...
	}
	````

1. Similarly, add a tracing statement to the **About** action, as shown (highlighted) below.
	
	<!-- mark:7 -->
	````C#
	public class QuoteController : Controller
	{
	  ...
	
	  public ActionResult About()
	  {
	    System.Diagnostics.Trace.TraceInformation("About called...");
	    return this.View();
	  }
	
	  ...
	}
	````

<a name="Ex2Verification"></a>
#### Verification ####

You are now ready to re-deploy and run the solution in Windows Azure. At this point, the application is ready for tracing and configured to collect and transfer all its diagnostics output to a table in storage services. You will use the Visual Studio tools to examine the diagnostics data generated by the application.

1. Deploy the application again, making sure to select the cloud service and the storage account created in Exercise 1.

	> **Note:** Because the slot that you chose is already occupied by the previous deployment, Visual Studio warns you and asks for confirmation before replacing it. Click **Replace** to overwrite the current deployment.
	>
	> ![Replacing the current deployment](Images/replacing-the-current-deployment.png?raw=true "Replacing the current deployment")
	>
	> _Replacing the current deployment_ 

1. Wait until the deployment completes and navigate to the deployed cloud service. In the browser window, complete the form making sure that you choose "_PORSCHE"_ for the **Make** of the vehicle and "_BOXSTER (BAD DATA)_" for the **Model** (the generic error page should be displayed).

1. Switch back to Visual Studio, open **Server Explorer** and right-click the **FabrikamInsurance** role instance under the **Cloud Services** node. Then select **View Diagnostic Data**.

	![Clicking View Diagnostics Data](Images/clicking-view-diagnostics-data.png?raw=true "Clicking View Diagnostics Data")
	
	_Clicking View Diagnostics Data_

1. In the **Diagnostics summary** window, expand the **Windows Azure application logs** section in order to see the list of messages. A log entry corresponding to the error message for the unhandled exception should appear. If the error message is not displayed yet, you might have to wait for the transfer period to elapse and then click the **Refresh** button to update the log list.
	
	![Showing the error trace message in the Diagnostics summary](Images/showing-the-error-trace-message.png?raw=true "Showing the error trace message in the Diagnostics summary")

	_Showing the exception message in the Diagnostics summary_

1. To view the output from other informational trace messages, return to the browser window and click **About** followed by **Quotes** to execute both actions in the controller. Recall that you inserted trace messages at the start of each method.

1. Go back to the **Diagnostics Summary** report. Note that you will not see new entries as only errors are displayed in the summary. In order to see the informational trace messages you need to click **View all data** below the **Windows Azure application log** table.

	![Clicking View all data](Images/clicking-view-all-data.png?raw=true "Clicking View all data")

	_Clicking View all data_

1. The Windows Azure Storage Table viewer should open, loading the content of the **WADLogsTable** table. Locate the log entries with the information trace messages. If the information messages are not displayed yet, you might have to wait for the transfer period to elapse and then click the **Refresh** icon from the toolbar reload the table contents.

	![Showing informational trace messages for the controller actions](Images/showing-informational-trace-messages.png?raw=true)

	_Showing informational trace messages for the controller actions_


<a name="Exercise3"></a>
### Exercise 3: Using IntelliTrace to Diagnose Role Start-Up Failures ###

> **Note:** This exercise is optional, since IntelliTrace is only available for **Visual Studio 2013 Ultimate Edition**. For more information on specific Visual Studio 2013 features, compare versions [here](http://www.microsoft.com/visualstudio/eng/products/compare).

IntelliTrace provides the ability to collect data about an application while it is executing. When you enable IntelliTrace, it records key code execution and environment data and then allows you to replay this data from within Visual Studio, stepping through the same code that executes in the cloud.

When you deploy a service to Windows Azure from within Visual Studio, you can enable IntelliTrace debugging to package the necessary IntelliTrace files along with an agent that Visual Studio will communicate with to retrieve the IntelliTrace data. Once enabled, IntelliTrace operates in the background, collecting information about the running service.

You can customize the basic IntelliTrace configuration specifying, which events to log, whether to collect call information, which modules and processes to collect logs for, and how much space to allocate to the recording (the default size is 250 MB).

The collected information is saved to an IntelliTrace file, which you can open later to start troubleshooting the problem. This information lets you step back in time to see what happened in the application and which events led to a crash.

In this exercise, you explore the use of IntelliTrace to diagnose a role start-up failure while deploying the Fabrikam Insurance application to Windows Azure.

> **Important:** IntelliTrace debugging is intended for debug scenarios only, and should not be used for a production deployment. 

<a name="Ex3Task1"></a>
#### Task 1 - Deploying the Application with IntelliTrace Enabled ####

In this task, you will publish to Windows Azure the FabrikamInsurance application directly from Visual Studio, with the IntelliTrace feature enabled.

1. If it is not already open, launch **Microsoft Visual Studio Ultimate 2013**.

1. In the **File** menu, choose **Open | Project/Solution...** and browse to **Ex3-DebuggingWithIntelliTrace\Begin** in the **Source** folder of the lab. Select **Begin.sln** and click Open.

1. In the **Solution Explorer**, right-click the **FabrikamInsurance.Azure** cloud project and select **Publish**.

	![Publishing the FabrikamInsurance.Azure project](Images/publishing-the-fabrikaminsuranceazure-project.png?raw=true "Publishing the FabrikamInsurance.Azure project")

	_Publishing the FabrikamInsurance.Azure project_

1. In the **Common Settings** tab, select the cloud service that you created during the first exercise of this lab.
 
	![Deployment Common Settings](Images/deployment-common-settings.png?raw=true "Deployment Common Settings")
	
	_Deployment Common Settings_

1. Click **Advanced Settings** tab. In the list labeled **Storage account** select the storage service that you created during the first exercise of this lab. Check the check box labeled **Enable IntelliTrace** and click **Next**.
 
	![Deployment Advanced Settings](Images/deployment-advanced-settings.png?raw=true "Deployment Advanced Settings")
	
	_Deployment Advanced Settings_

1. Review the Summary information. If everything is OK, click **Publish** to start the deployment process.

	![Starting Deployment](Images/start-deployment.png?raw=true "Starting Deployment")

	_Starting Deployment_

	> **Note:** Because the slot that you chose is already occupied by the previous deployment, Visual Studio warns you and asks for confirmation before replacing it. Click **Replace** to overwrite the current deployment.
	>
	> ![Replacing the current deployment](Images/replacing-the-current-deployment.png?raw=true "Replacing the current deployment")
	>
	> _Replacing the current deployment_ 

1. After you start a deployment, you can examine the Windows Azure activity log window to determine the status of the operation. If this window is not visible, in the **View** menu, point to **Other Windows**, and then select **Windows Azure Activity Log**.

	![Windows Azure Activity Log](Images/azure-activity-log.png?raw=true "Windows Azure Activity Log")

	_Windows Azure Activity Log_

1. You can examine the **History** panel on the right of the **Windows Azure Activity Log** window to determine the status of the deployment. Notice that the FabrikamWorker is in an **unknown** state.

	![Deployment Operation Details](Images/deployment-operation-details.png?raw=true "Deployment Operation Details")

	_Viewing detailed information about a deployment operation_

1. In the **Windows Azure Activity Log** window, click **Open in Server Explorer**.

	![Viewing the cloud service in Server Explorer](Images/cloud-service-in-server-explorer.png?raw=true "Viewing the cloud service in Server Explorer")

	_Viewing the cloud service in Server Explorer_

1. In the **Cloud Services** node, examine the role instance status of the hosted service where you deployed the **FabrikamInsurance** application. Notice that the label for the deployment slot indicates that IntelliTrace is enabled for this deployment and that the **FabrikamWorker** instance status is shown as “**Unknown**” here too.

	![Fabrikam Insurance Service Instance](Images/service-instance-busy.png?raw=true "Fabrikam Insurance Service Instance")

	_Fabrikam Insurance Service Instance_

<a name="Ex3Task2" />
#### Task 2 - Examining the IntelliTrace Logs to Determine the Cause of a Failure ####

In the previous task, the role failed to start due to an unknown reason. In this task, you will use IntelliTrace to determine what caused the failure.

1. In the **Cloud Services** node inside the **Server Explorer** windows, select the hosted service where you deployed the **FabrikamInsurance** application and expand it until you found the **FabrikamWorker** instance.

1. Now, right-click the slot node, labeled as **Instance 0 (Unknown)** in **Server Explorer**, and then select **View IntelliTrace Logs** to download the information to your workstation. After you do this, notice that Visual Studio creates a new IntelliTrace entry in the **Windows Azure Activity Log** window to display the progress of the download operation.

	![Downloading the IntelliTrace logs](Images/downloading-the-intellitrace-logs.png?raw=true "Downloading the IntelliTrace logs")

	_Downloading the IntelliTrace logs_

1. Wait for the download to complete, which may take several minutes. While this is happening, you can expand the corresponding entry in the **Windows Azure Activity Log** window and examine the **History** panel on the right to monitor the progress of the operation.

	![IntelliTrace operation history log](Images/intellitrace-operation-history-log.png?raw=true "IntelliTrace operation history log")

	_IntelliTrace operation history log_

	> **Note:** When you request the IntelliTrace logs, a snapshot of the information collected so far is uploaded from the VM instance where the role is running to your storage account and then downloaded to a disk file on your local computer. Once the log is transferred successfully, it is deleted from the storage.

1. After the download completes, Visual Studio automatically opens the IntelliTrace log file and displays it in a window. The **IntelliTrace Summary** window is divided into several sections that contain diagnostics information collected for the deployment.

	![IntelliTrace summary window](Images/intellitrace-summary-window.png?raw=true "IntelliTrace summary window")

	_IntelliTrace summary window_

	The sections available in the IntelliTrace summary window are:

	**Exception Data** - lists unhandled exceptions that occur during the data collection period with details about the exception type, the exception message, the number of exceptions of that kind that were raised, and the time of the newest exception. Selecting an entry in the list displays the corresponding call stack for the exception.

	If an exception occurs in your code, you can select the exception in the list and then click **Start Debugging** to open the appropriate source file in Visual Studio with the cursor placed on the line of code that raised the exception.

	![Exception Data section](Images/exception-data-section.png?raw=true "Exception Data section")

	_Exception Data section_

	**System Info** - shows information about the virtual machine environment where the diagnostics data was collected, including the computer name, operating system and CLR versions, physical memory and virtual memory available, number of processors, time zone, among other details.

	![System Info section](Images/system-info-section.png?raw=true "System Info section")

	_System Info section_

	**Threads List** - shows every thread active during the diagnostics collection period, with each thread showing its ID, a name, if available, and the starting and ending times of the thread.

	![Threads List section](Images/threads-list-section.png?raw=true "Threads List section")

	_Threads List section_

	**Modules** - lists every module loaded in memory, including the name of the module and the path from where it was loaded. This information can be useful to debug assembly-loading issues.

	![Modules section](Images/modules-section.png?raw=true "Modules section")

	_Modules section_

	> **Note:** For the current deployment, you used default settings to define the events captured by IntelliTrace.

	>To configure which events should be included in the IntelliTrace log, during the Publishing operation, click **Settings** in **Windows Azure Publish Settings** | **Advanced Settings** to open the **IntelliTrace Settings** window.

	>	![Configuring IntelliTrace settings](Images/configuring-intellitrace-settings.png?raw=true "Configuring IntelliTrace settings")

	> _Configuring IntelliTrace settings before publishing a service package_

1. To determine what caused the role to fail during start up in the previous task, expand the **Exception** **Data** section on the summary page and examine the list of exceptions.

	![Viewing exception data](Images/viewing-exception-data.png?raw=true "Viewing exception data")

	_Viewing exception data in the IntelliTrace summary window_

	In general, this list contains multiple exceptions, some of which may be ignored as they are handled by the runtime environment and do not cause the role to crash. There is no precise rule regarding which exceptions are normal and can be ignored, so you will typically review the list to identify potentially fatal exceptions. You will find that examining IntelliTrace logs for successful deployments will enhance your ability to discriminate significant entries in this list.

	Notice that the list includes a **FabrikamWorker.CustomException** with the message "_This is an example error._". This is a custom exception that was created to easily showcase IntelliTrace.

	To fix the problem, just remove the statement that throw the exception inside the **Run** method of the **WorkerRole** class.

1. Select the **FabrikamWorker.CustomException** exception and click **Debug Newest Exception in Group** in order to locate where the exception is being thrown.

	![Debugging the CustomException](Images/debugging-the-customexception.png?raw=true "Debugging the CustomException")

	_Debugging the CustomException_

1. The **WorkerRole.cs** file should open in Debugging mode, highlighting the statement where the CustomException is throwed.

	![Debugging the Worker Role](Images/debugging-the-worker-role.png?raw=true "Debugging the Worker Role")

	_Debugging the Worker Role_

1. In the IntelliTrace window, locate the highlighted event and click **Calls View** in order to switch to **IntelliTrace Calls View**.

	![Switching to the Calls View](Images/switching-to-the-calls-view.png?raw=true "Switching to the Calls View")

	_Switching to the Calls View_

1. In **IntelliTrace Calls View**, click one of the calls above the one highlighted in order to move the debugger position to other statements.

	![Changing to other statement using the IntelliTrace Calls View](Images/changing-to-other-statement-using-the-intelli.png?raw=true "Changing to other statement using the IntelliTrace Calls View")

	_Changing to other statement using the IntelliTrace Calls View_

1. Stop debugging by pressing **Shift + F5**.

1. Cancel and remove the failing deployment to Windows Azure. In the Windows Azure activity log window, right-click the deployment and select **Cancel and remove**.

	![Cancel the failing deployment](Images/cancel-failing-deployment.png?raw=true "Cancel the failing deployment")

	_Cancel the failing deployment_

## Next Steps ##

To learn more about debugging applications in Windows Azure, please refer to the following articles:

**Technical Reference**

This is a list of articles that expands the information on the technologies explained on this lab:

- [Debugging in Visual Studio](http://msdn.microsoft.com/en-us/library/sc65sadd.aspx>): extensively covers the debugging functions integrated with Visual Studio.

- [Debugging a Cloud Service with Emulator Express](http://msdn.microsoft.com/en-us/library/windowsazure/dn339018.aspx): by using Emulator Express, you can test and debug a cloud service without running Visual Studio as an administrator.

- [Initialize or Change Windows Azure Diagnostics Configuration](http://msdn.microsoft.com/en-us/library/windowsazure/hh411537.aspx): describes the different aspects of the Diagnostic configuration, like the Diagnostics data sources, the Diagnostics Monitor settings and the storage of the Diagnostics data.

- [Create and Use Performance Counters in a Windows Azure Application](http://msdn.microsoft.com/en-us/library/windowsazure/hh411542.aspx): you can use performance counters to measure the health of your Windows Azure application. You can collect data from existing performance counters or you can create custom performance counters to collect additional data.

- [How to: Configure Trace Switches](http://msdn.microsoft.com/en-us/library/t06xyy08.aspx): by placing **Trace Switches** in your code, you can control whether tracing occurs and how extensive it is. This lets you monitor the status of your application in a production environment.

- [Debug Your App by Recording Code Execution with IntelliTrace](http://msdn.microsoft.com/en-us/library/dd264915.aspx): explains how to you use IntelliTrace to record and trace your code's execution history.

**Development**

This is a list of developer-oriented articles related to debugging applications in Windows Azure:

- [Remotely Change the Diagnostic Monitor Configuration](http://msdn.microsoft.com/en-us/library/windowsazure/gg432992.aspx): after you have deployed a cloud service you can remotely change the configuration of the diagnostic monitor from code running in an application outside of Windows Azure using the [DeploymentDiagnosticManager](http://msdn.microsoft.com/en-us/library/windowsazure/microsoft.windowsazure.diagnostics.management.deploymentdiagnosticmanager.aspx) class.

- [Trace the Flow of Your Windows Azure Application](http://msdn.microsoft.com/en-us/library/windowsazure/hh411529.aspx): you can add tracing and debugging instrumentation to your Windows Azure application when you develop it, and you can use that instrumentation both while you are developing the application and after you have deployed it.

- [API Reference for IntelliTrace Extensibility](http://msdn.microsoft.com/en-us/library/ee661590.aspx): provides information to use IntelliTrace extensibility APIs to read and decode .itrace files in your application.

<a name="Summary"></a>
## Summary ##

By completing this hands-on lab, you learnt how to apply simple debugging techniques to troubleshoot your Windows Azure application locally and once you deploy it to the cloud. You saw how to use the Windows Azure Tools for Visual Studio to configure Windows Azure Diagnostics.

Using IntelliTrace, you quickly diagnosed a role that failed to start due to an exception.
