<a name="Title" />
# Introduction to Windows Azure Access Control Service 2.0 #

---
<a name="Overview" />
## Overview ##

Connecting one application to its users is one of the most basic requirements of any solution, whether deployed on-premises, in the cloud or on both.
The emergence of standards is helping to break the silos that traditionally isolate accounts stored by different Websites and business entities. However, offering application access to users coming from multiple sources can still be a daunting task. As of today, if you want to open your application to users coming from Facebook, Live ID, Google and business directories, the brute-force approach demands you to lean and implement four different authentication protocols. Changes in today’s world happen fast and often, forcing you to keep updating your protocol implementations to chase the latest evolutions of the authentication mechanisms of different user repositories. All this can require a disproportionate amount of energy, leaving you with fewer resources to focus on your business.

![A functional view of the Access Control Service](Images/a-functional-view-of-the-access-control-servi.png?raw=true)

Windows Azure Access Control Service (ACS) offers you a way to outsource authentication and decouple your application from the complexity of maintaining direct relationships with all the identity providers you want to tap from. ACS takes care of engaging every identity provider with its own authentication protocol, normalizing the authentication results in a protocol supported by the .NET framework tooling (namely the Windows Identity Foundation technology, or WIF) regardless of where the user is coming from. WIF allows you to elect the ACS as the authentication manager for your application with just few clicks. From that moment on ACS takes care of everything, including providing a UI for the user to choose among all the recognized identity providers. 

Furthermore, ACS offers you greater control over which user attributes should be assigned for every authentication event.  Moreover, in synergy with WIF, those attributes (called claims) can be easily accessed for making authorization decisions without forcing the developer to understand or even be aware of the lower level mechanisms that the authentication protocols entail.

In this hands-on lab you will learn how to take advantage of the ACS for outsourcing authentication, managing multiple identity sources, performing basic authorization tasks, enhancing a classic ASP.NET Web application with advanced identity capabilities and taking control of the authentication experience.

<a name="Objectives" />
### Objectives ###

In this hands-on lab, you will learn how to:

* Configure your application to outsource authentication to ACS
* Configure ACS to include the identity providers you want to leverage
* Modify your application to consume claims from ACS and make authorization decisions
* Customize the default authentication user experience provided by ACS
* Modify an ASP.NET Web application to factor out authentication code in a local Security Token Service (STS)

<a name="Prerequisites" />
### Prerequisites ###

The following is required to complete this hands-on lab:

