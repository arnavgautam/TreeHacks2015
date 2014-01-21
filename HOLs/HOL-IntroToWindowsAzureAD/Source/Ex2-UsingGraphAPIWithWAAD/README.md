Before executing the end solution provided for Exercise 2, execute the following steps:

- **Enable SSL** from the properties of the ExpenseReport project.
- Update the **project URL** in the **Web** tab of the ExpenseReport project properties in the _Use Local IIS Web server_ section with the SSL URL obtained from the previous step.
- Update the **App URL** of the configured application in the Windows Azure Management Portal with the SSL URL obtained from the first step.
- Run the **Identity and Access** wizard over the ExpenseReport project selecting the **Use a business identity provider** and updating the **Federation Metadata URL** placeholder with the one located in the configured application in the Windows Azure Management Portal and the **APP ID URI** placeholder with the value obtained from the first step.
- In the **Web.config** file, update the _[YOUR-CLIENT-ID]_ and the _[YOUR-APPLICATION-KEY-VALUE]_ placeholders with the values from the Configure tab of your application in the Windows Azure Management Portal. Also update the _[YOUR-APP-ID-URL]_ placeholder in the federation configuration element with the URL obtained from the first step.

For more information, check Task 2 and Task 3 from exercise 1 in the HOL.md document.