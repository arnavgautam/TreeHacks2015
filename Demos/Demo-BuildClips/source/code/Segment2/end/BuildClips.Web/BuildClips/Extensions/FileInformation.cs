namespace BuildClips.Extensions
{
    using System.Collections.Generic;
    using System.Net.Http;

    public class FileInformation
    {
        private FileInformation()
        {
        }

        public HttpContent Data { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public static FileInformation Parse(IEnumerable<HttpContent> contents, string contentKey)
        {
            HttpContent fileData;
            contents.TryGetFormFieldHttpContentValue(contentKey, out fileData);

            if (fileData == null)
            {
                return null;
            }

            var fileName = !string.IsNullOrWhiteSpace(fileData.Headers.ContentDisposition.FileName) ? fileData.Headers.ContentDisposition.FileName : "NoName";
            return new FileInformation 
                {
                    Type = fileData.Headers.ContentType != null ? fileData.Headers.ContentType.MediaType : "application/octet-stream",

                    // this is here because Chrome submits files in quotation marks which get treated as part of the filename and get escaped
                    Name = fileName.Replace("\"", string.Empty),

                    Data = fileData
                };
        }
    }
}