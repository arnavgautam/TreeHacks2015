namespace LogViewer
{
    using System;
    using System.Configuration;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Threading;
    using AzureDiagnostics;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    public class Program
    {
        private static string lastPartitionKey = string.Empty;
        private static string lastRowKey = string.Empty;

        public static void Main(string[] args)
        {
            string connectionString = (args.Length == 0) ? "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" : args[0];

            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings[connectionString]);
            CloudTableClient tableStorage = account.CreateCloudTableClient();
            tableStorage.CreateTableIfNotExist(TableStorageTraceListener.DIAGNOSTICS_TABLE);

            Utils.ProgressIndicator progress = new Utils.ProgressIndicator();
            Timer timer = new Timer(
              (state) =>
              {
                  progress.Disable();
                  QueryLogTable(tableStorage);
                  progress.Enable();
              },
              null,
              0,
              10000);

            Console.ReadLine();
        }

        private static void QueryLogTable(CloudTableClient tableStorage)
        {
            TableServiceContext context = tableStorage.GetDataServiceContext();
            DataServiceQuery query = context.CreateQuery<LogEntry>(TableStorageTraceListener.DIAGNOSTICS_TABLE)
                .Where(entry => entry.PartitionKey.CompareTo(lastPartitionKey) > 0
                || (entry.PartitionKey == lastPartitionKey && entry.RowKey.CompareTo(lastRowKey) > 0))
                as DataServiceQuery;

            foreach (AzureDiagnostics.LogEntry entry in query.Execute())
            {
                Console.WriteLine("{0} - {1}", entry.Timestamp, entry.Message);
                lastPartitionKey = entry.PartitionKey;
                lastRowKey = entry.RowKey;
            }
        }
    }
}
