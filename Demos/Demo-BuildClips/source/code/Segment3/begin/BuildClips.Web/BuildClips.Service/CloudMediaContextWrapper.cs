namespace BuildClips.Services
{
    using System;
    using System.Configuration;
    using System.Linq;

    using Microsoft.WindowsAzure.MediaServices.Client;

    public class CloudMediaContextWrapper
    {
        private readonly CloudMediaContext mediaContext;

        public CloudMediaContextWrapper()
        {
            this.mediaContext = new CloudMediaContext(
                ConfigurationManager.AppSettings["MediaServicesAccountName"],
                ConfigurationManager.AppSettings["MediaServicesAccountKey"]);
        }

        public IAsset GetAsset(string assetId)
        {
            return this.mediaContext.Assets.Where(a => a.Id == assetId).FirstOrDefault();
        }

        public IJob GetJob(string jobId)
        {
            return this.mediaContext.Jobs.Where(j => j.Id == jobId).FirstOrDefault();
        }

        public IMediaProcessor GetMediaProcessor(string processorId)
        {
            return this.mediaContext.MediaProcessors.Where(m => m.Id == processorId).FirstOrDefault();
        }

        public IAsset CreateAsset(string videoFilePath)
        {
            return this.mediaContext.Assets.Create(videoFilePath, AssetCreationOptions.None);
        }

        public IJob CreateJob(string jobName)
        {
            return this.mediaContext.Jobs.Create(jobName);
        }

        public ILocator CreateSasLocator(IAsset asset, AccessPermissions permissions, TimeSpan duration)
        {
            var accessPolicy = this.mediaContext.AccessPolicies.Create("Sas policy", duration, permissions);

            return this.mediaContext.Locators.CreateSasLocator(asset, accessPolicy, DateTime.UtcNow.AddMinutes(-5));
        }

        public ILocator CreateOriginLocator(IAsset asset)
        {
            var streamingPolicy = this.mediaContext.AccessPolicies.Create(
                "Streaming policy", TimeSpan.FromDays(1), AccessPermissions.Read);

            return this.mediaContext.Locators.CreateOriginLocator(
                asset, streamingPolicy, DateTime.UtcNow.AddMinutes(-5));
        }

        public void UpdateAsset(IAsset asset)
        {
            this.mediaContext.Assets.Update(asset);
        }

        public void DeleteAsset(IAsset asset)
        {
            this.mediaContext.Locators.Revoke(asset.Locators.Where(l => l.Type == LocatorType.Origin).FirstOrDefault());
            this.mediaContext.Assets.Delete(asset);
        }
    }
}
