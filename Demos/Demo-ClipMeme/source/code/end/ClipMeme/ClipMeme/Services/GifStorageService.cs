namespace ClipMeme.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;

    using ClipMeme.Models;
    using GifGenerator.Models;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Table;

    public class GifStorageService
    {
        private readonly CloudStorageAccount storageAccount;

        public GifStorageService()
        {
            this.storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
        }

        public Task<IEnumerable<Meme>> GetAllAsync()
        {
            var tableClient = this.storageAccount.CreateCloudTableClient();
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
            var blobClient = this.storageAccount.CreateCloudBlobClient();
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

            var queueClient = this.storageAccount.CreateCloudQueueClient();
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