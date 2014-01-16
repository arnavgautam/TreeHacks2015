using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoUploader_WebRole.Models
{
    public class PhotoViewModel
    {
        public string PartitionKey { get; set;}
        
        public string RowKey { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Uri { get; set; }
    }
}