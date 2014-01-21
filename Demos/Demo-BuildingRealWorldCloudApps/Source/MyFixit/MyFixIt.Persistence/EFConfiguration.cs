namespace MyFixIt.Persistence
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.SqlServer;

    public class EFConfiguration : DbConfiguration
    {
        public EFConfiguration()
        {
            // TODO
            this.SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy(maxRetryCount: 3, maxDelay: TimeSpan.FromSeconds(5)));
        }
    }
}