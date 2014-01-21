using PhotoUploader_WebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using PhotoUploader_WebRole.Services;

namespace PhotoUploader_WebRole.Controllers
{
    public class HomeController : Controller
    {
        private CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        private Uri uriTable = new Uri("http://127.0.0.1:10002/devstoreaccount1");
        private Uri uriBlob = new Uri("http://127.0.0.1:10000/devstoreaccount1");
        private Uri uriQueue = new Uri("http://127.0.0.1:10001/devstoreaccount1");

        public ActionResult Index()
        {
            var photoContext = this.GetPhotoContext();
            var photos = photoContext.GetPhotos();
            var photosViewModels = photos.Select(this.ToViewModel).ToList();
            return this.View(photosViewModels);
        }

        public async Task<ActionResult> Details(string partitionKey, string rowKey)
        {
            var photoContext = this.GetPhotoContext();
            var photo = await photoContext.GetByIdAsync(partitionKey, rowKey);

            if (photo == null)
            {
                return HttpNotFound();
            }

            var viewModel = this.ToViewModel(photo);

            if (!string.IsNullOrEmpty(photo.BlobReference))
            {
                viewModel.Uri = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference).Uri.ToString();
            }

            return this.View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(PhotoViewModel photoViewModel, HttpPostedFileBase file, FormCollection collection)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var photo = this.FromViewModel(photoViewModel);

            if (file != null)
            {
                // Save file stream to Blob Storage
                var blob = this.GetBlobContainer().GetBlockBlobReference(file.FileName);
                blob.Properties.ContentType = file.ContentType;
                await blob.UploadFromStreamAsync(file.InputStream);
                photo.BlobReference = file.FileName;
            }
            else
            {
                this.ModelState.AddModelError("File", new ArgumentNullException("file"));
                return this.View(photoViewModel);
            }

            // Save information to Table Storage
            var photoContext = this.GetPhotoContext();
            await photoContext.AddPhotoAsync(photo);

            // Send create notification
            var msg = new CloudQueueMessage("Photo Uploaded");
            await this.GetCloudQueue().AddMessageAsync(msg);

            return this.RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string partitionKey, string rowKey)
        {
            var photoContext = this.GetPhotoContext();
            var photo = await photoContext.GetByIdAsync(partitionKey, rowKey);

            if (photo == null)
            {
                return this.HttpNotFound();
            }

            var viewModel = this.ToViewModel(photo);

            if (!string.IsNullOrEmpty(photo.BlobReference))
            {
                viewModel.Uri = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference).Uri.ToString();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PhotoViewModel photoViewModel, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }

            var photoContext = this.GetPhotoContext();
            var entityToUpdate = await photoContext.GetByIdAsync(photoViewModel.PartitionKey, photoViewModel.RowKey);

            if (entityToUpdate == null)
            {
                return this.HttpNotFound();
            }

            // Update entity information from ViewModel
            entityToUpdate.Title = photoViewModel.Title;
            entityToUpdate.Description = photoViewModel.Description;
            await photoContext.UpdatePhotoAsync(entityToUpdate);

            return this.RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string partitionKey, string rowKey)
        {
            var photoContext = this.GetPhotoContext();
            var photo = await photoContext.GetByIdAsync(partitionKey, rowKey);

            if (photo == null)
            {
                return this.HttpNotFound();
            }

            var viewModel = this.ToViewModel(photo);

            if (!string.IsNullOrEmpty(photo.BlobReference))
            {
                viewModel.Uri = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference).Uri.ToString();
            }

            return this.View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            var photoContext = this.GetPhotoContext();
            var photo = await photoContext.GetByIdAsync(partitionKey, rowKey);

            if (photo == null)
            {
                return this.HttpNotFound();
            }

            await photoContext.DeletePhotoAsync(photo);

            //Deletes the Image from Blob Storage
            if (!string.IsNullOrEmpty(photo.BlobReference))
            {
                var blob = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference);
                await blob.DeleteIfExistsAsync();
            }

            // Send delete notification
            var msg = new CloudQueueMessage("Photo Deleted");
            await this.GetCloudQueue().AddMessageAsync(msg);

            return this.RedirectToAction("Index");
        }

        public async Task<ActionResult> Share(string partitionKey, string rowKey)
        {
            var photoContext = this.GetPhotoContext();

            var photo = await photoContext.GetByIdAsync(partitionKey, rowKey);
            if (photo == null)
            {
                return this.HttpNotFound();
            }

            var sas = string.Empty;
            if (!string.IsNullOrEmpty(photo.BlobReference))
            {
                var blobBlockReference = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference);
                sas = SasService.GetReadonlyUriWithSasForBlob(blobBlockReference.Name);
            }

            if (!string.IsNullOrEmpty(sas))
            {
                return View("Share", null, sas);
            }

            return RedirectToAction("Index");
        }

        private PhotoViewModel ToViewModel(PhotoEntity photo)
        {
            return new PhotoViewModel
            {
                PartitionKey = photo.PartitionKey,
                RowKey = photo.RowKey,
                Title = photo.Title,
                Description = photo.Description
            };
        }

        private PhotoEntity FromViewModel(PhotoViewModel photoViewModel)
        {
            var photo = new PhotoEntity(this.User.Identity.Name);
            photo.RowKey = photoViewModel.RowKey ?? photo.RowKey;
            photo.Title = photoViewModel.Title;
            photo.Description = photoViewModel.Description;
            return photo;
        }

        private PhotoDataServiceContext GetPhotoContext()
        {
            var sasToken = SasService.GetSasForTable(this.User.Identity.Name);
            var cloudTableClient = new CloudTableClient(this.uriTable, new StorageCredentials(sasToken));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            return photoContext;
        }

        private CloudBlobContainer GetBlobContainer()
        {
            var sasToken = SasService.GetSasForBlobContainer();
            var client = new CloudBlobClient(this.uriBlob, new StorageCredentials(sasToken));
            return client.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName"));
        }

        private CloudQueue GetCloudQueue()
        {
            var sasToken = SasService.GetAddSasForQueues();
            var queueClient = new CloudQueueClient(this.uriQueue, new StorageCredentials(sasToken));
            var queue = queueClient.GetQueueReference("messagequeue");
            return queue;
        }
    }
}