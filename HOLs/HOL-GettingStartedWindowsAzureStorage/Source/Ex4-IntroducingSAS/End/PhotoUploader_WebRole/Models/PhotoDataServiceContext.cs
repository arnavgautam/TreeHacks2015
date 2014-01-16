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

        public async Task UpdatePhotoAsync(PhotoEntity entityToUpdate)
        {
            var table = this.ServiceClient.GetTableReference("Photos");
            var operation = TableOperation.Replace(entityToUpdate);
            await table.ExecuteAsync(operation);
        }

        public async Task DeletePhotoAsync(PhotoEntity entityToDelete)
        {
            var table = this.ServiceClient.GetTableReference("Photos");
            var deleteOperation = TableOperation.Delete(entityToDelete);
            await table.ExecuteAsync(deleteOperation);
        }
    }
}