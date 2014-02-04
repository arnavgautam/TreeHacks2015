Before deploying to Windows Azure, you need to perform the following:
- Create a self signed certificate and configure the certificate in the cloud service as described in Exercise 4 of the lab document
- Update the "DiagnosticsConnectionString" from the "ServiceConfiguration.Cloud.cscfg" file with valid Storage Account values
- Update the "DefaultConnection" connection string entry from the "Web.config" file with a valid SQL Database connection string