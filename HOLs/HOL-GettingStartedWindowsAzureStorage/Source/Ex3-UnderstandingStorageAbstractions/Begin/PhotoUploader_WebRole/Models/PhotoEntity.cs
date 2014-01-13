using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoUploader_WebRole.Models
{
    public class PhotoEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string BlobReference { get; set; }
    }
}