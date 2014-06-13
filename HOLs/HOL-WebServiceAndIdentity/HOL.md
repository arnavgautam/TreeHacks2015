<a name="HOLTitle"></a>
# Web Services and Identity in Microsoft Azure #

---

<a name="Overview"></a>
## Overview ##

Windows Identity Foundation can simplify access to your Windows Communication Foundation (WCF) services, by providing the usual claims-based identity arsenal of good practices: authentication externalization, location independence, decoupling from credential types and many others. There is no reason for you not to enjoy the same advantages when you host your WCF services in Microsoft Azure: there are few practicalities that are intrinsic to the hosting platform, but the steps you need to follow are largely the same whether you are deploying your services on-premises or in the cloud. If you want to be fully aware of the differences between the two cases, you can optionally go through the lab "Web Services and Identity for Visual Studio 2010 Developers" from the [Identity Training Course](http://msdn.microsoft.com/en-us/IdentityTrainingCourse) and learn about how to use WCF and WIF on-premises before starting the current lab: please note that it is entirely optional, as this HOL is self-contained and independent.

This lab is a step by step guide that will help you to use claims-based identity for handling authentication and access management for your WCF services hosted in Microsoft Azure; it will show you how you can still take advantage of local identities for authenticating your users, despite the fact that your services are now hosted in the cloud. The lab will walk you through all the practicalities of taking advantage of the unique characteristics of the Microsoft Azure environment from your Windows Identity Foundation settings.

More precisely, you will learn how to:

- Use Windows Identity Foundation with WCF services hosted in Microsoft Azure
- Trusting an on-premises STS from a WCF service hosted in Microsoft Azure
- Using WIF & WCF tracing for a WCF service hosted in Microsoft Azure, taking advantage of blob storage for the traces
- Configure a WCF service to use load balancing
- Deploy a WCF service secured via WIF to the Microsoft Azure cloud

Windows Identity Foundation can do much more than what we cover in this lab: we hope that the skills you will learn here will help you in your further explorations of identity development.

The first lab will show you the process to configure a weather service to trust an on-premises development STS, and run the entire solution in the Compute Emulator. The second lab will add diagnostics and load balancing features to the WCF service implemented in the first lab. Finally, the third lab will walk you through the steps for running the solution to Microsoft Azure, which trusts an on-premises STS, generates diagnostic logs, and provides load balancing facilities. As shown on the figure below, an already provided client will be used to consume the WCF service running on the Compute emulator and afterwards in Microsoft Azure.

 ![A visual summary of what you will build in this lab](./images/A-visual-summary-of-what-you-will-build-in-this-lab.png?raw=true "A visual summary of what you will build in this lab")
 
_A visual summary of what you will build in this lab_

<a name="Objectives"></a>
### Objectives ###

In this hands-on lab, you will learn how to:

- Use Windows Identity Foundation for handling access to a WCF service hosted in the Microsoft Azure DevFabric by reusing on-premises identities
- Add STS references on a WCF service hosted in Microsoft Azure
- Add service references to a client which points to a WCF service hosted in Microsoft Azure
- Configure a WCF service to emit WIF and WCF traces in blob storage, and retrieve traces for offline analysis
- Provide custom SecurityTokenHandler and ServiceBehavior classes for enabling a WCF service to take advantage of load balancers
- Deploy to the Microsoft Azure staging and production evnironments a WCF service secured via WIF

<a name="Prerequisites"></a>
### Prerequisites ###

You must have the following items to complete this lab:

- Microsoft© Internet Information Services (IIS) 7.0 (with ASP.NET component, Static Content Support, and a Localhost SSL certificate installed)
- [Microsoft© .NET Framework 4.0](http://go.microsoft.com/fwlink/?linkid=186916)
- [Microsoft© Visual Studio 2010](http://www.microsoft.com/visualstudio/en-us/products/2010-editions)
- [Microsoft Azure Tools for Microsoft Visual Studio 1.7](http://www.microsoft.com/windowsazure/sdk/)
- [Microsoft© Windows Identity Foundation Runtime](http://support.microsoft.com/kb/974405)
- [Microsoft© Windows Identity Foundation SDK](http://www.microsoft.com/downloads/details.aspx?FamilyID=c148b2df-c7af-46bb-9162-2c9422208504)
- A Microsoft Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

>**Note:** This lab was designed to use Windows 7 Operating System.

<a name="Setup"></a>
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

>**Note:** Make sure you have checked all the dependencies for this lab before running the setup.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.

1. Right click the **Setup.cmd** file and click **Run as administrator**. This will launch the setup process that will configure your environment and install the Visual Studio code snippets for this lab.

1. Once you passed all the dependencies, the setup script will proceed with the certificates installation. Press **Y** if you want to continue with the required certificates installation.

	> **Note:** If you already have a "localhost" certificate needed by another application, ensure to make a backup copy of it before continue with the lab's certificates installation.

 	![Certificates installation finished](./images/Certificates-installation-finished.png?raw=true "Certificates installation finished")
 
	_Certificates installation finished_

	> **Note:** If you are running Windows 7 you might not see this window.

1. When finished press any key to close the setup console.

> **Note:** In addition to the setup script, inside the **Source\Setup** folder of this lab, there is a **Cleanup.cmd** file you can use to uninstall all the code snippets installed by setup scripts.
>
> When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

 
<a name="CodeSnippets"></a>
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2010 to avoid having to add it manually. 

---

<a name="Exercises"></a>
## Exercises ##

The following exercises make up this hands-on lab:

1. Using the Windows Identity Foundation with a WCF Service in Microsoft Azure

 
> **Note:** Each exercise is accompanied by a starting solution located in the Begin folder of the exercise that allows you to follow each exercise independently of the others. Please be aware that the code snippets that are added during an exercise are missing from these starting solutions and that they will not necessarily work until you complete the exercise. Inside the source code for an exercise, you will also find an End folder containing a Visual Studio solution with the code that results from completing the steps in the corresponding exercise. You can use these solutions as guidance if you need additional help as you work through this hands-on lab.

Estimated time to complete this lab: **60 minutes**


<a name="GettingStarted"></a>
## Getting Started: Setting up the Certificates and Local STS ##

<a name="GettingStartedTask1"></a>
#### Task 1 - Generating the Required Certificates ####

During this task, you will create an X.509 certificate used by the service you will implement in the Exercises of this lab. Your service will need a certificate in order to be able to publish endpoints on a SSL channel; the same certificate will be used for handling token encryption and decryption. Microsoft Azure expects to find X.509 certificates for cloud solutions in specific stores, hence it will be necessary to register your service certificate accordingly.

Note that in a real application you would probably obtain a certificate from a trusted certificate authority, and that the subject name would follow whatever DNS name you want to ultimately assign to your service. For the sake of simplicity, in this lab we will provide you with scripts for generating self-signed certificates which are not suitable for production use.

To do this, you will execute a script provided as part of the assets of the Lab.

1. Open a **Microsoft Visual Studio 2010 Command Prompt** with administrator privileges. From **Start | All Programs | Microsoft Visual Studio 2010 | Visual Studio Tools**, right-click **Visual Studio 2010 Command Prompt** and select **Run as administrator**.

1. Navigate to the **Assets\AzureCertificates** folder inside the **Source** folder of this Lab.

1. Execute the **SetupCertificates.cmd** script. The name you provide will be used as the subject of the generated certificate, in the form _**{yourprojectname}.cloudapp.net**_. Since we plan to use SSL, the subject must correspond to the URI of the application if you want to prevent warnings about mismatches. Remember that if you plan to deploy to the cloud, the name you pick must be unique: we suggest to use a name of a form that will remind you of the project's purpose, such as **wifwcfwazlab_{companyname}_.cloudapp.net**. Make sure to use only lower case letters.

	>**Note:** During this lab, "foo" will be used as sample service project name. When you see it, you should provide the name that you used to create your Cloud Service.

	>**IMPORTANT:** If the name you pick at this point will end up being already in use, if you want to perform the last task of the lab (deploy to the public cloud) you will need to choose a new, unique name and repeat various steps in the lab.

	````CMD
	SetupCertificates.cmd
	````

	>**Note:** This script will perform the following tasks:

	>\- Create a certificate for your Relying Party (RP) application using the **MakeCert** command and store it in the **LocalMachine\Personal** store.

	>\- Copy the generated certificate to the **CurrentUser\Personal** store so the Microsoft Azure Tools for Visual Studio can find it.

	>\- Copy the localhost certificate generated by Microsoft Azure SDK to the **LocalMachine\Trusted Root** store, so svcutil and "Add Service Reference" (used later in the lab) can connect to HTTPS metadata endpoints hosted in Compute Emulator. Note that, if this is the first time that you use the Microsoft Azure SDK on your machine, the certificate here mentioned may not have been already created: in that case you may need to perform extra steps later in the exercise.

 	![A visual summary of what you will build in Exercise 1](./images/A-visual-summary-of-what-you-will-build-in-Exercise-1.png?raw=true "A visual summary of what you will build in Exercise 1")
 
	_A visual summary of what you will build in Exercise 1_

	> **Note:** This script relies on Windows PowerShell scripts in its underlying implementation. In order to run those scripts, PowerShell execution policy must be set to **Unrestricted**. By default, PowerShell's execution policy is set to **Restricted**; that means that scripts - including those you write yourself - won't run. If needed, run the command below in an Administrator PowerShell command prompt to set your execution policy to **Unrestricted** (all scripts will run, regardless of where they come from and whether or not they've been signed. 

	> Set-ExecutionPolicy RemoteSigned

	> For more information, take a look at the following MSDN article: [http://technet.microsoft.com/en-us/library/ee176949.aspx](http://technet.microsoft.com/en-us/library/ee176949.aspx) 

1. Close the command prompt.

<a name="GettingStartedTask2"></a>
#### Task 2 - Creating the Local STS ####

Exercises in this lab need an STS to which you can outsource authentication to. You may have access to some local identity provider, for example your company's instance of ADFSv2, however that is not always the case. In order to ensure that you can successfully go through the lab without dependencies, we will make sure that you have access to a suitable identity provider by giving you instructions to create your own development time STS, hosted in your local IIS. WIF makes the task extremely easy, by providing a WCF project template which already contains most of the plumbing you need. Note that the STS you are building here can be used with on-premises services just as well.

>**Note:** If you do have an STS available in your environment, feel free to use it instead of creating one through this task. Just make sure to do it only if you know that your STS provides the necessary capabilities for the lab scenario and that you are confident enough in the topic to be able to adjust the instructions in the next tasks accordingly.

1. Open **Microsoft Visual Studio 2010** with administrator privileges. From **Start | All Programs | Microsoft Visual Studio 2010**, right-click **Microsoft Visual Studio 2010** and select **Run as administrator**. 

1. In the **File** menu, select **New | Web Site**.

1. In the **New Web Site** dialog, select the **WCF Security Token Service** template and press the **Browse** button at the right of the Location text box.

  	![Creating the WCF Security Token Service](./images/Creating-the-WCF-Security-Token-Service.png?raw=true "Creating the WCF Security Token Service")
 
	_Creating the WCF Security Token Service_

1. In the **Choose Location** dialog, select **Local IIS** at the left panel. You will create a new Web Application to host the STS, to do this, select the **Default Web Site** node on the tree at the right and press the **Create New Web Application** button (![LocalSTS and make sure that the Use Secure Sockets Layer option is checked.](./images/LocalSTS-and-make-sure-that-the-Use-Secure-Sockets-Layer-option-is-checked..png?raw=true "LocalSTS and make sure that the Use Secure Sockets Layer option is checked.")
 ). Set the name to **LocalSTS** and make sure that the **Use Secure Sockets Layer** option is checked.

	![Creating the new Web Application to host the STS](./images/Creating-the-new-Web-Application-to-host-the-STS.png?raw=true "Creating the new Web Application to host the STS")
 
	_Creating the new Web Application to host the STS_

1. Press **Open** to confirm the location and **OK** to finally create the STS. 

1. The development STS does not issue encrypted tokens by default: we need to go back to the STS project and change the STS configuration accordingly. Open the **Web.config** file of the **https://localhost/LocalSTS** project by double-clicking it in the **Solution Explorer**.

1. Inside the **\<appSettings>** section, set the signing certificate used by the STS by modifying the **SigninigCertificateName** property as it is shown below. Set its value to "IdentityTKStsCert" which is the CN for the certificate created as part of the setup for this Lab.

	<!-- mark:3 -->
	````XML
	  <appSettings>
	    <add key="IssuerName" value="ActiveSTS"/>
	    <add key="SigningCertificateName" value="CN=IdentityTKStsCert"/>
	    <add key="EncryptingCertificateName" value
	         ="CN=DefaultApplicationCertificate"/>
	  </appSettings>
	````

1. Inside the **\<appSettings>** section, set the certificate used by the STS to the one created for the Relying Party in previous task, modifying the **EncryptingCertificateName** property as it is shown below. Remember to update the placeholder with your actual Cloud Service name you used in the previous task.

	<!-- mark:4 -->
	````XML
	  <appSettings>
	    <add key="IssuerName" value="ActiveSTS"/>
	    <add key="SigningCertificateName" value="CN=IdentityTKStsCert"/>
	    <add key="EncryptingCertificateName" value
	         ="CN={yourprojectname}.cloudapp.net"/>
	  </appSettings>
	````

1. Metadata retrieval from the STS will be over https. To enable this, replace the **httpGetEnabled** attribute inside the **serviceMetadata** element to **httpsGetEnabled** (note the "s"):

	<!-- mark:6 -->
	````XML
	<system.serviceModel>
	   ...
	    <behaviors>
	        <serviceBehaviors>
	            <behavior name="ServiceBehavior">
	                <serviceMetadata httpsGetEnabled="true" />
	                <serviceDebug includeExceptionDetailInFaults="false" />
	            </behavior>
	        </serviceBehaviors>
	    </behaviors>
	</system.serviceModel>
	````

	>**Note:** Here we are changing the metadata retrieval to take place on https simply because it is good practice. It is by no means a Microsoft Azure-specific requirement; we may do the same on-premises.

1. Also update the **Mex endpoint** to use the **mexHttpsBinding** binding (note the "s"):

	<!-- mark:8 -->
	````XML
	<system.serviceModel>
	    <services>
	      <service name="Microsoft.IdentityModel.Protocols.WSTrust.WSTrustServiceContract" behaviorConfiguration="ServiceBehavior">
	        <endpoint address="IWSTrust13" binding="ws2007HttpBinding" contract="Microsoft.IdentityModel.Protocols.WSTrust.IWSTrust13SyncContract"  bindingConfiguration="ws2007HttpBindingConfiguration"/>
	        <host>
	          ...
	        </host>
	        <endpoint address="mex" binding="mexHttpsBinding" contract="IMetadataExchange" />
	      </service>
	    </services>
	    ...
	</system.serviceModel>
	````

1. Press **Ctrl + S** to save the **Web.config** file and close it.

<a name="Exercise1"></a>
### Exercise 1: Using Windows Identity Foundation with a WCF Service in Microsoft Azure ###

This exercise will walk you through the process of creating a WCF role, configure its service to trust an on-premises development STS, attach a client and run the entire solution in the Compute Emulator. This is easily accomplished with only minor modifications to the procedure you would have followed for obtaining the same result on premises. The main changes accommodate the fact that in Microsoft Azure a service is hosted at different addresses according to the environment (local, staging, production), a situation you would have to deal with in any multi-staged environment.

You will start in Task 1 by implementing a simple weather service and testing its behavior without authentication using a client provided by the lab. Afterwards in Task 2, you will use WIF tooling to establish a trust relationship between the weather service and a local STS, by configuring the WCF service binding and behaviors collection in a way that will make the service require tokens from LocalSTS from all its future callers. Then in Task 3 and 4, you will set up an HTTPS secure endpoint between the WCF service and the STS using an X.509 certificate, using the Microsoft Azure Visual Studio tooling UI for associating it to the RelyingParty WCF Service Role. You will also enable the weather service HTTPS endpoint port. Finally in the verification, you will update the client to test the HTTPS endpoint of the weather service.

<a name="Ex1Task1"></a>
#### Task 1 - Implementing the Weather Service ####

The sample Relying Party application you will build in this lab will simulate a service providing weather-related information, hosted on a WCF Role of Microsoft Azure. In this task, you will open a Microsoft Azure Project project with a WCF Role already created. Then, you will implement the Weather Service and verify its behavior using a client provided by the lab.

> **Note:** You require an STS project and appropriate certificates to complete this exercise. If you have not already done so, complete the **Getting Started**  section.

1. Open **Microsoft Visual Studio 2010** with administrator privileges. From **Start | All Programs | Microsoft Visual Studio 2010**, right-click **Microsoft Visual Studio 2010** and select **Run as administrator**. 

1. Open the **WIFWCFonWindowsAzure.sln** solution file from the **\Source\Ex1-UsingWIFandWCF\Begin** folder of this lab.

1. You will first setup some basic configuration for running the WCF Role. In the Solution Explorer, double-click the **RelyingParty** node inside the **Roles** folder of the **CloudConfiguration** project to bring up its properties page.

1. In the **Configuration** tab, make sure that **.NET trust level** is set to **Full Trust**, and uncheck the **HTTP endpoint** check box inside the **Startup action** section to avoid launching the browser when you run the CloudConfiguration project.

	> **Note:** If you can't uncheck the **HTTP endpoint** check box, close Visual Studio, and open it again as Administrator.

 	![Configuration for not launching the browser.](./images/Configuration-for-not-launching-the-browser..png?raw=true "Configuration for not launching the browser.")
 
	_Configuration for not launching the browser._

1. Now, you will set the WCF Service Web Role endpoints configuration. To do this, select the **Endpoints** tab and set the **Public Port** to _8000_ for _Endpoint1_. This will make the service to be available in port 8000.

 	![Configuring HTTP WeatherService endpoint](./images/Configuring-HTTP-WeatherService-endpoint.png?raw=true "Configuring HTTP WeatherService endpoint")
 
	_Configuring HTTP WeatherService endpoint_

	> **Note:** The port value above are meant to avoid collisions with other processes listening for connections on the local machine. A typical example of this would be IIS, which usually reserves the use of ports 80 and 443 for itself. If you pick ports that collide with ports already in use by other processes, DevFabric will assign to your service random ports making development difficult. If port 8000 is already in use in your machine, make sure you choose a different port accordingly.

1. You will now provide an implementation of the weather service. To do this, right-click the **RelyingParty** project and select **Add | Existing Item**.

1. In the **Add Existing Item** dialog, navigate to the **Assets\Service** folder inside the **Source** folder of this lab, select the **IWeatherService.cs** and **WeatherService.svc** files and press **Add**. These files set up a weather service, which provides random 3 and 10 day forecasts based on a given zip code.

 	![Adding WeatherService files to the RelyingParty](./images/Adding-WeatherService-files-to-the-RelyingParty.png?raw=true "Adding WeatherService files to the RelyingParty")
 
	_Adding WeatherService files to the RelyingParty_

1. You will now test the service behavior using the **Client** project that is already included in the solution. To do this, press **Ctrl + F5** to run the Cloud project without attaching the Visual Studio debugger. The **Compute Emulator** and the **Storage Emulator** will be launched.

 	![Opening the Compute Emulator UI](./images/Opening-the-Compute-Emulator-UI.png?raw=true "Opening the Compute Emulator UI")
 
	_Opening the Compute Emulator UI_

1. Right-click the Azure icon tray and select Show **Compute Emulator UI**. In the **Microsoft Azure Compute Emulator**, ensure that the service has started successfully browsing the tree at the left panel looking for the **RelyingParty** role.

 	![Checking that the RelyingParty role is running correctly](./images/Checking-that-the-RelyingParty-role-is-running-correctly.png?raw=true "Checking that the RelyingParty role is running correctly")
 
	_Checking that the RelyingParty role is running correctly_

1. Back in **Visual Studio**, right-click the **Client** project and select **Add Service Reference**. The service will be listening on the localhost (127.0.0.1) on the port we specified in previous steps, that is to say 8000. Enter **http://127.0.0.1:8000/WeatherService.svc** on the **Address** text box, and then press **Go**.

 	![Adding a Service reference to the RelyingParty running on Compute Emulator](./images/Adding-a-Service-reference-to-the-RelyingParty-running-on-Compute-Emulator.png?raw=true "Adding a Service reference to the RelyingParty running on Compute Emulator")
 
	_Adding a Service reference to the RelyingParty running on Compute Emulator_

1. Leave the default Service Namespace and press **OK** to add the reference to the WCF Service.

1. You will now set up the **Client** project that is included in the solution to consume the weather service you just added. To do this, right-click the **ForecastForm** node, select **View Code**, and add the following statement.

	(Code Snippet - _WebServicesAndIdentityInAzure Lab - ForecastForm Using Statement_)

	<!-- mark:1 -->
	````C#
	using Client.ServiceReference1;
	````

1. Replace the **ShowForecast** method implementation with the following code:

	(Code Snippet - _WebServicesAndIdentityInAzure Lab - ForecastForm Consuming Weather Service_)

	<!-- mark:1-78 -->
	````C#
	private void ShowForecast(int days, int zipCode)
	{
	    using (WeatherServiceClient relyingParty = new WeatherServiceClient())
	    {
	        WeatherInfo weatherInfo = null;
	
	        try
	        {
	            this.Cursor = Cursors.WaitCursor;
	            this.sourceLabel.Text = "Loading...";
	
	            if (days == 3)
	            {
	                weatherInfo = relyingParty.GetThreeDaysForecast(zipCode);
	            }
	            else if (days == 10)
	            {
	                weatherInfo = relyingParty.GetTenDaysForecast(zipCode);
	            }
	
	            this.DisplayForecast(weatherInfo.Forecast);
	            this.sourceLabel.Text = string.Format(
	                CultureInfo.InvariantCulture,
	                "Source: {0}",
	                weatherInfo.Observatory);
	        }
	        catch (MessageSecurityException ex)
	        {
	            this.sourceLabel.Text = string.Empty;
	            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
	            relyingParty.Abort();
	        }
	        finally
	        {
	            this.Cursor = Cursors.Default;
	        }
	    }
	}
	
	private void DisplayForecast(Weather[] forecast)
	{
	    this.forecastPanel.Controls.Clear();
	
	    for (int i = 0; i < forecast.Length; i++)
	    {
	        PictureBox pic = new PictureBox();
	        GroupBox box = new GroupBox();
	
	        box.Text = string.Format(
	            CultureInfo.CurrentCulture,
	            "{0:ddd dd}: {1}",
	            DateTime.Today.AddDays(i),
	            forecast[i]);
	        box.Height = 145;
	        box.Width = 130;
	        pic.Dock = DockStyle.Fill;
	        pic.SizeMode = PictureBoxSizeMode.CenterImage;
	        box.Controls.Add(pic);
	
	        switch (forecast[i])
	        {
	            case Weather.Sunny:
	                pic.Image = Resources.Sunny;
	                break;
	            case Weather.Cloudy:
	                pic.Image = Resources.Cloudy;
	                break;
	            case Weather.Snowy:
	                pic.Image = Resources.Snowy;
	                break;
	            case Weather.Rainy:
	                pic.Image = Resources.Rainy;
	                break;
	        }
	
	        this.forecastPanel.Controls.Add(box);
	    }
	}
	````

1. To test the weather service behavior without authentication, right-click the **Client** project and select **Debug | Start new instance**.

1. In the **Weather Station client**, enter any **Zip Code** (for example: 1000) and press the **Get 3 Days** button. You should get the forecast for the following 3 days.

 	![Getting the forecast for the following 3 days note that the port number might be different ](./images/Getting-the-forecast-for-the-following-3-days-note-that-the-port-number-might-be-different-.png?raw=true "Getting the forecast for the following 3 days note that the port number might be different ")
 
	_Getting the forecast for the following 3 days (note that the port number might be different)_

1. Close the **Weather Station client**.

	Stop the WCF Service Web Role running on Compute Emulator from its UI. To do this, right-click the **Compute Emulator and Storage** icon on the **Windows' tray bar** and select **Show Compute Emulator UI**. In the tree at the left panel, right-click the current deployment and select **Remove**.

<a name="Ex1Task2"></a>
#### Task 2 - Establishing a Trust Relationship between the WCF Service and the Development STS ####

Now that we have the WCF service running and the STS, it's time to establish a trust relationship between them. In practical terms, that means that the WCF service binding and behaviors collection will be configured in a way that will make the service require tokens from LocalSTS from all its future callers. The bulk of the task will be performed by the WIF tooling, and specifically the **Add STS Reference** wizard (which in turn is the Visual Studio integration of **fedutil.exe**, a standalone tool provided in the WIF SDK).

1. Open the **Web.config** file of the **RelyingParty** project by double-clicking it in the **Solution Explorer.**

1. The **fedutil.exe** tool expects to find the WCF service configuration within the local **Web.config** file. But since **WCF for .Net Framework 4** relies on a machine level configuration file, a temporal service element needs to be added to the default configuration. To do this, add the following services configuration inside the _\<system.serviceModel>_ section. 

	<!-- mark:3-9 -->
	````XML
	  <system.serviceModel>
	    
	    <services>
	      <service name="RelyingParty.WeatherService">
	        <endpoint address=""
	                  binding="basicHttpBinding"
	                  contract="RelyingParty.IWeatherService" />
	      </service>
	    </services>
	
	    ...
	````

1. Press **Ctrl + S** to save the **Web.config** file and close it.

1. Right-click the **RelyingParty** project and select **Add STS reference**. The Federation Utility will be displayed.

	> **Note:** If the **Add STS Reference** item is not shown in the contextual menu within Visual Studio 2010, you can manually launch the tool by executing the **fedutil.exe** command found in **%ProgramFiles%\Windows Identity Foundation SDK\v4.0**.  Browse and select the RelyingParty **Web.config** as the Application configuration location.

	>![application-configuration-location](images/application-configuration-location.png?raw=true)

1. Enter **https://{yourprojectname}.cloudapp.net/** as Application URI, changing the **{yourprojectname}** placeholder for your Cloud Service name, and press **Next**.

 	![Specifying the projects Application URI](./images/Specifying-the-projects-Application-URI.png?raw=true "Specifying the projects Application URI")
 
	_Specifying the project's Application URI_

1. Press **Next** in the **Application Information** Wizard step.

 	![Selecting the service to configure](./images/Selecting-the-service-to-configure.png?raw=true "Selecting the service to configure")
 
	_Selecting the service to configure_

1. Here you specify the address of the STS you want to outsource authentication to, that is to say the LocalSTS project you created in the previous task. To do this, select the **Use an Existing STS** option. Set the metadata document location to **https://localhost/LocalSTS** and press the **Test Location** button (make sure not to include spaces at the end of the metadata document location field). 

	The wizard will inspect the STS site searching for a metadata document and will find the **FederationMetadata.xml** file, which will be shown in a new browser instance. The full file path will be added in the metadata document location field in the wizard window. Close the browser and press **Next**.

 	![Using existing STS created on the previous task](./images/Using-existing-STS-created-on-the-previous-task.png?raw=true "Using existing STS created on the previous task")
 
	_Using existing STS created on the previous task_

1. In **Security token encryption**, choose **Enable encryption**. Use the certificate created on the [Getting Started: Setting up the Certificates and Local STS](#segment1) section for the Relying Party selecting the **Select an existing certificate from store** option and pressing the **Select Certificate** button. 

	In the **Select certificate** dialog, chose the **{yourprojectname}.cloudapp.net** certificate and press **OK**. Press **Next** to continue with the wizard.

 	![Configuring encryption for the communication with the STS](./images/Configuring-encryption-for-the-communication-with-the-STS.png?raw=true "Configuring encryption for the communication with the STS")
 
	_Configuring encryption for the communication with the STS_

1. Press **Next** in the **Offered Claims** Wizard step.

 	![Showing Offered claims by the STS](./images/Showing-Offered-claims-by-the-STS.png?raw=true "Showing Offered claims by the STS")
 
	_Showing Offered claims by the STS_

	>**Note:** The Offered claims dialog lists all the claims that a RP application can request to the STS. Name and Role are the default claims which are hardcoded in the WIF STS template: if you would be running the wizard against a proper STS, for example an ADFSv2 instance in your organization, the list would change accordingly.

1. Check the Wizard summary, and press **Finish**.

<a name="Ex1Task3"></a>
#### Task 3 - Adding the Certificates to the Relying Party ####

In the following steps you are going to configure the certificate created on the [Getting Started: Setting up the Certificates and Local STS](#segment1) section, using the Microsoft Azure Visual Studio tooling UI for associating it to the RelyingParty WCF Service Role.

1. In the **Solution Explorer**, double-click the **RelyingParty** node inside the **Roles** folder of the **CloudConfiguration** project to bring up its properties page.

1. Select the **Certificates** tab. Add a new certificate clicking the **Add Certificate** button at the top of the page. Set the certificate's name to **{yourprojectname}.cloudapp.net** and then click the button labeled with an ellipsis (...) of the **Thumprint** column.

 	![Creating the Relying Partys certificate](./images/Creating-the-Relying-Partys-certificate.png?raw=true "Creating the Relying Partys certificate")
 
	_Creating the Relying Party's certificate_

1. In the **Select a certificate** dialog, choose the certificate named **{yourprojectname}.cloudapp.net** and press **OK**.

 	![Configuring the certificate to the Relying Party. Note that the certificates listed here are all ](./images/Configuring-the-certificate-to-the-Relying-Party.-Note-that-the-certificates-listed-here-are-all-.png?raw=true "Configuring the certificate to the Relying Party. Note that the certificates listed here are all ")
 
 	_Configuring the certificate to the Relying Party. Note that the certificates listed here are all from the **LocalMachine\Personal** store_
1. Now, you will set the HTTPS WCF Service Web Role endpoint configuration. Select the **Endpoints** tab to enter to the endpoints' configuration page.

1. Click the **Add Endpoint** button. In the name column fill with _HttpsIn,_ set the protocol to **https**_,_ set **Public Port** to 8443 name and chose the **{yourprojectname}.cloudapp.net** certificate from the **SSL Certificate name** combo box.

 	![Configuring endpoints for the Relying Party](./images/Configuring-endpoints-for-the-Relying-Party.png?raw=true "Configuring endpoints for the Relying Party")
 
	_Configuring endpoints for the Relying Party_

	>**Note:** The port values above are meant to avoid collisions with other processes listening for connections on the local machine. A typical example of this would be IIS, which usually reserves the use of ports 80 and 443 for itself. If you pick ports that collide with ports already in use by other processes, DevFabric will assign to your service random ports making development difficult. If ports 8000 and 8443 are already in use in your machine, make sure you choose different ports accordingly.

1. Select the **Configuration** tab, and uncheck both check boxes inside the Startup action section to avoid launching the browser for the HTTP and HTTPS endpoints when you run the **CloudConfiguration** project.

 	![Relying Party general configuration](./images/Relying-Party-general-configuration.png?raw=true "Relying Party general configuration")
 
	_Relying Party general configuration_

1. Press **Ctrl + S** to save the modified properties for the **RelyingParty** Role.

1. Close the **RelyingParty Role** property page.

<a name="Ex1Task4"></a>
#### Task 4 - Configuring the WCF Service HTTPS Endpoint ####

In this task you will update the WCF Weather Service to use the HTTPS endpoint.

1. Open the **Web.config** file of the **RelyingParty** project.

1. Add a **name** to the behavior element as shown in the code below. Make the name of the behavior _RelyingParty.WeatherServiceBehavior_:

	<!-- mark:5 -->
	````XML
	<system.serviceModel>
	  ...
	  <behaviors>
	     <serviceBehaviors>
	        <behavior name="RelyingParty.WeatherServiceBehavior">
	  ...
	</system.serviceModel>
	
	````

1. Add the configuration behavior for KB971842 to the **behavior** named **RelyingParty.WeatherServiceBehavior** inside the \<system.serviceModel> configuration section.

	(Code Snippet - _WebServicesAndIdentityInAzure Lab - Configuration Behavior_)

	<!-- mark:10-15 -->
	````XML
	    <system.serviceModel>
	    ...
	    <behaviors>
	      <serviceBehaviors>
	        <behavior name="RelyingParty.WeatherServiceBehavior">
	          ... 
	          <serviceCredentials>
	            ...
	          </serviceCredentials>
	          <useRequestHeadersForMetadataAddress>
	            <defaultPorts>
	              <add scheme="http" port="8000" />
	              <add scheme="https" port="8443" />
	            </defaultPorts>
	          </useRequestHeadersForMetadataAddress>
	        </behavior>
	      </serviceBehaviors>
	    </behaviors>
	````

	>**Note:** WCF 3.5 default metadata publishing mechanisms do not work too well in Microsoft Azure. The issue is in the way in which URIs are included in WSDL documents: the default behavior will include the internal ports used by the Microsoft Azure load balancer, which are not addressable from external callers. As a result, you cannot simply create a service reference using the standard tools (**svcutil** or **add service reference** in Visual Studio). The issue is described in a KB article, which provides a hotfix for resolving the problem. The hotfix is listed among the prerequisites for the lab. The **useRequestHeadersForMetadataAddress** behavior configuration enables the hotfix and induces WCF to use the load balancer's address instead of one internal node address.

	>You can find further information at [http://support.microsoft.com/kb/971842/](http://support.microsoft.com/kb/971842/).

1. The wizard configured the service to use **ws2007FederationHttpBinding**, however we need to exercise more control in the way in which we handle messages. Remove the current **ws2007FederationHttpBinding** section inside bindings and add the following custom binding. Remember to update **{yourprojectname}** label with your Cloud Service name.

	(Code Snippet - _WebServicesAndIdentityInAzure Lab - CustomBinding_)

	<!-- mark:4-29 -->
	````XML
<system.serviceModel>
   ...
    <bindings>
        <customBinding>
            <binding name="RelyingParty.IWeatherService">
                <security authenticationMode="SecureConversation"
	                    messageSecurityVersion="WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10"
                    requireSecurityContextCancellation="false">
                    <secureConversationBootstrap authenticationMode="IssuedTokenOverTransport"
	                                         messageSecurityVersion="WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10">
                        <issuedTokenParameters>
                            <additionalRequestParameters>
                                <AppliesTo xmlns="http://schemas.xmlsoap.org/ws/2004/09/policy">
                                    <EndpointReference xmlns="http://www.w3.org/2005/08/addressing">
                                        <Address>https://{yourprojectname}.cloudapp.net/</Address>
                                    </EndpointReference>
                                </AppliesTo>
                            </additionalRequestParameters>
                            <claimTypeRequirements>
                                <add claimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" isOptional="true" />
                                <add claimType="http://schemas.microsoft.com/ws/2008/06/identity/claims/role" isOptional="true" />
                            </claimTypeRequirements>
                            <issuerMetadata address="https://localhost/LocalSTS/Service.svc/mex" />
                        </issuedTokenParameters>
                    </secureConversationBootstrap>
                </security>
                <httpsTransport />
            </binding>
        </customBinding>
    </bindings>
</system.serviceModel>
	````

	>**Note:** The main reason for which we need a custom binding is that we want to set the attribute **requireSecurityContextCancellation** of the **\<security>** element to false hence moving to cookie mode. This will allow you, later in the lab, to take control of the session token and accommodate its processing to the load balanced environment of Microsoft Azure.

1. Add the **behaviorConfiguration** attribute to the **service** named **RelyingParty.WeatherService**. 

	<!-- mark:3 -->
	````XML
	<system.serviceModel>  
	   <services>
	    <service name="RelyingParty.WeatherService" behaviorConfiguration="RelyingParty.WeatherServiceBehavior">
	    ...
	   </services>
	</system.serviceModel>  
	
	````

1. Remove the hardcoded address created by Federation Utility from the endpoint configuration. To do this, replace the endpoint element with the one below:

	<!-- strike:4 -->
	````XML
	<system.serviceModel>
	  <services>
	    <service name="RelyingParty.WeatherService" behaviorConfiguration="RelyingParty.WeatherServiceBehavior">
	      	      <endpoint address="" binding="customBinding" contract="RelyingParty.IWeatherService" bindingConfiguration="RelyingParty.IWeatherService" />
	
	        ...
	      </service>
	    </services>
	    ...
	</system.serviceModel>
	````

1. Also add **Mex endpoint** that use the **mexHttpsBinding** binding (note the "s"):

	<!-- mark:7 -->
	````XML
	<system.serviceModel>
	  <services>
	    <service name="RelyingParty.WeatherService" behaviorConfiguration="RelyingParty.WeatherServiceBehavior">
	      <!--<endpoint address="https://foo.cloudapp.net/" binding="ws2007FederationHttpBinding" contract="RelyingParty.IWeatherService" bindingConfiguration="RelyingParty.IWeatherService_ws2007FederationHttpBinding" />-->
	      <endpoint address="" binding="customBinding" contract="RelyingParty.IWeatherService" bindingConfiguration="RelyingParty.IWeatherService" />
	
	      <endpoint address="mex" binding="mexHttpsBinding" contract="IMetadataExchange" />        
	
	    </service>
	  </services>
	</system.serviceModel>
	````

1. The metadata retrieval from the WCF Service Web Role will also be over https. To enable this, replace the **httpGetEnabled** attribute inside the **serviceMetadata** element with **httpsGetEnabled** (note the "s"):

	<!-- mark:7 -->
	````XML
	<system.serviceModel>
	  ...
	  <behaviors>
	    <serviceBehaviors>
	      <behavior name="RelyingParty.WeatherServiceBehavior">
	        <federatedServiceHostConfiguration name="RelyingParty.WeatherService" />
	        <serviceMetadata httpsGetEnabled="true" />
	        <serviceDebug includeExceptionDetailInFaults="false" />
	        <serviceCredentials>
	          ...
	        </serviceCredentials>
	        <useRequestHeadersForMetadataAddress>
	          ...
	        </useRequestHeadersForMetadataAddress>
	        </behavior>
	      </serviceBehaviors>
	    </behaviors>
	</system.serviceModel>
	````

1. Update the **thumbprint** attribute value for the **LocalSTS** trusted issuer with the one shown below. This is the thumbprint of the certificate that the **LocalSTS** is using for signing.

	<!-- mark:8 -->
	````XML
	<microsoft.identityModel>
	    <service name="RelyingParty.WeatherService">
	      <audienceUris>
	        <add value="https://{yourprojectname}.cloudapp.net/" />
	      </audienceUris>
	      <issuerNameRegistry type="Microsoft.IdentityModel.Tokens.ConfigurationBasedIssuerNameRegistry, Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35">
	        <trustedIssuers>
	          <add thumbprint="40A1D2622BFBDAC80A38858AD8001E094547369B" name="https://localhost/LocalSTS/Service.svc" />
	        </trustedIssuers>
	      </issuerNameRegistry>
	    </service>
	  </microsoft.identityModel>
	````

1. Press **Ctrl + S** to save the **Web.config** file and close it.

1. You will configure the **WeatherService** to listen for request in any address where it is available. To do this, open the **WeatherService.svc.cs** file for the **RelyingParty** project, and add a **ServiceBehavior** attribute to the WeatherService class definition as it is shown on the following code.

	(Code Snippet - _WebServicesAndIdentityInAzure Lab - ServiceBehavior Attribute_)

	<!-- mark:3 -->
	````C#
	namespace RelyingParty
	{
	    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
	    public class WeatherService : IWeatherService
	    {
	        ...
	    }
	}
	````

	> **Note:** Decorating your service class with that attribute has the effect of turning off the address filter, so that incoming messages with different **To** elements are accepted.

1. Press **Ctrl + S** to save the **WeatherService.svc.cs** file and close it.

<a name="Verification"></a>
#### Verification ####

Your service is finally ready to run using the certificates. You will now update the client to consume the project using the HTTPS encrypted endpoint.

In order to verify that you have performed every step in the exercise correctly, proceed as follows:

1. Press **Ctrl + F5** to run the Cloud project without attaching the Visual Studio debugger. The DevFabric will be launched. Proceed as in Task 1 to verify in the **Compute Emulator** that the service has started successfully.

1. In the **Client** project, right-click the **ServiceReference1** node inside the**Service References** folder and select **Configure Service Reference**. 

 	![Configuring the Client service reference](./images/Configuring-the-Client-service-reference.png?raw=true "Configuring the Client service reference")
 
	_Configuring the Client service reference_

1. The service will now be listening on the localhost (127.0.0.1) on the SSL port we specified in the earlier tasks, that is to say 8443. Enter **https://127.0.0.1:8443/WeatherService.svc** on the Address text box and press **OK**.

 	![Weather Service configuration update](./images/Weather-Service-configuration-update.png?raw=true "Weather Service configuration update")
 
	_Weather Service configuration update_

	> **Note:** If it is the first time that you configure https bindings on Compute Emulator, you will see the following warning message: _"ID1025: A certificate chain processed, but terminated in a root certificate which is not trusted by the trust provider."_

	>To work around it, you have to copy the 127.0.0.1 certificate created by Microsoft Azure Tools for Visual Studio (which is available at **LocalMachine\Personal** store) to the **LocalMachine\Trusted Root** store.  You can do that by using the certificates MMC in Windows.

	>Another way to work around this is running again the **SetupCertificates.cmd** script, without providing any Cloud Service name. The script will copy the 127.0.0.1 certificate to **LocalMachine\Trusted Root** for you.

	>Once you successfully performed one of the steps above, you can repeat the service reference step.

1. Open the **app.config** file inside the **Client** project.

1. Move the **AppliesTo** element from the **trust:SecondaryParameters** section to **additionalRequestParameters**. Remember to update **{yourprojectname}** label with your Cloud Service name.

	<!-- mark:11-15 -->
	````XML
	<system.serviceModel>
	    <bindings>
	        <customBinding>
	            <binding name="CustomBinding_IWeatherService">
	                <security ...>
	                    <localClientSettings .../>
	                    <localServiceSettings .../>
	                    <secureConversationBootstrap ...>
	                        <issuedTokenParameters>
	                            <additionalRequestParameters>
	                                <AppliesTo xmlns="http://schemas.xmlsoap.org/ws/2004/09/policy">
	                                    <EndpointReference xmlns="http://www.w3.org/2005/08/addressing">
	                                    <Address>https://{yourprojectname}.cloudapp.net/</Address>
	                                    </EndpointReference>
	                                </AppliesTo>
	                                <trust:SecondaryParameters xmlns:trust="http://docs.oasis-open.org/ws-sx/ws-trust/200512">
	                                ...
	                                </trust:SecondaryParameters>
	                            </additionalRequestParameters>
	                        </issuedTokenParameters>
	                    </secureConversationBootstrap>
	                </security>
	            </binding>
	        </customBinding>
	    </bindings>
	</system.serviceModel>
	````

	>**Note:** The RP provides indications about the desired AppliesTo value in the SecondaryParameters element, and svcutil does capture it in the client configuration. However the client has to explicitly accept the settings by moving them in the RST: after all, a compromised RP may try to spoof the token by providing malicious addresses overriding the AppliesTo.

1. The client is finally ready to call the RelyingParty WCF Service. To test it, right-click the **Client** project and select **Debug | Start new instance**.

1. In the **Weather Station client**, enter any **Zip Code** (for example: 1000) and press the **Get 3 Days** button. You should get the forecast for the following 3 days. Note that now the Source uses the https port.

 	![Getting the forecast for the following 3 days note that the port number might be different](./images/Getting-the-forecast-for-the-following-3-days-note-that-the-port-number-might-be-different.png?raw=true "Getting the forecast for the following 3 days note that the port number might be different")
 
	_Getting the forecast for the following 3 days (note that the port number might be different)_

1. Close the **Weather Station client**.

1. Stop the WCF Service Web Role running on Compute Emulator from its UI. To do this, right-click the **Compute Emulator and Storage** icon on the **Windows' tray bar** and select **Show Compute Emulator UI**. In the tree at the left panel, right-click the current deployment and select **Remove**.

	> **Note:** In this first set of tasks you developed a simple but complete scenario. You created a certificate for your service, a cloud project with your WCF role and a local development STS. You learned how to use the WIF tooling for outsourcing the service authentication to an STS, what practicalities need to be handled for preparing a WCF service to run in Microsoft Azure and how to develop a client for the service. Finally, you verified that everything works by running the service in the DevFabric environment. That's pretty much all you need to know for getting started to host in Microsoft Azure your own services and secure them; in the following Exercises we will explore more advanced aspects of the WIF and WCF synergy in Microsoft Azure.

----

<a name="Summary"></a>
## Summary ##

Taking advantage of existing identities in new applications is one of the fundamental requirements in today's distributed systems, and the new wave of cloud based services is no exception.

By completing this hands-on lab you have learned how to:

- Use Windows Identity Foundation with WCF services hosted in Microsoft Azure

- Trusting an on-premises STS from a WCF service hosted in Microsoft Azure

- Using WIF & WCF diagnostics features for a WCF service hosted in Microsoft Azure, taking advantage of blob storage for saving traces

- Configure WIF for a WCF service which uses load balancing

- Deploy a WCF service secured via WIF to the Microsoft Azure

As you have discovered while going through the lab, the claims-based approach to identity enabled by Windows Identity Foundation can be applied with little or no modification to both on-premises and cloud application.

We hope that the programming skills you learned in this and the other identity labs will enable you to write solutions with the confidence that no matter where your application will end up being deployed, you took care of identity in consistent and effective manner.


