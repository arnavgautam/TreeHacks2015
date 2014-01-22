namespace BuildClips.Services.Models
{
    using System.Configuration;
    using System.Data.Entity;

    public class VideosContext : DbContext
    {
        public VideosContext() : base(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
        {
        }

        public DbSet<Video> Videos { get; set; }
    }
}
