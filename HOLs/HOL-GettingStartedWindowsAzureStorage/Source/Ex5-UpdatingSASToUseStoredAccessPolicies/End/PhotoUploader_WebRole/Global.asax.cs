using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;

namespace PhotoUploader_WebRole
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var cloudTableClient = storageAccount.CreateCloudTableClient();
            var table = cloudTableClient.GetTableReference("Photos");
            table.CreateIfNotExists();
            var cloudBlobClient = storageAccount.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName"));
            if (container.CreateIfNotExists())
            {
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            var cloudQueueClient = storageAccount.CreateCloudQueueClient();
            var queue = cloudQueueClient.GetQueueReference("messagequeue");
            queue.CreateIfNotExists();

            TablePermissions tp = new TablePermissions();
            tp.SharedAccessPolicies.Add("readonly", new SharedAccessTablePolicy { Permissions = SharedAccessTablePermissions.Query, SharedAccessExpiryTime = System.DateTime.UtcNow.AddMinutes(15) });
            tp.SharedAccessPolicies.Add("edit", new SharedAccessTablePolicy { Permissions = SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Update, SharedAccessExpiryTime = System.DateTime.UtcNow.AddMinutes(15) });
            tp.SharedAccessPolicies.Add("admin", new SharedAccessTablePolicy { Permissions = SharedAccessTablePermissions.Query | SharedAccessTablePermissions.Add | SharedAccessTablePermissions.Update | SharedAccessTablePermissions.Delete, SharedAccessExpiryTime = System.DateTime.UtcNow.AddMinutes(15) });
            tp.SharedAccessPolicies.Add("none", new SharedAccessTablePolicy { Permissions = SharedAccessTablePermissions.None, SharedAccessExpiryTime = System.DateTime.UtcNow.AddMinutes(15) });
            table.SetPermissions(tp);

            BlobContainerPermissions bp = new BlobContainerPermissions();
            bp.SharedAccessPolicies.Add("read", new SharedAccessBlobPolicy { Permissions = SharedAccessBlobPermissions.Read, SharedAccessExpiryTime = System.DateTime.UtcNow.AddMinutes(60) });
            container.SetPermissions(bp);

            QueuePermissions qp = new QueuePermissions();
            qp.SharedAccessPolicies.Add("add", new SharedAccessQueuePolicy { Permissions = SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Read, SharedAccessExpiryTime = System.DateTime.UtcNow.AddMinutes(15) });
            qp.SharedAccessPolicies.Add("process", new SharedAccessQueuePolicy { Permissions = SharedAccessQueuePermissions.ProcessMessages | SharedAccessQueuePermissions.Read, SharedAccessExpiryTime = System.DateTime.UtcNow.AddMinutes(15) });
            queue.SetPermissions(qp);
            queue.Metadata.Add("Resize", "true");
            queue.SetMetadata();
        }
    }
}
