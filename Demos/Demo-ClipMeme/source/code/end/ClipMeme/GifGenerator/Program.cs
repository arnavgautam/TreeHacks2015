namespace GifGenerator
{
    #region using System...
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Threading.Tasks;
    using GifGenerator.Models;
    using Microsoft.AspNet.SignalR.Client;
    #endregion
    using Microsoft.WindowsAzure.Jobs;
    using NGif;

    public class Program
    {
        private const string Url = "http://azuretkclipmeme.azurewebsites.net";
        private static IHubProxy hub;

        public static void Main(string[] args)
        {
            var hubConnection = new HubConnection(Url);
            hub = hubConnection.CreateHubProxy("GifServerHub");
            hubConnection.Start().Wait();

            Console.WriteLine("Connected to {0}", Url);

            var host = new JobHost();
            host.RunAndBlock();
        }

        public static async Task ProcessImage(
            [QueueInput("uploads")] Message message,
            [BlobInput("uploads/{BlobName}")] Stream input,
            [BlobOutput("memes/{BlobName}")] Stream output)
        {
            var encoder = new AnimatedGifEncoder();
            encoder.SetRepeat(0);

            int delay;
            var frames = ProcessInputFrames(input, message.OverlayText.ToUpperInvariant(), out delay);
            encoder.SetDelay(delay);

            using (var result = new MemoryStream())
            {
                encoder.Start(result);

                var idx = 1;
                foreach (var frame in frames)
                {
                    Console.WriteLine("Adding frame #{0}/{1}", idx, frames.Count);
                    encoder.AddFrame(frame);
                    idx++;
                }

                encoder.Finish();

                result.Position = 0;
                result.CopyTo(output);
            }

            var uri = SetMetadataAndCleanup(message);

            await SendCompleteNotification(message, uri);
        }

        private static async Task SendCompleteNotification(Message message, string uri)
        {
            Console.WriteLine("Invoked  GifGenerationCompleted with URL: {0}", uri);
            await hub.Invoke("GifGenerationCompleted", message.HubId, uri);
        }

        #region Private Methods
        private static IList<Bitmap> ProcessInputFrames(Stream input, string text, out int delay)
        {
            // default
            delay = 50;

            using (var copiedInput = new MemoryStream())
            {
                input.CopyTo(copiedInput);

                using (var gifImage = Image.FromStream(copiedInput))
                {
                    var frameCount = gifImage.GetFrameCount(FrameDimension.Time);

                    var frames = new List<Bitmap>();

                    for (var index = 0; index < frameCount; index++)
                    {
                        Console.WriteLine("Processing frame #{0}/{1}", index + 1, frameCount);
                        gifImage.SelectActiveFrame(FrameDimension.Time, index);
                        var bitmap = gifImage.Clone() as Bitmap;

                        if (index == 0)
                        {
                            delay = BitConverter.ToInt32(gifImage.GetPropertyItem(20736).Value, index) * 10;
                        }

                        frames.Add(RenderOverlay(bitmap, text));
                    }

                    return frames;
                }
            }
        }

        private static Bitmap RenderOverlay(Bitmap source, string overlayText)
        {
            // fails for indexed pixel format image (see: m1col.gif sample)
            var graphics = Graphics.FromImage(source);  
            var brush = new SolidBrush(Color.White);
            var fontSize = 32;
            var font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold);
            SizeF size;
            do
            {
                font = new Font("Microsoft Sans Serif", fontSize--, FontStyle.Bold);
                size = graphics.MeasureString(overlayText, font);
            } while (size.Width > source.Width);

            // shadows
            graphics.DrawString(overlayText, font, new SolidBrush(Color.Black), ((source.Width - size.Width) / 2) - 1, source.Height - size.Height);
            graphics.DrawString(overlayText, font, new SolidBrush(Color.Black), (source.Width - size.Width) / 2, source.Height - size.Height - 1);
            graphics.DrawString(overlayText, font, new SolidBrush(Color.Black), ((source.Width - size.Width) / 2) + 1, source.Height - size.Height);
            graphics.DrawString(overlayText, font, new SolidBrush(Color.Black), (source.Width - size.Width) / 2, source.Height - size.Height + 1);

            // black box
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(51, 0, 0, 0)), 0, source.Height - size.Height - 1, source.Width, size.Height + 1);

            // overlay text
            graphics.DrawString(overlayText, font, brush, (source.Width - size.Width) / 2, source.Height - size.Height);

            return source;
        }

        private static string SetMetadataAndCleanup(Message message)
        {
            // instance clients
            var storageClientAccount = Microsoft.WindowsAzure.CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureJobsData"].ConnectionString);
            var blobClient = new Microsoft.WindowsAzure.StorageClient.CloudBlobClient(storageClientAccount.BlobEndpoint, storageClientAccount.Credentials);
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureJobsData"].ConnectionString);
            var tableClient = new Microsoft.WindowsAzure.Storage.Table.CloudTableClient(storageAccount.TableEndpoint, storageAccount.Credentials);

            // get output blob uri
            var blobUri = blobClient.GetContainerReference("memes").GetBlockBlobReference(message.BlobName).Uri.AbsoluteUri;

            // save metadata
            var table = tableClient.GetTableReference("MemeMetadata");
            table.CreateIfNotExists();
            var requestOptions = new Microsoft.WindowsAzure.Storage.Table.TableRequestOptions()
            {
                RetryPolicy = new Microsoft.WindowsAzure.Storage.RetryPolicies.LinearRetry(TimeSpan.FromMilliseconds(500), 5)
            };
            table.Execute(Microsoft.WindowsAzure.Storage.Table.TableOperation.Insert(new MemeMetadata(message.BlobName, blobUri, message.OverlayText, message.UserName)), requestOptions);

            try
            {
                // delete source
                var container = blobClient.GetContainerReference("uploads");
                var blob = container.GetBlockBlobReference(message.BlobName);
                blob.Delete();
            }
            catch (Exception ex)
            {
                Console.WriteLine("WARNING! deleting blob: " + ex.Message);
            }

            return blobUri;
        }
        #endregion
    }
}