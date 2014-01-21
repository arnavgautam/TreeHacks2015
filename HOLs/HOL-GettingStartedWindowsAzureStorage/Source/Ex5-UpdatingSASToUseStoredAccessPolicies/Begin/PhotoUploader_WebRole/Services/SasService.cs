using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;

namespace PhotoUploader_WebRole.Services
{
    public class SasService
    {
        private static CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

        public static string GetSasForTable(string username)
        {
            var cloudTableClient = StorageAccount.CreateCloudTableClient();
            var policy = new SharedAccessTablePolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                Permissions = SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Delete | SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Update
            };

            var sasToken = cloudTableClient.GetTableReference("Photos").GetSharedAccessSignature(
                policy: policy,
                accessPolicyIdentifier: null,
                startPartitionKey: username,
                startRowKey: null,
                endPartitionKey: username,
                endRowKey: null);

            return sasToken;
        }

        public static string GetReadonlyUriWithSasForBlob(string blobName)
        {
            var cloudBlobClient = StorageAccount.CreateCloudBlobClient();
            var blob = cloudBlobClient.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName")).GetBlockBlobReference(blobName);
            var sas = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(2),
            });

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", blob.Uri, sas);
        }

        public static string GetSasForBlobContainer()
        {
            var cloudBlobClient = StorageAccount.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName"));
            if (container.CreateIfNotExists())
            {
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            var sas = container.GetSharedAccessSignature(new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Delete,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(2),
            });

            return sas;
        }

        public static string GetAddSasForQueues()
        {
            var cloudQueueClient = StorageAccount.CreateCloudQueueClient();
            var policy = new SharedAccessQueuePolicy()
            {
                Permissions = SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };

            var sasToken = cloudQueueClient.GetQueueReference("messagequeue").GetSharedAccessSignature(policy, null);
            return sasToken;
        }
    }
}