namespace MyFixIt.Persistence
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Web;

    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using MyFixIt.Logging;

    public class PhotoService : IPhotoService
    {
        private ILogger log = null;

        public PhotoService(ILogger logger)
        {
            this.log = logger;
        }

        public void CreateAndConfigure()
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                // Create a blob client and retrieve reference to images container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                // Create the "images" container if it doesn't already exist.
                if (container.CreateIfNotExists())
                {
                    // Enable public access on the newly created "images" container
                    container.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Blob
                        });

                    this.log.Information("Successfully created Blob Storage Images Container and made it public");
                }
            }
            catch (Exception ex)
            {
                this.log.Error(ex, "Failure to Create or Configure images container in Blob Storage Service");
            }
        }

        public string UploadPhoto(HttpPostedFileBase photoToUpload)
        {            
            if (photoToUpload == null || photoToUpload.ContentLength == 0)
            {
                return null;
            }

            string fullPath = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                // Create the blob client and reference the container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images");

                // Create a new dynamic name for the images we are about to upload
                string imageName = string.Format(
                    "task-photo-{0}{1}",
                    Guid.NewGuid().ToString(),
                    Path.GetExtension(photoToUpload.FileName));

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = photoToUpload.ContentType;
                blockBlob.UploadFromStream(photoToUpload.InputStream);

                // Convert to be HTTP based URI (default storage path is HTTPS)
                fullPath = string.Format("http://{0}{1}", blockBlob.Uri.DnsSafeHost, blockBlob.Uri.AbsolutePath);

                timespan.Stop();
                this.log.TraceApi("Blob Service", "PhotoService.UploadPhoto", timespan.Elapsed, "imagepath={0}", fullPath);
            }
            catch (Exception ex)
            {
                this.log.Error(ex, "Error upload photo blob to storage");
            }

            return fullPath;
        }
    }
}