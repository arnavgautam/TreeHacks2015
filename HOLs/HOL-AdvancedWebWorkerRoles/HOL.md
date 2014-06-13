<a name="HOLTop"></a>
# Advanced Web and Worker Roles #

---

<a name="Overview"></a>
## Overview ##
	
When Microsoft Azure was first released, there were a few, but significant restrictions in the programming model. Things like Full Trust, Administrative Access, and the full IIS feature-set were initially restricted for security reasons. This impacted the types of applications that could be created in Microsoft Azure because even small changes to things like configuration settings were often blocked by lack of administrative control over the VM Instances. Over time, those restrictions were lifted - first Full Trust, and now the ultimate control: Administrative access and Full IIS support.

Now, you can choose to run your Websites under IIS7, not in Hosted Web Core as in the past, but in full IIS. This means you can use all the facilities of IIS now like custom modules, multiple websites, VDIR support, application pool isolation, and more.

Additionally, you can now choose two different ways to exercise your administrative control. You can bootstrap the machine as an administrator using something called “Startup Tasks” shown in this lab. This temporarily raises your permissions to administrative and allows you perform small setups, update configuration settings, or other bootstrapping tasks. Once completed, your code will run as a normal, unprivileged user. The second method is that you can now configure your role to simply run as an administrator the entire time. In most cases, the Startup Tasks are the right choice as running your role with administrative permissions the entire time has security implications.

This lab introduces these new capabilities that are unlocked in Microsoft Azure and allow more advanced application scenarios.

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Use advanced service model features that enable hosting a Web role in IIS.

- Host multiple sites in a Web role and bind them to the same endpoint using host headers.
- Create virtual applications and map them to selected sites.
- Utilize user virtual directories to share common content between sites.
- Set up the role environment by executing start-up tasks to register a COM.
- Install complex components required by a web role, such as a scripting language's binary files.
	

>**Note:** This lab shows advanced features of Web and Worker roles in Microsoft Azure; it assumes that you have sufficient knowledge of Microsoft Azure. If you are beginner in Microsoft Azure, see the **Introduction to Microsoft Azure** lab first.

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:


- IIS 7 (with ASP.NET, WCF HTTP Activation)

- [Microsoft .NET Framework 4.0][1]

- [Microsoft Visual Studio 2010][2]

	- Microsoft Visual C++ 2010 (required for Exercise 2 of the lab)

- [Microsoft Azure Tools for Microsoft Visual Studio 1.7][3]

- [SQL Server 2012 Express Edition][4]

- A Microsoft Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)


[1]:http://go.microsoft.com/fwlink/?linkid=186916
[2]:http://msdn.microsoft.com/vstudio/products
[3]:http://www.microsoft.com/windowsazure/sdk/
[4]:http://www.microsoft.com/express/sql/download/


>**Note:** This lab was designed to use Windows 7 Operating System. 

<a name="Setup"></a>
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab’s **Source** folder.

1. Execute the **Setup.cmd** file in this folder with Administration privileges. This setup process will configure your environment and install the Visual Studio code snippets for this lab.

1. If the User Account Control dialog is shown, confirm the action to proceed.

	>**Note:** Make sure you have checked all the dependencies for this lab before running the setup.

<a name="CodeSnippets"></a>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2010 to avoid having to add it manually.

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

