namespace Expenses.Web.Helpers
{
    using System;
    using System.Configuration;
    using System.Security.Principal;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    public static class StorageHelper
    {
        public static CloudBlobContainer GetUserContainer(IIdentity user)
        {
            var connection = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("WAZStorageAccount"));
            var client = connection.CreateCloudBlobClient();
            client.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(5));

            var blobContainer = client.GetContainerReference(ExtractContainerName(user));
            blobContainer.CreateIfNotExist();
            blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });

            return blobContainer;
        }

        private static string ExtractContainerName(IIdentity user)
        {
            return user.Name.ToLowerInvariant().Replace(" ", "-");
        }
    }
}