namespace BuildClips.Services
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;

    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.MediaServices.Client;
    using Microsoft.WindowsAzure.StorageClient;

    public class CloudMediaService
    {
        private readonly CloudMediaContextWrapper mediaCloudHelper;

        public CloudMediaService()
        {
            this.mediaCloudHelper = new CloudMediaContextWrapper();
        }

        public IAsset GetAsset(string assetId)
        {
            return this.mediaCloudHelper.GetAsset(assetId);
        }

        public IJob GetJob(string jobId)
        {
            return this.mediaCloudHelper.GetJob(jobId);
        }

        public void DeleteAsset(IAsset asset)
        {
            this.mediaCloudHelper.DeleteAsset(asset);
        }

        public string GetAssetVideoUrl(IAsset asset)
        {
            this.ChangeContentTypeForFiles(asset, CloudMediaConstants.Mp4ContentType, CloudMediaConstants.Mp4FileExtension);

            var locator = this.mediaCloudHelper.CreateSasLocator(asset, AccessPermissions.Read, TimeSpan.FromDays(30));

            return this.GetFileUrl(locator, asset.AlternateId);
        }

        public IAsset CreateAssetFromStream(string name, Stream stream)
        {
            var temporalDirectoryPath = Path.GetTempPath();

            var videoFileName = string.Format(
                "{0}_{1}{2}", CloudMediaConstants.VideoFileTitlePrefix, Guid.NewGuid(), Path.GetExtension(name));
            var videoFilePath = Path.Combine(temporalDirectoryPath, videoFileName);

            using (var fileStream = File.Create(videoFilePath))
            {
                stream.CopyTo(fileStream);
            }

            var asset = this.mediaCloudHelper.CreateAsset(videoFilePath);

            File.Delete(videoFilePath);

            asset.Name = name;
            asset.AlternateId = videoFileName;
            this.mediaCloudHelper.UpdateAsset(asset);

            return asset;
        }

        public IJob ConvertAssetToSmoothStreaming(IAsset asset)
        {
            var processor = this.mediaCloudHelper.GetMediaProcessor(CloudMediaConstants.EncoderProcessorId);

            var job = this.mediaCloudHelper.CreateJob(asset.Name);

            var task = job.Tasks.AddNew(
                "MP4->SS Task",
                processor,
                CloudMediaConstants.H264SmoothStreamingEncodingPreset,
                TaskCreationOptions.None);

            task.InputMediaAssets.Add(asset);
            task.OutputMediaAssets.AddNew(asset.Name, true, AssetCreationOptions.None);

            job.Submit();

            return job;
        }

        public string PublishJobAsset(string jobId)
        {
            var job = this.GetJob(jobId);
            var asset = job.OutputMediaAssets[0];

            // Since the ODATA Linq provider doesn't support the First method, the files are first filtered using Where
            // Then, the First result of the filtered list is selected
            var manifestFile =
                asset.Files.Where(f => f.Name.EndsWith(string.Concat(".", CloudMediaConstants.ManifestFileExtension))).First();

            var originLocator = this.mediaCloudHelper.CreateOriginLocator(asset);

            var encodedVideoUrl = this.GetFileUrl(originLocator, manifestFile);

            this.BlockUntilFileIsAvailable(encodedVideoUrl);

            return encodedVideoUrl;
        }

        private string GetFileUrl(ILocator locator, IFileInfo file)
        {
            return this.GetFileUrl(locator, file.Name);
        }

        private string GetFileUrl(ILocator locator, string fileName)
        {
            var url = string.Empty;

            if (locator.Type == LocatorType.Sas)
            {
                url = this.GetSasFileUrl(locator, fileName);
            }
            else if (locator.Type == LocatorType.Origin)
            {
                url = this.GetOriginFileUrl(locator, fileName);
            }

            return url;
        }

        private string GetOriginFileUrl(ILocator locator, string fileName)
        {
            return locator.Path + fileName + "/manifest";
        }

        private string GetSasFileUrl(ILocator locator, string fileName)
        {
            string url;

            var queryPos = locator.Path.IndexOf('?');
            if (queryPos < 0)
            {
                var addSlash = locator.Path.EndsWith("/") ? string.Empty : "/";
                url = string.Concat(locator.Path, addSlash, fileName);
            }
            else
            {
                var slashPos = locator.Path.IndexOf("/?", StringComparison.InvariantCultureIgnoreCase);
                var slash = slashPos + 1 == queryPos ? string.Empty : "/";
                url = locator.Path.Replace("?", string.Concat(slash, fileName, "?"));
            }

            return url;
        }

        private void ChangeContentTypeForFiles(IAsset asset, string contentType, string fileExtension)
        {
            var connectionString =
                   ConfigurationManager.AppSettings["MediaServicesStorageAccountConnectionString"];

            var account = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient client = null;

            foreach (var assetFile in asset.Files)
            {
                bool shouldChangeContentType = !assetFile.Name.Contains(fileExtension) ||
                                               (assetFile.MimeType != null &&
                                                assetFile.MimeType.Equals(
                                                    contentType, StringComparison.InvariantCultureIgnoreCase));

                if (shouldChangeContentType)
                {
                    continue;
                }
               
                client = client ?? account.CreateCloudBlobClient();

                var writeSasLocator = this.mediaCloudHelper.CreateSasLocator(
                    asset, AccessPermissions.Write, TimeSpan.FromMinutes(2));
                var assetContainerName = writeSasLocator.Path.Split('?')[0];

                var blobPath = string.Concat(assetContainerName, "/", assetFile.Name);
                var blob = client.GetBlobReference(blobPath);
                blob.Properties.ContentType = contentType;
                blob.SetProperties();
            }
        }

        private void BlockUntilFileIsAvailable(string fileUrl)
        {
            var uri = new Uri(fileUrl, UriKind.Absolute);

            var statusCode = HttpStatusCode.BadRequest;

            var checkManifestAvailabilityMaxCount = 1;

            while (statusCode != HttpStatusCode.OK
                   && checkManifestAvailabilityMaxCount <= CloudMediaConstants.CheckFirstManifestAvailabilityWaitTime)
            {
                var request = WebRequest.CreateDefault(uri);
                try
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    statusCode = response.StatusCode;
                }
                catch (WebException exception)
                {
                    statusCode = ((HttpWebResponse)exception.Response).StatusCode;

                    Thread.Sleep(
                        TimeSpan.FromSeconds(
                            checkManifestAvailabilityMaxCount == 1
                                ? CloudMediaConstants.CheckFirstManifestAvailabilityWaitTime
                                : CloudMediaConstants.CheckManifestAvailabilityWaitTime));

                    checkManifestAvailabilityMaxCount++;
                }
            }

            if (checkManifestAvailabilityMaxCount > CloudMediaConstants.CheckFirstManifestAvailabilityWaitTime)
            {
                throw new Exception(string.Format("The file {0} is unavailable", fileUrl));
            }
        }
    }
}
