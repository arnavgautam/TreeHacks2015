<a name="Segment5" />
### Segment 5: Deploying and Managing Windows Azure apps ###

>**Note:** This segment is optional. Before executing these steps, make sure you have completed the procedure described in the **Setup and Configuration** section (see [Deploying a Cloud Service and Configuring New Relic (optional)](#setup9)) to properly set up and deploy your application.

In this segment, you will monitor and manage the Windows Azure Cloud Services that power the Windows 8 application. You will also show how to provision a partner service (New Relic) from the new Windows Azure Store.

1. Switch to the Windows Azure Management portal. By default, you will see the Media Services content page from the previous demos.

1. Open the previously deployed **BuildClips** cloud service.

	>**Speaking Point:** Let's go back to the BuildClips cloud service. Every cloud project in Azure has this nice dashboard view where we can see the overall health of the application. We can actually see individual machines, CPU, network and memory access, as well as the aggregate view of it. It displays separate information for both front-end and back-end roles.

	![Windows Azure Dashboard ](Images/windows-azure-dashboard.png?raw=true "Windows Azure Dashboard ")

	_Windows Azure dashboard_

1. Show that you have Production and Staging environments and how you can swap between them.

	>**Speaking Point:** We are having deployments in Production and in Staging. The Swap Deployment operation initiates a virtual IP swap between staging and production deployment environments for a service. If the service is currently running in the staging environment, it will be swapped to the production environment. If it is running in the production environment, it will be swapped to staging.

	![portal swap environments ](Images/portal-swap-environments.png?raw=true)

	_Swapping environments using the portal_

1. On the **Scale** page, show how you can scale up the number of web and worker role instances independently.

	>**Speaking Point:** The initial number or roles is determined by the **ServiceConfiguration** file that we uploaded. The **Instances** setting controls the number of roles that Windows Azure starts and is used to scale the service.

	![portal scale](Images/portal-scale.png?raw=true)

	_Scaling using the portal_

1. Go to the **MONITOR** page. Select **ADD METRICS** in the command bar to show how you can add a metric to the dashboard.

	>**Speaking Point:** In the monitor page, we can monitor key performance metrics for the cloud services in the portal, and customize what we monitor so we can meet our needs in order to check how our application is working.

	![portal add metrics](Images/portal-add-metrics.png?raw=true)

	_Adding new metrics to Windows Azure portal_

	![portal show metrics](Images/portal-show-metrics.png?raw=true)

	_Windows Azure portal metrics_

1. Navigate to the **Windows Azure Store** menu by clicking on **New** | **Store**. Scroll over all the available add-ons to show the multiple options that users have for extending their apps.

	>**Speaking Point:** The management portal makes it incredibly easy in the same way that we could create a new website, create a new virtual machine or mobile service, we can just go inside the portal and say new - store, and this will go ahead and bring up a list of services provided from Microsoft partners.

	>Here we can see services ranging from IP address checking, creating MySQL databases, MongoDB databases, or adding monitoring tools like New Relic. You can do it all directly from this portal. You can click on any of these and purchase them in your Windows Azure subscription without having to enter a new credit card or payment methods; you can use the existing payment mechanisms.

1. Select **New Relic** from the list.

	>**Speaking Point:** We will just try out the **New Relic** add-on as an example. It provides additional rich monitoring and outside-in monitoring support that we can take advantage of now easily within Windows Azure.

	![Adding New Relic Add-On](Images/adding-new-relic-add-on.png?raw=true "Adding New Relic Add-On")

	_Adding New Relic Add-On_

1. Click **Next** to show the **Personalize Add-on page**.

	>**Speaking Point:** They're actually offering a free edition of their standard package that's available to all Windows Azure customers, and we can run it, actually, on any number of servers.

	![Personalize-new-relic-add-on](Images/personalize-new-relic-add-on.png?raw=true "Personalize New Relic Add-On")

	_Personalize New Relic Add-On_

1. Do not complete the purchase. Close the window.

	>**Speaking Point:** I will now close this window since I have already configured my application to work with New Relic.

1. Navigate to the **Add-ons** section of the **Windows Azure Management portal** and select your previously created **New Relic** service. Show the dashboard options that the service offers.

	>**Speaking Point:** This is the service dashboard. We can see at a high level what's going on with the service. We can get the connection info, so if we need for example, the developer key in order to set up New Relic in our application and allow it to log data and send it to New Relic, we can do that. We can also upgrade it to the professional account later, if we want to.

	![New Relic Azure Dashboard](Images/new-relic-azure-dashboard.png?raw=true "New Relic Azure Dashboard")

	_New Relic Azure Dashboard_

1. Click **MANAGE** in the command bar to go to the **New Relic** dashboard and show it.

	> **Speaking Point:** By clicking this "manage" button, it will now do a single sign-on for us into the New Relic management portal for the service that they provide. Now, in a single view, I can actually drill in through the New Relic portal into my Windows Azure apps and services that are running in my cloud.

	![New Relic Dashboard](Images/newrelic-1-overview.png?raw=true "New Relic Dashboard")

	_New Relic dashboard_

1. Click on the **buildclips** application to drill into the service.

	>**Speaking Point:** We can see how our app server is doing from a health perspective. Once we drill down into an application, we can observe a quick overview of our application server including server processing time, our KPI scores as well as system throughputs.

	![Service Overview](Images/newrelic-n1.png?raw=true "Application Overview")

	_Service overview_

	>**Note:** Click on the **App Server** button to switch to the App Server view, if this is not the current view (New Relic remembers your last selection).

1. Click on the **Browser** button to go to the Browser view.

	>**Speaking Point:** They have some pretty cool features that allow us to see how browsers are doing, so we can actually see a browser view of responsiveness of the app in real time as well as how it's performing from various geographic locations around the country and around the world.

	>Two most important response times in performance tuning are shown in the upper-left corner - client perceived response time and server processing time. Client perceived response time is important because that's what our customers care about. Server processing time is useful when you try to fine-tune performances of specific server APIs.

	![Browser view](Images/newrelic-n2.png?raw=true "Browser view")

	_Browser view_

1. Click on the **Map** tab.

	>**Speaking Point:** This is a different system view that shows our dependencies on external services. If we recall the presentation slide just now, we can see this map matches with the architecture diagram pretty well.

	![Map view](Images/newrelic-3-map.png?raw=true "Map view")

	_Map view_
