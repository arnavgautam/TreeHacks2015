namespace BuildClips.Services
{
    using Microsoft.WindowsAzure.Storage;

    public static class StorageConfiguration
    {
        public static CloudStorageAccount StorageAccount
        {
            get
            {
                var storageConnectionString = Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("StorageConnectionString");
                
                return string.IsNullOrEmpty(storageConnectionString) ? 
                    CloudStorageAccount.DevelopmentStorageAccount : CloudStorageAccount.Parse(storageConnectionString);
            }
        }
    }
}
