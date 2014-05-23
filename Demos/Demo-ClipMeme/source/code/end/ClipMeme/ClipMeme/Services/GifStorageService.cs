namespace ClipMeme.Services
{
    using ClipMeme.Models;
    using GifGenerator.Models;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;

    public class GifStorageService
    {
        private readonly CloudStorageAccount _storageAccount;

        static GifStorageService()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var queueClient = storageAccount.CreateCloudQueueClient();
            var tableClient = storageAccount.CreateCloudTableClient();

            blobClient.GetContainerReference("uploads").CreateIfNotExists();
            var container = blobClient.GetContainerReference("memes");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Container });

            queueClient.GetQueueReference("uploads").CreateIfNotExists();

            tableClient.GetTableReference("MemeMetadata").CreateIfNotExists();
        }

        public GifStorageService()
        {
            _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
        }

        public Task<IEnumerable<Meme>> GetAllAsync()
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("MemeMetadata");
            var query = new TableQuery<MemeMetadata>();
            var requestOptions = new TableRequestOptions()
            {
                RetryPolicy = new LinearRetry(TimeSpan.FromMilliseconds(500), 5)
            };

            var memesMetadata = table.ExecuteQuery(query, requestOptions).OrderByDescending(md => md.BlobName);

            IEnumerable<Meme> result = new List<Meme>();
            foreach (var memeMetadata in memesMetadata)
            {
                (result as List<Meme>).Add(new Meme { Name = memeMetadata.Description, URL = memeMetadata.BlobUri, Username = memeMetadata.Username });
            }


            return Task.FromResult(result);
        }

        public async Task StoreGifAsync(string fileName, byte[] gifBytes, string mediaType, IDictionary<string, string> metadata)
        {

            var blobClient = _storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("uploads");

            var blob = container.GetBlockBlobReference(fileName);
            await blob.UploadFromByteArrayAsync(gifBytes, 0, gifBytes.Length);
            blob.Properties.ContentType = mediaType;
            foreach (var entry in metadata)
            {
                blob.Metadata.Add(entry.Key, entry.Value);
            }

            blob.SetMetadata();
            blob.SetProperties();

            var queueClient = _storageAccount.CreateCloudQueueClient();
            queueClient.RetryPolicy = new LinearRetry(TimeSpan.FromMilliseconds(500), 5);
            var queue = queueClient.GetQueueReference("uploads");

            var messageContent = new JavaScriptSerializer().Serialize(new Message
            {
                BlobName = fileName,
                OverlayText = metadata["OverlayText"],
                UserName = metadata["UserName"],
                HubId = metadata["HubId"]
            });

            await queue.AddMessageAsync(new CloudQueueMessage(messageContent));
        }
    }
}