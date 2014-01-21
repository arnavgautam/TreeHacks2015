namespace BuildClips.Services
{
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class VideoStorage
    {
        public async Task<string> UploadVideoAsync(Stream stream, string sourceFilename, string sourceContentType)
        {
            // Get the blob container
            var fileExtension = Path.GetExtension(sourceFilename.Replace("\"", ""));
            var blobName = string.Format(VideoBlobReferenceTemplate, Guid.NewGuid(), fileExtension);
            var container = GetBlobContainer(VideosContainer);

            // Create the Block Blob
            var videoBlob = container.GetBlockBlobReference(blobName);
            videoBlob.Properties.ContentType = sourceContentType;

            // Upload the video stream
            await UploadFromStreamAsync(videoBlob, stream);

            // Return the video url
            return videoBlob.Uri.ToString();
        }

        public async void DeleteVideoAsync(string videoUrl)
        {
            var blob = GetBlobContainer(VideosContainer).GetBlockBlobReference(videoUrl);
            bool result = await DeleteIfExistsAsync(blob);
        }

        public string GetVideoUrl(string videoUrl)
        {
            // TODO: this is a hack, we should implement checking if the SAS key is still valid.
            var uri = new Uri(videoUrl);
            if (!string.IsNullOrEmpty(uri.Query))
            {
                return videoUrl;
            }

            var container = GetBlobContainer(VideosContainer);

            var policy = GetNonExpiredContainerPolicy(container);
            var videoBlob = container.GetBlockBlobReference(videoUrl);
            var videoSas = videoBlob.GetSharedAccessSignature(policy);

            return string.Format("{0}{1}", videoBlob.Uri.AbsoluteUri, videoSas);
        }

        public string GetDefaultThumbnailUrl()
        {
            return string.Format("{0}/{1}.png", this.GetBlobContainer(ThumbnailsContainer).Uri.ToString(), DefaultThumbnail);
        }

        #region Private Methods

        private const string ConnectionStringSetting = "StorageConnectionString";
        private const string VideosContainer = "buildclips";
        private const string VideoBlobReferenceTemplate = "video_{0}{1}";
        private const string PolicyName = "VideoAccessPolicy";
        private const string ThumbnailsContainer = "thumbnails";
        private const string DefaultThumbnail = "nothumbnail";

        private const int PolicyExpirationTimeInDays = 30;
        public Microsoft.WindowsAzure.Storage.CloudStorageAccount StorageAccount { get; private set; }

        public VideoStorage()
        {
            StorageAccount = StorageConfiguration.StorageAccount;
            this.CreateContainerWithNoPublicAccess();
            this.CreateThumbnailsContainer();
        }

        private CloudBlobContainer GetBlobContainer(string containerName)
        {

            var client = this.StorageAccount.CreateCloudBlobClient();
            client.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(5), 3);
            return client.GetContainerReference(containerName);
        }

        private SharedAccessBlobPolicy GetNonExpiredContainerPolicy(CloudBlobContainer container)
        {
            SharedAccessBlobPolicy policy;
            bool policyExpired = false;
            var permissions = container.GetPermissions();
            var policyExists = permissions.SharedAccessPolicies.TryGetValue(PolicyName, out policy);

            if (policyExists)
            {
                policyExpired = policy.SharedAccessExpiryTime < DateTime.UtcNow;
            }

            if (policyExpired || !policyExists)
            {
                policy = CreateSharedAccessPolicy();
                permissions.SharedAccessPolicies.Remove(PolicyName);
                permissions.SharedAccessPolicies.Add(PolicyName, policy);
                container.SetPermissions(permissions);
            }

            return policy;
        }

        private void CreateContainerWithNoPublicAccess()
        {
            var container = this.GetBlobContainer(VideosContainer);
            container.CreateIfNotExists();

            var permissions = container.GetPermissions();

            if (!permissions.SharedAccessPolicies.ContainsKey(PolicyName))
            {
                permissions.SharedAccessPolicies.Add(PolicyName, CreateSharedAccessPolicy());
            }

            if (permissions.PublicAccess != BlobContainerPublicAccessType.Off)
            {
                permissions.PublicAccess = BlobContainerPublicAccessType.Off;
            }
        
            container.SetPermissions(permissions);
        }

        private SharedAccessBlobPolicy CreateSharedAccessPolicy()
        {
            return new SharedAccessBlobPolicy
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddDays(PolicyExpirationTimeInDays)
                };
        }

        private void CreateThumbnailsContainer()
        {
            var container = this.GetBlobContainer(ThumbnailsContainer);
            container.CreateIfNotExists();

            var permissions = container.GetPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            container.SetPermissions(permissions);
        }

        private static Task UploadFromStreamAsync(ICloudBlob blob, Stream stream)
        {
            return Task.Factory.FromAsync(blob.BeginUploadFromStream, blob.EndUploadFromStream, stream, null);
        }

        private static Task<bool> DeleteIfExistsAsync(ICloudBlob blob)
        {
            return Task<bool>.Factory.FromAsync(blob.BeginDeleteIfExists, blob.EndDeleteIfExists, (object)null);
        }

        #endregion
    }
}