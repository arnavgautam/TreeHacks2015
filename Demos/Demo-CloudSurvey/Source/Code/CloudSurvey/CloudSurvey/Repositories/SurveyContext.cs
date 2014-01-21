namespace CloudSurvey.Repositories
{
    using System.Configuration;
    using System.Data.Entity;
    using CloudSurvey.Models;

    public class SurveyContext : DbContext
    {
        public SurveyContext()
            : base(ConfigurationManager.ConnectionStrings["SurveyConnection"].ConnectionString)
        {
        }

        public SurveyContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<Survey> Surveys { get; set; }

        public DbSet<SurveySubmission> SurveySubmissions { get; set; }

        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }

        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Survey>().HasMany(e => e.Questions).WithOptional().WillCascadeOnDelete(true);
        }
    }
}