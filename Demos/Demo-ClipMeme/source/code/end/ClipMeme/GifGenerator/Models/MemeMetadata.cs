namespace GifGenerator.Models
{
    using System;
    using Microsoft.WindowsAzure.Storage.Table;

    public class MemeMetadata : TableEntity
    {
        public MemeMetadata()
        {

        }

        public MemeMetadata(string blobName, string uri, string description, string username)
        {
            this.PartitionKey = DateTime.UtcNow.ToString("yyyy-MM-dd");
            this.RowKey = Guid.NewGuid().ToString();
            this.BlobUri = uri;
            this.BlobName = blobName;
            this.Description = description;
            this.Username = username;
        }

        public string BlobName { get; set; }

        public string BlobUri { get; set; }

        public string Description { get; set; }

        public string Username { get; set; }
    }
}
