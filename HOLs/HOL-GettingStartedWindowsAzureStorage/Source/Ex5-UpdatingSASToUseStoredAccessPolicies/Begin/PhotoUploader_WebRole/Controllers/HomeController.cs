using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PhotoUploader_WebRole.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Collections.Generic;

namespace PhotoUploader_WebRole.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /

        public ActionResult Index()
        {
            CloudTableClient cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(this.PublicTableSas));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            var photoList = new List<PhotoViewModel>();

            var photos = photoContext.GetPhotos();
            if (photos.Count() > 0)
            {
                photoList = photos.Select(x => this.ToViewModel(x)).ToList();
            }

            var privatePhotos = new List<PhotoViewModel>();

            if (this.User.Identity.IsAuthenticated)
            {
                cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(this.AuthenticatedTableSas));
                photoContext = new PhotoDataServiceContext(cloudTableClient);

                photos = photoContext.GetPhotos();
                if (photos.Count() > 0)
                {
                    photoList.AddRange(photos.Select(x => this.ToViewModel(x)).ToList());
                }
            }

            return this.View(photoList);
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(string partitionKey, string rowKey)
        {
            var token = partitionKey == "Public" ? this.PublicTableSas : this.AuthenticatedTableSas;

            CloudTableClient cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(this.PublicTableSas));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            PhotoEntity photo = photoContext.GetById(partitionKey, rowKey);
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

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(PhotoViewModel photoViewModel, HttpPostedFileBase file, bool Public, FormCollection collection)
        {
            if (this.ModelState.IsValid)
            {
                photoViewModel.PartitionKey = Public ? "Public" : this.User.Identity.Name;
                var photo = this.FromViewModel(photoViewModel);

                if (file != null)
                {
                    //Save file stream to Blob Storage
                    var blob = this.GetBlobContainer().GetBlockBlobReference(file.FileName);
                    blob.Properties.ContentType = file.ContentType;
                    blob.UploadFromStream(file.InputStream);
                    photo.BlobReference = file.FileName;
                }
                else
                {
                    this.ModelState.AddModelError("File", new ArgumentNullException("file"));
                    return this.View(photoViewModel);
                }

                //Save information to Table Storage
                var token = Public ? this.PublicTableSas : this.AuthenticatedTableSas;
                if (!this.User.Identity.IsAuthenticated)
                {
                    token = this.PublicTableSas;
                    photo.PartitionKey = "Public";
                }

                CloudTableClient cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(token));
                var photoContext = new PhotoDataServiceContext(cloudTableClient);

                photoContext.AddPhoto(photo);

                try
                {
                    //Send create notification
                    var msg = new CloudQueueMessage("Photo Uploaded");
                    this.GetCloudQueue().AddMessage(msg);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceInformation("Error", "Couldn't send notification");
                }

                return this.RedirectToAction("Index");
            }

            return this.View();
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(string partitionKey, string rowKey)
        {
            string token = partitionKey == "Public" ? this.PublicTableSas : this.AuthenticatedTableSas;

            CloudTableClient cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(token));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            PhotoEntity photo = photoContext.GetById(partitionKey, rowKey);

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

        //
        // POST: /Home/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhotoViewModel photoViewModel, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var photo = this.FromViewModel(photoViewModel);

                var token = photoViewModel.PartitionKey == "Public" ? this.PublicTableSas : this.AuthenticatedTableSas;

                CloudTableClient cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(token));
                var photoContext = new PhotoDataServiceContext(cloudTableClient);
                photoContext.UpdatePhoto(photo);

                return this.RedirectToAction("Index");
            }

            return this.View();
        }

        //
        // GET: /Home/Delete/5
        public ActionResult Delete(string partitionKey, string rowKey)
        {
            string token = partitionKey == "Public" ? this.PublicTableSas : this.AuthenticatedTableSas;

            CloudTableClient cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(token));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            PhotoEntity photo = photoContext.GetById(partitionKey, rowKey);

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

        //
        // POST: /Home/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string partitionKey, string rowKey)
        {
            if (ModelState.IsValid)
            {
                string token = partitionKey == "Public" ? this.PublicTableSas : this.AuthenticatedTableSas;

                CloudTableClient cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(token));
                var photoContext = new PhotoDataServiceContext(cloudTableClient);
                PhotoEntity photo = photoContext.GetById(partitionKey, rowKey);

                if (photo == null)
                {
                    return this.HttpNotFound();
                }

                photoContext.DeletePhoto(photo);

                //Deletes the Image from Blob Storage
                if (!string.IsNullOrEmpty(photo.BlobReference))
                {
                    var blob = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference);
                    blob.DeleteIfExists();
                }

                try
                {
                    //Send delete notification
                    var msg = new CloudQueueMessage("Photo Deleted");
                    this.GetCloudQueue().AddMessage(msg);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceInformation("Error", "Couldn't send notification");
                }
            }
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ToPrivate(string partitionKey, string rowKey)
        {
            CloudTableClient cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(this.PublicTableSas));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            var photo = photoContext.GetById(partitionKey, rowKey);
            if (photo == null)
            {
                return this.HttpNotFound();
            }

            photoContext.DeletePhoto(photo);

            cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(this.AuthenticatedTableSas));
            photoContext = new PhotoDataServiceContext(cloudTableClient);
            photo.PartitionKey = this.User.Identity.Name;
            photoContext.AddPhoto(photo);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ToPublic(string partitionKey, string rowKey)
        {
            CloudTableClient cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(this.AuthenticatedTableSas));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            var photo = photoContext.GetById(partitionKey, rowKey);
            if (photo == null)
            {
                return this.HttpNotFound();
            }

            photoContext.DeletePhoto(photo);

            cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(this.PublicTableSas));
            photoContext = new PhotoDataServiceContext(cloudTableClient);
            photo.PartitionKey = "Public";
            photoContext.AddPhoto(photo);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Share(string partitionKey, string rowKey)
        {
            var cloudTableClient = new CloudTableClient(this.UriTable, new StorageCredentials(this.AuthenticatedTableSas));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);

            PhotoEntity photo = photoContext.GetById(partitionKey, rowKey);
            if (photo == null)
            {
                return this.HttpNotFound();
            }

            string sas = string.Empty;
            if (!string.IsNullOrEmpty(photo.BlobReference))
            {
                CloudBlockBlob blobBlockReference = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference);
                sas = photoContext.GetSaSForBlob(blobBlockReference, SharedAccessBlobPermissions.Read);
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
            var photo = new PhotoEntity
                {
                    Title = photoViewModel.Title,
                    Description = photoViewModel.Description
                };

            photo.PartitionKey = photoViewModel.PartitionKey ?? photo.PartitionKey;
            photo.RowKey = photoViewModel.RowKey ?? photo.RowKey;
            return photo;
        }

        private CloudBlobContainer GetBlobContainer()
        {
            var client = this.StorageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName"));
            if (container.CreateIfNotExists())
            {
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            return container;
        }

        private CloudQueue GetCloudQueue()
        {
            var queueClient = new CloudQueueClient(this.UriQueue, new StorageCredentials(this.QueueSas));
            var queue = queueClient.GetQueueReference("messagequeue");
            queue.CreateIfNotExists();
            return queue;
        }

    }
}
