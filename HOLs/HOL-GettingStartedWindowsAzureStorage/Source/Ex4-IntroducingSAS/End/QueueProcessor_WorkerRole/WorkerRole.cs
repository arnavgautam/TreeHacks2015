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
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("QueueProcessor_WorkerRole entry point called", "Information");

            // Initialize the account information
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // retrieve a reference to the messages queue
            var queueClient = new CloudQueueClient(this.uri, new StorageCredentials(this.GetProcessSasForQueues()));
            var queue = queueClient.GetQueueReference("messagequeue");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.TraceInformation("Working", "Information");
                if (queue.Exists())
                {
                    if (DateTime.UtcNow.AddMinutes(1) >= this.serviceQueueSasExpiryTime)
                    {
                        queueClient = new CloudQueueClient(this.uri, new StorageCredentials(this.GetProcessSasForQueues()));
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
        }


        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public string GetProcessSasForQueues()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var queue = storageAccount.CreateCloudQueueClient().GetQueueReference("messagequeue");
            queue.CreateIfNotExists();
            this.serviceQueueSasExpiryTime = DateTime.UtcNow.AddMinutes(15);
            return queue.GetSharedAccessSignature(new SharedAccessQueuePolicy() { Permissions = SharedAccessQueuePermissions.ProcessMessages | SharedAccessQueuePermissions.Read | SharedAccessQueuePermissions.Add | SharedAccessQueuePermissions.Update, SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15) }, null);
        }
    }
}
