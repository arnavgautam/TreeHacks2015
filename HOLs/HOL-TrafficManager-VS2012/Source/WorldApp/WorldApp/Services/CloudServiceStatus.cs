namespace WorldApp.Services
{
    using Microsoft.WindowsAzure.StorageClient;

    public class CloudServiceStatus : TableServiceEntity
    {
        public string Region { get; set; }

        public bool IsOnline { get; set; }
    }
}