<a name="AzureTrafficManager"></a>
# Windows Azure Traffic Manager - for Visual Studio 2012#

---

<a name="Overview"></a>
## Overview ##

The Traffic Manager is a load balancing solution that enables the distribution of incoming traffic among different cloud services in your Windows Azure subscription, regardless of their physical location. Traffic routing occurs as a the result of policies that you define and that are based on one of the following criteria:

- **Performance** - traffic is forwarded to the closest cloud service in terms of network latency
- **Round Robin** - traffic is distributed equally across all cloud services
- **Failover** - traffic is sent to a primary service and, if this service goes offline, to the next available service in a list

You assign each policy a DNS name and associate it with multiple cloud services. The load balancer responds to queries for the policy DNS name with the address of one of the associated cloud services that satisfies the criteria for the policy. Traffic Manager constantly monitors cloud services to ensure they are online and will not route traffic to any service that is unavailable. 

In this hands-on lab, you will explore Traffic Manager by publishing a very simple application to multiple cloud services, each one in a different geographic region, and then create several Traffic Manager policies to evaluate how the load balancer routes traffic across these services.

<a name="AboutDnsCaching"></a>
### About DNS Caching ###

The client resolver in Windows caches DNS host entries for the duration of their time-to-live (TTL). Whenever you evaluate Traffic Manager policies, retrieving host entries from the cache bypasses the policy and you could observe unexpected behavior. For example, accessing a service via the endpoint of a round robin policy will continue to yield the same host address for as long as the entry remains in the cache. In general, this is not a problem because the goal for this type of policy is to balance traffic among different clients. However, when you test the policy from a single client, you need to ensure that the DNS name is resolved for each request to observe the round robin behavior.

If the TTL of a DNS host entry in the cache expires, new requests for the same host name should result in the client resolver executing a fresh DNS query. However, browsers typically cache these entries for longer periods, even after their TTL has expired. To reflect the behavior of a Traffic Manager policy accurately when accessing the application through a browser, it is necessary to force the browser to clear its DNS cache before each request.

Throughout the lab, you will be required to access policy endpoints repeatedly to evaluate their behavior. To ensure predictable results, it is essential that you do not use a cached entry. Restarting the browser is one way to guarantee this condition, but requires closing every open browser window before each request, even those that display other sites.

To make this process simpler, the lab provides a registry script to shorten the lifetime of entries in the browser’s DNS cache. After executing the script and restarting the browser, evaluating a policy is simply a matter of waiting for the duration of the TTL and then refreshing the browser window. Note, however, that because this script changes Internet Explorer's configuration, you will only execute it inside a remote desktop session connected to one of the cloud services in the application.