- [Registering Sites, Applications, and Virtual Directories](#Exercise1)

- [Using Start-Up Tasks to Register a COM Component](#Exercise2)

- [Using Start-Up Tasks to Install PHP with the Web Platform Installer](#Exercise3)

	
Estimated time to complete this lab: **60 minutes**.

>**Note:** Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise. Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.
>
> When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

	
<a name="Exercise1"></a>
### Exercise 1: Registering Sites, Applications, and Virtual Directories ###

When hosted in [IIS Hosted Web Core (HWC)] (http://technet.microsoft.com/en-us/library/cc735238\(WS.10\).aspx), Microsoft Azure Web roles can support a single application bound to no more than a single HTTP and a single HTTPS endpoint. This model is enabled with minimal configuration and was the only one originally supported in Microsoft Azure when it was first introduced. To use HWC, you only need to specify the HTTP and HTTPS endpoints bound to the Web application in the service model, as shown in the figure below.

![XML Code simplified service model for Hosted Web Core hosting](images/simplified-service-model-for-hosted-web-core-hosting.png?raw=true "Simplified service model for Hosted Web Core hosting")
	

_Simplified service model for Hosted Web Core hosting_ 


While this approach is still valid and you can use it in many scenarios where a single application per role is sufficient or does not require multiple endpoints, more advanced capabilities are now available that provide full access to all IIS features. In this advanced model, applications are hosted in IIS instead, both in Microsoft Azure and in the compute emulator, allowing each role to support multiple sites, virtual applications and virtual directories, as well as providing support for binding each site to multiple endpoints.

To enable the Full IIS capabilities, the service model defines a **Sites** element that can contain one or more **Site** definitions, where each site is bound to one or more endpoints, as shown in the following figure.
	 
![Advanced service model for Full IIS hosting](images/advanced-service-model-for-full-iis-hosting.png?raw=true "Advanced service model for Full IIS hosting")

_Advanced service model for Full IIS hosting_ 

In this exercise, you will learn how to define different sites, applications, and virtual directories within a Web role using a single Web role application, which you will map to multiple sites. To show site customization, you will modify the master page of the application to show the site name on each page. During the exercise, you will create a Virtual Application for a hypothetical CRM application and enable it for selected sites. Additionally, you will use Virtual Directories to share common content between applications.

>**Note:** To reduce typing, you can right-click where you want to insert source code, select Insert Snippet, select My Code Snippets and then select the entry matching the current exercise step.

<a name="Ex1Task1"></a>
#### Task 1 - Defining Multiple Sites in a Web Role Using the Service Model ####

The service model in Microsoft Azure is determined by the service definition file, which defines the roles that comprise a service, optional local storage resources, configuration settings, certificates for SSL endpoints, and, as you will see in the following exercise, start-up tasks. 

In this task, you edit the service model to define three separate sites that map to the same Web application, namely, _Contoso_, _Fabrikam_, and _Litware_. These sites are all bound to a single endpoint and use host headers to isolate them.

>**Note:** This task requires the **hosts** file to be updated. If you did not execute the setup instruction for this lab, this exercise will not work. Proceed with the setup instructions before starting with this task.
	
1. Start Microsoft Visual Studio 2010 with administrator privileges.

1. Open the **Begin** solution located in **Ex1-FullIIS** in the **Source** folder of the lab.

1. In the Microsoft Azure project, open the service model file, **ServiceDefinition.csdef**. Notice that it defines a single Web role named **SampleWebApp**, which in turn defines a site named **Web**. 

1. In the **Sites** element of the **SampleWebApp** Web role, locate the nested **Site** element named _Web_ and change its **name** attribute to _Fabrikam_. Next, add a **physicalDirectory** attribute to this element that points to "_..\SampleWebApp_".

	>**Note:** This step changes the name of the site from its _Web_ default value to _Fabrikam_. Note that for sites other than the default Web site, you need to specify a physical directory for the site’s content. 

1. Now, locate the single **Binding** element for the _Fabrikam_ site and add a **hostHeader** attribute with a value of "_www.fabrikam.com_". The updated site definition should appear as shown in the following figure.

	![Definition for the Fabrikam site](images/definition-for-the-fabrikam-site.png?raw=true "Definition for the Fabrikam site")
	 
	_Definition for the Fabrikam site_ 

	>**Note:** The updated endpoint binding ensures that the site only responds to traffic that specifies the appropriate host header, namely requests for _www.fabrikam.com_.

1. Next, define a new site for _Contoso_ by inserting the following (highlighted) definition inside the **Sites** element.
	
	(Code Snippet - _AdvancedWebAndWorkerRoles-Ex1-01-ContosoSite-XML_)
	
	<!-- mark: 8-14 -->
	````XML
	<WebRole name="SampleWebApp">
	  <Sites>
	    <Site name="Fabrikam" physicalDirectory="..\SampleWebApp">
	      <Bindings>
	        <Binding name="HttpIn" endpointName="HttpIn" hostHeader="www.fabrikam.com" />
	      </Bindings>
	    </Site>
	    <Site name="Contoso" physicalDirectory="..\SampleWebApp">
	      <Bindings>
	        <Binding name="HttpIn" 
	                 endpointName="HttpIn" 
	                 hostHeader="www.contoso.com" />
	      </Bindings>
	    </Site>
	</Sites>
	  ...
	</WebRole>
	````	

1. Similarly, create a site for **Litware**, as shown (highlighted) below.
	
	(Code Snippet - _AdvancedWebAndWorkerRoles-Ex1-02-LitwareSite-XML_)

	<!-- mark: 17-23 -->
	````XML
	<WebRole name="SampleWebApp">
	  <Sites>
	    <Site name="Fabrikam" physicalDirectory="..\SampleWebApp">
	      <Bindings>
	        <Binding name="HttpIn" 
	                 endpointName="HttpIn" 
	                 hostHeader="www.fabrikam.com" />
	      </Bindings>
	    </Site>
	    <Site name="Contoso" physicalDirectory="..\SampleWebApp">
	      <Bindings>
	        <Binding name="HttpIn" 
	                 endpointName="HttpIn" 
	                 hostHeader="www.contoso.com" />
	      </Bindings>
	    </Site>
	    <Site name="Litware" physicalDirectory="..\SampleWebApp">
	      <Bindings>
	        <Binding name="HttpIn" 
	                 endpointName="HttpIn" 
	                 hostHeader="www.litware.com" />
	      </Bindings>
	    </Site>
    </Sites>
	  ...
	</WebRole>
	````
		
	>**Note:** All three sites are mapped to the same physical directory ("_..\SampleWebApp_") and bound to the same HTTP endpoint (_HttpIn)_. The endpoint binding, however, specifies a different host header value for each site.

1. Press **F5** to build and run the Microsoft Azure project. Wait for the application to launch in the compute emulator and for the browser to open pointing at the default site address. Notice that it shows an error page with a status **HTTP 400 Bad Request**.
	
	>**Note:** By default, the Microsoft Azure Tools for Visual Studio opens the default Web site in your browser. In this case, however, you have removed the default mapping and there is no longer any site accessible without the use of a host header, which explains the error response.

1. Start Internet Information Services (IIS) Manager.

1. Browse to the **Sites** node and notice that three new sites were created for _Fabrikam_, _Contoso_ and _Litware_. The name of the site is derived from the Microsoft Azure deployment ID, the Web role name, the instance ID, and the name of the site in the service model file.
	 

	![Internet Information Services Manager showing the sites created by the Web role](images/iis-manager-showing-the-sites-created-by-the-web-role.png?raw=true "Internet Information Services \(IIS\) Manager showing the sites created by the Web role")

	_Internet Information Services (IIS) Manager showing the sites created by the Web role_ 

1. In the (IIS) Manager, click Contoso node and take note of the **port** where it is hosted. You will find the port number in the right pane, inside **Manage Web Site**. Repeat this action with Fabrikam and Litware nodes. You will use these ports later in this exercise.
	 
	![Application Port](images/application-port.png?raw=true "Application Port")

	_Application Port_ 

1. Now, select the **Application Pools** node and review the pools created to support the applications hosted by the Web role.
	 
	
	![Application pools defined for the Web role sites](images/application-pools-defined-for-the-web-role-sites.png?raw=true "Application pools defined for the Web role sites")

	_Application pools defined for the Web role sites_ 

1. In the browser window, change the address to that of the _Contoso_ site. Make sure to use the same port number you obtained in the step 11. For example, if the browser is currently showing [http://127.0.0.1:81](http://127.0.0.1:81), set the address to http://www.contoso.com:82.

	>**Note:** The setup procedure updated the hosts file, adding three new entries for [www.contoso.com](http://www.contoso.com),  [www.fabrikam.com](http://www.fabrikam.com), and [www.litware.com](http://www.litware.com), all pointing to the loopback address (127.255.0.0).
 
1. Notice that the title shown in the home page is "My ASP.NET Application".

1. In Visual Studio, open the **Site.Master** file in the **SampleWebApp** project. Inside the body of the page, locate the **h1** element nested inside a **div** element with its **class** set to _title_. Replace its content, which should be “My ASP.NET Application”, with the following (highlighted) expression.

	(Code Snippet - _AdvancedWebAndWorkerRoles-Ex1-03-SampleWebAppTitle-HTML_)

	<!-- mark: 8 -->
	````aspx
     ...
	<body>
	  <form runat="server">
	  <div class="page">
	    <div class="header">
	      <div class="title">
	        <h1>
	          <%= System.Text.RegularExpressions.Regex.Match(System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteName(), "[^_]+$").Value%>
         </h1>
	      </div>
	    <div class="loginDisplay">
	    ...
	
	    </form>
	</body>
	...
	````
	
	>**Note:** The added code retrieves the Web site name using the **ApplicationHost** class in the **System.Web.Hosting.Environment** namespace and then uses a regular expression to extract a friendly name for the site.

1. Save the updated file.

1. In the browser window, refresh the page and notice how the title of the page now shows “Contoso” instead of “My ASP.NET Application”.
	
	>**Note:** Editing the site content while the application is running is a useful technique to use during debugging, allowing you to apply changes without restarting the deployment. Be aware that this requires the role to be hosted in IIS; otherwise, when using the simple service model that uses Hosted Web Core (HWC), changes only become effective after redeploying the service.

1. Open a new browser window and navigate to [http://www.litware.com](http://www.litware.coms). Notice that the Web site title in the new window is **Litware**. Similarly, open a third browser window and navigate to [http://www.fabrikam.com](http://www.fabrikam.com). 
	
	>**Note:** Remember to specify the correct port number for each address if the deployment is not using the standard HTTP port (80).
	
	![Multiple sites hosted in the Web role](images/multiple-sites-hosted-in-the-Web-role.png?raw=true "Multiple sites hosted in the Web role")

	_Multiple sites hosted in the Web role_ 

	>**Note:** In this particular case, all three sites are mapped to the same physical directory but use host headers to identify which site responds to a request. A Web role application can extract the site name from the request, as was shown earlier for the title, and then customize the output to render the pages differently depending on the requested site. You can, of course, map each site to a different physical directory and use different content for each one.
	
1. Close all browser windows.
	
<a name="Ex1Task2"></a>
#### Task 2 - Creating Virtual Applications and Virtual Directories ####

In this task, you create a new Virtual Application for an application that you only enable for selected sites. 

1. In Visual Studio, add the sample **CRM** application project located in the **Assets** folder of the lab to the solution. 

1. Now, in the Microsoft Azure project, open the service model file **ServiceDefinition.csdef**.

1. Locate to the **Site** element for the _Contoso_ site and insert the **VirtualApplication** definition, as shown (highlighted) below. 
	
	(Code Snippet - _AdvancedWebAndWorkerRoles-Ex1-04-ContosoVirtualApplicationDefinition-XML_)

	<!-- mark: 7-13-->
	````XML
	<WebRole name="SampleWebApp">
	  <Sites>
	    <Site name="Fabrikam" physicalDirectory="..\SampleWebApp">
	      ...
	    </Site>
	    <Site name="Contoso" physicalDirectory="..\SampleWebApp">
	      <VirtualApplication name="CRM"
	                          physicalDirectory="..\..\..\Assets\CRM">
	        <VirtualDirectory name="Scripts"
	                          physicalDirectory="..\SampleWebApp\Scripts" />
	        <VirtualDirectory name="Styles"
	                          physicalDirectory="..\SampleWebApp\Styles" />
	      </VirtualApplication>
         ...
	    </Site>
	    <Site name="Litware" physicalDirectory="..\SampleWebApp">
	      ...
	    </Site>
	  </Sites>
	  ...
	</WebRole>
	````

	>**Note:** The virtual application definition shown above creates a new Web application in the _Contoso_ site and maps it to the _CRM_ path, thus, making it reachable at _[http://www.contoso.com/CRM](http://www.contoso.com/CRM)_.

	>Because the CRM site shares content with the main site, in particular, its _Scripts_ and _Styles_ folders, these are created as virtual directories under the CRM site and mapped to the corresponding physical directories present in the main site.

1. Similarly, enable the CRM application for the _Litware_ site by inserting the following (highlighted) virtual application definition.

	(Code Snippet - _AdvancedWebAndWorkerRoles-Ex1-05-LitwareVirtualApplicationDefinition-XML_)

	<!-- mark: 10-16-->
	````XML
   <WebRole name="SampleWebApp">
	  <Sites>
	    <Site name="Fabrikam" physicalDirectory="..\SampleWebApp">
	      ...
	    </Site>
	    <Site name="Contoso" physicalDirectory="..\SampleWebApp">
	      ...
	    </Site>
	    <Site name="Litware" physicalDirectory="..\SampleWebApp">
	      <VirtualApplication name="CRM"
	                          physicalDirectory="..\..\..\Assets\CRM">
	        <VirtualDirectory name="Scripts"
	                          physicalDirectory="..\SampleWebApp\Scripts" />
	        <VirtualDirectory name="Styles"
	                          physicalDirectory="..\SampleWebApp\Styles" />
	      </VirtualApplication>
       ...
	    </Site>
	  </Sites>
	  ...
	</WebRole>
	````
	
1. Press **F5** to build and launch the application in the compute emulator.

1. In the browser window, navigate to [http://www.contoso.com](http://www.contoso.com), making sure that you specify the port number for the running deployment, if this is different from the standard HTTP port (80).

1. Now, navigate to [http://www.contoso.com/CRM](http://www.contoso.com/CRM) to view the CRM application running in the _Contoso_ site. 
	 
	![CRM virtual application mapped to the Contoso site](images/crm-virtual-application-mapped-to-the-contoso-site.png?raw=true "CRM virtual application mapped to the Contoso site")

	_CRM virtual application mapped to the Contoso site_ 

1. Repeat the previous step, only this time visit the same CRM application running in the _Litware_ site. 

1.	Close the browser window.


	
<a name="Exercise2"></a>
### Exercise 2: Using Start-Up Tasks to Register a COM Component ###

A start-up task is a command, either an executable or a script, executed prior to the start of a role instance. The command can perform set up tasks that are required to prepare the environment for the application, such as installing applications, registering COM components, configuring IIS settings, or registering performance counters.

In this exercise, you explore the use of start-up tasks to configure the environment where a service executes. To do this, you will use a sample Web application that requires a COM component to work.

<a name="Ex2Task1"></a>
#### Task 1 - Registering a COM Component ####

Many applications today still rely on functionality provided by "legacy" COM components. Moving these applications to the cloud requires that each virtual machine instance hosting the application have the necessary components installed and registered. Registration needs to be carried out upon role start up and requires administrative privileges.

In this task, you create a start-up task that registers the COM component required by the sample application.

>**Important**: Make sure that you have launched the dependency checker to set up the lab before starting this task. The setup procedure builds the COM component required by the solution from source code in the **Assets** folder. Note that you need to have Visual C++ installed for this purpose.

>In addition, to use the COM component, the identity of the process instantiating it must have access to the directory where the component is installed. To ensure this, avoid launching the service from a location inside your user profile folder and instead, copy the project files to a folder with unrestricted permissions.
	
1. Start Microsoft Visual Studio 2010 as an administrator.

1. Open the **Begin** solution located in **Ex2-StartupTasks** in the **Source** folder of the lab. The solution contains a Microsoft Azure project and a Web role that makes use of the **LegacyCOM** component.
	
	>**Note:** The **LegacyCOM** project, located in the **Assets** folder of the lab, implements a very simple COM library using Active Template Library (ATL). The library contains a single component class with a method that receives a name parameter and returns a greeting message. 
	
1. Press **F5** to build and run the application. During the build process in Visual Studio, a pre-build event registers the COM component. A message box should notify you when the registration succeeds. Click **OK** to proceed.
	 
	![Successful COM component registration](images/successful-com-component-registration.png?raw=true "Successful COM component registration")

	_Successful COM component registration_ 

	>**Note:** Normally, the Visual C++ development environment takes care of the registration. Here, the automatic registration in the **LegacyCOM** project has been intentionally disabled and instead, a post-build event configured to handle this requirement and to produce an explicit message to notify you when this occurs. Later in this task, you will see that the component, which only needs to be registered to build the solution, will be unregistered by a post-build step.
 
1. Wait for the application to launch in the Compute Emulator and for the Home page to open in your browser. Then, enter your name and then click **Greet me!**. Notice that the application responds with a greeting message generated by the COM component.
	 
	![Running application showing the output from the COM component](images/running-application-showing-the-output-from-t.png?raw=true "Running application showing the output from the COM component")

	_Running application showing the output from the COM component_ 

1. Close the browser window to stop debugging and shut down the application.

1. In the **Build Events** tab of the **SampleWebApp** project's properties, add a post-build event to unregister the COM component after the Web application is built. Use the following command line.

	<!-- mark: 1,2 -->
	````Post-buildEventCommandLine
	IF DEFINED PROCESSOR_ARCHITEW6432 (SET DEV_PLATFORM=%PROCESSOR_ARCHITEW6432%) ELSE (SET DEV_PLATFORM=%PROCESSOR_ARCHITECTURE%)
	regsvr32.exe /u "$(ProjectDir)\%DEV_PLATFORM%\LegacyCOM.dll"
	````
	
	>**Note:** To build the Web application, the component must be registered on the development machine. However, because you are using the same machine to launch the application in the compute emulator, you need to ensure that the COM component is not registered when you test the startup task; otherwise, you will be unable to determine whether the task succeeded. 
	
1. You will now execute the application without the required COM registration. Press **F5** to build and run the application in the compute emulator. Notice that when the solution builds, you first receive a notification that the COM component has been registered, and then, after the build completes, a second notification indicating that the component has been unregistered. Click **OK** to dismiss both message boxes.
	 

	![Successful unregistration of the COM component after the build completes](images/successful-unregistration-of-the-com-componen.png?raw=true "Successful unregistration of the COM component after the build completes")

	_Successful unregistration of the COM component after the build completes_ 

1. Wait for the application to launch in the compute emulator and for the Home page to open in your browser. In the browser window, enter your name and click **Greet me!**. Notice that after submitting the form, an exception is raised and execution halts in the debugger. 
	 
	![COMException raised when the COM component is not registered](images/comexception-raised-when-the-com-component-is.png?raw=true "COMException raised when the COM component is not registered")

	_COMException raised when the COM component is not registered_ 

1. Press **F5** to continue execution and allow the exception to be handled by ASP.NET.

	![ASP.NET unhandled exception handler](images/aspnet-unhandled-exception-handler.png?raw=true "ASP.NET unhandled exception handler")

	_ASP.NET unhandled exception handler_ 

1. Close the browser window to stop debugging and shut down the application.

1. Next, define a startup task to set up the role and register the COM component. To do this, open the **ServiceDefinition.csdef** file in the Microsoft Azure project, locate the **WebRole** element in the service model and inside it, insert a **Startup** element with a single task, as shown below.
	
	(Code Snippet - _AdvancedWebAndWorkerRoles-Ex2-01-StartUpTask-XML_)
	
	<!-- mark: 18-20 -->
	````XML
	<ServiceDefinition name="CloudService" xmlns="http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition">
	  <WebRole name="SampleWebApp">
	    <Sites>
	      <Site name="Web">
	        <Bindings>
	          <Binding name="HttpIn" endpointName="HttpIn" />
	        </Bindings>
	      </Site>
	    </Sites>
	    <ConfigurationSettings>
	    </ConfigurationSettings>
	    <Endpoints>
	      <InputEndpoint name="HttpIn" protocol="http" port="80" />
	    </Endpoints>
	    <Imports>
	      <Import moduleName="Diagnostics" />
	    </Imports>
	    <Startup>
	      <Task commandLine="Register.cmd" executionContext="elevated" taskType="simple" />
	    </Startup>
     </WebRole>
	</ServiceDefinition>
	````
	
	>**Note:** The **Startup** element contains one or more **Tasks** that the role executes during its bootstrapping process. Each task includes a **commandLine** parameter that defines a program or a batch file to execute.

	>Tasks can specify an **executionContext** that allows them to run in the same security context as the role or with administrative privileges. Because registering a COM component requires writing information to a registry hive that is not accessible to regular users, the registration task uses an elevated execution context.
	
	>Additionally, tasks can specify a type that determines how they are scheduled. The following types are available:

	> **Simple:** the startup process launches the task and does not proceed until the task completes.

	> **Background:** the startup process launches the task in the background and then continues.

	> **Foreground:** similar to a background task, but the role cannot shut down until all the foreground tasks have ended.

1. Create a new text file in the root of the **SampleWebApp** project and name it **Register.cmd**. This file will contain a script to register the COM component.

1. Add the following content to the **Register.cmd** script. 
	
	<!-- mark: 1-2 -->
	````Command
	echo off
	regsvr32.exe /s "%~dp0%PROCESSOR_ARCHITECTURE%\LegacyCOM.dll"
	````

	>**Note:** The script shown above registers the COM component using the **regsvr32.exe** utility that is normally present in any Windows distribution and is available in the Microsoft Azure Guest OS.
	
	>It registers the version of the component appropriate to the platform where the role is running. Notice that the path that specifies the component to register uses the _%PROCESSOR_ARCHITECTURE%_ environment variable to select the folder that matches the current platform, either _amd64_ for 64-bit systems or _x86_ for 32-bit systems. This allows you to run the same script locally, during development, if you are working on a 32-bit OS, or in the cloud, when you deploy the application to the 64-bit Microsoft Azure environment. Note that the lab setup procedure builds the COM component for both platforms.

	>Normally, during registration, the **regsvr32.exe** utility produces a message box that requires confirmation. When hosted in the compute emulator, you can close the dialog and proceed with the startup process. However, when deploying to Microsoft Azure, if the task stops to wait for user input, it will block, causing the role to remain in a busy state and never start. For this reason, the registration is performed in silent mode by appending an **/s** parameter to the command line.
	
1. Set the **Copy to Output Directory** property of the script file to **Copy always** and make sure that its **Build Action** property is set to **None**.
	 

	![Copying the startup script to the output directory](images/copying-the-startup-script-to-the-output-dire.png?raw=true "Copying the startup script to the output directory")

	_Copying the startup script to the output directory_ 

1. Once again, press **F5** to build and run the application in the compute emulator. 

1. Wait for the application to launch in the Compute Emulator and for the Home page to open in your browser. Then, enter your name and then click **Greet me!**. Notice that the application responds with a greeting message confirming that the start-up task successfully registered the COM component.

1. Close the browser window.

1. Optionally, you may want to deploy the service package to Microsoft Azure and test the COM registration process in the cloud.

	>**Note:** For this lab, you have access to the source code for the COM component and are able to build and deploy the 64-bit version when deploying to Microsoft Azure. If you need to register a COM component for which you do not have source code, be aware that 32-bit components cannot be accessed directly by a 64-bit role process, and instead need to be launched in a separate surrogate process. 

<a name="Exercise3"></a>
### Exercise 3: Using Start-Up tasks to install PHP with the Web Platform Installer ###

Applications deployed to the cloud usually have a set of prerequisites that must be installed on the host computer to be able to work correctly. In this kind of scenario, having the capabilities of the **Microsoft Web Platform Installer** (Web PI) at your service can be very handy.

In this exercise, you explore another possible use for start-up tasks in conjunction with the Web Platform Installer. You will learn how to install complex components such as the binary files for a new scripting language. To do this, you will create a simple PHP page, requiring that the PHP script processor be installed in order to work.

<a name="Ex3Task1"></a>
#### Task 1 - Installing PHP with the Web Platform installer ####

In this task, you will use multiple start-up tasks to install the Web Platform Installer and then install PHP using the Web PI.

1. Start Microsoft Visual Studio 2010 as an administrator.

1. Open the **Begin** solution located in **Ex3-InstallPHP** in the **Source** folder of the lab. The solution contains a Microsoft Azure project and a Web role to which you will add a PHP page.

1. Add a new PHP page at the root of the **PHPWebRole** project named **info**.**php**. To do this, you can select the **Text** **File** template in the **Add** **New** **Item** dialog and type _info.php_ in the **Name** field.
	 
	![Creating the info.php page](images/creating-the-infophp-page.png?raw=true "Creating the info.php page")

	_Creating the info.php page_ 

1. Add the following code to the **info**.**php** file.
	
	<!-- mark: 1-3-->
	````PHP
	<?php 
		phpinfo();
	?>
	````
	
1. Now, open the **Web.config** file, locate the **system.webServer** element, and then insert the following **defaultDocument** element to set the default document as **info.php**.

	(Code Snippet - _AdvancedWebAndWorkerRoles-Ex3-01-DefaultDocument-XML_)

	<!-- mark: 5-9-->
	````XML
	<configuration>
	  ...
	  <system.webServer>
	    <modules runAllManagedModulesForAllRequests="true"/>
	    <defaultDocument>
	      <files>
	        <add value="info.php" />
	      </files>
	    </defaultDocument>
     </system.webServer>
	</configuration>
	````
	
1. Set the **InstallPHP** project as the solution's start-up project.

1. Press **F5** to launch the Web Role and wait for the web browser window to appear, which will indicate that the role has been initialized. As PHP is not installed, you will receive an error message like the one shown in the following figure. If the default document is not shown by default, try browsing to **info.php**.
	
	
	![Error when PHP is not installed](images/error-when-php-is-not-installed.png?raw=true "Error when PHP is not installed")

	_Error when PHP is not installed_ 

1. Close the web browser window.

1. In **Solution Explorer**, double-click **InstallPHP.cmd** in the **PHPWebRole** project to examine this script file in the editor.
	 
	![Startup script to install PHP using the Web Platform Installer](images/startup-script-to-install-php-using-the-web-p.png?raw=true "Startup script to install PHP using the Web Platform Installer")

	_Startup script to install PHP using the Web Platform Installer_ 

	>**Note:** The **InstallPHP.cmd** script installs the most recent version of PHP using the Web Platform Installer command line tool [http://msdn.microsoft.com/en-us/library/gg433092.aspx](http://msdn.microsoft.com/en-us/library/gg433092.aspx). This tool simplifies the installation of the latest components of the Microsoft Web Platform, such as IIS, PHP, and SQL Server Express, among others, and enables you to automate the application and service installation of a Microsoft Azure role.

	>The script first enables the Windows Update service to allow the Web Platform Installer to use it for downloading components required by the PHP installation. Next, it launches the Web PI command line tool to install PHP. Once the installation is complete, it stops the Windows Update service and restores its startup mode to disabled.
	
	>**Important:** All startup script files have their **Build** **Action** set to **None** and **Copy to Output Directory** property set to **Copy always**. 
	
1. In Visual Studio, open the **ServiceDefinition.csdef** file in the **InstallPHP** Web role project.

1. Add the (highlighted) **Startup** configuration elements in the following code snippet, below the **ConfigurationSettings** element closing tag. In general, this section defines startup tasks and, in this case, includes the script required to install PHP on a Microsoft Azure host.

	(Code Snippet - _AdvancedWebAndWorkerRoles-Ex3-02-StartUpTasks-XML_)
	
	<!-- mark: 9-11 -->
	````XML
	...
	<WebRole name="PHPWebRole">
	  <Sites>
	    ...
	  </Sites>
	  <ConfigurationSettings>
	    ...
	  </ConfigurationSettings>
	  <Startup>
	    <Task commandLine="InstallPHP.cmd" executionContext="elevated" taskType="simple" />
	  </Startup>
	  <Endpoints>
	    ...
	  </Endpoints>
	  <Imports>
	    ...
	  </Imports>
	</WebRole>
	...
	````
	
1. Press **F5** to launch the Web role in the compute emulator. 

1. Wait for the role to start and show its default page. While the startup tasks execute to install PHP, Visual Studio may warn you one or more times that the role is taking longer than expected to start. If this happens, click **Yes** to continue waiting.
	 
	![Visual Studio warning during the PHP installation](images/visual-studio-warning-during-the-php-installa.png?raw=true "Visual Studio warning during the PHP installation")

	_Visual Studio warning during the PHP installation_

	>**Note:** Running the start-up task may take some time. Before continuing with the following steps, you should make sure PHP has been installed. You can check this in the **Programs and Features** list that can be accessed through the **Control Panel**.

1. Once the role starts successfully, a page similar to the one shown in the following figure is displayed, confirming that PHP was installed correctly.
	 
	![Confirming that the PHP installation was successful](images/confirming-that-the-php-installation-was-succ.png?raw=true "Confirming that the PHP installation was successful")

	_Confirming that the PHP installation was successful_

---

<a name="Summary"></a>

## Summary##

In this Hands-on Lab, you reviewed some advanced Microsoft Azure service model features. You saw how to enable hosting a Web role in Internet Information Server (IIS) to access its full set of features. By enabling IIS, you were able to host multiple sites in a single Web role and create virtual applications and directories. Additionally, you explored start-up tasks that you can use to prepare the role environment, and how to use them to register a COM component, as well as installing entire binary file sets for a scripting language.
