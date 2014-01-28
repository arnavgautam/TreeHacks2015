using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.OleDb;

namespace ConnectDemoApp
{
    public class OleDbConnectionDemo : SQLDatabaseConnectionDemo
    {
        protected override DbConnection CreateConnection(string userName, string password, string dataSource, string databaseName)
        {
            return new OleDbConnection(this.CreateOleDBConnectionString(userName, password, dataSource, databaseName));
        }

        protected override DbCommand CreateCommand(DbConnection connection)
        {
            return new OleDbCommand() { Connection = connection as OleDbConnection };
        }

        private string CreateOleDBConnectionString(string userName, string password, string dataSource, string databaseName)
        {
            string serverName = GetServerName(dataSource);

            OleDbConnectionStringBuilder connectionStringBuilder = new OleDbConnectionStringBuilder
            {
                Provider = "SQLOLEDB",
                DataSource = dataSource,
            };

            connectionStringBuilder["Initial Catalog"] = databaseName;
            connectionStringBuilder["UId"] = userName + "@" + serverName;
            connectionStringBuilder["Pwd"] = password;

            return connectionStringBuilder.ConnectionString;
        }
    }
}
