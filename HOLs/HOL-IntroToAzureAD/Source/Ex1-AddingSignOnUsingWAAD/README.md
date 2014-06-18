Before executing the end solution provided for Exercise 1, execute the following steps:

- **Enable SSL** from the properties of the ExpenseReport project.
- Update the **project URL** in the **Web** tab of the ExpenseReport project properties in the _Use Local IIS Web server_ section with the SSL URL obtained from the previous step.
- Update the **App URL** of the configured application in the Microsoft Azure Management Portal with the SSL URL obtained from the first step.
- In the **Web.config** file, update the following sections:

	- The _[APP-ID-URI]_ placeholder from the **audienceUris** in the **system.identityModel** section with your **App ID URI**.
	- The _[APP-ID-URI]_ placeholder from the **realm** attribute from the **federationConfiguration** element in the  **system.identityModel.services** section with your **App ID URI**
	- The _[YOUR-DIRECTORY-NAME]_ placeholder from the **issuer** attribute from the **federationConfiguration** element in the  **system.identityModel.services** section  with your directory name (without spaces)
	- The _[FEDERATION-METADATA-DOCUMENT]_ placeholder for the  **ida:FederationMetadataLocation** key in the **AppSettings** 
	- The _[APP-ID-URI]_ placeholder for the  **ida:Realm** key in the **AppSettings** 
	- The _[APP-ID-URI]_ placeholder for the  **ida:AudienceUri** key in the **AppSettings** 

For more information, check Task 2 and Task 3 from exercise 1 in the HOL.md document.
