using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace PhotoUploader_WebRole.Models
{
    public class PhotoDataServiceContext : TableServiceContext
    {
        public PhotoDataServiceContext(CloudTableClient client)
            : base(client)
        {

        }

        public IEnumerable<PhotoEntity> GetPhotos(string partitionKey)
        {
            CloudTable table = this.ServiceClient.GetTableReference("Photos");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<PhotoEntity> query = new TableQuery<PhotoEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            // Print the fields for each customer.
            return table.ExecuteQuery(query);
        }

        public PhotoEntity GetById(string partitionKey, string rowKey)
        {
            CloudTable table = this.ServiceClient.GetTableReference("Photos");
            TableOperation retrieveOperation = TableOperation.Retrieve<PhotoEntity>(partitionKey, rowKey);

            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
                return (PhotoEntity)retrievedResult.Result;
            else
                return null;
        }

        public void AddPhoto(PhotoEntity photo)
        {
            TableOperation operation = TableOperation.Insert(photo);
            CloudTable table = this.ServiceClient.GetTableReference("Photos");
            table.Execute(operation);
        }

        public void UpdatePhoto(PhotoEntity photo)
        {
            CloudTable table = this.ServiceClient.GetTableReference("Photos");
            TableOperation retrieveOperation = TableOperation.Retrieve<PhotoEntity>(photo.PartitionKey, photo.RowKey);

            TableResult retrievedResult = table.Execute(retrieveOperation);
            PhotoEntity updateEntity = (PhotoEntity)retrievedResult.Result;

            if (updateEntity != null)
            {
                updateEntity.Description = photo.Description;
                updateEntity.Title = photo.Title;

                TableOperation replaceOperation = TableOperation.Replace(updateEntity);
                table.Execute(replaceOperation);
            }
        }

        public void DeletePhoto(PhotoEntity photo)
        {
            CloudTable table = this.ServiceClient.GetTableReference("Photos");
            TableOperation retrieveOperation = TableOperation.Retrieve<PhotoEntity>(photo.PartitionKey, photo.RowKey);
            TableResult retrievedResult = table.Execute(retrieveOperation);
            PhotoEntity deleteEntity = (PhotoEntity)retrievedResult.Result;

            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                table.Execute(deleteOperation);
            }

        }

        public string GetSas(string policyId, string partition)
        {
            string sasToken = this.ServiceClient.GetTableReference("Photos").GetSharedAccessSignature(
                new SharedAccessTablePolicy()   /* access policy */,
                policyId     /* access policy identifier */,
                partition /* start partition key */,
                null     /* start row key */,
                partition /* end partition key */,
                null     /* end row key */);

            return sasToken;
        }

        public string GetSas(string partition)
        {
            SharedAccessTablePolicy policy = new SharedAccessTablePolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                Permissions = SharedAccessTablePermissions.Query
            };

            string sasToken = this.ServiceClient.GetTableReference("Photos").GetSharedAccessSignature(
                policy   /* access policy */,
                null     /* access policy identifier */,
                partition /* start partition key */,
                null     /* start row key */,
                partition /* end partition key */,
                null     /* end row key */);

            return sasToken;
        }

        public Uri GetUri()
        {
            return this.ServiceClient.BaseUri;
        }
    }
}