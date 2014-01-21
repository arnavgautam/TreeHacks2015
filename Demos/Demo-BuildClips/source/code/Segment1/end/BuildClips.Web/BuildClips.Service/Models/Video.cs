namespace BuildClips.Services.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Newtonsoft.Json;

    public class Video
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is Required")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is Required")]
        public string Description { get; set; }

        public string SourceVideoUrl { get; set; }

        public string EncodedVideoUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public string JobId { get; set; }

        [NotMapped]
        public JobStatus JobStatus { get; set; }
    }
}