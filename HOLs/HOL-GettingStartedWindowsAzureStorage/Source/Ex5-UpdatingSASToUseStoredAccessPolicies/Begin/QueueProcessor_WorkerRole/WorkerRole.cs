using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Auth;


namespace QueueProcessor_WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private DateTime serviceQueueSasExpiryTime;
        private Uri uri = new Uri("http://127.0.0.1:10001/devstoreaccount1");

        public override void Run()
        {
            Trace.TraceInformation("QueueProcessor_WorkerRole entry point called", "Information");
            var queueClient = new CloudQueueClient(this.uri, new StorageCredentials(this.GetQueueSas()));

            var queue = queueClient.GetQueueReference("messagequeue");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.TraceInformation("Working", "Information");
             
                if (DateTime.UtcNow.AddMinutes(1) >= this.serviceQueueSasExpiryTime)
                {
                    queueClient = new CloudQueueClient(this.uri, new StorageCredentials(this.GetQueueSas()));
                    queue = queueClient.GetQueueReference("messagequeue");
                }

                var msg = queue.GetMessage();

                if (msg != null)
                {
                    Trace.TraceInformation(string.Format("Message '{0}' processed.", msg.AsString));
                    queue.DeleteMessage(msg);
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        private string GetQueueSas()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var client = storageAccount.CreateCloudQueueClient();
            var queue = client.GetQueueReference("messagequeue");
            queue.CreateIfNotExists();
            var token = queue.GetSharedAccessSignature(
                       new SharedAccessQueuePolicy() { Permissions = SharedAccessQueuePermissions.ProcessMessages | SharedAccessQueuePermissions.Read | SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Update, SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15) },
                       null);

            this.serviceQueueSasExpiryTime = DateTime.UtcNow.AddMinutes(15);
            return token;
        }
    }
}
