namespace ClipMeme.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using ClipMeme.Models;
    using ClipMeme.Services;
  
    public class GifController : ApiController
    {
        public async Task<IEnumerable<Meme>> Get()
        {
            return await new GifStorageService().GetAllAsync();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public async Task Post()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();

            // Read the form data.
            await Request.Content.ReadAsMultipartAsync(provider);

            var fileName = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            var overlayText = string.Empty;
            var mediaType = string.Empty;
            var hubId = string.Empty;
            var username = string.Empty;

            byte[] bytes = null;
            foreach (var content in provider.Contents)
            {
                if (content.Headers.ContentDisposition.Name.Trim('"').Equals("textOverlay", StringComparison.InvariantCultureIgnoreCase))
                {
                    overlayText = await content.ReadAsStringAsync();
                }
                else if (content.Headers.ContentDisposition.Name.Trim('"').Equals("hubid", StringComparison.InvariantCultureIgnoreCase))
                {
                    hubId = await content.ReadAsStringAsync();
                }
                else if (content.Headers.ContentDisposition.Name.Trim('"').Equals("username", StringComparison.InvariantCultureIgnoreCase))
                {
                    username = await content.ReadAsStringAsync();
                }
                else if (content.Headers.ContentDisposition.Name.Trim('"').Equals("file", StringComparison.InvariantCultureIgnoreCase))
                {
                    fileName += "-" + content.Headers.ContentDisposition.FileName.Trim('"');
                    mediaType = content.Headers.ContentType.MediaType;
                    bytes = await content.ReadAsByteArrayAsync();
                }
            }

            await new GifStorageService().StoreGifAsync(
                fileName,
                bytes,
                mediaType,
                new Dictionary<string, string> 
                {
                    { "UserName", username },
                    { "OverlayText", overlayText },
                    { "HubId", hubId }
                });
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
