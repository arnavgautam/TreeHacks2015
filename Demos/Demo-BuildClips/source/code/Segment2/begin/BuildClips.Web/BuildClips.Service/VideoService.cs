namespace BuildClips.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using BuildClips.Services.Models;
    
    public class VideoService
    {
        private readonly VideosContext context;

        public VideoService()
        {
            this.context = new VideosContext();
        }

        public IQueryable<Video> GetAll()
        {
            return this.context.Videos.OrderByDescending(v => v.Id);
        }

        public async Task<Video> GetVideoAsync(int id)
        {
            var token = new CancellationToken();
            var video = await this.context.Videos.FindAsync(token, id);

            return video;
        }

        public async Task<Video> CreateVideoAsync(string title, string description, string name, string type, Stream dataStream)
        {
            // TODO: Start encoding uploaded video - BuildVideoServicecsCreate
            VideoStorage videoStorage = new VideoStorage();
            var videoUrl = await videoStorage.UploadVideoAsync(dataStream, name, type);
            videoUrl = videoStorage.GetVideoUrl(videoUrl);
            string jobId = null;

            var video = new Video
                {
                    Title = title,
                    Description = description,
                    SourceVideoUrl = videoUrl,
                    JobId = jobId
                };

            this.context.Videos.Add(video);
            await this.context.SaveChangesAsync();

            return video;
        }

        public Video Publish(int id)
        {
            var video = this.context.Videos.FirstOrDefault(v => v.Id == id);

            if (video == null)
            {
                return null;
            }

            //// TODO:  Publish video in Media Services - BuildVideoServicecsPublish
            video.JobId = null;

            this.context.SaveChanges();

            return video;
        }

        public async Task DeleteVideoAsync(int id)
        {
            var video = this.context.Videos.FirstOrDefault(v => v.Id == id);

            this.context.Videos.Remove(video);

            await this.context.SaveChangesAsync();
        }
    }
}
