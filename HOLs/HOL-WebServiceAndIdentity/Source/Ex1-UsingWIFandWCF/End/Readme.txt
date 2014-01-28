In order to run this End solution, the following steps are required:

1. Perform all the steps in the Getting Started of the lab.
2. Replace the following placeholders in all solutions:
        - {yourProjectName}, with the name you use for the certificate and the azure project (eg. foo)
        - {yourCertificateThumbprint}, with the thumbprint of the certificate you creating during the getting started (eg:939026E4657552526F49AA868DEA80F788991A73 ) 
                  You can find more information on how to retrieve the thumbprint of your certificate from the following MSDN article: http://msdn.microsoft.com/en-us/library/ms734695.aspx
        - {yourMachineName}, with the name of your machine
3. (optional) On the RelyingParty role configuration, in the Configuration tab | Startup action | Launch browser for,  uncheck the 'HTTP endpoint' and 'HTTPS endpoint' options.