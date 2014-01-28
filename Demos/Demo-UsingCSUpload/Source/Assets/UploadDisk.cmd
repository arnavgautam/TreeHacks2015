"C:\Program Files\Microsoft SDKs\Windows Azure\.NET SDK\2012-10\bin\csupload.exe" Set-Connection "SubscriptionId=[SUBSCRIPTION ID];CertificateThumbprint=[CERT THUMB PRINT];ServiceManagementEndpoint=https://management.core.windows.net"

"C:\Program Files\Microsoft SDKs\Windows Azure\.NET SDK\2012-10\bin\csupload.exe" Add-Disk -LiteralPath "C:\temp\mydatadisk.vhd" -Destination "https://[STORAGE ACCOUNT NAME].blob.core.windows.net/uploads/mydatadisk.vhd" -Label mydatadisk.vhd 
