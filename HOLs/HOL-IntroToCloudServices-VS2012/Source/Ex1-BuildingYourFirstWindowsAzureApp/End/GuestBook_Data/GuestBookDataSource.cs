using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace GuestBook_Data
{
    public class GuestBookDataSource
    {
        private static CloudStorageAccount storageAccount;
        private GuestBookDataContext context;

        static GuestBookDataSource()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));

            CloudTableClient cloudTableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = cloudTableClient.GetTableReference("GuestBookEntry");
            table.CreateIfNotExists();
        }

        public GuestBookDataSource()
        {
            this.context = new GuestBookDataContext(storageAccount.CreateCloudTableClient());
        }

        public IEnumerable<GuestBookEntry> GetGuestBookEntries()
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("GuestBookEntry");

            TableQuery<GuestBookEntry> query = new TableQuery<GuestBookEntry>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, DateTime.UtcNow.ToString("MMddyyyy")));

            return table.ExecuteQuery(query);
        }

        public void AddGuestBookEntry(GuestBookEntry newItem)
        {
            TableOperation operation = TableOperation.Insert(newItem);
            CloudTable table = context.ServiceClient.GetTableReference("GuestBookEntry");
            table.Execute(operation);
        }

        public void UpdateImageThumbnail(string partitionKey, string rowKey, string thumbUrl)
        {
            CloudTable table = context.ServiceClient.GetTableReference("GuestBookEntry");
            TableOperation retrieveOperation = TableOperation.Retrieve<GuestBookEntry>(partitionKey, rowKey);

            TableResult retrievedResult = table.Execute(retrieveOperation);
            GuestBookEntry updateEntity = (GuestBookEntry)retrievedResult.Result;

            if (updateEntity != null)
            {
                updateEntity.ThumbnailUrl = thumbUrl;

                TableOperation replaceOperation = TableOperation.Replace(updateEntity);
                table.Execute(replaceOperation);
            }
        } 
    }
}
