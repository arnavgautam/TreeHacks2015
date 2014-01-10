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
    public class HomeController : Controller
    {
        private CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        //private Uri Uri = new Uri("http://testing20130529.table.core.windows.net/");
        private Uri Uri = new Uri("http://testing20130529.table.core.windows.net/");

        //
        // GET: /

        public ActionResult Index()
        {
            this.RefreshAccessCredentials();

            CloudTableClient cloudTableClient = new CloudTableClient(this.Uri, new StorageCredentials(Session["Sas"].ToString()));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);

            var publicPhotos = photoContext.GetPhotos("Public").Select(x => this.ToViewModel(x)).ToList();
            var privatePhotos = new List<PhotoViewModel>();

            if (this.User != null)
            {
                privatePhotos = photoContext.GetPhotos(this.User.Identity.Name).Select(x => this.ToViewModel(x)).ToList();
            }

            return this.View();
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(string id)
        {
            this.RefreshAccessCredentials();

            CloudTableClient cloudTableClient = new CloudTableClient(this.Uri, new StorageCredentials(Session["Sas"].ToString()));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            PhotoEntity photo = photoContext.GetById("Public", id);

            if (photo == null)
            {
                photo = photoContext.GetById(this.User.Identity.Name, id);
                if (photo == null)
                {
                    return HttpNotFound();
                }
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

        [Authorize]
        public ActionResult Create()
        {
            this.RefreshAccessCredentials();

            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(PhotoViewModel photoViewModel, HttpPostedFileBase file, bool isPublic, FormCollection collection)
        {
            this.RefreshAccessCredentials();

            if (this.ModelState.IsValid)
            {
                photoViewModel.PartitionKey = isPublic ? "Public" : this.User.Identity.Name;
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
                CloudTableClient cloudTableClient = new CloudTableClient(this.Uri, new StorageCredentials(Session["Sas"].ToString()));
                var photoContext = new PhotoDataServiceContext(cloudTableClient);
                photoContext.AddPhoto(photo);

                //Send create notification
                var msg = new CloudQueueMessage("Photo Uploaded");
                this.GetCloudQueue().AddMessage(msg);

                return this.RedirectToAction("Index");
            }

            return this.View();
        }

        //
        // GET: /Home/Edit/5

        [Authorize]
        public ActionResult Edit(string id)
        {
            this.RefreshAccessCredentials();

            CloudTableClient cloudTableClient = new CloudTableClient(this.Uri, new StorageCredentials(Session["Sas"].ToString()));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            PhotoEntity photo = photoContext.GetById("Public", id);

            if (photo == null)
            {
                photo = photoContext.GetById(this.User.Identity.Name, id);
                if (photo == null)
                {
                    return this.HttpNotFound();
                }
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhotoViewModel photoViewModel, bool isPublic, FormCollection collection)
        {
            this.RefreshAccessCredentials();

            if (ModelState.IsValid)
            {
                var photo = this.FromViewModel(photoViewModel);

                //Update information in Table Storage
                CloudTableClient cloudTableClient = this.StorageAccount.CreateCloudTableClient();
                var photoContext = new PhotoDataServiceContext(cloudTableClient);
                photoContext.UpdatePhoto(photo);

                return this.RedirectToAction("Index");
            }

            return this.View();
        }

        //
        // GET: /Home/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            this.RefreshAccessCredentials();

            CloudTableClient cloudTableClient = new CloudTableClient(this.Uri, new StorageCredentials(Session["Sas"].ToString()));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            PhotoEntity photo = photoContext.GetById("Public", id);

            if (photo == null)
            {
                photo = photoContext.GetById(this.User.Identity.Name, id);
                if (photo == null)
                {
                    return this.HttpNotFound();
                }
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
        public ActionResult DeleteConfirmed(string id)
        {
            this.RefreshAccessCredentials();

            //Delete information From Table Storage
            CloudTableClient cloudTableClient = new CloudTableClient(this.Uri, new StorageCredentials(Session["Sas"].ToString()));
            var photoContext = new PhotoDataServiceContext(cloudTableClient);
            PhotoEntity photo = photoContext.GetById("Public", id);

            if (photo == null)
            {
                photo = photoContext.GetById(this.User.Identity.Name, id);
                if (photo == null)
                {
                    return this.HttpNotFound();
                }
            }
            photoContext.DeletePhoto(photo);

            //Deletes the Image from Blob Storage
            if (!string.IsNullOrEmpty(photo.BlobReference))
            {
                var blob = this.GetBlobContainer().GetBlockBlobReference(photo.BlobReference);
                blob.DeleteIfExists();
            }

            //Send delete notification
            var msg = new CloudQueueMessage("Photo Deleted");
            this.GetCloudQueue().AddMessage(msg);

            return this.RedirectToAction("Index");
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
            var queueClient = this.StorageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("messagequeue");
            queue.CreateIfNotExists();
            return queue;
        }

        public void RefreshAccessCredentials()
        {
            if ((Session["ExpireTime"] as DateTime? == null) || ((DateTime)Session["ExpireTime"] < DateTime.UtcNow))
            {
                CloudTableClient cloudTableClientAdmin = this.StorageAccount.CreateCloudTableClient();
                var photoContextAdmin = new PhotoDataServiceContext(cloudTableClientAdmin);

                Session["Sas"] = photoContextAdmin.GetSas("readonly", "Public");

                if (this.User != null)
                {
                    Session["MySas"] = photoContextAdmin.GetSas("admin", this.User.Identity.Name);
                    Session["Sas"] = photoContextAdmin.GetSas("admin", "Public");
                }

                Session["ExpireTime"] = DateTime.UtcNow.AddMinutes(15);
            }
        }
    }
}
