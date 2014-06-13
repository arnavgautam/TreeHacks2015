# Building Real World Cloud Apps #

## Getting Started ##

### Prerequisites ###

In order to execute this demo you need to set up your environment.

- Windows 8.1

- Visual Studio 2013

- Microsoft Azure PowerShell

- Microsoft Azure Active Directory

- New Relic account

### Setup & Configuration ###

#### Creating Environment ####

1. Open Internet Explorer, go to https://windows.azure.com/download/publishprofile.aspx and sign in using your Microsoft Account credentials. Save the publish settings file, you will use it in the following steps.

1. Open **Microsoft Azure Powershell** as Administrator.

1. Enable Powershell to run all scripts by executing the following command:

	```` PowerShell
	Set-ExecutionPolicy -ExecutionPolicy ByPass
	````

1. Replace the placeholder with your publish-setting file's path and execute the script.

	```` PowerShell
	Import-AzurePublishSettingsFile '[YOUR-PUBLISH-SETTINGS-PATH]'
	```` 

1. Change directory to **.\Source\MyFixIt\Automation\**.

1. Execute the following script to create **MyFixIt** environment on Azure.

	````PowerShell
	.\create-azure-website-env.ps1 -Name "[WEB-SITE-NAME]" -Location "[LOCATION]" -SqlDatabasePassword "[SQL-PASSOWORD]"
	````

	>**Note**: These are the input parameters for **create-azure-website-env.ps1** script

	> - Name: [Required] WebSite name. It needs to be alphanumeric
	> - Location: [Optional] By default to "West US", needs to be a location which all the services created here are available
	> - SqlDatabaseUserName: [Optional] default to "dbuser"
	> - SqlDatabasePassword: [Required] SQL username password	
	> - StartIPAddress: [Optional] Start IP address of the range you want to whitelist in SQL Azure firewall will try to detect if not specified
	> - EndIPAddress: [Optional] End IP address of the range you want to whitelist in SQL Azure firewall will try to detect if not specified


1. Change directory to **.\Source\AutomatedHelloWorld\Automation\**.

1. Execute the following script to create **AutomatedHelloWorld** solution environment.

	````PowerShell
	.\create-azure-website-env.ps1 -Name "[WEB-SITE-NAME]" -Location "[LOCATION]" -SqlDatabasePassword "[SQL-PASSOWORD]"
	````

	>**Note**: AutomatedHelloWorld is deployed during the demo.

#### Configuring Storage Account ####

1. Open **Web.config** on **.\Source\MyFixIt** and locate the **appSettings** section. Update the **StorageConnectionString** attribute with the storage account created with the pwoershell script.


#### Configuring Microsoft Azure Active Directory Authentication ####

1. Make sure you create an application in your **Microsoft Azure Active Directory** to allow authentication for **MyFixIt** app. 

1. Open **Web.config** on **.\Source\MyFixIt** and locate the **appSettings** section. Update the **ida:FederationMetadataLocation**, **ida:Realm** and **ida:AudienceUri** attributes with your custom Microsoft Azure Active Directory settings.

	````XML
	<appSettings>
		<add key="ida:FederationMetadataLocation" value="[FEDERATION-METADATA_DOCUMENT-URL]" />
		<add key="ida:Realm" value="[APP ID URI]" />
		<add key="ida:AudienceUri" value="[APP ID URI]" />
	</appSettings>
	````

1. Scroll down to the **system.identityModel** section and update the **audienceUri** value with your custom Microsoft Azure Active Directory audience uri.

	````XML
	 <system.identityModel>
		 <identityConfiguration>
			<issuerNameRegistry type="MyFixIt.Utils.DatabaseIssuerNameRegistry, MyFixIt" />
			<audienceUris>
			  <add value="[APP ID URI]" />
			</audienceUris>
			...
		 </identityConfiguration>
	  </system.identityModel>
	<system.webServer>
	````


1. Scroll down to the **system.identityModel.services** section and update the **audienceUri** value with your custom Microsoft Azure Active Directory audience uri.

	````XML
	<system.identityModel.services>
	 <federationConfiguration>
		<cookieHandler requireSsl="false" />
		<wsFederation passiveRedirectEnabled="false" issuer="https://login.windows.net/common/wsfed" realm="[APP ID URI]" requireHttps="false" />
	 </federationConfiguration>
	</system.identityModel.services>
	````

#### Configuring New Relic ####

1. Open **newrelic.config** on **.\Source\MyFixIt\newrelic** and update the **licensekey** attribute of the service with your custom license key and save the file.

	````XML
	<configuration xmlns="urn:newrelic-config" agentEnabled="true">
		<service licenseKey="[YOUR-LICENSE-KEY]" ssl="true" />
		
		...
	</configuration>
	````

#### Deploying the apps to Microsoft Azure ####

1. Open **Microsoft Azure Powershell** as Administrator.

1. Change directory to **.\Source\MyFixIt\Automation**.

1. Execute the following script to deploy **MyFixIt** to Microsoft Azure Website:

	````PowerShell
	.\deploy-azure-website-devbox.ps1 ..\MyFixIt\MyFixIt.csproj -Launch
	````
