using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;

namespace ConnectDemoApp
{
    public class AdoConnectionDemo : SQLDatabaseConnectionDemo
    {
        protected override DbConnection CreateConnection(string userName, string password, string dataSource, string databaseName)
        {
            return new SqlConnection(this.CreateAdoConnectionString(userName, password, dataSource, databaseName));
        }

        private string CreateAdoConnectionString(string userName, string password, string dataSource, string databaseName)
        {
            // create a new instance of the SQLConnectionStringBuilder
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                InitialCatalog = databaseName,
                Encrypt = true,
                TrustServerCertificate = false,
                UserID = userName,
                Password = password,
            };

            return connectionStringBuilder.ToString();
        }

        protected override DbCommand CreateCommand(DbConnection connection)
        {
            return new SqlCommand() { Connection = connection as SqlConnection };
        }
    }
}