For more information, see [Appendix B: Configuring the DNS Cache of the Browser](#AppendixB).

<a name="AboutTheWorldApplication"></a>
### About the World Application ###

The World Application is a sample ASP.NET application included in this hands-on lab to represent a global application that you could scale across multiple data centers by taking advantage of Traffic Manager. Although it does not implement any specific functionality of its own, the application does include some useful features that can assist you in the evaluation of Traffic Manager.

Some of these features are:

1. A customizable background and caption that allows you to determine, at a glance, which of the cloud services responds to a request

1. A timer-restarted with every page refresh-that shows the remaining time on the DNS host entry’s TTL and when it is safe to refresh the page to observe the result of a policy

1. A control panel showing the status of each cloud service deployed including commands to enable or disable traffic to individual services

1. Download links for registry scripts to configure the lifetime of DNS host entries in the browser’s cache

1. A timer-restarted after every change in the status of a service-that shows elapsed time since a change in the state of a service and estimating how long it takes Traffic Manager to become aware of the change

	![Home page of the World Application](Images/home-page-of-the-world-application.png?raw=true "Home page of the World Application")

	_Home page of the World Application_

Traffic Manager monitors cloud services by executing periodic HTTP GET requests to an endpoint that you specify when creating a policy. In the simplest case, this endpoint can be the URL to a file served by the application. Traffic Manager considers the service to be available if its monitoring endpoint responds with an HTTP status code of 200 OK within 5 seconds. For the application in this hands-on lab, this monitoring endpoint, located at the URL _/AppHealth_, has been implemented so that it can be disabled on demand. During the hands-on lab, you will exploit this feature to simulate a service failure.

<a name="Objectives"></a>
### Objectives ###

In this Hands-On Lab, you will learn how to:

- Define Traffic Manager policies
- Route traffic to the cloud service that offers the best performance
- Balance traffic across all cloud services
- Create a active / standby configuration that routes traffic to a primary service and, in the event of failure, to other secondary services
- Temporarily disable traffic to specific cloud services

<a name="Prerequisites"></a>
### Prerequisites ###

The following is required to complete this hands-on lab:

- [Microsoft .NET Framework 4.0](http://go.microsoft.com/fwlink/?linkid=186916)
- [Microsoft Visual Studio 2012](http://msdn.microsoft.com/vstudio/products/)
- [Windows Azure SDK and Windows Azure Tools for Microsoft Visual Studio 1.7](http://www.microsoft.com/windowsazure/sdk/)
- A Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)

>**Note:** This lab was designed to use Windows 8 Operating System.

---

<a name="Exercises"></a>
## Exercises ##

This hands-on lab includes the following exercises:

1. [Routing Traffic for Enhanced Performance](#Exercise1)
1. [Balancing Traffic across Cloud Services](#Exercise2)
1. [Increasing Availability with Failover Policies](#Exercise3)
1. [Managing Traffic Manager Policies](#Exercise4)

Estimated time to complete this lab: **60 minutes**.

> **Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

---

<a name="GettingStarted"></a>
### Getting Started: Preparing and Publishing the Application ###

During this hands-on lab, you will evaluate Traffic Manager by publishing a sample application to several cloud services. Furthermore, to obtain meaningful results when testing the performance load balancing method, these services should ideally be located in different geographic regions.

Before you begin, you need to configure the application to enable remote desktop. This will allow you to open a remote desktop session to each of your cloud services and test access to the application from other regions. 

In this exercise, you publish the application to multiple cloud services in your Windows Azure subscription. For each cloud service, you configure the application with settings appropriate to its region, create a cloud service in the Windows Azure Management Portal, upload the remote desktop certificate, and then deploy the application.

<a name="GettingStartedTask1"></a>
#### Task 1 – Preparing for Deployment ####

In this task, you prepare the application for deployment by enabling remote desktop access.

1. Open **Microsoft Visual Studio 2012** in elevated administrator mode by right-clicking the **Microsoft Visual Studio 2012** shortcut and choosing **Run as administrator**.

1. If the **User Account Control** dialog appears, confirm that you wish to proceed.

1. In the **File** menu, choose **Open** and then **Project/Solution**. In the **Open Project** dialog, browse to **\Source\WorldApp**, select **WorldApp.sln** and then click **Open**.

1. In **Solution Explorer**, right-click the **WorldAppService** project and select **Configure Remote Desktop**.

1. In the **Remote Desktop Configuration** dialog, check **Enable connections for all Roles**.

	![Configuring Remote Desktop settings](Images/configuring-remote-desktop-settings-for-all-r.png?raw=true "Configuring Remote Desktop settings")

	_Configuring Remote Desktop settings_

1. Expand the drop down list labeled **Create or select a certificate to encrypt the user credentials** and select **Create**. 

1. In the **Create Certificate** dialog, enter a name to identify the certificate, for example, _AzureRemote_, and then click **OK**.

	![Creating a certificate for Remote Desktop connections](Images/creating-a-certificate-for-remote-desktop-con.png?raw=true "Creating a certificate for Remote Desktop connections")

	_Creating a certificate for Remote Desktop connections_

1. Now, back in the **Remote Desktop Configuration** dialog, choose the newly created certificate from the drop down list, enter the name of the user that you will use to connect remotely to your role-this can be any name of your choice-enter a password and confirm it, and leave the account expiration date unchanged.

	![Configuring Remote Desktop settings](Images/configuring-remote-desktop-settings.png?raw=true "Configuring Remote Desktop settings")

	_Configuring Remote Desktop settings_

1. Before you close the dialog, click **View** next to the certificate drop down list. In the **Certificate** dialog, switch to the **Details** tab and click **Copy to File**. Follow the wizard to export the certificate to a file making sure that you **choose the option to export the private key**. Save the resulting file to a suitable location in your hard disk. You will need to upload this file to the Management Portal later, once you create a cloud service for your role.

1. Click **OK** to close the **Remote Desktop Configuration**.

<a name="GettingStartedTask2"></a>
#### Task 2 - Configuring Storage Account Credentials ####

In this task, you configure the credentials used by the application to access your Storage account.

> **Note:** All deployments used in this hands-on lab must share the same _DataConnectionString_ setting.

1. In **Solution Explorer**, expand the **Roles** node in the **WorldAppService** project, and then double-click the **WorldApp** role to open its properties window.

1. In the **WorldApp [role]** properties window, select the **Settings** tab, and then locate the **DataConnectionString** setting. Replace the placeholder labeled _[YOUR_STORAGE_ACCOUNT_NAME]_ with the name of your storage account and _[YOUR_STORAGE_ACCOUNT_KEY]_ with its key.

	![Configuring storage account credentials in Visual Studio](Images/configuring-storage-account-credentials-in-vi.png?raw=true "Configuring storage account credentials in Visual Studio")

	_Configuring storage account credentials in Visual Studio_

1. Repeat the previous step to configure the storage credentials for **Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString** setting. This is the storage account used for diagnostics.

<a name="GettingStartedTask3"></a>
#### Task 3 - Publishing the Application to Multiple Regions ####

In this task, you create the necessary cloud services in your Windows Azure subscription and then publish the application to different regions. You need to create at least two cloud services to be able to complete the steps in this lab, but three is the recommended number.

Before publishing each cloud service, you update its configuration to specify a number of settings that will allow you to identify which cloud service responds to a given request.

The following procedure describes the steps required to create a cloud service and publish the application to it. You need to repeat this procedure for each cloud service that you publish. The table below suggests values for the configuration settings in each deployment, but you may choose different values if desired.

| **URL**            | **Region**       | **Background** |
|------------------------------|---------------------------|------------------|----------------|
[appname]-europe-north    | North Europe     | _#5c87b2_      |
[appname]-us-northcentral | North Central US | _#ff9900_      |
[appname]-asia-east       | East Asia        | _#993333_      |

The pattern followed by the cloud service URL prefix is **_[appname]-[region]_**, where **_[appname]_** is a unique identifier common to all cloud services that you deploy during this hands-on lab and **_[region]_** is the Windows Azure region where the service is created. The full URL of a cloud service is based on its URL prefix and public so you must choose the **_[appname]_** portion to avoid colliding with cloud services from other users.

To create a cloud service and deploy the application:

1. Navigate to the [Windows Azure Management Portal](http://manage.windowsazure.com/) and log in with your Microsoft Account's credentials.

1. Click **New** | **Compute** | **Cloud Service** | **Quick Create**. Select a unique **URL** for your service, for example _worldapp-north-europe_. Refer to the table at the start of this task for suggested values. The dialog validates the URL prefix as you type it and warns you if the one you choose is unavailable.

1. To choose a region for the service, expand the drop down list labeled **Region / Affinity Group** and pick the region that corresponds to the service name and URL prefix used. Click **Create Cloud Service**.

	![Creating a new Cloud Service in Windows Azure](Images/creating-a-new-cloud-service-in-windows-azure.png?raw=true "Creating a new Cloud Service in Windows Azure")

	_Creating a new Cloud Service in Windows Azure_

1. Upload the certificate used to encrypt the Remote Desktop password to the newly created service. To do this, select **Cloud Services** from the left pane, and click the name of the service you just created. Then, click **Certificates** from the top of the cloud service.

	![Configuring the Cloud Service certificate](Images/configuring-the-cloud-service-certificate.png?raw=true "Configuring the Cloud Service certificate")

	_Configuring the Cloud Service certificate_

1. In the **Upload Certificate** dialog, select the certificate you created and exported during the previous task when you configured remote desktop, enter the assigned password, anc click **tick**.

	![Uploading the Cloud Service certificate](Images/uploading-the-cloud-service-certificate.png?raw=true "Uploading the Cloud Service certificate")

	_Uploading the Cloud Service certificate_

1. Return to Visual Studio and update the configuration settings used to identify the region where the service is located. To do this, in **Solution Explorer**, expand the **Roles** node of the **WorldAppService** project, double-click **WorldApp** to open the properties window for this role, and then select the **Settings** tab.

	> **Note:** The application includes several configuration settings that you update based on the Windows Azure region where you deploy the service, namely its region label, its URL prefix, and the background color for the UI. The latter setting allows you to identify which cloud service responds to a given request very easily.

1. In the **Settings** page, set the _HostedServiceRegion_, _HostedServiceUrlPrefix_, and _HostedServiceBackgroundColor_ settings to the values in the table at the start of this task that match the region chosen for this service.

	![Configuring the application based on its Windows Azure deployment region](Images/configuring-the-application-based-on-its-wind.png?raw=true "Configuring the application based on its Windows Azure deployment region")

	_Configuring the application based on its Windows Azure deployment region_

1. Once you configure the application, you can proceed to publish it. To do this, right-click the **WorldAppService** project in **Solution Explorer** and then select **Publish**.

1. In the **Publish Windows Azure Application** dialog, select the credentials for your Windows Azure subscription from the drop down list and click **Next**. 

	> **Note:** If you have not previously deployed an application to Windows Azure from Visual Studio, you first need to create and configure the necessary credentials. For more information, see [Appendix A: Configuring your Windows Azure Management Portal Credentials in Visual Studio](#AppendixA).

1. Next, select the cloud service you created for this region and set its **environment** to **Production**. Click **Publish** to begin the deployment.

	![Deploying the application to Windows Azure](Images/deploying-the-application-to-windows-azure.png?raw=true "Deploying the application to Windows Azure")

	_Deploying the application to Windows Azure_

	> **Note:** Traffic Manager policies only apply to cloud services in the production environment. You cannot route to services running in the staging environment.

1. After you start a deployment, you can examine the Windows Azure activity log window to determine the status of the operation. If this window is not visible, in the **View** menu, point to **Other Windows**, and then select **Windows Azure Activity Log**.  By default, the log shows a descriptive message and a progress bar to indicate the status of the deployment operation. To view detailed information about the operation in progress, click the green arrow on the left side of the activity log entry.

	![Monitoring the progress of the deployment operation](Images/monitoring-the-progress-of-the-deployment-ope.png?raw=true "Monitoring the progress of the deployment operation")

	_Monitoring the progress of the deployment operation_

1. Repeat the procedure described in this task to deploy the service to other regions referring to the previous steps for a detailed description. These steps have been summarized below:

	1. In the [Windows Azure Management Portal](http://manage.windowsazure.com/), create a cloud service in a different region than the one chosen previously. 

	1. Upload the certificate used to encrypt the remote desktop credentials. Use the same certificate that you created previously.

	1. Before proceeding, examine the **Windows Azure Activity Log** to ensure that the previous deployment has completed its _“Uploading…”_ phase. This is a precaution to avoid overwriting the service configuration file before it has been completely uploaded.

	1. Next, open the **Settings** page for the **WorldApp** role and update the _HostedServiceRegion_, _HostedServiceUrlPrefix_, and _HostedServiceBackgroundColor_ settings to reflect your choices when creating the current cloud service. Refer to the table at the start of this task for suggested configuration settings.

	1. Publish the project to the cloud service.

1. After you complete this task, the **Cloud Services** view in the Windows Azure Management Portal should list the cloud services for all regions that you deployed. You should have at least two cloud services in different regions. Make sure that the portal shows the status of all these services as _**Ready**_.

<a name="GettingStartedTask4"></a>
#### Task 4 - Configuring the DNS Cache for Testing ####

In the remainder of this lab, you will open remote desktop sessions to your cloud services and test the application from inside each session. As was described previously, for these tests to be effective the browser in each case needs to be configured by running a registry script that shortens the lifetime of DNS cache entries.

In this task, you open a remote desktop session to a role instance in each of the cloud services and configure the browser suitably.

1. In the Management Portal, locate your cloud service and click on its name to see the service management page. Click **Configure** from the top menu and then click **Remote** from the bottom pane.
	
	![Establishing a remote desktop session to a role instance](Images/establishing-a-remote-desktop-session-to-a-ro.png?raw=true "Establishing a remote desktop session to a role instance")

	_Establishing a remote desktop session to a role instance_

1. Click **Open** in Internet Explorer prompt to begin the Remote Connection.

	![Opening the Remote Desktop Connection](Images/opening-the-remote-desktop-connection.png?raw=true "Opening the Remote Desktop Connection")

	_Opening the Remote Desktop Connection_

1. Enter the remote connection account credentials you specified during Task1 of this exercise.

	![Remote connection account credentials](Images/remote-connection-account-credentials.png?raw=true "Remote connection account credentials")

	_Remote connection account credentials_

1. Inside the remote desktop session, open a browser window and navigate to the URL of the cloud service in the same region, namely _http://[appname]-[region].cloudapp.net_, where _[appname]-[region]_ is the URL prefix name that you configured for the cloud service. 

1. If access to the site is blocked by Internet Explorer’s enhanced security configuration, click **Add**.

	![Internet Explorer enhanced security configuration warning](Images/internet-explorer-enhanced-security-configura.png?raw=true "Internet Explorer enhanced security configuration warning")

	_Internet Explorer enhanced security configuration warning_

1. In the **Trusted Sites** dialog, click **Add** to insert the URL that you typed in the browser’s address bar to the trusted websites list. 

	![Adding the Traffic Manager to the trusted sites zone](Images/adding-the-traffic-manager-to-the-trusted-sit.png?raw=true "Adding the Traffic Manager to the trusted sites zone")

	_Adding the Traffic Manager to the trusted sites zone_
	
	>**Note:**  You can also add the URL for the Traffic Manager’s domain to the list of trusted websites. To do this, type _http://*.[appname].trafficmanager.net,_ where _[appname]_ is the prefix chosen for your application and then click **Add**.

1. Click **Close** twice to dismiss the enhanced security configuration dialogs and display the home page of the application.

1. Now, from the home page, download the registry script to configure the browser and shorten the lifetime of host entries in its DNS cache.

	![Downloading the browser DNS cache configuration script](Images/downloading-the-browser-dns-cache-configurati.png?raw=true "Downloading the browser DNS cache configuration script")

	_Downloading the browser DNS cache configuration script_

1. In the **File Download** dialog, click **Run** to launch the registry configuration script.

	![Configuring the DNS cache of the browser](Images/configuring-the-dns-cache-of-the-browser.png?raw=true "Configuring the DNS cache of the browser")

	_Configuring the DNS cache of the browser_

1. When prompted by the Registry Editor, click **Yes** to confirm that you wish to execute the script.

	![Registry editor security warning](Images/registry-editor-security-warning.png?raw=true "Registry editor security warning")

	_Registry editor security warning_

1. Click **OK** to close the message box confirming the successful update of the registry.

	![Successful update of the browser’s configuration](Images/successful-update-of-the-browsers-configurati.png?raw=true "Successful update of the browser’s configuration")

	_Successful update of the browser’s configuration_

1. Close all open browser windows and restart the browser for the changes to take effect.
1. Repeat the previous steps until you have configured the browser in each of your cloud services, one for each region.

	> **Note:** Each cloud service is deployed with 2 instances. Make sure that you always connect to the same instance, for example instance 0, so that you execute your tests in a previously configured session.

---

<a name="Exercise1"></a>
### Exercise 1: Routing Traffic for Enhanced Performance ###

Traffic Manager maintains a network performance table that it updates periodically and contains the round trip time between various IP addresses around the world and each Windows Azure data center. For the performance load balancing method, Traffic Manager forwards requests to the closest cloud service in terms of its network latency.

Fully evaluating the results of this policy requires accessing the application from different geographic locations. Fortunately, this does not require you to travel around the world to complete this exercise. Instead, because you are deploying the application to multiple cloud services in various regions, you can take advantage of remote desktop to log into a cloud service in each of these regions and then access the application from within the remote session. In this case, the role instance acts as both the client and the server.

<a name="Ex1Task1"></a>
#### Task 1 - Creating a Traffic Manager Performance Policy ####

In this task, you define a Traffic Manager policy that maximizes performance by forwarding traffic to the cloud service that offers the best performance for any given client. The load balancer bases its decision on performance tables that measure the round trip time of different IP addresses around the globe to each Windows Azure data center.

1. To use the Traffic Manager, you need to access the previous management portal version. In order to do this, hover the mouse pointer over **Preview** in the main page header and click **Take me to the previous portal**.

	![Switching to the Previous Portal](Images/switch-to-previous-portal.png?raw=true "Switching to the Prodution Portal")

	_Switching to the previous portal_

1. Once in the Production Portal, select the **Virtual Network** tab. Then, choose the **Policies** option under **Traffic Manager** and click **Create** on the ribbon.

	![Creating a Traffic Manager policy](Images/creating-a-windows-azure-traffic-manager-poli.png?raw=true "Creating a Traffic Manager policy")

	_Creating a Traffic Manager policy_

1. In the **Create Traffic Manager Policy** dialog, pick the subscription where you created the cloud services for this hands-on lab.

1. Specify the **load balancing method** as **Performance**.

1. Assign the **Hosted services to include in the policy** from the list of **Available DNS names**. Add all cloud services created for this hands-on lab. To add a service to the policy, select it and then click the arrow button in the middle.

	> **Note:** For subscriptions that contain a large number of cloud services, you can filter the list by typing the **[appname]** portion of the URL prefix in the text box shown above the list of DNS names.

1. Set the **Relative path and filename** of the monitoring endpoint to _/AppHealth_.

	> **Note:** Traffic Manager performs an HTTP (GET) request against the monitoring endpoint every 30 seconds to determine the service’s health. The service must respond with a 200 OK HTTP status code within 5 seconds; otherwise, Traffic Manager considers the service unhealthy and removes it from the load balancer rotation.

	> _/AppHealth_ is the URL of the monitoring endpoint for the lab’s application that can be disabled on demand to simulate a failure.

1. Next, choose a name for the **Traffic Manager DNS prefix**, for example, _performance.[appname]_, where _[appname]_ is the name chosen for your service.

	> **Note:** Traffic Manager uses the DNS prefix to build the URL for the service. Client requests to this URL are subject to the policy defined in Traffic Manager.

1. To complete the policy definition, set the DNS time-to-live (TTL) to _30_ seconds.

	> **Note:** The TTL determines for how long clients and secondary DNS servers cache a DNS host entry. Clients will continue to use a given cloud service until its entry in the cache expires.  For this hands-on lab, the TTL is set to the lowest possible value to allow policy results and changes in the status of services to be seen as early as possible. Note, however, that lowering this value increases DNS traffic and that you should consider keeping its default value for your production services.

	![Creating a Traffic Manager policy for load balancing based on network performance](Images/creating-a-traffic-manager-policy-for-load-ba.png?raw=true "Creating a Traffic Manager policy for load balancing based on network performance")

	_Creating a Traffic Manager policy for load balancing based on network performance_

1. Click **Create** to save the policy definition.

> **Note:** After creating a policy, wait for 2 minutes before you run any tests to allow the policy to propagate to all DNS servers.

<a name="Ex1Task2"></a>
#### Task 2 - Testing the Performance Policy ####

In this task, you test the performance policy defined previously. First, you test the policy from your own computer. Next, to observe the behavior of the policy when accessing the application from different geographic regions, you establish remote desktop sessions to every region where you deployed a cloud service. Inside each remote session, you browse the application to initiate a request originating from the same region as the cloud service. 

1. Open a browser window and navigate to _http://performance.[appname].trafficmanager.net/_, where _performance.[appname]_ is the DNS prefix name that you configured for the performance policy created in the previous task.  Notice the distinctive background color and the label in the first paragraph indicating the location of the cloud service that replies to the request. Verify that the response originated from the service closest to your current location.

	![Testing the Traffic Manager performance policy in your own machine](Images/testing-the-traffic-manager-performance-polic.png?raw=true "Testing the Traffic Manager performance policy in your own machine")

	_Testing the Traffic Manager performance policy in your own machine_

	> **Note:** For a performance policy, the load balancer determines which cloud service responds to a client request based on tables that record the round trip time between various IP addresses around the globe and each Windows Azure data center. Note, however, that while there is a strong correlation between distance and network latency and you would normally receive a response from the cloud service closest to your current location, other factors such as network topology and congestion could determine that you receive a response from a service that is further away.

1. Now, in the navigation pane of the Windows Azure Management Portal, select the **Hosted Services, Storage Accounts & CDN** tab and then the **Hosted Services** option.

1. Locate one of the cloud services created for this hands-on lab and expand its node to show its instances. Select one of the instances and then click **Connect** on the ribbon to open a remote desktop session to that instance.

1. Inside the remote desktop session, open a browser window and navigate to _http://performance.[appname].trafficmanager.net/_, where _performance.[appname]_ is the DNS prefix name that you configured for the performance policy. Verify that the response originates from the cloud service in the same region as the remote desktop session.

	> **Note:** Ensure that you connect to one of the role instances where you previously configured the DNS cache for the browser. Otherwise, click the link to download and run the registry configuration script and then restart the browser. After that, repeat the current step.

1. Open a command prompt window and type the following command:

	<!--mark: 1-->
	````CommandPrompt
	nslookup performance.[appname].trafficmanager.net
	````

	where _performance .[appname]_ is the DNS prefix name that you configured for the performance policy.

	Notice the **Name** and **Address** returned by the DNS query, which should match that of the cloud service nearest to your current location.

	![Using nslookup to determine the address returned by the performance policy](Images/using-nslookup-to-determine-the-address-retur.png?raw=true "Using nslookup to determine the address returned by the performance policy")

	_Using nslookup to determine the address returned by the performance policy_

1. Next, open a remote desktop session to each of the other regions and repeat the previous steps to access the application from that region.  Verify that in all cases, the response originates from the service hosted in the region where the remote desktop session is established.
 
	![Testing the performance policy from different geographic regions](Images/testing-the-performance-policy-from-different.png?raw=true "Testing the performance policy from different geographic regions")

	_Testing the performance policy from different geographic regions_

1. Do not close the remote desktop sessions. You will need them for the following exercises.

<a name="Ex1Task3"></a>
#### Task 3 - Simulating a Service Failure ####

In this task, you place the cloud service that is currently servicing your requests in an offline state to simulate a failure.

1. In the remote desktop session, navigate to _http://performance.[appname].trafficmanager.net/_ and determine the region of the cloud service that responds to the request.

1. Now, locate this region in the **Hosted Service Status** table and click **Disable** to set its state to _Offline_. Notice that the Health Monitor Timeout column starts a timer that shows the elapsed time since the state of the service changed.

	![Simulating the failure of a cloud service](Images/simulating-the-failure-of-a-hosted-service.png?raw=true "Simulating the failure of a cloud service")

	_Simulating the failure of a cloud service_
 
	> **Note:** The Health Monitor Timeout provides an estimate of how long it takes Traffic Manager to become aware of the change.

	> When a cloud service is disabled, its monitoring endpoint stops sending responses to simulate a failure. Traffic Manager performs a check of this endpoint at 30-second intervals and if it fails to receive a response to three consecutive polls, it considers the service as unavailable. Thus, it could take as much as 120 seconds for the service to failover.

	> After you disable a service, a timer on the page starts showing the elapsed time since the status of the service changed, providing an estimate of how long it takes the Traffic Manager to become aware of the failure.

1. Wait until the TTL expires and the **Health Monitor Timeout** shows a _Ready_ status.

1. Now, refresh the page and verify that the response now originates from a service located in a different region, presumably, the second closest region in terms of network latency.

1. Re-enable traffic to the previously disabled region and wait until the **Health Monitor Timeout** indicates that the change should be effective.

	> **Note:** When a service comes back online, Traffic Manager detects the change in its status within the next polling interval. Thus, the interval shown by the Health Monitor Timeout when switching from offline to online is only 30 seconds.

1. Refresh the page and confirm that the response is once again from the original cloud service.

---

<a name="Exercise2"></a>
### Exercise 2: Balancing Traffic across Cloud Services ###

The round robin load balancing method distributes load evenly among each of the cloud services assigned to the policy. It keeps track of the last cloud service that received traffic and sends traffic to the next one in the list of cloud services.

Traffic Manager removes a cloud service from the load balancer’s rotation if it determines that it is offline. Note, however, that if all services assigned to a policy are unavailable, Traffic Manager will ignore their status and return a response as if they were online.

<a name="Ex2Task1"></a>
#### Task 1 – Creating a Traffic Manager Round Robin Policy ####

In this task, you create a Traffic Manager policy that balances traffic evenly across all cloud services assigned to the policy.

1. In the Windows Azure Management Portal, select the **Virtual Network** tab. Then, choose the **Policies** option under **Traffic Manager** and click **Create** on the ribbon.

1. In the **Create Traffic Manager Policy** dialog, pick the subscription where you created the cloud services for this hands-on lab.

1. Specify the **load balancing method** as **Round Robin**.

1. Select the cloud services to include in the policy from the list of **Available DNS names**. Add all cloud services created for this hands-on lab. To add a service to the policy, select it and then click the arrow button in the middle.

	> **Note:** For subscriptions that contain a large number of cloud services, you can filter the list by typing the **[appname]** portion of the URL prefix in the text box shown above the list of DNS names.

1. Set the **Relative path and filename** of the monitoring endpoint to _/AppHealth_.

1. Next, choose a name for the **Traffic Manager DNS prefix**, for example, _roundrobin.[appname]_, where _[appname]_ is the name chosen for your service.

1. Now, set the DNS time to live (TTL) to 30 seconds.

	![Creating a Traffic Manager policy for load balancing traffic equally across cloud services](Images/creating-a-traffic-manager-policy-for-hosted.png?raw=true "Creating a Traffic Manager policy for load balancing traffic equally across cloud services")

	_Creating a Traffic Manager policy for load balancing traffic equally across cloud services_

1. Click **Create** to save the policy definition.

> **Note:** After creating a policy, wait for 2 minutes before you run any tests to allow the policy to propagate to all DNS servers.

<a name="Ex2Task2"></a>
#### Task 2 – Testing the Round Robin Policy ####

In this task, you test the round robin policy created during the previous task.

1. In a remote desktop session to one of your cloud services, open a browser window and navigate to _http://roundrobin.[appname].trafficmanager.net/_, where _roundrobin.[appname]_ is the DNS prefix name that you configured for the round robin policy.

	> **Note:** Ensure that you connect to one of the role instances where you previously configured the DNS cache for the browser. Otherwise, click the link to download and run the registry configuration script and then restart the browser. After that, repeat the current step.

1. Take note of the service that responds to the current request.

1. Wait for at least 30 seconds, which is the TTL configured for the current policy. For your convenience, a timer on the page provides a visual estimate of the remaining time. 

	![DNS host entry TTL](Images/dns-host-entry-ttl.png?raw=true "DNS host entry TTL")

	_DNS host entry TTL_

1. Once the TTL expires, refresh the page and notice that the background color of the page changes indicating that the response now originates from a cloud service located in a different region.

1. Wait until the TTL expires again and refresh the page one more time to verify that origin of the response has changed yet again.
 
	![Testing the round robin performance policy](Images/testing-the-round-robin-performance-policy.png?raw=true "Testing the round robin performance policy")

	_Testing the round robin performance policy_

1. Open a command prompt window and execute the following command. Take note of the name and address returned.

	<!--mark: 1-->
	````CommandPrompt
	nslookup roundrobin.[appname].trafficmanager.net 
	````

1. Repeat the command several times while waiting at least 30 seconds between attempts to allow the TTL to expire. Verify that the returned host cycles between each of the cloud services assigned to the round robin policy. 

	![Using nslookup to evaluate the round robin policy](Images/using-nslookup-to-evaluate-the-round-robin-po.png?raw=true "Using nslookup to evaluate the round robin policy")

	_Using nslookup to evaluate the round robin policy_

---

<a name="Exercise3"></a>
### Exercise 3: Increasing Availability with Failover Policies ###

When using a failover policy, if the primary cloud service is offline, traffic is sent to the next one in a sequence defined by the policy. To test this policy, the application provided with the lab includes a monitoring endpoint that Traffic Manager polls to determine whether the cloud service is available. 

<a name="Ex3Task1"></a>
#### Task 1 – Creating a Traffic Manager Failover Policy ####

In this task, you create a Traffic Manager policy that contains an ordered list of cloud services. Traffic is normally forwarded to the first service in this list—the primary service. If a service becomes unavailable, the load balancer switches to the next service in the list, and if that fails too, it will continue with the next in order.

1. In the Windows Azure Management Portal, select the **Virtual Network** tab. Then, choose the **Policies** option under **Traffic Manager** and click **Create** on the ribbon.

1. In the **Create Traffic Manager Policy** dialog, pick the subscription where you created the cloud services for this hands-on lab.

1. Specify the **load balancing method** as **Failover**.

1. Assign the **Hosted services to include in the policy** from the list of **Available DNS names**. Add all cloud services created for this hands-on lab. To add a service to the policy, select it and then click the arrow button in the middle.

	> **Note:** For subscriptions that contain a large number of cloud services, you can filter the list by typing the **[appname]** portion of the URL prefix in the text box shown above the list of DNS names.

1. Set the **Relative path and filename** of the monitoring endpoint to _/AppHealth_.

1. Next, choose a name for the **Traffic Manager DNS prefix**, for example, _failover.[appname]_, where _[appname]_ is the name chosen for your service.

1. Now, set the DNS time to live (TTL) to 30 seconds.

1. Unlike the performance and round robin policies, where order does not matter, the load balancer chooses an active service based on its position in the list of selected DNS names. Choose the cloud service that will act as the primary and then move it to the top of the list. Arrange the remaining (standby) services in the order in which they should be activated in case the preceding services in the list fail. To change the order of a cloud service, select its DNS name in the list and then click the up arrow button until the service is in the desired position.

	![Creating a failover Traffic Manager policy](Images/creating-a-failover-traffic-manager-policy.png?raw=true "Creating a failover Traffic Manager policy")

	_Creating a failover Traffic Manager policy_

1. Click **Create** to save the policy definition.

> **Note:** After creating a policy, wait for 2 minutes before you run any tests to allow the policy to propagate to all DNS servers.

<a name="Ex3Task2"></a>
#### Task 2 – Testing the Failover Policy ####

In this task, you test the failover policy created previously.

1. In a remote desktop session to one of your cloud services, open a browser window and navigate to _http://failover.[appname].trafficmanager.net/_, where _failover.[appname]_ is the DNS prefix name that you configured for the failover policy.

	> **Note:** Ensure that you connect to one of the role instances where you previously configured the DNS cache for the browser. Otherwise, click the link to download and run the registry configuration script and then restart the browser. After that, repeat the current step.

1. Verify that the cloud service that responds to the request is the one that you defined as primary by placing it at the top of the list of DNS names when you created the failover policy.

1. In the home page, the bottom half shows the status of each of your cloud services. Make sure that they are all Online.

	![Examining the status of the cloud services](Images/examining-the-status-of-the-hosted-services.png?raw=true "Examining the status of the cloud services")

	_Examining the status of the cloud services_

1. In the **Hosted Service Status** list, locate the primary service and then click **Disable** under **Manage Traffic**. Notice that the status of the service immediately changes to _Offline_ and the **Health Monitor Timeout** column begins displaying a timer.
 
	![Disabling a cloud service to simulate a failure](Images/disabling-a-hosted-service-to-simulate-a-fail.png?raw=true "Disabling a cloud service to simulate a failure")

	_Disabling a cloud service to simulate a failure_

	> **Note:** When a cloud service is disabled, its monitoring endpoint stops sending responses to simulate a failure. Traffic Manager performs a check of this endpoint at 30-second intervals and if it fails to receive a response to three consecutive polls, it considers the service as unavailable. Thus, it could take as much as 120 seconds for the service to failover.

	> After you disable a service, a timer on the page starts showing the elapsed time since the status of the service changed, providing an estimate of how long it takes the Traffic Manager to become aware of the failure and switch to the next service in the list.

1. Wait until the **Health Monitor Timeout** expires and its status is shown again as _Ready_. Then, refresh the page and verify that the response now originates from the second cloud service in the policy list.

1. Continue disabling cloud services following the same order as the policy list until all your cloud services are offline except one. After you disable each service, wait until the **Health Monitor Timeout** expires and then refresh the page. Confirm that the response is always from the next active (online) service in the policy list.

1. Disable the last remaining service, wait until the **Health Monitor Timeout** expires, and then refresh the page one more time. Notice that when there are no more services available, the load balancer acts as if all services are online and routes traffic to the first cloud service in the policy list again.

1. Re-enable each service again, one at a time, testing the policy after each change.

	> **Note:** When a service comes back online, Traffic Manager detects the change in its status within the next polling interval. Thus, the interval shown by the Health Monitor Timeout when switching from offline to online is only 30 seconds. 

---

<a name="Exercise4"></a>
### Exercise 4: Managing Traffic Manager Policies ###

In this exercise, you learn how to disable one of the cloud services in a policy to remove it from the load balancer rotation. Next, you disable a Traffic Manager policy to prevent routing through its URL.

<a name="Ex4Task1"></a>
#### Task 1 - Controlling Traffic to Individual Cloud Services ####

You can disable individual cloud services assigned to a Traffic Manager policy.

In this task, you disable one of the cloud services in the round robin policy and then verify that traffic stops being routed to this service when accessed through this policy.

1. Open the Traffic Manager interface in the Management Portal by selecting **Virtual Network** followed by the **Policies** option inside **Traffic Manager**.

1. Locate and expand the round robin policy. Then, select one of the cloud services assigned to this policy and click **Disable Traffic** on the ribbon.

	![Managing traffic to individual host services in a policy](Images/managing-traffic-to-individual-host-services.png?raw=true "Managing traffic to individual host services in a policy")

	_Managing traffic to individual host services in a policy_

1. In the **Disable Cloud Service in Traffic Manager policy** message box, click **Yes** to confirm that you wish to disable traffic to the selected service.

	![Disabling traffic to a cloud service in a Traffic Manager policy](Images/disabling-traffic-to-a-hosted-service-in-a-tr.png?raw=true "Disabling traffic to a cloud service in a Traffic Manager policy")

	_Disabling traffic to a cloud service in a Traffic Manager policy_ 

1. Now, repeat the steps you followed previously to test the round robin policy and confirm that the disabled cloud service is no longer included in the load balancer sequence.

	> **Note:** Disabling a cloud service in a Traffic Manager policy can be useful for temporarily removing a malfunctioning service or during maintenance tasks.

1. Next, open a remote desktop session to an instance of the cloud service that you disabled in the round robin policy.

1. In the remote desktop session, open a browser window and then access the service using the endpoint of the performance policy at _http://performance.[appname].trafficmanager.net/_. Notice that despite being disabled in the round robin policy, the service is still available when accessed through a different policy.

1. Re-enable the cloud service in the round robin policy. To do this, in the Management Portal, expand the round robin policy, select the disabled cloud service, and then click **Enable Traffic** on the ribbon.

	![Re-enabling traffic to a cloud service](Images/re-enabling-traffic-to-a-hosted-service.png?raw=true "Re-enabling traffic to a cloud service")

	_Re-enabling traffic to a cloud service_

1. Repeat the verification steps to confirm that the previously disabled cloud service is once again included in the load balancer sequence.

<a name="Ex4Task2"></a>
#### Task 2 - Disabling a Traffic Manager Policy ####

You can temporarily disable a Traffic Manager policy to prevent it from routing traffic and then re-enable it again later.

1. In a remote desktop session to one of your cloud services, open a browser window and navigate to _http://roundrobin.[appname].trafficmanager.net/_, where _roundrobin.[appname]_ is the DNS prefix name that you configured for the round robin policy.  Confirm that you are able to access the application.

1. Now, in the Management Portal, open the Traffic Manager interface by selecting **Virtual Network** followed by the **Policies** option inside **Traffic Manager**.

1. Select the round robin policy and then click **Disable Policy** on the ribbon.

	![Disabling a Traffic Manager policy](Images/disabling-a-traffic-manager-policy.png?raw=true "Disabling a Traffic Manager policy")

	_Disabling a Traffic Manager policy_

1. In the **Disable Traffic Manager policy** message box, click **Yes** to confirm that you wish to disable traffic to the specified policy.

	![Disabling traffic via the URL of a Traffic Manager policy](Images/disabling-traffic-via-the-url-of-a-traffic-ma.png?raw=true "Disabling traffic via the URL of a Traffic Manager policy")

	_Disabling traffic via the URL of a Traffic Manager policy_

1. In the remote desktop session, refresh the page to restart the TTL timer and then wait until it expires to ensure that you are not using a cached DNS entry.

1. Next, in the **Tools** menu of the browser, choose **Internet Options** to open the **Internet Options** dialog, then select the **General** tab and, under **Browsing history**, click **Delete**. 
	 
	![Managing the browsing history](Images/managing-the-browsing-history.png?raw=true "Managing the browsing history")

	_Managing the browsing history_

1. In the **Delete Browsing History** dialog, click **Delete files**. When prompted, click **Yes** to confirm that you want to delete all temporary Internet Explorer files. Finally, click **Close** followed by **OK** to close all open dialogs.
	 
	![Deleting temporary Internet files](Images/deleting-temporary-internet-files.png?raw=true "Deleting temporary Internet files")
	
	_Deleting temporary Internet files_
	
	> **Note:** Clearing the temporary files ensures that the browser does not render a cached version of the site if it cannot successfully resolve its address. Alternatively, in the **Internet Options** dialog, under **Browsing History**, click **Settings** and then select the option labeled **Check for newer versions of stored pages** to set its value to **Every time I visit the webpage**.

1. Now, refresh the page again and notice that the browser cannot display the application because the disabled policy no longer routes traffic with the specified URL.

	![Browser window showing that the application is no longer accessible through the disabled policy](Images/browser-window-showing-that-the-application-i.png?raw=true "Browser window showing that the application is no longer accessible through the disabled policy")

	_Browser window showing that the application is no longer accessible through the disabled policy_

1. Re-enable the round robin policy. To do this, in the Management Portal, select the policy and then click **Enable Policy** on the ribbon.

	![Enabling a Traffic Manager policy](Images/enabling-a-traffic-manager-policy.png?raw=true "Enabling a Traffic Manager policy")

	_Enabling a Traffic Manager policy_ 

1. Refresh the page and confirm that the application is once again available at the policy’s URL.

	> **Note:** It may take up to 2 minutes for the policy to propagate to all DNS servers.

---

<a name="Summary"></a>
## Summary ##

Windows Azure Traffic Manager enables you to manage and distribute incoming traffic to your Windows Azure cloud services whether they are deployed in the same data center or in different regions across the world.

In this hands-on lab, you explored available load balancing policies and learned how to use them to enhance performance, increase availability, and balance traffic to your cloud services.

---

<a name="AppendixA"></a>
## Appendix A: Configuring your Windows Azure Management Portal Credentials in Visual Studio ##

Follow these steps to save the credentials that allow you to deploy an application to your Windows Azure subscription from Visual Studio. The credentials are stored in your Visual Studio configuration so will only need to do this once for each subscription.

1. In **Solution Explorer**, right-click the Windows Azure project and select **Publish**.

	> **Note:** Ensure that you click the Windows Azure cloud project and not one of its associated roles.

1. It will prompt the **Publish Windows Azure Application** dialog. To publish an application, you first need to create the necessary credentials to access your Microsoft account. To add a new set of credentials to your configuration, expand the **Subscription** drop down list and select **Manage**. If you have already added your credentials, chose them and skip this task.

	![Adding Subscription’s credentials in Visual Studio](Images/adding-subscriptions-credentials-in-visual-st.png?raw=true "Adding Subscription’s credentials in Visual Studio")

	_Adding Subscription’s credentials in Visual Studio_

1. It will prompt a dialog to manage your Windows Azure authentication settings. Click **New** to define you authentication settings.

	![Adding Authentication Settings](Images/adding-authentication-settings.png?raw=true "Adding Authentication Settings")

	_Adding Authentication Settings_

1. To create the credentials, you require a certificate. If Visual Studio cannot find a suitable certificate in your personal certificate store, it will prompt you to create a new one; otherwise, in the **Windows Azure Project Management Authentication** dialog, expand the drop down list labeled **Create or select an existing certificate for authentication** and then select **Create**.

	![Creating a new certificate for authentication](Images/creating-a-new-certificate-for-authentication.png?raw=true "Creating a new certificate for authentication")

	_Creating a new certificate for authentication_

	> **Note:** The drop down list contains all the certificates that are suitable for authentication with the Azure Management API. This list includes the certificate you created earlier, during the PowerShell deployment exercise. Nevertheless, in this exercise, you create a new certificate to walk through the procedure required to generate certificates in Visual Studio.

1. In the **Create Certificate** dialog, enter a suitable name for the certificate, for example, _AzureMgmtVS_, and then click **OK**.

	![Creating a new management certificate](Images/creating-a-new-management-certificate.png?raw=true "Creating a new management certificate")

	_Creating a new management certificate_

1. Back in the **Windows Azure Project Management Authentication** dialog, ensure that the newly created certificate is selected. Notice that the issuer for this certificate is the Windows Azure Tools.

	![Selecting a certificate for the credentials](Images/selecting-a-certificate-for-the-credentials.png?raw=true "Selecting a certificate for the credentials")

	_Selecting a certificate for the credentials_

1. Now, click the link labeled **Copy the full path**. This copies the path of the certificate public key file to the clipboard.

	![Copying the path of the generated certificate public key file to the clipboard](Images/copying-the-path-of-the-generated-certificate.png?raw=true "Copying the path of the generated certificate public key file to the clipboard")

	_Copying the path of the generated certificate public key file to the clipboard_ 

	> **Note:** Visual Studio stores the public key file for the certificate it generates in a temporary folder inside your local data directory.

1. Click **OK** to dismiss the confirmation message box and then save the path in the clipboard to a safe location. You will need this value shortly, when you upload the certificate to the management portal.

	![Confirmation that the file path was copied to the clipboard successfully](Images/confirmation-that-the-file-path-was-copied-to.png?raw=true "Confirmation that the file path was copied to the clipboard successfully")

	_Confirmation that the file path was copied to the clipboard successfully_

1. Now, in the **Windows Azure Project Management Authentication** dialog, click the link labeled **Windows Azure Portal** to open a browser window and navigate to the Management Portal.

	![Opening the Developer Portal in your browser](Images/opening-the-developer-portal-in-your-browser.png?raw=true "Opening the Developer Portal in your browser")

	_Opening the Developer Portal in your browser_

1. At the Management Portal, sign in and then upload the certificate that you generated in Visual Studio using the same procedure described in **Task 2** of the previous exercise. Refer to that section of the document for detailed instructions.

	If you did not keep a record of the **Subscription ID** of your account, make a note of this value from the **Account** page now. You will require it for the next step.

	At the **API Certificates** page, when selecting a certificate file from your local storage, make sure to use the path that you copied earlier from your clipboard to specify the certificate public key file to upload. 

1. To complete the setup of your credentials, switch back to the **Windows Azure Project Management Authentication** dialog, enter your **subscription ID** and a name to identify the credentials and then click **OK**. 

	![Completing the credential setup procedure](Images/completing-the-credential-setup-procedure.png?raw=true "Completing the credential setup procedure")

	_Completing the credential setup procedure_

1. After you confirm the creation of the new credentials, Visual Studio uses them to access the management service to verify that the information that you provided is valid and notifies you if authentication fails. If this occurs, verify the information that you entered and then re-attempt the operation once again.

	![Authentication failure while accessing the management service](Images/authentication-failure-while-accessing-the-ma.png?raw=true "Authentication failure while accessing the management service")

	_Authentication failure while accessing the management service_ 

1. In the **Windows Azure Project Management Settings** dialog, you will see the recently created Authentication setting. Click **Close** to return to the Publish Wizard. You will continue with the publishing process in the next task.
 
	![Managing Authentication settings](Images/managing-authentication-settings.png?raw=true "Managing Authentication settings")

	_Managing Authentication settings_

1. Provide a name to identify your credentials, for example, _MyWindowsAzureAccount_, and then click **OK**.

<a name="AppendixB"></a>
## Appendix B: Configuring the DNS Cache of the Browser ##

The following steps describe how to configure the lifetime of DNS host entries in the browser’s cache using a registry script that you download from the home page of the lab’s application.

This procedure is optional. If you choose not to execute it, you can force the browser to clear its DNS cache by closing all open windows and restarting the browser before each request.

> **Important:** Running this script makes changes to your browser that can have an effect on your browsing experience. **You should only execute this script inside a remote desktop session to one of your cloud services.** 

> Nevertheless, if you choose to apply the script in your local machine, you can undo its changes by applying a complementary script that is also provided. Note that both scripts assume that you have not previously customized Internet Explorer’s DNS cache configuration. Otherwise, running these scripts could overwrite your original configuration.

> For more information, see [How Internet Explorer uses the cache for DNS host entries](http://support.microsoft.com/kb/263558).

> You can find both scripts in the **Assets** folder or download them from the home page of the application.

> The scripts are specific to Internet Explorer, so if you use a different browser, you will need to restart the browser before each request to clear its DNS cache.

#### How to configure the DNS Cache to decrease the lifetime of host entries to 30 seconds ####

1. In the home page of the application, download the registry script to shorten the lifetime of host entries in the browser’s DNS cache.

1. In the **File Download** dialog, click **Run** to launch the registry configuration script.

1. When prompted by the Registry Editor, click **Yes** to confirm that you wish to execute the script.

1. Click **OK** to close the message box confirming the successful update of the registry.

1. Close all open browser windows and restart the browser for the changes to take effect.

#### How to restore the DNS Cache configuration ####

1. In the home page of the application, download the registry script to restore the browser’s DNS cache configuration.

1. In the **File Download** dialog, click **Run** to launch the registry configuration script.

1. When prompted by the Registry Editor, click **Yes** to confirm that you wish to execute the script.

1. Click **OK** to close the message box confirming the successful update of the registry.

1. Close all open browser windows and restart the browser for the changes to take effect.