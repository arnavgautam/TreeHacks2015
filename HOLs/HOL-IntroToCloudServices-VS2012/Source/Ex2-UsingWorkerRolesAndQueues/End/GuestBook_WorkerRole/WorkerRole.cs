using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using GuestBook_Data;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GuestBook_WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private CloudQueue queue;
        private CloudBlobContainer container;

        public override void Run()
        {
            Trace.TraceInformation("Listening for queue messages...");

            while (true)
            {
                try
                {
                    // retrieve a new message from the queue
                    CloudQueueMessage msg = this.queue.GetMessage();
                    if (msg != null)
                    {
                        // parse message retrieved from queue
                        var messageParts = msg.AsString.Split(new char[] { ',' });
                        var imageBlobName = messageParts[0];
                        var partitionKey = messageParts[1];
                        var rowkey = messageParts[2];
                        Trace.TraceInformation("Processing image in blob '{0}'.", imageBlobName);

                        string thumbnailName = System.Text.RegularExpressions.Regex.Replace(imageBlobName, "([^\\.]+)(\\.[^\\.]+)?$", "$1-thumb$2");

                        CloudBlockBlob inputBlob = this.container.GetBlockBlobReference(imageBlobName);
                        CloudBlockBlob outputBlob = this.container.GetBlockBlobReference(thumbnailName);

                        using (Stream input = inputBlob.OpenRead())
                        using (Stream output = outputBlob.OpenWrite())
                        {
                            this.ProcessImage(input, output);

                            // commit the blob and set its properties
                            outputBlob.Properties.ContentType = "image/jpeg";
                            string thumbnailBlobUri = outputBlob.Uri.ToString();

                            // update the entry in table storage to point to the thumbnail
                            GuestBookDataSource ds = new GuestBookDataSource();
                            ds.UpdateImageThumbnail(partitionKey, rowkey, thumbnailBlobUri);

                            // remove message from queue
                            this.queue.DeleteMessage(msg);

                            Trace.TraceInformation("Generated thumbnail in blob '{0}'.", thumbnailBlobUri);
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (StorageException e)
                {
                    Trace.TraceError("Exception when processing queue item. Message: '{0}'", e.Message);
                    System.Threading.Thread.Sleep(5000);
                }
            }

        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // read storage account configuration settings
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

            // initialize blob storage
            CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();
            this.container = blobStorage.GetContainerReference("guestbookpics");

            // initialize queue storage 
            CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();
            this.queue = queueStorage.GetQueueReference("guestthumbs");

            Trace.TraceInformation("Creating container and queue...");

            bool storageInitialized = false;
            while (!storageInitialized)
            {
                try
                {
                    // create the blob container and allow public access
                    this.container.CreateIfNotExists();
                    var permissions = this.container.GetPermissions();
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    this.container.SetPermissions(permissions);

                    // create the message queue(s)
                    this.queue.CreateIfNotExists();

                    storageInitialized = true;
                }
                catch (StorageException e)
                {
                    var requestInformation = e.RequestInformation;
                    var errorCode = requestInformation.ExtendedErrorInformation.ErrorCode;//errorCode = ContainerAlreadyExists
                    var statusCode = (System.Net.HttpStatusCode)requestInformation.HttpStatusCode;//requestInformation.HttpStatusCode = 409, statusCode = Conflict
                    if (statusCode == HttpStatusCode.NotFound)
                    {
                        Trace.TraceError(
                          "Storage services initialization failure. "
                          + "Check your storage account configuration settings. If running locally, "
                          + "ensure that the Development Storage service is running. Message: '{0}'",
                          e.Message);
                        System.Threading.Thread.Sleep(5000);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return base.OnStart();

        }

        public void ProcessImage(Stream input, Stream output)
        {
            int width;
            int height;
            var originalImage = new Bitmap(input);

            if (originalImage.Width > originalImage.Height)
            {
                width = 128;
                height = 128 * originalImage.Height / originalImage.Width;
            }
            else
            {
                height = 128;
                width = 128 * originalImage.Width / originalImage.Height;
            }

            Bitmap thumbnailImage = null;

            try
            {
                thumbnailImage = new Bitmap(width, height);

                using (Graphics graphics = Graphics.FromImage(thumbnailImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(originalImage, 0, 0, width, height);
                }

                thumbnailImage.Save(output, ImageFormat.Jpeg);
            }
            finally
            {
                if (thumbnailImage != null)
                {
                    thumbnailImage.Dispose();
                }
            }
        }

    }
}
