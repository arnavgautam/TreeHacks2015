namespace MyFixIt.Persistence
{
    using System.Data.Entity;

    public class MyFixItContext : DbContext
    {
        public MyFixItContext()
            : base("name=appdb")
        {
        }

        public DbSet<MyFixIt.Persistence.FixItTask> FixItTasks { get; set; }
    }

    // EF follows a Code based Configration model and will look for a class deriving from DbConfiguration
    // for executing any Connection Resiliency strategies
}