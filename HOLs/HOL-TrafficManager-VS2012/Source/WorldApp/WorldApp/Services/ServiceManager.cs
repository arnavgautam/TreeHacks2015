namespace WorldApp.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    public class ServiceManager
    {
        private const string ServiceStatusTableName = "ServiceStatus";

        public void InitializeHostedServiceStatus(string serviceRegion, string serviceUrlPrefix)
        {
            try
            {
                CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
                CloudTableClient tableStorage = account.CreateCloudTableClient();
                TableServiceContext context = tableStorage.GetDataServiceContext();
                
                tableStorage.CreateTableIfNotExist(ServiceStatusTableName);

                CloudServiceStatus status = context.CreateQuery<CloudServiceStatus>(ServiceStatusTableName)
                                                    .Where(p => p.RowKey == serviceUrlPrefix)
                                                    .AsTableServiceQuery()
                                                    .SingleOrDefault();

                if (status == null)
                {
                    status = new CloudServiceStatus()
                    {
                        PartitionKey = string.Empty,
                        RowKey = serviceUrlPrefix,
                        Region = serviceRegion,
                        IsOnline = true
                    };

                    context.AddObject(ServiceStatusTableName, status);
                }
                else
                {
                    status.Region = serviceRegion;
                    status.IsOnline = true;
                    context.UpdateObject(status);
                }

                context.SaveChangesWithRetries();
            }
            catch (StorageClientException)
            {
                // A StorageClientException that returns the error message "Unexpected internal storage client error" may be retried. Other 
                // errors are due to incorrect request parameters and should not be retried with the same parameters.
                // DiagnosticUtility.Trace.LogException(scex, "Error initializing storage due to a client-side error.");
            }
            catch (StorageServerException)
            {
                // These may be transient and requests resulting in such exceptions can be retried with the same parameters.
                // DiagnosticUtility.Trace.LogException(ssex, "Error initializing storage due to a server-side error.");
            }
        }

        public void UpdateHostedServiceStatus(string serviceUrlPrefix, bool appIsOnline)
        {
            try
            {
                TableServiceContext context = GetContext();

                CloudServiceStatus status = context.CreateQuery<CloudServiceStatus>(ServiceStatusTableName)
                                                    .Where(p => p.RowKey == serviceUrlPrefix)
                                                    .AsTableServiceQuery()
                                                    .SingleOrDefault();
                if (status != null)
                {
                    status.IsOnline = appIsOnline;
                    context.UpdateObject(status);
                    context.SaveChangesWithRetries();
                }
            }
            catch (StorageClientException)
            {
                // A StorageClientException that returns the error message "Unexpected internal storage client error" may be retried. Other 
                // errors are due to incorrect request parameters and should not be retried with the same parameters.
                // DiagnosticUtility.Trace.LogException(scex, "Error initializing storage due to a client-side error.");
            }
            catch (StorageServerException)
            {
                // These may be transient and requests resulting in such exceptions can be retried with the same parameters.
                // DiagnosticUtility.Trace.LogException(ssex, "Error initializing storage due to a server-side error.");
            }
        }

        public IEnumerable<CloudServiceStatus> GetHostedServiceStatus()
        {
            TableServiceContext context = GetContext();

            return context.CreateQuery<CloudServiceStatus>(ServiceStatusTableName).AsTableServiceQuery();
        }

        public bool GetHostedServiceStatus(string serviceUrlPrefix)
        {            
            CloudServiceStatus status = this.GetHostedServiceStatus()
                                            .Where(service => service.RowKey == serviceUrlPrefix)
                                            .SingleOrDefault();
            if (status != null)
            {
                return status.IsOnline;
            }

            return false;
        }

        private static TableServiceContext GetContext()
        {
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            CloudTableClient tableStorage = account.CreateCloudTableClient();
            TableServiceContext context = tableStorage.GetDataServiceContext();
            return context;
        }
    }
}