using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using PhotoUploader_WebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoUploader_WebRole.Controllers
{
    public class BaseController : Controller
    {
        public CloudStorageAccount StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        public Uri UriTable = new Uri("http://127.0.0.1:10002/devstoreaccount1");
        public Uri UriQueue = new Uri("http://127.0.0.1:10001/devstoreaccount1");

        public string AuthenticatedTableSas { get; set; }

        public string PublicTableSas { get; set; }

        public string QueueSas { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CloudTableClient cloudTableClientAdmin = this.StorageAccount.CreateCloudTableClient();
            var photoContextAdmin = new PhotoDataServiceContext(cloudTableClientAdmin);

            if (this.User.Identity.IsAuthenticated)
            {
                this.AuthenticatedTableSas = photoContextAdmin.GetSas(this.User.Identity.Name, SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Delete | SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Update);
                this.PublicTableSas = photoContextAdmin.GetSas("Public", SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Delete | SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Update);
            }
            else
            {
                this.PublicTableSas = photoContextAdmin.GetSas("Public", SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Update | SharedAccessTablePermissions.Query);
                this.AuthenticatedTableSas = null;
            }

            this.QueueSas = this.StorageAccount.CreateCloudQueueClient().GetQueueReference("messagequeue").GetSharedAccessSignature(
                       new SharedAccessQueuePolicy() { Permissions = SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Read, SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15) },
                       null
                       );
        }
    }
}
