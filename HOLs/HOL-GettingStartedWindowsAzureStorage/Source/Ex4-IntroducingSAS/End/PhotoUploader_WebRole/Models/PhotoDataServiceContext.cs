using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
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

        public IEnumerable<PhotoEntity> GetPhotos()
        {
            return this.CreateQuery<PhotoEntity>("Photos");
        }

        public async Task<PhotoEntity> GetByIdAsync(string partitionKey, string rowKey)
        {
            var table = this.ServiceClient.GetTableReference("Photos");
            var operation = TableOperation.Retrieve<PhotoEntity>(partitionKey, rowKey);

            var retrievedResult = await table.ExecuteAsync(operation);
            return (PhotoEntity)retrievedResult.Result;
        }

        public async Task AddPhotoAsync(PhotoEntity photo)
        {
            var table = this.ServiceClient.GetTableReference("Photos");
            var operation = TableOperation.Insert(photo);
            await table.ExecuteAsync(operation);
        }

        public async Task UpdatePhotoAsync(PhotoEntity photo)
        {
            var table = this.ServiceClient.GetTableReference("Photos");
            var retrieveOperation = TableOperation.Retrieve<PhotoEntity>(photo.PartitionKey, photo.RowKey);

            var retrievedResult = await table.ExecuteAsync(retrieveOperation);
            var updateEntity = (PhotoEntity)retrievedResult.Result;

            if (updateEntity != null)
            {
                updateEntity.Description = photo.Description;
                updateEntity.Title = photo.Title;

                var replaceOperation = TableOperation.Replace(updateEntity);
                await table.ExecuteAsync(replaceOperation);
            }
        }

        public async Task DeletePhotoAsync(PhotoEntity entityToDelete)
        {
            var table = this.ServiceClient.GetTableReference("Photos");
            var deleteOperation = TableOperation.Delete(entityToDelete);
            await table.ExecuteAsync(deleteOperation);
        }

        public string GetSas(string partition, SharedAccessTablePermissions permissions)
        {
            SharedAccessTablePolicy policy = new SharedAccessTablePolicy()
            {
                Permissions = permissions,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };

            string sasToken = this.ServiceClient.GetTableReference("Photos").GetSharedAccessSignature(
                policy: policy,
                accessPolicyIdentifier: null,
                startPartitionKey: partition,
                startRowKey: null,
                endPartitionKey: partition,
                endRowKey: null);

            return sasToken;
        }
    }
}