- Windows Azure subscription - [sign up for a free trial](http://aka.ms/WATK-FreeTrial)
- [Visual Studio 2012 Professional or Visual Studio 2012 Ultimate][1]
- [Identity and Access Tools for Visual Studio 2012][2]

[1]: http://www.microsoft.com/visualstudio
[2]: http://visualstudiogallery.msdn.microsoft.com/e21bf653-dfe1-4d81-b3d3-795cb104066e

<a name="Setup" />
### Setup ###

In order to execute the exercises in this hands-on lab you need to set up your environment.

1. Open a Windows Explorer window and browse to the lab's **Source** folder.

1. Execute the **Setup.cmd** file with Administrator privileges to launch the setup process that will configure your environment and install the Visual Studio code snippets for this lab.

1. If the User Account Control dialog is shown, confirm the action to proceed.

> Make sure you have checked all the dependencies for this lab before running the setup.

<a name="UsingCodeSnippets" />
### Using the Code Snippets ###

Throughout the lab document, you will be instructed to insert code blocks. For your convenience, most of that code is provided as Visual Studio Code Snippets, which you can use from within Visual Studio 2012 to avoid having to add it manually. 

---
<a name="Exercises" />
## Exercises ##

This hands-on lab includes the following exercises:

1.	[Exercise 1: Use Access Control Service to Accept Users from Multiple Identity Providers](#Exercise1)
1.	[Exercise 2: Take control of the Sign-In experience](#Exercise2)
1.	[Exercise 3: Enabling claims based access for an ASP.NET Web Application by generating a local STS](#Exercise3)

> **Note:** Each exercise is accompanied by a starting solution. These solutions are missing some code sections that are completed through each exercise and therefore will not necessarily work if running them directly.
Inside each exercise you will also find an end folder where you find the resulting solution you should obtain after completing the exercises. You can use this solution as a guide if you need additional help working through the exercises.

Estimated time to complete this lab: **45** minutes.

> **Note:** When you first start Visual Studio, you must select one of the predefined settings collections. Every predefined collection is designed to match a particular development style and determines window layouts, editor behavior, IntelliSense code snippets, and dialog box options. The procedures in this lab describe the actions necessary to accomplish a given task in Visual Studio when using the **General Development Settings** collection. If you choose a different settings collection for your development environment, there may be differences in these procedures that you need to take into account.

<a name="Exercise1" />
### Exercise 1: Use Access Control Service to Accept Users from Multiple Identity Providers ###

In this first exercise you will familiarize yourself with ACS’ basic settings and terminology. The goal is to secure access to a newly created ASP.NET Web site. The Web site will accept users from Google, Yahoo! and Windows Live ID. As you will see in a minute, ACS makes it really easy.

<a name="Ex1Task1" />
#### Task 1 - Signing-up for Windows Azure Access Control Service and Creating the Service Namespace ####

In this task you are going to subscribe to ACS.

1. Navigate to <https://manage.windowsazure.com/>. You will be prompted for your Windows Live ID credentials if you are not already signed in.

1. Now you will add a new **Access Control Service Namespace**. Click the **+New** button.

	![New Button](Images/new-button.png?raw=true)

	_New Button_

1. Expand **App Services | Access Control | Quick Create**. Type in a name for your **Namespace**, choose your **Region** and click the **Create** button. Make sure to validate the availability of the name first. Service names must be **globally unique** as they are in the cloud and accessible by whomever you decide to grant access to.

	> **Note:** An Access Control Service Namespace is the unique component of the addresses at which all your endpoints on the Access Control Service will be available.

	![Creating a New ACS Namespace](Images/creating-a-new-acs-namespace.png?raw=true)

	_Creating a New ACS Namespace_

1. Please be patient while your service is activated. An _Active_ status displays when the provisioning process finishes.

	![Active Service Namespace](Images/active-service-namespace.png?raw=true)

	_Active Service Namespace_

<a name="Ex1Task2" />
#### Task 2 - Getting Management Service credentials ####

In this task you will use the ACS portal to get the credentials that will be used in your application for authenticating users.

1.	With the **Namespace** selected, click the **Manage** button on the bottom toolbar. Make sure that your browser is allowed to show popups. 
	 
	![Manage Button](Images/manage-button.png?raw=true)

	_Manage Button_

2.	This launches (in another browser window or tab) the **Access Control Service Management Portal**, shown in the figure below. 
	 
	![Access Control Service Management Portal](Images/acs-manage-portal.png?raw=true)

	_Access Control Service Management Portal_
 
	> **Note:** The Management Portal, and specifically the left-hand navigation menu, offers you a global view of all the settings you can change in ACS. In the typical application development flow, such as the one you are creating in this exercise, there is a natural order you can follow to provide ACS with the information it needs to set up authentication for you.
	
3.	Click the **Identity Providers** link in the **Trust Relationships** section of the menu. The main area of the management portal will display a page which helps you to manage the identity providers from where your application users will come from.
	  
	![Identity Providers](Images/identity-providers.png?raw=true)

	_Identity Providers_

4.	Click the **Add** link above the Identity Providers table, choose the identity providers you want to add in your Access Control Service namespace and click **Next**. Windows Live ID is there by default. For this exercise please pick Google and Yahoo! as well.  
	 
	![Adding an Identity Provider](Images/add-identity-providers.png?raw=true)

	_Adding an Identity Provider_
 
5.	Leave the default settings and click **Save.**

	![The Yahoo Identity Provider configuration page](Images/yahoo-identity-provider-config.png?raw=true)
	 
	_The Yahoo Identity Provider configuration page_
 
6.	You can follow the same steps to add Google as an Identity Provider. You’ll end up with the Identity Providers list shown below.
	 
	![Identity Providers configured](Images/identity-providers-configured.png?raw=true)
	
	_Identity Providers configured_
 
7.	Click the **Management service** link in the **Administration** section of the menu. The main area of the management portal will display a page which helps you to add or edit accounts for accessing the management service for your Access Control Service namespace.
	 
	![Management Service](Images/management-service.png?raw=true)

	_Management Service_
 
8.	Select the **ManagementClient** account.

9.	From the **Credentials** table, select the **Symmetric Key** credential type.

10.	Click **Show Key** and take note of the 256-bit symmetric key.

	![Symmetric Key](Images/symmetric-key.png?raw=true)
	
	_Symmetric Key_ 

<a name="Ex1Task3" />
#### Task 3 - Configuring the allowed Identity Providers  ####

By now you know that ACS takes care of accepting tokens in different formats from a number of possible identity providers via different protocols, and normalizes the incoming information into a different token format that your Web site will consume. ACS can emit different token types via various protocols. For Websites, the default protocol is WS-Federation. There’s no need to go in the fine details: it’s suffice to say that WS-Federation is a protocol that is natively understood by Windows Identity Foundation (WIF), a part of the .NET framework that allows you to easily outsource application authentication to tokens sources such as the ACS itself. In particular, WIF extends Visual Studio with a tool that can automatically configure your application to outsource authentication without requiring you to write a single line of code.

1. Open **Microsoft Visual Studio 2012** as administrator by right-clicking the **Microsoft Visual Studio 2012** shortcut and choosing **Run as administrator**.

2. From the **File** menu, choose **New | Project**.  

3. In the **New Project dialog**, expand **Visual C#** in the **Installed** list and select **Web**. Choose the **ASP.NET MVC 4 Web Application** template, set the **Name** of the project to _WebSiteACS_ and set a location for the solution. Click **OK**.

	![Creating a new MVC 4 Application in Visual Studio 2012](Images/new-mvc4-project.png?raw=true)

	_Creating a new MVC 4 Application in Visual Studio 2012_

1. In the **New ASP.NET MVC 4 Project** window, select **Intranet Application**, make sure the view engine is set to **Razor** and then click **OK**.

	![Selecting MVC Intranet Application](Images/selecting-mvc-intranet-application.png?raw=true)

	_Selecting MVC 4 Intranet Application template_

4.	Select your project in **Solution Explorer**, and in the **Properties** pane, switch **SSL Enabled** to _True_. Copy the **SSL URL**.

	![Switching Web App to use SSL](Images/switching-web-app-to-use-ssl.png?raw=true)

	_Switching Web App to use SSL_

	> **Note:** Visual Studio configures your application to serve content through HTTP. However, that would not be suitable for establishing secure sessions, given that it would leave communications unprotected and allow potential attackers to steal cookies and tokens. This is not mandatory during the development phase, as Windows Azure will not strictly enforce use of HTTPS. However, it is always a good practice.

5.	Right-click the project and choose **Properties**. Choose the **Web** tab on the left, scroll down to the **Use Local IIS Web server** option and paste the HTTPS URL in the **Project Url** field. Save settings (CTRL+S) and close the property tab.

	![setting local iis web server url](Images/setting-local-iis-web-server-url.png?raw=true)

	_Setting the Local IIS Web Server URL_

6.	In **Solution Explorer**, right-click the **WebSiteACS** project and select **Identity and Access**.
	 
	![Identity and Access Menu Option](Images/identity-and-access-option.png?raw=true)
	
	_Identity and Access Menu Option_
 
7.	In the **Identity and Access** window, go to **Providers** tab.

8.	Check the **Use the Windows Azure Access Control Service** option.

9.	Click the **Configure** link to configure your ACS namespace.
	 
	![Identity and Access configure option](Images/identity-and-access-configure.png?raw=true)

	_Identity and Access configure option_
 
10.	Enter the ACS namespace and the management key taken from previous task (step 10) and click **OK**.
	 
	![Configuring the ACS namespace](Images/configuring-acs-namespace.png?raw=true)

	_Configuring the ACS namespace_
 
11.	Choose the identity providers you want to add in your Access Control Service namespace. For this exercise please pick Windows Live ID, Google and Yahoo! as well.
	 
	![Configure ACS namespace - Selecting Identity Providers](Images/configuring-acs-providers.png?raw=true)

	_Configure ACS namespace - Selecting Identity Providers_
 
12.	Make sure that the Realm and the Return Url are the following:

	**Realm:** https://localhost:{port-number}/

	**Return Url:** https://localhost:{port-number}/

13.	Click **OK** to finish the configuration.
	
	![An example of how ACS receives a token as proof of authentication (in this case from Google, but it can come from any recognized IP) and can emit for the application a token containing the original claims](Images/acs-authentication-sample.png?raw=true)

	_An example of how ACS receives a token as proof of authentication (in this case from Google, but it can come from any recognized IP) and can emit for the application a token containing the original claims_

	> **Note:** When a user successfully authenticates with one identity provider, ACS receives from that identity provider an artifact (called “security token”) which represents the proof that authentication took place. You don’t need to know the details of how a security token looks like. The only information relevant at the moment is that the token contains information about the authenticated user, the so-called claims, which help ACS to establish who the current user is.
	ACS can process the incoming claims in various ways you will see a simple example of that in the next exercise. In the current exercise we will configure ACS to enforce its default behavior, which is to make sure that your application receives the claims as they came from the original identity providers. How does it do that? The ACS itself emits a token that your application is configured to consume (as you will see in just few steps). All it needs to do is to copy the incoming claims as is into the outgoing token.

<a name="Ex1Task4" />
### Task 4 - Verification ###
 
It’s time to give your newly secured Web site a spin. In order to verify that you have correctly performed all steps in exercise one, proceed as follows:

1.	Start debugging by pressing **F5**. 

1. A security certificate warning will appear in your browser. This is expected behavior, click **Continue to this website (not recommended)**.

	![Browser Displaying Security Certificate Warning](Images/browser-displaying-security-certificate-warni.png?raw=true)

	_Browser Displaying Security Certificate Warning_

	The relying party application (https://localhost:{port-number}/) will redirect to the Access Control Service to authenticate. 

1. Choose an Identity Provider. You will be redirected to the identity provider’s Web site where you will be prompted to enter your credentials. In this example we will use Windows Live ID choosing other options will lead to comparable experiences.
	 
	![Choosing your Favorite Identity Provider](Images/choosing-your-identity-provider.png?raw=true)

	_Choosing your Favorite Identity Provider_
 
3.	In the Windows Live ID Web site enter your Live ID and password.
	 
	![Login with Windows Live ID](Images/login-with-windows-live-id.png?raw=true)

	_Login with Windows Live ID_
 
4.	Upon successful authentication you will be redirected to ACS (observe the address bar) that will briefly process and redirect back to your Web site.

5.	The process is entirely transparent to the user. But the Web site is now configured to verify that if a valid token from the ACS is present in the call that is the case, hence you are granted access and the default page finally appears in the browser.
	 
	![User Authenticated](Images/user-authenticated.png?raw=true)

	_User Authenticated_
 
6.	Close the browser.

<a name="Exercise2" />
### Exercise 2: Take control of the Sign-In experience ###

Using the WIF tooling to outsource authentication to ACS creates what is often referred to as “blanket protection”.  From that moment on, any attempts from unauthenticated users to access any pages in the Web site triggers a redirection to the ACS. There are various scenarios where that is not the desired effect.  For example, you may want to leave some pages available to unauthenticated users, or you may want to take finer control of the authentication experience.

This exercise shows you how you can seamlessly integrate ACS authentication elements in your Web site. It demonstrates that you can take advantage of ACS’ advanced capabilities without having to break the experience and style you want for your web applications.

<a name="Ex2Task1" />
#### Task 1 - Generating a Controller to Handle Logins ####

In this task you will generate a new controller to handle logins for unauthenticated user by using the Identity and Access tool.

1. If not already open, start **Visual Studio 2012 Professional or Ultimate** and continue with the solution obtained from the previous exercise. Alternatively, you can open the **Begin.sln** solution from the **Source\Ex2-CustomSignInExperience\Begin** folder of this lab.

	> **Note:** If you opened the **Begin.sln** solution, you need to **enable SSL** from the properties of the WebSiteACS project; update the **project URL** in the **Web** tab of the WebSiteACS project properties in the _Use Local IIS Web server_ section with the SSL URL obtained from the previous step; and run steps 6 to 13 from [Task 3 of Exercise 1](#Ex1Task3).

1. Right-click the **WebSiteACS** project and click **Identity and Access**.

1. In the Identity and Access window, select the **Configuration** tab.

1. From the _Choose how to handle unauthenticated requests_ section, select the **Generate a controller in your project to handle the authentication experience at the following address** radio button. Leave the remaining values as they are and click **OK**.

	> **Note:** If a warning displays next to the "Use the Windows Azure Access Control Service" radio button after clicking OK, click the Configure link in the Providers tab and insert your ACS namespace and simmetric key. Then try again.

	![Generating a Controller to Handle Logins](Images/generating-a-controller-to-handle-logins.png?raw=true)

	_Generating a Controller to Handle Logins_

1. Let's take a look at the new files added to support the new login process. First, open **HrdAuthenticationController.cs** from the **Controllers** folder. This class has only a URL to ACS with a callback of a **ShowSigninPage** function that is inside the HrdAuthentication.js script. This is passed to the **HrdAuthentication\Login.cshtml** view.

	````C#
	public class HrdAuthenticationController : Controller
	{
		[AllowAnonymous]
		public ActionResult Login()
		{
			ViewBag.MetaDataScript = "https://[YOUR-ACS-NAMESPACE].accesscontrol.windows.net/v2/metadata/identityProviders.js?protocol=wsfederation&realm=[YOUR-SSL-URL]&version=1.0&callback=ShowSigninPage";
			return View("~/Views/HrdAuthentication/Login.cshtml");
		}
	}
	````

1. Open **Login.cshtml** from the **Views\HrdAuthentication** folder. It will render the list of providers taken from the URL passed from the HrdAuthenticationController in the ViewBag using the **HrdAuthentication.js** script.

	````CSHTML
	<hgroup class="title">
		 <h1>Log in</h1>
		 <h2> using one of the identity providers below</h2>
	</hgroup>

	<div id="IPDiv"/>

	@section Scripts {
		 @Scripts.Render("~/scripts/HrdAuthentication.js");

		 <script src="@ViewBag.MetaDataScript" type="text/javascript"></script>
	}
	````

1. Open **HrdAuthentication.js** from the **Scripts** folder. Notice that for each provider ip, it will create a link to the corresponding login URL.

	````JavaScript
	function ShowSigninPage(IPs) {
		 $.each(IPs, function (i, ip) {
			  $("#IPDiv").append('<a href="' + ip.LoginUrl + '">' + ip.Name + '</a><br/>');
		 });
	};
	````

1. Lastly, you will update the HomeController to require authentication when browsing the home page of the application. This will let the HrdAuthenticationController added previously to handle the rendering of the list of providers for the user to log in. To do this, open **HomeController.cs** from the **Controllers** folder. Add an **[Authorize]** annotation to the HomeController class as shown in the following code.

	<!-- mark: 3 -->
	````C#
	namespace WebSiteACS.Controllers
	{
		[Authorize]
		public class HomeController : Controller
		{
			...
		}
	}
	````

<a name="Ex2Task2" />
#### Task 2 - Verification ####

In this task you will experience the new look and feel of the login process to your application for unauthenticated users.

1. Start debugging by pressing **F5**.

1. A security certificate warning will appear in your browser. This is expected behavior, click **Continue to this website (not recommended)**.

	![Browser Displaying Security Certificate Warning](Images/browser-displaying-security-certificate-warni.png?raw=true)

	_Browser Displaying Security Certificate Warning_

1. The relying party application will redirect to the Access Control Service to authenticate. Choose your favorite Identity Provider (e.g.: _Windows Live ID_) from your custom login page and enter your credentials. 

	![Custom Login Page](Images/custom-login-page.png?raw=true)

	_Custom Login Page_

1. Once you inserted your credentials, Access Control will send the expected claims to the application. You are now authenticated.

	![User Authenticated](Images/user-authenticated.png?raw=true)

	_User Authenticated_

1. Close the browser.


<a name="Exercise3" />
### Exercise 3: Enabling claims based access for an ASP.NET Web Application by generating a local STS ###

Handling access control for a Web application is mainly a matter of handling three tasks:

* Authenticate the incoming user’s credentials

* Retrieve the attributes of the authenticated user and use them for granting or deny access

* Use the same attributes for customizing the user experience

In traditional development practice, you would implement those tasks by coding directly against the credentials that your application uses: that would require you to be skilled in security matters and would generate code that is difficult to maintain or re-host.

Windows Identity Foundation changes the game. As a first step, we _externalize authentication_. Instead of authenticating the users from our application, we trust an external authority to do so: we call such an authority _Security Token Service_, or _STS_. We configure our application to accept _security tokens_ from the STS, transmitted using standard protocols. Those tokens are the proof that the user successfully authenticated with the STS, which is all we need for considering the user authenticated with our application: hence we don’t need to worry about managing credentials anymore. For this reason, it is common in the security jargon to call an application that accepts tokens from an STS _relying party_ (or _RP_).

The security tokens can also contain information about users: we call those information _claims_. A claim can literally be anything: groups or roles the user belongs to, attributes like name or email, or permissions such as CanRead. The STS embeds in the token claims about the user at issuance time: the tokens are digitally signed, hence after issuance their values cannot be tampered with. If our application trusts the STS, it consider the claims in the tokens it produce as actually describing the user; as a result, our application no longer needs to look up user attributes for authorization or customization purposes. 

If this all sounds a bit confusing, don’t worry: it will all become clear as we walk through the lab.
In this first exercise, we will start from an existing ASP.NET application, which contains only some UI elements which need to be initialized with identity information about the current user. We will show how to secure the application and obtaining the identity info we need by taking advantage of a local STS.

![Redirects Sequence And Claims Flow](Images/redirects-sequence-and-claims-flow.png?raw=true)

In this exercise you are provided with a simple MVC 4 solution with a Secret Page that will be browsed by the user according to the claims issued by a Local STS.

<a name="Ex3Task1" />
#### Task 1 - Exploring the Initial Solution ####

In this task you will explore the provided begin solution.

1. Open **Microsoft Visual Studio 2012 Professional or Ultimate** with administrator privileges.

1. From the **File** menu, choose **Open | Project/Solution**. Browse to the **Source\Ex3-ClaimsEnableASPNET\Begin** folder of the lab and open the **Begin.sln** solution.

1. Press **Ctrl + F5** to run the app.

	![Home Page](Images/home-page.png?raw=true)

	_Home Page_

	> **Note:** The begin solution consists of a simple Home page with two labels; one for the user name and another to display the remaining days for the user birthday. A "_Go to the secret page_" link navigates to a second page which is supposed to be shown only if the user is in Manager role. There is no authentication code or configuration in place. 
	> This is as simple as it gets: the purpose here is showing you how to use the object model of Windows Identity Foundation. You easily transport what you will learn in this lab to your own realistic scenarios.

1. Click the **Go to the secret page** link.

	![Secret Page](Images/secret-page.png?raw=true)

	_Secret Page_

	> **Note:** In the following tasks you will modify the Web site to federate authentication and implement the remaining functionality to show the user name and days to birthday and authorizing the access to the secret page.

1. Close the browser.

<a name="Ex3Task2" />
#### Task 2 - Adding a Local STS to the Solution ####

The application has no authentication code at all. As mentioned before, you will outsource authentication to an STS you trust. In production you will have such an STS: but for now, there are none available. Fortunately, the Identity and Access tools allow us to easily configure a local STS that can be used for development purposes.

1. On the **Solution Explorer**, right-click the **WebSiteACS** project and select **Identity and Access**.

	![Identity and Access Menu Option](Images/identity-and-access-option.png?raw=true)
	
	_Identity and Access Menu Option_

1. From the **Identity and Access** window, go to the **Providers** tab.

	> **Note:** This tool helps to establish a trust relationship between a relying party application (the **WebSiteACS** application, in this case) and an STS. In this exercise, you will use the tool to configure the Local Development STS for development purposes.

1. Check the **“Use the Local Development STS to test your application”** option and click **OK**.

	> **Note:** The Local Development STS is a test endpoint, provided by the Identity and Access tools, which can be used on the local machine for getting claims of arbitrary types and values in your application. By choosing “Use the local development STS to test your application” you told the tools that you want your application to get tokens from the local STS, and the tools take care to configure your app accordingly. When you press F5, the tools launch an instance of Local STS and your application redirects the request to it. Local Development STS does not attempt to authenticate requests; it just issues a token with the claim types and values it is configured to issue.

	![Local STS Provider](Images/local-sts-provider.png?raw=true)

	_Local STS Provider_

1. Run the Web site (**Ctrl + F5**).

	> **Note:** This redirect happened because the **Identity and Access** tool configured our application to expect tokens from the Local Development STS. Hence from now on every unauthenticated user will be redirected to the STS, and only the ones that will successfully authenticate here will be able to access the application pages. Everything according to the plan.

	![Home Page](Images/home-page.png?raw=true)

	_Home Page_

	You took care of the authentication part, but now you have to think about how to acquire the claims you need for your application: Name, Birthdate and Role. Since you are using a local STS, you will have to modify the STS configuration in order to generate the appropriate claims.

1. Close the browser.

<a name="Ex3Task3" />
#### Task 3 - Managing Custom Claims in your Local STS ####

In this task you will manage roles in the web application by using custom claims in the Local STS.

1. On the **Solution Explorer**, right-click the **WebSiteACS** project and select **Identity and Access**.

1. Go to the **Local Development STS** tab and add a new claim required by the application. To do this, go to the claims grid and add a new one with the following values:

	**Type:** http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth

	**Display Name:** DateOfBirth

	**Value:** 5/5/1955
	
	![Adding DateOfBirth Claim in the List of Test Claims to Issue](Images/adding-dateofbirth-claim-in-the-list-of-test.png?raw=true)

	_Adding DateOfBirth Claim in the List of Test Claims to Issue_

1. Go to the **Role** claim and replace the _developer_ value with _Manager_. Click **OK** when you are finished.

	> **Note:** You won’t modify FederationMetadata.xml for this exercise. However, this is important for “advertising” the claims that your application needs. This would be useful if you would allow an external STS, such as an Active Directory Federation Services (ADFS) instance, to automatically configure itself for providing claims to your application.

	![Updating Role Claim Value From the List of Test Claims to Issue](Images/updating-role-claim-value-from-the-list-of-te.png?raw=true)

	_Updating Role Claim Value From the List of Test Claims to Issue_

	Optionally, you can modify the same settings from **LocalSTS.exe.config** file directly.

	> **Note:** All the settings for the Development Local STS come from this file, which lives in your application’s folder. That means that you can create multiple copies of those files with different settings for your various test cases.

1. Open **HomeController.cs** from the **Controllers** folder and add the following using statement.

	````C#
	using System.Security.Claims;
	````

	> **Note:** Here you will use the Windows Identity Foundation object model for retrieving claims values and driving the applications’ behavior. Sometimes, you will not even notice you are using Windows Identity Foundation, since its object model is seamlessly integrated with the existing ASP.NET model.

1. Find the **Index** action method and add the following highlighted line of code to pass the user name to the home view.

	<!-- mark:3 -->
	````C#
	public ActionResult Index()
	{
		ViewBag.UserName = User.Identity.Name;

		return View();
	}
	````

	> **Note:** This is exactly the code you write for accessing the user’s name in an ASP.NET application, regardless of the authentication type: Windows Identity Foundation is no exception.

1. Add the following highlighted code to the **Index** action method to get the birthday claim value and pass the remaining days until the user's birthday to the home view.

	(Code Snippet - _Introduction to ACS 2 - Ex3 Days To Birthday_)

	<!-- mark:5-8 -->
	````C#
	public ActionResult Index()
	{
		ViewBag.UserName = User.Identity.Name;

		DateTime birthDay = DateTime.Parse(ClaimsPrincipal.Current.FindFirst(ClaimTypes.DateOfBirth).Value);

		int days = (birthDay.DayOfYear - DateTime.Today.DayOfYear);
		ViewBag.DaysToBirthDay = ((days < 0) ? days + 365 + (DateTime.IsLeapYear(DateTime.Today.Year) ? 1 : 0) : days).ToString();
		
		return View();
	}
	````

1. Save your changes.

	> **Note:** What you see here is the claim identity model in action. First, we can do everything with ClaimsPrincipal (no need to get to the underlying ClaimsIdentity). Once we have that, we have a means of inspecting the claims we received from the STS: the next line queries the incoming claims collection, searching for the birth date of the user. The rest of the code just calculates how many days are left in the year before the user’s birthday.
Nothing of the code above gives away any details about which authentication technology has been used: the Windows Identity Foundation took care of all the details for us, and we are free to concentrate on our application logic.

1. Open the **Web.config** file of the relying party Web site (**WebSiteACS** project).

1. Insert the following block under the **configuration** element section to authorize access to **Home/SecretPage** page for the **Manager** role only.

	(Code Snippet - _Introduction to ACS 2 - Ex3 SecretPage Authorization Settings_)

	<!-- mark:4-11 -->
	````XML
	<configuration>
		...
		</appSettings>
		<location path="Home/SecretPage">
			<system.web>
				<authorization>
					<allow roles="Manager" />
					<deny users="*" />
				</authorization>
			</system.web>
		</location>
		<location path="FederationMetadata">
		...
	</configuration>
	````

	> 	**Note:** Again, this is exactly what you would do using ASP.NET roles. This means that if you already have code which uses those features you can move to a claims based model without having to rewrite anything.

1. Save your changes.

<a name="Ex3Task4" />
#### Task 4 - Verifying the Role Based Access ####

In this task you will verify that you can only access the secret page only with a manager role.

1. Start the application by pressing **Ctrl + F5**. The relying party application will redirect to the STS to authenticate.

	![Home Page](Images/home-page-2.png?raw=true)

	_Home Page_

	The STS sent to our application the claims it was expecting and the code we added takes advantage of them.

1.	Click the **Go to the secret page** link.

	![Secret Page](Images/secret-page-3.png?raw=true)

	_Secret Page_

	Since the STS includes a hardcoded claim for the **Manager** role you can navigate to the **Home/SecretPage** page which is limited to Manager users only. Let’s put this to test and see what happens if we change Terry’s role from **Manager** to **Sales**: this should make Terry unable to reach **Home/SecretPage**.

1. Close the browser.

1. On the **Solution Explorer**, right-click the **WebSiteACS** project and select **Identity and Access**.

1. Go to the **Local Development STS** tab. In the **Role** claim, replace the _Manager_ value with _Sales_. Click **OK** when you are finished.

	![Updating Role Claim Value From the List of Test Claims to Issue](Images/updating-role-claim-value.png?raw=true)

	_Updating Role Claim Value From the List of Test Claims to Issue_

1. Start the application by pressing **Ctrl+F5** and then click the **Go to the secret page** link. You should get an unauthorized error message since the STS did not issue a role claim with the **Manager** role.

	![Access Denied for Secret Page](Images/access-denied-for-secret-page.png?raw=true)

	_Access Denied for Secret Page_

<a name="Ex3Task5" />
#### Task 5 - Granting or Denying Access to the Website According to the User’s Age ####

In this task, you are going to delete the ASP.NET Role settings and add a claims authorization manager to change the permissions criteria. Also you will add a simple custom **ClaimsAuthorizationManager** class whose only capability is making sure that only callers older than a certain threshold age are granted access to a given web resource.

> **Note:** The fact that Windows Identity Foundation integrates well with existing authentication practices is a big advantage, since it allows you to protect existing skills and investments: however claims have much more expressive power than the classic roles and attributes, and can be used for achieving things that would have been impossible with traditional approaches. One simple example is using claims for expressing attributes that have non-string values, such as dates (date of birth, expiration date, etc), numbers (spending limit, weight, etc) or even structured data. Those data can be processed using criteria that are more sophisticated than the simple existence check (i.e. the user has claims A with value X, or he doesn’t): one example of such a criteria would be imposing that all the users of the website should be older than 21.
	
> The Windows Identity Foundation object model offers an extensibility point in the claims processing pipeline, which is specifically designed to allow you to inject your own claims authorization code. In order to do that, you are required to derive from the **ClaimsAuthorizationManager** class and implement your claim authorization logic in the method **CheckAccess**. It is common practice to express the authorization conditions outside of the code, for example in configuration files, and implement in **CheckAccess** generic evaluation logic so that the values & conditions can be changed at deployment time. This ensures that a change in the authorization conditions can be applied without recompiling anything. The conditions can be associated to the resources they refer to using some notation convention directly in the configuration files: however it is also possible to use explicit invocations from code, or decorating the resources themselves with attributes which will tie to authorization conditions and will fire the associated logic.

1. Right-click the **WebSiteACS** project and select **Add Reference**.

1. Select the **System.IdentityModel** assembly and click **OK**.

	![System Identity Model Reference](Images/system-identity-model-reference.png?raw=true)

	_System Identity Model Reference_

1. Right-click the **WebSiteACS** project and select **Add | Class**. Name it **AgeThresholdClaimsAuthorizationManager.cs**.

1. Replace the content of the new class with the following code to check access based on the DateOfBirth claim from the issued token.

	(Code Snippet - _Introduction to ACS 2 - Ex3 AgeThresholdClaimsAuthorizationManager_)

	````C#
	namespace WebSiteACS
	{
		 using System;
		 using System.Collections.Generic;
		 using System.IO;
		 using System.Security.Claims;
		 using System.Xml;

		 public class AgeThresholdClaimsAuthorizationManager : ClaimsAuthorizationManager
		 {
			  private static Dictionary<string, int> _policies = new Dictionary<string, int>();

			  public override void LoadCustomConfiguration(XmlNodeList nodelist)
			  {
					foreach (XmlNode node in nodelist)
					{
						 XmlTextReader rdr = new XmlTextReader(new StringReader(node.OuterXml));
						 rdr.MoveToContent();

						 string resource = rdr.GetAttribute("resource");

						 rdr.Read();

						 string claimType = rdr.GetAttribute("claimType");

						 if (claimType.CompareTo(ClaimTypes.DateOfBirth) != 0)
							  throw new NotSupportedException("Only birthdate claims are supported");

						 string minAge = rdr.GetAttribute("minAge");

						 _policies[resource] = int.Parse(minAge);
					}
			  }

			  public override bool CheckAccess(AuthorizationContext context)
			  {
					Uri webPage = new Uri(context.Resource[0].Value);

					if (_policies.ContainsKey(webPage.PathAndQuery))
					{
						 int minAge = _policies[webPage.PathAndQuery];
						 string userBirthdate = context.Principal.FindFirst(ClaimTypes.DateOfBirth).Value;

						 int userAge = DateTime.Now.Subtract(DateTime.Parse(userBirthdate)).Days / 365;

						 if (userAge < minAge)
						 {
							  return false;
						 }
					}

					return true;
			  }
		 }
	}
	````

	The class constructor takes care of reading the authorization conditions from the Web.config. The **CheckAccess** method retrieves the dateofbirth claim associated with the incoming user, and simply verifies that the corresponding age is above the threshold established by minAge.

1. Open the **Web.config** file and delete the SecretPage authorization settings.

	<!-- strikethrough:4-11 -->
	````XML
	<configuration>
		...
		</appSettings>
		<location path="Home/SecretPage">
			<system.web>
				<authorization>
					<allow roles="Manager" />
					<deny users="*" />
				</authorization>
			</system.web>
		</location>
		<location path="FederationMetadata">
		...
	</configuration>
	````

1. Add the **ClaimsAuthorizationModule** in the system.webServer modules configuration.

	(Code Snippet - _Introduction to ACS 2 - Ex3 ClaimsAuthorizationModule module_)

	<!-- mark:5 -->
	````XML
	<system.webServer>
		...
		<modules>
			...
			<add name="ClaimsAuthorizationModule" type="System.IdentityModel.Services.ClaimsAuthorizationModule, System.IdentityModel.Services, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" preCondition="managedHandler" />
		</modules>
		...
	</system.webServer>	
	````

1. Insert the **ClaimsAuthorizationManager** configuration to set the access policy for the **SecretPage**.

	(Code Snippet - _Introduction to ACS 2 - Ex3 ClaimsAuthorizationManager configuration_)

	<!-- mark:4-8 -->
	````XML
	<system.identityModel>
		<identityConfiguration>
			...
			<claimsAuthorizationManager type="WebSiteACS.AgeThresholdClaimsAuthorizationManager, WebSiteACS">
				<policy resource="/Home/SecretPage" action="GET">
					<claim claimType="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth" minAge="21" />
				</policy>
			</claimsAuthorizationManager>
		</identityConfiguration>
	</system.identityModel>
	````

1. Change the **Secret Page** message. To do this open the **SecretPage.cshtml** file from the **Views | Home** folder of the WebSiteACS project and replace the page text with the following one.
	
	<!-- mark:5 -->
	````CSHTML
	@{
		ViewBag.Title = "SecretPage";
	}

	<h2>Welcome to the Secret Page! You can get here only if you are 21 years old or older.</h2>
	````

<a name="Ex3Task6" />
#### Task 6 - Verifying the User’s Age Based Access ####

In this task you will verify that you can only access the secret page only if the user is 21 years old.

1. Start the application by pressing **Ctrl + F5**. The relying party application will redirect to the STS to authenticate.

	![Home Page](Images/home-page-2.png?raw=true)

	_Home Page_

	The STS sent to our application the claims it was expecting and the code we added takes advantage of them.

1.	Click the **Go to the secret page** link.

	![Secret Page](Images/secret-page-2.png?raw=true)

	_Secret Page_

	Since the STS includes a hardcoded claim for the **Birthdate** you can navigate to the **Home/SecretPage** page which is limited to users with an age greater or equal than 21 only. Let’s put this to test and see what happens if we change Terry’s **Birthdate** to **5/5/2012**: this should make Terry unable to reach **Home/SecretPage**

1.	Close the browser.

1. On the **Solution Explorer**, right-click the **WebSiteACS** project and select **Identity and Access**.

1. Go to the **Local Development STS** tab and replace **DateOfBirth** claim value with _5/5/2012_.

	![Updating DateOfBirth Claim Value From the List of Test Claims to Issue](Images/updating-dateofbirth-claim.png?raw=true)

	_Updating DateOfBirth Claim Value From the List of Test Claims to Issue_

1.	Start the application by pressing **Ctrl + F5** and click the **Go to the secret page** link. You should get an unauthorized error message since the STS issued a claim with your **Birthdate** that the ClaimsAuthorizationManager rejected.

	![Access Denied for Secret Page](Images/access-denied-for-secret-page-2.png?raw=true)

	_Access Denied for Secret Page_

---

<a name="summary" />
## Summary ##

By completing this hands-on lab you have learned how to:

* Configure your application to outsource authentication to ACS
* Configure ACS to include the identity providers you want to leverage
* Modify your application to consume claims from ACS and make authorization decisions
* Customize the default authentication user experience provided by ACS
* Modify an ASP.NET Web application to factor out authentication code in a local Security Token Service (STS)
