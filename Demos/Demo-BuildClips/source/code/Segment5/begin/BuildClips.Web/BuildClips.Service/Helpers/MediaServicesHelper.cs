namespace BuildClips.Services
{
    using Microsoft.WindowsAzure.MediaServices.Client;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;

    public static class MediaServicesHelper
    {
        private const string ManifestFileExtension = ".ism";
        private const string H264SmoothStreamingEncodingPreset = "H264 Smooth Streaming SD 16x9";
        private const string EncoderProcessorId = "nb:mpid:UUID:70bdc2c3-ebf4-42a9-8542-5afc1e55d217";
        private const int CheckFirstManifestAvailabilityWaitTime = 45;
        private const int CheckManifestAvailabilityWaitTime = 10;
        private const string EncodingTask = "MP4->SS Task";
        private const string ThumbnailTask = "Thumbnail";
        private const string ThumbnailPreset = @"<?xml version=""1.0"" encoding=""utf-16""?><Thumbnail Size=""350,200"" Type=""Png"" Filename=""{OriginalFilename}_{ThumbnailTime}.{DefaultExtension}""><Time Value=""0:0:1""/></Thumbnail>";

        public static IAsset CreateAssetFromStream(this CloudMediaContext context, string fileName, string title, string contentType, Stream stream)
        {
            var temporalDirectoryPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), Thread.CurrentThread.ManagedThreadId.ToString());
            var videoFilePath = Path.Combine(temporalDirectoryPath, fileName);

            try
            {
                Directory.CreateDirectory(temporalDirectoryPath);

                using (var fileStream = File.Create(videoFilePath))
                {
                    stream.CopyTo(fileStream);
                }

                var asset = context.Assets.Create(title, AssetCreationOptions.None);
                asset.AlternateId = fileName;
                asset.Update();

                var assetFile = asset.AssetFiles.Create(fileName);
                assetFile.Upload(videoFilePath);
                assetFile.IsPrimary = true;
                assetFile.MimeType = contentType;
                assetFile.Update();                

                return asset;
            }
            finally
            {
                File.Delete(videoFilePath);
                Directory.Delete(temporalDirectoryPath);
            }
        }

        public static string ConvertAssetToSmoothStreaming(this CloudMediaContext context, IAsset asset, bool createThumbnail)
        {
            var configuration = MediaServicesHelper.H264SmoothStreamingEncodingPreset;
            var processor = context.MediaProcessors.Where(m => m.Id == MediaServicesHelper.EncoderProcessorId).FirstOrDefault();            

            var job = context.Jobs.Create(asset.Name);

            var task = job.Tasks.AddNew(
                MediaServicesHelper.EncodingTask,
                processor,
                configuration,
                TaskOptions.None);

            task.InputAssets.Add(asset);
            var encodedAsset = task.OutputAssets.AddNew(asset.Name + " - [" + configuration + "]", true, AssetCreationOptions.None);
            encodedAsset.AlternateId = asset.AlternateId + "_0";

            if (createThumbnail)
            {
                var thumbnailTask = job.Tasks.AddNew(
                    MediaServicesHelper.ThumbnailTask,
                    processor,
                    MediaServicesHelper.ThumbnailPreset,
                    TaskOptions.None);

                thumbnailTask.InputAssets.Add(asset);
                var thumbnailAsset = thumbnailTask.OutputAssets.AddNew(asset.Name + " - [thumbnail]", true, AssetCreationOptions.None);
                thumbnailAsset.AlternateId = asset.AlternateId + "_1";
            }
            
            job.Submit();

            return job.Id;
        }

        public static bool PublishJobAsset(this CloudMediaContext context, string jobId, out string encodedVideoUrl, out string thumbnailUrl)
        {
            encodedVideoUrl = null;
            thumbnailUrl = null;
            var job = context.Jobs.Where(j => j.Id == jobId).FirstOrDefault();
            if ((job == null) || !(job.State == JobState.Finished || job.State == JobState.Canceled || job.State == JobState.Error))
            {
                return false;
            }
            
            var encodingTask = job.Tasks.Where(t => t.Name == MediaServicesHelper.EncodingTask).FirstOrDefault();
            if (encodingTask != null)
            {
                var encodedAsset = encodingTask.OutputAssets.FirstOrDefault();
                if (encodedAsset != null)
                {
                    var manifestFile =
                        encodedAsset.AssetFiles.Where(f => f.Name.EndsWith(MediaServicesHelper.ManifestFileExtension)).FirstOrDefault();
                    if (manifestFile == null)
                    {
                        return false;
                    }

                    var originLocator = context.CreateOriginLocator(encodedAsset);
                    encodedVideoUrl = originLocator.GetFileUrl(manifestFile);
                }
            }

            var thumbnailTask = job.Tasks.Where(t => t.Name == MediaServicesHelper.ThumbnailTask).FirstOrDefault();
            if (thumbnailTask != null)
            {
                var thumbnailAsset = thumbnailTask.OutputAssets.FirstOrDefault();
                if (thumbnailAsset != null)
                {
                    var thumbnailFile = thumbnailAsset.AssetFiles.FirstOrDefault();
                    if (thumbnailFile != null)
                    {
                        var sasLocator = context.CreateSasLocator(thumbnailAsset, AccessPermissions.Read, TimeSpan.FromDays(30.0));
                        thumbnailUrl = sasLocator.GetFileUrl(thumbnailFile.Name);
                    }
                }
            }

            BlockUntilFileIsAvailable(encodedVideoUrl);

            job.Delete();

            return true;
        }

        public static string GetAssetVideoUrl(this CloudMediaContext context, IAsset asset)
        {
            var locator = context.CreateSasLocator(asset, AccessPermissions.Read, TimeSpan.FromDays(30));

            return locator.GetFileUrl(asset.AlternateId);
        }

        public static IJob GetJob(this CloudMediaContext context, string jobId)
        {
            return context.Jobs.Where(j => j.Id == jobId).FirstOrDefault();
        }

        private static ILocator CreateOriginLocator(this CloudMediaContext context, IAsset asset)
        {
            var streamingPolicy = context.AccessPolicies.Create(
                "Streaming policy", TimeSpan.FromDays(30), AccessPermissions.Read);
            
            return context.Locators.CreateLocator(
                LocatorType.OnDemandOrigin,
                asset, streamingPolicy, DateTime.UtcNow.AddMinutes(-5));
        }

        public static ILocator CreateSasLocator(this CloudMediaContext context, IAsset asset, AccessPermissions permissions, TimeSpan duration)
        {
            var accessPolicy = context.AccessPolicies.Create(
                "Sas policy", duration, permissions);

            return context.Locators.CreateLocator(
                LocatorType.Sas,
                asset, accessPolicy, DateTime.UtcNow.AddMinutes(-5));
        }
        
        private static string GetFileUrl(this ILocator locator, IAssetFile file)
        {
            return locator.GetFileUrl(file.Name);
        }

        private static string GetFileUrl(this ILocator locator, string fileName)
        {
            if (locator.Type == LocatorType.Sas)
            {
                return string.Concat(locator.BaseUri, locator.BaseUri.EndsWith("/") ? string.Empty : "/", Uri.EscapeDataString(fileName), locator.ContentAccessComponent);
            }
            else if (locator.Type == LocatorType.OnDemandOrigin)
            {
                return string.Concat(locator.Path, locator.Path.EndsWith("/") ? string.Empty : "/", Uri.EscapeDataString(fileName), "/Manifest");
            }

            return string.Empty;
        }

        private static void BlockUntilFileIsAvailable(string fileUrl)
        {
            var uri = new Uri(fileUrl, UriKind.Absolute);

            var statusCode = HttpStatusCode.BadRequest;

            var checkManifestAvailabilityMaxCount = 1;

            while (statusCode != HttpStatusCode.OK
                   && checkManifestAvailabilityMaxCount <= MediaServicesHelper.CheckFirstManifestAvailabilityWaitTime)
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
                                ? MediaServicesHelper.CheckFirstManifestAvailabilityWaitTime
                                : MediaServicesHelper.CheckManifestAvailabilityWaitTime));

                    checkManifestAvailabilityMaxCount++;
                }
            }

            if (checkManifestAvailabilityMaxCount > MediaServicesHelper.CheckFirstManifestAvailabilityWaitTime)
            {
                throw new Exception(string.Format("The file {0} is unavailable", fileUrl));
            }
        }
    }
}
