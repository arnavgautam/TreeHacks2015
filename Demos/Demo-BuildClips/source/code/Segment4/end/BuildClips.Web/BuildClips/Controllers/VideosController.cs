namespace BuildClips.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using BuildClips.Extensions;
    using BuildClips.Services;
    using BuildClips.Services.Models;

    public class VideosController : ApiController
    {
        private readonly VideoService service;

        public VideosController()
        {
            this.service = new VideoService();
        }

        // GET /api/videos
        public IQueryable<Video> Get()
        {
            return this.service.GetAll();
        }

        // GET /api/videos/{id}
        public async Task<Video> Get(int id)
        {
            var video = await this.service.GetVideoAsync(id);
            if (video == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return video;
        }

        // POST: /api/videos        
        public async Task<HttpResponseMessage> Post()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.NotAcceptable);
            }

            var streamProvider = new MultipartMemoryStreamProvider();
            var provider = await Request.Content.ReadAsMultipartAsync(streamProvider);

            var videoInf = VideoInformation.Parse(provider.Contents);
            var validationErrors = videoInf.GetErrors();
            var isValid = !validationErrors.Any();

            if (isValid)
            {
                var video =
                    await
                    this.service.CreateVideoAsync(
                        videoInf.Title,
                        videoInf.Description,
                        videoInf.FileInformation.Name,
                        videoInf.FileInformation.Type,
                        await videoInf.FileInformation.Data.ReadAsStreamAsync());

                return Request.CreateResponse(HttpStatusCode.Created, video);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join("\n", validationErrors));
            }
        }

        // DELETE: /api/videos/{id}
        public async Task<HttpResponseMessage> Delete(int id)
        {
            try
            {
                await this.service.DeleteVideoAsync(id);
            }
            catch (InvalidOperationException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}