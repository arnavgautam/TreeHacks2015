namespace FabrikamWorker
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;

    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("FabrikamWorker entry point called", "Information");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("FabrikamStorageConnectionString"));

            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("calculator-queue");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();

            while (true)
            {
                foreach (CloudQueueMessage message in queue.GetMessages(20, TimeSpan.FromMinutes(5)))
                {
                    // Process all messages in less than 5 minutes, deleting each message after processing.
                    Trace.TraceInformation(string.Concat("Processing: ", message.AsString), "Information");
                    queue.DeleteMessage(message);
                }

                Thread.Sleep(10000);
                Trace.TraceInformation("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //// For information on handling configuration changes
            //// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
