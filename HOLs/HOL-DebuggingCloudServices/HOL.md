<a name="HOLTop"></a>
# Debugging Applications in Windows Azure #
---
<a name="Overview"></a>
## Overview ##

Using Visual Studio, you can debug applications in your local machine by stepping through code, setting breakpoints, and examining the value of program variables. For Windows Azure applications, the compute emulator allows you to run the code locally and debug it using these same features and techniques, making this process relatively straightforward.

Ideally, you should take advantage of the compute emulator and use Visual Studio to identify and fix most bugs in your code, as this provides the most productive environment for debugging. Nevertheless, some bugs might remain undetected and will only manifest themselves once you deploy the application to the cloud. These are often the result of missing dependencies or caused by differences in the execution environment. For addition information on environment issues, see [Differences between the Compute Emulator and Windows Azure](http://msdn.microsoft.com/en-us/library/windowsazure/gg432960.aspx).

Once you deploy an application to the cloud, you are no longer able to attach a debugger and instead, need to rely on debugging information written to logs in order to diagnose and troubleshoot application failures. Windows Azure provides comprehensive diagnostic facilities that allow capturing information from different sources, including Windows Azure application logs, IIS logs, failed request traces, Windows event logs, custom error logs, and crash dumps. The availability of this diagnostic information relies on the Windows Azure Diagnostics Monitor to collect data from individual role instances and transfer this information to Windows Azure storage for aggregation. Once the information is in storage, you can retrieve it and analyze it.

Sometimes an application may crash before it is able to produce logs that can help you determine the cause of the failure. With IntelliTrace debugging, a feature available in the Visual Studio 2012 Ultimate edition, you can log extensive debugging information for a role instance while it is running in Windows Azure. The lab discusses how to enable IntelliTrace for an Azure deployment to debug role start up failures.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will:

- Learn what features and techniques are available in Visual Studio and Windows Azure to debug applications once deployed to Windows Azure.

- Use a simple **TraceListener** to log directly to table storage and a viewer to retrieve these logs.

- Understand how to enable and use IntelliTrace to trace and debug applications.

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- IIS (with ASP.NET, WCF HTTP Activation)

- [Visual Studio Express 2012 for Web][1] or greater.

- [Windows Azure Tools for Microsoft Visual Studio 1.8][2]

- [SQL Server 2012 Express Edition (or later)][3]

- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

[1]: http://www.microsoft.com/visualstudio/
[2]: http://www.microsoft.com/windowsazure/sdk/
[3]: http://www.microsoft.com/express/sql/download/

>**Note:** This lab was designed for Windows 8

<a name="Setup"/>
### Setup ###
In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.

1. Right-click the **Setup.cmd** file and click **Run as administrator**. This will launch the setup process that will configure your environment and install the Visual Studio code snippets for this lab.

1. If the User Account Control dialog is shown, confirm the action to proceed.
 
>**Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="UsingCodeSnippets"/>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2012 to avoid having to add it manually.

---
<a name="Exercises"/>
## Exercises ##

This hands-on lab includes the following exercises:

- [Learn What Features and Techniques are Available in Visual Studio and Windows Azure](#Exercise1)

- [Adding Diagnostic Trace](#Exercise2)

- [Using IntelliTrace to Diagnose Role Start-Up Failures](#Exercise3) (Optional for Visual Studio 2012 Ultimate edition)

Estimated time to complete this lab: **40 minutes**.

<a name="Exercise1"></a>
### Exercise 1: Learn What Features and Techniques are Available in Visual Studio and Windows Azure ###

Because Windows Azure Diagnostics is oriented towards operational monitoring and has to cater for gathering information from multiple role instances, it requires that diagnostic data first be transferred from local storage in each role to Windows Azure storage, where it is aggregated. This requires programming scheduled transfers with the diagnostic monitor to copy logging data to Windows Azure storage at regular intervals, or else requesting a transfer of the logs on-demand. Moreover, information obtained in this manner provides a snapshot of the diagnostics data available at the time of the transfer. To retrieve updated data, a new transfer is necessary. When debugging a single role, and especially during the development phase, these actions add unnecessary friction to the process. To simplify the retrieval of diagnostics data from a deployed role, it is simpler to read information directly from Windows Azure storage, without requiring additional steps.

<a name="Ex1Task1"></a>
#### Task 1 - Exploring the Fabrikam Insurance Application ####

In this task, you build and run the Fabrikam Insurance application in the Web Development Server to become familiar with its operation.

1. Open Visual Studio in elevated administrator mode by right clicking the **Microsoft Visual Studio Express 2012 for Web** shortcut and choosing **Run as administrator**.

1. In the **File** menu, choose **Open Project**, browse to **Ex1-LoggingToAzureStorage** in the **Source** folder of the lab, select **Begin.sln** in the **Begin** folder and then click **Open**.

1. Set the start action of the project. To do this, in **Solution Explorer**, right-click the **FabrikamInsurance** project and then select **Properties**. In the properties window, switch to the **Web** tab and then, under **Start Action**, select the **Specific Page** option. Leave the page value blank.

 	![Configuring the start action of the project](Images/configuring-the-start-action-of-the-project.png?raw=true "Configuring the start action of the project")

	_Configuring the start action of the project_

1. Press **F5** to build and run the solution. The application should launch in the Web Development Server and open its **Auto Insurance Quotes** page in your browser.

1. To explore its operation, complete the form by choosing any combination of values from the **Vehicle Details** drop down lists and then click **Calculate** to obtain a quote for the insurance premium. Notice that after you submit the form, the page refreshes and shows the calculated amount.

	![Exploring the Fabrikam Insurance application](Images/exploring-the-fabrikam-insurance-application.png?raw=true "Exploring the Fabrikam Insurance application")
  
	_Exploring the Fabrikam Insurance application_

1. Go back to Visual Studio and press **SHIFT + F5** to stop debugging and shut down the application.

<a name="Ex1Task2"></a>
#### Task 2 - Running the Application as a Windows Azure Project ####

In this task, you create a new Windows Azure Project to prepare the application for deployment to Windows Azure.

  1. Add a new Windows Azure Project to the solution. To do this, in the **File** menu, point to **Add** and then select **New Project**. In the **Add** **New Project** dialog, expand **Visual C#** in the **Installed Templates** list and then select **Cloud**. Select the **Windows Azure Cloud Service** template. Set the **Name** of the project to **FabrikamInsuranceService** and accept the proposed location in the folder of the solution. Click **OK** to create the project.

	![creating-a-new-windows-azure-project-c](Images/creating-a-new-windows-azure-project-c.png?raw=true)

	_Creating a new Windows Azure Project_

  1. In the **New Windows Azure Project** dialog, click **OK** without adding any new roles to the solution.

  1. Now, in **Solution Explorer**, right-click the **Roles** node in the new **FabrikamInsuranceService** project, point to **Add**, and then select **Web Role Project in solution**. Then, in the **Associate with Role Project** dialog, select the **FabrikamInsurance** project, and click **OK**.

	![Associating the MVC application with the Windows Azure Project](Images/associating-the-mvc-application-with-the-azure-project.png?raw=true "Associating the MVC application with the Windows Azure Project")

	_Associating the MVC application with the Windows Azure Project_

  1. Now, add a role entry point to the MVC application. To do this, in **Solution Explorer**, right-click the **FabrikamInsurance** project, point to **Add**, and then select **Existing Item**. In the **Add Existing Item** dialog, browse to **Assets** in the **Source** folder of the lab. Inside this folder, select **WebRole.cs**, and then click **Add**.

	>**Note:** The **WebRole** class is a **RoleEntryPoint** derived class that contains methods that Windows Azure calls when it starts, runs, or stops the role. The provided code is the same that Visual Studio generates when you create a new Windows Azure Project.

  1. You are now ready to test the Windows Azure Project application. To launch the application in the compute emulator, set the **FabrikamInsuranceService** cloud project as the Startup project and press **F5**. Wait until the deployment completes and the browser opens to show its main page.

  1. Again, complete the entry form by choosing a combination of values from the drop down lists and then click **Calculate**. Ensure that you receive a valid response with the calculated premium as a result.

  1. Once you have verified that everything works in the compute emulator just as it did when hosted by the Web Development Server, you will now cause an exception by making the application process bad data that it does not handle correctly. To do this, change the values used for the calculation by setting the **Make** to "_PORSCHE"_ and the **Model** to "_BOXSTER (BAD DATA)"_.

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

<a name="Exercise2"></a>
### Exercise 2: Adding diagnostic trace ###

In this exercise, you debug a simple application by configuring a special trace listener that can write its output directly into a table in Windows Azure storage emulator.  To produce diagnostic data, you instrument the application to write its trace information using standard methods in the System.Diagnostics namespace. Finally, you create a simple log viewer application that can retrieve and display the contents of the diagnostics table.

The application that you will use for this exercise simulates an online auto insurance policy calculator. It has a single form where users can enter details about their vehicle and then submit the form to obtain an estimate on their insurance premium. Behind the scenes, the controller action that processes the form uses a separate assembly to calculate premiums based on the input from the user. The assembly contains a bug that causes it to raise an exception for input values that fall outside the expected range.

<a name="Ex2Task1"></a>
#### Task 1 - Adding Tracing Support to the Application ####

In the previous exercise, you briefly saw how to debug your application with Visual Studio when it executes locally in the compute emulator. To debug the application once you deploy it to the cloud, you need to write debugging information to the logs in order to diagnose an application failure.

In this task, you add a TraceListener to the project capable of logging diagnostics data directly into table storage, where you can easily retrieve it with a simple query. The source code for this project is already provided for you in the **Assets** folder of the lab. More information on the Trace Listener can be found here: [http://msdn.microsoft.com/en-us/library/system.diagnostics.tracelistener.aspx](http://msdn.microsoft.com/en-us/library/system.diagnostics.tracelistener.aspx)

1. If not already open, open Visual Studio in elevated administrator mode by right clicking the **Microsoft Visual Studio Express 2012 for Web** shortcut and choosing **Run as administrator**.

1. In the **File** menu, choose **Open Project**, browse to **Ex2-AddingDiagnosticTrace** in the **Source** folder of the lab, select **Begin.sln** in the **Begin** folder and then click **Open**.

	> **Note:** Alternatively you can continue working with the solution obtained after completing the previous exercise.

1. In **Solution Explorer**, right-click the **Begin** solution, point to **Add** and then select **Existing Project**. In the **Add Existing Project** dialog, browse to **Assets** in the **Source** folder of the lab, then navigate to **AzureDiagnostics** inside this folder, select the **AzureDiagnostics** project file and click **Open**.

1. Add a reference to the **AzureDiagnostics** library in the web role project. To do this, in **Solution Explorer**, right-click the **FabrikamInsurance** project and select **Add Reference**. In the **Add Reference** dialog, switch to the **Projects** tab located under the **Solution** panel, select **AzureDiagnostics** in the list of projects, and then click **OK**.

1. Open **Global.asax.cs** in the **FabrikamInsurance** project and insert the following namespace directives.

	````C#
	using Microsoft.WindowsAzure;
	using Microsoft.WindowsAzure.ServiceRuntime;
	````

1. Add the following (highlighted) method inside the **MvcApplication** class.

	(Code Snippet - WindowsAzureDebugging-Ex2-ConfigureTraceListener-CS)
	<!-- mark:3-25 -->
	````C#
	public class MvcApplication : System.Web.HttpApplication
	{
	  private static void ConfigureTraceListener()
	  {
	    bool enableTraceListener = false;
	    string enableTraceListenerSetting = RoleEnvironment.GetConfigurationSettingValue("EnableTableStorageTraceListener");
	    if (bool.TryParse(enableTraceListenerSetting, out enableTraceListener))
	    {
	      if (enableTraceListener)
	      {
	        AzureDiagnostics.TableStorageTraceListener listener =
	          new AzureDiagnostics.TableStorageTraceListener("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString")
	          {
	            Name = "TableStorageTraceListener"
	          };
	
	        System.Diagnostics.Trace.Listeners.Add(listener);
	        System.Diagnostics.Trace.AutoFlush = true;
	      }
	      else
	      {
	        System.Diagnostics.Trace.Listeners.Remove("TableStorageTraceListener");
	      }
	    }
	  }
	
	  ...
	}
	````

	>**Note:** The **ConfigureTraceListener** method retrieves the _EnableTableStorageTraceListener_ configuration setting and, if its value is _true_, it creates a new instance of the **TableStorageTraceListener** class, defined in the project that you added to the solution earlier, and then adds it to the collection of available trace listeners. Note that the method also enables the **AutoFlush** property of the **Trace** object to ensure that trace messages are written immediately to table storage, allowing you to retrieve them as they occur.

1. Now, insert the following (highlighted) code in the **Application_Start** method to set up the Windows Azure storage configuration settings publisher and to enable the **TableStorageTraceListener**. 

	(Code Snippet - WindowsAzureDebugging-Ex2- Application_Start-CS)
	<!-- mark:7-12 -->
	````C#
	public class MvcApplication : System.Web.HttpApplication
	{
	  ...
	
	  protected void Application_Start()
	  {
	    CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
	      {
	        configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
	      });
	
	    ConfigureTraceListener();
	
	    AreaRegistration.RegisterAllAreas();
	
	    WebApiConfig.Register(GlobalConfiguration.Configuration);
	    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
	    RouteConfig.RegisterRoutes(RouteTable.Routes);
	    BundleConfig.RegisterBundles(BundleTable.Bundles);
	  }
	}
	````

	>**Note:** TraceListeners can be added by configuring them in the **system.diagnostics** section of the configuration file. However, in this case, the role creates the listener programmatically allowing you to enable the listener only when you need it and while the service is running.

 	>![Enabling the TableStorageTraceListener in the configuration file](Images/enabling-the-tablestoragetracelistener-in-the-configuration-file.png?raw=true "Enabling the TableStorageTraceListener in the configuration file")

1. Next, define a configuration setting to control the diagnostics logging with the **TableStorageTraceListener**. To create the setting, expand the Roles node in the **FabrikamInsuranceService** project and then double-click the **FabrikamInsurance** role. In the role properties window, switch to the **Settings** page, click **Add Setting**, and then set the name of the new setting to _EnableTableStorageTraceListener_, the type as _String_, and the value as _false_.

 	![Creating a configuration setting to enable the trace listener](Images/creating-a-configuration-setting-to-enable-the-trace-listener.png?raw=true "Creating a configuration setting to enable the trace listener")

	_Creating a configuration setting to enable the trace listener_

1. Locate the **RoleEnvironmentChanging** event handler inside the **WebRole** class and replace its body with the following (highlighted) code.

	(Code Snippet - WindowsAzureDebugging-Ex2-WebRole RoleEnvironmentChanging-CS)
	<!-- mark:7-12 -->
	````C#
	public class WebRole : RoleEntryPoint
	{
	  ...
	
	  private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
	  {
	    // for any configuration setting change except EnableTableStorageTraceListener
	    if (e.Changes.OfType<RoleEnvironmentConfigurationSettingChange>().Any(change => change.ConfigurationSettingName != "EnableTableStorageTraceListener"))
	    {
	      // Set e.Cancel to true to restart this role instance
	      e.Cancel = true;
	    }
	  }
	}
	````

	>**Note:** The **RoleEnvironmentChanging** event occurs before a change to the service configuration is applied to the running instances of the role. The updated handler scans the collection of changes and restarts the role instance for any configuration setting change, unless the change only involves the value of the _EnableTableStorageTraceListener_ setting.  If this particular setting changes, the role instance is allowed to apply the change without restarting it.

1. Now, add the following (highlighted) code to define a handler for the **RoleEnvironmentChanged** event into the **Global.asax.cs**.

	(Code Snippet - WindowsAzureDebugging-Ex2-Global RoleEnvironmentChanged event handler-CS)
	<!-- mark:5-12 -->
	````C#
	public class MvcApplication : System.Web.HttpApplication
	{
	  ...
	
	  private void RoleEnvironmentChanged(object sender, RoleEnvironmentChangedEventArgs e)
	  {
	    // configure trace listener for any changes to EnableTableStorageTraceListener 
	    if (e.Changes.OfType<RoleEnvironmentConfigurationSettingChange>().Any(change => change.ConfigurationSettingName == "EnableTableStorageTraceListener"))
	    {
	      ConfigureTraceListener();
	    }
	  }
	
	  ...
	}
	````

	>**Note:** The **RoleEnvironmentChanged** event handler occurs after a change to the service configuration has been applied to the running instances of the role. If this change involves the _EnableTableStorageTraceListener_ configuration setting, the handler calls the **ConfigureTraceListener** method to enable or disable the trace listener.

1. Finally, insert the following (highlighted) line into the **Application_Start** method, immediately after to the call to the **ConfigureTraceListener** method, to subscribe to the **Changed** event of the **RoleEnvironment**.

	<!-- mark:14 -->
	````C#
	public class MvcApplication : System.Web.HttpApplication
	{
	  ...
	
	  protected void Application_Start()
	  {
	    CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
	      {
	        configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
	      });
	
	    ConfigureTraceListener();			
	
	    RoleEnvironment.Changed += this.RoleEnvironmentChanged;
	
	    AreaRegistration.RegisterAllAreas();
	
	    WebApiConfig.Register(GlobalConfiguration.Configuration);
	    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
	    RouteConfig.RegisterRoutes(RouteTable.Routes);
	    BundleConfig.RegisterBundles(BundleTable.Bundles);
	  }
	}
	````

1. To instrument the application and write diagnostics information to the error log, add a global error handler to the application. To do this, insert the following method into the **MVCApplication** class.

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

	>Note that the **Trace** object outputs the message to each listener in its **Listeners** collection, including the **TableStorageTraceListener**, provided you enable it in the configuration settings. Typically, the collection also contains instances of the **DefaultTraceListener** class and, when executing the solution in the compute emulator, the **DevelopmentFabricTraceListener**.  The latter writes its output to a log that you can view from the Compute Emulator UI. 

	>To write to the Windows Azure diagnostics log, a **DiagnosticMonitorTraceListener** can also be added to the **Web.config** or **App.config** file of the role. When using this type of trace listener, the logs are gathered locally in each role. To retrieve them, you first need to instruct the diagnostic monitor to copy the information to storage services. The role project templates included with the Windows Azure Tools for Microsoft Visual Studio already include the settings required to use the **DiagnosticMonitorTraceListener** in the configuration files it generates.

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

<a name="Ex2Task2"></a>
#### Task 2 - Creating a Log Viewer Tool ####

At this point, the application is ready for tracing and can send all its diagnostics output to a table in storage services. To view the trace logs, you now create a simple log viewer application that will periodically query the table and retrieve all entries added since it was last queried.

1. Add the LogViewer console application project to the solution. To do this, in **Solution Explorer**, right-click the **Begin** solution, point to **Add** and then select **Existing Project**. In the **Add Existing Project** dialog, browse to **Assets** in the **Source** folder of the lab, then navigate to **LogViewer** inside this folder, select the **LogViewer** project file and click **Open**.

1. Add references to the assemblies required by this project. To do this, in **Solution Explorer**, right-click the **LogViewer** project and select **Add Reference**. In the **Add Reference** dialog, switch to the **Framework** tab under the **Assemblies** panel and select **System.Configuration**, and **System.Data.Services.Client**. Next, switch to the **Extensions** tab and select **Microsoft.WindowsAzure.StorageClient**. Finally, click **OK**.

1. Next, add a reference to the diagnostics project in the solution. Repeat the previous step to open the **Add Reference** dialog, only this time select the **Solution** | **Projects** tab, select the **AzureDiagnostics** project and click **OK**.

1. Add a class to display a simple progress indicator in the console window to the project. To do this, in **Solution Explorer**, right-click **LogViewer**, point to **Add**, and select **Existing Item**. In the **Add Existing Item** dialog, browse to **Assets** in the **Source** folder of the lab, select the **ProgressIndicator.cs** file, and then click **Add**.

1. In **Solution Explorer**, double-click **Program.cs** to open this file and replace its namespace declarations with the following ones.

	(Code Snippet - WindowsAzureDebugging-Ex2-LogViewer namespaces-CS)
	<!-- mark:1-8 -->
	````C#
    using System;
    using System.Configuration;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Threading;
    using AzureDiagnostics;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;
	````

1. Define the following (highlighted) members in the **Program** class.

	(Code Snippet - _WindowsAzureDebugging-Ex2-LogViewer static members-CS_)
	<!-- mark:3-4 -->
	````C#
	public class Program
	{
	  private static string lastPartitionKey = string.Empty;
	  private static string lastRowKey = string.Empty;
	
	  static void Main(string[] args)
	  {
	  }
	}
	````

1. Next, insert the **QueryLogTable** method into the class.

	(Code Snippet - WindowsAzureDebugging-Ex2-QueryLogTable method-CS)
	<!-- mark:4-19 -->
	````C#
	class Program
	{
	  ...
	
	  private static void QueryLogTable(CloudTableClient tableStorage)
	  {
	    TableServiceContext context = tableStorage.GetDataServiceContext();
	    DataServiceQuery query = context.CreateQuery<LogEntry>(TableStorageTraceListener.DIAGNOSTICS_TABLE)
	        .Where(entry => entry.PartitionKey.CompareTo(lastPartitionKey) > 0
	        || (entry.PartitionKey == lastPartitionKey && entry.RowKey.CompareTo(lastRowKey) > 0))
	        as DataServiceQuery;
	
	    foreach (AzureDiagnostics.LogEntry entry in query.Execute())
	    {
	      Console.WriteLine("{0} - {1}", entry.Timestamp, entry.Message);
	      lastPartitionKey = entry.PartitionKey;
	      lastRowKey = entry.RowKey;
	    }
	  }
	
	  ...
	}
	````

	>**Note:** The rows in the diagnostic log table are stored with a primary key composed by the partition and row key properties, where both are based on the event tick count of the corresponding log entry and are thus ordered chronologically. The **QueryLogTable** method queries the table to retrieve all rows whose primary key value is greater than the last value obtained during the previous invocation of this method. This ensures that each time it is called, the method only retrieves new entries added to the log.

1. Finally, to complete the changes, insert the following (highlighted) code into the body of method **Main**.

	(Code Snippet - WindowsAzureDebugging-Ex2-LogViewer Main method-CS)
	<!-- mark:7-25 -->
	````C#
	class Program
	{
	  ...
	
	  static void Main(string[] args)
	  {
	    string connectionString = (args.Length == 0) ? "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" : args[0];

	    CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings[connectionString]);
	    CloudTableClient tableStorage = account.CreateCloudTableClient();
	    tableStorage.CreateTableIfNotExist(TableStorageTraceListener.DIAGNOSTICS_TABLE);

	    Utils.ProgressIndicator progress = new Utils.ProgressIndicator();
	    Timer timer = new Timer(
	      (state) =>
	      {
	        progress.Disable();
	        QueryLogTable(tableStorage);
	        progress.Enable();
	      },
	      null,
	      0,
	      10000);
	
	    Console.ReadLine();
	  }
	
	  ...
	}
	````

	>**Note:** The inserted code initializes the Windows Azure storage account information, creates the diagnostics table if necessary, and then starts a timer that periodically calls the **QueryLogMethod** defined in the previous step to display new entries in the diagnostics log.

1. To complete the viewer application, open the **App.config** file in the **LogViewer** project and insert the following (highlighted) **appSettings** section to define the _DiagnosticsConnectionString_ setting required to initialize the storage account information.

	<!-- mark:2-4 -->
	````XML
	<configuration>
	  <appSettings>
	    <add key="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" value="UseDevelopmentStorage=true"/>
	  </appSettings>
	  <startup>
	  ...
	````

<a name="Ex2Verification"></a>
#### Verification ####

You are now ready to execute the solution in the compute emulator. To enable the Table Storage trace listener dynamically without stopping the running service, you initially deploy the service with the _EnableTraceStorageTraceListener_ setting disabled and then, you change the setting in the configuration file to enable the listener and then upload it to re-configure the running service. Using the log viewer application, you examine the trace messages produced by the application.

1. Open the **Web.config** file of the **FabrikamInsurance** project and insert the following (highlighted) **customErrors** section as a direct child of the **system.web** element. 

	<!-- mark:5 -->
	````XML
	<configuration>
	  ...
	  <system.web>
	    ...
	    <customErrors mode="On" />
	  </system.web>
	  ...
	<configuration>
	````

	>**Note:** When you set the **customErrors** mode to _On_, ASP.NET displays generic error messages for both local and remote clients. With **customErrors** set to its default setting of _RemoteOnly_, once the application is deployed to Windows Azure and you access it remotely, you will also see the generic errors, so this step is not strictly necessary. However, it allows you to reproduce locally the behavior that you would observe once you deploy the application to the cloud.

1. To test the solution, you need to configure the Windows Azure Project and the log viewer application so that they both start simultaneously. To define the startup projects, right-click the solution node in **Solution Explorer** and select **Set StartUp Projects**. In the **Solution 'Begin' Property Pages** window, make sure to select **Startup Project** under **Common Properties**, and then select the option labeled **Multiple startup projects**. Next, set the **Action** for both the **LogViewer** and **FabrikamInsuranceService** projects to _Start_, leaving the remaining projects as _None_. Click **OK** to save the changes to the start-up configuration.

 	![Configuring the start-up projects for the solution](Images/configuring-the-start-up-projects-for-the-solution.png?raw=true "Configuring the start-up projects for the solution")

	_Configuring the start-up projects for the solution_

1. Press **CTRL + F5** to launch the application without attaching a debugger. Again, this reproduces the conditions that you would have once you deploy the application to the cloud. Wait until the deployment completes and the browser opens to show its main page.

1. In the browser window, complete the form making sure that you choose "_PORSCHE"_ for the **Make** of the vehicle and "_BOXSTER (BAD DATA)_" for the **Model**. Notice that this time, because you enabled the **customErrors** setting in the **Web.config** file, the application shows a generic error page instead of the exception details that you saw earlier. This is what you would also see had the application been deployed to Windows Azure. 

 	![Application error with customErrors enabled](Images/application-error-with-customerrors-enabled.png?raw=true "Application error with customErrors enabled")

	_Application error with customErrors enabled_

1. Examine the output from the log viewer application. Notice that, despite the error, the console window is still empty because the table storage trace listener is currently disabled.

1. Switch back to Visual Studio and, in **Solution Explorer**, expand the **Roles** node of the **FabrikamInsuranceService** project, and then double-click the **FabrikamInsurance** role to open its properties window. Select the **Settings** page, and then change the value of the _EnableTableStorageTraceListener_ setting to _true_. 

1. Press **CTRL + S** to save the changes to the configuration.

1. Open the compute emulator console by right-clicking its icon located in the system tray and selecting **Show Compute Emulator UI**. Record the ID for the current deployment. This is the numeric value shown enclosed in parenthesis, next to the deployment label.

 	![Compute Emulator UI showing the current deployment ID](Images/compute-emulator-ui-showing-the-current-deployment-id.png?raw=true "Compute Emulator UI showing the current deployment ID")

	_Compute Emulator UI showing the current deployment ID_

1. Now, open a **Windows Azure Command Prompt** as an administrator. To launch the command prompt as an administrator, right-click its shortcut and choose **Run as administrator**.

1. Change the current directory to the location of the **FabrikamInsuranceService** cloud project inside the current solution's folder. This folder contains the service configuration files, select ServiceConfiguration.Local.cscfg.

1. At the command prompt, execute the following command to update the configuration of the running deployment. Replace the [DEPLOYMENTID] placeholder with the value that you recorded earlier.

	````WindowsAzureCommandPrompt
	csrun /update:[DEPLOYMENTID];ServiceConfiguration.Local.cscfg
	````

 	![Updating the configuration of the running service](Images/updating-the-configuration-of-the-running-service.png?raw=true "Updating the configuration of the running service")

	_Updating the configuration of the running service_

	>**Note:** For applications deployed to the cloud, you would normally update the configuration of your running application through the Windows Azure Developer Portal or by using the Windows Azure Management API to upload a new configuration file.

1. Once you have updated the configuration and enabled the trace listener, return to the browser window, browse to the **Quotes** page, and re-enter the same parameters that caused the error previously (make _"PORSCHE"_, model _"BOXSTER (BAD DATA)"_). Then, click **Calculate** to submit the form again. The response should still show the error page.

1. Switch to the log viewer window and wait a few seconds until it refreshes.  Notice that the console now shows an entry with the error message for the unhandled exception, showing that the trace output generated by the running application is written directly to table storage.

 	![Viewer showing the error logged to table storage](Images/viewer-showing-the-error-logged-to-table-storage.png?raw=true "Viewer showing the error logged to table storage")

	_Viewer showing the error logged to table storage_

1. To view the output from other informational trace messages, return to the browser window and click **About** followed by **Quotes** to execute both actions in the controller. Recall that you inserted trace messages at the start of each method. Notice that the viewer console now displays a message for each of these actions.

	![Viewer showing informational trace messages for the controller actions](Images/viewer-showing-informational-trace-messages-for-the-controller-actions.png?raw=true)

	_Viewer showing informational trace messages for the controller actions_

1. In the log viewer window, press **Enter** key twice to exit the program.

1. Finally, delete the running deployment in the compute emulator. To do this, right-click the deployment in the **Service Deployments** tree view and select **Remove**.

<a name="Exercise3"></a>
### Exercise 3: Using IntelliTrace to Diagnose Role Start-Up Failures ###

> **Note:** This exercise is optional, since IntelliTrace is only available for **Visual Studio 2012 Ultimate Edition**. For more information on specific Visual Studio 2012 features, compare versions [here](http://www.microsoft.com/visualstudio/eng/products/compare).

IntelliTrace provides the ability to collect data about an application while it is executing. When you enable IntelliTrace, it records key code execution and environment data and then allows you to replay this data from within Visual Studio, stepping through the same code that executes in the cloud.

When you deploy a service to Windows Azure from within Visual Studio, you can enable IntelliTrace debugging to package the necessary IntelliTrace files along with an agent that Visual Studio will communicate with to retrieve the IntelliTrace data. Once enabled, IntelliTrace operates in the background, collecting information about the running service.

You can customize the basic IntelliTrace configuration specifying, which events to log, whether to collect call information, which modules and processes to collect logs for, and how much space to allocate to the recording (the default size is 250 MB).

The collected information is saved to an IntelliTrace file, which you can open later to start troubleshooting the problem. This information lets you step back in time to see what happened in the application and which events led to a crash.

In this exercise, you explore the use of IntelliTrace to diagnose a role start-up failure while deploying the Fabrikam Insurance application to Windows Azure.

> **Important:** IntelliTrace debugging is intended for debug scenarios only, and should not be used for a production deployment. 

<a name="Ex3Task1" />
#### Task 1 - Creating a Storage Account and a Cloud Service ####

The application you deploy in this exercise requires a Cloud Service and a Storage Account. In this task, you create a new storage account to allow the application to persist its data. In addition, you define a Cloud Service to host your web application.

1. Navigate to [http://manage.windowsazure.com](http://manage.windowsazure.com) using a Web browser and sign in using the Microsoft Account associated with your Windows Azure account.

	![Signing in to the Windows Azure Management portal](Images/signing-in-to-the-windows-azure-platform-mana.png?raw=true)

	_Signing in to the Windows Azure Management portal_

1. Create the **Storage Account** that the application will use to store its data. In the Windows Azure Management Portal, click **New** | **Data Services** | **Storage** | **Quick Create**.

1. Set a unique **URL**  (e.g. _fabrikaminsurancestorage_), select a region or an affinity group and click **Create Storage Account** to continue.

	![Creating a new storage account](Images/creating-a-new-storage-account.png?raw=true)

	_Creating a new storage account_

1. Wait until the Storage Account is created. Click your storage account name to go to its **Dashboard**.

	![Storage Accounts page](Images/storage-accounts-page.png?raw=true "Storage Accounts page")

	_Storage Accounts page_

1. Click **Manage Keys** at the bottom of the page in order to show the storage account's access keys.

	![Manage Storage Account Keys](Images/manage-storage-account-keys.png?raw=true "Manage Storage Account Keys")

	_Manage Storage Account Keys_

1. Copy the **Storage account name**, and the **Primary access key** values. You will use these values later on to configure the application.

	![Retrieving the storage access keys](Images/retrieving-the-storage-access-keys.png?raw=true)

	_Retrieving the storage access keys_

1. Next, create the **Cloud Service** that executes the application code. Click **New** | **Cloud Service** | **Quick Create**.

1. Select a **URL** for your Cloud Service (e.g. _fabrikaminsuranceservice_), the region or affinity group you selected for the storage and click **Create Cloud Service** to continue.

	![Creating a new Cloud Service](Images/creating-a-new-cloud-service.png?raw=true "Creating a new Cloud Service")

	_Creating a new Cloud Service_

1. Wait until your Cloud Service is created to continue. Do not close the browser window, you will use the portal for the next task.

	![Cloud Service Created](Images/cloud-service-created.png?raw=true "Cloud Service Created")

	_Cloud Service Created_

<a name="Ex3Task2"></a>
#### Task 2 - Preparing the Application for Deployment to the Cloud ####

When you publish your service using Visual Studio, the Windows Azure Tools upload the service package and then automatically start it. You will not have a chance to update the configuration settings before the service starts. Therefore, you must configure all the necessary settings before you publish the service.

In this task, you update the storage connection strings to point to your Storage Account.

1. If it is not already open, launch **Microsoft Visual Studio Ultimate 2012** as Administrator.

1. In the **File** menu, choose **Open Project** and browse to **Ex3-DebuggingWithIntelliTrace\Begin** in the **Source** folder of the lab. Select **Begin.sln** and click Open.

1. Configure the storage account connection strings. To do this, expand the **Roles** node in the **FabrikamInsuranceService** cloud project, double-click the **FabrikamInsurance** web role and switch to the **Settings** tab.

1. Now select the _DataConnectionString_ setting, ensure that the **Type** is set to Connection String and replace the placeholders with your storage **Account Name** and **Account Key** values saved during the task 1 of this exercise.

	![Configuring the storage account name and account key](Images/defining-connection-settings.png?raw=true "Configuring the storage account name and account key")

	_Configuring the storage account name and account key_

1. Repeat the previous step to configure the _Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString_ setting using the same account information.

1. Press **CTRL + S** to save your changes. 

<a name="Ex3Task3" />
#### Task 3 - Deploying the Application with IntelliTrace Enabled ####

In this task, you will log on to the Windows Azure Portal and download the publish-settings file. This file contains the secure credentials and additional information about your Windows Azure Subscription to use in your development environment.

Then, you will use these credentials to publish the FabrikamInsurance application directly from Visual Studio, with the IntelliTrace feature enabled.

1. Open an Internet Explorer browser and go to <https://windows.azure.com/download/publishprofile.aspx>.

1. Sign in using the Microsoft account associated with your Windows Azure account.

1. **Save** the publish-settings file to your local machine.

	![Downloading publish-settings file](Images/downloading-publish-settings-file.png?raw=true 'Downloading publish-settings file')

	_Downloading publish-settings file_

1. Switch to Visual Studio and in the **Solution Explorer**, right-click the **FabrikamInsuranceService** cloud project and select **Publish**.

1. In the **Publish Windows Azure Application** dialog, click **Import**, browse to the PublishSettings file you dowloaded, select it and click **Open**.

1. Back in the **Publish Windows Azure Application** dialog, select the subscription created from the _PublishSettings_ file and click **Next**.
 
	![Signing In](Images/waz-sign-in.png?raw=true "Signing In")

	_Signing In_

1. In the **Common Settings** tab, notice that the dialog populates the drop down list labeled **Cloud Service** with the information for all the services configured in your Windows Azure account. Select the cloud service that you created during the task 1 of this exercise.
 
	![Deployment Common Settings](Images/deployment-common-settings.png?raw=true "Deployment Common Settings")
	
	_Deployment Common Settings_

1. Click **Advanced Settings** tab. In the list labeled **Storage account** select the storage service that you created during the task 1 of this exercise. Check the check box labeled **Enable IntelliTrace** and click **Next**.
 
	![Deployment Advanced Settings](Images/deployment-advanced-settings.png?raw=true "Deployment Advanced Settings")
	
	_Deployment Advanced Settings_

1. Review the Summary information. If everything is OK, click **Publish** to start the deployment process.

	![Starting Deployment](Images/start-deployment.png?raw=true "Starting Deployment")

	_Starting Deployment_

1. After you start a deployment, you can examine the Windows Azure activity log window to determine the status of the operation. If this window is not visible, in the **View** menu, point to **Other Windows**, and then select **Windows Azure Activity Log**.

	![Windows Azure Activity Log](Images/azure-activity-log.png?raw=true "Windows Azure Activity Log")

	_Windows Azure Activity Log_

1. You can examine the **History** panel on the right of the **Windows Azure Activity Log** window to determine the status of the deployment. Notice that the status of the role instances are shown as **stopped**.

	![Deployment Operation Details](Images/deployment-operation-details.png?raw=true "Deployment Operation Details")

	_Viewing detailed information about a deployment operation_

<a name="Ex3Task4" />
#### Task 4 - Examining the IntelliTrace Logs to Determine the Cause of a Failure ####

In the previous task, the role failed to start due to an unknown reason. In this task, you will use IntelliTrace to determine what caused the failure.

1. In the **Windows Azure Activity Log** window, click **Open in Server Explorer**.

	![Viewing the cloud service in Server Explorer](Images/cloud-service-in-server-explorer.png?raw=true "Viewing the cloud service in Server Explorer")

	_Viewing the cloud service in Server Explorer_

	> **Note:** If this window is not currently visible, in the **View** menu, point to **Other Windows**, and then select **Windows Azure Activity Log**.

1. In the **Windows Azure Compute** node, examine the role instance status of the hosted service where you deployed the **FabrikamInsurance** application. Notice that the label for the deployment slot indicates that IntelliTrace is enabled for this deployment and that the status is shown as “**Busy**” here too.

	![Fabrikam Insurance Service Instance](Images/service-instance-busy.png?raw=true "Fabrikam Insurance Service Instance")

	_Fabrikam Insurance Service Instance_

1. Now, right-click the slot node, labeled as **Instance 0 (Busy)** in **Server Explorer**, and then select **View IntelliTrace Logs** to download the information to your workstation. After you do this, notice that Visual Studio creates a new IntelliTrace entry in the **Windows Azure Activity Log** window to display the progress of the download operation.

1. Wait for the download to complete, which may take several minutes. While this is happening, you can expand the corresponding entry in the **Windows Azure Activity Log** window and examine the **History** panel on the right to monitor the progress of the operation.

	![IntelliTrace operation history log](Images/intellitrace-operation-history-log.png?raw=true "IntelliTrace operation history log")

	_IntelliTrace operation history log_

	> **Note:** When you request the IntelliTrace logs, a snapshot of the information collected so far is uploaded from the VM instance where the role is running to your storage account and then downloaded to a disk file on your local computer. Once the log is transferred successfully, it is deleted from the storage.

1. After the download completes, Visual Studio automatically opens the IntelliTrace log file and displays it in a window. The **IntelliTrace Summary** window is divided into several sections that contain diagnostics information collected for the deployment.

	![IntelliTrace summary window](Images/intellitrace-summary-window.png?raw=true "IntelliTrace summary window")

	_IntelliTrace summary window_

	The sections available in the IntelliTrace summary window are:

	**Exception Data** - lists unhandled exceptions that occur during the data collection period with details about the exception type, the exception message, the thread where the exception was raised, an HResult, if applicable, and the time of the exception. Selecting an entry in the list displays the corresponding call stack for the exception.

	If an exception occurs in your code, you can select the exception in the list and then click **Start Debugging** to open the appropriate source file in Visual Studio with the cursor placed on the line of code that raised the exception.

	![Exception Data section](Images/exception-data-section.png?raw=true "Exception Data section")

	_Exception Data section_

	**Threads List** - shows every thread active during the diagnostics collection period, with each thread showing its ID, a name, if available, and the starting and ending times of the thread.

	![Threads List section](Images/threads-list-section.png?raw=true "Threads List section")

	_Threads List section_

	**System Info** - shows information about the virtual machine environment where the diagnostics data was collected, including the computer name, operating system and CLR versions, physical memory and virtual memory available, number of processors, time zone, among other details.

	![System Info section](Images/system-info-section.png?raw=true "System Info section")

	_System Info section_

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

	Notice that the list includes a **System.TypeLoadException** with the message "_Unable to load the role entry point due to the following exceptions: -- System.IO.FileNotFoundException: Could not load file or assembly 'System.Web.Mvc, ..._". This exception indicates that the role was unable to locate the **System.Web.Mvc** assembly required by the application and is, in fact, the reason why the role could not start successfully earlier. This type of error is common and very difficult to diagnose in the cloud, especially when you cannot reproduce it locally because all the required dependencies are already present in your development environment.

	In the following task, you will fix the problem and re-deploy the service package. 

	> **Note:** In general, you need to set **Copy Local** = _True_ for any assembly that is not installed by default in the Windows Azure VMs to ensure that it is deployed with your application. 

1. Cancel and remove the failing deployment to Windows Azure. In the Windows Azure activity log window, right-click the deployment and select **Cancel and remove**.

	![Cancel the failing deployment](Images/cancel-failing-deployment.png?raw=true "Cancel the failing deployment")

	_Cancel the failing deployment_

<a name="Ex3Task5" />
#### Task 5 - Fixing the Application and Re-Deploying (Optional) ####

Now that you identified the cause of the role start up failure as a missing assembly, you can correct the problem and re-deploy the application.

In this task, you add the missing assembly to the service package and re-deploy it to Windows Azure.

1. Ensure that the **System.Web.Mvc** assembly is included in the service package that you deploy to Windows Azure. To do this, expand the **References** node for the **FabrikamInsurance** project in **Solution Explorer**, right-click the **System.Web.Mvc** assembly and select **Properties**. Locate the **Copy Local** setting and change its value to _True_.

	![Including an assembly in the service package deployed to Windows Azure](Images/including-mvc-assembly.png?raw=true "Including an assembly in the service package deployed to Windows Azure")

	_Including an assembly in the service package deployed to Windows Azure_

1. In **Solution Explorer**, right-click the **FabrikamInsuranceService** cloud project and select **Publish**.
In the **Publish Windows Azure Application** dialog, leave the IntelliTrace option enabled and then click **Publish** to start the deployment process one more time. You may wish to change the **Deployment label** to identify the deployment as the one that contains the bug fix.

	![Deploying the updated application](Images/deploying-updated-application.png?raw=true "Deploying the updated application")

	_Deploying the updated application_

	> **Note:** Because the slot that you chose is already occupied by the previous deployment, Visual Studio warns you and asks for confirmation before replacing it. Click **Replace** to overwrite the current deployment.

1. Wait for the deployment operation to complete, which may take several minutes.

	![Deployment operation completed](Images/deployment-operation-completed.png?raw=true "Deployment operation completed")

	_Deployment operation completed_

1. Once the deployment is ready, in the **Windows Azure Activity Log** window, click the **Website URL** link for the deployment operation to open the application in your browser. Test the application and ensure that it is working properly.

1. You may download the IntelliTrace log for the running application and compare it with the log that you obtained earlier, when the application failed to start. This will increase your ability to recognize which exceptions are normal.

<a name="Summary"></a>
## Summary ##

By completing this hands-on lab, you learnt how to apply simple debugging techniques to troubleshoot your Windows Azure application once you deploy it to the cloud. You saw how to use standard .NET diagnostics to write diagnostics output directly into table storage with a custom trace listener.

Using IntelliTrace, you quickly diagnosed a role that failed to start due to a missing dependency; an error that would otherwise have been very difficult to diagnose in the cloud.